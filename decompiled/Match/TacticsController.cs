using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TacticsController
{
	public enum TacticSwapType
	{
		Lineup,
		LineupSub,
		SubLineup,
		LineupSquad,
		SquadLineup,
		SubSquad,
		SquadSub,
		Sub,
		Squad
	}

	public enum TacticsInstructionRole
	{
		NONE,
		CAPTAIN,
		PEN_TAKER,
		FK_LEFT,
		FK_RIGHT,
		CORNER_LEFT,
		CORNER_RIGHT
	}

	public enum TacticsUsage
	{
		LINEUP,
		SUB,
		SQUAD,
		ROLES
	}

	public static TacticsController Instance;

	private byte formationID;

	private byte widthStyle;

	private float mentalityValue;

	private float backlineValue;

	private int captainPlayerID;

	private int captainLineupIndex;

	private int penaltyPlayerID;

	private int penaltyLineupIndex;

	private int freeKickLeftPlayerID;

	private int freeKickLeftLineupIndex;

	private int freeKickRightPlayerID;

	private int freeKickRightLineupIndex;

	private int cornerLeftPlayerID;

	private int cornerLeftLineupIndex;

	private int cornerRightPlayerID;

	private int cornerRightLineupIndex;

	public cTactics MainSceneTactics;

	public cPlayerStats[] MatchPlayerStats;

	public cTactics MatchTactics;

	public cTactics MatchPlayerIdTactics;

	public cTactics ModifiedMatchTactics;

	public cTactics ModifiedMatchPlayerIdTactics;

	public static bool IsMatchTactics;

	public List<int> squadPlayers = new List<int>();

	private List<int> subPlayers = new List<int>();

	private int subsUsed;

	private int pendingSubCount;

	private List<int> pendingSubs = new List<int>();

	public List<TacticsPlayer> TacticsPlayers = new List<TacticsPlayer>();

	public TacticsScreen tacticsScreen;

	public TacticsInstructionsScreen tacticsInstructionScreen;

	public static bool ReInitialise;

	public static bool TacticsPending;

	public NextFixtureInfo NextFixture;

	public cTactics GetTacticsReference()
	{
		if (IsMatchTactics)
		{
			return ModifiedMatchTactics;
		}
		return MainSceneTactics;
	}

	public cTactics GetPlayerIDTactics()
	{
		if (IsMatchTactics)
		{
			return ModifiedMatchPlayerIdTactics;
		}
		if (MainSceneTactics == null)
		{
			MainSceneTactics = Game.instance.managedClub[Game.instance.g_currUser].currentTactics;
			return MainSceneTactics;
		}
		return MainSceneTactics;
	}

	public static TacticsController GetInstance()
	{
		if (Instance != null)
		{
			return Instance;
		}
		Instance = new TacticsController();
		return Instance;
	}

	public void InitMainSceneTactics()
	{
		MainSceneTactics = Game.instance.managedClub[Game.instance.g_currUser].currentTactics;
		formationID = Game.instance.managedClub[Game.instance.g_currUser].currentTactics.inst.formationid;
		widthStyle = Game.instance.managedClub[Game.instance.g_currUser].currentTactics.inst.width.value;
		mentalityValue = (int)Game.instance.managedClub[Game.instance.g_currUser].currentTactics.inst.mentality.value;
		backlineValue = (int)Game.instance.managedClub[Game.instance.g_currUser].currentTactics.inst.backline.value;
		TacticsPlayers.Clear();
		CustomFormationController.GetInstance().TacticSpaces.Clear();
		squadPlayers = PlayerSquad.GetPlayers(Game.instance.managedClub[Game.instance.g_currUser].ClubID).players.Except(MainSceneTactics.lineUp).ToList();
		squadPlayers = squadPlayers.Where((int i) => Player.GetStatus(i).firstTeam).ToList();
		squadPlayers.Sort(CustomSort.SortByRatingDesc);
		squadPlayers = squadPlayers.Take(7).ToList();
		NextFixture = Game.instance.GetNextFixture(Game.instance.managedClub[Game.instance.g_currUser].ClubID);
		string name = "TacticsScreenPitch(Clone)";
		tacticsScreen = GameObject.Find(name).GetComponent<TacticsScreen>();
		tacticsInstructionScreen = GameObject.Find("TacticsInstructionsScreen").GetComponent<TacticsInstructionsScreen>();
		SetupInstructionRoles();
		CustomFormationController.GetInstance().InitCurrentFormation();
	}

	public void InitMatchTactics(cTactics matchTactics, cPlayerStats[] playerStats)
	{
		TacticsPlayers.Clear();
		CustomFormationController.GetInstance().TacticSpaces.Clear();
		MatchTactics = matchTactics;
		MatchPlayerStats = playerStats;
		MatchPlayerIdTactics = new cTactics();
		Array.Copy(Game.instance.CurrentManagedClub.currentTactics.lineUp, MatchPlayerIdTactics.lineUp, Game.instance.CurrentManagedClub.currentTactics.lineUp.Length);
		MatchPlayerIdTactics.inst = Game.instance.CurrentManagedClub.currentTactics.inst;
		ModifiedMatchPlayerIdTactics = new cTactics();
		Array.Copy(MatchPlayerIdTactics.lineUp, ModifiedMatchPlayerIdTactics.lineUp, MatchPlayerIdTactics.lineUp.Length);
		MatchTactics.inst.copy(MatchPlayerIdTactics.inst);
		ModifiedMatchTactics = new cTactics(cleanCopy: true);
		ModifiedMatchTactics.inst.copy(MatchTactics.inst);
		Array.Copy(MatchTactics.lineUp, ModifiedMatchTactics.lineUp, MatchTactics.lineUp.Length);
		subPlayers = MatchPlayerIdTactics.lineUp.Skip(11).Take(8).ToList();
		CustomFormationController.GetInstance().InitCurrentFormation();
		tacticsScreen = GameObject.Find("OverlayCanvasMatch").transform.Find("TacticsScreen").GetComponent<TacticsScreen>();
		tacticsScreen.InitTacticsScreen();
		tacticsInstructionScreen = tacticsScreen.transform.Find("ScreenContainer/ScreenContent/ModalPanel/TacticsInstructionsScreen").GetComponent<TacticsInstructionsScreen>();
		tacticsInstructionScreen.InitInsructionScreen();
		SetupInstructionRoles();
		pendingSubCount = 0;
		pendingSubs.Clear();
		subsUsed = 0;
	}

	public void UpdateAllPlayersMatchStats()
	{
		for (int i = 0; i < TacticsPlayers.Count; i++)
		{
			TacticsPlayers[i].UpdateMatchStats();
		}
	}

	public void ConfirmedTactics()
	{
		if (pendingSubs.Count > 0)
		{
			subsUsed += pendingSubs.Count;
			int i;
			for (i = 0; i < pendingSubs.Count; i++)
			{
				TacticsPlayers.FirstOrDefault((TacticsPlayer p) => p.PlayerID == pendingSubs[i]).SubbedOff = true;
			}
			pendingSubs.Clear();
			pendingSubCount = 0;
		}
		Array.Copy(ModifiedMatchPlayerIdTactics.lineUp, MatchPlayerIdTactics.lineUp, ModifiedMatchPlayerIdTactics.lineUp.Length);
		subPlayers.RemoveAll((int item) => MatchPlayerIdTactics.lineUp.ToList().GetRange(0, 11).Contains(item));
		SetTacticsAreas();
		TacticsPending = false;
	}

	public void CancelTactics()
	{
		ModifiedMatchTactics.inst.copy(MatchTactics.inst);
		Array.Copy(MatchTactics.lineUp, ModifiedMatchTactics.lineUp, MatchTactics.lineUp.Length);
		Array.Copy(MatchPlayerIdTactics.lineUp, ModifiedMatchPlayerIdTactics.lineUp, MatchPlayerIdTactics.lineUp.Length);
		tacticsScreen.ReInitialiseTactics();
		for (int i = 0; i < TacticsPlayers.Count; i++)
		{
			if (pendingSubs.Contains(TacticsPlayers[i].PlayerID))
			{
				TacticsPlayers[i].SetSubOnImage(on: false);
			}
			if (subPlayers.Contains(TacticsPlayers[i].PlayerID))
			{
				TacticsPlayers[i].SetSubOffImage(on: false);
			}
		}
		pendingSubCount = 0;
		pendingSubs.Clear();
		TacticsPending = false;
	}

	public bool MatchTacticsChanged()
	{
		bool flag = MatchTactics.inst.equal(ModifiedMatchTactics.inst);
		bool flag2 = Enumerable.SequenceEqual(MatchTactics.lineUp, ModifiedMatchTactics.lineUp);
		return flag | flag2;
	}

	public float GetTotalTeamCohesion()
	{
		float num = 0f;
		for (int i = 0; i < TacticsPlayers.Count; i++)
		{
			if (TacticsPlayers[i].LineupIndex < 11 && TacticsPlayers[i].LineupIndex != -1)
			{
				num += (float)TacticsPlayers[i].PlayerCohesion;
			}
		}
		return Mathf.Round(num / 11f);
	}

	private bool SubsLeft()
	{
		return subsUsed + pendingSubCount < 3;
	}

	private bool PendingSubsLeft()
	{
		return pendingSubCount < 3;
	}

	public void SetPlayerYellowCards(int playerID)
	{
		TacticsPlayers.FirstOrDefault((TacticsPlayer i) => i.PlayerID == playerID).YellowCardCount++;
	}

	public void SetPlayerToSentOff(int playerID)
	{
		TacticsPlayers.FirstOrDefault((TacticsPlayer i) => i.PlayerID == playerID).SentOff = true;
	}

	public void SetPlayerInjured(int playerID)
	{
		TacticsPlayers.FirstOrDefault((TacticsPlayer i) => i.PlayerID == playerID).Injured = true;
	}

	public void SetFormationID(int formationID)
	{
		GetTacticsReference().inst.formationid = (byte)formationID;
		tacticsScreen.ChangeFormation((byte)formationID);
	}

	private void SetupInstructionRoles()
	{
		captainPlayerID = GetTacticsReference().inst.captainid;
		penaltyPlayerID = GetTacticsReference().inst.penaltytakerid;
		freeKickLeftPlayerID = GetTacticsReference().inst.deadballtakerleftid;
		freeKickRightPlayerID = GetTacticsReference().inst.deadballtakerrightid;
		cornerLeftPlayerID = GetTacticsReference().inst.cornertakerleftid;
		cornerRightPlayerID = GetTacticsReference().inst.cornertakerrightid;
		captainLineupIndex = Array.IndexOf(GetTacticsReference().lineUp, captainPlayerID);
		penaltyLineupIndex = Array.IndexOf(GetTacticsReference().lineUp, penaltyPlayerID);
		freeKickLeftLineupIndex = Array.IndexOf(GetTacticsReference().lineUp, freeKickLeftPlayerID);
		freeKickRightLineupIndex = Array.IndexOf(GetTacticsReference().lineUp, freeKickRightPlayerID);
		cornerLeftLineupIndex = Array.IndexOf(GetTacticsReference().lineUp, cornerLeftPlayerID);
		cornerRightLineupIndex = Array.IndexOf(GetTacticsReference().lineUp, cornerRightPlayerID);
	}

	public List<Tactics.PlayerRoles> GetPlayerRoles(int lineupIndex)
	{
		Tactics.TacticalPos tacticalPosFromPosition = Tactics.GetTacticalPosFromPosition(FormationData.g_formations[GetTacticsReference().inst.formationid][lineupIndex].pos);
		return Tactics.AvailableRoles[tacticalPosFromPosition].ToList();
	}

	public List<Tactics.PlayerRoles> GetUpdatedPlayerRoles(int lineupIndex)
	{
		Tactics.TacticalPos tacticalPosFromPosition = Tactics.GetTacticalPosFromPosition(CustomFormationController.GetInstance().CurrentFormation[lineupIndex].pos);
		return Tactics.AvailableRoles[tacticalPosFromPosition].ToList();
	}

	public List<TacticsInstructionRole> GetInstructionRoles(int playerID)
	{
		List<TacticsInstructionRole> list = new List<TacticsInstructionRole>();
		if (captainPlayerID == playerID)
		{
			list.Add(TacticsInstructionRole.CAPTAIN);
		}
		if (penaltyPlayerID == playerID)
		{
			list.Add(TacticsInstructionRole.PEN_TAKER);
		}
		if (freeKickLeftPlayerID == playerID)
		{
			list.Add(TacticsInstructionRole.FK_LEFT);
		}
		if (freeKickRightPlayerID == playerID)
		{
			list.Add(TacticsInstructionRole.FK_RIGHT);
		}
		if (cornerLeftPlayerID == playerID)
		{
			list.Add(TacticsInstructionRole.CORNER_LEFT);
		}
		if (cornerRightPlayerID == playerID)
		{
			list.Add(TacticsInstructionRole.CORNER_RIGHT);
		}
		return list;
	}

	public void AssignInstructionRole(TacticsInstructionRole role, int lineupIndex)
	{
		int playerID = GetPlayerIDTactics().lineUp[lineupIndex];
		AddInstructionRole(role, playerID);
		SetPlayerInstructionID(role, playerID);
		SetPlayerInstructionIndex(role, (byte)lineupIndex);
	}

	public int GetPlayerRoleIndex(int lineupIndex)
	{
		return GetTacticsReference().inst.playerRoles[lineupIndex].value;
	}

	public void SetPlayerRole(int lineupIndex, int roleIndex)
	{
		GetTacticsReference().inst.playerRoles[lineupIndex].value = roleIndex;
	}

	private byte GetPlayerInstructionIndex(TacticsInstructionRole role)
	{
		return role switch
		{
			TacticsInstructionRole.CAPTAIN => GetTacticsReference().inst.captain.value, 
			TacticsInstructionRole.PEN_TAKER => GetTacticsReference().inst.penaltytaker.value, 
			TacticsInstructionRole.FK_LEFT => GetTacticsReference().inst.deadballtakerleft.value, 
			TacticsInstructionRole.FK_RIGHT => GetTacticsReference().inst.deadballtakerright.value, 
			TacticsInstructionRole.CORNER_LEFT => GetTacticsReference().inst.cornertakerleft.value, 
			TacticsInstructionRole.CORNER_RIGHT => GetTacticsReference().inst.cornertakerright.value, 
			_ => (byte)7, 
		};
	}

	private int GetPlayerInstructionID(TacticsInstructionRole role)
	{
		return role switch
		{
			TacticsInstructionRole.CAPTAIN => GetTacticsReference().inst.captainid, 
			TacticsInstructionRole.PEN_TAKER => GetTacticsReference().inst.penaltytakerid, 
			TacticsInstructionRole.FK_LEFT => GetTacticsReference().inst.deadballtakerleftid, 
			TacticsInstructionRole.FK_RIGHT => GetTacticsReference().inst.deadballtakerrightid, 
			TacticsInstructionRole.CORNER_LEFT => GetTacticsReference().inst.cornertakerleftid, 
			TacticsInstructionRole.CORNER_RIGHT => GetTacticsReference().inst.cornertakerrightid, 
			_ => 7, 
		};
	}

	private void SetPlayerInstructionIndex(TacticsInstructionRole role, byte playerIx)
	{
		switch (role)
		{
		case TacticsInstructionRole.CAPTAIN:
			GetTacticsReference().inst.captain.value = playerIx;
			break;
		case TacticsInstructionRole.PEN_TAKER:
			GetTacticsReference().inst.penaltytaker.value = playerIx;
			break;
		case TacticsInstructionRole.FK_LEFT:
			GetTacticsReference().inst.deadballtakerleft.value = playerIx;
			break;
		case TacticsInstructionRole.FK_RIGHT:
			GetTacticsReference().inst.deadballtakerright.value = playerIx;
			break;
		case TacticsInstructionRole.CORNER_LEFT:
			GetTacticsReference().inst.cornertakerleft.value = playerIx;
			break;
		case TacticsInstructionRole.CORNER_RIGHT:
			GetTacticsReference().inst.cornertakerright.value = playerIx;
			break;
		}
	}

	private void SetPlayerInstructionID(TacticsInstructionRole role, int playerID)
	{
		switch (role)
		{
		case TacticsInstructionRole.CAPTAIN:
			GetTacticsReference().inst.captainid = playerID;
			break;
		case TacticsInstructionRole.PEN_TAKER:
			GetTacticsReference().inst.penaltytakerid = playerID;
			break;
		case TacticsInstructionRole.FK_LEFT:
			GetTacticsReference().inst.deadballtakerleftid = playerID;
			break;
		case TacticsInstructionRole.FK_RIGHT:
			GetTacticsReference().inst.deadballtakerrightid = playerID;
			break;
		case TacticsInstructionRole.CORNER_LEFT:
			GetTacticsReference().inst.cornertakerleftid = playerID;
			break;
		case TacticsInstructionRole.CORNER_RIGHT:
			GetTacticsReference().inst.cornertakerrightid = playerID;
			break;
		}
	}

	private TacticSwapType GetSwapType(TacticsUsage player1Area, TacticsUsage player2Area)
	{
		if (player1Area == TacticsUsage.LINEUP && player2Area == TacticsUsage.LINEUP)
		{
			return TacticSwapType.Lineup;
		}
		if (player1Area == TacticsUsage.LINEUP && player2Area == TacticsUsage.SUB)
		{
			return TacticSwapType.LineupSub;
		}
		if (player1Area == TacticsUsage.SUB && player2Area == TacticsUsage.LINEUP)
		{
			return TacticSwapType.SubLineup;
		}
		if (player1Area == TacticsUsage.SUB && player2Area == TacticsUsage.SUB)
		{
			return TacticSwapType.Sub;
		}
		if (player1Area == TacticsUsage.SQUAD && player2Area == TacticsUsage.LINEUP)
		{
			return TacticSwapType.SquadLineup;
		}
		if (player1Area == TacticsUsage.LINEUP && player2Area == TacticsUsage.SQUAD)
		{
			return TacticSwapType.LineupSquad;
		}
		if (player1Area == TacticsUsage.SQUAD && player2Area == TacticsUsage.SUB)
		{
			return TacticSwapType.SquadSub;
		}
		if (player1Area == TacticsUsage.SUB && player2Area == TacticsUsage.SQUAD)
		{
			return TacticSwapType.SubSquad;
		}
		return TacticSwapType.Lineup;
	}

	public void SwapPlayer(TacticsPlayer player1, TacticsPlayer player2)
	{
		TacticSwapType swapType = GetSwapType(player1.TacticsArea, player2.TacticsArea);
		int lineupIndex = player1.LineupIndex;
		int lineupIndex2 = player2.LineupIndex;
		if (player1.TacticsArea != TacticsUsage.SQUAD && player2.TacticsArea != TacticsUsage.SQUAD)
		{
			MainSceneTactics.lineUp[player1.LineupIndex] = player2.PlayerID;
			MainSceneTactics.lineUp[player2.LineupIndex] = player1.PlayerID;
			player1.SetLineupIndex(lineupIndex2);
			player2.SetLineupIndex(lineupIndex);
			switch (swapType)
			{
			case TacticSwapType.LineupSub:
			case TacticSwapType.SubLineup:
				SwapInstructionRoles(player1, player2);
				SwapTacticsArea(player1, player2);
				break;
			case TacticSwapType.Lineup:
				SwapInstructionRoleIndex(player1, player2);
				break;
			}
		}
		else
		{
			switch (swapType)
			{
			case TacticSwapType.SquadLineup:
			case TacticSwapType.SquadSub:
				AddPlayerToSquad(player2.PlayerID, player1.PlayerID);
				AddPlayerToLineup(player1.PlayerID, player2.LineupIndex);
				break;
			case TacticSwapType.LineupSquad:
			case TacticSwapType.SubSquad:
				AddPlayerToSquad(player1.PlayerID, player2.PlayerID);
				AddPlayerToLineup(player2.PlayerID, player1.LineupIndex);
				break;
			}
			player1.SetLineupIndex(lineupIndex2);
			player2.SetLineupIndex(lineupIndex);
			SwapInstructionRoles(player1, player2);
			SwapTacticsArea(player1, player2);
		}
		player1.UpdatePlayer();
		player2.UpdatePlayer();
		tacticsInstructionScreen.UpdateRoleDropdowns();
		tacticsScreen.UpdateTeamCohesion(triggerAnim: true);
	}

	private void AddPlayerToSquad(int addPlayerID, int removePlayerID)
	{
		int index = squadPlayers.IndexOf(removePlayerID);
		squadPlayers[index] = addPlayerID;
	}

	private void AddPlayerToLineup(int playerID, int lineupIndex)
	{
		MainSceneTactics.lineUp[lineupIndex] = playerID;
	}

	public bool SwapPlayersMatch(TacticsPlayer player1, TacticsPlayer player2)
	{
		TacticSwapType swapType = GetSwapType(player1.TacticsArea, player2.TacticsArea);
		bool flag = pendingSubs.Contains(player1.PlayerID);
		bool flag2 = pendingSubs.Contains(player2.PlayerID);
		bool flag3 = subPlayers.Contains(player1.PlayerID);
		bool flag4 = subPlayers.Contains(player2.PlayerID);
		if (player1.TacticsArea == TacticsUsage.LINEUP)
		{
			_ = player2.TacticsArea == TacticsUsage.LINEUP;
		}
		else
			_ = 0;
		bool flag5 = false;
		bool flag6 = false;
		switch (swapType)
		{
		case TacticSwapType.Lineup:
			flag5 = true;
			SwapInstructionRoleIndex(player1, player2);
			break;
		case TacticSwapType.LineupSub:
			if (flag3 & flag2)
			{
				pendingSubCount--;
				pendingSubs.Remove(player2.PlayerID);
				player1.SetSubOnImage(on: false);
				player2.SetSubOffImage(on: false);
				flag5 = true;
				flag6 = true;
			}
			else if (!flag3 & flag2)
			{
				int index2 = pendingSubs.IndexOf(player2.PlayerID);
				pendingSubs[index2] = player1.PlayerID;
				player1.SetSubOffImage(on: true);
				player2.SetSubOffImage(on: false);
				flag5 = true;
				flag6 = true;
			}
			else if (flag3 & flag4)
			{
				player1.SetSubOnImage(on: false);
				player2.SetSubOnImage(on: true);
				flag5 = true;
				flag6 = true;
			}
			else if (SubsLeft())
			{
				pendingSubCount++;
				pendingSubs.Add(player1.PlayerID);
				player1.SetSubOffImage(on: true);
				player2.SetSubOnImage(on: true);
				flag6 = true;
				flag5 = true;
			}
			break;
		case TacticSwapType.SubLineup:
			if (flag4 & flag)
			{
				pendingSubCount--;
				pendingSubs.Remove(player1.PlayerID);
				player1.SetSubOffImage(on: false);
				player2.SetSubOnImage(on: false);
				flag5 = true;
				flag6 = true;
			}
			else if (!flag4 & flag)
			{
				int index = pendingSubs.IndexOf(player1.PlayerID);
				pendingSubs[index] = player2.PlayerID;
				player2.SetSubOffImage(on: true);
				player1.SetSubOffImage(on: false);
				flag5 = true;
				flag6 = true;
			}
			else if (flag3 & flag4)
			{
				player2.SetSubOnImage(on: false);
				player1.SetSubOnImage(on: true);
				flag5 = true;
				flag6 = true;
			}
			else if (SubsLeft())
			{
				pendingSubCount++;
				pendingSubs.Add(player2.PlayerID);
				player1.SetSubOnImage(on: true);
				player2.SetSubOffImage(on: true);
				flag6 = true;
				flag5 = true;
			}
			break;
		case TacticSwapType.Sub:
			flag5 = true;
			break;
		}
		if (flag5)
		{
			int num = Array.IndexOf(ModifiedMatchTactics.lineUp, player1.LineupIndex);
			int num2 = Array.IndexOf(ModifiedMatchTactics.lineUp, player2.LineupIndex);
			int lineupIndex = player1.LineupIndex;
			int lineupIndex2 = player2.LineupIndex;
			ModifiedMatchTactics.lineUp[num] = player2.LineupIndex;
			ModifiedMatchTactics.lineUp[num2] = player1.LineupIndex;
			ModifiedMatchPlayerIdTactics.lineUp[player1.LineupIndex] = player2.PlayerID;
			ModifiedMatchPlayerIdTactics.lineUp[player2.LineupIndex] = player1.PlayerID;
			player1.SetLineupIndex(lineupIndex2);
			player2.SetLineupIndex(lineupIndex);
			if (flag6)
			{
				SwapInstructionRoles(player1, player2);
			}
			SwapTacticsArea(player1, player2);
			player1.UpdatePlayer();
			player2.UpdatePlayer();
			tacticsInstructionScreen.UpdateRoleDropdowns();
		}
		else
		{
			Helpers.ShowToast(Translation.GetText("swaperr-used-subs"));
		}
		return flag5;
	}

	private void SwapInstructionRoleIndex(TacticsPlayer player1, TacticsPlayer player2)
	{
		if (player1.InstructionRoles.Count == 0 || player2.InstructionRoles.Count == 0)
		{
			return;
		}
		List<TacticsInstructionRole> list = player1.InstructionRoles.Concat(player2.InstructionRoles).ToList();
		for (int i = 0; i < list.Count; i++)
		{
			TacticsInstructionRole tacticsInstructionRole = list[i];
			int playerID = 0;
			int num = -1;
			if (player1.InstructionRoles.Contains(tacticsInstructionRole))
			{
				playerID = player2.PlayerID;
				num = player2.LineupIndex;
			}
			else if (player2.InstructionRoles.Contains(tacticsInstructionRole))
			{
				playerID = player1.PlayerID;
				num = player1.LineupIndex;
			}
			SetPlayerInstructionID(tacticsInstructionRole, playerID);
			SetPlayerInstructionIndex(tacticsInstructionRole, (byte)num);
		}
	}

	private void SwapInstructionRoles(TacticsPlayer player1, TacticsPlayer player2)
	{
		List<TacticsInstructionRole> list = new List<TacticsInstructionRole>();
		list.AddRange(player1.InstructionRoles);
		List<TacticsInstructionRole> list2 = new List<TacticsInstructionRole>();
		list2.AddRange(player2.InstructionRoles);
		for (int i = 0; i < player1.InstructionRoles.Count; i++)
		{
			TacticsInstructionRole role = player1.InstructionRoles[i];
			SetPlayerInstructionID(role, player2.PlayerID);
			SetPlayerInstructionIndex(role, (byte)player2.LineupIndex);
		}
		for (int j = 0; j < player2.InstructionRoles.Count; j++)
		{
			TacticsInstructionRole role2 = player2.InstructionRoles[j];
			SetPlayerInstructionID(role2, player1.PlayerID);
			SetPlayerInstructionIndex(role2, (byte)player1.LineupIndex);
		}
		player1.InstructionRoles = list2;
		player2.InstructionRoles = list;
		tacticsInstructionScreen.UpdateRoleDropdowns();
	}

	private void AddInstructionRole(TacticsInstructionRole role, int playerID)
	{
		if (TacticsPlayers.Any((TacticsPlayer i) => i.InstructionRoles.Contains(role)))
		{
			TacticsPlayers.FirstOrDefault((TacticsPlayer i) => i.InstructionRoles.Contains(role)).InstructionRoles.Remove(role);
		}
		TacticsPlayers.FirstOrDefault((TacticsPlayer i) => i.PlayerID == playerID).InstructionRoles.Add(role);
	}

	private void SwapTacticsArea(TacticsPlayer player1, TacticsPlayer player2)
	{
		TacticsUsage tacticsArea = player1.TacticsArea;
		TacticsUsage tacticsArea2 = player2.TacticsArea;
		player1.TacticsArea = tacticsArea2;
		player2.TacticsArea = tacticsArea;
	}

	private void SetTacticsAreas()
	{
		for (int i = 0; i < MatchTactics.lineUp.Length; i++)
		{
			if (TacticsPlayers[i].LineupIndex < 11)
			{
				TacticsPlayers[i].TacticsArea = TacticsUsage.LINEUP;
			}
			else
			{
				TacticsPlayers[i].TacticsArea = TacticsUsage.SUB;
			}
		}
	}

	public void AutoPickTeam()
	{
		Club.SquadHealthCheck(Game.instance.managedClub[Game.instance.g_currUser].ClubID);
		int compID = -1;
		int compType = -1;
		if (Game.instance.g_matchday_nextfix != null)
		{
			compID = Game.instance.g_matchday_nextfix.competitionid;
			compType = Game.instance.g_matchday_nextfix.competitiontype;
		}
		Team team = new Team();
		team.clubid = Game.instance.managedClub[Game.instance.g_currUser].ClubID;
		Team.PickTeam(team, GetTacticsReference().inst.formationid, 0, compType, compID, justFirstTeam: true);
		for (int i = 0; i < 18; i++)
		{
			GetPlayerIDTactics().lineUp[i] = team.lineup[i];
			if (i < 11)
			{
				Tactics.TacticalPos tacticalPosFromPosition = Tactics.GetTacticalPosFromPosition(FormationData.g_formations[GetTacticsReference().inst.formationid][i].pos);
				Tactics.PlayerRoles description = (Tactics.PlayerRoles)PlayerData.GetDescription(GetPlayerIDTactics().lineUp[i]);
				int num = Array.IndexOf(Tactics.AvailableRoles[tacticalPosFromPosition], description);
				GetTacticsReference().inst.playerRoles[i].value = ((num >= 0) ? num : 0);
			}
		}
		tacticsScreen.UpdateTacticsScreen();
	}

	public static int SwitchPosXFormationData(int posX)
	{
		return posX switch
		{
			6 => 3, 
			5 => 2, 
			4 => 1, 
			3 => 0, 
			2 => -1, 
			1 => -2, 
			0 => -3, 
			_ => 0, 
		};
	}
}
