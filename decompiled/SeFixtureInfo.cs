using System.Collections.Generic;

public class SeFixtureInfo
{
	public class SeFixtureInfoTeamData
	{
		public short clubDataID;

		public string name;

		public string manager;

		public bool isAI;

		public string kitID;

		public string kitIDAway;

		public string colour;

		public string colourAway;

		public string numberColour;

		public string numberColourAway;

		public cInstructions inst;

		public string formations;

		public int formationID;

		public int direction;

		public SePlayerInfo[] players;
	}

	public class SePitchData
	{
		public SePitchSizeData size;

		public float friction;
	}

	public class SePitchPosData
	{
		public float x;

		public float y;
	}

	public class SePitchSizeData
	{
		public float x;

		public float y;
	}

	public enum TacticsInstType
	{
		WIDTH,
		MENTALITY,
		TEMPO,
		CREATIVITY,
		PASSING,
		ATT_STYLE,
		FORWARDS,
		WINGERS,
		MIDFIELDERS,
		PRESSING,
		TACKLING,
		BACKLINE,
		CAPTAIN,
		PEN_TAKER,
		L_FREEKICK,
		R_FREEKICK,
		L_CORNER,
		R_CORNER
	}

	public class TacticsOption
	{
		public byte value;

		public bool modded;

		public byte unmoddedValue;

		public TacticsOption(byte val)
		{
			value = val;
		}

		public void ModifyValue(byte newValue)
		{
			if (!modded)
			{
				unmoddedValue = value;
			}
			value = newValue;
			modded = true;
		}

		public void RevertValue()
		{
			if (modded)
			{
				value = unmoddedValue;
				unmoddedValue = 0;
				modded = false;
			}
		}
	}

	public class TacticsRole
	{
		public int value;

		public bool modded;

		public int unmoddedValue;

		public TacticsRole(int val)
		{
			value = val;
		}

		public void ModifyValue(int newValue)
		{
			if (!modded)
			{
				unmoddedValue = value;
			}
			value = newValue;
			modded = true;
		}

		public void RevertValue()
		{
			if (modded)
			{
				value = unmoddedValue;
				unmoddedValue = 0;
				modded = false;
			}
		}
	}

	public enum AttributeType
	{
		AERIAL_ABILITY,
		PACE,
		STRENGTH,
		AGGRESSION,
		COMPOSURE,
		CONCENTRATION,
		CREATIVITY,
		LEADERSHIP,
		MOVEMENT,
		POSITIONING,
		COMMAND_OF_AREA,
		DISTRIBUTION,
		HANDLING,
		ONE_ON_ONES,
		SHOT_STOPPING,
		FLAIR,
		MARKING,
		TACKLING,
		FINISHING,
		DRIBBLING,
		LONG_SHOTS,
		CROSSING,
		CONTROL,
		LONG_PASSING,
		PASSING,
		CORNERS,
		FREE_KICKS,
		PENALTIES,
		STAMINA,
		VOLLEYING,
		WORK_RATE,
		FITNESS,
		SAVING_PENALTIES
	}

	public class PlayerAttribute
	{
		public float value;

		public bool modded;

		public float unmoddedValue;

		public PlayerAttribute(float val)
		{
			value = val;
		}

		public void ModifyValue(float newValue)
		{
			if (!modded)
			{
				unmoddedValue = value;
			}
			value = newValue;
			modded = true;
		}

		public void RevertValue()
		{
			if (modded)
			{
				value = unmoddedValue;
				unmoddedValue = 0f;
				modded = false;
			}
		}
	}

	public class SePlayerInfo
	{
		public string foreName = "Bob";

		public string surName = "Charlton";

		public int pos;

		public int id;

		public int injType;

		public string foot = "b";

		public int rating = 85;

		public int number;

		public float height = 1.7f;

		public float weight = 90f;

		public PlayerAttribute aerialAbility = new PlayerAttribute(0.6f);

		public PlayerAttribute pace = new PlayerAttribute(0.6f);

		public PlayerAttribute strength = new PlayerAttribute(0.7f);

		public PlayerAttribute aggression = new PlayerAttribute(0.7f);

		public PlayerAttribute composure = new PlayerAttribute(0.5f);

		public PlayerAttribute concentration = new PlayerAttribute(0.7f);

		public PlayerAttribute creativity = new PlayerAttribute(0f);

		public PlayerAttribute leadership = new PlayerAttribute(0f);

		public PlayerAttribute movement = new PlayerAttribute(0f);

		public PlayerAttribute positioning = new PlayerAttribute(0.6f);

		public PlayerAttribute workRate = new PlayerAttribute(0f);

		public PlayerAttribute commandOfArea = new PlayerAttribute(0.5f);

		public PlayerAttribute distribution = new PlayerAttribute(0.2f);

		public PlayerAttribute handling = new PlayerAttribute(0.8f);

		public PlayerAttribute oneOnOnes = new PlayerAttribute(0.5f);

		public PlayerAttribute shotStopping = new PlayerAttribute(0.6f);

		public PlayerAttribute flair = new PlayerAttribute(0f);

		public PlayerAttribute marking = new PlayerAttribute(0.7f);

		public PlayerAttribute tackling = new PlayerAttribute(0.7f);

		public PlayerAttribute finishing = new PlayerAttribute(0.1f);

		public PlayerAttribute dribbling = new PlayerAttribute(0.7f);

		public PlayerAttribute longshots = new PlayerAttribute(0.7f);

		public PlayerAttribute crossing = new PlayerAttribute(0.7f);

		public PlayerAttribute control = new PlayerAttribute(0.7f);

		public PlayerAttribute longPassing = new PlayerAttribute(0.7f);

		public PlayerAttribute passing = new PlayerAttribute(0.7f);

		public PlayerAttribute corners = new PlayerAttribute(0f);

		public PlayerAttribute freekicks = new PlayerAttribute(0f);

		public PlayerAttribute penalties = new PlayerAttribute(0f);

		public PlayerAttribute stamina = new PlayerAttribute(0f);

		public PlayerAttribute volleying = new PlayerAttribute(0f);

		public PlayerAttribute workrate = new PlayerAttribute(0f);

		public PlayerAttribute fitness = new PlayerAttribute(100f);

		public PlayerAttribute savingPenalties = new PlayerAttribute(0f);

		public int skinColour;

		public int hairStyle;

		public int hairColour;

		public int facialHair;

		public bool longSleeves;

		public void RevertAttributeMods()
		{
			aerialAbility.RevertValue();
			pace.RevertValue();
			strength.RevertValue();
			aggression.RevertValue();
			composure.RevertValue();
			concentration.RevertValue();
			creativity.RevertValue();
			leadership.RevertValue();
			movement.RevertValue();
			positioning.RevertValue();
			workRate.RevertValue();
			commandOfArea.RevertValue();
			distribution.RevertValue();
			handling.RevertValue();
			oneOnOnes.RevertValue();
			shotStopping.RevertValue();
			flair.RevertValue();
			marking.RevertValue();
			tackling.RevertValue();
			finishing.RevertValue();
			dribbling.RevertValue();
			longshots.RevertValue();
			crossing.RevertValue();
			control.RevertValue();
			longPassing.RevertValue();
			passing.RevertValue();
			corners.RevertValue();
			freekicks.RevertValue();
			penalties.RevertValue();
			stamina.RevertValue();
			volleying.RevertValue();
			workrate.RevertValue();
			fitness.RevertValue();
			savingPenalties.RevertValue();
		}
	}

	public class SeFixtureInfoData
	{
		public int seed;

		public bool extraTime;

		public int historyLength;

		public string tournName;

		public string referee;

		public string stadium;

		public string date;

		public int capacity;

		public int attendance;

		public SePitchData pitch;

		public SePitchPosData pitchEntrance;

		public SeFixtureInfoTeamData home;

		public SeFixtureInfoTeamData away;

		public bool isLeg2;

		public int leg1HomeScore;

		public int leg1AwayScore;

		public bool penalties;
	}

	public SeFixtureInfoData fixtureInfo = new SeFixtureInfoData
	{
		seed = 3,
		extraTime = false,
		historyLength = 10,
		tournName = "Division 1",
		referee = "JohnSmith",
		stadium = "Deepdale",
		date = "Monday 7 Jan 2013",
		capacity = 22000,
		attendance = 16200,
		pitch = new SePitchData
		{
			size = new SePitchSizeData
			{
				x = 104f,
				y = 78f
			},
			friction = 0.06f
		},
		pitchEntrance = new SePitchPosData
		{
			x = 52f,
			y = -6f
		},
		home = new SeFixtureInfoTeamData
		{
			clubDataID = 61,
			name = "Preston North End",
			manager = "Joe Bloggs",
			isAI = true,
			kitID = "326",
			kitIDAway = "356",
			colour = "E90202",
			colourAway = "9505f0",
			numberColour = "FFFFFF",
			numberColourAway = "63D0FD",
			inst = new cInstructions(),
			formations = "442",
			formationID = 3,
			direction = 1,
			players = new SePlayerInfo[19]
			{
				null,
				new SePlayerInfo
				{
					surName = "redKeeper",
					pos = 1
				},
				new SePlayerInfo
				{
					surName = "red 2"
				},
				new SePlayerInfo
				{
					surName = "red 3"
				},
				new SePlayerInfo
				{
					surName = "red 4"
				},
				new SePlayerInfo
				{
					surName = "red 5"
				},
				new SePlayerInfo
				{
					surName = "red 6"
				},
				new SePlayerInfo
				{
					surName = "red 7"
				},
				new SePlayerInfo
				{
					surName = "red 8"
				},
				new SePlayerInfo
				{
					surName = "red 9"
				},
				new SePlayerInfo
				{
					surName = "red 10"
				},
				new SePlayerInfo
				{
					surName = "red 11"
				},
				new SePlayerInfo
				{
					surName = "red 12"
				},
				new SePlayerInfo
				{
					surName = "red 13"
				},
				new SePlayerInfo
				{
					surName = "red 14"
				},
				new SePlayerInfo
				{
					surName = "red 15"
				},
				new SePlayerInfo
				{
					surName = "red 16"
				},
				new SePlayerInfo
				{
					surName = "red 17"
				},
				new SePlayerInfo
				{
					surName = "red 18"
				}
			}
		},
		away = new SeFixtureInfoTeamData
		{
			clubDataID = 31,
			name = "Everton",
			manager = "David Moyes",
			isAI = true,
			kitID = "278",
			kitIDAway = "13",
			colour = "E90202",
			colourAway = "FEEC25",
			numberColour = "FFFFFF",
			numberColourAway = "2551B2",
			inst = new cInstructions(),
			formations = "442",
			formationID = 1,
			direction = 1,
			players = new SePlayerInfo[19]
			{
				null,
				new SePlayerInfo
				{
					surName = "blueKeeper"
				},
				new SePlayerInfo
				{
					surName = "blue 2"
				},
				new SePlayerInfo
				{
					surName = "blue 3"
				},
				new SePlayerInfo
				{
					surName = "blue 4"
				},
				new SePlayerInfo
				{
					surName = "blue 5"
				},
				new SePlayerInfo
				{
					surName = "blue 6"
				},
				new SePlayerInfo
				{
					surName = "blue 7"
				},
				new SePlayerInfo
				{
					surName = "blue 8"
				},
				new SePlayerInfo
				{
					surName = "blue 9"
				},
				new SePlayerInfo
				{
					surName = "blue 10"
				},
				new SePlayerInfo
				{
					surName = "blue 11"
				},
				new SePlayerInfo
				{
					surName = "blue 12"
				},
				new SePlayerInfo
				{
					surName = "blue 13"
				},
				new SePlayerInfo
				{
					surName = "blue 14"
				},
				new SePlayerInfo
				{
					surName = "blue 15"
				},
				new SePlayerInfo
				{
					surName = "blue 16"
				},
				new SePlayerInfo
				{
					surName = "blue 17"
				},
				new SePlayerInfo
				{
					surName = "blue 18"
				}
			}
		}
	};

	public static List<string> TACTICS_INST_NAME = new List<string>
	{
		"Width", "Mentality", "Tempo", "Creativity", "Passing Style", "Attacking Style", "Forwards", "Wingers", "Midfielders", "Pressing",
		"Tackling", "Back Line", "Captain", "Pen Taker", "Left FK Taker", "Right FK Taker", "Left Corner Taker", "Right Corner Taker"
	};

	public static List<string> ATTRIBUTE_NAMES = new List<string>
	{
		"Aerial Ability", "Pace", "Strength", "Aggression", "Composure", "Concentration", "Creativity", "Leadership", "Movement", "Positioning",
		"Command Of Area", "Distribution", "Handling", "One On Ones", "Shot Stopping", "Flair", "Marking", "Tackling", "Finishing", "Dribbling",
		"Long Shots", "Crossing", "Control", "Long Passing", "Passing", "Corners", "Free Kicks", "Penalties", "Stamina", "Volleying",
		"Work Rate", "Fitness", "Saving Penalties"
	};

	public Dictionary<int, SePlayerInfo> testAttributes = new Dictionary<int, SePlayerInfo>
	{
		{
			1,
			new SePlayerInfo
			{
				aerialAbility = new PlayerAttribute(0.7155418f),
				aggression = new PlayerAttribute(0.585662f),
				commandOfArea = new PlayerAttribute(0.7155418f),
				composure = new PlayerAttribute(0.649519f),
				concentration = new PlayerAttribute(0.7155418f),
				distribution = new PlayerAttribute(0.7155418f),
				fitness = new PlayerAttribute(100f),
				foot = "r",
				handling = new PlayerAttribute(0.7155418f),
				height = 1.91f,
				leadership = new PlayerAttribute(0.585662f),
				oneOnOnes = new PlayerAttribute(0.7155418f),
				pace = new PlayerAttribute(0.464758f),
				positioning = new PlayerAttribute(0.7155418f),
				rating = 89,
				savingPenalties = new PlayerAttribute(0.7155418f),
				shotStopping = new PlayerAttribute(0.85381496f),
				strength = new PlayerAttribute(0.585662f),
				weight = 84f
			}
		},
		{
			2,
			new SePlayerInfo
			{
				aerialAbility = new PlayerAttribute(0.92594546f),
				aggression = new PlayerAttribute(0.92594546f),
				composure = new PlayerAttribute(1f),
				concentration = new PlayerAttribute(0.92594546f),
				control = new PlayerAttribute(0.85381496f),
				corners = new PlayerAttribute(0.30186918f),
				creativity = new PlayerAttribute(0.30186918f),
				crossing = new PlayerAttribute(0.40789092f),
				dribbling = new PlayerAttribute(0.30186918f),
				finishing = new PlayerAttribute(0.30186918f),
				fitness = new PlayerAttribute(100f),
				flair = new PlayerAttribute(0.30186918f),
				foot = "r",
				freekicks = new PlayerAttribute(0.35355338f),
				height = 1.91f,
				leadership = new PlayerAttribute(0.7155418f),
				longPassing = new PlayerAttribute(0.585662f),
				longshots = new PlayerAttribute(0.40789092f),
				marking = new PlayerAttribute(1f),
				movement = new PlayerAttribute(0.30186918f),
				pace = new PlayerAttribute(0.7836613f),
				passing = new PlayerAttribute(0.7155418f),
				penalties = new PlayerAttribute(0.35355338f),
				positioning = new PlayerAttribute(1f),
				rating = 92,
				stamina = new PlayerAttribute(0.7836613f),
				strength = new PlayerAttribute(0.7836613f),
				tackling = new PlayerAttribute(0.92594546f),
				weight = 85f,
				workrate = new PlayerAttribute(0.7836613f)
			}
		},
		{
			3,
			new SePlayerInfo
			{
				aerialAbility = new PlayerAttribute(0.35355338f),
				aggression = new PlayerAttribute(0.7836613f),
				composure = new PlayerAttribute(0.7836613f),
				concentration = new PlayerAttribute(0.7836613f),
				control = new PlayerAttribute(1f),
				corners = new PlayerAttribute(0.40789092f),
				creativity = new PlayerAttribute(0.7836613f),
				crossing = new PlayerAttribute(0.7155418f),
				dribbling = new PlayerAttribute(1f),
				finishing = new PlayerAttribute(0.40789092f),
				fitness = new PlayerAttribute(100f),
				flair = new PlayerAttribute(0.30186918f),
				foot = "l",
				freekicks = new PlayerAttribute(0.464758f),
				height = 1.72f,
				leadership = new PlayerAttribute(0.7155418f),
				longPassing = new PlayerAttribute(0.649519f),
				longshots = new PlayerAttribute(0.5240468f),
				marking = new PlayerAttribute(0.85381496f),
				movement = new PlayerAttribute(1f),
				pace = new PlayerAttribute(0.7836613f),
				passing = new PlayerAttribute(0.7836613f),
				penalties = new PlayerAttribute(0.464758f),
				positioning = new PlayerAttribute(0.85381496f),
				rating = 93,
				stamina = new PlayerAttribute(0.7836613f),
				strength = new PlayerAttribute(0.7836613f),
				tackling = new PlayerAttribute(1f),
				weight = 73f,
				workrate = new PlayerAttribute(0.92594546f)
			}
		},
		{
			4,
			new SePlayerInfo
			{
				aerialAbility = new PlayerAttribute(1f),
				aggression = new PlayerAttribute(1f),
				composure = new PlayerAttribute(0.92594546f),
				concentration = new PlayerAttribute(1f),
				control = new PlayerAttribute(0.92594546f),
				corners = new PlayerAttribute(0.35355338f),
				creativity = new PlayerAttribute(0.30186918f),
				crossing = new PlayerAttribute(0.464758f),
				dribbling = new PlayerAttribute(0.35355338f),
				finishing = new PlayerAttribute(0.35355338f),
				fitness = new PlayerAttribute(100f),
				flair = new PlayerAttribute(0.30186918f),
				foot = "r",
				freekicks = new PlayerAttribute(0.40789092f),
				height = 1.83f,
				leadership = new PlayerAttribute(0.85381496f),
				longPassing = new PlayerAttribute(0.7155418f),
				longshots = new PlayerAttribute(0.464758f),
				marking = new PlayerAttribute(1f),
				movement = new PlayerAttribute(0.35355338f),
				pace = new PlayerAttribute(0.85381496f),
				passing = new PlayerAttribute(0.92594546f),
				penalties = new PlayerAttribute(0.40789092f),
				positioning = new PlayerAttribute(1f),
				rating = 96,
				stamina = new PlayerAttribute(1f),
				strength = new PlayerAttribute(0.85381496f),
				tackling = new PlayerAttribute(1f),
				weight = 81f,
				workrate = new PlayerAttribute(0.85381496f)
			}
		},
		{
			5,
			new SePlayerInfo
			{
				aerialAbility = new PlayerAttribute(0.92594546f),
				aggression = new PlayerAttribute(0.92594546f),
				composure = new PlayerAttribute(0.7836613f),
				concentration = new PlayerAttribute(0.92594546f),
				control = new PlayerAttribute(0.7836613f),
				corners = new PlayerAttribute(0.30186918f),
				creativity = new PlayerAttribute(0.30186918f),
				crossing = new PlayerAttribute(0.40789092f),
				dribbling = new PlayerAttribute(0.30186918f),
				finishing = new PlayerAttribute(0.30186918f),
				fitness = new PlayerAttribute(95f),
				flair = new PlayerAttribute(0.30186918f),
				foot = "r",
				freekicks = new PlayerAttribute(0.35355338f),
				height = 1.87f,
				leadership = new PlayerAttribute(0.7836613f),
				longPassing = new PlayerAttribute(0.585662f),
				longshots = new PlayerAttribute(0.40789092f),
				marking = new PlayerAttribute(1f),
				movement = new PlayerAttribute(0.30186918f),
				pace = new PlayerAttribute(0.7155418f),
				passing = new PlayerAttribute(0.7155418f),
				penalties = new PlayerAttribute(0.35355338f),
				positioning = new PlayerAttribute(0.85381496f),
				rating = 92,
				stamina = new PlayerAttribute(0.7836613f),
				strength = new PlayerAttribute(0.7836613f),
				tackling = new PlayerAttribute(1f),
				weight = 81f,
				workrate = new PlayerAttribute(0.85381496f)
			}
		},
		{
			6,
			new SePlayerInfo
			{
				aerialAbility = new PlayerAttribute(0.5240468f),
				aggression = new PlayerAttribute(0.5240468f),
				composure = new PlayerAttribute(1f),
				concentration = new PlayerAttribute(0.585662f),
				control = new PlayerAttribute(1f),
				corners = new PlayerAttribute(0.7155418f),
				creativity = new PlayerAttribute(1f),
				crossing = new PlayerAttribute(0.85381496f),
				dribbling = new PlayerAttribute(0.92594546f),
				finishing = new PlayerAttribute(0.85381496f),
				fitness = new PlayerAttribute(100f),
				flair = new PlayerAttribute(0.85381496f),
				foot = "b",
				freekicks = new PlayerAttribute(0.7155418f),
				height = 1.75f,
				leadership = new PlayerAttribute(0.7836613f),
				longPassing = new PlayerAttribute(1f),
				longshots = new PlayerAttribute(1f),
				marking = new PlayerAttribute(0.7155418f),
				movement = new PlayerAttribute(1f),
				pace = new PlayerAttribute(0.464758f),
				passing = new PlayerAttribute(1f),
				penalties = new PlayerAttribute(0.585662f),
				positioning = new PlayerAttribute(1f),
				rating = 96,
				stamina = new PlayerAttribute(0.85381496f),
				strength = new PlayerAttribute(0.5240468f),
				tackling = new PlayerAttribute(0.7155418f),
				weight = 67f,
				workrate = new PlayerAttribute(0.85381496f)
			}
		},
		{
			7,
			new SePlayerInfo
			{
				aerialAbility = new PlayerAttribute(0.35355338f),
				aggression = new PlayerAttribute(0.35355338f),
				composure = new PlayerAttribute(0.85381496f),
				concentration = new PlayerAttribute(0.35355338f),
				control = new PlayerAttribute(0.92594546f),
				corners = new PlayerAttribute(0.85381496f),
				creativity = new PlayerAttribute(0.7836613f),
				crossing = new PlayerAttribute(1f),
				dribbling = new PlayerAttribute(1f),
				finishing = new PlayerAttribute(0.85381496f),
				fitness = new PlayerAttribute(94f),
				flair = new PlayerAttribute(0.7836613f),
				foot = "l",
				freekicks = new PlayerAttribute(0.7836613f),
				height = 1.83f,
				leadership = new PlayerAttribute(0.7155418f),
				longPassing = new PlayerAttribute(0.7836613f),
				longshots = new PlayerAttribute(1f),
				marking = new PlayerAttribute(0.464758f),
				movement = new PlayerAttribute(0.92594546f),
				pace = new PlayerAttribute(0.92594546f),
				passing = new PlayerAttribute(0.7836613f),
				penalties = new PlayerAttribute(0.585662f),
				positioning = new PlayerAttribute(0.464758f),
				rating = 94,
				stamina = new PlayerAttribute(0.92594546f),
				strength = new PlayerAttribute(0.7836613f),
				tackling = new PlayerAttribute(0.464758f),
				weight = 74f,
				workrate = new PlayerAttribute(0.649519f)
			}
		},
		{
			8,
			new SePlayerInfo
			{
				aerialAbility = new PlayerAttribute(0.464758f),
				aggression = new PlayerAttribute(0.464758f),
				composure = new PlayerAttribute(1f),
				concentration = new PlayerAttribute(0.585662f),
				control = new PlayerAttribute(1f),
				corners = new PlayerAttribute(0.649519f),
				creativity = new PlayerAttribute(0.92594546f),
				crossing = new PlayerAttribute(0.85381496f),
				dribbling = new PlayerAttribute(0.7836613f),
				finishing = new PlayerAttribute(0.85381496f),
				fitness = new PlayerAttribute(100f),
				flair = new PlayerAttribute(0.585662f),
				foot = "b",
				freekicks = new PlayerAttribute(0.7836613f),
				height = 1.82f,
				leadership = new PlayerAttribute(0.7155418f),
				longPassing = new PlayerAttribute(1f),
				longshots = new PlayerAttribute(1f),
				marking = new PlayerAttribute(0.649519f),
				movement = new PlayerAttribute(0.92594546f),
				pace = new PlayerAttribute(0.464758f),
				passing = new PlayerAttribute(1f),
				penalties = new PlayerAttribute(0.585662f),
				positioning = new PlayerAttribute(0.92594546f),
				rating = 94,
				stamina = new PlayerAttribute(0.7836613f),
				strength = new PlayerAttribute(0.7836613f),
				tackling = new PlayerAttribute(0.649519f),
				weight = 78f,
				workrate = new PlayerAttribute(0.7836613f)
			}
		},
		{
			9,
			new SePlayerInfo
			{
				aerialAbility = new PlayerAttribute(1f),
				aggression = new PlayerAttribute(0.40789092f),
				composure = new PlayerAttribute(1f),
				concentration = new PlayerAttribute(0.40789092f),
				control = new PlayerAttribute(1f),
				corners = new PlayerAttribute(0.7836613f),
				creativity = new PlayerAttribute(0.92594546f),
				crossing = new PlayerAttribute(0.92594546f),
				dribbling = new PlayerAttribute(1f),
				finishing = new PlayerAttribute(1f),
				fitness = new PlayerAttribute(99f),
				flair = new PlayerAttribute(1f),
				foot = "b",
				freekicks = new PlayerAttribute(1f),
				height = 1.85f,
				leadership = new PlayerAttribute(0.92594546f),
				longPassing = new PlayerAttribute(0.649519f),
				longshots = new PlayerAttribute(1f),
				marking = new PlayerAttribute(0.2070628f),
				movement = new PlayerAttribute(1f),
				pace = new PlayerAttribute(1f),
				passing = new PlayerAttribute(0.92594546f),
				penalties = new PlayerAttribute(1f),
				positioning = new PlayerAttribute(0.2070628f),
				rating = 99,
				stamina = new PlayerAttribute(0.92594546f),
				strength = new PlayerAttribute(0.92594546f),
				tackling = new PlayerAttribute(0.2070628f),
				weight = 80f,
				workrate = new PlayerAttribute(0.7836613f)
			}
		},
		{
			10,
			new SePlayerInfo
			{
				aerialAbility = new PlayerAttribute(0.464758f),
				aggression = new PlayerAttribute(0.35355338f),
				composure = new PlayerAttribute(1f),
				concentration = new PlayerAttribute(0.464758f),
				control = new PlayerAttribute(1f),
				corners = new PlayerAttribute(0.649519f),
				creativity = new PlayerAttribute(0.92594546f),
				crossing = new PlayerAttribute(0.85381496f),
				dribbling = new PlayerAttribute(1f),
				finishing = new PlayerAttribute(0.85381496f),
				fitness = new PlayerAttribute(96f),
				flair = new PlayerAttribute(0.92594546f),
				foot = "r",
				freekicks = new PlayerAttribute(0.7836613f),
				height = 1.76f,
				leadership = new PlayerAttribute(0.7155418f),
				longPassing = new PlayerAttribute(1f),
				longshots = new PlayerAttribute(1f),
				marking = new PlayerAttribute(0.464758f),
				movement = new PlayerAttribute(1f),
				pace = new PlayerAttribute(0.7836613f),
				passing = new PlayerAttribute(1f),
				penalties = new PlayerAttribute(0.585662f),
				positioning = new PlayerAttribute(0.7836613f),
				rating = 94,
				stamina = new PlayerAttribute(0.585662f),
				strength = new PlayerAttribute(0.464758f),
				tackling = new PlayerAttribute(0.464758f),
				weight = 74f,
				workrate = new PlayerAttribute(0.585662f)
			}
		},
		{
			11,
			new SePlayerInfo
			{
				aerialAbility = new PlayerAttribute(0.464758f),
				aggression = new PlayerAttribute(0.35355338f),
				composure = new PlayerAttribute(0.85381496f),
				concentration = new PlayerAttribute(0.464758f),
				control = new PlayerAttribute(1f),
				corners = new PlayerAttribute(0.649519f),
				creativity = new PlayerAttribute(0.92594546f),
				crossing = new PlayerAttribute(1f),
				dribbling = new PlayerAttribute(1f),
				finishing = new PlayerAttribute(0.7836613f),
				fitness = new PlayerAttribute(100f),
				flair = new PlayerAttribute(0.92594546f),
				foot = "l",
				freekicks = new PlayerAttribute(0.92594546f),
				height = 1.8f,
				leadership = new PlayerAttribute(0.7155418f),
				longPassing = new PlayerAttribute(0.85381496f),
				longshots = new PlayerAttribute(0.92594546f),
				marking = new PlayerAttribute(0.40789092f),
				movement = new PlayerAttribute(0.92594546f),
				pace = new PlayerAttribute(0.7836613f),
				passing = new PlayerAttribute(0.92594546f),
				penalties = new PlayerAttribute(0.585662f),
				positioning = new PlayerAttribute(0.7155418f),
				rating = 93,
				stamina = new PlayerAttribute(0.585662f),
				strength = new PlayerAttribute(0.464758f),
				tackling = new PlayerAttribute(0.40789092f),
				weight = 75f,
				workrate = new PlayerAttribute(0.585662f)
			}
		},
		{
			101,
			new SePlayerInfo
			{
				aerialAbility = new PlayerAttribute(0.649519f),
				aggression = new PlayerAttribute(0.5240468f),
				commandOfArea = new PlayerAttribute(0.585662f),
				composure = new PlayerAttribute(0.464758f),
				concentration = new PlayerAttribute(0.649519f),
				distribution = new PlayerAttribute(0.585662f),
				fitness = new PlayerAttribute(100f),
				foot = "r",
				handling = new PlayerAttribute(0.585662f),
				height = 1.83f,
				leadership = new PlayerAttribute(0.5240468f),
				oneOnOnes = new PlayerAttribute(0.585662f),
				pace = new PlayerAttribute(0.35355338f),
				positioning = new PlayerAttribute(0.585662f),
				rating = 84,
				savingPenalties = new PlayerAttribute(0.585662f),
				shotStopping = new PlayerAttribute(0.585662f),
				strength = new PlayerAttribute(0.5240468f),
				weight = 75f
			}
		},
		{
			102,
			new SePlayerInfo
			{
				aerialAbility = new PlayerAttribute(0.7155418f),
				aggression = new PlayerAttribute(0.585662f),
				composure = new PlayerAttribute(0.464758f),
				concentration = new PlayerAttribute(0.5240468f),
				control = new PlayerAttribute(0.464758f),
				corners = new PlayerAttribute(0.40789092f),
				creativity = new PlayerAttribute(0.40789092f),
				crossing = new PlayerAttribute(0.40789092f),
				dribbling = new PlayerAttribute(0.40789092f),
				finishing = new PlayerAttribute(0.464758f),
				fitness = new PlayerAttribute(100f),
				flair = new PlayerAttribute(0.40789092f),
				foot = "r",
				freekicks = new PlayerAttribute(0.464758f),
				height = 1.75f,
				leadership = new PlayerAttribute(0.5240468f),
				longPassing = new PlayerAttribute(0.464758f),
				longshots = new PlayerAttribute(0.464758f),
				marking = new PlayerAttribute(0.40789092f),
				movement = new PlayerAttribute(0.464758f),
				pace = new PlayerAttribute(0.464758f),
				passing = new PlayerAttribute(0.5240468f),
				penalties = new PlayerAttribute(0.40789092f),
				positioning = new PlayerAttribute(0.464758f),
				rating = 83,
				stamina = new PlayerAttribute(0.7155418f),
				strength = new PlayerAttribute(0.40789092f),
				tackling = new PlayerAttribute(0.649519f),
				weight = 77f,
				workrate = new PlayerAttribute(0.649519f)
			}
		},
		{
			103,
			new SePlayerInfo
			{
				aerialAbility = new PlayerAttribute(0.585662f),
				aggression = new PlayerAttribute(0.7155418f),
				composure = new PlayerAttribute(0.464758f),
				concentration = new PlayerAttribute(0.7155418f),
				control = new PlayerAttribute(0.464758f),
				corners = new PlayerAttribute(0.2070628f),
				creativity = new PlayerAttribute(0.125f),
				crossing = new PlayerAttribute(0.2529822f),
				dribbling = new PlayerAttribute(0.2070628f),
				finishing = new PlayerAttribute(0.2070628f),
				fitness = new PlayerAttribute(97f),
				flair = new PlayerAttribute(0.125f),
				foot = "l",
				freekicks = new PlayerAttribute(0.16431677f),
				height = 1.85f,
				leadership = new PlayerAttribute(0.7155418f),
				longPassing = new PlayerAttribute(0.35355338f),
				longshots = new PlayerAttribute(0.2529822f),
				marking = new PlayerAttribute(0.5240468f),
				movement = new PlayerAttribute(0.2070628f),
				pace = new PlayerAttribute(0.30186918f),
				passing = new PlayerAttribute(0.40789092f),
				penalties = new PlayerAttribute(0.16431677f),
				positioning = new PlayerAttribute(0.5240468f),
				rating = 82,
				stamina = new PlayerAttribute(0.464758f),
				strength = new PlayerAttribute(0.5240468f),
				tackling = new PlayerAttribute(0.5240468f),
				weight = 76f,
				workrate = new PlayerAttribute(0.40789092f)
			}
		},
		{
			104,
			new SePlayerInfo
			{
				aerialAbility = new PlayerAttribute(0.585662f),
				aggression = new PlayerAttribute(0.649519f),
				composure = new PlayerAttribute(0.464758f),
				concentration = new PlayerAttribute(0.7155418f),
				control = new PlayerAttribute(0.464758f),
				corners = new PlayerAttribute(0.2070628f),
				creativity = new PlayerAttribute(0.2070628f),
				crossing = new PlayerAttribute(0.2529822f),
				dribbling = new PlayerAttribute(0.2070628f),
				finishing = new PlayerAttribute(0.2070628f),
				fitness = new PlayerAttribute(99f),
				flair = new PlayerAttribute(0.16431677f),
				foot = "r",
				freekicks = new PlayerAttribute(0.2070628f),
				height = 1.93f,
				leadership = new PlayerAttribute(0.5240468f),
				longPassing = new PlayerAttribute(0.40789092f),
				longshots = new PlayerAttribute(0.2529822f),
				marking = new PlayerAttribute(0.5240468f),
				movement = new PlayerAttribute(0.2529822f),
				pace = new PlayerAttribute(0.35355338f),
				passing = new PlayerAttribute(0.464758f),
				penalties = new PlayerAttribute(0.2070628f),
				positioning = new PlayerAttribute(0.5240468f),
				rating = 83,
				stamina = new PlayerAttribute(0.5240468f),
				strength = new PlayerAttribute(0.5240468f),
				tackling = new PlayerAttribute(0.5240468f),
				weight = 81f,
				workrate = new PlayerAttribute(0.464758f)
			}
		},
		{
			105,
			new SePlayerInfo
			{
				aerialAbility = new PlayerAttribute(0.585662f),
				aggression = new PlayerAttribute(0.649519f),
				composure = new PlayerAttribute(0.464758f),
				concentration = new PlayerAttribute(0.7155418f),
				control = new PlayerAttribute(0.464758f),
				corners = new PlayerAttribute(0.2070628f),
				creativity = new PlayerAttribute(0.2070628f),
				crossing = new PlayerAttribute(0.2529822f),
				dribbling = new PlayerAttribute(0.2070628f),
				finishing = new PlayerAttribute(0.2070628f),
				fitness = new PlayerAttribute(100f),
				flair = new PlayerAttribute(0.16431677f),
				foot = "r",
				freekicks = new PlayerAttribute(0.2070628f),
				height = 1.91f,
				leadership = new PlayerAttribute(0.5240468f),
				longPassing = new PlayerAttribute(0.40789092f),
				longshots = new PlayerAttribute(0.2529822f),
				marking = new PlayerAttribute(0.5240468f),
				movement = new PlayerAttribute(0.2529822f),
				pace = new PlayerAttribute(0.35355338f),
				passing = new PlayerAttribute(0.464758f),
				penalties = new PlayerAttribute(0.2070628f),
				positioning = new PlayerAttribute(0.5240468f),
				rating = 83,
				stamina = new PlayerAttribute(0.5240468f),
				strength = new PlayerAttribute(0.5240468f),
				tackling = new PlayerAttribute(0.5240468f),
				weight = 89f,
				workrate = new PlayerAttribute(0.464758f)
			}
		},
		{
			106,
			new SePlayerInfo
			{
				aerialAbility = new PlayerAttribute(0.464758f),
				aggression = new PlayerAttribute(0.649519f),
				composure = new PlayerAttribute(0.649519f),
				concentration = new PlayerAttribute(0.5240468f),
				control = new PlayerAttribute(0.585662f),
				corners = new PlayerAttribute(0.464758f),
				creativity = new PlayerAttribute(0.5240468f),
				crossing = new PlayerAttribute(0.585662f),
				dribbling = new PlayerAttribute(0.7836613f),
				finishing = new PlayerAttribute(0.649519f),
				fitness = new PlayerAttribute(97f),
				flair = new PlayerAttribute(0.464758f),
				foot = "r",
				freekicks = new PlayerAttribute(0.649519f),
				height = 1.83f,
				leadership = new PlayerAttribute(0.5240468f),
				longPassing = new PlayerAttribute(0.585662f),
				longshots = new PlayerAttribute(0.7836613f),
				marking = new PlayerAttribute(0.464758f),
				movement = new PlayerAttribute(0.649519f),
				pace = new PlayerAttribute(0.5240468f),
				passing = new PlayerAttribute(0.649519f),
				penalties = new PlayerAttribute(0.464758f),
				positioning = new PlayerAttribute(0.585662f),
				rating = 87,
				stamina = new PlayerAttribute(0.85381496f),
				strength = new PlayerAttribute(0.7155418f),
				tackling = new PlayerAttribute(0.585662f),
				weight = 80f,
				workrate = new PlayerAttribute(0.7155418f)
			}
		},
		{
			107,
			new SePlayerInfo
			{
				aerialAbility = new PlayerAttribute(0.2070628f),
				aggression = new PlayerAttribute(0.585662f),
				composure = new PlayerAttribute(0.464758f),
				concentration = new PlayerAttribute(0.585662f),
				control = new PlayerAttribute(0.464758f),
				corners = new PlayerAttribute(0.2529822f),
				creativity = new PlayerAttribute(0.35355338f),
				crossing = new PlayerAttribute(0.5240468f),
				dribbling = new PlayerAttribute(0.40789092f),
				finishing = new PlayerAttribute(0.30186918f),
				fitness = new PlayerAttribute(100f),
				flair = new PlayerAttribute(0.16431677f),
				foot = "r",
				freekicks = new PlayerAttribute(0.35355338f),
				height = 1.8f,
				leadership = new PlayerAttribute(0.5240468f),
				longPassing = new PlayerAttribute(0.40789092f),
				longshots = new PlayerAttribute(0.35355338f),
				marking = new PlayerAttribute(0.40789092f),
				movement = new PlayerAttribute(0.40789092f),
				pace = new PlayerAttribute(0.464758f),
				passing = new PlayerAttribute(0.40789092f),
				penalties = new PlayerAttribute(0.35355338f),
				positioning = new PlayerAttribute(0.40789092f),
				rating = 82,
				stamina = new PlayerAttribute(0.649519f),
				strength = new PlayerAttribute(0.40789092f),
				tackling = new PlayerAttribute(0.40789092f),
				weight = 83f,
				workrate = new PlayerAttribute(0.649519f)
			}
		},
		{
			108,
			new SePlayerInfo
			{
				aerialAbility = new PlayerAttribute(0.40789092f),
				aggression = new PlayerAttribute(0.649519f),
				composure = new PlayerAttribute(0.464758f),
				concentration = new PlayerAttribute(0.5240468f),
				control = new PlayerAttribute(0.464758f),
				corners = new PlayerAttribute(0.40789092f),
				creativity = new PlayerAttribute(0.464758f),
				crossing = new PlayerAttribute(0.464758f),
				dribbling = new PlayerAttribute(0.464758f),
				finishing = new PlayerAttribute(0.464758f),
				fitness = new PlayerAttribute(95f),
				flair = new PlayerAttribute(0.40789092f),
				foot = "r",
				freekicks = new PlayerAttribute(0.464758f),
				height = 1.79f,
				leadership = new PlayerAttribute(0.585662f),
				longPassing = new PlayerAttribute(0.464758f),
				longshots = new PlayerAttribute(0.5240468f),
				marking = new PlayerAttribute(0.40789092f),
				movement = new PlayerAttribute(0.5240468f),
				pace = new PlayerAttribute(0.464758f),
				passing = new PlayerAttribute(0.5240468f),
				penalties = new PlayerAttribute(0.40789092f),
				positioning = new PlayerAttribute(0.464758f),
				rating = 83,
				stamina = new PlayerAttribute(0.7155418f),
				strength = new PlayerAttribute(0.5240468f),
				tackling = new PlayerAttribute(0.464758f),
				weight = 76f,
				workrate = new PlayerAttribute(0.649519f)
			}
		},
		{
			109,
			new SePlayerInfo
			{
				aerialAbility = new PlayerAttribute(0.464758f),
				aggression = new PlayerAttribute(0.585662f),
				composure = new PlayerAttribute(0.5240468f),
				concentration = new PlayerAttribute(0.5240468f),
				control = new PlayerAttribute(0.5240468f),
				corners = new PlayerAttribute(0.40789092f),
				creativity = new PlayerAttribute(0.2529822f),
				crossing = new PlayerAttribute(0.30186918f),
				dribbling = new PlayerAttribute(0.35355338f),
				finishing = new PlayerAttribute(0.5240468f),
				fitness = new PlayerAttribute(100f),
				flair = new PlayerAttribute(0.35355338f),
				foot = "r",
				freekicks = new PlayerAttribute(0.35355338f),
				height = 1.85f,
				leadership = new PlayerAttribute(0.40789092f),
				longPassing = new PlayerAttribute(0.2529822f),
				longshots = new PlayerAttribute(0.40789092f),
				marking = new PlayerAttribute(0.08944272f),
				movement = new PlayerAttribute(0.5240468f),
				pace = new PlayerAttribute(0.5240468f),
				passing = new PlayerAttribute(0.35355338f),
				penalties = new PlayerAttribute(0.5240468f),
				positioning = new PlayerAttribute(0.08944272f),
				rating = 82,
				stamina = new PlayerAttribute(0.5240468f),
				strength = new PlayerAttribute(0.40789092f),
				tackling = new PlayerAttribute(0.08944272f),
				weight = 72f,
				workrate = new PlayerAttribute(0.5240468f)
			}
		},
		{
			110,
			new SePlayerInfo
			{
				aerialAbility = new PlayerAttribute(0.464758f),
				aggression = new PlayerAttribute(0.585662f),
				composure = new PlayerAttribute(0.464758f),
				concentration = new PlayerAttribute(0.40789092f),
				control = new PlayerAttribute(0.464758f),
				corners = new PlayerAttribute(0.35355338f),
				creativity = new PlayerAttribute(0.35355338f),
				crossing = new PlayerAttribute(0.40789092f),
				dribbling = new PlayerAttribute(0.40789092f),
				finishing = new PlayerAttribute(0.5240468f),
				fitness = new PlayerAttribute(100f),
				flair = new PlayerAttribute(0.35355338f),
				foot = "r",
				freekicks = new PlayerAttribute(0.40789092f),
				height = 1.7f,
				leadership = new PlayerAttribute(0.40789092f),
				longPassing = new PlayerAttribute(0.35355338f),
				longshots = new PlayerAttribute(0.40789092f),
				marking = new PlayerAttribute(0.08944272f),
				movement = new PlayerAttribute(0.5240468f),
				pace = new PlayerAttribute(0.5240468f),
				passing = new PlayerAttribute(0.40789092f),
				penalties = new PlayerAttribute(0.40789092f),
				positioning = new PlayerAttribute(0.08944272f),
				rating = 82,
				stamina = new PlayerAttribute(0.5240468f),
				strength = new PlayerAttribute(0.5240468f),
				tackling = new PlayerAttribute(0.08944272f),
				weight = 65f,
				workrate = new PlayerAttribute(0.7155418f)
			}
		},
		{
			111,
			new SePlayerInfo
			{
				aerialAbility = new PlayerAttribute(0.2070628f),
				aggression = new PlayerAttribute(0.585662f),
				composure = new PlayerAttribute(0.464758f),
				concentration = new PlayerAttribute(0.585662f),
				control = new PlayerAttribute(0.464758f),
				corners = new PlayerAttribute(0.2529822f),
				creativity = new PlayerAttribute(0.35355338f),
				crossing = new PlayerAttribute(0.5240468f),
				dribbling = new PlayerAttribute(0.40789092f),
				finishing = new PlayerAttribute(0.30186918f),
				fitness = new PlayerAttribute(98f),
				flair = new PlayerAttribute(0.16431677f),
				foot = "l",
				freekicks = new PlayerAttribute(0.35355338f),
				height = 1.81f,
				leadership = new PlayerAttribute(0.40789092f),
				longPassing = new PlayerAttribute(0.40789092f),
				longshots = new PlayerAttribute(0.35355338f),
				marking = new PlayerAttribute(0.40789092f),
				movement = new PlayerAttribute(0.40789092f),
				pace = new PlayerAttribute(0.585662f),
				passing = new PlayerAttribute(0.40789092f),
				penalties = new PlayerAttribute(0.35355338f),
				positioning = new PlayerAttribute(0.40789092f),
				rating = 82,
				stamina = new PlayerAttribute(0.7155418f),
				strength = new PlayerAttribute(0.40789092f),
				tackling = new PlayerAttribute(0.40789092f),
				weight = 78f,
				workrate = new PlayerAttribute(0.7155418f)
			}
		},
		{
			201,
			new SePlayerInfo
			{
				aggression = new PlayerAttribute(0.40789092f),
				composure = new PlayerAttribute(0.16431677f),
				concentration = new PlayerAttribute(0.464758f),
				pace = new PlayerAttribute(0.2070628f),
				strength = new PlayerAttribute(0.40789092f),
				aerialAbility = new PlayerAttribute(0.40789092f),
				leadership = new PlayerAttribute(0.2529822f),
				commandOfArea = new PlayerAttribute(0.16431677f),
				distribution = new PlayerAttribute(0.30186918f),
				handling = new PlayerAttribute(0.16431677f),
				oneOnOnes = new PlayerAttribute(0.16431677f),
				shotStopping = new PlayerAttribute(0.16431677f),
				positioning = new PlayerAttribute(0.16431677f),
				savingPenalties = new PlayerAttribute(0.16431677f),
				height = 1.82f,
				weight = 69f,
				foot = "r",
				rating = 69,
				fitness = new PlayerAttribute(100f)
			}
		},
		{
			202,
			new SePlayerInfo
			{
				aggression = new PlayerAttribute(0.585662f),
				composure = new PlayerAttribute(0.2070628f),
				concentration = new PlayerAttribute(0.585662f),
				pace = new PlayerAttribute(0.40789092f),
				strength = new PlayerAttribute(0.2529822f),
				aerialAbility = new PlayerAttribute(0.08944272f),
				flair = new PlayerAttribute(0.031622775f),
				leadership = new PlayerAttribute(0.2529822f),
				marking = new PlayerAttribute(0.2070628f),
				tackling = new PlayerAttribute(0.2070628f),
				positioning = new PlayerAttribute(0.2070628f),
				creativity = new PlayerAttribute(0.05809475f),
				movement = new PlayerAttribute(0.16431677f),
				finishing = new PlayerAttribute(0.125f),
				dribbling = new PlayerAttribute(0.125f),
				longshots = new PlayerAttribute(0.125f),
				crossing = new PlayerAttribute(0.16431677f),
				control = new PlayerAttribute(0.2070628f),
				longPassing = new PlayerAttribute(0.16431677f),
				passing = new PlayerAttribute(0.16431677f),
				corners = new PlayerAttribute(0.125f),
				freekicks = new PlayerAttribute(0.125f),
				penalties = new PlayerAttribute(0.125f),
				stamina = new PlayerAttribute(0.464758f),
				workrate = new PlayerAttribute(0.585662f),
				height = 1.73f,
				weight = 65f,
				foot = "r",
				rating = 71,
				fitness = new PlayerAttribute(100f)
			}
		},
		{
			203,
			new SePlayerInfo
			{
				aggression = new PlayerAttribute(0.585662f),
				composure = new PlayerAttribute(0.16431677f),
				concentration = new PlayerAttribute(0.585662f),
				pace = new PlayerAttribute(0.40789092f),
				strength = new PlayerAttribute(0.30186918f),
				aerialAbility = new PlayerAttribute(0.08944272f),
				flair = new PlayerAttribute(0.01118034f),
				leadership = new PlayerAttribute(0.16431677f),
				marking = new PlayerAttribute(0.2070628f),
				tackling = new PlayerAttribute(0.2070628f),
				positioning = new PlayerAttribute(0.2070628f),
				creativity = new PlayerAttribute(0.031622775f),
				movement = new PlayerAttribute(0.125f),
				finishing = new PlayerAttribute(0.08944272f),
				dribbling = new PlayerAttribute(0.125f),
				longshots = new PlayerAttribute(0.125f),
				crossing = new PlayerAttribute(0.16431677f),
				control = new PlayerAttribute(0.16431677f),
				longPassing = new PlayerAttribute(0.125f),
				passing = new PlayerAttribute(0.16431677f),
				corners = new PlayerAttribute(0.08944272f),
				freekicks = new PlayerAttribute(0.08944272f),
				penalties = new PlayerAttribute(0.08944272f),
				stamina = new PlayerAttribute(0.464758f),
				workrate = new PlayerAttribute(0.585662f),
				height = 1.88f,
				weight = 83f,
				foot = "l",
				rating = 70,
				fitness = new PlayerAttribute(100f)
			}
		},
		{
			204,
			new SePlayerInfo
			{
				aggression = new PlayerAttribute(0.649519f),
				composure = new PlayerAttribute(0.2070628f),
				concentration = new PlayerAttribute(0.649519f),
				pace = new PlayerAttribute(0.2529822f),
				strength = new PlayerAttribute(0.464758f),
				aerialAbility = new PlayerAttribute(0.464758f),
				flair = new PlayerAttribute(0.031622775f),
				leadership = new PlayerAttribute(0.35355338f),
				marking = new PlayerAttribute(0.2529822f),
				tackling = new PlayerAttribute(0.2529822f),
				positioning = new PlayerAttribute(0.2529822f),
				creativity = new PlayerAttribute(0.05809475f),
				movement = new PlayerAttribute(0.125f),
				finishing = new PlayerAttribute(0.08944272f),
				dribbling = new PlayerAttribute(0.08944272f),
				longshots = new PlayerAttribute(0.125f),
				crossing = new PlayerAttribute(0.125f),
				control = new PlayerAttribute(0.2070628f),
				longPassing = new PlayerAttribute(0.16431677f),
				passing = new PlayerAttribute(0.2070628f),
				corners = new PlayerAttribute(0.08944272f),
				freekicks = new PlayerAttribute(0.05809475f),
				penalties = new PlayerAttribute(0.05809475f),
				stamina = new PlayerAttribute(0.40789092f),
				workrate = new PlayerAttribute(0.35355338f),
				height = 1.83f,
				weight = 79f,
				foot = "r",
				rating = 72,
				fitness = new PlayerAttribute(100f)
			}
		},
		{
			205,
			new SePlayerInfo
			{
				aggression = new PlayerAttribute(0.7836613f),
				composure = new PlayerAttribute(0.16431677f),
				concentration = new PlayerAttribute(0.585662f),
				pace = new PlayerAttribute(0.2070628f),
				strength = new PlayerAttribute(0.464758f),
				aerialAbility = new PlayerAttribute(0.40789092f),
				flair = new PlayerAttribute(0.01118034f),
				leadership = new PlayerAttribute(0.30186918f),
				marking = new PlayerAttribute(0.2070628f),
				tackling = new PlayerAttribute(0.2070628f),
				positioning = new PlayerAttribute(0.2070628f),
				creativity = new PlayerAttribute(0.01118034f),
				movement = new PlayerAttribute(0.08944272f),
				finishing = new PlayerAttribute(0.08944272f),
				dribbling = new PlayerAttribute(0.08944272f),
				longshots = new PlayerAttribute(0.08944272f),
				crossing = new PlayerAttribute(0.08944272f),
				control = new PlayerAttribute(0.16431677f),
				longPassing = new PlayerAttribute(0.125f),
				passing = new PlayerAttribute(0.16431677f),
				corners = new PlayerAttribute(0.08944272f),
				freekicks = new PlayerAttribute(0.031622775f),
				penalties = new PlayerAttribute(0.031622775f),
				stamina = new PlayerAttribute(0.35355338f),
				workrate = new PlayerAttribute(0.35355338f),
				height = 1.87f,
				weight = 78f,
				foot = "r",
				rating = 70,
				fitness = new PlayerAttribute(97f)
			}
		},
		{
			206,
			new SePlayerInfo
			{
				aggression = new PlayerAttribute(0.7836613f),
				composure = new PlayerAttribute(0.30186918f),
				concentration = new PlayerAttribute(0.649519f),
				pace = new PlayerAttribute(0.464758f),
				strength = new PlayerAttribute(0.464758f),
				aerialAbility = new PlayerAttribute(0.125f),
				flair = new PlayerAttribute(0.05809475f),
				leadership = new PlayerAttribute(0.2529822f),
				marking = new PlayerAttribute(0.30186918f),
				tackling = new PlayerAttribute(0.30186918f),
				positioning = new PlayerAttribute(0.30186918f),
				creativity = new PlayerAttribute(0.16431677f),
				movement = new PlayerAttribute(0.2070628f),
				finishing = new PlayerAttribute(0.2070628f),
				dribbling = new PlayerAttribute(0.2070628f),
				longshots = new PlayerAttribute(0.2070628f),
				crossing = new PlayerAttribute(0.2070628f),
				control = new PlayerAttribute(0.30186918f),
				longPassing = new PlayerAttribute(0.2070628f),
				passing = new PlayerAttribute(0.2529822f),
				corners = new PlayerAttribute(0.16431677f),
				freekicks = new PlayerAttribute(0.2529822f),
				penalties = new PlayerAttribute(0.16431677f),
				stamina = new PlayerAttribute(0.649519f),
				workrate = new PlayerAttribute(0.649519f),
				height = 1.76f,
				weight = 76f,
				foot = "b",
				rating = 75,
				fitness = new PlayerAttribute(100f)
			}
		},
		{
			207,
			new SePlayerInfo
			{
				aggression = new PlayerAttribute(0.585662f),
				composure = new PlayerAttribute(0.2070628f),
				concentration = new PlayerAttribute(0.5240468f),
				pace = new PlayerAttribute(0.5240468f),
				strength = new PlayerAttribute(0.35355338f),
				aerialAbility = new PlayerAttribute(0.125f),
				flair = new PlayerAttribute(0.05809475f),
				leadership = new PlayerAttribute(0.16431677f),
				marking = new PlayerAttribute(0.2070628f),
				tackling = new PlayerAttribute(0.2070628f),
				positioning = new PlayerAttribute(0.2070628f),
				creativity = new PlayerAttribute(0.2070628f),
				movement = new PlayerAttribute(0.2070628f),
				finishing = new PlayerAttribute(0.16431677f),
				dribbling = new PlayerAttribute(0.2070628f),
				longshots = new PlayerAttribute(0.16431677f),
				crossing = new PlayerAttribute(0.2529822f),
				control = new PlayerAttribute(0.2070628f),
				longPassing = new PlayerAttribute(0.2070628f),
				passing = new PlayerAttribute(0.2070628f),
				corners = new PlayerAttribute(0.125f),
				freekicks = new PlayerAttribute(0.2070628f),
				penalties = new PlayerAttribute(0.2070628f),
				stamina = new PlayerAttribute(0.649519f),
				workrate = new PlayerAttribute(0.649519f),
				height = 1.78f,
				weight = 62f,
				foot = "r",
				rating = 72,
				fitness = new PlayerAttribute(100f)
			}
		},
		{
			208,
			new SePlayerInfo
			{
				aggression = new PlayerAttribute(0.585662f),
				composure = new PlayerAttribute(0.2070628f),
				concentration = new PlayerAttribute(0.40789092f),
				pace = new PlayerAttribute(0.40789092f),
				strength = new PlayerAttribute(0.30186918f),
				aerialAbility = new PlayerAttribute(0.2529822f),
				flair = new PlayerAttribute(0.2070628f),
				leadership = new PlayerAttribute(0.2070628f),
				marking = new PlayerAttribute(0.16431677f),
				tackling = new PlayerAttribute(0.16431677f),
				positioning = new PlayerAttribute(0.16431677f),
				creativity = new PlayerAttribute(0.2070628f),
				movement = new PlayerAttribute(0.16431677f),
				finishing = new PlayerAttribute(0.16431677f),
				dribbling = new PlayerAttribute(0.16431677f),
				longshots = new PlayerAttribute(0.16431677f),
				crossing = new PlayerAttribute(0.16431677f),
				control = new PlayerAttribute(0.2070628f),
				longPassing = new PlayerAttribute(0.16431677f),
				passing = new PlayerAttribute(0.2070628f),
				corners = new PlayerAttribute(0.16431677f),
				freekicks = new PlayerAttribute(0.2529822f),
				penalties = new PlayerAttribute(0.2070628f),
				stamina = new PlayerAttribute(0.585662f),
				workrate = new PlayerAttribute(0.585662f),
				height = 1.75f,
				weight = 70f,
				foot = "l",
				rating = 71,
				fitness = new PlayerAttribute(99f)
			}
		},
		{
			209,
			new SePlayerInfo
			{
				aggression = new PlayerAttribute(0.649519f),
				composure = new PlayerAttribute(0.2070628f),
				concentration = new PlayerAttribute(0.464758f),
				pace = new PlayerAttribute(0.35355338f),
				strength = new PlayerAttribute(0.5240468f),
				aerialAbility = new PlayerAttribute(0.464758f),
				flair = new PlayerAttribute(0.2070628f),
				leadership = new PlayerAttribute(0.2070628f),
				marking = new PlayerAttribute(0.08944272f),
				tackling = new PlayerAttribute(0.08944272f),
				positioning = new PlayerAttribute(0.08944272f),
				creativity = new PlayerAttribute(0.125f),
				movement = new PlayerAttribute(0.2070628f),
				finishing = new PlayerAttribute(0.2529822f),
				dribbling = new PlayerAttribute(0.16431677f),
				longshots = new PlayerAttribute(0.2070628f),
				crossing = new PlayerAttribute(0.16431677f),
				control = new PlayerAttribute(0.2070628f),
				longPassing = new PlayerAttribute(0.125f),
				passing = new PlayerAttribute(0.16431677f),
				corners = new PlayerAttribute(0.16431677f),
				freekicks = new PlayerAttribute(0.2070628f),
				penalties = new PlayerAttribute(0.2529822f),
				stamina = new PlayerAttribute(0.5240468f),
				workrate = new PlayerAttribute(0.649519f),
				height = 1.76f,
				weight = 70f,
				foot = "r",
				rating = 72,
				fitness = new PlayerAttribute(100f)
			}
		},
		{
			210,
			new SePlayerInfo
			{
				aggression = new PlayerAttribute(0.649519f),
				composure = new PlayerAttribute(0.2070628f),
				concentration = new PlayerAttribute(0.40789092f),
				pace = new PlayerAttribute(0.30186918f),
				strength = new PlayerAttribute(0.464758f),
				aerialAbility = new PlayerAttribute(0.40789092f),
				flair = new PlayerAttribute(0.2070628f),
				leadership = new PlayerAttribute(0.2070628f),
				marking = new PlayerAttribute(0.05809475f),
				tackling = new PlayerAttribute(0.05809475f),
				positioning = new PlayerAttribute(0.05809475f),
				creativity = new PlayerAttribute(0.125f),
				movement = new PlayerAttribute(0.16431677f),
				finishing = new PlayerAttribute(0.2070628f),
				dribbling = new PlayerAttribute(0.16431677f),
				longshots = new PlayerAttribute(0.16431677f),
				crossing = new PlayerAttribute(0.125f),
				control = new PlayerAttribute(0.2070628f),
				longPassing = new PlayerAttribute(0.125f),
				passing = new PlayerAttribute(0.16431677f),
				corners = new PlayerAttribute(0.16431677f),
				freekicks = new PlayerAttribute(0.2070628f),
				penalties = new PlayerAttribute(0.2529822f),
				stamina = new PlayerAttribute(0.464758f),
				workrate = new PlayerAttribute(0.585662f),
				height = 1.87f,
				weight = 76f,
				foot = "r",
				rating = 71,
				fitness = new PlayerAttribute(97f)
			}
		},
		{
			211,
			new SePlayerInfo
			{
				aggression = new PlayerAttribute(0.585662f),
				composure = new PlayerAttribute(0.01118034f),
				concentration = new PlayerAttribute(0.35355338f),
				pace = new PlayerAttribute(0.35355338f),
				strength = new PlayerAttribute(0.2070628f),
				aerialAbility = new PlayerAttribute(0.16431677f),
				flair = new PlayerAttribute(0.08944272f),
				leadership = new PlayerAttribute(0.031622775f),
				marking = new PlayerAttribute(0.01118034f),
				tackling = new PlayerAttribute(0.01118034f),
				positioning = new PlayerAttribute(0.01118034f),
				creativity = new PlayerAttribute(0.08944272f),
				movement = new PlayerAttribute(0.01118034f),
				finishing = new PlayerAttribute(0.01118034f),
				dribbling = new PlayerAttribute(0.01118034f),
				longshots = new PlayerAttribute(0.01118034f),
				crossing = new PlayerAttribute(0.01118034f),
				control = new PlayerAttribute(0.01118034f),
				longPassing = new PlayerAttribute(0.01118034f),
				passing = new PlayerAttribute(0.031622775f),
				corners = new PlayerAttribute(0.01118034f),
				freekicks = new PlayerAttribute(0.125f),
				penalties = new PlayerAttribute(0.08944272f),
				stamina = new PlayerAttribute(0.5240468f),
				workrate = new PlayerAttribute(0.5240468f),
				height = 1.75f,
				weight = 70f,
				foot = "r",
				rating = 60,
				fitness = new PlayerAttribute(97f)
			}
		}
	};
}
