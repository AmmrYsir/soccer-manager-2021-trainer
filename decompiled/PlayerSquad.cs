using System.Collections.Generic;
using UnityEngine;

public class PlayerSquad
{
	public static SquadStatusManager SquadStatusManager = new SquadStatusManager();

	public List<int> players { get; set; } = new List<int>();

	public List<int> loanedoutplayers { get; set; } = new List<int>();

	private SortableClubPlayerList _sortableplayerlist { get; } = new SortableClubPlayerList();

	public int Count => players.Count;

	public int this[int key] => players[key];

	public byte matchdayratings(int which)
	{
		return Game.instance.g_PlayerData[players[which]].CacheRating;
	}

	public static List<int> AllPlayers(short clubID)
	{
		List<int> list = new List<int>(Game.instance.g_ClubPlayer[clubID].players);
		list.AddRange(Game.instance.g_ClubPlayer[clubID].loanedoutplayers);
		return list;
	}

	public int IndexOf(int playerId)
	{
		return players.IndexOf(playerId);
	}

	public static PlayerSquad GetPlayers(short clubid)
	{
		if (!Game.instance.g_ClubPlayer.ContainsKey(clubid))
		{
			if (clubid == 0)
			{
				Game.instance.g_ClubPlayer[clubid] = new PlayerSquad();
				UnityEngine.Debug.LogError("PlayerSquad::GetPlayers() creating clubid 0");
				return Game.instance.g_ClubPlayer[clubid];
			}
			UnityEngine.Debug.LogError("PlayerSquad::GetPlayers() club id doesn't exist");
			return null;
		}
		return Game.instance.g_ClubPlayer[clubid];
	}

	public SortableClubPlayerList GetPlayersAsSortableListStatic()
	{
		_sortableplayerlist.Clear();
		foreach (int player in players)
		{
			_sortableplayerlist.Add(player);
		}
		return _sortableplayerlist;
	}

	public static List<int> GetLoanedOutPlayers(short clubid)
	{
		if (Game.instance.g_ClubPlayer.ContainsKey(clubid))
		{
			return Game.instance.g_ClubPlayer[clubid].loanedoutplayers;
		}
		return new List<int>();
	}

	public static void AddPlayer(short clubid, int playerid)
	{
		if (Game.instance.g_ClubPlayer.ContainsKey(clubid))
		{
			PlayerSquad playerSquad = Game.instance.g_ClubPlayer[clubid];
			if (!playerSquad.players.Contains(playerid))
			{
				playerSquad.players.Add(playerid);
				ClubPlayerSort.Sort(playerSquad);
			}
		}
	}

	public static void RemovePlayer(short clubid, int playerid)
	{
		PlayerSquad playerSquad = Game.instance.g_ClubPlayer[clubid];
		if (playerSquad.players.Contains(playerid))
		{
			playerSquad.players.Remove(playerid);
		}
		ClubPlayerSort.Sort(playerSquad);
	}

	public static void AddLoanedOutPlayer(short parentClubId, int playerId)
	{
		if (Game.instance.g_ClubPlayer.ContainsKey(parentClubId) && !Game.instance.g_ClubPlayer[parentClubId].loanedoutplayers.Contains(playerId))
		{
			Game.instance.g_ClubPlayer[parentClubId].loanedoutplayers.Add(playerId);
		}
	}

	public static void RemoveLoanedOutPlayer(short parentClubId, int playerId)
	{
		if (Game.instance.g_ClubPlayer.ContainsKey(parentClubId))
		{
			Game.instance.g_ClubPlayer[parentClubId].loanedoutplayers.Remove(playerId);
		}
	}

	public static void Debug(short clubID)
	{
		foreach (int player in Game.instance.g_ClubPlayer[clubID].players)
		{
			UnityEngine.Debug.Log("<color=red>" + Player.GetName(player) + " " + Player.GetRatingQuick(player) + "</color>");
		}
	}

	public static void Refresh()
	{
		foreach (PlayerSquad value in Game.instance.g_ClubPlayer.Values)
		{
			ClubPlayerSort.Sort(value);
		}
		SquadStatusManager.RefreshAllClubs();
	}

	public static void AddTrainingPlayers(short ClubId)
	{
		PlayerSquad playerSquad = Club.GetPlayers(ClubId);
		for (int i = 0; i < playerSquad.Count; i++)
		{
			Training.AddTrainingPlayer(playerSquad[i]);
		}
	}

	public static int GetClubNumberOfLoanedInPlayers(short clubId)
	{
		int num = 0;
		PlayerSquad playerSquad = Club.GetPlayers(clubId);
		for (int i = 0; i < playerSquad.Count; i++)
		{
			if (PlayerData.IsLoanedToClub(playerSquad[i]))
			{
				num++;
			}
		}
		return num;
	}

	public static bool HasEnoughPlayers(short ClubID, int playerID)
	{
		int position = Player.GetPosition(playerID, preferred: true);
		PlayerSquad playerSquad = Club.GetPlayers(ClubID);
		bool result = false;
		if (playerSquad.Count < 22)
		{
			result = true;
		}
		else if ((position & 1) != 0)
		{
			if (Helpers.GetPlayersByPos(playerSquad, 1).Count < 3)
			{
				result = true;
			}
		}
		else if ((position & 0x7E) != 0)
		{
			if (Helpers.GetPlayersByPos(playerSquad, 126).Count < 6)
			{
				result = true;
			}
		}
		else if ((position & 0x1FF0) != 0)
		{
			if (Helpers.GetPlayersByPos(playerSquad, 8176).Count < 8)
			{
				result = true;
			}
		}
		else if ((position & 0xE000) != 0 && Helpers.GetPlayersByPos(playerSquad, 57344).Count < 5)
		{
			result = true;
		}
		return result;
	}

	public int CountPlayersInPosition(int position)
	{
		int num = 0;
		foreach (int player in players)
		{
			if (Game.instance.g_PlayerData[player].Position == position)
			{
				num++;
			}
		}
		return num;
	}

	public void PlayerRetired(int playerID, short clubID)
	{
		if (PlayerData.IsGoalKeeper(playerID) && CountPlayersInPosition(1) < 2)
		{
			PlayerRespawn.giveRespawnToManagedClub(clubID, 1);
		}
		else if (Count < 22)
		{
			PlayerRespawn.giveRespawnToManagedClub(clubID, Helpers.GetPlayerBasicPositionMask(Player.GetPosition(playerID, preferred: true)));
		}
	}
}
