using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ManagedClub
{
	private const int PROFIT_TAX_RATE = 10;

	private const double INCOME_TAX_CAP = 1500000.0;

	private const int INCOME_TAX_RATE = 10;

	public short ClubID;

	public short AssociationID;

	public double StartBalance;

	public double Balance;

	public Dictionary<int, double> BalanceLog = new Dictionary<int, double>();

	public double WageBudget;

	public double TransferBudget;

	public List<BalanceSheet> BalanceSheet = new List<BalanceSheet>();

	public int CurrentBalanceSheet;

	public int TicketPrice;

	public int TvMoney;

	public int SponsorMoney;

	public int Attendance;

	public int StartWages;

	public float StartRating;

	public float StartPotential;

	public byte expectedLeaguePos;

	public byte expectedCupFinal;

	public LeagueEconomy LeagueEconomy;

	public List<Sponsor> AvailableSponsors = new List<Sponsor>();

	public Sponsor CurrentSponsor = new Sponsor();

	public DateTime DateSponsorTaken;

	public DateTime NextSponsorPayment;

	public Dictionary<int, int> ContinentalCupPrizeMoney = new Dictionary<int, int>();

	public Dictionary<int, int> DomesticCupPrizeMoney = new Dictionary<int, int>();

	public bool sacked;

	public bool ScoutKeepersWarned;

	public bool ScoutDefendersWarned;

	public bool ScoutMidfieldersWarned;

	public bool ScoutForwardsWarned;

	public int LatestCupRoundDrawn;

	public DateTime LatestFirstFixtureDate;

	public DateTime LatestSeasonStartDate;

	public DateTime LatestFixturePlayedDate;

	public cTactics currentTactics = new cTactics(cleanCopy: true);

	public Dictionary<int, PlayerStatusInfo> PlayerStatus;

	public int TacticsSquadView;

	public Dictionary<int, int> TrainingSession = new Dictionary<int, int>();

	public Dictionary<int, PlayerShortlist> PlayerShortlist = new Dictionary<int, PlayerShortlist>();

	public int MatchdayType = 1;

	public bool AssociationUpToDate;

	public int FacilityScore;

	public int MessageCounter;

	public List<Message> messages = new List<Message>();

	public List<Message> newsMessages = new List<Message>();

	public int SponsorSeasonID;

	public static void Init(ManagedClub managedClub, short clubID, PlayerSquad playerSquad = null, Team tm = null, bool takeover = false)
	{
		if (playerSquad == null)
		{
			playerSquad = PlayerSquad.GetPlayers(clubID);
		}
		if (tm == null)
		{
			tm = Game.instance.g_tm[0];
		}
		PlayerPrefs.DeleteKey("spotlightseed");
		PlayerPrefs.DeleteKey("spotlightseedYouth");
		if (clubID == 0)
		{
			managedClub.ClubID = 0;
			managedClub.AssociationID = 2;
			managedClub.Balance = 0.0;
			managedClub.StartBalance = 0.0;
			managedClub.PlayerStatus = new Dictionary<int, PlayerStatusInfo>();
			return;
		}
		managedClub.ClubID = clubID;
		managedClub.AssociationID = Club.GetAssociationID(clubID);
		managedClub.PlayerStatus = new Dictionary<int, PlayerStatusInfo>();
		for (int i = 0; i < playerSquad.Count; i++)
		{
			PlayerStatusInfo.Init(managedClub.PlayerStatus, playerSquad[i], (byte)(i + 1), isTransfer: false);
			if (UserManagerProfileController.UserHasTrait(ManagerTraitArea.PlayerWages))
			{
				PlayerData playerData = Game.instance.g_PlayerData[playerSquad[i]];
				_ = playerData.ContractWage;
				playerData.ContractWage = (int)UserManagerProfileController.ApplyTrait(ManagerTraitArea.PlayerWages, playerData.ContractWage);
				_ = playerData.ContractWage;
			}
		}
		SortableClubPlayerList playersAsSortableListStatic = playerSquad.GetPlayersAsSortableListStatic();
		playersAsSortableListStatic.Sort(CustomSort.SortByRatingDesc);
		for (int j = 0; j < playersAsSortableListStatic.Count; j++)
		{
			if (j < 25)
			{
				managedClub.PlayerStatus[playersAsSortableListStatic[j]].firstTeam = true;
			}
		}
		managedClub.currentTactics.inst.formationid = AssignValidFormation(Game.instance.g_ClubData[clubID].TeamFormation);
		PlayerSquad.SquadStatusManager.RefreshClub(clubID);
		Game.instance.SquadHealthCheck();
		tm.clubid = clubID;
		tm.aim = 1;
		Team.PickTeam(tm, managedClub.currentTactics.inst.formationid, 0, -1, -1, justFirstTeam: true);
		Team.PickTactics(tm, 0, quickload: false, 0);
		for (int k = 0; k < 18; k++)
		{
			managedClub.currentTactics.lineUp[k] = tm.lineup[k];
		}
		for (int l = 0; l < 11; l++)
		{
			managedClub.currentTactics.inst.plyrinst[l] = 0;
			managedClub.currentTactics.inst.playerRoles[l] = new SeFixtureInfo.TacticsRole(0);
		}
		cTactics cTactics2 = managedClub.currentTactics;
		cTactics2.inst.tacklingstyle.value = tm.tacklingstyle;
		cTactics2.inst.mentality.value = tm.mentality;
		cTactics2.inst.passingstyleid.value = tm.passingstyleid;
		cTactics2.inst.attackingstyle.value = tm.attackingstyle;
		cTactics2.inst.tempoid.value = tm.tempoid;
		cTactics2.inst.pressingid.value = tm.pressingid;
		cTactics2.inst.counterattack = tm.counterattack;
		cTactics2.inst.menbehindball = tm.menbehindball;
		cTactics2.inst.tightmarking = tm.tightmarking;
		cTactics2.inst.playoffside = tm.playoffside;
		cTactics2.inst.useplaymaker = tm.useplaymaker;
		cTactics2.inst.usetargetman = tm.usetargetman;
		cTactics2.inst.captain.value = tm.captain;
		cTactics2.inst.penaltytaker.value = tm.penaltytaker;
		cTactics2.inst.deadballtakerleft.value = tm.deadballtakerleft;
		cTactics2.inst.deadballtakerright.value = tm.deadballtakerleft;
		cTactics2.inst.cornertakerleft.value = tm.cornertakerleft;
		cTactics2.inst.cornertakerright.value = tm.cornertakerleft;
		cTactics2.inst.captainid = managedClub.currentTactics.lineUp[managedClub.currentTactics.inst.captain.value];
		cTactics2.inst.penaltytakerid = cTactics2.lineUp[cTactics2.inst.penaltytaker.value];
		cTactics2.inst.deadballtakerleftid = cTactics2.lineUp[cTactics2.inst.deadballtakerleft.value];
		cTactics2.inst.deadballtakerrightid = cTactics2.lineUp[cTactics2.inst.deadballtakerright.value];
		cTactics2.inst.cornertakerleftid = cTactics2.lineUp[cTactics2.inst.cornertakerleft.value];
		cTactics2.inst.cornertakerrightid = cTactics2.lineUp[cTactics2.inst.cornertakerright.value];
		cTactics2.inst.width.value = tm.width;
		cTactics2.inst.creativity.value = tm.creativity;
		cTactics2.inst.backline.value = tm.backline;
		cTactics2.inst.attforwards.value = tm.attforwards;
		cTactics2.inst.attmidfielders.value = tm.attmidfielders;
		cTactics2.inst.attwingers.value = tm.attwingers;
		managedClub.BalanceSheet.Clear();
		managedClub.BalanceSheet.Add(new BalanceSheet());
		managedClub.CurrentBalanceSheet = 0;
		managedClub.BalanceSheet[0].Date = Game.instance.CurrentDate;
		managedClub.BalanceSheet[0].SeasonTicketReceipts = Game.GetCurrentUserClub(Game.instance.g_currUser).BalanceSheet[0].SeasonTicketReceipts;
		managedClub.StartWages = Club.GetWages(clubID);
		managedClub.BalanceLog.Clear();
		managedClub.LeagueEconomy = new LeagueEconomy(clubID);
		managedClub.TicketPrice = Economy.CalcClubTicketPrice(clubID);
		managedClub.Attendance = Economy.CalcClubAttendance(clubID);
		managedClub.TvMoney = Economy.CalcWeeklyTvMoney(clubID);
		managedClub.StartBalance = Economy.CalcStartBalance(clubID);
		managedClub.Balance = managedClub.StartBalance;
		Club.GetLeagueDataId(clubID);
		managedClub.expectedLeaguePos = (byte)ClubAmbition.GetExpectedLeaguePosition(clubID);
		Game.instance.g_matchday_nextfix = null;
		Game.instance.g_matchday_flowpos = 0;
		if (takeover)
		{
			CreatePlayerContractWarnings();
			if (managedClub.TvMoney >= 10000)
			{
				Helpers.toNearest(managedClub.TvMoney, 10000.0);
			}
			else
			{
				Helpers.toNearest(managedClub.TvMoney, 1000.0);
			}
			MessageManager.NewAppointment(managedClub.ClubID, (int)managedClub.Balance);
			List<Transfer> list = Transfers.instance.historyManager.LoanedOutByClub(managedClub.ClubID);
			if (Enumerable.Count(list) > 0)
			{
				MessageManager.LoanedOutPlayers(list);
			}
			managedClub.Balance = Math.Max(managedClub.Balance, 0.0);
		}
	}

	public static void NewSeason(ManagedClub managedClub)
	{
		short clubID = managedClub.ClubID;
		bool flag = false;
		if (clubID <= 0)
		{
			return;
		}
		CreatePlayerContractWarnings();
		short associationID = Club.GetAssociationID(clubID);
		managedClub.StartRating = Club.GetAverageRatingPrecise(clubID);
		managedClub.StartPotential = (int)Club.GetAveragePotenial(clubID);
		managedClub.LeagueEconomy = new LeagueEconomy(clubID);
		Club.GetLeagueDataId(clubID);
		managedClub.expectedLeaguePos = (byte)ClubAmbition.GetExpectedLeaguePosition(clubID);
		managedClub.TicketPrice = Economy.CalcClubTicketPrice(clubID);
		managedClub.Attendance = Economy.CalcClubAttendance(clubID);
		managedClub.TvMoney = Economy.CalcWeeklyTvMoney(clubID);
		double num = 0.0;
		double num2 = 0.0;
		foreach (BalanceSheet item in managedClub.BalanceSheet)
		{
			num += item.GateReceipts + item.Merchandise + item.OtherIncome + item.PrizeMoney + item.SeasonTicketReceipts + item.Sponsor + item.TransfersIn + item.TVRevenue;
			num2 += item.Administration + item.GroundMaintenance + item.OtherOutgoings + item.Overheads + item.PlayerWages + item.StaffWages + item.TransfersOut + item.TrainingGround + item.YouthAcademy;
		}
		if (num > num2)
		{
			managedClub.Balance -= (num - num2) / 10.0;
		}
		managedClub.StartBalance = managedClub.Balance;
		if (managedClub.CurrentBalanceSheet == 0)
		{
			flag = true;
		}
		Game.instance.jobApplicationManager.midSeasonTakeover = false;
		managedClub.CurrentBalanceSheet = 0;
		managedClub.BalanceSheet = new List<BalanceSheet>();
		managedClub.BalanceSheet.Add(new BalanceSheet());
		managedClub.BalanceLog = new Dictionary<int, double>();
		managedClub.BalanceSheet[managedClub.CurrentBalanceSheet].SeasonTicketReceipts = Economy.CalcSeasonTicketSales(clubID);
		managedClub.Balance += managedClub.BalanceSheet[managedClub.CurrentBalanceSheet].SeasonTicketReceipts;
		managedClub.BalanceSheet[managedClub.CurrentBalanceSheet].Date = Game.instance.CurrentDate;
		if (!flag)
		{
			managedClub.BalanceSheet[managedClub.CurrentBalanceSheet].CashInjection = Economy.CalcChairmanInjection(clubID) / 4;
			managedClub.Balance += managedClub.BalanceSheet[managedClub.CurrentBalanceSheet].CashInjection;
			MessageManager.ChairmanInvests(clubID, managedClub.BalanceSheet[managedClub.CurrentBalanceSheet].CashInjection);
		}
		if (!Game.instance.g_association.ContainsKey(associationID))
		{
			return;
		}
		List<Cup> cups = Game.instance.g_season.Cups;
		int count = cups.Count;
		for (int i = 0; i < count; i++)
		{
			Cup cup = cups[i];
			if (cup == null || cup.AssocID != associationID || cup.InCup.IndexOf(clubID) == -1)
			{
				continue;
			}
			List<Fixture> fixture = cup.Turn[cup.CurrentRound].Fixture;
			bool flag2 = false;
			int count2 = fixture.Count;
			for (int j = 0; j < count2; j++)
			{
				int num3 = -1;
				if (fixture[j].homeClubId == clubID)
				{
					num3 = 0;
				}
				else if (fixture[j].awayClubId == clubID)
				{
					num3 = 1;
				}
				if (num3 != -1)
				{
					flag2 = true;
					RandomEvent.SendMediaQuestion(RandomEvent.QuestionType.Cup, fixture[j], associationID, cup.CurrentRound + 1, clubID, (num3 == 0) ? fixture[j].awayClubId : fixture[j].homeClubId);
					MessageManager.CupDrawn(fixture[j], cup);
					break;
				}
			}
			if (!flag2)
			{
				MessageManager.CupByeRecieved(cup);
			}
		}
	}

	private static byte AssignValidFormation(byte formationID)
	{
		switch (formationID)
		{
		case 3:
		case 5:
		case 8:
			return 23;
		case 7:
			return 4;
		case 9:
			return 15;
		case 10:
		case 13:
		case 21:
			return 1;
		case 11:
			return 2;
		case 14:
			return 0;
		case 17:
			return 6;
		case 18:
			return 17;
		case 22:
			return 17;
		case 24:
			return 23;
		case 25:
			return 6;
		case 26:
			return 17;
		default:
			return 0;
		}
	}

	internal void WeeklyBalanceSheetUpdate()
	{
		int wages = Club.GetWages(ClubID);
		double num = (double)(Game.instance.Random.GetRand() % 3) * 0.01;
		BalanceSheet balanceSheet = new BalanceSheet
		{
			Date = Game.instance.CurrentDate,
			PlayerWages = wages,
			StaffWages = wages / 3,
			TrainingGround = (double)wages * (0.01 + num),
			YouthAcademy = (double)wages * (0.02 + num),
			Administration = (double)wages * (0.005 + num),
			Overheads = (double)wages * (0.003 + num)
		};
		BalanceSheet.Add(balanceSheet);
		Balance -= (double)wages + balanceSheet.StaffWages + balanceSheet.TrainingGround + balanceSheet.YouthAcademy + balanceSheet.Administration + balanceSheet.Overheads;
		UpdateRevenueAmounts();
		num = -0.02;
		num += (double)(Game.instance.Random.GetRand() % 5) * 0.01;
		float num2 = Economy.CalcClubTicketPricePrecise(ClubID);
		int attendance = Club.GetAttendance(ClubID);
		double num3 = (double)((float)attendance * num2) * (0.15 + num) * (double)(1f + Facilities.GetClubzoneRevenueIncrease(30));
		num3 = num3 / 100.0 * (double)FinanceConfig.MerchSplit;
		double num4 = (double)((float)attendance * num2) * (0.15 + num) * (double)(1f + Facilities.GetClubzoneRevenueIncrease(32));
		num4 = num4 / 100.0 * (double)FinanceConfig.HospitalitySplit;
		if (Game.instance.GetNextFixture(ClubID) == null)
		{
			double num5 = Economy.CalcSeasonTicketSales(ClubID);
			double num6 = TvMoney;
			balanceSheet.SeasonTicketReceipts += num5;
			balanceSheet.TVRevenue += num6;
			balanceSheet.Merchandise += num3;
			balanceSheet.Hospitality += num4;
			Balance += num5 + num6 + num3;
		}
		else
		{
			double num7 = (double)((float)attendance * num2) * (0.06 + num);
			balanceSheet.Merchandise += num3;
			balanceSheet.Hospitality += num4;
			balanceSheet.OtherIncome += num7;
			Balance += num7 + num3;
			if (Game.instance.getIncome() > 1500000.0)
			{
				balanceSheet.Administration += Game.instance.getWeekIncome() / 10.0;
				Balance -= Game.instance.getWeekIncome() / 10.0;
			}
		}
		CurrentBalanceSheet++;
		CheckSponsorContract();
	}

	internal void UpdateContCupBalanceSheet(int prizeMoney, int compid)
	{
		if (ContinentalCupPrizeMoney.ContainsKey(compid))
		{
			ContinentalCupPrizeMoney[compid] += prizeMoney;
		}
		else
		{
			ContinentalCupPrizeMoney.Add(compid, prizeMoney);
		}
	}

	internal void UpdateDomCupBalanceSheet(int prizeMoney, int compid)
	{
		if (DomesticCupPrizeMoney.ContainsKey(compid))
		{
			DomesticCupPrizeMoney[compid] += prizeMoney;
		}
		else
		{
			DomesticCupPrizeMoney.Add(compid, prizeMoney);
		}
	}

	internal void CheckSponsorContract()
	{
		if (Game.instance.CurrentDate >= DateSponsorTaken.AddYears(CurrentSponsor.Seasons))
		{
			CurrentSponsor = new Sponsor();
		}
	}

	internal bool ShouldPaySponsorMoney()
	{
		if (CurrentSponsor.ID != 0)
		{
			return Game.instance.CurrentDate >= NextSponsorPayment;
		}
		return false;
	}

	public void PaySponsorMoney()
	{
		switch (CurrentSponsor.PaymentMethod)
		{
		case Sponsor.PaymentType.ANNUAL:
			BalanceSheet[CurrentBalanceSheet].Sponsor += GetAnnualSponsorMoney();
			Balance += GetAnnualSponsorMoney();
			NextSponsorPayment = Game.instance.CurrentDate.AddYears(1);
			UI.GetInstance().UpdateTopBar();
			break;
		case Sponsor.PaymentType.MONTHLY:
			BalanceSheet[CurrentBalanceSheet].Sponsor += GetMonthlySponsorMoney();
			Balance += GetMonthlySponsorMoney();
			NextSponsorPayment = Game.instance.CurrentDate.AddMonths(1);
			UI.GetInstance().UpdateTopBar();
			break;
		case Sponsor.PaymentType.WEEKLY:
			BalanceSheet[CurrentBalanceSheet].Sponsor += GetWeeklySponsorMoney();
			Balance += GetWeeklySponsorMoney();
			NextSponsorPayment = Game.instance.CurrentDate.AddDays(7.0);
			UI.GetInstance().UpdateTopBar();
			break;
		}
	}

	public int GetAnnualSponsorMoney()
	{
		return CurrentSponsor.GetPerSeasonAmount();
	}

	public int GetMonthlySponsorMoney()
	{
		return CurrentSponsor.GetPerSeasonAmount() / 12;
	}

	public int GetWeeklySponsorMoney()
	{
		return CurrentSponsor.GetPerSeasonAmount() / 52;
	}

	private BalanceSheet currentBalanceSheet()
	{
		if (BalanceSheet.Count >= CurrentBalanceSheet)
		{
			return BalanceSheet[CurrentBalanceSheet];
		}
		return BalanceSheet.Last();
	}

	public void PayPlayerSigninFee(int signingFee)
	{
		currentBalanceSheet().TransfersOut += signingFee;
		Balance -= signingFee;
	}

	public void PayPlayerTransferFee(int Fee)
	{
		currentBalanceSheet().TransfersIn += Fee;
	}

	public void ReceivedPlayerTransferFee(int amount, int signingFee)
	{
		if (amount > 0)
		{
			currentBalanceSheet().TransfersOut += amount;
		}
		currentBalanceSheet().TransfersOut += signingFee;
		Balance -= amount + signingFee;
	}

	public void UpdateBalanceLog()
	{
		int num = DateTime.DaysInMonth(Game.instance.CurrentDate.Year, Game.instance.CurrentDate.Month);
		if (Game.instance.CurrentDate.Day == num && !BalanceLog.ContainsKey(Game.instance.CurrentDate.Month))
		{
			BalanceLog.Add(Game.instance.CurrentDate.Month, Balance);
		}
	}

	public List<double> GetMonthlyIncomeForSeason()
	{
		List<double> list = new List<double>();
		int num = -1;
		int num2 = -1;
		for (int i = 0; i < BalanceSheet.Count; i++)
		{
			if (BalanceSheet[i].Date.Month != num)
			{
				list.Add(0.0);
				num = BalanceSheet[i].Date.Month;
				num2++;
			}
			list[num2] = list[num2] + BalanceSheet[i].CashInjection + BalanceSheet[i].GateReceipts + BalanceSheet[i].Hospitality + BalanceSheet[i].Merchandise + BalanceSheet[i].OtherIncome + BalanceSheet[i].PrizeMoney + BalanceSheet[i].SeasonTicketReceipts + BalanceSheet[i].Sponsor + BalanceSheet[i].TransfersOut + BalanceSheet[i].TVRevenue;
		}
		return list;
	}

	public List<double> GetMonthlyOutgoingsForSeason()
	{
		List<double> list = new List<double>();
		int num = -1;
		int num2 = -1;
		for (int i = 0; i < BalanceSheet.Count; i++)
		{
			if (BalanceSheet[i].Date.Month != num)
			{
				list.Add(0.0);
				num = BalanceSheet[i].Date.Month;
				num2++;
			}
			list[num2] = list[num2] + BalanceSheet[i].Administration + BalanceSheet[i].GroundMaintenance + BalanceSheet[i].OtherOutgoings + BalanceSheet[i].Overheads + BalanceSheet[i].PlayerWages + BalanceSheet[i].StaffWages + BalanceSheet[i].TrainingGround + BalanceSheet[i].TransfersIn + BalanceSheet[i].YouthAcademy;
		}
		return list;
	}

	public void AddTransferInAmount(int amount)
	{
		BalanceSheet[CurrentBalanceSheet].TransfersIn += amount;
		Balance += amount;
	}

	public void DeductSigningFeeAmount(int amount)
	{
		BalanceSheet[CurrentBalanceSheet].TransfersOut += amount;
		Balance -= amount;
	}

	public void SoldPlayer(int playerId, int amount)
	{
		Player.RemoveInvalidPlayersFromLineup(playerId);
		if (amount > 0)
		{
			Game.instance.CurrentManagedClub.AddTransferInAmount(amount);
		}
	}

	public void BoughtPlayer(int playerId, int amount, int signingFee)
	{
		BalanceSheet[CurrentBalanceSheet].TransfersOut += amount;
		BalanceSheet[CurrentBalanceSheet].TransfersOut += signingFee;
		Balance -= amount + signingFee;
	}

	public static void AddPlayer(int playerID, bool isTransfer)
	{
		if (Game.instance.managedClub.Any())
		{
			PlayerStatusInfo.Init(Game.instance.managedClub[Game.instance.g_currUser].PlayerStatus, playerID, 0, isTransfer);
			if (isTransfer)
			{
				Player.SetFirstTeam(playerID);
			}
			Player.RemoveFromShortList(playerID);
		}
	}

	public void CheckPositionWarningFlags(int pos)
	{
		if (ScoutKeepersWarned || ScoutDefendersWarned || ScoutMidfieldersWarned || ScoutForwardsWarned)
		{
			if ((pos & 1) != 0)
			{
				ScoutKeepersWarned = false;
			}
			if ((pos & 0x7E) != 0)
			{
				ScoutDefendersWarned = false;
			}
			if ((pos & 0x1FF0) != 0)
			{
				ScoutMidfieldersWarned = false;
			}
			if ((pos & 0xE000) != 0)
			{
				ScoutForwardsWarned = false;
			}
		}
	}

	internal void UpdateRevenueAmounts()
	{
		TicketPrice = Economy.CalcClubTicketPrice(ClubID);
		TvMoney = Economy.CalcWeeklyTvMoney(ClubID);
	}

	public void ScoutsWarned(int playerId, bool warn)
	{
		int position = Player.GetPosition(playerId);
		if ((position & 1) != 0)
		{
			ScoutKeepersWarned = warn;
		}
		if ((position & 0x7E) != 0)
		{
			ScoutDefendersWarned = warn;
		}
		if ((position & 0x1FF0) != 0)
		{
			ScoutMidfieldersWarned = warn;
		}
		if ((position & 0xE000) != 0)
		{
			ScoutForwardsWarned = warn;
		}
	}

	public void SwapDepartingPlayerFromLineup(int playerID, bool createMsg = true, int GK = 0)
	{
		int num = Array.IndexOf(currentTactics.lineUp, playerID);
		if (num == -1)
		{
			return;
		}
		int num2 = Team.PickBestReplacementPlayer(playerID);
		if (num2 > 0)
		{
			if (createMsg)
			{
				MessageManager.TransferCausedLineupChange(playerID);
			}
			currentTactics.lineUp[num] = num2;
			Player.SetFirstTeam(num2);
			Team.replaceSubstitutePlayer(num2);
		}
		else if (GK != 0)
		{
			num2 = GK;
			currentTactics.lineUp[num] = num2;
			Player.SetFirstTeam(num2);
			Team.replaceSubstitutePlayer(num2);
		}
		else
		{
			int num3 = PlayerRespawn.giveRespawnToManagedClub(ClubID, Player.GetPosition(playerID));
			Team.ReplaceDuplicate(num3);
			currentTactics.lineUp[num] = num3;
			Player.SetFirstTeam(num3);
		}
	}

	public static bool CreatePlayerContractWarnings()
	{
		short currentManagedClubId = Game.instance.CurrentManagedClubId;
		Dictionary<int, PlayerData> g_PlayerData = Game.instance.g_PlayerData;
		DateTime nextContractExpiryDate = Player.GetNextContractExpiryDate();
		_ = (nextContractExpiryDate.Date - Game.instance.CurrentDate.Date).Days;
		List<int> list = PlayerSquad.AllPlayers(currentManagedClubId);
		List<int> list2 = new List<int>();
		for (int i = 0; i < list.Count; i++)
		{
			if (g_PlayerData[list[i]].ContractExpiryDate.Date == nextContractExpiryDate.Date)
			{
				list2.Add(list[i]);
			}
		}
		if (Enumerable.Count(list2) > 0)
		{
			MessageManager.ContractExpiry(list2);
		}
		return false;
	}

	public static List<int> GetUnderPerformingPlayer()
	{
		List<int> list = new List<int>();
		int[] lineUp = Game.instance.CurrentManagedClub.currentTactics.lineUp;
		Dictionary<int, float> underPerformingRatings = new Dictionary<int, float>();
		float num = 0f;
		float num2 = 8f;
		foreach (int num3 in lineUp)
		{
			StatContainer currentSeasonStats = PlayerCareerStats.GetCurrentSeasonStats(num3);
			num = currentSeasonStats.GetAveragePerformance();
			if (currentSeasonStats.GetStat(StatType.App) > 2 && num < num2 && !list.Contains(num3))
			{
				list.Add(num3);
				underPerformingRatings.Add(num3, num);
			}
		}
		list.Sort((int b, int a) => underPerformingRatings[a].CompareTo(underPerformingRatings[b]));
		if (list.Count != 0)
		{
			return list.Take(4).ToList();
		}
		return null;
	}

	public static int GetKeyPlayer()
	{
		int[] lineUp = Game.instance.CurrentManagedClub.currentTactics.lineUp;
		byte b = 0;
		int result = 0;
		for (int i = 0; i < 11; i++)
		{
			int num = lineUp[i];
			if (num > 0)
			{
				byte ratingQuick = Player.GetRatingQuick(num);
				if (ratingQuick > b)
				{
					b = ratingQuick;
					result = num;
				}
			}
		}
		return result;
	}

	public static void SendPlayerRetirementWarningMessages()
	{
		short currentManagedClubId = Game.instance.CurrentManagedClubId;
		int num = 0;
		List<int> list = new List<int>();
		PlayerSquad players = Club.GetPlayers(currentManagedClubId);
		for (int i = 0; i < players.Count; i++)
		{
			int num2 = players[i];
			if (Player.GetStatus(num2).retirementWarned)
			{
				num++;
				list.Add(num2);
			}
		}
		int[] array = new int[num];
		int num3 = 0;
		foreach (int item in list)
		{
			array[num3] = item;
			num3++;
		}
		List<int> list2 = new List<int>();
		list2 = array.ToList();
		if (list2.Any())
		{
			MessageManager.PlayerRetiringSoon(list2);
		}
	}

	public static List<int> GetRandomBasedOnLowMorale()
	{
		PlayerSquad players = Club.GetPlayers(Game.instance.CurrentManagedClubId);
		List<int> list = new List<int>();
		Dictionary<int, byte> unhappyPlayersScores = new Dictionary<int, byte>();
		byte b = 0;
		for (int i = 0; i < players.Count; i++)
		{
			b = Player.GetMorale(players[i]);
			if (b < 4)
			{
				list.Add(players[i]);
				unhappyPlayersScores.Add(players[i], b);
			}
		}
		list.Sort((int a, int key) => unhappyPlayersScores[a].CompareTo(unhappyPlayersScores[key]));
		if (list.Any())
		{
			return list.Take(4).ToList();
		}
		return null;
	}

	public static List<int> GetRandomBasedOnHighMorale()
	{
		PlayerSquad players = Club.GetPlayers(Game.instance.CurrentManagedClubId);
		List<int> list = new List<int>();
		Dictionary<int, byte> happyPlayersScores = new Dictionary<int, byte>();
		byte b = 0;
		for (int i = 0; i < players.Count; i++)
		{
			b = Player.GetMorale(players[i]);
			if (b > 7)
			{
				list.Add(players[i]);
				happyPlayersScores.Add(players[i], b);
			}
		}
		list.Sort((int a, int key) => happyPlayersScores[a].CompareTo(happyPlayersScores[key]));
		if (list.Any())
		{
			return list.Take(4).ToList();
		}
		return null;
	}

	private static void respawnPlayer(int playerId)
	{
		if (playerId != 0)
		{
			AddPlayer(playerId, isTransfer: false);
			MessageManager.YouthPromotion(playerId);
			int num = Game.instance.g_PlayerData[playerId].GenerateNewContractLengthForUnmanagedClub();
			short currentUserClubID = Game.GetCurrentUserClubID();
			TransferPlayer.transferPlayer(playerId, 0, currentUserClubID, -3, Player.GenerateNewContractWages(playerId, currentUserClubID, num), Player.ConvertContractYearsToDate(num, Game.instance.g_ClubData[currentUserClubID].LeagueID));
		}
	}
}
