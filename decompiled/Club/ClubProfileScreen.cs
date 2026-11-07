using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ClubProfileScreen : MonoBehaviour
{
	public enum ClubTabs
	{
		Overview,
		Squad,
		Schedule,
		Transfers,
		Lineup,
		Stats
	}

	private Dictionary<ClubTabs, GameObject> tabButtons;

	private static Dictionary<ClubTabs, GameObject> tabContent;

	private ClubTabs currentTab;

	private GameObject clubProfileNavBar;

	private GameObject clubProfileContent;

	private short clubID;

	private List<int> Top3Players = new List<int>();

	public int keyPlayer;

	public bool isModal = true;

	public void InitClubProfileScreen(short clubid, bool IsModal)
	{
		clubID = clubid;
		isModal = IsModal;
		Club.SquadHealthCheck(clubid);
		if (isModal)
		{
			InitNavBar();
			FillTopInfo();
			FilllOverviewInfo();
			return;
		}
		NextFixtureInfo nextFixture = Game.instance.GetNextFixture(clubID);
		if (nextFixture != null)
		{
			if (nextFixture.fixture.homeClubId == clubID)
			{
				clubID = nextFixture.fixture.awayClubId;
			}
			else
			{
				clubID = nextFixture.fixture.homeClubId;
			}
			transform.Find("TeamInfo").gameObject.SetActive(value: true);
			transform.Find("Lineup").gameObject.SetActive(value: true);
			FillTopInfo();
			drawNextMatch(transform.Find("Lineup").gameObject);
		}
		else
		{
			transform.Find("TeamInfo").gameObject.SetActive(value: false);
			transform.Find("Lineup").gameObject.SetActive(value: false);
			transform.Find("ContentContainerWhite/ContentText").GetComponent<Text>().text = Translation.GetText("no-fixtures-available").ToUpper();
		}
	}

	private void InitNavBar()
	{
		clubProfileNavBar = transform.Find("TopPanel/ProfileNavBar").gameObject;
		clubProfileContent = transform.Find("Container").gameObject;
		tabButtons = new Dictionary<ClubTabs, GameObject>();
		tabButtons[ClubTabs.Overview] = clubProfileNavBar.transform.Find("Overview").gameObject;
		tabButtons[ClubTabs.Squad] = clubProfileNavBar.transform.Find("Squad").gameObject;
		tabButtons[ClubTabs.Schedule] = clubProfileNavBar.transform.Find("Schedule").gameObject;
		tabButtons[ClubTabs.Transfers] = clubProfileNavBar.transform.Find("Transfers").gameObject;
		tabButtons[ClubTabs.Lineup] = clubProfileNavBar.transform.Find("Scout").gameObject;
		tabButtons[ClubTabs.Stats] = clubProfileNavBar.transform.Find("Stats").gameObject;
		tabButtons[ClubTabs.Overview].GetComponentInChildren<Text>().text = Translation.GetText("overview").ToUpper();
		tabButtons[ClubTabs.Squad].GetComponentInChildren<Text>().text = Translation.GetText("squad").ToUpper();
		tabButtons[ClubTabs.Schedule].GetComponentInChildren<Text>().text = Translation.GetText("schedule").ToUpper();
		tabButtons[ClubTabs.Transfers].GetComponentInChildren<Text>().text = Translation.GetText("transfers").ToUpper();
		tabButtons[ClubTabs.Lineup].GetComponentInChildren<Text>().text = Translation.GetText("lineups").ToUpper();
		tabButtons[ClubTabs.Stats].GetComponentInChildren<Text>().text = Translation.GetText("stats").ToUpper();
		tabButtons[ClubTabs.Overview].GetComponent<Button>().onClick.AddListener(() =>
		{
			SwitchTab(ClubTabs.Overview);
		});
		tabButtons[ClubTabs.Squad].GetComponent<Button>().onClick.AddListener(() =>
		{
			SwitchTab(ClubTabs.Squad);
		});
		tabButtons[ClubTabs.Schedule].GetComponent<Button>().onClick.AddListener(() =>
		{
			SwitchTab(ClubTabs.Schedule);
		});
		tabButtons[ClubTabs.Transfers].GetComponent<Button>().onClick.AddListener(() =>
		{
			SwitchTab(ClubTabs.Transfers);
		});
		tabButtons[ClubTabs.Lineup].GetComponent<Button>().onClick.AddListener(() =>
		{
			SwitchTab(ClubTabs.Lineup);
		});
		tabButtons[ClubTabs.Stats].GetComponent<Button>().onClick.AddListener(() =>
		{
			SwitchTab(ClubTabs.Stats);
		});
		tabContent = new Dictionary<ClubTabs, GameObject>();
		tabContent[ClubTabs.Overview] = clubProfileContent.transform.Find("Overview").gameObject;
		tabContent[ClubTabs.Squad] = clubProfileContent.transform.Find("Squad").gameObject;
		tabContent[ClubTabs.Schedule] = clubProfileContent.transform.Find("Schedule").gameObject;
		tabContent[ClubTabs.Transfers] = clubProfileContent.transform.Find("Transfers").gameObject;
		tabContent[ClubTabs.Lineup] = clubProfileContent.transform.Find("Lineup").gameObject;
		tabContent[ClubTabs.Stats] = clubProfileContent.transform.Find("SquadStats").gameObject;
		tabContent[ClubTabs.Stats].GetComponent<SquadStatsController>().ClubID = clubID;
		SwitchTab(ClubTabs.Overview);
	}

	public void FillTopInfo()
	{
		GameObject gameObject = transform.Find("TeamInfo").gameObject;
		int leagueDataId = Club.GetLeagueDataId(clubID);
		short countryDataID = Club.GetCountryDataID(clubID);
		string clubName = Club.GetName(clubID);
		gameObject.transform.Find("TeamDetails/TeamName").GetComponent<Text>().text = clubName.ToUpper();
		ImageController.GetInstance().LoadImage(clubID, gameObject.transform.Find("TeamDetails/ClubBadgePanel/ClubBadgeImage").GetComponent<Image>(), ImageType.Club);
		Helpers.calculateStarRating(gameObject.transform.Find("TeamDetails/Rating10").gameObject, clubID);
		gameObject.transform.Find("Info/Country/Image").GetComponent<Image>().sprite = CountryFlagImageController.GetInstance().GetFlagImage(CountryData.GetCode(countryDataID));
		gameObject.transform.Find("Info/Country/Text").GetComponent<Text>().text = CountryData.GetName(countryDataID);
		gameObject.transform.Find("Info/League/Text").GetComponent<Text>().text = LeagueData.GetName(leagueDataId);
		ImageController.GetInstance().LoadImage(leagueDataId, gameObject.transform.Find("Info/League/Image").GetComponent<Image>(), ImageType.League);
		gameObject.transform.Find("Info/Finances/Title").GetComponent<Text>().text = Translation.GetText("finances").ToUpper();
		Helpers.calculateFiananceStarRating(gameObject.transform.Find("Info/Finances/Rating10").gameObject, clubID);
		if (isModal)
		{
			Button component = gameObject.transform.Find("Buttons/Takeover").GetComponent<Button>();
			int takeoverPrice = Club.GetTakeoverPrice(clubID);
			gameObject.transform.Find("Buttons/Takeover/ButtonText").GetComponent<Text>().text = Translation.GetText("take-over").ToUpper() + "\n" + takeoverPrice;
			component.gameObject.SetActive(value: false);
			component.onClick.RemoveAllListeners();
			if (Game.instance.ChosenCountries.Contains(LeagueData.GetAssocID(Club.GetLeagueDataId(clubID))) && !ClubList.GetAllUnPlayableClubsForAssoc(Club.GetAssociationID(clubID)).Contains(clubID))
			{
				if (Game.instance.ManagedClubProfile.CareerHistory.ContainsKey(clubID))
				{
					return;
				}
				component.gameObject.SetActive(value: true);
				component.onClick.AddListener(() =>
				{
					if (CustomerDataController.Instance.GetUserCurCreditAmount() >= takeoverPrice)
					{
						ModalController.Instance().ShowConfirmModal(Translation.GetText("confirm"), Translation.TemplateReplace(Translation.GetText("take-control-confirm"), new Dictionary<string, string> { { "Club Name", clubName } }), () =>
						{
							UI.GetInstance().ShowGenericLoading();
							Dictionary<string, object> callBackedVars = new Dictionary<string, object>
							{
								{
									"clubid",
									clubID.ToString()
								},
								{
									"clubdataid",
									Club.GetClubDataID(Game.instance.CurrentManagedClubId).ToString()
								}
							};
							CustomerDataController.Instance.ConsumeItem(0, takeoverPrice, UI.GetInstance().ClubTakeoverCallBack, callBackedVars);
						});
					}
					else
					{
						ModalController.Instance().ShowConfirmModal(Translation.GetText("not-enough-credits"), Translation.GetText("not-enough-credits"), () =>
						{
							UI.GetInstance().ChangeScreen(8, ShopScreen.ScreenTypes.Credits);
							ModalController.Instance().CloseAllModals();
						}, Translation.GetText("go-to-shop"));
					}
				});
			}
			else
			{
				gameObject.transform.Find("Buttons").gameObject.SetActive(value: false);
			}
		}
		else
		{
			gameObject.transform.Find("Buttons").gameObject.SetActive(value: false);
			short currentUserClubID = Game.GetCurrentUserClubID();
			Fixture fixture = Game.instance.GetNextFixture(currentUserClubID).fixture;
			string text = "";
			text = Helpers.FormatShortDateForFixture(fixture.Time) + " | ";
			text = ((fixture.homeClubId != currentUserClubID) ? (text + "[" + Translation.GetText("next-fixture-away") + "]") : (text + "[" + Translation.GetText("next-fixture-home") + "]"));
			text = text + " | " + Helpers.getCompetitionName(fixture.competitiontype, fixture.competitionid, Club.GetAssociationID(fixture.homeClubId)) + " ";
			gameObject.transform.Find("TeamDetails/FixtureInfo/FixtureInfoText").GetComponent<Text>().text = text;
		}
	}

	public void FilllOverviewInfo()
	{
		GameObject gameObject = transform.Find("Container/Overview/SquadOverview").gameObject;
		GameObject gameObject2 = transform.Find("Container/Overview/CompetitionOverview").gameObject;
		GameObject gameObject3 = transform.Find("Container/Overview/StadiumOverview").gameObject;
		int stadiumDataID = Club.GetStadiumDataID(clubID);
		gameObject3.transform.Find("Header/Text").GetComponent<Text>().text = Translation.GetText("stadium").ToUpper();
		gameObject3.transform.Find("List/Item1/Title").GetComponent<Text>().text = Translation.GetText("stadium").ToUpper();
		gameObject3.transform.Find("List/Item1/Value").GetComponent<Text>().text = StadiumData.GetName(stadiumDataID);
		gameObject3.transform.Find("List/Item2/Title").GetComponent<Text>().text = Translation.GetText("facilities-name-stadium-capacity").ToUpper();
		gameObject3.transform.Find("List/Item2/Value").GetComponent<Text>().text = Util.NumberFormatter(StadiumData.GetSize(stadiumDataID));
		gameObject3.transform.Find("List/Item3/Title").GetComponent<Text>().text = Translation.GetText("fanbase").ToUpper();
		gameObject3.transform.Find("List/Item3/Value").GetComponent<Text>().text = Util.NumberFormatter(Club.GetFanbaseSize(clubID));
		List<int> list = PlayerSquad.AllPlayers(clubID);
		List<Top3Players> top3 = new List<Top3Players>();
		top3.Clear();
		int num = 0;
		int num2 = 0;
		int num3 = 0;
		int num4 = 0;
		float num5 = 0f;
		int num6 = 0;
		for (int i = 0; i < list.Count; i++)
		{
			num = list[i];
			if (Player.GetClubIDLoanParent(num) != clubID && Player.GetFirstTeam(num))
			{
				num2++;
				num6 = Player.GetDisplayRating(num);
				top3.Add(new Top3Players
				{
					ID = num,
					Value = num6
				});
				num3 += Player.GetAge(num);
				num4 += num6;
				num5 += Player.GetValue(num);
			}
		}
		gameObject.transform.Find("Header/Text").GetComponent<Text>().text = Translation.GetText("first-team").ToUpper();
		if (num2 > 0)
		{
			gameObject.transform.Find("List/Item1/Title").GetComponent<Text>().text = Translation.GetText("squad-size").ToUpper();
			gameObject.transform.Find("List/Item1/Value").GetComponent<Text>().text = num2.ToString();
			gameObject.transform.Find("List/Item2/Title").GetComponent<Text>().text = Translation.GetText("average-age").ToUpper();
			gameObject.transform.Find("List/Item2/Value").GetComponent<Text>().text = (num3 / num2).ToString();
			gameObject.transform.Find("List/Item3/Title").GetComponent<Text>().text = Translation.GetText("average-rating").ToUpper();
			Helpers.SetPlayerRatingAndColour(gameObject.transform.Find("List/Item3/Value/RatingHexagon").gameObject, num4 / num2);
			gameObject.transform.Find("List/Item4/Title").GetComponent<Text>().text = Translation.GetText("average-value").ToUpper();
			gameObject.transform.Find("List/Item4/Value").GetComponent<Text>().text = Helpers.GetValueText(num5 / (float)num2);
			gameObject.transform.Find("List/Item5/Title").GetComponent<Text>().text = Translation.GetText("total-value").ToUpper();
			gameObject.transform.Find("List/Item5/Value").GetComponent<Text>().text = Helpers.GetValueText(num5);
		}
		else
		{
			gameObject.transform.Find("List/Item1/Value").GetComponent<Text>().text = num2.ToString();
			gameObject.transform.Find("List/Item2/Title").GetComponent<Text>().text = Translation.GetText("average-age").ToUpper();
			gameObject.transform.Find("List/Item2/Value").GetComponent<Text>().text = "-";
			gameObject.transform.Find("List/Item3/Title").GetComponent<Text>().text = Translation.GetText("average-rating").ToUpper();
			gameObject.transform.Find("List/Item3/RatingHexagon/HexagonOuter/Hexagon/Number").GetComponent<Text>().text = "-";
			gameObject.transform.Find("List/Item4/Title").GetComponent<Text>().text = Translation.GetText("average-value").ToUpper();
			gameObject.transform.Find("List/Item4/Value").GetComponent<Text>().text = "-";
			gameObject.transform.Find("List/Item5/Title").GetComponent<Text>().text = Translation.GetText("total-value").ToUpper();
			gameObject.transform.Find("List/Item5/Value").GetComponent<Text>().text = "-";
		}
		gameObject2.transform.Find("Header/Text").GetComponent<Text>().text = Translation.GetText("top-players").ToUpper();
		if (Enumerable.Count(top3) != 0)
		{
			top3 = top3.OrderBy((Top3Players g) => g.Value).Reverse().ToList();
			keyPlayer = top3[0].ID;
			gameObject2.transform.Find("PlayerCardsHolder/PlayerCardSmall1").GetComponent<PlayerCard>().GetPlayerImage(top3[0].ID);
			gameObject2.transform.Find("PlayerCardsHolder/PlayerCardSmall2").GetComponent<PlayerCard>().GetPlayerImage(top3[1].ID);
			gameObject2.transform.Find("PlayerCardsHolder/PlayerCardSmall3").GetComponent<PlayerCard>().GetPlayerImage(top3[2].ID);
			gameObject2.transform.Find("PlayerCardsHolder/PlayerCardSmall1").GetComponent<PlayerCard>().SetType(PlayerCardType.Small);
			gameObject2.transform.Find("PlayerCardsHolder/PlayerCardSmall2").GetComponent<PlayerCard>().SetType(PlayerCardType.Small);
			gameObject2.transform.Find("PlayerCardsHolder/PlayerCardSmall3").GetComponent<PlayerCard>().SetType(PlayerCardType.Small);
			gameObject2.transform.Find("PlayerCardsHolder/PlayerCardSmall1").GetComponent<PlayerCard>().FillPlayerDetails(top3[0].ID, clubID);
			gameObject2.transform.Find("PlayerCardsHolder/PlayerCardSmall2").GetComponent<PlayerCard>().FillPlayerDetails(top3[1].ID, clubID);
			gameObject2.transform.Find("PlayerCardsHolder/PlayerCardSmall3").GetComponent<PlayerCard>().FillPlayerDetails(top3[2].ID, clubID);
			gameObject2.transform.Find("PlayerCardsHolder/PlayerCardSmall1").GetComponent<Button>().onClick.RemoveAllListeners();
			gameObject2.transform.Find("PlayerCardsHolder/PlayerCardSmall1").GetComponent<Button>().onClick.AddListener(() =>
			{
				UI.GetInstance().ShowPlayerPage(top3[0].ID);
			});
			gameObject2.transform.Find("PlayerCardsHolder/PlayerCardSmall2").GetComponent<Button>().onClick.RemoveAllListeners();
			gameObject2.transform.Find("PlayerCardsHolder/PlayerCardSmall2").GetComponent<Button>().onClick.AddListener(() =>
			{
				UI.GetInstance().ShowPlayerPage(top3[1].ID);
			});
			gameObject2.transform.Find("PlayerCardsHolder/PlayerCardSmall3").GetComponent<Button>().onClick.RemoveAllListeners();
			gameObject2.transform.Find("PlayerCardsHolder/PlayerCardSmall3").GetComponent<Button>().onClick.AddListener(() =>
			{
				UI.GetInstance().ShowPlayerPage(top3[2].ID);
			});
		}
	}

	public void drawNextMatch(GameObject LineUp)
	{
		Transform transform = LineUp.transform.Find("HomeTactics/TacticPitch/Formation");
		byte b = Game.instance.g_ClubData[clubID].TeamFormation;
		byte averageRating = Club.GetAverageRating(Game.GetCurrentUserClubID());
		int num = Club.GetAverageRating(clubID) - averageRating;
		if (num > 5)
		{
			b = Team.GetCounterFormation(b, attacking: true);
		}
		if (num <= -5)
		{
			b = Team.GetCounterFormation(b, attacking: false);
		}
		Team team = new Team
		{
			clubid = clubID
		};
		Club.SquadHealthCheck(clubID);
		Team.PickTeam(team, b, 0, 2, Club.GetLeagueDataId(clubID), justFirstTeam: true);
		int[] lineup = team.lineup;
		if (isModal)
		{
			_ = base.transform.parent.gameObject;
		}
		else
		{
			GameObject.Find("GameCanvas");
		}
		float num2 = transform.GetComponent<RectTransform>().rect.height / 7f;
		float num3 = transform.GetComponent<RectTransform>().rect.width / 9f;
		float num4 = num3 / 2f;
		int num5 = 0;
		GameObject gameObject = null;
		int num6 = 0;
		int num7 = 0;
		while (num6 < 11)
		{
			int playerID = lineup[num6];
			FormationPosData obj = FormationData.g_formations[b][num7];
			num5 = TacticsController.SwitchPosXFormationData(obj.pos[0]);
			float num8 = obj.pos[1];
			if (num6 > 0)
			{
				num8 -= 0.5f;
			}
			RectTransform component = transform.transform.GetChild(num6).GetComponent<RectTransform>();
			component.anchorMin = new Vector2(0f, 0.5f);
			component.anchorMax = new Vector2(0f, 0.5f);
			component.anchoredPosition = new Vector2(num3 * num8 + num4, num2 * (float)num5);
			gameObject = transform.transform.GetChild(num6).gameObject;
			gameObject.transform.Find("PlayerNameBox/Text").GetComponent<Text>().text = Player.GetSurname(playerID, checkDuplicate: true);
			Helpers.SetPlayerRatingAndColour(gameObject.transform.Find("RatingHexagon").gameObject, Player.GetDisplayRating(playerID));
			gameObject.GetComponent<Button>().onClick.RemoveAllListeners();
			gameObject.GetComponent<Button>().onClick.AddListener(() =>
			{
				UI.GetInstance().ShowPlayerPage(playerID);
			});
			Color color = Helpers.HexToColour(Game.instance.g_ClubData[clubID].HomeColour1);
			gameObject.transform.Find("PlayerShirt").GetComponent<Image>().color = ((num6 == 0) ? FixtureKitHelper.GetHomeKeeperColor(color, color) : color);
			ClubKitStyle kitStyle = Club.GetKitStyle(Game.instance.g_ClubData[clubID].KitID, clubID);
			if (kitStyle != ClubKitStyle.Block)
			{
				Transform transform2 = null;
				switch (kitStyle)
				{
				case ClubKitStyle.HalfHalf:
					transform2 = gameObject.transform.Find("PlayerShirt/Halves");
					break;
				case ClubKitStyle.Hoops:
					transform2 = gameObject.transform.Find("PlayerShirt/Hoops");
					break;
				case ClubKitStyle.Sleeves:
					transform2 = gameObject.transform.Find("PlayerShirt/Sleeves");
					break;
				case ClubKitStyle.Stripes:
					transform2 = gameObject.transform.Find("PlayerShirt/Stripes");
					break;
				}
				if (transform2 != null && num6 > 0)
				{
					transform2.GetComponent<Image>().color = Helpers.HexToColour(Game.instance.g_ClubData[clubID].HomeColour2);
					transform2.gameObject.SetActive(value: true);
				}
			}
			num6++;
			num7++;
		}
		Transform transform3 = LineUp.transform.Find("Overview/KeyPlayers");
		Transform transform4 = LineUp.transform.Find("Overview/SquadInfo");
		transform3.Find("Header/Text").GetComponent<Text>().text = Translation.GetText("key-player").ToUpper();
		transform4.Find("Header/Text").GetComponent<Text>().text = Translation.GetText("information").ToUpper();
		transform4.Find("Injuries/Title").GetComponent<Text>().text = Translation.GetText("injuries").ToUpper();
		transform4.Find("suspensions/Title").GetComponent<Text>().text = Translation.GetText("suspensions").ToUpper();
		List<int> list = PlayerSquad.AllPlayers(clubID);
		List<Top3Players> top3 = new List<Top3Players>();
		top3.Clear();
		int num9 = 0;
		int num10 = 0;
		int competitionType = -1;
		int competitionID = -1;
		int num11 = 0;
		int num12 = 0;
		NextFixtureInfo nextFixture = Game.instance.GetNextFixture(clubID);
		if (nextFixture != null)
		{
			competitionType = nextFixture.fixture.competitiontype;
			competitionID = nextFixture.fixture.competitionid;
		}
		for (int num13 = 2; num13 < transform4.Find("Injuries").transform.childCount; num13++)
		{
			UnityEngine.Object.Destroy(transform4.Find("Injuries").transform.GetChild(num13).gameObject);
		}
		for (int num14 = 2; num14 < transform4.Find("suspensions").transform.childCount; num14++)
		{
			UnityEngine.Object.Destroy(transform4.Find("suspensions").transform.GetChild(num14).gameObject);
		}
		for (int num15 = 0; num15 < list.Count; num15++)
		{
			num9 = list[num15];
			if (Player.GetClubIDLoanParent(num9) != clubID && Player.GetFirstTeam(num9))
			{
				num10 = Player.GetDisplayRating(num9);
				top3.Add(new Top3Players
				{
					ID = num9,
					Value = num10
				});
				if (Player.IsInjured(num9))
				{
					GameObject gameObject2 = UnityEngine.Object.Instantiate(transform4.Find("Injuries/TextHelper").gameObject, transform4.Find("Injuries").transform, worldPositionStays: false);
					gameObject2.SetActive(value: true);
					gameObject2.transform.Find("Text").GetComponent<Text>().text = Player.GetShortName(num9);
					num11++;
				}
				if (Player.IsSuspended(num9, Player.GetAssociationID(num9), competitionType, competitionID))
				{
					GameObject gameObject3 = UnityEngine.Object.Instantiate(transform4.Find("suspensions/TextHelper").gameObject, transform4.Find("suspensions").transform, worldPositionStays: false);
					gameObject3.SetActive(value: true);
					gameObject3.transform.Find("Text").GetComponent<Text>().text = Player.GetShortName(num9);
					num12++;
				}
			}
		}
		if (Enumerable.Count(top3) != 0)
		{
			top3 = top3.OrderBy((Top3Players g) => g.Value).Reverse().ToList();
			keyPlayer = top3[0].ID;
			transform3.transform.Find("PlayerCardSmall").GetComponent<Button>().onClick.RemoveAllListeners();
			transform3.transform.Find("PlayerCardSmall").GetComponent<Button>().onClick.AddListener(() =>
			{
				UI.GetInstance().ShowPlayerPage(top3[0].ID);
			});
			transform3.transform.Find("PlayerCardSmall").GetComponent<PlayerCard>().GetPlayerImage(top3[0].ID);
			transform3.transform.Find("PlayerCardSmall").GetComponent<PlayerCard>().SetType(PlayerCardType.Small);
			transform3.transform.Find("PlayerCardSmall").GetComponent<PlayerCard>().FillPlayerDetails(top3[0].ID, clubID);
		}
		if (num11 == 0)
		{
			GameObject gameObject4 = UnityEngine.Object.Instantiate(transform4.Find("Injuries/TextHelper").gameObject, transform4.Find("Injuries").transform, worldPositionStays: false);
			gameObject4.SetActive(value: true);
			gameObject4.transform.Find("Text").GetComponent<Text>().text = Translation.GetText("None").ToUpper();
		}
		if (num12 == 0)
		{
			GameObject gameObject5 = UnityEngine.Object.Instantiate(transform4.Find("suspensions/TextHelper").gameObject, transform4.Find("suspensions").transform, worldPositionStays: false);
			gameObject5.SetActive(value: true);
			gameObject5.transform.Find("Text").GetComponent<Text>().text = Translation.GetText("None").ToUpper();
		}
	}

	private int SwitchPosXFormationData(int posX)
	{
		return posX switch
		{
			6 => 0, 
			5 => 1, 
			4 => 2, 
			3 => 3, 
			2 => 4, 
			1 => 5, 
			0 => 6, 
			_ => 0, 
		};
	}

	public void SwitchTab(ClubTabs selectedTab)
	{
		foreach (ClubTabs value in Enum.GetValues(typeof(ClubTabs)))
		{
			if (selectedTab == value)
			{
				currentTab = value;
				if (value != ClubTabs.Overview && tabButtons != null)
				{
					clubProfileNavBar.GetComponent<TabsBlue>().SelectTab(tabButtons[value]);
				}
				if (value == ClubTabs.Overview && tabButtons != null)
				{
					clubProfileNavBar.GetComponent<TabsBlue>().SelectTab(tabButtons[ClubTabs.Overview]);
				}
				tabContent[value].SetActive(value: true);
			}
			else if (tabContent.ContainsKey(value))
			{
				if (tabButtons != null)
				{
					clubProfileNavBar.GetComponent<TabsBlue>().UnselectTab(tabButtons[value]);
				}
				tabContent[value].SetActive(value: false);
			}
		}
		switch (selectedTab)
		{
		case ClubTabs.Squad:
			tabContent[ClubTabs.Squad].GetComponent<Squad>().InitUnmanagedSquadScreen();
			tabContent[ClubTabs.Squad].GetComponent<Squad>().DrawSquad(clubID);
			break;
		case ClubTabs.Schedule:
			tabContent[ClubTabs.Schedule].GetComponent<ScheduleScreen>().isModal = true;
			tabContent[ClubTabs.Schedule].GetComponent<ScheduleScreen>().drawScheduleScreen(clubID);
			break;
		case ClubTabs.Lineup:
			drawNextMatch(transform.Find("Container/Lineup").gameObject);
			break;
		case ClubTabs.Stats:
			tabContent[ClubTabs.Stats].GetComponent<SquadStatsController>().ClubID = clubID;
			break;
		case ClubTabs.Transfers:
			break;
		}
	}
}
