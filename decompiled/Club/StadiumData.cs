using System.Collections.Generic;

public class StadiumData
{
	public const int ClubWorldCupStadiumOne = 50000;

	public const int ClubWorldCupStadiumTwo = 50001;

	public string Name;

	public int Size;

	public int CountryDataID;

	public int StadiumDataID;

	public StadiumData()
	{
		Name = "STADIUM NAME UNKNOWN";
		StadiumDataID = 0;
		Size = 0;
		CountryDataID = 0;
	}

	public StadiumData(string file, List<int> FieldsInFile, int lineEnd)
	{
		Name = "STADIUM NAME UNKNOWN";
		StadiumDataID = DataPackHelpers.ParseSectionAsInt(FieldsInFile[7] + 1, lineEnd, file);
		Size = DataPackHelpers.ParseSectionAsInt(FieldsInFile[2] + 1, FieldsInFile[3], file);
		CountryDataID = DataPackHelpers.ParseSectionAsInt(FieldsInFile[6] + 1, FieldsInFile[7], file);
	}

	public StadiumData(string[] fields)
	{
		Name = "STADIUM NAME UNKNOWN";
		StadiumDataID = int.Parse(fields[7]);
		Size = int.Parse(fields[2]);
		CountryDataID = int.Parse(fields[6]);
	}

	public void LoadNamePack(string nameString)
	{
		Name = nameString;
	}

	public void LoadNamePack(string[] fields)
	{
		int num = Game.instance.g_StadiumDataIDLookup[int.Parse(fields[0])];
		Name = fields[1].Replace('ş', 's').Replace("&#39;", "'");
	}

	public static int GetCountryDataID(int ID)
	{
		if (Game.instance.g_StadiumData.ContainsKey(ID))
		{
			return Game.instance.g_StadiumData[ID].CountryDataID;
		}
		return 0;
	}

	public static string GetName(int ID)
	{
		if (Game.instance.g_StadiumData.ContainsKey(ID))
		{
			return Game.instance.g_StadiumData[ID].Name;
		}
		return ID switch
		{
			50000 => "Al Wakrah Stadium", 
			50001 => "Khalifa International Stadium", 
			_ => "UNKNOWN STADIUM NAME", 
		};
	}

	public static int GetSizeOrig(int ID)
	{
		if (Game.instance.g_StadiumData.ContainsKey(ID))
		{
			return Game.instance.g_StadiumData[ID].Size;
		}
		if (ID == 50000 || ID == 50001)
		{
			return 40000;
		}
		return 0;
	}

	private static void CheckAchievementStadiumSize(int size)
	{
		if (size >= 100000)
		{
			SteamAchievementsController.GetInstance().CheckAchievement(SteamAchievementsController.AchievementEnum.ACHIEVEMENT_1_9, isRequirementsMet: true);
		}
	}

	public static int GetSize(int ID)
	{
		if (Game.instance.g_StadiumData.ContainsKey(ID))
		{
			if (Game.instance.managedClub == null)
			{
				return Game.instance.g_StadiumData[ID].Size;
			}
			if (ID == Club.GetStadiumDataID(Game.instance.CurrentManagedClubId))
			{
				CheckAchievementStadiumSize(Game.instance.g_StadiumData[ID].Size + Game.instance.g_AddedStadiumSeats);
				return Game.instance.g_StadiumData[ID].Size + Game.instance.g_AddedStadiumSeats;
			}
			return Game.instance.g_StadiumData[ID].Size;
		}
		if (ID == 50000 || ID == 50001)
		{
			return 40000;
		}
		return 0;
	}
}
