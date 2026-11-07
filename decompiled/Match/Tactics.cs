using System.Collections.Generic;
using UnityEngine;

public class Tactics : MonoBehaviour
{
	public enum TacticalPos
	{
		UNKNOWN = -1,
		ALL_ROLES,
		GK,
		DC,
		DLR,
		DM,
		DMLR,
		MC,
		MLR,
		AM,
		AMLR,
		FC,
		FLR
	}

	public enum PlayerRoles
	{
		EMPTY = 0,
		KEEPER = 1,
		MODERN_KEEPER = 2,
		GENERAL_DEF = 11,
		STOPPER = 10,
		BALL_PLAYING_DEF = 12,
		FULL_BACK = 13,
		WING_BACK = 14,
		GENERAL_MID = 23,
		BOX_TO_BOX = 22,
		PLAYMAKER = 24,
		BALL_WINNING_MID = 20,
		DEEP_LYING_PLAYMAKER = 21,
		WIDE_MID = 25,
		WINGER = 27,
		ADV_PLAYMAKER = 28,
		SUPPORT_STRIKER = 29,
		GENERAL_FORWARD = 44,
		TARGET_MAN = 40,
		FINISHER = 41,
		WIDE_FORWARD = 42,
		DEEP_LYING_FORWARD = 43
	}

	public enum PosPriority
	{
		TOP = 6,
		EXTREME = 5,
		HIGH = 4,
		AVERAGE = 3,
		LOW = 2,
		VERY_LOW = 1,
		NONE = 0
	}

	public static Dictionary<byte, string> tacklingStyles = new Dictionary<byte, string>
	{
		{ 1, "inst-tacklingstyle-3" },
		{ 2, "inst-tacklingstyle-4" },
		{ 3, "inst-tacklingstyle-5" }
	};

	public static Dictionary<byte, string> passingStyles = new Dictionary<byte, string>
	{
		{ 1, "inst-passingstyleid-1" },
		{ 2, "inst-passingstyleid-2" },
		{ 3, "inst-passingstyleid-3" },
		{ 4, "inst-passingstyleid-4" }
	};

	public static Dictionary<byte, string> tempoStyle = new Dictionary<byte, string>
	{
		{ 1, "inst-tempoid-1" },
		{ 2, "inst-tempoid-2" },
		{ 3, "inst-tempoid-3" }
	};

	public static Dictionary<byte, string> widthStyle = new Dictionary<byte, string>
	{
		{ 1, "inst-width-1" },
		{ 2, "inst-width-3" },
		{ 3, "inst-width-5" }
	};

	public static Dictionary<byte, string> mentalityStyle = new Dictionary<byte, string>
	{
		{ 1, "inst-mentality-1" },
		{ 2, "inst-mentality-2" },
		{ 3, "inst-mentality-3" },
		{ 4, "inst-mentality-4" },
		{ 5, "inst-mentality-5" }
	};

	public static Dictionary<byte, string> attackingStyle = new Dictionary<byte, string>
	{
		{ 1, "inst-attackingstyle-1" },
		{ 2, "inst-attackingstyle-2" },
		{ 3, "inst-attackingstyle-3" },
		{ 4, "inst-attackingstyle-4" },
		{ 5, "inst-attackingstyle-5" }
	};

	public static Dictionary<byte, string> pressingStyle = new Dictionary<byte, string>
	{
		{ 1, "inst-pressingid-1" },
		{ 2, "inst-pressingid-2" },
		{ 3, "inst-pressingid-3" }
	};

	public static Dictionary<byte, string> creativeStyle = new Dictionary<byte, string>
	{
		{ 1, "inst-creativity-1" },
		{ 2, "inst-creativity-2" },
		{ 3, "inst-creativity-3" }
	};

	public static Dictionary<byte, string> forwardsInstruction = new Dictionary<byte, string>
	{
		{ 1, "inst-attackingstyle-1" },
		{ 2, "inst-att-forwards-2" },
		{ 3, "inst-att-forwards-1" }
	};

	public static Dictionary<byte, string> midfieldersInstruction = new Dictionary<byte, string>
	{
		{ 1, "inst-attackingstyle-1" },
		{ 2, "inst-att-midfielders-2" },
		{ 3, "inst-att-midfielders-1" }
	};

	public static Dictionary<byte, string> wingersInstruction = new Dictionary<byte, string>
	{
		{ 1, "inst-attackingstyle-1" },
		{ 2, "inst-att-wingers-3" },
		{ 3, "inst-att-wingers-1" },
		{ 4, "inst-att-forwards-2" }
	};

	public static Dictionary<byte, string> backLineStyle = new Dictionary<byte, string>
	{
		{ 1, "intensity-low" },
		{ 2, "inst-backline-3" },
		{ 3, "intensity-high" }
	};

	public static string[,] formationPositionNames = new string[10, 7]
	{
		{
			Helpers.GetPlayerPositionString(1),
			Helpers.GetPlayerPositionString(1),
			Helpers.GetPlayerPositionString(1),
			Helpers.GetPlayerPositionString(1),
			Helpers.GetPlayerPositionString(1),
			Helpers.GetPlayerPositionString(1),
			Helpers.GetPlayerPositionString(1)
		},
		{
			Helpers.GetPlayerPositionString(8),
			Helpers.GetPlayerPositionString(4),
			Helpers.GetPlayerPositionString(4),
			Helpers.GetPlayerPositionString(4),
			Helpers.GetPlayerPositionString(4),
			Helpers.GetPlayerPositionString(4),
			Helpers.GetPlayerPositionString(2)
		},
		{
			Helpers.GetPlayerPositionString(8),
			Helpers.GetPlayerPositionString(4),
			Helpers.GetPlayerPositionString(4),
			Helpers.GetPlayerPositionString(4),
			Helpers.GetPlayerPositionString(4),
			Helpers.GetPlayerPositionString(4),
			Helpers.GetPlayerPositionString(2)
		},
		{
			Helpers.GetPlayerPositionString(64),
			Helpers.GetPlayerPositionString(32),
			Helpers.GetPlayerPositionString(32),
			Helpers.GetPlayerPositionString(32),
			Helpers.GetPlayerPositionString(32),
			Helpers.GetPlayerPositionString(32),
			Helpers.GetPlayerPositionString(16)
		},
		{
			Helpers.GetPlayerPositionString(64),
			Helpers.GetPlayerPositionString(32),
			Helpers.GetPlayerPositionString(32),
			Helpers.GetPlayerPositionString(32),
			Helpers.GetPlayerPositionString(32),
			Helpers.GetPlayerPositionString(32),
			Helpers.GetPlayerPositionString(16)
		},
		{
			Helpers.GetPlayerPositionString(512),
			Helpers.GetPlayerPositionString(256),
			Helpers.GetPlayerPositionString(256),
			Helpers.GetPlayerPositionString(256),
			Helpers.GetPlayerPositionString(256),
			Helpers.GetPlayerPositionString(256),
			Helpers.GetPlayerPositionString(128)
		},
		{
			Helpers.GetPlayerPositionString(4096),
			Helpers.GetPlayerPositionString(2048),
			Helpers.GetPlayerPositionString(2048),
			Helpers.GetPlayerPositionString(2048),
			Helpers.GetPlayerPositionString(2048),
			Helpers.GetPlayerPositionString(2048),
			Helpers.GetPlayerPositionString(1024)
		},
		{
			Helpers.GetPlayerPositionString(4096),
			Helpers.GetPlayerPositionString(4096),
			Helpers.GetPlayerPositionString(2048),
			Helpers.GetPlayerPositionString(2048),
			Helpers.GetPlayerPositionString(2048),
			Helpers.GetPlayerPositionString(1024),
			Helpers.GetPlayerPositionString(1024)
		},
		{
			Helpers.GetPlayerPositionString(32768),
			Helpers.GetPlayerPositionString(32768),
			Helpers.GetPlayerPositionString(16384),
			Helpers.GetPlayerPositionString(16384),
			Helpers.GetPlayerPositionString(16384),
			Helpers.GetPlayerPositionString(8192),
			Helpers.GetPlayerPositionString(8192)
		},
		{
			Helpers.GetPlayerPositionString(32768),
			Helpers.GetPlayerPositionString(32768),
			Helpers.GetPlayerPositionString(16384),
			Helpers.GetPlayerPositionString(16384),
			Helpers.GetPlayerPositionString(16384),
			Helpers.GetPlayerPositionString(8192),
			Helpers.GetPlayerPositionString(8192)
		}
	};

	public static Constants.PlayerPosition[,] formationPositions = new Constants.PlayerPosition[10, 7]
	{
		{
			Constants.PlayerPosition.PLAYER_POS_GK,
			Constants.PlayerPosition.PLAYER_POS_GK,
			Constants.PlayerPosition.PLAYER_POS_GK,
			Constants.PlayerPosition.PLAYER_POS_GK,
			Constants.PlayerPosition.PLAYER_POS_GK,
			Constants.PlayerPosition.PLAYER_POS_GK,
			Constants.PlayerPosition.PLAYER_POS_GK
		},
		{
			Constants.PlayerPosition.PLAYER_POS_DR,
			Constants.PlayerPosition.PLAYER_POS_DC,
			Constants.PlayerPosition.PLAYER_POS_DC,
			Constants.PlayerPosition.PLAYER_POS_DC,
			Constants.PlayerPosition.PLAYER_POS_DC,
			Constants.PlayerPosition.PLAYER_POS_DC,
			Constants.PlayerPosition.PLAYER_POS_DL
		},
		{
			Constants.PlayerPosition.PLAYER_POS_DR,
			Constants.PlayerPosition.PLAYER_POS_DC,
			Constants.PlayerPosition.PLAYER_POS_DC,
			Constants.PlayerPosition.PLAYER_POS_DC,
			Constants.PlayerPosition.PLAYER_POS_DC,
			Constants.PlayerPosition.PLAYER_POS_DC,
			Constants.PlayerPosition.PLAYER_POS_DL
		},
		{
			Constants.PlayerPosition.PLAYER_POS_DMR,
			Constants.PlayerPosition.PLAYER_POS_DMC,
			Constants.PlayerPosition.PLAYER_POS_DMC,
			Constants.PlayerPosition.PLAYER_POS_DMC,
			Constants.PlayerPosition.PLAYER_POS_DMC,
			Constants.PlayerPosition.PLAYER_POS_DMC,
			Constants.PlayerPosition.PLAYER_POS_DML
		},
		{
			Constants.PlayerPosition.PLAYER_POS_DMR,
			Constants.PlayerPosition.PLAYER_POS_DMC,
			Constants.PlayerPosition.PLAYER_POS_DMC,
			Constants.PlayerPosition.PLAYER_POS_DMC,
			Constants.PlayerPosition.PLAYER_POS_DMC,
			Constants.PlayerPosition.PLAYER_POS_DMC,
			Constants.PlayerPosition.PLAYER_POS_DML
		},
		{
			Constants.PlayerPosition.PLAYER_POS_MR,
			Constants.PlayerPosition.PLAYER_POS_MC,
			Constants.PlayerPosition.PLAYER_POS_MC,
			Constants.PlayerPosition.PLAYER_POS_MC,
			Constants.PlayerPosition.PLAYER_POS_MC,
			Constants.PlayerPosition.PLAYER_POS_MC,
			Constants.PlayerPosition.PLAYER_POS_ML
		},
		{
			Constants.PlayerPosition.PLAYER_POS_AMR,
			Constants.PlayerPosition.PLAYER_POS_AMC,
			Constants.PlayerPosition.PLAYER_POS_AMC,
			Constants.PlayerPosition.PLAYER_POS_AMC,
			Constants.PlayerPosition.PLAYER_POS_AMC,
			Constants.PlayerPosition.PLAYER_POS_AMC,
			Constants.PlayerPosition.PLAYER_POS_AML
		},
		{
			Constants.PlayerPosition.PLAYER_POS_AMR,
			Constants.PlayerPosition.PLAYER_POS_AMR,
			Constants.PlayerPosition.PLAYER_POS_AMC,
			Constants.PlayerPosition.PLAYER_POS_AMC,
			Constants.PlayerPosition.PLAYER_POS_AMC,
			Constants.PlayerPosition.PLAYER_POS_AML,
			Constants.PlayerPosition.PLAYER_POS_AML
		},
		{
			Constants.PlayerPosition.PLAYER_POS_FR,
			Constants.PlayerPosition.PLAYER_POS_FR,
			Constants.PlayerPosition.PLAYER_POS_FC,
			Constants.PlayerPosition.PLAYER_POS_FC,
			Constants.PlayerPosition.PLAYER_POS_FC,
			Constants.PlayerPosition.PLAYER_POS_FL,
			Constants.PlayerPosition.PLAYER_POS_FL
		},
		{
			Constants.PlayerPosition.PLAYER_POS_FR,
			Constants.PlayerPosition.PLAYER_POS_FR,
			Constants.PlayerPosition.PLAYER_POS_FC,
			Constants.PlayerPosition.PLAYER_POS_FC,
			Constants.PlayerPosition.PLAYER_POS_FC,
			Constants.PlayerPosition.PLAYER_POS_FL,
			Constants.PlayerPosition.PLAYER_POS_FL
		}
	};

	public static Dictionary<TacticalPos, PlayerRoles[]> AvailableRoles = new Dictionary<TacticalPos, PlayerRoles[]>
	{
		{
			TacticalPos.UNKNOWN,
			new PlayerRoles[1]
		},
		{
			TacticalPos.ALL_ROLES,
			new PlayerRoles[1]
		},
		{
			TacticalPos.GK,
			new PlayerRoles[2]
			{
				PlayerRoles.KEEPER,
				PlayerRoles.MODERN_KEEPER
			}
		},
		{
			TacticalPos.DC,
			new PlayerRoles[3]
			{
				PlayerRoles.GENERAL_DEF,
				PlayerRoles.STOPPER,
				PlayerRoles.BALL_PLAYING_DEF
			}
		},
		{
			TacticalPos.DLR,
			new PlayerRoles[2]
			{
				PlayerRoles.FULL_BACK,
				PlayerRoles.WING_BACK
			}
		},
		{
			TacticalPos.DM,
			new PlayerRoles[3]
			{
				PlayerRoles.BALL_WINNING_MID,
				PlayerRoles.DEEP_LYING_PLAYMAKER,
				PlayerRoles.BOX_TO_BOX
			}
		},
		{
			TacticalPos.DMLR,
			new PlayerRoles[3]
			{
				PlayerRoles.FULL_BACK,
				PlayerRoles.WING_BACK,
				PlayerRoles.WIDE_MID
			}
		},
		{
			TacticalPos.MC,
			new PlayerRoles[5]
			{
				PlayerRoles.GENERAL_MID,
				PlayerRoles.BALL_WINNING_MID,
				PlayerRoles.BOX_TO_BOX,
				PlayerRoles.PLAYMAKER,
				PlayerRoles.ADV_PLAYMAKER
			}
		},
		{
			TacticalPos.MLR,
			new PlayerRoles[4]
			{
				PlayerRoles.WIDE_MID,
				PlayerRoles.WINGER,
				PlayerRoles.WING_BACK,
				PlayerRoles.PLAYMAKER
			}
		},
		{
			TacticalPos.AM,
			new PlayerRoles[2]
			{
				PlayerRoles.ADV_PLAYMAKER,
				PlayerRoles.SUPPORT_STRIKER
			}
		},
		{
			TacticalPos.AMLR,
			new PlayerRoles[3]
			{
				PlayerRoles.WINGER,
				PlayerRoles.ADV_PLAYMAKER,
				PlayerRoles.WIDE_FORWARD
			}
		},
		{
			TacticalPos.FC,
			new PlayerRoles[4]
			{
				PlayerRoles.GENERAL_FORWARD,
				PlayerRoles.TARGET_MAN,
				PlayerRoles.FINISHER,
				PlayerRoles.DEEP_LYING_FORWARD
			}
		},
		{
			TacticalPos.FLR,
			new PlayerRoles[3]
			{
				PlayerRoles.WIDE_FORWARD,
				PlayerRoles.GENERAL_FORWARD,
				PlayerRoles.WINGER
			}
		}
	};

	public static string GetFormationPositionName(int[] formationValues)
	{
		return formationPositionNames[formationValues[1], formationValues[0]];
	}

	public static Constants.PlayerPosition GetFormationPosition(int[] formationValues)
	{
		return formationPositions[formationValues[1], formationValues[0]];
	}

	public static float GetRoleThreat(TacticalPos tacticalPos)
	{
		switch (tacticalPos)
		{
		default:
			return 0f;
		case TacticalPos.DLR:
		case TacticalPos.DM:
			return 0.2f;
		case TacticalPos.DMLR:
			return 0.4f;
		case TacticalPos.MC:
			return 0.5f;
		case TacticalPos.MLR:
			return 0.7f;
		case TacticalPos.AM:
		case TacticalPos.AMLR:
			return 0.85f;
		case TacticalPos.FC:
		case TacticalPos.FLR:
			return 1f;
		}
	}

	public static TacticalPos GetTacticalPosFromPosition(int[] formationValues)
	{
		switch (GetFormationPosition(formationValues))
		{
		case Constants.PlayerPosition.PLAYER_POS_GK:
			return TacticalPos.GK;
		case Constants.PlayerPosition.PLAYER_POS_DC:
		case Constants.PlayerPosition.PLAYER_POS_D:
			return TacticalPos.DC;
		case Constants.PlayerPosition.PLAYER_POS_DL:
		case Constants.PlayerPosition.PLAYER_POS_DR:
			return TacticalPos.DLR;
		case Constants.PlayerPosition.PLAYER_POS_DMC:
		case Constants.PlayerPosition.PLAYER_POS_DM:
			return TacticalPos.DM;
		case Constants.PlayerPosition.PLAYER_POS_DML:
		case Constants.PlayerPosition.PLAYER_POS_DMR:
			return TacticalPos.DMLR;
		case Constants.PlayerPosition.PLAYER_POS_MC:
		case Constants.PlayerPosition.PLAYER_POS_M:
			return TacticalPos.MC;
		case Constants.PlayerPosition.PLAYER_POS_ML:
		case Constants.PlayerPosition.PLAYER_POS_MR:
			return TacticalPos.MLR;
		case Constants.PlayerPosition.PLAYER_POS_AMC:
		case Constants.PlayerPosition.PLAYER_POS_AM:
			return TacticalPos.AM;
		case Constants.PlayerPosition.PLAYER_POS_AML:
		case Constants.PlayerPosition.PLAYER_POS_AMR:
			return TacticalPos.AMLR;
		case Constants.PlayerPosition.PLAYER_POS_FC:
		case Constants.PlayerPosition.PLAYER_POS_F:
			return TacticalPos.FC;
		case Constants.PlayerPosition.PLAYER_POS_FL:
		case Constants.PlayerPosition.PLAYER_POS_FR:
			return TacticalPos.FLR;
		default:
			return TacticalPos.UNKNOWN;
		}
	}

	public static string GetRoleName(PlayerRoles instruction)
	{
		if (instruction == PlayerRoles.EMPTY)
		{
			return "Empty";
		}
		string text = Helpers.GetPlayerDescriptionString((int)instruction);
		if (text == "")
		{
			text = "Missing";
		}
		return text;
	}

	public static string GetRoleSpriteName(PlayerRoles instruction)
	{
		return instruction switch
		{
			PlayerRoles.BALL_WINNING_MID => "BallWinningMid", 
			PlayerRoles.ADV_PLAYMAKER => "AdvPlaymaker", 
			PlayerRoles.BALL_PLAYING_DEF => "BallPlayingDefender", 
			PlayerRoles.BOX_TO_BOX => "BoxToBox", 
			PlayerRoles.DEEP_LYING_FORWARD => "DeepLyingForward", 
			PlayerRoles.DEEP_LYING_PLAYMAKER => "DeepLyingPlaymaker", 
			PlayerRoles.FINISHER => "Finisher", 
			PlayerRoles.MODERN_KEEPER => "ModernKeeper", 
			PlayerRoles.PLAYMAKER => "Playmaker", 
			PlayerRoles.STOPPER => "Stopper", 
			PlayerRoles.SUPPORT_STRIKER => "SupportStriker", 
			PlayerRoles.TARGET_MAN => "TargetMan", 
			PlayerRoles.WIDE_FORWARD => "WideForward", 
			PlayerRoles.WIDE_MID => "WideMid", 
			PlayerRoles.WING_BACK => "Wingback", 
			PlayerRoles.WINGER => "Winger", 
			_ => "", 
		};
	}

	public static PosPriority GetMarkingPosPriority(TacticalPos yourTacticalPos, TacticalPos oppTacticalPos)
	{
		switch (yourTacticalPos)
		{
		case TacticalPos.DC:
			switch (oppTacticalPos)
			{
			case TacticalPos.FC:
				return PosPriority.TOP;
			case TacticalPos.AM:
			case TacticalPos.FLR:
				return PosPriority.EXTREME;
			case TacticalPos.AMLR:
				return PosPriority.HIGH;
			case TacticalPos.MC:
			case TacticalPos.MLR:
				return PosPriority.AVERAGE;
			case TacticalPos.DM:
			case TacticalPos.DMLR:
				return PosPriority.LOW;
			case TacticalPos.DC:
			case TacticalPos.DLR:
				return PosPriority.VERY_LOW;
			}
			break;
		case TacticalPos.DLR:
			switch (oppTacticalPos)
			{
			case TacticalPos.FLR:
				return PosPriority.TOP;
			case TacticalPos.AMLR:
				return PosPriority.EXTREME;
			case TacticalPos.MLR:
			case TacticalPos.AM:
			case TacticalPos.FC:
				return PosPriority.HIGH;
			case TacticalPos.DLR:
			case TacticalPos.DMLR:
			case TacticalPos.MC:
				return PosPriority.AVERAGE;
			case TacticalPos.DM:
				return PosPriority.LOW;
			case TacticalPos.DC:
				return PosPriority.VERY_LOW;
			}
			break;
		case TacticalPos.DM:
			switch (oppTacticalPos)
			{
			case TacticalPos.AM:
				return PosPriority.EXTREME;
			case TacticalPos.AMLR:
			case TacticalPos.FC:
				return PosPriority.HIGH;
			case TacticalPos.FLR:
				return PosPriority.AVERAGE;
			case TacticalPos.MC:
			case TacticalPos.MLR:
				return PosPriority.LOW;
			case TacticalPos.DM:
			case TacticalPos.DMLR:
				return PosPriority.VERY_LOW;
			}
			break;
		case TacticalPos.DMLR:
			switch (oppTacticalPos)
			{
			case TacticalPos.AMLR:
				return PosPriority.EXTREME;
			case TacticalPos.AM:
			case TacticalPos.FC:
			case TacticalPos.FLR:
				return PosPriority.HIGH;
			case TacticalPos.DLR:
			case TacticalPos.DMLR:
			case TacticalPos.MLR:
				return PosPriority.AVERAGE;
			case TacticalPos.MC:
				return PosPriority.LOW;
			case TacticalPos.DM:
				return PosPriority.VERY_LOW;
			}
			break;
		case TacticalPos.MC:
		case TacticalPos.MLR:
			switch (oppTacticalPos)
			{
			case TacticalPos.FC:
			case TacticalPos.FLR:
				return PosPriority.HIGH;
			case TacticalPos.AM:
			case TacticalPos.AMLR:
				return PosPriority.AVERAGE;
			case TacticalPos.MC:
			case TacticalPos.MLR:
				return PosPriority.LOW;
			case TacticalPos.DM:
			case TacticalPos.DMLR:
				return PosPriority.VERY_LOW;
			}
			break;
		case TacticalPos.AM:
		case TacticalPos.AMLR:
		case TacticalPos.FC:
		case TacticalPos.FLR:
			switch (oppTacticalPos)
			{
			case TacticalPos.FC:
			case TacticalPos.FLR:
				return PosPriority.AVERAGE;
			case TacticalPos.AM:
			case TacticalPos.AMLR:
				return PosPriority.LOW;
			}
			break;
		}
		return PosPriority.NONE;
	}
}
