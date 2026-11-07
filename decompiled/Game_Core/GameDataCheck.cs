using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class GameDataCheck : MonoBehaviour
{
	private class MostExpensivePlayers
	{
		private const int LIST_SIZE = 200;

		public List<KeyValuePair<int, float>> List = new List<KeyValuePair<int, float>>();

		private float minPlayerValue = float.MaxValue;

		private int minPlayerIdx = -1;

		public bool CheckIfValid(int playerId, float value)
		{
			if (List.Count < 200)
			{
				return true;
			}
			return value > minPlayerValue;
		}

		public void Add(int playerId, float value)
		{
			if (List.Count < 200)
			{
				List.Add(new KeyValuePair<int, float>(playerId, value));
				if (value < minPlayerValue)
				{
					minPlayerValue = value;
					minPlayerIdx = List.Count - 1;
				}
			}
			else
			{
				List[minPlayerIdx] = new KeyValuePair<int, float>(playerId, value);
				SetMinValue();
			}
		}

		public void Sort()
		{
			List.Sort((KeyValuePair<int, float> a, KeyValuePair<int, float> b) =>
			{
				if (a.Value < b.Value)
				{
					return 1;
				}
				return (a.Value > b.Value) ? (-1) : 0;
			});
		}

		private void SetMinValue()
		{
			minPlayerValue = float.MaxValue;
			minPlayerIdx = -1;
			for (int i = 0; i < 200; i++)
			{
				if (List[i].Value < minPlayerValue)
				{
					minPlayerValue = List[i].Value;
					minPlayerIdx = i;
				}
			}
		}
	}

	private List<int> leagueIds;

	private List<int> unplayableLeagueIds;

	private List<int> cupIds;

	private List<int> continentalCupIds;

	private List<int> playerIds;

	public static short LAST_EUROPEAN_SEASON = 2019;

	public static short LAST_REST_OF_THE_WORLD_SEASON = 2018;

	public static string CHECKS_DIRECTORY = "DataChecks/";

	public void RunGameDataChecks()
	{
		Init();
		RunLeagueDataChecks();
		RunStadiumDataChecks();
		RunCompetitionFormatChecks();
		RunCompetitionHistoryChecks();
	}

	private void Init()
	{
		leagueIds = new List<int>(Game.instance.g_LeagueData.Keys);
		unplayableLeagueIds = new List<int>(Game.instance.g_UnplayableLeagueData.Keys);
		cupIds = new List<int>(Game.instance.g_CupData.Keys);
		continentalCupIds = new List<int>(Game.instance.g_continentalCompetitions.Keys);
		playerIds = new List<int>(Game.instance.g_PlayerData.Keys);
		if (!Directory.Exists(CHECKS_DIRECTORY))
		{
			Directory.CreateDirectory(CHECKS_DIRECTORY);
		}
	}

	private bool RunLeagueDataChecks()
	{
		return true;
	}

	private bool RunStadiumDataChecks()
	{
		bool result = true;
		StreamWriter streamWriter = new StreamWriter(CHECKS_DIRECTORY + "stadiumdata.csv");
		streamWriter.WriteLine("Club ID,Club Name,Club Data ID,Stadium ID,Stadium Name");
		streamWriter.Flush();
		for (int i = 0; i < leagueIds.Count; i++)
		{
			byte key = (byte)leagueIds[i];
			LeagueData leagueData = Game.instance.g_LeagueData[key];
			for (int j = 0; j < leagueData.ClubIDs.Count; j++)
			{
				short num = leagueData.ClubIDs[j];
				short clubDataID = Club.GetClubDataID(num);
				int stadiumDataID = Club.GetStadiumDataID(num);
				if (!StadiumStaticData.StadiumList.ContainsKey(clubDataID))
				{
					Debug.LogError("Clubs stadium does not have static data - clubId:" + num + " clubName:" + Club.GetName(num) + " clubDataId:" + clubDataID + " stadiumId:" + stadiumDataID + " stadiumName:" + StadiumData.GetName(stadiumDataID));
					streamWriter.WriteLine(num + "," + Club.GetName(num) + "," + clubDataID + "," + stadiumDataID + "," + StadiumData.GetName(stadiumDataID));
					streamWriter.Flush();
					result = false;
				}
			}
		}
		return result;
	}

	private bool RunCompetitionFormatChecks()
	{
		bool result = true;
		Dictionary<int, LeagueData> g_LeagueData = Game.instance.g_LeagueData;
		List<int> list = new List<int>(g_LeagueData.Keys);
		for (int i = 0; i < list.Count; i++)
		{
			int num = list[i];
			LeagueData leagueData = g_LeagueData[num];
			int num2 = 0;
			num2 = leagueData.LeagueID switch
			{
				28 => 20, 
				29 => 24, 
				30 => 24, 
				31 => 24, 
				32 => 24, 
				89 => 20, 
				90 => 22, 
				51 => 20, 
				52 => 20, 
				36 => 20, 
				37 => 20, 
				38 => 18, 
				39 => 18, 
				40 => 18, 
				41 => 20, 
				95 => 18, 
				96 => 18, 
				14 => 20, 
				15 => 20, 
				_ => leagueData.ClubIDs.Count, 
			};
			if (leagueData.ClubIDs.Count != num2)
			{
				Debug.LogError("League does not have the expected number of teams - this could break the custom cup leagueId:" + num + " leagueName:" + CountryData.GetName(leagueData.AssociationID) + " leagueDivision:" + leagueData.Division);
				result = false;
			}
		}
		return result;
	}

	private bool RunCompetitionHistoryChecks()
	{
		bool result = true;
		StreamWriter streamWriter = new StreamWriter(CHECKS_DIRECTORY + "competitiondata.csv");
		streamWriter.WriteLine("Year,Comp Type,Comp ID,Comp Name,Country");
		streamWriter.Flush();
		for (int i = 0; i < leagueIds.Count; i++)
		{
			byte b = (byte)leagueIds[i];
			LeagueData leagueData = Game.instance.g_LeagueData[b];
			if (leagueData.Division == 1)
			{
				short num = ((CountryData.GetContinent(leagueData.AssociationID) == 1) ? LAST_EUROPEAN_SEASON : LAST_REST_OF_THE_WORLD_SEASON);
				if (!leagueData.Honours.ContainsKey(num))
				{
					Debug.LogError("League does not winners for " + num + " - leagueId:" + b + " leagueDataId:" + leagueData.LeagueID + " leagueName:" + leagueData.Name + " leagueCounty:" + CountryData.GetName(leagueData.AssociationID));
					streamWriter.WriteLine(num + ",League," + leagueData.LeagueID + "," + leagueData.Name + "," + CountryData.GetName(leagueData.AssociationID));
					streamWriter.Flush();
					result = false;
				}
			}
		}
		for (int j = 0; j < cupIds.Count; j++)
		{
			int num2 = cupIds[j];
			CupData cupData = Game.instance.g_CupData[num2];
			if (cupData.CupOrderIndex == 1)
			{
				int countryIDFromCode = CountryData.GetCountryIDFromCode(cupData.CountryCode);
				short num3 = ((CountryData.GetContinent(countryIDFromCode) == 1) ? LAST_EUROPEAN_SEASON : LAST_REST_OF_THE_WORLD_SEASON);
				if (!cupData.Honours.ContainsKey(num3))
				{
					Debug.LogError("Cup does not winners for " + num3 + " - cupId:" + num2 + " cupDataId:" + cupData.CupDataID + " cupName:" + cupData.Name + " cupCounty:" + CountryData.GetName(countryIDFromCode));
					streamWriter.WriteLine(num3 + ",Cup," + cupData.CupDataID + "," + cupData.Name + "," + CountryData.GetName(countryIDFromCode));
					streamWriter.Flush();
					result = false;
				}
			}
		}
		for (int k = 0; k < continentalCupIds.Count; k++)
		{
			int num4 = continentalCupIds[k];
			ContinentalComp continentalComp = Game.instance.g_continentalCompetitions[num4];
			short num5 = (continentalComp.continents.Contains(1) ? LAST_EUROPEAN_SEASON : LAST_REST_OF_THE_WORLD_SEASON);
			if (!continentalComp.Honours.ContainsKey(num5))
			{
				Debug.LogError("Continental Cup does not winners for " + num5 + " - cupId:" + num4 + " cupName:" + continentalComp.name);
				streamWriter.WriteLine(num5 + ",ContiCup," + num4 + "," + continentalComp.name + ",");
				streamWriter.Flush();
				result = false;
			}
		}
		return result;
	}

	public void RunGetMostExpensivePlayers()
	{
		Init();
		GetMostExpensivePlayers();
	}

	private void GetMostExpensivePlayers()
	{
		StreamWriter streamWriter = new StreamWriter(CHECKS_DIRECTORY + "mostexpensivedata.tsv");
		streamWriter.WriteLine("Name\tRating\tAge\tClub\tCountry\tPosition\tValue");
		streamWriter.Flush();
		MostExpensivePlayers mostExpensivePlayers = new MostExpensivePlayers();
		for (int i = 0; i < playerIds.Count; i++)
		{
			int num = playerIds[i];
			float value = Player.GetValue(num);
			if (mostExpensivePlayers.CheckIfValid(num, value))
			{
				mostExpensivePlayers.Add(num, value);
			}
		}
		mostExpensivePlayers.Sort();
		for (int j = 0; j < mostExpensivePlayers.List.Count; j++)
		{
			int key = mostExpensivePlayers.List[j].Key;
			float value2 = mostExpensivePlayers.List[j].Value;
			streamWriter.WriteLine(Player.GetName(key) + "\t" + Player.GetDisplayRating(key) + "\t" + Player.GetAge(key) + "\t" + Club.GetName(Player.GetClubID(key)) + "\t" + CountryData.GetName(PlayerData.GetCountryDataID(key)) + "\t" + Helpers.GetPlayerPositionString(Player.GetPosition(key)) + "\t£" + Helpers.RoundMonetaryValue(value2));
			streamWriter.Flush();
		}
	}
}
