using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json.Linq;
using Steamworks;
using UnityEngine;
using UnityEngine.Networking;

[DisallowMultipleComponent]
public class SteamManager : MonoBehaviour
{
	private string transId;

	private uint appID = 1448530u;

	private static SteamManager instance;

	private static bool everInitialized;

	private bool initialized;

	public static SteamManager Instance
	{
		get
		{
			if (instance == null)
			{
				return new GameObject("SteamManager").AddComponent<SteamManager>();
			}
			return instance;
		}
	}

	public static bool Initialized => Instance.initialized;

	private static void SteamAPIDebugTextHook(int nSeverity, StringBuilder pchDebugText)
	{
	}

	private void Awake()
	{
		if (instance != null)
		{
			UnityEngine.Object.Destroy(gameObject);
			return;
		}
		instance = this;
		UnityEngine.Object.DontDestroyOnLoad(gameObject);
		if (everInitialized)
		{
			throw new Exception("Tried to Initialize the SteamAPI twice in one session!");
		}
		UnityEngine.Object.DontDestroyOnLoad(gameObject);
		try
		{
			if (SteamClient.RestartAppIfNecessary(appID))
			{
				Application.Quit();
				return;
			}
		}
		catch (DllNotFoundException ex)
		{
			Debug.LogError("[Steamworks.NET] Could not load [lib]steam_api.dll/so/dylib. It's likely not in the correct location. Refer to the README for more details.\n" + ex, this);
			Application.Quit();
			return;
		}
		SteamClient.Init(appID, asyncCallbacks: false);
		initialized = true;
		everInitialized = true;
		SetUpResponses();
		StartCoroutine(BeginLoad());
	}

	public IEnumerator BeginLoad()
	{
		yield return new WaitForSeconds(1f);
		if (!InitGameSplashScreen.InitialiseFlag.InitDone)
		{
			InitGameSplashScreen.InitialiseFlag.InitDone = true;
		}
		InitGameSplashScreen.Instance.InitGame();
	}

	public void SetUpResponses()
	{
		SteamUser.OnMicroTxnAuthorizationResponse += OnPurchaseFinished;
	}

	private void OnDestroy()
	{
		if (!(instance != this))
		{
			instance = null;
			if (initialized)
			{
				SteamClient.Shutdown();
			}
		}
	}

	private void OnApplicationCancelQuit()
	{
		if (!(instance != this))
		{
			instance = null;
			if (initialized)
			{
				SteamClient.Shutdown();
			}
		}
	}

	public void BeginAuthentication()
	{
		AuthenticationController.Instance.ValidateUser();
	}

	private void OnApplicationQuit()
	{
		if (!(instance != this))
		{
			instance = null;
			if (initialized)
			{
				SteamClient.Shutdown();
			}
		}
	}

	private void Update()
	{
		if (initialized)
		{
			SteamClient.RunCallbacks();
		}
	}

	public string GetSteamID()
	{
		string result = "";
		try
		{
			if (Initialized)
			{
				result = SteamClient.SteamId.ToString();
			}
		}
		catch (Exception)
		{
		}
		return result;
	}

	public void InitSteamTransaction(string productId)
	{
		try
		{
			GetSteamID();
			AjaxRequest ajaxRequest = new AjaxRequest();
			Dictionary<string, object> dictionary = new Dictionary<string, object> { { "productid", productId } };
			string lang = PlayerPrefs.GetString("prefsserverlang");
			string text = Constants.AVAILABLE_LANGUAGES.FirstOrDefault((KeyValuePair<string, string> x) => x.Value == lang).Key;
			if (string.IsNullOrEmpty(text))
			{
				text = "en";
			}
			text = text.Substring(0, 2);
			string value = Shop.GetCurSymbol().ToString();
			dictionary.Add("lang", text);
			dictionary.Add("currencycode", value);
			ajaxRequest.PostAsync(dictionary, InitSteamTransactionCallback, AjaxRequest.DataTitles.initialiseSteampayment, 30, null, shouldHideLoading: false);
		}
		catch (Exception ex)
		{
			Debug.LogError("Error in InitSteamTransaction() " + ex.Message);
		}
	}

	private void InitSteamTransactionCallback(UnityWebRequest response)
	{
	}

	private void OnPurchaseFinished(AppId appid, ulong orderid, bool success)
	{
		if (success)
		{
			try
			{
				GetSteamID();
				new AjaxRequest().PostAsync(new Dictionary<string, object> { { "orderId", orderid } }, OnPurchaseFinishedCallBack, AjaxRequest.DataTitles.finaliseSteamPayment);
				return;
			}
			catch (Exception ex)
			{
				Debug.LogError("Error in FinalizeSteamTransaction() " + ex.Message);
				return;
			}
		}
		UI.GetInstance().HideGenericLoading();
	}

	private void OnPurchaseFinishedCallBack(UnityWebRequest response)
	{
		JObject jObject = JObject.Parse(response.downloadHandler.text);
		if (CustomerDataController.Instance.JsonDataGetSuccess(jObject))
		{
			jObject["productid"].ToString();
			if (jObject.ContainsKey("userdata"))
			{
				Helpers.LogConsole("###ProcessPurchase().method, resultobj contains userdata, updating profile.");
				CustomerDataController.Instance.UpdateCustomerData(jObject);
				Helpers.LogConsole("###ProcessPurchase().method, updating profile complete");
			}
			Helpers.LogConsole("###ProcessPurchase().method, Checking for items added data.");
			if (jObject.ContainsKey("itemsadded"))
			{
				Helpers.LogConsole("###ProcessPurchase().method, Contains items added data, adding these items.");
				PurchasedProductData sendupData = new PurchasedProductData();
				Shop.GiveDynamicPurchasedProduct((JObject)jObject["itemsadded"], sendupData);
				Helpers.LogConsole("###ProcessPurchase().method, Items added complete");
			}
			Helpers.LogConsole("###ProcessPurchase().method, Finished checking Userdata / Itemsadded data");
			Helpers.LogConsole("###ProcessPurchase().method, -- Checking if ui.getinstance is not null");
			if (UI.GetInstance() != null)
			{
				Helpers.LogConsole("###ProcessPurchase().method, -- ui.instance is not null, updating shop");
				if (UI.GetInstance().shopScreen != null)
				{
					Helpers.LogConsole("###ProcessPurchase().method, -- shop screen generated, allowing refresh.");
					UI.GetInstance().shopScreen.InitShopScreen();
				}
				else
				{
					Helpers.LogConsole("###ProcessPurchase().method, -- shop screen not generated yet, don't need to init.");
				}
				UI.GetInstance().topBar.GetComponent<TopBar>().UpdatePointsBar();
				UI.GetInstance().overviewScreen.DrawOverviewScreen(0);
				ModalController.Instance().CloseAllModals();
				UI.GetInstance().HideGenericLoading();
				Helpers.ShowToast(Translation.GetText("purchased"));
			}
			Helpers.LogConsole("###ProcessPurchase().method, -- Finished");
		}
		else
		{
			UI.GetInstance().HideGenericLoading();
		}
	}

	public void UpdateSteamShopPricesDict(JObject JsonData)
	{
	}
}
