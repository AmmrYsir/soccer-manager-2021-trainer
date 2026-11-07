using System;
using System.Collections.Generic;
using System.Linq;

[Serializable]
public class Season
{
	public class SeasonHonours
	{
		public int ClubDataID;

		public int Year;
	}

	public List<ChosenLeague> Leagues = new List<ChosenLeague>();

	public List<Cup> Cups = new List<Cup>();

	public bool Finished;

	public Schedule Schedule = new Schedule();

	public List<int> CountriesBetweenSeasons = new List<int>();

	public List<int> CountriesLeaguesComplete = new List<int>();

	internal SeasonIDStringManager seasonIdStringManager = new SeasonIDStringManager();

	public string SeasonID { get; set; } = "NOT_INITIALISED";

	public Cup GetCupFromID(int compid)
	{
		Cup result = null;
		foreach (Cup cup in Cups)
		{
			if (cup.CupDataID == compid)
			{
				result = cup;
				break;
			}
		}
		return result;
	}

	public Cup GetCupFromOrderIx(int countryid, int orderIx)
	{
		Cup result = null;
		foreach (Cup cup in Cups)
		{
			if (cup.AssocID == countryid && cup.CupOrderIndex == orderIx)
			{
				result = cup;
				break;
			}
		}
		return result;
	}

	internal int FindCurrentSeasonID()
	{
		return FindSeasonID(SeasonID);
	}

	internal int FindSeasonID(string _seasonID)
	{
		return seasonIdStringManager.FindID(_seasonID);
	}

	private void CreateSeasonID(string _seasonId)
	{
		SeasonID = _seasonId;
		seasonIdStringManager.AddEntry(SeasonID);
	}

	public static bool IsFirstSeason(string seasonId = "", int assocId = -1)
	{
		if (assocId == -1)
		{
			assocId = Club.GetAssociationID(Game.GetCurrentUserClubID());
		}
		if (seasonId == "")
		{
			seasonId = Game.instance.g_season.SeasonID;
		}
		return seasonId == Association.GetFirstSeasonDetails(assocId).SeasonID;
	}

	public static string GetNextSeasonID(string seasonid)
	{
		string text = "";
		if (seasonid.IndexOf("/") == -1)
		{
			int num = int.Parse(seasonid);
			num++;
			return text + num;
		}
		int num2 = int.Parse(seasonid.Substring(0, seasonid.IndexOf("/")));
		int num3 = int.Parse(seasonid.Substring(seasonid.IndexOf("/") + 1));
		num2++;
		num3++;
		return text + num2 + "/" + num3;
	}

	public static string GetCurrentSeasonID()
	{
		List<string> allSeasonIDsForDate = Association.GetAllSeasonIDsForDate(Game.GetCurrentUserClubAssociation(), Game.instance.CurrentDate);
		return allSeasonIDsForDate[allSeasonIDsForDate.Count - 1];
	}

	public static FirstFixtureDetails GetFirstFixtureDetails(Season season, int clubID)
	{
		FirstFixtureDetails firstFixtureDetails = new FirstFixtureDetails();
		FixtureSetList schedule = season.Schedule.schedule;
		if (schedule != null)
		{
			int num = schedule.Count();
			for (int i = 0; i < num; i++)
			{
				int count = schedule[i].Fixtures.Count;
				for (int j = 0; j < count; j++)
				{
					Fixture fixture = schedule[i].Fixtures[j];
					if (fixture.competitiontype == 2)
					{
						if (fixture.homeClubId == clubID)
						{
							firstFixtureDetails.Date = schedule[i].Date;
							firstFixtureDetails.Opponent = fixture.awayClubId;
							firstFixtureDetails.Venue = "home";
							firstFixtureDetails.CompetitionID = fixture.competitionid;
							firstFixtureDetails.CompetitionType = fixture.competitiontype;
							return firstFixtureDetails;
						}
						if (fixture.awayClubId == clubID)
						{
							firstFixtureDetails.Date = schedule[i].Date;
							firstFixtureDetails.Opponent = fixture.homeClubId;
							firstFixtureDetails.Venue = "away";
							firstFixtureDetails.CompetitionID = fixture.competitionid;
							firstFixtureDetails.CompetitionType = fixture.competitiontype;
							return firstFixtureDetails;
						}
					}
				}
			}
		}
		return null;
	}

	public static void processNewSeasonManagedClub(string seasonID, Season season, int assocID, DateTime? date)
	{
		if (!date.HasValue)
		{
			date = Game.instance.CurrentDate;
		}
		ManagedClub[] managedClub = Game.instance.managedClub;
		if (managedClub == null || managedClub.Length == 0 || managedClub[Game.instance.g_currUser].ClubID == 0 || managedClub[Game.instance.g_currUser].AssociationID != assocID)
		{
			return;
		}
		short currentManagedClubId = Game.instance.CurrentManagedClubId;
		FirstFixtureDetails firstFixtureDetails = GetFirstFixtureDetails(season, currentManagedClubId);
		if (firstFixtureDetails != null)
		{
			if (firstFixtureDetails.Date <= Game.instance.CurrentManagedClub.LatestFirstFixtureDate)
			{
				return;
			}
			ManagedClub.NewSeason(Game.instance.CurrentManagedClub);
			string text = Translation.TemplateReplace(Translation.GetText("msg-newseason-soon-fixture"), new Dictionary<string, string>
			{
				{
					"Club Name 1",
					Club.GetName(currentManagedClubId)
				},
				{
					"Club Name 2",
					Club.GetName(firstFixtureDetails.Opponent)
				},
				{
					"Date",
					firstFixtureDetails.Date.ToString("dd/MM/yyyy")
				}
			});
			Game.instance.ScheduleMessage(new object[3]
			{
				(short)17,
				text,
				firstFixtureDetails.Opponent
			}, firstFixtureDetails.Date.AddDays(-7.0));
			List<FirstFixtureDetails> importantFixtures = Game.instance.GetImportantFixtures(currentManagedClubId);
			List<FirstFixtureDetails> list = new List<FirstFixtureDetails>();
			list.Add(firstFixtureDetails);
			for (int i = 0; i < importantFixtures.Count; i++)
			{
				list.Add(importantFixtures[i]);
			}
			short[] oppositionIds = new short[3];
			DateTime[] fixtureDates = new DateTime[3];
			string[] venueNames = new string[3] { "", "", "" };
			Game.instance.g_season.GetImportantFixtureMessageData(list, oppositionIds, fixtureDates, venueNames);
			if (Enumerable.Count(list) > 0)
			{
				MessageManager.FixtureListPublished(seasonID, list);
				MessageManager.NewSeasonWorldNewsMessage(list[0].Date);
			}
			Game.instance.CurrentManagedClub.LatestFirstFixtureDate = firstFixtureDetails.Date;
		}
		if (Game.instance.CurrentManagedClub.TvMoney >= 10000)
		{
			Helpers.toNearest(Game.instance.CurrentManagedClub.TvMoney, 10000.0);
		}
		else
		{
			Helpers.toNearest(Game.instance.CurrentManagedClub.TvMoney, 1000.0);
		}
		Game.instance.CurrentManagedClub.DomesticCupPrizeMoney = new Dictionary<int, int>();
		Game.instance.CurrentManagedClub.ContinentalCupPrizeMoney = new Dictionary<int, int>();
		List<Transfer> list2 = Transfers.instance.historyManager.LoanedOutByClub(currentManagedClubId);
		if (Enumerable.Count(list2) > 0)
		{
			MessageManager.LoanedOutPlayers(list2);
		}
	}

	private static string GetImportantFixtureMessageDetails(List<FirstFixtureDetails> importantFixtures)
	{
		string text = "";
		int num = 0;
		int num2 = 3;
		List<int> list = new List<int>();
		for (int i = 0; i < importantFixtures.Count; i++)
		{
			if (list.IndexOf(importantFixtures[i].Opponent) == -1)
			{
				if (num++ > num2)
				{
					break;
				}
				text += Club.GetLink(importantFixtures[i].Opponent);
				text = ((!(importantFixtures[i].Venue == "home")) ? (text + " " + Translation.GetText("away-from-home") + " ") : (text + " " + Translation.GetText("at-home") + " "));
				text = text + " (" + Helpers.FormatShortDateForVersion(importantFixtures[i].Date) + ")";
				text += Environment.NewLine;
				list.Add(importantFixtures[i].Opponent);
			}
		}
		return text;
	}

	public void GetImportantFixtureMessageData(List<FirstFixtureDetails> importantFixtures, short[] oppositionIds, DateTime[] fixtureDates, string[] venueNames)
	{
		for (int i = 0; i < importantFixtures.Count && i < 3; i++)
		{
			oppositionIds[i] = importantFixtures[i].Opponent;
			fixtureDates[i] = importantFixtures[i].Date;
			venueNames[i] = importantFixtures[i].Venue;
		}
	}

	public static string GetFirstFixtureMessageDetails(FirstFixtureDetails fixtureInfo)
	{
		if (fixtureInfo.Venue == "home")
		{
			return Translation.TemplateReplace(Translation.GetText("msg-fixtures-published-1-2-1"), new Dictionary<string, string>
			{
				{
					"Club Name 1",
					Club.GetName(Game.instance.CurrentManagedClubId)
				},
				{
					"Club Name 2",
					Club.GetLink(fixtureInfo.Opponent)
				},
				{
					"Date",
					Helpers.FormatShortDateForVersion(fixtureInfo.Date)
				}
			});
		}
		return Translation.TemplateReplace(Translation.GetText("msg-fixtures-published-1-2-2"), new Dictionary<string, string>
		{
			{
				"Club Name 1",
				Club.GetName(Game.instance.CurrentManagedClubId)
			},
			{
				"Club Name 2",
				Club.GetLink(fixtureInfo.Opponent)
			},
			{
				"Date",
				Helpers.FormatShortDateForVersion(fixtureInfo.Date)
			}
		});
	}

	public static FixtureSet CreateFixtureSetForDate(FixtureSetList schedule, DateTime thedate)
	{
		FixtureSet result = null;
		int num = schedule.Count();
		FixtureSet fixtureSet = null;
		if (num == 0)
		{
			fixtureSet = new FixtureSet();
			fixtureSet.Date = thedate;
			schedule.Add(fixtureSet);
			result = schedule[0];
		}
		else
		{
			bool flag = false;
			for (int i = 0; i < num; i++)
			{
				if (schedule[i].Date == thedate)
				{
					result = schedule[i];
					flag = true;
					break;
				}
				if (schedule[i].Date > thedate)
				{
					flag = true;
					fixtureSet = new FixtureSet();
					fixtureSet.Date = thedate;
					schedule.Insert(i, fixtureSet);
					result = schedule[i];
					break;
				}
			}
			if (!flag)
			{
				fixtureSet = new FixtureSet();
				fixtureSet.Date = thedate;
				schedule.Add(fixtureSet);
				result = schedule[num];
			}
		}
		return result;
	}

	public static bool IsFirstDayOfNewSeason()
	{
		if (Game.instance.CurrentDate.Day == 1)
		{
			LeagueData topDivision = Association.GetTopDivision(Game.GetCurrentUserClubAssociation());
			return Game.instance.CurrentDate.Month == topDivision.SeasonOneStart;
		}
		return false;
	}

	public static void NewSeasonProcess()
	{
		PlayerHiddenAttributes.ProcessAnnual();
		PlayerList.RefreshPlayerCachedRatings();
		PlayerSquad.Refresh();
		Game.instance.g_injuredPlayers.ClearSeasonHistory();
	}

	public static void NewSeason(string seasonID, Season season, int assocID, DateTime startDate, bool useInitialClubIDs)
	{
		int seed = Game.instance.Random.GetSeed();
		Game.instance.Random.SetSeed((Game.instance.Seed + assocID + startDate.GetEpochSeconds()) % 32768);
		season.CreateSeasonID(seasonID);
		season.CountriesBetweenSeasons.Remove(assocID);
		season.CountriesLeaguesComplete.Remove(assocID);
		Game.instance.g_season.Schedule.RemoveFixturesForCountry(assocID);
		Game.instance.g_association[assocID].MatchBan = new Dictionary<int, int>();
		Game.instance.g_association[assocID].MatchBanHistory = new Dictionary<int, MatchBanHistoryArray>();
		int num = -1;
		if (!useInitialClubIDs)
		{
			foreach (ChosenLeague league in season.Leagues)
			{
				if (league.AssocID == assocID && league.Division == 1)
				{
					num = league.ClubIX[0];
					_ = league.ClubID[num];
				}
			}
		}
		Game.instance.g_injuredPlayers = new InjuredPlayers();
		foreach (int key in Game.instance.g_LeagueData.Keys)
		{
			if (Game.instance.g_LeagueData[key].AssociationID == assocID)
			{
				ChosenLeague.Init(key, startDate, useInitialClubIDs, seasonID);
			}
		}
		processNewSeasonManagedClub(seasonID, season, assocID, startDate);
		Game.instance.Random.SetSeed(seed);
	}

	public static FixtureSet FindFixturesForDate(FixtureSetList schedule, DateTime thedate)
	{
		FixtureSet result = null;
		int num = schedule.Count();
		for (int i = 0; i < num; i++)
		{
			if (schedule[i].Date == thedate)
			{
				result = schedule[i];
				break;
			}
			if (schedule[i].Date > thedate)
			{
				break;
			}
		}
		return result;
	}

	public static void ProcessEndOfSeason(int countryID)
	{
		try
		{
			Helpers.calculateTableEntries();
			Season g_season = Game.instance.g_season;
			if (g_season.CountriesBetweenSeasons.Contains(countryID))
			{
				return;
			}
			g_season.CountriesBetweenSeasons.Add(countryID);
			ManagedClub currentUserClub = Game.GetCurrentUserClub(Game.instance.g_currUser);
			List<string> allSeasonIDsForDate = Association.GetAllSeasonIDsForDate(countryID, Game.instance.CurrentDate);
			string text = allSeasonIDsForDate[allSeasonIDsForDate.Count - 1];
			bool flag = false;
			if (currentUserClub != null && countryID == Club.GetAssociationID(currentUserClub.ClubID))
			{
				flag = true;
			}
			int num = 0;
			int num2 = -1;
			for (int i = 0; i < g_season.Leagues.Count; i++)
			{
				if (g_season.Leagues[i].AssocID == countryID)
				{
					if (num2 == -1)
					{
						num2 = i;
					}
					num++;
				}
			}
			int num3 = -1;
			int num4 = 0;
			for (int j = 0; j < g_season.Cups.Count; j++)
			{
				if (g_season.Cups[j].AssocID == countryID)
				{
					if (num3 == -1)
					{
						num3 = j;
					}
					num4++;
				}
			}
			SeasonLog seasonLog = null;
			int num5 = num2;
			int num6 = 0;
			while (num5 < num2 + num)
			{
				ChosenLeague chosenLeague = g_season.Leagues[num5];
				if (chosenLeague == null)
				{
					Debug.LogError("** OLD LEAGUE UNDEFINED **");
					throw new Exception("** OLD LEAGUE UNDEFINED **");
				}
				LeagueData league = LeagueData.GetLeague(chosenLeague.Division, countryID);
				byte division = chosenLeague.Division;
				seasonLog = CreateOrGetSeasonLog(countryID, text);
				if (num5 == num2)
				{
					for (int k = 0; k < 8; k++)
					{
						seasonLog.Leagues[num6].PromotedClubs[k] = chosenLeague.ClubID[chosenLeague.ClubIX[k]];
					}
				}
				seasonLog.Leagues[num6].AllLeaguePositions = new List<short>();
				for (int l = 0; l < chosenLeague.NumClubs; l++)
				{
					seasonLog.Leagues[num6].AllLeaguePositions.Add(chosenLeague.ClubID[chosenLeague.ClubIX[l]]);
				}
				if (division + 1 <= num)
				{
					ChosenLeague chosenLeague2 = g_season.Leagues[num5 + 1];
					if (chosenLeague2 == null)
					{
						Debug.LogError("** OLD LEAGUE BELOW UNDEFINED **");
						throw new Exception("** OLD LEAGUE BELOW UNDEFINED **");
					}
					byte b = league.NumRelegated;
					if (b == -1)
					{
						b = 3;
					}
					short currentUserClubID = Game.GetCurrentUserClubID();
					PlayerSquad players = Club.GetPlayers(currentUserClubID);
					int num7 = chosenLeague.NumClubs - b;
					int num8 = 0;
					while (num7 < chosenLeague.NumClubs)
					{
						byte b2 = chosenLeague.ClubIX[num7];
						short num9 = chosenLeague.ClubID[b2];
						byte b3 = chosenLeague2.ClubIX[num8];
						short num10 = chosenLeague2.ClubID[b3];
						seasonLog.Leagues[num6].RelegatedClubs[num8] = num9;
						if (flag && currentUserClubID == num9)
						{
							MessageManager.CompsRelegated();
							players = Club.GetPlayers(currentUserClubID);
							int count = players.Count;
							for (int m = 0; m < count; m++)
							{
								if (Game.instance.Random.GetRand() % 2 == 0)
								{
									Player.ChangeMorale(players[m], -1, replayed: true);
								}
								else
								{
									Player.ChangeMorale(players[m], -2, replayed: true);
								}
							}
						}
						if (seasonLog.Leagues[num6 + 1].PromotedClubs[num8] == 0)
						{
							seasonLog.Leagues[num6 + 1].PromotedClubs[num8] = num10;
						}
						else
						{
							num10 = seasonLog.Leagues[num6 + 1].PromotedClubs[num8];
						}
						if (flag && currentUserClubID == num10)
						{
							MessageManager.CompsPromoted();
							SteamAchievementsController.GetInstance().CheckAchievement(SteamAchievementsController.AchievementEnum.ACHIEVEMENT_1_5, isRequirementsMet: true);
							players = Club.GetPlayers(currentUserClubID);
							int count = players.Count;
							for (int m = 0; m < count; m++)
							{
								if (Game.instance.Random.GetRand() % 2 == 0)
								{
									Player.ChangeMorale(players[m], 1, replayed: true);
								}
								else
								{
									Player.ChangeMorale(players[m], 2, replayed: true);
								}
							}
						}
						num7++;
						num8++;
					}
				}
				num5++;
				num6++;
			}
			if (g_season.Cups.Count > 0 && num3 != -1 && g_season.Cups[num3].InCup.Count == 1)
			{
				for (int n = 0; n < num4; n++)
				{
					if (g_season.Cups[num3 + n].InCup.Count == 1)
					{
						seasonLog.CupWinners.Add(g_season.Cups[num3 + n].InCup[0]);
						if (g_season.Cups[num3 + n].InCup[0] == currentUserClub.ClubID)
						{
							Game.instance.NumCupsWonThisSave++;
						}
					}
				}
			}
			HonoursHelper honoursHelper = new HonoursHelper();
			for (int num11 = 0; num11 < num; num11++)
			{
				LeagueData league2 = LeagueData.GetLeague(num11 + 1, countryID);
				if (league2 != null)
				{
					Game.LogConsole("ADDING HONOURS FOR LEAGUE " + league2.Name + " YEAR " + text);
					honoursHelper.AddWinnerToHonours(league2, seasonLog.Leagues[num11].PromotedClubs[0], text);
				}
			}
			if (!flag)
			{
				return;
			}
			DateTime nextSeasonAnnouncementDate = Association.GetSeasonStartDate(countryID, text).AddMonths(13);
			byte leaguePosition = LeagueList.GetLeaguePosition(currentUserClub.ClubID);
			int prizeMoney = Economy.AwardPrizeMoney4League(leaguePosition);
			int leagueDataId = Club.GetLeagueDataId(currentUserClub.ClubID);
			short currentTopClub = ChosenLeague.GetCurrentTopClub(leagueDataId);
			MessageManager.CompsWonLeague(currentTopClub, leagueDataId);
			PlayerSquad players2 = Club.GetPlayers(currentUserClub.ClubID);
			if (leaguePosition == 1)
			{
				players2 = Club.GetPlayers(currentUserClub.ClubID);
				int count2 = players2.Count;
				for (int num12 = 0; num12 < count2; num12++)
				{
					if (Game.instance.Random.GetRand() % 2 == 0)
					{
						Player.ChangeMorale(players2[num12], 2, replayed: true);
					}
					else
					{
						Player.ChangeMorale(players2[num12], 3, replayed: true);
					}
				}
				Game.instance.NumLeaguesWonThisSave++;
				Dictionary<string, string> parameters = new Dictionary<string, string> { 
				{
					"leagueName",
					EventLoggingParam.GetLeagueNameParamValue()
				} };
				EventLoggingController.LogEvent("leaguewon", parameters);
				SteamAchievementsController.GetInstance().CheckTopDivisionAchievement(currentTopClub, currentUserClub.ClubID, leagueDataId);
				Game.instance.ManagedClubProfile.AddLeagueTrophy(leagueDataId);
				Game.instance.ManagedClubProfile.CurrentClubConfidence.CalculateWinCompBoardConfidence(2);
			}
			SteamAchievementsController.GetInstance().CheckEndOfSeasonAchievements(leagueDataId);
			MessageManager.SeasonFinished(text, prizeMoney, leaguePosition, nextSeasonAnnouncementDate);
		}
		catch (Exception ex)
		{
			Game.instance.processEndOfSeasonErrorLogMessage = ex.Message + " " + ex.StackTrace;
			Game.instance.showErrorReportBugModal = true;
			Debug.LogError("SEASON_processEndOfSeason ERROR: " + ex.Message);
		}
	}

	public static void ProcessLeaguePlayoffs(int countryID, bool getPastFixtures = false)
	{
		Season g_season = Game.instance.g_season;
		int num = 0;
		int num2 = -1;
		for (int i = 0; i < g_season.Leagues.Count; i++)
		{
			if (g_season.Leagues[i].AssocID == countryID)
			{
				if (num2 == -1)
				{
					num2 = i;
				}
				num++;
			}
		}
		int num3 = num2;
		int num4 = 0;
		while (num3 < num2 + num)
		{
			LeagueData league = LeagueData.GetLeague(g_season.Leagues[num3].Division, countryID);
			if (league.NumPlayoffPromoted == 4)
			{
				List<short> promotionPlayoffTeams = ChosenLeague.GetPromotionPlayoffTeams(league.LeagueID);
				List<Fixture> list = (getPastFixtures ? g_season.Schedule.GetAllPromotionPlayoffUnplayedFixtures(league.LeagueID) : g_season.Schedule.GetPromotionPlayoffUnplayedFixtures(league.LeagueID));
				if (list.Count == 4)
				{
					list[0].setClubIds(promotionPlayoffTeams[0], promotionPlayoffTeams[3]);
					list[1].setClubIds(promotionPlayoffTeams[1], promotionPlayoffTeams[2]);
					list[2].setClubIds(promotionPlayoffTeams[3], promotionPlayoffTeams[0]);
					list[3].setClubIds(promotionPlayoffTeams[2], promotionPlayoffTeams[1]);
					Fixture fixture = new Fixture
					{
						countryid = league.AssociationID,
						dateoffset = list[0].dateoffset + 1,
						competitiontype = 7,
						competitionid = league.LeagueID,
						Time = list[0].Time.AddDays(4.0).AddHours(-list[0].Time.Hour)
					};
					fixture.CreateEventLists();
					if (fixture.Time.DayOfWeek != DayOfWeek.Saturday && fixture.Time.DayOfWeek != DayOfWeek.Sunday)
					{
						fixture.Time = fixture.Time.AddHours(20.0);
					}
					else
					{
						fixture.Time = fixture.Time.AddHours(15.0);
					}
					g_season.Schedule.AddFixture(fixture, fixture.Time, forceInsert: true);
				}
			}
			if (league.NumPlayoffRelegated == 2)
			{
				List<short> relegationPlayoffTeams = ChosenLeague.GetRelegationPlayoffTeams(league.LeagueID);
				List<Fixture> relegationPlayoffFixturesForLeague = g_season.Schedule.GetRelegationPlayoffFixturesForLeague(league.LeagueID);
				if (relegationPlayoffFixturesForLeague.Count == 2)
				{
					relegationPlayoffFixturesForLeague[0].setClubIds(relegationPlayoffTeams[0], relegationPlayoffTeams[1]);
					relegationPlayoffFixturesForLeague[1].setClubIds(relegationPlayoffTeams[1], relegationPlayoffTeams[0]);
				}
			}
			num3++;
			num4++;
		}
		if (!g_season.CountriesLeaguesComplete.Contains(countryID))
		{
			g_season.CountriesLeaguesComplete.Add(countryID);
		}
	}

	public static void ProcessSeasonLog(int countryID, SeasonLog seasonLog)
	{
		if (!IsSeasonLogValid(countryID, seasonLog))
		{
			Debug.LogError("ProcessSeasonLog() SeasonLog is invalid for country " + countryID + ". Autofix was run");
			FixBrokenSeasonLog(countryID, seasonLog);
		}
		_ = seasonLog.Leagues.Count;
		int num = 0;
		int num2 = -1;
		for (int i = 0; i < Game.instance.g_season.Leagues.Count; i++)
		{
			if (Game.instance.g_season.Leagues[i].AssocID == countryID)
			{
				num = i;
				num2++;
			}
		}
		Season g_season = Game.instance.g_season;
		Dictionary<short, Club> g_ClubData = Game.instance.g_ClubData;
		while (num2 > 0 && num > 0)
		{
			PromotionLog promotionLog = seasonLog.Leagues[num2];
			PromotionLog promotionLog2 = seasonLog.Leagues[num2 - 1];
			int index = num;
			byte numClubs = g_season.Leagues[index].NumClubs;
			short[] clubID = g_season.Leagues[index].ClubID;
			int index2 = num - 1;
			byte numClubs2 = g_season.Leagues[index2].NumClubs;
			short[] clubID2 = g_season.Leagues[index2].ClubID;
			for (int j = 0; j < 4; j++)
			{
				short num3 = promotionLog.PromotedClubs[j];
				short num4 = promotionLog2.RelegatedClubs[j];
				if ((num3 == 0 && num4 != 0) || (num3 != 0 && num4 == 0))
				{
					Debug.LogError("ProcessSeasonLog() - promoted/relegated club was set without a valid club to swap with. Problem with end of season code / wiki data? Promoted club: " + num3 + " Relegated club: " + num4);
				}
				else
				{
					if (num3 == 0)
					{
						continue;
					}
					for (int k = 0; k < numClubs; k++)
					{
						if (clubID[k] == num3)
						{
							clubID[k] = num4;
							g_ClubData[num4].LeagueID = g_season.Leagues[index].LeagueDataID;
							break;
						}
					}
					for (int l = 0; l < numClubs2; l++)
					{
						if (clubID2[l] == num4)
						{
							clubID2[l] = num3;
							g_ClubData[num3].LeagueID = g_season.Leagues[index2].LeagueDataID;
							break;
						}
					}
					if (num4 == Game.GetCurrentUserClubID())
					{
						Game.instance.NumTimesRelegatedThisSave++;
						Dictionary<string, string> parameters = new Dictionary<string, string> { 
						{
							"leagueName",
							EventLoggingParam.GetLeagueNameParamValue()
						} };
						EventLoggingController.LogEvent("relegated", parameters);
					}
					if (num3 == Game.GetCurrentUserClubID())
					{
						Game.instance.NumTimesPromotedThisSave++;
						Dictionary<string, string> parameters2 = new Dictionary<string, string> { 
						{
							"leagueName",
							EventLoggingParam.GetLeagueNameParamValue()
						} };
						EventLoggingController.LogEvent("promoted", parameters2);
					}
				}
			}
			num2--;
			num--;
		}
		for (int m = 0; m < Game.instance.g_season.Leagues.Count; m++)
		{
			ChosenLeague chosenLeague = Game.instance.g_season.Leagues[m];
			if (chosenLeague.AssocID == countryID)
			{
				Game.instance.g_LeagueData[chosenLeague.LeagueDataID].ClubIDs = chosenLeague.ClubID.Where((short x) => x != 0).ToList();
			}
		}
	}

	private static bool IsSeasonLogValid(int countryID, SeasonLog seasonLog)
	{
		if (!IsSeasonLogCupsValid(countryID, seasonLog) || !IsSeasonLogLeaguesValid(countryID, seasonLog))
		{
			return false;
		}
		return true;
	}

	private static bool IsSeasonLogCupsValid(int countryID, SeasonLog seasonLog)
	{
		List<Cup> allCups = Association.GetAllCups(countryID);
		int count = seasonLog.CupWinners.Where((short x) => x != 0).ToList().Count;
		int count2 = seasonLog.CupRunnersUp.Where((KeyValuePair<int, short> x) => x.Value != 0).ToList().Count;
		if (allCups.Count != count || allCups.Count != count2)
		{
			return false;
		}
		return true;
	}

	private static bool IsSeasonLogLeaguesValid(int countryID, SeasonLog seasonLog)
	{
		foreach (PromotionLog league in seasonLog.Leagues)
		{
			if (league.AllLeaguePositions.Count == 0 || league.PromotedClubs[0] == 0)
			{
				return false;
			}
		}
		return true;
	}

	public static void ProcessContinentalSeasonLog()
	{
		if (!IsContinentalSeasonLogValid())
		{
			Debug.LogError("ProcessContinentalSeasonLog() is invalid. Autofix was run.");
			FixBrokenContinentalSeasonLog();
		}
	}

	private static bool IsContinentalSeasonLogValid()
	{
		bool flag = true;
		foreach (int key in Game.instance.g_continentalCompetitions.Keys)
		{
			flag = flag && IsContinentalSeasonLogValid(key);
		}
		return flag;
	}

	private static bool IsContinentalSeasonLogValid(int continentalCompId)
	{
		if (ContinentalSeasonLogExists(continentalCompId))
		{
			List<ContinentalSeasonLogDictionary> g_continental_seasonlog = Game.instance.g_continental_seasonlog;
			string previousSeasonID = GetPreviousSeasonID(GetCurrentSeasonID());
			if (g_continental_seasonlog[continentalCompId].ContainsKey(previousSeasonID) && g_continental_seasonlog[continentalCompId][previousSeasonID] != 0)
			{
				return true;
			}
		}
		return false;
	}

	private static bool ContinentalSeasonLogExists(int continentalCompId)
	{
		return Game.instance.g_continental_seasonlog[continentalCompId] != null;
	}

	private static void FixBrokenSeasonLog(int countryID, SeasonLog seasonLog)
	{
		if (!IsSeasonLogCupsValid(countryID, seasonLog))
		{
			FixBrokenSeasonLogCups(countryID, seasonLog);
		}
		if (!IsSeasonLogLeaguesValid(countryID, seasonLog))
		{
			FixBrokenSeasonLogLeagues(countryID, seasonLog);
		}
	}

	private static void FixBrokenSeasonLogCups(int countryID, SeasonLog seasonLog)
	{
		List<Cup> allCups = Association.GetAllCups(countryID);
		ChosenLeague chosenLeague = Association.GetAllLeagues(countryID)[0];
		seasonLog.CupWinners.Clear();
		foreach (Cup item in allCups)
		{
			seasonLog.CupWinners.Add(chosenLeague.GetClubAtPos(0));
			seasonLog.CupRunnersUp[item.CupDataID] = chosenLeague.GetClubAtPos(1);
		}
	}

	private static void FixBrokenSeasonLogLeagues(int countryID, SeasonLog seasonLog)
	{
		List<ChosenLeague> allLeagues = Association.GetAllLeagues(countryID);
		for (int i = 0; i < seasonLog.Leagues.Count; i++)
		{
			PromotionLog promotionLog = seasonLog.Leagues[i];
			ChosenLeague chosenLeague = allLeagues[i];
			LeagueData league = LeagueData.GetLeague(i + 1, countryID);
			int num = league.NumPromoted;
			int numRelegated = league.NumRelegated;
			if (promotionLog.AllLeaguePositions.Count == 0)
			{
				for (int j = 0; j < chosenLeague.NumClubs; j++)
				{
					promotionLog.AllLeaguePositions.Add(chosenLeague.GetClubAtPos(j));
				}
			}
			if (promotionLog.PromotedClubs[0] == 0)
			{
				if (chosenLeague.Division == 1)
				{
					num = 8;
				}
				for (int k = 0; k < num; k++)
				{
					promotionLog.PromotedClubs[k] = chosenLeague.GetClubAtPos(k);
				}
				for (int l = 0; l < numRelegated; l++)
				{
					promotionLog.RelegatedClubs[l] = chosenLeague.GetClubAtPos(chosenLeague.NumClubs - 1 - l);
				}
			}
		}
	}

	private static void FixBrokenContinentalSeasonLog()
	{
		foreach (int key in Game.instance.g_continentalCompetitions.Keys)
		{
			if (!IsContinentalSeasonLogValid(key))
			{
				FixBrokenContinentalSeasonLog(key);
			}
		}
	}

	private static void FixBrokenContinentalSeasonLog(int continentalCompId)
	{
		List<ContinentalSeasonLogDictionary> g_continental_seasonlog = Game.instance.g_continental_seasonlog;
		string previousSeasonID = GetPreviousSeasonID(GetCurrentSeasonID());
		if (!ContinentalSeasonLogExists(continentalCompId))
		{
			g_continental_seasonlog[continentalCompId] = new ContinentalSeasonLogDictionary();
		}
		if (!g_continental_seasonlog[continentalCompId].ContainsKey(previousSeasonID))
		{
			g_continental_seasonlog[continentalCompId].Add(previousSeasonID, 0);
		}
		if (g_continental_seasonlog[continentalCompId][previousSeasonID] == 0)
		{
			ChosenLeague chosenLeague = Association.GetAllLeagues(Game.GetCurrentUserClubAssociation())[0];
			short num = 0;
			short num2 = 0;
			List<short> seasonLogContinentalWinnersInSeason = GetSeasonLogContinentalWinnersInSeason(previousSeasonID);
			do
			{
				num = chosenLeague.GetClubAtPos(num2++);
			}
			while (seasonLogContinentalWinnersInSeason.Contains(num));
			g_continental_seasonlog[continentalCompId][previousSeasonID] = num;
		}
	}

	private static List<short> GetSeasonLogContinentalWinnersInSeason(string seasonId)
	{
		List<short> list = new List<short>();
		List<ContinentalSeasonLogDictionary> g_continental_seasonlog = Game.instance.g_continental_seasonlog;
		foreach (int key in Game.instance.g_continentalCompetitions.Keys)
		{
			if (ContinentalSeasonLogExists(key) && g_continental_seasonlog[key].ContainsKey(seasonId))
			{
				list.Add(g_continental_seasonlog[key][seasonId]);
			}
		}
		return list;
	}

	public static SeasonLog CreateOrGetSeasonLog(int associd, string seasonid)
	{
		if (!Game.instance.g_association_seasonlog.ContainsKey(associd))
		{
			Game.instance.g_association_seasonlog.Add(associd, new AssociationSeasonLogList());
		}
		AssociationSeasonLogList associationSeasonLogList = Game.instance.g_association_seasonlog[associd];
		SeasonLog seasonLog = null;
		for (int i = 0; i < associationSeasonLogList.Count(); i++)
		{
			if (associationSeasonLogList[i].SeasonID == seasonid)
			{
				seasonLog = associationSeasonLogList[i];
				break;
			}
		}
		if (seasonLog == null)
		{
			seasonLog = new SeasonLog(associd, seasonid);
			associationSeasonLogList.Add(seasonLog);
		}
		return seasonLog;
	}

	public static bool SeasonLogExists(int associd, string seasonid)
	{
		bool result = false;
		Dictionary<int, AssociationSeasonLogList> g_association_seasonlog = Game.instance.g_association_seasonlog;
		if (g_association_seasonlog.ContainsKey(associd) && g_association_seasonlog[associd] != null)
		{
			for (int i = 0; i < g_association_seasonlog[associd].Count(); i++)
			{
				if (g_association_seasonlog[associd] == null)
				{
					break;
				}
				if (g_association_seasonlog[associd][i] != null && g_association_seasonlog[associd][i].SeasonID == seasonid)
				{
					result = true;
					break;
				}
			}
		}
		return result;
	}

	public static SeasonLog GetSeasonLog(int associd, string seasonid)
	{
		SeasonLog result = null;
		Dictionary<int, AssociationSeasonLogList> g_association_seasonlog = Game.instance.g_association_seasonlog;
		if (g_association_seasonlog.ContainsKey(associd) && g_association_seasonlog[associd] != null)
		{
			for (int i = 0; i < g_association_seasonlog[associd].Count(); i++)
			{
				if (g_association_seasonlog[associd] == null)
				{
					break;
				}
				if (g_association_seasonlog[associd][i] != null && g_association_seasonlog[associd][i].SeasonID == seasonid)
				{
					result = g_association_seasonlog[associd][i];
					break;
				}
			}
		}
		return result;
	}

	public static int GetStartYear(string seasonid)
	{
		if (seasonid.IndexOf("/") == -1)
		{
			return int.Parse(seasonid);
		}
		return int.Parse(seasonid.Substring(0, seasonid.IndexOf("/")));
	}

	public static int GetEndYear(string seasonid)
	{
		if (seasonid.IndexOf("/") == -1)
		{
			return int.Parse(seasonid);
		}
		return int.Parse(seasonid.Substring(seasonid.IndexOf("/") + 1));
	}

	public static string GetPreviousSeasonID(string seasonid)
	{
		string text = "";
		if (seasonid.IndexOf("/") == -1)
		{
			int num = int.Parse(seasonid);
			num--;
			return text + num;
		}
		int num2 = int.Parse(seasonid.Substring(0, seasonid.IndexOf("/")));
		int num3 = int.Parse(seasonid.Substring(seasonid.IndexOf("/") + 1));
		num2--;
		num3--;
		return text + num2 + "/" + num3;
	}

	public static DateTime GetNextUnplayedFixtureDate()
	{
		FixtureSetList schedule = Game.instance.g_season.Schedule.schedule;
		int num = schedule.Count();
		DateTime dateTime = DateTime.MinValue;
		for (int i = 0; i < num; i++)
		{
			if (!(dateTime == DateTime.MinValue))
			{
				break;
			}
			int count = schedule[i].Fixtures.Count;
			for (int j = 0; j < count; j++)
			{
				if (!schedule[i].Fixtures[j].IsPlayed)
				{
					dateTime = schedule[i].Date;
					break;
				}
			}
		}
		return dateTime;
	}

	public static bool HasRemainingFixturesThisSeason()
	{
		List<FixtureSet> list = Game.instance.g_season.Schedule.schedule.List;
		if (list.Count > 0)
		{
			foreach (Fixture fixture in list[list.Count - 1].Fixtures)
			{
				if (!fixture.IsPlayed)
				{
					return true;
				}
			}
			return false;
		}
		return false;
	}

	public static List<SeasonHonours> GetPastLeagueWinners(int associd, int division)
	{
		List<SeasonHonours> list = new List<SeasonHonours>();
		division--;
		DateTime currentDate = Game.instance.CurrentDate;
		List<string> allSeasonIDsForDate = Association.GetAllSeasonIDsForDate(associd, currentDate);
		int count = allSeasonIDsForDate.Count;
		for (int i = 0; i < count; i++)
		{
			string text = allSeasonIDsForDate[i];
			if (SeasonLogExists(associd, text))
			{
				short clubDataID = CreateOrGetSeasonLog(associd, text).Leagues[division].PromotedClubs[0];
				int num = text.IndexOf("/");
				if (num != -1)
				{
					text = text.Substring(num + 1);
				}
				SeasonHonours seasonHonours = new SeasonHonours();
				seasonHonours.ClubDataID = clubDataID;
				seasonHonours.Year = int.Parse(text);
				list.Add(seasonHonours);
			}
		}
		return list;
	}
}
