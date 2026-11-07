using System;
using System.Collections.Generic;
using System.Linq;

public class YouthAcademy
{
	private static YouthAcademy _instance;

	public static void ClearYouthAcademy()
	{
		Game.instance.g_youthManager = new YouthManager();
		Game.instance.g_youthManager.NextYouthIntakeDate = Game.instance.CurrentDate.AddMonths(6);
	}

	public static int GetYouthIntakeWeeks(int level)
	{
		return FacilitiesConfig.YOUTH_INTAKE_FREQ_BASE_AMOUNT + Facilities.GetUpgradeIncrease(11, level);
	}

	public static byte GenerateYouthRating()
	{
		int upgradeLevel = Facilities.GetUpgradeLevel(12);
		Random random = new Random();
		return upgradeLevel switch
		{
			0 => (byte)random.Next(60, 63), 
			1 => (byte)random.Next(60, 68), 
			2 => (byte)random.Next(63, 73), 
			3 => (byte)random.Next(68, 76), 
			4 => (byte)random.Next(73, 79), 
			5 => (byte)random.Next(77, 82), 
			_ => (byte)random.Next(60, 63), 
		};
	}

	public static byte GenerateYouthPotential()
	{
		int upgradeLevel = Facilities.GetUpgradeLevel(13);
		Random random = new Random();
		return upgradeLevel switch
		{
			0 => (byte)random.Next(63, 73), 
			1 => (byte)random.Next(68, 79), 
			2 => (byte)random.Next(77, 84), 
			3 => (byte)random.Next(81, 89), 
			4 => (byte)random.Next(86, 94), 
			5 => (byte)random.Next(91, 99), 
			_ => (byte)random.Next(63, 67), 
		};
	}

	public static int GetNationalityFromPotential(int potential, int pos)
	{
		if (potential <= 90)
		{
			return Club.GetCountryDataID(Game.GetCurrentUserClubID());
		}
		byte b = (byte)((potential > 94) ? 5 : 4);
		try
		{
			return Helpers.WeightedRand(Game.instance.g_player_respawnprofile.nationalitychance[Helpers.GetPlayerBasicPosition(pos), b].Get());
		}
		catch
		{
			Game.instance.g_player_respawnprofile.RespawnProfileSetNationalityChance();
			return Helpers.WeightedRand(Game.instance.g_player_respawnprofile.nationalitychance[Helpers.GetPlayerBasicPosition(pos), b].Get());
		}
	}

	public static RGYBCode GetColourForYouth()
	{
		int rand = Game.instance.Random.GetRand(0, 100);
		int num = FacilitiesConfig.YOUTH_COMPATIBILITY_CHANCE_SAME[Facilities.GetUpgradeLevel(16)];
		int num2 = num + FacilitiesConfig.YOUTH_COMPATIBILITY_CHANCE_ADJ;
		_ = FacilitiesConfig.YOUTH_COMPATIBILITY_CHANCE_OPP[Facilities.GetUpgradeLevel(16)];
		if (rand < num)
		{
			return Club.GetRGYBCode(Game.GetCurrentUserClubID());
		}
		if (rand < num2)
		{
			return RGYBCodeHelpers.GetNextCodeClockwise(Club.GetRGYBCode(Game.GetCurrentUserClubID()));
		}
		return RGYBCodeHelpers.GetOppositeCode(Club.GetRGYBCode(Game.GetCurrentUserClubID()));
	}

	public static void ProcessYouthIntake()
	{
		if (!(Game.instance.g_youthManager.NextYouthIntakeDate <= Game.instance.CurrentDate))
		{
			return;
		}
		int intakeSize = GetIntakeSize(Facilities.GetFacilityLevel(1));
		Random random = new Random();
		List<int> list = Game.instance.g_PlayerData.Keys.ToList();
		Game.instance.g_youthManager.CurrentIntakePlayers.Clear();
		for (int i = 0; i < intakeSize; i++)
		{
			int num = list[random.Next(list.Count)];
			byte potential = GenerateYouthPotential();
			int num2 = GetNationalityFromPotential(potential, PlayerData.GetPosition(num));
			if (num2 <= 0)
			{
				num2 = Club.GetCountryDataID(Game.GetCurrentUserClubID());
			}
			RGYBCode colourForYouth = GetColourForYouth();
			int num3 = PlayerRespawn.RespawnYouthPlayerForClub(num, random.Next(16, 18), num2, potential, colourForYouth);
			Game.instance.g_youthManager.CurrentIntakePlayers.Add(num3);
			CreatePredictionsForPlayer(num3);
		}
		EventLoggingController.LogEvent("youthIntake");
		int num4 = GetYouthIntakeWeeks(Facilities.GetFacilityLevel(1)) * 7;
		Game.instance.g_youthManager.NextYouthIntakeDate = Game.instance.g_youthManager.NextYouthIntakeDate.AddDays(num4);
		Game.instance.g_youthManager.YouthThisIntake = 0;
		TasklistManager.CreateTask(TaskType.YouthIntake, removeOnClick: true, 63);
	}

	public static int GetIntakeSize(int level)
	{
		return FacilitiesConfig.YOUTH_INTAKE_SIZE_BASE_AMOUNT + Facilities.GetUpgradeIncrease(10, level);
	}

	public static int GetPerIntakeAmount(int level)
	{
		return FacilitiesConfig.YOUTH_PER_INTAKE_BASE_AMOUNT + Facilities.GetUpgradeIncrease(14, level);
	}

	public static int GetCapacity(int level)
	{
		return FacilitiesConfig.YOUTH_CAPACITY_BASE_AMOUNT + Facilities.GetUpgradeIncrease(15, level);
	}

	public static float GetTrainingDebuff()
	{
		float num = 10f;
		if (UserManagerProfileController.UserHasTrait(ManagerTraitArea.YouthTraining))
		{
			num = (float)UserManagerProfileController.ApplyTrait(ManagerTraitArea.YouthTraining, num);
		}
		return num;
	}

	public static int GetPotentialRange()
	{
		return FacilitiesConfig.YOUTH_REPORT_RANGE - Facilities.GetUpgradeLevel(18);
	}

	public static int GetRatingRange()
	{
		return FacilitiesConfig.YOUTH_REPORT_RANGE - Facilities.GetUpgradeLevel(17);
	}

	public static void SetPotentialRange(int player, YouthPredictedRatings youth)
	{
		try
		{
			int potentialRange = GetPotentialRange();
			int num = new Random().Next(-3, 3);
			int potential = Game.instance.g_PlayerData[player].Potential;
			youth.LowerPotential = (int)Attributes.MapRating(potential - (potentialRange / 2 + num));
			youth.UpperPotential = (int)Attributes.MapRating(potential + (potentialRange / 2 - num));
		}
		catch
		{
		}
	}

	public static void SetRatingRange(int player, YouthPredictedRatings youth)
	{
		int ratingRange = GetRatingRange();
		int num = new Random().Next(-3, 3);
		int rating = Game.instance.g_PlayerData[player].GetRating();
		youth.LowerRating = (int)Attributes.MapRating(rating - (ratingRange / 2 + num));
		youth.UpperRating = (int)Attributes.MapRating(rating + (ratingRange / 2 - num));
	}

	public static void CreatePredictionsForPlayer(int player)
	{
		YouthPredictedRatings youthPredictedRatings = new YouthPredictedRatings();
		SetPotentialRange(player, youthPredictedRatings);
		SetRatingRange(player, youthPredictedRatings);
		Game.instance.g_youthManager.YouthPlayerPredictions.Add(player, youthPredictedRatings);
	}

	internal static int GetYouthBlobs()
	{
		return 0;
	}

	public static void PromoteYouthPlayer(int playerId)
	{
		if (playerId != 0)
		{
			short currentUserClubID = Game.GetCurrentUserClubID();
			if (Game.instance.managedClub != null && Game.instance.managedClub.Length != 0)
			{
				ManagedClub obj = Game.instance.managedClub[Game.instance.g_currUser];
				Player.SetReserveTeam(playerId);
				Player.RemoveFromShortList(playerId);
				int num = Game.instance.g_PlayerData[playerId].GenerateNewContractLengthForUnmanagedClub();
				Game.instance.g_PlayerData[playerId].ClubId = currentUserClubID;
				PlayerSquad.AddPlayer(currentUserClubID, playerId);
				Game.instance.g_PlayerData[playerId].CreateNewContract(Player.GenerateNewContractWages(playerId, currentUserClubID, (byte)num), Player.ConvertContractYearsToDate(num, Game.instance.g_ClubData[currentUserClubID].LeagueID));
				PlayerStatusInfo.Init(obj.PlayerStatus, playerId, 0, isTransfer: false);
			}
		}
	}

	public static YouthAcademy GetInstance()
	{
		if (_instance != null)
		{
			return _instance;
		}
		_instance = new YouthAcademy();
		return _instance;
	}

	internal static bool IsPlayerInYouthAcademy(int playerID)
	{
		return Game.instance.g_youthManager.YouthPlayers.Contains(playerID);
	}

	public static void RemovePlayerFromYouthPlayersList(int playerID)
	{
		if (Game.instance.g_youthManager.YouthPlayers.Contains(playerID))
		{
			Game.instance.g_youthManager.YouthPlayers.Remove(playerID);
		}
	}
}
