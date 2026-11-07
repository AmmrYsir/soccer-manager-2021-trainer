using System;
using System.Collections.Generic;

public class TransferBid
{
	private const float NEGOTIATION_PERCENTAGE = 0.3f;

	public const short LOAN_AMOUNT = -1;

	public const short LOAN_RETURN_AMOUNT = -2;

	public const short RESPAWN_AMOUNT = -3;

	public const short RETIRED_AMOUNT = -4;

	public const short NEW_CONTRACT_AMOUNT = -5;

	public const short FREE_AGENT_AMOUNT = -6;

	public short clubIdFrom { get; set; }

	public short clubIdTo { get; set; }

	public int playerId { get; set; }

	public DateTime date { get; set; } = DateTime.MinValue;

	public int amount { get; set; }

	public short sellonPercentage { get; set; }

	public eBidStatus processed { get; set; }

	public short negotiationCount { get; set; }

	public ContractInfo contract { get; set; }

	public int suggestedAmount { get; set; }

	public int previousOffer { get; set; }

	public bool PlayerRejected()
	{
		if (processed == eBidStatus.PLAYER_REJECTED_UPSET || processed == eBidStatus.REJECTED_HAPPY_AT_CLUB || processed == eBidStatus.REJECTED_CLUB_RATING || processed == eBidStatus.REJECTED_MOVE_COUNTRY || processed == eBidStatus.PLAYER_REJECTED_CLUB_RECENT_RESULTS || processed == eBidStatus.CLUB_REJECTED_IMPORTANT_PLAYER || processed == eBidStatus.PLAYER_REJECTED_RETIRING_SOON)
		{
			return true;
		}
		return false;
	}

	public void TryWithdrawContract()
	{
		if (processed != eBidStatus.PLAYER_REJECTED_CONTRACT && processed != eBidStatus.ACCEPTED && processed != eBidStatus.ACCEPTED_FINALISED && (Game.instance.CurrentDate - date).TotalSeconds > 1814400.0)
		{
			contract.Initialise();
			rejectContractOffer(eContractRejectReason.TookTooLong, 0, 0);
		}
	}

	public void rejectTransferBid(eBidStatus reason, float requiredBid = 0f)
	{
		ManagedClub currentUserClub = Game.GetCurrentUserClub(Game.instance.g_currUser);
		int clubID = currentUserClub.ClubID;
		_ = currentUserClub.messages;
		if (clubIdFrom != clubID && clubIdTo != clubID)
		{
			processed = reason;
			if (Player.IsOnShortList(playerId))
			{
				MessageManager.TransferShortlistBidRejected(this);
			}
			if (Player.GetRatingQuick(playerId) >= 90)
			{
				MessageManager.UnmanagedTransferBidRejected(this);
			}
			return;
		}
		switch (reason)
		{
		case eBidStatus.CLUB_REJECTED_DEADLINE_PASSED:
			processed = reason;
			Transfers.instance.removePlayerFromInProgressList(playerId);
			Messages.DeleteTransferBidRecievedForPlayer(playerId, clubIdFrom);
			if ((Game.instance.CurrentDate - date).Days > 1)
			{
				MessageManager.TransferBidWithdrawn(this, reason);
			}
			return;
		case eBidStatus.CLUB_REJECTED_NO_MORE_NEGOTIATIONS:
		case eBidStatus.CLUB_REJECTED_NEGOTIATION_UNACCEPTABLE:
			processed = reason;
			Transfers.instance.removePlayerFromInProgressList(playerId);
			MessageManager.TransferBidWithdrawn(this, reason);
			return;
		case eBidStatus.CLUB_REJECTED_SELL_TO_RIVALS:
			processed = reason;
			Transfers.instance.removePlayerFromInProgressList(playerId);
			MessageManager.TransferBidRejected(this);
			return;
		}
		if (clubIdFrom == clubID && (float)amount >= Player.GetValue(playerId) * 2f && reason != eBidStatus.REJECTED_HAPPY_AT_CLUB && reason != eBidStatus.REJECTED_CLUB_RATING && reason != eBidStatus.REJECTED_MOVE_COUNTRY && reason != eBidStatus.PLAYER_REJECTED_UPSET && reason != eBidStatus.PLAYER_REJECTED_CLUB_RECENT_RESULTS && reason != eBidStatus.CLUB_REJECTED_HAD_BETTER_OFFER)
		{
			accept(chairmanAccepts: true);
			return;
		}
		processed = reason;
		Transfers.instance.removePlayerFromInProgressList(playerId);
		if (clubIdFrom == clubID)
		{
			if (processed == eBidStatus.CLUB_REJECTED_IMPORTANT_PLAYER)
			{
				MessageManager.TransferBidRejected(this);
			}
			else if (PlayerRejected())
			{
				MessageManager.TransferBidAccepted(this);
				MessageManager.TransferBidAcceptedPlayerRejected(this);
			}
			else if (requiredBid != 0f)
			{
				Game.instance.Random.SetSeed(Game.instance.Seed);
				float num = (float)Game.instance.Random.GetRand(1.05, 1.2);
				requiredBid *= num;
				if (requiredBid > 1000000f)
				{
					requiredBid = Helpers.toNearestUp(requiredBid, 100000.0);
				}
				else if (requiredBid > 100000f)
				{
					requiredBid = Helpers.toNearestUp(requiredBid, 10000.0);
				}
				else
				{
					requiredBid = Helpers.toNearestUp(requiredBid, 1000.0);
					if (requiredBid == 0f)
					{
						requiredBid = 1000f;
					}
				}
				if (requiredBid - requiredBid * 0.3f < (float)amount)
				{
					suggestedAmount = (int)requiredBid;
					MessageManager.TransferBidRejected(this, requiredBid);
					if (clubIdFrom == clubID)
					{
						suggestedAmount = 0;
					}
				}
				else
				{
					MessageManager.TransferBidRejected(this);
				}
			}
			else
			{
				MessageManager.TransferBidRejected(this);
			}
		}
		else
		{
			if (amount == -1)
			{
				Messages.DeleteLoanBidReceivedForPlayer(playerId, clubIdFrom);
			}
			else
			{
				Messages.DeleteTransferBidRecievedForPlayer(playerId, clubIdFrom);
			}
			suggestedAmount = (int)requiredBid;
			MessageManager.TransferBidRejected(this, suggestedAmount);
		}
	}

	public void accept(bool chairmanAccepts = false)
	{
		short currentManagedClubId = Game.instance.CurrentManagedClubId;
		if (clubIdTo != currentManagedClubId && clubIdFrom != currentManagedClubId)
		{
			processed = eBidStatus.ACCEPTED_TRANSFER_SOON;
			return;
		}
		Transfers.instance.addPlayerToInProgressList(playerId);
		processed = eBidStatus.ACCEPTED;
		_ = Game.instance.CurrentManagedClub.messages;
		if (clubIdTo == currentManagedClubId)
		{
			Messages.DeleteOffersReceivedForPlayer(playerId);
			Transfers.instance.bidManager.removeUnprocessedTransferBids(playerId);
			if (!chairmanAccepts)
			{
				MessageManager.TransferBidAccepted(this);
			}
		}
		else
		{
			MessageManager.TransferBidAccepted(this);
			MessageManager.TransferContractNegotiations(playerId, this);
		}
	}

	public bool Process(bool isLastDay)
	{
		bool result = false;
		int currentManagedClubId = Game.instance.CurrentManagedClubId;
		PlayerData playerData = Game.instance.g_PlayerData[playerId];
		switch (processed)
		{
		case eBidStatus.UNPROCESSED:
		{
			int position = playerData.Position;
			float value = playerData.GetValue();
			float num4 = 0f;
			if (clubIdTo == currentManagedClubId)
			{
				if (isLastDay || (Game.instance.CurrentDate - date).TotalSeconds >= 1209600.0)
				{
					rejectTransferBid(eBidStatus.CLUB_REJECTED_DEADLINE_PASSED);
				}
			}
			else if (clubIdFrom == currentManagedClubId)
			{
				if (Transfers.instance.isPlayerAvailable(playerId, clubIdTo))
				{
					if (Transfers.instance.playerOnBlackList(playerId))
					{
						rejectTransferBid(eBidStatus.PLAYER_REJECTED_UPSET);
						break;
					}
					if (playerData.YearsToRetirement() <= 0)
					{
						rejectTransferBid(eBidStatus.PLAYER_REJECTED_RETIRING_SOON);
						break;
					}
					if (Club.AreBitterRivals(clubIdFrom, clubIdTo))
					{
						rejectTransferBid(eBidStatus.CLUB_REJECTED_SELL_TO_RIVALS);
						break;
					}
					num4 = getRequiredTransferBidAmount(playerId, clubIdFrom, value);
					if ((float)amount >= num4)
					{
						if (Player.decidesTransfer(playerId, clubIdFrom, transferListed: true))
						{
							accept();
						}
						else
						{
							rejectTransferBid(eBidStatus.REJECTED_CLUB_RATING);
						}
					}
					else
					{
						rejectTransferBid(eBidStatus.CLUB_REJECTED_BID_TOO_LOW, num4);
					}
					break;
				}
				if (Transfers.instance.playerOnBlackList(playerId))
				{
					rejectTransferBid(eBidStatus.PLAYER_REJECTED_UPSET);
					break;
				}
				if (Player.YearsToRetirement(playerId) <= 0)
				{
					rejectTransferBid(eBidStatus.PLAYER_REJECTED_RETIRING_SOON);
					break;
				}
				if (Club.AreBitterRivals(clubIdFrom, clubIdTo))
				{
					rejectTransferBid(eBidStatus.CLUB_REJECTED_SELL_TO_RIVALS);
					break;
				}
				bool flag = false;
				bool flag2 = false;
				int num5 = 0;
				num5 = (((position & 1) != 0) ? 1 : (((position & 0x7E) != 0) ? 126 : (((position & 0x1FF0) == 0) ? 57344 : 8176)));
				if (Helpers.GetPlayersByPos(Club.GetPlayers(clubIdTo), num5).Count > Club.numberOfPlayersrequiredForPosition(clubIdTo, position))
				{
					flag = true;
				}
				if (!flag)
				{
					rejectTransferBid(eBidStatus.PLAYER_REJECTED_NO_REPLACEMENT);
				}
				else
				{
					if (!flag || flag2)
					{
						break;
					}
					num4 = getRequiredTransferBidAmount(playerId, clubIdFrom, value, fixSeed: false);
					if ((float)amount >= num4)
					{
						if (Player.decidesTransfer(playerId, clubIdFrom, transferListed: false))
						{
							processed = eBidStatus.UNMANAGED_CONSIDERING;
							Transfers.instance.addPlayerToInProgressList(playerId);
							bool flag3 = false;
							if (Player.GetRatingQuick(playerId) > 80)
							{
								flag3 = true;
							}
							else if (!playerData.InDecline && playerData.Potential > 87)
							{
								flag3 = true;
							}
							if (flag3)
							{
								flag3 = Game.instance.Random.GetRand(1) == 0;
							}
							if (!flag3)
							{
								flag3 = Game.instance.Random.GetRand(5) == 0;
							}
							if (!flag3 || !Transfers.instance.bidManager.canMakeBidForPlayer(playerId))
							{
								break;
							}
							List<short> interestedUnmanagedClubsForPlayer = Player.getInterestedUnmanagedClubsForPlayer(playerId, clubIdTo);
							Dictionary<short, Club> g_ClubData = Game.instance.g_ClubData;
							bool flag4 = Player.IsOnTransferList(playerId);
							bool flag5 = false;
							for (int i = 0; i < interestedUnmanagedClubsForPlayer.Count; i++)
							{
								if (flag5)
								{
									break;
								}
								short num6 = interestedUnmanagedClubsForPlayer[i];
								if (!Transfers.instance.bidManager.clubsHaveHadRivalBids(currentManagedClubId, num6) && (flag4 || Transfers.instance.CompareClubTo(g_ClubData[num6], g_ClubData[clubIdTo]) > 0))
								{
									Transfers.instance.bidManager.placeTransferBid(num6, playerId, getRequiredTransferBidAmount(playerId, num6, Player.GetValue(playerId)));
									flag5 = true;
								}
							}
						}
						else
						{
							rejectTransferBid(eBidStatus.REJECTED_CLUB_RATING);
						}
					}
					else
					{
						rejectTransferBid(eBidStatus.CLUB_REJECTED_BID_TOO_LOW, num4);
					}
				}
			}
			else
			{
				switch (processed)
				{
				case eBidStatus.UNPROCESSED:
					processed = eBidStatus.UNMANAGED_CONSIDERING;
					break;
				}
			}
			break;
		}
		case eBidStatus.UNMANAGED_CONSIDERING:
		{
			List<TransferBid> list = new List<TransferBid> { this };
			List<TransferBid> transferBids = Transfers.instance.bidManager.transferBids;
			int count = transferBids.Count;
			for (int j = 0; j < count; j++)
			{
				TransferBid transferBid = transferBids[j];
				if (this == transferBid || transferBid.playerId != playerId)
				{
					continue;
				}
				if (transferBid.processed == eBidStatus.ACCEPTED)
				{
					for (int k = 0; k < count; k++)
					{
						if (transferBids[k].playerId == playerId && (transferBids[k].processed == eBidStatus.UNMANAGED_CONSIDERING || transferBids[k].processed == eBidStatus.UNPROCESSED))
						{
							transferBids[k].rejectTransferBid(eBidStatus.CLUB_REJECTED_HAD_BETTER_OFFER);
						}
					}
					list = null;
					break;
				}
				if (transferBid.processed == eBidStatus.UNMANAGED_CONSIDERING || transferBid.processed == eBidStatus.UNPROCESSED)
				{
					if (!isLastDay && (transferBid.processed == eBidStatus.UNPROCESSED || (Game.instance.CurrentDate - transferBid.date).TotalSeconds < 86400.0))
					{
						list = null;
						break;
					}
					list.Add(transferBid);
				}
			}
			if (list == null || list.Count <= 0)
			{
				break;
			}
			int count2 = list.Count;
			int num7 = 0;
			int num8 = 0;
			bool flag6 = false;
			if (Player.IsOnTransferList(playerId))
			{
				Transfers.instance.bidManager.loanManager.RejectAllLoanBidsForPlayer(playerId);
			}
			else if ((Player.IsOnLoanList(playerId) || PlayerData.IsLoanedToClub(playerId)) && Transfers.instance.bidManager.loanManager.getUnprocessedLoanBidCountForPlayer(playerId) > 0)
			{
				for (int l = 0; l < count2; l++)
				{
					list[l].rejectTransferBid(eBidStatus.CLUB_REJECTED_NO_REASON_GIVEN);
				}
				flag6 = true;
			}
			if (count2 > 1 && !flag6)
			{
				for (int m = 0; m < count2; m++)
				{
					if (list[m].amount >= num8)
					{
						num8 = list[m].amount;
						num7 = m;
					}
				}
				for (int n = 0; n < count2; n++)
				{
					if (n == num7)
					{
						list[n].accept();
					}
					else
					{
						list[n].rejectTransferBid(eBidStatus.CLUB_REJECTED_HAD_BETTER_OFFER);
					}
				}
			}
			else if (!flag6)
			{
				if (clubIdFrom != currentManagedClubId && Game.instance.Random.GetRand(3) == 0)
				{
					rejectTransferBid(eBidStatus.CLUB_REJECTED_IMPORTANT_PLAYER);
				}
				else
				{
					accept();
				}
			}
			break;
		}
		case eBidStatus.CLUB_REJECTED_BID_TOO_LOW:
			if (clubIdTo == currentManagedClubId)
			{
				negotiationCount++;
				int rand = Game.instance.Random.GetRand(0, 3);
				double num2 = (double)suggestedAmount / (double)amount;
				if (negotiationCount > 1 || rand == 0)
				{
					rejectTransferBid(eBidStatus.CLUB_REJECTED_NO_MORE_NEGOTIATIONS);
				}
				else if (rand == 1 && num2 <= 1.1)
				{
					Transfers.instance.bidManager.placeTransferBid(clubIdFrom, playerId, suggestedAmount);
				}
				else if (num2 <= 1.2)
				{
					float num3 = Helpers.RoundMonetaryValue(Game.instance.Random.GetRand(1.0, Math.Min(1.1, num2)) * (double)amount);
					Transfers.instance.bidManager.placeTransferBid(clubIdFrom, playerId, (num3 < (float)suggestedAmount) ? num3 : ((float)suggestedAmount));
				}
				else
				{
					rejectTransferBid(eBidStatus.CLUB_REJECTED_NEGOTIATION_UNACCEPTABLE);
				}
			}
			break;
		case eBidStatus.ACCEPTED_TRANSFER_SOON:
			processed = eBidStatus.ACCEPTED_TRANSFER_NOW;
			break;
		case eBidStatus.ACCEPTED_TRANSFER_NOW:
		{
			if (Transfers.instance.transferWindowHandler.IsAssociationsOutsideOfTransferWindow(Club.GetAssociationID(clubIdFrom)))
			{
				processed = eBidStatus.CLUB_REJECTED_DEADLINE_PASSED;
				result = true;
				if (Player.IsOnShortList(playerId))
				{
					MessageManager.TransferShortlistBidCancelled(this);
				}
				if (Player.GetRatingQuick(playerId) >= 90)
				{
					MessageManager.UnmanagedTransferCancelled(this);
				}
				break;
			}
			if (Player.IsTransferBanned(playerId))
			{
				processed = eBidStatus.CLUB_REJECTED_DEADLINE_PASSED;
				result = true;
				if (Player.IsOnShortList(playerId))
				{
					MessageManager.TransferShortlistBidCancelled(this);
				}
				if (Player.GetRatingQuick(playerId) >= 90)
				{
					MessageManager.UnmanagedTransferCancelled(this);
				}
				break;
			}
			if (Player.IsDaysInjuredGreaterThan(playerId, 7))
			{
				processed = eBidStatus.NEGOTIATIONS_BROKE_DOWN_INJURY;
				result = true;
				if (Player.IsOnShortList(playerId))
				{
					MessageManager.TransferShortlistBidCancelled(this);
				}
				if (Player.GetRatingQuick(playerId) >= 90)
				{
					MessageManager.UnmanagedTransferCancelled(this);
				}
				break;
			}
			processed = eBidStatus.ACCEPTED_FINALISED;
			if (Player.IsOnShortList(playerId))
			{
				MessageManager.TransferShortlistBidCompleted(this);
			}
			if (Player.GetRatingQuick(playerId) >= 90)
			{
				MessageManager.UnmanagedTransferCompleted(this);
			}
			int num = Game.instance.g_PlayerData[playerId].GenerateNewContractLengthForUnmanagedClub();
			TransferPlayer.transferPlayer(playerId, clubIdTo, clubIdFrom, amount, Player.GenerateNewContractWages(playerId, clubIdFrom, num), Player.ConvertContractYearsToDate(num, Game.instance.g_ClubData[clubIdFrom].LeagueID));
			result = true;
			break;
		}
		}
		return result;
	}

	public void StorePreviousOffer(int Amount)
	{
		previousOffer = amount;
		amount = Amount;
		processed = eBidStatus.UNPROCESSED;
	}

	public bool isUnprocessedBidForPlayerFromClub(int playerId, short ClubId)
	{
		if (isUnprocessedBidForPlayer(playerId) && clubIdFrom == ClubId)
		{
			return true;
		}
		return false;
	}

	public bool isUnprocessedBidForPlayer(int playerIdx)
	{
		if (playerId == playerIdx)
		{
			if (processed != eBidStatus.UNPROCESSED && processed != eBidStatus.ACCEPTED && processed != eBidStatus.UNMANAGED_CONSIDERING && processed != eBidStatus.ACCEPTED_TRANSFER_NOW && processed != eBidStatus.ACCEPTED_TRANSFER_SOON && (processed != eBidStatus.CLUB_REJECTED_BID_TOO_LOW || suggestedAmount == 0))
			{
				if (processed == eBidStatus.PLAYER_REJECTED_CONTRACT)
				{
					return contract.demandedWages != 0;
				}
				return false;
			}
			return true;
		}
		return false;
	}

	public static float getRequiredTransferBidAmount(int playerId, short clubFromId, float value, bool fixSeed = true)
	{
		int seed = 0;
		if (fixSeed)
		{
			seed = Game.instance.Random.GetSeed();
			Game.instance.Random.SetSeed(Game.instance.Seed + playerId + clubFromId);
		}
		bool flag = Transfers.instance.IsPlayerOnTransferList(playerId);
		SortableClubPlayerList playersAsSortableListStatic = Club.GetPlayers(Player.GetClubID(playerId)).GetPlayersAsSortableListStatic();
		playersAsSortableListStatic.Sort(CustomSort.SortByRatingQuickOnlyDesc);
		int num = playersAsSortableListStatic.IndexOf(playerId);
		int days = (Player.GetContractExpiryDate(playerId) - Game.instance.CurrentDate).Days;
		float num2 = (float)Game.instance.Random.GetRand(1, 100) / 100f;
		float num3;
		float num4;
		if (Player.IsTransferBanned(playerId))
		{
			num3 = 2.5f;
			num4 = 3f;
		}
		else if (days < 366)
		{
			num3 = 0.55f;
			num4 = 0.8f;
		}
		else if (num < 5)
		{
			if (flag)
			{
				num3 = 1.1f;
				num4 = 1.4f;
			}
			else
			{
				num3 = 1.3f;
				num4 = 1.5f;
			}
		}
		else if (flag)
		{
			num3 = 0.65f;
			num4 = 0.85f;
		}
		else
		{
			num3 = 1.1f;
			num4 = 1.35f;
		}
		float num5 = ((float)days / 365f - 1f) / 4f;
		if (num5 == 3f)
		{
			num3 += 0.1f;
			num4 += 0.1f;
		}
		else if (num5 == 4f)
		{
			num3 += 0.5f;
			num4 += 0.5f;
		}
		else if (num5 == 5f)
		{
			num3++;
			num4++;
		}
		float num6 = (num4 - num3) * num2 + num3;
		if (fixSeed)
		{
			Game.instance.Random.SetSeed(seed);
		}
		return Helpers.RoundMonetaryValue(value * num6);
	}

	public void acceptContractOffer()
	{
		int associationID = Club.GetAssociationID(clubIdFrom);
		ManagedClub currentManagedClub = Game.instance.CurrentManagedClub;
		PlayerData playerData = Game.instance.g_PlayerData[playerId];
		if (amount == -5)
		{
			playerData.CreateNewContract(contract.wagesPerWeek, contract.contractExpiry);
			Player.ChangeMorale(playerId, 3, replayed: true);
			MessageManager.TransferRenewedContractAccepted(this);
			Messages.DeleteContractMessagesForPlayer(playerId);
			playerData.Concerns.ContractWage.Process(playerId);
			Transfers.instance.removePlayerFromInProgressList(playerId);
			currentManagedClub.PayPlayerSigninFee(contract.signingFee);
		}
		else if (clubIdTo != 0 && Transfers.instance.transferWindowHandler.IsAssociationsOutsideOfTransferWindow((short)associationID))
		{
			DateTime nextTransferWindowDate = Transfers.instance.transferWindowHandler.getNextTransferWindowDate((short)associationID);
			if (nextTransferWindowDate != DateTime.MinValue)
			{
				if (contract == null)
				{
					int num = playerData.GenerateNewContractLengthForUnmanagedClub();
					contract = new ContractInfo
					{
						wagesPerWeek = Player.GenerateNewContractWages(playerId, clubIdFrom, num),
						contractExpiry = Player.ConvertContractYearsToDate(num, Game.instance.g_ClubData[clubIdFrom].LeagueID)
					};
				}
				Transfers.instance.createPendingTransfer(playerId, clubIdTo, clubIdFrom, amount, nextTransferWindowDate, contract);
			}
		}
		else
		{
			int wages;
			DateTime contractExpiry;
			ePlayerSquadStatus squadStatus;
			if (contract == null)
			{
				int num2 = playerData.GenerateNewContractLengthForUnmanagedClub();
				wages = Player.GenerateNewContractWages(playerId, clubIdFrom, num2);
				contractExpiry = Player.ConvertContractYearsToDate(num2, Game.instance.g_ClubData[clubIdFrom].LeagueID);
				squadStatus = ePlayerSquadStatus.None;
			}
			else
			{
				wages = contract.wagesPerWeek;
				contractExpiry = contract.contractExpiry;
				squadStatus = contract.squadStatus;
			}
			TransferPlayer.transferPlayer(playerId, clubIdTo, clubIdFrom, amount, wages, contractExpiry, squadStatus);
			MessageManager.TransferContractAccepted(this);
			if (clubIdTo == currentManagedClub.ClubID)
			{
				Player.RemoveInvalidPlayersFromLineup(playerId);
				Transfers.instance.removePlayerFromInProgressList(playerId);
				if (amount != -1 && amount != -2)
				{
					currentManagedClub.PayPlayerTransferFee(amount);
				}
				currentManagedClub.Balance += amount;
			}
			else
			{
				ManagedClub.AddPlayer(playerId, isTransfer: true);
				Transfers.instance.removePlayerFromInProgressList(playerId);
				currentManagedClub.ReceivedPlayerTransferFee(Math.Max(0, amount), contract.signingFee);
			}
			currentManagedClub.CheckPositionWarningFlags(Player.GetPosition(playerId));
		}
		processed = eBidStatus.ACCEPTED_FINALISED;
	}

	public void rejectContractOffer(eContractRejectReason reason, int demandedWages = 0, short preferredClub = 0)
	{
		processed = eBidStatus.PLAYER_REJECTED_CONTRACT;
		contract.demandedWages = 0;
		if (demandedWages != 0)
		{
			contract.demandedWages = demandedWages;
		}
		switch (reason)
		{
		case eContractRejectReason.OtherOffer:
			MessageManager.TransferContractRejected(this, demandedWages, reason, preferredClub);
			break;
		case eContractRejectReason.TookTooLong:
			MessageManager.TransferContractRejected(this, 0, reason, 0);
			break;
		default:
			MessageManager.TransferContractRejected(this, demandedWages, reason, 0);
			break;
		}
		Transfers.instance.removePlayerFromInProgressList(playerId);
	}

	public bool managedClubBalanceSufficient()
	{
		return Math.Round(Game.instance.CurrentManagedClub.Balance) - (double)(amount + contract.signingFee) >= 0.0;
	}
}
