using System.Collections.Generic;
using UnityEngine;

public class FormationData : MonoBehaviour
{
	public class FormationDataFor2DPitch
	{
		public int numrows;

		public int numcols;

		public FormationPosData[] positions;
	}

	public const byte NUM_POSITIONS = 16;

	public const byte GRID_NUM_ROWS = 9;

	public const byte GRID_NUM_COLS = 7;

	public static int PLAYER_POS_GK = 1;

	public static int PLAYER_POS_DL = 2;

	public static int PLAYER_POS_DC = 4;

	public static int PLAYER_POS_DR = 8;

	public static int PLAYER_POS_DML = 16;

	public static int PLAYER_POS_DMC = 32;

	public static int PLAYER_POS_DMR = 64;

	public static int PLAYER_POS_ML = 128;

	public static int PLAYER_POS_MC = 256;

	public static int PLAYER_POS_MR = 512;

	public static int PLAYER_POS_AML = 1024;

	public static int PLAYER_POS_AMC = 2048;

	public static int PLAYER_POS_AMR = 4096;

	public static int PLAYER_POS_FL = 8192;

	public static int PLAYER_POS_FC = 16384;

	public static int PLAYER_POS_FR = 32768;

	public static int[] posarray = new int[16]
	{
		PLAYER_POS_GK, PLAYER_POS_DL, PLAYER_POS_DC, PLAYER_POS_DR, PLAYER_POS_DML, PLAYER_POS_DMC, PLAYER_POS_DMR, PLAYER_POS_ML, PLAYER_POS_MC, PLAYER_POS_MR,
		PLAYER_POS_AML, PLAYER_POS_AMC, PLAYER_POS_AMR, PLAYER_POS_FL, PLAYER_POS_FC, PLAYER_POS_FR
	};

	public static byte[] dbr_formationDisplayOrder = new byte[28]
	{
		0, 9, 26, 14, 15, 19, 20, 22, 2, 11,
		12, 24, 1, 10, 27, 13, 4, 7, 17, 18,
		3, 16, 21, 23, 6, 25, 8, 5
	};

	public static string[] dbr_formationNames = new string[28]
	{
		"4-4-2", "4-3-3", "4-5-1", "3-4-3", "3-5-2", "3-3-4", "5-4-1", "5-3-2 A", "5-2-3", "4-4-2 B",
		"4-3-3 B", "4-5-1 B", "4-2-3-1", "4-3-2-1", "4-4-1-1", "4-3-1-2", "3-4-1-2", "5-3-2 B", "5-3-2 C", "4-2-4",
		"4-2-2-2", "3-4-2-1", "4-1-3-2", "3-4-2-1", "4-2-3-1 B", "5-4-1 B", "4-4-2 C", "3-1-3-3"
	};

	public static string[] dbr_chinaFormationNames = new string[28]
	{
		"4-4-2", "4-3-3", "4-5-1", "3-4-3", "3-5-2", "3-3-4", "5-4-1", "5-3-2 A", "5-2-3", "4-4-2菱形",
		"4-3-3靠边", "4-5-1防守", "4-2-3-1", "4-3-2-1", "4-4-1-1", "4-3-1-2", "3-4-1-2", "5-3-2", "5-3-2 C", "4-2-4",
		"4-2-2-2", "3-4-2-1", "4-1-3-2", "3-2-2-2-1", "4-2-3-1靠边", "5-4-1 B", "4-4-2集中", "3-1-3-3"
	};

	public static int[,] g_black_gridpos = new int[9, 7];

	public static int[,] g_orange_gridpos = new int[9, 7];

	public static Dictionary<int, List<int>> alternative_positions = new Dictionary<int, List<int>>
	{
		{
			PLAYER_POS_GK,
			new List<int>()
		},
		{
			PLAYER_POS_DL,
			new List<int> { PLAYER_POS_DML, PLAYER_POS_DR, PLAYER_POS_DMR, PLAYER_POS_DC }
		},
		{
			PLAYER_POS_DR,
			new List<int> { PLAYER_POS_DMR, PLAYER_POS_DL, PLAYER_POS_DML, PLAYER_POS_DC }
		},
		{
			PLAYER_POS_DC,
			new List<int> { PLAYER_POS_DMC, PLAYER_POS_DL, PLAYER_POS_DR }
		},
		{
			PLAYER_POS_DML,
			new List<int> { PLAYER_POS_DL, PLAYER_POS_DMR, PLAYER_POS_DR, PLAYER_POS_AML, PLAYER_POS_AMR, PLAYER_POS_DMC }
		},
		{
			PLAYER_POS_DMR,
			new List<int> { PLAYER_POS_DR, PLAYER_POS_DML, PLAYER_POS_DL, PLAYER_POS_AMR, PLAYER_POS_AML, PLAYER_POS_DMC }
		},
		{
			PLAYER_POS_DMC,
			new List<int> { PLAYER_POS_MC, PLAYER_POS_AMC, PLAYER_POS_DC, PLAYER_POS_DML, PLAYER_POS_DMR }
		},
		{
			PLAYER_POS_ML,
			new List<int> { PLAYER_POS_DML, PLAYER_POS_MR, PLAYER_POS_DMR, PLAYER_POS_MC, PLAYER_POS_AML, PLAYER_POS_AMR }
		},
		{
			PLAYER_POS_MR,
			new List<int> { PLAYER_POS_DMR, PLAYER_POS_ML, PLAYER_POS_DML, PLAYER_POS_MC, PLAYER_POS_AMR, PLAYER_POS_AML }
		},
		{
			PLAYER_POS_MC,
			new List<int> { PLAYER_POS_DMC, PLAYER_POS_AMC, PLAYER_POS_AML, PLAYER_POS_AMR, PLAYER_POS_ML, PLAYER_POS_MR }
		},
		{
			PLAYER_POS_AML,
			new List<int> { PLAYER_POS_FL, PLAYER_POS_AMR, PLAYER_POS_FR, PLAYER_POS_AMC, PLAYER_POS_DML, PLAYER_POS_DMR }
		},
		{
			PLAYER_POS_AMR,
			new List<int> { PLAYER_POS_FR, PLAYER_POS_AML, PLAYER_POS_FL, PLAYER_POS_AMC, PLAYER_POS_DMR, PLAYER_POS_DML }
		},
		{
			PLAYER_POS_AMC,
			new List<int> { PLAYER_POS_AML, PLAYER_POS_AMR, PLAYER_POS_MC, PLAYER_POS_FC }
		},
		{
			PLAYER_POS_FL,
			new List<int> { PLAYER_POS_AML, PLAYER_POS_FR, PLAYER_POS_AMR, PLAYER_POS_FC }
		},
		{
			PLAYER_POS_FR,
			new List<int> { PLAYER_POS_AMR, PLAYER_POS_FL, PLAYER_POS_AML, PLAYER_POS_FC }
		},
		{
			PLAYER_POS_FC,
			new List<int> { PLAYER_POS_FL, PLAYER_POS_FR, PLAYER_POS_AMC }
		}
	};

	public static Dictionary<int, int> alternative_positions_strict = new Dictionary<int, int>
	{
		{ PLAYER_POS_GK, PLAYER_POS_GK },
		{
			PLAYER_POS_DL,
			PLAYER_POS_DL | PLAYER_POS_DML | PLAYER_POS_DR | PLAYER_POS_DMR
		},
		{
			PLAYER_POS_DR,
			PLAYER_POS_DR | PLAYER_POS_DMR | PLAYER_POS_DL | PLAYER_POS_DML
		},
		{ PLAYER_POS_DC, PLAYER_POS_DC },
		{
			PLAYER_POS_DML,
			PLAYER_POS_DML | PLAYER_POS_DL | PLAYER_POS_AML | PLAYER_POS_DMR | PLAYER_POS_DR | PLAYER_POS_AMR
		},
		{
			PLAYER_POS_DMR,
			PLAYER_POS_DMR | PLAYER_POS_DR | PLAYER_POS_AMR | PLAYER_POS_DML | PLAYER_POS_DL | PLAYER_POS_AML
		},
		{
			PLAYER_POS_DMC,
			PLAYER_POS_DMC | PLAYER_POS_MC
		},
		{
			PLAYER_POS_ML,
			PLAYER_POS_ML | PLAYER_POS_DML | PLAYER_POS_AML | PLAYER_POS_MR | PLAYER_POS_DMR | PLAYER_POS_AMR | PLAYER_POS_MC
		},
		{
			PLAYER_POS_MR,
			PLAYER_POS_MR | PLAYER_POS_DMR | PLAYER_POS_AMR | PLAYER_POS_ML | PLAYER_POS_DML | PLAYER_POS_AML | PLAYER_POS_MC
		},
		{
			PLAYER_POS_MC,
			PLAYER_POS_MC | PLAYER_POS_DMC | PLAYER_POS_AMC
		},
		{
			PLAYER_POS_AML,
			PLAYER_POS_AML | PLAYER_POS_FL | PLAYER_POS_DML | PLAYER_POS_AMR | PLAYER_POS_FR | PLAYER_POS_DMR | PLAYER_POS_AMC
		},
		{
			PLAYER_POS_AMR,
			PLAYER_POS_AMR | PLAYER_POS_FR | PLAYER_POS_DMR | PLAYER_POS_AML | PLAYER_POS_FL | PLAYER_POS_DML | PLAYER_POS_AMC
		},
		{
			PLAYER_POS_AMC,
			PLAYER_POS_AMC | PLAYER_POS_MC | PLAYER_POS_AML | PLAYER_POS_AMR
		},
		{
			PLAYER_POS_FL,
			PLAYER_POS_FL | PLAYER_POS_AML | PLAYER_POS_FR | PLAYER_POS_AMR | PLAYER_POS_AMC | PLAYER_POS_FC
		},
		{
			PLAYER_POS_FR,
			PLAYER_POS_FR | PLAYER_POS_AMR | PLAYER_POS_FL | PLAYER_POS_AML | PLAYER_POS_AMC | PLAYER_POS_FC
		},
		{
			PLAYER_POS_FC,
			PLAYER_POS_FC | PLAYER_POS_FL | PLAYER_POS_FR | PLAYER_POS_AMC
		}
	};

	public static FormationPositionGrid g_formation_grid_position_3_at_the_back = new FormationPositionGrid(new int[9, 7]
	{
		{ PLAYER_POS_GK, PLAYER_POS_GK, PLAYER_POS_GK, PLAYER_POS_GK, PLAYER_POS_GK, PLAYER_POS_GK, PLAYER_POS_GK },
		{
			PLAYER_POS_DR | PLAYER_POS_DMR,
			PLAYER_POS_DC,
			PLAYER_POS_DC,
			PLAYER_POS_DC,
			PLAYER_POS_DC,
			PLAYER_POS_DC,
			PLAYER_POS_DL | PLAYER_POS_DML
		},
		{
			PLAYER_POS_DR | PLAYER_POS_DMR,
			PLAYER_POS_DC,
			PLAYER_POS_DC,
			PLAYER_POS_DC,
			PLAYER_POS_DC,
			PLAYER_POS_DC,
			PLAYER_POS_DL | PLAYER_POS_DML
		},
		{ PLAYER_POS_DMR, PLAYER_POS_DMC, PLAYER_POS_DMC, PLAYER_POS_DMC, PLAYER_POS_DMC, PLAYER_POS_DMC, PLAYER_POS_DML },
		{
			PLAYER_POS_DMR,
			PLAYER_POS_DMC | PLAYER_POS_MC,
			PLAYER_POS_DMC | PLAYER_POS_MC,
			PLAYER_POS_DMC | PLAYER_POS_MC,
			PLAYER_POS_DMC | PLAYER_POS_MC,
			PLAYER_POS_DMC | PLAYER_POS_MC,
			PLAYER_POS_DML
		},
		{
			PLAYER_POS_MR | PLAYER_POS_DMR,
			PLAYER_POS_DMC | PLAYER_POS_MC,
			PLAYER_POS_DMC | PLAYER_POS_MC,
			PLAYER_POS_DMC | PLAYER_POS_MC,
			PLAYER_POS_DMC | PLAYER_POS_MC,
			PLAYER_POS_DMC | PLAYER_POS_MC,
			PLAYER_POS_ML | PLAYER_POS_DML
		},
		{
			PLAYER_POS_AMR,
			PLAYER_POS_AMR | PLAYER_POS_AMC,
			PLAYER_POS_AMC,
			PLAYER_POS_AMC,
			PLAYER_POS_AMC,
			PLAYER_POS_AML | PLAYER_POS_AMC,
			PLAYER_POS_AML
		},
		{
			PLAYER_POS_FR | PLAYER_POS_AMR,
			PLAYER_POS_FR | PLAYER_POS_AMR | PLAYER_POS_AMC,
			PLAYER_POS_AMC | PLAYER_POS_FC,
			PLAYER_POS_AMC | PLAYER_POS_FC,
			PLAYER_POS_AMC | PLAYER_POS_FC,
			PLAYER_POS_FL | PLAYER_POS_AML | PLAYER_POS_AMC,
			PLAYER_POS_FL | PLAYER_POS_AML
		},
		{
			PLAYER_POS_FR,
			PLAYER_POS_FR | PLAYER_POS_FC,
			PLAYER_POS_FC,
			PLAYER_POS_FC,
			PLAYER_POS_FC,
			PLAYER_POS_FL | PLAYER_POS_FC,
			PLAYER_POS_FL
		}
	});

	public static FormationPositionGrid g_formation_grid_position_4_at_the_back = new FormationPositionGrid(new int[9, 7]
	{
		{ PLAYER_POS_GK, PLAYER_POS_GK, PLAYER_POS_GK, PLAYER_POS_GK, PLAYER_POS_GK, PLAYER_POS_GK, PLAYER_POS_GK },
		{
			PLAYER_POS_DR | PLAYER_POS_DMR,
			PLAYER_POS_DC,
			PLAYER_POS_DC,
			PLAYER_POS_DC,
			PLAYER_POS_DC,
			PLAYER_POS_DC,
			PLAYER_POS_DL | PLAYER_POS_DML
		},
		{
			PLAYER_POS_DR | PLAYER_POS_DMR,
			PLAYER_POS_DC,
			PLAYER_POS_DC,
			PLAYER_POS_DC,
			PLAYER_POS_DC,
			PLAYER_POS_DC,
			PLAYER_POS_DL | PLAYER_POS_DML
		},
		{ PLAYER_POS_DMR, PLAYER_POS_DMC, PLAYER_POS_DMC, PLAYER_POS_DMC, PLAYER_POS_DMC, PLAYER_POS_DMC, PLAYER_POS_DML },
		{
			PLAYER_POS_DMR,
			PLAYER_POS_DMC | PLAYER_POS_MC,
			PLAYER_POS_DMC | PLAYER_POS_MC,
			PLAYER_POS_DMC | PLAYER_POS_MC,
			PLAYER_POS_DMC | PLAYER_POS_MC,
			PLAYER_POS_DMC | PLAYER_POS_MC,
			PLAYER_POS_DML
		},
		{
			PLAYER_POS_MR | PLAYER_POS_DMR | PLAYER_POS_AMR,
			PLAYER_POS_DMC | PLAYER_POS_MC,
			PLAYER_POS_DMC | PLAYER_POS_MC,
			PLAYER_POS_DMC | PLAYER_POS_MC,
			PLAYER_POS_DMC | PLAYER_POS_MC,
			PLAYER_POS_DMC | PLAYER_POS_MC,
			PLAYER_POS_ML | PLAYER_POS_DML | PLAYER_POS_AML
		},
		{
			PLAYER_POS_AMR,
			PLAYER_POS_AMR | PLAYER_POS_AMC,
			PLAYER_POS_AMC,
			PLAYER_POS_AMC,
			PLAYER_POS_AMC,
			PLAYER_POS_AML | PLAYER_POS_AMC,
			PLAYER_POS_AML
		},
		{
			PLAYER_POS_FR | PLAYER_POS_AMR,
			PLAYER_POS_FR | PLAYER_POS_AMR | PLAYER_POS_AMC,
			PLAYER_POS_AMC | PLAYER_POS_FC,
			PLAYER_POS_AMC | PLAYER_POS_FC,
			PLAYER_POS_AMC | PLAYER_POS_FC,
			PLAYER_POS_FL | PLAYER_POS_AML | PLAYER_POS_AMC,
			PLAYER_POS_FL | PLAYER_POS_AML
		},
		{
			PLAYER_POS_FR,
			PLAYER_POS_FR | PLAYER_POS_FC,
			PLAYER_POS_FC,
			PLAYER_POS_FC,
			PLAYER_POS_FC,
			PLAYER_POS_FL | PLAYER_POS_FC,
			PLAYER_POS_FL
		}
	});

	public static Dictionary<byte, byte> formation_num_defenders = new Dictionary<byte, byte>
	{
		{ 3, 3 },
		{ 4, 3 },
		{ 5, 3 },
		{ 6, 3 },
		{ 7, 3 },
		{ 8, 3 },
		{ 16, 3 },
		{ 17, 3 },
		{ 18, 3 },
		{ 21, 3 },
		{ 23, 3 },
		{ 25, 3 },
		{ 27, 3 },
		{ 0, 4 },
		{ 1, 4 },
		{ 2, 4 },
		{ 9, 4 },
		{ 10, 4 },
		{ 11, 4 },
		{ 12, 4 },
		{ 13, 4 },
		{ 14, 4 },
		{ 15, 4 },
		{ 19, 4 },
		{ 20, 4 },
		{ 22, 4 },
		{ 24, 4 },
		{ 26, 4 }
	};

	public static byte[,,] Grid_outtapos_multi = new byte[16, 9, 7]
	{
		{
			{ 0, 0, 0, 0, 0, 0, 0 },
			{ 40, 40, 40, 40, 40, 40, 40 },
			{ 40, 40, 40, 40, 40, 40, 40 },
			{ 40, 40, 40, 40, 40, 40, 40 },
			{ 40, 40, 40, 40, 40, 40, 40 },
			{ 40, 40, 40, 40, 40, 40, 40 },
			{ 40, 40, 40, 40, 40, 40, 40 },
			{ 40, 40, 40, 40, 40, 40, 40 },
			{ 40, 40, 40, 40, 40, 40, 40 }
		},
		{
			{ 40, 40, 40, 40, 40, 40, 40 },
			{ 2, 4, 4, 4, 4, 2, 0 },
			{ 2, 4, 4, 4, 4, 2, 0 },
			{ 2, 4, 4, 4, 4, 2, 0 },
			{ 5, 5, 5, 5, 5, 2, 0 },
			{ 15, 15, 15, 15, 15, 15, 15 },
			{ 25, 25, 25, 25, 25, 25, 25 },
			{ 35, 35, 35, 35, 35, 35, 35 },
			{ 35, 35, 35, 35, 35, 35, 35 }
		},
		{
			{ 40, 40, 40, 40, 40, 40, 40 },
			{ 3, 0, 0, 0, 0, 0, 3 },
			{ 3, 0, 0, 0, 0, 0, 3 },
			{ 3, 0, 0, 0, 0, 0, 3 },
			{ 5, 5, 5, 5, 5, 5, 5 },
			{ 15, 15, 15, 15, 15, 15, 15 },
			{ 25, 25, 25, 25, 25, 25, 25 },
			{ 35, 35, 35, 35, 35, 35, 35 },
			{ 35, 35, 35, 35, 35, 35, 35 }
		},
		{
			{ 40, 40, 40, 40, 40, 40, 40 },
			{ 0, 2, 4, 4, 4, 4, 2 },
			{ 0, 2, 4, 4, 4, 4, 2 },
			{ 0, 2, 4, 4, 4, 4, 2 },
			{ 0, 2, 5, 5, 5, 5, 5 },
			{ 15, 15, 15, 15, 15, 15, 15 },
			{ 25, 25, 25, 25, 25, 25, 25 },
			{ 35, 35, 35, 35, 35, 35, 35 },
			{ 35, 35, 35, 35, 35, 35, 35 }
		},
		{
			{ 40, 40, 40, 40, 40, 40, 40 },
			{ 15, 15, 15, 15, 15, 15, 15 },
			{ 5, 5, 5, 5, 5, 5, 5 },
			{ 1, 3, 3, 3, 3, 2, 0 },
			{ 1, 3, 3, 3, 3, 2, 0 },
			{ 1, 3, 3, 3, 3, 2, 0 },
			{ 15, 5, 5, 5, 5, 5, 15 },
			{ 25, 15, 15, 15, 15, 15, 25 },
			{ 25, 25, 25, 25, 25, 25, 25 }
		},
		{
			{ 40, 40, 40, 40, 40, 40, 40 },
			{ 15, 15, 15, 15, 15, 15, 15 },
			{ 5, 5, 5, 5, 5, 5, 5 },
			{ 3, 0, 0, 0, 0, 0, 3 },
			{ 3, 0, 0, 0, 0, 0, 3 },
			{ 3, 3, 3, 3, 3, 3, 3 },
			{ 15, 5, 5, 5, 5, 5, 15 },
			{ 25, 15, 15, 15, 15, 15, 25 },
			{ 25, 25, 25, 25, 25, 25, 25 }
		},
		{
			{ 40, 40, 40, 40, 40, 40, 40 },
			{ 15, 15, 15, 15, 15, 15, 15 },
			{ 5, 5, 5, 5, 5, 5, 5 },
			{ 0, 2, 3, 3, 3, 3, 1 },
			{ 0, 2, 3, 3, 3, 3, 1 },
			{ 0, 2, 3, 3, 3, 3, 1 },
			{ 15, 5, 5, 5, 5, 5, 15 },
			{ 25, 15, 15, 15, 15, 15, 25 },
			{ 25, 25, 25, 25, 25, 25, 25 }
		},
		{
			{ 40, 40, 40, 40, 40, 40, 40 },
			{ 15, 15, 15, 15, 15, 15, 15 },
			{ 15, 15, 15, 15, 15, 15, 15 },
			{ 5, 5, 5, 5, 5, 5, 5 },
			{ 3, 3, 3, 3, 3, 2, 2 },
			{ 1, 3, 3, 3, 3, 2, 0 },
			{ 1, 3, 3, 3, 3, 2, 0 },
			{ 15, 5, 5, 5, 5, 5, 5 },
			{ 15, 15, 15, 15, 15, 15, 15 }
		},
		{
			{ 40, 40, 40, 40, 40, 40, 40 },
			{ 15, 15, 15, 15, 15, 15, 15 },
			{ 15, 15, 15, 15, 15, 15, 15 },
			{ 5, 5, 5, 5, 5, 5, 5 },
			{ 3, 1, 1, 1, 1, 1, 3 },
			{ 3, 0, 0, 0, 0, 0, 3 },
			{ 3, 3, 3, 3, 3, 3, 3 },
			{ 15, 5, 5, 5, 5, 5, 15 },
			{ 15, 15, 15, 15, 15, 15, 15 }
		},
		{
			{ 40, 40, 40, 40, 40, 40, 40 },
			{ 15, 15, 15, 15, 15, 15, 15 },
			{ 15, 15, 15, 15, 15, 15, 15 },
			{ 5, 5, 5, 5, 5, 5, 5 },
			{ 2, 2, 3, 3, 3, 3, 3 },
			{ 0, 2, 3, 3, 3, 3, 1 },
			{ 0, 2, 3, 3, 3, 3, 1 },
			{ 5, 5, 5, 5, 5, 5, 15 },
			{ 15, 15, 15, 15, 15, 15, 15 }
		},
		{
			{ 40, 40, 40, 40, 40, 40, 40 },
			{ 35, 35, 35, 35, 35, 35, 35 },
			{ 35, 35, 35, 35, 35, 35, 35 },
			{ 25, 25, 35, 25, 25, 25, 25 },
			{ 5, 15, 15, 15, 15, 15, 5 },
			{ 4, 4, 5, 5, 5, 2, 1 },
			{ 1, 3, 3, 3, 3, 1, 0 },
			{ 1, 2, 3, 3, 3, 0, 0 },
			{ 1, 3, 5, 5, 5, 2, 0 }
		},
		{
			{ 40, 40, 40, 40, 40, 40, 40 },
			{ 25, 25, 25, 25, 25, 25, 25 },
			{ 25, 25, 25, 25, 25, 25, 25 },
			{ 15, 15, 15, 15, 15, 15, 15 },
			{ 5, 5, 5, 5, 5, 5, 5 },
			{ 3, 2, 2, 2, 2, 2, 3 },
			{ 3, 0, 0, 0, 0, 0, 3 },
			{ 5, 1, 0, 0, 0, 1, 5 },
			{ 15, 15, 15, 15, 15, 15, 15 }
		},
		{
			{ 40, 40, 40, 40, 40, 40, 40 },
			{ 35, 35, 35, 35, 35, 35, 35 },
			{ 35, 35, 35, 35, 35, 35, 35 },
			{ 25, 25, 35, 25, 25, 25, 25 },
			{ 5, 15, 15, 15, 15, 15, 5 },
			{ 1, 2, 5, 5, 5, 4, 4 },
			{ 0, 1, 3, 3, 3, 3, 1 },
			{ 0, 0, 3, 3, 3, 2, 1 },
			{ 0, 2, 5, 5, 5, 3, 1 }
		},
		{
			{ 40, 40, 40, 40, 40, 40, 40 },
			{ 35, 35, 35, 35, 35, 35, 35 },
			{ 35, 35, 35, 35, 35, 35, 35 },
			{ 35, 35, 35, 35, 35, 35, 35 },
			{ 25, 25, 25, 25, 25, 25, 25 },
			{ 15, 15, 15, 15, 15, 15, 15 },
			{ 5, 5, 5, 5, 5, 5, 5 },
			{ 1, 2, 3, 3, 2, 0, 0 },
			{ 1, 2, 3, 3, 2, 0, 0 }
		},
		{
			{ 40, 40, 40, 40, 40, 40, 40 },
			{ 35, 35, 35, 35, 35, 35, 35 },
			{ 35, 35, 35, 35, 35, 35, 35 },
			{ 35, 35, 35, 35, 35, 35, 35 },
			{ 25, 25, 25, 25, 25, 25, 25 },
			{ 15, 15, 15, 15, 15, 15, 15 },
			{ 5, 5, 5, 5, 5, 5, 5 },
			{ 3, 1, 0, 0, 0, 1, 3 },
			{ 3, 1, 0, 0, 0, 1, 3 }
		},
		{
			{ 40, 40, 40, 40, 40, 40, 40 },
			{ 35, 35, 35, 35, 35, 35, 35 },
			{ 35, 35, 35, 35, 35, 35, 35 },
			{ 35, 35, 35, 35, 35, 35, 35 },
			{ 25, 25, 25, 25, 25, 25, 25 },
			{ 15, 15, 15, 15, 15, 15, 15 },
			{ 5, 5, 5, 5, 5, 5, 5 },
			{ 0, 0, 2, 3, 3, 2, 1 },
			{ 0, 0, 2, 3, 3, 2, 1 }
		}
	};

	public static List<byte> invalidFormationIds = new List<byte> { 5, 7, 18, 21, 25 };

	public static List<byte> UserFormations = new List<byte>
	{
		0, 15, 20, 19, 1, 12, 2, 4, 16, 23,
		6, 27
	};

	public static FormationPosData[][] g_formations = new FormationPosData[32][]
	{
		new FormationPosData[11]
		{
			new FormationPosData(3, 0),
			new FormationPosData(0, 2),
			new FormationPosData(6, 2),
			new FormationPosData(2, 2),
			new FormationPosData(4, 2),
			new FormationPosData(2, 5),
			new FormationPosData(0, 5),
			new FormationPosData(4, 5),
			new FormationPosData(2, 8),
			new FormationPosData(4, 8),
			new FormationPosData(6, 5)
		},
		new FormationPosData[11]
		{
			new FormationPosData(3, 0),
			new FormationPosData(0, 2),
			new FormationPosData(6, 2),
			new FormationPosData(2, 2),
			new FormationPosData(4, 2),
			new FormationPosData(3, 5),
			new FormationPosData(1, 5),
			new FormationPosData(5, 5),
			new FormationPosData(3, 8),
			new FormationPosData(1, 7),
			new FormationPosData(5, 7)
		},
		new FormationPosData[11]
		{
			new FormationPosData(3, 0),
			new FormationPosData(0, 2),
			new FormationPosData(6, 2),
			new FormationPosData(2, 2),
			new FormationPosData(4, 2),
			new FormationPosData(2, 4),
			new FormationPosData(0, 5),
			new FormationPosData(4, 4),
			new FormationPosData(3, 8),
			new FormationPosData(3, 6),
			new FormationPosData(6, 5)
		},
		new FormationPosData[11]
		{
			new FormationPosData(3, 0),
			new FormationPosData(1, 2),
			new FormationPosData(5, 2),
			new FormationPosData(3, 2),
			new FormationPosData(2, 5),
			new FormationPosData(4, 5),
			new FormationPosData(0, 5),
			new FormationPosData(6, 5),
			new FormationPosData(3, 8),
			new FormationPosData(1, 8),
			new FormationPosData(5, 8)
		},
		new FormationPosData[11]
		{
			new FormationPosData(3, 0),
			new FormationPosData(1, 2),
			new FormationPosData(5, 2),
			new FormationPosData(3, 2),
			new FormationPosData(3, 4),
			new FormationPosData(4, 5),
			new FormationPosData(0, 5),
			new FormationPosData(2, 5),
			new FormationPosData(4, 8),
			new FormationPosData(2, 8),
			new FormationPosData(6, 5)
		},
		new FormationPosData[0],
		new FormationPosData[11]
		{
			new FormationPosData(3, 0),
			new FormationPosData(1, 2),
			new FormationPosData(5, 2),
			new FormationPosData(3, 2),
			new FormationPosData(0, 3),
			new FormationPosData(6, 3),
			new FormationPosData(1, 5),
			new FormationPosData(3, 4),
			new FormationPosData(3, 8),
			new FormationPosData(3, 6),
			new FormationPosData(5, 5)
		},
		new FormationPosData[0],
		new FormationPosData[11]
		{
			new FormationPosData(3, 0),
			new FormationPosData(1, 2),
			new FormationPosData(5, 2),
			new FormationPosData(3, 2),
			new FormationPosData(0, 4),
			new FormationPosData(6, 4),
			new FormationPosData(2, 5),
			new FormationPosData(4, 5),
			new FormationPosData(3, 8),
			new FormationPosData(1, 8),
			new FormationPosData(5, 8)
		},
		new FormationPosData[11]
		{
			new FormationPosData(3, 0),
			new FormationPosData(0, 2),
			new FormationPosData(6, 2),
			new FormationPosData(2, 2),
			new FormationPosData(4, 2),
			new FormationPosData(3, 4),
			new FormationPosData(0, 5),
			new FormationPosData(3, 6),
			new FormationPosData(2, 8),
			new FormationPosData(4, 8),
			new FormationPosData(6, 5)
		},
		new FormationPosData[11]
		{
			new FormationPosData(3, 0),
			new FormationPosData(0, 2),
			new FormationPosData(6, 2),
			new FormationPosData(2, 2),
			new FormationPosData(4, 2),
			new FormationPosData(3, 4),
			new FormationPosData(2, 5),
			new FormationPosData(4, 5),
			new FormationPosData(3, 8),
			new FormationPosData(0, 7),
			new FormationPosData(6, 7)
		},
		new FormationPosData[11]
		{
			new FormationPosData(3, 0),
			new FormationPosData(0, 2),
			new FormationPosData(6, 2),
			new FormationPosData(2, 2),
			new FormationPosData(4, 2),
			new FormationPosData(2, 5),
			new FormationPosData(0, 5),
			new FormationPosData(4, 5),
			new FormationPosData(3, 8),
			new FormationPosData(3, 4),
			new FormationPosData(6, 5)
		},
		new FormationPosData[11]
		{
			new FormationPosData(3, 0),
			new FormationPosData(0, 2),
			new FormationPosData(6, 2),
			new FormationPosData(2, 2),
			new FormationPosData(4, 2),
			new FormationPosData(2, 4),
			new FormationPosData(1, 6),
			new FormationPosData(4, 4),
			new FormationPosData(3, 8),
			new FormationPosData(3, 6),
			new FormationPosData(5, 6)
		},
		new FormationPosData[11]
		{
			new FormationPosData(3, 0),
			new FormationPosData(0, 2),
			new FormationPosData(6, 2),
			new FormationPosData(2, 2),
			new FormationPosData(4, 2),
			new FormationPosData(3, 4),
			new FormationPosData(1, 5),
			new FormationPosData(5, 5),
			new FormationPosData(3, 8),
			new FormationPosData(2, 6),
			new FormationPosData(4, 6)
		},
		new FormationPosData[11]
		{
			new FormationPosData(3, 0),
			new FormationPosData(0, 2),
			new FormationPosData(6, 2),
			new FormationPosData(2, 2),
			new FormationPosData(4, 2),
			new FormationPosData(2, 5),
			new FormationPosData(0, 5),
			new FormationPosData(4, 5),
			new FormationPosData(2, 8),
			new FormationPosData(4, 7),
			new FormationPosData(6, 5)
		},
		new FormationPosData[11]
		{
			new FormationPosData(3, 0),
			new FormationPosData(0, 2),
			new FormationPosData(6, 2),
			new FormationPosData(2, 2),
			new FormationPosData(4, 2),
			new FormationPosData(3, 5),
			new FormationPosData(1, 5),
			new FormationPosData(3, 7),
			new FormationPosData(2, 8),
			new FormationPosData(4, 8),
			new FormationPosData(5, 5)
		},
		new FormationPosData[11]
		{
			new FormationPosData(3, 0),
			new FormationPosData(1, 2),
			new FormationPosData(5, 2),
			new FormationPosData(3, 2),
			new FormationPosData(2, 5),
			new FormationPosData(4, 5),
			new FormationPosData(0, 5),
			new FormationPosData(6, 5),
			new FormationPosData(2, 8),
			new FormationPosData(3, 7),
			new FormationPosData(4, 8)
		},
		new FormationPosData[11]
		{
			new FormationPosData(3, 0),
			new FormationPosData(1, 2),
			new FormationPosData(5, 2),
			new FormationPosData(3, 1),
			new FormationPosData(0, 4),
			new FormationPosData(6, 4),
			new FormationPosData(2, 5),
			new FormationPosData(3, 4),
			new FormationPosData(4, 8),
			new FormationPosData(2, 8),
			new FormationPosData(4, 5)
		},
		new FormationPosData[0],
		new FormationPosData[11]
		{
			new FormationPosData(3, 0),
			new FormationPosData(0, 2),
			new FormationPosData(6, 2),
			new FormationPosData(2, 2),
			new FormationPosData(4, 2),
			new FormationPosData(2, 5),
			new FormationPosData(0, 7),
			new FormationPosData(4, 5),
			new FormationPosData(2, 8),
			new FormationPosData(4, 8),
			new FormationPosData(6, 7)
		},
		new FormationPosData[11]
		{
			new FormationPosData(3, 0),
			new FormationPosData(0, 2),
			new FormationPosData(6, 2),
			new FormationPosData(2, 2),
			new FormationPosData(4, 2),
			new FormationPosData(2, 4),
			new FormationPosData(1, 6),
			new FormationPosData(4, 4),
			new FormationPosData(2, 8),
			new FormationPosData(4, 8),
			new FormationPosData(5, 6)
		},
		new FormationPosData[0],
		new FormationPosData[11]
		{
			new FormationPosData(3, 0),
			new FormationPosData(0, 2),
			new FormationPosData(6, 2),
			new FormationPosData(2, 2),
			new FormationPosData(4, 2),
			new FormationPosData(1, 6),
			new FormationPosData(3, 6),
			new FormationPosData(3, 4),
			new FormationPosData(2, 8),
			new FormationPosData(4, 8),
			new FormationPosData(5, 6)
		},
		new FormationPosData[11]
		{
			new FormationPosData(3, 0),
			new FormationPosData(1, 2),
			new FormationPosData(5, 2),
			new FormationPosData(3, 2),
			new FormationPosData(2, 4),
			new FormationPosData(4, 4),
			new FormationPosData(0, 5),
			new FormationPosData(6, 5),
			new FormationPosData(3, 8),
			new FormationPosData(2, 7),
			new FormationPosData(4, 7)
		},
		new FormationPosData[11]
		{
			new FormationPosData(3, 0),
			new FormationPosData(0, 2),
			new FormationPosData(6, 2),
			new FormationPosData(2, 2),
			new FormationPosData(4, 2),
			new FormationPosData(2, 4),
			new FormationPosData(0, 6),
			new FormationPosData(4, 4),
			new FormationPosData(3, 8),
			new FormationPosData(3, 6),
			new FormationPosData(6, 6)
		},
		new FormationPosData[0],
		new FormationPosData[11]
		{
			new FormationPosData(3, 0),
			new FormationPosData(0, 2),
			new FormationPosData(6, 2),
			new FormationPosData(2, 2),
			new FormationPosData(4, 2),
			new FormationPosData(3, 4),
			new FormationPosData(1, 5),
			new FormationPosData(3, 6),
			new FormationPosData(2, 8),
			new FormationPosData(4, 8),
			new FormationPosData(5, 5)
		},
		new FormationPosData[11]
		{
			new FormationPosData(3, 0),
			new FormationPosData(1, 2),
			new FormationPosData(5, 2),
			new FormationPosData(3, 2),
			new FormationPosData(3, 4),
			new FormationPosData(1, 5),
			new FormationPosData(1, 8),
			new FormationPosData(5, 5),
			new FormationPosData(3, 8),
			new FormationPosData(3, 6),
			new FormationPosData(5, 8)
		},
		new FormationPosData[11]
		{
			new FormationPosData(0, 0),
			new FormationPosData(0, 0),
			new FormationPosData(0, 0),
			new FormationPosData(0, 0),
			new FormationPosData(0, 0),
			new FormationPosData(0, 0),
			new FormationPosData(0, 0),
			new FormationPosData(0, 0),
			new FormationPosData(0, 0),
			new FormationPosData(0, 0),
			new FormationPosData(0, 0)
		},
		new FormationPosData[11]
		{
			new FormationPosData(0, 0),
			new FormationPosData(0, 0),
			new FormationPosData(0, 0),
			new FormationPosData(0, 0),
			new FormationPosData(0, 0),
			new FormationPosData(0, 0),
			new FormationPosData(0, 0),
			new FormationPosData(0, 0),
			new FormationPosData(0, 0),
			new FormationPosData(0, 0),
			new FormationPosData(0, 0)
		},
		new FormationPosData[11]
		{
			new FormationPosData(0, 0),
			new FormationPosData(0, 0),
			new FormationPosData(0, 0),
			new FormationPosData(0, 0),
			new FormationPosData(0, 0),
			new FormationPosData(0, 0),
			new FormationPosData(0, 0),
			new FormationPosData(0, 0),
			new FormationPosData(0, 0),
			new FormationPosData(0, 0),
			new FormationPosData(0, 0)
		},
		new FormationPosData[11]
		{
			new FormationPosData(0, 0),
			new FormationPosData(0, 0),
			new FormationPosData(0, 0),
			new FormationPosData(0, 0),
			new FormationPosData(0, 0),
			new FormationPosData(0, 0),
			new FormationPosData(0, 0),
			new FormationPosData(0, 0),
			new FormationPosData(0, 0),
			new FormationPosData(0, 0),
			new FormationPosData(0, 0)
		}
	};

	public static string GetName(int formationIndex)
	{
		if (formationIndex >= 28)
		{
			if (!UI.GetInstance().instantMatchMode && Game.instance.ManagedClubProfile.CustomFormations.ContainsKey(formationIndex))
			{
				return Game.instance.ManagedClubProfile.CustomFormations[formationIndex].Name;
			}
			return "Empty";
		}
		string[][] array = new string[4][]
		{
			new string[2] { "Wingers", "inst-wingers" },
			new string[2] { "Diamond", "inst-diamond" },
			new string[2] { "Defensive", "inst-defensive" },
			new string[2] { "Sweeper", "inst-sweeper" }
		};
		string text = dbr_formationNames[formationIndex];
		for (int i = 0; i < array.Length; i++)
		{
			string key = array[i][0];
			string text2 = Translation.GetText(array[i][1] ?? "");
			text = Translation.TemplateReplace(text, new Dictionary<string, string> { { key, text2 } });
		}
		return text;
	}

	public static FormationDataFor2DPitch GetFormation(int formationid)
	{
		return new FormationDataFor2DPitch
		{
			numrows = 9,
			numcols = 7,
			positions = g_formations[formationid]
		};
	}

	public static void MakeGridPos()
	{
		for (int i = 0; i < posarray.Length; i++)
		{
			for (int j = 0; j < 9; j++)
			{
				for (int k = 0; k < 7; k++)
				{
					if (Grid_outtapos_multi[i, j, k] == 0)
					{
						g_black_gridpos[j, k] |= posarray[i];
					}
					else if (Grid_outtapos_multi[i, j, k] < 15)
					{
						g_orange_gridpos[j, k] |= posarray[i];
					}
				}
			}
		}
	}

	public static FormationPositionGrid GetFormationPositionGrid(byte formationId)
	{
		if (formation_num_defenders.ContainsKey(formationId))
		{
			if (formation_num_defenders[formationId] != 3)
			{
				return g_formation_grid_position_4_at_the_back;
			}
			return g_formation_grid_position_3_at_the_back;
		}
		return g_formation_grid_position_4_at_the_back;
	}
}
