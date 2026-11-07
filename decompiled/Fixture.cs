using System;
using System.Collections.Generic;
using System.Linq;

[Serializable]
public class Fixture
{
	public short[] clubId = new short[2];

	public FixtureClub[] fixtureClubs;

	public sbyte[] leg1Score = new sbyte[2];

	public byte[] score = new byte[2];

	public sbyte[] penalty = new sbyte[2];

	public int roundNumber;

	public List<Goal> goal;

	public List<Assist> assist;

	public List<GameEvent> yellow;

	public List<GameEvent> red;

	public List<Injury> injury;

	public List<Sub> sub;

	public DateTime Time { get; set; }

	public int fixtureID { get; set; }

	public bool isPlayed { get; set; }

	public int dateoffset { get; set; }

	public int countryid { get; set; }

	public int competitiontype { get; set; }

	public bool finalCompetitionFixture { get; set; }

	public int competitionid { get; set; }

	public int stadiumid { get; set; }

	public int attendance { get; set; }

	public int manofmatch { get; set; }

	public bool IsPlayed => isPlayed;

	public byte homeScore => score[0];

	public byte awayScore => score[1];

	public byte homePosession => fixtureClubs[0].possession;

	public byte awayPosession => fixtureClubs[1].possession;

	public byte homeShotsOnTarget => fixtureClubs[0].shotsontarget;

	public byte awayShotsOnTarget => fixtureClubs[1].shotsontarget;

	public short homePenalties => penalty[0];

	public short awayPenalties => penalty[1];

	public short homeClubId => clubId[0];

	public short awayClubId => clubId[1];

	public byte homeFormationId => fixtureClubs[0].formationId;

	public byte awayFormationId => fixtureClubs[1].formationId;

	public sbyte homePenalty => penalty[0];

	public sbyte awayPenalty => penalty[1];

	public byte homeActualScore => (byte)(homeScore + Math.Max(homePenalty, (sbyte)0));

	public byte awayActualScore => (byte)(awayScore + Math.Max(awayPenalty, (sbyte)0));

	public FixturePlayer[] tm1 => fixtureClubs[0].playerData;

	public FixturePlayer[] tm2 => fixtureClubs[1].playerData;

	public Fixture()
	{
		fixtureID = -1;
		leg1Score[0] = (leg1Score[1] = -1);
		penalty[0] = (penalty[1] = -1);
	}

	public void InitPreMatch()
	{
		CreateClubs();
		CreateEventLists();
	}

	internal void CreateEventLists()
	{
		injury = new List<Injury>();
		red = new List<GameEvent>();
		yellow = new List<GameEvent>();
		sub = new List<Sub>();
		assist = new List<Assist>();
		goal = new List<Goal>();
	}

	internal void CreateClubs()
	{
		fixtureClubs = new FixtureClub[2];
		fixtureClubs[0] = new FixtureClub();
		fixtureClubs[1] = new FixtureClub();
		fixtureClubs[0].clubId = homeClubId;
		fixtureClubs[1].clubId = awayClubId;
	}

	public Fixture(Fixture fixture)
	{
		dateoffset = fixture.dateoffset;
		competitiontype = fixture.competitiontype;
		competitionid = fixture.competitionid;
		leg1Score = fixture.leg1Score;
		clubId = fixture.clubId;
		stadiumid = fixture.stadiumid;
		attendance = fixture.attendance;
		goal = new List<Goal>(fixture.goal);
		assist = new List<Assist>(fixture.assist);
		yellow = new List<GameEvent>(fixture.yellow);
		red = new List<GameEvent>(fixture.red);
		injury = new List<Injury>(fixture.injury);
		sub = new List<Sub>(fixture.sub);
		manofmatch = fixture.manofmatch;
		clubId = fixture.clubId;
	}

	public static bool operator ==(Fixture f1, Fixture f2)
	{
		return f1?.Equals(f2) ?? ((object)f2 == null);
	}

	public static bool operator !=(Fixture f1, Fixture f2)
	{
		return !(f1 == f2);
	}

	public override bool Equals(object obj)
	{
		if (obj == null || !GetType().Equals(obj.GetType()))
		{
			return false;
		}
		Fixture fixture = (Fixture)obj;
		if (fixture.Time == Time && fixture.homeClubId == homeClubId)
		{
			return fixture.awayClubId == awayClubId;
		}
		return false;
	}

	public override int GetHashCode()
	{
		return homeClubId.GetHashCode() ^ awayClubId.GetHashCode() ^ Time.GetHashCode();
	}

	public int goalsForPlayer(int playerID)
	{
		int num = 0;
		for (int i = 0; i < goal.Count; i++)
		{
			if (goal[i].playerid == playerID)
			{
				num++;
			}
		}
		return num;
	}

	public int MinsForPlayer(int playerID)
	{
		for (int i = 0; i < fixtureClubs.Length; i++)
		{
			FixtureClub fixtureClub = fixtureClubs[i];
			for (int j = 0; j < fixtureClub.playerData.Length; j++)
			{
				if (fixtureClub.playerData[j].playerId == playerID)
				{
					return fixtureClub.playerData[j].TimePlayed;
				}
			}
		}
		return 0;
	}

	public int PerformanceForPlayer(int playerID)
	{
		for (int i = 0; i < fixtureClubs.Length; i++)
		{
			FixtureClub fixtureClub = fixtureClubs[i];
			for (int j = 0; j < fixtureClub.playerData.Length; j++)
			{
				if (fixtureClub.playerData[j].playerId == playerID)
				{
					return fixtureClub.playerData[j].performance;
				}
			}
		}
		return 0;
	}

	public void addAssist(int _playerId, int _time, int _type)
	{
		assist.Add(new Assist
		{
			playerid = _playerId,
			time = _time,
			type = _type
		});
	}

	public void addGoal(Goal _Goal)
	{
		goal.Add(_Goal);
	}

	public int addGoal(int _PlayerId, int _Time, int _Type)
	{
		goal.Add(new Goal
		{
			playerid = _PlayerId,
			time = _Time,
			type = _Type
		});
		return goal.Count - 1;
	}

	public void addInjury(int _playerId, int _time, int _length, int _type)
	{
		injury.Add(new Injury
		{
			playerid = _playerId,
			time = _time,
			length = _length,
			type = _type
		});
	}

	public void addSub(int _playerOffId, int _playerOnId, int _time)
	{
		sub.Add(new Sub
		{
			playeroffid = _playerOffId,
			playeronid = _playerOnId,
			time = _time
		});
	}

	public void addYellow(int _playerId, int _time)
	{
		yellow.Add(new GameEvent
		{
			playerid = _playerId,
			time = _time
		});
	}

	public int countYellowForPlayer(int _playerId)
	{
		int num = 0;
		foreach (GameEvent item in yellow)
		{
			if (item.playerid == _playerId)
			{
				num++;
			}
		}
		return num;
	}

	public void addRed(int _playerId, int _time)
	{
		if (countYellowForPlayer(_playerId) == 2)
		{
			foreach (GameEvent item in yellow)
			{
				if (item.playerid == _playerId && item.time > _time)
				{
					_time = item.time;
				}
			}
		}
		red.Add(new GameEvent
		{
			playerid = _playerId,
			time = _time
		});
	}

	public bool IsForClub(short _clubid)
	{
		if (homeClubId == _clubid || awayClubId == _clubid)
		{
			return true;
		}
		return false;
	}

	public static byte GetFavouritesValue(short clubid)
	{
		byte predictedLeaguePosition = Club.GetPredictedLeaguePosition(clubid);
		byte leaguePosition = LeagueList.GetLeaguePosition(clubid);
		byte averageRating = Club.GetAverageRating(clubid);
		int pointsFromLast6Games = Helpers.GetLeagueFromSeason(Club.GetLeagueDataId(clubid)).GetPointsFromLast6Games(clubid);
		byte b = 0;
		byte b2 = 0;
		byte gamesPlayedCurrentClub = Game.instance.ManagedClubProfile.GetGamesPlayedCurrentClub();
		Dictionary<int, LeagueData> g_LeagueData = Game.instance.g_LeagueData;
		if (g_LeagueData.ContainsKey(Club.GetLeagueDataId(clubid)))
		{
			int numTeams = g_LeagueData[Club.GetLeagueDataId(clubid)].NumTeams;
			for (int i = 0; i < numTeams; i++)
			{
				if (i == predictedLeaguePosition)
				{
					b = (byte)(numTeams - predictedLeaguePosition);
				}
				if (i == leaguePosition && gamesPlayedCurrentClub > 9)
				{
					b2 = ((predictedLeaguePosition <= leaguePosition) ? ((byte)(-leaguePosition + predictedLeaguePosition)) : ((byte)(predictedLeaguePosition - leaguePosition)));
				}
			}
			return (byte)(b2 + averageRating + pointsFromLast6Games + b);
		}
		return 0;
	}

	public static byte GetFavouritesBasedOnValue(short ManagedClubID, short clubID2)
	{
		byte favouritesValue = GetFavouritesValue(ManagedClubID);
		byte favouritesValue2 = GetFavouritesValue(clubID2);
		return (byte)(favouritesValue - favouritesValue2);
	}

	public static bool IsFixtureForClub(Fixture fixture, int clubid)
	{
		if (fixture.clubId[0] != clubid && fixture.clubId[1] != clubid)
		{
			return false;
		}
		return true;
	}

	public static bool IsLeague(Fixture fixture)
	{
		if (fixture.competitiontype != 2)
		{
			return false;
		}
		return true;
	}

	public static bool IsBigMatch(Fixture fixture)
	{
		bool result = false;
		if (fixture.competitiontype == 3)
		{
			if (Game.instance.g_season.GetCupFromID(fixture.competitionid).InCup.Count <= 8)
			{
				result = true;
			}
		}
		else if (fixture.competitiontype == 4)
		{
			ContinentalComp continentalComp = Game.instance.g_continentalCompetitions[fixture.competitionid];
			Cup cup = continentalComp.cup;
			if (continentalComp.Status == ContinentalCompStatus.CupStage && cup.InCup.Count <= 8)
			{
				result = true;
			}
		}
		else if (fixture.competitiontype == 2)
		{
			ChosenLeague leagueFromSeason = Helpers.GetLeagueFromSeason(fixture.competitionid);
			int num = Array.IndexOf(leagueFromSeason.ClubID, fixture.homeClubId);
			int num2 = leagueFromSeason.GamesWon[num] * 3 + leagueFromSeason.GamesDrawn[num];
			int num3 = Array.IndexOf(leagueFromSeason.ClubID, fixture.awayClubId);
			int num4 = leagueFromSeason.GamesWon[num3] * 3 + leagueFromSeason.GamesDrawn[num3];
			if ((leagueFromSeason.GamesPld[num] >= 15 || leagueFromSeason.GamesPld[num3] >= 15) && Math.Abs(num2 - num4) <= 5)
			{
				result = true;
			}
		}
		if (Club.AreRivals(fixture.homeClubId, fixture.awayClubId))
		{
			result = true;
		}
		return result;
	}

	public float GetPlayerShareOfMatch(int playerID)
	{
		if (fixtureClubs == null)
		{
			RestoreClubsFromSQL();
		}
		if (fixtureClubs[0].Contains(playerID))
		{
			return fixtureClubs[0].GetPlayerShareOfMatch(playerID);
		}
		return fixtureClubs[1].GetPlayerShareOfMatch(playerID);
	}

	public void UpdateClubStats(int _HorA, byte _Posession, byte _ShotsOnGoal, byte _ShotsOnTarget, byte _Corners)
	{
		fixtureClubs[_HorA].UpdateStats(_Posession, _ShotsOnGoal, _ShotsOnTarget, _Corners);
	}

	public void setPosession(byte _Home, byte _Away)
	{
		fixtureClubs[0].possession = _Home;
		fixtureClubs[1].possession = _Away;
	}

	public void setClubIds(short _HomeClub, short _AwayClub)
	{
		clubId[0] = _HomeClub;
		clubId[1] = _AwayClub;
		stadiumid = Club.GetStadiumDataID(_HomeClub);
		if (IsForClub(Game.GetCurrentUserClubID()))
		{
			Game.instance.g_season.Schedule.UpdateUnplayedFixtureDateForManagedClub(this);
		}
	}

	public void setPenalties(sbyte _home, sbyte _away)
	{
		penalty[0] = _home;
		penalty[1] = _away;
	}

	public void setleg1Score(sbyte _home, sbyte _away)
	{
		leg1Score[0] = _home;
		leg1Score[1] = _away;
	}

	public void Score(int _Which, byte _Score)
	{
		score[_Which] = _Score;
	}

	public void HomeScore(byte _x)
	{
		score[0] = _x;
	}

	public void AwayScore(byte _x)
	{
		score[1] = _x;
	}

	public void HomeClubId(short _Id)
	{
		clubId[0] = _Id;
	}

	public void AwayClubId(short _Id)
	{
		clubId[1] = _Id;
	}

	public short clubid(int _Which)
	{
		return clubId[_Which];
	}

	public void FormationId(int _Which, byte _formationId)
	{
		fixtureClubs[_Which].formationId = _formationId;
	}

	public void addToSQL()
	{
		fixtureID = Game.instance.sqLiteService.fixtureTbl.AddItem(this);
		fixtureClubs[0].AddToSQL(fixtureID, competitionid);
		fixtureClubs[1].AddToSQL(fixtureID, competitionid);
		Game.instance.sqLiteService.gameGoalEventTbl.Add(goal, fixtureID, competitionid);
		Game.instance.sqLiteService.gameAssistEventTbl.Add(assist, fixtureID, competitionid);
		Game.instance.sqLiteService.gameDisciplinaryEventTbl.Add(yellow, red, fixtureID, competitionid);
		Game.instance.sqLiteService.gameSubEventTbl.Add(sub, fixtureID, competitionid);
		ClearSQL();
	}

	public void RestoreClubsFromSQL()
	{
		if (!IsPlayed || fixtureID <= -1 || fixtureClubs != null)
		{
			return;
		}
		IEnumerable<FixtureClubRec> clubRecsForFixture = Game.instance.sqLiteService.fixtureClubTbl.GetClubRecsForFixture(fixtureID);
		CreateClubs();
		foreach (FixtureClubRec item in clubRecsForFixture)
		{
			if (item.ClubId == homeClubId)
			{
				fixtureClubs[0].RestoreFromSQL(fixtureID, item);
			}
			else
			{
				fixtureClubs[1].RestoreFromSQL(fixtureID, item);
			}
		}
	}

	public void RestoreFromSQL()
	{
		if (IsPlayed && fixtureID > -1)
		{
			RestoreClubsFromSQL();
			if (goal == null)
			{
				CreateEventLists();
				Game.instance.sqLiteService.gameGoalEventTbl.Restore(goal, fixtureID);
				Game.instance.sqLiteService.gameAssistEventTbl.Restore(assist, fixtureID);
				Game.instance.sqLiteService.gameDisciplinaryEventTbl.Restore(yellow, red, fixtureID);
				Game.instance.sqLiteService.gameInjuryEventTbl.Restore(injury, fixtureID);
				Game.instance.sqLiteService.gameSubEventTbl.Restore(sub, fixtureID);
			}
		}
	}

	public void PersistSQL()
	{
		if (fixtureID != -1)
		{
			ClearSQL();
		}
		else if (fixtureID == -1 && IsPlayed)
		{
			addToSQL();
		}
	}

	public void ClearSQL()
	{
		int currentManagedClubId = Game.instance.CurrentManagedClubId;
		if (IsPlayed && fixtureID > -1 && fixtureClubs != null && fixtureClubs[0].clubId != currentManagedClubId && fixtureClubs[1].clubId != currentManagedClubId)
		{
			fixtureClubs = null;
			goal = null;
			assist = null;
			yellow = null;
			red = null;
			injury = null;
			sub = null;
		}
	}

	public void DebugForceResult()
	{
		int currentManagedClubId = Game.instance.CurrentManagedClubId;
		if (homeClubId == currentManagedClubId || awayClubId == currentManagedClubId)
		{
			int num = ((homeClubId != currentManagedClubId) ? 1 : 0);
			int num2 = ((num == 0) ? 1 : 0);
			switch (Game.debugForceResults)
			{
			case Game.DebugForceResults.WIN:
				score[num] = 5;
				score[num2] = 0;
				break;
			case Game.DebugForceResults.LOSE:
				score[num] = 0;
				score[num2] = 5;
				break;
			case Game.DebugForceResults.DRAW:
				score[num] = 5;
				score[num2] = 5;
				break;
			}
		}
	}

	private bool WasPlayerSentOff(int playerId)
	{
		bool result = false;
		foreach (GameEvent item in red)
		{
			if (item.playerid == playerId)
			{
				result = true;
				break;
			}
		}
		return result;
	}

	private bool WasPlayerInjured(int playerId)
	{
		return false;
	}

	public FixtureHelper GetFixtureHelper()
	{
		return FixtureHelperFactory.GetFixtureHelper(this);
	}

	public void PostMatch(DateTime fixturetime)
	{
		short currentManagedClubId = Game.instance.CurrentManagedClubId;
		if (!IsPlayed && (homeClubId == currentManagedClubId || awayClubId == currentManagedClubId))
		{
			setPosession(50, 50);
			int num = ((homeClubId != Game.instance.CurrentManagedClubId) ? 1 : 0);
			Score(num, 5);
			Score(1 - num, 0);
		}
		bool flag = false;
		if (competitiontype == 5)
		{
			flag = true;
			if (homeClubId == Game.instance.CurrentManagedClubId || awayClubId == Game.instance.CurrentManagedClubId)
			{
				if (Club.GetNumberOfGamesPlayedThisSeason(Game.instance.CurrentManagedClubId) == 1)
				{
					NotificationController.CancelScheduledNotificationsByType(201);
					NotificationController.CancelScheduledNotificationsByType(301);
					NotificationController.ScheduleNotification(21600, Game.GameName, Translation.GetText("push-notification-transfer-window-open-2"), 302);
				}
				NextFixtureInfo nextFixture = Game.instance.GetNextFixture(Game.instance.CurrentManagedClubId);
				if (nextFixture != null && nextFixture.fixture.competitiontype == 2)
				{
					string value = ((homeClubId == Game.GetCurrentUserClubID()) ? Club.GetName(awayClubId) : Club.GetName(homeClubId));
					NotificationController.ScheduleNotification(43200, Game.GameName, Translation.TemplateReplace(Translation.GetText("push-notification-season-start"), new Dictionary<string, string> { { "Club Name", value } }), 202);
				}
			}
		}
		else if (competitiontype == 2 && (homeClubId == Game.instance.CurrentManagedClubId || awayClubId == Game.instance.CurrentManagedClubId) && Club.GetNumberOfGamesPlayedThisSeason(Game.instance.CurrentManagedClubId) == 4)
		{
			NotificationController.CancelScheduledNotificationsByType(202);
		}
		int num2 = -1;
		int num3 = -1;
		if (homeClubId == currentManagedClubId)
		{
			num2 = 0;
			num3 = 1;
		}
		else if (awayClubId == currentManagedClubId)
		{
			num2 = 1;
			num3 = 0;
		}
		List<int> list = null;
		short associationID = Club.GetAssociationID(currentManagedClubId);
		if (num2 != -1)
		{
			list = Club.GetPlayers(currentManagedClubId).players;
			if (!(fixturetime < Game.instance.CurrentManagedClub.LatestFixturePlayedDate))
			{
				Game.instance.CurrentManagedClub.LatestFixturePlayedDate = fixturetime;
			}
			List<Message> messages = Game.instance.CurrentManagedClub.messages;
			for (int i = 0; i < messages.Count; i++)
			{
				if (messages[i].Type == 1009)
				{
					messages.RemoveAt(i);
				}
			}
		}
		if (homeClubId == currentManagedClubId || awayClubId == currentManagedClubId)
		{
			FixturePlayer[] array = ((homeClubId == Game.instance.CurrentManagedClubId) ? tm1 : tm2);
			List<int> list2 = new List<int>();
			FixturePlayer[] array2 = array;
			foreach (FixturePlayer fixturePlayer in array2)
			{
				bool flag2 = false;
				for (int k = 0; k < injury.Count; k++)
				{
					if (fixturePlayer.playerId == injury[k].playerid)
					{
						flag2 = true;
					}
				}
				if (Player.GetStatus(fixturePlayer.playerId).fitness < 65 && !flag2)
				{
					list2.Add(fixturePlayer.playerId);
				}
			}
			if (Enumerable.Count(list2) > 0)
			{
				MessageManager.PlayersLowFitness(list2);
			}
		}
		for (int l = 0; l < injury.Count; l++)
		{
			int playerid = injury[l].playerid;
			if (Player.GetAssociationID(playerid) != associationID)
			{
				continue;
			}
			DateTime endDate = fixturetime.AddDays(injury[l].length);
			Game.instance.g_injuredPlayers.AddPlayerInjury(playerid, fixturetime, endDate, injury[l].type);
			if (num2 != -1 && list.Contains(playerid) && (Game.instance.CurrentDate - fixturetime).Minutes < 180)
			{
				MessageManager.PlayerInjuredFromLastMatch(playerid, injury[l], (homeClubId == currentManagedClubId) ? awayClubId : homeClubId);
				if (injury[l].length > 60)
				{
					MessageManager.PlayerLongTermInjured(playerid, injury[l].length, injury[l].type);
				}
				Player.GetStatus(playerid).reduceFitnessForInjury(injury[l].length);
			}
		}
		ReduceMatchBans();
		if (!flag)
		{
			Game.instance.g_recentbans = new List<int>();
			for (int m = 0; m < red.Count; m++)
			{
				int playerid2 = red[m].playerid;
				byte b = 3;
				int num4 = 0;
				for (int n = 0; n < yellow.Count; n++)
				{
					if (yellow[n].playerid == playerid2 && ++num4 >= 2)
					{
						b = 1;
						break;
					}
				}
				if (competitiontype == 4)
				{
					if (Game.instance.g_continentalCompetitions[competitionid].matchban.ContainsKey(playerid2))
					{
						Game.instance.g_continentalCompetitions[competitionid].matchban[playerid2] = b;
					}
					else
					{
						Game.instance.g_continentalCompetitions[competitionid].matchban.Add(playerid2, b);
					}
					Game.instance.g_recentbans.Add(playerid2);
				}
				else
				{
					if (Game.instance.g_association[countryid].MatchBan.ContainsKey(playerid2))
					{
						Game.instance.g_association[countryid].MatchBan[playerid2] = b;
					}
					else
					{
						Game.instance.g_association[countryid].MatchBan.Add(playerid2, b);
					}
					Game.instance.g_recentbans.Add(playerid2);
				}
				if (num2 != -1 && list.IndexOf(playerid2) != -1 && (Game.instance.CurrentDate - fixturetime).Minutes < 180)
				{
					short oppClubID = ((homeClubId == currentManagedClubId) ? awayClubId : homeClubId);
					MessageManager.PlayerBannedFromLastMatch(playerid2, b, competitionid, competitiontype, oppClubID);
				}
			}
		}
		if (num2 != -1 && (Game.instance.CurrentDate - fixturetime).Minutes < 180)
		{
			ManagedClub currentManagedClub = Game.instance.CurrentManagedClub;
			double num6;
			double num7;
			double num8;
			double num9;
			double num10;
			double num11;
			if (homeClubId == currentManagedClubId)
			{
				float num5 = Economy.CalcClubTicketPricePrecise(currentManagedClubId);
				num6 = (double)(((float)attendance - (float)(attendance / 100 * 20)) * num5) * 1.6;
				num7 = (double)((float)attendance * num5) * 0.16 * (double)(1f + Facilities.GetClubzoneRevenueIncrease(30));
				num7 = num7 / 100.0 * (double)FinanceConfig.MerchSplit;
				num8 = (double)((float)attendance * num5) * 0.16 * (double)(1f + Facilities.GetClubzoneRevenueIncrease(32));
				num8 = num8 / 100.0 * (double)FinanceConfig.HospitalitySplit;
				num9 = (double)((float)attendance * num5) * 0.05;
				num10 = 0.0;
				num11 = num6 / 10.0;
			}
			else
			{
				num6 = 0.0;
				num7 = 0.0;
				num8 = 0.0;
				num9 = 0.0;
				num10 = 0.0;
				num11 = 0.0;
			}
			if (competitiontype == 2)
			{
				num10 = Game.instance.CurrentManagedClub.TvMoney;
			}
			else if (competitiontype == 3)
			{
				num6 = (double)(attendance * Economy.CalcClubTicketPrice(homeClubId)) * 0.5;
			}
			else if (competitiontype == 4)
			{
				num6 = 0.0;
			}
			if (currentManagedClub.BalanceSheet != null && currentManagedClub.BalanceSheet[currentManagedClub.CurrentBalanceSheet] != null)
			{
				double num12 = num6 + num10 + num7 + num8 + num9;
				num12 -= num11;
				BalanceSheet balanceSheet = currentManagedClub.BalanceSheet[currentManagedClub.CurrentBalanceSheet];
				balanceSheet.GateReceipts += num6;
				balanceSheet.TVRevenue += num10;
				balanceSheet.Merchandise += num7;
				balanceSheet.Hospitality += num8;
				balanceSheet.OtherIncome += num9;
				balanceSheet.GroundMaintenance += num11;
				currentManagedClub.Balance += num12;
			}
			if (score[num2] > score[num3])
			{
				Game.instance.g_boardConfidencePercentage += Game.instance.Random.GetRand(1, 10);
			}
			else if (score[num2] < score[num3])
			{
				Game.instance.g_boardConfidencePercentage -= Game.instance.Random.GetRand(1, 10);
			}
			Game.instance.g_boardConfidencePercentage = Math.Min(100, Game.instance.g_boardConfidencePercentage);
			Game.instance.g_boardConfidencePercentage = Math.Max(0, Game.instance.g_boardConfidencePercentage);
			if (competitiontype != 5)
			{
				int count = list.Count;
				Constants.MatchOutcome matchOutcome = Constants.MatchOutcome.Drawn;
				if (homeScore != awayScore)
				{
					matchOutcome = (((homeScore <= awayScore) ? (num2 != 1) : (num2 != 0)) ? Constants.MatchOutcome.Lost : Constants.MatchOutcome.Won);
				}
				bool flag3 = IsBigMatch(this);
				List<int> topPlayers = Club.GetTopPlayers(currentManagedClubId, 15);
				for (int num13 = 0; num13 < count; num13++)
				{
					int num14 = list[num13];
					int num15 = ((num2 == 0) ? fixtureClubs[0].TeamPosition(num14) : fixtureClubs[1].TeamPosition(num14));
					bool flag4 = topPlayers.IndexOf(num14) > -1;
					float num16 = 0f;
					int num17 = 0;
					if (num15 > -1)
					{
						num16 = (int)((num2 == 0) ? tm1[num15].performance : tm2[num15].performance);
					}
					if (num16 > 0f)
					{
						num17 = matchOutcome switch
						{
							Constants.MatchOutcome.Won => 1, 
							Constants.MatchOutcome.Drawn => 0, 
							_ => -1, 
						};
					}
					if (flag3)
					{
						if (flag4 && num16 == 0f)
						{
							if (!Player.IsInjured(num14) && !Player.IsSuspended(num14, Game.instance.CurrentManagedClub.AssociationID, competitiontype, competitionid))
							{
								num17 = -1;
							}
						}
						else if (!flag4 && num16 > 0f && matchOutcome != Constants.MatchOutcome.Lost)
						{
							num17 = 1;
						}
					}
					if (num16 > 0f)
					{
						bool flag5 = WasPlayerSentOff(num14);
						bool flag6 = WasPlayerInjured(num14);
						byte morale = Player.GetMorale(list[num13]);
						if (flag5)
						{
							Player.ChangeMorale(list[num13], -2, replayed: true);
						}
						else if (flag6)
						{
							Player.ChangeMorale(list[num13], -1, replayed: true);
						}
						else if (num16 >= 8f)
						{
							if (morale <= 4)
							{
								Player.ChangeMorale(list[num13], 2, replayed: true);
							}
							else
							{
								Player.ChangeMorale(list[num13], 1, replayed: true);
							}
						}
						else if (num16 > 0f && num16 < 6f)
						{
							if (num16 <= 4f)
							{
								Player.ChangeMorale(list[num13], -1, replayed: true);
							}
							else if (Game.instance.Random.GetRand() % 3 == 0)
							{
								Player.ChangeMorale(list[num13], -1, replayed: true);
							}
						}
						else if (num16 == 7f && morale <= 4)
						{
							Player.ChangeMorale(list[num13], 1, replayed: true);
						}
						else if (num17 != 0)
						{
							int num18 = 10;
							if (flag3)
							{
								num18 = ((!flag4) ? 1 : 2);
							}
							if (Game.instance.Random.GetRand() % num18 == 0)
							{
								Player.ChangeMorale(list[num13], num17, replayed: true);
							}
						}
					}
					else if (num17 != 0 && Game.instance.Random.GetRand() % 2 == 0)
					{
						Player.ChangeMorale(list[num13], num17, replayed: true);
					}
				}
			}
		}
		PlayerCareerStats.RecordAllPlayerStats(this);
		isPlayed = true;
		if (IsFixtureForClub(this, Game.GetCurrentUserClubID()))
		{
			PlayerConcernsScheduler.Instance.AddToPriorityQueue(ConcernType.PlayerForm, Game.GetCurrentUserClubID());
			PlayerConcernsScheduler.Instance.AddToPriorityQueue(ConcernType.GameTime, Game.GetCurrentUserClubID());
		}
		if (competitiontype == 3)
		{
			Cup.PostFixture(this, fixturetime);
		}
		else if (competitiontype == 4)
		{
			ContinentalComp.PostFixture(this, fixturetime);
		}
		else if (competitiontype == 7)
		{
			List<Fixture> list3 = null;
			list3 = Game.instance.g_season.Schedule.GetAllPromotionPlayoffUnplayedFixtures(competitionid);
			if (list3.Count > 2)
			{
				foreach (Fixture item in list3)
				{
					if (item.awayClubId == homeClubId && item.homeClubId == awayClubId)
					{
						item.setleg1Score((sbyte)homeScore, (sbyte)awayScore);
						break;
					}
				}
			}
			if (list3.Count == 2)
			{
				list3.RemoveAt(1);
			}
			if (list3.Count == 1)
			{
				short promotionPlayoffAggWinner = ChosenLeague.GetPromotionPlayoffAggWinner(this);
				if (list3[0].homeClubId == 0)
				{
					list3[0].HomeClubId(promotionPlayoffAggWinner);
					list3[0].stadiumid = Club.GetStadiumDataID(promotionPlayoffAggWinner);
				}
				else
				{
					list3[0].AwayClubId(promotionPlayoffAggWinner);
				}
			}
			else if (list3.Count == 0)
			{
				short num19 = 0;
				if (homeScore + homePenalties > awayScore + awayPenalties)
				{
					num19 = homeClubId;
				}
				else
				{
					num19 = ((homeScore + homePenalties >= awayScore + awayPenalties) ? homeClubId : awayClubId);
				}
				LeagueData leagueData = Game.instance.g_LeagueData[competitionid];
				List<string> allSeasonIDsForDate = Association.GetAllSeasonIDsForDate(leagueData.AssociationID, Game.instance.CurrentDate);
				string seasonid = allSeasonIDsForDate[allSeasonIDsForDate.Count - 1];
				SeasonLog seasonLog = Season.CreateOrGetSeasonLog(leagueData.AssociationID, seasonid);
				int num20 = leagueData.NumPromoted - 1;
				seasonLog.Leagues[leagueData.Division - 1].PromotedClubs[num20] = num19;
			}
		}
		else if (competitiontype == 8 && Game.instance.g_season.Schedule.GetRelegationPlayoffUnplayedFixtures(competitionid).Count == 0)
		{
			short relegationPlayoffAggLoser = ChosenLeague.GetRelegationPlayoffAggLoser(this);
			LeagueData leagueData2 = Game.instance.g_LeagueData[competitionid];
			List<string> allSeasonIDsForDate2 = Association.GetAllSeasonIDsForDate(leagueData2.AssociationID, Game.instance.CurrentDate);
			string seasonid2 = allSeasonIDsForDate2[allSeasonIDsForDate2.Count - 1];
			SeasonLog seasonLog2 = Season.CreateOrGetSeasonLog(leagueData2.AssociationID, seasonid2);
			int num21 = leagueData2.NumRelegated - 1;
			seasonLog2.Leagues[leagueData2.Division - 1].RelegatedClubs[num21] = relegationPlayoffAggLoser;
		}
		if (IsFixtureForClub(this, Game.GetCurrentUserClubID()))
		{
			RandomEvent.ClearOldMessages();
		}
	}

	private void ReduceMatchBans()
	{
		for (int i = 0; i < 2; i++)
		{
			PlayerSquad players = PlayerSquad.GetPlayers(clubid(i));
			int count = Game.instance.g_ClubPlayer[clubid(i)].Count;
			if (competitiontype == 4)
			{
				Dictionary<int, int> matchban = Game.instance.g_continentalCompetitions[competitionid].matchban;
				for (int j = 0; j < count; j++)
				{
					int key = players[j];
					if (matchban.ContainsKey(key))
					{
						if (matchban[key] > 0)
						{
							matchban[key]--;
						}
						if (matchban[key] == 0)
						{
							matchban.Remove(key);
						}
					}
				}
			}
			else
			{
				if (competitiontype == 5)
				{
					continue;
				}
				for (int k = 0; k < count; k++)
				{
					int key2 = players[k];
					if (Game.instance.g_association.ContainsKey(countryid) && Game.instance.g_association[countryid].MatchBan.ContainsKey(key2))
					{
						if (Game.instance.g_association[countryid].MatchBan[key2] > 0)
						{
							Game.instance.g_association[countryid].MatchBan[key2]--;
						}
						if (Game.instance.g_association[countryid].MatchBan[key2] == 0)
						{
							Game.instance.g_association[countryid].MatchBan.Remove(key2);
						}
					}
				}
			}
		}
	}

	public void PreFixture(DateTime fixturetime, bool replay = false)
	{
		FixtureHelperFactory.GetFixtureHelper(this);
		short currentUserClubID = Game.GetCurrentUserClubID();
		Club.SquadHealthCheck(homeClubId, this);
		Club.SquadHealthCheck(awayClubId, this);
		int seed = Game.instance.Random.GetSeed();
		int seed2 = (Game.instance.Seed + fixturetime.GetEpochSeconds() + homeClubId + awayClubId) * 1234 % 32768;
		Game.instance.Random.SetSeed(seed2);
		if (!replay)
		{
			bool flag = false;
			int[] array = new int[2];
			int[] array2 = new int[2];
			array[0] = 1;
			array2[0] = 1;
			array[1] = 1;
			array2[1] = 1;
			flag = ((homeClubId != Game.instance.CurrentManagedClubId && awayClubId != Game.instance.CurrentManagedClubId) ? true : false);
			if (competitiontype == 4)
			{
				array[0] = 1;
				array2[0] = 0;
				array[1] = 1;
				array2[1] = 0;
			}
			else if (competitiontype == 5)
			{
				array[0] = 0;
				array2[0] = 2;
				array[1] = 0;
				array2[1] = 2;
			}
			else
			{
				int strength = Team.GetStrength(homeClubId);
				int strength2 = Team.GetStrength(awayClubId);
				if (strength + 60 < strength2)
				{
					array[0] = 0;
					array2[0] = 1;
					array[1] = 1;
					array2[1] = 2;
				}
				else if (strength > strength2 + 60)
				{
					array[0] = 1;
					array2[0] = 2;
					array[1] = 0;
					array2[1] = 1;
				}
			}
			byte averageRating = Club.GetAverageRating(homeClubId);
			byte averageRating2 = Club.GetAverageRating(awayClubId);
			int oppTeamRatingDiff = averageRating - averageRating2;
			int oppTeamRatingDiff2 = averageRating2 - averageRating;
			Team.LoadTeam(Game.instance.g_tm[0], 0, homeClubId, flag, array2[0], array[0], competitiontype, competitionid, oppTeamRatingDiff);
			Team.LoadTeam(Game.instance.g_tm[1], 1, awayClubId, flag, array2[1], array[1], competitiontype, competitionid, oppTeamRatingDiff2);
		}
		int num = 0;
		switch (competitiontype)
		{
		case 2:
		case 7:
		case 8:
			attendance = Economy.CalcMatchAttendance(homeClubId, awayClubId);
			if (competitiontype == 7 || competitiontype == 8)
			{
				attendance += attendance / 8;
			}
			stadiumid = Club.GetStadiumDataID(homeClubId);
			num = StadiumData.GetSize(stadiumid);
			break;
		case 3:
			if (stadiumid != Club.GetStadiumDataID(homeClubId))
			{
				num = StadiumData.GetSize(stadiumid);
				attendance = (int)((double)num * Game.instance.Random.GetRand(0.99, 1.0));
			}
			else
			{
				attendance = Economy.CalcMatchAttendance(homeClubId, awayClubId);
				num = StadiumData.GetSize(stadiumid);
			}
			break;
		case 4:
			attendance = (int)((double)Club.GetAttendance(homeClubId) * 1.4);
			num = StadiumData.GetSize(stadiumid);
			if (stadiumid == Club.GetStadiumDataID(Game.GetCurrentUserClubID()))
			{
				num -= Game.instance.g_BuildingSeats;
			}
			if (Club.GetAttendance(awayClubId) > attendance)
			{
				int num2 = Club.GetAttendance(awayClubId) / 4;
				attendance += num2;
			}
			break;
		case 5:
		{
			byte averageRating3 = Club.GetAverageRating(homeClubId);
			byte averageRating4 = Club.GetAverageRating(awayClubId);
			if (averageRating3 >= 90 && averageRating4 >= 90)
			{
				attendance = Economy.CalcMatchAttendance(homeClubId, awayClubId);
			}
			else if (averageRating4 >= 90)
			{
				attendance = Economy.CalcMatchAttendance(homeClubId, awayClubId) / 3 * 2;
			}
			else if (averageRating4 > averageRating3 + 5)
			{
				attendance = Economy.CalcMatchAttendance(homeClubId, awayClubId) / 2;
			}
			else
			{
				attendance = Economy.CalcMatchAttendance(homeClubId, awayClubId) / 4;
			}
			stadiumid = Club.GetStadiumDataID(homeClubId);
			num = StadiumData.GetSize(stadiumid);
			break;
		}
		default:
			Debug.LogError("Unhandled competition type in calc attendance");
			break;
		}
		attendance += 750;
		attendance -= Game.instance.Random.GetRand() % 1501;
		if (attendance >= num)
		{
			attendance = num;
			attendance -= Game.instance.Random.GetRand() % 51;
		}
		if (attendance < 500)
		{
			attendance = Game.instance.Random.GetRand(500, 600);
		}
		if (homeClubId == currentUserClubID)
		{
			int[] lineup = Game.instance.g_tm[1].lineup;
			foreach (int playerID in lineup)
			{
				if (PlayerData.GetSquadNo(playerID) == 0)
				{
					byte squadNo = Player.SetSquadNo(playerID);
					PlayerData.SetSquadNo(playerID, squadNo);
				}
			}
		}
		else if (awayClubId == currentUserClubID)
		{
			int[] lineup = Game.instance.g_tm[0].lineup;
			foreach (int playerID2 in lineup)
			{
				if (PlayerData.GetSquadNo(playerID2) == 0)
				{
					byte squadNo2 = Player.SetSquadNo(playerID2);
					PlayerData.SetSquadNo(playerID2, squadNo2);
				}
			}
		}
		Game.instance.Random.SetSeed(seed);
	}
}
