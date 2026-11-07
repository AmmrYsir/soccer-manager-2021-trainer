using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CielaSpike;
using Newtonsoft.Json;
using SimpleJSON;
using UnityEngine;
using UnityEngine.Networking;

public class Save : MonoBehaviour
{
	public enum SaveGameState
	{
		StartSave,
		BackedUpFiles,
		CompletedSave,
		DeletedBackups
	}

	public class SaveLog
	{
		public int SaveSlot;

		public SaveGameState SaveState;
	}

	public static int saveVersion = 18;

	public static string buildVersion;

	public static string saveUID;

	public static int dataPackID;

	public static int currentSlot = -1;

	private static bool showSaveAnimation = false;

	private static string saveFileName = "";

	private static float saveStartTime = 0f;

	public static bool saveInProgress = false;

	private static string errorMethod = "";

	private static string errorMessage = "";

	private static string errorStackTrace = "";

	public static bool LoadGameFailure = false;

	public static string LoadGameFailureError = "";

	public static void Init()
	{
		Game.instance.sqLiteService = new SQLiteService();
	}

	public static void SaveData()
	{
		SaveData(currentSlot);
	}

	public static void SaveData(int slotNumber, bool showAnimation = true, bool firstSave = false)
	{
		if (!SaveDataReturnResult(slotNumber, showAnimation, firstSave))
		{
			ModalController.Instance().ExitGame();
		}
	}

	public static bool SaveDataReturnResult(int slotNumber, bool showAnimation = false, bool firstSave = false)
	{
		if (saveInProgress && Time.realtimeSinceStartup - saveStartTime > 5f)
		{
			saveInProgress = false;
		}
		if (saveInProgress)
		{
			Helpers.ShowToast(Translation.GetText("saving-please-wait"));
			return false;
		}
		bool flag = false;
		SaveLog saveLog = new SaveLog
		{
			SaveSlot = slotNumber,
			SaveState = SaveGameState.StartSave
		};
		WriteSaveLog(saveLog);
		try
		{
			saveInProgress = true;
			saveStartTime = Time.realtimeSinceStartup;
			Game.instance.sqLiteService.CloseConnection();
			if (!firstSave)
			{
				BackupSaveFiles();
				saveLog.SaveState = SaveGameState.BackedUpFiles;
				WriteSaveLog(saveLog);
			}
			Game.instance.sqLiteService.Persist();
			SaveGame(slotNumber, showAnimation, firstSave);
			saveLog.SaveState = SaveGameState.CompletedSave;
			WriteSaveLog(saveLog);
			flag = true;
		}
		catch (Exception ex)
		{
			ModalController.Instance().CloseAllModals();
			Debug.LogError("Save Game Failed " + ex.Message);
			Helpers.ShowSaveToast();
			Helpers.ShowSaveToastFailed();
			flag = false;
		}
		finally
		{
			if (flag && (Game.instance.DEBUG_TOOLS_PROCESSING || Game.instance.DEBUG_TOOLS_PROCESSING_MATCH_ENGINE) && DebugTools.GetSaveCheckpointsEnabled())
			{
				Game.instance.sqLiteService.CloseConnection();
				MakeSaveSnapshot(slotNumber);
				Game.instance.sqLiteService.OpenConnection(slotNumber);
			}
			if (flag)
			{
				DeleteBackupSaveFiles();
				saveLog.SaveState = SaveGameState.DeletedBackups;
				WriteSaveLog(saveLog);
				Game.instance.sqLiteService.OpenConnection(slotNumber);
			}
			else
			{
				HandleFailedSave(slotNumber, saveLog.SaveState);
			}
			saveInProgress = false;
		}
		DeleteSaveLog();
		return flag;
	}

	private static void WriteSaveLog(SaveLog saveLog)
	{
		using StreamWriter textWriter = File.CreateText(Application.persistentDataPath + "/SaveLog.txt");
		new JsonSerializer().Serialize(textWriter, saveLog);
	}

	private static SaveLog GetSaveLog()
	{
		SaveLog saveLog = null;
		using StreamReader reader = File.OpenText(Application.persistentDataPath + "/SaveLog.txt");
		return (SaveLog)new JsonSerializer().Deserialize(reader, typeof(SaveLog));
	}

	private static void DeleteSaveLog()
	{
		string path = Application.persistentDataPath + "/SaveLog.txt";
		try
		{
			if (File.Exists(path))
			{
				File.Delete(path);
			}
		}
		catch (Exception)
		{
		}
	}

	private static void BackupSaveFiles()
	{
		Game.instance.sqLiteService.SaveDBToTemp();
		string text = Application.persistentDataPath + "/SM21_save_" + currentSlot;
		string destFileName = Application.persistentDataPath + "/SM21_save_BACKUP";
		if (File.Exists(text))
		{
			File.Copy(text, destFileName, overwrite: true);
			return;
		}
		throw new Exception("Could not find file " + text);
	}

	private static void DeleteBackupSaveFiles()
	{
		try
		{
			Game.instance.sqLiteService.DeleteDB(-1);
			string identifier = "SM21_save_BACKUP";
			if (ES2.Exists(identifier))
			{
				ES2.Delete(identifier);
			}
		}
		catch (Exception)
		{
		}
	}

	private static void HandleFailedSave(int slotNumber, SaveGameState saveState)
	{
		Game.instance.sqLiteService.CloseConnection();
		switch (saveState)
		{
		case SaveGameState.BackedUpFiles:
			try
			{
				Game.instance.sqLiteService.CopyDB(-1, slotNumber);
				try
				{
					string text = "SM21_" + slotNumber + ".db-journal";
					if (File.Exists(Application.persistentDataPath + "/" + text))
					{
						File.Delete(Application.persistentDataPath + "/" + text);
					}
				}
				catch (Exception)
				{
				}
				string destFileName = Application.persistentDataPath + "/SM21_save_" + slotNumber;
				File.Copy(Application.persistentDataPath + "/SM21_save_BACKUP", destFileName, overwrite: true);
			}
			catch (Exception)
			{
			}
			break;
		default:
			_ = 2;
			break;
		case SaveGameState.StartSave:
			break;
		}
		DeleteBackupSaveFiles();
	}

	public static void ProcessSaveRecovery()
	{
		string path = Application.persistentDataPath + "/SaveLog.txt";
		try
		{
			if (File.Exists(path))
			{
				SaveLog saveLog = GetSaveLog();
				if (saveLog != null)
				{
					HandleFailedSave(saveLog.SaveSlot, saveLog.SaveState);
				}
				DeleteSaveLog();
			}
		}
		catch (Exception)
		{
		}
	}

	private static void MakeSaveSnapshot(int slotNumber)
	{
		DateTime currentDate = Game.instance.CurrentDate;
		string text = "SM21_save_" + slotNumber;
		string text2 = "SM21_save_" + currentDate.Year + "_" + currentDate.Month.ToString("D2") + "_" + currentDate.Day.ToString("D2");
		string text3 = "SM21_" + slotNumber + ".db";
		string text4 = "SM21_" + currentDate.Year + "_" + currentDate.Month.ToString("D2") + "_" + currentDate.Day.ToString("D2") + ".db";
		File.Copy(Application.persistentDataPath + "/" + text, Application.persistentDataPath + "/" + text2, overwrite: true);
		File.Copy(Application.persistentDataPath + "/" + text3, Application.persistentDataPath + "/" + text4, overwrite: true);
	}

	public static string GenerateUniqueID(int slot, int club)
	{
		return DateTime.Now.ToString() + "_" + slot + "_" + club;
	}

	private static void SaveGame(int slotNumber, bool showAnimation = true, bool firstSave = false)
	{
		if (saveVersion != 18)
		{
			saveVersion = 18;
		}
		short currentUserClubID = Game.GetCurrentUserClubID();
		_ = Game.instance.g_currUser;
		SaveMetadata saveMetadata = LoadMetaData(currentSlot);
		string text;
		int loadedDataPackId;
		if ((saveMetadata == null) | firstSave)
		{
			text = GenerateUniqueID(slotNumber, currentUserClubID);
			loadedDataPackId = DataPackController.GetInstance().LoadedDataPackId;
		}
		else
		{
			text = saveMetadata.SaveUID;
			loadedDataPackId = saveMetadata.DataPackID;
		}
		SaveMetadata param = new SaveMetadata
		{
			Balance = (int)Game.instance.CurrentManagedClub.Balance,
			BuildVersion = buildVersion,
			ClubBadge = ((Game.instance.g_ClubData[currentUserClubID].LogoFilename != null) ? Game.instance.g_ClubData[currentUserClubID].LogoFilename : ""),
			SaveUID = text,
			GameDate = Game.instance.CurrentDate,
			LeaguePosition = LeagueList.GetLeaguePosition(currentUserClubID),
			Name = Club.GetName(currentUserClubID),
			SaveDate = DateTime.Now,
			ClubID = currentUserClubID,
			ClubDataID = Game.instance.g_ClubData[currentUserClubID].ClubDataID,
			DataPackID = loadedDataPackId
		};
		dataPackID = loadedDataPackId;
		saveUID = text;
		string text2 = "SM21_save_" + slotNumber;
		CustomerDataController.Instance.GetCurUserID();
		showSaveAnimation = showAnimation;
		saveFileName = text2;
		if (showSaveAnimation)
		{
			Helpers.ShowSaveToast();
		}
		Game.instance.transferInstance = Transfers.GetInstance();
		try
		{
			string identifier = "SM21_save_TEMP?tag=metadata".Replace("&#34;", "");
			ES2.Save(param, identifier);
			if (SaveInBackgroundThread())
			{
				SaveSuccess();
				return;
			}
			Helpers.ShowSaveToastFailed();
			throw new Exception("Save failed");
		}
		catch (IOException ex)
		{
			if (ex.Message.IndexOf("Disk full") != -1)
			{
				Game.isOutOfMemoryError = true;
			}
			else
			{
				Game.saveError = true;
				errorMessage = string.Concat(ex.GetType(), " - ", ex.Message);
				errorStackTrace = ex.StackTrace;
				errorMethod = "Save::SaveGame()";
			}
			throw ex;
		}
		catch (Exception ex2)
		{
			Game.saveError = true;
			errorMessage = string.Concat(ex2.GetType(), " - ", ex2.Message);
			errorStackTrace = ex2.StackTrace;
			errorMethod = "Save::SaveGame()";
			Helpers.ShowSaveToastFailed();
			ES2.Delete("SM21_save_TEMP");
			throw ex2;
		}
	}

	private static bool SaveInBackgroundThread()
	{
		bool result = false;
		try
		{
			ES2.Save(Game.instance, "SM21_save_TEMP?tag=game");
			result = true;
		}
		catch (OutOfMemoryException)
		{
			Game.isOutOfMemoryError = true;
		}
		catch (Exception ex2)
		{
			Game.saveError = true;
			errorMessage = string.Concat(ex2.GetType(), " - ClubID = ", Game.GetCurrentUserClubID().ToString(), " - ", ex2.Message);
			errorStackTrace = ex2.StackTrace;
			errorMethod = "Save::SaveInBackgroundThread()";
			Helpers.ShowSaveToastFailed();
			ES2.Delete("SM21_save_TEMP");
		}
		return result;
	}

	private static void SaveSuccess()
	{
		if (ES2.Exists("SM21_save_TEMP?tag=metadata"))
		{
			if (ES2.Exists(saveFileName))
			{
				ES2.Delete(saveFileName);
			}
			ES2.Rename("SM21_save_TEMP", saveFileName);
			if (showSaveAnimation)
			{
				Helpers.ShowSaveToastDone();
			}
		}
		else
		{
			Helpers.ShowSaveToastFailed();
			Game.saveError = true;
			errorMessage = " No Meta data found after Save - ClubID = " + Game.GetCurrentUserClubID();
			errorStackTrace = "";
			errorMethod = "Save::SaveSuccess()";
		}
	}

	public static void ReOpenDatabaseConnection()
	{
		if (currentSlot >= 0)
		{
			Game.instance.sqLiteService.OpenConnection(currentSlot);
		}
	}

	public static void GetCouldSaveMetadata()
	{
		new AjaxRequest();
		new Dictionary<string, object>
		{
			{ "action", "getcloudmeta" },
			{
				"custid",
				CustomerDataController.Instance.GetCurUserID()
			}
		};
	}

	private static void CloudMetaDataCallback(UnityWebRequest response)
	{
		if (response.error == null && JSON.Parse(response.downloadHandler.text)["Slots"].Count > 0)
		{
			UI.GetInstance().ShowLoadGameBtn();
		}
	}

	public static void GetAndDrawCloudSaveSlotMetadata()
	{
		new AjaxRequest();
		new Dictionary<string, object>
		{
			{ "action", "getcloudmeta" },
			{
				"custid",
				CustomerDataController.Instance.GetCurUserID()
			}
		};
	}

	private static void DrawCloudMetaDataCallback(UnityWebRequest response)
	{
		if (response.error == null)
		{
			JSONNode jSONNode = JSON.Parse(response.downloadHandler.text)["Slots"];
			int count = jSONNode.Count;
			Dictionary<int, SaveMetadata> dictionary = new Dictionary<int, SaveMetadata>();
			for (int i = 0; i < count; i++)
			{
				SaveMetadata saveMetadata = new SaveMetadata();
				int asInt = jSONNode[i]["SlotNum"].AsInt;
				short clubID = (short)jSONNode[i]["ClubID"].AsInt;
				string value = jSONNode[i]["SaveDate"].Value;
				string value2 = jSONNode[i]["GameDate"].Value;
				saveMetadata.ClubID = clubID;
				saveMetadata.GameDate = DateTime.Parse(value2);
				saveMetadata.SaveDate = DateTime.Parse(value);
				dictionary.Add(asInt, saveMetadata);
			}
			if (ModalController.Instance().IsSaveLoadModalActive())
			{
				SaveLoadScreen.instance.DrawCloudSaveSlots(dictionary);
			}
		}
	}

	public static void GetFileFromCloud(int cloudSlotNum, int localSlotNum)
	{
		string curUserID = CustomerDataController.Instance.GetCurUserID();
		UnityWebRequest unityWebRequest = new UnityWebRequest("https://storage.googleapis.com/sm20savefiles/SM21_save_" + curUserID + "_" + cloudSlotNum);
		while (!unityWebRequest.isDone)
		{
		}
		if (unityWebRequest.error == null)
		{
			byte[] data = unityWebRequest.downloadHandler.data;
			string identifier = "SM21_save_" + localSlotNum;
			ES2.SaveRaw(data, identifier);
			if (SaveLoadScreen.instance != null)
			{
				SaveLoadScreen.instance.drawSaveSlots();
			}
			if (UI.screens[0].activeSelf)
			{
				SplashLoadingScreen.instance.DrawStartScreen();
			}
		}
	}

	public static Dictionary<int, SaveMetadata> GetSaveGameSlotMetadata()
	{
		Dictionary<int, SaveMetadata> dictionary = new Dictionary<int, SaveMetadata>();
		try
		{
			int num = 10;
			if (PlayerPrefs.HasKey("prefserversaveslots") && PlayerPrefs.GetString("prefserversaveslots") != "")
			{
				num = Convert.ToInt32(PlayerPrefs.GetString("prefserversaveslots"));
			}
			for (int i = 0; i < num; i++)
			{
				SaveMetadata saveMetadata = LoadMetaData(i);
				if (saveMetadata != null)
				{
					dictionary.Add(i, saveMetadata);
				}
			}
		}
		catch (Exception ex)
		{
			Debug.LogError("Save::GetSaveGameSlotMetadata() - exception " + ex.Message);
			Game.saveError = true;
			errorMessage = string.Concat(ex.GetType(), " - ", ex.Message);
			errorStackTrace = ex.StackTrace;
			errorMethod = "Save::GetSaveGameSlotMetadata()";
		}
		return dictionary;
	}

	public static int GetNumSaveSlots()
	{
		int num = 0;
		int num2 = 5;
		if (PlayerPrefs.HasKey("prefserversaveslots") && PlayerPrefs.GetString("prefserversaveslots") != "")
		{
			num2 = 5;
		}
		CustomerDataController.Instance.GetCurUserID();
		for (int i = 0; i < num2; i++)
		{
			if (ES2.Exists("SM21_save_" + i))
			{
				num++;
			}
		}
		return num;
	}

	public static bool isSaveSlotsEmpty()
	{
		return CustomerDataController.Instance.GetSaveSlots() == 0;
	}

	public static void DeleteFromCloud(int slotNumber)
	{
		new AjaxRequest();
		string curUserID = CustomerDataController.Instance.GetCurUserID();
		new Dictionary<string, object>
		{
			{ "action", "deleteCloudSlot" },
			{
				"custid",
				CustomerDataController.Instance.GetCurUserID()
			},
			{
				"slotnum",
				slotNumber.ToString()
			},
			{
				"filename",
				"SM21_save_" + curUserID + "_" + slotNumber
			}
		};
	}

	private static void DeleteFromCloudCallback(UnityWebRequest response)
	{
	}

	public static void DeleteSaveGame(int slotNumber)
	{
		try
		{
			CustomerDataController.Instance.GetCurUserID();
			string identifier = "SM21_save_" + slotNumber;
			if (ES2.Exists(identifier))
			{
				ES2.Delete(identifier);
				Game.instance.sqLiteService.CloseConnectionWithGarbageCollection();
				Game.instance.sqLiteService.DeleteDB(slotNumber);
				if (UI.GetInstance().g_currentscreen == 0)
				{
					SplashLoadingScreen.instance.DrawStartScreen();
				}
			}
		}
		catch (Exception ex)
		{
			Debug.LogError("Save::DeleteSaveGame() - failed to delete from slot " + slotNumber + " - " + ex.Message);
			Game.saveError = true;
			errorMessage = string.Concat(ex.GetType(), " - ", ex.Message);
			errorStackTrace = ex.StackTrace;
			errorMethod = "Save::DeleteSaveGame()";
		}
	}

	public static SaveMetadata LoadMetaData(int slotNumber)
	{
		string curUserID = CustomerDataController.Instance.GetCurUserID();
		string text = "SM21_save_" + slotNumber;
		Game.LogConsole("### LoadMetaData - customerID = " + curUserID);
		SaveMetadata result = null;
		try
		{
			if (ES2.Exists(text))
			{
				result = ES2.Load<SaveMetadata>(text + "?tag=metadata");
			}
		}
		catch (Exception)
		{
		}
		return result;
	}

	public static string GetSaveUniqueID(int slotNumber)
	{
		return LoadMetaData(slotNumber).SaveUID;
	}

	public static IEnumerator LoadData(int slotNumber, string customerID)
	{
		string saveFilename = "SM21_save_" + slotNumber;
		yield return Ninja.JumpToUnity;
		yield return Ninja.JumpBack;
		if (!ES2.Exists(saveFilename))
		{
			yield break;
		}
		Constants.isGameLoading = true;
		yield return Ninja.JumpToUnity;
		SaveMetadata saveMetadata = ES2.Load<SaveMetadata>(saveFilename + "?tag=metadata");
		buildVersion = saveMetadata.BuildVersion;
		saveUID = saveMetadata.SaveUID;
		dataPackID = saveMetadata.DataPackID;
		UI.SetLoadingScreenStatus(Translation.GetText("loading-save-game"));
		ImageController.GetInstance().SetupImageCacheDirectories(saveMetadata.DataPackID);
		yield return new WaitForSeconds(0.05f);
		(float percentageDecimal, string loadingDescription)[] loadingInfo = new (float, string)[5]
		{
			(0.1f, Translation.GetText("loading-save-game")),
			(0.2f, Translation.GetText("downloading-game-data")),
			(0.4f, Translation.GetText("loading-images")),
			(0.6f, Translation.GetText("loading-league")),
			(0.8f, Translation.GetText("loading-schedule-data"))
		};
		for (int i = 0; i < loadingInfo.Length; i++)
		{
			if (!LoadGameFailure)
			{
				try
				{
					ES2.Load<Game>(saveFilename + "?tag=game");
					UI.SetLoadingScreenStatus(loadingInfo[i].loadingDescription, loadingInfo[i].percentageDecimal);
				}
				catch (Exception ex)
				{
					LoadGameFailure = true;
					LoadGameFailureError = ex.Message;
				}
				yield return new WaitForSeconds(0.05f);
			}
		}
		yield return new WaitForEndOfFrame();
		if (!LoadGameFailure)
		{
			try
			{
				Game.instance.sqLiteService.OpenConnection(slotNumber);
				Game.instance.g_injuredPlayers = new InjuredPlayers();
				Game.instance.sqLiteService.RestoreAll();
				Transfers.instance.unattachedPlayerManager.RebuildLists();
			}
			catch (Exception ex2)
			{
				LoadGameFailure = true;
				LoadGameFailureError = ex2.Message;
			}
		}
		if (LoadGameFailure)
		{
			Constants.isGameLoading = false;
			yield break;
		}
		try
		{
			Game.instance.GameStart();
			bool loadingInGame = currentSlot != -1;
			currentSlot = slotNumber;
			fillMissingData(loadingInGame);
			Stadium.yourGrassCut = PlayerPrefs.GetInt("GrassCutType" + currentSlot);
			EventLoggingController.LogEvent("loadgame", "clubName", EventLoggingParam.GetClubNameParamValue());
			Game.CheckManagedClubPlayers();
		}
		catch (Exception ex3)
		{
			LoadGameFailure = true;
			LoadGameFailureError = ex3.Message;
		}
		if (LoadGameFailure)
		{
			Constants.isGameLoading = false;
			yield break;
		}
		yield return Ninja.JumpBack;
		Constants.isGameLoading = false;
	}

	public static void fillMissingData(bool loadingInGame)
	{
		Game.LogConsole("Entering fillMissingData");
		Game instance = Game.instance;
		Transfers.instance = instance.transferInstance;
		Transfers.instance.initialise(instance.CurrentDate, isLoadGame: true);
		ClubList.PostLoad();
		PlayerList.PostLoad();
		PlayerConcernsScheduler.Instance.SetupTriggers();
		instance.g_season.Schedule.RemoveUnplayedCupGames();
		Cup.RefreshScheduleAfterGameLoad();
		ContinentalComp.RefreshScheduleAfterGameLoad();
		FormationData.MakeGridPos();
		CustomFormationController.LoadSavedFormations();
		if (loadingInGame)
		{
			UI.GetInstance().topBar.SetActive(value: true);
			UI.GetInstance().topBar.GetComponent<TopBar>().UpdateTeamName();
			UI.GetInstance().CheckForAndCreateScreenIfNeeded(14);
		}
	}

	public static int GetMostRecentSaveID(Dictionary<int, SaveMetadata> saves = null)
	{
		if (saves == null)
		{
			saves = GetSaveGameSlotMetadata();
		}
		int num = -1;
		if (saves.Count != 0)
		{
			num = saves.First().Key;
			foreach (int key in saves.Keys)
			{
				if (saves[num].SaveDate < saves[key].SaveDate)
				{
					num = key;
				}
			}
		}
		return num;
	}

	public static void SendErrorToDatabase()
	{
		new AjaxRequest();
		new Dictionary<string, object>
		{
			{ "action", "reportSaveError" },
			{
				"custid",
				CustomerDataController.Instance.GetCurUserID()
			},
			{
				"errormsg",
				errorMethod + " - " + errorMessage + " - " + errorStackTrace
			}
		};
		if (errorMessage.IndexOf("Disk full") == -1 && errorMessage.IndexOf("Too many open files") == -1)
		{
			string text = ((Application.platform == RuntimePlatform.IPhonePlayer) ? Application.version : Application.version);
			_ = Game.latestVersion == text;
		}
	}

	private static void SendErrorCallback(UnityWebRequest response)
	{
	}

	public static int saveVersionCompareTo(string version)
	{
		if (currentSlot == -1)
		{
			throw new Exception("Cannot use this outside of a saved game");
		}
		return version.CompareTo(buildVersion);
	}
}
