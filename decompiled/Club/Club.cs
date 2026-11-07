using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Club
{
	public class PlayedFixtureDetails
	{
		public DateTime fixturetime;

		public Fixture fixture;
	}

	public class MaxValueData
	{
		public float value;

		public int playerID;
	}

	public class ClubMinMaxRating
	{
		public int Min;

		public int Max;
	}

	public const int TopClubAverageRating = 93;

	public const byte MIN_NUM_GOALKEEPERS = 2;

	public const byte MIN_NUM_DEFENDERS = 5;

	public const byte MIN_NUM_MIDFIELDERS = 5;

	public const byte MIN_NUM_FORWARDS = 4;

	public short ClubDataID { get; set; }

	public string Name { get; set; } = "";

	public short CountryDataID { get; set; }

	public short AssociationID { get; set; }

	public short LeagueID { get; set; }

	public short StadiumDataID { get; set; }

	public RGYBCode RGYBCode { get; set; }

	public int ManagerID { get; set; }

	public short YearFounded { get; set; }

	public string HomeColour1 { get; set; }

	public string HomeColour2 { get; set; }

	public string AwayColour1 { get; set; }

	public string AwayColour2 { get; set; }

	public string LogoFilename { get; set; }

	public short KitID { get; set; }

	public short AwayKitID { get; set; }

	public string KitDesignFilename { get; set; }

	public string KitDesignAwayFilename { get; set; }

	public string HomeShortsColour1 { get; set; } = "";

	public string HomeShortsColour2 { get; set; } = "";

	public string AwayShortsColour1 { get; set; } = "";

	public string AwayShortsColour2 { get; set; } = "";

	public byte ShortsID { get; set; }

	public byte AwayShortsID { get; set; }

	public byte Reputation { get; set; }

	public int OrigFanbase { get; set; }

	public int CurrentFanbase { get; set; }

	public string ShortName { get; set; } = "";

	public string MediumName { get; set; } = "";

	public List<short> Rivals { get; set; }

	public List<short> BitterRivals { get; set; }

	public bool RichOwners { get; set; }

	public byte TeamFormation { get; set; }

	public byte TacklingStyle { get; set; }

	public byte Mentality { get; set; }

	public byte PassingStyleID { get; set; }

	public byte AttackingStyle { get; set; }

	public byte TempoID { get; set; }

	public byte PressingID { get; set; }

	public short Captain { get; set; }

	public short ViceCaptain { get; set; }

	public byte CoEfficient { get; set; }

	public byte winTally { get; set; }

	public byte unbeatenTally { get; set; }

	public byte cleanSheetsTally { get; set; }

	public List<byte> AvailableSquadNo { get; set; }

	public byte OrigAverageRating { get; set; }

	public List<short> TopPlayers { get; set; }

	public DateTime LastTransferOutDate { get; set; }

	public DateTime LastTransferInDate { get; set; }

	public byte transferTier { get; set; }

	public List<int> transferlist { get; set; } = new List<int>();

	public TransferProfile profile { get; } = new TransferProfile();

	public TransferTransactionsManager transferTransactionsManager { get; set; } = new TransferTransactionsManager();

	public Club()
	{
		TopPlayers = new List<short>();
		Rivals = new List<short>();
		BitterRivals = new List<short>();
		RichOwners = false;
	}

	public Club(string file, List<int> FieldsInFile)
	{
		short num = (short)DataPackHelpers.ParseSectionAsInt(FieldsInFile[0] + 1, FieldsInFile[1], file);
		CountryDataID = (short)DataPackHelpers.ParseSectionAsInt(FieldsInFile[2] + 1, FieldsInFile[3], file);
		Name = "Club " + DataPackHelpers.ParseSectionAsInt(FieldsInFile[18] + 1, FieldsInFile[19], file);
		StadiumDataID = (short)DataPackHelpers.ParseSectionAsInt(FieldsInFile[3] + 1, FieldsInFile[4], file);
		YearFounded = (short)DataPackHelpers.ParseSectionAsInt(FieldsInFile[4] + 1, FieldsInFile[5], file);
		HomeColour1 = DataPackHelpers.ParseSectionAsString(FieldsInFile[8] + 1, FieldsInFile[9], file);
		HomeColour2 = DataPackHelpers.ParseSectionAsString(FieldsInFile[9] + 1, FieldsInFile[10], file);
		AwayColour1 = DataPackHelpers.ParseSectionAsString(FieldsInFile[10] + 1, FieldsInFile[11], file);
		AwayColour2 = DataPackHelpers.ParseSectionAsString(FieldsInFile[11] + 1, FieldsInFile[12], file);
		KitID = (short)DataPackHelpers.ParseSectionAsInt(FieldsInFile[12] + 1, FieldsInFile[13], file);
		AwayKitID = (short)DataPackHelpers.ParseSectionAsInt(FieldsInFile[13] + 1, FieldsInFile[14], file);
		KitDesignFilename = DataPackHelpers.ParseSectionAsString(FieldsInFile[14] + 1, FieldsInFile[15], file);
		KitDesignAwayFilename = DataPackHelpers.ParseSectionAsString(FieldsInFile[15] + 1, FieldsInFile[16], file);
		Reputation = (byte)DataPackHelpers.ParseSectionAsInt(FieldsInFile[5] + 1, FieldsInFile[6], file);
		OrigFanbase = DataPackHelpers.ParseSectionAsInt(FieldsInFile[7] + 1, FieldsInFile[8], file);
		CurrentFanbase = DataPackHelpers.ParseSectionAsInt(FieldsInFile[7] + 1, FieldsInFile[8], file);
		ClubDataID = (short)DataPackHelpers.ParseSectionAsInt(FieldsInFile[18] + 1, FieldsInFile[19], file);
		TeamFormation = (byte)DataPackHelpers.ParseSectionAsInt(FieldsInFile[19] + 1, FieldsInFile[20], file);
		TacklingStyle = (byte)DataPackHelpers.ParseSectionAsInt(FieldsInFile[20] + 1, FieldsInFile[21], file);
		Mentality = (byte)DataPackHelpers.ParseSectionAsInt(FieldsInFile[21] + 1, FieldsInFile[22], file);
		PassingStyleID = (byte)DataPackHelpers.ParseSectionAsInt(FieldsInFile[22] + 1, FieldsInFile[23], file);
		AttackingStyle = (byte)DataPackHelpers.ParseSectionAsInt(FieldsInFile[23] + 1, FieldsInFile[24], file);
		TempoID = (byte)DataPackHelpers.ParseSectionAsInt(FieldsInFile[24] + 1, FieldsInFile[25], file);
		PressingID = (byte)DataPackHelpers.ParseSectionAsInt(FieldsInFile[25] + 1, FieldsInFile[26], file);
		Captain = (short)DataPackHelpers.ParseSectionAsInt(FieldsInFile[26] + 1, FieldsInFile[27], file);
		ViceCaptain = (short)DataPackHelpers.ParseSectionAsInt(FieldsInFile[27] + 1, FieldsInFile[28], file);
		CoEfficient = (byte)DataPackHelpers.ParseSectionAsInt(FieldsInFile[28] + 1, FieldsInFile[29], file);
		OrigAverageRating = (byte)DataPackHelpers.ParseSectionAsInt(FieldsInFile[30] + 1, FieldsInFile[31], file);
		TopPlayers = new List<short>
		{
			(short)DataPackHelpers.ParseSectionAsInt(FieldsInFile[31] + 1, FieldsInFile[32], file),
			(short)DataPackHelpers.ParseSectionAsInt(FieldsInFile[32] + 1, FieldsInFile[33], file),
			(short)DataPackHelpers.ParseSectionAsInt(FieldsInFile[33] + 1, FieldsInFile[34], file)
		};
		Rivals = new List<short>();
		BitterRivals = new List<short>();
		RichOwners = false;
		if (FormationData.invalidFormationIds.Contains(TeamFormation))
		{
			AssignValidFormationID();
		}
		AvailableSquadNo = new List<byte>();
		for (byte b = 1; b < 100; b++)
		{
			AvailableSquadNo.Add(b);
		}
		bool flag = false;
		Dictionary<byte, byte> lEAGUE_TRANSFER_TIERS = TransferLeagueData.LEAGUE_TRANSFER_TIERS;
		Dictionary<short, byte> cLUB_TRANSFER_TIERS = TransferLeagueData.CLUB_TRANSFER_TIERS;
		foreach (int key in Game.instance.g_LeagueData.Keys)
		{
			if (flag)
			{
				continue;
			}
			LeagueData leagueData = Game.instance.g_LeagueData[key];
			for (int i = 0; i < leagueData.ClubIDs.Count; i++)
			{
				if (leagueData.ClubIDs[i] != num)
				{
					continue;
				}
				AssociationID = leagueData.AssociationID;
				LeagueID = (short)key;
				if (transferTier == 0)
				{
					transferTier = lEAGUE_TRANSFER_TIERS[(byte)key];
					if (cLUB_TRANSFER_TIERS.ContainsKey(ClubDataID) && cLUB_TRANSFER_TIERS[ClubDataID] < transferTier)
					{
						transferTier = cLUB_TRANSFER_TIERS[ClubDataID];
					}
				}
				flag = true;
				break;
			}
		}
		foreach (int key2 in Game.instance.g_UnplayableLeagueData.Keys)
		{
			if (flag)
			{
				continue;
			}
			LeagueData leagueData2 = Game.instance.g_UnplayableLeagueData[key2];
			for (int j = 0; j < leagueData2.ClubIDs.Count; j++)
			{
				if (leagueData2.ClubIDs[j] == num)
				{
					AssociationID = leagueData2.AssociationID;
					LeagueID = (short)key2;
					flag = true;
					break;
				}
			}
		}
	}

	public static bool isClubActive(short clubId)
	{
		short associationID = GetAssociationID(clubId);
		bool flag = Game.instance.ChosenCountries.Contains(associationID);
		if (!flag)
		{
			flag = Continents.UnplayableTopDivisionsClubListContains(clubId);
			if (!flag)
			{
				flag = ContinentalComp.IsClubLikelyInvolvedInClubWorldCup(clubId);
			}
		}
		return flag;
	}

	public void removePlayerFromClubTransferList(int playerId, bool isManagedClub)
	{
		int num = transferlist.IndexOf(playerId);
		if (num != -1)
		{
			transferlist.RemoveAt(num);
		}
		if (isManagedClub)
		{
			Transfers.instance.transferListManager.RemovePlayerFromTransferList(playerId);
		}
	}

	public static void RetirePlayer(int playerId, short clubId, bool refreshClub = true)
	{
		Club club = Game.instance.g_ClubData[clubId];
		TransferProfile transferProfile = club.profile;
		transferProfile.removePlayer(playerId);
		if (refreshClub)
		{
			transferProfile.RefreshClubTopPlayers(clubId);
		}
		club.removePlayerFromClubTransferList(playerId, clubId == Game.instance.CurrentManagedClubId);
	}

	private void AssignValidFormationID()
	{
		switch (TeamFormation)
		{
		case 7:
			TeamFormation = 17;
			break;
		case 5:
			TeamFormation = 1;
			break;
		case 18:
			TeamFormation = 17;
			break;
		case 21:
			TeamFormation = 23;
			break;
		case 25:
			TeamFormation = 6;
			break;
		default:
			TeamFormation = 0;
			break;
		}
	}

	internal static bool WillBuyPlayers(short clubId, DateTime oneWeekAgo)
	{
		if (Game.instance.g_ClubData[clubId].LastTransferInDate > oneWeekAgo)
		{
			return false;
		}
		if (Game.instance.g_ClubPlayer[clubId].Count > 100)
		{
			return false;
		}
		return true;
	}

	public static byte GetCoefficient(short clubid)
	{
		if (Game.instance.g_ClubData.ContainsKey(clubid))
		{
			return Game.instance.g_ClubData[clubid].CoEfficient;
		}
		return 0;
	}

	public static bool GetRichOwners(short clubid)
	{
		if (Game.instance.g_ClubData.ContainsKey(clubid))
		{
			return Game.instance.g_ClubData[clubid].RichOwners;
		}
		return false;
	}

	public void LoadNamePack(string nameString, string shortNameString, string fileUrlString)
	{
		Name = nameString;
		LogoFilename = fileUrlString;
		ShortName = shortNameString;
	}

	public void LoadNamePack(string[] fields)
	{
		Name = fields[1].Replace('ş', 's').Replace("&#39;", "'");
		LogoFilename = fields[3];
		if (fields.Length > 5)
		{
			ShortName = fields[5];
		}
	}

	public static void InitRGYBCodes()
	{
		foreach (Club value in Game.instance.g_ClubData.Values)
		{
			value.RGYBCode = RGYBCodeHelpers.GetRandomRGYBCode();
		}
	}

	public static RGYBCode GetRGYBCode(short clubID)
	{
		return Game.instance.g_ClubData[clubID].RGYBCode;
	}

	public static List<int> GetStaff(int clubid)
	{
		return GetStaff(clubid, -1);
	}

	public static List<int> GetStaff(int clubid, int stafftype)
	{
		List<int> list = new List<int>();
		int num = 0;
		List<int> list2 = new List<int>();
		Dictionary<int, int> dictionary = new Dictionary<int, int>();
		foreach (int key in Game.instance.g_staffdata.Keys)
		{
			Staff staff = Game.instance.g_staffdata[key];
			if (staff.ClubID == clubid && (stafftype == -1 || staff.Type == stafftype))
			{
				list2.Add(key);
				dictionary.Add(key, num);
				num++;
			}
		}
		List<TransferBid> g_stafftransferbids = Game.instance.g_stafftransferbids;
		int num2 = g_stafftransferbids.Count - 1;
		while (num2 > -1 && g_stafftransferbids[num2].date > Game.instance.CurrentDate)
		{
			short clubIdFrom = g_stafftransferbids[num2].clubIdFrom;
			short clubIdTo = g_stafftransferbids[num2].clubIdTo;
			eBidStatus processed = g_stafftransferbids[num2].processed;
			int playerId = g_stafftransferbids[num2].playerId;
			if (stafftype == -1 || Staff.GetType(playerId) == stafftype)
			{
				if (clubIdFrom == clubid && processed == eBidStatus.FINALISED)
				{
					if (dictionary.ContainsKey(playerId))
					{
						list2[dictionary[playerId]] = -1;
					}
				}
				else if (clubIdTo == clubid && (processed == eBidStatus.FINALISED || processed == eBidStatus.RELEASED))
				{
					if (dictionary.ContainsKey(playerId))
					{
						list2[dictionary[playerId]] = playerId;
					}
					else
					{
						list2.Add(playerId);
						dictionary.Add(playerId, num);
						num++;
					}
				}
			}
			num2--;
		}
		for (int i = 0; i < num; i++)
		{
			if (list2[i] != -1)
			{
				list.Add(list2[i]);
			}
		}
		return list;
	}

	public static int GetWages(short clubID)
	{
		int num = 0;
		PlayerSquad players = GetPlayers(clubID);
		for (int i = 0; i < players.Count; i++)
		{
			num += Player.GetWages(players[i]);
		}
		GetStaff(clubID, -1);
		for (int j = 0; j < players.Count; j++)
		{
			num += Staff.GetWages(players[j], clubID);
		}
		return num;
	}

	public static List<PlayedFixtureDetails> GetPlayedFixtures(short clubid, DateTime startdate, DateTime enddate)
	{
		List<PlayedFixtureDetails> list = new List<PlayedFixtureDetails>();
		if (clubid == 0)
		{
			return list;
		}
		FixtureSetList schedule = Game.instance.g_season.Schedule.schedule;
		if (schedule != null)
		{
			int num = schedule.Count();
			for (int i = 0; i < num; i++)
			{
				if (!(schedule[i].Date >= startdate) || !(schedule[i].Date <= enddate))
				{
					continue;
				}
				DateTime date = schedule[i].Date;
				int count = schedule[i].Fixtures.Count;
				for (int j = 0; j < count; j++)
				{
					Fixture fixture = schedule[i].Fixtures[j];
					if (fixture.IsPlayed && (fixture.homeClubId == clubid || fixture.awayClubId == clubid))
					{
						PlayedFixtureDetails playedFixtureDetails = new PlayedFixtureDetails();
						playedFixtureDetails.fixturetime = date;
						playedFixtureDetails.fixture = fixture;
						list.Add(playedFixtureDetails);
						break;
					}
				}
			}
		}
		return list;
	}

	public static Dictionary<int, string> GetAllPlayersForm(short clubid)
	{
		int num = 6;
		Dictionary<int, List<float>> dictionary = new Dictionary<int, List<float>>();
		Dictionary<int, string> dictionary2 = new Dictionary<int, string>();
		PlayerSquad players = GetPlayers(clubid);
		int count = players.Count;
		for (int i = 0; i < count; i++)
		{
			int key = players[i];
			dictionary[key] = new List<float>();
		}
		short associationID = GetAssociationID(clubid);
		string currentSeasonID = Association.GetCurrentSeasonID(associationID);
		DateTime seasonStartDate = Association.GetSeasonStartDate(associationID, currentSeasonID);
		List<PlayedFixtureDetails> playedFixtures = GetPlayedFixtures(clubid, seasonStartDate, Game.instance.CurrentDate);
		for (int num2 = playedFixtures.Count - 1; num2 >= 0; num2--)
		{
			if (playedFixtures[num2] != null)
			{
				Fixture fixture = playedFixtures[num2].fixture;
				for (int j = 0; j < 18; j++)
				{
					float num3;
					int playerId;
					if (fixture.homeClubId == clubid)
					{
						num3 = (int)fixture.tm1[j].performance;
						playerId = fixture.tm1[j].playerId;
					}
					else
					{
						num3 = (int)fixture.tm2[j].performance;
						playerId = fixture.tm2[j].playerId;
					}
					if (dictionary[playerId] != null && dictionary[playerId].Count < num && num3 > 0f)
					{
						dictionary[playerId].Add(num3);
					}
				}
			}
		}
		for (int k = 0; k < count; k++)
		{
			int key2 = players[k];
			dictionary2[key2] = "";
			for (int l = 0; l < num; l++)
			{
				dictionary2[key2] = "-" + dictionary2[key2];
			}
			dictionary2[key2] = dictionary2[key2].Substring(1);
		}
		return dictionary2;
	}

	public static List<byte> GetAvailableSquadNo(short clubid)
	{
		if (Game.instance.g_ClubData.ContainsKey(clubid))
		{
			return Game.instance.g_ClubData[clubid].AvailableSquadNo;
		}
		return null;
	}

	public static void SetSquadNoAvailablity(short clubid, byte squadNo, bool available)
	{
		if (clubid > 0)
		{
			Game.instance.g_ClubData[clubid].SetSquadNoAvailability(squadNo, available);
		}
	}

	public void SetSquadNoAvailability(byte squadNo, bool available)
	{
		if (squadNo != 0)
		{
			if (available)
			{
				AvailableSquadNo.Add(squadNo);
			}
			else
			{
				AvailableSquadNo.Remove(squadNo);
			}
		}
	}

	public static List<int> GetUnassignedPlayers(short clubid)
	{
		List<int> list = new List<int>();
		PlayerSquad players = GetPlayers(clubid);
		for (int i = 0; i < players.Count; i++)
		{
			int num = players[i];
			if (PlayerData.GetSquadNo(num) == 0)
			{
				list.Add(num);
			}
		}
		return list;
	}

	public static int GetAverageWages(short clubid)
	{
		PlayerSquad players = GetPlayers(clubid);
		int num = 0;
		int count = players.Count;
		for (int i = 0; i < count; i++)
		{
			num += Player.GetWages(players[i]);
		}
		return (int)Math.Round((decimal)num / (decimal)players.Count);
	}

	public static int GetAverageAge(short clubid)
	{
		int numPlayers = GetNumPlayers(clubid);
		return (int)Math.Round((decimal)(GetPlayers(clubid).players.Sum((int i) => Player.GetAge(i)) / numPlayers), 0, MidpointRounding.AwayFromZero);
	}

	public static int GetYouthPlayedMinutes(short clubid, string seasonID)
	{
		int num = 0;
		DateTime seasonStartDate = Association.GetSeasonStartDate(GetAssociationID(Game.GetCurrentUserClubID()), seasonID);
		List<int> players = GetPlayers(Game.GetCurrentUserClubID()).players;
		for (int i = 0; i < players.Count; i++)
		{
			if (Player.GetAge(players[i], seasonStartDate) <= 22)
			{
				num += PlayerCareerStats.GetSeasonStats(players[i], seasonID).GetStat(StatType.MinsPlayed);
			}
		}
		return num;
	}

	public static int GetFanbaseSize(short clubid)
	{
		int currentUserClubID = Game.GetCurrentUserClubID();
		if (Game.instance.g_ClubData.ContainsKey(clubid))
		{
			if (clubid == currentUserClubID)
			{
				RecalcFanbaseSize(clubid);
			}
			return Game.instance.g_ClubData[clubid].CurrentFanbase;
		}
		return 0;
	}

	public static void RecalcFanbaseSize(short clubid)
	{
		if (Game.instance.g_ClubData.ContainsKey(clubid))
		{
			int leagueDataId = GetLeagueDataId(clubid);
			int stadiumDataID = GetStadiumDataID(clubid);
			byte averageRating = GetAverageRating(clubid);
			byte averageRatingOrig = GetAverageRatingOrig(clubid);
			int ratingQuick = LeagueData.GetRatingQuick((short)leagueDataId);
			int ratingOrig = LeagueData.GetRatingOrig(leagueDataId);
			int size = StadiumData.GetSize(stadiumDataID);
			int sizeOrig = StadiumData.GetSizeOrig(stadiumDataID);
			double num = Math.Pow((float)(int)averageRating / (float)(int)averageRatingOrig, 4.0) * Math.Pow((float)ratingQuick / (float)ratingOrig, 4.0) * (double)Game.instance.g_ClubData[clubid].OrigFanbase * Math.Pow((float)size / (float)sizeOrig, 0.75);
			Game.instance.g_ClubData[clubid].CurrentFanbase = (int)num;
		}
	}

	public static MaxValueData GetMaxValuePlayer(short clubid)
	{
		PlayerSquad players = GetPlayers(clubid);
		int count = players.Count;
		float num = 0f;
		int playerID = 0;
		for (int i = 0; i < count; i++)
		{
			float value = Player.GetValue(players[i]);
			if (value > num)
			{
				playerID = players[i];
				num = value;
			}
		}
		return new MaxValueData
		{
			value = num,
			playerID = playerID
		};
	}

	public static List<int> GetLoanedOutPlayers(short clubId)
	{
		return new List<int>(PlayerSquad.GetLoanedOutPlayers(clubId));
	}

	public static int GetTopScorer(short clubId)
	{
		int result = 0;
		int num = 0;
		foreach (int item in PlayerSquad.AllPlayers(clubId))
		{
			StatContainer currentLeagueStats = PlayerCareerStats.GetCurrentLeagueStats(item);
			if (currentLeagueStats.GetStat(StatType.Goal) > num)
			{
				result = item;
				num = currentLeagueStats.GetStat(StatType.Goal);
			}
		}
		return result;
	}

	public static int GetPlayerWages(short clubid)
	{
		PlayerSquad players = GetPlayers(clubid);
		int num = 0;
		for (int i = 0; i < players.Count; i++)
		{
			num += Player.GetWages(players[i]);
		}
		return num;
	}

	public static byte GetPredictedLeaguePos(short clubid)
	{
		byte leaguePosition = LeagueList.GetLeaguePosition(clubid);
		List<short> clubsByValue = ChosenLeague.GetClubsByValue(GetLeagueDataId(clubid));
		List<int> list = new List<int>();
		foreach (short item in clubsByValue)
		{
			list.Add(GetAverageRating(item));
		}
		int num = 0;
		foreach (int item2 in list)
		{
			num += item2;
		}
		_ = num / clubsByValue.Count;
		return leaguePosition;
	}

	public static byte GetAveragePotenial(short clubid)
	{
		Dictionary<int, int> dictionary = new Dictionary<int, int>();
		PlayerSquad players = GetPlayers(clubid);
		for (int i = 0; i < players.Count; i++)
		{
			PlayerData playerData = Game.instance.g_PlayerData[players[i]];
			if (playerData.Age() < 23)
			{
				dictionary.Add(players[i], playerData.Potential);
			}
		}
		dictionary = dictionary.OrderByDescending((KeyValuePair<int, int> kpv) => kpv.Value).ToDictionary((KeyValuePair<int, int> kpv) => kpv.Key, (KeyValuePair<int, int> kpv) => kpv.Value);
		short num = 1;
		float num2 = 0f;
		foreach (int key in dictionary.Keys)
		{
			if (num <= 11)
			{
				num2 += (float)dictionary[key];
				num++;
			}
		}
		return (byte)(num2 / (float)num);
	}

	public static byte GetAverageRatingOrig(short clubid)
	{
		if (Game.instance.g_ClubData[clubid].OrigAverageRating <= 0)
		{
			return 30;
		}
		return Game.instance.g_ClubData[clubid].OrigAverageRating;
	}

	public static byte GetAverageRating(short clubid, bool forceAccuratePlayerRating = false)
	{
		PlayerSquad players = GetPlayers(clubid);
		List<byte> list = new List<byte>();
		float num = 0f;
		float num2 = 0f;
		int count = players.Count;
		if (count != 0)
		{
			for (int i = 0; i < count; i++)
			{
				if ((Game.instance.managedClub != null && clubid == Game.instance.CurrentManagedClubId) | forceAccuratePlayerRating)
				{
					list.Add(Player.GetRating(players[i], DateTime.MinValue));
				}
				else
				{
					list.Add(Player.GetRatingQuick(players[i]));
				}
			}
			list.Sort();
			list.Reverse();
			int num3 = 0;
			for (num3 = 0; num3 < count && num3 < 11; num3++)
			{
				num += (float)(int)list[num3];
			}
			num2 = num / (float)num3;
		}
		else
		{
			num2 = 60f;
		}
		return (byte)Math.Round(num2);
	}

	public static float GetAverageRatingPrecise(short clubid)
	{
		PlayerSquad players = GetPlayers(clubid);
		List<int> list = new List<int>();
		float num = 0f;
		int count = players.Count;
		for (int i = 0; i < count; i++)
		{
			list.Add(Player.GetRatingQuick(players[i]));
		}
		list.Sort();
		list.Reverse();
		int num2 = 0;
		for (num2 = 0; num2 < count && num2 < 11; num2++)
		{
			num += (float)list[num2];
		}
		return num / (float)num2;
	}

	public static byte GetTopFifteenTeamRating(short clubid, int chosenPlayers = 15)
	{
		List<int> list = new List<int>(PlayerSquad.GetPlayers(clubid).players.OrderByDescending((int i) => Player.GetDisplayRating(i)).ToList());
		float num = 0f;
		int num2 = Math.Min(list.Count, chosenPlayers);
		for (int num3 = 0; num3 < num2; num3++)
		{
			num += (float)Player.GetDisplayRating(list[num3]);
		}
		return (byte)(num / (float)num2);
	}

	public static bool CheckMaxFirstTeam(short clubID)
	{
		bool result = true;
		int num = 0;
		PlayerSquad players = GetPlayers(clubID);
		for (int i = 0; i < players.Count; i++)
		{
			if (Player.GetStatus(players[i]).firstTeam)
			{
				num++;
			}
		}
		if (num >= 25)
		{
			result = false;
		}
		return result;
	}

	public static bool CheckMinFirstTeam(short clubID)
	{
		bool result = true;
		int num = 0;
		PlayerSquad players = GetPlayers(clubID);
		for (int i = 0; i < players.Count; i++)
		{
			if (Player.GetStatus(players[i]).firstTeam)
			{
				num++;
			}
		}
		if (num <= 20)
		{
			result = false;
		}
		return result;
	}

	public static ClubMinMaxRating GetPlayersMaxMinRating(short clubid, int idxfrom, int idxto)
	{
		PlayerSquad players = GetPlayers(clubid);
		List<byte> list = new List<byte>();
		int num = 99;
		int num2 = 0;
		int count = players.Count;
		for (int i = 0; i < Math.Max(count, idxto); i++)
		{
			if (players.Count > i)
			{
				list.Add(Player.GetRatingQuick(players[i]));
			}
			else
			{
				list.Add(60);
			}
		}
		list.Sort((byte a, byte b) => b - a);
		for (int num3 = idxfrom; num3 < idxto; num3++)
		{
			if (list[num3] < num)
			{
				num = list[num3];
			}
			if (list[num3] > num2)
			{
				num2 = list[num3];
			}
		}
		return new ClubMinMaxRating
		{
			Min = num,
			Max = num2
		};
	}

	public static int GetAttendance(short clubID)
	{
		int val = ((Game.GetCurrentUserClubID() != clubID) ? Economy.CalcClubAttendance(clubID) : Game.instance.CurrentManagedClub.Attendance);
		return Math.Max(0, val);
	}

	public bool WillSellPlayers(short clubId)
	{
		if (Game.instance.g_ClubPlayer[clubId].Count <= 22)
		{
			return false;
		}
		return true;
	}

	public static int GetNumPlayers(short ID)
	{
		if (!Game.instance.g_ClubPlayer.TryGetValue(ID, out var value))
		{
			return 0;
		}
		return value.Count;
	}

	public static string GetLink(short ID)
	{
		if (ID == 0)
		{
			return Translation.GetText("free-agent");
		}
		return Helpers.GetLinkText(GetName(ID), Helpers.LinkTextType.Club, ID);
	}

	public static string GetLogoFilename(short ID)
	{
		if (Game.instance.g_ClubData.ContainsKey(ID))
		{
			return Game.instance.g_ClubData[ID].LogoFilename;
		}
		return "";
	}

	public static string GetName(short ID)
	{
		return GetName(ID, appendClubID: false);
	}

	public static string GetName(short ID, bool appendClubID)
	{
		string text = "UNKNOWN";
		if (ID == 0)
		{
			return Translation.GetText("free-agent");
		}
		try
		{
			text = Game.instance.g_ClubData[ID].Name;
			if (appendClubID)
			{
				text = text + " (" + ID + ")";
			}
		}
		catch (Exception)
		{
		}
		return text;
	}

	public static string GetShortName(short ID)
	{
		string text = "";
		text = ((!(Game.instance.g_ClubData[ID].ShortName != "") || !(Game.instance.g_ClubData[ID].ShortName != "TBC")) ? GetName(ID) : Game.instance.g_ClubData[ID].ShortName);
		return text.Substring(0, Math.Min(3, text.Length));
	}

	public static string GetMediumName(short ID)
	{
		string text = "";
		if (Game.instance.g_ClubData[ID].MediumName != "" && Game.instance.g_ClubData[ID].MediumName != "TBC")
		{
			return Game.instance.g_ClubData[ID].MediumName;
		}
		return GetName(ID);
	}

	public static short GetClubDataID(short ID)
	{
		if (Game.instance.g_ClubData.ContainsKey(ID))
		{
			return Game.instance.g_ClubData[ID].ClubDataID;
		}
		return 0;
	}

	public static int GetStadiumDataID(short ID)
	{
		if (Game.instance.g_ClubData.ContainsKey(ID))
		{
			return Game.instance.g_ClubData[ID].StadiumDataID;
		}
		return 0;
	}

	public static short GetReputation(short ID)
	{
		if (Game.instance.g_ClubData.ContainsKey(ID))
		{
			return Game.instance.g_ClubData[ID].Reputation;
		}
		return 0;
	}

	public static short GetClubsLeagueNumTeams(short ID)
	{
		return ChosenLeague.GetNumTeams(GetLeagueDataId(ID));
	}

	public static PlayerSquad GetPlayers(short clubid)
	{
		if (!Game.instance.g_ClubPlayer.ContainsKey(clubid))
		{
			if (clubid == 0)
			{
				Game.instance.g_ClubPlayer[clubid] = new PlayerSquad();
				return Game.instance.g_ClubPlayer[clubid];
			}
			if (!Game.instance.g_ClubData.ContainsKey(clubid))
			{
				Debug.LogError("Club.GetPlayers(): clubid not in g_ClubData: ID: " + clubid);
			}
			return null;
		}
		return Game.instance.g_ClubPlayer[clubid];
	}

	public static List<int> GetPlayersInclLoanedOut(short clubid)
	{
		List<int> list = new List<int>();
		PlayerSquad playerSquad = Game.instance.g_ClubPlayer[clubid];
		for (int i = 0; i < playerSquad.Count; i++)
		{
			list.Add(playerSquad.players[i]);
		}
		list.AddRange(PlayerSquad.GetLoanedOutPlayers(clubid));
		return list;
	}

	public static List<int> GetPlayersByPosition(short clubid, int requiredPosition, bool preferedPos = false)
	{
		List<int> list = new List<int>();
		PlayerSquad playerSquad = Game.instance.g_ClubPlayer[clubid];
		for (int i = 0; i < playerSquad.Count; i++)
		{
			int num = playerSquad.players[i];
			if ((((preferedPos && PlayerData.GetPreferredPosition(num) > 0) ? PlayerData.GetPreferredPosition(num) : PlayerData.GetPosition(num)) & requiredPosition) > 0)
			{
				list.Add(num);
			}
		}
		return list;
	}

	public static List<int> GetPlayersByDescription(short clubid, byte requiredDesc)
	{
		List<int> list = new List<int>();
		PlayerSquad playerSquad = Game.instance.g_ClubPlayer[clubid];
		for (int i = 0; i < playerSquad.Count; i++)
		{
			int num = playerSquad.players[i];
			if (Game.instance.g_PlayerData[num].GetPlayerDescriptionFromPosition(Player.GetPosition(num)) == requiredDesc)
			{
				list.Add(num);
			}
		}
		return list;
	}

	public static List<int> GetTopPlayers(short clubid, short numTopPlayers = 11)
	{
		Dictionary<int, byte> playerRatings = new Dictionary<int, byte>();
		PlayerSquad playerSquad = Game.instance.g_ClubPlayer[clubid];
		List<int> list = new List<int>(playerSquad.players.Take(playerSquad.Count));
		for (int i = 0; i < playerSquad.Count; i++)
		{
			playerRatings[playerSquad.players[i]] = Player.GetRating(playerSquad.players[i]);
		}
		list.Sort((int a, int b) => playerRatings[b] - playerRatings[a]);
		return new List<int>(list.Take(Math.Min(playerSquad.Count, numTopPlayers)));
	}

	public static int GetTopPlayer(short clubid)
	{
		return GetPlayers(clubid).players.OrderByDescending((int i) => Player.GetRating(i)).First();
	}

	public static bool AreSimilarRatedTeams(short clubid1, short clubid2)
	{
		if (Math.Abs(GetAverageRating(clubid1) - GetAverageRating(clubid2)) < 2)
		{
			return true;
		}
		return false;
	}

	public static bool IsKeyGame(short clubid1, short clubid2)
	{
		bool result = false;
		short numberOfLeagueGamesPlayed = GetNumberOfLeagueGamesPlayed(clubid1);
		byte predictedLeaguePosition = GetPredictedLeaguePosition(clubid1);
		byte predictedLeaguePosition2 = GetPredictedLeaguePosition(clubid2);
		if (numberOfLeagueGamesPlayed > 30)
		{
			if ((predictedLeaguePosition - predictedLeaguePosition2 < 3 && predictedLeaguePosition - predictedLeaguePosition2 >= 0) || (predictedLeaguePosition2 - predictedLeaguePosition < 3 && predictedLeaguePosition2 - predictedLeaguePosition >= 0))
			{
				result = true;
			}
		}
		else if (numberOfLeagueGamesPlayed > 15 && ((predictedLeaguePosition - predictedLeaguePosition2 < 5 && predictedLeaguePosition - predictedLeaguePosition2 >= 0) || (predictedLeaguePosition2 - predictedLeaguePosition < 5 && predictedLeaguePosition2 - predictedLeaguePosition >= 0)))
		{
			result = true;
		}
		return result;
	}

	public static bool AreRivals(short clubid1, short clubid2)
	{
		if (Game.instance.g_ClubData[clubid1].Rivals.IndexOf(clubid2) != -1 || Game.instance.g_ClubData[clubid2].Rivals.IndexOf(clubid1) != -1)
		{
			return true;
		}
		return false;
	}

	public static bool AreBitterRivals(short clubid1, short clubid2)
	{
		if (clubid1 > 0 && clubid2 > 0 && (Game.instance.g_ClubData[clubid1].BitterRivals.IndexOf(clubid2) != -1 || Game.instance.g_ClubData[clubid2].BitterRivals.IndexOf(clubid1) != -1))
		{
			return true;
		}
		return false;
	}

	public static short GetCountryDataID(short clubID)
	{
		if (Game.instance.g_ClubData.ContainsKey(clubID))
		{
			return Game.instance.g_ClubData[clubID].CountryDataID;
		}
		return 0;
	}

	public static short GetAssociationID(short clubID)
	{
		if (clubID == 0 || !Game.instance.g_ClubData.ContainsKey(clubID))
		{
			return 2;
		}
		return Game.instance.g_ClubData[clubID].AssociationID;
	}

	public static short? GetContinentID(short clubID)
	{
		short? result = null;
		if (Game.instance.g_ClubData.ContainsKey(clubID))
		{
			short associationID = GetAssociationID(clubID);
			List<short> list = new List<short>
			{
				64, 68, 107, 74, 81, 225, 193, 17, 20, 53,
				56, 57, 160, 168, 182, 190, 192, 211, 208, 231,
				181, 87
			};
			List<short> list2 = new List<short> { 32, 13, 41, 48, 232 };
			List<short> list3 = new List<short> { 138, 233 };
			List<short> list4 = new List<short> { 42, 111, 118, 16 };
			if (list.Contains(associationID))
			{
				result = 1;
			}
			else if (list2.Contains(associationID))
			{
				result = 2;
			}
			else if (list3.Contains(associationID))
			{
				result = 3;
			}
			else if (list4.Contains(associationID))
			{
				result = 4;
			}
		}
		return result;
	}

	public static byte GetDivision(short clubID)
	{
		int leagueDataId = GetLeagueDataId(clubID);
		if (Game.instance.g_LeagueData.ContainsKey(leagueDataId))
		{
			return (byte)(Game.instance.g_LeagueData[leagueDataId].Division - 1);
		}
		return byte.MaxValue;
	}

	public static byte GetPredictedLeaguePosition(short clubId)
	{
		int num = -1;
		ChosenLeague chosenLeague = null;
		for (int i = 0; i < Game.instance.g_season.Leagues.Count; i++)
		{
			chosenLeague = Game.instance.g_season.Leagues[i];
			if (Array.IndexOf(chosenLeague.ClubID, clubId) > -1)
			{
				num = i;
				break;
			}
		}
		if (num != -1 && chosenLeague != null)
		{
			List<short> list = chosenLeague.ClubID.Where((short x) => x != 0).ToList();
			Dictionary<short, short> clubRating = ((IEnumerable<short>)list).ToDictionary((Func<short, short>)((short x) => x), (Func<short, short>)((short x) => GetTopFifteenTeamRating(x)));
			list.Sort((short a, short b) => clubRating[b].CompareTo(clubRating[a]));
			return (byte)list.IndexOf(clubId);
		}
		return 0;
	}

	public static int GetClubPerformanceInLeague(short clubId)
	{
		short num = (short)(GetPredictedLeaguePosition(clubId) - LeagueList.GetLeaguePosition(clubId));
		if (num > -3 && num < 3)
		{
			return 0;
		}
		if (num <= 0)
		{
			return -1;
		}
		return 1;
	}

	public static short GetNumberOfLeagueGamesPlayed(int clubId)
	{
		int num = -1;
		ChosenLeague chosenLeague = null;
		for (int i = 0; i < Game.instance.g_season.Leagues.Count; i++)
		{
			chosenLeague = Game.instance.g_season.Leagues[i];
			num = Array.IndexOf(chosenLeague.ClubID, (short)clubId);
			if (num > -1)
			{
				break;
			}
		}
		if (num > -1 && chosenLeague != null)
		{
			return chosenLeague.GamesPld[num];
		}
		return -1;
	}

	public static bool IsTeamPlayingWell(short ClubId)
	{
		return Game.instance.ManagedClubProfile.GetFanConfidence() >= 40;
	}

	public static int GetCurrentInjuryCount(short clubId)
	{
		PlayerSquad players = GetPlayers(clubId);
		int num = 0;
		for (int i = 0; i < players.Count; i++)
		{
			if (Player.IsInjured(players[i]))
			{
				num++;
			}
		}
		return num;
	}

	public static int GetLeagueDataId(short clubID)
	{
		if (Game.instance.g_ClubData.ContainsKey(clubID))
		{
			return Game.instance.g_ClubData[clubID].LeagueID;
		}
		return -1;
	}

	public static int GetManagerID(int clubID)
	{
		if (Game.GetCurrentUserClubID() == clubID)
		{
			return 0;
		}
		foreach (int key in Game.instance.g_ManagerData.Keys)
		{
			if (Game.instance.g_ManagerData[key].ClubID == clubID)
			{
				return key;
			}
		}
		return -1;
	}

	public static short GetNumberOfGamesPlayedThisSeason(short clubid)
	{
		Schedule schedule = Game.instance.g_season.Schedule;
		short num = 0;
		foreach (FixtureSet item in schedule.schedule.List)
		{
			if (item.ContainsGameFor(clubid))
			{
				num++;
			}
		}
		return num;
	}

	public static byte GetClubDifficulty(short clubid, int leagueid)
	{
		List<short> clubIDs = Game.instance.g_LeagueData[leagueid].ClubIDs;
		Dictionary<short, float> dictionary = new Dictionary<short, float>();
		float averageRatingPrecise = GetAverageRatingPrecise(clubid);
		foreach (short item in clubIDs)
		{
			dictionary.Add(item, GetAverageRatingPrecise(item));
		}
		dictionary = dictionary.OrderByDescending((KeyValuePair<short, float> kpv) => kpv.Value).ToDictionary((KeyValuePair<short, float> kpv) => kpv.Key, (KeyValuePair<short, float> kpv) => kpv.Value);
		float value = dictionary.Last().Value;
		float num = (dictionary.First().Value - dictionary.Last().Value) / 5f;
		if (averageRatingPrecise >= value && (double)averageRatingPrecise < (double)value + 0.5 * (double)num)
		{
			return 1;
		}
		if (averageRatingPrecise >= value && averageRatingPrecise < value + num)
		{
			return 2;
		}
		if ((double)averageRatingPrecise >= (double)value + 0.5 * (double)num && (double)averageRatingPrecise < (double)value + 1.5 * (double)num)
		{
			return 3;
		}
		if (averageRatingPrecise >= value + num && averageRatingPrecise < value + 2f * num)
		{
			return 4;
		}
		if ((double)averageRatingPrecise >= (double)value + 1.5 * (double)num && (double)averageRatingPrecise < (double)value + 2.5 * (double)num)
		{
			return 5;
		}
		if (averageRatingPrecise >= value + 2f * num && averageRatingPrecise < value + 3f * num)
		{
			return 6;
		}
		if ((double)averageRatingPrecise >= (double)value + 2.5 * (double)num && (double)averageRatingPrecise < (double)value + 3.5 * (double)num)
		{
			return 7;
		}
		if (averageRatingPrecise >= value + 3f * num && averageRatingPrecise < value + 4f * num)
		{
			return 8;
		}
		if ((double)averageRatingPrecise >= (double)value + 3.5 * (double)num && (double)averageRatingPrecise < (double)value + 4.5 * (double)num)
		{
			return 9;
		}
		if (averageRatingPrecise >= value + 4f * num && averageRatingPrecise < value + 5f * num)
		{
			return 10;
		}
		if (clubid == dictionary.First().Key)
		{
			return 10;
		}
		return 1;
	}

	public static short GetExpectedCupPosition(short clubid, Cup cup)
	{
		List<short> inCup = cup.InCup;
		short numRounds = cup.NumRounds;
		Dictionary<int, int> dictionary = new Dictionary<int, int>();
		foreach (short item in inCup)
		{
			dictionary.Add(item, GetAverageRating(item));
		}
		dictionary = dictionary.OrderByDescending((KeyValuePair<int, int> kpv) => kpv.Value).ToDictionary((KeyValuePair<int, int> kpv) => kpv.Key, (KeyValuePair<int, int> kpv) => kpv.Value);
		int num = 0;
		for (int num2 = 0; num2 < inCup.Count; num2++)
		{
			if (dictionary.ElementAt(num2).Key == clubid)
			{
				num = num2;
			}
		}
		if (num < 2)
		{
			Game.instance.CurrentManagedClub.expectedCupFinal = 1;
			return numRounds;
		}
		if (num < 4)
		{
			Game.instance.CurrentManagedClub.expectedCupFinal = 2;
			return (short)(numRounds - 1);
		}
		if (num < 8)
		{
			Game.instance.CurrentManagedClub.expectedCupFinal = 3;
			return (short)(numRounds - 2);
		}
		for (int num3 = 2; num3 < numRounds; num3++)
		{
			if (num < 8 * num3)
			{
				short num4 = (short)(numRounds - (num3 + 1));
				if (num4 == 0)
				{
					return (short)(numRounds - (numRounds - 1));
				}
				return num4;
			}
		}
		return (short)(numRounds - (numRounds - 1));
	}

	public static int GetTakeoverPrice(short clubid)
	{
		return (int)(Math.Pow((float)(int)GetAverageRating(clubid) / 99f, 5.0) * (Math.Pow((float)LeagueData.GetRating(GetLeagueDataId(clubid)) / 99f, 4.0) * 1000.0) * (double)(GetAverageRatingPrecise(clubid) / GetAverageRatingPrecise(Game.instance.CurrentManagedClubId)));
	}

	public static double GetSeasonProfits(short clubid, bool viewIncome = false)
	{
		int playerWages = GetPlayerWages(clubid);
		double num = (double)playerWages * 0.02 * 52.0 + (double)playerWages * 0.01 * 52.0 + (double)playerWages * 0.02 * 52.0 + (double)playerWages * 0.05 * 52.0 + (double)playerWages * 0.03 * 52.0;
		List<ClubScheduleInfo> schedule = GetSchedule(clubid);
		int num2 = 0;
		foreach (ClubScheduleInfo item in schedule)
		{
			if (item.HomeClubID == clubid)
			{
				num2++;
			}
		}
		if (num2 == 0)
		{
			num2 = 20;
		}
		int num3 = Economy.CalcClubTicketPrice(clubid);
		int attendance = GetAttendance(clubid);
		double num4 = Economy.CalcSeasonTicketSales(clubid);
		double num5 = num3 * attendance * num2;
		double num6 = Economy.CalcWeeklyTvMoney(clubid) * 52;
		double num7 = Economy.CalcSponsorshipMoney(clubid) * 52;
		double num8 = (double)(num3 * attendance) * 0.23 * (double)(num2 / 2);
		double num9 = num3 * attendance * (num2 / 2);
		double num10 = num4 + num5 + num6 + num7 + num9;
		double num11 = num8 + num + (double)(playerWages * 52);
		if (viewIncome)
		{
			return num10;
		}
		return num10 - num11;
	}

	public static short GetFinancialDifficulty(short clubid)
	{
		Dictionary<short, double> keyValues = Game.instance.g_LeagueFinances[GetLeagueDataId(clubid)].keyValues;
		GetName(clubid);
		GetLeagueDataId(clubid);
		_ = Game.instance.g_LeagueFinances;
		double num = 0.0;
		foreach (short key in keyValues.Keys)
		{
			if (key == clubid)
			{
				num = keyValues[key];
			}
		}
		double value = keyValues.First().Value;
		double value2 = keyValues.Last().Value;
		short num2 = 0;
		if (keyValues.Count > 2)
		{
			while (value > keyValues.ElementAt(1).Value * 2.0 && keyValues.ElementAt(1).Value > 0.0)
			{
				keyValues.Remove(keyValues.First().Key);
				value = keyValues.First().Value;
				num2++;
			}
		}
		double num3 = (value - value2) / 5.0;
		short num4 = 10;
		if (num >= value2 && num < value2 + 0.5 * num3)
		{
			num4 = (short)(1 + num2);
		}
		else if (num >= value2 + num3 * 0.5 && num < value2 + 1.0 * num3)
		{
			num4 = (short)(2 + num2);
		}
		else if (num >= value2 + num3 && num < value2 + 1.5 * num3)
		{
			num4 = (short)(3 + num2);
		}
		else if (num >= value2 + 1.5 * num3 && num < value2 + 2.0 * num3)
		{
			num4 = (short)(4 + num2);
		}
		else if (num >= value2 + 2.0 * num3 && num < value2 + 2.5 * num3)
		{
			num4 = (short)(5 + num2);
		}
		else if (num >= value2 + 2.5 * num3 && num < value2 + 3.0 * num3)
		{
			num4 = (short)(6 + num2);
		}
		else if (num >= value2 + 3.0 * num3 && num < value2 + 3.5 * num3)
		{
			num4 = (short)(7 + num2);
		}
		else if (num >= value2 + 3.5 * num3 && num < value2 + 4.0 * num3)
		{
			num4 = (short)(8 + num2);
		}
		else if (num >= value2 + 4.0 * num3 && num < value2 + 4.5 * num3)
		{
			num4 = (short)(9 + num2);
		}
		else if (num >= value2 + 4.5 * num3 && num < value2 + 5.0 * num3)
		{
			num4 = (short)(10 + num2);
		}
		if (clubid == keyValues.First().Key)
		{
			num4 = (short)(10 + num2);
		}
		else if (!keyValues.ContainsKey(clubid))
		{
			num4 = 6;
		}
		if (num4 <= 10)
		{
			return num4;
		}
		return 10;
	}

	public static int GetSizeOfClub(short clubid)
	{
		ManagedClub currentManagedClub = Game.instance.CurrentManagedClub;
		if (currentManagedClub.FacilityScore == 0)
		{
			int fanbaseSize = GetFanbaseSize(clubid);
			PlayerSquad players = GetPlayers(clubid);
			int count = players.Count;
			List<byte> list = new List<byte>();
			int num = 0;
			for (int i = 0; i < count; i++)
			{
				list.Add((byte)((Player.GetRating(players[i]) - 50) * 2));
			}
			list.Sort();
			list.Reverse();
			for (int j = 0; j < count; j++)
			{
				if (j < 18)
				{
					num += list[j];
				}
			}
			double num2 = Math.Round((double)fanbaseSize / 20000.0);
			double num3 = Math.Floor((double)num / 18.0 / 10.0);
			int facilityScore = ((num2 + num3 > 10.0) ? 10 : ((int)(num2 + num3)));
			currentManagedClub.FacilityScore = facilityScore;
		}
		return currentManagedClub.FacilityScore;
	}

	public static double GetSquadValue(short clubid)
	{
		double num = 0.0;
		List<int> players = GetPlayers(clubid).players;
		for (int i = 0; i < players.Count; i++)
		{
			num += (double)Player.GetValue(players[i]);
		}
		return num;
	}

	public static float GetAverageSquadValue(short clubID)
	{
		float num = GetNumPlayers(clubID);
		return GetPlayers(clubID).players.Sum((int i) => Player.GetValue(i)) / num;
	}

	public static List<int> GetTop5Earners(short clubid)
	{
		List<int> players = GetPlayers(clubid).players;
		Dictionary<int, int> dictionary = new Dictionary<int, int>();
		for (int i = 0; i < players.Count; i++)
		{
			dictionary.Add(players[i], Player.GetWages(players[i]));
		}
		dictionary = dictionary.OrderByDescending((KeyValuePair<int, int> x) => x.Value).ToDictionary((KeyValuePair<int, int> pair) => pair.Key, (KeyValuePair<int, int> pair) => pair.Value);
		return dictionary.Keys.Slice(0, 5).ToList();
	}

	public static List<ClubScheduleInfo> GetSchedule(short clubid)
	{
		List<ClubScheduleInfo> list = new List<ClubScheduleInfo>();
		short associationID = GetAssociationID(clubid);
		string seasonID = Game.instance.g_season.SeasonID;
		if (Association.GetTopDivision(associationID) == null)
		{
			return list;
		}
		DateTime seasonStartDate = Association.GetSeasonStartDate(associationID, seasonID);
		FixtureSetList schedule = Game.instance.g_season.Schedule.schedule;
		int num = schedule.Count();
		for (int i = 0; i < num; i++)
		{
			List<Fixture> fixtures = schedule[i].Fixtures;
			int j;
			for (j = 0; j < fixtures.Count; j++)
			{
				if ((fixtures[j].homeClubId != clubid && fixtures[j].awayClubId != clubid) || !(schedule[i].Date >= seasonStartDate))
				{
					continue;
				}
				bool flag = false;
				if (fixtures[j].competitiontype == 3)
				{
					Cup cup = Game.instance.g_season.Cups.First((Cup x) => x.CupDataID == fixtures[j].competitionid);
					if (cup != null && cup.HideCurrentRoundUntil > Game.instance.CurrentDate)
					{
						for (int num2 = 0; num2 < cup.Turn[cup.CurrentRound].NumFixtures; num2++)
						{
							if (cup.Turn[cup.CurrentRound].Fixture[num2].homeClubId == fixtures[j].homeClubId && cup.Turn[cup.CurrentRound].Fixture[num2].awayClubId == fixtures[j].awayClubId)
							{
								flag = true;
								break;
							}
						}
					}
				}
				if (!flag)
				{
					ClubScheduleInfo clubScheduleInfo = new ClubScheduleInfo();
					clubScheduleInfo.Date = schedule[i].Date;
					clubScheduleInfo.Fixture = fixtures[j];
					clubScheduleInfo.CompetitionType = fixtures[j].competitiontype;
					clubScheduleInfo.Played = fixtures[j].IsPlayed;
					clubScheduleInfo.CountryID = fixtures[j].countryid;
					list.Add(clubScheduleInfo);
				}
				break;
			}
		}
		string text = Translation.GetText("next-fixture-home");
		string text2 = Translation.GetText("next-fixture-away");
		string text3 = Translation.GetText("final");
		int count = list.Count;
		for (int num3 = 0; num3 < count; num3++)
		{
			Fixture fixture = list[num3].Fixture;
			list[num3].CompetitionName = Helpers.getCompetitionName(list[num3].CompetitionType, fixture.competitionid, associationID);
			list[num3].CompetitionID = fixture.competitionid;
			if (fixture.homeClubId == clubid)
			{
				list[num3].LocSymbol = "[" + text + "]";
				list[num3].OpponentID = fixture.awayClubId;
			}
			else
			{
				list[num3].LocSymbol = "[" + text2 + "]";
				list[num3].OpponentID = fixture.homeClubId;
			}
			if (fixture.finalCompetitionFixture && fixture.competitiontype != 2)
			{
				list[num3].LocSymbol = text3;
			}
			if (fixture.IsPlayed)
			{
				list[num3].HomeScore = fixture.homeScore;
				list[num3].AwayScore = fixture.awayScore;
			}
			else
			{
				list[num3].HomeScore = (list[num3].AwayScore = 0);
			}
			list[num3].HomeClubID = fixture.homeClubId;
			list[num3].AwayClubID = fixture.awayClubId;
			list[num3].OpponentName = GetName(list[num3].OpponentID);
		}
		return list;
	}

	public static List<ClubScheduleInfo> GetLastNumberOfPlayedFixtures(short clubID, int numFixtures)
	{
		List<ClubScheduleInfo> list = (from i in GetSchedule(clubID)
			where i.Fixture.isPlayed && i.Fixture.IsForClub(clubID)
			select i).ToList();
		if (list.Count < numFixtures)
		{
			return list;
		}
		return list.Skip(Math.Max(0, Enumerable.Count(list) - numFixtures)).ToList();
	}

	public static bool DidWinLastMatch(short clubId)
	{
		List<ClubScheduleInfo> lastNumberOfPlayedFixtures = GetLastNumberOfPlayedFixtures(clubId, 1);
		int num = 0;
		if (num < lastNumberOfPlayedFixtures.Count)
		{
			if (lastNumberOfPlayedFixtures[num].Fixture.homeClubId == clubId)
			{
				return lastNumberOfPlayedFixtures[num].Fixture.homeScore > lastNumberOfPlayedFixtures[num].Fixture.awayScore;
			}
			return lastNumberOfPlayedFixtures[num].Fixture.awayScore > lastNumberOfPlayedFixtures[num].Fixture.homeScore;
		}
		return false;
	}

	public static int GetTotalPointsFromLastGames(List<ClubScheduleInfo> clubsSchedule, short clubID)
	{
		int num = clubsSchedule.Where((ClubScheduleInfo i) => i.HomeClubID == clubID && i.HomeScore > i.AwayScore).Count() + clubsSchedule.Where((ClubScheduleInfo i) => i.AwayClubID == clubID && i.AwayScore > i.HomeScore).Count();
		int num2 = clubsSchedule.Where((ClubScheduleInfo i) => i.HomeScore == i.AwayScore && i.Fixture.homePenalty == -1 && i.Fixture.awayPenalties == -1).Count();
		int num3 = clubsSchedule.Where((ClubScheduleInfo i) => i.HomeClubID == clubID && i.Fixture.homePenalty > i.Fixture.awayPenalties).Count() + clubsSchedule.Where((ClubScheduleInfo i) => i.AwayClubID == clubID && i.Fixture.awayPenalties > i.Fixture.homePenalties).Count();
		return (num + num3) * 3 + num2;
	}

	public static List<ClubScheduleInfo> GetFixturesInRange(short clubID, DateTime dateFrom, DateTime dateTo)
	{
		return (from i in GetSchedule(clubID)
			where i.Fixture.isPlayed && i.Date >= dateFrom && i.Date <= dateTo
			select i).ToList();
	}

	public static List<Fixture> GetFullFixturesInRange(short clubID, DateTime dateFrom, DateTime dateTo)
	{
		List<ClubScheduleInfo> fixturesInRange = GetFixturesInRange(clubID, dateFrom, dateTo);
		List<Fixture> list = new List<Fixture>();
		foreach (ClubScheduleInfo item in fixturesInRange)
		{
			Fixture fixture = item.Fixture;
			fixture.RestoreFromSQL();
			list.Add(fixture);
		}
		return list;
	}

	public static void populateTransferList(short clubid)
	{
		if (clubid <= 0)
		{
			return;
		}
		Club club = Game.instance.g_ClubData[clubid];
		club.transferlist.Clear();
		SortableClubPlayerList playersAsSortableListStatic = Game.instance.g_ClubPlayer[clubid].GetPlayersAsSortableListStatic();
		for (int i = 0; i < playersAsSortableListStatic.Count; i++)
		{
			PlayerData playerData = Game.instance.g_PlayerData[playersAsSortableListStatic[i]];
			if (playerData.SquadStatus == ePlayerSquadStatus.NotNeeded && !PlayerData.IsLoanedToClub(playerData.PlayerId) && !Player.IsTransferBanned(playerData.PlayerId))
			{
				club.transferlist.Add(playerData.PlayerId);
			}
		}
	}

	public static int numberOfPlayersrequiredForPosition(short clubid, int position)
	{
		int reputation = GetReputation(clubid);
		if (reputation == 20)
		{
			if ((position & 1) != 0)
			{
				return 3;
			}
			if ((position & 0x7E) != 0)
			{
				return 8;
			}
			if ((position & 0x1FF0) != 0)
			{
				return 9;
			}
			if ((position & 0xE000) != 0)
			{
				return 6;
			}
			return 999999;
		}
		if (reputation >= 13)
		{
			if ((position & 1) != 0)
			{
				return 2;
			}
			if ((position & 0x7E) != 0)
			{
				return 7;
			}
			if ((position & 0x1FF0) != 0)
			{
				return 7;
			}
			if ((position & 0xE000) != 0)
			{
				return 4;
			}
			return 999999;
		}
		if ((position & 1) != 0)
		{
			return 1;
		}
		if ((position & 0x7E) != 0)
		{
			return 4;
		}
		if ((position & 0x1FF0) != 0)
		{
			return 4;
		}
		if ((position & 0xE000) != 0)
		{
			return 2;
		}
		return 999999;
	}

	public static void EnsureSquadAdequatePosition(short clubId, int playerPos)
	{
		PlayerSquad players = GetPlayers(clubId);
		if (players.Count < 18)
		{
			PlayerRespawn.RespawnPlayerForClub(clubId, (Constants.PlayerPosition)playerPos);
		}
		int genericPositionMask = Player.GetGenericPositionMask(playerPos);
		List<int> playersByPos = Helpers.GetPlayersByPos(players, genericPositionMask, doSort: false);
		int num = 0;
		if ((genericPositionMask & 1) != 0)
		{
			num = 2;
		}
		else if ((genericPositionMask & 0xE) != 0)
		{
			num = 5;
		}
		else if ((genericPositionMask & 0x1FF0) != 0)
		{
			num = 5;
		}
		else if ((genericPositionMask & 0xE000) != 0)
		{
			num = 4;
		}
		if (playersByPos.Count < num)
		{
			PlayerRespawn.RespawnPlayerForClub(clubId, (Constants.PlayerPosition)genericPositionMask);
		}
	}

	internal static void PostMatch(short clubID, Matchday.MatchResult result)
	{
		Club club = Game.instance.g_ClubData[clubID];
		switch (result)
		{
		case Matchday.MatchResult.Win:
			club.winTally++;
			club.unbeatenTally++;
			break;
		case Matchday.MatchResult.Draw:
			club.winTally = 0;
			club.unbeatenTally++;
			break;
		case Matchday.MatchResult.Lose:
			club.winTally = 0;
			club.unbeatenTally = 0;
			break;
		}
	}

	public static void SquadHealthCheck(short clubId, Fixture fixture = null)
	{
		int num = 0;
		bool flag = false;
		Dictionary<short, PlayerSquad> g_ClubPlayer = Game.instance.g_ClubPlayer;
		if (!g_ClubPlayer.ContainsKey(clubId))
		{
			return;
		}
		int num2 = 0;
		int num3 = 0;
		int i = 0;
		int assocID = -1;
		int competitionType = -1;
		int competitionID = -1;
		if (fixture != null)
		{
			assocID = GetAssociationID(fixture.homeClubId);
			competitionType = fixture.competitiontype;
			competitionID = fixture.competitionid;
		}
		int num4 = 0;
		foreach (int player in g_ClubPlayer[clubId].players)
		{
			PlayerData playerData = Game.instance.g_PlayerData[player];
			if (!playerData.IsInjured() && !playerData.IsSuspended(assocID, competitionType, competitionID))
			{
				num3++;
				if (playerData.GetPosition() == 1)
				{
					num2++;
					continue;
				}
				if (flag && PlayerData.GetCountryDataID(player) == 42)
				{
					num++;
				}
				i++;
			}
			else
			{
				playerData.NotAvailableForMatch(assocID, competitionType, competitionID);
				num4++;
			}
		}
		if (num2 == 0)
		{
			PlayerRespawn.giveRespawnToClub(clubId, 1, 75);
			num3++;
		}
		if (num3 < 20)
		{
			while (num3 < 20)
			{
				PlayerRespawn.giveRespawnToClub(clubId, Constants.outfieldPos[Game.instance.Random.GetRand() % 3], 75);
				num3++;
				num++;
				i++;
			}
		}
		if (flag)
		{
			int num5 = 15 - num;
			for (int j = 0; j < num5; j++)
			{
				PlayerRespawn.giveRespawnToClub(clubId, Constants.outfieldPos[Game.instance.Random.GetRand() % 3], 75);
			}
		}
		for (; i < 18; i++)
		{
			PlayerRespawn.giveRespawnToClub(clubId, Constants.outfieldPos[Game.instance.Random.GetRand() % 3], 75);
			num3++;
		}
	}

	public static ClubKitStyle GetKitStyle(short kitID, short clubID)
	{
		short[] source = new short[43]
		{
			235, 171, 299, 268, 109, 111, 303, 48, 304, 241,
			49, 146, 242, 50, 51, 52, 85, 277, 53, 86,
			278, 54, 55, 57, 58, 59, 91, 60, 252, 61,
			253, 62, 63, 255, 64, 320, 259, 357, 358, 72,
			168, 296, 330
		};
		short[] source2 = new short[25]
		{
			172, 108, 204, 44, 45, 46, 79, 47, 80, 145,
			82, 149, 150, 182, 183, 92, 220, 93, 318, 225,
			97, 291, 261, 326, 232
		};
		short[] source3 = new short[18]
		{
			238, 14, 16, 177, 17, 273, 305, 18, 19, 308,
			20, 276, 247, 87, 317, 356, 69, 70
		};
		short[] source4 = new short[18]
		{
			199, 71, 363, 207, 81, 307, 147, 243, 181, 21,
			22, 23, 184, 24, 25, 26, 314, 187
		};
		ClubKitStyle originalStyle = ClubKitStyle.Block;
		if (source.Contains(kitID))
		{
			originalStyle = ClubKitStyle.Stripes;
		}
		else if (source2.Contains(kitID))
		{
			originalStyle = ClubKitStyle.Sleeves;
		}
		else if (source3.Contains(kitID))
		{
			originalStyle = ClubKitStyle.HalfHalf;
		}
		else if (source4.Contains(kitID))
		{
			originalStyle = ClubKitStyle.Hoops;
		}
		return FixtureKitHelper.GetHomeKitPattern(originalStyle, clubID);
	}

	public static Color GetHomePrimaryColour(short clubid)
	{
		if (!Game.instance.g_ClubData.ContainsKey(clubid))
		{
			return new Color(102f, 102f, 102f);
		}
		if (clubid != 0)
		{
			return Helpers.HexToColour(Game.instance.g_ClubData[clubid].HomeColour1);
		}
		return new Color(102f, 102f, 102f);
	}

	internal void InitLeagueData(LeagueData leagueData)
	{
		AssociationID = leagueData.AssociationID;
		LeagueID = leagueData.LeagueID;
	}
}
