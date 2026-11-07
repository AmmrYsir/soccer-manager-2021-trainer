using System;
using System.Collections.Generic;
using System.Linq;

public class ManagedClubProfile
{
	public string CurrentSeasonID;

	public short CurrentClubID;

	public Dictionary<int, CustomFormation> CustomFormations = new Dictionary<int, CustomFormation>();

	public OverallObjective SeasonObjective;

	public ClubObjectivesList ObjectivesList = new ClubObjectivesList();

	public ClubConfidence CurrentClubConfidence { get; set; } = new ClubConfidence();

	public Dictionary<short, CareerEntry> CareerHistory { get; set; } = new Dictionary<short, CareerEntry>();

	public Dictionary<short, TrophyList> ClubWonTrophies { get; set; } = new Dictionary<short, TrophyList>();

	public int RecordSigningAmount { get; set; }

	public int RecordSigningPlayerID { get; set; }

	public int RecordSigningReceivedAmount { get; set; }

	public int RecordSigningReceivedPlayerID { get; set; }

	public int RecordAttendance { get; set; }

	public int RecordAttendanceClubID { get; set; }

	public short RecordWinOppClub { get; set; }

	public int[] RecordWinScore { get; set; } = new int[2];

	public short RecordLossOppClub { get; set; }

	public int[] RecordLossScore { get; set; } = new int[2];

	public int CurrentUnbeatenStreak { get; set; }

	public int HighestUnbeatenStreak { get; set; }

	public int CurrentWinStreak { get; set; }

	public int HighestWinStreak { get; set; }

	public short GoalsScoredSeason { get; set; }

	public short GoalsConcededSeason { get; set; }

	public short CleanSheetsSeason { get; set; }

	public short AverageAttendanceSeason { get; set; }

	public short HomeMatchesSeason { get; set; }

	public ManagedClubProfile()
	{
	}

	public ManagedClubProfile(string seasonID, short clubID)
	{
		CurrentSeasonID = seasonID;
		CurrentClubID = clubID;
		AddNewCareerEntry();
		SeasonObjective = ClubObjectives.SetSeasonObjective();
		CurrentClubConfidence.BoardConfidenceMonth = Game.instance.CurrentDate.AddMonths(3);
		ObjectivesList.ClubObjectives.AddRange(ClubObjectives.PopulateClubObjectives(SeasonObjective).ClubObjectives);
	}

	public void SetNewClubID(short clubID)
	{
		CurrentClubID = clubID;
	}

	public void SetNewSeasonID(string seasonID)
	{
		CurrentSeasonID = seasonID;
	}

	public void SetBoardConfidenceDate(DateTime date)
	{
		CurrentClubConfidence.BoardConfidenceMonth = date.AddMonths(3);
	}

	public void TakeJobOffer(short newClubID)
	{
		CareerHistory[CurrentClubID].EndDate = Game.instance.CurrentDate;
		CurrentClubID = newClubID;
		AddNewCareerEntry();
		ResetRecords();
		CurrentClubConfidence = new ClubConfidence();
		SeasonObjective = ClubObjectives.SetSeasonObjective();
		ObjectivesList.ClubObjectives.Clear();
		if (Game.instance.CurrentDate.Month == Association.GetTopDivision(Game.GetCurrentUserClubAssociation()).SeasonOneStart)
		{
			ObjectivesList.ClubObjectives.AddRange(ClubObjectives.PopulateClubObjectives(SeasonObjective).ClubObjectives);
		}
		else if (Club.GetNumberOfLeagueGamesPlayed(Game.GetCurrentUserClubID()) < 20)
		{
			ObjectivesList.ClubObjectives.AddRange(ClubObjectives.PopulateCompObjectives(SeasonObjective, midSeason: true).ClubObjectives);
		}
	}

	public void InitNewSeason()
	{
		AddNewSeasonHistory(Game.instance.g_season.SeasonID);
		SetBoardConfidenceDate(Association.GetSeasonStartDate(Club.GetAssociationID(Game.GetCurrentUserClubID()), Game.instance.g_season.SeasonID));
		ResetObjectives();
		ResetSeasonRecords();
	}

	private void ResetRecords()
	{
		RecordSigningAmount = 0;
		RecordSigningPlayerID = 0;
		RecordSigningReceivedAmount = 0;
		RecordSigningReceivedPlayerID = 0;
		RecordAttendance = 0;
		RecordWinOppClub = 0;
		RecordWinScore = new int[2];
		RecordLossOppClub = 0;
		RecordLossScore = new int[2];
		CurrentUnbeatenStreak = 0;
		HighestUnbeatenStreak = 0;
		CurrentWinStreak = 0;
		HighestWinStreak = 0;
	}

	private void ResetSeasonRecords()
	{
		GoalsScoredSeason = 0;
		GoalsConcededSeason = 0;
		HomeMatchesSeason = 0;
		AverageAttendanceSeason = 0;
		CleanSheetsSeason = 0;
	}

	public void AddNewCareerEntry()
	{
		CareerHistory.Add(CurrentClubID, new CareerEntry(CurrentClubID, Game.instance.CurrentDate, CurrentSeasonID));
	}

	public void AddNewSeasonHistory(string nextSeasonID)
	{
		CurrentSeasonID = nextSeasonID;
		if (!CareerHistory[CurrentClubID].seasonHistory.ContainsKey(CurrentSeasonID))
		{
			CareerHistory[CurrentClubID].seasonHistory.Add(CurrentSeasonID, new SeasonHistory(nextSeasonID));
		}
	}

	public void AddLeagueTrophy(int leagueID)
	{
		string text = ((Game.instance.IsGermanUser && LeagueData.IsLeagueForDFLLicense(leagueID)) ? ("League-" + leagueID + "-dfl") : ("League - " + leagueID));
		LeagueData.GetName(leagueID);
		string imagePath = "Trophies/Leagues/" + text;
		Trophy trophy = new Trophy(Trophy.TrophyType.League, leagueID, text, imagePath);
		AddOrUpdateTrophy(trophy);
		UserManagerProfileController.AddOrUpdateTrophy(CurrentClubID, new Trophy(trophy));
	}

	public void AddCupTrophy(CupData cupData)
	{
		string text = cupData.CountryCode + " - " + cupData.CupDataID;
		_ = cupData.Name;
		string imagePath = "Trophies/Cup/" + text;
		Trophy trophy = new Trophy(Trophy.TrophyType.Cup, cupData.CupDataID, text, imagePath);
		AddOrUpdateTrophy(trophy);
		UserManagerProfileController.AddOrUpdateTrophy(CurrentClubID, new Trophy(trophy));
	}

	public void AddContinentalTrophy(int compID)
	{
		string text = "Cont - " + compID;
		ContinentalComp.GetName(compID);
		string imagePath = "Trophies/Continental/" + text;
		Trophy trophy = new Trophy(Trophy.TrophyType.Continental, compID, text, imagePath);
		AddOrUpdateTrophy(trophy);
		UserManagerProfileController.AddOrUpdateTrophy(CurrentClubID, new Trophy(trophy));
	}

	private void AddOrUpdateTrophy(Trophy trophy)
	{
		if (!ClubWonTrophies.ContainsKey(CurrentClubID))
		{
			ClubWonTrophies.Add(CurrentClubID, new TrophyList(trophy));
		}
		else if (!ClubWonTrophies[CurrentClubID].Trophies.Any((Trophy i) => i.TrophyID == trophy.TrophyID))
		{
			ClubWonTrophies[CurrentClubID].Trophies.Add(trophy);
		}
		else
		{
			ClubWonTrophies[CurrentClubID].Trophies.FirstOrDefault((Trophy i) => i.TrophyID == trophy.TrophyID).TimesWon++;
			ClubWonTrophies[CurrentClubID].Trophies.FirstOrDefault((Trophy i) => i.TrophyID == trophy.TrophyID).SeasonsWon.Add(Game.instance.g_season.SeasonID);
		}
		CareerHistory[CurrentClubID].seasonHistory[CurrentSeasonID].TrophiesWon++;
	}

	public int GetClubSeasonTrophyCount(short clubID, string seasonID)
	{
		if (ClubWonTrophies.ContainsKey(clubID))
		{
			return ClubWonTrophies[clubID].Trophies.Where((Trophy i) => i.SeasonsWon.Contains(seasonID)).Count();
		}
		return 0;
	}

	public int GetTimesTrophyWonByClub(short clubID, string trophyID)
	{
		if (ClubWonTrophies.ContainsKey(clubID) && ClubWonTrophies[clubID].Trophies.Any((Trophy i) => i.TrophyID == trophyID))
		{
			return ClubWonTrophies[clubID].Trophies.FirstOrDefault((Trophy i) => i.TrophyID == trophyID).TimesWon;
		}
		return 0;
	}

	public string GetYearsTrophyWonByClub(short clubID, string trophyID)
	{
		string yearsWon = "";
		if (ClubWonTrophies.ContainsKey(clubID))
		{
			ClubWonTrophies[CurrentClubID].Trophies.FirstOrDefault((Trophy i) => i.TrophyID == trophyID).SeasonsWon.ForEach((string i) =>
			{
				yearsWon = yearsWon + i.ToString() + Environment.NewLine;
			});
		}
		return yearsWon;
	}

	public int GetAmountOfClubsManaged()
	{
		return CareerHistory.Count;
	}

	public bool ManagedInMultipleCountries()
	{
		return (from i in CareerHistory
			group i by Club.GetAssociationID(i.Key) into i
			select i.First()).ToList().Count > 1;
	}

	public CareerEntry GetCurrentCareerEntry()
	{
		return CareerHistory[CurrentClubID];
	}

	public SeasonHistory GetCurrentSeasonHistory()
	{
		return CareerHistory[CurrentClubID].seasonHistory[CurrentSeasonID];
	}

	public List<SeasonHistory> GetAllCurrentClubSeasonHistory()
	{
		return CareerHistory[CurrentClubID].seasonHistory.Values.ToList();
	}

	public List<CareerEntry> GetAllCareerHistory()
	{
		return CareerHistory.Values.ToList();
	}

	public byte GetGamesPlayedCurrentClub()
	{
		return (byte)CareerHistory[CurrentClubID].seasonHistory.Sum((KeyValuePair<string, SeasonHistory> i) => i.Value.GamesPlayed);
	}

	public byte GetGamesPlayedSeason(string seasonID)
	{
		return (byte)CareerHistory[CurrentClubID].seasonHistory.Where((KeyValuePair<string, SeasonHistory> i) => i.Key == seasonID).Sum((KeyValuePair<string, SeasonHistory> i) => i.Value.GamesPlayed);
	}

	public byte GetGamesWonSeason(string seasonID)
	{
		return (byte)CareerHistory[CurrentClubID].seasonHistory.Where((KeyValuePair<string, SeasonHistory> i) => i.Key == seasonID).Sum((KeyValuePair<string, SeasonHistory> i) => i.Value.Wins);
	}

	public void SetRecordSigning(int amount, int playerID)
	{
		RecordSigningAmount = amount;
		RecordSigningPlayerID = playerID;
	}

	public int GetRecordSigningAmount()
	{
		return RecordSigningAmount;
	}

	public int GetRecordSigningPlayerID()
	{
		return RecordSigningPlayerID;
	}

	public void SetRecordSigningReceived(int amount, int playerID)
	{
		RecordSigningReceivedAmount = amount;
		RecordSigningReceivedPlayerID = playerID;
	}

	public int GetRecordSigningReceivedAmount()
	{
		return RecordSigningReceivedAmount;
	}

	public int GetRecordSigningReceivedPlayerID()
	{
		return RecordSigningReceivedPlayerID;
	}

	public void SetRecordAttendance(int attendance, int clubID)
	{
		RecordAttendance = attendance;
		RecordAttendanceClubID = clubID;
	}

	public int GetRecordAttendance()
	{
		return RecordAttendance;
	}

	public int GetRecordAttendanceClubID()
	{
		return RecordAttendanceClubID;
	}

	public void IncreaseHomeMatches()
	{
		HomeMatchesSeason++;
	}

	public short GetHomeMatches()
	{
		return HomeMatchesSeason;
	}

	public void SetRecordWin(int[] score, short oppClub)
	{
		RecordWinScore = score;
		RecordWinOppClub = oppClub;
	}

	public int[] GetRecordWinScore()
	{
		return RecordWinScore;
	}

	public short GetRecordWinOppClub()
	{
		return RecordWinOppClub;
	}

	public void SetRecordLoss(int[] score, short oppClub)
	{
		RecordLossScore = score;
		RecordLossOppClub = oppClub;
	}

	public int[] GetRecordLossScore()
	{
		return RecordLossScore;
	}

	public short GetRecordLossOppClub()
	{
		return RecordLossOppClub;
	}

	public void ResetUnbeatenStreak()
	{
		CurrentUnbeatenStreak = 0;
	}

	public void IncreaseUnbeatenStreak()
	{
		CurrentUnbeatenStreak++;
	}

	public int GetCurrentUnbeatenStreak()
	{
		return CurrentUnbeatenStreak;
	}

	public void SetHighestUnbeatenStreak(int streak)
	{
		HighestUnbeatenStreak = streak;
	}

	public int GetHighestUnbeatenStreak()
	{
		return HighestUnbeatenStreak;
	}

	public void ResetCurrentWinStreak()
	{
		CurrentWinStreak = 0;
	}

	public void IncreaseCurrentWinStreak()
	{
		CurrentWinStreak++;
	}

	public int GetCurrentWinStreak()
	{
		return CurrentWinStreak;
	}

	public void SetHighestWinStreak(int winStreak)
	{
		HighestWinStreak = winStreak;
	}

	public int GetHighestWinStreak()
	{
		return HighestWinStreak;
	}

	public byte GetCurrentClubWins()
	{
		return (byte)CareerHistory[CurrentClubID].seasonHistory.Sum((KeyValuePair<string, SeasonHistory> i) => i.Value.Wins);
	}

	public short GetGoalsScored()
	{
		return GoalsScoredSeason;
	}

	public void SetGoalsScored(short goals)
	{
		GoalsScoredSeason += goals;
	}

	public short GetGoalsConceded()
	{
		return GoalsConcededSeason;
	}

	public void SetGoalsConceded(short goals)
	{
		GoalsConcededSeason += goals;
	}

	public short GetCleanSheets()
	{
		return CleanSheetsSeason;
	}

	public void IncreaseCleanSheets()
	{
		CleanSheetsSeason++;
	}

	public void AddAttendance(short attendance)
	{
		AverageAttendanceSeason += attendance;
	}

	public int GetAverageAttendance()
	{
		return AverageAttendanceSeason * 1000 / GetHomeMatches();
	}

	public byte GetCurrentClubLoses()
	{
		return (byte)CareerHistory[CurrentClubID].seasonHistory.Sum((KeyValuePair<string, SeasonHistory> i) => i.Value.Loses);
	}

	public byte GetCurrentClubDraws()
	{
		return (byte)CareerHistory[CurrentClubID].seasonHistory.Sum((KeyValuePair<string, SeasonHistory> i) => i.Value.Draws);
	}

	internal void PostMatch(Matchday.MatchResult result, short userClubID, short oppClubID, bool canLevelUp = false)
	{
		Helpers.LogConsole("###TheoBugTest() -- PostMatch -- entered with the value of canLevelUp ==  " + canLevelUp);
		CareerHistory[CurrentClubID].seasonHistory[CurrentSeasonID].GamesPlayed++;
		Helpers.LogConsole("###TheoBugTest() -- PostMatch -- Carreer history set.");
		short goalsScored = ((Game.instance.g_matchday_nextfix.homeClubId == userClubID) ? Game.instance.g_matchday_nextfix.homeScore : Game.instance.g_matchday_nextfix.awayScore);
		short num = ((Game.instance.g_matchday_nextfix.homeClubId == userClubID) ? Game.instance.g_matchday_nextfix.awayScore : Game.instance.g_matchday_nextfix.homeScore);
		Helpers.LogConsole("###TheoBugTest() -- PostMatch -- Goal values set");
		switch (result)
		{
		case Matchday.MatchResult.Win:
			CareerHistory[CurrentClubID].seasonHistory[CurrentSeasonID].Wins++;
			IncreaseCurrentWinStreak();
			IncreaseUnbeatenStreak();
			if (((Game.instance.g_matchday_nextfix.homeClubId == userClubID) ? Game.instance.g_matchday_nextfix.homeScore : Game.instance.g_matchday_nextfix.awayScore) > GetRecordWinScore().Max())
			{
				SetRecordWin(new int[2]
				{
					Game.instance.g_matchday_nextfix.homeScore,
					Game.instance.g_matchday_nextfix.awayScore
				}, oppClubID);
			}
			break;
		case Matchday.MatchResult.Draw:
			CareerHistory[CurrentClubID].seasonHistory[CurrentSeasonID].Draws++;
			ResetCurrentWinStreak();
			IncreaseUnbeatenStreak();
			break;
		case Matchday.MatchResult.Lose:
			CareerHistory[CurrentClubID].seasonHistory[CurrentSeasonID].Loses++;
			ResetCurrentWinStreak();
			ResetUnbeatenStreak();
			if (((Game.instance.g_matchday_nextfix.homeClubId == userClubID) ? Game.instance.g_matchday_nextfix.awayScore : Game.instance.g_matchday_nextfix.homeScore) > GetRecordLossScore().Max())
			{
				SetRecordLoss(new int[2]
				{
					Game.instance.g_matchday_nextfix.homeScore,
					Game.instance.g_matchday_nextfix.awayScore
				}, oppClubID);
			}
			break;
		}
		Helpers.LogConsole("###TheoBugTest() -- PostMatch -- Switch for result state done.");
		if (num == 0)
		{
			IncreaseCleanSheets();
		}
		Helpers.LogConsole("###TheoBugTest() -- PostMatch -- increase clean sheet value done");
		SetGoalsScored(goalsScored);
		Helpers.LogConsole("###TheoBugTest() -- PostMatch -- set goal scored");
		SetGoalsConceded(num);
		Helpers.LogConsole("###TheoBugTest() -- PostMatch -- set goal conceded");
		SetPostMatchRecords();
		Helpers.LogConsole("###TheoBugTest() -- PostMatch -- SetPostMatchRecords");
		SetSeasonResult();
		Helpers.LogConsole("###TheoBugTest() -- PostMatch -- SetSeasonResult");
		if (CareerHistory[CurrentClubID].seasonHistory[CurrentSeasonID].GamesPlayed >= 10)
		{
			CurrentClubConfidence.CalculateFanConfidence();
			CurrentClubConfidence.CalculatePostMatchBoardConfidence(result, userClubID, oppClubID);
		}
		Helpers.LogConsole("###TheoBugTest() -- PostMatch -- CalculateFanConfidence / postMatch conf done");
		if (!UserManagerProfileController.UserHasCareerEntry(Save.saveUID))
		{
			Helpers.LogConsole("###TheoBugTest() -- PostMatch -- UserHasCareerEntry is false... attempting to add instead.");
			if (CareerHistory[CurrentClubID].seasonHistory[CurrentSeasonID].GamesPlayed >= 10)
			{
				Helpers.LogConsole("###TheoBugTest() -- PostMatch -- UserHasCareerEntry is true... user has played enough matches to create a global career entry.");
				UserManagerProfileController.AddNewCareerEntry(Save.saveUID, CareerHistory[CurrentClubID], canLevelUp);
				return;
			}
			Helpers.LogConsole("###TheoBugTest() -- AddNewCareerEntry -- Games less then 10, just saving instead.");
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			dictionary.Add("CanLevelUp", canLevelUp);
			Helpers.LogConsole("###TheoBugTest() -- AddNewCareerEntry -- user is able to level up, calling save function to allow levelup.");
			CustomerDataController.Instance.SaveUserManagerProfile(CustomerDataController.Instance.SaveUserDataCallback, dictionary);
		}
		else
		{
			Helpers.LogConsole("###TheoBugTest() -- PostMatch -- UserHasCareerEntry is true... attempting to update instead.");
			UserManagerProfileController.UpdateCareerEntry(Save.saveUID, CareerHistory[CurrentClubID], canLevelUp);
		}
	}

	private void SetPostMatchRecords()
	{
		if (GetHighestWinStreak() < GetCurrentWinStreak())
		{
			SetHighestWinStreak(GetCurrentWinStreak());
		}
		if (GetHighestUnbeatenStreak() < GetCurrentUnbeatenStreak())
		{
			SetHighestUnbeatenStreak(GetCurrentUnbeatenStreak());
		}
	}

	private void SetSeasonResult()
	{
		byte gamesPlayed = CareerHistory[CurrentClubID].seasonHistory[CurrentSeasonID].GamesPlayed;
		byte wins = CareerHistory[CurrentClubID].seasonHistory[CurrentSeasonID].Wins;
		byte draws = CareerHistory[CurrentClubID].seasonHistory[CurrentSeasonID].Draws;
		byte loses = CareerHistory[CurrentClubID].seasonHistory[CurrentSeasonID].Loses;
		CareerHistory[CurrentClubID].seasonHistory[CurrentSeasonID].WinPercentage = (float)(int)wins / (float)(int)gamesPlayed * 100f;
		CareerHistory[CurrentClubID].seasonHistory[CurrentSeasonID].DrawPercentage = (float)(int)draws / (float)(int)gamesPlayed * 100f;
		CareerHistory[CurrentClubID].seasonHistory[CurrentSeasonID].LosePercentage = (float)(int)loses / (float)(int)gamesPlayed * 100f;
	}

	public void SaveCustomFormation(int formationID)
	{
		if (!CustomFormations.ContainsKey(formationID))
		{
			CustomFormations.Add(formationID, new CustomFormation());
			CustomFormations[formationID].Name = FormationData.GetName(TacticsController.GetInstance().GetTacticsReference().inst.formationid);
		}
		for (int i = 0; i < CustomFormationController.GetInstance().CurrentFormation.Length; i++)
		{
			int num = CustomFormationController.GetInstance().CurrentFormation[i].pos[0];
			int num2 = CustomFormationController.GetInstance().CurrentFormation[i].pos[1];
			Game.instance.ManagedClubProfile.CustomFormations[formationID].Formation[i].pos[0] = num;
			Game.instance.ManagedClubProfile.CustomFormations[formationID].Formation[i].pos[1] = num2;
			FormationData.g_formations[formationID][i].pos[0] = num;
			FormationData.g_formations[formationID][i].pos[1] = num2;
		}
		CustomFormations.Remove(31);
		FormationHelperFactory.UpdateFormationHelperCustom((byte)formationID);
		TacticsController.GetInstance().GetTacticsReference().inst.formationid = (byte)formationID;
		Game.instance.ManagedClubProfile.CustomFormations[formationID].Instructions.copy(TacticsController.GetInstance().GetTacticsReference().inst);
		TacticsController.GetInstance().tacticsScreen.UpdateFormationUI();
		Save.SaveData(Save.currentSlot, showAnimation: false);
	}

	public void UpdateCurrentObjectiveProgress()
	{
		for (int i = 0; i < ObjectivesList.ClubObjectives.Count; i++)
		{
			ObjectivesList.ClubObjectives[i].UpdateObjectiveProgress();
		}
	}

	public void ResetObjectives()
	{
		int num = 0;
		int num2 = 0;
		for (int i = 0; i < ObjectivesList.ClubObjectives.Count; i++)
		{
			if (ObjectivesList.ClubObjectives[i].Completed)
			{
				num++;
				num2 += 1000;
				CurrentClubConfidence.CalculateObjectivesCompleteBoardConfidence(ObjectivesList.ClubObjectives[i].Length);
			}
			else if (ObjectivesList.ClubObjectives[i].Failed)
			{
				CurrentClubConfidence.CalculateObjectivesFailedBoardConfidence(ObjectivesList.ClubObjectives[i].Length);
			}
		}
		int seasonID = Season.GetStartYear(Season.GetCurrentSeasonID());
		ObjectivesList.ClubObjectives.RemoveAll((ClubObjectivesData clubObjectivesData) => clubObjectivesData.Completed || clubObjectivesData.Failed || seasonID >= clubObjectivesData.EndSeasonID);
		Game.LogConsole("COMPLETED " + num + " OBJECTIVES. 1000 for each. Prize money " + num2);
		Game.instance.CurrentManagedClub.Balance += num2;
		SeasonObjective = ClubObjectives.SetSeasonObjective();
		ObjectivesList.ClubObjectives.AddRange(ClubObjectives.PopulateClubObjectives(SeasonObjective).ClubObjectives);
	}

	public void CheckAllObjectives()
	{
		for (int i = 0; i < ObjectivesList.ClubObjectives.Count; i++)
		{
			ObjectivesList.ClubObjectives[i].CheckObjectiveComplete();
		}
	}

	public void CheckFinancialSeasonEndObjectives()
	{
		CheckObjectiveProgress(ClubObjectiveSubType.ReduceWage);
		CheckObjectiveProgress(ClubObjectiveSubType.MaintainWage);
		CheckObjectiveProgress(ClubObjectiveSubType.Profit);
	}

	public bool HasLongTermCompObjective()
	{
		return ObjectivesList.ClubObjectives.Any((ClubObjectivesData i) => i.Length == ClubObjectiveLength.Long && i.ObjType == ClubObjectiveType.Comp);
	}

	public bool HasLongTermFinanceObjective()
	{
		return ObjectivesList.ClubObjectives.Any((ClubObjectivesData i) => i.Length == ClubObjectiveLength.Long && i.ObjType == ClubObjectiveType.Financial);
	}

	public bool HasLongTermGrowthObjective()
	{
		return ObjectivesList.ClubObjectives.Any((ClubObjectivesData i) => i.Length == ClubObjectiveLength.Long && i.ObjType == ClubObjectiveType.Growth);
	}

	public bool ClubHasObjective(ClubObjectiveSubType subType)
	{
		return ObjectivesList.ClubObjectives.Any((ClubObjectivesData i) => i.SubType == subType);
	}

	public void CheckObjectiveProgress(ClubObjectiveSubType subtype)
	{
		if (ClubHasObjective(subtype))
		{
			ObjectivesList.ClubObjectives.FirstOrDefault((ClubObjectivesData i) => i.SubType == subtype).CheckObjectiveComplete();
		}
	}

	public void CheckSignPlayersObjective(ClubObjectiveSubType subType)
	{
		if (ClubHasObjective(subType))
		{
			GrowthObjective growthObjective = (GrowthObjective)ObjectivesList.ClubObjectives.FirstOrDefault((ClubObjectivesData i) => i.SubType == subType);
			growthObjective.CurrentAmount++;
			growthObjective.CheckObjectiveComplete();
		}
	}

	public void CheckPlayerRatingObjective(ClubObjectiveSubType subType, int increase)
	{
		if (ClubHasObjective(subType))
		{
			GrowthObjective growthObjective = (GrowthObjective)ObjectivesList.ClubObjectives.FirstOrDefault((ClubObjectivesData i) => i.SubType == subType);
			growthObjective.CurrentAmount += increase;
			growthObjective.CheckObjectiveComplete();
		}
	}

	public void CheckClubZoneObjectives()
	{
		CheckObjectiveProgress(ClubObjectiveSubType.ClubZoneUpgrade);
		CheckObjectiveProgress(ClubObjectiveSubType.ClubZoneUpgradeLong);
		CheckObjectiveProgress(ClubObjectiveSubType.MaxClubZoneLong);
	}

	public void CheckFacilityObjectives(ClubObjectiveSubType subType)
	{
		if (ClubHasObjective(subType))
		{
			GrowthObjective growthObjective = (GrowthObjective)ObjectivesList.ClubObjectives.FirstOrDefault((ClubObjectivesData i) => i.SubType == subType);
			growthObjective.CurrentAmount++;
			growthObjective.CheckObjectiveComplete();
		}
	}

	public int GetBoardConfidence()
	{
		return CurrentClubConfidence.BoardConfidence;
	}

	public int GetFanConfidence()
	{
		return CurrentClubConfidence.FanConfidence;
	}

	public double GetManagerRating()
	{
		return CurrentClubConfidence.GetManagerRating();
	}
}
