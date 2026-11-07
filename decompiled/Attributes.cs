using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Attributes
{
	public const int ATTR_MAX_PLAYER_RATING = 40;

	public const int CONST_ATTRIBUTE_MAX_VALUE = 29;

	public const int FOG_OF_WAR_EFFECT_MAX_RATING = 91;

	public static int[] allAttributesKeeper = new int[14]
	{
		10, 12, 13, 14, 15, 16, 1, 4, 5, 6,
		0, 2, 8, 11
	};

	public static int[] allAttributesOutfield = new int[25]
	{
		10, 11, 12, 14, 15, 16, 17, 18, 1, 19,
		20, 21, 4, 5, 6, 25, 27, 0, 2, 7,
		8, 13, 24, 23, 22
	};

	public static Dictionary<int, int> keeperAttributeTypes = new Dictionary<int, int>
	{
		{ 10, 0 },
		{ 11, 0 },
		{ 12, 0 },
		{ 13, 0 },
		{ 14, 0 },
		{ 15, 0 },
		{ 16, 0 },
		{ 6, 2 },
		{ 4, 2 },
		{ 5, 2 },
		{ 0, 1 },
		{ 1, 1 },
		{ 2, 1 },
		{ 8, 1 }
	};

	public static Dictionary<int, int> outfieldAttributeTypes = new Dictionary<int, int>
	{
		{ 10, 0 },
		{ 11, 0 },
		{ 12, 0 },
		{ 14, 0 },
		{ 19, 0 },
		{ 20, 0 },
		{ 21, 0 },
		{ 24, 0 },
		{ 23, 0 },
		{ 22, 0 },
		{ 18, 0 },
		{ 17, 0 },
		{ 13, 0 },
		{ 15, 0 },
		{ 16, 0 },
		{ 6, 2 },
		{ 4, 2 },
		{ 25, 2 },
		{ 5, 2 },
		{ 27, 2 },
		{ 0, 1 },
		{ 1, 1 },
		{ 2, 1 },
		{ 7, 1 },
		{ 8, 1 }
	};

	public static List<int> defenderOnlyAttributes = new List<int> { 10, 11, 12, 2 };

	public static List<int> attackerOnlyAttributes = new List<int> { 14, 15, 16, 17, 18, 24, 23, 22, 13, 7 };

	private static Dictionary<int, string> attrConverterBase = new Dictionary<int, string>
	{
		{ 0, "attr-aggression" },
		{ 1, "attr-composure" },
		{ 2, "attr-concentration" },
		{ 4, "attr-pace" },
		{ 5, "attr-strength" },
		{ 6, "attr-aerialAbility" },
		{ 7, "attr-flair" },
		{ 8, "attr-leadership" }
	};

	private static Dictionary<int, string> attrConverterOutfield = new Dictionary<int, string>
	{
		{ 7, "attr-flair" },
		{ 10, "attr-marking" },
		{ 11, "attr-tackling" },
		{ 12, "attr-positioning" },
		{ 13, "attr-creativity" },
		{ 14, "attr-movement" },
		{ 15, "attr-finishing" },
		{ 16, "attr-dribbling" },
		{ 17, "attr-longshots" },
		{ 18, "attr-crossing" },
		{ 19, "attr-control" },
		{ 20, "attr-longPassing" },
		{ 21, "attr-passing" },
		{ 22, "attr-corners" },
		{ 23, "attr-freekicks" },
		{ 24, "attr-penalties" },
		{ 25, "attr-stamina" },
		{ 27, "attr-workrate" }
	};

	private static Dictionary<int, string> attrConverterKeeper = new Dictionary<int, string>
	{
		{ 10, "attr-commandOfArea" },
		{ 11, "attr-distribution" },
		{ 12, "attr-handling" },
		{ 13, "attr-oneOnOnes" },
		{ 14, "attr-shotStopping" },
		{ 15, "attr-positioning" },
		{ 16, "attr-savingPenalties" }
	};

	private static Dictionary<int, int> overallRatingMap = new Dictionary<int, int>
	{
		{ 99, 99 },
		{ 98, 98 },
		{ 97, 96 },
		{ 96, 94 },
		{ 95, 92 },
		{ 94, 90 },
		{ 93, 88 },
		{ 92, 86 },
		{ 91, 84 },
		{ 90, 82 },
		{ 89, 80 },
		{ 88, 78 },
		{ 87, 76 },
		{ 86, 74 },
		{ 85, 72 },
		{ 84, 70 },
		{ 83, 68 },
		{ 82, 66 },
		{ 81, 64 },
		{ 80, 62 },
		{ 79, 60 },
		{ 78, 58 },
		{ 77, 56 },
		{ 76, 54 },
		{ 75, 52 },
		{ 74, 51 },
		{ 73, 50 },
		{ 72, 49 },
		{ 71, 48 },
		{ 70, 47 },
		{ 69, 46 },
		{ 68, 45 },
		{ 67, 44 },
		{ 66, 43 },
		{ 65, 42 },
		{ 64, 41 },
		{ 63, 40 },
		{ 62, 39 },
		{ 61, 38 },
		{ 60, 37 }
	};

	public static Dictionary<GroupedGKAttributes, byte[]> gkGroupAttributes = new Dictionary<GroupedGKAttributes, byte[]>
	{
		{
			GroupedGKAttributes.Overall,
			new byte[1]
		},
		{
			GroupedGKAttributes.Defense,
			new byte[8] { 10, 12, 13, 14, 15, 16, 2, 6 }
		},
		{
			GroupedGKAttributes.Attack,
			new byte[10] { 11, 19, 20, 21, 22, 23, 24, 25, 26, 27 }
		},
		{
			GroupedGKAttributes.Saving,
			new byte[5] { 12, 14, 15, 2, 16 }
		},
		{
			GroupedGKAttributes.Passing,
			new byte[1] { 11 }
		},
		{
			GroupedGKAttributes.OneOnOne,
			new byte[1] { 13 }
		},
		{
			GroupedGKAttributes.Aerial,
			new byte[1] { 6 }
		}
	};

	public static Dictionary<GroupedOutfieldAttributes, byte[]> outfieldGroupAttributes = new Dictionary<GroupedOutfieldAttributes, byte[]>
	{
		{
			GroupedOutfieldAttributes.Overall,
			new byte[1]
		},
		{
			GroupedOutfieldAttributes.Defense,
			new byte[1]
		},
		{
			GroupedOutfieldAttributes.Attack,
			new byte[1]
		},
		{
			GroupedOutfieldAttributes.Tackling,
			new byte[3] { 11, 12, 10 }
		},
		{
			GroupedOutfieldAttributes.Shooting,
			new byte[2] { 15, 17 }
		},
		{
			GroupedOutfieldAttributes.Passing,
			new byte[3] { 21, 1, 20 }
		},
		{
			GroupedOutfieldAttributes.Pace,
			new byte[1] { 4 }
		},
		{
			GroupedOutfieldAttributes.Strength,
			new byte[1] { 5 }
		},
		{
			GroupedOutfieldAttributes.Dribbling,
			new byte[2] { 16, 7 }
		}
	};

	public static Dictionary<int, double[]> attributesByDescription = new Dictionary<int, double[]>
	{
		{
			1,
			new double[14]
			{
				1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 0.9, 0.7, 0.8, 1.0,
				0.8, 1.0, 0.5, 1.0
			}
		},
		{
			10,
			new double[26]
			{
				1.0, 1.0, 1.0, 0.5, 0.5, 0.5, 0.6, 0.6, 0.95, 0.95,
				0.8, 0.8, 0.7, 0.9, 1.0, 0.8, 0.8, 1.0, 1.0, 0.5,
				0.5, 0.5, 0.6, 0.6, 0.5, 0.6
			}
		},
		{
			11,
			new double[26]
			{
				1.0, 1.0, 1.0, 0.6, 0.5, 0.5, 0.6, 0.6, 0.95, 0.95,
				0.8, 0.9, 0.7, 0.8, 1.0, 0.8, 0.8, 0.9, 1.0, 0.5,
				0.5, 0.6, 0.6, 0.6, 0.5, 0.6
			}
		},
		{
			12,
			new double[26]
			{
				1.0, 1.0, 1.0, 0.8, 0.6, 0.8, 0.8, 0.7, 1.0, 1.0,
				0.9, 1.0, 0.7, 0.8, 0.9, 0.8, 0.8, 0.9, 1.0, 0.6,
				0.5, 0.8, 0.7, 0.7, 0.6, 0.7
			}
		},
		{
			13,
			new double[26]
			{
				1.0, 1.0, 1.0, 0.8, 0.6, 0.7, 0.7, 0.9, 0.95, 0.95,
				0.8, 0.9, 0.8, 0.7, 0.6, 0.9, 1.0, 0.8, 1.0, 0.5,
				0.5, 0.6, 0.7, 0.7, 0.6, 0.8
			}
		},
		{
			14,
			new double[26]
			{
				0.9, 0.9, 0.9, 0.9, 0.7, 0.9, 0.8, 1.0, 0.95, 0.95,
				0.9, 0.9, 0.9, 0.7, 0.6, 1.0, 1.0, 0.8, 0.9, 0.6,
				0.6, 0.8, 0.8, 0.8, 0.6, 0.9
			}
		},
		{
			20,
			new double[26]
			{
				1.0, 1.0, 1.0, 0.8, 0.7, 0.7, 0.7, 0.7, 0.95, 0.95,
				0.8, 0.9, 0.8, 0.8, 0.6, 1.0, 1.0, 1.0, 1.0, 0.5,
				0.5, 0.7, 0.7, 0.8, 0.6, 0.8
			}
		},
		{
			21,
			new double[26]
			{
				0.7, 0.9, 0.9, 0.9, 0.8, 0.8, 0.9, 0.9, 1.0, 1.0,
				1.0, 1.0, 0.7, 0.7, 0.7, 0.8, 0.8, 0.8, 0.9, 0.8,
				0.5, 1.0, 0.8, 0.9, 0.7, 0.9
			}
		},
		{
			22,
			new double[26]
			{
				0.8, 0.9, 0.9, 1.0, 0.9, 0.9, 1.0, 0.9, 0.95, 0.95,
				0.9, 1.0, 0.8, 0.8, 0.8, 1.0, 1.0, 0.9, 0.8, 0.8,
				0.5, 0.9, 0.8, 0.9, 0.8, 0.8
			}
		},
		{
			23,
			new double[26]
			{
				0.8, 0.9, 0.9, 0.9, 0.9, 0.8, 0.9, 0.8, 0.95, 0.95,
				0.9, 1.0, 0.8, 0.7, 0.8, 1.0, 1.0, 0.8, 0.8, 0.8,
				0.5, 0.8, 0.8, 0.9, 0.8, 0.8
			}
		},
		{
			24,
			new double[26]
			{
				0.8, 0.8, 1.0, 1.0, 0.9, 0.9, 1.0, 0.9, 1.0, 1.0,
				1.0, 1.0, 0.7, 0.7, 0.7, 0.9, 0.9, 0.7, 0.8, 0.8,
				0.5, 1.0, 0.8, 0.9, 0.8, 0.9
			}
		},
		{
			25,
			new double[26]
			{
				0.8, 0.9, 0.8, 0.9, 0.8, 0.9, 0.9, 1.0, 0.95, 0.95,
				0.9, 0.9, 0.8, 0.7, 0.6, 1.0, 1.0, 0.8, 0.7, 0.8,
				0.5, 0.9, 0.8, 0.9, 0.8, 0.9
			}
		},
		{
			27,
			new double[26]
			{
				0.6, 0.6, 0.6, 1.0, 0.9, 1.0, 0.9, 1.0, 0.95, 0.95,
				0.9, 0.9, 0.9, 0.7, 0.6, 0.9, 0.9, 0.6, 0.6, 1.0,
				0.5, 1.0, 0.8, 0.9, 0.8, 1.0
			}
		},
		{
			28,
			new double[26]
			{
				0.6, 0.6, 0.9, 1.0, 0.9, 1.0, 1.0, 0.9, 1.0, 1.0,
				1.0, 1.0, 0.7, 0.7, 0.7, 0.8, 0.8, 0.6, 0.7, 1.0,
				0.5, 1.0, 0.8, 1.0, 0.8, 0.9
			}
		},
		{
			40,
			new double[26]
			{
				0.4, 0.4, 0.4, 0.9, 1.0, 0.8, 0.9, 0.7, 0.95, 0.95,
				0.6, 0.8, 0.7, 0.9, 1.0, 0.9, 1.0, 0.9, 0.8, 0.8,
				0.5, 0.7, 0.9, 0.8, 0.8, 0.6
			}
		},
		{
			41,
			new double[26]
			{
				0.3, 0.3, 0.3, 1.0, 1.0, 0.8, 0.9, 0.7, 1.0, 1.0,
				0.6, 0.8, 0.8, 0.7, 0.9, 0.8, 0.8, 0.8, 0.8, 0.8,
				0.5, 0.7, 1.0, 0.8, 0.9, 0.6
			}
		},
		{
			42,
			new double[26]
			{
				0.3, 0.3, 0.3, 1.0, 1.0, 1.0, 0.9, 0.9, 0.95, 0.95,
				0.7, 0.9, 0.9, 0.7, 0.7, 0.9, 0.9, 0.6, 0.6, 1.0,
				0.5, 0.9, 1.0, 1.0, 0.8, 0.9
			}
		},
		{
			43,
			new double[26]
			{
				0.3, 0.3, 0.3, 1.0, 1.0, 1.0, 1.0, 0.9, 0.95, 0.95,
				0.8, 1.0, 0.7, 0.8, 0.8, 0.8, 0.9, 0.8, 0.8, 1.0,
				0.5, 1.0, 0.9, 0.9, 0.9, 0.8
			}
		},
		{
			44,
			new double[26]
			{
				0.3, 0.3, 0.3, 1.0, 1.0, 0.9, 0.9, 0.9, 0.95, 0.95,
				0.8, 0.9, 0.8, 0.8, 0.9, 0.8, 1.0, 0.8, 0.7, 0.8,
				0.5, 0.8, 0.9, 0.9, 0.8, 0.8
			}
		}
	};

	public static double MapRating(double rating)
	{
		if (rating >= 99.0)
		{
			return rating;
		}
		if (rating <= 75.0)
		{
			return rating - 23.0;
		}
		double num = rating - Math.Truncate(rating);
		return (double)(((int)rating - 52) * 2 + 6) + num;
	}

	public static string attributeIdToFullName(int attr, bool isKeeper)
	{
		if (attrConverterBase.ContainsKey(attr))
		{
			return Translation.GetText(attrConverterBase[attr]);
		}
		if (isKeeper)
		{
			if (attrConverterKeeper.ContainsKey(attr))
			{
				return Translation.GetText(attrConverterKeeper[attr]);
			}
		}
		else if (attrConverterOutfield.ContainsKey(attr))
		{
			return Translation.GetText(attrConverterOutfield[attr]);
		}
		return "Unknown";
	}

	public static Color32 getAttributeValueColour(int value)
	{
		if (value > 0 && value < 25)
		{
			return Constants.attributeRedColour;
		}
		if (value >= 25 && value <= 50)
		{
			return Constants.attributeOrangeColour;
		}
		if (value > 50 && value < 75)
		{
			return Constants.attributeYellowColour;
		}
		if (value >= 75 && value <= 90)
		{
			return Constants.attributeDarkGreenColour;
		}
		if (value > 90)
		{
			return Constants.attributeGreenColour;
		}
		if (value < 0)
		{
			return Constants.attributeGreyColour;
		}
		return default;
	}

	public static PlayerAttributes GetAttributesForDisplay(int playerid, AttrOpts opts = null)
	{
		if (opts == null)
		{
			opts = new AttrOpts();
			opts.SetDefaultAttrOpts();
			opts.fogofwar = false;
		}
		if (Game.instance.managedClub != null)
		{
			short currentManagedClubId = Game.instance.CurrentManagedClubId;
			if (Player.GetClubID(playerid) == currentManagedClubId || Club.GetLoanedOutPlayers(currentManagedClubId).Contains(playerid))
			{
				opts.fogofwar = false;
			}
		}
		return SetAttributesForPlayer(playerid, opts);
	}

	public static double[] GetAttributesForRadarChart(int playerid)
	{
		bool flag = PlayerData.IsGoalKeeper(playerid);
		PlayerAttributes playerAttributes = SetAttributesForPlayer(playerid);
		return (!flag) ? new double[7]
		{
			GetTechnicalRating(playerAttributes, flag),
			GetMentalRating(playerAttributes, flag),
			GetAttackingRating(playerAttributes, flag),
			GetDefendingRating(playerAttributes, flag),
			GetPhysicalRating(playerAttributes, flag),
			playerAttributes.attrsDisplay[6],
			playerAttributes.attrsDisplay[4]
		} : new double[7]
		{
			GetTechnicalRating(playerAttributes, flag),
			GetMentalRating(playerAttributes, flag),
			playerAttributes.attrsDisplay[11],
			playerAttributes.attrsDisplay[14],
			GetPhysicalRating(playerAttributes, flag),
			playerAttributes.attrsDisplay[6],
			playerAttributes.attrsDisplay[4]
		};
	}

	public static double GetTechnicalRating(PlayerAttributes attributes, bool isKeeper)
	{
		if (!isKeeper)
		{
			return (double)(attributes.attrsDisplay[14] + attributes.attrsDisplay[19] + attributes.attrsDisplay[20] + attributes.attrsDisplay[21] + attributes.attrsDisplay[24] + attributes.attrsDisplay[23] + attributes.attrsDisplay[22] + attributes.attrsDisplay[18] + attributes.attrsDisplay[17]) / 9.0;
		}
		return (double)(attributes.attrsDisplay[10] + attributes.attrsDisplay[11] + attributes.attrsDisplay[12] + attributes.attrsDisplay[13] + attributes.attrsDisplay[14] + attributes.attrsDisplay[15] + attributes.attrsDisplay[16]) / 7.0;
	}

	public static double GetMentalRating(PlayerAttributes attributes, bool isKeeper)
	{
		double num = attributes.attrsDisplay[0] + attributes.attrsDisplay[1] + attributes.attrsDisplay[2] + attributes.attrsDisplay[7] + attributes.attrsDisplay[8];
		if (!isKeeper)
		{
			return num / 5.0;
		}
		return num / 4.0;
	}

	public static double GetAttackingRating(PlayerAttributes attributes, bool isKeeper)
	{
		double num = 0.0;
		foreach (int attackerOnlyAttribute in attackerOnlyAttributes)
		{
			num += (double)attributes.attrsDisplay[attackerOnlyAttribute];
		}
		return num / (double)attackerOnlyAttributes.Count;
	}

	public static double GetDefendingRating(PlayerAttributes attributes, bool isKeeper)
	{
		double num = 0.0;
		foreach (int defenderOnlyAttribute in defenderOnlyAttributes)
		{
			num += (double)attributes.attrsDisplay[defenderOnlyAttribute];
		}
		return num / (double)defenderOnlyAttributes.Count;
	}

	public static double GetPhysicalRating(PlayerAttributes attributes, bool isKeeper)
	{
		double num = attributes.attrsDisplay[6] + attributes.attrsDisplay[4] + attributes.attrsDisplay[25] + attributes.attrsDisplay[5] + attributes.attrsDisplay[27];
		if (!isKeeper)
		{
			return num / 5.0;
		}
		return num / 3.0;
	}

	public static List<string> GetRadarChartCategories(bool isKeeper)
	{
		List<string> list = new List<string>();
		if (isKeeper)
		{
			return new List<string>
			{
				Translation.GetText("attr-type-technical"),
				Translation.GetText("mental"),
				Translation.GetText("attr-distribution"),
				Translation.GetText("attr-shotStopping"),
				Translation.GetText("attr-type-physical"),
				Translation.GetText("attr-aerialAbility"),
				Translation.GetText("pace")
			};
		}
		return new List<string>
		{
			Translation.GetText("attr-type-technical"),
			Translation.GetText("mental"),
			Translation.GetText("inst-mentality-4"),
			Translation.GetText("defending"),
			Translation.GetText("attr-type-physical"),
			Translation.GetText("attr-aerialAbility"),
			Translation.GetText("pace")
		};
	}

	public static PlayerAttributes GetAttributesForPlayerSpotlight(int playerid, AttrOpts opts = null)
	{
		if (opts == null)
		{
			opts = new AttrOpts();
			opts.SetDefaultAttrOpts();
		}
		return SetAttributesForPlayer(playerid, opts);
	}

	public static PlayerAttributes GetAttributesForMatchEngine(int playerid, AttrOpts opts = null)
	{
		if (opts == null)
		{
			opts = new AttrOpts();
			opts.SetDefaultAttrOpts();
		}
		return SetAttributesForPlayer(playerid, opts);
	}

	public static PlayerAttributes SetAttributesForPlayer(int playerid, AttrOpts opts = null)
	{
		if (opts == null)
		{
			opts = new AttrOpts();
			opts.SetDefaultAttrOpts();
		}
		AttrPlayerStruct playerDataObj = GetPlayerDataObj(playerid, opts);
		if (Game.instance.g_PlayersScouted.ContainsKey(playerid))
		{
			opts.alwaysShowAttDef = true;
		}
		if (Game.instance.g_trainingDrillManager.TrainingPlayer.ContainsKey(playerid))
		{
			opts.fogofwar = false;
		}
		PlayerAttributes playerAttributes = SetAttributesForPlayer(playerDataObj, opts);
		if (Game.instance.g_trainingDrillManager.TrainingPlayer.ContainsKey(playerid))
		{
			TrainingPlayer trainingPlayer = Game.instance.g_trainingDrillManager.TrainingPlayer[playerid];
			if (opts.youthBoosts)
			{
				playerAttributes.attrsDisplay = playerAttributes.attrsDisplay.Select((float a, int i) => a + trainingPlayer.youthAcademyBoosts[i]).ToArray();
			}
			playerAttributes.attrs = playerAttributes.attrsDisplay.Select((float attribute) => attributeOutOf20(attribute)).ToArray();
			playerAttributes.groupedAttrs = GetGroupedPlayerAttributes(((IEnumerable<float>)playerAttributes.attrsDisplay).Select((Func<float, double>)((float i) => i)).ToArray(), playerDataObj.rating, playerAttributes.attrsDisplay.Select((float i) => 0).ToArray(), playerDataObj.desc, playerDataObj.age, playerDataObj.potential, showAttDef: true, fogOfWar: false);
		}
		return playerAttributes;
	}

	public static PlayerAttributes SetAttributesForPlayer(AttrPlayerStruct playerdata, AttrOpts opts = null)
	{
		if (opts == null)
		{
			opts = new AttrOpts();
			opts.SetDefaultAttrOpts();
		}
		if (playerdata.usevotes && playerdata.attrsstr != null && playerdata.attrsstr != "" && playerdata.numvotes != 0)
		{
			playerdata.orderAttrVotes = GetAttributeVotesOrderArray(playerdata.attrsstr);
		}
		if (((playerdata.pos & 1) != 0 && playerdata.desc != 1) || ((playerdata.pos & 1) == 0 && playerdata.desc == 1))
		{
			playerdata.desc = 0;
		}
		if (playerdata.desc == 0)
		{
			if (playerdata.prefpos != 0)
			{
				playerdata.desc = GetPlayerDescriptionFromPosition(playerdata.prefpos);
			}
			else
			{
				playerdata.desc = GetPlayerDescriptionFromPosition(playerdata.pos);
			}
		}
		playerdata.allattributes = new double[30];
		SetAttributesByDescription(playerdata, opts);
		PlayerAttributes playerAttributes = new PlayerAttributes();
		playerAttributes.attrs = playerdata.allattributes;
		playerAttributes.attrsDisplay = playerAttributes.attrs.Select((double attribute) => attributeOutOf100(attribute)).ToArray();
		if (opts.fogofwar)
		{
			playerAttributes.attrsDisplay = FogOfWarifyAttributes(playerdata.playerid, playerdata.rating, playerdata.pos == 1, playerAttributes.attrsDisplay);
		}
		playerAttributes.groupedAttrs = GetGroupedPlayerAttributes(((IEnumerable<double>)playerAttributes.attrs).Select((Func<double, double>)((double attribute) => attributeOutOf100(attribute))).ToArray(), playerdata.rating, playerAttributes.attrsDisplay.Select((float a, int i) => (a == -1f) ? 1 : 0).ToArray(), playerdata.desc, playerdata.age, playerdata.potential, opts.alwaysShowAttDef, opts.fogofwar);
		playerAttributes.desc = playerdata.desc;
		playerAttributes.orderedAttrsVotes = playerdata.orderAttrVotes;
		playerAttributes.isKeeper = playerdata.pos == 1;
		return playerAttributes;
	}

	public static float[] FogOfWarifyAttributes(int playerid, double playerRating, bool isKeeper, float[] attributes)
	{
		foreach (int item in GetAttributesAffectedByFogOfWar(playerid, playerRating, isKeeper, null, attributes))
		{
			attributes[item] = -1f;
		}
		return attributes;
	}

	public static List<int> GetAttributesAffectedByFogOfWar(int playerid, double playerRating, bool isKeeper, float? revealAttributesEffect, float[] attributes)
	{
		List<int> result = new List<int>();
		Dictionary<int, float> dictionary = new Dictionary<int, float>();
		if (playerRating > 91.0)
		{
			return result;
		}
		for (int i = 0; i < attributes.Length; i++)
		{
			dictionary.Add(i, attributes[i]);
		}
		int seed = Game.instance.Seed;
		RNG random = Game.instance.Random;
		int seed2 = seed + playerid;
		int seed3 = random.GetSeed();
		random.SetSeed(seed2);
		int[] array = (isKeeper ? ((int[])allAttributesKeeper.Clone()) : ((int[])allAttributesOutfield.Clone()));
		array.SeededShuffle();
		if (!revealAttributesEffect.HasValue)
		{
			revealAttributesEffect = (Game.instance.g_PlayersScouted.ContainsKey(playerid) ? Game.instance.g_PlayersScouted[playerid].attributeEffect : 0f);
		}
		float num = 1f;
		if (playerRating >= 74.0)
		{
			num = 1f - ((float)playerRating - 74f) / 18f;
		}
		num -= revealAttributesEffect.Value * num;
		int num2 = (int)Mathf.Round(num * (float)array.Length);
		List<KeyValuePair<int, float>> list = dictionary.ToList();
		list.Sort((KeyValuePair<int, float> pair1, KeyValuePair<int, float> pair2) => pair1.Value.CompareTo(pair2.Value));
		list.Reverse();
		int count = ((num2 != 0) ? (array.Count() - num2) : array.Length);
		list.RemoveRange(0, count);
		random.SetSeed(seed3);
		List<int> list2 = new List<int>();
		for (int num3 = 0; num3 < list.Count; num3++)
		{
			list2.Add(list.ElementAt(num3).Key);
		}
		return list2;
	}

	public static AttrPlayerStruct GetPlayerDataObj(int pid, AttrOpts opts)
	{
		PlayerData playerData = Game.instance.g_PlayerData[pid];
		AttrPlayerStruct attrPlayerStruct = new AttrPlayerStruct
		{
			playerid = pid,
			pos = playerData.Position,
			prefpos = playerData.PreferredPosition,
			desc = playerData.Description,
			potential = playerData.Potential,
			age = Player.GetAge(pid, opts.date),
			dob = playerData.DateOfBirth,
			attrsstr = playerData.AttributesRank,
			numvotes = playerData.AttributesRankVotes,
			usevotes = opts.usevotes
		};
		if (opts.matchdayrating)
		{
			attrPlayerStruct.rating = (int)Player.GetMatchDayRating(pid);
		}
		else
		{
			attrPlayerStruct.rating = Player.GetRatingPrecise(pid, opts.date);
		}
		if (opts.randonage)
		{
			int seed = Game.instance.Seed;
			RNG random = Game.instance.Random;
			int seed2 = random.GetSeed();
			int seed3 = (seed + pid + attrPlayerStruct.age) % 32768;
			random.SetSeed(seed3);
			int num = (Game.instance.Random.GetRand() % 365 + Game.instance.Random.GetRand() % 365) / 2;
			long num2 = attrPlayerStruct.dob.GetEpochSecondsLong() + attrPlayerStruct.age * 31557600;
			if (Game.instance.CurrentDate.GetEpochSecondsLong() > num2 + num * 86400)
			{
				attrPlayerStruct.age++;
			}
			random.SetSeed(seed2);
		}
		return attrPlayerStruct;
	}

	public static float[] GetGroupedPlayerAttributes(double[] attributes, double rating, int[] hiddenAttributes, int playerDesc, int age, int potential, bool showAttDef = false, bool fogOfWar = true)
	{
		if (playerDesc == 1)
		{
			return GroupGKAttributes(attributes, rating, hiddenAttributes, age, potential, showAttDef, fogOfWar);
		}
		return GroupOutfieldAttributes(attributes, rating, hiddenAttributes, playerDesc, age, potential, showAttDef, fogOfWar);
	}

	public static float attributeOutOf100(double value)
	{
		return (float)value * 5f;
	}

	public static double attributeOutOf20(float value)
	{
		return value / 5f;
	}

	private static float[] GroupGKAttributes(double[] attributes, double rating, int[] hiddenAttributes, int age, int potential, bool showAttDef = false, bool fogOfWar = true)
	{
		float[] array = new float[gkGroupAttributes.Count];
		foreach (KeyValuePair<GroupedGKAttributes, byte[]> gkGroupAttribute in gkGroupAttributes)
		{
			GroupedGKAttributes key = gkGroupAttribute.Key;
			byte[] value = gkGroupAttribute.Value;
			bool flag = false;
			int num = (int)key;
			double num2 = 0.0;
			if (key == GroupedGKAttributes.Overall)
			{
				continue;
			}
			foreach (int num3 in value)
			{
				double num4 = attributes[num3];
				num2 += num4;
				if (hiddenAttributes[num3] > 0)
				{
					flag = true;
				}
			}
			num2 /= (double)value.Length;
			if ((key == GroupedGKAttributes.Attack || key == GroupedGKAttributes.Defense || key == GroupedGKAttributes.Overall) & showAttDef)
			{
				flag = false;
			}
			array[num] = (flag ? (-1) : ((int)Math.Round(num2)));
		}
		if (rating >= 88.0 || (potential >= 90 && age >= 28) || !fogOfWar || showAttDef)
		{
			array[0] = (int)GetGKOverallRating(attributes, array[2], array[1], rating, age, potential);
		}
		else
		{
			array[0] = -1f;
		}
		return array;
	}

	private static float[] GroupOutfieldAttributes(double[] attributes, double rating, int[] hiddenAttributes, int desc, int age, int potential, bool showAttDef = false, bool fogOfWar = true)
	{
		float[] array = new float[outfieldGroupAttributes.Count];
		foreach (KeyValuePair<GroupedOutfieldAttributes, byte[]> outfieldGroupAttribute in outfieldGroupAttributes)
		{
			GroupedOutfieldAttributes key = outfieldGroupAttribute.Key;
			byte[] array2 = outfieldGroupAttribute.Value;
			bool flag = false;
			int num = (int)key;
			double num2 = 0.0;
			if (key == GroupedOutfieldAttributes.Overall)
			{
				continue;
			}
			if (desc != 1 && key == GroupedOutfieldAttributes.Defense)
			{
				array2 = ((desc >= 20) ? new byte[5] { 10, 11, 12, 0, 2 } : new byte[6] { 10, 11, 12, 0, 2, 6 });
			}
			else if (desc != 1 && key == GroupedOutfieldAttributes.Attack)
			{
				array2 = ((desc < 40) ? new byte[12]
				{
					14, 15, 16, 17, 18, 19, 20, 21, 13, 1,
					7, 4
				} : new byte[10] { 14, 15, 16, 17, 19, 1, 7, 6, 4, 5 });
			}
			foreach (int num3 in array2)
			{
				double num4 = attributes[num3];
				num2 += num4;
				if (hiddenAttributes[num3] > 0)
				{
					flag = true;
				}
			}
			num2 /= (double)array2.Length;
			if ((key == GroupedOutfieldAttributes.Attack || key == GroupedOutfieldAttributes.Defense || key == GroupedOutfieldAttributes.Overall) & showAttDef)
			{
				flag = false;
			}
			array[num] = (flag ? (-1) : ((int)Math.Round(num2)));
		}
		if (rating >= 88.0 || (potential >= 90 && age >= 28) || !fogOfWar || showAttDef)
		{
			array[0] = (int)GetOutfieldOverallRating(attributes, array[2], array[1], rating, desc, age, potential);
		}
		else
		{
			array[0] = -1f;
		}
		return array;
	}

	private static double GetGKOverallRating(double[] attributes, double attacking, double defending, double rating, int age, int potential)
	{
		double num = MapRating(rating);
		num -= 2.0;
		if (age >= 38)
		{
			num -= 4.0;
		}
		else if (age >= 37)
		{
			num -= 3.0;
		}
		else if (age >= 35)
		{
			num -= 2.0;
		}
		else if (age >= 33)
		{
			num--;
		}
		if (potential >= 95)
		{
			num += 4.0;
		}
		else if (potential >= 94)
		{
			num += 3.0;
		}
		else if (potential >= 92)
		{
			num += 2.0;
		}
		else if (potential >= 90)
		{
			num++;
		}
		if (!(num <= 99.0))
		{
			return 99.0;
		}
		return num;
	}

	private static double GetOutfieldOverallRating(double[] attributes, double attacking, double defending, double rating, int desc, int age, int potential)
	{
		double num = MapRating(rating);
		num -= 2.0;
		if (age >= 36)
		{
			num -= 4.0;
		}
		else if (age >= 35)
		{
			num -= 3.0;
		}
		else if (age >= 33)
		{
			num -= 2.0;
		}
		else if (age >= 31)
		{
			num--;
		}
		if (potential >= 96)
		{
			num += 4.0;
		}
		else if (potential >= 94)
		{
			num += 3.0;
		}
		else if (potential >= 92)
		{
			num += 2.0;
		}
		else if (potential >= 90)
		{
			num++;
		}
		if (!(num <= 99.0))
		{
			return 99.0;
		}
		return num;
	}

	private static List<int[]> GetAttributeVotesOrderArray(string attrsstr)
	{
		List<int[]> list = new List<int[]>();
		int[] array = Array.ConvertAll(attrsstr.Split(','), int.Parse);
		for (int i = 0; i < array.Length; i++)
		{
			if (array[i] > 0)
			{
				list.Add(new int[2]
				{
					i,
					30 - array[i]
				});
			}
			else
			{
				list.Add(new int[2] { i, 0 });
			}
		}
		list.Sort((int[] a, int[] b) => b[1] - a[1]);
		return list;
	}

	public static int GetPlayerDescriptionFromPosition(int position)
	{
		int result = 0;
		if ((position & 1) > 0)
		{
			result = 1;
		}
		else if ((position & 0xE) > 0)
		{
			if ((position & 4) > 0 && ((position & 2) > 0 || (position & 8) > 0))
			{
				result = 11;
			}
			else if ((position & 4) > 0 && ((position & 0x10) > 0 || (position & 0x40) > 0))
			{
				result = 11;
			}
			else if ((position & 4) > 0)
			{
				result = 10;
			}
			else if ((position & 0x10) <= 0 && (position & 0x40) <= 0)
			{
				result = (((position & 2) <= 0 && (position & 8) <= 0) ? 11 : 13);
			}
			else
			{
				result = 14;
			}
		}
		else if ((position & 0x70) > 0)
		{
			if ((position & 0x20) > 0)
			{
				result = 20;
			}
			else
			{
				result = (((position & 0x10) <= 0 && (position & 0x40) <= 0) ? 20 : 14);
			}
		}
		else if ((position & 0x1C00) > 0)
		{
			if ((position & 0x800) > 0 && (position & 0xE000) > 0)
			{
				result = 43;
			}
			else if ((position & 0x800) > 0)
			{
				result = 28;
			}
			else
			{
				result = (((position & 0x400) <= 0 && (position & 0x1000) <= 0) ? 28 : 27);
			}
		}
		else if ((position & 0x380) > 0)
		{
			if ((position & 0x100) > 0)
			{
				result = 23;
			}
			else
			{
				result = (((position & 0x80) <= 0 && (position & 0x200) <= 0) ? 23 : 25);
			}
		}
		else if ((position & 0xE000) > 0)
		{
			if ((position & 0x2000) > 0 && (position & 0x4000) > 0 && (position & 0x2000) > 0)
			{
				result = 44;
			}
			else if ((position & 0x2000) > 0 || (position & 0x8000) > 0)
			{
				result = 42;
			}
			else
			{
				result = (((position & 0x4000) <= 0) ? 44 : 40);
			}
		}
		return result;
	}

	private static void SetAttributesByDescription(AttrPlayerStruct playerdata, AttrOpts opts)
	{
		double num = playerdata.rating - 59.0;
		if (num < 1.0)
		{
			num = 1.0;
		}
		int num2 = playerdata.potential - 59;
		if (num2 < 1)
		{
			num2 = 1;
		}
		bool flag = playerdata.pos == 1;
		AttrPlayerStruct playerdata2 = new AttrPlayerStruct
		{
			playerid = playerdata.playerid,
			pos = playerdata.pos,
			prefpos = playerdata.prefpos,
			desc = playerdata.desc,
			rating = num,
			potential = num2,
			age = playerdata.age,
			numvotes = playerdata.numvotes,
			usevotes = playerdata.usevotes,
			orderAttrVotes = playerdata.orderAttrVotes
		};
		int[] array = ((!flag) ? allAttributesOutfield : allAttributesKeeper);
		int numberOfVotedAttributesToUse = GetNumberOfVotedAttributesToUse(num2, playerdata.desc);
		int numberOfTopAttributes = (int)Math.Round((double)numberOfVotedAttributesToUse / 3.0);
		int[] array2 = new int[30];
		int num3 = 0;
		if (playerdata.usevotes && playerdata.numvotes > 0 && playerdata.orderAttrVotes.Count > 0)
		{
			for (int i = 0; i < numberOfVotedAttributesToUse; i++)
			{
				if (playerdata.orderAttrVotes[i][1] > 0)
				{
					num3 = playerdata.orderAttrVotes[i][0];
					array2[num3] = i + 1;
				}
			}
		}
		for (int j = 0; j < array.Length; j++)
		{
			num3 = array[j];
			SetAttribute(playerdata.allattributes, num3, attributesByDescription[playerdata.desc][j], playerdata2, numberOfTopAttributes, array2, doNotRound: true);
		}
	}

	public static int GetNumberOfVotedAttributesToUse(int playerrating, int desc)
	{
		int num = 0;
		playerrating += 59;
		num = ((playerrating < 70) ? 1 : ((playerrating < 75) ? 2 : ((playerrating < 77) ? 3 : ((playerrating < 79) ? 4 : ((playerrating < 85) ? 5 : ((playerrating < 88) ? 6 : ((playerrating < 90) ? 7 : (playerrating switch
		{
			90 => 8, 
			91 => 8, 
			92 => 9, 
			93 => 9, 
			94 => 10, 
			95 => 11, 
			96 => 12, 
			97 => 13, 
			_ => 14, 
		}))))))));
		if (desc == 1)
		{
			num = (int)Math.Ceiling((double)num / 2.0);
		}
		else if (desc < 20)
		{
			num = (int)Math.Ceiling((double)num * 0.66);
		}
		return num;
	}

	private static void SetAttribute(double[] attributes, int attrid, double attrrank, AttrPlayerStruct playerdata, int numberOfTopAttributes, int[] attrVoteOrder, bool doNotRound)
	{
		Func<double, bool, double> func = (double value, bool noNotRound) => doNotRound ? value : Math.Round(value);
		double rating = playerdata.rating;
		int age = playerdata.age;
		bool flag = playerdata.pos == 1;
		int potential = playerdata.potential;
		attrrank = ((AttributesFunctions.GetAttributeType(attrid, flag) == 0) ? (attrrank - 0.2) : (attrrank - 0.1));
		int num = 0;
		int num2 = 0;
		double num3 = attrrank;
		double num4 = 0.0;
		if (rating >= 39.0)
		{
			num = 19;
			num2 = 19;
		}
		else if (rating >= 37.0)
		{
			num = 18;
			num2 = 18;
		}
		else
		{
			num = 17;
			num2 = 17;
		}
		int num5 = 0;
		if (playerdata.usevotes && playerdata.numvotes > 0)
		{
			num5 = attrVoteOrder[attrid];
		}
		if (num5 != 0)
		{
			num3 = ((num5 > numberOfTopAttributes) ? 0.9 : 1.0);
			if (num3 < attrrank)
			{
				num3 = attrrank;
			}
			num += 2;
			num2 = num2;
			num4 = ((num5 == 0 || num5 > numberOfTopAttributes) ? 0.8 : 0.9);
			if (num4 > attrrank)
			{
				attrrank = num4;
			}
		}
		if (flag)
		{
			switch (attrid)
			{
			default:
				if (num5 != 0)
				{
					if (num5 <= numberOfTopAttributes)
					{
						attributes[attrid] = func(attrrank * (rating / 40.0 * 20.0), arg2: true) + 3.0;
					}
					else
					{
						attributes[attrid] = func(attrrank * (rating / 40.0 * 20.0), arg2: true) + 2.0;
					}
				}
				else
				{
					attributes[attrid] = func(attrrank * (rating / 40.0 * 20.0), arg2: true) + 1.0;
				}
				break;
			case 4:
				attributes[attrid] = func(num3 * (double)num, arg2: true);
				attributes[attrid] -= SkewAttributeOnRating(rating, 4);
				attributes[attrid] -= SkewAttributeOnAge(age, 18, 0.3);
				attributes[attrid] -= SkewAttributeOnAge(29, age, 0.3);
				break;
			case 5:
				attributes[attrid] = func(num3 * (double)num, arg2: true);
				attributes[attrid] -= SkewAttributeOnRating(rating, 4);
				attributes[attrid] -= SkewAttributeOnAge(age, 22, 0.3);
				attributes[attrid] -= SkewAttributeOnAge(34, age, 0.3);
				break;
			case 6:
				attributes[attrid] = func(num3 * (double)num, arg2: true);
				attributes[attrid] -= SkewAttributeOnRating(rating, 8);
				break;
			case 0:
				attributes[attrid] = func(num3 * (double)num2, arg2: true);
				attributes[attrid] -= SkewAttributeOnRating(rating, 4);
				break;
			case 2:
				attributes[attrid] = func(num3 * (double)num2, arg2: true);
				attributes[attrid] -= SkewAttributeOnRating(rating, 7);
				break;
			case 3:
				if (num5 != 0)
				{
					attributes[attrid] = func(num3 * (double)num2, arg2: true);
				}
				else
				{
					attributes[attrid] = func((double)potential / 40.0 * (double)num2, arg2: true);
				}
				attributes[attrid] -= SkewAttributeOnRating(rating, 6);
				attributes[attrid] -= SkewAttributeOnAge(29, age, 0.5);
				break;
			case 7:
				attributes[attrid] = func(num3 * (double)num2, arg2: true);
				attributes[attrid] -= SkewAttributeOnRating(rating, 10);
				break;
			case 8:
			{
				if (num5 != 0)
				{
					attributes[attrid] = func(num3 * (double)num2, arg2: true);
					break;
				}
				int num6 = 0;
				if (age > 29)
				{
					num6 = age - 29;
				}
				attributes[attrid] = func(rating / 40.0 * 20.0 * 0.55 + (double)potential / 40.0 * 20.0 * 0.35 + (double)num6 * 0.75, arg2: true);
				break;
			}
			case 9:
				attributes[attrid] = func(num3 * (double)num2, arg2: true);
				attributes[attrid] -= SkewAttributeOnRating(rating, 6);
				break;
			case 11:
				attributes[attrid] = func(num3 * (double)num2, arg2: true);
				attributes[attrid] -= SkewAttributeOnRating(rating, 10);
				break;
			}
		}
		else
		{
			switch (attrid)
			{
			default:
				if (num5 != 0)
				{
					if (num5 <= numberOfTopAttributes)
					{
						attributes[attrid] = func(attrrank * (rating / 40.0 * 20.0), arg2: true) + 3.0;
					}
					else
					{
						attributes[attrid] = func(attrrank * (rating / 40.0 * 20.0), arg2: true) + 2.0;
					}
				}
				else
				{
					attributes[attrid] = func(attrrank * (rating / 40.0 * 20.0), arg2: true) + 1.0;
				}
				break;
			case 5:
				attributes[attrid] = func(num3 * (double)num, arg2: true);
				attributes[attrid] -= SkewAttributeOnRating(rating, 4);
				attributes[attrid] -= SkewAttributeOnAge(age, 22, 0.3);
				attributes[attrid] -= SkewAttributeOnAge(34, age, 0.3);
				break;
			case 4:
				attributes[attrid] = func(num3 * (double)num, arg2: true);
				attributes[attrid] -= SkewAttributeOnRating(rating, 4);
				attributes[attrid] -= SkewAttributeOnAge(age, 18, 0.3);
				attributes[attrid] -= SkewAttributeOnAge(29, age, 0.3);
				break;
			case 6:
				attributes[attrid] = func(num3 * (double)num, arg2: true);
				attributes[attrid] -= SkewAttributeOnRating(rating, 8);
				break;
			case 25:
				attributes[attrid] = func(num3 * (double)num, arg2: true);
				attributes[attrid] -= SkewAttributeOnRating(rating, 4);
				attributes[attrid] -= SkewAttributeOnAge(age, 18, 0.3);
				attributes[attrid] -= SkewAttributeOnAge(32, age, 0.3);
				break;
			case 27:
				attributes[attrid] = func(num3 * (double)num, arg2: true);
				attributes[attrid] -= SkewAttributeOnRating(rating, 4);
				attributes[attrid] -= SkewAttributeOnAge(30, age, 0.3);
				break;
			case 0:
				attributes[attrid] = func(num3 * (double)num2, arg2: true);
				attributes[attrid] -= SkewAttributeOnAge(34, age, 0.5);
				break;
			case 2:
				attributes[attrid] = func(num3 * (double)num2, arg2: true);
				attributes[attrid] -= SkewAttributeOnRating(rating, 4);
				break;
			case 3:
				if (num5 != 0)
				{
					attributes[attrid] = func(num3 * (double)num2, arg2: true);
				}
				else
				{
					attributes[attrid] = func((double)potential / 40.0 * (double)num2, arg2: true);
				}
				attributes[attrid] -= SkewAttributeOnRating(rating, 6);
				attributes[attrid] -= SkewAttributeOnAge(29, age, 0.5);
				break;
			case 7:
				attributes[attrid] = func(num3 * (double)num2, arg2: true);
				attributes[attrid] -= SkewAttributeOnRating(rating, 10);
				break;
			case 8:
			{
				if (num5 != 0)
				{
					attributes[attrid] = func(num3 * (double)num2, arg2: true);
					break;
				}
				int num7 = 0;
				if (age > 29)
				{
					num7 = age - 29;
				}
				attributes[attrid] = func(rating / 40.0 * 20.0 * 0.55 + (double)potential / 40.0 * 20.0 * 0.35 + (double)num7 * 0.75, arg2: true);
				break;
			}
			case 9:
				attributes[attrid] = func(num3 * (double)num2, arg2: true);
				attributes[attrid] -= SkewAttributeOnRating(rating, 6);
				break;
			case 13:
				attributes[attrid] = func(num3 * (double)num2, arg2: true);
				attributes[attrid] -= SkewAttributeOnRating(rating, 10);
				break;
			case 24:
				attributes[attrid] = func(num3 * (double)num2, arg2: true);
				attributes[attrid] -= SkewAttributeOnRating(rating, 10);
				break;
			case 23:
				attributes[attrid] = func(num3 * (double)num2, arg2: true);
				attributes[attrid] -= SkewAttributeOnRating(rating, 10);
				break;
			case 26:
				attributes[attrid] = func(num3 * (double)num2, arg2: true);
				attributes[attrid] -= SkewAttributeOnRating(rating, 10);
				break;
			}
		}
		if (attributes[attrid] > 20.0)
		{
			attributes[attrid] = 20.0;
		}
		else if (attributes[attrid] < 1.0)
		{
			attributes[attrid] = 1.0;
		}
	}

	private static double SkewAttributeOnRating(double rating, int maxdeduction)
	{
		double start = 5.0;
		double end = 36.0;
		return SkewAttributeOnRating(rating, maxdeduction, start, end);
	}

	private static double SkewAttributeOnRating(double rating, int maxdeduction, double start, double end)
	{
		if (rating > start)
		{
			if (rating > end)
			{
				return 0.0;
			}
			return (double)maxdeduction - (rating - start) / (end - start) * (double)maxdeduction;
		}
		return maxdeduction;
	}

	private static double SkewAttributeOnAge(int start, int end, double detriment)
	{
		int num = end - start;
		double result = 0.0;
		if (num > 0)
		{
			result = (double)num * detriment;
		}
		return result;
	}
}
