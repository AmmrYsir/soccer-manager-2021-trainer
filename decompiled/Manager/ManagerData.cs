using System;
using System.Collections.Generic;
using System.Linq;

public class ManagerData
{
	public string Forename = "";

	public string Surname = "";

	public int CountryDataID;

	public DateTime DateOfBirth;

	public short ClubID;

	public string PhotoFilename = "";

	public int ManagerDataID;

	public byte Rating;

	public byte TeamFormation;

	public int PlayerDataID;

	public CareerEntryList CareerHistory = new CareerEntryList();

	public RGYBCode RGYBCode { get; set; }

	public ManagerData()
	{
		Forename = "Unknown";
		Surname = "Manager";
		CountryDataID = 0;
		DateOfBirth = DateTime.MinValue;
		ClubID = 0;
		PhotoFilename = "";
		ManagerDataID = 0;
		Rating = 0;
		TeamFormation = 0;
		PlayerDataID = 0;
	}

	public ManagerData(string file, List<int> FieldsInFile, int ID, int endLine)
	{
		CountryDataID = DataPackHelpers.ParseSectionAsInt(FieldsInFile[3] + 1, FieldsInFile[4], file);
		DateOfBirth = Extensions.FromEpochSeconds(default, DataPackHelpers.ParseSectionAsInt(FieldsInFile[4] + 1, FieldsInFile[5], file));
		ClubID = (short)DataPackHelpers.ParseSectionAsInt(FieldsInFile[5] + 1, FieldsInFile[6], file);
		ManagerDataID = DataPackHelpers.ParseSectionAsInt(FieldsInFile[7] + 1, FieldsInFile[8], file);
		Rating = (byte)DataPackHelpers.ParseSectionAsInt(FieldsInFile[8] + 1, FieldsInFile[9], file);
		TeamFormation = (byte)DataPackHelpers.ParseSectionAsInt(FieldsInFile[9] + 1, FieldsInFile[10], file);
		PlayerDataID = DataPackHelpers.ParseSectionAsInt(FieldsInFile[10] + 1, endLine, file);
	}

	public ManagerData(string[] fields)
	{
		Forename = "";
		Surname = "";
		CountryDataID = int.Parse(fields[3]);
		DateOfBirth = Extensions.FromEpochSeconds(default, int.Parse(fields[4]));
		ClubID = short.Parse(fields[5]);
		PhotoFilename = "";
		ManagerDataID = int.Parse(fields[9]);
		Rating = 1;
		TeamFormation = 0;
		PlayerDataID = 0;
	}

	public void LoadNamePack(string forenameString, string surNameString, string photoUrlString)
	{
		Forename = forenameString;
		Surname = surNameString;
		PhotoFilename = photoUrlString;
	}

	public static void InitRGYBCodes()
	{
		foreach (ManagerData value in Game.instance.g_ManagerData.Values)
		{
			if (value.ClubID != 0)
			{
				value.RGYBCode = Club.GetRGYBCode(value.ClubID);
			}
			else
			{
				value.RGYBCode = RGYBCodeHelpers.GetRandomRGYBCode();
			}
		}
	}

	public static RGYBCode GetRGYBCode(int managerID)
	{
		return Game.instance.g_ManagerData[managerID].RGYBCode;
	}

	public static List<CareerEntry> GetCareerHistory(int managerID)
	{
		List<CareerEntry> result = null;
		Dictionary<int, ManagerData> g_ManagerData = Game.instance.g_ManagerData;
		if (g_ManagerData.ContainsKey(managerID))
		{
			result = g_ManagerData[managerID].CareerHistory.CareerEntries;
		}
		return result;
	}

	public static DateTime GetCurrentJobStartDate(int managerID)
	{
		if (Game.instance.g_ManagerData[managerID].CareerHistory.CareerEntries.Count > 0)
		{
			return Game.instance.g_ManagerData[managerID].CareerHistory.CareerEntries.Last().StartDate;
		}
		return DateTime.MinValue;
	}

	public static string GetName(int ID)
	{
		string result = Translation.GetText("Squad-Manager");
		if (ID == 0)
		{
			result = CustomerDataController.Instance.CustomerDataHolder.Userdata.UserManagerProfile.ManagerUserName;
			result = result ?? Translation.GetText("Squad-Manager");
		}
		else if (Game.instance.g_ManagerData.ContainsKey(ID))
		{
			result = Game.instance.g_ManagerData[ID].Forename + " " + Game.instance.g_ManagerData[ID].Surname;
		}
		return result;
	}

	public static int GetCountryDataID(int ID)
	{
		if (Game.instance.g_ManagerData.ContainsKey(ID))
		{
			return Game.instance.g_ManagerData[ID].CountryDataID;
		}
		return 0;
	}

	public static short GetClubID(int managerID)
	{
		return Game.instance.g_ManagerData[managerID].ClubID;
	}
}
