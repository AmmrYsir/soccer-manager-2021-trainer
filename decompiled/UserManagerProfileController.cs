using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;
using UnityEngine.Networking;

public static class UserManagerProfileController
{
	public const int MaxManagerLevels = 50;

	public const int PointsAwardWinMatch = 25;

	public const int PointsAwardWinComp = 100;

	public static List<ManagerTrait> StaticTraits;

	public static Dictionary<int, UserManagerLevel> StaticManagerLevels;

	public const int GamesNeedForCareerEntry = 10;

	public const int MaxCareerEntries = 15;

	public static bool ModalDisplayed;

	private static RewardVideoType localSaveRVT;

	private static GameObject localSaveModal;

	public static bool IsAdult => GetUserManagerProfileCurrent().IsOver18;

	public static UserManagerProfileContainer GetUserManagerProfileCurrent()
	{
		return CustomerDataController.Instance.CustomerDataHolder.Userdata.UserManagerProfile;
	}

	public static void PopulateStaticManagerTraits()
	{
		StaticTraits = JsonConvert.DeserializeObject<ManagerTraitList>(((TextAsset)Resources.Load("ManagerTraitsJson", typeof(TextAsset))).text, new JsonSerializerSettings
		{
			TypeNameHandling = TypeNameHandling.Objects
		}).ManagerTraits.OrderByDescending((ManagerTrait i) => i.BaseTrait).ToList();
	}

	public static void PopulateUserManagerLevels()
	{
		StaticManagerLevels = JsonConvert.DeserializeObject<UserManagerLevels>(((TextAsset)Resources.Load("UserManagerLevelsJson", typeof(TextAsset))).text, new JsonSerializerSettings
		{
			TypeNameHandling = TypeNameHandling.Objects
		}).Levels;
	}

	public static void SetNewUserProfile(string username, ManagerType baseType, bool isOver18)
	{
		UserManagerProfileContainer userManagerProfileCurrent = GetUserManagerProfileCurrent();
		userManagerProfileCurrent.ManagerUserName = username;
		userManagerProfileCurrent.BaseManagerType = baseType;
		userManagerProfileCurrent.IsOver18 = isOver18;
		AddBaseManagerTraits();
		CustomerDataController.Instance.SaveUserManagerprofileLoadingScreen(CustomerDataController.Instance.SaveUserDataCallback);
	}

	public static void AddOrUpdateTrophy(short clubID, Trophy trophy)
	{
		UserManagerProfileContainer userManagerProfileCurrent = GetUserManagerProfileCurrent();
		Trophy trophyCopy = trophy;
		if (!userManagerProfileCurrent.TrophiesEarned.ContainsKey(clubID))
		{
			userManagerProfileCurrent.TrophiesEarned.Add(clubID, new TrophyList(trophyCopy));
		}
		else if (!userManagerProfileCurrent.TrophiesEarned[clubID].Trophies.Any((Trophy i) => i.TrophyID == trophyCopy.TrophyID))
		{
			userManagerProfileCurrent.TrophiesEarned[clubID].Trophies.Add(trophyCopy);
		}
		else
		{
			userManagerProfileCurrent.TrophiesEarned[clubID].Trophies.FirstOrDefault((Trophy i) => i.TrophyID == trophyCopy.TrophyID).TimesWon++;
			userManagerProfileCurrent.TrophiesEarned[clubID].Trophies.FirstOrDefault((Trophy i) => i.TrophyID == trophyCopy.TrophyID).SeasonsWon.Add(Game.instance.g_season.SeasonID);
		}
		if (UserHasCareerEntry(Save.saveUID))
		{
			UpdateCareerEntry(Save.saveUID, Game.instance.ManagedClubProfile.GetCurrentCareerEntry());
		}
		else
		{
			AddNewCareerEntry(Save.saveUID, Game.instance.ManagedClubProfile.GetCurrentCareerEntry());
		}
	}

	public static int GetClubSeasonTrophyCount(short clubID, string seasonID)
	{
		UserManagerProfileContainer userManagerProfileCurrent = GetUserManagerProfileCurrent();
		if (userManagerProfileCurrent.TrophiesEarned.ContainsKey(clubID))
		{
			return userManagerProfileCurrent.TrophiesEarned[clubID].Trophies.Where((Trophy i) => i.SeasonsWon.Contains(seasonID)).Count();
		}
		return 0;
	}

	public static bool UserHasCareerEntry(string UID)
	{
		return GetUserManagerProfileCurrent().CareerHistory.ContainsKey(UID);
	}

	public static bool UserHasClubCareerEntry(string UID, short clubID)
	{
		return GetUserManagerProfileCurrent().CareerHistory[UID].CareerEntries.Any((CareerEntry i) => i.ClubID == clubID);
	}

	public static void AddNewCareerEntry(string UID, CareerEntry careerEntry, bool canLevelUp = false)
	{
		Helpers.LogConsole("###TheoBugTest() -- AddNewCareerEntry -- Enetered, with canLevelUp == " + canLevelUp);
		UserManagerProfileContainer userManagerProfileCurrent = GetUserManagerProfileCurrent();
		Helpers.LogConsole("###TheoBugTest() -- AddNewCareerEntry -- Got Cur User Data");
		if (userManagerProfileCurrent.CareerHistory.Count >= 15)
		{
			userManagerProfileCurrent.CareerHistory.Remove(userManagerProfileCurrent.CareerHistory.Keys.First());
		}
		Helpers.LogConsole("###TheoBugTest() -- AddNewCareerEntry -- checked to see if count is bigger then max.");
		if (!userManagerProfileCurrent.CareerHistory.ContainsKey(UID))
		{
			userManagerProfileCurrent.CareerHistory.Add(UID, new CareerEntryList(careerEntry));
		}
		Helpers.LogConsole("###TheoBugTest() -- AddNewCareerEntry -- checked to see if it contains key");
		if (canLevelUp)
		{
			Helpers.LogConsole("###TheoBugTest() -- AddNewCareerEntry -- user is able to level up");
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			dictionary.Add("CanLevelUp", true);
			Helpers.LogConsole("###TheoBugTest() -- AddNewCareerEntry -- user is able to level up, calling save function to allow levelup.");
			CustomerDataController.Instance.SaveUserManagerProfile(CustomerDataController.Instance.SaveUserDataCallback, dictionary);
		}
		else
		{
			Helpers.LogConsole("###TheoBugTest() -- AddNewCareerEntry -- user is not able to level up, saving anyway.");
			CustomerDataController.Instance.SaveUserManagerProfile(CustomerDataController.Instance.SaveUserDataCallback);
		}
	}

	public static void UpdateCareerEntry(string UID, CareerEntry careerEntry, bool canLevelUp = false)
	{
		Helpers.LogConsole("###TheoBugTest() -- UpdateCareerEntry -- Enetered, with canLevelUp == " + canLevelUp);
		UserManagerProfileContainer userManagerProfileCurrent = GetUserManagerProfileCurrent();
		Helpers.LogConsole("###TheoBugTest() -- UpdateCareerEntry -- Got Cur User Data");
		if (!userManagerProfileCurrent.CareerHistory[UID].CareerEntries.Any((CareerEntry i) => i.ClubID == careerEntry.ClubID))
		{
			userManagerProfileCurrent.CareerHistory[UID].CareerEntries.Add(careerEntry);
		}
		else
		{
			int index = userManagerProfileCurrent.CareerHistory[UID].CareerEntries.FindIndex((CareerEntry i) => i.ClubID == careerEntry.ClubID);
			userManagerProfileCurrent.CareerHistory[UID].CareerEntries[index] = careerEntry;
		}
		Helpers.LogConsole("###TheoBugTest() -- UpdateCareerEntry -- checked to see If the global history for this save slot does not contain this club. Add a new entry. Else update");
		Helpers.LogConsole("###TheoBugTest() -- UpdateCareerEntry -- Checking to see if level up is set.");
		if (canLevelUp)
		{
			Helpers.LogConsole("###TheoBugTest() -- UpdateCareerEntry -- user is able to level up");
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			dictionary.Add("CanLevelUp", true);
			Helpers.LogConsole("###TheoBugTest() -- UpdateCareerEntry -- calling level up function");
			CustomerDataController.Instance.SaveUserManagerProfile(CustomerDataController.Instance.SaveUserDataCallback, dictionary);
		}
		else
		{
			Helpers.LogConsole("###TheoBugTest() -- UpdateCareerEntry -- user is not able to level up, just saving anyway.");
			CustomerDataController.Instance.SaveUserManagerProfile(CustomerDataController.Instance.SaveUserDataCallback);
		}
	}

	public static bool ManagedInMultipleCountries()
	{
		return GetUserManagerProfileCurrent().CareerHistory.Values.Select((CareerEntryList i) => (from x in i.CareerEntries
			group x by Club.GetAssociationID(x.ClubID)).First()).ToList().Count > 1;
	}

	public static List<CareerEntry> GetAllCareerEntries()
	{
		UserManagerProfileContainer userManagerProfileCurrent = GetUserManagerProfileCurrent();
		if (userManagerProfileCurrent.CareerHistory.Count > 0)
		{
			return userManagerProfileCurrent.CareerHistory.SelectMany((KeyValuePair<string, CareerEntryList> i) => i.Value.CareerEntries).ToList();
		}
		return new List<CareerEntry>();
	}

	public static List<SeasonHistory> GetAllSeasonHistory()
	{
		UserManagerProfileContainer userManagerProfileCurrent = GetUserManagerProfileCurrent();
		if (userManagerProfileCurrent.CareerHistory.Count > 0)
		{
			return userManagerProfileCurrent.CareerHistory.SelectMany((KeyValuePair<string, CareerEntryList> i) => i.Value.CareerEntries.SelectMany((CareerEntry s) => s.seasonHistory.Values)).ToList();
		}
		return new List<SeasonHistory>();
	}

	public static Dictionary<string, float> GetAllSeasonsStats()
	{
		UserManagerProfileContainer userManagerProfileCurrent = GetUserManagerProfileCurrent();
		List<SeasonHistory> allSeasonHistory = GetAllSeasonHistory();
		Dictionary<string, float> dictionary = new Dictionary<string, float>();
		if (userManagerProfileCurrent.CareerHistory.Count > 0)
		{
			dictionary.Add("Games", allSeasonHistory.Sum((SeasonHistory i) => i.GamesPlayed));
			dictionary.Add("Wins", allSeasonHistory.Sum((SeasonHistory i) => i.Wins));
			dictionary.Add("Draws", allSeasonHistory.Sum((SeasonHistory i) => i.Draws));
			dictionary.Add("Loses", allSeasonHistory.Sum((SeasonHistory i) => i.Loses));
			dictionary.Add("WinPercentage", (float)allSeasonHistory.Sum((SeasonHistory i) => i.Wins) / (float)allSeasonHistory.Sum((SeasonHistory i) => i.GamesPlayed) * 100f);
			dictionary.Add("DrawPercentage", (float)allSeasonHistory.Sum((SeasonHistory i) => i.Draws) / (float)allSeasonHistory.Sum((SeasonHistory i) => i.GamesPlayed) * 100f);
			dictionary.Add("TrophyCount", userManagerProfileCurrent.TrophiesEarned.Values.Sum((TrophyList i) => i.Trophies.Sum((Trophy t) => t.TimesWon)));
		}
		else
		{
			dictionary.Add("Games", 0f);
			dictionary.Add("Wins", 0f);
			dictionary.Add("Draws", 0f);
			dictionary.Add("Loses", 0f);
			dictionary.Add("WinPercentage", 100f);
			dictionary.Add("DrawPercentage", 0f);
			dictionary.Add("TrophyCount", 0f);
		}
		return dictionary;
	}

	public static int GetTotalClubsManaged()
	{
		return GetUserManagerProfileCurrent().CareerHistory.Values.Count;
	}

	public static void AddManagerTraitToUser(ManagerTrait trait)
	{
		UserManagerProfileContainer userManagerProfileCurrent = GetUserManagerProfileCurrent();
		UsersManagerTraits item = new UsersManagerTraits(trait);
		userManagerProfileCurrent.UsersObtainedTraits.Add(item);
		CustomerDataController.Instance.SaveUserManagerProfile(CustomerDataController.Instance.SaveUserDataCallback);
	}

	public static void IncreaseTraitLevel(ManagerTraitArea area)
	{
		UsersManagerTraits usersManagerTraits = GetUserManagerProfileCurrent().UsersObtainedTraits.FirstOrDefault((UsersManagerTraits i) => i.Area == area);
		if (usersManagerTraits.CurrentTraitLevel < usersManagerTraits.LevelCount)
		{
			usersManagerTraits.CurrentTraitLevel++;
		}
		CustomerDataController.Instance.SaveUserManagerProfile(CustomerDataController.Instance.SaveUserDataCallback);
	}

	public static void AddBaseManagerTraits()
	{
		UserManagerProfileContainer curData = GetUserManagerProfileCurrent();
		curData.UsersObtainedTraits = StaticTraits.FindAll((ManagerTrait i) => i.Type == curData.BaseManagerType && i.BaseTrait).ConvertAll((ManagerTrait x) => new UsersManagerTraits(x));
	}

	public static bool SetBaseManager()
	{
		UserManagerProfileContainer userManagerProfileCurrent = GetUserManagerProfileCurrent();
		ManagerType key = (from i in (from x in userManagerProfileCurrent.UsersObtainedTraits
				group x by x.Type).ToDictionary((IGrouping<ManagerType, UsersManagerTraits> i) => i.Key, (IGrouping<ManagerType, UsersManagerTraits> i) => i.ToList())
			orderby i.Value.Sum((UsersManagerTraits x) => x.CurrentTraitLevel) descending
			select i).ToDictionary((KeyValuePair<ManagerType, List<UsersManagerTraits>> y) => y.Key, (KeyValuePair<ManagerType, List<UsersManagerTraits>> z) => z.Value).First().Key;
		if (key != userManagerProfileCurrent.BaseManagerType)
		{
			userManagerProfileCurrent.BaseManagerType = key;
			Helpers.ShowToast(Translation.GetText("new-manager-type").Replace("{managerType}", GetManagerTypeName()));
			CustomerDataController.Instance.SaveUserManagerProfile(CustomerDataController.Instance.SaveUserDataCallback);
			return true;
		}
		return false;
	}

	public static string GetManagerTypeName()
	{
		return GetUserManagerProfileCurrent().BaseManagerType switch
		{
			ManagerType.SuitManager => Translation.GetText("mantype-suit"), 
			ManagerType.TacticalManager => Translation.GetText("mantype-tact"), 
			ManagerType.TrainingManager => Translation.GetText("mantype-train"), 
			_ => "", 
		};
	}

	public static bool UserHasTrait(ManagerTraitArea area)
	{
		return GetUserManagerProfileCurrent().UsersObtainedTraits.Any((UsersManagerTraits i) => i.Area == area);
	}

	public static double ApplyTrait(ManagerTraitArea area, double orginalValue)
	{
		return GetUserManagerProfileCurrent().UsersObtainedTraits.FirstOrDefault((UsersManagerTraits i) => i.Area == area).ApplyAffect(orginalValue);
	}

	public static UsersManagerTraits GetUserTrait(ManagerTraitArea area)
	{
		return GetUserManagerProfileCurrent().UsersObtainedTraits.FirstOrDefault((UsersManagerTraits i) => i.Area == area);
	}

	public static bool TraitMaxed(ManagerTraitArea area)
	{
		return GetUserManagerProfileCurrent().UsersObtainedTraits.FirstOrDefault((UsersManagerTraits i) => i.Area == area).CurrentTraitLevel == GetMaxTraitLevel(area) - 1;
	}

	public static int GetCurrentTraitLevel(ManagerTraitArea area)
	{
		return GetUserManagerProfileCurrent().UsersObtainedTraits.FirstOrDefault((UsersManagerTraits i) => i.Area == area).CurrentTraitLevel;
	}

	public static int GetNextTraitLevel(ManagerTraitArea area)
	{
		int num = GetCurrentTraitLevel(area) + 1;
		int maxTraitLevel = GetMaxTraitLevel(area);
		if (num >= maxTraitLevel)
		{
			return maxTraitLevel - 1;
		}
		return num;
	}

	public static int GetMaxTraitLevel(ManagerTraitArea area)
	{
		return GetUserManagerProfileCurrent().UsersObtainedTraits.FirstOrDefault((UsersManagerTraits i) => i.Area == area).LevelCount;
	}

	public static int GetCurrentTraitCost(ManagerTraitArea area)
	{
		return GetUserManagerProfileCurrent().UsersObtainedTraits.FirstOrDefault((UsersManagerTraits i) => i.Area == area).Levels[GetCurrentTraitLevel(area)].LevelCost;
	}

	public static int GetNextTraitCost(ManagerTraitArea area)
	{
		return GetUserManagerProfileCurrent().UsersObtainedTraits.FirstOrDefault((UsersManagerTraits i) => i.Area == area).Levels[GetNextTraitLevel(area)].LevelCost;
	}

	public static void SetDataPack(int packId, string packName, string packURL)
	{
		UserManagerProfileContainer userManagerProfileCurrent = GetUserManagerProfileCurrent();
		userManagerProfileCurrent.DataPackHolder.DataPackId = packId;
		userManagerProfileCurrent.DataPackHolder.DataPackName = packName;
		userManagerProfileCurrent.DataPackHolder.DataPackUrl = packURL;
		userManagerProfileCurrent.DataPackHolder.ForceDataPackChange = ForceDataPackSelectionStatus.No;
	}

	public static string GetDataPackName()
	{
		return GetUserManagerProfileCurrent().DataPackHolder.DataPackName;
	}

	public static long GetDataPackID()
	{
		return GetUserManagerProfileCurrent().DataPackHolder.DataPackId;
	}

	public static string GetDataPackUrl()
	{
		return GetUserManagerProfileCurrent().DataPackHolder.DataPackUrl;
	}

	public static ForceDataPackSelectionStatus GetDataPackStatus()
	{
		return GetUserManagerProfileCurrent().DataPackHolder.ForceDataPackChange;
	}

	public static void AddManagerPonts(int pointsAmount)
	{
		GetUserManagerProfileCurrent().ManagerPoints += pointsAmount;
		CustomerDataController.Instance.SaveUserManagerProfile(CustomerDataController.Instance.SaveUserDataCallback);
	}

	public static void AddManagerXPAfterMatch(ManagerPointAwardType pointType)
	{
		Helpers.LogConsole("###TheoBugTest() AddManagerXPAfterMatch enterd");
		UserManagerProfileContainer userManagerProfileCurrent = GetUserManagerProfileCurrent();
		switch (pointType)
		{
		case ManagerPointAwardType.WonMatch:
			Helpers.LogConsole("###TheoBugTest() WonMatch, adding xp -- before xp == " + userManagerProfileCurrent.ManagerXP);
			userManagerProfileCurrent.ManagerXP += 25;
			Helpers.LogConsole("###TheoBugTest() WonMatch, added xp -- after xp == " + userManagerProfileCurrent.ManagerXP);
			break;
		case ManagerPointAwardType.WonComp:
			Helpers.LogConsole("###TheoBugTest() WonMatch, adding xp -- before xp == " + userManagerProfileCurrent.ManagerXP);
			userManagerProfileCurrent.ManagerXP += 100;
			Helpers.LogConsole("###TheoBugTest() WonMatch, added xp -- after xp == " + userManagerProfileCurrent.ManagerXP);
			break;
		}
	}

	public static int GetNextLevelXP()
	{
		if (CustomerDataController.Instance.CustomerDataHolder.Userdata.ManagerLevel < 50)
		{
			return StaticManagerLevels[CustomerDataController.Instance.CustomerDataHolder.Userdata.ManagerLevel + 1].XPRequired;
		}
		return StaticManagerLevels[CustomerDataController.Instance.CustomerDataHolder.Userdata.ManagerLevel].XPRequired;
	}

	public static bool CanLevelUp()
	{
		Helpers.LogConsole("###TheoBugTest() -- CanLevelUp() -- Entered");
		int managerXP = GetUserManagerProfileCurrent().ManagerXP;
		Helpers.LogConsole("###TheoBugTest() -- CanLevelUp() -- curxp == " + managerXP);
		if (CustomerDataController.Instance.CustomerDataHolder.Userdata.ManagerLevel < 50)
		{
			Helpers.LogConsole("###TheoBugTest() -- CanLevelUp() -- User is not max level, allowing check.");
			foreach (KeyValuePair<int, UserManagerLevel> staticManagerLevel in StaticManagerLevels)
			{
				if (CustomerDataController.Instance.CustomerDataHolder.Userdata.ManagerLevel >= staticManagerLevel.Value.LevelID)
				{
					continue;
				}
				Helpers.LogConsole("###TheoBugTest() -- CanLevelUp() -- Checking if user is able to level for cur level of -- " + staticManagerLevel.Value.LevelID);
				if (CustomerDataController.Instance.CustomerDataHolder.Userdata.ManagerLevel < 50)
				{
					Helpers.LogConsole("###TheoBugTest() -- CanLevelUp() -- Checking if user is able to level for cur level of -- " + staticManagerLevel.Value.LevelID + " -- Criteria(xp) == " + staticManagerLevel.Value.XPRequired + " -- CURXP == " + managerXP);
					if (managerXP >= staticManagerLevel.Value.XPRequired)
					{
						Helpers.LogConsole("###TheoBugTest() -- CanLevelUp() -- User is able to level up for level -- " + CustomerDataController.Instance.CustomerDataHolder.Userdata.ManagerLevel + " , returning true.");
						return true;
					}
					Helpers.LogConsole("###TheoBugTest() -- CanLevelUp() -- User is able to level up for level -- " + CustomerDataController.Instance.CustomerDataHolder.Userdata.ManagerLevel + " , returning false.");
					return false;
				}
			}
		}
		Helpers.LogConsole("###TheoBugTest() -- CanLevelUp() -- User is not able to level up, returning false.");
		return false;
	}

	public static void ManagerLevelUp()
	{
		Helpers.LogConsole("###TheoBugTest() -- ManagerLevelUp -- Entered");
		int managerXP;
		int num = (managerXP = GetUserManagerProfileCurrent().ManagerXP);
		Helpers.LogConsole("###TheoBugTest() -- ManagerLevelUp -- Orig XP == " + managerXP);
		Helpers.LogConsole("###TheoBugTest() -- ManagerLevelUp -- updatedXP == " + num);
		Helpers.LogConsole("###TheoBugTest() -- ManagerLevelUp -- LevelUp Check Started");
		if (CustomerDataController.Instance.CustomerDataHolder.Userdata.ManagerLevel >= 50)
		{
			return;
		}
		Helpers.LogConsole("###TheoBugTest() -- ManagerLevelUp -- User is not max level");
		foreach (KeyValuePair<int, UserManagerLevel> staticManagerLevel in StaticManagerLevels)
		{
			if (CustomerDataController.Instance.CustomerDataHolder.Userdata.ManagerLevel < staticManagerLevel.Value.LevelID && CustomerDataController.Instance.CustomerDataHolder.Userdata.ManagerLevel < 50)
			{
				Helpers.LogConsole("###TheoBugTest() -- ManagerLevelUp() -- Checking if user is able to level for cur level of -- " + staticManagerLevel.Value.LevelID + " -- Criteria(xp) == " + staticManagerLevel.Value.XPRequired + " -- CURXP == " + num);
				if (num >= staticManagerLevel.Value.XPRequired)
				{
					Helpers.LogConsole("###TheoBugTest() -- ManagerLevelUp() -- User has enough xp to level up, allowing level up. ");
					Helpers.LogConsole("###ManagerLevelUp()- User had enough xp to level up, deducting required xp for this level.");
					num -= staticManagerLevel.Value.XPRequired;
					Helpers.LogConsole("###TheoBugTest() -- ManagerLevelUp() -- Xp for this level requirement removed from existing xp. ");
					Helpers.LogConsole("###TheoBugTest() -- ManagerLevelUp -- After removal Orig XP == " + managerXP);
					Helpers.LogConsole("###TheoBugTest() -- ManagerLevelUp -- After removal updatedXP == " + num);
					Helpers.LogConsole("###ManagerLevelUp()- Pinging up to server request to level up.");
					Helpers.LogConsole("###TheoBugTest() -- ManagerLevelUp() -- Pinging up to server request to level up.");
					GiveManagerLevelUpReward(num, managerXP);
					TasklistManager.CreateTask(TaskType.ManagerLevelUp, removeOnClick: true, 15);
				}
				break;
			}
		}
	}

	public static void GiveManagerLevelUpReward(int updatedXP, int orignalXpamount)
	{
		Helpers.LogConsole("###TheoBugTest() -- GiveManagerLevelUpReward() -- Called");
		AjaxRequest ajaxRequest = new AjaxRequest();
		Dictionary<string, object> postData = new Dictionary<string, object> { { "xp", updatedXP } };
		Dictionary<string, object> callBackedVars = new Dictionary<string, object> { { "origXP", orignalXpamount } };
		Helpers.LogConsole("###TheoBugTest() -- GiveManagerLevelUpReward() -- posting");
		ajaxRequest.PostAsync(postData, GiveManagerLevelUpRewardCallBack, AjaxRequest.DataTitles.managerLevelUp, 30, callBackedVars);
	}

	private static void GiveManagerLevelUpRewardCallBack(UnityWebRequest response)
	{
		Helpers.LogConsole("###TheoBugTest() -- GiveManagerLevelUpRewardCallBack() -- Enterd");
		JObject jObject = (jObject = JObject.Parse(response.downloadHandler.text));
		Helpers.LogConsole("###TheoBugTest() -- GiveManagerLevelUpRewardCallBack() -- jsonData parsed");
		JObject jObject2 = JObject.Parse(jObject["callback"].ToString());
		Helpers.LogConsole("###TheoBugTest() -- GiveManagerLevelUpRewardCallBack() -- jsonDataOptional parsed");
		if (CustomerDataController.Instance.JsonDataGetSuccess(jObject))
		{
			Helpers.LogConsole("###TheoBugTest() -- GiveManagerLevelUpRewardCallBack() -- Sucess = true");
			CustomerDataController.Instance.UpdateCustomerData(jObject);
			Helpers.LogConsole("###TheoBugTest() -- GiveManagerLevelUpRewardCallBack() -- updated user profile with new data");
			Dictionary<string, string> parameters = new Dictionary<string, string>
			{
				{
					"managerLevel",
					"mangerLevel_" + CustomerDataController.Instance.CustomerDataHolder.Userdata.ManagerLevel
				},
				{
					"clubName",
					EventLoggingParam.GetClubNameParamValue()
				},
				{
					"leagueName",
					EventLoggingParam.GetLeagueNameParamValue()
				},
				{
					"startingLeague",
					EventLoggingParam.GetStartingLeagueParamValue()
				}
			};
			EventLoggingController.LogEvent("managerLevelUp", parameters);
			Helpers.LogConsole("###TheoBugTest() -- GiveManagerLevelUpRewardCallBack() -- firebase log event occured.");
			Helpers.LogConsole("###TheoBugTest() -- GiveManagerLevelUpRewardCallBack() -- Calling show level up modal");
			ModalController.Instance().ShowLevelUpModal(CustomerDataController.Instance.CustomerDataHolder.Userdata.ManagerLevel);
			Helpers.LogConsole("###TheoBugTest() -- GiveManagerLevelUpRewardCallBack() -- called level up modal");
		}
		else
		{
			Helpers.LogConsole("###TheoBugTest() -- GiveManagerLevelUpRewardCallBack() -- Sucess = false, restroing orignal XP.");
			CustomerDataController.Instance.CustomerDataHolder.Userdata.UserManagerProfile.ManagerXP = (int)jObject2["origXP"];
		}
	}

	public static void RemoveManagerPoints(int pointAmount, bool modal = false)
	{
		GetUserManagerProfileCurrent().ManagerPoints -= pointAmount;
		if (!modal)
		{
			UI.GetInstance().UpdateTopBar();
		}
		CustomerDataController.Instance.SaveUserManagerProfile(CustomerDataController.Instance.SaveUserDataCallback);
	}

	public static void ResetManagerPoints(bool modal = false)
	{
		UserManagerProfileScreen.Instance.resetSucces = true;
		ModalDisplayed = modal;
		UI.GetInstance().ShowGenericLoading();
		CustomerDataController.Instance.ConsumeItem(0, 100, ResetManagerPointsCallback);
	}

	private static void ResetManagerPointsCallback(UnityWebRequest response)
	{
		JObject jObject = (jObject = JObject.Parse(response.downloadHandler.text));
		if (CustomerDataController.Instance.JsonDataGetSuccess(jObject))
		{
			int creditAmount = (int)jObject["updatedvalues"]["Credits"];
			CustomerDataController.Instance.SetCreditAmount(creditAmount);
			CustomerDataController.Instance.CustomerDataHolder.Userdata.UserManagerProfile.UsersObtainedTraits.Clear();
			AddBaseManagerTraits();
			CustomerDataController.Instance.CustomerDataHolder.Userdata.UserManagerProfile.ManagerPoints = CustomerDataController.Instance.CustomerDataHolder.Userdata.CurObtainedManagerPoints;
			CustomerDataController.Instance.SaveUserManagerProfile(ResetSaveCallback);
			UI.GetInstance().ShowGenericLoading();
			if (ModalDisplayed)
			{
				GameObject.Find("UserProfileModal").GetComponent<UserProfileModal>().ResetTraits();
			}
			else
			{
				UI.GetInstance().UpdateTopBar();
				UserManagerProfileScreen.Instance.ResetScreen();
			}
		}
		else
		{
			Helpers.ShowToast(Translation.GetText("error-server"));
		}
		ModalDisplayed = false;
	}

	private static void ResetSaveCallback(UnityWebRequest response)
	{
		JObject data = (data = JObject.Parse(response.downloadHandler.text));
		bool flag = CustomerDataController.Instance.JsonDataGetSuccess(data);
		UI.GetInstance().HideGenericLoading();
		ServerRequestController.Instance.ResetHideLoadingPanel();
		if (!flag)
		{
			ServerRequestController.Instance.ShowRetryError("Failed to save reset", forceLoadingScreen: true, () =>
			{
				CustomerDataController.Instance.SaveUserManagerProfile(ResetSaveCallback);
			});
		}
	}

	public static bool HasSpecialOfferExpired()
	{
		UserManagerProfileContainer userManagerProfileCurrent = GetUserManagerProfileCurrent();
		if (Constants.UnixEpochTime.AddMilliseconds(userManagerProfileCurrent.SpecialOfferExpiryTime) <= CustomerDataController.Instance.ServerTime)
		{
			return true;
		}
		return false;
	}

	public static void SpecialofferSetLogic(bool CanSave = false)
	{
		UserManagerProfileContainer userManagerProfileCurrent = GetUserManagerProfileCurrent();
		DateTime dateTime = Constants.UnixEpochTime.AddMilliseconds(userManagerProfileCurrent.SpecialOfferExpiryTime);
		Constants.UnixEpochTime.AddMilliseconds(userManagerProfileCurrent.SpecialOfferEndTime);
		double num = CustomerDataController.Instance.CurUserSpentAmount();
		if (!(dateTime <= CustomerDataController.Instance.ServerTime))
		{
			return;
		}
		if (num == 0.0)
		{
			SpecialOfferSetNew("sm21_bundle_1_0", CanSave);
		}
		else if (num > 5.0)
		{
			if (num > 15.0)
			{
				SpecialOfferSetNew("sm21_bundle_4_0", CanSave);
			}
			else
			{
				SpecialOfferSetNew("sm21_bundle_3_0", CanSave);
			}
		}
		else
		{
			SpecialOfferSetNew("sm21_bundle_2_0", CanSave);
		}
	}

	public static void SpecialOfferSetNew(string nextBundleID, bool canSave = false)
	{
		UserManagerProfileContainer userManagerProfileCurrent = GetUserManagerProfileCurrent();
		DateTimeOffset dateTimeOffset = CustomerDataController.Instance.ServerTime;
		userManagerProfileCurrent.SpecialOfferEndTime = dateTimeOffset.AddDays(1.0).ToUnixTimeMilliseconds();
		userManagerProfileCurrent.SpecialOfferExpiryTime = dateTimeOffset.AddDays(3.0).ToUnixTimeMilliseconds();
		userManagerProfileCurrent.SpecialOfferType = nextBundleID;
		if (canSave)
		{
			CustomerDataController.Instance.SaveUserManagerProfile(SpecialOfferSetNewCallback);
		}
	}

	private static void SpecialOfferSetNewCallback(UnityWebRequest response)
	{
		if (string.IsNullOrEmpty(response.error))
		{
			JObject data = JObject.Parse(response.downloadHandler.text);
			if (!CustomerDataController.Instance.JsonDataGetSuccess(data))
			{
				Debug.LogError("###SpecialOfferSetSave### -- Error from GetCustomerDataCallback -- " + response.responseCode + " ----- " + response.error);
				ServerRequestController.Instance.ShowRetryError("Failed to retrieve customer data", forceLoadingScreen: true, () =>
				{
					CustomerDataController.Instance.SaveUserManagerProfile(SpecialOfferSetNewCallback);
				});
			}
			return;
		}
		Debug.LogError("###SpecialOfferSetSave### -- Error from GetCustomerDataCallback -- " + response.responseCode + " ----- " + response.error);
		ServerRequestController.Instance.ShowRetryError("Error from GetCustomerDataCallback -- " + response.responseCode + " ----- " + response.error, forceLoadingScreen: true, () =>
		{
			CustomerDataController.Instance.SaveUserManagerProfile(SpecialOfferSetNewCallback);
		});
	}

	public static void SetNewFreePackRewardVideoDate(GameObject modalLocalSave, RewardVideoType toLocalSave)
	{
		Helpers.LogConsole("###SetNewFreePackRewardVideoDate(). Started.");
		UserManagerProfileContainer userManagerProfileCurrent = GetUserManagerProfileCurrent();
		userManagerProfileCurrent.RewardVideoFreePackDate = ((DateTimeOffset)CustomerDataController.Instance.ServerTime).ToUnixTimeMilliseconds();
		Helpers.LogConsole("###SetNewFreePackRewardVideoDate(). rewardVideoClaimedDate has been set to time now. -- " + userManagerProfileCurrent.RewardVideoFreePackDate);
		localSaveModal = modalLocalSave;
		localSaveRVT = toLocalSave;
		Helpers.LogConsole("###SetNewFreePackRewardVideoDate(). local vars saved so they can be passed back into the callback.");
		Helpers.LogConsole("###SetNewFreePackRewardVideoDate(). calling save.");
		CustomerDataController.Instance.SaveUserManagerProfile(SetNewFreePackRewardVideoDateCallback);
	}

	private static void SetNewFreePackRewardVideoDateCallback(UnityWebRequest response)
	{
		Helpers.LogConsole("###SetNewFreePackRewardVideoDateCallback(). Enterd");
		if (string.IsNullOrEmpty(response.error))
		{
			JObject data = JObject.Parse(response.downloadHandler.text);
			if (CustomerDataController.Instance.JsonDataGetSuccess(data))
			{
				ModalController.Instance().OpenRewardedVideoPack(localSaveModal, localSaveRVT);
				return;
			}
			Debug.LogError("####SetNewFreePackRewardVideoDateCallback() -- Error from SetNewFreePackRewardVideoDateCallback -- " + response.responseCode + " ----- " + response.error);
			ServerRequestController.Instance.ShowRetryError("Error from SetNewFreePackRewardVideoDateCallback -- " + response.responseCode + " ----- " + response.error, forceLoadingScreen: true, () =>
			{
				ModalController.Instance().ShowRewardedVideoPackModal(localSaveRVT);
			});
		}
		else
		{
			Debug.LogError("###SetNewFreePackRewardVideoDateCallback() -- Error from SetNewFreePackRewardVideoDateCallback -- " + response.responseCode + " ----- " + response.error);
			ServerRequestController.Instance.ShowRetryError("Error from SetNewFreePackRewardVideoDateCallback -- " + response.responseCode + " ----- " + response.error, forceLoadingScreen: true, () =>
			{
				ModalController.Instance().ShowRewardedVideoPackModal(localSaveRVT);
			});
		}
	}

	public static bool CanClaimRewardVideoPack()
	{
		UserManagerProfileContainer userManagerProfileCurrent = GetUserManagerProfileCurrent();
		long num = 14400000L;
		long num2 = CustomerDataController.Instance.CustomerDataHolder.Userdata.FreePackDate - num;
		DateTime dateTime = CustomerDataController.Instance.minValue.AddMilliseconds(num2);
		if (userManagerProfileCurrent.RewardVideoFreePackDate <= 0)
		{
			Helpers.LogConsole("###CanClaimRewardVideoPack(). RewardVideoFreePackDate is less than 0, generating a usable value.");
			userManagerProfileCurrent.RewardVideoFreePackDate = ((DateTimeOffset)CustomerDataController.Instance.ServerTime).ToUnixTimeMilliseconds();
		}
		if (!(CustomerDataController.Instance.minValue.AddMilliseconds(userManagerProfileCurrent.RewardVideoFreePackDate) < dateTime))
		{
			return false;
		}
		return true;
	}
}
