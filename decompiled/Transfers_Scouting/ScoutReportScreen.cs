using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ScoutReportScreen : MonoBehaviour
{
	private enum FilterType
	{
		MIN_AGE,
		MAX_AGE,
		POSITION,
		MIN_VALUE,
		MAX_VALUE,
		RATING,
		POTENTIAL,
		INTERESTED
	}

	private List<ScoutSearch> searchsDrawn = new List<ScoutSearch>();

	private List<int> scoutedPositions = new List<int>();

	private double minVal;

	private double maxVal;

	private FilterSettings FilterSettings = new FilterSettings();

	private int[] ratingBands = new int[6] { 40, 50, 60, 70, 80, 90 };

	private int[] ageBands = new int[7] { 18, 21, 24, 27, 30, 33, 35 };

	private double[] valueBands = new double[6];

	public void Awake()
	{
		GameObject minAgeButton = transform.Find("ScreenContainer/ScoutRight/List/Helper/DropDownButton").gameObject;
		minAgeButton.GetComponent<Button>().onClick.AddListener(() =>
		{
			minAgeButton.transform.Find("Dropdown List").GetComponent<Canvas>().sortingLayerName = "Modal";
			GameObject.Find("Blocker").transform.SetParent(transform);
			GameObject.Find("Blocker").transform.localPosition = Vector3.zero;
		});
		GameObject maxAgeButton = transform.Find("ScreenContainer/ScoutRight/List/Helper (1)/DropDownButton").gameObject;
		maxAgeButton.GetComponent<Button>().onClick.AddListener(() =>
		{
			maxAgeButton.transform.Find("Dropdown List").GetComponent<Canvas>().sortingLayerName = "Modal";
			GameObject.Find("Blocker").transform.SetParent(transform);
			GameObject.Find("Blocker").transform.localPosition = Vector3.zero;
		});
		GameObject posButton = transform.Find("ScreenContainer/ScoutRight/List/Helper (2)/DropDownButton").gameObject;
		posButton.GetComponent<Button>().onClick.AddListener(() =>
		{
			posButton.transform.Find("Dropdown List").GetComponent<Canvas>().sortingLayerName = "Modal";
			GameObject.Find("Blocker").transform.SetParent(transform);
			GameObject.Find("Blocker").transform.localPosition = Vector3.zero;
		});
		GameObject minValButton = transform.Find("ScreenContainer/ScoutRight/List/Helper (3)/DropDownButton").gameObject;
		minValButton.GetComponent<Button>().onClick.AddListener(() =>
		{
			minValButton.transform.Find("Dropdown List").GetComponent<Canvas>().sortingLayerName = "Modal";
			GameObject.Find("Blocker").transform.SetParent(transform);
			GameObject.Find("Blocker").transform.localPosition = Vector3.zero;
		});
		GameObject maxValButton = transform.Find("ScreenContainer/ScoutRight/List/Helper (4)/DropDownButton").gameObject;
		maxValButton.GetComponent<Button>().onClick.AddListener(() =>
		{
			maxValButton.transform.Find("Dropdown List").GetComponent<Canvas>().sortingLayerName = "Modal";
			GameObject.Find("Blocker").transform.SetParent(transform);
			GameObject.Find("Blocker").transform.localPosition = Vector3.zero;
		});
		GameObject ratingButton = transform.Find("ScreenContainer/ScoutRight/List/Helper (5)/DropDownButton").gameObject;
		ratingButton.GetComponent<Button>().onClick.AddListener(() =>
		{
			ratingButton.transform.Find("Dropdown List").GetComponent<Canvas>().sortingLayerName = "Modal";
			GameObject.Find("Blocker").transform.SetParent(transform);
			GameObject.Find("Blocker").transform.localPosition = Vector3.zero;
		});
		GameObject potentialButton = transform.Find("ScreenContainer/ScoutRight/List/Helper (6)/DropDownButton").gameObject;
		potentialButton.GetComponent<Button>().onClick.AddListener(() =>
		{
			potentialButton.transform.Find("Dropdown List").GetComponent<Canvas>().sortingLayerName = "Modal";
			GameObject.Find("Blocker").transform.SetParent(transform);
			GameObject.Find("Blocker").transform.localPosition = Vector3.zero;
		});
		GameObject interestedButton = transform.Find("ScreenContainer/ScoutRight/List/Helper (7)/DropDownButton").gameObject;
		interestedButton.GetComponent<Button>().onClick.AddListener(() =>
		{
			interestedButton.transform.Find("Dropdown List").GetComponent<Canvas>().sortingLayerName = "Modal";
			GameObject.Find("Blocker").transform.SetParent(transform);
			GameObject.Find("Blocker").transform.localPosition = Vector3.zero;
		});
	}

	public void DrawScreen()
	{
		DrawResults();
		InitFilters();
		DrawTopBar();
		EnableFilteredResults();
	}

	private void EnableFilteredResults()
	{
		foreach (Transform item in base.transform.Find("ScreenContainer/ScoutAssignments/Content/ScrollView/Viewport/OptionsPanel"))
		{
			if (item.name.Contains("Player"))
			{
				item.gameObject.SetActive(value: true);
			}
		}
	}

	private void DrawResults()
	{
		transform.Find("ScreenContainer/ScoutAssignments/ContentHeader/Text").GetComponent<Text>().text = Translation.GetText("scout-reports").ToUpper();
		foreach (ScoutSearch item in Game.instance.g_ScoutingSearch)
		{
			if (item.dateend < Game.instance.CurrentDate.AddMonths(3) && !searchsDrawn.Contains(item))
			{
				DrawPlayersFromReport(item);
				searchsDrawn.Add(item);
			}
		}
	}

	private void DrawPlayersFromReport(ScoutSearch scoutSearch)
	{
		int currentUserClubID = Game.GetCurrentUserClubID();
		foreach (ScoutSearchPlayer result in scoutSearch.results)
		{
			if (!result.deleted && Player.GetClubID(result.playerid) != currentUserClubID)
			{
				DrawPlayer(result, scoutSearch.dateend);
			}
		}
		transform.Find("ScreenContainer/ScoutAssignments/Content/ScrollView/Viewport/OptionsPanel/ReportHelper").gameObject.SetActive(value: false);
	}

	private void DrawPlayer(ScoutSearchPlayer player, DateTime dateend)
	{
		GameObject newPlayer = UnityEngine.Object.Instantiate(transform.Find("ScreenContainer/ScoutAssignments/Content/ScrollView/Viewport/OptionsPanel/ReportHelper").gameObject, transform.Find("ScreenContainer/ScoutAssignments/Content/ScrollView/Viewport/OptionsPanel/ReportHelper").parent);
		double num = Player.GetValue(player.playerid);
		newPlayer.SetActive(value: true);
		newPlayer.transform.SetAsFirstSibling();
		newPlayer.name = "PlayerReport" + player.playerid;
		newPlayer.transform.Find("Header/PlayerName").GetComponent<Text>().text = Player.GetName(player.playerid);
		newPlayer.transform.Find("ImagePanel/PlayerCardSmall").GetComponent<PlayerCard>().SetType(PlayerCardType.Small);
		newPlayer.transform.Find("ImagePanel/PlayerCardSmall").GetComponent<PlayerCard>().FillPlayerDetails(player.playerid, Player.GetClubID(player.playerid));
		newPlayer.transform.Find("ImagePanel/PlayerCardSmall").GetComponent<PlayerCard>().GetPlayerImage(player.playerid);
		newPlayer.transform.Find("Details1/Development/Title").GetComponent<Text>().text = Translation.GetText("player-development");
		newPlayer.transform.Find("Details1/PotentialRating/Title").GetComponent<Text>().text = Translation.GetText("potential");
		newPlayer.transform.Find("ButtonPrimary/ButtonText").GetComponent<Text>().text = Translation.GetText("view").ToUpper();
		if (Player.GetDisplayRating(player.playerid) > player.lowerPotential)
		{
			player.lowerPotential = Player.GetDisplayRating(player.playerid);
		}
		if (player.higherPotential > 99)
		{
			player.higherPotential = 99;
		}
		switch (Player.GetRatingState(player.playerid))
		{
		case PlayerRatingState.INCREASING:
			newPlayer.transform.Find("Details1/Development/Value").GetComponent<Image>().sprite = UI.GetInstance().GetMoralSprite(9);
			break;
		case PlayerRatingState.PEAK:
			newPlayer.transform.Find("Details1/Development/Value").GetComponent<Image>().sprite = UI.GetInstance().GetMoralSprite(5);
			break;
		case PlayerRatingState.DECLINING:
			newPlayer.transform.Find("Details1/Development/Value").GetComponent<Image>().sprite = UI.GetInstance().GetMoralSprite(1);
			break;
		}
		Helpers.SetPlayerRatingAndColour(newPlayer.transform.Find("Details1/PotentialRating/Value/Rating/OldRating").gameObject, player.lowerPotential);
		Helpers.SetPlayerRatingAndColour(newPlayer.transform.Find("Details1/PotentialRating/Value/Rating/NewRating").gameObject, player.higherPotential);
		short currentUserClubID = Game.GetCurrentUserClubID();
		RGYBCode rGYBCode = Club.GetRGYBCode(currentUserClubID);
		RGYBCode rGYBCode2 = PlayerData.GetRGYBCode(player.playerid);
		switch (RGYBCodeHelpers.GetCodeCompatibility(rGYBCode, rGYBCode2))
		{
		case RGYBCompatibility.High:
			newPlayer.transform.Find("Details2/ColourCode/Title").GetComponent<Text>().text = Translation.TemplateReplace(Translation.GetText("high-squad-chemistry-2"), new Dictionary<string, string>
			{
				{
					"Player Name",
					Player.GetName(player.playerid)
				},
				{
					"Club Name",
					Club.GetName(currentUserClubID)
				}
			});
			break;
		case RGYBCompatibility.Medium:
			newPlayer.transform.Find("Details2/ColourCode/Title").GetComponent<Text>().text = Translation.TemplateReplace(Translation.GetText("medium-squad-chemistry-2"), new Dictionary<string, string>
			{
				{
					"Player Name",
					Player.GetName(player.playerid)
				},
				{
					"Club Name",
					Club.GetName(currentUserClubID)
				}
			});
			break;
		default:
			newPlayer.transform.Find("Details2/ColourCode/Title").GetComponent<Text>().text = Translation.TemplateReplace(Translation.GetText("low-squad-chemistry-2"), new Dictionary<string, string>
			{
				{
					"Player Name",
					Player.GetName(player.playerid)
				},
				{
					"Club Name",
					Club.GetName(currentUserClubID)
				}
			});
			break;
		}
		string text = "";
		int clubID = Player.GetClubID(player.playerid);
		if (clubID != 0 && Transfers.instance.IsPlayerOnTransferList(player.playerid))
		{
			text = Translation.GetText("transfer-listed");
		}
		else if (clubID != 0 && Transfers.instance.transferListManager.isPlayerOnLoanList(player.playerid))
		{
			text = Translation.GetText("loan-listed");
		}
		else
		{
			text = ((clubID != 0) ? Translation.GetText("not-listed") : Translation.GetText("free-agent"));
		}
		newPlayer.transform.Find("Details2/Transfer Status/Title").GetComponent<Text>().text = text;
		newPlayer.transform.Find("Header/Date").GetComponent<Text>().text = Helpers.FormatShorterDateForVersion(dateend);
		newPlayer.transform.Find("ButtonPrimary").GetComponent<Button>().onClick.RemoveAllListeners();
		newPlayer.transform.Find("ButtonPrimary").GetComponent<Button>().onClick.AddListener(() =>
		{
			ModalController.Instance().ShowPlayerProfileModal(player.playerid, PlayerProfileScreen.PlayerTabs.Report);
			EventLoggingController.LogEvent("scoutReportViewReport");
		});
		newPlayer.transform.Find("Header/Delete").GetComponent<Button>().onClick.RemoveAllListeners();
		newPlayer.transform.Find("Header/Delete").GetComponent<Button>().onClick.AddListener(() =>
		{
			DeleteReport(player, newPlayer);
			EventLoggingController.LogEvent("scoutReportDeleteReport");
		});
		if (!scoutedPositions.Contains(PlayerData.GetPreferredPosition(player.playerid)))
		{
			scoutedPositions.Add(PlayerData.GetPreferredPosition(player.playerid));
		}
		if (num < minVal || minVal == 0.0)
		{
			minVal = num;
		}
		if (num > maxVal)
		{
			maxVal = num;
		}
	}

	private void DeleteReport(ScoutSearchPlayer player, GameObject row)
	{
		player.deleted = true;
		UnityEngine.Object.Destroy(row);
	}

	private void InitFilters()
	{
		transform.Find("ScreenContainer/ScoutRight/Header/Text").GetComponent<Text>().text = Translation.GetText("filter");
		transform.Find("ScreenContainer/ScoutRight/List/Helper/Title").GetComponent<Text>().text = Translation.GetText("minimum-age");
		transform.Find("ScreenContainer/ScoutRight/List/Helper (1)/Title").GetComponent<Text>().text = Translation.GetText("maximum-age");
		transform.Find("ScreenContainer/ScoutRight/List/Helper (2)/Title").GetComponent<Text>().text = Translation.GetText("position");
		transform.Find("ScreenContainer/ScoutRight/List/Helper (3)/Title").GetComponent<Text>().text = Translation.GetText("minimum-value");
		transform.Find("ScreenContainer/ScoutRight/List/Helper (4)/Title").GetComponent<Text>().text = Translation.GetText("maximum-value");
		transform.Find("ScreenContainer/ScoutRight/List/Helper (5)/Title").GetComponent<Text>().text = Translation.GetText("base-rating");
		transform.Find("ScreenContainer/ScoutRight/List/Helper (6)/Title").GetComponent<Text>().text = Translation.GetText("potential");
		transform.Find("ScreenContainer/ScoutRight/List/Helper (7)/Title").GetComponent<Text>().text = Translation.GetText("interested");
		Dropdown minAgeDropdown = transform.Find("ScreenContainer/ScoutRight/List/Helper/DropDownButton").GetComponent<Dropdown>();
		List<Dropdown.OptionData> ageData = GetAgeData();
		minAgeDropdown.ClearOptions();
		minAgeDropdown.AddOptions(ageData);
		Dropdown maxAgeDropdown = transform.Find("ScreenContainer/ScoutRight/List/Helper (1)/DropDownButton").GetComponent<Dropdown>();
		maxAgeDropdown.ClearOptions();
		maxAgeDropdown.AddOptions(ageData);
		Dropdown positionDropdown = transform.Find("ScreenContainer/ScoutRight/List/Helper (2)/DropDownButton").GetComponent<Dropdown>();
		positionDropdown.ClearOptions();
		positionDropdown.AddOptions(GetPositionData());
		Dropdown minValDropdown = transform.Find("ScreenContainer/ScoutRight/List/Helper (3)/DropDownButton").GetComponent<Dropdown>();
		Dropdown maxValDropdown = transform.Find("ScreenContainer/ScoutRight/List/Helper (4)/DropDownButton").GetComponent<Dropdown>();
		List<Dropdown.OptionData> valueData = GetValueData();
		minValDropdown.ClearOptions();
		maxValDropdown.ClearOptions();
		minValDropdown.AddOptions(valueData);
		maxValDropdown.AddOptions(valueData);
		Dropdown ratingDropdown = transform.Find("ScreenContainer/ScoutRight/List/Helper (5)/DropDownButton").GetComponent<Dropdown>();
		Dropdown potentialDropdown = transform.Find("ScreenContainer/ScoutRight/List/Helper (6)/DropDownButton").GetComponent<Dropdown>();
		List<Dropdown.OptionData> ratingData = GetRatingData();
		ratingDropdown.ClearOptions();
		potentialDropdown.ClearOptions();
		ratingDropdown.AddOptions(ratingData);
		potentialDropdown.AddOptions(ratingData);
		Dropdown interestedDropdown = transform.Find("ScreenContainer/ScoutRight/List/Helper (7)/DropDownButton").GetComponent<Dropdown>();
		interestedDropdown.ClearOptions();
		interestedDropdown.AddOptions(new List<Dropdown.OptionData>
		{
			new Dropdown.OptionData(Translation.GetText("any").ToUpper()),
			new Dropdown.OptionData(Translation.GetText("interested")),
			new Dropdown.OptionData(Translation.GetText("not-interested"))
		});
		minAgeDropdown.onValueChanged.RemoveAllListeners();
		minAgeDropdown.onValueChanged.AddListener(delegate
		{
			SetFilterValue(FilterType.MIN_AGE, minAgeDropdown.value);
			FilterResults();
		});
		maxAgeDropdown.onValueChanged.RemoveAllListeners();
		maxAgeDropdown.onValueChanged.AddListener(delegate
		{
			SetFilterValue(FilterType.MAX_AGE, maxAgeDropdown.value);
			FilterResults();
		});
		positionDropdown.onValueChanged.RemoveAllListeners();
		positionDropdown.onValueChanged.AddListener(delegate
		{
			SetFilterValue(FilterType.POSITION, positionDropdown.value);
			FilterResults();
		});
		minValDropdown.onValueChanged.RemoveAllListeners();
		minValDropdown.onValueChanged.AddListener(delegate
		{
			SetFilterValue(FilterType.MIN_VALUE, minValDropdown.value);
			FilterResults();
		});
		maxValDropdown.onValueChanged.RemoveAllListeners();
		maxValDropdown.onValueChanged.AddListener(delegate
		{
			SetFilterValue(FilterType.MAX_VALUE, maxValDropdown.value);
			FilterResults();
		});
		ratingDropdown.onValueChanged.RemoveAllListeners();
		ratingDropdown.onValueChanged.AddListener(delegate
		{
			SetFilterValue(FilterType.RATING, ratingDropdown.value);
			FilterResults();
		});
		potentialDropdown.onValueChanged.RemoveAllListeners();
		potentialDropdown.onValueChanged.AddListener(delegate
		{
			SetFilterValue(FilterType.POTENTIAL, potentialDropdown.value);
			FilterResults();
		});
		interestedDropdown.onValueChanged.RemoveAllListeners();
		interestedDropdown.onValueChanged.AddListener(delegate
		{
			SetFilterValue(FilterType.INTERESTED, interestedDropdown.value);
			FilterResults();
		});
	}

	private List<Dropdown.OptionData> GetAgeData()
	{
		List<Dropdown.OptionData> list = new List<Dropdown.OptionData>
		{
			new Dropdown.OptionData(Translation.GetText("any").ToUpper())
		};
		for (int i = 15; i < 36; i++)
		{
			list.Add(new Dropdown.OptionData(i.ToString()));
		}
		return list;
	}

	private List<Dropdown.OptionData> GetPositionData()
	{
		List<Dropdown.OptionData> list = new List<Dropdown.OptionData>
		{
			new Dropdown.OptionData(Translation.GetText("any").ToUpper())
		};
		foreach (int scoutedPosition in scoutedPositions)
		{
			list.Add(new Dropdown.OptionData(Helpers.GetPlayerPositionString(scoutedPosition)));
		}
		return list;
	}

	private List<Dropdown.OptionData> GetValueData()
	{
		List<Dropdown.OptionData> list = new List<Dropdown.OptionData>
		{
			new Dropdown.OptionData(Translation.GetText("any").ToUpper())
		};
		if (searchsDrawn.Count > 0)
		{
			minVal = RoundDown(minVal);
			maxVal = RoundUp(maxVal);
			double num = (maxVal - minVal) / 5.0;
			valueBands = new double[6]
			{
				minVal,
				minVal + num,
				minVal + num * 2.0,
				minVal + num * 3.0,
				minVal + num * 4.0,
				maxVal
			};
			for (int i = 0; i < 6; i++)
			{
				list.Add(new Dropdown.OptionData(Helpers.GetValueText(valueBands[i])));
			}
		}
		return list;
	}

	private List<Dropdown.OptionData> GetRatingData()
	{
		return new List<Dropdown.OptionData>
		{
			new Dropdown.OptionData(Translation.GetText("any").ToUpper()),
			new Dropdown.OptionData("40-49"),
			new Dropdown.OptionData("50-59"),
			new Dropdown.OptionData("60-69"),
			new Dropdown.OptionData("70-79"),
			new Dropdown.OptionData("80-89"),
			new Dropdown.OptionData("90-99")
		};
	}

	private double RoundDown(double val)
	{
		if (val > 10000000.0)
		{
			return Math.Floor(val / 10000000.0) * 10000000.0;
		}
		if (val > 1000000.0)
		{
			return Math.Floor(val / 1000000.0) * 1000000.0;
		}
		if (val > 100000.0)
		{
			return Math.Floor(val / 100000.0) * 100000.0;
		}
		return val;
	}

	private double RoundUp(double val)
	{
		if (val > 10000000.0)
		{
			return Math.Ceiling(val / 10000000.0) * 10000000.0;
		}
		if (val > 1000000.0)
		{
			return Math.Ceiling(val / 1000000.0) * 1000000.0;
		}
		if (val > 100000.0)
		{
			return Math.Ceiling(val / 100000.0) * 100000.0;
		}
		return val;
	}

	private void SetFilterValue(FilterType filterType, int value)
	{
		switch (filterType)
		{
		case FilterType.INTERESTED:
			FilterSettings.interested = value;
			break;
		case FilterType.MAX_AGE:
			if (value == 0)
			{
				FilterSettings.maxAge = 0;
			}
			else
			{
				FilterSettings.maxAge = value + 14;
			}
			break;
		case FilterType.MAX_VALUE:
			if (value == 0)
			{
				FilterSettings.maxValue = 0.0;
			}
			else
			{
				FilterSettings.maxValue = valueBands[value - 1];
			}
			break;
		case FilterType.MIN_AGE:
			if (value == 0)
			{
				FilterSettings.minAge = 0;
			}
			else
			{
				FilterSettings.minAge = value + 14;
			}
			break;
		case FilterType.MIN_VALUE:
			if (value == 0)
			{
				FilterSettings.minValue = 0.0;
			}
			else
			{
				FilterSettings.minValue = valueBands[value - 1];
			}
			break;
		case FilterType.POSITION:
			if (value == 0)
			{
				FilterSettings.position = 0;
			}
			else
			{
				FilterSettings.position = scoutedPositions.ElementAt(value - 1);
			}
			break;
		case FilterType.POTENTIAL:
			if (value == 0)
			{
				FilterSettings.potential = 0;
			}
			else
			{
				FilterSettings.potential = ratingBands[value - 1];
			}
			break;
		case FilterType.RATING:
			if (value == 0)
			{
				FilterSettings.rating = 0;
			}
			else
			{
				FilterSettings.rating = ratingBands[value - 1];
			}
			break;
		}
	}

	private void FilterResults()
	{
		foreach (ScoutSearch item in searchsDrawn)
		{
			foreach (ScoutSearchPlayer result in item.results)
			{
				if (result.deleted)
				{
					continue;
				}
				if (ShouldDisablePlayer(result))
				{
					if (transform.Find("ScreenContainer/ScoutAssignments/Content/ScrollView/Viewport/OptionsPanel/PlayerReport" + result.playerid) != null)
					{
						transform.Find("ScreenContainer/ScoutAssignments/Content/ScrollView/Viewport/OptionsPanel/PlayerReport" + result.playerid).gameObject.SetActive(value: false);
					}
				}
				else if (transform.Find("ScreenContainer/ScoutAssignments/Content/ScrollView/Viewport/OptionsPanel/PlayerReport" + result.playerid) != null)
				{
					transform.Find("ScreenContainer/ScoutAssignments/Content/ScrollView/Viewport/OptionsPanel/PlayerReport" + result.playerid).gameObject.SetActive(value: true);
				}
			}
		}
	}

	private bool ShouldDisablePlayer(ScoutSearchPlayer player)
	{
		int age = Player.GetAge(player.playerid);
		double num = Player.GetValue(player.playerid);
		if (FilterSettings.minAge != 0 && age < FilterSettings.minAge)
		{
			return true;
		}
		if (FilterSettings.maxAge != 0 && age > FilterSettings.maxAge)
		{
			return true;
		}
		if (FilterSettings.position != 0 && PlayerData.GetPreferredPosition(player.playerid) != FilterSettings.position)
		{
			return true;
		}
		if (FilterSettings.minValue != 0.0 && num < FilterSettings.minValue)
		{
			return true;
		}
		if (FilterSettings.maxValue != 0.0 && num > FilterSettings.maxValue)
		{
			return true;
		}
		if (FilterSettings.rating != 0)
		{
			int displayRating = Player.GetDisplayRating(player.playerid);
			if (displayRating < FilterSettings.rating || displayRating > FilterSettings.rating + 9)
			{
				return true;
			}
		}
		if (FilterSettings.potential != 0 && (player.lowerPotential < FilterSettings.potential || player.lowerPotential > FilterSettings.potential + 9))
		{
			return true;
		}
		if (FilterSettings.interested != 0)
		{
			bool flag = Player.canGoToClub(player.playerid, Game.GetCurrentUserClubID());
			if (FilterSettings.interested == 1)
			{
				if (!flag)
				{
					return true;
				}
			}
			else if (flag)
			{
				return true;
			}
		}
		return false;
	}

	private void FillPlayerInfo(int playerID, short clubID, GameObject playerClone)
	{
		int competitionType = -1;
		int competitionID = -1;
		NextFixtureInfo nextFixture = Game.instance.GetNextFixture(clubID);
		if (nextFixture != null)
		{
			competitionType = nextFixture.fixture.competitiontype;
			competitionID = nextFixture.fixture.competitionid;
		}
		playerClone.transform.Find("Image").GetComponent<Image>().color = Helpers.HexToColour("FFFFFF00");
		if (Player.IsInjured(playerID))
		{
			playerClone.transform.Find("Image").GetComponent<Image>().color = Helpers.HexToColour("FF0000FF");
			playerClone.transform.Find("Image/Text").GetComponent<Text>().text = Translation.GetText("playerinfo-injured-acronym");
		}
		else if (Player.IsSuspended(playerID, Player.GetAssociationID(playerID), competitionType, competitionID))
		{
			playerClone.transform.Find("Image").GetComponent<Image>().color = Helpers.HexToColour("FF0000FF");
			playerClone.transform.Find("Image/Text").GetComponent<Text>().text = Translation.GetText("playerinfo-suspended-acronym");
		}
		else if (PlayerData.IsLoanedToClub(playerID))
		{
			playerClone.transform.Find("Image").GetComponent<Image>().color = Helpers.HexToColour("048704FF");
			playerClone.transform.Find("Image/Text").GetComponent<Text>().text = Translation.GetText("playerinfo-on-loan-acronym");
		}
		else if (Player.IsOnTransferList(playerID))
		{
			if (clubID == Game.instance.CurrentManagedClubId)
			{
				playerClone.transform.Find("Image").GetComponent<Image>().color = Helpers.HexToColour("5271E3FF");
				playerClone.transform.Find("Image/Text").GetComponent<Text>().text = Translation.GetText("playerinfo-transfer-listed-acronym");
			}
			else if (Player.GetRating(playerID) < 90)
			{
				playerClone.transform.Find("Image").GetComponent<Image>().color = Helpers.HexToColour("5271E3FF");
				playerClone.transform.Find("Image/Text").GetComponent<Text>().text = Translation.GetText("playerinfo-transfer-listed-acronym");
			}
		}
		else if (Player.IsOnLoanList(playerID))
		{
			playerClone.transform.Find("Image").GetComponent<Image>().color = Helpers.HexToColour("5271E3FF");
			playerClone.transform.Find("Image/Text").GetComponent<Text>().text = Translation.GetText("playerinfo-loan-listed-acronym");
		}
		else
		{
			playerClone.transform.Find("Image/Text").GetComponent<Text>().color = Color.black;
			playerClone.transform.Find("Image/Text").GetComponent<Text>().text = "-";
		}
	}

	private void DrawTopBar()
	{
		FacilitiesTechTree.DrawCurrentLevelPanel(transform.Find("CurrentLevelPanel").gameObject, 4);
	}
}
