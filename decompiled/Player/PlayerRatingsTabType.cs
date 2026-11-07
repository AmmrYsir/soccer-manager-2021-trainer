using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using SimpleJSON;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class DebugTools : MonoBehaviour
{
	public enum TabType
	{
		Schedule,
		Respawn,
		PlayerRatings,
		Facilities,
		Player,
		Data,
		Training
	}

	private enum CheckpointType
	{
		Weekly,
		Monthly,
		Seasonally
	}

	public enum RespawnTabType
	{
		RespawnLog,
		UnitTests
	}

	public enum PlayerRatingsTabType
	{
		ClubPlayers,
		PlayerPotential,
		PlayerAttributes,
		Training
	}

	private enum AdvanceUnit
	{
		Days,
		Months
	}

	public static Game game = Game.instance;

	private static CheckpointType SaveCheckpointType = CheckpointType.Weekly;

	private static bool UseSaveCheckpoints = false;

	private static DateTime DateLastSaved;

	private static int LastSaveMonth;

	private static bool SavedThisSeason = false;

	private DateTime theDate;

	public void Update()
	{
		if (theDate != Game.instance.CurrentDate)
		{
			GameObject gameObject = UI.GetInstance().overlayCanvas.transform.Find("DebugTools").gameObject;
			gameObject.transform.Find("Panel/MainPanel/InputDay").GetComponent<InputField>().SetTextWithoutNotify(Game.instance.CurrentDate.Day.ToString());
			gameObject.transform.Find("Panel/MainPanel/InputMonth").GetComponent<InputField>().SetTextWithoutNotify(Game.instance.CurrentDate.Month.ToString());
			gameObject.transform.Find("Panel/MainPanel/InputYear").GetComponent<InputField>().SetTextWithoutNotify(Game.instance.CurrentDate.Year.ToString());
			theDate = Game.instance.CurrentDate;
		}
	}

	public static void DrawDebugTools()
	{
		GameObject overlayCanvas = UI.GetInstance().overlayCanvas;
		GameObject tools = overlayCanvas.transform.Find("DebugTools").gameObject;
		UI.GetInstance().canSwipeScreen = false;
		tools.SetActive(value: false);
		tools.transform.Find("Panel/MainPanel/Continue").GetComponent<Button>().onClick.RemoveAllListeners();
		tools.transform.Find("Panel/MainPanel/Continue").GetComponent<Button>().onClick.AddListener(() =>
		{
			DoContinue();
		});
		tools.transform.Find("Panel/MainPanel/AdvanceOneMonth").GetComponent<Button>().onClick.RemoveAllListeners();
		tools.transform.Find("Panel/MainPanel/AdvanceOneMonth").GetComponent<Button>().onClick.AddListener(() =>
		{
			AdvanceSkipToDate(1, AdvanceUnit.Months);
		});
		tools.transform.Find("Panel/MainPanel/AdvanceThreeMonths").GetComponent<Button>().onClick.RemoveAllListeners();
		tools.transform.Find("Panel/MainPanel/AdvanceThreeMonths").GetComponent<Button>().onClick.AddListener(() =>
		{
			AdvanceSkipToDate(3, AdvanceUnit.Months);
		});
		tools.transform.Find("Panel/MainPanel/AdvanceSixMonths").GetComponent<Button>().onClick.RemoveAllListeners();
		tools.transform.Find("Panel/MainPanel/AdvanceSixMonths").GetComponent<Button>().onClick.AddListener(() =>
		{
			AdvanceSkipToDate(6, AdvanceUnit.Months);
		});
		tools.transform.Find("Panel/MainPanel/AdvanceTwelveMonths").GetComponent<Button>().onClick.RemoveAllListeners();
		tools.transform.Find("Panel/MainPanel/AdvanceTwelveMonths").GetComponent<Button>().onClick.AddListener(() =>
		{
			AdvanceSkipToDate(12, AdvanceUnit.Months);
		});
		tools.transform.Find("Panel/MainPanel/AdvanceOneDay").GetComponent<Button>().onClick.RemoveAllListeners();
		tools.transform.Find("Panel/MainPanel/AdvanceOneDay").GetComponent<Button>().onClick.AddListener(() =>
		{
			AdvanceSkipToDate(1, AdvanceUnit.Days);
		});
		tools.transform.Find("Panel/MainPanel/AdvanceThreeDays").GetComponent<Button>().onClick.RemoveAllListeners();
		tools.transform.Find("Panel/MainPanel/AdvanceThreeDays").GetComponent<Button>().onClick.AddListener(() =>
		{
			AdvanceSkipToDate(3, AdvanceUnit.Days);
		});
		tools.transform.Find("Panel/MainPanel/AdvanceSevenDays").GetComponent<Button>().onClick.RemoveAllListeners();
		tools.transform.Find("Panel/MainPanel/AdvanceSevenDays").GetComponent<Button>().onClick.AddListener(() =>
		{
			AdvanceSkipToDate(7, AdvanceUnit.Days);
		});
		tools.transform.Find("Panel/MainPanel/AdvanceFourteenDays").GetComponent<Button>().onClick.RemoveAllListeners();
		tools.transform.Find("Panel/MainPanel/AdvanceFourteenDays").GetComponent<Button>().onClick.AddListener(() =>
		{
			AdvanceSkipToDate(14, AdvanceUnit.Days);
		});
		Transform transform = tools.transform.Find("Panel/MainPanel/AdvanceEndSeason");
		List<ClubScheduleInfo> list = new List<ClubScheduleInfo>();
		if (Game.instance != null && Game.instance.managedClub != null)
		{
			list = Club.GetSchedule(Game.instance.CurrentManagedClubId);
		}
		if (list.Count > 0)
		{
			DateTime lastFixtureDate = list[list.Count - 1].Date;
			transform.Find("Text").GetComponent<Text>().text = "Last Fixture: " + lastFixtureDate.ToString("dd-MM-yyyy");
			Button component = transform.GetComponent<Button>();
			component.onClick.RemoveAllListeners();
			component.onClick.AddListener(() =>
			{
				SetSkipToDate(lastFixtureDate);
			});
		}
		else
		{
			transform.gameObject.SetActive(value: false);
		}
		tools.transform.Find("Panel/CloseButton").GetComponent<Button>().onClick.RemoveAllListeners();
		tools.transform.Find("Panel/CloseButton").GetComponent<Button>().onClick.AddListener(() =>
		{
			UI.GetInstance().canSwipeScreen = true;
			tools.SetActive(value: false);
		});
		tools.transform.Find("Panel/Tabs/ScheduleTab").GetComponent<Button>().onClick.RemoveAllListeners();
		tools.transform.Find("Panel/Tabs/ScheduleTab").GetComponent<Button>().onClick.AddListener(() =>
		{
			ChangeTab(TabType.Schedule);
		});
		tools.transform.Find("Panel/Tabs/RespawnTab").GetComponent<Button>().onClick.RemoveAllListeners();
		tools.transform.Find("Panel/Tabs/RespawnTab").GetComponent<Button>().onClick.AddListener(() =>
		{
			ChangeTab(TabType.Respawn);
		});
		tools.transform.Find("Panel/Tabs/PlayerRatingsTab").GetComponent<Button>().onClick.RemoveAllListeners();
		tools.transform.Find("Panel/Tabs/PlayerRatingsTab").GetComponent<Button>().onClick.AddListener(() =>
		{
			ChangeTab(TabType.PlayerRatings);
		});
		tools.transform.Find("Panel/Tabs/FacilitiesTab").GetComponent<Button>().onClick.RemoveAllListeners();
		tools.transform.Find("Panel/Tabs/FacilitiesTab").GetComponent<Button>().onClick.AddListener(() =>
		{
			ChangeTab(TabType.Facilities);
		});
		tools.transform.Find("Panel/Tabs/DataTab").GetComponent<Button>().onClick.RemoveAllListeners();
		tools.transform.Find("Panel/Tabs/DataTab").GetComponent<Button>().onClick.AddListener(() =>
		{
			ChangeTab(TabType.Data);
		});
		tools.transform.Find("Panel/Tabs/PlayerTab").GetComponent<Button>().onClick.RemoveAllListeners();
		tools.transform.Find("Panel/Tabs/PlayerTab").GetComponent<Button>().onClick.AddListener(() =>
		{
			ChangeTab(TabType.Player);
		});
		tools.transform.Find("Panel/Tabs/TrainingTab").GetComponent<Button>().onClick.RemoveAllListeners();
		tools.transform.Find("Panel/Tabs/TrainingTab").GetComponent<Button>().onClick.AddListener(() =>
		{
			ChangeTab(TabType.Training);
		});
		ChangeTab(TabType.Schedule);
		tools.transform.Find("Panel/MainPanel/SaveCheckpointsContainer/EnabledButton").GetComponent<Button>().onClick.RemoveAllListeners();
		tools.transform.Find("Panel/MainPanel/SaveCheckpointsContainer/Options/WeekOption").GetComponent<Button>().onClick.RemoveAllListeners();
		tools.transform.Find("Panel/MainPanel/SaveCheckpointsContainer/Options/MonthOption").GetComponent<Button>().onClick.RemoveAllListeners();
		tools.transform.Find("Panel/MainPanel/SaveCheckpointsContainer/Options/SeasonOption").GetComponent<Button>().onClick.RemoveAllListeners();
		tools.transform.Find("Panel/MainPanel/SaveCheckpointsContainer/EnabledButton").GetComponent<Button>().onClick.AddListener(() =>
		{
			ToggleSaveCheckpoints();
		});
		tools.transform.Find("Panel/MainPanel/SaveCheckpointsContainer/Options/WeekOption").GetComponent<Button>().onClick.AddListener(() =>
		{
			SelectCheckpointType(CheckpointType.Weekly);
		});
		tools.transform.Find("Panel/MainPanel/SaveCheckpointsContainer/Options/MonthOption").GetComponent<Button>().onClick.AddListener(() =>
		{
			SelectCheckpointType(CheckpointType.Monthly);
		});
		tools.transform.Find("Panel/MainPanel/SaveCheckpointsContainer/Options/SeasonOption").GetComponent<Button>().onClick.AddListener(() =>
		{
			SelectCheckpointType(CheckpointType.Seasonally);
		});
		Transform transform2 = tools.transform.Find("Panel/MainPanel/DailyLoginContainer");
		Button enableButton = transform2.Find("EnabledButton").GetComponent<Button>();
		enableButton.onClick.RemoveAllListeners();
		enableButton.onClick.AddListener(() =>
		{
			ToggleDailyLoginEnabled(enableButton.transform);
		});
		Button component2 = transform2.Find("OpenButton").GetComponent<Button>();
		component2.onClick.RemoveAllListeners();
		component2.onClick.AddListener(() =>
		{
			ModalController.Instance().ShowDailyLoginModal();
		});
		Button component3 = tools.transform.Find("Panel/MainPanel/SaveNowButton").GetComponent<Button>();
		component3.onClick.RemoveAllListeners();
		component3.onClick.AddListener(() =>
		{
			Save.SaveData(Save.currentSlot);
		});
		Button tutorialButton = tools.transform.Find("Panel/MainPanel/TutorialModeButton").GetComponent<Button>();
		tutorialButton.onClick.RemoveAllListeners();
		tutorialButton.onClick.AddListener(() =>
		{
			ToggleTutorialModeEnabled(tutorialButton.transform);
		});
		Button component4 = tools.transform.Find("Panel/MainPanel/TutorialClearHistoryButton").GetComponent<Button>();
		component4.onClick.RemoveAllListeners();
		component4.onClick.AddListener(() =>
		{
			TutorialClearHistory();
		});
		Button component5 = tools.transform.Find("Panel/MainPanel/TestErrorModal").GetComponent<Button>();
		component5.onClick.RemoveAllListeners();
		component5.onClick.AddListener(() =>
		{
			Debug.LogError("Dan's test error");
		});
		DrawMainDebug();
		if (UI.GetInstance().g_currentscreen != 0)
		{
			tools.transform.Find("Panel/Tabs/PlayerTab").gameObject.SetActive(value: true);
		}
		else
		{
			tools.transform.Find("Panel/Tabs/PlayerTab").gameObject.SetActive(value: false);
		}
	}

	private static void ToggleDailyLoginEnabled(Transform button)
	{
		DailyLoginModal.DebugMode = !DailyLoginModal.DebugMode;
		button.Find("Text").GetComponent<Text>().text = (DailyLoginModal.DebugMode ? "Enabled" : "Disabled");
		button.GetComponent<Image>().color = (DailyLoginModal.DebugMode ? Color.green : Color.red);
	}

	private static void ToggleTutorialModeEnabled(Transform button)
	{
		TutorialController.DevelopmentTesting = !TutorialController.DevelopmentTesting;
		button.Find("Text").GetComponent<Text>().text = (TutorialController.DevelopmentTesting ? "Tutorial Mode: Enabled" : "Tutorial Mode: Disabled");
		button.GetComponent<Image>().color = (TutorialController.DevelopmentTesting ? Color.green : Color.red);
	}

	private static void TutorialClearHistory()
	{
		TutorialController.GetInstance().ClearTutorialHistory();
		Helpers.ShowToast("Tutorial PlayerPrefs deleted!");
	}

	public static void ChangeTab(TabType tabType)
	{
		GameObject overlayCanvas = UI.GetInstance().overlayCanvas;
		GameObject gameObject = overlayCanvas.transform.Find("DebugTools/Panel/MainPanel").gameObject;
		GameObject gameObject2 = overlayCanvas.transform.Find("DebugTools/Panel/RespawnPanel").gameObject;
		GameObject gameObject3 = overlayCanvas.transform.Find("DebugTools/Panel/PlayerRatingsPanel").gameObject;
		GameObject gameObject4 = overlayCanvas.transform.Find("DebugTools/Panel/FacilitiesPanel").gameObject;
		GameObject gameObject5 = overlayCanvas.transform.Find("DebugTools/Panel/DataPanel").gameObject;
		GameObject gameObject6 = overlayCanvas.transform.Find("DebugTools/Panel/PlayerPanel").gameObject;
		GameObject gameObject7 = overlayCanvas.transform.Find("DebugTools/Panel/TrainingPanel").gameObject;
		gameObject.SetActive(value: false);
		gameObject2.SetActive(value: false);
		gameObject3.SetActive(value: false);
		gameObject4.SetActive(value: false);
		gameObject5.SetActive(value: false);
		gameObject6.SetActive(value: false);
		gameObject7.SetActive(value: false);
		switch (tabType)
		{
		default:
			gameObject.SetActive(value: true);
			break;
		case TabType.Respawn:
			DrawRespawnDebug();
			break;
		case TabType.PlayerRatings:
			DrawPlayerRatingsDebug();
			break;
		case TabType.Facilities:
			DrawFacilitiesDebug();
			break;
		case TabType.Data:
			DrawDataTool();
			break;
		case TabType.Player:
			DrawPlayerDebug();
			break;
		case TabType.Training:
			DrawTrainingTab();
			break;
		}
	}

	private static void ToggleSaveCheckpoints()
	{
		UseSaveCheckpoints = !UseSaveCheckpoints;
		UpdateSaveCheckpointUI();
	}

	private static void SelectCheckpointType(CheckpointType selected)
	{
		UseSaveCheckpoints = true;
		UpdateSaveCheckpointUI();
		SaveCheckpointType = selected;
		UpdateCheckpointOptionsUI();
	}

	private static void UpdateSaveCheckpointUI()
	{
		GameObject gameObject = UI.GetInstance().overlayCanvas.transform.Find("DebugTools").gameObject;
		gameObject.transform.Find("Panel/MainPanel/SaveCheckpointsContainer/EnabledButton").GetComponent<Image>().color = (UseSaveCheckpoints ? Color.green : Color.red);
		gameObject.transform.Find("Panel/MainPanel/SaveCheckpointsContainer/Options").gameObject.SetActive(UseSaveCheckpoints);
	}

	private static void UpdateCheckpointOptionsUI()
	{
		GameObject gameObject = UI.GetInstance().overlayCanvas.transform.Find("DebugTools").gameObject;
		gameObject.transform.Find("Panel/MainPanel/SaveCheckpointsContainer/Options/WeekOption").GetComponent<Image>().color = Color.red;
		gameObject.transform.Find("Panel/MainPanel/SaveCheckpointsContainer/Options/MonthOption").GetComponent<Image>().color = Color.red;
		gameObject.transform.Find("Panel/MainPanel/SaveCheckpointsContainer/Options/SeasonOption").GetComponent<Image>().color = Color.red;
		switch (SaveCheckpointType)
		{
		case CheckpointType.Weekly:
			gameObject.transform.Find("Panel/MainPanel/SaveCheckpointsContainer/Options/WeekOption").GetComponent<Image>().color = Color.green;
			break;
		case CheckpointType.Monthly:
			gameObject.transform.Find("Panel/MainPanel/SaveCheckpointsContainer/Options/MonthOption").GetComponent<Image>().color = Color.green;
			break;
		case CheckpointType.Seasonally:
			gameObject.transform.Find("Panel/MainPanel/SaveCheckpointsContainer/Options/SeasonOption").GetComponent<Image>().color = Color.green;
			break;
		}
	}

	public static void DrawScheduleDebug()
	{
		UI.GetInstance().overlayCanvas.transform.Find("ScheduleDebugScreen").gameObject.SetActive(value: true);
	}

	public static void DrawMainDebug()
	{
		GameObject panel = UI.GetInstance().overlayCanvas.transform.Find("DebugTools/Panel/MainPanel").gameObject;
		panel.transform.Find("ForceResultsButton/Text").GetComponent<Text>().text = Settings.GetForceResultsText();
		panel.transform.Find("ForceResultsButton").GetComponent<Button>().onClick.RemoveAllListeners();
		panel.transform.Find("ForceResultsButton").GetComponent<Button>().onClick.AddListener(() =>
		{
			Settings.ToggleForceResults(panel.transform.Find("ForceResultsButton/Text").GetComponent<Text>());
		});
		panel.transform.Find("AutoPickButton/Text").GetComponent<Text>().text = (Game.debugAutoPickTeam ? "ON" : "OFF");
		panel.transform.Find("AutoPickButton").GetComponent<Button>().onClick.RemoveAllListeners();
		panel.transform.Find("AutoPickButton").GetComponent<Button>().onClick.AddListener(() =>
		{
			Game.debugAutoPickTeam = !Game.debugAutoPickTeam;
			panel.transform.Find("AutoPickButton/Text").GetComponent<Text>().text = (Game.debugAutoPickTeam ? "ON" : "OFF");
		});
		if (Game.instance.CurrentManagedClub.MatchdayType == 0)
		{
			panel.transform.Find("MatchEngineBtn/Text").GetComponent<Text>().text = "Quick Engine";
		}
		else
		{
			panel.transform.Find("MatchEngineBtn/Text").GetComponent<Text>().text = "2D Pitch";
		}
		panel.transform.Find("MatchEngineBtn").GetComponent<Button>().onClick.RemoveAllListeners();
		panel.transform.Find("MatchEngineBtn").GetComponent<Button>().onClick.AddListener(() =>
		{
			Settings.ToggleMatchdayType(panel.transform.Find("MatchEngineBtn/Text").GetComponent<Text>());
		});
		panel.transform.Find("AddCreditsBtn").GetComponent<Button>().onClick.RemoveAllListeners();
		panel.transform.Find("AddCreditsBtn").GetComponent<Button>().onClick.AddListener(() =>
		{
			new AjaxRequest();
			new Dictionary<string, object>
			{
				{ "action", "DebugAddCreditsForUser" },
				{
					"amount",
					panel.transform.Find("AddCreditsField").GetComponent<InputField>().text
				},
				{
					"custid",
					CustomerDataController.Instance.GetCurUserID()
				}
			};
		});
		panel.transform.Find("AddCashBtn").GetComponent<Button>().onClick.RemoveAllListeners();
		panel.transform.Find("AddCashBtn").GetComponent<Button>().onClick.AddListener(() =>
		{
			Game.instance.CurrentManagedClub.Balance += float.Parse(panel.transform.Find("AddCashField").GetComponent<InputField>().text);
			Helpers.ShowToast("Cash added");
		});
		panel.transform.Find("SetLevelBtn").GetComponent<Button>().onClick.RemoveAllListeners();
		panel.transform.Find("SetLevelBtn").GetComponent<Button>().onClick.AddListener(() =>
		{
			CustomerDataController.Instance.CustomerDataHolder.Userdata.ManagerLevel = panel.transform.Find("SetLevelDropdown").GetComponent<Dropdown>().value + 1;
			Helpers.ShowToast("Manager level set");
		});
	}

	private static void DebugAddCreditCallback(UnityWebRequest response)
	{
		JSONNode jSONNode = JSON.Parse(response.downloadHandler.text);
		if (jSONNode["balance"] != null)
		{
			CustomerDataController.Instance.SetCreditAmount(jSONNode["balance"]);
			Helpers.ShowToast("Credits added");
		}
		else
		{
			Helpers.ShowToast("Failed to add credits");
		}
	}

	public static void RefreshScheduleDebug()
	{
		GameObject gameObject = UI.GetInstance().overlayCanvas.transform.Find("DebugTools").gameObject.transform.Find("Panel/SchedulePanel").gameObject;
		gameObject.transform.Find("ScheduleNextGameDate").GetComponent<Text>().text = Game.instance.g_season.Schedule.UnplayedScheduleDate.ToShortDateString() + " " + Game.instance.g_season.Schedule.UnplayedScheduleDate.ToShortTimeString();
		gameObject.transform.Find("ScheduleGamesCount").GetComponent<Text>().text = Game.instance.g_season.Schedule.DEBUG_GetScheduledGamesCount().ToString();
		gameObject.transform.Find("ScheduleUnplayedGames").GetComponent<Text>().text = Game.instance.g_season.Schedule.DEBUG_GetUnplayedGamesCount().ToString();
		gameObject.transform.Find("UnplayedGameExists").GetComponent<Text>().text = Game.instance.g_season.Schedule.UnplayedMatchExists().ToString();
		Text component = gameObject.transform.Find("UnitTestPanel/UnitTestText").GetComponent<Text>();
		component.text = TEST_FixtureTimesValid() + Environment.NewLine;
		component.text = component.text + TEST_IllegalUnplayedGames() + Environment.NewLine;
	}

	public static void DrawRespawnDebug()
	{
		GameObject gameObject = UI.GetInstance().overlayCanvas.transform.Find("DebugTools/Panel/RespawnPanel").gameObject;
		GameObject gameObject2 = gameObject.transform.Find("RespawnTabs").gameObject;
		gameObject2.transform.Find("RespawnLogTab").GetComponent<Button>().onClick.RemoveAllListeners();
		gameObject2.transform.Find("RespawnLogTab").GetComponent<Button>().onClick.AddListener(() =>
		{
			ChangeRespawnTab(RespawnTabType.RespawnLog);
		});
		gameObject2.transform.Find("RespawnUnitTestsTab").GetComponent<Button>().onClick.RemoveAllListeners();
		gameObject2.transform.Find("RespawnUnitTestsTab").GetComponent<Button>().onClick.AddListener(() =>
		{
			ChangeRespawnTab(RespawnTabType.UnitTests);
		});
		gameObject.transform.Find("RespawnLog/Refresh").GetComponent<Button>().onClick.RemoveAllListeners();
		gameObject.transform.Find("RespawnLog/Refresh").GetComponent<Button>().onClick.AddListener(() =>
		{
			DrawRespawnLog();
		});
		gameObject.transform.Find("UnitTests/RunUnitTests").GetComponent<Button>().onClick.RemoveAllListeners();
		gameObject.transform.Find("UnitTests/RunUnitTests").GetComponent<Button>().onClick.AddListener(() =>
		{
			RunRespawnUnitTests();
		});
		gameObject.SetActive(value: true);
		ChangeRespawnTab(RespawnTabType.RespawnLog);
	}

	public static void ChangeRespawnTab(RespawnTabType tabType)
	{
		GameObject gameObject = UI.GetInstance().overlayCanvas.transform.Find("DebugTools/Panel/RespawnPanel").gameObject;
		GameObject gameObject2 = gameObject.transform.Find("RespawnLog").gameObject;
		GameObject gameObject3 = gameObject.transform.Find("UnitTests").gameObject;
		gameObject2.SetActive(value: false);
		gameObject3.SetActive(value: false);
		if (tabType == RespawnTabType.RespawnLog || tabType != RespawnTabType.UnitTests)
		{
			gameObject2.SetActive(value: true);
		}
		else
		{
			gameObject3.SetActive(value: true);
		}
	}

	public static void AddRespawnLogMessage(string log)
	{
	}

	public static void DrawRespawnLog()
	{
		GameObject gameObject = UI.GetInstance().overlayCanvas.transform.Find("DebugTools/Panel/RespawnPanel").gameObject;
		GameObject gameObject2 = gameObject.transform.Find("RespawnLog/ScrollView/Content").gameObject;
		GameObject original = gameObject2.transform.Find("Row").gameObject;
		int g_PlayerData_NextPlayerID = Game.instance.g_PlayerData_NextPlayerID;
		int num = int.Parse(gameObject.transform.Find("RespawnLog/LastRespawnID").GetComponent<Text>().text);
		if (num >= g_PlayerData_NextPlayerID)
		{
			return;
		}
		for (int i = num; i < g_PlayerData_NextPlayerID; i++)
		{
			if (Game.instance.g_PlayerData[i].Status == ePlayerStatus.RESPAWNED)
			{
				int countryDataID = PlayerData.GetCountryDataID(i);
				string text = string.Concat(i, ": ", PlayerData.GetPlayerRespawnDate(i), " ");
				text = text + Player.GetName(i) + " (" + PlayerData.GetPlayerDataID(i) + ") ";
				text = text + CountryData.GetName(countryDataID) + " (" + countryDataID + ") ";
				UnityEngine.Object.Instantiate(original, gameObject2.transform, worldPositionStays: false).GetComponentInChildren<Text>().text = text;
			}
		}
		gameObject.transform.Find("RespawnLog/LastRespawnID").GetComponent<Text>().text = g_PlayerData_NextPlayerID.ToString();
	}

	public static void RunRespawnUnitTests()
	{
		GameObject gameObject = UI.GetInstance().overlayCanvas.transform.Find("DebugTools/Panel/RespawnPanel").gameObject.transform.Find("UnitTests/UnitTestResult/Content").gameObject;
		GameObject original = gameObject.transform.Find("Row").gameObject;
		List<string> list = new List<string>();
		string text = null;
		ClearChildObjects(gameObject);
		UnityEngine.Object.Instantiate(original, gameObject.transform, worldPositionStays: false).GetComponentInChildren<Text>().text = Game.instance.CurrentDate.ToString();
		int num = 50;
		text = "<b>Player Retirement Test</b>\n";
		text = text + "Players should be retired before they reach the age of " + num + "\n";
		text = text + TEST_PlayerAges(num) + "\n";
		list.Add(text);
		float num2 = 0.2f;
		text = "<b>Player Respawn Distribution</b>\n";
		text = text + "The distribution of players across age and country should be no more than x" + num2 + " different to the original profile\n";
		text = text + TEST_PlayerRespawnDistribution(num2) + "\n";
		list.Add(text);
		text = "<b>Player Potential</b>\n";
		text += "All players should have a player potential set that is greater than 60 and less than 100\n";
		text = text + TEST_PlayerPotential() + "\n";
		list.Add(text);
		for (int i = 0; i < list.Count; i++)
		{
			UnityEngine.Object.Instantiate(original, gameObject.transform, worldPositionStays: false).GetComponentInChildren<Text>().text = list[i];
		}
		ChangeRespawnTab(RespawnTabType.UnitTests);
	}

	public static void DrawPlayerRatingsDebug()
	{
		GameObject gameObject = UI.GetInstance().overlayCanvas.transform.Find("DebugTools/Panel/PlayerRatingsPanel").gameObject;
		GameObject gameObject2 = gameObject.transform.Find("PlayerRatingsTabs").gameObject;
		gameObject2.transform.Find("ClubPlayersTab").GetComponent<Button>().onClick.RemoveAllListeners();
		gameObject2.transform.Find("ClubPlayersTab").GetComponent<Button>().onClick.AddListener(() =>
		{
			ChangePlayerRatingsTab(PlayerRatingsTabType.ClubPlayers);
		});
		gameObject2.transform.Find("PlayerPotentialTab").GetComponent<Button>().onClick.RemoveAllListeners();
		gameObject2.transform.Find("PlayerPotentialTab").GetComponent<Button>().onClick.AddListener(() =>
		{
			ChangePlayerRatingsTab(PlayerRatingsTabType.PlayerPotential);
		});
		gameObject2.transform.Find("PlayerAttributesTab").GetComponent<Button>().onClick.RemoveAllListeners();
		gameObject2.transform.Find("PlayerAttributesTab").GetComponent<Button>().onClick.AddListener(() =>
		{
			ChangePlayerRatingsTab(PlayerRatingsTabType.PlayerAttributes);
		});
		gameObject2.transform.Find("TrainingTab").GetComponent<Button>().onClick.RemoveAllListeners();
		gameObject2.transform.Find("TrainingTab").GetComponent<Button>().onClick.AddListener(() =>
		{
			ChangePlayerRatingsTab(PlayerRatingsTabType.Training);
		});
		gameObject.SetActive(value: true);
		ChangePlayerRatingsTab(PlayerRatingsTabType.ClubPlayers);
	}

	public static void ChangePlayerRatingsTab(PlayerRatingsTabType tabType)
	{
		GameObject gameObject = UI.GetInstance().overlayCanvas.transform.Find("DebugTools/Panel/PlayerRatingsPanel").gameObject;
		GameObject gameObject2 = gameObject.transform.Find("ClubPlayers").gameObject;
		GameObject gameObject3 = gameObject.transform.Find("PlayerPotential").gameObject;
		GameObject gameObject4 = gameObject.transform.Find("PlayerAttributes").gameObject;
		GameObject gameObject5 = gameObject.transform.Find("Training").gameObject;
		gameObject2.SetActive(value: false);
		gameObject3.SetActive(value: false);
		gameObject4.SetActive(value: false);
		gameObject5.SetActive(value: false);
		switch (tabType)
		{
		default:
			DrawClubPlayerRatings();
			break;
		case PlayerRatingsTabType.PlayerPotential:
			DrawPlayerPotential();
			break;
		case PlayerRatingsTabType.PlayerAttributes:
			DrawPlayerAttributes();
			break;
		case PlayerRatingsTabType.Training:
			DrawTraining();
			break;
		}
	}

	public static void DrawClubPlayerRatings()
	{
		GameObject gameObject = UI.GetInstance().overlayCanvas.transform.Find("DebugTools/Panel/PlayerRatingsPanel").gameObject.transform.Find("ClubPlayers").gameObject;
		GameObject gameObject2 = gameObject.transform.Find("PlayerList/Content").gameObject;
		GameObject original = gameObject2.transform.Find("Row").gameObject;
		ClearChildObjects(gameObject2, 3);
		short associationID = Game.instance.CurrentManagedClub.AssociationID;
		string currentSeasonID = Association.GetCurrentSeasonID(associationID);
		string nextSeasonID = Season.GetNextSeasonID(currentSeasonID);
		Association.GetSeasonStartDate(associationID, currentSeasonID);
		DateTime seasonStartDate = Association.GetSeasonStartDate(associationID, nextSeasonID);
		AttrOpts attrOpts = new AttrOpts();
		attrOpts.SetDefaultAttrOpts();
		attrOpts.date = seasonStartDate;
		attrOpts.playertrainingon = false;
		PlayerSquad players = Club.GetPlayers(Game.instance.CurrentManagedClubId);
		for (int i = 0; i < players.Count; i++)
		{
			int num = players[i];
			int rating = Player.GetRating(num);
			int rating2 = Player.GetRating(num, seasonStartDate);
			PlayerAttributes playerAttributes = Attributes.SetAttributesForPlayer(num);
			float num2 = playerAttributes.attrsDisplay.Sum();
			PlayerAttributes playerAttributes2 = Attributes.SetAttributesForPlayer(num, attrOpts);
			float num3 = playerAttributes2.attrsDisplay.Sum();
			float[] array = playerAttributes.attrsDisplay.Where((float num7) => num7 > 0f).ToArray();
			float[] array2 = playerAttributes2.attrsDisplay.Where((float num7) => num7 > 0f).ToArray();
			string text;
			if (rating2 > rating)
			{
				text = "lime";
			}
			else
			{
				text = ((rating2 < rating) ? "red" : "black");
			}
			string text2;
			if (num3 > num2)
			{
				text2 = "lime";
			}
			else
			{
				text2 = ((num3 < num2) ? "red" : "black");
			}
			GameObject gameObject3 = UnityEngine.Object.Instantiate(original, gameObject2.transform, worldPositionStays: false);
			gameObject3.transform.Find("PlayerName").GetComponent<Text>().text = Player.GetName(num);
			gameObject3.transform.Find("Position").GetComponent<Text>().text = Helpers.GetPlayerPositionString(Player.GetPosition(num, preferred: true));
			gameObject3.transform.Find("Age").GetComponent<Text>().text = Player.GetAge(num).ToString();
			gameObject3.transform.Find("SeasonStartRating").GetComponent<Text>().text = rating.ToString();
			gameObject3.transform.Find("SeasonEndRating").GetComponent<Text>().text = "<color=\"" + text + "\">" + rating2 + "</color>";
			gameObject3.transform.Find("AttrsStart").GetComponent<Text>().text = num2.ToString();
			gameObject3.transform.Find("AttrsEnd").GetComponent<Text>().text = "<color=\"" + text2 + "\">" + num3 + "</color>";
			string text3 = Player.GetName(num) + " ";
			for (int num4 = 0; num4 < 1; num4++)
			{
				float num5 = array2[num4] - array[num4];
				float num6 = num5 / 52f;
				text3 = text3 + "old=" + array[num4] + " new=" + array2[num4] + " change=" + num5 + " perweek=" + num6;
			}
		}
		gameObject.SetActive(value: true);
	}

	public static void DrawPlayerPotential()
	{
		GameObject gameObject = UI.GetInstance().overlayCanvas.transform.Find("DebugTools/Panel/PlayerRatingsPanel").gameObject.transform.Find("PlayerPotential").gameObject;
		foreach (Transform child in gameObject.transform.Find("Filter").gameObject.transform)
		{
			child.GetComponent<Button>().onClick.RemoveAllListeners();
			child.GetComponent<Button>().onClick.AddListener(() =>
			{
				FilterPlayerPotentialList(child.GetComponentInChildren<Text>().text);
			});
		}
		FilterPlayerPotentialList("14");
		gameObject.SetActive(value: true);
	}

	public static void FilterPlayerPotentialList(string filterAge)
	{
		GameObject gameObject = UI.GetInstance().overlayCanvas.transform.Find("DebugTools/Panel/PlayerRatingsPanel").gameObject.transform.Find("PlayerPotential").gameObject;
		if ((bool)gameObject.transform.Find("PlayerListCopy"))
		{
			UnityEngine.Object.Destroy(gameObject.transform.Find("PlayerListCopy").gameObject);
		}
		GameObject gameObject2 = gameObject.transform.Find("PlayerList").gameObject;
		GameObject gameObject3 = UnityEngine.Object.Instantiate(gameObject2, gameObject.transform, worldPositionStays: false);
		gameObject3.name = "PlayerListCopy";
		gameObject2.SetActive(value: false);
		gameObject3.SetActive(value: true);
		GameObject gameObject4 = gameObject3.transform.Find("Content").gameObject;
		GameObject gameObject5 = gameObject4.transform.Find("Row").gameObject;
		gameObject5.SetActive(value: false);
		int num = int.Parse(filterAge);
		List<int> list = new List<int>();
		for (int i = 1; i < Game.instance.g_PlayerData_NextPlayerID; i++)
		{
			if (!PlayerData.GetRetired(i))
			{
				list.Add(i);
			}
		}
		Dictionary<int, PlayerData> playerData = Game.instance.g_PlayerData;
		list.Sort((int a, int b) =>
		{
			int generatedPotential2 = playerData[a].GeneratedPotential;
			int generatedPotential3 = playerData[b].GeneratedPotential;
			if (generatedPotential2 > generatedPotential3)
			{
				return -1;
			}
			return (generatedPotential3 > generatedPotential2) ? 1 : (Player.GetRating(b) - Player.GetRating(a));
		});
		for (int num2 = 0; num2 < list.Count; num2++)
		{
			int num3 = list[num2];
			if (Player.GetAge(num3) == num)
			{
				bool indecline = PlayerData.GetIndecline(num3);
				int generatedPotential = playerData[num3].GeneratedPotential;
				int startAge = ((!PlayerData.IsRespawn(num3)) ? Player.GetAge(num3, Game.instance.StartDate) : 0);
				byte rating = Game.instance.g_PlayerData[num3].GetRating();
				int num4 = PlayerData.GetPreferredPosition(num3);
				if (num4 == 0)
				{
					num4 = PlayerData.GetPosition(num3);
				}
				int playerPotentialRandomness = PlayerPotentialManager.GetPlayerPotentialRandomness((short)startAge);
				int num5 = generatedPotential - playerPotentialRandomness;
				int minimumPlayerPotential = PlayerPotentialManager.GetMinimumPlayerPotential((byte)startAge, rating);
				if (num5 < minimumPlayerPotential)
				{
					num5 = minimumPlayerPotential;
				}
				int num6 = generatedPotential + playerPotentialRandomness;
				int num7 = PlayerPotentialManager.GetMaximumPlayerPotential((byte)startAge, num4, rating);
				if (num7 < generatedPotential)
				{
					num7 = generatedPotential;
				}
				if (num6 > num7)
				{
					num6 = num7;
				}
				GameObject gameObject6 = UnityEngine.Object.Instantiate(gameObject5, gameObject4.transform, worldPositionStays: false);
				gameObject6.transform.Find("ID").GetComponent<Text>().text = num3.ToString();
				gameObject6.transform.Find("Name").GetComponent<Text>().text = Player.GetName(num3);
				gameObject6.transform.Find("Rating").GetComponent<Text>().text = Player.GetRating(num3).ToString();
				gameObject6.transform.Find("Position").GetComponent<Text>().text = Helpers.GetPlayerPositionString(Player.GetPosition(num3, preferred: true));
				gameObject6.transform.Find("Potential").GetComponent<Text>().text = generatedPotential.ToString() + " (" + Game.instance.g_PlayerData[num3].Potential + ")";
				if (indecline)
				{
					gameObject6.transform.Find("Up").gameObject.SetActive(value: false);
					gameObject6.transform.Find("Down").gameObject.SetActive(value: true);
				}
				else
				{
					gameObject6.transform.Find("Up").gameObject.SetActive(value: true);
					gameObject6.transform.Find("Down").gameObject.SetActive(value: false);
				}
				gameObject6.transform.Find("PotentialRange").GetComponent<Text>().text = num5 + " - <color=blue>" + generatedPotential + "</color> - " + num6;
				gameObject6.SetActive(value: true);
			}
		}
	}

	private static void DrawPlayerAttributes()
	{
		GameObject gameObject = UI.GetInstance().overlayCanvas.transform.Find("DebugTools/Panel/PlayerRatingsPanel").gameObject.transform.Find("PlayerAttributes").gameObject;
		GameObject topPanel = gameObject.transform.Find("TopPanel").gameObject;
		GameObject content = gameObject.transform.Find("Content").gameObject;
		Button component = topPanel.transform.Find("SearchButton").GetComponent<Button>();
		component.onClick.RemoveAllListeners();
		component.onClick.AddListener(() =>
		{
			int num = int.Parse(topPanel.transform.Find("SearchField/Text").GetComponent<Text>().text);
			if (num > 0)
			{
				SearchPlayerAttributes(num);
			}
		});
		Button component2 = content.transform.Find("Edit/Refresh").GetComponent<Button>();
		component2.onClick.RemoveAllListeners();
		component2.onClick.AddListener(() =>
		{
			SearchPlayerAttributes(int.Parse(content.transform.Find("Edit/PlayerDataID").GetComponent<Text>().text), refresh: true);
		});
		if (content.transform.Find("Edit/PlayerDataID").GetComponent<Text>().text == "")
		{
			content.SetActive(value: false);
		}
		gameObject.SetActive(value: true);
	}

	public static void DrawTraining()
	{
		GameObject gameObject = UI.GetInstance().overlayCanvas.transform.Find("DebugTools/Panel/PlayerRatingsPanel").gameObject;
		GameObject trainingPanel = gameObject.transform.Find("Training").gameObject;
		trainingPanel.transform.Find("InitTraining").GetComponent<Button>().onClick.AddListener(() =>
		{
			Training._InitTrainingDebug();
		});
		trainingPanel.transform.Find("RunTraining").GetComponent<Button>().onClick.AddListener(() =>
		{
			Training._RunTrainingDebug();
			Training._DebugAllPlayerTrainingAttributes();
		});
		trainingPanel.transform.Find("GameObject/Input_att1").GetComponent<InputField>().text = Training.attrIncreases[0].ToString();
		trainingPanel.transform.Find("GameObject/Input_att2").GetComponent<InputField>().text = Training.attrIncreases[1].ToString();
		trainingPanel.transform.Find("GameObject/Input_att3").GetComponent<InputField>().text = Training.attrIncreases[2].ToString();
		trainingPanel.transform.Find("GameObject/Input_inj1").GetComponent<InputField>().text = Training.injuryChances[0].ToString();
		trainingPanel.transform.Find("GameObject/Input_inj2").GetComponent<InputField>().text = Training.injuryChances[1].ToString();
		trainingPanel.transform.Find("GameObject/Input_inj3").GetComponent<InputField>().text = Training.injuryChances[2].ToString();
		trainingPanel.transform.Find("GameObject/Input_fit1").GetComponent<InputField>().text = Training.fitnessMods[0].ToString();
		trainingPanel.transform.Find("GameObject/Input_fit2").GetComponent<InputField>().text = Training.fitnessMods[1].ToString();
		trainingPanel.transform.Find("GameObject/Input_fit3").GetComponent<InputField>().text = Training.fitnessMods[2].ToString();
		trainingPanel.transform.Find("GameObject/Input_rest").GetComponent<InputField>().text = TrainingDrills.drillFitnessMod[19].ToString();
		trainingPanel.transform.Find("GameObject/Input_fac").GetComponent<InputField>().text = Training.facilityBoostAmount.ToString();
		trainingPanel.transform.Find("SaveBtn").GetComponent<Button>().onClick.AddListener(() =>
		{
			SetNewTrainingValues(trainingPanel);
		});
		trainingPanel.transform.Find("ResetBtn").GetComponent<Button>().onClick.AddListener(() =>
		{
			ResetTrainingValues(trainingPanel);
		});
		trainingPanel.transform.Find("GameObject/Input_setint").GetComponent<InputField>().text = Game.instance.g_trainingDrillManager.DrillIntensity(0, 0).ToString();
		trainingPanel.SetActive(value: true);
	}

	public static void SetNewTrainingValues(GameObject trainingPanel)
	{
		Training.attrIncreases[0] = float.Parse(trainingPanel.transform.Find("GameObject/Input_att1").GetComponent<InputField>().text);
		Training.attrIncreases[1] = float.Parse(trainingPanel.transform.Find("GameObject/Input_att2").GetComponent<InputField>().text);
		Training.attrIncreases[2] = float.Parse(trainingPanel.transform.Find("GameObject/Input_att3").GetComponent<InputField>().text);
		Training.injuryChances[0] = double.Parse(trainingPanel.transform.Find("GameObject/Input_inj1").GetComponent<InputField>().text);
		Training.injuryChances[1] = double.Parse(trainingPanel.transform.Find("GameObject/Input_inj2").GetComponent<InputField>().text);
		Training.injuryChances[2] = double.Parse(trainingPanel.transform.Find("GameObject/Input_inj3").GetComponent<InputField>().text);
		Training.fitnessMods[0] = double.Parse(trainingPanel.transform.Find("GameObject/Input_fit1").GetComponent<InputField>().text);
		Training.fitnessMods[1] = double.Parse(trainingPanel.transform.Find("GameObject/Input_fit2").GetComponent<InputField>().text);
		Training.fitnessMods[2] = double.Parse(trainingPanel.transform.Find("GameObject/Input_fit3").GetComponent<InputField>().text);
		TrainingDrills.drillFitnessMod[19][0] = float.Parse(trainingPanel.transform.Find("GameObject/Input_rest").GetComponent<InputField>().text);
		Training.facilityBoostAmount = float.Parse(trainingPanel.transform.Find("GameObject/Input_fac").GetComponent<InputField>().text);
		Helpers.ShowToast("values set");
		for (int i = 0; i < 3; i++)
		{
			for (int j = 0; j < 7; j++)
			{
			}
		}
	}

	public static void ResetTrainingValues(GameObject trainingPanel)
	{
		trainingPanel.transform.Find("GameObject/Input_att1").GetComponent<InputField>().text = Training.attrIncreases[0].ToString();
		trainingPanel.transform.Find("GameObject/Input_att2").GetComponent<InputField>().text = Training.attrIncreases[1].ToString();
		trainingPanel.transform.Find("GameObject/Input_att3").GetComponent<InputField>().text = Training.attrIncreases[2].ToString();
		trainingPanel.transform.Find("GameObject/Input_inj1").GetComponent<InputField>().text = Training.injuryChances[0].ToString();
		trainingPanel.transform.Find("GameObject/Input_inj2").GetComponent<InputField>().text = Training.injuryChances[1].ToString();
		trainingPanel.transform.Find("GameObject/Input_inj3").GetComponent<InputField>().text = Training.injuryChances[2].ToString();
		trainingPanel.transform.Find("GameObject/Input_fit1").GetComponent<InputField>().text = Training.fitnessMods[0].ToString();
		trainingPanel.transform.Find("GameObject/Input_fit2").GetComponent<InputField>().text = Training.fitnessMods[1].ToString();
		trainingPanel.transform.Find("GameObject/Input_fit3").GetComponent<InputField>().text = Training.fitnessMods[2].ToString();
		trainingPanel.transform.Find("GameObject/Input_rest").GetComponent<InputField>().text = TrainingDrills.drillFitnessMod[19].ToString();
		trainingPanel.transform.Find("GameObject/Input_fac").GetComponent<InputField>().text = Training.facilityBoostAmount.ToString();
		trainingPanel.transform.Find("GameObject/Input_setint").GetComponent<InputField>().text = Game.instance.g_trainingDrillManager.DrillIntensity(0, 0).ToString();
	}

	public static void RunTraining()
	{
	}

	private static void SearchPlayerAttributes(int playerDataId, bool refresh = false)
	{
		GameObject gameObject = UI.GetInstance().overlayCanvas.transform.Find("DebugTools/Panel/PlayerRatingsPanel").gameObject.transform.Find("PlayerAttributes").gameObject.transform.Find("Content").gameObject;
		GameObject gameObject2 = gameObject.transform.Find("PlayerName").gameObject;
		GameObject original = gameObject.transform.Find("AttributeRow").gameObject;
		GameObject gameObject3 = gameObject.transform.Find("TechnicalAttrs").gameObject;
		GameObject gameObject4 = gameObject.transform.Find("MentalAttrs").gameObject;
		GameObject gameObject5 = gameObject.transform.Find("PhysicalAttrs").gameObject;
		GameObject gameObject6 = gameObject.transform.Find("Edit").gameObject;
		GameObject gameObject7 = gameObject6.transform.Find("PlayerDataID").gameObject;
		GameObject gameObject8 = gameObject6.transform.Find("RatingText").gameObject;
		GameObject gameObject9 = gameObject6.transform.Find("PotentialText").gameObject;
		int num = 0;
		PlayerData value = null;
		for (int i = 1; i < Game.instance.g_PlayerData_NextPlayerID; i++)
		{
			if (Game.instance.g_PlayerData.TryGetValue(i, out value) && PlayerData.GetPlayerDataID(i) == playerDataId)
			{
				num = i;
				break;
			}
		}
		if (num > 0)
		{
			int num2 = 0;
			int num3 = 0;
			if (refresh)
			{
				string text = gameObject8.transform.Find("Text").GetComponent<Text>().text;
				string text2 = gameObject9.transform.Find("Text").GetComponent<Text>().text;
				num2 = ((text != "") ? int.Parse(text) : value.GetRating());
				num3 = ((text2 != "") ? int.Parse(text2) : value.Potential);
			}
			else
			{
				num2 = value.GetRating();
				num3 = value.Potential;
				gameObject8.GetComponentInChildren<Text>().text = num2.ToString();
				gameObject9.GetComponentInChildren<Text>().text = num3.ToString();
			}
			AttrOpts attrOpts = new AttrOpts();
			attrOpts.SetDefaultAttrOpts();
			attrOpts.playertrainingon = false;
			AttrPlayerStruct playerDataObj = Attributes.GetPlayerDataObj(num, attrOpts);
			playerDataObj.rating = num2;
			playerDataObj.potential = num3;
			PlayerAttributes playerAttributes = Attributes.SetAttributesForPlayer(playerDataObj, attrOpts);
			playerDataObj.rating++;
			PlayerAttributes playerAttributes2 = Attributes.SetAttributesForPlayer(playerDataObj, attrOpts);
			gameObject2.GetComponent<Text>().text = Player.GetName(num);
			gameObject7.GetComponent<Text>().text = playerDataId.ToString();
			bool isKeeper = PlayerData.GetPosition(num) == 1;
			ClearChildObjects(gameObject3, 0);
			ClearChildObjects(gameObject4, 0);
			ClearChildObjects(gameObject5, 0);
			int[] technicalAttributesDisplayOrder = AttributesFunctions.GetTechnicalAttributesDisplayOrder(isKeeper);
			foreach (int num4 in technicalAttributesDisplayOrder)
			{
				FillPlayerAttributeRow(UnityEngine.Object.Instantiate(original, gameObject3.transform, worldPositionStays: false), num4, playerAttributes.attrsDisplay[num4], isKeeper, playerAttributes2.attrsDisplay[num4]);
			}
			technicalAttributesDisplayOrder = AttributesFunctions.GetMentalAttributesDisplayOrder(isKeeper);
			foreach (int num5 in technicalAttributesDisplayOrder)
			{
				FillPlayerAttributeRow(UnityEngine.Object.Instantiate(original, gameObject4.transform, worldPositionStays: false), num5, playerAttributes.attrsDisplay[num5], isKeeper, playerAttributes2.attrsDisplay[num5]);
			}
			technicalAttributesDisplayOrder = AttributesFunctions.GetPhysicalAttributesDisplayOrder(isKeeper);
			foreach (int num6 in technicalAttributesDisplayOrder)
			{
				FillPlayerAttributeRow(UnityEngine.Object.Instantiate(original, gameObject5.transform, worldPositionStays: false), num6, playerAttributes.attrsDisplay[num6], isKeeper, playerAttributes2.attrsDisplay[num6]);
			}
			DrawScoutAttributeRecommendations(num, playerAttributes);
			gameObject.SetActive(value: true);
		}
		else
		{
			gameObject.SetActive(value: false);
		}
	}

	private static void DrawScoutAttributeRecommendations(int playerId, PlayerAttributes playerAttributes)
	{
		GameObject gameObject = UI.GetInstance().overlayCanvas.transform.Find("DebugTools/Panel/PlayerRatingsPanel/PlayerAttributes/Content/ScoutAnalysis").gameObject;
		int desc = playerAttributes.desc;
		bool flag = desc == 1;
		int[] array = (flag ? Attributes.allAttributesKeeper : Attributes.allAttributesOutfield);
		int numberOfVotedAttributesToUse = Attributes.GetNumberOfVotedAttributesToUse(Game.instance.g_PlayerData[playerId].Potential, desc);
		int[] array2 = (from i in (from i in playerAttributes.orderedAttrsVotes.Take(numberOfVotedAttributesToUse)
				where i[1] > 0
				select i).ToList()
			select i[0]).ToArray();
		float num = (float)playerAttributes.attrs.Sum();
		playerAttributes.attrs.Max();
		float num2 = array.Length;
		float num3 = Mathf.Ceil(num / num2);
		float num4 = num3;
		string text = "";
		text = text + "avg: " + num3 + " target: " + num4 + " numTopAttrs: " + numberOfVotedAttributesToUse + " topAttrs: " + string.Join(",", array2.Select((int i) => i.ToString()).ToArray()) + "\n";
		Scout scout = Game.instance.g_Scouts[0];
		foreach (int num6 in array)
		{
			bool flag2 = Array.IndexOf(array2, num6) > -1;
			bool flag3 = scout.PlayerHasRequiredAttributes(playerId, new int[1] { num6 }, playerAttributes);
			text = text + AttributesFunctions.GetAttributeName(num6, flag) + ": ";
			text += (flag3 ? ("<color=green>RECOMMENDED " + (flag2 ? "(Top attribute)" : "(Position Based)") + "</color>") : "<color=red>NOT RECOMMENDED</color>");
			text += "\n";
		}
		gameObject.GetComponent<Text>().text = text;
	}

	private static void FillPlayerAttributeRow(GameObject row, int attrId, float attrValue, bool isKeeper, float increaseAttrValue = 0f)
	{
		row.transform.Find("Name").GetComponent<Text>().text = AttributesFunctions.GetAttributeName(attrId, isKeeper);
		row.transform.Find("Colour").GetComponent<Image>().color = Attributes.getAttributeValueColour((int)attrValue);
		row.transform.Find("Colour").GetComponentInChildren<Text>().text = attrValue.ToString();
		if (increaseAttrValue > 0f)
		{
			float num = increaseAttrValue - attrValue;
			if (num != 0f)
			{
				row.transform.Find("RatingIncrease").GetComponent<Text>().text = ((num > 0f) ? ("<color=lime>+" + num + "</color>") : ("<color=red>" + num + "</color>"));
			}
			else
			{
				row.transform.Find("RatingIncrease").gameObject.SetActive(value: false);
			}
		}
		row.SetActive(value: true);
	}

	public static void DrawPlayerDebug()
	{
		GameObject overlayCanvas = UI.GetInstance().overlayCanvas;
		GameObject playerpanel = overlayCanvas.transform.Find("DebugTools/Panel/PlayerPanel").gameObject;
		playerpanel.SetActive(value: true);
		Dropdown dropdown = playerpanel.transform.Find("SelectDropdown").GetComponent<Dropdown>();
		PopulatePlayerDropdown(dropdown);
		Button component = playerpanel.transform.Find("InjureBtn").GetComponent<Button>();
		Button component2 = playerpanel.transform.Find("SuspensionBtn").GetComponent<Button>();
		Button component3 = playerpanel.transform.Find("BestPlayersBtn").GetComponent<Button>();
		component.onClick.RemoveAllListeners();
		component.onClick.AddListener(() =>
		{
			InjurePlayer(GetPlayerIDFromIndex(Game.GetCurrentUserClubID(), dropdown.value), int.Parse(playerpanel.transform.Find("InjureField").GetComponent<InputField>().text));
		});
		component2.onClick.RemoveAllListeners();
		component2.onClick.AddListener(() =>
		{
			SuspendPlayerFromLeagueDebug(GetPlayerIDFromIndex(Game.GetCurrentUserClubID(), dropdown.value), int.Parse(playerpanel.transform.Find("SuspensionField").GetComponent<InputField>().text));
		});
		component3.onClick.RemoveAllListeners();
		component3.onClick.AddListener(() =>
		{
			CreateMostExpensivePlayersTSVForGoalDotCom();
		});
	}

	private static void PopulatePlayerDropdown(Dropdown dropdown)
	{
		short currentUserClubID = Game.GetCurrentUserClubID();
		dropdown.ClearOptions();
		if (currentUserClubID <= 0)
		{
			return;
		}
		List<Dropdown.OptionData> list = new List<Dropdown.OptionData>();
		foreach (int player in Club.GetPlayers(currentUserClubID).players)
		{
			list.Add(new Dropdown.OptionData(player + ": " + Player.GetName(player)));
		}
		dropdown.AddOptions(list);
	}

	private static int GetPlayerIDFromIndex(short clubid, int index)
	{
		return Club.GetPlayers(clubid).players.ElementAt(index);
	}

	public static void DrawDataTool()
	{
		GameObject gameObject = UI.GetInstance().overlayCanvas.transform.Find("DebugTools/Panel/DataPanel").gameObject;
		gameObject.SetActive(value: true);
		if (!gameObject.GetComponent<DebugDataTool>().hasInit)
		{
			gameObject.GetComponent<DebugDataTool>().Init();
		}
	}

	public static void DrawFacilitiesDebug()
	{
		GameObject overlayCanvas = UI.GetInstance().overlayCanvas;
		GameObject facilitiesPanel = overlayCanvas.transform.Find("DebugTools/Panel/FacilitiesPanel").gameObject;
		facilitiesPanel.SetActive(value: true);
		facilitiesPanel.transform.Find("TrainingInput").GetComponent<InputField>().SetTextWithoutNotify(Facilities.GetFacilityLevel(2).ToString());
		facilitiesPanel.transform.Find("YouthInput").GetComponent<InputField>().SetTextWithoutNotify(Facilities.GetFacilityLevel(1).ToString());
		facilitiesPanel.transform.Find("ScoutingInput").GetComponent<InputField>().SetTextWithoutNotify(Facilities.GetFacilityLevel(4).ToString());
		facilitiesPanel.transform.Find("ClubInput").GetComponent<InputField>().SetTextWithoutNotify(Facilities.GetFacilityLevel(0).ToString());
		facilitiesPanel.transform.Find("TrainingButton").GetComponent<Button>().onClick.RemoveAllListeners();
		facilitiesPanel.transform.Find("TrainingButton").GetComponent<Button>().onClick.AddListener(() =>
		{
			Game.instance.FacilityManager.FacilityLevels[2] = int.Parse(facilitiesPanel.transform.Find("TrainingInput").GetComponent<InputField>().text);
			Game.instance.g_trainingManager.SetDrillsAvailable();
		});
		facilitiesPanel.transform.Find("YouthButton").GetComponent<Button>().onClick.RemoveAllListeners();
		facilitiesPanel.transform.Find("YouthButton").GetComponent<Button>().onClick.AddListener(() =>
		{
			Game.instance.FacilityManager.FacilityLevels[1] = int.Parse(facilitiesPanel.transform.Find("YouthInput").GetComponent<InputField>().text);
		});
		facilitiesPanel.transform.Find("ScoutingButton").GetComponent<Button>().onClick.RemoveAllListeners();
		facilitiesPanel.transform.Find("ScoutingButton").GetComponent<Button>().onClick.AddListener(() =>
		{
			Game.instance.FacilityManager.FacilityLevels[4] = int.Parse(facilitiesPanel.transform.Find("ScoutingInput").GetComponent<InputField>().text);
		});
		facilitiesPanel.transform.Find("ClubButton").GetComponent<Button>().onClick.RemoveAllListeners();
		facilitiesPanel.transform.Find("ClubButton").GetComponent<Button>().onClick.AddListener(() =>
		{
			Game.instance.FacilityManager.FacilityLevels[0] = int.Parse(facilitiesPanel.transform.Find("ClubInput").GetComponent<InputField>().text);
		});
		Button component = facilitiesPanel.transform.Find("UnlockAllBtn").GetComponent<Button>();
		component.onClick.RemoveAllListeners();
		component.onClick.AddListener(() =>
		{
			Game.instance.FacilityManager.FacilityLevels[2] = 10;
			Game.instance.FacilityManager.FacilityLevels[1] = 10;
			Game.instance.FacilityManager.FacilityLevels[4] = 10;
			Game.instance.FacilityManager.FacilityLevels[0] = 10;
			facilitiesPanel.transform.Find("TrainingInput").GetComponent<InputField>().SetTextWithoutNotify(Facilities.GetFacilityLevel(2).ToString());
			facilitiesPanel.transform.Find("YouthInput").GetComponent<InputField>().SetTextWithoutNotify(Facilities.GetFacilityLevel(1).ToString());
			facilitiesPanel.transform.Find("ScoutingInput").GetComponent<InputField>().SetTextWithoutNotify(Facilities.GetFacilityLevel(4).ToString());
			facilitiesPanel.transform.Find("ClubInput").GetComponent<InputField>().SetTextWithoutNotify(Facilities.GetFacilityLevel(0).ToString());
			Game.instance.g_trainingManager.SetDrillsAvailable();
		});
	}

	private static void ClearChildObjects(GameObject parent, int leaveRows = 1)
	{
		foreach (Transform item in parent.transform)
		{
			if (item.GetSiblingIndex() >= leaveRows)
			{
				UnityEngine.Object.Destroy(item.gameObject);
			}
		}
	}

	public static void DrawTrainingTab()
	{
		GameObject gameObject = UI.GetInstance().overlayCanvas.transform.Find("DebugTools").gameObject;
		GameObject panel = gameObject.transform.Find("Panel/TrainingPanel").gameObject;
		panel.SetActive(value: true);
		Dropdown youthDropdown = panel.transform.Find("SelectDropdown").GetComponent<Dropdown>();
		youthDropdown.options.Clear();
		youthDropdown.options = new List<Dropdown.OptionData>
		{
			new Dropdown.OptionData("None"),
			new Dropdown.OptionData("Low gametime"),
			new Dropdown.OptionData("High gametime")
		};
		youthDropdown.onValueChanged.RemoveAllListeners();
		youthDropdown.onValueChanged.AddListener(delegate
		{
			Game.instance.g_trainingManager.DebugYouthTraining = youthDropdown.value;
			MonoBehaviour.print("youth training debug changed to: " + youthDropdown.value);
		});
		Button component = panel.transform.Find("WeeksBtn").GetComponent<Button>();
		component.onClick.RemoveAllListeners();
		component.onClick.AddListener(() =>
		{
			if (int.TryParse(panel.transform.Find("WeeksField/Text").GetComponent<Text>().text, out var result))
			{
				RunDebugTraining(result);
			}
			else
			{
				Helpers.ShowToast("Invalid weeks entered");
			}
		});
	}

	public static void RunDebugTraining(int weeks)
	{
		for (int i = 0; i < weeks; i++)
		{
			Game.instance.g_trainingManager.LastSessionDate = Game.instance.CurrentDate;
			Training.Process();
			Game.instance.CurrentDate = Game.instance.CurrentDate.AddDays(7.0);
		}
	}

	public static bool GetSaveCheckpointsEnabled()
	{
		return UseSaveCheckpoints;
	}

	public static void SetDate(DateTime date)
	{
		GameObject gameObject = UI.GetInstance().overlayCanvas.transform.Find("DebugTools").gameObject.transform.Find("Panel/MainPanel").gameObject;
		gameObject.transform.Find("InputDay").GetComponent<InputField>().text = date.Day.ToString();
		gameObject.transform.Find("InputMonth").GetComponent<InputField>().text = date.Month.ToString();
		gameObject.transform.Find("InputYear").GetComponent<InputField>().text = date.Year.ToString();
	}

	public static IEnumerator ContinueAfterMatchEngine()
	{
		UI.GetInstance().ChangeScreen(4);
		yield return new WaitForEndOfFrame();
		SetDate(Game.instance.DEBUG_TOOLS_PROCESSING_DATE);
		DoContinue();
	}

	private static void SetSkipToDate(DateTime date)
	{
		Transform transform = UI.GetInstance().overlayCanvas.transform.Find("DebugTools").Find("Panel/MainPanel");
		InputField component = transform.Find("InputDay").GetComponent<InputField>();
		InputField component2 = transform.Find("InputMonth").GetComponent<InputField>();
		InputField component3 = transform.Find("InputYear").GetComponent<InputField>();
		component.text = date.Day.ToString();
		component2.text = date.Month.ToString();
		component3.text = date.Year.ToString();
	}

	private static void AdvanceSkipToDate(int amount, AdvanceUnit unit)
	{
		Transform transform = UI.GetInstance().overlayCanvas.transform.Find("DebugTools").Find("Panel/MainPanel");
		InputField component = transform.Find("InputDay").GetComponent<InputField>();
		InputField component2 = transform.Find("InputMonth").GetComponent<InputField>();
		InputField component3 = transform.Find("InputYear").GetComponent<InputField>();
		bool flag = int.TryParse(component.text, out var result);
		bool flag2 = int.TryParse(component2.text, out var result2);
		bool flag3 = int.TryParse(component3.text, out var result3);
		if (flag & flag2 & flag3)
		{
			DateTime dateTime = new DateTime(result3, result2, result);
			dateTime = ((unit == AdvanceUnit.Days) ? dateTime.AddDays(amount) : dateTime.AddMonths(amount));
			component.text = dateTime.Day.ToString();
			component2.text = dateTime.Month.ToString();
			component3.text = dateTime.Year.ToString();
			transform.Find("Mentalist").gameObject.SetActive(dateTime.Month == 12 && dateTime.Day >= 23 && dateTime.Day <= 27);
		}
	}

	public static void DoContinue()
	{
		GameObject gameObject = UI.GetInstance().overlayCanvas.transform.Find("DebugTools").gameObject.transform.Find("Panel/MainPanel").gameObject;
		bool flag = int.TryParse(gameObject.transform.Find("InputDay").GetComponent<InputField>().text, out var result);
		bool flag2 = int.TryParse(gameObject.transform.Find("InputMonth").GetComponent<InputField>().text, out var result2);
		bool flag3 = int.TryParse(gameObject.transform.Find("InputYear").GetComponent<InputField>().text, out var result3);
		bool flag4 = false;
		if (flag & flag2 & flag3)
		{
			try
			{
				DateTime dateTime = new DateTime(result3, result2, result);
				if (dateTime > Game.instance.CurrentDate)
				{
					flag4 = true;
					LastSaveMonth = Game.instance.CurrentDate.Month;
					DateLastSaved = Game.instance.CurrentDate;
					Game.instance.DEBUG_TOOLS_PROCESSING = true;
					Game.instance.DEBUG_TOOLS_PROCESSING_MATCH_ENGINE = true;
					Game.instance.DEBUG_TOOLS_PROCESSING_DATE = dateTime;
					Game.instance.SkipMatch = true;
					PlayerPrefs.SetInt("MatchSpeed", 5);
					PlayerPrefs.SetInt("HighlightMode_1.0.5", 5);
					while (Game.instance.CurrentDate < dateTime && !Game.instance.ReturningFromMatchday)
					{
						IEnumerator enumerator = Game.instance.Continue();
						DoSaveCheckpoints();
						while (enumerator.MoveNext())
						{
						}
					}
				}
			}
			catch (Exception ex)
			{
				Debug.LogError("DebugTools::DoContinue() - exception: " + ex.Message + " " + ex.StackTrace);
			}
			finally
			{
				Game.instance.DEBUG_TOOLS_PROCESSING = false;
				Game.instance.DEBUG_TOOLS_PROCESSING_MATCH_ENGINE = false;
				UI.GetInstance().ChangeScreen(UI.GetInstance().g_currentscreen);
				Save.SaveData(Save.currentSlot);
			}
		}
		if (!flag4)
		{
			Helpers.ShowToast("Invalid date pal");
		}
	}

	private static void DoSaveCheckpoints()
	{
		if (!UseSaveCheckpoints)
		{
			return;
		}
		switch (SaveCheckpointType)
		{
		case CheckpointType.Weekly:
			if ((Game.instance.CurrentDate - DateLastSaved).TotalDays >= 7.0)
			{
				Save.SaveData(Save.currentSlot, showAnimation: false);
				DateLastSaved = Game.instance.CurrentDate;
			}
			break;
		case CheckpointType.Monthly:
			if (LastSaveMonth < Game.instance.CurrentDate.Month || (LastSaveMonth == 12 && Game.instance.CurrentDate.Month == 1))
			{
				Save.SaveData(Save.currentSlot, showAnimation: false);
				LastSaveMonth = Game.instance.CurrentDate.Month;
			}
			break;
		case CheckpointType.Seasonally:
		{
			byte seasonOneStart = Association.GetTopDivision(Club.GetAssociationID(Game.GetCurrentUserClubID())).SeasonOneStart;
			if (Game.instance.CurrentDate.Month == seasonOneStart - 1 && Game.instance.CurrentDate.Day > 21 && !SavedThisSeason)
			{
				Save.SaveData(Save.currentSlot, showAnimation: false);
				SavedThisSeason = true;
			}
			else if (Game.instance.CurrentDate.Month == seasonOneStart && SavedThisSeason)
			{
				SavedThisSeason = false;
			}
			break;
		}
		}
	}

	public static void InjurePlayer(int playerID, int days)
	{
		Game.instance.g_injuredPlayers.AddPlayerInjury(playerID, Game.instance.CurrentDate, Game.instance.CurrentDate.AddDays(days), 100);
		Helpers.ShowToast("(DEBUG) Player injured");
	}

	public static void SuspendPlayerFromLeagueDebug(int playerID, int matches)
	{
		int associationID = Player.GetAssociationID(playerID);
		Game.instance.g_association[associationID].MatchBan.Add(playerID, matches);
		Helpers.ShowToast("(DEBUG) Player suspended from league");
	}

	public static void CreateMostExpensivePlayersTSVForGoalDotCom()
	{
		string path = Application.persistentDataPath + "/PlayerData.tsv";
		List<PlayerData> source = Game.instance.g_PlayerData.Values.OrderByDescending((PlayerData x) => x.GetValue()).Take(1000).ToList();
		source = source.OrderByDescending((PlayerData x) => Player.GetRating(x.PlayerId)).ToList();
		using StreamWriter streamWriter = new StreamWriter(path);
		streamWriter.Write("PlayerDataId\tName\tRating\tAge\tClub\tCountry\tPosition\tValue\tMarking\tTackling\tPositioning\tMovement\tFinishing\tDribbling\tLong Shots\tCrossing\tControl\tLong Passing\tPassing\tPenalties\tFree kicks\tCorners\tCreativity\tAerial Ability\tPace\tStamina\tStrength\tWork Rate\tAggression\tComposure\tConcentration\tFlair\tLeadership\n");
		foreach (PlayerData item in source)
		{
			bool flag = item.IsGoalKeeper();
			PlayerAttributes attributesForDisplay = Attributes.GetAttributesForDisplay(item.PlayerId);
			streamWriter.Write(item.PlayerDataID + "\t" + Player.GetName(item.PlayerId, longName: true) + "\t" + Player.GetDisplayRating(item.PlayerId) + "\t" + item.Age() + "\t" + Club.GetName(item.ClubId) + "\t" + CountryData.GetName(item.CountryDataID) + "\t" + Helpers.GetPlayerPositionString(item.Position) + "\t" + Helpers.GetValueText(item.GetValue()) + "\t" + $"{((!flag) ? ((int)attributesForDisplay.attrsDisplay[10]) : 0)}" + "\t" + $"{((!flag) ? ((int)attributesForDisplay.attrsDisplay[11]) : 0)}" + "\t" + $"{((!flag) ? ((int)attributesForDisplay.attrsDisplay[12]) : 0)}" + "\t" + $"{((!flag) ? ((int)attributesForDisplay.attrsDisplay[14]) : 0)}" + "\t" + $"{((!flag) ? ((int)attributesForDisplay.attrsDisplay[15]) : 0)}" + "\t" + $"{((!flag) ? ((int)attributesForDisplay.attrsDisplay[16]) : 0)}" + "\t" + $"{((!flag) ? ((int)attributesForDisplay.attrsDisplay[17]) : 0)}" + "\t" + $"{((!flag) ? ((int)attributesForDisplay.attrsDisplay[18]) : 0)}" + "\t" + $"{((!flag) ? ((int)attributesForDisplay.attrsDisplay[19]) : 0)}" + "\t" + $"{((!flag) ? ((int)attributesForDisplay.attrsDisplay[20]) : 0)}" + "\t" + $"{((!flag) ? ((int)attributesForDisplay.attrsDisplay[21]) : 0)}" + "\t" + $"{((!flag) ? ((int)attributesForDisplay.attrsDisplay[24]) : 0)}" + "\t" + $"{((!flag) ? ((int)attributesForDisplay.attrsDisplay[23]) : 0)}" + "\t" + $"{((!flag) ? ((int)attributesForDisplay.attrsDisplay[22]) : 0)}" + "\t" + $"{((!flag) ? ((int)attributesForDisplay.attrsDisplay[13]) : 0)}" + "\t" + $"{((!flag) ? ((int)attributesForDisplay.attrsDisplay[6]) : 0)}" + "\t" + $"{((!flag) ? ((int)attributesForDisplay.attrsDisplay[4]) : 0)}" + "\t" + $"{((!flag) ? ((int)attributesForDisplay.attrsDisplay[25]) : 0)}" + "\t" + $"{((!flag) ? ((int)attributesForDisplay.attrsDisplay[5]) : 0)}" + "\t" + $"{((!flag) ? ((int)attributesForDisplay.attrsDisplay[27]) : 0)}" + "\t" + $"{((!flag) ? ((int)attributesForDisplay.attrsDisplay[0]) : 0)}" + "\t" + $"{((!flag) ? ((int)attributesForDisplay.attrsDisplay[1]) : 0)}" + "\t" + $"{((!flag) ? ((int)attributesForDisplay.attrsDisplay[2]) : 0)}" + "\t" + $"{((!flag) ? ((int)attributesForDisplay.attrsDisplay[7]) : 0)}" + "\t" + $"{((!flag) ? ((int)attributesForDisplay.attrsDisplay[8]) : 0)}" + "\n");
		}
	}

	public static string TEST_IllegalUnplayedGames()
	{
		string text = "TEST_IllegalUnplayedGames() : ";
		try
		{
			int num = 0;
			for (int i = 0; i < Enumerable.Count(Game.instance.g_season.Schedule.schedule.List); i++)
			{
				if (!(Game.instance.g_season.Schedule.schedule.List[i].Date < Game.instance.CurrentDate))
				{
					continue;
				}
				List<Fixture> fixtures = Game.instance.g_season.Schedule.schedule.List[i].Fixtures;
				for (int j = 0; j < fixtures.Count; j++)
				{
					if (!fixtures[j].IsPlayed)
					{
						num++;
					}
				}
			}
			if (num > 0)
			{
				text = text + "FAIL - " + num + " played fixtures before the current date";
			}
		}
		catch (Exception ex)
		{
			text = text + "EXCEPTION: " + ex.Message;
		}
		return text;
	}

	public static string TEST_FixtureTimesValid()
	{
		string text = "TEST_FixtureTimesValid() : ";
		try
		{
			int num = 0;
			for (int i = 0; i < Enumerable.Count(Game.instance.g_season.Schedule.schedule.List); i++)
			{
				List<Fixture> fixtures = Game.instance.g_season.Schedule.schedule.List[i].Fixtures;
				for (int j = 0; j < fixtures.Count; j++)
				{
					if (fixtures[j].Time.Hour == 0)
					{
						num++;
					}
				}
			}
			if (num > 0)
			{
				text = text + "FAIL - " + num + " invalid fixture times";
			}
		}
		catch (Exception ex)
		{
			text = text + "EXCEPTION: " + ex.Message;
		}
		return text;
	}

	public static string TEST_PlayerAges(int maxPlayerAge)
	{
		string text = "TEST_PlayerAges() : ";
		string text2 = "";
		try
		{
			PlayerData value = null;
			for (int i = 1; i < Game.instance.g_PlayerData_NextPlayerID; i++)
			{
				if (Game.instance.g_PlayerData.TryGetValue(i, out value) && !PlayerData.GetRetired(i) && value.Age() > maxPlayerAge)
				{
					text2 = text2 + "{ " + Player.GetName(i) + "[" + i + "] = " + Player.GetAge(i) + " }, ";
				}
			}
			text = ((!(text2 != "")) ? (text + "<color=lime>PASS</color>") : (text + "<color=red>FAIL</color> - invalid player ages (retirement failed) : " + text2));
		}
		catch (Exception ex)
		{
			text = text + "EXCEPTION: " + ex.Message;
		}
		return text;
	}

	public static string TEST_PlayerRespawnDistribution(float diff)
	{
		string text = "TEST_PlayerRespawnDistribution() : ";
		string text2 = "";
		PlayerRespawnProfile g_player_respawnprofile = Game.instance.g_player_respawnprofile;
		int num = 0;
		int num2 = 1;
		g_player_respawnprofile.FillRespawnProfile();
		g_player_respawnprofile.FillSummaryForRequiredRespawns();
		int[,] origsummary = g_player_respawnprofile.origsummary;
		int[,] currsummary = g_player_respawnprofile.currsummary;
		int[,] diffsummary = g_player_respawnprofile.diffsummary;
		try
		{
			for (int i = 0; i < 6; i++)
			{
				diffsummary[i, num] = 0;
				diffsummary[i, num2] = 0;
				if (origsummary[i, num] > 0 && (double)currsummary[i, num] / (double)origsummary[i, num] < 1.0 - (double)diff)
				{
					diffsummary[i, num] = origsummary[i, num] - currsummary[i, num];
					text2 = text2 + PlayerRespawnProfile.GetRatingBandString(i) + "[" + num + "] needs: " + diffsummary[i, num] + " ";
				}
				if (origsummary[i, num2] > 0 && (double)currsummary[i, num2] / (double)origsummary[i, num2] < 1.0 - (double)diff)
				{
					diffsummary[i, num2] = origsummary[i, num2] - currsummary[i, num2];
					text2 = text2 + PlayerRespawnProfile.GetRatingBandString(i) + "[" + num2 + "] needs: " + diffsummary[i, num2] + " ";
				}
			}
			text = ((!(text2 != "")) ? (text + "<color=lime>PASS</color>") : (text + "<color=red>FAIL</color> - respawned players have not been transfered to a club : " + text2));
		}
		catch (Exception ex)
		{
			text = text + "EXCEPTION: " + ex.Message;
		}
		return text;
	}

	public static string TEST_PlayerPotential()
	{
		string text = "TEST_PlayerPotential() : ";
		string text2 = "";
		try
		{
			for (int i = 1; i < Game.instance.g_PlayerData_NextPlayerID; i++)
			{
				if (Game.instance.g_PlayerData.TryGetValue(i, out var _) && !PlayerData.GetRetired(i))
				{
					int potential = Game.instance.g_PlayerData[i].Potential;
					if (potential < 60 || potential > 99)
					{
						text2 = text2 + "{ " + Player.GetName(i) + "[" + i + "(" + PlayerData.GetPlayerDataID(i) + ")] = " + potential + " }, ";
					}
				}
			}
			text = ((!(text2 != "")) ? (text + "<color=lime>PASS</color>") : (text + "<color=red>FAIL</color> - invalid player potential/s : " + text2));
		}
		catch (Exception ex)
		{
			text = text + "EXCEPTION: " + ex.Message;
		}
		return text;
	}
}
