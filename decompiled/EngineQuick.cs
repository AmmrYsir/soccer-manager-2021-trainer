using System;
using System.Collections.Generic;

public class EngineQuick
{
	private const int DEFENDERSHOTCHANCE = 1;

	private const int MIDFIELDERSHOTCHANCE = 5;

	private const int ATTACKERSHOTCHANCE = 50;

	private const int DEFENDERASSISTCHANCE = 10;

	private const int MIDFIELDERASSISTCHANCE = 50;

	private const int ATTACKERASSISTCHANCE = 35;

	public static bool IsTimeUsed(int eq_event_type, int eventtype)
	{
		if (eq_event_type == 0)
		{
			return false;
		}
		if ((eq_event_type & 1) != 0 || (eq_event_type & 2) != 0)
		{
			_ = 8;
			return true;
		}
		return true;
	}

	public static bool IsPlyOnPitch(int teamix, int plyix, int time)
	{
		List<EngineQuick_TmStats> g_eq_tmstats = Game.instance.g_eq_tmstats;
		if (g_eq_tmstats[teamix].timestart[plyix] == -1)
		{
			return false;
		}
		if (time >= g_eq_tmstats[teamix].timestart[plyix] && time <= g_eq_tmstats[teamix].timefinnish[plyix])
		{
			return true;
		}
		return false;
	}

	public static int GetGoalScorer(Team tm, int time, int whichSide)
	{
		int num = tm.lineup.Length;
		int num2 = 0;
		List<int> list = new List<int>();
		for (int i = 0; i < num; i++)
		{
			int num3 = 0;
			if (IsPlyOnPitch(whichSide, i, time))
			{
				switch (Player.PrimaryRole(tm.lineup[i]))
				{
				case ePlayerPosition.DEFENDER:
					num3 = 1;
					break;
				case ePlayerPosition.MIDFIELDER:
					num3 = 5;
					break;
				case ePlayerPosition.ATTACKER:
					num3 = 50;
					break;
				}
				num2 += tm.plyrating[i] * num3;
				list.Add(num2);
			}
		}
		int num4 = Game.instance.Random.GetRand() % num2;
		int result = 0;
		for (int j = 0; j < num; j++)
		{
			if (num4 < list[j])
			{
				result = j;
				break;
			}
		}
		return result;
	}

	public static int GetGoalAssist(Team tm, int scorer, int time, int whichSide)
	{
		int num = tm.lineup.Length;
		int num2 = 0;
		List<int> list = new List<int>();
		for (int i = 0; i < num; i++)
		{
			int num3 = 0;
			if (i != scorer && IsPlyOnPitch(whichSide, i, time))
			{
				switch (Player.PrimaryRole(tm.lineup[i]))
				{
				case ePlayerPosition.DEFENDER:
					num3 = 10;
					break;
				case ePlayerPosition.MIDFIELDER:
					num3 = 50;
					break;
				case ePlayerPosition.ATTACKER:
					num3 = 35;
					break;
				}
				num2 += tm.plyrating[i] * num3;
				list.Add(num2);
			}
		}
		int num4 = Game.instance.Random.GetRand() % num2;
		int result = 0;
		for (int j = 0; j < num; j++)
		{
			if (num4 < list[j])
			{
				result = j;
				break;
			}
		}
		return result;
	}

	public static int GetOwnGoalScorer(Team tm)
	{
		int[] array = new int[18];
		int num = 0;
		int num2 = tm.lineup.Length;
		for (int i = 0; i < num2; i++)
		{
			if (num >= array.Length)
			{
				break;
			}
			if ((PlayerData.GetPosition(tm.lineup[i]) & 1) != 0)
			{
				array[num++] = i;
			}
		}
		for (int j = 0; j < num2; j++)
		{
			if (num >= array.Length)
			{
				break;
			}
			if ((PlayerData.GetPosition(tm.lineup[j]) & 0xE) != 0)
			{
				array[num++] = j;
			}
		}
		for (int k = 0; k < num2; k++)
		{
			if (num >= array.Length)
			{
				break;
			}
			if ((PlayerData.GetPosition(tm.lineup[k]) & 0x1FF0) != 0)
			{
				array[num++] = k;
			}
		}
		for (int l = 0; l < num2; l++)
		{
			if (num >= array.Length)
			{
				break;
			}
			if ((PlayerData.GetPosition(tm.lineup[l]) & 0xE000) != 0)
			{
				array[num++] = l;
			}
		}
		return array[Game.instance.Random.GetRand() % (Game.instance.Random.GetRand() % num2 + 1)];
	}

	public static int FindMoM()
	{
		List<EngineQuick_TmStats> g_eq_tmstats = Game.instance.g_eq_tmstats;
		int num = 18;
		int num2 = 0;
		int num3 = 0;
		int num4 = 0;
		int num5 = 0;
		for (int i = 0; i < 2; i++)
		{
			for (int j = 0; j < num; j++)
			{
				int num6 = g_eq_tmstats[i].timestart[j];
				int num7 = g_eq_tmstats[i].timefinnish[j] - num6;
				if (g_eq_tmstats[i].performance[j] > num4 && num7 >= 45)
				{
					num2 = j;
					num3 = i;
					num4 = g_eq_tmstats[i].performance[j];
					num5 = g_eq_tmstats[i].goal[j];
				}
				else if (g_eq_tmstats[i].performance[j] == num4 && num7 >= 45 && g_eq_tmstats[i].goal[j] > num5)
				{
					num2 = j;
					num3 = i;
					num4 = g_eq_tmstats[i].performance[j];
					num5 = g_eq_tmstats[i].goal[j];
				}
			}
		}
		g_eq_tmstats[num3].ismon[num2] = 1;
		return Game.instance.g_tm[num3].lineup[num2];
	}

	public static void PlayFixture(Fixture fixture, DateTime fixturetime)
	{
		FixtureHelper fixtureHelper = fixture.GetFixtureHelper();
		Game.instance.g_fixtureTime = fixturetime;
		List<EngineQuick_TmStats> g_eq_tmstats = Game.instance.g_eq_tmstats;
		int[] array = new int[91];
		int[] array2 = new int[91];
		int[] array3 = new int[91];
		Team[] g_tm = Game.instance.g_tm;
		fixture.InitPreMatch();
		int num = 18;
		if (fixture.IsPlayed)
		{
			return;
		}
		g_eq_tmstats[0].Initialise();
		g_eq_tmstats[1].Initialise();
		for (int i = 0; i < 91; i++)
		{
			array[i] = 0;
		}
		int[] array4 = new int[2];
		PlayerSquad[] array5 = new PlayerSquad[2];
		if (g_tm[0].lineup != null && g_tm[1].lineup != null)
		{
			array4[0] = 0;
			array4[1] = 0;
			for (int j = 0; j < 2; j++)
			{
				fixture.fixtureClubs[j].formationId = g_tm[j].formationid;
				array5[j] = PlayerSquad.GetPlayers(g_tm[j].clubid);
				int count = array5[j].Count;
				for (int k = 0; k < 11; k++)
				{
					for (int l = 0; l < count; l++)
					{
						if (g_tm[j].lineup[k] == array5[j][l])
						{
							array4[j] += array5[j].matchdayratings(l);
							break;
						}
					}
				}
			}
			fixture.HomeScore(0);
			fixture.AwayScore(0);
			if (array4[0] > array4[1])
			{
				if (array4[0] - array4[1] > 2)
				{
					fixture.HomeScore((byte)((Game.instance.Random.GetRand() % 100 * 7 / 130) | 0));
					fixture.AwayScore((byte)((Game.instance.Random.GetRand() % 100 * 3 / 140) | 0));
				}
				else
				{
					fixture.HomeScore((byte)((Game.instance.Random.GetRand() % 100 * 7 / 130) | 0));
					fixture.AwayScore((byte)((Game.instance.Random.GetRand() % 100 * 3 / 130) | 0));
				}
			}
			else if (array4[1] > array4[0])
			{
				if (array4[1] - array4[0] > 2)
				{
					fixture.AwayScore((byte)((Game.instance.Random.GetRand() % 100 * 7 / 130) | 0));
					fixture.HomeScore((byte)((Game.instance.Random.GetRand() % 100 * 3 / 140) | 0));
				}
				else
				{
					fixture.AwayScore((byte)((Game.instance.Random.GetRand() % 100 * 7 / 130) | 0));
					fixture.HomeScore((byte)((Game.instance.Random.GetRand() % 100 * 3 / 130) | 0));
				}
			}
			else
			{
				fixture.HomeScore((byte)((Game.instance.Random.GetRand() % 100 * 5 / 130) | 0));
				fixture.AwayScore(fixture.homeScore);
			}
			bool flag = false;
			if (fixtureHelper.GoesToPenaltiesIfTied())
			{
				if (fixtureHelper.IsRound2Legs())
				{
					Fixture2LegHelper fixture2LegHelper = new Fixture2LegHelper(fixture);
					flag = fixture2LegHelper.homeInLeg1OverallAggregate() == fixture2LegHelper.awayInLeg1OverallAggregate();
				}
				else
				{
					flag = fixture.homeScore == fixture.awayScore;
				}
			}
			if (flag)
			{
				byte[] penScore = Helpers.GetPenScore(Game.instance.Random.GetRand() % 100, Game.instance.Random.GetRand() % 2 == 0);
				fixture.setPenalties((sbyte)penScore[0], (sbyte)penScore[1]);
			}
			int num2 = ((fixture.homeScore <= fixture.awayScore) ? ((fixture.homeScore < fixture.awayScore) ? 1 : 2) : 0);
			int num3 = ((fixture.homeScore >= fixture.awayScore + (3 + Game.instance.Random.GetRand() % 2) || fixture.awayScore >= fixture.homeScore + (3 + Game.instance.Random.GetRand() % 2)) ? 1 : 0);
			int num4 = 0;
			int num5 = 0;
			int num6 = 0;
			int num7 = 0;
			for (int m = 0; m < 2; m++)
			{
				int num8;
				int num9;
				if (num4 == 1 && Game.instance.Random.GetRand() % 4 == 0)
				{
					num8 = 165;
					num9 = 4;
				}
				else
				{
					num8 = 330;
					num9 = 9;
				}
				if (g_tm[m].tacklingstyle > 1)
				{
					int num10 = g_tm[m].tacklingstyle - 1;
					num8 -= 20 * num10;
					num9 -= 2 * num10;
				}
				if (num9 <= 0)
				{
					num9 = 1;
				}
				if (num8 <= 0)
				{
					num8 = 1;
				}
				int num11 = 25;
				int num12 = 10;
				int num13 = 10;
				if (g_tm[1 - m].tacklingstyle > 1)
				{
					int num10 = g_tm[1 - m].tacklingstyle - 1;
					num11 -= num10;
					num12 -= num10;
					num13 -= num10;
				}
				if (num11 <= 0)
				{
					num11 = 1;
				}
				if (num12 <= 0)
				{
					num12 = 1;
				}
				if (num13 <= 0)
				{
					num13 = 1;
				}
				for (int n = 0; n < 3; n++)
				{
					if (Game.instance.Random.GetRand() % 6 != 0)
					{
						continue;
					}
					short num14 = (short)(Game.instance.Random.GetRand() % 51 + 30);
					while (IsTimeUsed(array[num14], 32))
					{
						num14 = (short)(Game.instance.Random.GetRand() % 51 + 30);
					}
					int num15 = Game.instance.Random.GetRand() % 10 + 1;
					while (g_eq_tmstats[m].timefinnish[num15] < 90)
					{
						num15 = Game.instance.Random.GetRand() % 10 + 1;
					}
					int num16 = 0;
					if ((PlayerData.GetPosition(g_tm[m].lineup[num15]) & 0xE) != 0)
					{
						num16 = 14;
					}
					else if ((PlayerData.GetPosition(g_tm[m].lineup[num15]) & 0x1FF0) != 0)
					{
						num16 = 8176;
					}
					else if ((PlayerData.GetPosition(g_tm[m].lineup[num15]) & 0xE000) != 0)
					{
						num16 = 57344;
					}
					int num17 = 0;
					for (int num18 = 11; num18 < 18; num18++)
					{
						num7 = num18;
						if ((PlayerData.GetPosition(g_tm[m].lineup[num7]) & 1) == 0 && (PlayerData.GetPosition(g_tm[m].lineup[num7]) & num16) != 0 && g_eq_tmstats[m].timestart[num7] == -1)
						{
							num17 = 1;
							break;
						}
					}
					if (num17 == 0)
					{
						num7 = Game.instance.Random.GetRand() % 3 + 11;
						bool flag2 = false;
						for (int num19 = 0; num19 < 14; num19++)
						{
							if (g_eq_tmstats[m].timestart[num19] == -1 && PlayerData.GetPosition(g_tm[m].lineup[num19]) != 1)
							{
								flag2 = true;
							}
						}
						if (!flag2)
						{
							break;
						}
						while (g_eq_tmstats[m].timestart[num7] != -1 || (PlayerData.GetPosition(g_tm[m].lineup[num7]) & 1) != 0)
						{
							num7 = Game.instance.Random.GetRand() % 3 + 11;
						}
					}
					fixture.addSub(g_tm[m].lineup[num15], g_tm[m].lineup[num7], num14);
					g_eq_tmstats[m].timefinnish[num15] = num14;
					g_eq_tmstats[m].timestart[num7] = num14;
					g_eq_tmstats[m].timefinnish[num7] = 90;
					array[num14] |= 32;
				}
				for (int num20 = 0; num20 < 6; num20++)
				{
					if (Game.instance.Random.GetRand() % num8 == 0)
					{
						short num14 = (short)(Game.instance.Random.GetRand() % 90 + 1);
						while (IsTimeUsed(array[num14], 1))
						{
							num14 = (short)(Game.instance.Random.GetRand() % 90 + 1);
						}
						int num21 = Game.instance.Random.GetRand() % (num - 1) + 1;
						while (!IsPlyOnPitch(m, num21, num14))
						{
							num21 = Game.instance.Random.GetRand() % (num - 1) + 1;
						}
						fixture.addRed(g_tm[m].lineup[num21], num14);
						g_eq_tmstats[m].red[num21] = 1;
						g_eq_tmstats[m].timefinnish[num21] = num14;
						array[num14] |= 1;
					}
				}
				for (int num22 = 0; num22 < 6; num22++)
				{
					if (Game.instance.Random.GetRand() % num9 != 0)
					{
						continue;
					}
					short num14 = (short)(Game.instance.Random.GetRand() % 90 + 1);
					while (IsTimeUsed(array[num14], 2))
					{
						num14 = (short)(Game.instance.Random.GetRand() % 90 + 1);
					}
					int num21 = Game.instance.Random.GetRand() % (num - 1) + 1;
					while (!IsPlyOnPitch(m, num21, num14))
					{
						num21 = Game.instance.Random.GetRand() % (num - 1) + 1;
					}
					if (g_eq_tmstats[m].red[num21] <= 0)
					{
						if (g_eq_tmstats[m].yellow[num21] == 0)
						{
							fixture.addYellow(g_tm[m].lineup[num21], num14);
							g_eq_tmstats[m].yellow[num21] = 1;
							array[num14] |= 2;
							continue;
						}
						fixture.addYellow(g_tm[m].lineup[num21], num14);
						g_eq_tmstats[m].yellow[num21]++;
						array[num14] |= 2;
						fixture.addRed(g_tm[m].lineup[num21], num14);
						g_eq_tmstats[m].red[num21] = 1;
						g_eq_tmstats[m].timefinnish[num21] = num14;
						array[num14] |= 1;
					}
				}
				bool flag3 = true;
				if (fixture.competitiontype == 4)
				{
					flag3 = false;
				}
				else
				{
					short currentUserClubID = Game.GetCurrentUserClubID();
					if (currentUserClubID != 0)
					{
						short associationID = Club.GetAssociationID(currentUserClubID);
						if (Club.GetAssociationID(fixture.homeClubId) != associationID || Club.GetAssociationID(fixture.awayClubId) != associationID)
						{
							flag3 = false;
						}
					}
				}
				if (flag3)
				{
					for (int num23 = 0; num23 < 6; num23++)
					{
						if (Game.instance.Random.GetRand() % 24 == 0)
						{
							short num14 = (short)(Game.instance.Random.GetRand() % 90 + 1);
							while (IsTimeUsed(array[num14], 4))
							{
								num14 = (short)(Game.instance.Random.GetRand() % 90 + 1);
							}
							int num21 = Game.instance.Random.GetRand() % (num - 1) + 1;
							while (!IsPlyOnPitch(m, num21, num14))
							{
								num21 = Game.instance.Random.GetRand() % (num - 1) + 1;
							}
							int length = Game.instance.Random.GetRand() % (Game.instance.Random.GetRand() % (Game.instance.Random.GetRand() % 83 + 1) + 1) + 2;
							List<int> injuryIDsFromLength = InjuriesData.GetInjuryIDsFromLength(length);
							int type = 0;
							if (injuryIDsFromLength.Count > 0)
							{
								type = injuryIDsFromLength[Game.instance.Random.GetRand() % injuryIDsFromLength.Count];
							}
							fixture.addInjury(g_tm[m].lineup[num21], num14, length, type);
							g_eq_tmstats[m].injury[num21] = 1;
							g_eq_tmstats[m].timefinnish[num21] = num14;
							array[num14] |= 4;
						}
					}
				}
				for (int num24 = 0; num24 < fixture.score[m]; num24++)
				{
					if (Game.instance.Random.GetRand() % 240 == 0)
					{
						short num14 = (short)(Game.instance.Random.GetRand() % 90 + 1);
						while (IsTimeUsed(array[num14], 16))
						{
							num14 = (short)(Game.instance.Random.GetRand() % 90 + 1);
						}
						int num21 = GetOwnGoalScorer(g_tm[1 - m]);
						while (!IsPlyOnPitch(1 - m, num21, num14))
						{
							num21 = GetOwnGoalScorer(g_tm[1 - m]);
						}
						array[num14] |= 16;
						array2[num14] = 4;
						array3[num14] = 0;
						fixture.addGoal(g_tm[1 - m].lineup[num21], num14, array2[num14]);
						g_eq_tmstats[1 - m].owngoal[num21]++;
					}
					else
					{
						short num14 = (short)(Game.instance.Random.GetRand() % 90 + 1);
						while (IsTimeUsed(array[num14], 8))
						{
							num14 = (short)(Game.instance.Random.GetRand() % 90 + 1);
						}
						array[num14] |= 8;
						int num21;
						if (Game.instance.Random.GetRand() % num11 == 0 && IsPlyOnPitch(m, g_tm[m].penaltytaker, num14))
						{
							num21 = g_tm[m].penaltytaker;
							array2[num14] = 1;
							array3[num14] = 0;
						}
						else if (Game.instance.Random.GetRand() % num12 == 0 && IsPlyOnPitch(m, g_tm[m].deadballtakerleft, num14))
						{
							num21 = g_tm[m].deadballtakerleft;
							array2[num14] = 2;
							array3[num14] = 0;
						}
						else if (Game.instance.Random.GetRand() % 4000 == 0 && IsPlyOnPitch(m, g_tm[m].cornertakerleft, num14))
						{
							num21 = g_tm[m].cornertakerleft;
							array2[num14] = 3;
							array3[num14] = 0;
						}
						else
						{
							num21 = GetGoalScorer(g_tm[m], num14, m);
							array2[num14] = 0;
							int num25;
							if (Game.instance.Random.GetRand() % num13 == 0 && IsPlyOnPitch(m, g_tm[m].deadballtakerleft, num14) && g_tm[m].deadballtakerleft != num21)
							{
								num25 = g_tm[m].deadballtakerleft;
								array3[num14] = 1;
							}
							else if (Game.instance.Random.GetRand() % 25 == 0 && IsPlyOnPitch(m, g_tm[m].cornertakerleft, num14) && g_tm[m].cornertakerleft != num21)
							{
								num25 = g_tm[m].cornertakerleft;
								array3[num14] = 2;
							}
							else
							{
								num25 = GetGoalAssist(g_tm[m], num21, num14, m);
								array3[num14] = 0;
							}
							fixture.addAssist(g_tm[m].lineup[num25], num14, array3[num14]);
							g_eq_tmstats[m].assist[num25]++;
							num6++;
						}
						num5 = fixture.addGoal(g_tm[m].lineup[num21], num14, array2[num14]);
						g_eq_tmstats[m].goal[num21]++;
					}
					num5++;
				}
			}
			int matchSkillAverage = (array4[0] + array4[1]) / 22;
			CalculateTeamPlayerPerformance(0, num3, num2, fixture.tm1, matchSkillAverage, array5[0]);
			CalculateTeamPlayerPerformance(1, num3, num2, fixture.tm2, matchSkillAverage, array5[1]);
			FixtureClub fixtureClub = fixture.fixtureClubs[0];
			FixtureClub fixtureClub2 = fixture.fixtureClubs[1];
			switch (num2)
			{
			case 0:
				fixtureClub.shotsongoal = (byte)(9 + (Game.instance.Random.GetRand() % 5 + Game.instance.Random.GetRand() % 5));
				if (fixtureClub.shotsongoal > fixtureClub2.shotsongoal)
				{
					fixtureClub.possession = (byte)(58 - Game.instance.Random.GetRand() % 15);
				}
				else
				{
					fixtureClub.shotsongoal += 3;
					fixtureClub.possession = (byte)(55 - Game.instance.Random.GetRand() % 10);
				}
				if (num3 == 1)
				{
					fixtureClub.possession = (byte)Math.Max(54, fixtureClub.possession + 10);
				}
				fixtureClub2.shotsongoal = (byte)(4 + (Game.instance.Random.GetRand() % 6 + Game.instance.Random.GetRand() % 6));
				fixtureClub2.possession = (byte)(100 - fixtureClub.possession);
				break;
			case 1:
				fixtureClub2.shotsongoal = (byte)(9 + (Game.instance.Random.GetRand() % 5 + Game.instance.Random.GetRand() % 5));
				if (fixtureClub2.shotsongoal > fixtureClub.shotsongoal)
				{
					fixtureClub2.possession = (byte)(60 - Game.instance.Random.GetRand() % 20);
				}
				else
				{
					fixtureClub2.shotsongoal += 3;
					fixtureClub2.possession = (byte)(55 - Game.instance.Random.GetRand() % 15);
				}
				if (num3 == 1)
				{
					fixtureClub.shotsongoal = (byte)(4 + (Game.instance.Random.GetRand() % 6 + Game.instance.Random.GetRand() % 6));
					fixtureClub2.possession = (byte)Math.Max(54, fixtureClub2.possession + 10);
				}
				fixtureClub.shotsongoal = (byte)(4 + (Game.instance.Random.GetRand() % 6 + Game.instance.Random.GetRand() % 6));
				fixtureClub.possession = (byte)(100 - fixtureClub2.possession);
				break;
			default:
				fixtureClub.shotsongoal = (byte)(7 + (Game.instance.Random.GetRand() % 5 + Game.instance.Random.GetRand() % 5));
				fixtureClub2.shotsongoal = (byte)(7 + (Game.instance.Random.GetRand() % 5 + Game.instance.Random.GetRand() % 5));
				fixtureClub.possession = (byte)(45 + Game.instance.Random.GetRand() % 10);
				fixtureClub2.possession = (byte)(100 - fixtureClub.possession);
				break;
			}
			fixtureClub.shotsontarget = (byte)(fixtureClub.shotsongoal - (Game.instance.Random.GetRand() % fixtureClub.shotsongoal + 1));
			fixtureClub2.shotsontarget = (byte)(fixtureClub2.shotsongoal - (Game.instance.Random.GetRand() % fixtureClub2.shotsongoal + 1));
			fixtureClub.shotsongoal = Math.Max(fixtureClub.shotsongoal, fixture.homeScore);
			fixtureClub2.shotsongoal = Math.Max(fixtureClub2.shotsongoal, fixture.awayScore);
			fixtureClub.shotsontarget = Math.Max(fixtureClub.shotsontarget, fixture.homeScore);
			fixtureClub2.shotsontarget = Math.Max(fixtureClub2.shotsontarget, fixture.awayScore);
			fixture.manofmatch = FindMoM();
			fixtureClub.managerId = (short)Club.GetManagerID(fixture.homeClubId);
			fixtureClub2.managerId = (short)Club.GetManagerID(fixture.awayClubId);
			fixtureClub.corners = (byte)(3 + Game.instance.Random.GetRand() % 6);
			fixtureClub2.corners = (byte)(3 + Game.instance.Random.GetRand() % 6);
			if (fixture.homeClubId == Game.instance.CurrentManagedClubId || fixture.awayClubId == Game.instance.CurrentManagedClubId)
			{
				Matchday.AddFixtureClubMessages(fixture, fixturetime);
			}
			fixture.isPlayed = true;
		}
		else
		{
			Debug.LogError("ENGINEQUICK_playfixture - ERROR - one of the teams could not field a side - setting score to 0-0 for now");
			fixture.Score(0, 0);
			fixture.Score(1, 0);
		}
		if (Game.debugForceResults != Game.DebugForceResults.OFF)
		{
			fixture.DebugForceResult();
		}
	}

	private static void CalculateTeamPlayerPerformance(int WhichTeam, int bigwin, int matchoutcome, FixturePlayer[] fixturePlayers, int matchSkillAverage, PlayerSquad clubPlayers)
	{
		byte b = 0;
		List<EngineQuick_TmStats> g_eq_tmstats = Game.instance.g_eq_tmstats;
		Team[] g_tm = Game.instance.g_tm;
		for (int i = 0; i < 18; i++)
		{
			int num = 0;
			int num2 = 0;
			for (int j = 0; j < clubPlayers.Count; j++)
			{
				if (clubPlayers[j] == g_tm[WhichTeam].lineup[i])
				{
					num2 = clubPlayers.matchdayratings(j);
					break;
				}
			}
			if (g_eq_tmstats[WhichTeam].timestart[i] >= 0 && g_eq_tmstats[WhichTeam].timefinnish[i] > 0)
			{
				num = num2 - matchSkillAverage;
				if (num == 0)
				{
					b = (byte)Game.instance.Random.GetRand(6, 7);
				}
				else if (num > 0)
				{
					b = ((num > 1) ? ((byte)Game.instance.Random.GetRand(7, 8)) : ((byte)Game.instance.Random.GetRand(6, 8)));
				}
				else if (num < 0)
				{
					if (num >= -2)
					{
						b = (byte)Game.instance.Random.GetRand(5, 7);
					}
					else
					{
						b = ((num < -5) ? ((byte)Game.instance.Random.GetRand(4, 6)) : ((byte)Game.instance.Random.GetRand(5, 6)));
					}
				}
				if (g_eq_tmstats[WhichTeam].owngoal[i] > 0)
				{
					b--;
				}
				if (g_eq_tmstats[WhichTeam].red[i] > 0)
				{
					b -= 2;
				}
				else if (g_eq_tmstats[WhichTeam].yellow[i] > 0)
				{
					b--;
				}
				b = Math.Max(b, (byte)3);
				switch (g_eq_tmstats[WhichTeam].goal[i] + g_eq_tmstats[WhichTeam].assist[i])
				{
				case 1:
					b++;
					break;
				case 2:
					b += 2;
					break;
				default:
					b = 10;
					break;
				case 0:
					break;
				}
				b = Math.Min(b, (byte)10);
			}
			else
			{
				b = 0;
			}
			g_eq_tmstats[WhichTeam].performance[i] = b;
			fixturePlayers[i].Update(g_tm[WhichTeam].lineup[i], g_eq_tmstats[WhichTeam].timestart[i], g_eq_tmstats[WhichTeam].timefinnish[i], b);
		}
	}
}
