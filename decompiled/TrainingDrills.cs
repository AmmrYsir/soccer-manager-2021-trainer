using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TrainingDrills
{
	public const int DEFENSIVE_DRILL = 0;

	public const int ATTACKING_DRILL = 1;

	public const int PHYSICAL_DRILL = 2;

	public const int BASIC_ATTACKING = 0;

	public const int SET_PIECES = 1;

	public const int TECHNICAL_TRAINING = 2;

	public const int SHOOTING = 3;

	public const int PASSING = 4;

	public const int DRIBBLING = 5;

	public const int BASIC_DEFENDING = 6;

	public const int BASIC_GOALKEEPING = 7;

	public const int POSITIONING = 8;

	public const int TACKLING = 9;

	public const int ART_OF_DEFENDING = 10;

	public const int DEFENDING_CHARACTERISTICS = 11;

	public const int ADVANCED_GOALKEEPING = 12;

	public const int BASIC_PHYSICAL = 13;

	public const int LAPS_OF_THE_PITCH = 14;

	public const int BLEEP_TEST = 15;

	public const int WEIGHTS = 16;

	public const int JUMPING = 17;

	public const int SHUTTLE_RUNS = 18;

	public const int REST = 19;

	public static int[] basicDrills = new int[4] { 0, 6, 7, 13 };

	public static int[] goalKeeperDrills = new int[2] { 7, 12 };

	public static Dictionary<int, int> unlockableDrills = new Dictionary<int, int>();

	public static Dictionary<int, int[]> drillAttributes = new Dictionary<int, int[]>
	{
		{
			1,
			new int[3] { 24, 23, 22 }
		},
		{
			2,
			new int[2] { 19, 13 }
		},
		{
			3,
			new int[3] { 15, 17, 1 }
		},
		{
			4,
			new int[3] { 18, 21, 20 }
		},
		{
			5,
			new int[4] { 14, 16, 7, 4 }
		},
		{
			8,
			new int[1] { 12 }
		},
		{
			9,
			new int[2] { 11, 0 }
		},
		{
			10,
			new int[3] { 10, 1, 2 }
		},
		{
			11,
			new int[3] { 12, 6, 5 }
		},
		{
			12,
			new int[4] { 10, 13, 11, 14 }
		},
		{
			14,
			new int[1] { 25 }
		},
		{
			15,
			new int[1] { 27 }
		},
		{
			16,
			new int[1] { 5 }
		},
		{
			17,
			new int[1] { 6 }
		},
		{
			18,
			new int[1] { 4 }
		},
		{
			0,
			new int[11]
			{
				14, 15, 16, 17, 18, 19, 20, 21, 13, 1,
				7
			}
		},
		{
			6,
			new int[5] { 10, 11, 12, 0, 2 }
		},
		{
			7,
			new int[12]
			{
				10, 11, 12, 13, 14, 15, 16, 1, 2, 6,
				4, 5
			}
		},
		{
			13,
			new int[5] { 6, 4, 25, 5, 27 }
		},
		{
			19,
			new int[0]
		}
	};

	public static Dictionary<int, float[]> drillFitnessMod = new Dictionary<int, float[]>
	{
		{
			0,
			new float[3]
			{
				(float)Training.fitnessMods[0],
				(float)Training.fitnessMods[1],
				(float)Training.fitnessMods[2]
			}
		},
		{
			1,
			new float[3]
			{
				(float)Training.fitnessMods[0],
				(float)Training.fitnessMods[1],
				(float)Training.fitnessMods[2]
			}
		},
		{
			2,
			new float[3]
			{
				(float)Training.fitnessMods[0],
				(float)Training.fitnessMods[1],
				(float)Training.fitnessMods[2]
			}
		},
		{
			3,
			new float[3]
			{
				(float)Training.fitnessMods[0],
				(float)Training.fitnessMods[1],
				(float)Training.fitnessMods[2]
			}
		},
		{
			4,
			new float[3]
			{
				(float)Training.fitnessMods[0],
				(float)Training.fitnessMods[1],
				(float)Training.fitnessMods[2]
			}
		},
		{
			5,
			new float[3]
			{
				(float)Training.fitnessMods[0],
				(float)Training.fitnessMods[1],
				(float)Training.fitnessMods[2]
			}
		},
		{
			6,
			new float[3]
			{
				(float)Training.fitnessMods[0],
				(float)Training.fitnessMods[1],
				(float)Training.fitnessMods[2]
			}
		},
		{
			7,
			new float[3]
			{
				(float)Training.fitnessMods[0],
				(float)Training.fitnessMods[1],
				(float)Training.fitnessMods[2]
			}
		},
		{
			8,
			new float[3]
			{
				(float)Training.fitnessMods[0],
				(float)Training.fitnessMods[1],
				(float)Training.fitnessMods[2]
			}
		},
		{
			9,
			new float[3]
			{
				(float)Training.fitnessMods[0],
				(float)Training.fitnessMods[1],
				(float)Training.fitnessMods[2]
			}
		},
		{
			10,
			new float[3]
			{
				(float)Training.fitnessMods[0],
				(float)Training.fitnessMods[1],
				(float)Training.fitnessMods[2]
			}
		},
		{
			11,
			new float[3]
			{
				(float)Training.fitnessMods[0],
				(float)Training.fitnessMods[1],
				(float)Training.fitnessMods[2]
			}
		},
		{
			12,
			new float[3]
			{
				(float)Training.fitnessMods[0],
				(float)Training.fitnessMods[1],
				(float)Training.fitnessMods[2]
			}
		},
		{
			13,
			new float[3]
			{
				(float)Training.fitnessMods[0],
				(float)Training.fitnessMods[1],
				(float)Training.fitnessMods[2]
			}
		},
		{
			14,
			new float[3]
			{
				(float)Training.fitnessMods[0],
				(float)Training.fitnessMods[1],
				(float)Training.fitnessMods[2]
			}
		},
		{
			15,
			new float[3]
			{
				(float)Training.fitnessMods[0],
				(float)Training.fitnessMods[1],
				(float)Training.fitnessMods[2]
			}
		},
		{
			16,
			new float[3]
			{
				(float)Training.fitnessMods[0],
				(float)Training.fitnessMods[1],
				(float)Training.fitnessMods[2]
			}
		},
		{
			17,
			new float[3]
			{
				(float)Training.fitnessMods[0],
				(float)Training.fitnessMods[1],
				(float)Training.fitnessMods[2]
			}
		},
		{
			18,
			new float[3]
			{
				(float)Training.fitnessMods[0],
				(float)Training.fitnessMods[1],
				(float)Training.fitnessMods[2]
			}
		},
		{
			19,
			new float[1] { TrainingConfig.RestIncreaseAmount }
		}
	};

	public static Dictionary<int, float> drillInjuryChance = new Dictionary<int, float>
	{
		{ 0, 0.01f },
		{ 1, 0.01f },
		{ 2, 0.01f },
		{ 3, 0.01f },
		{ 4, 0.01f },
		{ 5, 0.01f },
		{ 6, 0.01f },
		{ 7, 0.01f },
		{ 8, 0.01f },
		{ 9, 0.01f },
		{ 10, 0.01f },
		{ 11, 0.01f },
		{ 12, 0.01f },
		{ 13, 0.01f },
		{ 14, 0.01f },
		{ 15, 0.01f },
		{ 16, 0.01f },
		{ 17, 0.01f },
		{ 18, 0.01f },
		{ 19, 0f }
	};

	public static List<int> attackingDrills = new List<int> { 0, 1, 2, 3, 4, 5 };

	public static List<int> defendingDrills = new List<int> { 6, 7, 8, 9, 10, 11, 12 };

	public static List<int> physicalDrills = new List<int> { 13, 14, 15, 16, 17, 18 };

	public static Dictionary<int, int[]> getDrillAttributes()
	{
		return drillAttributes;
	}

	public static float getInjuryChancePerDay(int day)
	{
		return getDrillInjuryChance(Game.instance.g_trainingDrillManager.DrillsByTime(0, day), Game.instance.g_trainingDrillManager.DrillIntensity(0, day)) + getDrillInjuryChance(Game.instance.g_trainingDrillManager.DrillsByTime(1, day), Game.instance.g_trainingDrillManager.DrillIntensity(1, day));
	}

	public static float getInjuryChance(int drillid, int intensity)
	{
		if (drillInjuryChance.Keys.Contains(drillid))
		{
			return (float)Training.injuryChances[intensity];
		}
		return 0f;
	}

	public static float getFitnessModPerDay(int day)
	{
		return getDrillFitnessMod(Game.instance.g_trainingDrillManager.DrillsByTime(0, day), Game.instance.g_trainingDrillManager.DrillIntensity(0, day)) + getDrillFitnessMod(Game.instance.g_trainingDrillManager.DrillsByTime(1, day), Game.instance.g_trainingDrillManager.DrillIntensity(1, day));
	}

	public static float getDrillFitnessMod(int drillid, int intensity = 0)
	{
		if (drillid == 19)
		{
			intensity = 0;
		}
		if (drillFitnessMod.Keys.Contains(drillid))
		{
			return drillFitnessMod[drillid][intensity];
		}
		return 0f;
	}

	public static float getDrillInjuryChance(int drillid, int intensity = 0)
	{
		if (drillid == 19)
		{
			return 0f;
		}
		if (drillInjuryChance.Keys.Contains(drillid))
		{
			return (float)Training.injuryChances[intensity];
		}
		return 0f;
	}

	public static string getDrillName(int drillid)
	{
		return drillid switch
		{
			0 => Translation.GetText("basic-attacking"), 
			1 => Translation.GetText("set-pieces"), 
			2 => Translation.GetText("technical-training"), 
			3 => Translation.GetText("shooting"), 
			4 => Translation.GetText("passing"), 
			5 => Translation.GetText("dribbling"), 
			6 => Translation.GetText("basic-defending"), 
			7 => Translation.GetText("basic-goalkeeping"), 
			8 => Translation.GetText("positioning"), 
			9 => Translation.GetText("tackling"), 
			10 => Translation.GetText("art-of-defending"), 
			11 => Translation.GetText("defending-characteristics"), 
			12 => Translation.GetText("advanced-goalkeeping"), 
			13 => Translation.GetText("basic-physical"), 
			14 => Translation.GetText("laps-of-the-pitch"), 
			15 => Translation.GetText("bleep-test"), 
			16 => Translation.GetText("weights"), 
			17 => Translation.GetText("jumping"), 
			18 => Translation.GetText("shuttle-runs"), 
			19 => Translation.GetText("rest"), 
			_ => "", 
		};
	}

	public static string getDrillNameUntranslated(int drillid)
	{
		return drillid switch
		{
			0 => "basic-attacking", 
			1 => "set-pieces", 
			2 => "technical-training", 
			3 => "shooting", 
			4 => "passing", 
			5 => "dribbling", 
			6 => "basic-defending", 
			7 => "basic-goalkeeping", 
			8 => "positioning", 
			9 => "tackling", 
			10 => "art-of-defending", 
			11 => "defending-characteristics", 
			12 => "advanced-goalkeeping", 
			13 => "basic-physical", 
			14 => "laps-of-the-pitch", 
			15 => "bleep-test", 
			16 => "weights", 
			17 => "jumping", 
			18 => "shuttle-runs", 
			19 => "rest", 
			_ => "", 
		};
	}

	public static Sprite GetDrillSprite(int drillid)
	{
		return drillid switch
		{
			0 => Resources.Load<Sprite>("Drillimages/basicattackingdrill"), 
			1 => Resources.Load<Sprite>("Drillimages/setpiecedrill"), 
			2 => Resources.Load<Sprite>("Drillimages/technicaltrainingdrill"), 
			3 => Resources.Load<Sprite>("Drillimages/shootingdrill"), 
			4 => Resources.Load<Sprite>("Drillimages/passingdrill"), 
			5 => Resources.Load<Sprite>("Drillimages/dribblingdrill"), 
			6 => Resources.Load<Sprite>("Drillimages/basicdefending"), 
			7 => Resources.Load<Sprite>("Drillimages/basicgoalkeeping"), 
			8 => Resources.Load<Sprite>("Drillimages/positioning"), 
			9 => Resources.Load<Sprite>("Drillimages/tackling"), 
			10 => Resources.Load<Sprite>("Drillimages/artofdefending"), 
			11 => Resources.Load<Sprite>("Drillimages/defendingcharacteristics"), 
			12 => Resources.Load<Sprite>("Drillimages/advancedgoalkeeping"), 
			13 => Resources.Load<Sprite>("Drillimages/basicphysical"), 
			14 => Resources.Load<Sprite>("Drillimages/lapsofpitch"), 
			15 => Resources.Load<Sprite>("Drillimages/bleeptest"), 
			16 => Resources.Load<Sprite>("Drillimages/weights"), 
			17 => Resources.Load<Sprite>("Drillimages/jumping"), 
			18 => Resources.Load<Sprite>("Drillimages/shuttleruns"), 
			_ => null, 
		};
	}

	public static int GetDrillType(int drillid)
	{
		int result = -1;
		List<int>[] array = new List<int>[3] { defendingDrills, attackingDrills, physicalDrills };
		List<int> list = new List<int> { 0, 1, 2 };
		for (int i = 0; i < array.Length; i++)
		{
			if (array[i].IndexOf(drillid) != -1)
			{
				return list[i];
			}
		}
		return result;
	}

	public static int Drill2FacilityID(int drillId)
	{
		if (!unlockableDrills.ContainsKey(drillId))
		{
			return -1;
		}
		return unlockableDrills[drillId];
	}

	public static bool IsBasicDrill(int drillid)
	{
		return Array.IndexOf(basicDrills, drillid) > -1;
	}

	public static bool IsGoalKeeperDrill(int drillid)
	{
		return Array.IndexOf(goalKeeperDrills, drillid) > -1;
	}

	public static int[] getBasicDrills()
	{
		return basicDrills;
	}

	public static Dictionary<int, int> getUnlockableDrills()
	{
		return unlockableDrills;
	}

	public static bool isDrillUnlocked(int drillid)
	{
		if (unlockableDrills.ContainsKey(drillid))
		{
			_ = unlockableDrills[drillid];
			if (Facilities.GetFacilityLevel(2) > 0)
			{
				return true;
			}
			return false;
		}
		return true;
	}

	public static int getSlotDrill(int slotid)
	{
		return Game.instance.g_trainingDrillManager.SelectedDrills[slotid];
	}

	public static void selectDrill(int slotid, int drillid)
	{
		Game.instance.g_trainingDrillManager.SelectedDrills[slotid] = drillid;
	}

	public static void deselectDrill(int slotid)
	{
		Game.instance.g_trainingDrillManager.SelectedDrills[slotid] = -1;
	}

	public static int getNumAttackDrillsSet()
	{
		int num = 0;
		for (int i = 0; i < Game.instance.g_trainingDrillManager.SelectedDrills.Count; i++)
		{
			int item = Game.instance.g_trainingDrillManager.SelectedDrills[i + 1];
			if (attackingDrills.Contains(item))
			{
				num++;
			}
		}
		return num;
	}

	public static int getNumDefendingDrillsSet()
	{
		int num = 0;
		for (int i = 0; i < Game.instance.g_trainingDrillManager.SelectedDrills.Count; i++)
		{
			int item = Game.instance.g_trainingDrillManager.SelectedDrills[i + 1];
			if (defendingDrills.Contains(item))
			{
				num++;
			}
		}
		return num;
	}

	public static int getNumPhysicalDrillsSet()
	{
		int num = 0;
		for (int i = 0; i < Game.instance.g_trainingDrillManager.SelectedDrills.Count; i++)
		{
			int item = Game.instance.g_trainingDrillManager.SelectedDrills[i + 1];
			if (physicalDrills.Contains(item))
			{
				num++;
			}
		}
		return num;
	}
}
