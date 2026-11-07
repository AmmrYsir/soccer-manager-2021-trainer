using System.Collections.Generic;

public class cSoccerEngine
{
	public enum specialShotTypes
	{
		NULL,
		OPEN_PLAY,
		FREE_KICK,
		PEN,
		CORNER,
		KICKOFF
	}

	public enum celebTypes
	{
		KNEE_SLIDE,
		CHEST_SLIDE,
		ARMS_DOWN,
		ARMS_OUT,
		HANDS_OUT,
		POINT_TO_SKY,
		CAN_YOU_HEAR_ME,
		AEROPLANE,
		HEART,
		HANDS_BEHIND_HEAD,
		SHUSH,
		TELEPHONE,
		SUCK_THUMB
	}

	public enum flourishTypes
	{
		CHEERING,
		CLAPPING,
		FIST_PUMP,
		FLEX,
		NUMBER1,
		KISS_FINGER,
		MEXICAN_WAVE
	}

	public enum backgroundFlourishTypes
	{
		ARMS_UP,
		ARMS_OUT,
		PUMP,
		FIST_PUMP
	}

	public enum shotTypes
	{
		NULL,
		KICK,
		PLACED,
		DRIVE,
		HEADER,
		CHIP,
		VOLLEY,
		FLYING_VOLLEY,
		OVERHEAD_KICK,
		BACKHEEL,
		DIVING_HEADER,
		NONE
	}

	public enum goalTypes
	{
		OPEN_PLAY = 0,
		PENALTY = 1,
		OWN_GOAL = 4
	}

	public enum runTypes
	{
		SPEED = 1,
		TIME
	}

	public enum kickPlacements
	{
		TO_FEET = 1,
		FRONT_GOAL,
		FRONT_GOALLINE,
		FRONT_DIRECTION,
		FRONT_POINT,
		SHOT
	}

	public enum shotAimWidth
	{
		LEFT,
		MIDDLE,
		RIGHT
	}

	public enum shotAimHeight
	{
		LOW,
		MIDDLE,
		HIGH
	}

	public enum saveMethods
	{
		NULL,
		NO_REACT,
		EASY_MOVE_TO_POSITION,
		DIVE,
		RUN_AND_DIVE,
		NO_REACT_BODY
	}

	public static cSoccerEngine instance = null;

	public SeController seController;

	public const float pitchLength = 104f;

	public const float pitchWidth = 78f;

	public const float pitchHalfLength = 52f;

	public const float pitchHalfWidth = 39f;

	public const float pitchThirdLength = 34.666668f;

	public const float pitchQuarterLength = 26f;

	public const float pitchQuarterWidth = 19.5f;

	public const float goalDepth = 1.8f;

	public const float goalY = 7.32f;

	public const float goalZ = 2.44f;

	public const float linesmenOffset = 1.5f;

	public cPlaces places = new cPlaces();

	public cMatch match;

	public cMatchStats matchStats;

	public cReferee referee;

	public cMatchSetup matchSetup;

	public cInjuries injuries;

	public cRand rand;

	public static Dictionary<string, float> skillSkews = new Dictionary<string, float>
	{
		{ "aerialAbility", 1.5f },
		{ "aggression", 1.5f },
		{ "composure", 1.5f },
		{ "concentration", 1.5f },
		{ "control", 1.5f },
		{ "corners", 1.5f },
		{ "creativity", 1.5f },
		{ "crossing", 1.5f },
		{ "dribbling", 1.5f },
		{ "finishing", 1.5f },
		{ "flair", 1.5f },
		{ "freekicks", 1.5f },
		{ "leadership", 1.5f },
		{ "longPassing", 1.5f },
		{ "longshots", 1.5f },
		{ "marking", 1.5f },
		{ "movement", 1.5f },
		{ "pace", 1.5f },
		{ "passing", 1.5f },
		{ "penalties", 1.5f },
		{ "positioning", 1.5f },
		{ "stamina", 1.5f },
		{ "strength", 1.5f },
		{ "tackling", 1.5f },
		{ "workrate", 1.5f },
		{ "commandOfArea", 1.5f },
		{ "distribution", 1.5f },
		{ "handling", 1.5f },
		{ "oneOnOnes", 1.5f },
		{ "savingPenalties", 1.5f },
		{ "shotStopping", 1.5f }
	};

	private const float KNEE_SLIDE_CHANCE = 1f;

	private const float CHEST_SLIDE_CHANCE = 1f;

	private const float ARMS_OUT_CHANCE = 1f;

	private const float HANDS_OUT_CHANCE = 1f;

	private const float POINT_TO_SKY_CHANCE = 1f;

	private const float CAN_YOU_HEAR_ME_CHANCE = 0.3f;

	private const float AEROPLANE_CHANCE = 1f;

	private const float ARMS_DOWN_CHANCE = 1f;

	private const float HEART_CHANCE = 0.5f;

	private const float HANDS_BEHIND_HEAD_CHANCE = 0.7f;

	private const float SHUSH_CHANCE = 0.3f;

	private const float TELEPHONE_CHANCE = 0.5f;

	private const float SUCK_THUMB_CHANCE = 0.5f;

	public Dictionary<celebTypes, float> celebrationChances = new Dictionary<celebTypes, float>
	{
		{
			celebTypes.KNEE_SLIDE,
			1f
		},
		{
			celebTypes.CHEST_SLIDE,
			1f
		},
		{
			celebTypes.ARMS_DOWN,
			1f
		},
		{
			celebTypes.ARMS_OUT,
			1f
		},
		{
			celebTypes.HANDS_OUT,
			1f
		},
		{
			celebTypes.POINT_TO_SKY,
			1f
		},
		{
			celebTypes.CAN_YOU_HEAR_ME,
			0.3f
		},
		{
			celebTypes.AEROPLANE,
			1f
		},
		{
			celebTypes.HEART,
			0.5f
		},
		{
			celebTypes.HANDS_BEHIND_HEAD,
			0.7f
		},
		{
			celebTypes.SHUSH,
			0.3f
		},
		{
			celebTypes.TELEPHONE,
			0.5f
		},
		{
			celebTypes.SUCK_THUMB,
			0.5f
		}
	};

	private const float TOTAL_CELEBRATION_CHANCE = 9.8f;

	private const float FLOURISH_CHEERING_CHANCE = 1f;

	private const float FLOURISH_CLAPPING_CHANCE = 1f;

	private const float FLOURISH_FIST_PUMP_CHANCE = 1f;

	private const float FLOURISH_FLEX_CHANCE = 1f;

	private const float FLOURISH_NUMBER1_CHANCE = 1f;

	private const float FLOURISH_KISS_FINGER_CHANCE = 1f;

	private const float FLOURISH_MEXICAN_WAVE_CHANCE = 1f;

	public Dictionary<flourishTypes, float> flourishChances = new Dictionary<flourishTypes, float>
	{
		{
			flourishTypes.CHEERING,
			1f
		},
		{
			flourishTypes.CLAPPING,
			1f
		},
		{
			flourishTypes.FIST_PUMP,
			1f
		},
		{
			flourishTypes.FLEX,
			1f
		},
		{
			flourishTypes.NUMBER1,
			1f
		},
		{
			flourishTypes.KISS_FINGER,
			1f
		},
		{
			flourishTypes.MEXICAN_WAVE,
			1f
		}
	};

	private const float TOTAL_FLOURISH_CHANCE = 7f;

	private const float BACKGROUND_FLOURISH_ARMS_UP_CHANCE = 1f;

	private const float BACKGROUND_FLOURISH_ARMS_OUT_CHANCE = 1f;

	private const float BACKGROUND_FLOURISH_PUMP_CHANCE = 1f;

	private const float BACKGROUND_FLOURISH_FIST_PUMP_CHANCE = 1f;

	public Dictionary<backgroundFlourishTypes, float> backGroundFlourishChances = new Dictionary<backgroundFlourishTypes, float>
	{
		{
			backgroundFlourishTypes.ARMS_UP,
			1f
		},
		{
			backgroundFlourishTypes.ARMS_OUT,
			1f
		},
		{
			backgroundFlourishTypes.PUMP,
			1f
		},
		{
			backgroundFlourishTypes.FIST_PUMP,
			1f
		}
	};

	private const float TOTAL_BACKGROUND_FLOURISH_CHANCE = 4f;

	public cSoccerEngine(int seed, SeController seController)
	{
		instance = this;
		this.seController = seController;
		rand = new cRand(seed);
		matchSetup = new cMatchSetup();
		matchStats = new cMatchStats();
		injuries = new cInjuries();
		injuries.soccerEngine = this;
		match = new cMatch();
		createPlaces();
		match.init(matchSetup, this);
	}

	public celebTypes GetRandomCelebration()
	{
		float num = 0f;
		float num2 = rand.Float() * 9.8f;
		foreach (KeyValuePair<celebTypes, float> celebrationChance in celebrationChances)
		{
			num += celebrationChance.Value;
			if (num2 < num)
			{
				return celebrationChance.Key;
			}
		}
		return celebTypes.ARMS_OUT;
	}

	public flourishTypes GetRandomFlourish()
	{
		float num = 0f;
		float num2 = rand.Float() * 7f;
		foreach (KeyValuePair<flourishTypes, float> flourishChance in flourishChances)
		{
			num += flourishChance.Value;
			if (num2 < num)
			{
				return flourishChance.Key;
			}
		}
		return flourishTypes.CHEERING;
	}

	public backgroundFlourishTypes GetRandomBackgroundFlourish()
	{
		float num = 0f;
		float num2 = rand.Float() * 4f;
		foreach (KeyValuePair<backgroundFlourishTypes, float> backGroundFlourishChance in backGroundFlourishChances)
		{
			num += backGroundFlourishChance.Value;
			if (num2 < num)
			{
				return backGroundFlourishChance.Key;
			}
		}
		return backgroundFlourishTypes.ARMS_UP;
	}

	public string otherTeam(string homeAway)
	{
		if (homeAway == "home")
		{
			return "away";
		}
		return "home";
	}

	public bool inPenaltyArea(v3d pos, int goalDirection)
	{
		cRect cRect2 = places.penaltyArea[1];
		v3d v3d2 = v3d.pitchPos(goalDirection, pos, makeNew: true);
		bool result = false;
		if (v3d2.vx < cRect2.bottomRight.vx && v3d2.vx > cRect2.topLeft.vx && v3d2.vy < cRect2.bottomRight.vy && v3d2.vy > cRect2.topLeft.vy)
		{
			result = true;
		}
		return result;
	}

	public bool isOnPitch(v3d pos, float delta = 0f)
	{
		float num = 52f - delta;
		float num2 = 39f - delta;
		if (pos.vx > 0f - num && pos.vy > 0f - num2 && pos.vx < num)
		{
			return pos.vy < num2;
		}
		return false;
	}

	public void createPlaces()
	{
		float num = 78f;
		cMatchSetup.cDimensions dimensions = matchSetup.dimensions;
		float num2 = 104f / 2f;
		float num3 = num / 2f;
		places.topCorner = new v3d[2]
		{
			new v3d(0f - num2, 0f - num3),
			new v3d(num2, 0f - num3)
		};
		places.bottomCorner = new v3d[2]
		{
			new v3d(0f - num2, num3),
			new v3d(num2, num3)
		};
		places.corner = new v3d(num2, num3);
		places.goal = new v3d[2]
		{
			new v3d(0f - num2),
			new v3d(num2)
		};
		places.penaltySpot = new v3d[2]
		{
			new v3d(dimensions.penaltyspot - num2),
			new v3d(num2 - dimensions.penaltyspot)
		};
		places.penaltyTargetDefault = new v3d[2]
		{
			new v3d(0f - num2 - dimensions.goal.vz / 2f),
			new v3d(num2 + dimensions.goal.vz / 2f)
		};
		places.penaltyArea = new cRect[2]
		{
			new cRect(new v3d(dimensions.penaltyarea.x - num2, dimensions.penaltyarea.y / 2f), new v3d(0f - num2, (0f - dimensions.penaltyarea.y) / 2f)),
			new cRect(new v3d(num2, dimensions.penaltyarea.y / 2f), new v3d(num2 - dimensions.penaltyarea.x, (0f - dimensions.penaltyarea.y) / 2f))
		};
		places.frontPost = new v3d[2]
		{
			new v3d(0f - num2, (0f - dimensions.goal.vy) / 2f),
			new v3d(num2, (0f - dimensions.goal.vy) / 2f)
		};
		places.backPost = new v3d[2]
		{
			new v3d(0f - num2, dimensions.goal.vy / 2f),
			new v3d(num2, dimensions.goal.vy / 2f)
		};
		places.tunnel = dimensions.tunnel;
		places.tunnelEnd = v3d.clone(places.tunnel);
		places.tunnelEnd.vy -= 15f;
		places.subEntrance = new v3d[2]
		{
			new v3d(0f, 0f - num3),
			new v3d(0f, 0f - num3)
		};
		places.bench = new v3d[2]
		{
			new v3d(-7f, 0f - num3 - 5f),
			new v3d(7f, 0f - num3 - 5f)
		};
	}
}
