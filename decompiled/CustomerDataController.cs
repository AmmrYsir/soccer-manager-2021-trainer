using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.Networking;

public class CustomerDataController : MonoBehaviour
{
	public static CustomerDataController Instance;

	public List<NewsFeedController.InfoData> NewsFeedData = new List<NewsFeedController.InfoData>();

	public CustomerData CustomerDataHolder;

	public DateTime minValue = Constants.UnixEpochTime;

	public bool FirstTimeUserProfile;

	public bool TimerIsMisMatched;

	public DateTime ServerTime;

	private Action<bool> dailyLoginModalHandler;

	private GameObject dailyLoginRewardOrigin;

	public void Awake()
	{
		if (Instance == null)
		{
			UnityEngine.Object.DontDestroyOnLoad(transform.gameObject);
			Instance = this;
		}
		else
		{
			UnityEngine.Object.Destroy(transform.gameObject);
		}
	}

	public void Update()
	{
		ServerTime = ServerTime.AddSeconds(Time.unscaledDeltaTime);
		if (TimerIsMisMatched)
		{
			TimerIsMisMatched = false;
		}
	}

	public void OnApplicationPause(bool pause)
	{
		if (InitGameSplashScreen.InitialiseFlag.InitialisingDone() && !pause)
		{
			TimerIsMisMatched = true;
			StartCoroutine(WaitForCorrectTime());
		}
	}

	public void LogConsole(string log)
	{
	}

	private IEnumerator WaitForCorrectTime()
	{
		yield return new WaitUntil(() => !TimerIsMisMatched);
	}

	public async void GetCustomerData()
	{
		AjaxRequest ajaxRequest = new AjaxRequest();
		Dictionary<string, object> postData = new Dictionary<string, object>();
		await ajaxRequest.PostAsync(postData, GetCustomerDataCallback, AjaxRequest.DataTitles.serverRetrieveData);
	}

	private void GetCustomerDataCallback(UnityWebRequest response)
	{
		if (string.IsNullOrEmpty(response.error))
		{
			JObject jObject = JObject.Parse(response.downloadHandler.text);
			if (JsonDataGetSuccess(jObject))
			{
				LogConsole("###FillCustomerData### Parsing Started");
				FillCustomerData(jObject);
				LogConsole("###FillCustomerData### Parsing Finished");
				return;
			}
			Debug.LogError("###CustomerData### -- Error from GetCustomerDataCallback -- " + response.responseCode + " ----- " + response.error);
			ServerRequestController.Instance.ShowRetryError("Failed to retrieve customer data", forceLoadingScreen: true, () =>
			{
				AuthenticationController.Instance.ValidateUser();
			});
		}
		else
		{
			Debug.LogError("###CustomerData### -- Error from GetCustomerDataCallback -- " + response.responseCode + " ----- " + response.error);
			ServerRequestController.Instance.ShowRetryError("Error from GetCustomerDataCallback -- " + response.responseCode + " ----- " + response.error, forceLoadingScreen: true, () =>
			{
				AuthenticationController.Instance.ValidateUser();
			});
		}
	}

	public void FillCustomerData(JObject parsedResult)
	{
		LogConsole("###CustomerDataFill### Parsing the new passed in data.");
		CustomerData customerData = JsonConvert.DeserializeObject<CustomerData>(parsedResult.ToString());
		LogConsole("###CustomerDataFill### Updating the CustomerDataHolder with the new data.");
		CustomerDataHolder = customerData;
		if (customerData.Userdata.UserManagerProfile == null)
		{
			LogConsole("###CustomerDataFill### UMP is null, freshening it up.");
			FreshUserProfileSetUp();
		}
		LogConsole("###CustomerDataFill### Running firstGameLaunchSetup");
		FirstGameLaunchSetUp(skipAds: false);
		if (!IsValidApplicationVersion())
		{
			LogConsole("###CustomerDataFill### Progress stopped due to game is out of date, showing modal");
			ServerRequestController.Instance.showUpdateModal();
			return;
		}
		if (!InitGameSplashScreen.InitialiseFlag.AuthDone)
		{
			InitGameSplashScreen.InitialiseFlag.AuthDone = true;
		}
		LogConsole("###CustomerDataFill### Complete");
	}

	public void UpdateCustomerData(JObject parsedResult)
	{
		LogConsole("###UpdateCustomerData(). Parsing the new passed in data.");
		CustomerData customerData = JsonConvert.DeserializeObject<CustomerData>(parsedResult.ToString());
		LogConsole("###UpdateCustomerData(). Updating the CustomerDataHolder with the new data.");
		CustomerDataHolder = customerData;
		if (customerData.Userdata.UserManagerProfile == null)
		{
			LogConsole("###UpdateCustomerData(). UMP is null, freshening it up.");
			FreshUserProfileSetUp();
		}
		LogConsole("###UpdateCustomerData(). Running firstGameLaunchSetup");
		FirstGameLaunchSetUp(skipAds: true);
		LogConsole("###UpdateCustomerData(). Complete");
	}

	public void FirstGameLaunchSetUp(bool skipAds)
	{
		if (CustomerDataHolder.Userdata.FreePackDate <= 0)
		{
			LogConsole("###CustomerDataFill### FreePackDate is null or less then 0, setting up a new freepack date so it's viable.");
			DateTimeOffset dateTimeOffset = ServerTime;
			CustomerDataHolder.Userdata.FreePackDate = dateTimeOffset.ToUnixTimeMilliseconds();
		}
		if (CustomerDataHolder.Userdata.UserManagerProfile.RewardVideoFreePackDate <= 0)
		{
			LogConsole("###CustomerDataFill### RewardVideoFreePackDate is null or less then 0, setting up a new RewardVideoFreePackDate date so it's viable.");
			DateTimeOffset dateTimeOffset2 = ServerTime;
			CustomerDataHolder.Userdata.UserManagerProfile.RewardVideoFreePackDate = dateTimeOffset2.ToUnixTimeMilliseconds();
		}
		if (CustomerDataHolder.Userdata.UserManagerProfile.ManagerUserName != null && CustomerDataHolder.Userdata.UserManagerProfile.ManagerUserName.Length > 20)
		{
			CustomerDataHolder.Userdata.UserManagerProfile.ManagerUserName = CustomerDataHolder.Userdata.UserManagerProfile.ManagerUserName.Substring(0, 20);
		}
		LogConsole("###CustomerDataFill### Settings PlayerPref for Accepted Ads");
		PlayerPrefs.SetString("hasacceptedtargetedadsnew", CustomerDataHolder.Userdata.UserManagerProfile.HasAcceptedTargetedAds ? 1.ToString() : 0.ToString());
		LogConsole("###CustomerDataFill### Settings PlayerPref for Save Slots");
		PlayerPrefs.SetString("prefserversaveslots", CustomerDataHolder.Userdata.SaveSlots.ToString());
		LogConsole("###CustomerDataFill### Currency Key Check");
		if (!PlayerPrefs.HasKey("prefcurrency"))
		{
			LogConsole("###CustomerDataFill### Currency key doesn't have a value, setting one");
			Helpers.SetCurrency(CustomerDataHolder.Userdata.UserManagerProfile.CustomerDeviceData.CountryCode);
		}
		if (!skipAds)
		{
			LogConsole("###CustomerDataFill### Settings Analytics ID");
			Analytics.SetUserId(GetCurUserID());
		}
	}

	public void FreshUserProfileSetUp()
	{
		LogConsole("###CustomerDataFill### Fresh User Required Setup.");
		FirstTimeUserProfile = true;
		UserManagerProfileContainer userManagerProfileContainer = new UserManagerProfileContainer();
		DataPackContainer dataPackHolder = new DataPackContainer();
		LogConsole("###CustomerDataFill### Generated a new datapack container for fresh user.");
		userManagerProfileContainer.DataPackHolder = dataPackHolder;
		LogConsole("###CustomerDataFill### Set users manager xp to 0 to begin with.");
		userManagerProfileContainer.ManagerXP = 0;
		LogConsole("###CustomerDataFill### Set users manager select to unselected.");
		userManagerProfileContainer.BaseManagerType = ManagerType.Unselected;
		LogConsole("###CustomerDataFill### Datapack forced to be selected for first time launch");
		userManagerProfileContainer.DataPackHolder.ForceDataPackChange = ForceDataPackSelectionStatus.Yes;
		LogConsole("###CustomerDataFill###  Setting the new UMP profile with the new data.");
		CustomerDataHolder.Userdata.UserManagerProfile = userManagerProfileContainer;
		LogConsole("###CustomerDataFill### Updating the device information.");
		UpdateDeviceInformation();
		LogConsole("###CustomerDataFill### Setting the first special offer, i.e starter bundle.");
		UserManagerProfileController.SpecialOfferSetNew("sm21_bundle_0");
	}

	public void UpdateDeviceInformation()
	{
		Helpers.LogConsole("###UpdateDeviceInformation(). -- Updating Device information before save.");
		CustomerDeviceData customerDeviceData = new CustomerDeviceData();
		customerDeviceData.AppVersion = Application.version;
		Helpers.LogConsole("###UpdateDeviceInformation(). -- Setting AppVersion to -- " + customerDeviceData.AppVersion);
		customerDeviceData.CountryCode = GetCountryCode();
		Helpers.LogConsole("###UpdateDeviceInformation(). -- Setting CountryCode to -- " + customerDeviceData.CountryCode);
		customerDeviceData.DeviceName = SystemInfo.deviceModel;
		Helpers.LogConsole("###UpdateDeviceInformation(). -- Setting DeviceName to -- " + customerDeviceData.DeviceName);
		customerDeviceData.Language = Application.systemLanguage.ToString();
		Helpers.LogConsole("###UpdateDeviceInformation(). -- Setting Language to -- " + customerDeviceData.Language);
		customerDeviceData.Platform = Application.platform.ToString();
		Helpers.LogConsole("###UpdateDeviceInformation(). -- Setting Platform to -- " + customerDeviceData.Platform);
		customerDeviceData.DeviceOS = SystemInfo.operatingSystem;
		Helpers.LogConsole("###UpdateDeviceInformation(). -- Setting DeviceOS to -- " + customerDeviceData.DeviceOS);
		CustomerDataHolder.Userdata.UserManagerProfile.CustomerDeviceData = customerDeviceData;
	}

	public CustomerDeviceData GenerateNewDeviceData()
	{
		Helpers.LogConsole("###UpdateDeviceInformation(). -- Updating Device information before save.");
		CustomerDeviceData customerDeviceData = new CustomerDeviceData();
		customerDeviceData.AppVersion = Application.version;
		Helpers.LogConsole("###UpdateDeviceInformation(). -- Setting AppVersion to -- " + customerDeviceData.AppVersion);
		customerDeviceData.CountryCode = GetCountryCode();
		Helpers.LogConsole("###UpdateDeviceInformation(). -- Setting CountryCode to -- " + customerDeviceData.CountryCode);
		customerDeviceData.DeviceName = SystemInfo.deviceModel;
		Helpers.LogConsole("###UpdateDeviceInformation(). -- Setting DeviceName to -- " + customerDeviceData.DeviceName);
		customerDeviceData.Language = Application.systemLanguage.ToString();
		Helpers.LogConsole("###UpdateDeviceInformation(). -- Setting Language to -- " + customerDeviceData.Language);
		customerDeviceData.Platform = Application.platform.ToString();
		Helpers.LogConsole("###UpdateDeviceInformation(). -- Setting Platform to -- " + customerDeviceData.Platform);
		customerDeviceData.DeviceOS = SystemInfo.operatingSystem;
		Helpers.LogConsole("###UpdateDeviceInformation(). -- Setting DeviceOS to -- " + customerDeviceData.DeviceOS);
		if (CustomerDataHolder.Userdata != null)
		{
			CustomerDataHolder.Userdata.UserManagerProfile.CustomerDeviceData = customerDeviceData;
		}
		return customerDeviceData;
	}

	private string GetCountryCode()
	{
		string result = "";
		if (Application.platform != RuntimePlatform.Android)
		{
			Helpers.LogConsole("###GetCountryCode(). -- Getting Country code for Ios/unity/else from RegionInfo.CurrentRegion");
			result = RegionInfo.CurrentRegion.ToString();
		}
		return result;
	}

	public async void SaveUserManagerprofileLoadingScreen(AjaxRequestUnityWebRequestCallback callback)
	{
		string advertisingID = await GetAdvertisingID();
		CustomerDataHolder.Userdata.UserManagerProfile.AdvertisingID = advertisingID;
		string value = JsonConvert.SerializeObject(CustomerDataHolder.Userdata.UserManagerProfile, Formatting.Indented);
		AjaxRequest ajaxRequest = new AjaxRequest();
		Dictionary<string, object> postData = new Dictionary<string, object> { { "userprofile", value } };
		if (UI.GetInstance() != null)
		{
			UI.GetInstance().ShowGenericLoading();
		}
		await ajaxRequest.PostAsync(postData, callback, AjaxRequest.DataTitles.serverSaveData);
	}

	public async void SaveUserManagerProfile(AjaxRequestUnityWebRequestCallback callback, Dictionary<string, object> callBackedVars = null)
	{
		Helpers.LogConsole("###SaveUserManagerProfile() starting save of UMP");
		string advertisingID = await GetAdvertisingID();
		CustomerDataHolder.Userdata.UserManagerProfile.AdvertisingID = advertisingID;
		UpdateDeviceInformation();
		string value = JsonConvert.SerializeObject(CustomerDataHolder.Userdata.UserManagerProfile, Formatting.Indented);
		AjaxRequest ajaxRequest = new AjaxRequest();
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		dictionary.Add("userprofile", value);
		if (callBackedVars != null)
		{
			Helpers.LogConsole("###SaveUserManagerProfile(). Pinging server to save UMP, WITH callbacked vars");
			await ajaxRequest.PostAsync(dictionary, callback, AjaxRequest.DataTitles.serverSaveData, 30, callBackedVars);
		}
		else
		{
			Helpers.LogConsole("###SaveUserManagerProfile(). Pinging server to save UMP.");
			await ajaxRequest.PostAsync(dictionary, callback, AjaxRequest.DataTitles.serverSaveData);
		}
	}

	private async Task<string> GetAdvertisingID()
	{
		return "";
	}

	public void UpdateMetaData(JObject parsedMetaDeta)
	{
		MetaDataContainer metadata = JsonConvert.DeserializeObject<MetaDataContainer>(parsedMetaDeta.ToString());
		if (CustomerDataHolder != null)
		{
			CustomerDataHolder.Metadata = metadata;
			ServerTime = Constants.UnixEpochTime.AddMilliseconds(CustomerDataHolder.Metadata.ServerTime);
			Shop.UpdateShopPricesSteam(parsedMetaDeta);
		}
	}

	public bool HaveEnoughCredits(int creditsToTake)
	{
		if (GetUserCurCreditAmount() - creditsToTake >= 0)
		{
			return true;
		}
		return false;
	}

	public bool HasPurchasedItem(string itemName)
	{
		if (CustomerDataHolder.Userdata.PurchasedShopItems.FirstOrDefault((PurchasedShopItem i) => i.PurchasedItemID == itemName) != null)
		{
			return true;
		}
		return false;
	}

	public bool IsValidApplicationVersion()
	{
		return true;
	}

	public void SaveUserDataCallback(UnityWebRequest response)
	{
		if (string.IsNullOrEmpty(response.error))
		{
			JObject jObject = JObject.Parse(response.downloadHandler.text);
			if (JsonDataGetSuccess(jObject))
			{
				if (!InitGameSplashScreen.InitialiseFlag.AuthDone)
				{
					InitGameSplashScreen.InitialiseFlag.AuthDone = true;
				}
				if (jObject["callback"] != null)
				{
					JObject jObject2 = JObject.Parse(jObject["callback"].ToString());
					if (jObject2.ContainsKey("CanLevelUp") && (bool)jObject2["CanLevelUp"])
					{
						UserManagerProfileController.ManagerLevelUp();
					}
				}
				return;
			}
			Debug.LogError("###CustomerData### -- Error from SaveUserDataCallback -- " + response.responseCode + " ----- " + response.error);
			ServerRequestController.Instance.ShowRetryError("Error from SaveUserDataCallback -- " + response.responseCode + " ----- " + response.error, forceLoadingScreen: true, () =>
			{
				SaveUserManagerProfile(SaveUserDataCallback);
			});
		}
		else
		{
			Debug.LogError("###CustomerData### -- Error from SaveUserDataCallback -- " + response.responseCode + " ----- " + response.error);
			ServerRequestController.Instance.ShowRetryError("Error from SaveUserDataCallback -- " + response.responseCode + " ----- " + response.error, forceLoadingScreen: true, () =>
			{
				SaveUserManagerProfile(SaveUserDataCallback);
			});
		}
	}

	public void GetNewsFeedData()
	{
		string text = "en_EN";
		text = ((!(Game.GetInstance() != null)) ? Translation.GetSavedLanguage() : Game.instance.CustomerLang);
		new AjaxRequest().PostAsync(new Dictionary<string, object> { { "lang", text } }, GetNewsFeedDataCallBack, AjaxRequest.DataTitles.newsFeedRetrieve);
	}

	public void GetNewsFeedDataCallBack(UnityWebRequest response)
	{
		JObject jObject = JObject.Parse(response.downloadHandler.text);
		if (JsonDataGetSuccess(jObject))
		{
			JArray jArray = JArray.Parse(jObject["newsdata"].ToString());
			NewsFeedData.Clear();
			foreach (JToken item in jArray)
			{
				NewsFeedController.InfoData infoData = new NewsFeedController.InfoData();
				bool ErrorOccured = false;
				infoData = JsonConvert.DeserializeObject<NewsFeedController.InfoData>(item.ToString(), new JsonSerializerSettings
				{
					Error = (object sender, ErrorEventArgs args) =>
					{
						ErrorOccured = true;
						args.ErrorContext.Handled = true;
					}
				});
				if (!ErrorOccured && IsNewsJsonNotRubbish(infoData))
				{
					NewsFeedData.Add(infoData);
				}
			}
			NewsFeedData.Sort((NewsFeedController.InfoData p1, NewsFeedController.InfoData p2) => p2.Date.CompareTo(p1.Date));
			if (UI.GetInstance() != null)
			{
				UI.GetInstance().UpdateNewsFeedBlob();
			}
		}
		else
		{
			ServerRequestController.Instance.ShowRetryError("Failed to retrieve first time news feed data", forceLoadingScreen: true, () =>
			{
				GetNewsFeedData();
			});
		}
	}

	public bool IsNewsJsonNotRubbish(NewsFeedController.InfoData newsFeedDataObj)
	{
		bool flag = false;
		if (newsFeedDataObj.BoxType > 0)
		{
			flag = true;
			if (!string.IsNullOrWhiteSpace(newsFeedDataObj.ImageURL))
			{
				flag = true;
				if (!string.IsNullOrWhiteSpace(newsFeedDataObj.Title))
				{
					flag = true;
					if (!string.IsNullOrWhiteSpace(newsFeedDataObj.LinkURL))
					{
						flag = true;
						if (!string.IsNullOrWhiteSpace(newsFeedDataObj.Category))
						{
							flag = true;
							if (newsFeedDataObj.Date > 0)
							{
								return true;
							}
							return false;
						}
						return false;
					}
					return false;
				}
				return false;
			}
			return false;
		}
		return false;
	}

	public async void ConsumeItem(int boostAmount, int creditAmount, AjaxRequestUnityWebRequestCallback callback, Dictionary<string, object> callBackedVars = null)
	{
		AjaxRequest ajaxRequest = new AjaxRequest();
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		if (creditAmount > 0)
		{
			if (CustomerDataHolder.Userdata.Credits - creditAmount < 0)
			{
				ShowNotEnoughModal(isCredits: true);
				return;
			}
			dictionary.Add("Credits", creditAmount);
		}
		if (boostAmount > 0)
		{
			if (CustomerDataHolder.Userdata.Boosts - boostAmount < 0)
			{
				ShowNotEnoughModal(isCredits: false);
				return;
			}
			dictionary.Add("Boosts", boostAmount);
		}
		string value = JsonConvert.SerializeObject(dictionary, Formatting.Indented);
		Dictionary<string, object> dictionary2 = new Dictionary<string, object>();
		dictionary2.Add("items", value);
		if (callBackedVars != null)
		{
			string value2 = JsonConvert.SerializeObject(callBackedVars, Formatting.Indented);
			dictionary2.Add("callback", value2);
		}
		await ajaxRequest.PostAsync(dictionary2, callback, AjaxRequest.DataTitles.consumeItem);
	}

	public void RequstPreorderBonusSet()
	{
		if (UI.GetInstance() != null)
		{
			UI.GetInstance().ShowGenericLoading();
		}
		AjaxRequest ajaxRequest = new AjaxRequest();
		Dictionary<string, object> postData = new Dictionary<string, object>();
		Helpers.LogConsole("RequstPreorderBonusSet(). sending up request ");
		ajaxRequest.PostAsync(postData, RequstPreorderBonusSetCallBack, AjaxRequest.DataTitles.claimPreOrderBonus);
	}

	private void RequstPreorderBonusSetCallBack(UnityWebRequest response)
	{
		JObject jObject = (jObject = JObject.Parse(response.downloadHandler.text));
		bool flag = JsonDataGetSuccess(jObject);
		Helpers.LogConsole("RequstPreorderBonusSetCallBack(). in callback.");
		if (UI.GetInstance() != null)
		{
			UI.GetInstance().HideGenericLoading();
		}
		if (flag)
		{
			Helpers.LogConsole("RequstPreorderBonusSetCallBack(). Success, filling profile with the new data.");
			UpdateCustomerData(jObject);
			Helpers.LogConsole("RequstPreorderBonusSetCallBack(). profile fulfilled.");
		}
		else
		{
			Helpers.ShowToast(Translation.GetText("error-server"));
		}
	}

	private void ShowNotEnoughModal(bool isCredits)
	{
		if (UI.GetInstance() != null)
		{
			UI.GetInstance().HideGenericLoading();
		}
		if (!(ModalController.Instance() != null))
		{
			return;
		}
		if (isCredits)
		{
			ModalController.Instance().ShowConfirmModal(Translation.GetText("not-enough-credits"), Translation.GetText("not-enough-credits"), () =>
			{
				UI.GetInstance().ChangeScreen(8, ShopScreen.ScreenTypes.Credits);
				ModalController.Instance().CloseAllModals();
			}, Translation.GetText("go-to-shop"));
		}
		else
		{
			ModalController.Instance().ShowConfirmModal(Translation.GetText("not-enough-boosts"), Translation.GetText("not-enough-boosts"), () =>
			{
				UI.GetInstance().ChangeScreen(8, ShopScreen.ScreenTypes.Boosts);
				ModalController.Instance().CloseAllModals();
			}, Translation.GetText("go-to-shop"));
		}
	}

	public int GetSaveSlots()
	{
		return CustomerDataHolder.Userdata.SaveSlots;
	}

	public void SetSaveSlots(int value)
	{
		CustomerDataHolder.Userdata.SaveSlots = value;
	}

	public bool HasVipLite()
	{
		return HasPurchasedItem("sm21_vip_light");
	}

	public bool HasVip()
	{
		if (!CustomerDataHolder.Userdata.VIP)
		{
			return HasVipLite();
		}
		return true;
	}

	public bool HasGoldVip()
	{
		return CustomerDataHolder.Userdata.VIP;
	}

	public void SetVip(bool value)
	{
		CustomerDataHolder.Userdata.VIP = value;
	}

	public int GetUserCurCreditAmount()
	{
		int num = CustomerDataHolder.Userdata.Credits;
		if (num < 0)
		{
			num = 0;
		}
		return num;
	}

	public void SetCreditAmount(int creditAmount)
	{
		CustomerDataHolder.Userdata.Credits = creditAmount;
	}

	public void AddCredits(int creditAmount, GameObject creditLocation = null)
	{
		CustomerDataHolder.Userdata.Credits += creditAmount;
		if (creditLocation != null)
		{
			UI.GetInstance().DrawCreditsFlying(creditLocation);
		}
		if (PlayerPrefs.HasKey("pendingpurchase"))
		{
			PlayerPrefs.DeleteKey("pendingpurchase");
			PlayerPrefs.Save();
		}
	}

	public int GetCurUserBoostAmount()
	{
		int num = CustomerDataHolder.Userdata.Boosts;
		if (num < 0)
		{
			num = 0;
		}
		return num;
	}

	public void SetCurUserBoostAmount(int value)
	{
		CustomerDataHolder.Userdata.Boosts = value;
	}

	public void AddBoosts(int value, bool isShop, GameObject boostLocation = null)
	{
		CustomerDataHolder.Userdata.Boosts += value;
		if (boostLocation != null)
		{
			UI.GetInstance().DrawFitnessBoostFlying(boostLocation, isShop);
		}
	}

	public DateTime GetFreePackEndTime()
	{
		if (CustomerDataHolder.Userdata.FreePackDate <= 0)
		{
			DateTimeOffset dateTimeOffset = ServerTime;
			CustomerDataHolder.Userdata.FreePackDate = dateTimeOffset.ToUnixTimeMilliseconds();
		}
		return minValue.AddMilliseconds(CustomerDataHolder.Userdata.FreePackDate);
	}

	public double DaysPassedSincelastPurchase()
	{
		double num = 3560.0;
		foreach (PurchasedShopItem purchasedShopItem in CustomerDataHolder.Userdata.PurchasedShopItems)
		{
			double num2 = DaysPassedSincePurchasedItem(purchasedShopItem.PurchaseDate);
			if (num2 < num)
			{
				num = num2;
			}
		}
		return num;
	}

	private double DaysPassedSincePurchasedItem(long purchaseDateData)
	{
		DateTime dateTime = minValue.AddMilliseconds(purchaseDateData);
		double totalDays = (ServerTime - dateTime).TotalDays;
		if (dateTime != minValue && totalDays >= 0.0)
		{
			return totalDays;
		}
		return 0.0;
	}

	public bool HasBoughtCurrentSpecialOffer()
	{
		string specialOfferType = CustomerDataHolder.Userdata.UserManagerProfile.SpecialOfferType;
		DateTime dateTime = minValue.AddMilliseconds(CustomerDataHolder.Userdata.UserManagerProfile.SpecialOfferEndTime);
		DateTime dateTime2 = dateTime.AddDays(-1.0);
		foreach (PurchasedShopItem purchasedShopItem in CustomerDataHolder.Userdata.PurchasedShopItems)
		{
			if (purchasedShopItem.PurchasedItemID == specialOfferType)
			{
				DateTime dateTime3 = minValue.AddMilliseconds(purchasedShopItem.PurchaseDate);
				if (dateTime3 >= dateTime2 && dateTime3 <= dateTime)
				{
					return true;
				}
			}
		}
		return false;
	}

	public double GetMoneySpentInPastTime(int days)
	{
		double num = 0.0;
		DateTime dateTime = ServerTime.AddDays(-days);
		foreach (PurchasedShopItem purchasedShopItem in CustomerDataHolder.Userdata.PurchasedShopItems)
		{
			DateTime dateTime2 = minValue.AddMilliseconds(purchasedShopItem.PurchaseDate);
			if (dateTime2 >= dateTime && dateTime2 <= ServerTime)
			{
				num += purchasedShopItem.RealWorldValue;
			}
		}
		Helpers.LogConsole("###GetMoneySpentInPastTime(). -- In past -- " + days + " -- the user has spent the following of £" + num);
		return num;
	}

	public double CurUserSpentAmount()
	{
		if (CustomerDataHolder.Userdata.PurchaseTotal < 0.0)
		{
			CustomerDataHolder.Userdata.PurchaseTotal = 0.0;
		}
		return CustomerDataHolder.Userdata.PurchaseTotal;
	}

	public DateTime GetSpecialOfferEndTime()
	{
		if (CustomerDataHolder.Userdata.UserManagerProfile.SpecialOfferEndTime <= 0)
		{
			DateTimeOffset dateTimeOffset = ServerTime.AddDays(1.0);
			CustomerDataHolder.Userdata.UserManagerProfile.SpecialOfferEndTime = dateTimeOffset.ToUnixTimeMilliseconds();
		}
		return minValue.AddMilliseconds(CustomerDataHolder.Userdata.UserManagerProfile.SpecialOfferEndTime);
	}

	public string GetSpecialOfferType()
	{
		return CustomerDataHolder.Userdata.UserManagerProfile.SpecialOfferType;
	}

	public void UpdateFreePackTime(long Val)
	{
		CustomerDataHolder.Userdata.FreePackDate = Val;
	}

	public void AddSaveSlots(int val)
	{
		CustomerDataHolder.Userdata.SaveSlots += val;
		PlayerPrefs.SetString("prefserversaveslots", CustomerDataHolder.Userdata.SaveSlots.ToString());
	}

	public string GetCurUserID()
	{
		_ = string.Empty;
		return SteamManager.Instance.GetSteamID();
	}

	public void ClaimDailyLoginRewards(Action<bool> dailyLoginModalHandler = null, GameObject rewardOrigin = null)
	{
		this.dailyLoginModalHandler = dailyLoginModalHandler;
		dailyLoginRewardOrigin = rewardOrigin;
		AjaxRequest ajaxRequest = new AjaxRequest();
		Dictionary<string, object> postData = new Dictionary<string, object>();
		ajaxRequest.PostAsync(postData, ClaimDailyLoginRewardsCallBack, AjaxRequest.DataTitles.claimDailyLogin);
	}

	private void ClaimDailyLoginRewardsCallBack(UnityWebRequest response)
	{
		JObject jObject = (jObject = JObject.Parse(response.downloadHandler.text));
		bool flag = JsonDataGetSuccess(jObject);
		bool obj = false;
		if (flag)
		{
			if (jObject.ContainsKey("reward"))
			{
				obj = true;
				JObject jObject2 = JObject.Parse(jObject["reward"].ToString());
				if (jObject2.ContainsKey("Boosts"))
				{
					int value = (int)jObject["reward"]["Boosts"];
					AddBoosts(value, isShop: false, dailyLoginRewardOrigin);
				}
				if (jObject2.ContainsKey("Credits"))
				{
					int creditAmount = (int)jObject["reward"]["Credits"];
					AddCredits(creditAmount, dailyLoginRewardOrigin);
				}
			}
			if (jObject.ContainsKey("userdata"))
			{
				UpdateCustomerData(jObject);
			}
		}
		if (dailyLoginModalHandler != null)
		{
			dailyLoginModalHandler(obj);
			dailyLoginModalHandler = null;
		}
	}

	public bool CanShowPreOrderBonusClaim()
	{
		Helpers.LogConsole("CanShowPreOrderBonusClaim(). -- Starting check for if user is viable for pre-order reward");
		DateTime dateTime = minValue.AddMilliseconds(CustomerDataHolder.Metadata.SettingsContainer.ReleaseDate);
		Helpers.LogConsole("CanShowPreOrderBonusClaim(). -- game release date == " + dateTime);
		DateTime dateTime2 = dateTime.AddDays(3.0);
		Helpers.LogConsole("CanShowPreOrderBonusClaim(). -- pre-order bonus cutoff date == " + dateTime2);
		if (!CustomerDataHolder.Userdata.ReleaseRewardClaimed && ServerTime < dateTime2)
		{
			Helpers.LogConsole("CanShowPreOrderBonusClaim(). -- user is eligble for claiming pre-order bonus.");
			return true;
		}
		Helpers.LogConsole("CanShowPreOrderBonusClaim(). -- user is illegible for claiming pre-order bonus.");
		return false;
	}

	public void SimulationTestForDailyLoginRewards(int credits, int boosts, GameObject origin)
	{
		AddCredits(credits, origin);
		AddBoosts(boosts, origin);
	}

	public bool JsonDataGetSuccess(JObject data)
	{
		if (data.ContainsKey("success"))
		{
			return (bool)data["success"];
		}
		if (data.ContainsKey("code"))
		{
			if ((int)data["code"] == 200)
			{
				return true;
			}
			return false;
		}
		return false;
	}
}
