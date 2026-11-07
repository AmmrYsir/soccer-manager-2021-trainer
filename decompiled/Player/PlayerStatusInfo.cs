using System;
using System.Collections.Generic;

public class PlayerStatusInfo
{
	public byte shirtNum { get; set; }

	public byte fitness { get; set; } = 100;

	public byte morale { get; set; } = 4;

	public byte moraleBoost { get; set; }

	public DateTime moraleBoostDate { get; set; } = DateTime.MinValue;

	public bool firstTeam { get; set; }

	public bool retirementWarned { get; set; }

	public DateTime lastBidDate { get; set; } = DateTime.MinValue;

	public bool IsInstantPurchase { get; set; }

	public PlayerTeamCohesion Cohesion { get; set; } = new PlayerTeamCohesion();

	public PlayerConcernsManagedClub Concerns { get; set; } = new PlayerConcernsManagedClub();

	public ePlayerSquadStatus ExpectedSquadStatus { get; set; }

	public static void Init(Dictionary<int, PlayerStatusInfo> playerStatus, int playerid, byte shirtnum, bool isTransfer)
	{
		if (shirtnum == 0)
		{
			shirtnum = 1;
			foreach (int key in playerStatus.Keys)
			{
				if (playerStatus[key].shirtNum > shirtnum)
				{
					shirtnum = playerStatus[key].shirtNum;
				}
			}
		}
		if (!playerStatus.ContainsKey(playerid))
		{
			playerStatus.Add(playerid, new PlayerStatusInfo());
		}
		playerStatus[playerid].shirtNum = shirtnum;
		playerStatus[playerid].fitness = Player.CalcFitness(playerid);
		if (Game.instance.CurrentDate == Game.instance.StartDate)
		{
			playerStatus[playerid].fitness -= 10;
		}
		playerStatus[playerid].morale = PlayerData.CalcMorale(playerid);
		if (isTransfer)
		{
			playerStatus[playerid].Cohesion.SetTransferCohesion(playerid);
		}
		else if (!UI.GetInstance().instantMatchMode)
		{
			playerStatus[playerid].Cohesion.SetStartingCohesion(playerid);
		}
	}

	public void reduceFitnessForInjury(int _length)
	{
		if (fitness > 50)
		{
			fitness -= 10;
			fitness -= (byte)Math.Min(_length + 1, fitness);
			if (fitness < 40)
			{
				fitness = (byte)(40 + Game.instance.Random.GetRand() % 10);
			}
		}
	}
}
