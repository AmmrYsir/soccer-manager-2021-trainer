using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using UnityEngine;
using UnityEngine.Networking;

public class MatchdayBoost
{
	public bool ReshowBoost;

	public const int NUM_PLAYER_OPTIONS = 3;

	public const int NUM_TOP_PLAYERS = 18;

	private int[] playerOptions = new int[3];

	private float[] playerBoostAmount = new float[3];

	private byte[] playerNewRating = new byte[3];

	private bool success;

	private int selectedPlayerIdx = -1;

	public bool[] selectedPlayer = new bool[3];

	public bool FillPlayerOptions()
	{
		short currentManagedClubId = Game.instance.CurrentManagedClubId;
		List<int> playerChoices = GetPlayerChoices();
		if (playerChoices.Count < 3)
		{
			return false;
		}
		for (int i = 0; i < 3; i++)
		{
			if (playerOptions[i] == 0 || Player.GetClubID(playerOptions[i]) != currentManagedClubId)
			{
				playerOptions[i] = SelectRandomPlayer(playerChoices);
			}
		}
		List<int> list = new List<int>(playerOptions);
		list.Sort((int a, int playerID) => Player.GetRating(playerID) - Player.GetRating(a));
		float num = 5f;
		if (UserManagerProfileController.UserHasTrait(ManagerTraitArea.PreMatchBoost))
		{
			num = (int)UserManagerProfileController.ApplyTrait(ManagerTraitArea.PreMatchBoost, num);
		}
		for (int num2 = 0; num2 < 3; num2++)
		{
			if (list.IndexOf(playerOptions[num2]) == 0)
			{
				playerBoostAmount[num2] = num;
			}
			else if (list.IndexOf(playerOptions[num2]) == 1)
			{
				playerBoostAmount[num2] = num;
			}
			else
			{
				playerBoostAmount[num2] = num;
			}
			playerNewRating[num2] = Player.GetRating(playerOptions[num2]);
			byte b = (byte)Attributes.GetAttributesForDisplay(playerOptions[num2]).groupedAttrs[0];
			playerNewRating[num2] = (byte)Math.Min((double)(int)b + Math.Ceiling(playerBoostAmount[num2] / 100f * (float)(int)b), 99.0);
		}
		return true;
	}

	public void BoostButtonClicked(int index, int boostAmount)
	{
		selectedPlayerIdx = index;
		UI.GetInstance().ShowGenericLoading();
		CustomerDataController.Instance.ConsumeItem(boostAmount, 0, BoostPlayerCallback);
		if (CustomerDataController.Instance.GetCurUserBoostAmount() < boostAmount)
		{
			ReshowBoost = true;
		}
		else
		{
			ReshowBoost = false;
		}
	}

	private static void BoostPlayerCallback(UnityWebRequest response)
	{
		UI.GetInstance().HideGenericLoading();
		JObject jObject = JObject.Parse(response.downloadHandler.text);
		if (CustomerDataController.Instance.JsonDataGetSuccess(jObject))
		{
			int curUserBoostAmount = (int)jObject["updatedvalues"]["Boosts"];
			CustomerDataController.Instance.SetCurUserBoostAmount(curUserBoostAmount);
			Game.instance.matchdayBoost.AwardPreMatchBoost();
			GameObject gameObject = GameObject.Find("PlayerBoostModal(Clone)");
			if ((bool)gameObject)
			{
				gameObject.GetComponent<MatchdayBoostModal>().HandleRewardVideoCompleted();
			}
			UI.GetInstance().UpdateTopBar();
		}
	}

	public void SelectPlayer(int index)
	{
		selectedPlayerIdx = index;
		EventLoggingController.LogEvent("adReward_preMatchBoost");
		AdsController.Instance.ShowRewardVideo(RewardVideoType.PreMatchPlayerBoost);
	}

	public bool DidUsePreMatchBoost()
	{
		return success;
	}

	public List<int> GetSelectedPreMatchBoostPlayers()
	{
		List<int> list = new List<int>();
		if (success)
		{
			for (int i = 0; i < 3; i++)
			{
				if (selectedPlayer[i])
				{
					list.Add(playerOptions[i]);
				}
			}
		}
		return list;
	}

	public float GetSelectedPreMatchBoostAmount(int playerId)
	{
		if (success)
		{
			int num = Array.IndexOf(playerOptions, playerId);
			if (num != -1)
			{
				return playerBoostAmount[num];
			}
		}
		return 0f;
	}

	public byte GetSelectedPreMatchNewRating(int playerId)
	{
		if (success)
		{
			int num = Array.IndexOf(playerOptions, playerId);
			if (num != -1)
			{
				return playerNewRating[num];
			}
		}
		return 0;
	}

	public int[] GetBoostPlayerOptions()
	{
		return playerOptions;
	}

	public float[] GetBoostPlayerAmounts()
	{
		return playerBoostAmount;
	}

	public byte[] GetBoostPlayerNewRatings()
	{
		return playerNewRating;
	}

	public void AwardPreMatchBoost()
	{
		selectedPlayer[selectedPlayerIdx] = true;
		success = true;
	}

	public bool[] GetSelectedPlayers()
	{
		return selectedPlayer;
	}

	public int GetLastSelectedPlayer()
	{
		return selectedPlayerIdx;
	}

	private List<int> GetPlayerChoices()
	{
		ManagedClub currentManagedClub = Game.instance.CurrentManagedClub;
		Fixture g_matchday_nextfix = Game.instance.g_matchday_nextfix;
		List<int> list = new List<int>();
		List<int> list2 = new List<int>();
		for (int i = 0; i < 11; i++)
		{
			list.Add(currentManagedClub.currentTactics.lineUp[i]);
		}
		list.Shuffle();
		for (int j = 0; j < list.Count; j++)
		{
			int num = list[j];
			if (!Player.IsInjured(num) && !Player.IsSuspended(num, currentManagedClub.AssociationID, g_matchday_nextfix.competitiontype, g_matchday_nextfix.competitionid) && Player.GetFirstTeam(num))
			{
				list2.Add(num);
			}
			if (list2.Count >= 18)
			{
				break;
			}
		}
		return list2;
	}

	private int SelectRandomPlayer(List<int> topPlayers)
	{
		int num = 0;
		do
		{
			num = topPlayers.SelectRandomElement();
		}
		while (Array.IndexOf(playerOptions, num) >= 0);
		return num;
	}
}
