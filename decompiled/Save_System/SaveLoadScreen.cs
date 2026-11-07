using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SaveLoadScreen : MonoBehaviour
{
	private enum Tabs
	{
		Local,
		Cloud
	}

	[Header("VIP")]
	public Text VIPTitle;

	public Text VIPAdfree;

	public Text VIPExtraSlot;

	public Text VIPDoubleRewards;

	public Button VIPBtn;

	public Text VIPBtnPrice;

	[Header("VIP PLUS")]
	public Text VIPplusTitle;

	public Text VIPplusAdfree;

	public Text VIPplusExtraSlot;

	public Text VIPplusDoubleRewards;

	public Text VIPplusCredits;

	public Text VIPplusManagerPoint;

	public Button VIPplusBtn;

	public Text VIPplusBtnPrice;

	public static SaveLoadScreen instance;

	private static GameObject ModalPanel;

	private static GameObject screen;

	private static GameObject CloudSlotContainer;

	private static GameObject SaveSlotContainer;

	private static GameObject overwritePanel;

	private static GameObject NavBar;

	private static bool isNewGame;

	private static bool isLoadGame;

	private Tabs currentTab;

	private int CloudSaveCount;

	private void Awake()
	{
		if (instance == null)
		{
			instance = this;
		}
		else if (instance != this)
		{
			Object.Destroy(gameObject);
		}
	}

	public static void init(bool newGame, bool loadGame)
	{
		string n = "SaveLoadModal(Clone)/BottomPanel";
		ModalPanel = GameObject.Find("ModalPanel(Clone)");
		screen = ModalPanel.transform.Find(n).gameObject;
		CloudSlotContainer = screen.transform.Find("CloudSlotContainer").gameObject;
		SaveSlotContainer = screen.transform.Find("SaveSlotContainer").gameObject;
		overwritePanel = screen.transform.Find("OverwritePanel").gameObject;
		NavBar = screen.transform.Find("NavBar").gameObject;
		SaveSlotContainer.transform.Find("Panel/Text").GetComponent<Text>().text = Translation.GetText("save-slots").ToUpper();
		isNewGame = newGame;
		isLoadGame = loadGame;
		NavBar.SetActive(value: true);
	}

	public void drawSaveSlots()
	{
		GameObject gameObject = SaveSlotContainer.transform.Find("ScrollView").gameObject.transform.Find("SaveSlots").gameObject;
		GameObject original = gameObject.transform.Find("Helper").gameObject;
		int saveSlots = CustomerDataController.Instance.CustomerDataHolder.Userdata.SaveSlots;
		removeOldRows(gameObject);
		Dictionary<int, SaveMetadata> saveGameSlotMetadata = Save.GetSaveGameSlotMetadata();
		NavBar.transform.Find("LocalTab/Text").GetComponent<Text>().text = Translation.GetText("local-slots") + " (" + saveGameSlotMetadata.Count + ")";
		for (int i = 0; i < saveSlots; i++)
		{
			int num = i;
			GameObject gameObject2 = Object.Instantiate(original, gameObject.transform, worldPositionStays: false);
			gameObject2.name = i.ToString();
			gameObject2.SetActive(value: true);
			GameObject panel = gameObject2.transform.Find("Panel").gameObject;
			if (saveGameSlotMetadata.ContainsKey(num))
			{
				DrawFilledSlotMetaData19(num, saveGameSlotMetadata, gameObject2);
			}
			else if (!isLoadGame)
			{
				drawEmptyMetaData(panel, num);
			}
			else
			{
				gameObject2.SetActive(value: false);
			}
		}
		SetupNavBar();
		currentTab = Tabs.Local;
		StartCoroutine(RepositionSaveSlotsToTop());
	}

	private void TranslateVIPElemets()
	{
		VIPTitle.text = Translation.GetText("vip-member").ToUpper();
		VIPAdfree.text = Translation.GetText("ad-free");
		VIPExtraSlot.text = "2 " + Translation.GetText("save-game-slots");
		VIPDoubleRewards.text = Translation.GetText("double-bonus");
		VIPplusTitle.text = Translation.GetText("vip-member").ToUpper();
		VIPplusAdfree.text = Translation.GetText("ad-free");
		VIPplusExtraSlot.text = "5 " + Translation.GetText("save-game-slots");
		VIPplusDoubleRewards.text = Translation.GetText("double-bonus");
		VIPplusCredits.text = "2 750";
		VIPplusManagerPoint.text = "150";
	}

	private void SetVIPOnClickAndPrices()
	{
		VIPBtn.onClick.AddListener(() =>
		{
			Shop.PurchaseInAppProduct("sm21_vip_light", VIPBtn.gameObject);
			drawSaveSlots();
		});
		VIPplusBtn.onClick.AddListener(() =>
		{
			Shop.PurchaseInAppProduct("sm21_vip", VIPplusBtn.gameObject);
			drawSaveSlots();
		});
	}

	private void SetupNavBar()
	{
		GameObject CloudTabButton = NavBar.transform.Find("CloudTab").gameObject;
		GameObject LocalTabButton = NavBar.transform.Find("LocalTab").gameObject;
		CloudTabButton.GetComponent<Button>().onClick.RemoveAllListeners();
		LocalTabButton.GetComponent<Button>().onClick.RemoveAllListeners();
		CloudTabButton.GetComponent<Button>().onClick.AddListener(() =>
		{
			SwitchTabs(Tabs.Cloud);
			CloudTabButton.GetComponent<Image>().color = Helpers.HexToColour("6CDEAB");
			CloudTabButton.GetComponentInChildren<Text>().color = Helpers.HexToColour("1D1F27");
			LocalTabButton.GetComponent<Image>().color = Helpers.HexToColour("1D1F27");
			LocalTabButton.GetComponentInChildren<Text>().color = Helpers.HexToColour("6CDEAB");
		});
		LocalTabButton.GetComponent<Button>().onClick.AddListener(() =>
		{
			SwitchTabs(Tabs.Local);
			CloudTabButton.GetComponent<Image>().color = Helpers.HexToColour("1D1F27");
			CloudTabButton.GetComponentInChildren<Text>().color = Helpers.HexToColour("6CDEAB");
			LocalTabButton.GetComponent<Image>().color = Helpers.HexToColour("6CDEAB");
			LocalTabButton.GetComponentInChildren<Text>().color = Helpers.HexToColour("1D1F27");
		});
	}

	private void SwitchTabs(Tabs SelectedTab)
	{
		currentTab = SelectedTab;
		CloudSlotContainer.SetActive(SelectedTab == Tabs.Cloud);
		SaveSlotContainer.SetActive(SelectedTab == Tabs.Local);
	}

	private IEnumerator RepositionSaveSlotsToTop()
	{
		yield return new WaitForSeconds(0.2f);
		GameObject ScrollView = SaveSlotContainer.transform.Find("ScrollView").gameObject;
		GameObject gameObject = ScrollView.transform.Find("SaveSlots").gameObject;
		gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(gameObject.GetComponent<RectTransform>().anchoredPosition.x, 0f);
		ScrollView.GetComponent<ScrollRect>().enabled = false;
		yield return new WaitForSeconds(0.5f);
		ScrollView.GetComponent<ScrollRect>().enabled = true;
	}

	private void StartNewGame(int selectedSlot)
	{
		SelectSlot(selectedSlot);
		LaunchNewGame();
	}

	private void TakeUserBackToStartScreen()
	{
		ModalController.Instance().CloseAllModals();
		UI.screens[0].SetActive(value: true);
		UI.screens[2].SetActive(value: false);
	}

	private static void SelectSlot(int selectedSlot)
	{
		Save.currentSlot = selectedSlot;
		ModalController.Instance().CloseModal();
	}

	private static void LaunchNewGame()
	{
		UI.GetInstance().clubPickerScreen.LaunchPickClubCoroutine(ClubPickerScreen.getPickedClubID());
	}

	public static void SelectSlotAndStartNewGame(int selectedSlot)
	{
		Save.currentSlot = selectedSlot;
		ModalController.Instance().CloseModal();
		UI.GetInstance().clubPickerScreen.LaunchPickClubCoroutine(ClubPickerScreen.getPickedClubID());
	}

	private static void removeOldRows(GameObject Container)
	{
		int childCount = Container.transform.childCount;
		for (int i = 3; i < childCount; i++)
		{
			Object.Destroy(Container.transform.GetChild(i).gameObject);
		}
	}

	public void DrawFilledSlotMetaData19(int slotNum, Dictionary<int, SaveMetadata> metaData, GameObject saveClonePanel)
	{
		saveClonePanel.name = slotNum.ToString();
		GameObject gameObject = saveClonePanel.transform.Find("Panel").gameObject;
		gameObject.transform.Find("Name").GetComponent<Text>().text = metaData[slotNum].Name.ToUpper() + " - " + Translation.GetText("slot").ToUpper() + " " + (slotNum + 1);
		gameObject.transform.Find("GameDate").GetComponent<Text>().text = Translation.GetText("game-date") + ": " + Helpers.FormatShortDateForVersion(metaData[slotNum].GameDate);
		gameObject.transform.Find("SaveDate").GetComponent<Text>().text = Translation.GetText("save-date") + ": " + Helpers.FormatShortDateForVersion(metaData[slotNum].SaveDate);
		gameObject.transform.Find("ButtonPanel/BtnLoad/Text").GetComponent<Text>().text = Translation.GetText("Load-Game").ToUpper();
		gameObject.transform.Find("19GameBadge").gameObject.SetActive(value: true);
		gameObject.transform.Find("18GameBadge").gameObject.SetActive(value: false);
		if (metaData[slotNum].BuildVersion != "DEV")
		{
			gameObject.transform.Find("Version").GetComponent<Text>().text = metaData[slotNum].BuildVersion;
		}
		else
		{
			gameObject.transform.Find("Version").GetComponent<Text>().text = null;
		}
		ImageController.GetInstance().LoadSaveSlotClubImage(gameObject.transform.Find("ClubBadge").GetComponent<Image>(), metaData[slotNum]);
		bool showConfirmOnLoad = false;
		gameObject.transform.Find("ClubBadge").gameObject.SetActive(value: true);
		gameObject.transform.Find("EmptyImage").gameObject.SetActive(value: false);
		Button component = gameObject.transform.Find("ButtonPanel/BtnSave").GetComponent<Button>();
		Button component2 = gameObject.transform.Find("ButtonPanel/BtnLoad").GetComponent<Button>();
		Button component3 = gameObject.transform.Find("ButtonPanel/BtnCloud").GetComponent<Button>();
		Button component4 = gameObject.transform.Find("BtnDelete").GetComponent<Button>();
		if (slotNum == Save.currentSlot)
		{
			if (!isLoadGame)
			{
				component.onClick.AddListener(() =>
				{
					Save.SaveData(slotNum);
					ModalController.Instance().CloseModal();
				});
				component.transform.Find("Text").GetComponent<Text>().text = Translation.GetText("Save-Game").ToUpper();
			}
			else
			{
				component.gameObject.SetActive(value: false);
				component4.gameObject.SetActive(value: false);
			}
			component2.gameObject.SetActive(value: false);
		}
		else if (!isLoadGame && !isNewGame)
		{
			component.gameObject.SetActive(value: true);
			component.transform.Find("Text").GetComponent<Text>().text = Translation.GetText("Save-Game").ToUpper();
			component.onClick.AddListener(() =>
			{
				Save.SaveData(slotNum);
				ModalController.Instance().CloseModal();
			});
			component2.gameObject.SetActive(value: false);
		}
		else if (!isLoadGame)
		{
			component.gameObject.SetActive(value: true);
			component.transform.Find("Text").GetComponent<Text>().text = Translation.GetText("Save-Game").ToUpper();
			component.onClick.AddListener(() =>
			{
				StartNewGame(slotNum);
			});
			component2.gameObject.SetActive(value: false);
		}
		else if (!isNewGame)
		{
			component.gameObject.SetActive(value: false);
			component2.gameObject.SetActive(value: true);
			component2.onClick.AddListener(() =>
			{
				if (showConfirmOnLoad)
				{
					ModalController.Instance().ShowConfirmModal(Translation.GetText("Load-Game"), Translation.GetText("out-of-date-msg"), () =>
					{
						LoadUsersSaveFile(slotNum);
					});
				}
				else
				{
					LoadUsersSaveFile(slotNum);
				}
			});
		}
		component3.gameObject.SetActive(value: false);
		component3.onClick.AddListener(() =>
		{
			SelectCloudSlot4LocalSave(slotNum);
		});
		component4.onClick.AddListener(() =>
		{
			deleteSave19(slotNum, metaData, saveClonePanel);
		});
		saveClonePanel.SetActive(value: true);
	}

	private static void LoadUsersSaveFile(int slotNum)
	{
		ModalController.Instance().CloseAllModals();
		UI.GetInstance().StartCoroutine(UI.GetInstance().LoadSaveGameButtonClickHandler(slotNum));
	}

	public static void drawEmptyMetaData(GameObject Panel, int slotNum)
	{
		Panel.transform.Find("Name").GetComponent<Text>().text = Translation.GetText("save-to-slot");
		Panel.transform.Find("GameDate").GetComponent<Text>().text = null;
		Panel.transform.Find("SaveDate").GetComponent<Text>().text = null;
		Panel.transform.Find("Version").GetComponent<Text>().text = null;
		Button component = Panel.transform.Find("ButtonPanel/BtnSave").GetComponent<Button>();
		Button component2 = Panel.transform.Find("ButtonPanel/BtnLoad").GetComponent<Button>();
		Button component3 = Panel.transform.Find("ButtonPanel/BtnCloud").GetComponent<Button>();
		Button component4 = Panel.transform.Find("BtnDelete").GetComponent<Button>();
		if (!isLoadGame)
		{
			if (isNewGame)
			{
				component.onClick.AddListener(() =>
				{
					instance.StartNewGame(slotNum);
				});
			}
			else
			{
				component.onClick.AddListener(() =>
				{
					Save.SaveData(slotNum);
					ModalController.Instance().CloseModal();
				});
			}
			component.gameObject.SetActive(value: true);
			component.transform.Find("Text").GetComponent<Text>().text = Translation.GetText("Save-Game");
		}
		component3.gameObject.SetActive(value: false);
		component4.gameObject.SetActive(value: false);
		component2.gameObject.SetActive(value: false);
	}

	public static void DrawEmptySlotForChina(GameObject Panel, int slotNum)
	{
		Panel.transform.Find("Name").GetComponent<Text>().text = Translation.GetText("save-to-slot");
		Panel.transform.Find("GameDate").GetComponent<Text>().text = null;
		Panel.transform.Find("SaveDate").GetComponent<Text>().text = null;
		Panel.transform.Find("Season").GetComponent<Text>().text = null;
		Panel.transform.Find("Version").GetComponent<Text>().text = null;
	}

	private static void setButtonsActive(bool active, Dictionary<int, SaveMetadata> metaData)
	{
		GameObject gameObject = SaveSlotContainer.transform.Find("ScrollView").transform.Find("SaveSlots").gameObject;
		int childCount = gameObject.transform.childCount;
		for (int i = 3; i < childCount; i++)
		{
			GameObject gameObject2 = gameObject.transform.GetChild(i).gameObject;
			if (metaData.ContainsKey(i - 1))
			{
				Button component = gameObject2.transform.Find("Panel").transform.Find("ButtonPanel/BtnSave").GetComponent<Button>();
				Button component2 = gameObject2.transform.Find("Panel").transform.Find("ButtonPanel/BtnLoad").GetComponent<Button>();
				gameObject2.transform.Find("Panel").transform.Find("ButtonPanel/BtnCloud").GetComponent<Button>();
				Button component3 = gameObject2.transform.Find("Panel").transform.Find("BtnDelete").GetComponent<Button>();
				component.interactable = active;
				component2.interactable = active;
				component3.interactable = active;
			}
			else
			{
				gameObject2.transform.Find("Panel").transform.Find("ButtonPanel/BtnLoad").GetComponent<Button>().interactable = active;
			}
		}
	}

	private static void deleteSave19(int id, Dictionary<int, SaveMetadata> metaData, GameObject Panel)
	{
		setButtonsActive(active: false, metaData);
		ScrollRect ScrollView = SaveSlotContainer.transform.Find("ScrollView").GetComponent<ScrollRect>();
		ScrollView.vertical = false;
		overwritePanel.SetActive(value: true);
		overwritePanel.transform.Find("ModalText").GetComponent<Text>().text = Translation.GetText("delete");
		Button component = overwritePanel.transform.Find("ConfirmBtn").GetComponent<Button>();
		component.transform.Find("Text").GetComponent<Text>().text = Translation.GetText("ok");
		component.onClick.RemoveAllListeners();
		Button component2 = overwritePanel.transform.Find("CancelBtn").GetComponent<Button>();
		component2.transform.Find("Text").GetComponent<Text>().text = Translation.GetText("cancel");
		component2.onClick.RemoveAllListeners();
		component.onClick.AddListener(() =>
		{
			Save.DeleteSaveGame(id);
			resetSaveGameSlotAfterDelete(Panel, id);
			overwritePanel.SetActive(value: false);
			setButtonsActive(active: true, metaData);
			ScrollView.vertical = true;
		});
		component2.onClick.AddListener(() =>
		{
			overwritePanel.SetActive(value: false);
			setButtonsActive(active: true, metaData);
			ScrollView.vertical = true;
		});
	}

	private static void resetSaveGameSlotAfterDelete(GameObject Slot, int SlotNum)
	{
		GameObject gameObject = Slot.transform.Find("Panel").gameObject;
		drawEmptyMetaData(gameObject, SlotNum);
		gameObject.transform.Find("ClubBadge").gameObject.SetActive(value: false);
		gameObject.transform.Find("EmptyImage").gameObject.SetActive(value: true);
		gameObject.transform.Find("18GameBadge").gameObject.SetActive(value: false);
		gameObject.transform.Find("19GameBadge").gameObject.SetActive(value: false);
		Dictionary<int, SaveMetadata> saveGameSlotMetadata = Save.GetSaveGameSlotMetadata();
		NavBar.transform.Find("LocalTab/Text").GetComponent<Text>().text = Translation.GetText("local-slots") + " (" + saveGameSlotMetadata.Count + ")";
	}

	public void DrawSlotSelectForRepairedSave()
	{
		GameObject gameObject = SaveSlotContainer.transform.Find("ScrollView").gameObject.transform.Find("SaveSlots").gameObject;
		GameObject original = gameObject.transform.Find("Helper").gameObject;
		int num = 1;
		num = CustomerDataController.Instance.CustomerDataHolder.Userdata.SaveSlots;
		removeOldRows(gameObject);
		Dictionary<int, SaveMetadata> saveGameSlotMetadata = Save.GetSaveGameSlotMetadata();
		for (int i = 0; i < num; i++)
		{
			int num2 = i;
			GameObject gameObject2 = Object.Instantiate(original, gameObject.transform, worldPositionStays: false);
			gameObject2.name = i.ToString();
			gameObject2.SetActive(value: true);
			GameObject gameObject3 = gameObject2.transform.Find("Panel").gameObject;
			if (saveGameSlotMetadata.ContainsKey(num2))
			{
				DrawFilledSlotMetaDataForRepairedSave(num2, saveGameSlotMetadata, gameObject2);
			}
			else
			{
				drawEmptyMetaDataForRepairedSave(gameObject3, num2);
			}
			gameObject3.transform.Find("ButtonPanel/BtnCloud").gameObject.SetActive(value: false);
		}
		NavBar.transform.Find("CloudTab").gameObject.SetActive(value: false);
		NavBar.transform.Find("LocalTab").gameObject.SetActive(value: false);
		StartCoroutine(RepositionSaveSlotsToTop());
	}

	public void DrawFilledSlotMetaDataForRepairedSave(int slotNum, Dictionary<int, SaveMetadata> metaData, GameObject saveClonePanel)
	{
		saveClonePanel.name = slotNum.ToString();
		GameObject gameObject = saveClonePanel.transform.Find("Panel").gameObject;
		gameObject.transform.Find("Name").GetComponent<Text>().text = metaData[slotNum].Name + " - " + Translation.GetText("slot") + " " + (slotNum + 1);
		gameObject.transform.Find("GameDate").GetComponent<Text>().text = Translation.GetText("game-date") + ": " + Helpers.FormatShortDateForVersion(metaData[slotNum].GameDate);
		gameObject.transform.Find("SaveDate").GetComponent<Text>().text = Translation.GetText("save-date") + ": " + Helpers.FormatShortDateForVersion(metaData[slotNum].SaveDate);
		gameObject.transform.Find("ButtonPanel/BtnLoad/Text").GetComponent<Text>().text = Translation.GetText("Load-Game");
		gameObject.transform.Find("19GameBadge").gameObject.SetActive(value: true);
		gameObject.transform.Find("18GameBadge").gameObject.SetActive(value: false);
		if (metaData[slotNum].BuildVersion != "DEV")
		{
			gameObject.transform.Find("Version").GetComponent<Text>().text = metaData[slotNum].BuildVersion;
		}
		else
		{
			gameObject.transform.Find("Version").GetComponent<Text>().text = null;
		}
		if (metaData[slotNum].ClubBadge != "")
		{
			ImageController.GetInstance().LoadSaveSlotClubImage(gameObject.transform.Find("ClubBadge").GetComponent<Image>(), metaData[slotNum]);
		}
		else
		{
			gameObject.transform.Find("ClubBadge").GetComponent<Image>().sprite = Resources.Load<Sprite>("noimagefoundclub");
		}
		gameObject.transform.Find("CloudStatus").gameObject.SetActive(value: false);
		gameObject.transform.Find("ClubBadge").gameObject.SetActive(value: true);
		gameObject.transform.Find("EmptyImage").gameObject.SetActive(value: false);
		Button component = gameObject.transform.Find("ButtonPanel/BtnSave").GetComponent<Button>();
		Button component2 = gameObject.transform.Find("ButtonPanel/BtnLoad").GetComponent<Button>();
		Button component3 = gameObject.transform.Find("ButtonPanel/BtnCloud").GetComponent<Button>();
		Button component4 = gameObject.transform.Find("BtnDelete").GetComponent<Button>();
		component.gameObject.SetActive(value: true);
		component.transform.Find("Text").GetComponent<Text>().text = Translation.GetText("Save-Game");
		component.onClick.AddListener(() =>
		{
			HelpController.DownloadRepairedSaveFile(slotNum);
		});
		component2.gameObject.SetActive(value: false);
		component3.gameObject.SetActive(value: false);
		component4.gameObject.SetActive(value: false);
		saveClonePanel.SetActive(value: true);
	}

	public void drawEmptyMetaDataForRepairedSave(GameObject Panel, int slotNum)
	{
		Panel.transform.Find("Name").GetComponent<Text>().text = Translation.GetText("save-to-slot");
		Panel.transform.Find("GameDate").GetComponent<Text>().text = null;
		Panel.transform.Find("SaveDate").GetComponent<Text>().text = null;
		Panel.transform.Find("Version").GetComponent<Text>().text = null;
		Button component = Panel.transform.Find("ButtonPanel/BtnSave").GetComponent<Button>();
		Button component2 = Panel.transform.Find("ButtonPanel/BtnLoad").GetComponent<Button>();
		Button component3 = Panel.transform.Find("ButtonPanel/BtnCloud").GetComponent<Button>();
		Button component4 = Panel.transform.Find("BtnDelete").GetComponent<Button>();
		component.onClick.AddListener(() =>
		{
			HelpController.DownloadRepairedSaveFile(slotNum);
		});
		component3.gameObject.SetActive(value: false);
		component4.gameObject.SetActive(value: false);
		component2.gameObject.SetActive(value: false);
	}

	public void DrawCloudSaveSlots(Dictionary<int, SaveMetadata> metaData)
	{
		GameObject gameObject = CloudSlotContainer.transform.Find("ScrollView").gameObject.transform.Find("SaveSlots").gameObject;
		GameObject gameObject2 = gameObject.transform.Find("Helper").gameObject;
		removeOldRows(gameObject);
		int num = 1;
		num = CustomerDataController.Instance.CustomerDataHolder.Userdata.SaveSlots;
		CloudSaveCount = metaData.Count;
		NavBar.transform.Find("CloudTab/Text").GetComponent<Text>().text = Translation.GetText("cloud-slots") + " (" + CloudSaveCount + ")";
		for (int i = 0; i < num; i++)
		{
			GameObject gameObject3 = Object.Instantiate(gameObject2);
			gameObject3.transform.SetParent(gameObject.transform);
			gameObject3.transform.localScale = gameObject2.transform.localScale;
			gameObject3.SetActive(value: true);
			gameObject3.name = i.ToString();
			if (metaData.ContainsKey(i))
			{
				DrawCloudMetaData(i, metaData[i], gameObject3, screen);
			}
			else
			{
				drawEmptyCloudMetaData(gameObject3, i);
			}
		}
	}

	public void DrawCloudMetaData(int slotNum, SaveMetadata metaData, GameObject saveClonePanel, GameObject saveLoadScreen)
	{
		saveClonePanel.name = slotNum.ToString();
		GameObject Panel = saveClonePanel.transform.Find("Panel").gameObject;
		Panel.transform.Find("Name").GetComponent<Text>().text = Club.GetName(metaData.ClubID) + " - " + Translation.GetText("slot") + " " + (slotNum + 1);
		Panel.transform.Find("GameDate").GetComponent<Text>().text = Translation.GetText("game-date") + ": " + Helpers.FormatShortDateForVersion(metaData.GameDate);
		Panel.transform.Find("SaveDate").GetComponent<Text>().text = Translation.GetText("save-date") + ": " + Helpers.FormatShortDateForVersion(metaData.SaveDate);
		ImageController.GetInstance().LoadSaveSlotClubImage(Panel.transform.Find("ClubBadge").GetComponent<Image>(), metaData);
		Panel.transform.Find("ClubBadge").gameObject.SetActive(value: true);
		Panel.transform.Find("EmptyImage").gameObject.SetActive(value: false);
		Button component = Panel.transform.Find("ButtonPanel/BtnSave").GetComponent<Button>();
		Button component2 = Panel.transform.Find("ButtonPanel/BtnLoad").GetComponent<Button>();
		Button component3 = Panel.transform.Find("BtnDelete").GetComponent<Button>();
		Button component4 = Panel.transform.Find("ButtonPanel/BtnCloud").GetComponent<Button>();
		Panel.transform.Find("Version").gameObject.SetActive(value: false);
		component.gameObject.SetActive(value: false);
		component4.gameObject.SetActive(value: false);
		component2.gameObject.SetActive(UI.screens[0].activeSelf);
		component3.onClick.AddListener(() =>
		{
			deleteCloudSave(slotNum, Panel);
		});
		component2.transform.Find("Text").GetComponent<Text>().text = Translation.GetText("download");
		component2.onClick.AddListener(() =>
		{
			ShowLocalSlotsForCloudDownload(slotNum);
		});
	}

	public void drawEmptyCloudMetaData(GameObject saveClonePanel, int slotNum)
	{
		GameObject gameObject = saveClonePanel.transform.Find("Panel").gameObject;
		gameObject.transform.Find("Name").GetComponent<Text>().text = Translation.GetText("slot") + " " + (slotNum + 1);
		gameObject.transform.Find("GameDate").GetComponent<Text>().text = Translation.GetText("game-date");
		gameObject.transform.Find("SaveDate").GetComponent<Text>().text = Translation.GetText("save-date");
		gameObject.transform.Find("GameDate").gameObject.SetActive(value: false);
		gameObject.transform.Find("SaveDate").gameObject.SetActive(value: false);
		gameObject.transform.Find("BtnDelete").gameObject.SetActive(value: false);
		gameObject.transform.Find("ButtonPanel/BtnSave").gameObject.SetActive(value: false);
		gameObject.transform.Find("ButtonPanel/BtnLoad").gameObject.SetActive(value: false);
		gameObject.transform.Find("ButtonPanel/BtnCloud").gameObject.SetActive(value: false);
		gameObject.transform.Find("Version").gameObject.SetActive(value: false);
	}

	public void deleteCloudSave(int id, GameObject Panel)
	{
		ScrollRect ScrollView = CloudSlotContainer.transform.Find("ScrollView").GetComponent<ScrollRect>();
		ScrollView.vertical = false;
		overwritePanel.SetActive(value: true);
		overwritePanel.transform.Find("ModalText").GetComponent<Text>().text = Translation.GetText("delete");
		Button component = overwritePanel.transform.Find("ConfirmBtn").GetComponent<Button>();
		component.onClick.RemoveAllListeners();
		Button component2 = overwritePanel.transform.Find("CancelBtn").GetComponent<Button>();
		component2.onClick.RemoveAllListeners();
		component.transform.Find("Text").GetComponent<Text>().text = Translation.GetText("ok");
		component2.transform.Find("Text").GetComponent<Text>().text = Translation.GetText("cancel");
		component.onClick.AddListener(() =>
		{
			Save.DeleteFromCloud(id);
			resetCloudSlotAfterDelete(Panel, id);
			overwritePanel.SetActive(value: false);
			ScrollView.vertical = true;
		});
		component2.onClick.AddListener(() =>
		{
			overwritePanel.SetActive(value: false);
			ScrollView.vertical = true;
		});
	}

	public void ShowLocalSlotsForCloudDownload(int cloudSlotNum)
	{
		SwitchTabs(Tabs.Local);
		GameObject gameObject = SaveSlotContainer.transform.Find("ScrollView").gameObject.transform.Find("SaveSlots").gameObject;
		int childCount = gameObject.transform.childCount;
		for (int i = 1; i < childCount; i++)
		{
			int slotNum = i - 1;
			GameObject gameObject2 = gameObject.transform.GetChild(i).gameObject;
			gameObject2.SetActive(value: true);
			GameObject gameObject3 = gameObject2.transform.Find("Panel").gameObject;
			Button component = gameObject3.transform.Find("ButtonPanel/BtnSave").GetComponent<Button>();
			Button component2 = gameObject3.transform.Find("ButtonPanel/BtnLoad").GetComponent<Button>();
			Button component3 = gameObject3.transform.Find("ButtonPanel/BtnCloud").GetComponent<Button>();
			component.gameObject.SetActive(value: true);
			component.onClick.AddListener(() =>
			{
				Save.GetFileFromCloud(cloudSlotNum, slotNum);
			});
			component.transform.Find("Text").GetComponent<Text>().text = Translation.GetText("save-to-slot");
			component2.gameObject.SetActive(value: false);
			component3.gameObject.SetActive(value: false);
		}
	}

	private void resetCloudSlotAfterDelete(GameObject Panel, int slotNum)
	{
		Panel.transform.Find("Name").GetComponent<Text>().text = Translation.GetText("slot") + " " + slotNum;
		Panel.transform.Find("GameDate").GetComponent<Text>().text = null;
		Panel.transform.Find("SaveDate").GetComponent<Text>().text = null;
		Panel.transform.Find("ClubBadge").gameObject.SetActive(value: false);
		Panel.transform.Find("EmptyImage").gameObject.SetActive(value: true);
		Panel.transform.Find("ButtonPanel/BtnSave").gameObject.SetActive(value: false);
		Panel.transform.Find("ButtonPanel/BtnLoad").gameObject.SetActive(value: false);
		CloudSaveCount--;
		NavBar.transform.Find("CloudTab/Text").GetComponent<Text>().text = Translation.GetText("cloud-slots") + " (" + CloudSaveCount + ")";
	}

	private void SelectCloudSlot4LocalSave(int slotNum)
	{
		SwitchTabs(Tabs.Cloud);
		GameObject gameObject = CloudSlotContainer.transform.Find("ScrollView").gameObject.transform.Find("SaveSlots").gameObject;
		int childCount = gameObject.transform.childCount;
		for (int i = 1; i < childCount; i++)
		{
			int cloudSlotNum = i - 1;
			GameObject gameObject2 = gameObject.transform.GetChild(i).gameObject;
			gameObject2.SetActive(value: true);
			GameObject gameObject3 = gameObject2.transform.Find("Panel").gameObject;
			Button component = gameObject3.transform.Find("ButtonPanel/BtnSave").GetComponent<Button>();
			Button component2 = gameObject3.transform.Find("ButtonPanel/BtnLoad").GetComponent<Button>();
			Button component3 = gameObject3.transform.Find("ButtonPanel/BtnCloud").GetComponent<Button>();
			component.gameObject.SetActive(value: true);
			component.onClick.AddListener(() =>
			{
				StartFileUpload(slotNum, cloudSlotNum);
			});
			component.transform.Find("Text").GetComponent<Text>().text = Translation.GetText("save-to-slot");
			component2.gameObject.SetActive(value: false);
			component3.gameObject.SetActive(value: false);
		}
	}

	private void StartFileUpload(int slotNum, int cloudSlotNum)
	{
		screen.transform.Find("CloudUIBlocker").gameObject.SetActive(value: true);
		screen.transform.Find("CloudUIBlocker/Text").GetComponent<Text>().text = Translation.GetText("please-wait");
		Game.instance.upload.StartUpload("SM21_save_" + slotNum, cloudSlotNum, slotNum);
	}

	public static void FinaliseFileUpload()
	{
		Save.GetAndDrawCloudSaveSlotMetadata();
		screen.transform.Find("CloudUIBlocker").gameObject.SetActive(value: false);
	}
}
