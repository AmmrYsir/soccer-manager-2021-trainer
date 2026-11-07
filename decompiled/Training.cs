using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Training
{
	public enum TrainingSession
	{
		AVAILABLE,
		LOCKED,
		ATTACKING,
		DEFENDING,
		GOALKEEPING,
		PHYSICAL,
		TECHNICAL,
		TACTICAL
	}

	public const int DAYS_BETWEEN_TRAINING = 7;

	public const int IMPROVEMENTS_PER_DRILL = 5;

	public const int GK_IMPROVEMENTS_PER_DRILL = 1;

	public static int ATTRIBUTE_ALTERATION_CAP = TrainingConfig.AttributeCap;

	public static int ATTRIBUTE_TOTAL_CAP = TrainingConfig.AttributeTotalCap;

	public const int PHYSICAL_ATTRIBUTE = 0;

	public const int DEFENDING_ATTRIBUTE = 1;

	public const int ATTACKING_ATTRIBUTE = 2;

	public const int GOALKEEPING_ATTRIBUTE = 3;

	public const int SETPIECES_ATTRIBUTE = 4;

	public const int TRAINING_MORNING = 0;

	public const int TRAINING_EARLY_AFTERNOON = 1;

	public const int TRAINING_LATE_AFTERNOON = 2;

	public const int INTENSITY_LOW = 0;

	public const int INTENSITY_MEDIUM = 1;

	public const int INTENSITY_HIGH = 2;

	public static float[] attrIncreases = new float[3]
	{
		TrainingConfig.AttributeIncreaseLow,
		TrainingConfig.AttributeIncreaseMid,
		TrainingConfig.AttributeIncreaseHigh
	};

	public static double[] injuryChances = new double[3]
	{
		TrainingConfig.InjuryChanceLow,
		TrainingConfig.InjuryChangeMid,
		TrainingConfig.InjuryChangeHigh
	};

	public static double[] fitnessMods = new double[3]
	{
		TrainingConfig.FitnessDecreaseLow,
		TrainingConfig.FitnessDecreaseMid,
		TrainingConfig.FitnessDecreaseHigh
	};

	public static int fitnessModsGK = -1;

	public static float facilityBoostAmount = TrainingConfig.FacilityBoostAmount;

	public static float untrainDecreaseAmount = TrainingConfig.UntrainDecreaseAmount;

	public static float restIncreaseAmount = TrainingConfig.RestIncreaseAmount;

	public static float cohesionWeeklyDecreaseAmount = 0.01f;

	public int TotalGameTime;

	public static Dictionary<TrainingSession, int> DrillFitnessBoosts = new Dictionary<TrainingSession, int>
	{
		{
			TrainingSession.ATTACKING,
			2
		},
		{
			TrainingSession.DEFENDING,
			2
		},
		{
			TrainingSession.GOALKEEPING,
			1
		},
		{
			TrainingSession.PHYSICAL,
			5
		},
		{
			TrainingSession.TACTICAL,
			1
		},
		{
			TrainingSession.TECHNICAL,
			2
		}
	};

	public static Dictionary<int, double> STARTING_EXTRA_ATTRIBUTE_POINTS = new Dictionary<int, double>
	{
		{ 2, 16.0 },
		{ 1, 8.0 },
		{ 3, 8.0 },
		{ 0, 8.0 },
		{ 4, 8.0 }
	};

	public static Dictionary<int, double> EXTRA_ATTRIBUTE_POINTS_LEVEL_MULTIPLIER = new Dictionary<int, double>
	{
		{ 2, 0.0 },
		{ 1, 0.0 },
		{ 3, 0.0 },
		{ 0, 0.0 },
		{ 4, 0.0 }
	};

	public static Dictionary<int, double> BOOST_EXTRA_ATTRIBUTE_POINTS_PER_LEVEL = new Dictionary<int, double>
	{
		{ 2, 2.0 },
		{ 1, 1.0 },
		{ 3, 1.0 },
		{ 0, 1.0 },
		{ 4, 1.0 }
	};

	public static int DEFENSIVE_DRILL_POSITION_MASK = 1023;

	public static int ATTACKING_DRILL_POSITION_MASK = 65520;

	public static Dictionary<int, int[]> ATTRIBUTE_TRAINING_GROUPS = new Dictionary<int, int[]>
	{
		{
			0,
			new int[5] { 6, 4, 25, 5, 27 }
		},
		{
			1,
			new int[5] { 10, 11, 12, 0, 2 }
		},
		{
			2,
			new int[11]
			{
				14, 15, 16, 17, 18, 19, 20, 21, 13, 1,
				7
			}
		},
		{
			3,
			new int[13]
			{
				10, 12, 13, 14, 15, 4, 1, 16, 5, 6,
				0, 2, 11
			}
		},
		{
			4,
			new int[3] { 23, 22, 24 }
		}
	};

	public List<int> selectedDrills;

	public Dictionary<int, List<int>> selectedDrillsByDay;

	public Dictionary<int, float> drillLevel;

	public Dictionary<int, float> branchBoostLevels;

	public bool[] branchBoostUnlocks = new bool[3];

	public double[] extraAttributePointsUnlocked;

	private Dictionary<int, float[]> allPlayerAttributes;

	private static Training _instance;

	private Dictionary<int, List<int>> attributeIncrease;

	public static void AdjustTrainingOnVersion()
	{
		if (Save.currentSlot > -1 && Save.saveVersionCompareTo("1.1.3") > 0)
		{
			TrainingDrills.drillFitnessMod[19] = new float[1] { 16f };
		}
	}

	public static bool ProcessIfNotMatchDay(int time)
	{
		NextFixtureInfo nextFixture = Game.instance.GetNextFixture(Game.GetCurrentUserClubID());
		DateTime currentDate = Game.instance.CurrentDate;
		if (nextFixture != null && (nextFixture.date.Day != currentDate.Day || nextFixture.date.Month != currentDate.Month))
		{
			return Process();
		}
		return false;
	}

	public static bool Process()
	{
		AdjustTrainingOnVersion();
		ProcessMediaResponse();
		Training instance = GetInstance();
		instance.ProcessTraining();
		instance.RunTrainingDrills();
		ReduceChangesForDepartedPlayers();
		ChanceOfRandomInjury();
		SqlLogTraining();
		return PromptToRunTraining();
	}

	private static void SqlLogTraining()
	{
	}

	public static byte GetSessionConstant(TrainingSession type)
	{
		return type switch
		{
			TrainingSession.ATTACKING => (byte)4, 
			TrainingSession.DEFENDING => (byte)3, 
			TrainingSession.GOALKEEPING => (byte)2, 
			TrainingSession.PHYSICAL => (byte)7, 
			TrainingSession.TACTICAL => (byte)6, 
			TrainingSession.TECHNICAL => (byte)5, 
			_ => (byte)0, 
		};
	}

	public static string GetSessionName(TrainingSession type)
	{
		return type switch
		{
			TrainingSession.ATTACKING => Translation.GetText("attacking"), 
			TrainingSession.AVAILABLE => Translation.GetText("status-available"), 
			TrainingSession.DEFENDING => Translation.GetText("defending"), 
			TrainingSession.GOALKEEPING => Translation.GetText("training-type-goalkeeping"), 
			TrainingSession.PHYSICAL => Translation.GetText("attr-type-physical"), 
			TrainingSession.TACTICAL => Translation.GetText("tactical"), 
			TrainingSession.TECHNICAL => Translation.GetText("technical"), 
			_ => "", 
		};
	}

	public static int GetSessionFitnessIncrease(TrainingSession type)
	{
		if (DrillFitnessBoosts.ContainsKey(type))
		{
			return DrillFitnessBoosts[type];
		}
		return 0;
	}

	public static float GetTrainingInjuryPronePercentage()
	{
		float num = 100f - TrainingConfig.InjuryChance;
		if (UserManagerProfileController.UserHasTrait(ManagerTraitArea.TrainingInjury))
		{
			num -= (float)(UserManagerProfileController.GetCurrentTraitLevel(ManagerTraitArea.TrainingInjury) + 1);
		}
		float clubzoneInjuryPrevention = Facilities.GetClubzoneInjuryPrevention();
		float num2 = num / 100f * clubzoneInjuryPrevention;
		return num - num2;
	}

	public static float GetCapacityDecrease()
	{
		int num = Club.GetNumPlayers(Game.GetCurrentUserClubID()) - Facilities.GetValueForNonStarUpgrade(1, Facilities.GetFacilityLevel(2));
		if (num <= 0)
		{
			return 0f;
		}
		float num2 = (float)num * TrainingConfig.CapacityPenaltyPerc;
		if (num2 > 50f)
		{
			num2 = 50f;
		}
		return num2;
	}

	public static float GetTrainingIncrease(int playerID, int[] sessionAmounts)
	{
		int playerBasicPosition = Helpers.GetPlayerBasicPosition(PlayerData.GetPosition(playerID));
		float num = 1f;
		float num2 = Player.GetRatingState(playerID) switch
		{
			PlayerRatingState.INCREASING => TrainingConfig.IncreaseDaysAmount, 
			PlayerRatingState.DECLINING => TrainingConfig.DeclineDecreaseAmount, 
			_ => TrainingConfig.DeclineDelayAmount, 
		};
		if (Player.GetMorale(playerID) <= TrainingConfig.MoraleLevelDecrease)
		{
			num -= (float)TrainingConfig.MoraleDecreasePerc / 100f;
		}
		num2 = playerBasicPosition switch
		{
			0 => num2 * (1f + Facilities.GetDrillIncrease(2) * (float)sessionAmounts[2] / 100f) * num, 
			1 => num2 * (1f + Facilities.GetDrillIncrease(3) * (float)sessionAmounts[1] / 100f) * num, 
			2 => num2 * (1f + Facilities.GetDrillIncrease(5) * (float)sessionAmounts[4] / 100f) * num, 
			3 => num2 * (1f + Facilities.GetDrillIncrease(4) * (float)sessionAmounts[0] / 100f) * num, 
			_ => TrainingConfig.IncreaseDaysAmount, 
		} * Game.instance.g_trainingManager.CapacityPen;
		if (Player.GetAge(playerID) < 24)
		{
			if (Game.instance.g_trainingManager.DebugYouthTraining == 0)
			{
				int num3 = PlayerCareerStats.GetCurrentSeasonStats(playerID).Stats[8];
				if (Game.instance.g_trainingManager.TotalGameTime > 0)
				{
					float num4 = num3 / Game.instance.g_trainingManager.TotalGameTime * 100;
					if (num4 >= (float)TrainingConfig.YouthGameTimeHighAmount)
					{
						num2 *= TrainingConfig.YouthGameTimeHighBoost;
					}
					else if (num4 >= (float)TrainingConfig.YouthGameTimeLowAmount)
					{
						num2 *= TrainingConfig.YouthGameTimeLowBoost;
					}
					num2 *= (float)(0.75 + 0.05 * (double)Facilities.GetUpgradeLevel(19));
				}
			}
			else
			{
				num2 = UseDebugYouthAmount(num2);
			}
		}
		return num2;
	}

	private static float UseDebugYouthAmount(float amount)
	{
		if (Game.instance.g_trainingManager.DebugYouthTraining == 1)
		{
			return amount *= TrainingConfig.YouthGameTimeLowBoost;
		}
		if (Game.instance.g_trainingManager.DebugYouthTraining == 2)
		{
			return amount *= TrainingConfig.YouthGameTimeHighBoost;
		}
		return amount;
	}

	public static void ApplyTeamChemistryIncrease(float perc, int playerID)
	{
		Player.GetTeamCohesion(playerID).AlterTrainingCohesion(perc);
	}

	public static int GetPlayerTrainingChemistryIncrease(float perc, int playerID)
	{
		PlayerTeamCohesion teamCohesion = Player.GetTeamCohesion(playerID);
		_ = teamCohesion.Total;
		return Mathf.Clamp((int)((float)teamCohesion.Total * perc) - teamCohesion.Total, 1, 20);
	}

	public static void ApplyLoanedOutPlayerIncrease(int playerID)
	{
		try
		{
			Game.instance.g_trainingDrillManager.TrainingPlayer[playerID].DaysIncrease += 7f;
		}
		catch
		{
			Game.instance.g_trainingDrillManager.AddTrainingPlayer(playerID);
			Game.instance.g_trainingDrillManager.TrainingPlayer[playerID].DaysIncrease += 7f;
		}
	}

	public static void ApplyTeamChemistryDecrease(int playerID)
	{
		float num = (float)Player.GetTeamCohesion(playerID).Total * cohesionWeeklyDecreaseAmount;
		Player.GetTeamCohesion(playerID).AlterTrainingCohesion(0f - num);
	}

	public void ApplyTrainingIncrease(float days, int playerID)
	{
		TrainingPlayer trainingPlayer = Game.instance.g_trainingDrillManager.TrainingPlayer[playerID];
		switch (Player.GetRatingState(playerID))
		{
		case PlayerRatingState.INCREASING:
		{
			trainingPlayer.DaysIncrease += days;
			float num2 = days - TrainingConfig.IncreaseDaysAmount;
			if (num2 < 0f)
			{
				num2 = 0f;
			}
			trainingPlayer.DeclineDelay += num2;
			Game.instance.g_trainingManager.LastReportSummary.ImprovedPlayersAmount++;
			break;
		}
		case PlayerRatingState.PEAK:
			trainingPlayer.DeclineDelay += days;
			Game.instance.g_trainingManager.LastReportSummary.PlayersAtPeakAmount++;
			break;
		case PlayerRatingState.DECLINING:
		{
			float num = days - TrainingConfig.IncreaseDaysAmount;
			if (num < 0f)
			{
				num = 0f;
			}
			trainingPlayer.DeclineDecrease += num;
			Game.instance.g_trainingManager.LastReportSummary.PlayersPastPeakAmount++;
			break;
		}
		}
	}

	public static bool PromptToRunTraining()
	{
		return CanTrainingBeRun();
	}

	public static void ProcessMediaResponse()
	{
		List<RandEvReaction> list = new List<RandEvReaction>();
		List<RandEvReaction> list2 = new List<RandEvReaction>();
		List<int> list3 = new List<int>();
		List<int> list4 = new List<int>();
		List<int> list5 = new List<int>();
		int num = 0;
		int num2 = 0;
		for (num = 0; num < Game.instance.g_futureRespQueue.Count; num++)
		{
			RandEvReaction randEvReaction = Game.instance.g_futureRespQueue[num];
			if (randEvReaction.type == 1)
			{
				list.Add(randEvReaction);
				list3.Add(num);
				for (int i = 0; i < randEvReaction.playerIDs.Count; i++)
				{
					num2 = randEvReaction.playerIDs[i];
					list4.Add(num2);
				}
			}
			else if (randEvReaction.type == 2)
			{
				list2.Add(randEvReaction);
				list3.Add(num);
				for (int j = 0; j < randEvReaction.playerIDs.Count; j++)
				{
					num2 = randEvReaction.playerIDs[j];
					list5.Add(num2);
				}
			}
		}
		for (num = 0; num < list3.Count; num++)
		{
			int index = list3.Count - num - 1;
			_ = list3[index];
			Game.instance.g_futureRespQueue.RemoveRange(index, 1);
		}
	}

	public static void ResetTraining()
	{
		Game.instance.g_trainingManager.ResetSessions();
	}

	public static Training GetInstance()
	{
		if (_instance != null)
		{
			return _instance;
		}
		_instance = new Training();
		return _instance;
	}

	public static void InitTraining()
	{
		Game.instance.g_trainingDrillManager.Initialise();
	}

	public void ProcessTraining(bool debugTraining = false)
	{
		PlayerSquad players = Club.GetPlayers(Game.instance.CurrentManagedClubId);
		DateTime currentDate = Game.instance.CurrentDate;
		if (currentDate.Month == 1)
		{
			_ = currentDate.Day == 1;
		}
		else
			_ = 0;
		for (int i = 0; i < players.Count; i++)
		{
			int num = players[i];
			if (!Game.instance.g_trainingDrillManager.TrainingPlayer.ContainsKey(num))
			{
				AddTrainingPlayer(num);
			}
		}
	}

	public static int GetNumTraingingBlobs()
	{
		int num = 0;
		if (CanTrainingBeRun())
		{
			num = 1;
		}
		foreach (int key in Game.instance.FacilityManager.FacilityBlobs.Keys)
		{
			if (Game.instance.FacilityManager.FacilityBlobs[key] == 2)
			{
				num++;
			}
		}
		return num;
	}

	public static bool CanTrainingBeRun()
	{
		return Game.instance.g_trainingManager.IsTrainingAvailable();
	}

	public void RunTrainingDrills()
	{
		if (!PlayerPrefs.HasKey("prefanalyticsruntraining"))
		{
			PlayerPrefs.SetInt("prefanalyticsruntraining", 1);
		}
		PlayerSquad players = Club.GetPlayers(Game.instance.CurrentManagedClubId);
		for (int i = 0; i < players.Count; i++)
		{
			int num = players[i];
			if (!Game.instance.g_trainingDrillManager.TrainingPlayer.ContainsKey(num))
			{
				AddTrainingPlayer(num);
			}
		}
		ProcessTrainingDrills(players);
		Game.instance.g_trainingDrillManager.LastTrainingReport.lastTrainingRun = Game.instance.CurrentDate;
		UI.GetInstance().UpdateFacilitiesSideBarBlobs();
		PlayerList.RefreshManagedPlayerCachedRatings();
	}

	public static void AddTrainingPlayer(int playerId)
	{
		Game.instance.g_trainingDrillManager.AddTrainingPlayer(playerId);
	}

	public void InitTrainingDrillEffects()
	{
		selectedDrills = new List<int>();
		Dictionary<int, int> dictionary = Game.instance.g_trainingDrillManager.SelectedDrills;
		for (int i = 0; i < dictionary.Count; i++)
		{
			selectedDrills.Add(dictionary[i + 1]);
		}
		FillDrillAndBoostLevelData();
	}

	public void ProcessTrainingDrills(PlayerSquad allPlayers)
	{
		Game.instance.g_trainingManager.LastReportSummary = new TrainingReportSummary();
		attributeIncrease = new Dictionary<int, List<int>>();
		GetTrainedAttributes();
		List<int> list = new List<int>();
		int num = 0;
		int num2 = 0;
		int num3 = 0;
		for (int i = 0; i < allPlayers.Count; i++)
		{
			int num4 = allPlayers[i];
			if (!Player.IsInjured(num4))
			{
				list.Add(num4);
				if (Player.GetPosition(num4) == 1)
				{
					num3++;
				}
			}
			else
			{
				if (Player.GetPosition(num4) == 1)
				{
					num++;
				}
				num2++;
			}
		}
		Game.instance.g_trainingManager.LastReportSummary.InjuredPlayers = num2;
		_ = 5;
		if (Game.instance.CurrentDate.DayOfWeek - 1 >= DayOfWeek.Sunday)
		{
			_ = Game.instance.CurrentDate.DayOfWeek;
		}
		_ = Game.instance.CurrentManagedClub;
		int[] increaseAmounts = TrainingManager.GetIncreaseAmounts();
		bool flag = increaseAmounts[5] > 0;
		float perc = 1f;
		if (flag)
		{
			perc = 1f + (float)increaseAmounts[5] * Facilities.GetDrillIncrease(6) / 100f;
		}
		GetCapacityDecrease();
		int num5 = (Game.instance.g_trainingManager.IsTrainingAvailable() ? Game.instance.g_trainingManager.GetFitnessIncrease() : (-1));
		Game.instance.g_trainingManager.LastReportSummary.FitnessIncrease = num5;
		if (num5 != -1)
		{
			PlayerList.ProcessFitness(num5);
		}
		TrainingManager.CalcTotalGameTime();
		TrainingManager.CalcCapacityPen();
		for (int j = 0; j < list.Count; j++)
		{
			if (flag)
			{
				ApplyTeamChemistryIncrease(GetPlayerTrainingChemistryIncrease(perc, list[j]), list[j]);
			}
			ApplyTrainingIncrease(GetTrainingIncrease(list[j], increaseAmounts), list[j]);
			ApplyPhyscialAttributesIncrease(list[j], increaseAmounts[3]);
		}
	}

	private void ApplyPhyscialAttributesIncrease(int playerID, int sessionAmount)
	{
		int[] physicalAttributesDisplayOrder = AttributesFunctions.GetPhysicalAttributesDisplayOrder(Player.GetPosition(playerID) == 1);
		for (int i = 0; i < physicalAttributesDisplayOrder.Length; i++)
		{
			IncreasePlayerAttribute(playerID, physicalAttributesDisplayOrder[i], sessionAmount);
		}
	}

	public static List<int> GetTrainedAttributes()
	{
		List<int> list = new List<int>();
		List<int> list2 = new List<int>();
		for (int i = 0; i < 2; i++)
		{
			for (int j = 0; j < 7; j++)
			{
				if (!list2.Contains(Game.instance.g_trainingDrillManager.DrillsByTime(i, j)))
				{
					list2.Add(Game.instance.g_trainingDrillManager.DrillsByTime(i, j));
				}
			}
		}
		for (int k = 0; k < list2.Count; k++)
		{
			list = TrainingDrills.drillAttributes[list2[k]].Union(list).ToList();
		}
		return list;
	}

	public bool CanPlayerAttributeIncrease(int playerId, int attrId)
	{
		TrainingPlayer trainingPlayer = Game.instance.g_trainingDrillManager.TrainingPlayer[playerId];
		bool flag = attributeIncrease.ContainsKey(attrId) && attributeIncrease[attrId].Contains(playerId);
		if (trainingPlayer.attributeAlterations[attrId] < (float)GetAttributeAlterationCap() && allPlayerAttributes[playerId][attrId] < 100f)
		{
			return !flag;
		}
		return false;
	}

	public int GetAttributeAlterationCap()
	{
		return ATTRIBUTE_ALTERATION_CAP;
	}

	public void IncreasePlayerAttribute(int playerId, int attributeId, int sessionAmount)
	{
		TrainingPlayer trainingPlayer = Game.instance.g_trainingDrillManager.TrainingPlayer[playerId];
		float num = TrainingConfig.PhysicalAttrIncrease * (float)sessionAmount;
		if (attributeIncrease.ContainsKey(attributeId))
		{
			attributeIncrease[attributeId].Add(playerId);
		}
		else
		{
			attributeIncrease[attributeId] = new List<int>();
			attributeIncrease[attributeId].Add(playerId);
		}
		int attributeAlterationCap = GetAttributeAlterationCap();
		if (trainingPlayer.attributeAlterations[attributeId] + num > (float)attributeAlterationCap)
		{
			num = (float)attributeAlterationCap - trainingPlayer.attributeAlterations[attributeId];
		}
		trainingPlayer.attributeAlterations[attributeId] += num;
	}

	public Dictionary<int, List<int>> GetPlayerAttributeIncreasesFromTraining()
	{
		Dictionary<int, List<int>> dictionary = new Dictionary<int, List<int>>();
		foreach (KeyValuePair<int, PlayerTrainingAttributeReportList> lastAttributeChange in Game.instance.g_trainingDrillManager.LastTrainingReport.lastAttributeChanges)
		{
			foreach (PlayerTrainingAttributeReport item in lastAttributeChange.Value.list)
			{
				if (dictionary.ContainsKey(item.playerId))
				{
					dictionary[item.playerId].Add(item.attributeId);
					continue;
				}
				dictionary[item.playerId] = new List<int> { item.attributeId };
			}
		}
		return dictionary;
	}

	public void FillDrillAndBoostLevelData()
	{
		drillLevel = new Dictionary<int, float>();
		branchBoostLevels = new Dictionary<int, float>();
		int[] array = selectedDrills.Where((int i) => i != -1).ToArray();
		foreach (int num2 in array)
		{
			drillLevel[num2] = (TrainingDrills.IsBasicDrill(num2) ? 1 : Facilities.GetFacilityLevel(2));
		}
		branchBoostLevels[0] = Facilities.GetFacilityLevel(2);
		branchBoostLevels[1] = Facilities.GetFacilityLevel(2);
		branchBoostLevels[2] = Facilities.GetFacilityLevel(2);
	}

	public static int GetOverallDrillIntensity()
	{
		int num = 0;
		for (int i = 0; i < 2; i++)
		{
			for (int j = 0; j < 7; j++)
			{
				num += Game.instance.g_trainingDrillManager.DrillIntensity(i, j);
			}
		}
		return num / 14;
	}

	public float GetAttributeIncreaseValue(int drillId, int playerId, int intensity)
	{
		return attrIncreases[intensity];
	}

	public void SetAttributeIncreaseValue(int drillId, int playerId, int intensity)
	{
		int[] drillAttributes = GetDrillAttributes(drillId);
		bool flag = TrainingDrills.IsGoalKeeperDrill(drillId);
		float num = Game.instance.g_trainingDrillManager.TrainingPlayer[playerId].attributeAlterations.Sum();
		int[] array = drillAttributes;
		foreach (int num2 in array)
		{
			if (!(num < (float)ATTRIBUTE_TOTAL_CAP))
			{
				continue;
			}
			GetAttributeTrainingGroupType(num2);
			float num3 = Player.GetAttributeIncrease(playerId, num2, GetAttributeIncreaseValue(drillId, playerId, intensity));
			if (Player.InDeclineAccurate(playerId))
			{
				num3 *= 0.2f;
			}
			if (!(Game.instance.g_trainingDrillManager.TrainingPlayer[playerId].attributeAlterations[num2] < (float)ATTRIBUTE_ALTERATION_CAP))
			{
				continue;
			}
			if (Attributes.outfieldAttributeTypes[num2] == 0)
			{
				if ((!flag && !PlayerData.IsGoalKeeper(playerId)) || (flag && PlayerData.IsGoalKeeper(playerId)))
				{
					Game.instance.g_trainingDrillManager.TrainingPlayer[playerId].attributeAlterations[num2] += num3;
					num += num3;
					if (Game.instance.g_trainingDrillManager.TrainingPlayer[playerId].attributeAlterations[num2] > (float)ATTRIBUTE_ALTERATION_CAP)
					{
						Game.instance.g_trainingDrillManager.TrainingPlayer[playerId].attributeAlterations[num2] = ATTRIBUTE_ALTERATION_CAP;
					}
				}
			}
			else
			{
				Game.instance.g_trainingDrillManager.TrainingPlayer[playerId].attributeAlterations[num2] += num3;
				num += num3;
				if (Game.instance.g_trainingDrillManager.TrainingPlayer[playerId].attributeAlterations[num2] > (float)ATTRIBUTE_ALTERATION_CAP)
				{
					Game.instance.g_trainingDrillManager.TrainingPlayer[playerId].attributeAlterations[num2] = ATTRIBUTE_ALTERATION_CAP;
				}
			}
		}
	}

	public static List<float[]> GetEndOfMonthIncrease()
	{
		Tuple<Dictionary<int, float>, Dictionary<int, float>> setDrillsByAmount = GetSetDrillsByAmount();
		List<float[]> list = new List<float[]>
		{
			new float[30],
			new float[30]
		};
		foreach (int key in setDrillsByAmount.Item1.Keys)
		{
			list[0][key] = setDrillsByAmount.Item1[key];
		}
		foreach (int key2 in setDrillsByAmount.Item2.Keys)
		{
			list[1][key2] = setDrillsByAmount.Item2[key2];
		}
		return list;
	}

	public static float[] GetEndOfMonthIncreaseForPlayer(int playerID)
	{
		float[] array = new float[30];
		Dictionary<int, float> setDrillsByAmountForPlayer = GetSetDrillsByAmountForPlayer(playerID);
		foreach (int key in setDrillsByAmountForPlayer.Keys)
		{
			array[key] = setDrillsByAmountForPlayer[key];
		}
		return array;
	}

	public static void AddPlayerToRest(int playerID, int days)
	{
		if (!Game.instance.g_trainingDrillManager.RestingPlayers.ContainsKey(playerID))
		{
			Game.instance.g_trainingDrillManager.RestingPlayers.Add(playerID, Game.instance.CurrentDate.AddDays(days));
		}
		else
		{
			Game.instance.g_trainingDrillManager.RestingPlayers[playerID].AddDays(days);
		}
	}

	public static void RemovePlayerFromRest(int playerID)
	{
		if (Game.instance.g_trainingDrillManager.RestingPlayers.ContainsKey(playerID))
		{
			Game.instance.g_trainingDrillManager.RestingPlayers.Remove(playerID);
		}
	}

	public static Dictionary<int, float> GetSetDrillsByAmountForPlayer(int playerID)
	{
		Dictionary<int, float> dictionary = new Dictionary<int, float>();
		DateTime currentDate = Game.instance.CurrentDate;
		DateTime dateTime = new DateTime(currentDate.Year, currentDate.Month, DateTime.DaysInMonth(currentDate.Year, currentDate.Month));
		List<ClubScheduleInfo> schedule = Club.GetSchedule(Game.GetCurrentUserClubID());
		List<int> list = new List<int>();
		for (int i = 0; i < Enumerable.Count(schedule); i++)
		{
			if (schedule[i].Date >= currentDate && schedule[i].Date <= dateTime)
			{
				list.Add(schedule[i].Date.Day);
			}
			else if (schedule[i].Date > dateTime)
			{
				break;
			}
		}
		DateTime dateTime2 = default;
		if (Player.IsInjured(playerID))
		{
			dateTime2 = Player.getInjuryReturnDate(playerID);
		}
		if (Player.IsResting(playerID))
		{
			dateTime2 = ((Player.GetRestReturnDate(playerID) > dateTime2) ? Player.GetRestReturnDate(playerID) : dateTime2);
		}
		int num = DateTime.DaysInMonth(currentDate.Year, currentDate.Month) - currentDate.Day;
		TrainingDrillManager g_trainingDrillManager = Game.instance.g_trainingDrillManager;
		for (int j = 1; j <= num; j++)
		{
			DateTime dateTime3 = currentDate.AddDays(j);
			if (!(dateTime3 > dateTime2) || list.Contains(dateTime3.Day))
			{
				continue;
			}
			int day = (int)((dateTime3.DayOfWeek == DayOfWeek.Sunday) ? DayOfWeek.Saturday : (dateTime3.DayOfWeek - 1));
			for (int k = 0; k < 2; k++)
			{
				int[] array = TrainingDrills.drillAttributes[g_trainingDrillManager.DrillsByTime(k, day)];
				bool flag = TrainingDrills.IsGoalKeeperDrill(g_trainingDrillManager.DrillsByTime(k, day));
				for (int l = 0; l < array.Count(); l++)
				{
					if (Attributes.outfieldAttributeTypes[array[l]] == 0)
					{
						if ((!flag && !PlayerData.IsGoalKeeper(playerID)) || (flag && PlayerData.IsGoalKeeper(playerID)))
						{
							if (!dictionary.ContainsKey(array[l]))
							{
								dictionary.Add(array[l], attrIncreases[g_trainingDrillManager.DrillIntensity(k, day)]);
							}
							else
							{
								dictionary[array[l]] += attrIncreases[g_trainingDrillManager.DrillIntensity(k, day)];
							}
						}
					}
					else if (!dictionary.ContainsKey(array[l]))
					{
						dictionary.Add(array[l], attrIncreases[Game.instance.g_trainingDrillManager.DrillIntensity(k, day)]);
					}
					else
					{
						dictionary[array[l]] += attrIncreases[Game.instance.g_trainingDrillManager.DrillIntensity(k, day)];
					}
				}
			}
		}
		return dictionary;
	}

	public static Tuple<Dictionary<int, float>, Dictionary<int, float>> GetSetDrillsByAmount()
	{
		Dictionary<int, float> dictionary = new Dictionary<int, float>();
		Dictionary<int, float> dictionary2 = new Dictionary<int, float>();
		DateTime currentDate = Game.instance.CurrentDate;
		DateTime dateTime = new DateTime(currentDate.Year, currentDate.Month, DateTime.DaysInMonth(currentDate.Year, currentDate.Month));
		List<ClubScheduleInfo> schedule = Club.GetSchedule(Game.GetCurrentUserClubID());
		List<int> list = new List<int>();
		for (int i = 0; i < Enumerable.Count(schedule); i++)
		{
			if (schedule[i].Date >= currentDate && schedule[i].Date <= dateTime)
			{
				list.Add(schedule[i].Date.Day);
			}
			else if (schedule[i].Date > dateTime)
			{
				break;
			}
		}
		int num = DateTime.DaysInMonth(currentDate.Year, currentDate.Month) - currentDate.Day;
		TrainingDrillManager g_trainingDrillManager = Game.instance.g_trainingDrillManager;
		for (int j = 1; j <= num; j++)
		{
			DateTime dateTime2 = currentDate.AddDays(j);
			if (list.Contains(dateTime2.Day))
			{
				continue;
			}
			int day = (int)((dateTime2.DayOfWeek == DayOfWeek.Sunday) ? DayOfWeek.Saturday : (dateTime2.DayOfWeek - 1));
			for (int k = 0; k < 2; k++)
			{
				int[] array = TrainingDrills.drillAttributes[g_trainingDrillManager.DrillsByTime(k, day)];
				bool flag = TrainingDrills.IsGoalKeeperDrill(g_trainingDrillManager.DrillsByTime(k, day));
				for (int l = 0; l < array.Count(); l++)
				{
					if (Attributes.outfieldAttributeTypes[array[l]] == 0)
					{
						if (!flag)
						{
							if (!dictionary.ContainsKey(array[l]))
							{
								dictionary.Add(array[l], attrIncreases[g_trainingDrillManager.DrillIntensity(k, day)]);
							}
							else
							{
								dictionary[array[l]] += attrIncreases[g_trainingDrillManager.DrillIntensity(k, day)];
							}
						}
						else if (!dictionary2.ContainsKey(array[l]))
						{
							dictionary2.Add(array[l], attrIncreases[g_trainingDrillManager.DrillIntensity(k, day)]);
						}
						else
						{
							dictionary2[array[l]] += attrIncreases[g_trainingDrillManager.DrillIntensity(k, day)];
						}
					}
					else
					{
						if (!dictionary.ContainsKey(array[l]))
						{
							dictionary.Add(array[l], attrIncreases[g_trainingDrillManager.DrillIntensity(k, day)]);
						}
						else
						{
							dictionary[array[l]] += attrIncreases[g_trainingDrillManager.DrillIntensity(k, day)];
						}
						if (!dictionary2.ContainsKey(array[l]))
						{
							dictionary2.Add(array[l], attrIncreases[g_trainingDrillManager.DrillIntensity(k, day)]);
						}
						else
						{
							dictionary2[array[l]] += attrIncreases[g_trainingDrillManager.DrillIntensity(k, day)];
						}
					}
				}
			}
		}
		if (!list.Contains(currentDate.Day))
		{
			int day = (int)((currentDate.DayOfWeek == DayOfWeek.Sunday) ? DayOfWeek.Saturday : (currentDate.DayOfWeek - 1));
			AddRemainingSessions(dictionary, day);
			AddRemainingSessions(dictionary2, day);
		}
		return Tuple.Create(dictionary, dictionary2);
	}

	public static Dictionary<int, float> AddRemainingSessions(Dictionary<int, float> drillAmounts, int dayofweek)
	{
		TrainingDrillManager g_trainingDrillManager = Game.instance.g_trainingDrillManager;
		if (Game.instance.CurrentDate.Hour < 11 && Game.instance.CurrentDate.Hour > 8)
		{
			if (drillAmounts.ContainsKey(g_trainingDrillManager.DrillsByTime(1, dayofweek)))
			{
				drillAmounts[g_trainingDrillManager.DrillsByTime(1, dayofweek)] += attrIncreases[g_trainingDrillManager.DrillIntensity(1, dayofweek)];
			}
			else
			{
				drillAmounts.Add(g_trainingDrillManager.DrillsByTime(1, dayofweek), attrIncreases[g_trainingDrillManager.DrillIntensity(1, dayofweek)]);
			}
		}
		else if (Game.instance.CurrentDate.Hour < 8)
		{
			if (drillAmounts.ContainsKey(g_trainingDrillManager.DrillsByTime(0, dayofweek)))
			{
				drillAmounts[g_trainingDrillManager.DrillsByTime(0, dayofweek)] += attrIncreases[g_trainingDrillManager.DrillIntensity(0, dayofweek)];
			}
			else
			{
				drillAmounts.Add(g_trainingDrillManager.DrillsByTime(0, dayofweek), attrIncreases[g_trainingDrillManager.DrillIntensity(0, dayofweek)]);
			}
			if (drillAmounts.ContainsKey(g_trainingDrillManager.DrillsByTime(1, dayofweek)))
			{
				drillAmounts[g_trainingDrillManager.DrillsByTime(1, dayofweek)] += attrIncreases[g_trainingDrillManager.DrillIntensity(1, dayofweek)];
			}
			else
			{
				drillAmounts.Add(g_trainingDrillManager.DrillsByTime(1, dayofweek), attrIncreases[g_trainingDrillManager.DrillIntensity(1, dayofweek)]);
			}
		}
		return drillAmounts;
	}

	public int[] GetDrillAttributes(int drillId)
	{
		return TrainingDrills.drillAttributes[drillId];
	}

	public static int GetAttributeTrainingGroupType(int attrId)
	{
		int result = -1;
		foreach (KeyValuePair<int, int[]> aTTRIBUTE_TRAINING_GROUP in ATTRIBUTE_TRAINING_GROUPS)
		{
			int key = aTTRIBUTE_TRAINING_GROUP.Key;
			if (Array.IndexOf(aTTRIBUTE_TRAINING_GROUP.Value, attrId) > -1)
			{
				result = key;
				break;
			}
		}
		return result;
	}

	private static void ReduceChangesForDepartedPlayers()
	{
	}

	private static void ChanceOfRandomInjury()
	{
	}

	private static string _debugAttributeGroupName(int attrGroupType)
	{
		return attrGroupType switch
		{
			2 => "ATTACKING", 
			1 => "DEFENDING", 
			3 => "GOALKEEPING", 
			0 => "PHYSICAL", 
			4 => "SETPIECES", 
			_ => "UNKNOWN", 
		};
	}

	public static void _DebugAllPlayerTrainingAttributes()
	{
		PlayerSquad players = Club.GetPlayers(Game.instance.CurrentManagedClubId);
		for (int i = 0; i < players.Count; i++)
		{
			int num = players[i];
			if (Game.instance.g_trainingDrillManager.TrainingPlayer.ContainsKey(num))
			{
				_ = Game.instance.g_trainingDrillManager.TrainingPlayer[num];
				if (Player.GetPosition(num) != 1)
				{
					_ = Attributes.allAttributesOutfield;
				}
				else
				{
					_ = Attributes.allAttributesKeeper;
				}
			}
		}
	}

	private static void _DebugPlayerRatingCache(int playerId)
	{
		string text = Player.GetName(playerId) + " (" + playerId + ")";
		List<int> source = new List<int>();
		text = text + " Rating = " + Player.GetRating(playerId);
		text = text + " Potential = " + Game.instance.g_PlayerData[playerId].Potential;
		for (int i = 0; i < 25; i++)
		{
		}
		text = text + " " + string.Join(",", source.Select((int num) => num.ToString()).ToArray());
	}

	private static void _DebugAllPlayers()
	{
		foreach (KeyValuePair<int, PlayerData> g_PlayerDatum in Game.instance.g_PlayerData)
		{
			int key = g_PlayerDatum.Key;
			_ = g_PlayerDatum.Value.Potential;
			bool flag = Player.InDeclineAccurate(key);
			bool flag2 = false;
			if (!flag)
			{
				for (int i = 0; i < 25; i++)
				{
				}
			}
		}
	}

	public static void _InitTrainingDebug()
	{
		GetInstance().InitTrainingDrillEffects();
	}

	public static void _RunTrainingDebug()
	{
		GetInstance().ProcessTraining(debugTraining: true);
	}
}
