using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ManagerStatsScreen : MonoBehaviour
{
	private enum TabView
	{
		Stats,
		Trophies,
		Records,
		Transfers,
		JobOffers
	}

	private GameObject allSeasonsStatsPanel;

	private GameObject allSeasonsStatRow;

	private GameObject nostatsRow;

	private Transform managerBreakdown;

	private Transform managerStatsContainer;

	private Transform currentTeamInfo;

	private Transform trophyContainer;

	private Transform recordsContainer;

	private Transform transfersContainer;

	private Transform jobOffersContainer;

	private GameObject jobOfferRowContainer;

	private GameObject jobOffersRow;

	private GameObject noJobsRow;

	private Button statsTab;

	private Button trophiesTab;

	private Button recordsTab;

	private Button transfersTab;

	private Button jobOffersTab;

	private Sprite ratingFull;

	private Sprite ratingEmpty;

	private Sprite ratingHalf;

	private bool initialised;

	private void Start()
	{
		managerBreakdown = transform.Find("ScreenContainer/ManagerBreakdown");
		statsTab = transform.Find("ScreenContainer/NavBar/Stats").GetComponent<Button>();
		trophiesTab = transform.Find("ScreenContainer/NavBar/Trophies").GetComponent<Button>();
		recordsTab = transform.Find("ScreenContainer/NavBar/Records").GetComponent<Button>();
		statsTab.onClick.AddListener(() =>
		{
			ChangeView(TabView.Stats);
		});
		trophiesTab.onClick.AddListener(() =>
		{
			ChangeView(TabView.Trophies);
		});
		recordsTab.onClick.AddListener(() =>
		{
			ChangeView(TabView.Records);
		});
		managerStatsContainer = transform.Find("ScreenContainer/ContentContainer/ManagerStats");
		trophyContainer = transform.Find("ScreenContainer/ContentContainer/TropheyCabinet");
		recordsContainer = transform.Find("ScreenContainer/ContentContainer/Records");
		currentTeamInfo = managerStatsContainer.transform.Find("TeamInfo");
		allSeasonsStatsPanel = transform.Find("ScreenContainer/ContentContainer/ManagerStats/ManagerHistory/Viewport/ManagerHistoryPanel").gameObject;
		allSeasonsStatRow = allSeasonsStatsPanel.transform.Find("TableRow").gameObject;
		nostatsRow = allSeasonsStatsPanel.transform.Find("NoStats").gameObject;
		ratingFull = Resources.Load<Sprite>("ManagerTraits/rating_full");
		ratingEmpty = Resources.Load<Sprite>("ManagerTraits/rating_empty");
		ratingHalf = Resources.Load<Sprite>("ManagerTraits/rating_half");
		InitStatsScreen();
		DrawRecords();
		InitTranslations();
	}

	private void OnEnable()
	{
		if (initialised)
		{
			UpdateManagerStats();
			UpdateSeasonHistory();
			DrawRecords();
		}
	}

	private void ChangeView(TabView view)
	{
		managerStatsContainer.gameObject.SetActive(view == TabView.Stats);
		trophyContainer.gameObject.SetActive(view == TabView.Trophies);
		recordsContainer.gameObject.SetActive(view == TabView.Records);
	}

	private void InitStatsScreen()
	{
		SeasonHistory currentSeasonHistory = Game.instance.ManagedClubProfile.GetCurrentSeasonHistory();
		bool flag = currentSeasonHistory.GamesPlayed > 0;
		short currentClubID = Game.instance.ManagedClubProfile.CurrentClubID;
		string currentSeasonID = Game.instance.ManagedClubProfile.CurrentSeasonID;
		Helpers.SetPlayerRatingAndColour(managerBreakdown.transform.Find("List/ManagerRating/Value/RatingHexagon").gameObject, (float)Game.instance.ManagedClubProfile.GetManagerRating());
		managerBreakdown.transform.Find("WinPercentage/Win").GetComponent<Image>().fillAmount = (flag ? (currentSeasonHistory.WinPercentage / 100f) : 1f);
		managerBreakdown.transform.Find("WinPercentage/Draw").GetComponent<Image>().fillAmount = (flag ? (currentSeasonHistory.WinPercentage / 100f + currentSeasonHistory.DrawPercentage / 100f) : 0f);
		managerBreakdown.transform.Find("WinPercentage/Amount").GetComponent<Text>().text = currentSeasonHistory.WinPercentage.ToString("0") + "%";
		managerBreakdown.transform.Find("List/GamesManaged/Value").GetComponent<Text>().text = currentSeasonHistory.GamesPlayed.ToString();
		managerBreakdown.transform.Find("List/Wins/Value").GetComponent<Text>().text = currentSeasonHistory.Wins.ToString();
		managerBreakdown.transform.Find("List/Draws/Value").GetComponent<Text>().text = currentSeasonHistory.Draws.ToString();
		managerBreakdown.transform.Find("List/Losses/Value").GetComponent<Text>().text = currentSeasonHistory.Loses.ToString();
		managerBreakdown.transform.Find("List/TrophiesWon/Value").GetComponent<Text>().text = Game.instance.ManagedClubProfile.GetClubSeasonTrophyCount(currentClubID, currentSeasonID).ToString();
		currentTeamInfo.transform.Find("TeamDetails/TeamName").GetComponent<Text>().text = Club.GetName(Game.GetCurrentUserClubID()).ToUpper();
		Helpers.calculateStarRating(currentTeamInfo.transform.Find("TeamDetails/Rating10").gameObject, Game.GetCurrentUserClubID());
		ImageController.GetInstance().LoadImage(Game.GetCurrentUserClubID(), currentTeamInfo.transform.Find("TeamDetails/ClubBadgePanel/ClubBadgeImage").GetComponent<Image>(), ImageType.Club);
		currentTeamInfo.transform.Find("Info/Country/Image").GetComponent<Image>().sprite = CountryFlagImageController.GetInstance().GetFlagImage(CountryData.GetCode(Club.GetCountryDataID(Game.GetCurrentUserClubID())));
		currentTeamInfo.transform.Find("Info/Country/Text").GetComponent<Text>().text = CountryData.GetName(Club.GetCountryDataID(Game.GetCurrentUserClubID())).ToUpper();
		currentTeamInfo.transform.Find("Info/League/Text").GetComponent<Text>().text = LeagueData.GetName(Club.GetLeagueDataId(Game.GetCurrentUserClubID())).ToUpper();
		currentTeamInfo.transform.Find("Info/League/Image").GetComponent<Image>().sprite = CountryFlagImageController.GetInstance().GetFlagImage(CountryData.GetCode(Club.GetCountryDataID(Game.GetCurrentUserClubID())));
		Helpers.calculateFiananceStarRating(currentTeamInfo.transform.Find("Info/Finances/Rating10").gameObject, Game.GetCurrentUserClubID());
		InitManagerHistory();
		initialised = true;
	}

	private void InitTranslations()
	{
		statsTab.transform.Find("text").GetComponent<Text>().text = Translation.GetText("stats").ToUpper();
		trophiesTab.transform.Find("text").GetComponent<Text>().text = Translation.GetText("trophies").ToUpper();
		recordsTab.transform.Find("text").GetComponent<Text>().text = Translation.GetText("records").ToUpper();
		currentTeamInfo.transform.Find("Info/Finances/Title").GetComponent<Text>().text = Translation.GetText("finances").ToUpper();
		managerBreakdown.transform.Find("List/ManagerRating/Title").GetComponent<Text>().text = Translation.GetText("Squad-Rating").ToUpper();
		managerBreakdown.transform.Find("List/GamesManaged/Title").GetComponent<Text>().text = Translation.GetText("glossary-P").ToUpper();
		managerBreakdown.transform.Find("List/Wins/Title").GetComponent<Text>().text = Translation.GetText("glossary-W").ToUpper();
		managerBreakdown.transform.Find("List/Draws/Title").GetComponent<Text>().text = Translation.GetText("glossary-D").ToUpper();
		managerBreakdown.transform.Find("List/Losses/Title").GetComponent<Text>().text = Translation.GetText("glossary-L").ToUpper();
		managerBreakdown.transform.Find("List/TrophiesWon/Title").GetComponent<Text>().text = Translation.GetText("trophies").ToUpper();
		managerBreakdown.transform.Find("WinPercentage/WinPercentageTitle/ContentTextBlue").GetComponent<Text>().text = Translation.GetText("win-percentage").ToUpper();
		managerBreakdown.transform.Find("Header/Text").GetComponent<Text>().text = Translation.GetText("manager-breakdown").ToUpper();
		allSeasonsStatsPanel.transform.Find("DateRow/ContentHeader/SeasonHeader").GetComponent<Text>().text = Translation.GetText("gn-Season").ToUpper();
		allSeasonsStatsPanel.transform.Find("DateRow/ContentHeader/ClubHeader").GetComponent<Text>().text = Translation.GetText("Squad-Club").ToUpper();
		allSeasonsStatsPanel.transform.Find("DateRow/ContentHeader/LocationHeader").GetComponent<Text>().text = Translation.GetText("country").ToUpper();
		allSeasonsStatsPanel.transform.Find("DateRow/ContentHeader/WinHeader").GetComponent<Text>().text = Translation.GetText("w").ToUpper();
		allSeasonsStatsPanel.transform.Find("DateRow/ContentHeader/DrawsHeader").GetComponent<Text>().text = Translation.GetText("d").ToUpper();
		allSeasonsStatsPanel.transform.Find("DateRow/ContentHeader/LossHeader").GetComponent<Text>().text = Translation.GetText("l").ToUpper();
		allSeasonsStatsPanel.transform.Find("DateRow/ContentHeader/VerdictHeader").GetComponent<Text>().text = Translation.GetText("win-percent-header").ToUpper();
		allSeasonsStatsPanel.transform.Find("DateRow/ContentHeader/TrophiesHeader").GetComponent<Text>().text = Translation.GetText("trophies").ToUpper();
		recordsContainer.transform.Find("TopPanel/Record1/RecordHeader/Text").GetComponent<Text>().text = Translation.GetText("record-win").ToUpper();
		recordsContainer.transform.Find("TopPanel/Record2/RecordHeader/Text").GetComponent<Text>().text = Translation.GetText("record-loss").ToUpper();
		recordsContainer.transform.Find("TopPanel/Record3/RecordHeader/Text").GetComponent<Text>().text = Translation.GetText("record-attendance").ToUpper();
		recordsContainer.transform.Find("MiddlePanel/Record1/RecordHeader/Text").GetComponent<Text>().text = Translation.GetText("record-signing").ToUpper();
		recordsContainer.transform.Find("MiddlePanel/Record2/RecordHeader/Text").GetComponent<Text>().text = Translation.GetText("record-fee-received").ToUpper();
		recordsContainer.transform.Find("BottomPanel/Record1/RecordHeader/Text").GetComponent<Text>().text = Translation.GetText("record-win-streak").ToUpper();
		recordsContainer.transform.Find("BottomPanel/Record2/RecordHeader/Text").GetComponent<Text>().text = Translation.GetText("record-games-unbeaten").ToUpper();
		trophyContainer.transform.Find("ScreenContainer/ContentContainerWhite/ContentHeader/Text").GetComponent<Text>().text = Translation.GetText("trophy-cabinet").ToUpper();
		trophyContainer.transform.Find("ScreenContainer/ContentContainerWhite/TrophyHistory/HeaderRow/ContentHeader/SeasonHeader").GetComponent<Text>().text = Translation.GetText("gn-Season").ToUpper();
		trophyContainer.transform.Find("ScreenContainer/ContentContainerWhite/TrophyHistory/HeaderRow/ContentHeader/ClubHeader").GetComponent<Text>().text = Translation.GetText("Squad-Club").ToUpper();
		trophyContainer.transform.Find("ScreenContainer/ContentContainerWhite/TrophyHistory/HeaderRow/ContentHeader/LocationHeader").GetComponent<Text>().text = Translation.GetText("country").ToUpper();
		trophyContainer.transform.Find("ScreenContainer/ContentContainerWhite/TrophyHistory/HeaderRow/ContentHeader/TrophiesHeader").GetComponent<Text>().text = Translation.GetText("trophies").ToUpper();
		trophyContainer.transform.Find("ScreenContainer/ContentContainerWhite/TrophyHistory/Viewport/TrophyHistoryPanel/NoStats/Text").GetComponent<Text>().text = Translation.GetText("None");
	}

	private void InitManagerHistory()
	{
		foreach (KeyValuePair<short, CareerEntry> item in Game.instance.ManagedClubProfile.CareerHistory)
		{
			foreach (KeyValuePair<string, SeasonHistory> item2 in item.Value.seasonHistory)
			{
				short key = item.Key;
				_ = item2.Value;
				string key2 = item2.Key;
				GameObject gameObject = Object.Instantiate(allSeasonsStatRow, allSeasonsStatRow.transform.parent);
				gameObject.transform.Find("Season").GetComponent<Text>().text = key2;
				gameObject.transform.Find("Club").GetComponent<Text>().text = Club.GetName(key);
				gameObject.transform.Find("Location").GetComponent<Text>().text = CountryData.GetName(Club.GetCountryDataID(key));
				gameObject.transform.Find("Win").GetComponent<Text>().text = item2.Value.Wins.ToString();
				gameObject.transform.Find("Draws").GetComponent<Text>().text = item2.Value.Draws.ToString();
				gameObject.transform.Find("Loss").GetComponent<Text>().text = item2.Value.Loses.ToString();
				gameObject.transform.Find("Verdict").GetComponent<Text>().text = item2.Value.WinPercentage.ToString("0") + "%";
				gameObject.transform.Find("Trophies").GetComponent<Text>().text = Game.instance.ManagedClubProfile.GetClubSeasonTrophyCount(item.Key, key2).ToString();
				gameObject.name = key + key2.Replace("/", "");
				gameObject.SetActive(value: true);
			}
		}
	}

	private void UpdateManagerStats()
	{
		SeasonHistory currentSeasonHistory = Game.instance.ManagedClubProfile.GetCurrentSeasonHistory();
		bool flag = currentSeasonHistory.GamesPlayed > 0;
		short currentClubID = Game.instance.ManagedClubProfile.CurrentClubID;
		string currentSeasonID = Game.instance.ManagedClubProfile.CurrentSeasonID;
		managerBreakdown.transform.Find("WinPercentage/Win").GetComponent<Image>().fillAmount = (flag ? (currentSeasonHistory.WinPercentage / 100f) : 1f);
		managerBreakdown.transform.Find("WinPercentage/Draw").GetComponent<Image>().fillAmount = (flag ? (currentSeasonHistory.WinPercentage / 100f + currentSeasonHistory.DrawPercentage / 100f) : 0f);
		managerBreakdown.transform.Find("WinPercentage/Amount").GetComponent<Text>().text = currentSeasonHistory.WinPercentage.ToString("0") + "%";
		Helpers.SetPlayerRatingAndColour(managerBreakdown.transform.Find("List/ManagerRating/Value/RatingHexagon").gameObject, (float)Game.instance.ManagedClubProfile.GetManagerRating());
		managerBreakdown.transform.Find("List/GamesManaged/Value").GetComponent<Text>().text = currentSeasonHistory.GamesPlayed.ToString();
		managerBreakdown.transform.Find("List/Wins/Value").GetComponent<Text>().text = currentSeasonHistory.Wins.ToString();
		managerBreakdown.transform.Find("List/Draws/Value").GetComponent<Text>().text = currentSeasonHistory.Draws.ToString();
		managerBreakdown.transform.Find("List/Losses/Value").GetComponent<Text>().text = currentSeasonHistory.Loses.ToString();
		managerBreakdown.transform.Find("List/TrophiesWon/Value").GetComponent<Text>().text = Game.instance.ManagedClubProfile.GetClubSeasonTrophyCount(currentClubID, currentSeasonID).ToString();
		currentTeamInfo.transform.Find("TeamDetails/TeamName").GetComponent<Text>().text = Club.GetName(Game.GetCurrentUserClubID());
		Helpers.calculateStarRating(currentTeamInfo.transform.Find("TeamDetails/Rating10").gameObject, Game.GetCurrentUserClubID());
		ImageController.GetInstance().LoadImage(Game.GetCurrentUserClubID(), currentTeamInfo.transform.Find("TeamDetails/ClubBadgePanel/ClubBadgeImage").GetComponent<Image>(), ImageType.Club);
		currentTeamInfo.transform.Find("Info/Country/Image").GetComponent<Image>().sprite = CountryFlagImageController.GetInstance().GetFlagImage(CountryData.GetCode(Club.GetCountryDataID(Game.GetCurrentUserClubID())));
		currentTeamInfo.transform.Find("Info/Country/Text").GetComponent<Text>().text = CountryData.GetName(Club.GetCountryDataID(Game.GetCurrentUserClubID()));
		currentTeamInfo.transform.Find("Info/League/Text").GetComponent<Text>().text = LeagueData.GetName(Club.GetLeagueDataId(Game.GetCurrentUserClubID()));
		currentTeamInfo.transform.Find("Info/League/Image").GetComponent<Image>().sprite = CountryFlagImageController.GetInstance().GetFlagImage(CountryData.GetCode(Club.GetCountryDataID(Game.GetCurrentUserClubID())));
		Helpers.calculateFiananceStarRating(currentTeamInfo.transform.Find("Info/Finances/Rating10").gameObject, Game.GetCurrentUserClubID());
	}

	private void UpdateSeasonHistory()
	{
		short currentClubID = Game.instance.ManagedClubProfile.CurrentClubID;
		List<SeasonHistory> allCurrentClubSeasonHistory = Game.instance.ManagedClubProfile.GetAllCurrentClubSeasonHistory();
		for (int i = 0; i < allCurrentClubSeasonHistory.Count; i++)
		{
			SeasonHistory seasonHistory = allCurrentClubSeasonHistory[i];
			if (allSeasonsStatsPanel.transform.Find(currentClubID + seasonHistory.SeasonID.Replace("/", "")) != null)
			{
				GameObject gameObject = allSeasonsStatsPanel.transform.Find(currentClubID + seasonHistory.SeasonID.Replace("/", "")).gameObject;
				gameObject.transform.Find("Win").GetComponent<Text>().text = seasonHistory.Wins.ToString();
				gameObject.transform.Find("Draws").GetComponent<Text>().text = seasonHistory.Draws.ToString();
				gameObject.transform.Find("Loss").GetComponent<Text>().text = seasonHistory.Loses.ToString();
				gameObject.transform.Find("Verdict").GetComponent<Text>().text = seasonHistory.WinPercentage.ToString("0") + "%";
				gameObject.transform.Find("Trophies").GetComponent<Text>().text = Game.instance.ManagedClubProfile.GetClubSeasonTrophyCount(currentClubID, seasonHistory.SeasonID).ToString();
			}
			else
			{
				AddNewSeasonHistoryRow();
			}
		}
	}

	private void AddNewSeasonHistoryRow()
	{
		SeasonHistory currentSeasonHistory = Game.instance.ManagedClubProfile.GetCurrentSeasonHistory();
		short currentClubID = Game.instance.ManagedClubProfile.CurrentClubID;
		string currentSeasonID = Game.instance.ManagedClubProfile.CurrentSeasonID;
		GameObject gameObject = Object.Instantiate(allSeasonsStatRow, allSeasonsStatRow.transform.parent);
		gameObject.transform.Find("Season").GetComponent<Text>().text = currentSeasonID;
		gameObject.transform.Find("Club").GetComponent<Text>().text = Club.GetName(currentClubID);
		gameObject.transform.Find("Location").GetComponent<Text>().text = CountryData.GetName(Club.GetCountryDataID(currentClubID));
		gameObject.transform.Find("Win").GetComponent<Text>().text = currentSeasonHistory.Wins.ToString();
		gameObject.transform.Find("Draws").GetComponent<Text>().text = currentSeasonHistory.Draws.ToString();
		gameObject.transform.Find("Loss").GetComponent<Text>().text = currentSeasonHistory.Loses.ToString();
		gameObject.transform.Find("Verdict").GetComponent<Text>().text = currentSeasonHistory.WinPercentage.ToString("0") + "%";
		gameObject.transform.Find("Trophies").GetComponent<Text>().text = Game.instance.ManagedClubProfile.GetClubSeasonTrophyCount(currentClubID, currentSeasonID).ToString();
		gameObject.name = currentClubID + currentSeasonID.Replace("/", "");
		gameObject.SetActive(value: true);
	}

	private void DrawRecords()
	{
		if (Game.instance.ManagedClubProfile.GetRecordAttendanceClubID() != 0)
		{
			recordsContainer.transform.Find("TopPanel/Record3/RecordValue/ContentTextBlue").GetComponent<Text>().text = Util.NumberFormatter(Game.instance.ManagedClubProfile.GetRecordAttendance());
			ImageController.GetInstance().LoadImage(Game.instance.ManagedClubProfile.GetRecordAttendanceClubID(), recordsContainer.transform.Find("TopPanel/Record3/ImageContainer/TeamImage").GetComponent<Image>(), ImageType.Club);
		}
		if (Game.instance.ManagedClubProfile.GetRecordWinOppClub() != 0)
		{
			string text = Game.instance.ManagedClubProfile.GetRecordWinScore()[0] + " - " + Game.instance.ManagedClubProfile.GetRecordWinScore()[1];
			recordsContainer.transform.Find("TopPanel/Record1/RecordValue/ContentTextBlue").GetComponent<Text>().text = text;
			recordsContainer.transform.Find("TopPanel/Record1/TeamName/ContentTextBlue").GetComponent<Text>().text = Club.GetName(Game.instance.ManagedClubProfile.GetRecordWinOppClub());
			ImageController.GetInstance().LoadImage(Game.instance.ManagedClubProfile.GetRecordWinOppClub(), recordsContainer.transform.Find("TopPanel/Record1/ImageContainer/TeamImage").GetComponent<Image>(), ImageType.Club);
		}
		if (Game.instance.ManagedClubProfile.GetRecordLossOppClub() != 0)
		{
			string text2 = Game.instance.ManagedClubProfile.GetRecordLossScore()[0] + " - " + Game.instance.ManagedClubProfile.GetRecordLossScore()[1];
			recordsContainer.transform.Find("TopPanel/Record2/RecordValue/ContentTextBlue").GetComponent<Text>().text = text2;
			recordsContainer.transform.Find("TopPanel/Record2/TeamName/ContentTextBlue").GetComponent<Text>().text = Club.GetName(Game.instance.ManagedClubProfile.GetRecordLossOppClub());
			ImageController.GetInstance().LoadImage(Game.instance.ManagedClubProfile.GetRecordLossOppClub(), recordsContainer.transform.Find("TopPanel/Record2/ImageContainer/TeamImage").GetComponent<Image>(), ImageType.Club);
		}
		if (Game.instance.ManagedClubProfile.GetRecordSigningPlayerID() != 0)
		{
			recordsContainer.transform.Find("MiddlePanel/Record1/RecordValue/ContentTextBlue").GetComponent<Text>().text = Helpers.GetValueText(Game.instance.ManagedClubProfile.GetRecordSigningAmount());
			recordsContainer.transform.Find("MiddlePanel/Record1/TeamName/ContentTextBlue").GetComponent<Text>().text = Player.GetName(Game.instance.ManagedClubProfile.GetRecordSigningPlayerID());
			ImageController.GetInstance().LoadImage(Game.instance.ManagedClubProfile.GetRecordSigningPlayerID(), recordsContainer.transform.Find("MiddlePanel/Record1/ImageContainer/TeamImage").GetComponent<Image>(), ImageType.Player);
		}
		if (Game.instance.ManagedClubProfile.GetRecordSigningReceivedPlayerID() != 0)
		{
			recordsContainer.transform.Find("MiddlePanel/Record2/RecordValue/ContentTextBlue").GetComponent<Text>().text = Helpers.GetValueText(Game.instance.ManagedClubProfile.GetRecordSigningReceivedAmount());
			recordsContainer.transform.Find("MiddlePanel/Record2/TeamName/ContentTextBlue").GetComponent<Text>().text = Player.GetName(Game.instance.ManagedClubProfile.GetRecordSigningReceivedPlayerID());
			ImageController.GetInstance().LoadImage(Game.instance.ManagedClubProfile.GetRecordSigningReceivedPlayerID(), recordsContainer.transform.Find("MiddlePanel/Record2/ImageContainer/TeamImage").GetComponent<Image>(), ImageType.Player);
		}
		recordsContainer.transform.Find("BottomPanel/Record1/RecordValue/ContentTextBlue").GetComponent<Text>().text = Game.instance.ManagedClubProfile.GetHighestWinStreak().ToString();
		recordsContainer.transform.Find("BottomPanel/Record2/RecordValue/ContentTextBlue").GetComponent<Text>().text = Game.instance.ManagedClubProfile.GetHighestUnbeatenStreak().ToString();
		ImageController.GetInstance().LoadImage(Game.GetCurrentUserClubID(), recordsContainer.transform.Find("BottomPanel/Record1/ImageContainer/TeamImage").GetComponent<Image>(), ImageType.Club);
		ImageController.GetInstance().LoadImage(Game.GetCurrentUserClubID(), recordsContainer.transform.Find("BottomPanel/Record2/ImageContainer/TeamImage").GetComponent<Image>(), ImageType.Club);
	}
}
