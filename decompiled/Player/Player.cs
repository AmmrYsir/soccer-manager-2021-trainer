using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class Player
{
	public const short PLAYER_TRANSFER_BAN_DAYS = 274;

	private const int MaxContractLength = 5;

	private const int MaxContractLengthOver32 = 3;

	private static DateTime cachedNextContractExpiryWarningDate = DateTime.MinValue;

	public static string signPlayerType = "";

	public static int signPlayerPlayerID = 0;

	public static int signPlayerClubID = 0;

	public static int signPlayerCost = 0;

	public static PlayerRatingState GetRatingState(int playerID)
	{
		return Game.instance.g_PlayerData[playerID].GetRatingState();
	}

	public static ePlayerPosition PrimaryRole(int playerID)
	{
		ePlayerPosition result = ePlayerPosition.MIDFIELDER;
		switch ((Constants.PlayerPosition)PlayerData.GetPosition(playerID))
		{
		case Constants.PlayerPosition.PLAYER_POS_GK:
			result = ePlayerPosition.GOALKEEPER;
			break;
		case Constants.PlayerPosition.PLAYER_POS_DL:
		case Constants.PlayerPosition.PLAYER_POS_DC:
		case Constants.PlayerPosition.PLAYER_POS_DR:
		case Constants.PlayerPosition.PLAYER_POS_D:
		case Constants.PlayerPosition.PLAYER_POS_DML:
		case Constants.PlayerPosition.PLAYER_POS_DMR:
		case Constants.PlayerPosition.PLAYER_POS_ANY_DEF:
			result = ePlayerPosition.DEFENDER;
			break;
		case Constants.PlayerPosition.PLAYER_POS_DMC:
		case Constants.PlayerPosition.PLAYER_POS_DM:
		case Constants.PlayerPosition.PLAYER_POS_ML:
		case Constants.PlayerPosition.PLAYER_POS_MC:
		case Constants.PlayerPosition.PLAYER_POS_MR:
		case Constants.PlayerPosition.PLAYER_POS_M:
		case Constants.PlayerPosition.PLAYER_POS_ANY_MID:
			result = ePlayerPosition.MIDFIELDER;
			break;
		case Constants.PlayerPosition.PLAYER_POS_AML:
		case Constants.PlayerPosition.PLAYER_POS_AMC:
		case Constants.PlayerPosition.PLAYER_POS_AMR:
		case Constants.PlayerPosition.PLAYER_POS_AM:
		case Constants.PlayerPosition.PLAYER_POS_FL:
		case Constants.PlayerPosition.PLAYER_POS_FC:
		case Constants.PlayerPosition.PLAYER_POS_FR:
		case Constants.PlayerPosition.PLAYER_POS_F:
			result = ePlayerPosition.ATTACKER;
			break;
		}
		return result;
	}

	public static bool SetFirstTeam(int playerID)
	{
		if (Club.CheckMaxFirstTeam(Game.GetCurrentUserClubID()))
		{
			GetStatus(playerID).firstTeam = true;
			return true;
		}
		return false;
	}

	public static byte SetSquadNo(int playerID, List<byte> availableNo = null)
	{
		bool flag = false;
		byte b = 0;
		byte b2 = 0;
		short clubID = GetClubID(playerID);
		if (clubID == 0)
		{
			return 0;
		}
		PlayerData playerData = Game.instance.g_PlayerData[playerID];
		Club club = Game.instance.g_ClubData[clubID];
		int position = playerData.GetPosition(preferred: true);
		int position2 = playerData.GetPosition();
		if (availableNo == null)
		{
			availableNo = Club.GetAvailableSquadNo(clubID);
			flag = true;
		}
		byte ratingInClub = GetRatingInClub(playerID, clubID);
		if (ratingInClub < 15)
		{
			switch (position)
			{
			case 1:
				b = 1;
				break;
			case 8:
			case 64:
				b = 2;
				break;
			case 2:
			case 16:
				b = 3;
				break;
			case 4:
				b = 4;
				b2 = 5;
				break;
			case 32:
				b = 6;
				break;
			case 256:
				b = 6;
				b2 = 8;
				break;
			case 512:
			case 4096:
				b = 7;
				break;
			case 2048:
				b = 8;
				b2 = 10;
				break;
			case 16384:
				b = 9;
				break;
			case 128:
			case 1024:
				b = 11;
				break;
			}
			if (availableNo.Contains(b))
			{
				if (flag)
				{
					club.SetSquadNoAvailability(b, available: false);
				}
				return b;
			}
			if (availableNo.Contains(b2))
			{
				if (flag)
				{
					club.SetSquadNoAvailability(b2, available: false);
				}
				return b2;
			}
			if (((2 & position2) != 0 || (0x10 & position2) != 0) && availableNo.Contains(2))
			{
				b = 2;
				if (flag)
				{
					club.SetSquadNoAvailability(b, available: false);
				}
				return b;
			}
			if (((8 & position2) != 0 || (0x40 & position2) != 0) && availableNo.Contains(3))
			{
				b = 3;
				if (flag)
				{
					club.SetSquadNoAvailability(b, available: false);
				}
				return b;
			}
			if ((4 & position2) != 0)
			{
				if (availableNo.Contains(4))
				{
					b = 4;
					if (flag)
					{
						club.SetSquadNoAvailability(b, available: false);
					}
					return b;
				}
				if (availableNo.Contains(5))
				{
					b = 5;
					if (flag)
					{
						club.SetSquadNoAvailability(b, available: false);
					}
					return b;
				}
			}
			if (((0x20 & position2) != 0 || (0x100 & position2) != 0) && availableNo.Contains(6))
			{
				b = 6;
				if (flag)
				{
					club.SetSquadNoAvailability(b, available: false);
				}
				return b;
			}
			if (((0x1000 & position2) != 0 || (0x200 & position2) != 0) && availableNo.Contains(7))
			{
				b = 7;
				if (flag)
				{
					club.SetSquadNoAvailability(b, available: false);
				}
				return b;
			}
			if (((0x100 & position2) != 0 || (0x800 & position2) != 0) && availableNo.Contains(8))
			{
				b = 8;
				if (flag)
				{
					club.SetSquadNoAvailability(b, available: false);
				}
				return b;
			}
			if ((0x4000 & position2) != 0 && availableNo.Contains(9))
			{
				b = 9;
				if (flag)
				{
					club.SetSquadNoAvailability(b, available: false);
				}
				return b;
			}
			if ((0x800 & position2) != 0 && availableNo.Contains(10))
			{
				b = 10;
				if (flag)
				{
					club.SetSquadNoAvailability(b, available: false);
				}
				return b;
			}
			if (((0x400 & position2) != 0 || (0x80 & position2) != 0) && availableNo.Contains(11))
			{
				b = 11;
				if (flag)
				{
					club.SetSquadNoAvailability(b, available: false);
				}
				return b;
			}
			availableNo.Sort();
			for (int i = 0; i < availableNo.Count; i++)
			{
				byte b3 = availableNo[i];
				if ((b3 > 11) & (b3 != 13))
				{
					b = b3;
					if (flag)
					{
						club.SetSquadNoAvailability(b, available: false);
					}
					return b;
				}
			}
		}
		else
		{
			byte b4 = (byte)((ratingInClub >= 30) ? 29 : 11);
			availableNo.Sort();
			for (int j = 0; j < availableNo.Count; j++)
			{
				byte b5 = availableNo[j];
				if (b5 == 13 && position == 1)
				{
					b = b5;
					if (flag)
					{
						club.SetSquadNoAvailability(b, available: false);
					}
					return b;
				}
				if (b5 > b4 && b5 != 13)
				{
					b = b5;
					if (flag)
					{
						club.SetSquadNoAvailability(b, available: false);
					}
					return b;
				}
			}
		}
		return b;
	}

	public static List<byte> GetPlayerForm(int playerID)
	{
		List<byte> list = new List<byte>();
		int num = 0;
		short clubID = GetClubID(playerID);
		if (clubID != 0)
		{
			short associationID = Club.GetAssociationID(clubID);
			string currentSeasonID = Association.GetCurrentSeasonID(associationID);
			DateTime seasonStartDate = Association.GetSeasonStartDate(associationID, currentSeasonID);
			List<Club.PlayedFixtureDetails> playedFixtures = Club.GetPlayedFixtures(clubID, seasonStartDate, Game.instance.CurrentDate);
			for (int num2 = playedFixtures.Count - 1; num2 >= 0; num2--)
			{
				Fixture fixture = playedFixtures[num2].fixture;
				fixture.RestoreFromSQL();
				FixtureClub fixtureClub = ((fixture.fixtureClubs[0].clubId == clubID) ? fixture.fixtureClubs[0] : fixture.fixtureClubs[1]);
				int num3 = fixtureClub.TeamPosition(playerID);
				if (num3 != -1 && fixtureClub.playerData[num3].performance > 0)
				{
					list.Add(fixtureClub.playerData[num3].performance);
					num++;
				}
				if (num >= 5)
				{
					break;
				}
			}
		}
		list.Reverse();
		list.AddRange(new byte[5 - num]);
		return list;
	}

	public static void Retire(int playerID)
	{
		PlayerData playerData = Game.instance.g_PlayerData[playerID];
		if (playerData.Status == ePlayerStatus.RESPAWNED)
		{
			Transfers.instance.unattachedPlayerManager.RemovePlayer(playerID);
			playerData.Status = ePlayerStatus.RETIRED;
		}
		else
		{
			playerData.Status = ePlayerStatus.RETIRED;
			if (playerData.ClubId != 0)
			{
				_ = Game.instance.g_ClubPlayer[playerData.ClubId];
				if (playerData.ClubId == Game.instance.CurrentManagedClubId)
				{
					MessageManager.PlayerRetired(playerID, playerData.ClubId);
					Game.instance.g_ClubPlayer[playerData.ClubId].PlayerRetired(playerID, playerData.ClubId);
					PlayerSquad.RemovePlayer(playerData.ClubId, playerID);
					Game.instance.CurrentManagedClub.SwapDepartingPlayerFromLineup(playerID);
				}
				if (Game.instance.g_clubdata_playerswantingbigclubtransfer.TryGetValue(playerData.ClubId, out var value))
				{
					for (int num = value.List.Count - 1; num >= 0; num--)
					{
						if (value[num] == playerID)
						{
							value.RemoveAt(num);
						}
					}
				}
				if (Game.instance.g_trainingDrillManager.TrainingPlayer.ContainsKey(playerID))
				{
					Game.instance.g_trainingDrillManager.TrainingPlayer.Remove(playerID);
				}
				PlayerSquad.RemovePlayer(playerData.ClubId, playerID);
				Club.SetSquadNoAvailablity(playerData.ClubId, playerData.SquadNo, available: true);
				playerData.SquadNo = 0;
				Club.RetirePlayer(playerID, playerData.ClubId);
				playerData.ClubId = 0;
			}
		}
		if (Game.instance.g_playerCareerStats.ContainsKey(playerID))
		{
			Game.instance.g_playerCareerStats.Remove(playerID);
		}
		RemoveFromShortList(playerID);
	}

	public static bool decidesTransfer(int playerId, short clubFromId, bool transferListed)
	{
		if (Game.instance.g_vip_respawnid == playerId && clubFromId == Game.instance.CurrentManagedClubId)
		{
			return true;
		}
		if (Game.instance.g_PlayerData[playerId].willSignforClub(clubFromId) && ratingForClub(playerId, clubFromId) >= 45)
		{
			return true;
		}
		return false;
	}

	public static bool canGoToClub(int playerId, short clubFromId)
	{
		if (Game.instance.g_vip_respawnid == playerId && clubFromId == Game.instance.CurrentManagedClubId)
		{
			return true;
		}
		if (PlayerData.IsLoanedToClub(playerId))
		{
			return false;
		}
		if (IsTransferBanned(playerId))
		{
			return false;
		}
		if (Game.instance.g_PlayerData[playerId].willSignforClub(clubFromId) && ratingForClub(playerId, clubFromId) >= 45)
		{
			return true;
		}
		return false;
	}

	internal static List<short> getInterestedUnmanagedClubsForPlayer(int playerId, short playerClubId, List<short> partnerClubs = null)
	{
		List<short> list = new List<short>();
		if (IsTransferBanned(playerId))
		{
			return list;
		}
		PlayerData playerData = Game.instance.g_PlayerData[playerId];
		int transferRating = playerData.GetTransferRating();
		int num = playerData.Age();
		int num2 = (int)GetValue(playerId);
		int num3 = 0;
		short num4 = 0;
		Game.instance.g_ClubData.TryGetValue(playerClubId, out var value);
		if (value != null)
		{
			if (partnerClubs == null)
			{
				partnerClubs = Association.GetClubList(value.AssociationID);
			}
			num3 = value.transferTier;
			num4 = value.AssociationID;
		}
		if (playerData.WillMoveOverseas)
		{
			if (Game.instance.Random.GetRand(5) < 3)
			{
				partnerClubs.Clear();
			}
			partnerClubs.AddRange(ClubList.GetAllClubsTransferTier(Math.Max(5, num3)));
		}
		foreach (short partnerClub in partnerClubs)
		{
			if (partnerClub == playerData.ClubId)
			{
				continue;
			}
			Club club = Game.instance.g_ClubData[partnerClub];
			if (partnerClub != Game.instance.CurrentManagedClubId && club.transferTier > 0 && (num4 == club.AssociationID || club.LeagueID == Association.GetTopDivision(club.AssociationID).LeagueID))
			{
				TransferTier tier = Transfers.instance.transferTierList.GetTier(club.transferTier);
				if (transferRating >= tier.MinimumPotential && (num >= 23 || (transferRating >= tier.MinimumPotential && num2 <= tier.Under23Value && (num3 == 0 || tier.Id <= num3))) && (num <= 22 || num >= 30 || (transferRating >= tier.MaximumPotential - (tier.MaximumPotential - tier.MinimumPotential) / 2 && num2 <= tier.Under30Value)) && (num <= 29 || (num2 <= tier.Over29Value && (num3 == 0 || tier.Id >= num3))) && !Transfers.instance.bidManager.HasUnprocessedBidFromClub(playerId, partnerClub))
				{
					list.Add(partnerClub);
				}
			}
		}
		return list;
	}

	internal static List<short> getInterestedUnmanagedClubsLoanPlayer(int playerId, short playerClubId, List<short> partnerClubs = null)
	{
		List<short> list = new List<short>();
		PlayerData playerData = Game.instance.g_PlayerData[playerId];
		if (!IsRetiring(playerId) && !playerData.IsLoanedToClub() && (Game.instance.CurrentDate - playerData.LastTransferDate).TotalDays > 365.0)
		{
			if (partnerClubs == null)
			{
				partnerClubs = Association.GetClubList(Club.GetAssociationID(playerClubId));
			}
			int transferRating = playerData.GetTransferRating();
			int num = playerData.Age();
			if (Game.instance.g_ClubData[playerClubId].transferTier <= 5 && transferRating > Transfers.instance.transferTierList.list[5].MinimumPotential && num < 34 && num > 17)
			{
				if (Game.instance.Random.GetRand(3) < 2)
				{
					partnerClubs.Clear();
				}
				partnerClubs.AddRange(ClubList.GetAllClubsTransferTier(5));
			}
			int num2 = (int)GetValue(playerId);
			int transferTier = Game.instance.g_ClubData[playerClubId].transferTier;
			foreach (short partnerClub in partnerClubs)
			{
				if (partnerClub != Game.instance.CurrentManagedClubId && Game.instance.g_ClubData[partnerClub].transferTier > 0)
				{
					TransferTier tier = Transfers.instance.transferTierList.GetTier(Game.instance.g_ClubData[partnerClub].transferTier);
					if (transferRating >= tier.MinimumPotential && (num >= 23 || (transferRating >= tier.MinimumPotential && num2 <= tier.Under23Value)) && (num <= 22 || num >= 30 || (transferRating >= tier.MaximumPotential - (tier.MaximumPotential - tier.MinimumPotential) / 2 && num2 <= tier.Under30Value)) && (num <= 29 || (num2 <= tier.Over29Value && tier.Id >= transferTier)) && !Transfers.instance.bidManager.loanManager.HasMadeLoanBid(playerId, partnerClub))
					{
						list.Add(partnerClub);
					}
				}
			}
		}
		list.Shuffle();
		return list;
	}

	public static int ratingForClub(int playerid, short biddingClubID)
	{
		PlayerData playerData = Game.instance.g_PlayerData[playerid];
		int rating = GetRating(playerid);
		int potential = playerData.Potential;
		byte averageRating = Club.GetAverageRating(biddingClubID);
		short num = (PlayerData.IsLoanedToClub(playerid) ? GetClubIDLoanParent(playerid) : GetClubID(playerid));
		if (num == 0)
		{
			if (averageRating > rating - 5)
			{
				return 45;
			}
			return 0;
		}
		byte averageRating2 = Club.GetAverageRating(num);
		Club.GetPlayers(num).GetPlayersAsSortableListStatic().Sort(CustomSort.SortByRatingQuickOnlyDesc);
		bool flag = !playerData.ReachedPotential && potential >= averageRating2 - 1 && playerData.Age() <= 23;
		int transferTier = Game.instance.g_ClubData[biddingClubID].transferTier;
		int transferTier2 = Game.instance.g_ClubData[num].transferTier;
		if (transferTier <= transferTier2 || averageRating >= averageRating2)
		{
			return 45;
		}
		ePlayerSquadStatus squadStatus = playerData.SquadStatus;
		if (transferTier <= transferTier2 + 1)
		{
			if (PlayerSquadStatus.Compare(ePlayerSquadStatus.FirstTeam, squadStatus) >= 0)
			{
				return 45;
			}
			if (playerData.Age() >= 30)
			{
				return 45;
			}
		}
		ePlayerWantsTransferReason transferWantedReason = playerData.GetTransferWantedReason();
		if (averageRating > averageRating2 - 3 && (transferWantedReason == ePlayerWantsTransferReason.MORE_GAMETIME || transferWantedReason == ePlayerWantsTransferReason.BETTER_CONTRACT))
		{
			return 45;
		}
		if (averageRating >= averageRating2 - 6 && PlayerSquadStatus.Compare(ePlayerSquadStatus.Rotation, squadStatus) >= 0 && !flag)
		{
			return 45;
		}
		if (averageRating >= averageRating2 - 9 && PlayerSquadStatus.Compare(ePlayerSquadStatus.Backup, squadStatus) >= 0 && !flag)
		{
			return 45;
		}
		if (num == Game.instance.CurrentManagedClubId && rating <= averageRating)
		{
			return 45;
		}
		if (averageRating > averageRating2 - 5)
		{
			return 40;
		}
		return 0;
	}

	public static bool IsRetiring(int playerID)
	{
		bool result = false;
		PlayerStatusInfo status = GetStatus(playerID);
		if (status != null && status.retirementWarned)
		{
			result = true;
		}
		return result;
	}

	public static PlayerStatusInfo GetStatus(int playerID)
	{
		ManagedClub currentUserClub = Game.GetCurrentUserClub(0);
		int clubID = currentUserClub.ClubID;
		short clubID2 = GetClubID(playerID);
		PlayerStatusInfo result = null;
		if (clubID2 == clubID)
		{
			if (!currentUserClub.PlayerStatus.ContainsKey(playerID))
			{
				Debug.LogError("Player::GetStatus() - PlayerStatus was not found for playerid " + playerID);
				PlayerStatusInfo.Init(currentUserClub.PlayerStatus, playerID, 0, isTransfer: true);
			}
			result = currentUserClub.PlayerStatus[playerID];
		}
		else if (currentUserClub.PlayerStatus.ContainsKey(playerID))
		{
			result = currentUserClub.PlayerStatus[playerID];
		}
		return result;
	}

	public static int GetGenericPositionMask(int position)
	{
		if ((position & 1) != 0)
		{
			return 1;
		}
		if ((position & 0xE) != 0)
		{
			return 14;
		}
		if ((position & 0x1FF0) != 0)
		{
			return 8176;
		}
		return 57344;
	}

	internal static int RetirementCategory(int _PlayerId)
	{
		int ratingQuick = GetRatingQuick(_PlayerId);
		int genericPositionMask = GetGenericPositionMask(GetPosition(_PlayerId));
		if (genericPositionMask == 1)
		{
			if (ratingQuick > 93)
			{
				return 0;
			}
			if (ratingQuick > 89)
			{
				return 1;
			}
			return 2;
		}
		if (GetLongevity(_PlayerId) == PLAYER_LONGEVITY.GOOD)
		{
			if (ratingQuick > 93)
			{
				return 3;
			}
			if (ratingQuick > 89)
			{
				return 4;
			}
			return 5;
		}
		if (ratingQuick > 93)
		{
			return 6;
		}
		if (ratingQuick > 89)
		{
			return 7;
		}
		return 8;
	}

	public static float GetCurrentSeasonNumGamesPlayed(int playerID, short clubID)
	{
		StatContainer currentSeasonStatsForClub = PlayerCareerStats.GetCurrentSeasonStatsForClub(playerID, clubID);
		return currentSeasonStatsForClub.GetStat(StatType.App) + currentSeasonStatsForClub.GetStat(StatType.SubApp) / 2;
	}

	public static int GetNumberOfConsecutiveGamesPlayed(int playerid, short clubID)
	{
		List<ClubScheduleInfo> schedule = Club.GetSchedule(clubID);
		int num = 0;
		for (int num2 = schedule.Count - 1; num2 >= 0; num2--)
		{
			Fixture fixture = schedule[num2].Fixture;
			if (fixture.IsPlayed && fixture.IsForClub(clubID) && fixture.competitiontype != 5)
			{
				if (fixture.GetPlayerShareOfMatch(playerid) == 0f)
				{
					break;
				}
				num++;
			}
		}
		return num;
	}

	public static int GetNumberOfNoneConsecutiveGamesPlayed(int playerid, short clubID)
	{
		List<ClubScheduleInfo> list = (from i in Club.GetSchedule(clubID)
			where i.Fixture.isPlayed && i.Fixture.IsForClub(clubID)
			select i).ToList();
		int num = 0;
		for (int num2 = list.Count - 1; num2 >= 0; num2--)
		{
			Fixture fixture = list[num2].Fixture;
			if (fixture.competitiontype != 5)
			{
				if (fixture.GetPlayerShareOfMatch(playerid) != 0f)
				{
					break;
				}
				num++;
			}
		}
		return num;
	}

	public static float GetRecentNumGamesPlayed(int playerID, short clubID, int numGames = 10)
	{
		List<ClubScheduleInfo> schedule = Club.GetSchedule(clubID);
		float num = 0f;
		int num2 = 0;
		int num3 = schedule.Count - 1;
		while (num3 >= 0 && num2 < numGames)
		{
			Fixture fixture = schedule[num3].Fixture;
			if (fixture.IsPlayed && fixture.IsForClub(clubID) && fixture.competitiontype != 5)
			{
				num += fixture.GetPlayerShareOfMatch(playerID);
				num2++;
			}
			num3--;
		}
		return num;
	}

	public static bool IsChineseBuildPlayer(int playerID)
	{
		return Translation.chineseBuildPlayerNames.ContainsKey(PlayerData.GetPlayerDataID(playerID));
	}

	public static string GetName(int playerID, bool longName = false)
	{
		return GetName(playerID, 0, longName);
	}

	public static string GetName(int playerID, int maxLength, bool longName = false)
	{
		if (longName)
		{
			string forename = PlayerData.GetForename(playerID);
			string surname = PlayerData.GetSurname(playerID);
			if (forename == null || surname == null || forename.Length == 0 || surname.Length == 0)
			{
				return playerID.ToString() ?? "";
			}
			string text = "";
			if (maxLength != 0)
			{
				text = forename.Substring(0, 1) + ". " + surname;
				if (text.Length > maxLength)
				{
					text = text.Substring(0, maxLength) + "..";
				}
			}
			else
			{
				text = forename + " " + surname;
			}
			return text;
		}
		return GetShortName(playerID);
	}

	public static string GetSurname(int playerID, bool checkDuplicate = false)
	{
		string text = "";
		if (playerID > 0)
		{
			text = PlayerData.GetSurname(playerID);
			if (text == null || text.Length == 0)
			{
				return playerID.ToString() ?? "";
			}
			if (checkDuplicate)
			{
				foreach (int item in PlayerSquad.AllPlayers(GetClubID(playerID)))
				{
					if (GetSurname(item) == text && item != playerID)
					{
						return GetShortName(playerID);
					}
				}
			}
		}
		return text;
	}

	public static string GetShortName(int PlayerID)
	{
		string result = "";
		if (PlayerID > 0)
		{
			string forename = PlayerData.GetForename(PlayerID);
			string surname = PlayerData.GetSurname(PlayerID);
			result = (string.IsNullOrEmpty(forename) ? surname : (forename.Substring(0, 1) + ". " + surname));
		}
		return result;
	}

	public static int GetAssociationID(int playerID)
	{
		return Club.GetAssociationID(Game.instance.g_PlayerData[playerID].ClubId);
	}

	public static int GetRandomPosition()
	{
		return Game.instance.Random.GetRand(3) switch
		{
			0 => 1, 
			1 => 14, 
			2 => 896, 
			3 => 57344, 
			_ => 0, 
		};
	}

	public static bool NotPlayedInLastFourMatches(int playerID)
	{
		List<ClubScheduleInfo> lastNumberOfPlayedFixtures = Club.GetLastNumberOfPlayedFixtures(Game.instance.CurrentManagedClubId, 4);
		if (lastNumberOfPlayedFixtures.Count != 0)
		{
			return lastNumberOfPlayedFixtures.Any((ClubScheduleInfo i) => i.Fixture.GetPlayerShareOfMatch(playerID) == 0f);
		}
		return true;
	}

	public static bool IsNotMatchFit(int playerID)
	{
		if (GetPosition(playerID) != 1 && GetFitness(playerID) <= 90)
		{
			return NotPlayedInLastFourMatches(playerID);
		}
		return false;
	}

	public static bool GetInDecline(int playerID)
	{
		PlayerData playerData = Game.instance.g_PlayerData[playerID];
		if (playerData.InDecline)
		{
			return true;
		}
		if (playerData.Age() >= playerData.GetDeclineAge())
		{
			return GetRating(playerID, Game.instance.CurrentDate) < playerData.Potential;
		}
		return false;
	}

	public static byte GetDescription(int playerID)
	{
		return Game.instance.g_PlayerData[playerID].GetDescription();
	}

	public static byte GetAge(int playerID)
	{
		return (byte)Game.instance.g_PlayerData[playerID].Age();
	}

	public static byte GetAge(int playerID, DateTime theDate)
	{
		return Game.instance.g_PlayerData[playerID].Age(theDate);
	}

	public static double GetAgeInDays(int playerID)
	{
		return PlayerData.GetDateOfBirth(playerID).Subtract(DateTime.MinValue).TotalDays;
	}

	public static int YearsToRetirement(int playerId)
	{
		return Game.instance.g_PlayerData[playerId].YearsToRetirement();
	}

	public static bool IsSuitableBigPlayer(int playerid)
	{
		PlayerData playerData = Game.instance.g_PlayerData[playerid];
		if (!IsTransferBanned(playerid) && playerData.CacheRating > 90 && GetClubID(playerid) != Game.instance.CurrentManagedClubId && playerData.YearsToRetirement() > 1)
		{
			return true;
		}
		return false;
	}

	public static int GetAgeInMonths(int playerID)
	{
		DateTime currentDate = Game.instance.CurrentDate;
		DateTime dateOfBirth = PlayerData.GetDateOfBirth(playerID);
		return currentDate.Month - dateOfBirth.Month + 12 * (currentDate.Year - dateOfBirth.Year);
	}

	public static short GetClubIDLoanParent(int playerID)
	{
		if (PlayerData.IsLoanedToClub(playerID))
		{
			return Game.instance.g_PlayerData[playerID].LoanParentClubId;
		}
		return -1;
	}

	public static short GetClubID(int playerID)
	{
		if (playerID == 0)
		{
			return 0;
		}
		return Game.instance.g_PlayerData[playerID].ClubId;
	}

	public static int GetDisplayRating(int playerID)
	{
		PlayerAttributes attributesForDisplay = Attributes.GetAttributesForDisplay(playerID);
		if (GetPosition(playerID) != 1)
		{
			return (int)attributesForDisplay.groupedAttrs[0];
		}
		return (int)attributesForDisplay.groupedAttrs[0];
	}

	public static float GetDisplayRatingByDate(int playerID, DateTime date)
	{
		AttrOpts attrOpts = new AttrOpts();
		attrOpts.SetDefaultAttrOpts();
		attrOpts.date = date;
		attrOpts.playertrainingon = false;
		PlayerAttributes attributesForDisplay = Attributes.GetAttributesForDisplay(playerID, attrOpts);
		if (GetPosition(playerID) != 1)
		{
			return attributesForDisplay.groupedAttrs[0];
		}
		return attributesForDisplay.groupedAttrs[0];
	}

	public static double GetDisplayRatingPrecise(int playerID)
	{
		return Game.instance.g_PlayerData[playerID].GetDisplayRatingPrecise();
	}

	public static double GetDisplayRatingAfterTraining(int playerID)
	{
		return Game.instance.g_PlayerData[playerID].GetDisplayRatingAfterTraining();
	}

	public static byte GetRating(int playerID)
	{
		return GetRating(playerID, DateTime.MinValue);
	}

	public static double GetRatingPrecise(int playerID, DateTime theDate, bool getTrainingIncrease = false)
	{
		return Game.instance.g_PlayerData[playerID].GetRatingPrecise(theDate, getTrainingIncrease);
	}

	public static byte GetRating(int playerID, DateTime theDate)
	{
		return Game.instance.g_PlayerData[playerID].GetRating(theDate);
	}

	public static DateTime GetPotentialDate(int playerID)
	{
		return Game.instance.g_PlayerData[playerID].GetPotentialDate();
	}

	public static byte GetRatingQuick(int playerID)
	{
		return Math.Max((byte)60, Game.instance.g_PlayerData[playerID].CacheRating);
	}

	public static int GetPosition(int playerID, bool preferred = false)
	{
		return PlayerData.GetPosition(playerID, preferred);
	}

	public static byte GetFitness(int playerID)
	{
		if (GetClubID(playerID) == Game.instance.CurrentManagedClubId)
		{
			return GetStatus(playerID)?.fitness ?? 0;
		}
		return CalcFitness(playerID);
	}

	public static bool GetFirstTeam(int playerID)
	{
		bool result = false;
		short clubID = GetClubID(playerID);
		if (clubID == Game.instance.CurrentManagedClubId)
		{
			if (GetStatus(playerID) != null)
			{
				result = GetStatus(playerID).firstTeam;
			}
		}
		else if (new List<int>(PlayerSquad.GetPlayers(clubID).players.OrderByDescending((int i) => Game.instance.g_PlayerData[i].Rating).ToList()).IndexOf(playerID) < 25)
		{
			result = true;
		}
		return result;
	}

	public static PLAYER_LONGEVITY GetLongevity(int playerID)
	{
		return Game.instance.g_PlayerData[playerID].GetLongevity();
	}

	public static byte GetMatchDayRating(int playerID)
	{
		return GetRatingQuick(playerID);
	}

	public static float GetValue(int playerID)
	{
		return Game.instance.g_PlayerData[playerID].GetValue();
	}

	public static byte GetMorale(int playerID)
	{
		return PlayerData.GetMorale(playerID);
	}

	public static int IsTrainingAsDesired(int playerID, int workrate, int intensity)
	{
		if (workrate == 0)
		{
			return 0;
		}
		if (workrate >= 75)
		{
			return intensity - 1;
		}
		if (workrate <= 40)
		{
			switch (intensity)
			{
			case 2:
				return -1;
			case 1:
				return 0;
			case 0:
				return 1;
			}
		}
		return 0;
	}

	public static bool IsPlayingWell(int playerID)
	{
		float recentAveragePerformance = GetRecentAveragePerformance(playerID);
		if (recentAveragePerformance != -1f)
		{
			return recentAveragePerformance >= 6f;
		}
		return true;
	}

	public static float GetRecentAveragePerformance(int playerID)
	{
		float result = 0f;
		short clubID = GetClubID(playerID);
		int num = 0;
		byte b = 0;
		byte b2 = 0;
		byte b3 = 0;
		int num2 = 0;
		if (clubID != 0)
		{
			List<ClubScheduleInfo> schedule = Club.GetSchedule(clubID);
			int num3 = schedule.Count - 1;
			while (num3 >= 0 && num2 != 6)
			{
				Fixture fixture = schedule[num3].Fixture;
				if (fixture.IsPlayed && (fixture.homeClubId == clubID || fixture.awayClubId == clubID))
				{
					int num4 = -1;
					int num5 = -1;
					fixture.RestoreFromSQL();
					for (int i = 0; i < 18; i++)
					{
						if (fixture.tm1[i].playerId == playerID && fixture.tm1[i].performance != 0)
						{
							num4 = i;
							break;
						}
					}
					if (num4 == -1)
					{
						for (int j = 0; j < 18; j++)
						{
							if (fixture.tm2[j].playerId == playerID && fixture.tm2[j].performance != 0)
							{
								num5 = j;
								break;
							}
						}
					}
					if (num4 != -1 || num5 != -1)
					{
						if (num4 != -1 && fixture.tm1[num4].TimePlayed > 0)
						{
							if (fixture.tm1[num4].timeStarted == 0)
							{
								b++;
								b3 += fixture.tm1[num4].performance;
								num++;
							}
							else if (fixture.tm1[num4].timeStarted > 0)
							{
								b2++;
								b3 += fixture.tm1[num4].performance;
								num++;
							}
						}
						else if (fixture.tm2[num5].TimePlayed > 0)
						{
							if (fixture.tm2[num5].timeStarted == 0)
							{
								b++;
								b3 += fixture.tm2[num5].performance;
								num++;
							}
							else if (fixture.tm2[num5].timeStarted > 0)
							{
								b2++;
								b3 += fixture.tm2[num5].performance;
								num++;
							}
						}
						if (b > 0 || b2 > 0)
						{
							num2++;
							result = b3 / (b + b2);
						}
					}
				}
				num3--;
			}
		}
		if (num2 <= 4)
		{
			result = -1f;
		}
		return result;
	}

	public static byte GetRetirementAge(int playerID)
	{
		return Game.instance.g_PlayerData[playerID].RetirementAge();
	}

	public static byte CalcFitness(int playerID)
	{
		int num = Game.instance.Seed + playerID + Game.instance.CurrentDate.GetEpochSeconds();
		int seed = Game.instance.Random.GetSeed();
		Game.instance.Random.SetSeed(num % 32768);
		byte result = (byte)(90 + Game.instance.Random.GetRand() % 11);
		Game.instance.Random.SetSeed(seed);
		return result;
	}

	public static double GetPlayerBasicValue(byte playerRating, bool isPeaked, int playerPotential = 0)
	{
		if (isPeaked)
		{
			return 600000.0 * (Math.Pow((int)playerRating, 21.0) / Math.Pow(99.0, 21.0));
		}
		return 600000.0 * (Math.Pow((int)playerRating, 21.0) / Math.Pow(99.0, 21.0)) * 0.9 + 600000.0 * (Math.Pow(playerPotential, 21.0) / Math.Pow(99.0, 21.0)) * 0.1;
	}

	public static byte ChangeMorale(int playerID, int change, bool replayed)
	{
		if (GetClubID(playerID) == Game.instance.CurrentManagedClubId)
		{
			PlayerStatusInfo status = GetStatus(playerID);
			status.morale = (byte)Math.Max(1, Math.Min(10, status.morale + change));
			if (IsPlayerMoralCapped(playerID) && status.morale > 5)
			{
				status.morale = 5;
			}
			return 1;
		}
		return 0;
	}

	private static bool IsPlayerMoralCapped(int playerID)
	{
		bool result = false;
		if (PlayerData.HasGameTimeConcerns(playerID) || Transfers.instance.IsPlayerOnTransferList(playerID))
		{
			result = true;
		}
		return result;
	}

	public static void SetMorale(int playerID, int newMorale)
	{
		if (GetClubID(playerID) == Game.instance.CurrentManagedClubId)
		{
			PlayerStatusInfo status = GetStatus(playerID);
			status.morale = (byte)Math.Max(1, Math.Min(10, newMorale));
			if (IsPlayerMoralCapped(playerID) && status.morale > 5)
			{
				status.morale = 5;
			}
		}
	}

	public static void RejectTransferOffer(int playerID, eBidStatus rejectReason)
	{
		int num = 0;
		short clubID = GetClubID(playerID);
		PlayerData playerData = Game.instance.g_PlayerData[playerID];
		bool flag = PlayerData.HasGameTimeConcerns(playerID);
		bool flag2 = playerData.SquadStatus == ePlayerSquadStatus.KeyPlayer || playerData.SquadStatus == ePlayerSquadStatus.FirstTeam;
		bool flag3 = Club.GetNumberOfLeagueGamesPlayed(clubID) < 10 || Club.GetClubPerformanceInLeague(clubID) >= 0;
		switch (rejectReason)
		{
		case eBidStatus.CLUB_REJECTED_NO_REASON_GIVEN:
			if (!flag || !flag3)
			{
				num = -1;
			}
			break;
		case eBidStatus.CLUB_REJECTED_IMPORTANT_PLAYER:
		case eBidStatus.REJECTED_CLUB_RATING:
			if (!flag || !flag2)
			{
				num = -1;
			}
			break;
		case eBidStatus.CLUB_REJECTED_BID_TOO_LOW:
		case eBidStatus.REJECTED_MOVE_COUNTRY:
			if (!flag && !flag3)
			{
				num = -1;
			}
			break;
		case eBidStatus.REJECTED_HAPPY_AT_CLUB:
			if (!flag || GetAge(playerID) > 22)
			{
				num = -1;
			}
			break;
		}
		if (num != 0)
		{
			ChangeMorale(playerID, num, replayed: true);
		}
	}

	public static List<int> GetFavouriteTeamMates(int playerId)
	{
		return new List<int>();
	}

	public static void AddToShortList(int playerID, bool replayed)
	{
		PlayerShortlist playerShortlist = new PlayerShortlist();
		playerShortlist.date = Game.instance.CurrentDate;
		playerShortlist.notes = "Blah blah blah";
		if (!Game.instance.CurrentManagedClub.PlayerShortlist.ContainsKey(playerID))
		{
			Game.instance.CurrentManagedClub.PlayerShortlist.Add(playerID, playerShortlist);
		}
	}

	public static void RemoveFromShortList(int playerID)
	{
		if (Game.instance.CurrentManagedClub.PlayerShortlist.TryGetValue(playerID, out var _))
		{
			Game.instance.CurrentManagedClub.PlayerShortlist.Remove(playerID);
		}
	}

	public static bool IsOnShortList(int playerID)
	{
		return Game.instance.CurrentManagedClub.PlayerShortlist.ContainsKey(playerID);
	}

	public static bool IsInjured(int playerID)
	{
		return Game.instance.g_PlayerData[playerID].IsInjured();
	}

	public static bool WasInjured(int playerID, DateTime date)
	{
		InjuredPlayers g_injuredPlayers = Game.instance.g_injuredPlayers;
		if (g_injuredPlayers.Injuries.TryGetValue(playerID, out var value) && date >= value.Start && date <= value.End)
		{
			return true;
		}
		if (g_injuredPlayers.InjuriesThisSeason.ContainsKey(playerID))
		{
			foreach (InjuredPlayer item in g_injuredPlayers.InjuriesThisSeason[playerID])
			{
				if (date >= item.Start && date <= item.End)
				{
					return true;
				}
			}
		}
		return false;
	}

	public static string getInjuryName(int playerID)
	{
		string result = "UNKNOWN";
		if (Game.instance.g_injuredPlayers.Injuries.ContainsKey(playerID))
		{
			DateTime start = Game.instance.g_injuredPlayers.Injuries[playerID].Start;
			DateTime end = Game.instance.g_injuredPlayers.Injuries[playerID].End;
			int type = Game.instance.g_injuredPlayers.Injuries[playerID].Type;
			if (start <= Game.instance.CurrentDate && end > Game.instance.CurrentDate)
			{
				result = Translation.GetText(InjuriesData.g_newInjuries[type][0]);
			}
		}
		return result;
	}

	public static DateTime getInjuryReturnDate(int playerID)
	{
		DateTime result = DateTime.MinValue;
		if (Game.instance.g_injuredPlayers.Injuries.ContainsKey(playerID) && Game.instance.g_injuredPlayers.Injuries[playerID].Start <= Game.instance.CurrentDate && Game.instance.g_injuredPlayers.Injuries[playerID].End > Game.instance.CurrentDate)
		{
			result = Game.instance.g_injuredPlayers.Injuries[playerID].End;
		}
		return result;
	}

	public static double getInjuryDaysLeft(int playerID)
	{
		return (getInjuryReturnDate(playerID) - Game.instance.CurrentDate).TotalDays;
	}

	public static bool IsDaysInjuredGreaterThan(int playerID, int injuryDays = 0)
	{
		if (!IsInjured(playerID))
		{
			return false;
		}
		return getInjuryDaysLeft(playerID) > (double)injuryDays;
	}

	public static double getHealCost(int playerID)
	{
		return Math.Ceiling(getInjuryDaysLeft(playerID) / 7.0) * 25.0;
	}

	public static bool IsResting(int playerID)
	{
		return Game.instance.g_trainingDrillManager.RestingPlayers.ContainsKey(playerID);
	}

	public static DateTime GetRestReturnDate(int playerID)
	{
		return Game.instance.g_trainingDrillManager.RestingPlayers[playerID];
	}

	public static void ReturnFromRest(int playerID)
	{
		Game.instance.g_trainingDrillManager.RestingPlayers.Remove(playerID);
	}

	public static float GetAttributeIncrease(int playerID, int att, float attVal)
	{
		if (Attributes.attributesByDescription[GetDescription(playerID)].Length > att)
		{
			return attVal * (float)Attributes.attributesByDescription[GetDescription(playerID)][att];
		}
		return attVal;
	}

	public static bool HasPlayerRatingIncreasedRecently(int playerID)
	{
		if (Game.instance.g_trainingManager.ImprovedPlayers.ContainsKey(playerID))
		{
			return Game.instance.g_trainingManager.ImprovedPlayers[playerID] > Game.instance.CurrentDate.AddMonths(-1);
		}
		return false;
	}

	public static bool IsSuspended(int playerID, int assocID, int competitionType, int competitionID)
	{
		return Game.instance.g_PlayerData[playerID].IsSuspended(assocID, competitionType, competitionID);
	}

	public static int getSuspendedNumGames(int playerID, int assocID, int competitionType, int competitionID)
	{
		int result = -1;
		if (competitionType == 4)
		{
			if (Game.instance.g_continentalCompetitions[competitionID] != null && Game.instance.g_continentalCompetitions[competitionID].matchban.ContainsKey(playerID) && Game.instance.g_continentalCompetitions[competitionID].matchban[playerID] > 0 && GetClubID(playerID) != 0)
			{
				result = Game.instance.g_continentalCompetitions[competitionID].matchban[playerID];
			}
		}
		else if (assocID != 2 && assocID != 3 && Game.instance.g_association.ContainsKey(assocID))
		{
			Dictionary<int, int> matchBan = Game.instance.g_association[assocID].MatchBan;
			if (matchBan.ContainsKey(playerID) && matchBan[playerID] > 0 && GetClubID(playerID) != 0)
			{
				result = matchBan[playerID];
			}
		}
		return result;
	}

	public static void InitPlayerProfiles()
	{
		Game.instance.g_player_respawnprofile.Init();
	}

	public static void RemoveInvalidPlayersFromLineup(int playerID, bool createMsg = true)
	{
		ManagedClub currentManagedClub = Game.instance.CurrentManagedClub;
		int num = Array.IndexOf(currentManagedClub.currentTactics.lineUp, playerID);
		if (num == -1)
		{
			return;
		}
		currentManagedClub.currentTactics.lineUp[num] = 0;
		if (createMsg)
		{
			MessageManager.TransferCausedLineupChange(playerID);
		}
		bool flag = (GetPosition(playerID) & 1) != 0;
		PlayerSquad players = Club.GetPlayers(currentManagedClub.ClubID);
		int count = players.Count;
		bool flag2 = false;
		for (int i = 0; i < count; i++)
		{
			int num2 = players[i];
			bool flag3 = (GetPosition(num2) & 1) != 0;
			if (flag == flag3 && num2 != playerID && !currentManagedClub.currentTactics.lineUp.Contains(num2))
			{
				currentManagedClub.currentTactics.lineUp[num] = num2;
				flag2 = true;
			}
		}
		if (!flag2)
		{
			Game.instance.g_tm[0].clubid = currentManagedClub.ClubID;
			Game.instance.g_tm[0].aim = 1;
			Team.PickTeam(Game.instance.g_tm[0], currentManagedClub.currentTactics.inst.formationid, 0);
			Team.PickTactics(Game.instance.g_tm[0], 0, quickload: false, 0);
		}
	}

	public static byte GetRatingInClub(int playerID, short clubID)
	{
		List<int> list = new List<int>(Club.GetPlayers(clubID).players);
		list.Sort(CustomSort.SortByRatingDesc);
		for (byte b = 0; b < list.Count; b++)
		{
			if (list[b] == playerID)
			{
				return b;
			}
		}
		return 0;
	}

	public static void RemoveNewlyRetired(List<int> playersToRemove)
	{
		foreach (int item in playersToRemove)
		{
			Game.instance.sqLiteService.retiredPlayerTbl.Add(item);
			PlayerData.RemovePlayer(item);
		}
	}

	public static bool HasRecentOrImminentRatingChange(int playerID)
	{
		DateTime currentDate = Game.instance.CurrentDate;
		PlayerRatingHistory lastRatingChange = GetLastRatingChange(playerID);
		int num = 2592000;
		if (lastRatingChange != null && currentDate - lastRatingChange.date < TimeSpan.FromSeconds(num))
		{
			return true;
		}
		return GetRating(playerID, currentDate) != GetRating(playerID, currentDate.AddSeconds(num));
	}

	public static bool HasRecentManagedRatingChange(int playerID)
	{
		DateTime currentDate = Game.instance.CurrentDate;
		DateTime dateTime = currentDate.AddSeconds(-13132800.0);
		ManagedRatingList managedRatingList = Game.instance.g_PlayerData_ManagedRating[playerID];
		int num = 0;
		if (managedRatingList != null)
		{
			int num2 = managedRatingList.Count() - 1;
			while (num2 >= 0 && managedRatingList[num2].timestamp >= dateTime)
			{
				num += managedRatingList[num2].rating;
				num2--;
			}
		}
		if (Math.Abs(num) > 0)
		{
			return true;
		}
		return false;
	}

	public static PlayerRatingHistory GetLastRatingChange(int playerID)
	{
		List<PlayerRatingHistory> playerRatingHistory = Helpers.getPlayerRatingHistory(playerID);
		if (playerRatingHistory != null && playerRatingHistory.Count > 0)
		{
			return playerRatingHistory[playerRatingHistory.Count - 1];
		}
		return null;
	}

	public static bool InDeclineAccurate(int playerID)
	{
		if (PlayerData.GetIndecline(playerID))
		{
			return true;
		}
		return GetPotentialDate(playerID) < Game.instance.CurrentDate;
	}

	public static bool PlayerBidAllowed(int playerID, bool ignoreTransferBan)
	{
		short currentUserClubID = Game.GetCurrentUserClubID();
		bool result = true;
		if (currentUserClubID == 0)
		{
			Helpers.ShowToast(Translation.GetText("transfer-bid-not-allowed") + " " + Translation.GetText("you-are-currently-unemployed"));
			result = false;
		}
		else
		{
			TransferBid transferBid = Transfers.instance.bidManager.UnprocessedBid(playerID, currentUserClubID);
			if (transferBid != null && transferBid.processed != eBidStatus.CLUB_REJECTED_BID_TOO_LOW)
			{
				Helpers.ShowToast(Translation.GetText("transfer-bid-not-allowed") + " " + Translation.GetText("transfer-bid-already-being-considered"));
				result = false;
			}
			if (!ignoreTransferBan && IsTransferBanned(playerID))
			{
				Helpers.ShowToast(Translation.GetText("transfer-bid-not-allowed") + " " + Translation.GetText("transfer-unmanaged-player-transfer-banned"));
				result = false;
			}
			if (Transfers.instance.bidManager.hasImminentTransferBid(playerID))
			{
				Helpers.ShowToast(Translation.GetText("transfer-bid-not-allowed") + " " + Translation.GetText("player-in-talks-with-other-club"));
				result = false;
			}
			if (Transfers.instance.clubSquadCapReached(currentUserClubID))
			{
				Helpers.ShowToast(Translation.GetText("transfer-bid-not-allowed") + " " + Translation.GetText("squad-cap-reached"));
				result = false;
			}
		}
		return result;
	}

	public static bool IsTransferBanned(int playerid)
	{
		return Game.instance.g_PlayerData[playerid].IsTransferBanned();
	}

	public static void RefreshPlayerCachedRating(int playerId)
	{
		PlayerData playerData = Game.instance.g_PlayerData[playerId];
		byte rating = GetRating(playerId, Game.instance.CurrentDate);
		if (rating < playerData.CacheRating)
		{
			playerData.InDecline = true;
		}
		playerData.CacheRating = rating;
	}

	public static bool IsRetired(int playerId)
	{
		return Game.instance.g_PlayerData[playerId].Status == ePlayerStatus.RETIRED;
	}

	public static bool IsEligibleForTransferBids(int playerId)
	{
		PlayerData playerData = Game.instance.g_PlayerData[playerId];
		if (playerData.IsLoanedToClub() || playerData.IsTransferBanned())
		{
			return false;
		}
		if (Transfers.instance.bidManager.getUnprocessedBidCountForPlayer(playerId) == 0 && !Transfers.instance.isPlayerOnInProgressList(playerId))
		{
			return true;
		}
		return false;
	}

	public static bool IsOnTransferList(int playerId)
	{
		return Transfers.instance.transferListManager.IsOnTransferList(playerId);
	}

	public static bool IsOnLoanList(int playerId)
	{
		return Transfers.instance.transferListManager.IsOnLoanList(playerId);
	}

	public static bool AddToTransferList(int playerId)
	{
		return Transfers.instance.transferListManager.AddToTransferList(playerId);
	}

	internal static int GetLeagueDataId(int PlayerId)
	{
		return Club.GetLeagueDataId(GetClubID(PlayerId));
	}

	public static PlayerTeamCohesion GetTeamCohesion(int playerID)
	{
		return GetStatus(playerID).Cohesion;
	}

	public static int GetWages(int playerID)
	{
		return Game.instance.g_PlayerData[playerID].ContractWage;
	}

	public static double GetDaysRemainingOnContract(int playerID)
	{
		return (Game.instance.g_PlayerData[playerID].ContractExpiryDate - Game.instance.CurrentDate).TotalDays;
	}

	public static DateTime GetContractExpiryDate(int playerId)
	{
		return Game.instance.g_PlayerData[playerId].ContractExpiryDate;
	}

	public static byte GetYearsLeftOnContract(int playerId)
	{
		return (byte)(Game.instance.g_PlayerData[playerId].ContractExpiryDate.Year - GetNextContractExpiryDate().Year + 1);
	}

	public static DateTime GetNextContractExpiryDate(int leagueId = -1)
	{
		if (leagueId == -1)
		{
			short currentManagedClubId = Game.instance.CurrentManagedClubId;
			leagueId = Game.instance.g_ClubData[currentManagedClubId].LeagueID;
		}
		if (leagueId != 0)
		{
			LeagueData leagueData = LeagueData.GetLeagueData(leagueId);
			DateTime currentDate = Game.instance.CurrentDate;
			if (currentDate.Month >= leagueData.SeasonOneStart)
			{
				return new DateTime(currentDate.Year + 1, leagueData.SeasonOneStart, 1, 0, 0, 0);
			}
			return new DateTime(currentDate.Year, leagueData.SeasonOneStart, 1, 0, 0, 0);
		}
		return DateTime.MinValue;
	}

	public static bool WillPlayerRenewContract(int playerId)
	{
		return Game.instance.g_PlayerData[playerId].WillPlayerRenewContract();
	}

	public static bool WillAcceptSquadStatus(PlayerData playerData, ePlayerSquadStatus squadStatus, short clubID)
	{
		ePlayerSquadStatus expectedSquadStatusForClub = GetExpectedSquadStatusForClub(playerData, clubID);
		int num = PlayerSquadStatus.Compare(squadStatus, expectedSquadStatusForClub);
		if (num >= 0)
		{
			return true;
		}
		if (num >= -1)
		{
			return playerData.IsPlayerHappyWithReducedSquadStatus(clubID);
		}
		return false;
	}

	public static ePlayerSquadStatus GetExpectedSquadStatusForClub(PlayerData playerData, short clubID)
	{
		return playerData.GetExpectedSquadStatusForClub(clubID);
	}

	public static ePlayerSquadStatus GetWantedSquadStatusForAge(int playerID)
	{
		return Game.instance.g_PlayerData[playerID].GetWantedSquadStatusForAge();
	}

	public static bool IsPlayerHappyWithReducedSquadStatus(int playerID, short clubID)
	{
		return Game.instance.g_PlayerData[playerID].IsPlayerHappyWithReducedSquadStatus(clubID);
	}

	public static double GetPlayerSquadRoleMultiplier(byte playerRating, double teamRating)
	{
		return (double)(int)playerRating / teamRating * 0.5 + 0.5;
	}

	public static double GetTeamRatingMultiplier(int teamRating)
	{
		return (double)teamRating / 94.0;
	}

	public static double GetLeagueRatingMultiplier(float leagueRating)
	{
		return (double)leagueRating / 89.0;
	}

	public static double GetContractLengthMultiplier(int contractLength)
	{
		return 1.0 + (double)(contractLength - 1) / 40.0;
	}

	public static int GenerateNewContractWages(int playerId, short clubIdFrom, int contractLength)
	{
		PlayerData playerData = Game.instance.g_PlayerData[playerId];
		short leagueID = Game.instance.g_ClubData[clubIdFrom].LeagueID;
		byte averageRating = Club.GetAverageRating(clubIdFrom);
		byte ratingQuick = playerData.GetRatingQuick();
		int num = GenerateNewContractWages(GetPlayerBasicValue(ratingQuick, playerData.InDecline, playerData.Potential), GetPlayerSquadRoleMultiplier(ratingQuick, (int)averageRating), GetTeamRatingMultiplier(averageRating), GetLeagueRatingMultiplier(LeagueData.GetRatingQuick(leagueID)), GetContractLengthMultiplier(contractLength));
		if (Game.GetCurrentUserClubID() == clubIdFrom && UserManagerProfileController.UserHasTrait(ManagerTraitArea.PlayerWages))
		{
			num = (int)UserManagerProfileController.ApplyTrait(ManagerTraitArea.PlayerWages, num);
		}
		return Helpers.RoundMonetaryValue(num);
	}

	public static int GenerateNewContractWages(double value, double squadRoleMultiplier, double teamRatingMultiplier, double divisionRatingMultiplier, double contractLengthMultiplier)
	{
		return (int)Math.Max(value * squadRoleMultiplier * teamRatingMultiplier * divisionRatingMultiplier * contractLengthMultiplier, 50.0);
	}

	public static bool CanExtendContract(int playerID)
	{
		return GetYearsLeftOnContract(playerID) < GetMaximumContractOfferLength(playerID);
	}

	public static float CalculateExpectedNewContractLength(int playerID)
	{
		int minimumContractOfferLength = GetMinimumContractOfferLength(playerID);
		return Mathf.Round((float)(GetMaximumContractOfferLength(playerID) - minimumContractOfferLength) / 2f + (float)minimumContractOfferLength);
	}

	public static double GetRequestedWageMultiplier(PlayerData playerData)
	{
		int seed = Game.instance.Random.GetSeed();
		if (playerData.IsLoanedToClub())
		{
			Game.instance.Random.SetSeed(playerData.LoanParentClubId + playerData.PlayerId);
		}
		else
		{
			Game.instance.Random.SetSeed(playerData.ClubId + playerData.PlayerId);
		}
		double rand = Game.instance.Random.GetRand(1.0, 1.1);
		Game.instance.Random.SetSeed(seed);
		return rand;
	}

	public static int GetMinimumContractOfferLength(int playerID)
	{
		PlayerData playerData = Game.instance.g_PlayerData[playerID];
		int num = 1;
		if ((double)playerData.GetYearsRemainingOnContract() <= 0.5)
		{
			num = 2;
		}
		if (playerData.ClubId == Game.instance.CurrentManagedClubId)
		{
			num = GetYearsLeftOnContract(playerID) + 1;
		}
		if (playerData.Age() <= 22)
		{
			num = Math.Max(num, 3);
		}
		return num;
	}

	public static int GetMaximumContractOfferLength(int playerID)
	{
		int result = 5;
		if (GetAge(playerID) >= 32)
		{
			result = 3;
		}
		return result;
	}

	public static float CalculateTotalSigningFee(int playerId, short toClubId)
	{
		return CalculatePlayerSigningFee(playerId, toClubId) + CalculatePlayerCompensationFee(playerId);
	}

	public static float CalculatePlayerSigningFee(int playerId, short toClubId)
	{
		return Game.instance.g_PlayerData[playerId].CalculatePlayerSigningFee(toClubId);
	}

	public static float CalculatePlayerCompensationFee(int playerId)
	{
		return Game.instance.g_PlayerData[playerId].CalculatePlayerCompensationFee();
	}

	public static DateTime ConvertContractYearsToDate(int numYears, int leagueId = -1)
	{
		if (leagueId == -1)
		{
			short currentManagedClubId = Game.instance.CurrentManagedClubId;
			leagueId = Game.instance.g_ClubData[currentManagedClubId].LeagueID;
		}
		DateTime nextContractExpiryDate = GetNextContractExpiryDate(leagueId);
		return new DateTime(nextContractExpiryDate.Year + (numYears - 1), nextContractExpiryDate.Month, nextContractExpiryDate.Day);
	}

	public static DateTime GetNextContractExpiryWarningDate()
	{
		if (Game.instance.CurrentDate.Date > cachedNextContractExpiryWarningDate)
		{
			DateTime nextContractExpiryDate = GetNextContractExpiryDate();
			if (Game.instance.CurrentDate < nextContractExpiryDate.AddMonths(-6))
			{
				cachedNextContractExpiryWarningDate = nextContractExpiryDate.AddMonths(-6);
			}
			else if (Game.instance.CurrentDate < nextContractExpiryDate.AddMonths(-1))
			{
				cachedNextContractExpiryWarningDate = nextContractExpiryDate.AddMonths(-1);
			}
			else if (Game.instance.CurrentDate < nextContractExpiryDate.AddDays(-7.0))
			{
				cachedNextContractExpiryWarningDate = nextContractExpiryDate.AddDays(-7.0);
			}
		}
		return cachedNextContractExpiryWarningDate;
	}

	public static int GenerateInitialPlayerContractWage(PlayerData playerData, PlayerContractHelper helper, int contractLength)
	{
		return Helpers.RoundMonetaryValue(GenerateNewContractWages(GetPlayerBasicValue(playerData.Rating, playerData.InDecline, playerData.Potential), GetPlayerSquadRoleMultiplier(playerData.Rating, (int)helper.teamAverageRating), helper.teamRatingMultiplier, helper.leagueRatingMultiplier, GetContractLengthMultiplier(contractLength)));
	}

	public static void GetPlayerInfoString(int playerID, short clubID, GameObject playerClone)
	{
		int competitionType = -1;
		int competitionID = -1;
		NextFixtureInfo nextFixture = Game.instance.GetNextFixture(clubID);
		if (nextFixture != null)
		{
			competitionType = nextFixture.fixture.competitiontype;
			competitionID = nextFixture.fixture.competitionid;
		}
		playerClone.transform.Find("PlayerContent/PlayerInfoBox").GetComponent<Image>().color = Helpers.HexToColour("FFFFFF00");
		if ((from x in Transfers.instance.bidManager.getUnprocessedBidForPlayer(playerID)
			where x.amount != -5
			select x).Count() > 0)
		{
			playerClone.transform.Find("PlayerContent/PlayerInfoBox").GetComponent<Image>().color = Helpers.HexToColour("048704FF");
			playerClone.transform.Find("PlayerContent/PlayerInfoBox/PlayerInfo").GetComponent<Text>().text = Translation.GetText("playerinfo-transfer-bid-acronym");
		}
		else if (PlayerData.IsLoanedToClub(playerID))
		{
			playerClone.transform.Find("PlayerContent/PlayerInfoBox").GetComponent<Image>().color = Helpers.HexToColour("048704FF");
			playerClone.transform.Find("PlayerContent/PlayerInfoBox/PlayerInfo").GetComponent<Text>().text = Translation.GetText("playerinfo-on-loan-acronym");
		}
		else if (Transfers.instance.bidManager.loanManager.getUnprocessedLoanBidCountForPlayer(playerID) > 0)
		{
			playerClone.transform.Find("PlayerContent/PlayerInfoBox").GetComponent<Image>().color = Helpers.HexToColour("048704FF");
			playerClone.transform.Find("PlayerContent/PlayerInfoBox/PlayerInfo").GetComponent<Text>().text = Translation.GetText("playerinfo-loan-bid-acronym");
		}
		else if (IsInjured(playerID))
		{
			playerClone.transform.Find("PlayerContent/PlayerInfoBox").GetComponent<Image>().color = Helpers.HexToColour("FF0000FF");
			playerClone.transform.Find("PlayerContent/PlayerInfoBox/PlayerInfo").GetComponent<Text>().text = Translation.GetText("playerinfo-injured-acronym");
		}
		else if (IsSuspended(playerID, GetAssociationID(playerID), competitionType, competitionID))
		{
			playerClone.transform.Find("PlayerContent/PlayerInfoBox").GetComponent<Image>().color = Helpers.HexToColour("FF0000FF");
			playerClone.transform.Find("PlayerContent/PlayerInfoBox/PlayerInfo").GetComponent<Text>().text = Translation.GetText("playerinfo-suspended-acronym");
		}
		else if (IsOnTransferList(playerID))
		{
			if (clubID == Game.instance.CurrentManagedClubId)
			{
				playerClone.transform.Find("PlayerContent/PlayerInfoBox").GetComponent<Image>().color = Helpers.HexToColour("5271E3FF");
				playerClone.transform.Find("PlayerContent/PlayerInfoBox/PlayerInfo").GetComponent<Text>().text = Translation.GetText("playerinfo-transfer-listed-acronym");
			}
			else if (GetRating(playerID) < 90)
			{
				playerClone.transform.Find("PlayerContent/PlayerInfoBox").GetComponent<Image>().color = Helpers.HexToColour("5271E3FF");
				playerClone.transform.Find("PlayerContent/PlayerInfoBox/PlayerInfo").GetComponent<Text>().text = Translation.GetText("playerinfo-transfer-listed-acronym");
			}
		}
		else if (IsOnLoanList(playerID))
		{
			playerClone.transform.Find("PlayerContent/PlayerInfoBox").GetComponent<Image>().color = Helpers.HexToColour("5271E3FF");
			playerClone.transform.Find("PlayerContent/PlayerInfoBox/PlayerInfo").GetComponent<Text>().text = Translation.GetText("playerinfo-loan-listed-acronym");
		}
		else if (PlayerData.HasConcern(playerID) && GetMorale(playerID) < 4)
		{
			playerClone.transform.Find("PlayerContent/PlayerInfoBox").GetComponent<Image>().color = Helpers.HexToColour("FF0000FF");
			playerClone.transform.Find("PlayerContent/PlayerInfoBox/PlayerInfo").GetComponent<Text>().text = Translation.GetText("playerinfo-unhappy-acronym");
		}
	}

	public static string GetPlayerInfoString(int playerID, short clubID)
	{
		int competitionType = -1;
		int competitionID = -1;
		NextFixtureInfo nextFixture = Game.instance.GetNextFixture(clubID);
		if (nextFixture != null)
		{
			competitionType = nextFixture.fixture.competitiontype;
			competitionID = nextFixture.fixture.competitionid;
		}
		if ((from x in Transfers.instance.bidManager.getUnprocessedBidForPlayer(playerID)
			where x.amount != -5
			select x).Count() > 0)
		{
			return Translation.GetText("playerinfo-transfer-bid-acronym");
		}
		if (PlayerData.IsLoanedToClub(playerID))
		{
			return Translation.GetText("playerinfo-on-loan-acronym");
		}
		if (Transfers.instance.bidManager.loanManager.getUnprocessedLoanBidCountForPlayer(playerID) > 0)
		{
			return Translation.GetText("playerinfo-loan-bid-acronym");
		}
		if (IsInjured(playerID))
		{
			return Translation.GetText("playerinfo-injured-acronym");
		}
		if (IsSuspended(playerID, GetAssociationID(playerID), competitionType, competitionID))
		{
			return Translation.GetText("playerinfo-suspended-acronym");
		}
		if (IsOnTransferList(playerID))
		{
			if (clubID == Game.instance.CurrentManagedClubId)
			{
				return Translation.GetText("playerinfo-transfer-listed-acronym");
			}
			if (GetRating(playerID) < 90)
			{
				return Translation.GetText("playerinfo-transfer-listed-acronym");
			}
		}
		else
		{
			if (IsOnLoanList(playerID))
			{
				return Translation.GetText("playerinfo-loan-listed-acronym");
			}
			if (PlayerData.HasConcern(playerID) && GetMorale(playerID) < 4)
			{
				return Translation.GetText("playerinfo-unhappy-acronym");
			}
		}
		return "";
	}

	public static Color GetPositionColour(int playerID)
	{
		PlayerData playerData = Game.instance.g_PlayerData[playerID];
		if (playerData.PreferredPosition == 0)
		{
			return Helpers.GetPositionColour(playerData.Position);
		}
		return Helpers.GetPositionColour(playerData.PreferredPosition);
	}

	public static double GetReleaseCompensationAmount(int playerId)
	{
		return Math.Ceiling((double)(GetContractExpiryDate(playerId) - Game.instance.CurrentDate).Days / 7.0) * (double)GetWages(playerId) / 2.0;
	}

	public static void SignPlayer(int playerID, short clubID, bool discount = false)
	{
		int num = GetCreditPlayerValue(GetValueForShop(playerID));
		if (Game.instance.g_ClubPlayer[Game.GetCurrentUserClubID()].Count == 150)
		{
			Helpers.ShowToast(Translation.GetText("squad-cap-reached"));
			return;
		}
		string value = "signplayer";
		if (discount)
		{
			num = num / 4 * 3;
			value = "playerspotlight";
		}
		if (num < 18)
		{
			num = 18;
		}
		if (CustomerDataController.Instance.GetUserCurCreditAmount() >= num)
		{
			signPlayerType = value;
			signPlayerCost = num;
			signPlayerPlayerID = playerID;
			signPlayerClubID = clubID;
			UI.GetInstance().ShowGenericLoading();
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			dictionary.Add("productname", value);
			dictionary.Add("playerid", playerID.ToString());
			CustomerDataController.Instance.ConsumeItem(0, num, SignPlayerCallback, dictionary);
		}
		else
		{
			ShowNotEnoughCreditsModal();
		}
	}

	public static void SignPlayerCallback(UnityWebRequest response)
	{
		UI.GetInstance().HideGenericLoading();
		JObject jObject = JObject.Parse(response.downloadHandler.text);
		JObject jObject2 = JObject.Parse(jObject["callback"].ToString());
		if (CustomerDataController.Instance.JsonDataGetSuccess(jObject))
		{
			int num = (int)jObject["updatedvalues"]["Credits"];
			int num2 = (int)jObject2["playerid"];
			int creditsSpent = CustomerDataController.Instance.GetUserCurCreditAmount() - num;
			short clubID = GetClubID(num2);
			string? itemName = (string?)jObject2["productname"];
			CustomerDataController.Instance.SetCreditAmount(num);
			EventLoggingController.LogInGamePurchaseEvent(itemName, creditsSpent);
			short currentUserClubID = Game.GetCurrentUserClubID();
			int num3 = 5;
			TransferPlayer.transferPlayer(num2, clubID, currentUserClubID, 0, GenerateNewContractWages(num2, currentUserClubID, num3), ConvertContractYearsToDate(num3, Game.instance.g_ClubData[currentUserClubID].LeagueID), PlayerSquad.SquadStatusManager.GetExpectedSquadStatusWithinClubForNewPlayer(clubID, num2));
			GetStatus(num2).IsInstantPurchase = true;
			Transfers.instance.bidManager.removeAllBidsForPlayer(num2);
			RemoveFromShortList(num2);
			Helpers.ShowToast(Translation.GetText("cheat-sign-player-message-title"));
			ModalController.Instance().CloseModal();
			UI.GetInstance().ChangeScreen(4);
			Save.SaveData(Save.currentSlot);
		}
		else
		{
			ShowNotEnoughCreditsModal();
		}
	}

	protected static void ShowNotEnoughCreditsModal()
	{
		ModalController.Instance().ShowConfirmModal(Translation.GetText("not-enough-credits"), Translation.GetText("not-enough-credits"), () =>
		{
			UI.GetInstance().ChangeScreen(8, ShopScreen.ScreenTypes.Credits);
			ModalController.Instance().CloseAllModals();
		}, Translation.GetText("go-to-shop"));
	}

	public static int GetCreditPlayerValue(float passedValue)
	{
		return GetNewCreditPlayerValue(passedValue);
	}

	public static int GetNewCreditPlayerValue(float passedValue)
	{
		float num = ((float)(int)Club.GetAverageRating(Game.instance.CurrentManagedClubId) + 1f) / 100f;
		float num2 = passedValue / (5000000f * (float)Math.Pow(num, 3.0) / 100f);
		if (num2 < 25f)
		{
			num2 = 25f;
		}
		return (int)num2;
	}

	public static int GetNewCreditPlayerValueSpotlight(float passedValue)
	{
		float num = ((float)(int)Club.GetAverageRating(Game.instance.CurrentManagedClubId) + 1f) / 100f;
		float num2 = passedValue / (5000000f * (float)Math.Pow(num, 3.0) / 100f) * 0.7f;
		if (num2 < 25f)
		{
			num2 = 25f;
		}
		return (int)num2;
	}

	public static float GetValueForShop(int playerID)
	{
		return GetValue(playerID);
	}

	public static string GetMoraleName(byte morale)
	{
		if (morale <= 2)
		{
			return Translation.GetText("verypoor");
		}
		if (morale > 2 && morale <= 4)
		{
			return Translation.GetText("poor");
		}
		if (morale > 4 && morale <= 6)
		{
			return Translation.GetText("okay");
		}
		if (morale > 6 && morale <= 8)
		{
			return Translation.GetText("good");
		}
		return Translation.GetText("verygood");
	}

	public static void MoveToFirstTeamNewPlayer(int playerID)
	{
		if (GetStatus(playerID).firstTeam)
		{
			Helpers.ShowToast(Translation.GetText("sent-to-firstteam"));
			return;
		}
		if (SetFirstTeam(playerID))
		{
			Helpers.ShowToast(Translation.GetText("sent-to-firstteam"));
			ModalController.Instance().CloseAllModals();
			return;
		}
		Helpers.ShowToast(Translation.TemplateReplace(Translation.GetText("first-team-max"), new Dictionary<string, string> { 
		{
			"Num Players",
			((byte)25).ToString()
		} }));
		short clubID = GetClubID(playerID);
		ModalController.Instance().ShowSquadModal(clubID, playerID);
	}

	public static bool MoveToReserveTeamNewPlayer(int playerID, bool showToast = true, int replacementID = 0)
	{
		bool flag = false;
		if (!GetStatus(playerID).firstTeam)
		{
			Helpers.ShowToast(Translation.GetText("sent-to-reserveteam"));
			return true;
		}
		bool flag2 = SetReserveTeam(playerID, replacementID);
		if (flag2)
		{
			if (showToast)
			{
				Helpers.ShowToast(Translation.GetText("sent-to-reserveteam"));
			}
			PlayerData.IsGoalKeeper(playerID);
			int[] lineUp = Game.instance.CurrentManagedClub.currentTactics.lineUp;
			for (int i = 0; i < 18; i++)
			{
				if (lineUp[i] == playerID)
				{
					flag = true;
				}
			}
			if (flag)
			{
				Game.instance.CurrentManagedClub.SwapDepartingPlayerFromLineup(playerID, createMsg: false, replacementID);
			}
			ModalController.Instance().CloseAllModals();
		}
		return flag2;
	}

	public static bool SetReserveTeam(int playerID, int replacementID = 0)
	{
		short currentUserClubID = Game.GetCurrentUserClubID();
		if (GetClubID(playerID) == currentUserClubID)
		{
			if (Club.CheckMinFirstTeam(currentUserClubID))
			{
				if (Game.GetCurrentUserClub(Game.instance.g_currUser).currentTactics.lineUp.Contains(playerID))
				{
					if (PlayerData.IsGoalKeeper(playerID))
					{
						if (replacementID > 0 && PlayerData.IsGoalKeeper(replacementID))
						{
							GetStatus(playerID).firstTeam = false;
							GetStatus(replacementID).firstTeam = true;
							return true;
						}
						Helpers.ShowToast(Translation.GetText("unable-to-move-player"));
						return false;
					}
					GetStatus(playerID).firstTeam = false;
				}
				else
				{
					GetStatus(playerID).firstTeam = false;
				}
				return true;
			}
			Helpers.ShowToast(Translation.TemplateReplace(Translation.GetText("first-team-min"), new Dictionary<string, string> { 
			{
				"Num Players",
				((byte)20).ToString()
			} }));
			return false;
		}
		return false;
	}

	private static bool CanMoveChinesePlayerToReserveTeam(int movedPlayerID)
	{
		short currentManagedClubId = Game.instance.CurrentManagedClubId;
		PlayerSquad players = Club.GetPlayers(currentManagedClubId);
		NextFixtureInfo nextFixture = Game.instance.GetNextFixture(currentManagedClubId);
		if (PlayerData.GetPosition(movedPlayerID) == 1 || PlayerData.GetCountryDataID(movedPlayerID) != 42 || IsInjured(movedPlayerID) || (nextFixture != null && IsSuspended(movedPlayerID, GetAssociationID(movedPlayerID), nextFixture.fixture.competitiontype, nextFixture.fixture.competitionid)))
		{
			return true;
		}
		int num = 15;
		int num2 = 0;
		for (int i = 0; i < players.Count; i++)
		{
			int playerID = players[i];
			bool firstTeam = GetFirstTeam(playerID);
			if (PlayerData.GetPosition(playerID) != 1 && !IsInjured(playerID) && (nextFixture == null || !IsSuspended(playerID, GetAssociationID(playerID), nextFixture.fixture.competitiontype, nextFixture.fixture.competitionid)) && PlayerData.GetCountryDataID(playerID) == 42 && firstTeam)
			{
				num2++;
			}
		}
		return num2 > num;
	}

	public static void ReleasePlayer(int playerID)
	{
		if (PlayerData.IsLoanedToClub(playerID))
		{
			Helpers.ShowToast(Translation.GetText("chairman-blocks-release"));
			return;
		}
		PlayerSquad players = Club.GetPlayers(Game.instance.CurrentManagedClubId);
		int num = 0;
		bool isNewYouthPlayer = YouthAcademy.IsPlayerInYouthAcademy(playerID);
		if (GetPosition(playerID) == 1)
		{
			for (int i = 0; i < players.Count; i++)
			{
				if (GetPosition(players[i]) == 1)
				{
					num++;
				}
			}
		}
		else
		{
			num = 10;
		}
		if (players.Count <= 22 || num < 3)
		{
			Helpers.ShowToast(Translation.GetText("chairman-blocks-release"));
			return;
		}
		string text = Translation.TemplateReplace(Translation.GetText("msg-player-release-text"), new Dictionary<string, string>
		{
			{
				"Player Name",
				GetSurname(playerID)
			},
			{
				"player full name",
				GetSurname(playerID)
			}
		});
		text += "\n\n";
		double balance = Game.instance.CurrentManagedClub.Balance;
		double releaseCompensationAmount = GetReleaseCompensationAmount(playerID);
		bool enoughFunds = releaseCompensationAmount <= balance;
		if (!isNewYouthPlayer)
		{
			text += Translation.TemplateReplace(Translation.GetText("transfer-release-are-you-sure-compo"), new Dictionary<string, string> { 
			{
				"Amount",
				(enoughFunds ? "<color=red>" : "<color=red>") + Helpers.GetLongValueText(GetReleaseCompensationAmount(playerID)) + "</color>"
			} });
		}
		ModalController.Instance().ShowConfirmModal(Translation.GetText("msg-player-release-title"), text, () =>
		{
			if (enoughFunds | isNewYouthPlayer)
			{
				ReleasePlayerConfirmed(playerID);
			}
			else
			{
				Helpers.ShowToast(Translation.GetText("not-enough-transfer-funds"));
			}
		});
		EventLoggingController.LogEvent("playerSold_released");
	}

	private static void ReleasePlayerConfirmed(int playerID)
	{
		string text = Translation.TemplateReplace(Translation.GetText("msg-player-released-title"), new Dictionary<string, string>
		{
			{
				"Player Name",
				GetSurname(playerID)
			},
			{
				"Club Name",
				Club.GetName(Game.GetCurrentUserClubID())
			}
		});
		GetFirstTeam(playerID);
		if (Transfers.instance.CanReleasePlayer(playerID))
		{
			ManagedClub currentManagedClub = Game.instance.CurrentManagedClub;
			double num = GetReleaseCompensationAmount(playerID);
			if (YouthAcademy.IsPlayerInYouthAcademy(playerID))
			{
				num = 0.0;
			}
			currentManagedClub.BalanceSheet[currentManagedClub.CurrentBalanceSheet].TransfersOut += num;
			currentManagedClub.Balance -= num;
			Club.SetSquadNoAvailablity(currentManagedClub.ClubID, PlayerData.GetSquadNo(playerID), available: true);
			PlayerData.SetSquadNo(playerID, 0);
			Helpers.ShowToast(text);
			ModalController.Instance().CloseAllModals();
			Game.instance.CurrentManagedClub.SwapDepartingPlayerFromLineup(playerID, createMsg: false);
			if (UI.GetInstance().g_currentscreen == 14)
			{
				TacticsController.GetInstance().tacticsScreen.UpdateTacticsScreen();
			}
			if (UI.GetInstance().g_currentscreen == 9)
			{
				UI.GetInstance().CheckForAndCreateScreenIfNeeded(9);
				UI.GetInstance().squadScreen.Refresh();
			}
			EventLoggingController.LogEvent("playersold_released");
		}
		else
		{
			Helpers.ShowToast(Translation.GetText("chairman-blocks-release"));
		}
	}
}
