using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Matchday
{
	internal enum MatchResult
	{
		Win,
		Lose,
		Draw
	}

	public class TacticsValid
	{
		public bool success;

		public string reason;
	}

	public static bool BuildUpProcess()
	{
		Fixture fixture = Game.ClubHasUnplayedMatchToday(Game.instance.CurrentManagedClubId);
		CreatePreviewMatchMessageIfMatchToday();
		if (fixture != null)
		{
			return RandomEvent.SendMediaQuestion(RandomEvent.QuestionType.NextFixture, fixture, -1, -1, -1, -1);
		}
		return false;
	}

	private static void CreatePreviewMatchMessageIfMatchToday()
	{
		NextFixtureInfo nextFixture = Game.instance.GetNextFixture(Game.GetCurrentUserClubID());
		if (nextFixture != null)
		{
			double totalHours = (nextFixture.fixture.Time - Game.instance.CurrentDate).TotalHours;
			if (totalHours >= 0.0 && totalHours < 12.0)
			{
				CreatePreviewMatchMessage(nextFixture.fixture);
			}
		}
	}

	private static void CreatePreviewMatchMessage(Fixture nextFix)
	{
		short key = ((nextFix.homeClubId == Game.instance.CurrentManagedClubId) ? nextFix.awayClubId : nextFix.homeClubId);
		if (Game.instance.g_ClubData.ContainsKey(key))
		{
			MessageManager.ScoutNextOpposition(nextFix, Game.instance.g_ClubData[key].TeamFormation);
		}
	}

	public static bool Process()
	{
		switch (Game.instance.g_matchday_flowpos)
		{
		case 0:
			if (Game.instance.g_season.Schedule.UnplayedMatchExistsForDisplay())
			{
				Fixture fixture3 = Game.ClubHasUnplayedMatchToday(Game.instance.CurrentManagedClubId);
				bool flag = Schedule.HasClubUnplayedMatchScheduledNow(Game.instance.CurrentManagedClubId, Game.instance.CurrentDate);
				if (fixture3 != null && flag)
				{
					Game.instance.g_matchday_nextfix = fixture3;
				}
				if (flag)
				{
					GameLoopController.Instance.PauseGameLoop();
				}
				else
				{
					GameLoopController.Instance.DelayGameLoopForMatchdayFixturesScreen();
				}
				return true;
			}
			if (Game.instance.g_season.Schedule.UnplayedScheduleDate != DateTime.MinValue && Game.instance.g_season.Schedule.UnplayedScheduleDate <= Game.instance.CurrentDate.AddHours(-2.0))
			{
				Game.instance.g_season.Schedule.BringUpToDate();
			}
			return false;
		case 1:
			if (Game.instance.ui.g_currentscreen != 19)
			{
				GameLoopController.Instance.PauseGameLoopForMatchdayFixturesScreen();
				return true;
			}
			if (Game.instance.g_matchday_nextfix == null)
			{
				Fixture fixture2 = Game.ClubHasUnplayedMatchToday(Game.instance.CurrentManagedClubId);
				bool flag = Schedule.HasClubUnplayedMatchScheduledNow(Game.instance.CurrentManagedClubId, Game.instance.CurrentDate);
				if ((fixture2 != null) & flag)
				{
					Game.instance.g_matchday_nextfix = fixture2;
				}
			}
			if (Game.instance.g_matchday_nextfix != null)
			{
				if (Game.instance.g_matchday_nextfix.competitiontype == 5)
				{
					Game.instance.g_matchday_flowpos = 3;
					Process();
					break;
				}
				Game.instance.g_matchday_flowpos++;
				goto case 2;
			}
			Game.instance.g_matchday_flowpos = 11;
			Process();
			return true;
		case 2:
			Game.instance.g_matchday_flowpos++;
			goto case 3;
		case 3:
		{
			Fixture fixture = Game.ClubHasUnplayedMatchToday(Game.instance.CurrentManagedClubId);
			bool flag = Schedule.HasClubUnplayedMatchScheduledNow(Game.instance.CurrentManagedClubId, Game.instance.CurrentDate);
			if ((fixture != null) & flag)
			{
				Game.instance.g_matchday_nextfix = fixture;
			}
			if (Game.instance.ui.g_currentscreen != 14)
			{
				GameLoopController.Instance.PauseGameLoop(14);
				return true;
			}
			TacticsValid tacticsValid = ValidateTactics();
			if (tacticsValid.reason == "squad-number-not-set" && !tacticsValid.success && !Game.instance.DEBUG_TOOLS_PROCESSING)
			{
				GameLoopController.Instance.PauseGameLoopForMatchdaySquadNumberModal();
				break;
			}
			if (tacticsValid.success || Game.instance.DEBUG_TOOLS_PROCESSING)
			{
				Game.instance.g_matchday_flowpos = 4;
				if (Game.instance.CurrentManagedClub.MatchdayType != 0 && object.Equals(Game.instance.matchdayBoost, null))
				{
					bool flag2 = false;
					MatchdayBoost matchdayBoost;
					if (object.Equals(Game.instance.matchdayBoost, null))
					{
						matchdayBoost = new MatchdayBoost();
						Game.instance.matchdayBoost = matchdayBoost;
						flag2 = matchdayBoost.FillPlayerOptions();
					}
					else
					{
						matchdayBoost = Game.instance.matchdayBoost;
						flag2 = true;
					}
					if (flag2)
					{
						GameLoopController.Instance.PauseGameLoopForPreMatchBoostModal(matchdayBoost);
						Game.instance.g_matchday_flowpos = 3;
					}
					else
					{
						Game.instance.g_matchday_flowpos = 6;
					}
					break;
				}
				goto case 6;
			}
			Helpers.ShowToast(tacticsValid.reason);
			GameLoopController.Instance.PauseGameLoop(14);
			break;
		}
		case 6:
		{
			Fixture fixture4 = Game.ClubHasUnplayedMatchToday(Game.instance.CurrentManagedClubId);
			bool flag = Schedule.HasClubUnplayedMatchScheduledNow(Game.instance.CurrentManagedClubId, Game.instance.CurrentDate);
			if ((fixture4 != null) & flag)
			{
				Game.instance.g_matchday_nextfix = fixture4;
			}
			if (Game.instance.ui.g_currentscreen != 34)
			{
				short currentUserClubID = Game.GetCurrentUserClubID();
				if (!Game.instance.ManagedClubProfile.CareerHistory.ContainsKey(currentUserClubID))
				{
					Game.instance.ManagedClubProfile.AddNewCareerEntry();
				}
				Game.instance.g_matchday_nextfix.PreFixture(Game.instance.CurrentDate);
				GameLoopController.Instance.PauseGameLoopForMatchdayPreviewScreen();
				return true;
			}
			Game.instance.g_matchday_saveseed = Game.instance.Random.GetSeed();
			int[][] array2 = new int[2][];
			for (int k = 0; k < 2; k++)
			{
				array2[k] = new int[18];
				for (int l = 0; l < 18; l++)
				{
					array2[k][l] = Game.instance.g_tm[k].lineup[l];
				}
			}
			Game.instance.g_fixtureTime = Game.instance.CurrentDate;
			_ = Game.instance.g_matchday_nextfix;
			_ = Game.instance;
			Game.instance.Random.GetSeed();
			Game.instance.g_matchday_flowpos = 4;
			goto case 4;
		}
		case 4:
		{
			Fixture fixture = Game.ClubHasUnplayedMatchToday(Game.instance.CurrentManagedClubId);
			bool flag = Schedule.HasClubUnplayedMatchScheduledNow(Game.instance.CurrentManagedClubId, Game.instance.CurrentDate);
			if ((fixture != null) & flag)
			{
				Game.instance.g_matchday_nextfix = fixture;
			}
			if (Game.instance.g_matchday_flowpos != 4)
			{
				break;
			}
			Game.instance.g_matchday_saveseed = Game.instance.Random.GetSeed();
			int[][] array = new int[2][];
			for (int i = 0; i < 2; i++)
			{
				array[i] = new int[18];
				for (int j = 0; j < 18; j++)
				{
					array[i][j] = Game.instance.g_tm[i].lineup[j];
				}
			}
			Game.instance.g_fixtureTime = Game.instance.CurrentDate;
			bool flag3 = true;
			SetLiveScoreDataForMatchDay(fixture);
			Game.instance.g_matchday_nextfix.PreFixture(Game.instance.CurrentDate);
			if (Game.instance.CurrentManagedClub.MatchdayType == 0 || !flag3)
			{
				EngineQuick.PlayFixture(Game.instance.g_matchday_nextfix, Game.instance.CurrentDate);
				ProcessAfterMatchDay();
				break;
			}
			FixtureConverter.LoadFixtureIntoSE(Game.instance.g_matchday_nextfix, Game.instance.g_fixtureTime, Game.instance.g_matchday_saveseed);
			Game.instance.ReturningFromMatchday = true;
			if (SceneManager.GetActiveScene().name != "Match")
			{
				UnityEngine.Object.DontDestroyOnLoad(Game.instance);
				int sceneBuildIndex = 2;
				if (MatchTimeOfDay.IsNightMatch())
				{
					sceneBuildIndex = 3;
				}
				SceneManager.LoadScene(sceneBuildIndex);
			}
			break;
		}
		case 11:
			Helpers.calculateTableEntries();
			Game.instance.g_season.Schedule.BringUpToDate();
			RunMissedFixtures();
			Game.instance.CurrentDate = Game.instance.CurrentDate.AddHours(2.0);
			if (Game.instance.g_matchday_nextfix != null)
			{
				GameLoopController.Instance.PauseGameLoopForMatchdayResultsScreen();
			}
			else
			{
				GameLoopController.Instance.DelayGameLoopForMatchdayResultsScreen();
			}
			Game.instance.g_matchday_flowpos++;
			Helpers.calculateTableEntries();
			ScheduleNextMatchReminders();
			if (Game.instance.g_matchday_nextfix != null)
			{
				Game.instance.g_matchday_nextfix = null;
				if (!Game.instance.DEBUG_TOOLS_PROCESSING)
				{
					Save.SaveData(Save.currentSlot);
				}
				CheckManagedPlayerAvailability(null);
			}
			return true;
		case 5:
			Game.instance.g_bringUpToDate = false;
			if (Game.instance.CurrentManagedClub.MatchdayType == 1)
			{
				return false;
			}
			break;
		case 12:
			Game.instance.g_bringUpToDate = false;
			Game.instance.g_matchday_flowpos = 13;
			return true;
		}
		return true;
	}

	private static void SetLiveScoreDataForMatchDay(Fixture fixture)
	{
		if (!Constants.LIVE_SCORES_ENABLED)
		{
			return;
		}
		Game.instance.g_season.Schedule.UpdateUnplayedFixtureDate(fixture);
		Game.instance.g_season.Schedule.BringUpToDate();
		if (Game.instance.g_LiveScores != null)
		{
			Game.instance.g_LiveScores = Game.instance.g_LiveScores.OrderBy((LiveScoresOutput value) => value.eventTime).ToList();
		}
		else
		{
			Game.instance.g_LiveScores = new List<LiveScoresOutput>();
		}
	}

	public static void RunMissedFixtures()
	{
		Fixture fixture = Game.ClubHasUnplayedMatchToday(Game.instance.CurrentManagedClubId);
		if (fixture != null && fixture.Time != DateTime.MinValue && fixture.Time <= Game.instance.CurrentDate.AddHours(2.0))
		{
			if (fixture.Time > Game.instance.CurrentDate.AddHours(1.0))
			{
				Game.instance.CurrentDate = Game.instance.CurrentDate.AddHours(1.0);
			}
			return;
		}
		_ = Game.instance.g_season.Schedule.UnplayedScheduleDateForDisplay;
		if (Game.instance.g_season.Schedule.UnplayedScheduleDateForDisplay != DateTime.MinValue && Game.instance.CurrentDate.AddHours(2.0) >= Game.instance.g_season.Schedule.UnplayedScheduleDateForDisplay)
		{
			Game.instance.CurrentDate = Game.instance.CurrentDate.AddHours(1.0);
			Game.instance.g_season.Schedule.BringUpToDate();
			RunMissedFixtures();
		}
	}

	public static LiveTableOutput AddLiveTableRow(LiveTable row)
	{
		if (!Constants.LIVE_SCORES_ENABLED)
		{
			return null;
		}
		LiveTableOutput liveTableOutput = new LiveTableOutput
		{
			clubID = row.clubID,
			clubName = row.clubName,
			StartPosition = row.StartPosition,
			StartingGD = row.goalDifference,
			leagueID = row.leagueID,
			played = row.played,
			startingPoints = row.points
		};
		liveTableOutput.currentPoints = liveTableOutput.startingPoints;
		liveTableOutput.currentPosition = liveTableOutput.StartPosition;
		liveTableOutput.currentGoalDifference = liveTableOutput.StartingGD;
		return liveTableOutput;
	}

	public static LiveTable SetUpLiveTable(bool groupStages = true)
	{
		if (!Constants.LIVE_SCORES_ENABLED)
		{
			return null;
		}
		short currentManagedClubId = Game.instance.CurrentManagedClubId;
		ContinentalComp continentalComp = Game.instance.g_continentalCompetitions[Game.instance.g_matchday_nextfix.competitionid];
		LiveTable liveTable = new LiveTable();
		List<CupGroup> groups = continentalComp.groups;
		int num = 0;
		int num2 = 0;
		for (int i = 0; i < continentalComp.numclubs; i++)
		{
			for (int j = 0; j < groups[num2].numclubs; j++)
			{
				num = groups[num2].clubix[j];
				liveTable.clubID = groups[num2].clubid[num];
				if (currentManagedClubId == liveTable.clubID)
				{
					break;
				}
			}
			if (currentManagedClubId == liveTable.clubID)
			{
				break;
			}
			num2++;
		}
		int num3 = 0;
		int num4 = 0;
		int played = 0;
		int points = 0;
		for (int k = 0; k < groups[num2].numclubs; k++)
		{
			num = groups[num2].clubix[k];
			liveTable.clubID = groups[num2].clubid[num];
			liveTable.clubName = Club.GetName(liveTable.clubID);
			liveTable.StartPosition = k + 1;
			if (groups[num2].gamespld.Count != 0)
			{
				num3 = groups[num2].goalsfor[num];
				num4 = groups[num2].goalsagainst[num];
				played = groups[num2].gamespld[num];
				points = groups[num2].gameswon[num] * 3 + groups[num2].gamesdrawn[num];
			}
			liveTable.goalDifference = num3 - num4;
			liveTable.played = played;
			liveTable.points = points;
			Game.instance.g_LiveTable.Add(AddLiveTableRow(liveTable));
		}
		return liveTable;
	}

	public static LiveTable SetUpLiveTable()
	{
		if (!Constants.LIVE_SCORES_ENABLED)
		{
			return null;
		}
		short currentManagedClubId = Game.instance.CurrentManagedClubId;
		LeagueList.GetLeaguePosition(currentManagedClubId);
		int leagueDataId = Club.GetLeagueDataId(currentManagedClubId);
		Club.GetDivision(currentManagedClubId);
		ChosenLeague leagueFromSeason = Helpers.GetLeagueFromSeason(leagueDataId);
		LiveTable liveTable = new LiveTable();
		for (int i = 0; i < leagueFromSeason.NumClubs; i++)
		{
			int num = leagueFromSeason.ClubIX[i];
			liveTable.clubID = leagueFromSeason.ClubID[num];
			liveTable.clubName = Club.GetName(liveTable.clubID);
			liveTable.StartPosition = i + 1;
			int goalDifference = leagueFromSeason.GoalsFor[num] - leagueFromSeason.GoalsAgainst[num];
			liveTable.leagueID = leagueDataId;
			liveTable.goalDifference = goalDifference;
			liveTable.played = leagueFromSeason.GamesPld[num];
			int points = leagueFromSeason.GamesWon[num] * 3 + leagueFromSeason.GamesDrawn[num];
			liveTable.points = points;
			Game.instance.g_LiveTable.Add(AddLiveTableRow(liveTable));
		}
		return liveTable;
	}

	public static LiveScoresOutput AddEvent(LiveScores Event)
	{
		LiveScoresOutput liveScoresOutput = new LiveScoresOutput();
		liveScoresOutput.homeScore = Event.homeScore;
		liveScoresOutput.awayScore = Event.awayScore;
		liveScoresOutput.awayClubID = Event.awayClubID;
		liveScoresOutput.awayClubName = Event.awayClubName;
		liveScoresOutput.homeClubID = Event.homeClubID;
		liveScoresOutput.homeClubName = Event.homeClubName;
		liveScoresOutput.competitionID = Event.competitionID;
		liveScoresOutput.HomeEvent = Event.isAtHome;
		liveScoresOutput.redCard = Event.redCard;
		liveScoresOutput.goalEvent = Event.goalEvent;
		if (!liveScoresOutput.redCard)
		{
			liveScoresOutput.playerID = Event.playerID;
			liveScoresOutput.playerName = Event.playerName;
			liveScoresOutput.eventTime = Event.eventTime;
			liveScoresOutput.type = Event.type;
		}
		else
		{
			liveScoresOutput.playerID = Event.playerID;
			liveScoresOutput.playerName = Event.playerName;
			liveScoresOutput.eventTime = Event.eventTime;
			liveScoresOutput.cardTime = Event.cardTime;
		}
		return liveScoresOutput;
	}

	public static LiveScores SetTodaysFixturesLiveScores(Fixture fixture, Fixture playerNextfixture)
	{
		LiveScores liveScores = new LiveScores();
		if (fixture.competitionid == playerNextfixture.competitionid && fixture.competitiontype == playerNextfixture.competitiontype)
		{
			if (playerNextfixture.competitiontype == 5)
			{
				short currentUserClubID = Game.GetCurrentUserClubID();
				if (Club.GetLeagueDataId(currentUserClubID) != Club.GetLeagueDataId(fixture.homeClubId) && Club.GetLeagueDataId(currentUserClubID) != Club.GetLeagueDataId(fixture.awayClubId))
				{
					return liveScores;
				}
			}
			liveScores.homeClubID = fixture.homeClubId;
			liveScores.awayClubID = fixture.awayClubId;
			liveScores.homeClubName = Club.GetName(liveScores.homeClubID);
			liveScores.awayClubName = Club.GetName(liveScores.awayClubID);
			liveScores.competitionID = fixture.competitionid;
			fixture.red = fixture.red.OrderBy((GameEvent x) => x.time).ToList();
			for (int num = 0; num < fixture.red.Count; num++)
			{
				liveScores.redCard = true;
				liveScores.eventTime = fixture.red[num].time;
				liveScores.cardTime = fixture.red[num].time;
				liveScores.playerID = fixture.red[num].playerid;
				liveScores.playerName = Player.GetName(liveScores.playerID);
				for (int num2 = 0; num2 < fixture.goal.Count; num2++)
				{
					if (fixture.goal[num2].time < fixture.red[num].time)
					{
						if (liveScores.homeClubID == Player.GetClubID(fixture.goal[num2].playerid))
						{
							liveScores.homeScore++;
						}
						else
						{
							liveScores.awayScore++;
						}
					}
				}
				liveScores.goalEvent = false;
				if (liveScores.homeClubID == Player.GetClubID(liveScores.playerID))
				{
					liveScores.isAtHome = true;
				}
				else
				{
					liveScores.isAtHome = false;
				}
				addToLiveScores(AddEvent(liveScores));
			}
			liveScores.homeScore = 0;
			liveScores.awayScore = 0;
			fixture.goal.Sort((Goal a, Goal b) => a.time.CompareTo(b.time));
			for (int num3 = 0; num3 < fixture.goal.Count; num3++)
			{
				liveScores.eventTime = fixture.goal[num3].time;
				liveScores.playerID = fixture.goal[num3].playerid;
				liveScores.playerName = Player.GetName(liveScores.playerID);
				liveScores.goalEvent = true;
				if (liveScores.homeClubID == Player.GetClubID(liveScores.playerID))
				{
					liveScores.homeScore++;
					liveScores.isAtHome = true;
				}
				else
				{
					liveScores.awayScore++;
					liveScores.isAtHome = false;
				}
				liveScores.type = fixture.goal[num3].type;
				addToLiveScores(AddEvent(liveScores));
			}
			addToAllGames(AddEvent(liveScores));
		}
		return liveScores;
	}

	private static void addToLiveScores(LiveScoresOutput item)
	{
		Game.instance.g_LiveScores.Add(item);
	}

	private static void addToAllGames(LiveScoresOutput item)
	{
		Game.instance.g_allGames.Add(item);
	}

	private static void ScheduleNextMatchReminders()
	{
		short currentUserClubID = Game.GetCurrentUserClubID();
		NextFixtureInfo nextFixture = Game.instance.GetNextFixture(currentUserClubID);
		if (nextFixture != null)
		{
			string name = Game.instance.g_ClubData[currentUserClubID].Name;
			string name2 = Game.instance.g_ClubData[nextFixture.fixture.homeClubId].Name;
			if (nextFixture.fixture.homeClubId == currentUserClubID)
			{
				name2 = Game.instance.g_ClubData[nextFixture.fixture.awayClubId].Name;
			}
			NotificationController.ScheduleNextMatchReminders(name, name2);
		}
	}

	public static void ProcessAfterMatchDay()
	{
		Helpers.LogConsole("###TheoBugTest() -- ProcessAfterMatchDayCalled");
		Save.saveInProgress = false;
		Game.instance.g_matchday_nextfix.PostMatch(Game.instance.CurrentDate);
		SteamAchievementsController.GetInstance().AchievementTrackerUpdateCheckMatch(Game.instance.g_matchday_nextfix);
		Game.instance.Random.SetSeed(Game.instance.g_matchday_saveseed);
		Game.instance.matchdayBoost = null;
		Game.instance.g_matchday_flowpos = 11;
		Game.instance.ui.ChangeScreen(35, new MatchReportScreenStackOpts
		{
			fixture = Game.instance.g_matchday_nextfix
		});
		UI.GetInstance().ClearStack();
		TacticsController.IsMatchTactics = false;
		if (Game.instance.CurrentManagedClub.MatchdayType == 0)
		{
			UI.GetInstance().CheckForAndCreateScreenIfNeeded(34);
			_ = UI.screens[34];
		}
		GameObject gameObject = GameObject.Find("OverlayCanvas").transform.Find("LoadSpinner").gameObject;
		if (gameObject.activeSelf)
		{
			gameObject.transform.Find("LoadingPanel/GameCalendar").gameObject.SetActive(value: false);
			gameObject.SetActive(value: false);
		}
		if (!PlayerPrefs.HasKey("prefanalyticsmatchcomplete"))
		{
			PlayerPrefs.SetInt("prefanalyticsmatchcomplete", 1);
		}
		Helpers.LogConsole("###TheoBugTest() -- random fixture stuff before state checker");
		Fixture g_matchday_nextfix = Game.instance.g_matchday_nextfix;
		short currentManagedClubId = Game.instance.CurrentManagedClubId;
		short usersClubID = currentManagedClubId;
		short oppClubID = ((g_matchday_nextfix.homeClubId != currentManagedClubId) ? g_matchday_nextfix.homeClubId : g_matchday_nextfix.awayClubId);
		int value = 5;
		if (!PlayerPrefs.HasKey("RATE_REMIND_LIMIT"))
		{
			PlayerPrefs.SetInt("RATE_REMIND_LIMIT", value);
		}
		else
		{
			value = PlayerPrefs.GetInt("RATE_REMIND_LIMIT") - 1;
			if (value < 0)
			{
				value = 0;
			}
			PlayerPrefs.SetInt("RATE_REMIND_LIMIT", value);
		}
		Helpers.LogConsole("###TheoBugTest() -- Match State Checker, see if won/lost/draw.");
		if ((g_matchday_nextfix.homeClubId == currentManagedClubId && g_matchday_nextfix.homeScore > g_matchday_nextfix.awayScore) || (g_matchday_nextfix.awayClubId == currentManagedClubId && g_matchday_nextfix.awayScore > g_matchday_nextfix.homeScore))
		{
			Helpers.LogConsole("###TheoBugTest() -- Match was a win, enterd statement.");
			new ManagerPointAward
			{
				ReasonTextKey = Constants.CONST_MANAGER_REWARD_WIN_MATCH.ReasonTextKey,
				ManagerPoints = Constants.CONST_MANAGER_REWARD_WIN_MATCH.ManagerPoints
			};
			Helpers.LogConsole("###TheoBugTest() -- win -- Awards generated");
			UserManagerProfileController.AddManagerXPAfterMatch(ManagerPointAwardType.WonMatch);
			Helpers.LogConsole("###TheoBugTest() -- win -- manager xp added");
			Helpers.LogConsole("###TheoBugTest() -- win -- Checking to see if can level up");
			bool flag = UserManagerProfileController.CanLevelUp();
			Helpers.LogConsole("###TheoBugTest() -- win -- level up check finished, finished with value == " + flag);
			Helpers.LogConsole("###TheoBugTest() -- win -- Checking to see if it's a freindly.");
			if (g_matchday_nextfix.competitiontype != 5)
			{
				Helpers.LogConsole("###TheoBugTest() -- win -- it is not a friendly, call UpdateManagerProfile()");
				UpdateManagerProfile(usersClubID, oppClubID, MatchResult.Win, flag);
			}
			else
			{
				Helpers.LogConsole("###TheoBugTest() -- win -- it is a friendly, just saving the users profile instead.");
				Dictionary<string, object> dictionary = new Dictionary<string, object>();
				dictionary.Add("CanLevelUp", flag);
				Helpers.LogConsole("###TheoBugTest() -- win -- calling friendly save");
				CustomerDataController.Instance.SaveUserManagerProfile(CustomerDataController.Instance.SaveUserDataCallback, dictionary);
			}
			EventLoggingController.LogEvent("matchwin");
		}
		bool flag2 = UserManagerProfileController.CanLevelUp();
		if ((g_matchday_nextfix.homeClubId == currentManagedClubId && g_matchday_nextfix.homeScore < g_matchday_nextfix.awayScore) || (g_matchday_nextfix.awayClubId == currentManagedClubId && g_matchday_nextfix.awayScore < g_matchday_nextfix.homeScore))
		{
			Helpers.LogConsole("###TheoBugTest() -- lose -- loss area entered");
			if (g_matchday_nextfix.competitiontype != 5)
			{
				Helpers.LogConsole("###TheoBugTest() -- lose -- match is not a friendly, updating profile.");
				UpdateManagerProfile(usersClubID, oppClubID, MatchResult.Lose, flag2);
			}
			else
			{
				Helpers.LogConsole("###TheoBugTest() -- lose -- match is a friendly, allowing save.");
				Dictionary<string, object> dictionary2 = new Dictionary<string, object>();
				dictionary2.Add("CanLevelUp", flag2);
				Helpers.LogConsole("###TheoBugTest() -- lose -- match is a friendly, calling save");
				CustomerDataController.Instance.SaveUserManagerProfile(CustomerDataController.Instance.SaveUserDataCallback, dictionary2);
			}
			EventLoggingController.LogEvent("matchlose");
		}
		if ((g_matchday_nextfix.homeClubId == currentManagedClubId && g_matchday_nextfix.homeScore == g_matchday_nextfix.awayScore) || (g_matchday_nextfix.awayClubId == currentManagedClubId && g_matchday_nextfix.awayScore == g_matchday_nextfix.homeScore))
		{
			Helpers.LogConsole("###TheoBugTest() -- draw -- draw area entered");
			if (g_matchday_nextfix.competitiontype != 5)
			{
				Helpers.LogConsole("###TheoBugTest() -- draw -- match is not a friendly, updating profile.");
				UpdateManagerProfile(usersClubID, oppClubID, MatchResult.Draw, flag2);
			}
			else
			{
				Helpers.LogConsole("###TheoBugTest() -- draw -- match is a friendly, allowing save.");
				Dictionary<string, object> dictionary3 = new Dictionary<string, object>();
				dictionary3.Add("CanLevelUp", flag2);
				Helpers.LogConsole("###TheoBugTest() -- draw -- match is a friendly, calling save");
				CustomerDataController.Instance.SaveUserManagerProfile(CustomerDataController.Instance.SaveUserDataCallback, dictionary3);
			}
			EventLoggingController.LogEvent("matchdraw");
		}
		if (g_matchday_nextfix.homeClubId == currentManagedClubId && g_matchday_nextfix.stadiumid == Club.GetStadiumDataID(currentManagedClubId))
		{
			Game.instance.ManagedClubProfile.IncreaseHomeMatches();
			Game.instance.ManagedClubProfile.AddAttendance((short)(g_matchday_nextfix.attendance / 1000));
			if (Game.instance.ManagedClubProfile.GetRecordAttendance() < g_matchday_nextfix.attendance)
			{
				Game.instance.ManagedClubProfile.SetRecordAttendance(g_matchday_nextfix.attendance, g_matchday_nextfix.awayClubId);
			}
		}
	}

	public static int EncodeWinningAndUnbeatenStreak(int winning, int unbeaten, int cleansheet)
	{
		return (winning & 0xFF) | (((unbeaten & 0xFF) << 8) | ((cleansheet & 0xFF) << 16));
	}

	public static int[] DecodeWinningStreakAndUnbeatenStreak(int encodedUnbeatenWinStreak)
	{
		return new int[3]
		{
			encodedUnbeatenWinStreak & 0xFF,
			(encodedUnbeatenWinStreak >> 8) & 0xFF,
			(encodedUnbeatenWinStreak >> 16) & 0xFF
		};
	}

	private static void UpdateManagerProfile(short usersClubID, short oppClubID, MatchResult result, bool canLevelUp = false)
	{
		Helpers.LogConsole(string.Concat("###TheoBugTest() -- UpdateManagerProfile -- Called with Result == ", result, " -- and canlevelUp == ", canLevelUp.ToString()));
		Game.instance.ManagedClubProfile.PostMatch(result, usersClubID, oppClubID, canLevelUp);
		Club.PostMatch(usersClubID, result);
	}

	public static void AddFixtureClubMessages(Fixture fixture, DateTime fixturetime)
	{
		if (!(fixture != null) || (!(fixturetime == DateTime.MinValue) && (Game.instance.CurrentDate - fixturetime).Days > 14))
		{
			return;
		}
		fixture.RestoreClubsFromSQL();
		List<Message> messages = Game.instance.CurrentManagedClub.messages;
		int count = messages.Count;
		for (int i = 0; i < count; i++)
		{
			if (messages[i].Type == 101)
			{
				return;
			}
		}
		if (fixture.competitiontype != 2)
		{
			return;
		}
		int num = 0;
		int num2 = 1;
		if (fixture.awayClubId == Game.instance.CurrentManagedClubId)
		{
			num = 1;
			num2 = 0;
		}
		byte leaguePosition = LeagueList.GetLeaguePosition(fixture.clubid(num));
		byte leaguePosition2 = LeagueList.GetLeaguePosition(fixture.clubid(num2));
		if (Math.Abs(leaguePosition - leaguePosition2) > 7)
		{
			if (leaguePosition < leaguePosition2 && fixture.score[num] < fixture.score[num2])
			{
				MessageManager.BoardUnhappyWithResult(fixture);
			}
			else if (leaguePosition > leaguePosition2 && fixture.score[num] > fixture.score[num2])
			{
				MessageManager.BoardHappyWithResult(fixture);
			}
		}
	}

	public static TacticsValid ValidateTactics()
	{
		int num = 0;
		bool success = true;
		string reason = "";
		bool flag = false;
		short currentManagedClubId = Game.instance.CurrentManagedClubId;
		int competitionType = -1;
		int competitionID = -1;
		NextFixtureInfo nextFixtureInfo = null;
		if (Game.instance.managedClub.Length != 0)
		{
			nextFixtureInfo = Game.instance.GetNextFixture(currentManagedClubId);
			if (nextFixtureInfo != null)
			{
				competitionType = nextFixtureInfo.fixture.competitiontype;
				competitionID = nextFixtureInfo.fixture.competitionid;
			}
		}
		for (int i = 0; i < Game.instance.CurrentManagedClub.currentTactics.lineUp.Length; i++)
		{
			num = Game.instance.CurrentManagedClub.currentTactics.lineUp[i];
			if (num != 0 && !Player.IsRetired(num) && Game.instance.g_PlayerData.ContainsKey(num))
			{
				if (PlayerData.GetSquadNo(num) == 0)
				{
					flag = true;
				}
				if (Player.IsInjured(num))
				{
					success = false;
					reason = Translation.GetText("tact-submit-inj-suspended-err");
					break;
				}
				if (Player.IsSuspended(num, Player.GetAssociationID(num), competitionType, competitionID))
				{
					success = false;
					reason = Translation.GetText("tact-submit-inj-suspended-err");
					break;
				}
				continue;
			}
			Helpers.ShowToast(Translation.GetText("invalid-lineup"));
			UI.GetInstance().ChangeScreen(9, false);
			success = false;
			reason = "";
			break;
		}
		if (flag)
		{
			reason = "squad-number-not-set";
			success = false;
		}
		return new TacticsValid
		{
			success = success,
			reason = reason
		};
	}

	public static void CheckManagedPlayerAvailability(NextFixtureInfo nextFix)
	{
		PlayerSquad players = Club.GetPlayers(Game.instance.CurrentManagedClubId);
		int count = players.Count;
		int num = 0;
		int num2 = 0;
		int competitionType = -1;
		int competitionID = -1;
		if (nextFix != null)
		{
			competitionType = nextFix.fixture.competitiontype;
			competitionID = nextFix.fixture.competitionid;
		}
		for (int i = 0; i < count; i++)
		{
			int playerID = players[i];
			bool flag = true;
			if (Player.IsInjured(playerID))
			{
				flag = false;
			}
			else if (nextFix != null && Player.IsSuspended(playerID, Game.instance.CurrentManagedClub.AssociationID, competitionType, competitionID))
			{
				flag = false;
			}
			if (flag)
			{
				if (Player.GetPosition(playerID) == 1)
				{
					num2++;
				}
				else
				{
					num++;
				}
			}
		}
		int seed = Game.instance.Random.GetSeed();
		Game.instance.Random.SetSeed((Game.instance.Seed + Game.instance.CurrentDate.GetEpochSeconds()) % 32768);
		bool flag2 = true;
		while (flag2 && num2 == 0)
		{
			flag2 = PlayerRespawn.RespawnPlayerForClub(Game.instance.CurrentManagedClubId, Constants.PlayerPosition.PLAYER_POS_GK);
			num2++;
		}
		while (flag2 && num < 18)
		{
			int num3 = -1;
			do
			{
				num3 = Player.GetRandomPosition();
			}
			while (num3 == 1);
			flag2 = PlayerRespawn.RespawnPlayerForClub(Game.instance.CurrentManagedClubId, (Constants.PlayerPosition)num3);
			num++;
		}
		Game.instance.Random.SetSeed(seed);
	}

	public static string GetMatchOutcome(int homeScore, int awayScore, int homePenScore, int awayPenScore, int homeAway)
	{
		string text = "";
		if (homeScore > awayScore)
		{
			if (homeAway == 0)
			{
				return "w";
			}
			return "l";
		}
		if (awayScore > homeScore)
		{
			if (homeAway == 0)
			{
				return "l";
			}
			return "w";
		}
		if ((homePenScore > 0 || awayPenScore > 0) && homePenScore > awayPenScore)
		{
			if (homeAway == 0)
			{
				return "wp";
			}
			return "lp";
		}
		if ((homePenScore > 0 || awayPenScore > 0) && homePenScore < awayPenScore)
		{
			if (homeAway == 0)
			{
				return "lp";
			}
			return "wp";
		}
		return "d";
	}
}
