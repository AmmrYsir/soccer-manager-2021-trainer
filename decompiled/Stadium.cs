using System;
using UnityEngine;

public static class Stadium
{
	public const int MAX_CORNER_LEVEL = 1;

	public const int MAX_STAND_LEVEL = 8;

	public const int MAX_STADIUM_CAPACITY = 128000;

	public static StadiumPiece[] yourStadium;

	public static int yourGrassCut;

	public static bool yourStadiumGenerated;

	public static void ProcessStadiumUpgrade()
	{
		Game instance = Game.instance;
		if (instance.g_StadiumUpgradeInProgress && DateTime.Compare(instance.g_StadiumPieceConstructionFinish, instance.CurrentDate) <= 0)
		{
			StadiumPiece underConstructionPiece = StadiumScreen.GetUnderConstructionPiece();
			int num = StadiumStaticData.GetStadiumPieceCapacity(underConstructionPiece.pieceLevel, underConstructionPiece.pieceLocation) - StadiumStaticData.GetStadiumPieceCapacity(instance.g_PrevStandLevel, underConstructionPiece.pieceLocation);
			for (int i = 0; i < yourStadium.Length; i++)
			{
				yourStadium[i].underConstruction = false;
			}
			instance.g_AddedStadiumSeats += num;
			instance.g_StadiumUpgradeInProgress = false;
			instance.g_BuildingSeats = 0;
			Game.instance.CurrentManagedClub.Attendance = Economy.CalcClubAttendance(Game.instance.CurrentManagedClubId);
			Game.instance.ManagedClubProfile.CheckObjectiveProgress(ClubObjectiveSubType.UpgradeStadiumCapacity);
			Game.instance.ManagedClubProfile.CheckObjectiveProgress(ClubObjectiveSubType.UpgradeStadiumLong);
			if (!Game.ShowOffineText)
			{
				MessageManager.StadiumExpansionCompletion();
			}
		}
	}

	public static StadiumPiece[] SetupStadium(short clubID = -1, bool forceRegen = false)
	{
		bool flag = false;
		StadiumPiece[] array = new StadiumPiece[8];
		for (int i = 0; i < array.Length; i++)
		{
			array[i] = new StadiumPiece((StadiumGenerator.StadiumPos)i);
		}
		short num = 0;
		if (clubID == -1)
		{
			num = Game.instance.CurrentManagedClubId;
			flag = true;
		}
		else
		{
			num = clubID;
		}
		if (!forceRegen && (yourStadiumGenerated & flag))
		{
			return yourStadium;
		}
		short clubDataID = Club.GetClubDataID(num);
		StadiumStaticData.Load();
		StadiumStaticData.StadiumBuilding stadiumBuilding = ((!StadiumStaticData.StadiumList.ContainsKey(clubDataID)) ? StadiumStaticData.GetFallbackStadium(num) : StadiumStaticData.StadiumList[clubDataID]);
		for (int j = 1; j < stadiumBuilding.stands.Length; j += 2)
		{
			if (stadiumBuilding.stands[j] == 1)
			{
				stadiumBuilding.stands[j] = 2;
			}
		}
		for (int k = 0; k < array.Length; k++)
		{
			array[k].pieceLevel = stadiumBuilding.stands[k];
			array[k].roofType = stadiumBuilding.roof[k];
		}
		array = CalculateEndCaps(array);
		if (flag | forceRegen)
		{
			yourStadium = array;
			Game.instance.g_StadiumPieces = yourStadium;
			Game.instance.g_StadiumUpgradeInProgress = false;
			Game.instance.g_AddedStadiumSeats = 0;
			Game.instance.g_BuildingSeats = 0;
			yourGrassCut = 0;
			PlayerPrefs.SetInt("GrassCutType" + Save.currentSlot, 0);
			yourStadiumGenerated = true;
		}
		return array;
	}

	public static StadiumPiece[] CalculateEndCaps(StadiumPiece[] pieces)
	{
		for (int i = 0; i < pieces.Length; i += 2)
		{
			if (i == 0)
			{
				StadiumPiece stadiumPiece = pieces[pieces.Length - 1];
				if (stadiumPiece.pieceLevel > 0 && !stadiumPiece.underConstruction)
				{
					pieces[i].leftCap = false;
				}
				StadiumPiece stadiumPiece2 = pieces[i + 1];
				if (stadiumPiece2.pieceLevel > 0 && !stadiumPiece2.underConstruction)
				{
					pieces[i].rightCap = false;
				}
			}
			else if (i != pieces.Length - 1)
			{
				int num = 0;
				if (pieces[i].isCorner)
				{
					num = 1;
				}
				StadiumPiece stadiumPiece3 = pieces[i - 1];
				if (stadiumPiece3.pieceLevel > num && !stadiumPiece3.underConstruction)
				{
					pieces[i].leftCap = false;
				}
				StadiumPiece stadiumPiece4 = pieces[i + 1];
				if (stadiumPiece4.pieceLevel > num && !stadiumPiece4.underConstruction)
				{
					pieces[i].rightCap = false;
				}
			}
		}
		return pieces;
	}

	public static void UpgradePiece(StadiumPiece stadiumPiece, int level)
	{
		yourStadium[(int)stadiumPiece.pieceLocation].underConstruction = true;
		Game.instance.g_PrevStandLevel = stadiumPiece.pieceLevel;
		yourStadium[(int)stadiumPiece.pieceLocation].pieceLevel = level;
		if (yourStadium[(int)stadiumPiece.pieceLocation].roofType == 0 && ShouldPieceHaveRoof(yourStadium))
		{
			yourStadium[(int)stadiumPiece.pieceLocation].roofType = 1;
		}
		yourStadium = CalculateEndCaps(yourStadium);
	}

	public static void SetStadiumPieceAsComplete()
	{
	}

	public static bool ShouldPieceHaveRoof(StadiumPiece[] pieces)
	{
		int num = 0;
		for (int i = 0; i < pieces.Length; i += 2)
		{
			if (pieces[i].roofType == 1)
			{
				num++;
			}
		}
		if (num < 2)
		{
			return false;
		}
		return true;
	}
}
