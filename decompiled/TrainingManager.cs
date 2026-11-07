using System;
using System.Collections.Generic;
using System.Linq;

public class TrainingManager
{
	public List<Training.TrainingSession> Sessions = new List<Training.TrainingSession>
	{
		Training.TrainingSession.AVAILABLE,
		Training.TrainingSession.AVAILABLE,
		Training.TrainingSession.LOCKED,
		Training.TrainingSession.LOCKED,
		Training.TrainingSession.LOCKED,
		Training.TrainingSession.LOCKED,
		Training.TrainingSession.LOCKED
	};

	public Training.TrainingSession ManagerTraitSession = Training.TrainingSession.LOCKED;

	public DateTime LastSessionDate;

	public int SessionCount;

	private int SessionCap = 2;

	public TrainingReportSummary LastReportSummary = new TrainingReportSummary();

	public List<TrainingPlayerChanges> PlayerChanges = new List<TrainingPlayerChanges>();

	public Dictionary<int, DateTime> ImprovedPlayers = new Dictionary<int, DateTime>();

	public static int TrainingCreditStartCost = 50;

	public int TrainingCreditCost = 50;

	public bool UsedRewardAd;

	public int TotalGameTime;

	public float CapacityPen;

	public int DebugYouthTraining;

	public static void CalcTotalGameTime()
	{
		ChosenLeague leagueFromSeason = Helpers.GetLeagueFromSeason(Club.GetLeagueDataId(Game.GetCurrentUserClubID()));
		Game.instance.g_trainingManager.TotalGameTime = leagueFromSeason.GetNumGamesPlayed(Game.GetCurrentUserClubID()) * 90;
	}

	public static void CalcCapacityPen()
	{
		Game.instance.g_trainingManager.CapacityPen = 1f + Training.GetCapacityDecrease() / 100f;
	}

	public static bool HasUsedRewardAd()
	{
		if (Save.saveVersion >= 3)
		{
			return Game.instance.g_trainingManager.UsedRewardAd;
		}
		return false;
	}

	public static void SetUsedRewardAd(bool used)
	{
		if (Save.saveVersion >= 3)
		{
			Game.instance.g_trainingManager.UsedRewardAd = used;
		}
	}

	public static void AddImprovedPlayer(int playerID)
	{
		if (Player.GetRatingState(playerID) != PlayerRatingState.INCREASING)
		{
			return;
		}
		if (Game.instance.g_trainingManager.ImprovedPlayers.ContainsKey(playerID))
		{
			Game.instance.g_trainingManager.ImprovedPlayers.Remove(playerID);
		}
		foreach (int item in Game.instance.g_trainingManager.ImprovedPlayers.Keys.ToList())
		{
			if (Game.instance.g_trainingManager.ImprovedPlayers[item] < Game.instance.CurrentDate.AddMonths(-1))
			{
				Game.instance.g_trainingManager.ImprovedPlayers.Remove(item);
			}
		}
		Game.instance.g_trainingManager.ImprovedPlayers.Add(playerID, Game.instance.CurrentDate);
	}

	public bool IsTrainingAvailable()
	{
		if ((Game.instance.g_trainingManager.LastSessionDate.AddDays(7.0) - Game.instance.CurrentDate).Days <= 0)
		{
			return true;
		}
		return false;
	}

	public bool CanAddSession()
	{
		return SessionCount >= SessionCap;
	}

	public int GetSessionCap()
	{
		return Facilities.GetValueForNonStarUpgrade(0, Facilities.GetFacilityLevel(2));
	}

	public bool SessionAtLimit(Training.TrainingSession sel)
	{
		int num = 0;
		foreach (Training.TrainingSession session in Sessions)
		{
			if (session == sel)
			{
				num++;
			}
		}
		if (Game.instance.g_trainingManager.ManagerTraitSession == sel)
		{
			num++;
		}
		return num >= TrainingConfig.SessionLimit;
	}

	public void AddSession(Training.TrainingSession sessionType, int index)
	{
		if (CanAddSession())
		{
			Sessions[index] = sessionType;
			SessionCount++;
		}
	}

	public void RemoveSession(int index)
	{
		Sessions[index] = Training.TrainingSession.AVAILABLE;
	}

	public void EmptySessions()
	{
		for (int i = 0; i < SessionCap; i++)
		{
			Sessions[i] = Training.TrainingSession.AVAILABLE;
		}
	}

	public void SetDrillsAvailable()
	{
		int sessionCap = GetSessionCap();
		for (int i = 0; i < sessionCap; i++)
		{
			if (Sessions[i] == Training.TrainingSession.LOCKED)
			{
				Sessions[i] = Training.TrainingSession.AVAILABLE;
			}
		}
	}

	public void ResetSessions()
	{
		for (int i = 0; i < 10; i++)
		{
			if (i < 3)
			{
				Sessions[i] = Training.TrainingSession.AVAILABLE;
			}
			else
			{
				Sessions[i] = Training.TrainingSession.LOCKED;
			}
		}
	}

	public static int[] GetIncreaseAmounts()
	{
		int[] array = new int[6];
		for (int i = 0; i < Game.instance.g_trainingManager.Sessions.Count; i++)
		{
			Training.TrainingSession trainingSession = Game.instance.g_trainingManager.Sessions[i];
			if (trainingSession != Training.TrainingSession.AVAILABLE && trainingSession != Training.TrainingSession.LOCKED)
			{
				array[(int)(trainingSession - 2)]++;
			}
		}
		if (Game.instance.g_trainingManager.ManagerTraitSession - 2 > (Training.TrainingSession)(-1) && (int)(Game.instance.g_trainingManager.ManagerTraitSession - 2) < array.Count() && UserManagerProfileController.UserHasTrait(ManagerTraitArea.TrainingSlot) && Game.instance.g_trainingManager.ManagerTraitSession != Training.TrainingSession.AVAILABLE && Game.instance.g_trainingManager.ManagerTraitSession != Training.TrainingSession.LOCKED)
		{
			array[(int)(Game.instance.g_trainingManager.ManagerTraitSession - 2)] += UserManagerProfileController.GetCurrentTraitLevel(ManagerTraitArea.TrainingSlot) + 1;
		}
		return array;
	}

	public int GetFitnessIncrease()
	{
		int num = 2;
		for (int i = 0; i < Game.instance.g_trainingManager.Sessions.Count; i++)
		{
			num += Training.GetSessionFitnessIncrease(Game.instance.g_trainingManager.Sessions[i]);
		}
		if (UserManagerProfileController.UserHasTrait(ManagerTraitArea.TrainingSlot) && Game.instance.g_trainingManager.ManagerTraitSession != Training.TrainingSession.AVAILABLE)
		{
			num += Training.GetSessionFitnessIncrease(Game.instance.g_trainingManager.ManagerTraitSession);
		}
		if (num > 10)
		{
			num = 10;
		}
		return num;
	}
}
