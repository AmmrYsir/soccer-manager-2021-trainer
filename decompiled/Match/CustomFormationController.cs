using System.Collections.Generic;
using System.Linq;

public class CustomFormationController
{
	public enum CustomFormationError
	{
		LeftWing,
		RightWing,
		Column,
		Defence,
		Midfield,
		Forward
	}

	public static CustomFormationController Instance;

	public static FormationPosData[] CustomFormationSpaces = new FormationPosData[49]
	{
		new FormationPosData(0, 2),
		new FormationPosData(1, 2),
		new FormationPosData(2, 2),
		new FormationPosData(3, 2),
		new FormationPosData(4, 2),
		new FormationPosData(5, 2),
		new FormationPosData(6, 2),
		new FormationPosData(0, 3),
		new FormationPosData(1, 3),
		new FormationPosData(2, 3),
		new FormationPosData(3, 3),
		new FormationPosData(4, 3),
		new FormationPosData(5, 3),
		new FormationPosData(6, 3),
		new FormationPosData(0, 4),
		new FormationPosData(1, 4),
		new FormationPosData(2, 4),
		new FormationPosData(3, 4),
		new FormationPosData(4, 4),
		new FormationPosData(5, 4),
		new FormationPosData(6, 4),
		new FormationPosData(0, 5),
		new FormationPosData(1, 5),
		new FormationPosData(2, 5),
		new FormationPosData(3, 5),
		new FormationPosData(4, 5),
		new FormationPosData(5, 5),
		new FormationPosData(6, 5),
		new FormationPosData(0, 6),
		new FormationPosData(1, 6),
		new FormationPosData(2, 6),
		new FormationPosData(3, 6),
		new FormationPosData(4, 6),
		new FormationPosData(5, 6),
		new FormationPosData(6, 6),
		new FormationPosData(0, 7),
		new FormationPosData(1, 7),
		new FormationPosData(2, 7),
		new FormationPosData(3, 7),
		new FormationPosData(4, 7),
		new FormationPosData(5, 7),
		new FormationPosData(6, 7),
		new FormationPosData(0, 8),
		new FormationPosData(1, 8),
		new FormationPosData(2, 8),
		new FormationPosData(3, 8),
		new FormationPosData(4, 8),
		new FormationPosData(5, 8),
		new FormationPosData(6, 8)
	};

	public const int CustomFormationID1 = 28;

	public const int CustomFormationID2 = 29;

	public const int CustomFormationID3 = 30;

	public const int CustomFormationIDSoftSave = 31;

	public static List<int> CustomFormationIDs = new List<int> { 28, 29, 30 };

	public const int MaxColumnCount = 5;

	public const int MaxWingCount = 3;

	public const int MinDefenceCount = 2;

	public const int MinMidfieldCount = 2;

	public const int MinForwardCount = 1;

	private int currLeftWingCount;

	private int currRightWingCount;

	private int currDefenceCount;

	private int currMidfieldCount;

	private int currForwardCount;

	public List<TacticsGridSpace> TacticSpaces = new List<TacticsGridSpace>();

	public FormationPosData[] CurrentFormation = new FormationPosData[11];

	public static bool SoftSavedFormation;

	public static bool NewSavedFormation;

	public static CustomFormationController GetInstance()
	{
		if (Instance != null)
		{
			return Instance;
		}
		Instance = new CustomFormationController();
		return Instance;
	}

	public static void LoadSavedFormations()
	{
		foreach (KeyValuePair<int, CustomFormation> customFormation in Game.instance.ManagedClubProfile.CustomFormations)
		{
			FormationData.g_formations[customFormation.Key] = customFormation.Value.Formation.ToArray();
		}
	}

	public void InitCurrentFormation()
	{
		for (int i = 0; i < FormationData.g_formations[TacticsController.GetInstance().GetTacticsReference().inst.formationid].Length; i++)
		{
			int x = FormationData.g_formations[TacticsController.GetInstance().GetTacticsReference().inst.formationid][i].pos[0];
			int y = FormationData.g_formations[TacticsController.GetInstance().GetTacticsReference().inst.formationid][i].pos[1];
			CurrentFormation[i] = new FormationPosData(x, y);
		}
	}

	public void SoftSaveCustomFormation()
	{
		if (!Game.instance.ManagedClubProfile.CustomFormations.ContainsKey(31))
		{
			Game.instance.ManagedClubProfile.CustomFormations.Add(31, new CustomFormation());
			Game.instance.ManagedClubProfile.CustomFormations[31].Name = FormationData.GetName(TacticsController.GetInstance().GetTacticsReference().inst.formationid) + "*";
		}
		for (int i = 0; i < CurrentFormation.Length; i++)
		{
			int num = CurrentFormation[i].pos[0];
			int num2 = CurrentFormation[i].pos[1];
			Game.instance.ManagedClubProfile.CustomFormations[31].Formation[i].pos[0] = num;
			Game.instance.ManagedClubProfile.CustomFormations[31].Formation[i].pos[1] = num2;
			FormationData.g_formations[31][i].pos[0] = num;
			FormationData.g_formations[31][i].pos[1] = num2;
		}
		TacticsController.GetInstance().GetTacticsReference().inst.formationid = 31;
		Game.instance.ManagedClubProfile.CustomFormations[31].Instructions.copy(TacticsController.GetInstance().GetTacticsReference().inst);
		TacticsController.GetInstance().tacticsScreen.UpdateSoftSaveUI();
		FormationHelperFactory.UpdateFormationHelperCustom(31);
	}

	public void SetCurrentWingAndPositionCounts()
	{
		SetCurrentWingCount();
		SetCurrentPositionCount();
	}

	private void SetCurrentWingCount()
	{
		currLeftWingCount = Enumerable.Count(TacticSpaces, (TacticsGridSpace i) => i.occupied && i.AssignedGridSection == TacticsGridSpace.GridSection.Left);
		currRightWingCount = Enumerable.Count(TacticSpaces, (TacticsGridSpace i) => i.occupied && i.AssignedGridSection == TacticsGridSpace.GridSection.Right);
	}

	private void SetCurrentPositionCount()
	{
		currDefenceCount = Enumerable.Count(TacticSpaces, (TacticsGridSpace i) => i.occupied && i.AssignedPositionSection == TacticsGridSpace.PositionSection.Defence);
		currMidfieldCount = Enumerable.Count(TacticSpaces, (TacticsGridSpace i) => i.occupied && i.AssignedPositionSection == TacticsGridSpace.PositionSection.Midfielder);
		currForwardCount = Enumerable.Count(TacticSpaces, (TacticsGridSpace i) => i.occupied && i.AssignedPositionSection == TacticsGridSpace.PositionSection.Forward);
	}

	public static int GetColumnCount(int column)
	{
		return Enumerable.Count(Instance.TacticSpaces, (TacticsGridSpace i) => i.occupied && i.positionY == column);
	}

	public static int GetWingCount(TacticsGridSpace.GridSection gridSection)
	{
		return gridSection switch
		{
			TacticsGridSpace.GridSection.Left => GetCurrentLeftWingCount(), 
			TacticsGridSpace.GridSection.Right => GetCurrentRightWingCount(), 
			_ => 0, 
		};
	}

	public static int GetCurrentLeftWingCount()
	{
		return Instance.currLeftWingCount;
	}

	public static int GetCurrentRightWingCount()
	{
		return Instance.currRightWingCount;
	}

	public static int GetMinimumPositionCount(TacticsGridSpace.PositionSection positionSection)
	{
		return positionSection switch
		{
			TacticsGridSpace.PositionSection.Defence => 2, 
			TacticsGridSpace.PositionSection.Midfielder => 2, 
			TacticsGridSpace.PositionSection.Forward => 1, 
			_ => 0, 
		};
	}

	public static int GetPositionCount(TacticsGridSpace.PositionSection positionSection)
	{
		return positionSection switch
		{
			TacticsGridSpace.PositionSection.Defence => GetCurrentDefenceCount(), 
			TacticsGridSpace.PositionSection.Midfielder => GetCurrentMidfieldCount(), 
			TacticsGridSpace.PositionSection.Forward => GetCurrentForwardCount(), 
			_ => 0, 
		};
	}

	public static int GetCurrentDefenceCount()
	{
		return Instance.currDefenceCount;
	}

	public static int GetCurrentMidfieldCount()
	{
		return Instance.currMidfieldCount;
	}

	public static int GetCurrentForwardCount()
	{
		return Instance.currForwardCount;
	}

	public void UpdateCurrentFormation(int formationIndex, TacticsGridSpace tacticSpace)
	{
		CurrentFormation[formationIndex].pos[0] = tacticSpace.positionX;
		CurrentFormation[formationIndex].pos[1] = tacticSpace.positionY;
		SetCurrentWingCount();
		SetCurrentPositionCount();
		CheckFormationExists();
	}

	public void CheckFormationExists()
	{
		bool flag = false;
		for (int i = 0; i < FormationData.g_formations.Length - 1; i++)
		{
			if (FormationData.g_formations[i].Length == 0)
			{
				continue;
			}
			for (int j = 0; j < FormationData.g_formations[i].Length; j++)
			{
				for (int k = 0; k < 11; k++)
				{
					flag = Enumerable.SequenceEqual(FormationData.g_formations[i][j].pos, CurrentFormation[k].pos);
					if (flag)
					{
						break;
					}
				}
				if (!flag)
				{
					break;
				}
			}
			if (flag)
			{
				Game.LogConsole("FORMATION EXISTS " + FormationData.GetName(i));
				if (!TacticsController.IsMatchTactics)
				{
					Game.instance.ManagedClubProfile.CustomFormations.Remove(31);
				}
				TacticsController.GetInstance().SetFormationID(i);
				break;
			}
		}
		if (!flag)
		{
			Game.LogConsole("CUSTOM FORMATION");
			if (!TacticsController.IsMatchTactics)
			{
				SoftSaveCustomFormation();
			}
		}
	}

	public static void ShowPositionError(TacticsGridSpace.PositionSection positionSection)
	{
		switch (positionSection)
		{
		case TacticsGridSpace.PositionSection.Defence:
			Helpers.ShowToast(Translation.GetText("cust-tactics-defence").Replace("{x}", 2.ToString()));
			break;
		case TacticsGridSpace.PositionSection.Midfielder:
			Helpers.ShowToast(Translation.GetText("cust-tactics-midfield").Replace("{x}", 2.ToString()));
			break;
		case TacticsGridSpace.PositionSection.Forward:
			Helpers.ShowToast(Translation.GetText("cust-tactics-forward").Replace("{x}", 1.ToString()));
			break;
		}
	}

	private int FindCustomFormationSaveSlot()
	{
		for (int i = 0; i < CustomFormationIDs.Count; i++)
		{
			int num = CustomFormationIDs[i];
			if (!Game.instance.ManagedClubProfile.CustomFormations.ContainsKey(num))
			{
				return num;
			}
		}
		return 0;
	}

	public static void ShowWingError(TacticsGridSpace.GridSection gridSection)
	{
		switch (gridSection)
		{
		case TacticsGridSpace.GridSection.Left:
			Helpers.ShowToast(Translation.GetText("cust-tactics-players-left"));
			break;
		case TacticsGridSpace.GridSection.Right:
			Helpers.ShowToast(Translation.GetText("cust-tactics-players-right"));
			break;
		}
	}

	public static void ShowColumnError()
	{
		Helpers.ShowToast(Translation.GetText("cust-tactics-players-line"));
	}
}
