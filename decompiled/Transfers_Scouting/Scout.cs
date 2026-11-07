using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Scout
{
	public int id;

	protected int ratingForesightMonths;

	protected int ratingMarginOfError;

	protected int potentialMarginOfError;

	protected int startingMaxRating;

	protected int startingMinRating;

	protected int squadMaxRating;

	protected int squadMinRating;

	protected int youthMaxRating;

	protected int maxContractLength;

	public static readonly Dictionary<ScoutAssignmentTypes, ScoutAssignmentTypeDefaults> scoutAssignmentTypeDefaults = new Dictionary<ScoutAssignmentTypes, ScoutAssignmentTypeDefaults>
	{
		{
			ScoutAssignmentTypes.Player,
			new ScoutAssignmentTypeDefaults(15, 1, 1)
		},
		{
			ScoutAssignmentTypes.Club,
			new ScoutAssignmentTypeDefaults(30, 2, 30)
		},
		{
			ScoutAssignmentTypes.Country,
			new ScoutAssignmentTypeDefaults(30, 4, 30)
		}
	};

	public Scout(int id = -1)
	{
		if (id != -1)
		{
			this.id = id;
		}
	}

	public string GetName()
	{
		int countryDataID = Club.GetCountryDataID(Game.instance.CurrentManagedClubId);
		string[] array = Generator.GetPlayerName(countryDataID).Split(',');
		return array[0][0] + ". " + array[1];
	}

	public int GetCurrentAssignmentIndex()
	{
		List<ScoutSearch> g_ScoutingSearch = Game.instance.g_ScoutingSearch;
		int result = -1;
		for (int i = 0; i < g_ScoutingSearch.Count; i++)
		{
			ScoutSearch scoutSearch = g_ScoutingSearch[i];
			if (scoutSearch.staffid == id && scoutSearch.dateend > Game.instance.CurrentDate)
			{
				result = i;
				break;
			}
		}
		return result;
	}

	public ScoutSearch GetCurrentSearch()
	{
		List<ScoutSearch> g_ScoutingSearch = Game.instance.g_ScoutingSearch;
		for (int i = 0; i < g_ScoutingSearch.Count; i++)
		{
			ScoutSearch scoutSearch = g_ScoutingSearch[i];
			if (scoutSearch.staffid == id && scoutSearch.dateend > Game.instance.CurrentDate)
			{
				return scoutSearch;
			}
		}
		return null;
	}

	public int GetNumberOfActiveCoachAssignments()
	{
		int num = -1;
		foreach (int key in Game.instance.g_coachReports.Keys)
		{
			if (Game.instance.g_coachReports[key].IsReportInProgress(key))
			{
				num++;
			}
		}
		return num;
	}

	public int CreateCountryScoutReport(ScoutAssignmentTypes type, ScoutingOptions options)
	{
		if (GetCurrentAssignmentIndex() == -1)
		{
			List<ScoutSearch> g_ScoutingSearch = Game.instance.g_ScoutingSearch;
			string searchid = ((double)(g_ScoutingSearch.Count + id) + Game.instance.CurrentDate.Subtract(new DateTime(1970, 1, 1)).TotalSeconds).ToString();
			ScoutSearch scoutSearch = new ScoutSearch();
			scoutSearch.FillNewCountryScoutSearch(searchid, id, (int)type, options);
			g_ScoutingSearch.Add(scoutSearch);
			Game.instance.g_lastScoutSearch = scoutSearch;
			return g_ScoutingSearch.Count - 1;
		}
		return -1;
	}

	public int CreatePlayerScoutReport(int playerId)
	{
		if (GetCurrentAssignmentIndex() == -1)
		{
			List<int> availableSearchNations = Scouting.GetAvailableSearchNations();
			short clubID = Player.GetClubID(playerId);
			if ((clubID > 0 && availableSearchNations.Contains(CountryData.GetContinent(Club.GetCountryDataID(clubID)))) || clubID == 0)
			{
				List<ScoutSearch> g_ScoutingSearch = Game.instance.g_ScoutingSearch;
				string searchid = ((double)(g_ScoutingSearch.Count + id) + Game.instance.CurrentDate.Subtract(new DateTime(1970, 1, 1)).TotalSeconds).ToString();
				ScoutSearch scoutSearch = new ScoutSearch();
				scoutSearch.FillNewScoutSearch(searchid, id, 0, new int[1] { playerId }, 0, null);
				g_ScoutingSearch.Add(scoutSearch);
				return g_ScoutingSearch.Count - 1;
			}
		}
		return -1;
	}

	public bool RecallScout(bool addClubMessage = true)
	{
		int currentAssignmentIndex = GetCurrentAssignmentIndex();
		if (currentAssignmentIndex > -1)
		{
			DeleteScoutReport(Game.instance.g_ScoutingSearch[currentAssignmentIndex].searchid);
			return true;
		}
		return false;
	}

	public void ProcessAssignment(bool paid = false)
	{
		int currentAssignmentIndex = GetCurrentAssignmentIndex();
		if (currentAssignmentIndex != -1)
		{
			CalculateScoutSkills(paid);
			CalculateClubRatingBoundaries();
			switch ((ScoutAssignmentTypes)Game.instance.g_ScoutingSearch[currentAssignmentIndex].type)
			{
			case ScoutAssignmentTypes.Country:
				ProcessCountryAssignment(currentAssignmentIndex);
				break;
			case ScoutAssignmentTypes.Player:
				ProcessPlayerAssignment(currentAssignmentIndex, paid);
				break;
			}
			TasklistManager.CreateTask(TaskType.ScoutReturned, removeOnClick: true, 66);
		}
	}

	public ScoutSearchPlayer ProcessCoachAssignment(int playerID)
	{
		CalculateCoachSkills();
		CalculateClubRatingBoundaries();
		return ProcessCoachReportAssignment(playerID);
	}

	public void ProcessPlayerAssignment(int idx, bool paid = false)
	{
		ScoutSearch scoutSearch = Game.instance.g_ScoutingSearch[idx];
		if (scoutSearch.id.Length >= 1)
		{
			int playerId = scoutSearch.id[0];
			ScoutSearchPlayer item = AnalysePlayer(playerId);
			scoutSearch.dateend = Game.instance.CurrentDate;
			scoutSearch.results = new List<ScoutSearchPlayer> { item };
			RevealScoutReportPlayerAttributes(idx, paid);
		}
	}

	public ScoutSearchPlayer ProcessCoachReportAssignment(int playerID)
	{
		return AnalysePlayer(playerID);
	}

	public static void DeleteCoachReports()
	{
		int num = 6;
		DateTime currentDate = Game.instance.CurrentDate;
		List<int> list = new List<int>();
		foreach (int key in Game.instance.g_coachReports.Keys)
		{
			CoachReport coachReport = Game.instance.g_coachReports[key];
			if ((double)currentDate.Subtract(coachReport.dateend).Days / 30.436875 > (double)num)
			{
				list.Add(key);
			}
		}
		foreach (int item in list)
		{
			Game.instance.g_coachReports.Remove(item);
		}
	}

	private bool PlayerIsMatchForReport(int playerId, ScoutSearch scoutReport)
	{
		if (Game.instance.g_PlayersScouted.ContainsKey(playerId))
		{
			return false;
		}
		if (!Player.canGoToClub(playerId, Game.GetCurrentUserClubID()))
		{
			return false;
		}
		int position = Player.GetPosition(playerId);
		int num = (PlayerData.IsLoanedToClub(playerId) ? Game.instance.g_PlayerData[playerId].LoanParentClubId : Player.GetClubID(playerId));
		if (num == Game.instance.CurrentManagedClubId)
		{
			return false;
		}
		if (scoutReport.positions != 0 && (position & scoutReport.positions) <= 0)
		{
			return false;
		}
		if (scoutReport.age[0] != 0)
		{
			int age = Player.GetAge(playerId);
			if (age < scoutReport.age[0] || age > scoutReport.age[1])
			{
				return false;
			}
		}
		if (scoutReport.value[0] != 0.0)
		{
			double num2 = Player.GetValue(playerId);
			if (num2 < scoutReport.value[0] || num2 > scoutReport.value[1])
			{
				return false;
			}
		}
		if (maxContractLength != 0 && (int)Player.GetDaysRemainingOnContract(playerId) / 30 > maxContractLength)
		{
			return false;
		}
		if (scoutReport.country != 0)
		{
			if (scoutReport.league != 0)
			{
				if (Club.GetLeagueDataId((short)num) != scoutReport.league)
				{
					return false;
				}
			}
			else if (LeagueData.GetAssocID(Club.GetLeagueDataId((short)num)) != scoutReport.country)
			{
				return false;
			}
		}
		if (scoutReport.nationality != 0 && PlayerData.GetCountryDataID(playerId) != scoutReport.nationality)
		{
			return false;
		}
		if (scoutReport.transferStatus != 0)
		{
			if (scoutReport.transferStatus == 3 && (num == 0 || !Transfers.instance.IsPlayerOnTransferList(playerId)))
			{
				return false;
			}
			if (scoutReport.transferStatus == 2 && (num == 0 || !Transfers.instance.transferListManager.isPlayerOnLoanList(playerId)))
			{
				return false;
			}
			if (scoutReport.transferStatus == 1 && (num == 0 || Transfers.instance.IsPlayerOnTransferList(playerId) || Transfers.instance.transferListManager.isPlayerOnLoanList(playerId)))
			{
				return false;
			}
			if (scoutReport.transferStatus == 4 && Player.GetClubID(playerId) != 0)
			{
				return false;
			}
		}
		return true;
	}

	private int GetMaxContractLength(Constants.SearchEnquiry searchReq)
	{
		return searchReq switch
		{
			Constants.SearchEnquiry.FreeAgent => 0, 
			Constants.SearchEnquiry.ThreeMonths => 3, 
			Constants.SearchEnquiry.SixMonths => 6, 
			Constants.SearchEnquiry.TwelveMonths => 12, 
			Constants.SearchEnquiry.EighteenMonths => 18, 
			Constants.SearchEnquiry.TwentyFourMonths => 24, 
			_ => int.MaxValue, 
		};
	}

	public bool IsPlayerInRatingRange(int clubAverage, int playerRating)
	{
		if (playerRating > clubAverage - 5)
		{
			return playerRating < clubAverage + 10;
		}
		return false;
	}

	public void ProcessCountryAssignment(int idx)
	{
		int averageRating = Club.GetAverageRating(Game.GetCurrentUserClubID());
		MonoBehaviour.print("Club rating: " + averageRating);
		ScoutSearch scoutSearch = Game.instance.g_ScoutingSearch[idx];
		if (scoutSearch.contractLength[0] != 0f)
		{
			maxContractLength = GetMaxContractLength((Constants.SearchEnquiry)scoutSearch.contractLength[0]);
		}
		else
		{
			maxContractLength = 0;
		}
		List<ScoutSearchPlayer> list = new List<ScoutSearchPlayer>();
		List<ScoutSearchPlayer> list2 = new List<ScoutSearchPlayer>();
		List<int> list3 = new List<int>();
		if (scoutSearch.country != 0)
		{
			list3.AddRange(Association.GetPlayerList(scoutSearch.country));
		}
		else if (Save.saveVersion >= 14 && scoutSearch.countries.Count > 0)
		{
			for (int i = 0; i < scoutSearch.countries.Count; i++)
			{
				list3.AddRange(Association.GetPlayerList(scoutSearch.countries[i]));
			}
		}
		else
		{
			int continent = CountryData.GetContinent(Club.GetAssociationID(Game.GetCurrentUserClubID()));
			foreach (KeyValuePair<int, CountryData> g_CountryDatum in Game.instance.g_CountryData)
			{
				if (g_CountryDatum.Value.Continent == continent && g_CountryDatum.Value.Popular)
				{
					list3.AddRange(Association.GetPlayerList(g_CountryDatum.Key));
				}
			}
			list3.AddRange(Association.GetPlayerList(2));
		}
		list3.Shuffle();
		int resultsAmount = Scouting.GetResultsAmount(Facilities.GetFacilityLevel(4));
		for (int j = 0; j < list3.Count; j++)
		{
			int num = list3[j];
			Player.GetPosition(num);
			if (!PlayerData.IsLoanedToClub(num))
			{
				Player.GetClubID(num);
			}
			else
			{
				_ = Game.instance.g_PlayerData[num].LoanParentClubId;
			}
			if (!PlayerIsMatchForReport(num, scoutSearch))
			{
				continue;
			}
			ScoutSearchPlayer scoutSearchPlayer = AnalysePlayer(num);
			_ = scoutSearchPlayer.recommendation;
			if (!IsPlayerInRatingRange(averageRating, Player.GetRating(scoutSearchPlayer.playerid)))
			{
				list.Add(scoutSearchPlayer);
				continue;
			}
			MonoBehaviour.print("Adding player + " + scoutSearchPlayer.playerid + " rating: " + Player.GetRating(scoutSearchPlayer.playerid));
			list2.Add(scoutSearchPlayer);
			if (list2.Count == resultsAmount)
			{
				break;
			}
		}
		List<ScoutSearchPlayer> list4 = new List<ScoutSearchPlayer>();
		list2 = list2.OrderByDescending((ScoutSearchPlayer player) => player.rating).ToList();
		for (int num2 = 0; num2 < resultsAmount; num2++)
		{
			if (list2.Count < 1)
			{
				break;
			}
			if (num2 == 0 || num2 == 1)
			{
				list4.Add(list2.ElementAt(0));
				list2.RemoveAt(0);
			}
			else
			{
				byte index = (byte)Game.instance.Random.GetRand(list2.Count);
				list4.Add(list2.ElementAt(index));
				list2.RemoveAt(index);
			}
		}
		if (list4.Count < resultsAmount)
		{
			list = list.OrderByDescending((ScoutSearchPlayer player) => player.rating).ToList();
			byte b = (byte)(resultsAmount - list4.Count);
			for (int num3 = 0; num3 < b; num3++)
			{
				if (list.Count < 1)
				{
					break;
				}
				if (num3 == 0 || num3 == 1)
				{
					list4.Add(list.ElementAt(0));
					list.RemoveAt(0);
				}
				else
				{
					byte index2 = (byte)Game.instance.Random.GetRand(list.Count);
					list4.Add(list.ElementAt(index2));
					list.RemoveAt(index2);
				}
			}
		}
		scoutSearch.dateend = Game.instance.CurrentDate;
		scoutSearch.results = list4;
		RevealScoutReportPlayerAttributes(idx);
	}

	public void RevealScoutReportPlayerAttributes(int idx, bool paid = false)
	{
		if (Game.instance.g_ScoutingSearch.Count <= idx)
		{
			return;
		}
		foreach (ScoutSearchPlayer result in Game.instance.g_ScoutingSearch[idx].results)
		{
			ScoutedAttributesPlayer scoutedAttributesPlayer = new ScoutedAttributesPlayer();
			scoutedAttributesPlayer.date = Game.instance.CurrentDate;
			scoutedAttributesPlayer.attributeEffect = 1f;
			Game.instance.g_PlayersScouted[result.playerid] = scoutedAttributesPlayer;
		}
	}

	public void CalculateScoutSkills(bool paid = false)
	{
		ratingForesightMonths = (int)(GetJudgingPlayerAbilityScore(0) * 12f);
		ratingMarginOfError = 5 - (int)(GetJudgingPlayerAbilityScore(0) * 5f);
		potentialMarginOfError = 6 - (int)(GetJudgingPlayerPotentialScore(0) * 5f);
		if (paid)
		{
			ratingForesightMonths = 12;
			ratingMarginOfError = 0;
			potentialMarginOfError = 1;
		}
	}

	public void CalculateCoachSkills(bool paid = false)
	{
		ratingForesightMonths = (int)(GetJudgingPlayerAbilityScore(1) * 12f);
		ratingMarginOfError = 5 - (int)(GetJudgingPlayerAbilityScore(1) * 5f);
		potentialMarginOfError = 6 - (int)(GetJudgingPlayerPotentialScore(1) * 5f);
		if (paid)
		{
			ratingForesightMonths = 12;
			ratingMarginOfError = 0;
			potentialMarginOfError = 1;
		}
	}

	public void CalculateClubRatingBoundaries()
	{
		short currentManagedClubId = Game.instance.CurrentManagedClubId;
		Club.ClubMinMaxRating playersMaxMinRating = Club.GetPlayersMaxMinRating(currentManagedClubId, 0, 11);
		startingMinRating = playersMaxMinRating.Min;
		startingMaxRating = playersMaxMinRating.Max + 3;
		int num = startingMinRating + 5;
		startingMaxRating = ((num > startingMaxRating) ? num : startingMaxRating);
		int idxto = Math.Min(24, Club.GetNumPlayers(currentManagedClubId));
		playersMaxMinRating = Club.GetPlayersMaxMinRating(currentManagedClubId, 11, idxto);
		squadMinRating = playersMaxMinRating.Min;
		squadMaxRating = playersMaxMinRating.Max;
		playersMaxMinRating = Club.GetPlayersMaxMinRating(currentManagedClubId, 0, 1);
		youthMaxRating = playersMaxMinRating.Max + 4;
	}

	public ScoutSearchPlayer AnalysePlayer(int playerId, int[] preferredAttributes = null)
	{
		int seed = Game.instance.Random.GetSeed();
		Game.instance.Random.SetSeed((playerId + id) * 1234 % 32768);
		ScoutSearchPlayer scoutSearchPlayer = new ScoutSearchPlayer();
		PlayerData playerData = Game.instance.g_PlayerData[playerId];
		int rating = Player.GetRating(playerId, Game.instance.CurrentDate.AddMonths(ratingForesightMonths));
		int potential = playerData.Potential;
		int potential2 = playerData.Potential;
		int position = Player.GetPosition(playerId);
		int playerAge = playerData.Age();
		int ratingEstimate = GetRatingEstimate(rating);
		int potentialEstimate = GetPotentialEstimate(rating, potential, position, playerAge);
		int potentialEstimate2 = GetPotentialEstimate(rating, potential2, position, playerAge);
		SetPotentialRange(scoutSearchPlayer, potential2);
		SetRatingRange(scoutSearchPlayer, rating);
		scoutSearchPlayer.playerid = playerId;
		scoutSearchPlayer.rating = rating;
		int[] detailedPlayerRecommendation = GetDetailedPlayerRecommendation(ratingEstimate, potentialEstimate, playerAge);
		int[] detailedPlayerRecommendation2 = GetDetailedPlayerRecommendation(ratingEstimate, potentialEstimate2, playerAge);
		scoutSearchPlayer.recommendation = ((detailedPlayerRecommendation[0] >= detailedPlayerRecommendation2[0]) ? detailedPlayerRecommendation[0] : detailedPlayerRecommendation2[0]);
		scoutSearchPlayer.prediction = ((detailedPlayerRecommendation[1] >= detailedPlayerRecommendation2[1]) ? detailedPlayerRecommendation[1] : detailedPlayerRecommendation2[1]);
		if (scoutSearchPlayer.recommendation > -1)
		{
			scoutSearchPlayer.recommendationAttributes = preferredAttributes == null;
			if (preferredAttributes != null && PlayerHasRequiredAttributes(playerId, preferredAttributes))
			{
				scoutSearchPlayer.recommendationAttributes = true;
			}
		}
		Game.instance.Random.SetSeed(seed);
		return scoutSearchPlayer;
	}

	public void SetPotentialRange(ScoutSearchPlayer player, int potential)
	{
		int scoutPotentialRange = Scouting.GetScoutPotentialRange();
		int num = new System.Random().Next(-3, 3);
		player.lowerPotential = (int)Attributes.MapRating(potential - (scoutPotentialRange / 2 + num));
		player.higherPotential = (int)Attributes.MapRating(potential + (scoutPotentialRange / 2 - num));
	}

	private void SetRatingRange(ScoutSearchPlayer player, int rating)
	{
		int scoutRatingRange = Scouting.GetScoutRatingRange();
		int num = new System.Random().Next(-3, 3);
		player.lowerRating = (int)Attributes.MapRating(rating - (scoutRatingRange / 2 + num));
		player.higherRating = (int)Attributes.MapRating(rating + (scoutRatingRange / 2 - num));
	}

	public int GetRatingEstimate(int playerRating)
	{
		return playerRating - ratingMarginOfError + Game.instance.Random.GetRand() % (ratingMarginOfError * 2 + 1);
	}

	public int GetPotentialEstimate(int playerRating, int playerPotential, int playerPosition, int playerAge)
	{
		int maxPotentialEstimate = GetMaxPotentialEstimate(playerRating, playerPosition, playerAge);
		int num = playerPotential - potentialMarginOfError + Game.instance.Random.GetRand() % (potentialMarginOfError * 2 + 1);
		if (num < playerRating)
		{
			num = playerRating;
		}
		else if (num > maxPotentialEstimate)
		{
			num = maxPotentialEstimate;
		}
		return num;
	}

	public int GetPlayerRecommendation(int ratingEstimate, int potentialEstimate, int playerAge)
	{
		if (ratingEstimate >= startingMinRating && ratingEstimate <= startingMaxRating)
		{
			return 0;
		}
		if (ratingEstimate >= squadMinRating && ratingEstimate <= squadMaxRating)
		{
			return 1;
		}
		if (potentialEstimate >= squadMinRating && potentialEstimate <= youthMaxRating && playerAge <= 23)
		{
			return 2;
		}
		if (ratingEstimate > startingMaxRating || potentialEstimate > youthMaxRating)
		{
			return -2;
		}
		return -1;
	}

	public int[] GetDetailedPlayerRecommendation(int ratingEstimate, int potentialEstimate, int playerAge)
	{
		int playerRecommendation = GetPlayerRecommendation(ratingEstimate, potentialEstimate, playerAge);
		int num = 0;
		int num2 = 0;
		int num3 = 0;
		if (playerRecommendation != -1)
		{
			if (playerRecommendation == 2 || playerAge <= 26)
			{
				num2 = ((potentialEstimate < startingMinRating) ? 5 : GetPlayerRatingRelativeStrength(potentialEstimate, startingMinRating, startingMaxRating));
			}
			switch (playerRecommendation)
			{
			case 0:
				num = GetPlayerRatingRelativeStrength(ratingEstimate, startingMinRating, startingMaxRating);
				break;
			case 1:
				num = GetPlayerRatingRelativeStrength(ratingEstimate, squadMinRating, squadMaxRating);
				break;
			}
			if (playerRecommendation == 0 && num <= num2)
			{
				num2 = 0;
			}
			num3 = Scouting.EncodeRatingAndPotentialDescriptions(num, num2);
		}
		return new int[2] { playerRecommendation, num3 };
	}

	public static int GetPlayerRatingRelativeStrength(int ratingEstimate, int catMinRating, int catMaxRating)
	{
		int num = -1;
		if (ratingEstimate >= catMinRating)
		{
			if (ratingEstimate >= 94)
			{
				return 1;
			}
			float num2 = (float)(catMaxRating - catMinRating) / 4f;
			float num3 = (float)catMaxRating - num2;
			num = 2;
			while ((float)ratingEstimate < Mathf.Round(num3))
			{
				num++;
				num3 -= num2;
			}
		}
		return num;
	}

	public bool PlayerHasRequiredAttributes(int playerId, int[] preferredAttributes, PlayerAttributes attributes = null)
	{
		if (attributes == null)
		{
			attributes = Attributes.SetAttributesForPlayer(playerId);
			foreach (int item in Attributes.GetAttributesAffectedByFogOfWar(playerId, (int)Player.GetRating(playerId), attributes.isKeeper, GetJudgingPlayerAttributesScore(), attributes.attrsDisplay))
			{
				attributes.attrsDisplay[item] = -1f;
			}
		}
		bool result = true;
		float num = (float)attributes.attrs.Sum();
		attributes.attrs.Max();
		float num2 = (attributes.isKeeper ? Attributes.allAttributesKeeper.Length : Attributes.allAttributesOutfield.Length);
		float num3 = Mathf.Ceil(num / num2);
		foreach (int num4 in preferredAttributes)
		{
			if (attributes.attrs[num4] < (double)num3 || attributes.attrsDisplay[num4] < 0f)
			{
				result = false;
				break;
			}
		}
		return result;
	}

	public int GetMaxPotentialEstimate(int rating, int position, int age)
	{
		int num = 99;
		if (age >= 30)
		{
			num = (((position & 1) > 0) ? 5 : 4);
		}
		else if (age >= 28)
		{
			num = (((position & 1) > 0) ? 7 : 5);
		}
		else if (age >= 27)
		{
			num = (((position & 1) > 0) ? 8 : 6);
		}
		else if (age >= 26)
		{
			num = (((position & 1) > 0) ? 9 : 7);
		}
		if (rating >= 90)
		{
			num -= 3;
		}
		else if (rating >= 85)
		{
			num -= 2;
		}
		else if (rating >= 80)
		{
			num--;
		}
		int num2 = rating + num;
		if (num2 <= 99)
		{
			return num2;
		}
		return 99;
	}

	public static void Init()
	{
		CreateAnyMissingScouts();
	}

	public static void CreateAnyMissingScouts()
	{
		int noOfScouts = Scouting.GetNoOfScouts(Facilities.GetFacilityLevel(4));
		for (int i = Game.instance.g_Scouts.Count; i < noOfScouts; i++)
		{
			CreateScout();
		}
	}

	public static int CreateScout()
	{
		int seed = Game.instance.Random.GetSeed();
		Game.instance.Random.SetSeed(Game.instance.Seed + ((Game.instance.managedClub != null) ? Game.instance.CurrentManagedClubId : Game.instance.g_startClubID));
		List<int> list = new List<int>(Game.instance.g_Scouts.Keys);
		bool flag;
		int rand;
		do
		{
			flag = true;
			rand = Game.instance.Random.GetRand(0, 2147483646);
			if (list.Contains(rand))
			{
				flag = false;
			}
		}
		while (!flag);
		Game.instance.g_Scouts[rand] = new Scout(rand);
		Game.instance.Random.SetSeed(seed);
		return rand;
	}

	public static int FindScoutReportById(string reportId)
	{
		List<ScoutSearch> g_ScoutingSearch = Game.instance.g_ScoutingSearch;
		int result = -1;
		for (int i = 0; i < g_ScoutingSearch.Count; i++)
		{
			if (g_ScoutingSearch[i].searchid == reportId)
			{
				result = i;
				break;
			}
		}
		return result;
	}

	public static void DeleteScoutReport(string reportId)
	{
		List<ScoutSearch> g_ScoutingSearch = Game.instance.g_ScoutingSearch;
		int num = FindScoutReportById(reportId);
		if (num > -1)
		{
			g_ScoutingSearch.RemoveAt(num);
		}
	}

	public static float GetTotalAssignmentDuration(int scoutId, int[] items, ScoutAssignmentTypes type)
	{
		int assignmentDuration = Game.instance.g_Scouts[scoutId].GetAssignmentDuration(type);
		int num = items.Length;
		return (float)assignmentDuration * (float)num;
	}

	public static int GetNumberOfScouts()
	{
		int num = 0;
		return 2 + num;
	}

	public int[] GetAvailableSearchContinents()
	{
		if (0 == 0)
		{
			int continent = CountryData.GetContinent(Club.GetCountryDataID(Game.instance.CurrentManagedClubId));
			return new int[1] { continent };
		}
		return new int[6] { 1, 2, 3, 4, 5, 6 };
	}

	public int GetAvailableNumSearchableRoles()
	{
		int result = 1;
		int result2 = 3;
		if (0 != 0)
		{
			return result2;
		}
		return result;
	}

	public int GetAvailableNumSearchablePositions()
	{
		return 1 switch
		{
			2 => 15, 
			1 => 3, 
			_ => 1, 
		};
	}

	public int GetAvailableNumSearchableCountries()
	{
		return 1;
	}

	public int GetAvailableNumSearchableAttributes()
	{
		return 1;
	}

	public int GetAssignmentDuration(ScoutAssignmentTypes type)
	{
		int result = scoutAssignmentTypeDefaults[type].duration;
		if (type == ScoutAssignmentTypes.Club || type == ScoutAssignmentTypes.Country)
		{
			result = Facilities.GetScoutReturnTime(Facilities.GetFacilityLevel(4));
		}
		return result;
	}

	public int GetAssignmentNumberOfPlayersFound(ScoutAssignmentTypes type)
	{
		int num = 10;
		return 5 + num;
	}

	public float GetJudgingPlayerAbilityScore(int Facility)
	{
		return 0.7f;
	}

	public float GetJudgingPlayerPotentialScore(int Facility)
	{
		return 0.7f;
	}

	public float GetJudgingPlayerAttributesScore()
	{
		return 0.7f;
	}
}
