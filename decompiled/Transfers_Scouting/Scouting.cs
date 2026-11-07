using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;
using UnityEngine;
using UnityEngine.Networking;

public class Scouting
{
	public static void Init()
	{
		Scout.Init();
	}

	public static void RefreshScouts()
	{
		int numberOfScouts = Scout.GetNumberOfScouts();
		int i;
		for (i = Game.instance.g_Scouts.Count; i < numberOfScouts; i++)
		{
			Scout.CreateScout();
		}
		for (int j = 0; j < i; j++)
		{
		}
	}

	public static void ResetScouts(bool changeAllScouts = true)
	{
		int num = ((!changeAllScouts) ? Scout.GetNumberOfScouts() : 0);
		_ = Game.instance.g_Scouts.Count;
		List<int> list = new List<int>(Game.instance.g_Scouts.Keys);
		UI.GetInstance().CheckForAndCreateScreenIfNeeded(66);
		int num2 = 0;
		foreach (int item in list)
		{
			Scout scout = Game.instance.g_Scouts[item];
			int currentAssignmentIndex = scout.GetCurrentAssignmentIndex();
			if (currentAssignmentIndex > -1)
			{
				_ = Game.instance.g_ScoutingSearch[currentAssignmentIndex];
				scout.RecallScout(addClubMessage: false);
			}
			if (++num2 > num)
			{
				Game.instance.g_Scouts.Remove(item);
			}
		}
		if (Game.instance.g_ScoutingSearch.Count > 0)
		{
			Game.instance.g_ScoutingSearch.Clear();
		}
		RefreshScouts();
	}

	public static ScoutSearch GetScoutSearchById(string id)
	{
		for (int num = Game.instance.g_ScoutingSearch.Count - 1; num >= 0; num--)
		{
			if (Game.instance.g_ScoutingSearch[num].searchid == id)
			{
				return Game.instance.g_ScoutingSearch[num];
			}
		}
		return null;
	}

	public static int GetAvailableScoutId()
	{
		int result = -1;
		foreach (KeyValuePair<int, Scout> g_Scout in Game.instance.g_Scouts)
		{
			Scout value = g_Scout.Value;
			if (value.GetCurrentAssignmentIndex() == -1)
			{
				result = value.id;
				break;
			}
		}
		return result;
	}

	public static int GetAvailableScoutAmounts()
	{
		int num = 0;
		for (int i = 0; i < Game.instance.g_Scouts.Count; i++)
		{
			if (Game.instance.g_Scouts.ElementAt(i).Value.GetCurrentAssignmentIndex() != -1)
			{
				num++;
			}
		}
		return num;
	}

	public static List<int> GetAvailableSearchNations()
	{
		int continent = CountryData.GetContinent(Club.GetCountryDataID(Game.instance.CurrentManagedClubId));
		int[] array = new int[5] { 1, 2, 3, 4, 6 };
		List<int> list = new List<int> { continent };
		int nationsSelectable = GetNationsSelectable(Facilities.GetFacilityLevel(4));
		int num = 1;
		for (int i = 0; i < array.Length; i++)
		{
			if (array[i] != continent && num < nationsSelectable)
			{
				list.Add(array[i]);
				num++;
			}
		}
		return list;
	}

	public static bool CreateScoutCountryReport(int scoutId, ScoutingOptions options)
	{
		if (Game.instance.g_Scouts[scoutId].CreateCountryScoutReport(ScoutAssignmentTypes.Country, options) > -1)
		{
			ModalController.Instance().CloseAllModals();
			UI.GetInstance().UpdateFacilitiesBlobs();
			if (!PlayerPrefs.HasKey("prefanalyticsrunscout"))
			{
				PlayerPrefs.SetInt("prefanalyticsrunscout", 1);
			}
			return true;
		}
		return false;
	}

	public static bool CreateScoutLeagueReport(int scoutId, ScoutingOptions options)
	{
		if (Game.instance.g_Scouts[scoutId].CreateCountryScoutReport(ScoutAssignmentTypes.League, options) > -1)
		{
			ModalController.Instance().CloseAllModals();
			UI.GetInstance().UpdateFacilitiesBlobs();
			if (!PlayerPrefs.HasKey("prefanalyticsrunscout"))
			{
				PlayerPrefs.SetInt("prefanalyticsrunscout", 1);
			}
			return true;
		}
		return false;
	}

	public static bool CreateScoutClubReport(int scoutId, ScoutingOptions options)
	{
		if (Game.instance.g_Scouts[scoutId].CreateCountryScoutReport(ScoutAssignmentTypes.Club, options) > -1)
		{
			ModalController.Instance().CloseAllModals();
			UI.GetInstance().UpdateFacilitiesBlobs();
			if (!PlayerPrefs.HasKey("prefanalyticsrunscout"))
			{
				PlayerPrefs.SetInt("prefanalyticsrunscout", 1);
			}
			return true;
		}
		return false;
	}

	public static bool CreateScoutPlayerReport(int playerId)
	{
		int availableScoutId = GetAvailableScoutId();
		if (availableScoutId > -1)
		{
			Scout scout = Game.instance.g_Scouts[availableScoutId];
			int continent = CountryData.GetContinent(Club.GetCountryDataID(Player.GetClubID(playerId)));
			if (GetAvailableSearchNations().Contains(continent) || Player.GetClubID(playerId) == 0)
			{
				scout.CreatePlayerScoutReport(playerId);
				UI.GetInstance().UpdateFacilitiesBlobs();
				if (!PlayerPrefs.HasKey("prefanalyticsrunscout"))
				{
					PlayerPrefs.SetInt("prefanalyticsrunscout", 1);
				}
				return true;
			}
			Helpers.ShowToast(Translation.GetText("scout-player-invalid"));
		}
		else
		{
			Helpers.ShowToast(Translation.GetText("no-available-scouts"));
		}
		return false;
	}

	public static void RecallScout(int scoutId)
	{
		Scout scout = Game.instance.g_Scouts[scoutId];
		int currentAssignmentIndex = scout.GetCurrentAssignmentIndex();
		if (currentAssignmentIndex > -1)
		{
			_ = Game.instance.g_ScoutingSearch[currentAssignmentIndex];
			_ = Game.instance.g_ScoutingSearch[currentAssignmentIndex].searchid;
			_ = Game.instance.g_ScoutingSearch[currentAssignmentIndex].type;
			scout.RecallScout();
		}
	}

	public static int GetInstantReportCost()
	{
		return 100;
	}

	public static void BuyFinishReport(string reportId, int showPlayerModal = 0)
	{
		int instantReportCost = GetInstantReportCost();
		if (CustomerDataController.Instance.GetUserCurCreditAmount() >= instantReportCost)
		{
			if (ProcessUncompleteScoutReports(reportId))
			{
				ScoutSearch scoutSearchById = GetScoutSearchById(reportId);
				if (scoutSearchById.type == 0)
				{
					EventLoggingController.LogEvent("scouting_playerrushed");
				}
				else
				{
					EventLoggingController.LogEvent("scouting_assignmentrushed");
				}
				if (scoutSearchById.type == 0 || scoutSearchById.results.Count > 0)
				{
					UI.GetInstance().ShowGenericLoading();
					Dictionary<string, object> dictionary = new Dictionary<string, object>();
					dictionary.Add("reportid", reportId.ToString());
					dictionary.Add("showplayermodal", showPlayerModal.ToString());
					CustomerDataController.Instance.ConsumeItem(0, instantReportCost, BuyFinishReportCallback, dictionary);
				}
				else
				{
					Helpers.ShowToast(Translation.GetText("no-players"));
					ModalController.Instance().CloseAllModals();
				}
				if (GameObject.Find("OverlayCanvas/ModalPanel(Clone)/PlayerProfileModal(Clone)") != null)
				{
					GameObject.Find("OverlayCanvas/ModalPanel(Clone)/PlayerProfileModal(Clone)").GetComponent<PlayerProfileScreen>().SwitchTab(PlayerProfileScreen.PlayerTabs.Profile);
				}
			}
		}
		else
		{
			ShowNotEnoughCredits();
		}
	}

	public static void HandleBuyFinishReportError(string reportId)
	{
		GetScoutSearchById(reportId).results = new List<ScoutSearchPlayer>();
	}

	public static void BuyFinishReportCallback(UnityWebRequest response)
	{
		UI.GetInstance().HideGenericLoading();
		JObject jObject = JObject.Parse(response.downloadHandler.text);
		JObject jObject2 = JObject.Parse(jObject["callback"].ToString());
		if (CustomerDataController.Instance.JsonDataGetSuccess(jObject))
		{
			int num = (int)jObject["updatedvalues"]["Credits"];
			_ = (string?)jObject2["reportid"];
			int num2 = (int)jObject2["showplayermodal"];
			int creditsSpent = CustomerDataController.Instance.GetUserCurCreditAmount() - num;
			EventLoggingController.LogInGamePurchaseEvent("scoutreport", creditsSpent);
			if (num2 > 0)
			{
				ModalController.Instance().CloseAllModals();
				ModalController.Instance().ShowPlayerProfileModal(num2);
			}
			CustomerDataController.Instance.SetCreditAmount(num);
			UI.GetInstance().UpdateTopBar();
			Save.SaveData(Save.currentSlot);
		}
		else
		{
			HandleBuyFinishReportError((string?)jObject2["reportid"]);
			ShowNotEnoughCredits();
		}
	}

	private static void ShowNotEnoughCredits()
	{
		ModalController.Instance().ShowConfirmModal(Translation.GetText("not-enough-credits"), Translation.GetText("not-enough-credits"), () =>
		{
			UI.GetInstance().ChangeScreen(8, ShopScreen.ScreenTypes.Credits);
			ModalController.Instance().CloseAllModals();
		}, Translation.GetText("go-to-shop"));
	}

	public static ScoutSearchPlayer FindPlayerReportWithinScoutReports(int playerId)
	{
		ScoutSearchPlayer result = null;
		for (int num = Game.instance.g_ScoutingSearch.Count - 1; num >= 0; num--)
		{
			ScoutSearch scoutSearch = Game.instance.g_ScoutingSearch[num];
			for (int i = 0; i < scoutSearch.results.Count; i++)
			{
				if (scoutSearch.results[i].playerid == playerId)
				{
					result = scoutSearch.results[i];
					break;
				}
			}
		}
		return result;
	}

	public static int IsPlayerBeingScouted(int playerId)
	{
		foreach (KeyValuePair<int, Scout> g_Scout in Game.instance.g_Scouts)
		{
			int currentAssignmentIndex = g_Scout.Value.GetCurrentAssignmentIndex();
			if (currentAssignmentIndex > -1)
			{
				ScoutSearch scoutSearch = Game.instance.g_ScoutingSearch[currentAssignmentIndex];
				if (scoutSearch.type == 0 && scoutSearch.id[0] == playerId)
				{
					return currentAssignmentIndex;
				}
			}
		}
		return -1;
	}

	public static int GetScoutAssignmentDuration(int idx)
	{
		ScoutSearch scoutSearch = Game.instance.g_ScoutingSearch[idx];
		return (int)Scout.GetTotalAssignmentDuration(scoutSearch.staffid, scoutSearch.id, (ScoutAssignmentTypes)scoutSearch.type);
	}

	public static int GetNumUnviewedScoutReports()
	{
		return GetNumUnviewedCountryScoutReports() + GetNumUnviewedPlayerScoutReports();
	}

	public static int GetNumUnviewedCountryScoutReports()
	{
		int num = 0;
		foreach (ScoutSearch item in Game.instance.g_ScoutingSearch)
		{
			if (!item.viewed && item.dateend != DateTime.MaxValue && item.type == 2)
			{
				num++;
			}
		}
		return num;
	}

	public static int GetNumUnviewedPlayerScoutReports()
	{
		int num = 0;
		foreach (ScoutSearch item in Game.instance.g_ScoutingSearch)
		{
			if (!item.viewed && item.dateend != DateTime.MaxValue && item.type == 0)
			{
				num++;
			}
		}
		return num;
	}

	public static int GetNumScoutingBlobs()
	{
		int num = 0;
		foreach (int key in Game.instance.FacilityManager.FacilityBlobs.Keys)
		{
			if (Game.instance.FacilityManager.FacilityBlobs[key] == 4)
			{
				num++;
			}
		}
		if (Game.instance.g_ScoutingSearch.Count == 0)
		{
			num++;
		}
		return num + GetNumUnviewedScoutReports();
	}

	public static void SetAllScoutReportsViewed()
	{
		for (int i = 0; i < Game.instance.g_ScoutingSearch.Count; i++)
		{
			ScoutSearch scoutSearch = Game.instance.g_ScoutingSearch[i];
			if (scoutSearch.dateend != DateTime.MaxValue && !scoutSearch.viewed)
			{
				SetScoutReportViewed(i, updateBlobs: false);
			}
		}
		UI.GetInstance().UpdateFacilitiesSideBarBlobs();
	}

	public static void SetScoutReportViewed(int idx, bool updateBlobs = true)
	{
		if (idx < Game.instance.g_ScoutingSearch.Count)
		{
			Game.instance.g_ScoutingSearch[idx].viewed = true;
			if (updateBlobs)
			{
				UI.GetInstance().UpdateFacilitiesBlobs();
			}
		}
	}

	public static int EncodeRatingAndPotentialDescriptions(int ratingDesc, int potentialDesc)
	{
		return (ratingDesc & 0xFF) | ((potentialDesc & 0xFF) << 8);
	}

	public static int[] DecodeRatingAndPotentialDescriptions(int encodedRatingPotentialDesc)
	{
		return new int[2]
		{
			encodedRatingPotentialDesc & 0xFF,
			(encodedRatingPotentialDesc >> 8) & 0xFF
		};
	}

	public static ScoutSearchPlayer CreateOwnPlayerPotential(int playerID)
	{
		ScoutSearch scoutSearch = new ScoutSearch();
		Scout scout = new Scout();
		scoutSearch.id[0] = playerID;
		scoutSearch.id[1] = Facilities.GetUpgradeLevel(25);
		scoutSearch.type = 4;
		ScoutSearchPlayer scoutSearchPlayer = new ScoutSearchPlayer
		{
			playerid = playerID
		};
		scout.SetPotentialRange(scoutSearchPlayer, Game.instance.g_PlayerData[playerID].Potential);
		scoutSearch.results.Add(scoutSearchPlayer);
		Game.instance.g_ScoutingSearch.Add(scoutSearch);
		return scoutSearchPlayer;
	}

	public static ScoutSearchPlayer GetOwnPlayerPotential(int playerID)
	{
		ScoutSearchPlayer result = null;
		int upgradeLevel = Facilities.GetUpgradeLevel(25);
		for (int i = 0; i < Game.instance.g_ScoutingSearch.Count; i++)
		{
			if (Game.instance.g_ScoutingSearch[i].type == 4 && Game.instance.g_ScoutingSearch[i].id[0] == playerID)
			{
				if (Game.instance.g_ScoutingSearch[i].id[1] < upgradeLevel)
				{
					Game.instance.g_ScoutingSearch.Remove(Game.instance.g_ScoutingSearch[i]);
					return CreateOwnPlayerPotential(playerID);
				}
				return Game.instance.g_ScoutingSearch[i].results[0];
			}
		}
		return result;
	}

	public static int GetScoutPotentialRange()
	{
		return FacilitiesConfig.SCOUTING_REPORT_RANGE - Facilities.GetUpgradeLevel(25);
	}

	public static int GetScoutRatingRange()
	{
		return FacilitiesConfig.SCOUTING_REPORT_RANGE - Facilities.GetUpgradeLevel(26);
	}

	public static int GetScoutReportReturnTime(int level)
	{
		return FacilitiesConfig.SCOUT_REPORT_RETURN_TIME_BASE_AMOUNT - Facilities.GetUpgradeIncrease(21, level);
	}

	public static int GetNoOfScouts(int level)
	{
		return FacilitiesConfig.SCOUT_NO_OF_SCOUTS_BASE_AMOUNT + Facilities.GetUpgradeIncrease(20, level);
	}

	public static int GetNationsSelectable(int level)
	{
		return FacilitiesConfig.SCOUT_NATION_BASE_AMOUNT + Facilities.GetUpgradeIncrease(22, level);
	}

	public static int GetPositionsSelectable(int level)
	{
		return FacilitiesConfig.SCOUT_POSITION_BASE_AMOUNT + Facilities.GetUpgradeIncrease(23, level);
	}

	public static int GetResultsAmount(int level)
	{
		return FacilitiesConfig.SCOUT_RESULTS_RETURNED + Facilities.GetUpgradeIncrease(24, level);
	}

	public static bool Process()
	{
		bool result = ProcessUncompleteScoutReports();
		ProcessExpiredScoutReports();
		return result;
	}

	public static bool ProcessUncompleteScoutReports(string scoutReportId = null)
	{
		bool result = false;
		for (int i = 0; i < Game.instance.g_ScoutingSearch.Count; i++)
		{
			ScoutSearch scoutSearch = Game.instance.g_ScoutingSearch[i];
			if (scoutSearch.dateend == DateTime.MaxValue && scoutSearch.type != 4)
			{
				float num = GetScoutReportReturnTime(Facilities.GetFacilityLevel(4));
				if ((DateTime.Compare(scoutSearch.datestart.AddDays(num).Date, Game.instance.CurrentDate) <= 0 || (scoutReportId != null && scoutReportId == scoutSearch.searchid)) && Game.instance.g_Scouts.ContainsKey(scoutSearch.staffid))
				{
					Game.instance.g_Scouts[scoutSearch.staffid].ProcessAssignment(scoutReportId != null);
					result = true;
				}
			}
		}
		return result;
	}

	public static void ProcessExpiredScoutReports()
	{
		for (int num = Game.instance.g_ScoutingSearch.Count - 1; num >= 0; num--)
		{
			ScoutSearch scoutSearch = Game.instance.g_ScoutingSearch[num];
			if ((Game.instance.CurrentDate - scoutSearch.dateend).Days > 180)
			{
				foreach (ScoutSearchPlayer result in scoutSearch.results)
				{
					int playerid = result.playerid;
					if (Game.instance.g_PlayersScouted.ContainsKey(playerid) && Game.instance.g_PlayersScouted[playerid].date == scoutSearch.dateend)
					{
						Game.instance.g_PlayersScouted.Remove(playerid);
					}
				}
				Game.instance.g_ScoutingSearch.RemoveAt(num);
			}
		}
	}
}
