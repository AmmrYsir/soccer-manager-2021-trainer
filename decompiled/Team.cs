using System;
using System.Collections.Generic;
using System.Linq;

public class Team
{
	public short clubid;

	public int aim;

	public int[] lineup;

	public byte[] plyrating;

	public byte[] shirtnum;

	public byte[] fitness;

	public byte[] morale;

	public byte formationid;

	public byte tacklingstyle;

	public byte mentality;

	public byte passingstyleid;

	public byte attackingstyle;

	public byte tempoid;

	public byte pressingid;

	public bool counterattack;

	public bool menbehindball;

	public bool tightmarking;

	public bool playoffside;

	public bool useplaymaker;

	public bool usetargetman;

	public byte captain;

	public byte penaltytaker;

	public byte deadballtakerleft;

	public byte deadballtakerright;

	public byte cornertakerleft;

	public byte cornertakerright;

	public byte playmaker;

	public byte targetman;

	public byte width;

	public byte backline;

	public byte creativity;

	public byte attforwards;

	public byte attmidfielders;

	public byte attwingers;

	public int[] plyrinst;

	public int[] playerRoles;

	public Team()
	{
		clubid = 0;
		aim = 1;
		lineup = new int[18];
		plyrating = new byte[18];
		shirtnum = new byte[18];
		fitness = new byte[18];
		morale = new byte[18];
		formationid = 0;
		tacklingstyle = 1;
		mentality = 3;
		passingstyleid = 2;
		attackingstyle = 1;
		tempoid = 2;
		pressingid = 2;
		counterattack = false;
		menbehindball = false;
		tightmarking = false;
		playoffside = false;
		useplaymaker = false;
		usetargetman = false;
		captain = 0;
		penaltytaker = 0;
		deadballtakerleft = 0;
		cornertakerleft = 0;
		playmaker = 0;
		targetman = 0;
		plyrinst = new int[11];
		playerRoles = new int[11];
		width = 2;
		backline = 2;
		creativity = 2;
		attforwards = 3;
		attmidfielders = 3;
		attwingers = 3;
	}

	public static byte PickFormation(short clubid)
	{
		if (Game.instance.g_ClubData.ContainsKey(clubid))
		{
			return Game.instance.g_ClubData[clubid].TeamFormation;
		}
		return 0;
	}

	public static byte GetCounterFormation(byte formationID, bool attacking)
	{
		if (attacking)
		{
			switch (formationID)
			{
			case 0:
			case 15:
			case 20:
				return 19;
			default:
				return 1;
			case 4:
				return 16;
			case 6:
				return 23;
			}
		}
		switch (formationID)
		{
		case 19:
			return 0;
		default:
			return 2;
		case 4:
		case 16:
		case 23:
		case 27:
			return 6;
		}
	}

	public static bool CanSetBestTeamIgnoringAvailabililty(short clubID)
	{
		List<int> players = Game.instance.g_ClubPlayer[clubID].players;
		int num = 0;
		int num2 = 0;
		for (int i = 0; i < players.Count; i++)
		{
			if (PlayerData.IsGoalKeeper(players[i]))
			{
				num++;
			}
			else
			{
				num2++;
			}
		}
		if (num > 0)
		{
			return num2 >= 18;
		}
		return false;
	}

	public static void SetBestTeamIgnoreAvailability(Team tm, short clubID)
	{
		tm.clubid = clubID;
		tm.aim = 1;
		tm.formationid = PickFormation(clubID);
		PickTeam(tm, tm.formationid, 0, -1, -1, justFirstTeam: false, ignoreAvailability: true);
	}

	public static void PickTeam(Team tm, int formationid, int strength)
	{
		PickTeam(tm, formationid, strength, -1, -1, justFirstTeam: false);
	}

	public static void PickTeam(Team tm, int formationid, int strength, int compType, int compID, bool justFirstTeam, bool ignoreAvailability = false)
	{
		int associationID = Club.GetAssociationID(tm.clubid);
		List<int> list = new List<int>(PlayerSquad.GetPlayers(tm.clubid).players);
		byte[] array;
		if (justFirstTeam)
		{
			List<KeyValuePair<int, byte>> list2 = new List<KeyValuePair<int, byte>>();
			list = list.Where((int i) => Player.GetFirstTeam(i)).ToList();
			foreach (int item in list)
			{
				if (!PlayerData.GetRetired(item))
				{
					list2.Add(new KeyValuePair<int, byte>(item, Player.GetRatingQuick(item)));
				}
			}
			list2.Sort((KeyValuePair<int, byte> i, KeyValuePair<int, byte> j) => i.Value.CompareTo(j.Value));
			list = list2.Select((KeyValuePair<int, byte> i) => i.Key).ToList();
			array = list2.Select((KeyValuePair<int, byte> i) => i.Value).ToArray();
		}
		else
		{
			List<KeyValuePair<int, byte>> list3 = new List<KeyValuePair<int, byte>>();
			foreach (int item2 in list)
			{
				list3.Add(new KeyValuePair<int, byte>(item2, Player.GetRatingQuick(item2)));
			}
			list3.Sort((KeyValuePair<int, byte> i, KeyValuePair<int, byte> j) => i.Value.CompareTo(j.Value));
			list = list3.Select((KeyValuePair<int, byte> i) => i.Key).ToList();
			array = list3.Select((KeyValuePair<int, byte> i) => i.Value).ToArray();
		}
		int count = list.Count;
		_ = FormationData.g_black_gridpos;
		int num = 18;
		int[] array2 = new int[num];
		_ = new int[num];
		Dictionary<int, bool> dictionary = new Dictionary<int, bool>();
		Dictionary<int, bool> dictionary2 = new Dictionary<int, bool>();
		bool flag = false;
		int num2 = 0;
		if (!ignoreAvailability)
		{
			for (int num3 = list.Count - 1; num3 >= 0; num3--)
			{
				bool flag2 = Player.IsInjured(list[num3]);
				bool flag3 = Player.IsSuspended(list[num3], associationID, compType, compID);
				if (flag2 | flag3)
				{
					dictionary.Add(list[num3], value: true);
				}
				else if (flag && PlayerData.GetCountryDataID(list[num3]) != 42 && ++num2 > 3)
				{
					dictionary.Add(list[num3], value: true);
				}
				if (Player.GetFitness(list[num3]) < 70)
				{
					dictionary2.Add(list[num3], value: true);
				}
			}
		}
		FormationHelper formationHelper = FormationHelperFactory.GetFormationHelper((byte)formationid);
		array2[0] = 1;
		tm.lineup[0] = 0;
		tm.plyrating[0] = 0;
		int num4 = 1;
		for (num4 = 1; num4 < 11; num4++)
		{
			array2[num4] = formationHelper.formationPositions[num4];
			tm.lineup[num4] = 0;
			tm.plyrating[num4] = 0;
		}
		array2[num4] = 1;
		tm.lineup[num4] = 0;
		tm.plyrating[num4] = 0;
		for (num4++; num4 < num; num4++)
		{
			if (num4 % 3 == 0)
			{
				array2[num4] = 14;
			}
			if (num4 % 3 == 1)
			{
				array2[num4] = 8176;
			}
			if (num4 % 3 == 2)
			{
				array2[num4] = 57344;
			}
			tm.lineup[num4] = 0;
			tm.plyrating[num4] = 0;
		}
		int num5 = 999;
		int num6 = 0;
		switch (strength)
		{
		case 3:
			num5 = 2;
			num6 = 8;
			if (count > num + num6)
			{
				break;
			}
			goto case 2;
		case 2:
			num5 = 3;
			num6 = 5;
			if (count > num + num6)
			{
				break;
			}
			goto case 1;
		case 1:
			num5 = 9;
			num6 = 2;
			if (count > num + num6)
			{
				break;
			}
			goto default;
		default:
			num5 = 9999;
			num6 = 0;
			break;
		}
		int num7 = 0;
		int num8 = 0;
		num4 = count - 1;
		Game.instance.Random.SetSeed(Game.instance.CurrentDate.DayOfYear);
		while (num7 < num && num4 != 0)
		{
			num8 = 0;
			if (num6 > 0 && count > num + num6 && Game.instance.Random.GetRand() % num5 == 0)
			{
				num6--;
				num4--;
				continue;
			}
			if (!dictionary.ContainsKey(list[num4]) && !dictionary2.ContainsKey(list[num4]))
			{
				int preferredPosition = Game.instance.g_PlayerData[list[num4]].PreferredPosition;
				int position = Game.instance.g_PlayerData[list[num4]].Position;
				for (int num9 = 0; num9 < num; num9++)
				{
					if ((position & array2[num9]) != 0 && tm.lineup[num9] == 0)
					{
						if (preferredPosition <= 0)
						{
							tm.lineup[num9] = list[num4];
							tm.plyrating[num9] = array[num4];
							num7++;
							break;
						}
						if ((preferredPosition & array2[num9]) != 0 && num9 < 11)
						{
							tm.lineup[num9] = list[num4];
							tm.plyrating[num9] = array[num4];
							num7++;
							break;
						}
						if (num8 == 0)
						{
							num8 = num9;
						}
					}
					if (num9 == num - 1 && num8 != 0)
					{
						tm.lineup[num8] = list[num4];
						tm.plyrating[num8] = array[num4];
						num7++;
					}
				}
			}
			num4--;
		}
		int num10 = tm.lineup.Length;
		while (num10-- > 0)
		{
			if (tm.lineup[num10] != 0)
			{
				continue;
			}
			num4 = count;
			while (num4-- > 0)
			{
				if (!dictionary.ContainsKey(list[num4]) && !tm.lineup.Contains(list[num4]))
				{
					tm.lineup[num10] = list[num4];
					tm.plyrating[num10] = array[num4];
				}
			}
		}
		int num11 = num4;
		int num12 = 0;
		int num13 = 0;
		for (num4 = 11; num4 < num; num4++)
		{
			if (dictionary.ContainsKey(tm.lineup[num4]) || (Game.instance.g_PlayerData[tm.lineup[num4]].Position & 1) != 0)
			{
				continue;
			}
			int num14 = tm.plyrating[num4];
			num11 = num4;
			num12 = 0;
			num13 = 0;
			for (int num15 = 1; num15 < 11; num15++)
			{
				if ((Game.instance.g_PlayerData[tm.lineup[num4]].Position & array2[num15]) == 0)
				{
					continue;
				}
				int num16 = 99;
				int num17 = tm.plyrating[num15];
				if (num14 > num17 && num17 < num16)
				{
					num16 = num17;
					num12 = num15;
					num13 = 0;
				}
				for (int num18 = 1; num18 < 11; num18++)
				{
					if ((Game.instance.g_PlayerData[tm.lineup[num15]].Position & array2[num18]) != 0 && num15 != num18)
					{
						int num19 = tm.plyrating[num18];
						if (num14 > num19 && num19 < num16)
						{
							num16 = num19;
							num12 = num15;
							num13 = num18;
						}
					}
				}
			}
			if (num12 > 0)
			{
				if (num13 > 0)
				{
					int num20 = tm.lineup[num13];
					tm.lineup[num13] = tm.lineup[num12];
					tm.lineup[num12] = tm.lineup[num11];
					tm.lineup[num11] = num20;
					byte b = tm.plyrating[num13];
					tm.plyrating[num13] = tm.plyrating[num12];
					tm.plyrating[num12] = tm.plyrating[num11];
					tm.plyrating[num11] = b;
				}
				else
				{
					int num20 = tm.lineup[num12];
					tm.lineup[num12] = tm.lineup[num11];
					tm.lineup[num11] = num20;
					byte b = tm.plyrating[num12];
					tm.plyrating[num12] = tm.plyrating[num11];
					tm.plyrating[num11] = b;
				}
			}
		}
		for (num4 = 1; num4 < 11; num4++)
		{
			if (tm.lineup[num4] != 0)
			{
				continue;
			}
			if ((array2[num4] & 0xE) != 0)
			{
				num11 = 14;
			}
			else
			{
				num11 = (((array2[num4] & 0x1FF0) == 0) ? 57344 : 8176);
			}
			for (int num21 = 0; num21 < count; num21++)
			{
				if (dictionary.ContainsKey(list[num21]) || (Game.instance.g_PlayerData[list[num21]].Position & num11) <= 0 || dictionary.ContainsKey(list[num21]))
				{
					continue;
				}
				bool flag4 = false;
				for (int num22 = 0; num22 < tm.lineup.Length; num22++)
				{
					if (tm.lineup[num22] == list[num21])
					{
						if (num22 >= 11)
						{
							tm.lineup[num22] = 0;
							tm.plyrating[num22] = 0;
						}
						else
						{
							flag4 = true;
						}
						break;
					}
				}
				if (!flag4)
				{
					tm.lineup[num4] = list[num21];
					tm.plyrating[num4] = array[num21];
					break;
				}
			}
			if (tm.lineup[num4] != 0)
			{
				continue;
			}
			for (int num23 = 0; num23 < count; num23++)
			{
				if (dictionary.ContainsKey(list[num23]))
				{
					continue;
				}
				bool flag5 = false;
				for (int num24 = 0; num24 < tm.lineup.Length; num24++)
				{
					if (tm.lineup[num24] == list[num23])
					{
						if (num24 >= 11)
						{
							tm.lineup[num24] = 0;
							tm.plyrating[num24] = 0;
						}
						else
						{
							flag5 = true;
						}
						break;
					}
				}
				if (!flag5)
				{
					tm.lineup[num4] = list[num23];
					tm.plyrating[num4] = array[num23];
					break;
				}
			}
		}
		for (num4 = 11; num4 < num; num4++)
		{
			if (tm.lineup[num4] != 0)
			{
				continue;
			}
			for (int num25 = 0; num25 < count; num25++)
			{
				if (dictionary.ContainsKey(list[num25]))
				{
					continue;
				}
				bool flag6 = false;
				for (int num26 = 0; num26 < tm.lineup.Length; num26++)
				{
					if (tm.lineup[num26] == list[num25])
					{
						flag6 = true;
						break;
					}
				}
				if (!flag6)
				{
					tm.lineup[num4] = list[num25];
					tm.plyrating[num4] = array[num25];
					break;
				}
			}
		}
		for (num4 = 1; num4 < 11; num4++)
		{
			if ((Game.instance.g_PlayerData[tm.lineup[num4]].Position & 1) == 0)
			{
				continue;
			}
			for (int num27 = 0; num27 < count; num27++)
			{
				if (dictionary.ContainsKey(list[num27]) || (Game.instance.g_PlayerData[list[num27]].Position & 1) != 0)
				{
					continue;
				}
				bool flag7 = false;
				for (int num28 = 0; num28 < tm.lineup.Length; num28++)
				{
					if (tm.lineup[num28] == list[num27])
					{
						flag7 = true;
						break;
					}
				}
				if (!flag7)
				{
					tm.lineup[num4] = list[num27];
					tm.plyrating[num4] = array[num27];
					break;
				}
			}
		}
		if ((Game.instance.g_PlayerData[tm.lineup[0]].Position & 1) != 0)
		{
			return;
		}
		for (int num29 = 0; num29 < count; num29++)
		{
			if (dictionary.ContainsKey(list[num29]) || (Game.instance.g_PlayerData[list[num29]].Position & 1) == 0)
			{
				continue;
			}
			int num30 = -1;
			for (int num31 = 0; num31 < tm.lineup.Length; num31++)
			{
				if (tm.lineup[num31] == list[num29])
				{
					num30 = num31;
					break;
				}
			}
			if (num30 == -1)
			{
				tm.lineup[0] = list[num29];
				tm.plyrating[0] = array[num29];
				break;
			}
			int num32 = tm.lineup[0];
			byte b2 = tm.plyrating[0];
			tm.lineup[0] = tm.lineup[num30];
			tm.plyrating[0] = tm.plyrating[num30];
			tm.lineup[num30] = num32;
			tm.plyrating[num30] = b2;
			break;
		}
	}

	public static int PickBestReplacementPlayer(int playerID)
	{
		ManagedClub currentManagedClub = Game.instance.CurrentManagedClub;
		int position = Player.GetPosition(playerID, preferred: true);
		int position2 = Player.GetPosition(playerID);
		int num = 0;
		int num2 = 0;
		int num3 = 0;
		int num4 = 0;
		int playerBasicPositionMask = Helpers.GetPlayerBasicPositionMask(position);
		int[] array = new int[currentManagedClub.currentTactics.lineUp.Length];
		for (int i = 0; i < array.Length; i++)
		{
			array[i] = currentManagedClub.currentTactics.lineUp[i];
		}
		List<int> list = PlayerSquad.GetPlayers(currentManagedClub.ClubID).players.Except(array.Take(11)).ToList();
		for (int j = 0; j < list.Count; j++)
		{
			if ((!Player.GetStatus(list[j]).firstTeam && playerBasicPositionMask != 1) || playerID == list[j])
			{
				continue;
			}
			int rating = Player.GetRating(list[j]);
			int position3 = Player.GetPosition(list[j]);
			if ((position & position3) != 0)
			{
				if (rating >= num4)
				{
					num2 = list[j];
					num4 = rating;
				}
			}
			else if ((playerBasicPositionMask & position3) != 0)
			{
				if (rating >= num3)
				{
					num = list[j];
					num3 = rating;
				}
			}
			else if ((position3 & position2) != 0 && rating >= num3)
			{
				num = list[j];
				num3 = rating;
			}
		}
		if (num2 != 0)
		{
			return num2;
		}
		if (num != 0)
		{
			return num;
		}
		return 0;
	}

	public static void replaceSubstitutePlayer(int playerID)
	{
		ManagedClub currentManagedClub = Game.instance.CurrentManagedClub;
		int num = 0;
		int num2 = Array.IndexOf(currentManagedClub.currentTactics.lineUp, playerID);
		int num3 = Array.LastIndexOf(currentManagedClub.currentTactics.lineUp, playerID);
		if (num3 != -1 && num3 != num2)
		{
			num = ReplaceDuplicate(playerID);
			if (num > 0)
			{
				currentManagedClub.currentTactics.lineUp[num3] = num;
				Player.SetFirstTeam(num);
			}
			else
			{
				int num4 = PlayerRespawn.giveRespawnToManagedClub(currentManagedClub.ClubID, Player.GetPosition(playerID));
				currentManagedClub.currentTactics.lineUp[num3] = num4;
				Player.SetFirstTeam(num4);
			}
		}
	}

	public static int ReplaceDuplicate(int PlayerID)
	{
		List<int> list = new List<int>(PlayerSquad.GetPlayers(Game.instance.CurrentManagedClubId).players.Except(Game.instance.CurrentManagedClub.currentTactics.lineUp).ToList());
		int num = 0;
		int num2 = 0;
		int position = Player.GetPosition(PlayerID);
		for (int i = 0; i < list.Count; i++)
		{
			if (PlayerID != list[i])
			{
				int rating = Player.GetRating(list[i]);
				int position2 = Player.GetPosition(list[i]);
				if (rating >= num2)
				{
					num = list[i];
					num2 = rating;
				}
				else if ((position2 & position) != 0)
				{
					num = list[i];
					num2 = rating;
				}
				else if (num == 0)
				{
					num = list[i];
				}
			}
		}
		return num;
	}

	public static void PickTactics(Team tm, int tmix, bool quickload, int oppTeamRatingDiff)
	{
		TacticAction g_tacticaction = Game.instance.g_tacticaction;
		Game.instance.g_tm_aim[tmix] = tm.aim;
		Club club = Game.instance.g_ClubData[tm.clubid];
		tm.tacklingstyle = (byte)(club.TacklingStyle + 1);
		tm.mentality = (byte)(club.Mentality + 1);
		tm.passingstyleid = (byte)(club.PassingStyleID + 1);
		tm.attackingstyle = (byte)(club.AttackingStyle + 1);
		tm.tempoid = (byte)(club.TempoID + 1);
		tm.pressingid = (byte)(club.PressingID + 1);
		if (!quickload)
		{
			SetTmInstructions(tmix, 0, 1);
			tm.tacklingstyle = g_tacticaction.tacklingstyle;
			tm.mentality = g_tacticaction.mentality;
			tm.passingstyleid = g_tacticaction.passingstyleid;
			tm.attackingstyle = g_tacticaction.attackingstyle;
			tm.tempoid = g_tacticaction.tempoid;
			tm.pressingid = g_tacticaction.pressingid;
		}
		for (int i = 0; i < 18; i++)
		{
			g_tacticaction.plyix[i] = i;
		}
		SetPlyInstructions(tm);
		tm.counterattack = g_tacticaction.counterattack;
		tm.menbehindball = g_tacticaction.menbehindball;
		tm.tightmarking = g_tacticaction.tightmarking;
		tm.playoffside = g_tacticaction.playoffside;
		tm.useplaymaker = g_tacticaction.useplaymaker;
		tm.usetargetman = g_tacticaction.usetargetman;
		tm.captain = g_tacticaction.captain;
		tm.penaltytaker = g_tacticaction.penaltytaker;
		tm.deadballtakerleft = g_tacticaction.deadballtakerleft;
		tm.cornertakerleft = g_tacticaction.cornertakerleft;
		tm.playmaker = g_tacticaction.playmaker;
		tm.targetman = g_tacticaction.targetman;
		tm.width = g_tacticaction.width;
		tm.backline = g_tacticaction.backline;
		tm.creativity = g_tacticaction.creativity;
		tm.attforwards = g_tacticaction.attforwards;
		tm.attmidfielders = g_tacticaction.attmidfielders;
		tm.attwingers = g_tacticaction.attwingers;
		tm.playerRoles = g_tacticaction.playerRoles;
		bool flag = tmix == 0;
		if (oppTeamRatingDiff >= 5)
		{
			tm.mentality = 4;
		}
		else if (oppTeamRatingDiff <= -5)
		{
			tm.mentality = 1;
		}
		else if (oppTeamRatingDiff >= 2)
		{
			tm.mentality = 4;
		}
		else if (oppTeamRatingDiff <= -2)
		{
			tm.mentality = 2;
		}
		else
		{
			if (oppTeamRatingDiff < 1 && oppTeamRatingDiff > -1)
			{
				return;
			}
			if (flag)
			{
				if (oppTeamRatingDiff >= 0)
				{
					tm.mentality = 4;
				}
				else
				{
					tm.mentality = 3;
				}
			}
			else
			{
				tm.mentality = 3;
			}
		}
	}

	public static Constants.TacticChange ChangeTactics(int time, int homescore, int awayscore, int tmix)
	{
		TacticInfo g_tacticinfo = Game.instance.g_tacticinfo;
		TacticAction g_tacticaction = Game.instance.g_tacticaction;
		Team[] g_tm = Game.instance.g_tm;
		Constants.TacticChange tacticChange = Constants.TacticChange.NONE;
		int num = 0;
		num = ((time >= 45) ? ((time < 60) ? 1 : ((time >= 80) ? 3 : 2)) : 0);
		int gamestate = ((homescore > awayscore) ? ((tmix != 0) ? 2 : 0) : ((homescore >= awayscore) ? 1 : ((tmix == 0) ? 2 : 0)));
		int[] g_tm_aim = Game.instance.g_tm_aim;
		g_tm_aim[tmix] = Game.instance.g_tm[tmix].aim;
		if (g_tacticinfo.numreds > g_tacticinfo.numreds_opp + 1)
		{
			g_tm_aim[tmix]--;
			if (g_tm_aim[tmix] < 0)
			{
				g_tm_aim[tmix] = 0;
			}
		}
		if (g_tacticinfo.numreds_opp > g_tacticinfo.numreds + 1)
		{
			g_tm_aim[tmix]++;
			if (g_tm_aim[tmix] > 2)
			{
				g_tm_aim[tmix] = 2;
			}
		}
		for (int i = 0; i < 18; i++)
		{
			g_tacticaction.plyix[i] = g_tacticinfo.plyix[i];
		}
		for (int j = 0; j < 11; j++)
		{
			g_tacticaction.plyrinst[j] = g_tacticinfo.plyrinst[j];
		}
		g_tacticaction.formationid = g_tacticinfo.formationid;
		g_tacticaction.width = g_tacticinfo.width;
		g_tacticaction.creativity = g_tacticinfo.creativity;
		g_tacticaction.backline = g_tacticinfo.backline;
		g_tacticaction.attforwards = g_tacticinfo.attforwards;
		g_tacticaction.attmidfielders = g_tacticinfo.attmidfielders;
		g_tacticaction.attwingers = g_tacticinfo.attwingers;
		tacticChange |= SetTmInstructions(tmix, num, gamestate);
		FormationPosData[][] g_formations = FormationData.g_formations;
		int num2 = -1;
		int num3 = -1;
		for (int k = 0; k < g_tacticinfo.numinj; k++)
		{
			if (g_tacticinfo.injinfo[k].time != time)
			{
				continue;
			}
			for (int l = 0; l < 11; l++)
			{
				if (g_tacticinfo.plyix[l] == g_tacticinfo.injinfo[k].plyoff)
				{
					num2 = l;
					break;
				}
			}
			if (num2 != -1)
			{
				num3 = GetBestSub(tmix, num2);
				if (num3 != -1)
				{
					int num4 = g_tacticaction.plyix[num2];
					g_tacticaction.plyix[num2] = g_tacticaction.plyix[num3];
					g_tacticaction.plyix[num3] = num4;
					tacticChange |= Constants.TacticChange.SUB;
				}
			}
		}
		int num5 = 0;
		int num6 = 0;
		int num7 = 0;
		int[] array = new int[10];
		int[] array2 = new int[10];
		int[] array3 = new int[10];
		int num8 = 0;
		int num9 = 0;
		int num10 = 0;
		int num11 = time - 1;
		int num12 = time + 1;
		for (int m = 0; m < g_tacticinfo.numreds; m++)
		{
			if (g_tacticinfo.redinfo[m].time > num12 || g_tacticinfo.redinfo[m].time < num11)
			{
				continue;
			}
			for (int n = 0; n < 11; n++)
			{
				if (g_tacticinfo.plyix[n] == g_tacticinfo.redinfo[m].plyoff)
				{
					num5 = n;
					int num13 = g_formations[g_tacticinfo.formationid][n].pos[1];
					switch (num13)
					{
					case 0:
						num6 = 1;
						break;
					case 1:
					case 2:
						num6 = 14;
						break;
					default:
						num6 = ((num13 < 3 || num13 > 5) ? 57344 : 8176);
						break;
					}
				}
				else
				{
					if (!IsFitToPlay(n))
					{
						continue;
					}
					int num14 = g_formations[g_tacticinfo.formationid][n].pos[1];
					switch (num14)
					{
					case 1:
					case 2:
						array[num8] = n;
						num8++;
						continue;
					case 0:
						continue;
					}
					if (num14 >= 3 && num14 <= 5)
					{
						array2[num9] = n;
						num9++;
					}
					else
					{
						array3[num10] = n;
						num10++;
					}
				}
			}
			bool flag = false;
			switch (num6)
			{
			case 14:
				if (num10 > 1)
				{
					num7 = array3[0];
					flag = true;
				}
				else if (num9 > 1)
				{
					num7 = array2[0];
					flag = true;
				}
				break;
			case 8176:
				if (num10 > 1)
				{
					num7 = array3[0];
					flag = true;
				}
				break;
			case 57344:
				if (num10 < 1 && num9 > 1)
				{
					num7 = array2[0];
					flag = true;
				}
				break;
			}
			if (flag)
			{
				int num4 = g_tacticaction.plyix[num7];
				g_tacticaction.plyix[num7] = g_tacticaction.plyix[num5];
				g_tacticaction.plyix[num5] = num4;
				tacticChange |= Constants.TacticChange.SWAPPLAYER;
				num3 = GetBestSub(tmix, num5);
				if (num3 != -1)
				{
					num4 = g_tacticaction.plyix[num5];
					g_tacticaction.plyix[num5] = num3;
					g_tacticaction.plyix[num3] = num4;
					tacticChange |= Constants.TacticChange.SUB;
				}
			}
		}
		for (int num15 = 0; num15 < 11; num15++)
		{
			Tactics.TacticalPos tacticalPosFromPosition = Tactics.GetTacticalPosFromPosition(FormationData.g_formations[g_tacticaction.formationid][num15].pos);
			Tactics.PlayerRoles description = (Tactics.PlayerRoles)PlayerData.GetDescription(g_tm[tmix].lineup[g_tacticaction.plyix[num15]]);
			int num16 = Array.IndexOf(Tactics.AvailableRoles[tacticalPosFromPosition], description);
			if (num16 < 0)
			{
				num16 = 0;
			}
			g_tacticaction.playerRoles[num15] = num16;
		}
		SetPlyInstructions(g_tm[tmix]);
		return tacticChange;
	}

	public static int GetBestSub(int tmix, int plyoff_plyix)
	{
		TacticInfo g_tacticinfo = Game.instance.g_tacticinfo;
		TacticAction g_tacticaction = Game.instance.g_tacticaction;
		Team[] g_tm = Game.instance.g_tm;
		int[,] g_black_gridpos = FormationData.g_black_gridpos;
		FormationPosData[][] g_formations = FormationData.g_formations;
		int num = 0;
		int num2 = 0;
		int num3 = 0;
		if (g_tacticinfo.numsubsmade < 3)
		{
			for (int i = 0; i < 4; i++)
			{
				int num4 = g_formations[g_tacticinfo.formationid][plyoff_plyix].pos[0];
				int num5 = g_formations[g_tacticinfo.formationid][plyoff_plyix].pos[1];
				switch (i)
				{
				case 0:
					num = g_black_gridpos[num5, num4];
					break;
				case 1:
					switch (num5)
					{
					case 0:
						num = 14;
						break;
					case 1:
					case 2:
						num = 14;
						break;
					default:
						num = ((num5 < 3 || num5 > 5) ? 57344 : 8176);
						break;
					}
					break;
				case 2:
					switch (num5)
					{
					case 0:
						num = 65520;
						break;
					case 1:
					case 2:
						num = 8176;
						break;
					default:
						num = ((num5 < 3 || num5 > 5) ? 8176 : 57358);
						break;
					}
					break;
				case 3:
					num = ((num5 >= 1 && num5 <= 2) ? 57345 : ((num5 >= 3 && num5 <= 5) ? 1 : 15));
					break;
				}
				for (int j = 11; j < 18; j++)
				{
					if (IsFitToPlay(j))
					{
						num2 = PlayerData.GetPosition(g_tm[tmix].lineup[g_tacticaction.plyix[j]]);
						if ((num & num2) != 0)
						{
							num3 = j;
							break;
						}
					}
				}
				if (num3 > 0)
				{
					break;
				}
			}
		}
		return num3;
	}

	public static void LoadTeam(Team tm, int tmix, short clubid, bool quickload, int strength, int aim)
	{
		LoadTeam(tm, tmix, clubid, quickload, strength, aim, -1, -1, -1);
	}

	public static void LoadTeam(Team tm, int tmix, short clubid, bool quickload, int strength, int aim, int fixtureCompType, int fixtureCompID, int oppTeamRatingDiff)
	{
		ManagedClub currentManagedClub = Game.instance.CurrentManagedClub;
		tm.clubid = clubid;
		tm.aim = aim;
		if (tm.clubid == currentManagedClub.ClubID && !Game.debugAutoPickTeam)
		{
			tm.formationid = currentManagedClub.currentTactics.inst.formationid;
			currentManagedClub.currentTactics.lineUp.CopyTo(tm.lineup, 0);
			for (int i = 0; i < tm.lineup.Length; i++)
			{
				if (tm.lineup[i] > 0)
				{
					tm.plyrating[i] = Player.GetRatingQuick(tm.lineup[i]);
				}
			}
			if (!quickload)
			{
				for (int j = 0; j < 11; j++)
				{
					PlayerStatusInfo status = Player.GetStatus(tm.lineup[j]);
					tm.shirtnum[j] = status.shirtNum;
					tm.fitness[j] = status.fitness;
					tm.morale[j] = status.morale;
				}
			}
			tm.tacklingstyle = currentManagedClub.currentTactics.inst.tacklingstyle.value;
			tm.mentality = currentManagedClub.currentTactics.inst.mentality.value;
			tm.passingstyleid = currentManagedClub.currentTactics.inst.passingstyleid.value;
			tm.attackingstyle = currentManagedClub.currentTactics.inst.attackingstyle.value;
			tm.tempoid = currentManagedClub.currentTactics.inst.tempoid.value;
			tm.pressingid = currentManagedClub.currentTactics.inst.pressingid.value;
			tm.counterattack = currentManagedClub.currentTactics.inst.counterattack;
			tm.menbehindball = currentManagedClub.currentTactics.inst.menbehindball;
			tm.tightmarking = currentManagedClub.currentTactics.inst.tightmarking;
			tm.playoffside = currentManagedClub.currentTactics.inst.playoffside;
			tm.useplaymaker = currentManagedClub.currentTactics.inst.useplaymaker;
			tm.usetargetman = currentManagedClub.currentTactics.inst.usetargetman;
			tm.captain = currentManagedClub.currentTactics.inst.captain.value;
			tm.penaltytaker = currentManagedClub.currentTactics.inst.penaltytaker.value;
			tm.deadballtakerleft = currentManagedClub.currentTactics.inst.deadballtakerleft.value;
			tm.deadballtakerright = currentManagedClub.currentTactics.inst.deadballtakerright.value;
			tm.cornertakerleft = currentManagedClub.currentTactics.inst.cornertakerleft.value;
			tm.cornertakerright = currentManagedClub.currentTactics.inst.cornertakerright.value;
			tm.width = currentManagedClub.currentTactics.inst.width.value;
			tm.backline = currentManagedClub.currentTactics.inst.backline.value;
			tm.creativity = currentManagedClub.currentTactics.inst.creativity.value;
			tm.attforwards = currentManagedClub.currentTactics.inst.attforwards.value;
			tm.attmidfielders = currentManagedClub.currentTactics.inst.attmidfielders.value;
			tm.attwingers = currentManagedClub.currentTactics.inst.attwingers.value;
			for (int k = 0; k < 11; k++)
			{
				tm.plyrinst[k] = currentManagedClub.currentTactics.inst.plyrinst[k];
			}
			for (int l = 0; l < 11; l++)
			{
				tm.playerRoles[l] = currentManagedClub.currentTactics.inst.playerRoles[l].value;
			}
			return;
		}
		tm.formationid = PickFormation(clubid);
		if (oppTeamRatingDiff > 5)
		{
			tm.formationid = GetCounterFormation(tm.formationid, attacking: true);
		}
		if (oppTeamRatingDiff <= -5)
		{
			tm.formationid = GetCounterFormation(tm.formationid, attacking: false);
		}
		PickTeam(tm, tm.formationid, strength, fixtureCompType, fixtureCompID, justFirstTeam: false);
		if (!quickload)
		{
			for (byte b = 0; b < 11; b++)
			{
				tm.shirtnum[b] = (byte)(b + 1);
				tm.fitness[b] = Player.GetFitness(tm.lineup[b]);
				tm.morale[b] = Player.GetMorale(tm.lineup[b]);
			}
		}
		PickTactics(tm, tmix, quickload, oppTeamRatingDiff);
		for (int m = 0; m < 11; m++)
		{
			tm.plyrinst[m] = 0;
		}
		for (int n = 0; n < 11; n++)
		{
			Tactics.TacticalPos tacticalPosFromPosition = Tactics.GetTacticalPosFromPosition(FormationData.g_formations[tm.formationid][n].pos);
			Tactics.PlayerRoles description = (Tactics.PlayerRoles)PlayerData.GetDescription(tm.lineup[n]);
			int num = Array.IndexOf(Tactics.AvailableRoles[tacticalPosFromPosition], description);
			if (num < 0)
			{
				num = 0;
			}
			tm.playerRoles[n] = num;
		}
	}

	public static bool IsFitToPlay(int plyix)
	{
		TacticInfo g_tacticinfo = Game.instance.g_tacticinfo;
		int num = g_tacticinfo.plyix[plyix];
		for (int i = 0; i < g_tacticinfo.numsubsmade; i++)
		{
			if (num == g_tacticinfo.subsmade[i].plyoff)
			{
				return false;
			}
		}
		for (int j = 0; j < g_tacticinfo.numinj; j++)
		{
			if (num == g_tacticinfo.injinfo[j].plyoff)
			{
				return false;
			}
		}
		for (int k = 0; k < g_tacticinfo.numreds; k++)
		{
			if (num == g_tacticinfo.redinfo[k].plyoff)
			{
				return false;
			}
		}
		return true;
	}

	public static Constants.TacticChange SetTmInstructions(int tmix, int timeseg, int gamestate)
	{
		Team team = Game.instance.g_tm[tmix];
		TacticInfo g_tacticinfo = Game.instance.g_tacticinfo;
		TacticAction g_tacticaction = Game.instance.g_tacticaction;
		g_tacticaction.tacklingstyle = team.tacklingstyle;
		g_tacticaction.mentality = team.mentality;
		g_tacticaction.passingstyleid = team.passingstyleid;
		g_tacticaction.attackingstyle = team.attackingstyle;
		g_tacticaction.tempoid = team.tempoid;
		g_tacticaction.pressingid = team.pressingid;
		g_tacticaction.width = team.width;
		g_tacticaction.creativity = team.creativity;
		g_tacticaction.backline = team.backline;
		g_tacticaction.attforwards = team.attforwards;
		g_tacticaction.attmidfielders = team.attmidfielders;
		g_tacticaction.attwingers = team.attwingers;
		g_tacticaction.playerRoles = new int[11];
		for (int i = 0; i < 11; i++)
		{
			g_tacticaction.playerRoles[i] = team.playerRoles[i];
		}
		int[] g_tm_aim = Game.instance.g_tm_aim;
		if (g_tm_aim[tmix] == 0)
		{
			short num = Constants.g_team_aims_drawok[timeseg][gamestate];
			if (num != 0)
			{
				g_tacticaction.tacklingstyle = (byte)(g_tacticaction.tacklingstyle + num);
			}
			g_tacticaction.mentality = (byte)(g_tacticaction.mentality + num);
			g_tacticaction.tempoid = (byte)(g_tacticaction.tempoid + num);
		}
		else if (g_tm_aim[tmix] == 1)
		{
			short num = Constants.g_team_aims_win[timeseg][gamestate];
			if (num != 0)
			{
				g_tacticaction.tacklingstyle = (byte)(g_tacticaction.tacklingstyle + num);
			}
			g_tacticaction.mentality = (byte)(g_tacticaction.mentality + num);
			g_tacticaction.tempoid = (byte)(g_tacticaction.tempoid + num);
		}
		else if (g_tm_aim[tmix] == 2)
		{
			short num = Constants.g_team_aims_mustwin[timeseg][gamestate];
			if (num != 0)
			{
				g_tacticaction.tacklingstyle = (byte)(g_tacticaction.tacklingstyle + num);
			}
			g_tacticaction.mentality = (byte)(g_tacticaction.mentality + num);
			g_tacticaction.tempoid = (byte)(g_tacticaction.tempoid + num);
		}
		if (g_tacticaction.tacklingstyle < 1)
		{
			g_tacticaction.tacklingstyle = 1;
		}
		if (g_tacticaction.tacklingstyle > 3)
		{
			g_tacticaction.tacklingstyle = 3;
		}
		if (g_tacticaction.mentality < 1)
		{
			g_tacticaction.mentality = 1;
		}
		if (g_tacticaction.mentality > 5)
		{
			g_tacticaction.mentality = 5;
		}
		if (g_tacticaction.tempoid < 1)
		{
			g_tacticaction.tempoid = 1;
		}
		if (g_tacticaction.tempoid > 3)
		{
			g_tacticaction.tempoid = 3;
		}
		if (g_tacticaction.width < 1)
		{
			g_tacticaction.width = 1;
		}
		if (g_tacticaction.width > 3)
		{
			g_tacticaction.width = 3;
		}
		if (g_tacticaction.creativity < 1)
		{
			g_tacticaction.creativity = 1;
		}
		if (g_tacticaction.creativity > 3)
		{
			g_tacticaction.creativity = 3;
		}
		if (g_tacticaction.backline < 1)
		{
			g_tacticaction.backline = 1;
		}
		if (g_tacticaction.backline > 3)
		{
			g_tacticaction.backline = 3;
		}
		if (g_tacticaction.attforwards < 1)
		{
			g_tacticaction.attforwards = 1;
		}
		if (g_tacticaction.attforwards > 2)
		{
			g_tacticaction.attforwards = 2;
		}
		if (g_tacticaction.attmidfielders < 1)
		{
			g_tacticaction.attmidfielders = 1;
		}
		if (g_tacticaction.attmidfielders > 2)
		{
			g_tacticaction.attmidfielders = 2;
		}
		if (g_tacticaction.attwingers < 1)
		{
			g_tacticaction.attwingers = 1;
		}
		if (g_tacticaction.attwingers > 2)
		{
			g_tacticaction.attwingers = 2;
		}
		if (g_tacticaction.mentality > g_tacticinfo.mentality)
		{
			return Constants.TacticChange.ATTACKING;
		}
		if (g_tacticaction.mentality < g_tacticinfo.mentality)
		{
			return Constants.TacticChange.DEFENSIVE;
		}
		return Constants.TacticChange.NONE;
	}

	public static int SetPlyInstructions(Team tm)
	{
		TacticAction g_tacticaction = Game.instance.g_tacticaction;
		TacticInfo g_tacticinfo = Game.instance.g_tacticinfo;
		g_tacticaction.counterattack = false;
		g_tacticaction.menbehindball = false;
		g_tacticaction.tightmarking = false;
		g_tacticaction.playoffside = false;
		g_tacticaction.useplaymaker = false;
		g_tacticaction.usetargetman = false;
		byte b = 7;
		byte b2 = 7;
		byte b3 = 7;
		byte b4 = 7;
		byte b5 = 0;
		byte b6 = 0;
		byte b7 = 0;
		byte b8 = 0;
		byte b9 = 0;
		byte b10 = 0;
		Dictionary<short, PlayerSquad> g_ClubPlayer = Game.instance.g_ClubPlayer;
		PlayerSquad players = PlayerSquad.GetPlayers(tm.clubid);
		int count = g_ClubPlayer[tm.clubid].Count;
		for (byte b11 = 1; b11 < 11; b11++)
		{
			bool flag = true;
			for (int i = 0; i < g_tacticinfo.numreds; i++)
			{
				if (g_tacticinfo.redinfo[i].plyoff == g_tacticaction.plyix[b11])
				{
					flag = false;
				}
			}
			for (int j = 0; j < g_tacticinfo.numinj; j++)
			{
				if (g_tacticinfo.injinfo[j].plyoff == g_tacticaction.plyix[b11])
				{
					flag = false;
				}
			}
			if (flag)
			{
				for (int k = 0; k < count; k++)
				{
					if (tm.lineup[g_tacticaction.plyix[b11]] != players[k])
					{
						continue;
					}
					byte b12 = g_ClubPlayer[tm.clubid].matchdayratings(k);
					if (b12 > b)
					{
						b = b12;
						g_tacticaction.captain = b11;
					}
					if ((PlayerData.GetPosition(tm.lineup[g_tacticaction.plyix[b11]]) & 0xE000) != 0)
					{
						if (b12 > b2)
						{
							b2 = b12;
							g_tacticaction.penaltytaker = b11;
						}
						if (b12 > b10)
						{
							b10 = b12;
							b8 = b11;
						}
					}
					if ((PlayerData.GetPosition(tm.lineup[g_tacticaction.plyix[b11]]) & 0x1FF0) != 0)
					{
						if (b12 > b3)
						{
							b3 = b12;
							g_tacticaction.deadballtakerleft = b11;
						}
						if (b12 > b4)
						{
							b4 = b12;
							g_tacticaction.cornertakerleft = b11;
						}
						if (b12 > b9)
						{
							b9 = b12;
							b6 = b11;
						}
					}
					int description = Player.GetDescription(tm.lineup[g_tacticaction.plyix[b11]]);
					if ((description == 21 || description == 24 || description == 28) && b12 > b5)
					{
						b5 = b12;
						g_tacticaction.playmaker = b11;
						g_tacticaction.useplaymaker = true;
					}
					if (description == 40 && b12 > b7)
					{
						b7 = b12;
						g_tacticaction.targetman = b11;
						g_tacticaction.usetargetman = true;
					}
				}
			}
		}
		if (b5 == 0)
		{
			g_tacticaction.playmaker = b6;
		}
		if (b7 == 0)
		{
			g_tacticaction.targetman = b8;
		}
		return 0;
	}

	public static int GetStrength(short clubid)
	{
		int num = 0;
		int num2 = 0;
		int num3 = Game.instance.g_ClubPlayer[clubid].Count - 1;
		while (num < 11 && num3 > 0)
		{
			num2 += Game.instance.g_ClubPlayer[clubid].matchdayratings(num3);
			num++;
			num3--;
		}
		return num2;
	}
}
