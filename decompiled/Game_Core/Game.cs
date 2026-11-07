using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using CielaSpike;
using SimpleJSON;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Purchasing;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Game : MonoBehaviour
{
	public enum GameAccess
	{
		FullAccess,
		BlockNewGames,
		BlockLoadGame
	}

	public enum DebugForceResults
	{
		OFF,
		WIN,
		LOSE,
		DRAW
	}

	public enum MatchAdvertProvider
	{
		NO_EXTERNAL_ADS = 0,
		SELF_HOSTED = 1,
		BIDSTACK = 2,
		OTHER = 99
	}

	public enum PackState
	{
		Unknown,
		WaitForNextPack,
		PackAvailable,
		Offline
	}

	public struct PlayedGameInfo
	{
		public int homeScore;

		public int awayScore;
	}

	private const int MaxHoursToProgress = 72;

	public static readonly DateTime GameStartDate = new DateTime(2020, 7, 1);

	public static Game instance;

	[HideInInspector]
	public DateTime WageExceedingIncomeDetectionDate = DateTime.MinValue;

	public GameAccess gameAccess;

	public short blockGameAccessPromoAd;

	public int androidAuthRetries;

	public bool DEBUG_TOOLS_PROCESSING;

	public bool DEBUG_TOOLS_PROCESSING_MATCH_ENGINE;

	public DateTime DEBUG_TOOLS_PROCESSING_DATE;

	public string processEndOfSeasonErrorLogMessage = "";

	public bool showErrorReportBugModal;

	public bool hasShownErrorReportBugModal;

	public string lastErrorMessage = "";

	public static DebugForceResults debugForceResults = DebugForceResults.OFF;

	public static bool debugFast2DPitchEngine = false;

	public static bool debugAutoPickTeam = false;

	public bool debugUnlimitedTraining;

	public List<string> betaKnownIssues;

	public List<string> betaFixedIssues;

	[HideInInspector]
	public bool hasCustomFixture;

	public int ManagedCountry;

	public int NumGamesPlayedThisSession;

	public List<int> screenStack = new List<int>();

	public List<object> screenStackOpts = new List<object>();

	public List<RandEvReaction> g_futureRespQueue = new List<RandEvReaction>();

	public SaveFileUpload upload;

	public Transfers transferInstance;

	public static bool forceClubImageUpdate = false;

	public static string CurrencySymbol = "£";

	public static double CurrencyConversionRate = 1.0;

	public bool isUltraThinDevice;

	public bool ReturningFromMatchday;

	public bool SkipMatch;

	public SQLiteDataService sqLiteDataService;

	public SQLiteService sqLiteService;

	[HideInInspector]
	public static float AdTimer = 0f;

	[HideInInspector]
	public static bool ShowAd = false;

	[HideInInspector]
	public static int AdInterval = int.MaxValue;

	public DateTime CurrentDate;

	public DateTime StartDate;

	private Color redTextColour = new Color(1f, 0f, 0f);

	private Color orangeTextColour = new Color(1f, 0.44f, 0f);

	private Color oliveTextColour = new Color(0.63f, 0.81f, 0f);

	private Color greenTextColour = new Color(0f, 1f, 0.13f);

	private Color redTextColourFitness = Helpers.HexToColour("921000");

	private Color darkOrangeColourFitness = Helpers.HexToColour("7D2000");

	private Color orangeTextColourFitness = Helpers.HexToColour("D7702C");

	private Color darkerYellowTextColourFitness = Helpers.HexToColour("E7C600");

	private Color brigherYellowTextColourFitness = Helpers.HexToColour("EBFF2B");

	private Color slightGreenTextColourFitness = Helpers.HexToColour("B2FF35");

	private Color brighterGreenTextColourFitness = Helpers.HexToColour("5DFF3B");

	private Color greenTextColourFitness = Helpers.HexToColour("26D241");

	public List<int> ChosenCountries { get; set; } = new List<int>();

	public static System.Random Rand { get; set; } = new System.Random();

	public RNG Random { get; set; } = new RNG();

	public int Seed { get; set; }

	public int g_currUser { get; set; }

	public byte drillInjuryDay { get; set; }

	public SeFixtureInfo g_fixInfo { get; set; } = new SeFixtureInfo();

	public Fixture g_matchday_nextfix { get; set; }

	public List<LiveScoresOutput> g_LiveScores { get; set; }

	public List<LiveScoresOutput> g_allGames { get; set; }

	public List<LiveTableOutput> g_LiveTable { get; set; }

	public Dictionary<int, CoachReport> g_coachReports { get; set; } = new Dictionary<int, CoachReport>();

	public int g_matchday_saveseed { get; set; }

	public List<ScheduledMessage> g_scheduledMessages { get; set; } = new List<ScheduledMessage>();

	public DateTime g_nextScheduledMessageDate { get; set; }

	public TacticInfo g_tacticinfo { get; set; } = new TacticInfo();

	public TacticAction g_tacticaction { get; set; } = new TacticAction();

	public int[] g_tm_aim { get; set; } = new int[2];

	public UI ui { get; set; }

	public bool g_bringUpToDate { get; set; }

	public int g_matchday_flowpos { get; set; }

	public bool ClubMessagesEnabled { get; set; } = true;

	public bool g_loadingSaveGame { get; set; }

	public Dictionary<int, AssociationSeasonLogList> g_association_seasonlog { get; set; } = new Dictionary<int, AssociationSeasonLogList>();

	public Dictionary<int, ContinentalComp> g_continentalCompetitions { get; set; } = new Dictionary<int, ContinentalComp>();

	public List<ContinentalSeasonLogDictionary> g_continental_seasonlog { get; set; } = new List<ContinentalSeasonLogDictionary>();

	public Team[] g_tm { get; set; } = new Team[2];

	public Season g_season { get; set; } = new Season();

	public Dictionary<int, Association> g_association { get; set; } = new Dictionary<int, Association>();

	public InjuredPlayers g_injuredPlayers { get; set; }

	public List<int> g_recentbans { get; set; } = new List<int>();

	public Dictionary<int, CountryData> g_CountryData { get; set; } = new Dictionary<int, CountryData>();

	public Dictionary<int, PlayerData> g_PlayerData { get; set; } = new Dictionary<int, PlayerData>();

	public int g_PlayerData_NextPlayerID { get; set; }

	public Dictionary<int, int> g_playerDataIDLookup { get; set; } = new Dictionary<int, int>();

	public Dictionary<short, Club> g_ClubData { get; set; } = new Dictionary<short, Club>();

	public int g_ClubDataLastClubIndex { get; set; }

	public Dictionary<short, short> g_ClubDataIDLookup { get; set; } = new Dictionary<short, short>();

	public Dictionary<int, LeagueData> g_LeagueData { get; set; } = new Dictionary<int, LeagueData>();

	public Dictionary<int, LeagueData> g_UnplayableLeagueData { get; set; } = new Dictionary<int, LeagueData>();

	public Dictionary<int, LeagueFinances> g_LeagueFinances { get; set; } = new Dictionary<int, LeagueFinances>();

	public Dictionary<int, DateTime> g_LeagueFinanceSchedule { get; set; } = new Dictionary<int, DateTime>();

	public Dictionary<int, PlayerCareerStats> g_playerCareerStats { get; set; } = new Dictionary<int, PlayerCareerStats>();

	public Dictionary<int, CupData> g_CupData { get; set; } = new Dictionary<int, CupData>();

	public StadiumPiece[] g_StadiumPieces { get; set; } = (from i in Enumerable.Range(0, 8)
		select new StadiumPiece()).ToArray();

	public DateTime g_StadiumPieceConstructionFinish { get; set; }

	public bool g_StadiumUpgradeInProgress { get; set; }

	public int g_AddedStadiumSeats { get; set; }

	public int g_BuildingSeats { get; set; }

	public int g_PrevStandLevel { get; set; }

	public Dictionary<int, StadiumData> g_StadiumData { get; set; } = new Dictionary<int, StadiumData>();

	public Dictionary<int, int> g_StadiumDataIDLookup { get; set; } = new Dictionary<int, int>();

	public Dictionary<int, ManagerData> g_ManagerData { get; set; } = new Dictionary<int, ManagerData>();

	public Dictionary<int, int> g_ManagerDataIDLookup { get; set; } = new Dictionary<int, int>();

	public Dictionary<short, PlayerSquad> g_ClubPlayer { get; set; } = new Dictionary<short, PlayerSquad>();

	public Dictionary<short, ListInt> g_clubdata_playerswantingbigclubtransfer { get; set; } = new Dictionary<short, ListInt>();

	public Dictionary<int, DateTime> g_clubdata_lastloandate { get; set; } = new Dictionary<int, DateTime>();

	public Dictionary<int, ManagedRatingList> g_PlayerData_ManagedRating { get; set; } = new Dictionary<int, ManagedRatingList>();

	public Dictionary<int, ManagedAttributesList> g_PlayerData_ManagedAttributes { get; set; } = new Dictionary<int, ManagedAttributesList>();

	public FacilityManager FacilityManager { get; set; } = new FacilityManager();

	public Dictionary<int, Scout> g_Scouts { get; set; } = new Dictionary<int, Scout>();

	public List<ScoutSearch> g_ScoutingSearch { get; set; } = new List<ScoutSearch>();

	public ScoutSearch g_lastScoutSearch { get; set; } = new ScoutSearch();

	public List<int> g_lastScoutCountries { get; set; } = new List<int>();

	public Dictionary<int, ScoutedAttributesPlayer> g_PlayersScouted { get; set; } = new Dictionary<int, ScoutedAttributesPlayer>();

	public TrainingDrillManager g_trainingDrillManager { get; set; } = new TrainingDrillManager();

	public TrainingManager g_trainingManager { get; set; } = new TrainingManager();

	public YouthManager g_youthManager { get; set; } = new YouthManager();

	public TasklistManager g_taskListManager { get; set; } = new TasklistManager();

	public Dictionary<int, Staff> g_staffdata { get; set; } = new Dictionary<int, Staff>();

	public int g_staffdata_nextstaffid { get; set; }

	public List<int> staffCreationQueue { get; set; } = new List<int>();

	public List<TransferBid> g_stafftransferbids { get; set; } = new List<TransferBid>();

	public Dictionary<int, ArrayInt> g_staffdata_attributes { get; set; } = new Dictionary<int, ArrayInt>();

	public Dictionary<int, double> g_playersExpectedGames { get; set; }

	public bool g_vipPackage { get; set; }

	public string g_vipCountryCode { get; set; } = "UK";

	public int g_vip_respawnid { get; set; }

	public string g_vip_respawn_firstname { get; set; } = "";

	public string g_vip_respawn_lastname { get; set; } = "";

	private string googleServerAuthCode { get; set; } = "";

	public string iosCode { get; set; } = "";

	private string steamCode { get; set; } = "";

	private string facebookCode { get; set; } = "";

	private int platformType { get; set; }

	private string platformID { get; set; } = "";

	private string steamId { get; set; } = "";

	private string facebookId { get; set; } = "";

	public PushNotifications pushNotifications { get; set; } = new PushNotifications();

	public string facebookName { get; set; }

	public float fbCurrencyOffset { get; set; }

	public float fbUsdExchange { get; set; }

	public float fbUsdExchangeInverse { get; set; }

	public string fbUserCurrency { get; set; }

	public int matchAdvertType { get; set; } = 1;

	public int matchAdvertLength { get; set; } = 30;

	public int matchAdsToRequest { get; set; } = 3;

	public string[] selfHostedAdvertURLs { get; set; } = new string[0];

	public int percentageOfMatchesToShowOurAds { get; set; }

	public string CustomerCountryCode { get; set; } = "";

	public string CustomerLang { get; set; } = "";

	public PlayerRespawnProfile g_player_respawnprofile { get; set; } = new PlayerRespawnProfile();

	public List<EngineQuick_TmStats> g_eq_tmstats { get; set; } = new List<EngineQuick_TmStats>
	{
		new EngineQuick_TmStats(),
		new EngineQuick_TmStats()
	};

	public DateTime g_fixtureTime { get; set; }

	public int g_boardConfidencePercentage { get; set; } = 60;

	public ManagedClub[] managedClub { get; set; }

	public ManagedClubProfile ManagedClubProfile { get; set; }

	public short g_startClubID { get; set; }

	public int g_startLeagueID { get; set; }

	public int g_clubRating { get; set; }

	public static bool drawUnmanagedStadium { get; set; } = false;

	public bool matchReportVisible { get; set; }

	public bool showLevelUpModalOnContinue { get; set; }

	public bool showRateAppModalOnContinue { get; set; }

	public bool showRewardModal { get; set; }

	public bool alwaysAppStore { get; set; } = true;

	public bool maintenenceMode { get; set; }

	public bool hasShownUpcomingMaintenence { get; set; }

	public bool usePremLogos { get; set; }

	public bool EndOfSeasonScreen { get; set; }

	public JobApplicationManager jobApplicationManager { get; set; }

	public DateTime BalanceSheetTime { get; set; }

	public int NumUnreadSupportMsgs { get; set; }

	public int infoDate { get; set; }

	public PackState freePackState { get; set; }

	public AchievementsTracker achievementsTracker { get; set; } = new AchievementsTracker();

	public PlayerConcernsScheduler playerConcernsScheduler { get; set; } = new PlayerConcernsScheduler();

	public MatchdayBoost matchdayBoost { get; set; }

	public int ManagerPoints { get; set; }

	public static bool saveError { get; set; } = false;

	public static bool isOutOfMemoryError { get; set; } = false;

	public static string latestVersion { get; set; } = "";

	public static bool enableInHouseInterstitial { get; set; } = false;

	public static bool SaveNextContinue { get; set; } = false;

	public static bool ShowOffineText { get; set; } = false;

	public static bool checkForAutoDownload { get; set; } = true;

	public static bool isBackupAfterSwitchAccount { get; set; } = false;

	public static string loginResultString { get; set; } = "";

	public bool hasSetSession { get; set; }

	public int NumGamesPlayedThisSave { get; set; }

	public int NumSeasonsPlayedThisSave { get; set; }

	public int NumTimesPromotedThisSave { get; set; }

	public int NumTimesRelegatedThisSave { get; set; }

	public int NumLeaguesWonThisSave { get; set; }

	public int NumCupsWonThisSave { get; set; }

	public string QuickSDKUsername { get; set; }

	public string QuickSDKUid { get; set; }

	public string QuickSDKToken { get; set; }

	public int ManagerLevel { get; set; } = 1;

	private int clubPoints { get; set; } = 10;

	public static SFOnlineUser sfUser { get; set; }

	public bool restartAppOneSDK { get; set; }

	public ManagedClub CurrentManagedClub => managedClub[instance.g_currUser];

	public short CurrentManagedClubId => managedClub[instance.g_currUser].ClubID;

	public int ClubPoints => clubPoints;

	public ShopPricesData shopPricesData { get; set; }

	public FacebookRewardData freePackData { get; set; } = new FacebookRewardData();

	public FacebookInviteData inviteData { get; set; } = new FacebookInviteData();

	private string wanCMSLoginSign { get; set; }

	private string wanCMSLogintime { get; set; }

	private string tencentUsername { get; set; }

	private string tencentToken { get; set; }

	private string tencentUserid { get; set; }

	public static string GameName => "Soccer Manager 2021";

	public bool IsGermanUser => CustomerLang == "de_DE";

	public IEnumerator Continue()
	{
		yield return Ninja.JumpToUnity;
		HideLoadingScreenInfo();
		GameLoopController.Instance.Reset();
		int counter = 0;
		int hourOfDay = CurrentDate.Hour;
		_ = instance.CurrentDate;
		bool showToolTip = true;
		int hoursToolTip = 0;
		do
		{
			int num = counter + 1;
			counter = num;
			if (num == 72)
			{
				GameLoopController.Instance.PauseGameLoop();
				break;
			}
			DateTime unplayedScheduleDateForManagedClub = g_season.Schedule.UnplayedScheduleDateForManagedClub;
			bool managedClubHasMatchNow = unplayedScheduleDateForManagedClub == instance.CurrentDate;
			if (!managedClubHasMatchNow && g_matchday_flowpos == 0)
			{
				CurrentDate = CurrentDate.AddHours(1.0);
				hourOfDay = ((hourOfDay != 23) ? (hourOfDay + 1) : 0);
				hoursToolTip++;
			}
			Array.IndexOf(Constants.CONST_FIXTURE_HOURS, (byte)hourOfDay);
			yield return Ninja.JumpToUnity;
			if (Matchday.Process())
			{
				if (g_matchday_flowpos == 0)
				{
					g_matchday_flowpos++;
				}
				if (g_matchday_flowpos == 13)
				{
					g_matchday_flowpos = 0;
				}
				if (g_matchday_flowpos == 12)
				{
					hourOfDay += 2;
				}
				showToolTip = false;
			}
			sendScheduledMessages();
			if (CurrentDate.Day == 1 && hourOfDay == 0)
			{
				LogConsole("DEBUG: Current date is: " + CurrentDate);
			}
			if ((CurrentDate.Day == 1 || CurrentDate.Day == 15) && hourOfDay == 0)
			{
				Scout.DeleteCoachReports();
			}
			yield return Ninja.JumpToUnity;
			if (UI.GetInstance().g_currentscreen == 36)
			{
				UI.GetInstance().CheckForAndCreateScreenIfNeeded(36);
				int compid = UI.GetInstance().cupDrawScreen.compid;
				if (compid != -1)
				{
					UI.GetInstance().cupDrawScreen.FinishContinentalDraw(compid);
				}
			}
			Awards.Process();
			if (hourOfDay != 10)
			{
				yield return Ninja.JumpToUnity;
				UI.GetInstance().UpdateLoadingSpinnerText();
				yield return Ninja.JumpBack;
				yield return Ninja.JumpToUnity;
			}
			switch (hourOfDay)
			{
			case 0:
				if (CurrentDate.Day == 1)
				{
					LeagueData topDivision = Association.GetTopDivision(GetCurrentUserClubAssociation());
					if (CurrentDate.Month == topDivision.SeasonOneStart)
					{
						Season.NewSeasonProcess();
					}
				}
				GameHealthCheck.DailyHealthCheck();
				CurrentManagedClub.UpdateBalanceLog();
				if (CurrentManagedClub.ShouldPaySponsorMoney())
				{
					CurrentManagedClub.PaySponsorMoney();
					if (CurrentManagedClub.CurrentSponsor.PaymentMethod == Sponsor.PaymentType.ANNUAL)
					{
						MessageManager.SponsorDealSecondSeason(CurrentManagedClub.CurrentSponsor.ID, CurrentManagedClub.CurrentSponsor.GetPerSeasonAmount());
					}
				}
				break;
			case 1:
				if (CurrentDate.Day == 1)
				{
					yield return Ninja.JumpToUnity;
					BringSeasonsUpToDate();
					yield return Ninja.JumpBack;
					yield return Ninja.JumpToUnity;
					Messages.CleanupInbox();
					yield return Ninja.JumpBack;
					ContinentalComp.ClubWorldCupMessageHandler.TriggerScheduleMessageStartDecember(CurrentDate.Month);
					instance.ManagedClubProfile.CurrentClubConfidence.CalculateBoardConfidenceMonthly();
					yield return Ninja.JumpToUnity;
					if (ProcessManagerSacking())
					{
						instance.Random.SetSeed(instance.CurrentDate.GetEpochSeconds());
						ModalController.Instance().ShowSackedModal();
						instance.CurrentManagedClub.sacked = true;
						Save.SaveData(Save.currentSlot);
						GameLoopController.Instance.PauseGameLoop();
					}
					jobApplicationManager.ProcessVacancies();
					yield return Ninja.JumpBack;
				}
				ContinentalComp.DrawNextRoundIfDueDayArrived();
				if (CurrentDate.DayOfWeek == DayOfWeek.Saturday)
				{
					Transfers.instance.transferTransactions.Update();
					if (CurrentDate > BalanceSheetTime.AddDays(6.0))
					{
						BalanceSheetTime = CurrentDate;
						CurrentManagedClub.WeeklyBalanceSheetUpdate();
					}
				}
				break;
			case 2:
				SeasonLogHealthCheck();
				break;
			case 6:
				if (!DEBUG_TOOLS_PROCESSING)
				{
					int num2 = instance.Random.GetRand() % 60;
					if (Club.GetNumberOfLeagueGamesPlayed(GetCurrentUserClubID()) > 10 && num2 == 0)
					{
						ModalController.Instance().showJobOffersModal();
						GameLoopController.Instance.PauseGameLoop();
					}
				}
				break;
			case 7:
				if (PlayerList.Process())
				{
					GameLoopController.Instance.PauseGameLoop();
				}
				if (instance.CurrentDate.Date == Player.GetNextContractExpiryWarningDate() && ManagedClub.CreatePlayerContractWarnings())
				{
					GameLoopController.Instance.PauseGameLoop();
				}
				PlayerList.ProcessUnmanagedClubsPlayerContracts();
				if (instance.CurrentDate.Day == 1)
				{
					PlayerList.ProcessFreeAgents();
				}
				break;
			case 8:
				yield return Ninja.JumpToUnity;
				if (Scouting.Process())
				{
					GameLoopController.Instance.PauseGameLoop();
				}
				yield return Ninja.JumpBack;
				break;
			case 9:
				if (Transfers.instance.Process())
				{
					GameLoopController.Instance.PauseGameLoop();
				}
				SquadHealthCheck();
				Staff.ProcessTransfers();
				LeagueFinancesController.UpdateLeagueFinances();
				break;
			case 10:
				YouthAcademy.ProcessYouthIntake();
				PlayerConcernsScheduler.Instance.Process();
				break;
			case 11:
			{
				RandomEvent.SendMediaQuestion(RandomEvent.QuestionType.LowBalance, null, -1, -1, -1, -1);
				RandomEvent.SendMediaQuestion(RandomEvent.QuestionType.Discipline, null, -1, -1, -1, -1);
				RandomEvent.SendMediaQuestion(RandomEvent.QuestionType.HighMorale, null, -1, -1, -1, -1);
				DateTime midSeasonTransferWindowDate = Transfers.instance.transferWindowHandler.GetMidSeasonTransferWindowDate(CurrentManagedClub.AssociationID);
				if (midSeasonTransferWindowDate != DateTime.MinValue && CurrentDate.Day == midSeasonTransferWindowDate.Day && CurrentDate.Month == midSeasonTransferWindowDate.Month)
				{
					sendUserProgressMessage();
				}
				if (Matchday.BuildUpProcess())
				{
					GameLoopController.Instance.PauseGameLoop();
				}
				break;
			}
			case 12:
				if (CurrentDate.DayOfWeek == DayOfWeek.Sunday || CurrentDate.DayOfWeek == DayOfWeek.Thursday)
				{
					Cup.DrawNextRoundIfDue();
				}
				if (Transfers.instance.transferWindowHandler.isLastDayOfTransferWindow(instance.CurrentManagedClub.AssociationID, instance.CurrentDate) && Transfers.instance.Process())
				{
					GameLoopController.Instance.PauseGameLoop();
				}
				break;
			case 20:
				ContinentalComp.ProgressCompetitionIfRequired();
				if (Transfers.instance.transferWindowHandler.isLastDayOfTransferWindow(instance.CurrentManagedClub.AssociationID, instance.CurrentDate) && Transfers.instance.Process())
				{
					GameLoopController.Instance.PauseGameLoop();
				}
				break;
			case 23:
				if (Transfers.instance.transferWindowHandler.isLastDayOfTransferWindow(instance.CurrentManagedClub.AssociationID, instance.CurrentDate) && Transfers.instance.Process())
				{
					GameLoopController.Instance.PauseGameLoop();
				}
				break;
			}
			if (CurrentManagedClubId == 0 && CurrentDate.DayOfWeek == DayOfWeek.Saturday && CurrentDate.Hour == 12)
			{
				GameLoopController.Instance.PauseGameLoop();
			}
			if (managedClubHasMatchNow)
			{
				break;
			}
			Stadium.ProcessStadiumUpgrade();
			Facilities.ProcessUpgrades();
			yield return Ninja.JumpToUnity;
			if (GameLoopController.Instance.IsDelayed())
			{
				GameLoopController.Instance.ShowDelayScreen();
				yield return StartCoroutine(DelayLoadingScreen());
			}
			else if (showToolTip)
			{
				showToolTip = false;
				yield return StartCoroutine(DrawToolTip());
			}
		}
		while (!GameLoopController.Instance.IsPaused());
		yield return Ninja.JumpToUnity;
		ui.UpdateTopBar();
		if (SaveNextContinue && !DEBUG_TOOLS_PROCESSING)
		{
			Save.SaveData(Save.currentSlot, showAnimation: false);
			SaveNextContinue = false;
		}
		GameLoopController.Instance.ShowInboxIfApplicable();
		GameLoopController.Instance.ShowNextScreen();
		ReviewController.Instance.CanShowReviewOrEnjoymentPrompt(Club.DidWinLastMatch(instance.CurrentManagedClubId));
	}

	public IEnumerator DelayLoadingScreen()
	{
		GameLoopController.Instance.SetIsDelayed(set: false);
		yield return new WaitForSeconds(GameLoopController.Instance.DelayInSecs);
	}

	public void HideLoadingScreenInfo()
	{
		GameObject gameObject = GameObject.Find("OverlayCanvas").transform.Find("LoadSpinner/LoadingPanel/ResultsPanel").gameObject;
		gameObject.transform.Find("MatchDayResultsFixtures").gameObject.SetActive(value: false);
		gameObject.transform.Find("ToolTipContainer").gameObject.SetActive(value: false);
		gameObject.transform.Find("DefaultText").gameObject.SetActive(value: true);
		gameObject.transform.Find("DefaultText").GetComponent<Text>().text = Translation.GetText("default-spinner-text") + "...";
	}

	public IEnumerator DrawToolTip()
	{
		int num = 15;
		GameObject gameObject = GameObject.Find("OverlayCanvas").transform.Find("LoadSpinner/LoadingPanel/ResultsPanel").gameObject;
		gameObject.transform.Find("ToolTipContainer").gameObject.SetActive(value: true);
		gameObject.transform.Find("DefaultText").gameObject.SetActive(value: false);
		int rand = instance.Random.GetRand(0, 14);
		for (int i = 0; i < num; i++)
		{
			int index = i + 2;
			gameObject.transform.Find("ToolTipContainer").GetChild(index).gameObject.SetActive(value: false);
			if (rand == i)
			{
				gameObject.transform.Find("ToolTipContainer").GetChild(index).gameObject.SetActive(value: true);
				gameObject.transform.Find("ToolTipContainer/TextImage/ContentTextGray").GetComponent<Text>().text = toolTipText(rand + 1);
			}
		}
		yield return new WaitForSeconds(2f);
	}

	private string toolTipText(int toolTips)
	{
		return toolTips switch
		{
			1 => Translation.GetText("tooltip-loading-1"), 
			2 => Translation.GetText("tooltip-loading-2"), 
			3 => Translation.GetText("tooltip-loading-3"), 
			4 => Translation.GetText("tooltip-loading-4"), 
			5 => Translation.GetText("tooltip-loading-5"), 
			6 => Translation.GetText("tooltip-loading-6"), 
			7 => Translation.GetText("tooltip-loading-7"), 
			8 => Translation.GetText("tooltip-loading-8"), 
			9 => Translation.GetText("tooltip-loading-9"), 
			10 => Translation.GetText("tooltip-loading-10"), 
			11 => Translation.GetText("tooltip-loading-11"), 
			12 => Translation.GetText("tooltip-loading-12"), 
			13 => Translation.GetText("tooltip-loading-13"), 
			14 => Translation.GetText("tooltip-loading-14"), 
			15 => Translation.GetText("tooltip-loading-15"), 
			_ => "SM2021", 
		};
	}

	private bool Continue_DebugTransfers(int hourOfDay)
	{
		bool result = false;
		switch (hourOfDay)
		{
		case 0:
			if (CurrentDate.Day == 1 && CurrentDate.Month == 1)
			{
				PlayerList.RefreshPlayerCachedRatings();
			}
			GameHealthCheck.RunTransfersHealthCheck();
			break;
		case 1:
			if (CurrentDate.Day == 1)
			{
				ProcessMonthsRetirementsAndRespawns(CurrentDate.Month);
			}
			break;
		case 7:
			PlayerList.ProcessUnmanagedClubsPlayerContracts();
			PlayerList.ProcessFreeAgents();
			break;
		case 9:
			result = Transfers.instance.Process();
			break;
		}
		return result;
	}

	public void isFacebookConnectFreePackAvailable()
	{
		freePackData.connectFreePackAvailable = 0;
	}

	public void SetFacebookFreePackUnlocked()
	{
		freePackData.connectFreePackAvailable = 1;
		freePackData.connectFreePackUnlocked = true;
	}

	public void SetFacebookConnectFreePackClaimed()
	{
		freePackData.connectFreePackAvailable = 1;
		freePackData.connectFreePackUnlocked = false;
		freePackData.connectFreePackClaimed = true;
	}

	public bool HasEverConnectedFacebook()
	{
		return freePackData.hasConnectedFacebook;
	}

	public void SetConnectedFacebook()
	{
		freePackData.hasConnectedFacebook = true;
		EventLoggingController.LogEvent("sociallinkfacebook", "matchesplayed", EventLoggingController.GetMatchPlayedString());
	}

	public void AddVIP(GameObject source)
	{
		UI.GetInstance().ChangeScreen(8, ShopScreen.ScreenTypes.Front);
		if (source != null)
		{
			UI.GetInstance().DrawVIPFlying(source);
		}
		CustomerDataController.Instance.AddCredits(500);
		CustomerDataController.Instance.AddBoosts(40, isShop: false);
		CustomerDataController.Instance.AddSaveSlots(5);
		CustomerDataController.Instance.CustomerDataHolder.Userdata.VIP = true;
		if (PlayerPrefs.HasKey("pendingpurchase"))
		{
			PlayerPrefs.DeleteKey("pendingpurchase");
			PlayerPrefs.Save();
		}
	}

	public void Start()
	{
		_ = Constants.runOnce;
		restartAppOneSDK = false;
		ShowAd = false;
		if (!Constants.isGameLoading)
		{
			GameStart();
		}
	}

	public void GameStart()
	{
		ES2.Init();
		UserManagerProfileData.LoadUserManagerProfile();
		Stadium.yourGrassCut = 0;
		UnityEngine.Object.DontDestroyOnLoad(GameObject.Find("GameContainer").gameObject);
		if (PlayerPrefs.HasKey("PrefLang"))
		{
			CustomerLang = PlayerPrefs.GetString("PrefLang");
			if (Translation.RTLlanguages.Contains(CustomerLang))
			{
				Constants.usingRTL = true;
			}
			else
			{
				Constants.usingRTL = false;
			}
		}
		if (PlayerPrefs.HasKey("prefcurrency"))
		{
			short num = (short)PlayerPrefs.GetInt("prefcurrency");
			if (Constants.CONST_CURRENCY_CONVERSION.ContainsKey(num))
			{
				Helpers.SetCurrency(num);
			}
			else
			{
				PlayerPrefs.DeleteKey("prefcurrency");
			}
		}
		RuntimeErrorHandler runtimeErrorHandler = RuntimeErrorHandler.GetInstance();
		if (runtimeErrorHandler != null)
		{
			runtimeErrorHandler.ShowErrorModalOnLoadIfRequired();
		}
		InitGame();
	}

	public void InitGame()
	{
		if (Constants.runOnce)
		{
			_ = UI.GetInstance().instantMatchMode;
			sqLiteService = new SQLiteService();
			sqLiteDataService = new SQLiteDataService();
			sqLiteDataService.Start();
			Save.ProcessSaveRecovery();
			UI.GetInstance().HideGenericLoading();
			if (pushNotifications == null)
			{
				pushNotifications = new PushNotifications();
			}
			pushNotifications.Startup();
			if (managedClub == null)
			{
				LogConsole("Game::InitGame() - managed club NULL");
			}
			else
			{
				LogConsole("Game::InitGame() - managed club length: " + managedClub.Length);
			}
			CurrentDate = DateTime.MinValue;
			StartDate = CurrentDate;
			ui = GameObject.Find("UI").GetComponent<UI>();
			StaticDataParser.PopulateStaticData();
			MessageEditorController.SetupMessageTemplateList();
			UI.GetInstance().UpdateNewsFeedBlob();
			UserManagerProfileController.PopulateStaticManagerTraits();
			UserManagerProfileController.PopulateUserManagerLevels();
			ClubObjectives.PopulateStaticClubObjectives();
			DataPackController.GetInstance().ApplyDefaultDataPack();
			if (!UI.GetInstance().instantMatchMode)
			{
				StartGameServerLogicUpdate();
			}
			if (!hasSetSession)
			{
				int num = PlayerPrefs.GetInt("sessions", 0);
				PlayerPrefs.SetInt("sessions", num + 1);
				num = PlayerPrefs.GetInt("sessions", 0);
				hasSetSession = true;
				Helpers.LogConsole("###CurrentSessionCount(). === " + num);
			}
			ManagerData value = new ManagerData(new string[10]
			{
				"0",
				"Romeo",
				"Zondervan",
				"0",
				DateTime.MinValue.GetEpochSeconds().ToString(),
				"0",
				"0",
				"1",
				"0",
				"0"
			});
			g_ManagerData.Add(0, value);
			g_tm[0] = new Team();
			g_tm[1] = new Team();
			if (SplashLoadingScreen.instance != null)
			{
				SplashLoadingScreen.instance.DrawStartScreen();
			}
			if (!UI.GetInstance().instantMatchMode && CustomerDataController.Instance.FirstTimeUserProfile)
			{
				StartCoroutine(WaitAndShowTermsModal(0.5f));
			}
			upload = new SaveFileUpload();
			Training.InitTraining();
			TasklistManager.CreateStartingTasks();
			Constants.runOnce = false;
			_ = UI.GetInstance().instantMatchMode;
			StartCoroutine(UnloadUnusedAssets());
		}
		else if (instance.managedClub != null)
		{
			ui = GameObject.Find("UI").GetComponent<UI>();
			upload = new SaveFileUpload();
			CheckForTicketPriceError();
			if (!UI.GetInstance().instantMatchMode)
			{
				EventLoggingController.LogEvent("loadgame", "clubname", EventLoggingController.GetClubString());
			}
		}
	}

	public void StartGameServerLogicUpdate()
	{
		if (CustomerDataController.Instance.CustomerDataHolder == null)
		{
			return;
		}
		if (CustomerDataController.Instance.CustomerDataHolder.Metadata != null)
		{
			if (CustomerDataController.Instance.CustomerDataHolder.Metadata.Version != null && !Debug.isDebugBuild)
			{
				if (!CustomerDataController.Instance.IsValidApplicationVersion())
				{
					ServerRequestController.Instance.showUpdateModal();
					latestVersion = Application.version;
				}
				else if (Application.platform == RuntimePlatform.IPhonePlayer)
				{
					latestVersion = Application.version;
				}
				else
				{
					latestVersion = Application.version;
				}
			}
			if (CustomerDataController.Instance.CustomerDataHolder.Metadata.SettingsContainer != null)
			{
				SettingsContainer settingsContainer = CustomerDataController.Instance.CustomerDataHolder.Metadata.SettingsContainer;
				AdGlobalSettingsContainer adGlobalSettingsContainer = CustomerDataController.Instance.CustomerDataHolder.Metadata.AdGlobalSettingsContainer;
				if (CustomerDataController.Instance.CustomerDataHolder.Metadata.SettingsContainer.PromoAdData != null)
				{
					PromoAdvertController promoAdvertController = PromoAdvertController.GetInstance();
					promoAdvertController.Config = Convert.ToInt16(CustomerDataController.Instance.CustomerDataHolder.Metadata.SettingsContainer.PromoAdValue);
					promoAdvertController.StorePromoAdvertsFromBackend();
					promoAdvertController.ShowPromoAdvertAndIncrementViewCountIfApplicable();
				}
				if (settingsContainer.RateAppModeNew != null)
				{
					if (settingsContainer.RateAppModeNew == "AlwaysAppStore")
					{
						alwaysAppStore = true;
					}
					else
					{
						alwaysAppStore = false;
					}
				}
				else
				{
					alwaysAppStore = true;
				}
				_ = settingsContainer.Logo;
				usePremLogos = settingsContainer.Logo;
				_ = settingsContainer.MaintenanceMode;
				if (settingsContainer.MaintenanceMode)
				{
					maintenenceMode = true;
					StartCoroutine(delayedShowMaintenenceModal());
				}
				else
				{
					maintenenceMode = false;
				}
				_ = settingsContainer.ScheduledMaintenanceDate;
				long scheduledMaintenanceDate = settingsContainer.ScheduledMaintenanceDate;
				if (Helpers.ShouldShowUpcomingMaintenance(scheduledMaintenanceDate))
				{
					ModalController.Instance().ShowScheduledMaintenence(scheduledMaintenanceDate);
				}
				if (adGlobalSettingsContainer != null && adGlobalSettingsContainer.ToString() != "\"null\"")
				{
					matchAdvertLength = adGlobalSettingsContainer.AdGlobalSettingsDesktop.AdLength;
					matchAdvertType = adGlobalSettingsContainer.AdGlobalSettingsDesktop.AdType;
					matchAdsToRequest = adGlobalSettingsContainer.AdGlobalSettingsDesktop.NumAdsToRequest;
					percentageOfMatchesToShowOurAds = adGlobalSettingsContainer.AdGlobalSettingsDesktop.OwnAdPercentage;
					if (adGlobalSettingsContainer.AdGlobalURLS != null && adGlobalSettingsContainer.ToString() != "\"null\"" && adGlobalSettingsContainer.AdGlobalURLS.Count > 0)
					{
						selfHostedAdvertURLs = new string[adGlobalSettingsContainer.AdGlobalURLS.Count];
						for (int i = 0; i < adGlobalSettingsContainer.AdGlobalURLS.Count; i++)
						{
							selfHostedAdvertURLs[i] = adGlobalSettingsContainer.AdGlobalURLS[i].ToString().Replace("\"", "");
						}
					}
				}
				_ = settingsContainer.GameAccess;
				gameAccess = (GameAccess)int.Parse(settingsContainer.GameAccess.ToString());
				_ = settingsContainer.GameAccessPromoId;
				blockGameAccessPromoAd = short.Parse(settingsContainer.GameAccessPromoId.ToString());
			}
		}
		if (CustomerDataController.Instance.CustomerDataHolder.Userdata != null)
		{
			_ = CustomerDataController.Instance.CustomerDataHolder.Userdata.NumSupportMsgs;
			NumUnreadSupportMsgs = CustomerDataController.Instance.CustomerDataHolder.Userdata.NumSupportMsgs;
			if (CustomerDataController.Instance.CustomerDataHolder.Userdata.UserManagerProfile != null && UserManagerProfileController.HasSpecialOfferExpired() && !CustomerDataController.Instance.FirstTimeUserProfile)
			{
				UserManagerProfileController.SpecialofferSetLogic(CanSave: true);
			}
		}
	}

	private IEnumerator delayedShowMaintenenceModal()
	{
		yield return new WaitForEndOfFrame();
		yield return new WaitForEndOfFrame();
		yield return new WaitForEndOfFrame();
		yield return new WaitForEndOfFrame();
		yield return new WaitForEndOfFrame();
	}

	public IEnumerator UnloadUnusedAssets()
	{
		AsyncOperation unloading = Resources.UnloadUnusedAssets();
		while (!unloading.isDone)
		{
			yield return null;
		}
	}

	public AwardsContainerList Goal50Awards(string seasonid)
	{
		return instance.sqLiteService.playerAwardTbl.GetSeasonAward(AwardType.Goal50, seasonid);
	}

	public AwardsContainerList NxGnU19Awards(string seasonid)
	{
		return instance.sqLiteService.playerAwardTbl.GetSeasonAward(AwardType.NxGnU19, seasonid);
	}

	public AwardsContainer GetLeagueSeasonAward(AwardType awardType, string seasonId, int leagueDataId)
	{
		AwardsContainer result = null;
		AwardsContainerList leagueSeasonAward = sqLiteService.playerAwardTbl.GetLeagueSeasonAward(awardType, seasonId, leagueDataId);
		if (leagueSeasonAward.List.Count == 1)
		{
			result = leagueSeasonAward.List[0];
		}
		return result;
	}

	private void CheckForTicketPriceError()
	{
		if (CurrentManagedClub.TicketPrice <= 1)
		{
			CurrentManagedClub.LeagueEconomy = new LeagueEconomy(GetCurrentUserClubID());
			CurrentManagedClub.TicketPrice = Economy.CalcClubTicketPrice(GetCurrentUserClubID());
		}
	}

	private IEnumerator WaitForInitialResponse()
	{
		yield return new WaitForEndOfFrame();
		InitGame();
	}

	private IEnumerator WaitAndShowChangeNameModal(float waitTime, bool startGame)
	{
		yield return new WaitForSeconds(0f);
		ModalController.Instance().ShowUsernameChangeModal(startGame);
	}

	private IEnumerator WaitAndShowTermsModal(float waitTime)
	{
		yield return new WaitForSeconds(waitTime);
		ModalController.Instance().ShowTermsModal();
	}

	private IEnumerator WaitAndShowChinaAndroidStartModal(float waitTime)
	{
		yield return new WaitForSeconds(waitTime);
		ModalController.Instance().ShowTextModal("《梦幻足球世界》3月30日震撼首测", "《梦幻足球世界》于2018年3月30日11点开启删档不计费测试，敬请大家来战~\n本次测试时间：2018年3月30日11: 00 - 2018年4月5日12: 00\n《中超足球经理》是由中超官方授权、首款真实模拟经营手游。沉浸式比赛视角，带来前所未有的深度互动\n汇集16家顶级球队，数百名真实中超球员，打造属于自己的中超梦之队。");
	}

	public void InitialiseInAppPurchasing()
	{
		LogConsole("***** Game::InitialiseInAppPurchasing()");
	}

	public void UnityToAndroid(string functionName)
	{
		new AndroidJavaClass("com.unity3d.player.UnityPlayer").GetStatic<AndroidJavaObject>("currentActivity").Call(functionName);
	}

	public void UnityToAndroid(string functionName, string jsonParameter)
	{
		new AndroidJavaClass("com.unity3d.player.UnityPlayer").GetStatic<AndroidJavaObject>("currentActivity").Call(functionName, jsonParameter);
	}

	public bool IsNewUser()
	{
		return Save.GetMostRecentSaveID() == -1;
	}

	public void StartBackendSyncCallback(UnityWebRequest response)
	{
		LogConsole("###StartBackendSyncCallback response.text: " + response.downloadHandler.text);
		JSONNode jSONNode = JSON.Parse(response.downloadHandler.text);
		if (jSONNode != null)
		{
			JSONNode jSONNode2 = jSONNode["CustomerSPData"];
			if (jSONNode2 == null)
			{
				Debug.LogError(" DUMMYDATA**** Remove line 1535 onwards -- game.cs before release.");
				jSONNode2 = JSON.Parse(((TextAsset)Resources.Load("DummyCustomerData", typeof(TextAsset))).text)["CustomerSPData"];
			}
			_ = jSONNode2 != null;
		}
	}

	public void BetaIssuesCallback(WWW response)
	{
		JSONNode jSONNode = JSON.Parse(response.text);
		if (!(jSONNode != null))
		{
			return;
		}
		if (jSONNode["known"] != null)
		{
			JSONNode jSONNode2 = jSONNode["known"];
			for (int i = 0; i < jSONNode2.Count; i++)
			{
				betaKnownIssues.Add(jSONNode2[i]);
			}
		}
		if (jSONNode["resolved"] != null)
		{
			JSONNode jSONNode3 = jSONNode["resolved"];
			for (int j = 0; j < jSONNode3.Count; j++)
			{
				betaFixedIssues.Add(jSONNode3[j]);
			}
		}
		if (SceneManager.GetActiveScene().name != "Match")
		{
			ModalController.Instance().ShowBetaIssuesModal();
		}
	}

	public void doOnesdkExit()
	{
	}

	private void doOnesdkLogin()
	{
	}

	public void doOnesdkLogout()
	{
	}

	public void setOnesdknRoleData(int customerID, string userName, int balance, string clanName)
	{
		LogConsole("###GameManager.setOnesdknRoleData()");
	}

	public void onesdkCharge(string code)
	{
	}

	private void onesdkPaymentCallback(string result)
	{
	}

	private void ExitResult(string result)
	{
	}

	private void LoginResult(string result)
	{
		LogConsole("LoginResult  result = " + result);
		loginResultString = result;
		bool flag = false;
		SFJSONObject sFJSONObject = new SFJSONObject(result);
		string text = (string)sFJSONObject.get("result");
		SFJSONObject sFJSONObject2 = (SFJSONObject)sFJSONObject.get("userinfo");
		if (sFJSONObject2 != null)
		{
			string text2 = (string)sFJSONObject2.get("channeluserid");
			if (sfUser != null && sfUser.getChannelUserId() != text2 && !isBackupAfterSwitchAccount && UI.GetInstance().g_currentscreen != 0)
			{
				flag = true;
			}
		}
		if (("1" == text) & flag)
		{
			isBackupAfterSwitchAccount = true;
		}
		else
		{
			LoginResultAction();
		}
	}

	public void LoginResultAction()
	{
	}

	public void ProcessQuickSDKLogin(string username, string uid, string token)
	{
	}

	public void CreateQuickSDKRole(int customerId, string username, int balance, string partyName)
	{
	}

	public void ResetOnesSDKApp()
	{
		if (sfUser != null)
		{
			LogConsole("Game::ResetOnesSDKApp channelid= " + sfUser.getChannelId() + " userid= " + sfUser.getChannelUserId());
		}
		LogConsole("not during match, just restart");
		restartAppOneSDK = false;
		RestartApp();
	}

	private IEnumerator ChangeScene(string sceneName)
	{
		AsyncOperation async = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);
		async.allowSceneActivation = false;
		while (async.progress < 0.9f)
		{
			yield return null;
		}
		async.allowSceneActivation = true;
	}

	public void RestartApp()
	{
		if (SceneManager.GetActiveScene().name == "SP_Generated_Scene")
		{
			LogConsole("###GameManager.RestartApp()");
			UnityEngine.Object.Destroy(GameObject.Find("GameContainer"));
			SceneManager.LoadScene(1);
		}
		else
		{
			UnityEngine.Object.Destroy(GameObject.Find("GameContainer"));
			StartCoroutine(LoadBackToMainScene());
		}
	}

	private IEnumerator LoadBackToMainScene()
	{
		yield return SceneManager.LoadSceneAsync(1);
		LogConsole("Loading complete");
	}

	public void WanCMSLogin()
	{
	}

	public void WanCMSLoginFail(string failResult)
	{
		LogConsole("WanCMS login failed: " + failResult);
		ModalController.Instance().CloseModal();
		GameStart();
	}

	public float GetShopPriceData(string id)
	{
		LogConsole("GetShopPriceData. item: " + id);
		int index = 0;
		switch (id)
		{
		case "sm20_bundle_1":
			index = 0;
			break;
		case "sm20_bundle_2":
			index = 1;
			break;
		case "sm20_bundle_3":
			index = 2;
			break;
		case "sm20_credits_1":
			index = 3;
			break;
		case "sm20_credits_2":
			index = 4;
			break;
		case "sm20_credits_3":
			index = 5;
			break;
		case "sm20_credits_4":
			index = 6;
			break;
		case "sm20_credits_5":
			index = 7;
			break;
		case "sm20_credits_6":
			index = 8;
			break;
		case "sm20_credits_1_0":
			index = 9;
			break;
		case "sm20_bundle_4":
			index = 10;
			break;
		case "sm20_vip":
			index = 11;
			break;
		case "sm20_bundle_4_0":
			index = 12;
			break;
		case "sm20_bundle_3_0":
			index = 13;
			break;
		case "sm20_bundle_1_0":
			index = 14;
			break;
		}
		LogConsole("shop num: " + shopPricesData.prices.Count);
		return shopPricesData.prices[index];
	}

	public string GetShopItemName(string id, int price = 0)
	{
		LogConsole("GetShopItemName");
		string result = "";
		switch (id)
		{
		case "sm20_bundle_1":
			result = "超级套餐";
			break;
		case "sm20_bundle_2":
			result = "豪华套餐";
			break;
		case "sm20_bundle_3":
			result = "终极套餐";
			break;
		case "sm20_bundle_4":
			result = "冠军套餐";
			break;
		case "sm20_bundle_1_0":
			result = "超级特价套餐";
			break;
		case "sm20_bundle_3_0":
			result = "终极特价套餐";
			break;
		case "sm20_bundle_4_0":
			result = "冠军特价套餐";
			break;
		case "sm20_vip":
			result = "VIP大礼包";
			break;
		case "sm20_credits_1":
			result = price + "元礼包";
			break;
		case "sm20_credits_2":
			result = price + "元礼包";
			break;
		case "sm20_credits_3":
			result = price + "元礼包";
			break;
		case "sm20_credits_4":
			result = price + "元礼包";
			break;
		case "sm20_credits_5":
			result = price + "元礼包";
			break;
		case "sm20_credits_6":
			result = price + "元礼包";
			break;
		}
		LogConsole("shop num: " + shopPricesData.prices.Count);
		return result;
	}

	public void WanCMSPaymentInitAndroid(string code)
	{
		LogConsole("WanCMSPaymentInitAndroid");
	}

	public void WanCMSPaymentInit(string name)
	{
		if (shopPricesData != null)
		{
			int num = (int)Math.Round(GetShopPriceData(name));
			string curUserID = CustomerDataController.Instance.GetCurUserID();
			string text = MiniJson.JsonEncode(new Dictionary<string, string>
			{
				{ "messageType", "payment" },
				{ "productName", name },
				{ "productDesc", "test" },
				{ "customerId", curUserID },
				{
					"price",
					num.ToString()
				}
			});
			LogConsole("WanCMSPaymentInit Unity side, sending " + text);
		}
		else
		{
			LogConsole("shopPricesData not available for id: " + name);
		}
	}

	public void Awake()
	{
		if (!Constants.isGameLoading)
		{
			if (managedClub == null)
			{
				LogConsole("Game::Awake() - managed club NULL");
			}
			else
			{
				LogConsole("Game::Awake() - managed club length: " + managedClub.Length);
			}
			if (instance == null)
			{
				instance = this;
			}
			else if (instance != this)
			{
				UnityEngine.Object.Destroy(gameObject);
			}
			_ = ui != null;
		}
	}

	public static Game GetInstance()
	{
		if (instance != null)
		{
			return instance;
		}
		LogConsole("There is no Game instance object");
		return new Game();
	}

	public bool IsStartingNewSavesBlocked()
	{
		return (gameAccess & GameAccess.BlockNewGames) > GameAccess.FullAccess;
	}

	public bool IsLoadingSavesBlocked()
	{
		return (gameAccess & GameAccess.BlockLoadGame) > GameAccess.FullAccess;
	}

	public IEnumerator Init(short managedClubID)
	{
		return Init(managedClubID, -1, gameLoading: false);
	}

	public IEnumerator Init(short managedClubId, int seed, bool gameLoading)
	{
		yield return Ninja.JumpToUnity;
		UI.SetLoadingScreenStatus(Translation.GetText("loading-fixture-data"), 0.2f);
		yield return Ninja.JumpBack;
		g_startClubID = managedClubId;
		g_startLeagueID = Club.GetLeagueDataId(managedClubId);
		ManagedCountry = Club.GetAssociationID(managedClubId);
		if (seed != -1)
		{
			Seed = seed;
		}
		else
		{
			System.Random random = new System.Random();
			Seed = random.Next(1000);
		}
		Random.SetSeed(Seed);
		FormationData.MakeGridPos();
		ContinentalComp.RemoveInvalidCompetitions();
		int num = 0;
		int num2 = 0;
		foreach (int key in g_LeagueData.Keys)
		{
			LeagueData leagueData = g_LeagueData[key];
			int count = leagueData.ClubIDs.Count;
			for (int i = 0; i < count; i++)
			{
				if (leagueData.ClubIDs[i] == managedClubId)
				{
					num = leagueData.AssociationID;
					num2 = leagueData.Division;
					break;
				}
			}
			LeagueData.Init(key);
		}
		int leagueCountry = num;
		LogConsole("Club Data ID " + managedClubId + " is in division " + num2 + " in " + leagueCountry);
		SetStartDate(Club.GetLeagueDataId(managedClubId));
		jobApplicationManager = new JobApplicationManager(StartDate);
		g_youthManager.NextYouthIntakeDate = StartDate.AddMonths(1).AddDays(StartDate.Day * -1 + 1);
		int existingManagerId = Club.GetManagerID(managedClubId);
		MoveManager(0, managedClubId);
		yield return Ninja.JumpToUnity;
		UI.SetLoadingScreenStatus(Translation.GetText("loading-fixture-data"), 0.4f);
		foreach (int chosenCountry in instance.ChosenCountries)
		{
			Helpers.CreateAndInitAssociation(chosenCountry);
		}
		PreSeason.GenerateFriendlies();
		foreach (KeyValuePair<int, ContinentalComp> g_continentalCompetition in g_continentalCompetitions)
		{
			g_continentalCompetition.Value.ReadContinentalStructure();
		}
		ReInitialiseCups(ChosenCountries);
		UI.SetLoadingScreenStatus(Translation.GetText("loading-squad-data"), 0.6f);
		instance.sqLiteService.CloseConnectionWithGarbageCollection();
		instance.sqLiteService.DeleteDB(Save.currentSlot);
		instance.sqLiteService.Start(Save.currentSlot);
		Transfers.GetInstance().bidManager.loanManager.SetupPreGameLoans();
		PlayerPotentialManager.GenerateAllPlayersPotential();
		PlayerList.RefreshPlayerCachedRatings();
		Player.InitPlayerProfiles();
		FacilityManager = new FacilityManager();
		Scouting.Init();
		foreach (int key2 in instance.g_PlayerData.Keys)
		{
			instance.g_PlayerData[key2].HiddenAttributes.Init(key2);
			instance.sqLiteService.PlayerStaticAdd.Add(key2);
		}
		LogConsole("creating MANAGEDCLUB");
		managedClub = new ManagedClub[1];
		managedClub[g_currUser] = new ManagedClub();
		LogConsole("Manager Name = " + ManagerData.GetName(0));
		PlayerSquad.SquadStatusManager.InitNewGame();
		InitRGYBCodes();
		ManagedClub.Init(managedClub[g_currUser], managedClubId);
		UI.SetLoadingScreenStatus(Translation.GetText("loading-league"), 0.75f);
		yield return Ninja.JumpBack;
		yield return Ninja.JumpToUnity;
		PlayerList.GeneratePlayerContracts();
		PlayerSquad players = Club.GetPlayers(managedClubId);
		for (int j = 0; j < players.Count; j++)
		{
			Training.AddTrainingPlayer(players[j]);
		}
		Transfers.instance.Process();
		UI.SetLoadingScreenStatus(Translation.GetText("loading-transfer-data"), 0.85f);
		StadiumStaticData.Load();
		g_StadiumPieces = Stadium.SetupStadium(-1, forceRegen: true);
		yield return Ninja.JumpBack;
		if (!gameLoading)
		{
			Staff.GenerateStaff();
		}
		string latestSeasonID = Association.GetAllSeasonIDsForDate(leagueCountry, StartDate).Last();
		yield return Ninja.JumpToUnity;
		UI.SetLoadingScreenStatus(Translation.GetText("loading-please-wait"), 0.9f);
		Season.processNewSeasonManagedClub(latestSeasonID, instance.g_season, leagueCountry, StartDate);
		Transfers.instance.SendMessages(leagueCountry);
		yield return Ninja.JumpBack;
		if (existingManagerId != -1 && !gameLoading)
		{
			MoveManager(existingManagerId, 0);
		}
		yield return Ninja.JumpToUnity;
		UI.SetLoadingScreenStatus(Translation.GetText("loading-please-wait"), 0.95f);
		yield return Ninja.JumpBack;
		Helpers.calculateTableEntries();
		yield return Ninja.JumpToUnity;
		PlayerConcernsScheduler.Instance.Process();
		yield return Ninja.JumpBack;
		g_playerDataIDLookup = null;
		g_ClubDataIDLookup = null;
		g_ManagerDataIDLookup = null;
		instance.ManagedClubProfile = new ManagedClubProfile(instance.g_season.SeasonID, managedClubId);
		MessageManager.NewAppointment(managedClubId, (int)instance.managedClub[instance.g_currUser].Balance);
		yield return Ninja.JumpToUnity;
		g_clubRating = Helpers.getClubPlayersRating(managedClubId);
		try
		{
			createStartGameScoutReports();
		}
		catch (Exception ex)
		{
			Debug.LogError(ex.Message);
		}
		Save.SaveData(Save.currentSlot, showAnimation: true, firstSave: true);
		string curUserID = CustomerDataController.Instance.GetCurUserID();
		PlayerPrefs.SetInt("Slot_" + curUserID + "_" + Save.currentSlot, (int)DateTime.Now.Subtract(new DateTime(1970, 1, 1)).TotalSeconds);
		InitialiseTrackingVariables();
		TrackNewGameEvent(managedClubId);
		if (Transfers.instance.transferWindowHandler.insideTransferWindow(Club.GetAssociationID(managedClubId)))
		{
			EventLoggingController.LogEvent("transferwindowopen");
		}
		FixtureHelper fixtureHelper = FixtureHelperFactory.GetFixtureHelper(instance.GetNextFixture(GetCurrentUserClubID()).fixture);
		string message = Translation.TemplateReplace(Translation.GetText("push-notification-friendly-match"), new Dictionary<string, string>
		{
			{
				"your club",
				fixtureHelper.ownClubName()
			},
			{
				"Club Name",
				fixtureHelper.oppClubName()
			}
		});
		NotificationController.ScheduleNotification(43200, GameName, message, 201);
		PlayerPrefs.SetString("prefdatevisitedtraining" + Save.currentSlot, "0");
		PlayerPrefs.SetInt("spotlightsave", 0);
		PlayerPrefs.SetInt("spotlightsaveYouth", 0);
		ContinentalComp.ProgressCompetitionIfRequired();
		InitialiseInAppPurchasing();
		yield return Ninja.JumpBack;
	}

	private void createStartGameScoutReports()
	{
		List<int> list = new List<int>();
		ScoutingOptions scoutingOptions = new ScoutingOptions();
		scoutingOptions.age = new int[2] { 21, 33 };
		scoutingOptions.contractLength = new float[2] { 0f, 10f };
		scoutingOptions.country = new List<int>();
		scoutingOptions.league = 0;
		scoutingOptions.nationality = 0;
		scoutingOptions.transferStatus = 0;
		scoutingOptions.value = new double[2]
		{
			instance.CurrentManagedClub.Balance / 2.0,
			instance.CurrentManagedClub.Balance * 2.0
		};
		scoutingOptions.positions = list;
		list.Clear();
		list.Add(8192);
		list.Add(16384);
		list.Add(32768);
		list.Add(1024);
		list.Add(4096);
		scoutingOptions.positions = list;
		Scouting.CreateScoutCountryReport(Scouting.GetAvailableScoutId(), scoutingOptions);
		Scouting.ProcessUncompleteScoutReports(instance.g_lastScoutSearch.searchid);
		list.Clear();
		list.Add(32);
		list.Add(128);
		list.Add(256);
		list.Add(512);
		list.Add(2048);
		scoutingOptions.positions = list;
		Scouting.CreateScoutCountryReport(Scouting.GetAvailableScoutId(), scoutingOptions);
		Scouting.ProcessUncompleteScoutReports(instance.g_lastScoutSearch.searchid);
		list.Clear();
		list.Add(2);
		list.Add(4);
		list.Add(8);
		list.Add(16);
		list.Add(64);
		scoutingOptions.positions = list;
		Scouting.CreateScoutCountryReport(Scouting.GetAvailableScoutId(), scoutingOptions);
		Scouting.ProcessUncompleteScoutReports(instance.g_lastScoutSearch.searchid);
	}

	private void InitialiseTrackingVariables()
	{
		NumGamesPlayedThisSave = 0;
		NumSeasonsPlayedThisSave = 0;
		NumTimesPromotedThisSave = 0;
		NumTimesRelegatedThisSave = 0;
		NumLeaguesWonThisSave = 0;
		NumCupsWonThisSave = 0;
	}

	private void TrackNewGameEvent(short managedClubId)
	{
		Dictionary<string, string> parameters = new Dictionary<string, string>
		{
			{
				"clubName",
				EventLoggingParam.GetClubNameParamValue()
			},
			{
				"leagueName",
				EventLoggingParam.GetLeagueNameParamValue()
			},
			{
				"gameMode",
				EventLoggingParam.GetGameModeParamValue()
			}
		};
		EventLoggingController.LogEvent("newgame", parameters);
		string eventName;
		switch (instance.CustomerLang)
		{
		default:
			return;
		case "ar_EG":
			eventName = "newGame_Arabic";
			break;
		case "de_DE":
			eventName = "newGame_German";
			break;
		case "en_EN":
			eventName = "newGame_English";
			break;
		case "es_ES":
			eventName = "newGame_Spanish";
			break;
		case "fr_FR":
			eventName = "newGame_French";
			break;
		case "id_ID":
			eventName = "newGame_Indonesian";
			break;
		case "it_IT":
			eventName = "newGame_Italian";
			break;
		case "ja_JP":
			eventName = "newGame_Japanese";
			break;
		case "ko_KR":
			eventName = "newGame_Korean";
			break;
		case "nl_NL":
			eventName = "newGame_Dutch";
			break;
		case "pt_PT":
			eventName = "newGame_Portugese";
			break;
		case "ru_RU":
			eventName = "newGame_Russian";
			break;
		case "th_TH":
			eventName = "newGame_Thai";
			break;
		case "tr_TR":
			eventName = "newGame_Turkish";
			break;
		case "zh_CN":
			eventName = "newGame_ChineseSimplified";
			break;
		case "zh_TW":
			eventName = "newGame_ChineseTraditional";
			break;
		}
		EventLoggingController.LogEvent(eventName, parameters);
	}

	private void InitRGYBCodes()
	{
		int seed = Random.GetSeed();
		Random.SetSeed(Seed);
		Club.InitRGYBCodes();
		ManagerData.InitRGYBCodes();
		PlayerList.InitRGYBCodes();
		Random.SetSeed(seed);
	}

	public void TakeJobOffer(short managedClubDataId, short oldClubID)
	{
		ModalController.Instance().CloseAllModals();
		Messages.DeleteAllMessages();
		WorldNewsScreen.DeleteAllMessages();
		g_scheduledMessages.Clear();
		ModalController.Instance().msgReadCount = 0;
		Transfers.instance.TakeJobOffer(managedClubDataId);
		instance.CurrentManagedClub.PlayerShortlist.Clear();
		ManagedCountry = Club.GetAssociationID(managedClubDataId);
		ManagedClub.Init(CurrentManagedClub, managedClubDataId, null, null, takeover: true);
		ManagedClubProfile.TakeJobOffer(managedClubDataId);
		MoveManager(0, managedClubDataId);
		instance.g_season.Schedule.UpdateUnplayedFixtureDateForDisplayAndManagedClub(instance.CurrentDate);
		instance.g_season.Schedule.UpdateAssociationForArrangedFriendlies(managedClubDataId, ManagedCountry);
		try
		{
			Facilities.ResetFacilities();
			YouthAcademy.ClearYouthAcademy();
			instance.g_trainingManager = new TrainingManager();
			instance.g_taskListManager = new TasklistManager();
			instance.CurrentManagedClub.CurrentSponsor = new Sponsor();
			instance.CurrentManagedClub.AvailableSponsors = new List<Sponsor>();
			instance.CurrentManagedClub.NextSponsorPayment = default;
			instance.CurrentManagedClub.DateSponsorTaken = default;
			instance.CurrentManagedClub.DomesticCupPrizeMoney = new Dictionary<int, int>();
			instance.CurrentManagedClub.ContinentalCupPrizeMoney = new Dictionary<int, int>();
			TasklistManager.CreateStartingTasks();
			Stadium.SetupStadium(-1, forceRegen: true);
			UI.GetInstance().InitClubOverViewScreen();
			GameObject gameObject = GameObject.Find("OverlayCanvas/TopBar/ClubLogo");
			GameObject gameObject2 = GameObject.Find("OverlayCanvas/TopBar/TeamName");
			_ = instance.g_ClubData[managedClubDataId];
			gameObject2.GetComponent<Text>().text = Club.GetName(managedClubDataId);
			ImageController.GetInstance().LoadImage(managedClubDataId, gameObject.GetComponent<Image>(), ImageType.Club);
			CurrentManagedClub.sacked = false;
		}
		catch (Exception ex)
		{
			Debug.LogError(ex.Message);
		}
		Save.SaveData(Save.currentSlot);
	}

	public static void MoveManager(int managerID, short clubID)
	{
		LogConsole("Game::movemanager - moving manager id: " + managerID + " to club id: " + clubID);
		if (managerID == 0)
		{
			CareerEntryList careerHistory = instance.g_ManagerData[managerID].CareerHistory;
			if (careerHistory.CareerEntries.Count > 0)
			{
				careerHistory.CareerEntries.Last().EndDate = instance.CurrentDate;
			}
			careerHistory.CareerEntries.Add(new CareerEntry
			{
				ClubID = clubID,
				StartDate = instance.CurrentDate,
				EndDate = DateTime.MaxValue
			});
		}
		instance.g_ManagerData[managerID].ClubID = clubID;
	}

	public static bool Validator(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
	{
		return true;
	}

	private void ClearStaticData()
	{
		g_continental_seasonlog = new List<ContinentalSeasonLogDictionary>();
		g_CupData = new Dictionary<int, CupData>();
		g_LeagueData = new Dictionary<int, LeagueData>();
		g_ClubDataLastClubIndex = 0;
		g_ClubData = new Dictionary<short, Club>();
		g_ClubDataIDLookup = new Dictionary<short, short>();
		g_StadiumData = new Dictionary<int, StadiumData>();
		g_StadiumDataIDLookup = new Dictionary<int, int>();
		g_ManagerData = new Dictionary<int, ManagerData>();
		g_PlayerData = new Dictionary<int, PlayerData>();
		g_playerDataIDLookup = new Dictionary<int, int>();
		g_PlayerData_NextPlayerID = 0;
		g_ClubPlayer = new Dictionary<short, PlayerSquad>();
	}

	public bool ProcessManagerSacking()
	{
		if (DEBUG_TOOLS_PROCESSING)
		{
			return false;
		}
		if (Club.GetNumberOfLeagueGamesPlayed(GetCurrentUserClubID()) > 10 && instance.ManagedClubProfile.GetBoardConfidence() <= 30 && instance.ManagedClubProfile.GetBoardConfidence() <= 20)
		{
			if (instance.ManagedClubProfile.GetBoardConfidence() <= 10)
			{
				return true;
			}
			if (new System.Random().Next(0, 2) != 0)
			{
				return true;
			}
		}
		return false;
	}

	public int CalculateBoardExpectations(DateTime thedate)
	{
		if (thedate == DateTime.MinValue)
		{
			thedate = StartDate;
		}
		short currentManagedClubId = CurrentManagedClubId;
		int leagueDataId = Club.GetLeagueDataId(currentManagedClubId);
		int num = 1;
		int count = g_LeagueData[leagueDataId].ClubIDs.Count;
		int topFifteenTeamRating = Club.GetTopFifteenTeamRating(currentManagedClubId);
		List<short> list = new List<short>();
		list = instance.g_LeagueData[leagueDataId].ClubIDs;
		for (int i = 0; i < list.Count; i++)
		{
			if (list[i] != currentManagedClubId && Club.GetTopFifteenTeamRating(list[i]) > topFifteenTeamRating)
			{
				num++;
			}
		}
		if (num == 1)
		{
			if (g_LeagueData[leagueDataId].Division == 1)
			{
				return 1;
			}
			if (Random.GetRand(1) == 0)
			{
				return 1;
			}
			return 5;
		}
		if ((double)num <= Math.Floor((double)count / 4.0))
		{
			if (g_LeagueData[leagueDataId].Division > 1)
			{
				return 5;
			}
			return 2;
		}
		if ((double)num >= Math.Ceiling((double)count / 4.0) && (double)num <= Math.Floor((double)(count / 4) * 3.0))
		{
			return 3;
		}
		return 4;
	}

	public static ManagedClub GetCurrentUserClub(int currUser)
	{
		if (currUser == 0)
		{
			currUser = instance.g_currUser;
		}
		if (instance == null || instance.managedClub == null || instance.managedClub.Length == 0 || instance.CurrentManagedClub == null)
		{
			return null;
		}
		return instance.CurrentManagedClub;
	}

	public static short GetCurrentUserClubID()
	{
		ManagedClub currentUserClub = GetCurrentUserClub(instance.g_currUser);
		if (currentUserClub != null)
		{
			return currentUserClub.ClubID;
		}
		if (instance.CurrentDate == instance.StartDate)
		{
			return instance.g_startClubID;
		}
		return 0;
	}

	public static int GetCurrentUserClubAssociation()
	{
		return GetCurrentUserClub(instance.g_currUser)?.AssociationID ?? Club.GetAssociationID((instance.managedClub != null) ? instance.CurrentManagedClubId : instance.g_startClubID);
	}

	public List<int> GetAllContinents()
	{
		List<int> list = new List<int>();
		for (byte b = 1; b <= 6; b++)
		{
			list.Add(b);
		}
		return list;
	}

	public List<int> GetCountriesFromContinent(int continentid)
	{
		List<int> list = new List<int>();
		Dictionary<int, string> dictionary = new Dictionary<int, string>();
		List<int> list2 = new List<int>();
		foreach (KeyValuePair<int, CountryData> g_CountryDatum in g_CountryData)
		{
			if (g_CountryDatum.Value.Continent == continentid)
			{
				list.Add(g_CountryDatum.Key);
			}
		}
		foreach (KeyValuePair<int, LeagueData> g_LeagueDatum in g_LeagueData)
		{
			for (int i = 0; i < list.Count; i++)
			{
				if (g_LeagueDatum.Value.AssociationID == list[i] && g_LeagueDatum.Value.Division == 1)
				{
					dictionary[list[i]] = g_CountryData[list[i]].Name;
					break;
				}
			}
		}
		foreach (KeyValuePair<int, string> item in dictionary.OrderBy((KeyValuePair<int, string> key) => key.Value))
		{
			list2.Add(item.Key);
		}
		return list2;
	}

	public void ScheduleMessage(object[] msgTypeAndParams, DateTime date)
	{
		ScheduledMessage item = new ScheduledMessage(msgTypeAndParams, date);
		if (instance.g_nextScheduledMessageDate == DateTime.MinValue || instance.g_nextScheduledMessageDate > date)
		{
			g_nextScheduledMessageDate = date;
			LogConsole("Game::scheduleMessage() - scheduling message at position 0 for date: " + date);
			g_scheduledMessages.Insert(0, item);
			return;
		}
		int count = g_scheduledMessages.Count;
		for (int i = 0; i < count; i++)
		{
			if (g_scheduledMessages[i].date >= date)
			{
				LogConsole("Game::scheduleMessage() - scheduling message at position " + i + " for date: " + date);
				g_scheduledMessages.Insert(i, item);
				break;
			}
		}
	}

	public List<FirstFixtureDetails> GetImportantFixtures(int clubid)
	{
		List<FirstFixtureDetails> list = new List<FirstFixtureDetails>();
		Schedule schedule = g_season.Schedule;
		int num = schedule.schedule.Count();
		for (int i = 5; i < num; i++)
		{
			int count = schedule.schedule[i].Fixtures.Count;
			for (int j = 0; j < count; j++)
			{
				Fixture fixture = schedule.schedule[i].Fixtures[j];
				if (fixture.competitiontype == 2 && (fixture.homeClubId == clubid || fixture.awayClubId == clubid) && Club.AreRivals(fixture.homeClubId, fixture.awayClubId))
				{
					if (fixture.homeClubId == clubid)
					{
						FirstFixtureDetails firstFixtureDetails = new FirstFixtureDetails();
						firstFixtureDetails.Date = schedule.schedule[i].Date;
						firstFixtureDetails.Opponent = fixture.awayClubId;
						firstFixtureDetails.Venue = "home";
						firstFixtureDetails.CompetitionID = fixture.competitionid;
						firstFixtureDetails.CompetitionType = fixture.competitiontype;
						list.Add(firstFixtureDetails);
					}
					else
					{
						FirstFixtureDetails firstFixtureDetails2 = new FirstFixtureDetails();
						firstFixtureDetails2.Date = schedule.schedule[i].Date;
						firstFixtureDetails2.Opponent = fixture.homeClubId;
						firstFixtureDetails2.Venue = "away";
						firstFixtureDetails2.CompetitionID = fixture.competitionid;
						firstFixtureDetails2.CompetitionType = fixture.competitiontype;
						list.Add(firstFixtureDetails2);
					}
					break;
				}
			}
		}
		return list;
	}

	public List<FirstFixtureDetails> GetFriendlyFixtures(int clubID)
	{
		List<FirstFixtureDetails> list = new List<FirstFixtureDetails>();
		FixtureSetList schedule = g_season.Schedule.schedule;
		if (schedule != null)
		{
			int num = schedule.Count();
			for (int i = 0; i < num; i++)
			{
				int count = schedule[i].Fixtures.Count;
				for (int j = 0; j < count; j++)
				{
					Fixture fixture = schedule[i].Fixtures[j];
					if (fixture.competitiontype == 5)
					{
						if (fixture.homeClubId == clubID)
						{
							FirstFixtureDetails firstFixtureDetails = new FirstFixtureDetails();
							firstFixtureDetails.Date = schedule[i].Date;
							firstFixtureDetails.Opponent = fixture.awayClubId;
							firstFixtureDetails.Venue = "home";
							list.Add(firstFixtureDetails);
							break;
						}
						if (fixture.awayClubId == clubID)
						{
							FirstFixtureDetails firstFixtureDetails2 = new FirstFixtureDetails();
							firstFixtureDetails2.Date = schedule[i].Date;
							firstFixtureDetails2.Opponent = fixture.homeClubId;
							firstFixtureDetails2.Venue = "away";
							list.Add(firstFixtureDetails2);
							break;
						}
					}
				}
			}
		}
		return list;
	}

	public List<int> getLeagueIDsForAssoc(int associd)
	{
		List<int> list = new List<int>();
		foreach (LeagueData value in g_LeagueData.Values)
		{
			if (value.AssociationID == associd)
			{
				list.Add(value.LeagueID);
			}
		}
		return list;
	}

	public void sendScheduledMessages()
	{
		int num = 0;
		DateTime currentDate = CurrentDate;
		if (!(g_nextScheduledMessageDate <= currentDate))
		{
			return;
		}
		int count = g_scheduledMessages.Count;
		for (int i = 0; i < count; i++)
		{
			if (g_scheduledMessages[i].date <= currentDate)
			{
				if ((short)g_scheduledMessages[i].typeAndParams[0] == 17)
				{
					Messages.Add((short)g_scheduledMessages[i].typeAndParams[0], g_scheduledMessages[i].typeAndParams[1], g_scheduledMessages[i].typeAndParams[2]);
				}
				else
				{
					Messages.Add((short)g_scheduledMessages[i].typeAndParams[0], g_scheduledMessages[i].typeAndParams.Slice(1, g_scheduledMessages[i].typeAndParams.Count - 1).ToArray());
				}
				num++;
			}
		}
		g_scheduledMessages.RemoveRange(0, num);
		if (g_scheduledMessages.Count == 0)
		{
			g_nextScheduledMessageDate = DateTime.MinValue;
		}
		else
		{
			g_nextScheduledMessageDate = g_scheduledMessages[0].date;
		}
	}

	public void sendUserProgressMessage()
	{
		short currentUserClubID = GetCurrentUserClubID();
		Club.GetAssociationID(currentUserClubID);
		byte leaguePosition = LeagueList.GetLeaguePosition(currentUserClubID);
		byte division = Club.GetDivision(currentUserClubID);
		int continent = CountryData.GetContinent(Club.GetAssociationID(currentUserClubID));
		int leagueDataId = Club.GetLeagueDataId(currentUserClubID);
		byte numTeams = g_LeagueData[leagueDataId].NumTeams;
		string text = null;
		int stringIndex = 0;
		if (leaguePosition < numTeams / 2)
		{
			if (division == 0)
			{
				if (continent == 1)
				{
					text = Translation.GetText("msg-progress-top-half-top-table-europe");
					stringIndex = 0;
				}
			}
			else if (leaguePosition <= 5)
			{
				text = Translation.GetText("msg-progress-top-5-january-window");
				stringIndex = 1;
			}
			else
			{
				text = Translation.GetText("msg-progress-promotion-place-not-top-league");
				stringIndex = 2;
			}
		}
		else
		{
			text = Translation.GetText("msg-progress-relegation-zone");
			stringIndex = 3;
		}
		if (text != null)
		{
			MessageManager.BoardStatement(stringIndex);
		}
	}

	public void BringSeasonsUpToDate()
	{
		DateTime currentDate = CurrentDate;
		int month = currentDate.Month;
		currentDate = currentDate.AddHours(-2.0);
		List<int> list = new List<int>();
		int count = ChosenCountries.Count;
		for (int i = 0; i < count; i++)
		{
			int num = ChosenCountries[i];
			LeagueData topDivision = Association.GetTopDivision(num);
			if (topDivision == null)
			{
				LogConsole("WARNING: GAME_bringSeasonsUpToDate() - Top division was null for associd: ' + associd + ' - not bringing seasons up to date");
				continue;
			}
			byte seasonOneStart = topDivision.SeasonOneStart;
			_ = topDivision.SeasonOneEnd;
			if (month == seasonOneStart)
			{
				list.Add(num);
			}
		}
		int count2 = list.Count;
		if (count2 > 0)
		{
			instance.sqLiteService.RemoveAllSingleSeasonData();
			instance.g_season.Schedule.RemoveAllFixtures();
		}
		if (count2 > 0)
		{
			Season.ProcessContinentalSeasonLog();
			bool flag = true;
			for (int j = 0; j < count2; j++)
			{
				int num2 = list[j];
				List<string> allSeasonIDsForDate = Association.GetAllSeasonIDsForDate(num2, currentDate);
				string text = "";
				if (allSeasonIDsForDate.Count > 0)
				{
					text = allSeasonIDsForDate[allSeasonIDsForDate.Count - 1];
					if (!Season.SeasonLogExists(num2, text))
					{
						Debug.LogError("Game::BringSeasonsUpToDate() - no seasonlog existed for season " + text + " of assoc " + num2 + " - pretty sure this is bad");
					}
					Season.ProcessSeasonLog(num2, Season.CreateOrGetSeasonLog(num2, text));
				}
				string nextSeasonID = Season.GetNextSeasonID(text);
				_ = instance.g_season.SeasonID;
				GameHealthCheck.SetSeasonSuccessState(success: false);
				Season.NewSeason(nextSeasonID, g_season, num2, Association.GetSeasonStartDate(num2, nextSeasonID), useInitialClubIDs: false);
				GameHealthCheck.SetSeasonSuccessState(GameHealthCheck.CheckLeaguesInSeason(g_season));
				if (flag)
				{
					TrackSeasonCompleteEvent();
					flag = false;
				}
			}
			PreSeason.GenerateFriendlies();
		}
		Helpers.calculateTableEntries();
		ProcessMonthsRetirementsAndRespawns(month);
		bool flag2 = ReInitialiseContinentalCompetitions(currentDate, month);
		ReInitialiseCups(list);
		if (flag2)
		{
			instance.ManagedClubProfile.CheckAllObjectives();
			if (DEBUG_TOOLS_PROCESSING)
			{
				instance.ManagedClubProfile.InitNewSeason();
			}
			else
			{
				GameLoopController.Instance.PauseGameLoopEndOfSeason();
			}
		}
	}

	private void TrackSeasonCompleteEvent()
	{
		NumSeasonsPlayedThisSave++;
		if (NumSeasonsPlayedThisSave == 1)
		{
			EventLoggingController.LogEvent("seasoncomplete_x1", new Dictionary<string, string>
			{
				{
					"clubName",
					EventLoggingParam.GetClubNameParamValue()
				},
				{
					"leagueName",
					EventLoggingParam.GetLeagueNameParamValue()
				},
				{
					"gameMode",
					EventLoggingParam.GetGameModeParamValue()
				}
			});
			return;
		}
		string text = "";
		switch (NumSeasonsPlayedThisSave)
		{
		case 2:
			text = "seasoncomplete_x2";
			break;
		case 3:
			text = "seasoncomplete_x3";
			break;
		case 4:
			text = "seasoncomplete_x4";
			break;
		case 5:
			text = "seasoncomplete_x5";
			break;
		case 10:
			text = "seasoncomplete_x10";
			break;
		case 15:
			text = "seasoncomplete_x15";
			break;
		case 20:
			text = "seasoncomplete_x20";
			break;
		}
		if (!string.IsNullOrEmpty(text))
		{
			EventLoggingController.LogEvent(text, new Dictionary<string, string>
			{
				{
					"clubName",
					EventLoggingParam.GetClubNameParamValue()
				},
				{
					"leagueName",
					EventLoggingParam.GetLeagueNameParamValue()
				}
			});
		}
	}

	public void ProcessMonthsRetirementsAndRespawns(int currentMonth)
	{
		instance.g_player_respawnprofile.FillRespawnProfile();
		List<int> list = new List<int>();
		List<int> playersToRespawn = new List<int>();
		foreach (int key in g_LeagueData.Keys)
		{
			LeagueData leagueData = g_LeagueData[key];
			int associationID = leagueData.AssociationID;
			string currentSeasonID = Association.GetCurrentSeasonID(CurrentManagedClub.AssociationID);
			if (leagueData.Division == 1 && currentMonth == Association.GetSeasonStartDate(associationID, currentSeasonID).Month)
			{
				list.Add(associationID);
			}
		}
		if (currentMonth == 6)
		{
			list.Add(2);
		}
		DebugTools.AddRespawnLogMessage("Game::ProcessMonthsRetirementsAndRespawns() assocsToRetire Array: " + Helpers.ObjectStringify(list));
		for (int i = 0; i < list.Count; i++)
		{
			PlayerList.RetireAssoc(list[i], playersToRespawn);
		}
		PlayerRespawn.Respawn(playersToRespawn);
	}

	public List<NextFixtureInfo> GetNextThisWeeksFixtures(short clubid, int? comptype = null)
	{
		List<NextFixtureInfo> list = new List<NextFixtureInfo>();
		if (clubid == 0)
		{
			return null;
		}
		CultureInfo cultureInfo = new CultureInfo("en-GB");
		Calendar calendar = cultureInfo.Calendar;
		DateTime currentDate = CurrentDate;
		int weekOfYear = calendar.GetWeekOfYear(currentDate.Date, cultureInfo.DateTimeFormat.CalendarWeekRule, cultureInfo.DateTimeFormat.FirstDayOfWeek);
		FixtureSetList schedule = g_season.Schedule.schedule;
		int num = schedule.Count();
		for (int i = 0; i < num; i++)
		{
			if (calendar.GetWeekOfYear(schedule[i].Date, cultureInfo.DateTimeFormat.CalendarWeekRule, cultureInfo.DateTimeFormat.FirstDayOfWeek) != weekOfYear)
			{
				continue;
			}
			int count = schedule[i].Fixtures.Count;
			for (int j = 0; j < count; j++)
			{
				if ((schedule[i].Fixtures[j].homeClubId == clubid || schedule[i].Fixtures[j].awayClubId == clubid) && (!comptype.HasValue || schedule[i].Fixtures[j].competitiontype == comptype))
				{
					if (!(schedule[i].Date < currentDate.AddDays(8.0)))
					{
						return list;
					}
					NextFixtureInfo nextFixtureInfo = new NextFixtureInfo();
					nextFixtureInfo.leaguedataid = null;
					nextFixtureInfo.date = schedule[i].Date;
					nextFixtureInfo.fixture = schedule[i].Fixtures[j];
					list.Add(nextFixtureInfo);
				}
			}
		}
		return list;
	}

	public List<NextFixtureInfo> GetNextFixtureMultiple(int amount, short clubid)
	{
		return GetNextFixtureMultiple(amount, clubid, null);
	}

	public List<NextFixtureInfo> GetNextFixtureMultiple(int amount, short clubid, int? comptype)
	{
		List<NextFixtureInfo> list = new List<NextFixtureInfo>();
		if (clubid == 0)
		{
			return null;
		}
		Club.GetAssociationID(clubid);
		DateTime currentDate = CurrentDate;
		FixtureSetList schedule = g_season.Schedule.schedule;
		int num = schedule.Count();
		for (int i = 0; i < num; i++)
		{
			if (!(schedule[i].Date >= currentDate))
			{
				continue;
			}
			int count = schedule[i].Fixtures.Count;
			for (int j = 0; j < count; j++)
			{
				if ((schedule[i].Fixtures[j].homeClubId == clubid || schedule[i].Fixtures[j].awayClubId == clubid) && (!comptype.HasValue || schedule[i].Fixtures[j].competitiontype == comptype))
				{
					if (list.Count >= amount)
					{
						return list;
					}
					NextFixtureInfo nextFixtureInfo = new NextFixtureInfo();
					nextFixtureInfo.leaguedataid = null;
					nextFixtureInfo.date = schedule[i].Date;
					nextFixtureInfo.fixture = schedule[i].Fixtures[j];
					list.Add(nextFixtureInfo);
				}
			}
		}
		return list;
	}

	public NextFixtureInfo GetNextFixture(short clubid, int? comptype = null)
	{
		if (clubid == 0)
		{
			return null;
		}
		Club.GetAssociationID(clubid);
		DateTime currentDate = CurrentDate;
		FixtureSetList schedule = g_season.Schedule.schedule;
		int num = schedule.Count();
		for (int i = 0; i < num; i++)
		{
			if (!(schedule[i].Date >= currentDate))
			{
				continue;
			}
			int count = schedule[i].Fixtures.Count;
			for (int j = 0; j < count; j++)
			{
				if ((schedule[i].Fixtures[j].homeClubId == clubid || schedule[i].Fixtures[j].awayClubId == clubid) && (!comptype.HasValue || schedule[i].Fixtures[j].competitiontype == comptype))
				{
					return new NextFixtureInfo
					{
						leaguedataid = null,
						date = schedule[i].Date,
						fixture = schedule[i].Fixtures[j]
					};
				}
			}
		}
		return null;
	}

	public NextFixtureInfo GetLastFixture(short clubid, int? comptype = null)
	{
		NextFixtureInfo result = null;
		if (clubid == 0)
		{
			return null;
		}
		Club.GetAssociationID(clubid);
		DateTime currentDate = CurrentDate;
		FixtureSetList schedule = g_season.Schedule.schedule;
		int num = schedule.Count();
		for (int i = 0; i < num; i++)
		{
			if (!(schedule[i].Date < currentDate))
			{
				continue;
			}
			int count = schedule[i].Fixtures.Count;
			for (int j = 0; j < count; j++)
			{
				if ((schedule[i].Fixtures[j].homeClubId == clubid || schedule[i].Fixtures[j].awayClubId == clubid) && (!comptype.HasValue || schedule[i].Fixtures[j].competitiontype == comptype))
				{
					result = new NextFixtureInfo
					{
						leaguedataid = null,
						date = schedule[i].Date,
						fixture = schedule[i].Fixtures[j]
					};
				}
			}
		}
		return result;
	}

	public void SetStartDate(int LeagueDataID)
	{
		LogConsole("Game::setStartDate - leaguedataid: " + LeagueDataID);
		StartDate = DateTime.MinValue;
		StartDate = Association.GetFirstSeasonDetails(g_LeagueData[LeagueDataID].AssociationID).StartDate;
		StartDate = StartDate.AddDays(-1.0);
		StartDate = new DateTime(StartDate.Year, StartDate.Month, StartDate.Day, 14, 0, 0);
		CurrentDate = StartDate;
	}

	public static void ReInitialiseCups(List<int> assocsToInitialise)
	{
		Season season = instance.g_season;
		_ = season.SeasonID;
		foreach (int item in assocsToInitialise)
		{
			string code = CountryData.GetCode(item);
			Constants.CONST_EXTRA_CUP_COUNTRY_CODES.Contains(code);
			List<Cup> list = new List<Cup>();
			for (int i = 0; i < season.Cups.Count; i++)
			{
				Cup cup = season.Cups[i];
				if ((cup.CupOrderIndex == 2 || cup.CupOrderIndex == 3) && cup.InCup.Count == 1)
				{
					list.Add(cup);
				}
			}
			for (int j = 0; j < list.Count; j++)
			{
				season.Cups.Remove(list[j]);
			}
			if (item == 2 || item == 1)
			{
				continue;
			}
			foreach (int key in instance.g_CupData.Keys)
			{
				if (instance.g_CupData[key].CountryCode == code)
				{
					Cup.Initialise(key, item, season.SeasonID, instance.g_CupData[key].SubType);
				}
			}
		}
	}

	public static bool ReInitialiseContinentalCompetitions(DateTime currentdate, int currentMonth)
	{
		bool flag = true;
		bool result = false;
		foreach (ContinentalComp value in instance.g_continentalCompetitions.Values)
		{
			if (value == null || value.competitionid == 7)
			{
				continue;
			}
			int num = 1;
			if (ContinentalComp.IsCompetitionEuropean(value))
			{
				num = 2;
			}
			if (value.startmonth != currentMonth + num || value.competitionid == 4)
			{
				continue;
			}
			int num2 = value.continents[0];
			foreach (int key in instance.g_CountryData.Keys)
			{
				if (!instance.g_CountryData[key].Popular || CountryData.GetContinent(key) != num2)
				{
					continue;
				}
				List<string> allSeasonIDsForDate = Association.GetAllSeasonIDsForDate(key, currentdate.AddMonths(1));
				if (allSeasonIDsForDate.Count <= 0)
				{
					continue;
				}
				string text = allSeasonIDsForDate[allSeasonIDsForDate.Count - 1];
				if (value.SeasonID != text)
				{
					if (flag)
					{
						foreach (ContinentalComp value2 in instance.g_continentalCompetitions.Values)
						{
							value2.Reset();
						}
						flag = false;
					}
					value.CreateCompetition(text);
					if (CountryData.GetContinent(Club.GetAssociationID(GetCurrentUserClubID())) == num2)
					{
						LogConsole("RESETTING CONTINENTAL COMP FOR " + CountryData.GetName(key));
						result = true;
					}
				}
				else
				{
					LogConsole("Game::initialiseContinentalCompetitions() - continental comp has already been inited for new season (" + text + ") - breaking out");
				}
				break;
			}
		}
		return result;
	}

	private static void _DebugMatchDayMoraleChange(int playerId, bool isAvailable, int moraleChange, bool isTopPlayer, bool isBigMatch, Constants.MatchOutcome outcome, int performance, int matchEventMorale)
	{
		string text = "";
		switch (matchEventMorale)
		{
		case 0:
			text = ((performance == 0) ? "Did not participate in match, so is no change in morale. " : ("Played in match which is the team " + outcome switch
			{
				Constants.MatchOutcome.Won => "Won, so chance to increase in morale (" + moraleChange + ")", 
				Constants.MatchOutcome.Drawn => "Drawn, so no change in morale (" + moraleChange + ")", 
				_ => "Lost, so chance to decrease in morale (" + moraleChange + ")", 
			}));
			if ((isBigMatch && performance == 0) & isTopPlayer & isAvailable)
			{
				text = "Is a top player who did not participate in a big game, so chance to decrease in morale (" + moraleChange + ")";
			}
			else if (isBigMatch && performance > 0 && outcome != Constants.MatchOutcome.Lost && !isTopPlayer)
			{
				text = "Is not a top player who played a part in a big game, which wasn't a defeat, so increase in morale (" + moraleChange + ")";
			}
			break;
		default:
			text = "Error";
			break;
		case 1:
			text = "Player was sent off in the match, so decrease morale (" + moraleChange + ")";
			break;
		case 2:
			text = "Player was injured in the match, so decrease morale (" + moraleChange + ")";
			break;
		case 3:
			text = "Player had a great game, so increase morale (" + moraleChange + ")";
			break;
		case 4:
			text = "Player had an awful game, so decrease morale (" + moraleChange + ")";
			break;
		case 5:
			text = "Player had an bad game, so POSSIBLY decrease morale (" + moraleChange + ")";
			break;
		}
		LogConsole(Player.GetName(playerId) + " (" + playerId + "). perf = " + performance + ". " + text);
	}

	public static Fixture ClubHasUnplayedMatchToday(int clubid)
	{
		return ClubHasUnplayedMatchOnDate(clubid, instance.CurrentDate);
	}

	public static Fixture ClubHasUnplayedMatchOnDate(int clubid, DateTime thedate)
	{
		if (clubid == 0)
		{
			return null;
		}
		DateTime dateTime = Helpers.TruncateDate(thedate);
		DateTime dateTime2 = dateTime.AddDays(1.0);
		dateTime2.AddSeconds(-60.0);
		FixtureSetList schedule = instance.g_season.Schedule.schedule;
		int num = schedule.Count();
		for (int i = 0; i < num; i++)
		{
			if (schedule[i].Date >= dateTime2)
			{
				return null;
			}
			if (!(schedule[i].Date >= dateTime) || !(schedule[i].Date <= dateTime2))
			{
				continue;
			}
			List<Fixture> fixtures = schedule[i].Fixtures;
			int count = fixtures.Count;
			for (int j = 0; j < count; j++)
			{
				Fixture fixture = fixtures[j];
				if (!fixture.IsPlayed && (fixture.homeClubId == clubid || fixture.awayClubId == clubid))
				{
					return fixture;
				}
			}
		}
		return null;
	}

	public static Fixture AssociationHasUnplayedMatchToday(DateTime thedate)
	{
		DateTime dateTime = thedate.AddDays(-1.0);
		dateTime.AddSeconds(-60.0);
		FixtureSetList schedule = instance.g_season.Schedule.schedule;
		int num = schedule.Count();
		for (int i = 0; i < num; i++)
		{
			if (!(schedule[i].Date >= dateTime) || !(schedule[i].Date <= thedate))
			{
				continue;
			}
			List<Fixture> fixtures = schedule[i].Fixtures;
			int count = fixtures.Count;
			for (int j = 0; j < count; j++)
			{
				Fixture fixture = fixtures[j];
				if (!fixture.IsPlayed)
				{
					return fixture;
				}
			}
		}
		return null;
	}

	public static bool AssociationHasUnplayedMatch(int associd)
	{
		return AssociationHasUnplayedMatch(associd, DateTime.MinValue);
	}

	public static bool AssociationHasUnplayedMatch(int associd, DateTime thedate)
	{
		return AssociationHasUnplayedMatch(associd, thedate, null);
	}

	public static bool AssociationHasUnplayedMatch(int associd, DateTime thedate, bool? includeContinentalComps)
	{
		if (!instance.g_association.ContainsKey(associd))
		{
			return false;
		}
		if (!includeContinentalComps.HasValue)
		{
			includeContinentalComps = true;
		}
		if (thedate == DateTime.MinValue)
		{
			thedate = instance.CurrentDate;
		}
		FixtureSet fixtureSet = Season.FindFixturesForDate(instance.g_season.Schedule.schedule, thedate);
		if (fixtureSet == null)
		{
			return false;
		}
		int count = fixtureSet.Fixtures.Count;
		for (int i = 0; i < count; i++)
		{
			if (!fixtureSet.Fixtures[i].IsPlayed && (fixtureSet.Fixtures[i].competitiontype != 4 || includeContinentalComps.Value))
			{
				return true;
			}
		}
		return false;
	}

	public DateTime getLastLoanDate(int id)
	{
		if (g_clubdata_lastloandate.ContainsKey(id))
		{
			return g_clubdata_lastloandate[id];
		}
		return DateTime.MinValue;
	}

	private List<int> calculatePointsForManagerAward(int club1Rating, int club2Rating, int club1Score, int club2Score)
	{
		int item;
		int item2;
		if (club1Rating > club2Rating)
		{
			if (club1Score > club2Score)
			{
				item = 3;
				item2 = 0;
			}
			else if (club2Score > club1Score)
			{
				item = 0;
				item2 = 5;
			}
			else
			{
				item = 1;
				item2 = 2;
			}
		}
		else if (club2Rating > club1Rating)
		{
			if (club2Score > club1Score)
			{
				item = 0;
				item2 = 3;
			}
			else if (club1Score > club2Score)
			{
				item = 5;
				item2 = 0;
			}
			else
			{
				item2 = 1;
				item = 2;
			}
		}
		else if (club1Score > club2Score)
		{
			item = 3;
			item2 = 0;
		}
		else if (club2Score > club1Score)
		{
			item = 0;
			item2 = 3;
		}
		else
		{
			item = 1;
			item2 = 1;
		}
		return new List<int> { item, item2 };
	}

	private DateTime getTeamAwardMinDate(bool isSeason)
	{
		DateTime dateTime = CurrentDate.AddSeconds(-2419200.0);
		if (isSeason)
		{
			return Association.GetSeasonStartDate(Club.GetAssociationID(GetCurrentUserClubID()), instance.g_season.SeasonID);
		}
		DateTime dateTime2 = dateTime;
		return dateTime2.AddDays(-(dateTime2.Day - 1));
	}

	public Color getColourForText(float Value)
	{
		if (Value <= 2f)
		{
			return redTextColour;
		}
		if (Value <= 4f)
		{
			return redTextColour;
		}
		if (Value <= 6f)
		{
			return orangeTextColour;
		}
		if (Value <= 8f)
		{
			return oliveTextColour;
		}
		return greenTextColour;
	}

	public Color getColourForFitnessText(float Value)
	{
		if (Value <= 60f)
		{
			return redTextColourFitness;
		}
		if (Value <= 60f)
		{
			return darkOrangeColourFitness;
		}
		if (Value <= 70f)
		{
			return orangeTextColourFitness;
		}
		if (Value <= 75f)
		{
			return darkerYellowTextColourFitness;
		}
		if (Value <= 80f)
		{
			return brigherYellowTextColourFitness;
		}
		if (Value <= 90f)
		{
			return slightGreenTextColourFitness;
		}
		if (Value <= 95f)
		{
			return brighterGreenTextColourFitness;
		}
		return greenTextColourFitness;
	}

	public double getIncome()
	{
		double num = 0.0;
		foreach (BalanceSheet item in CurrentManagedClub.BalanceSheet)
		{
			num += item.GateReceipts + item.Merchandise + item.OtherIncome + item.PrizeMoney + item.SeasonTicketReceipts + item.Sponsor + item.TransfersIn + item.TVRevenue;
		}
		return num;
	}

	public double getWeekIncome()
	{
		if (CurrentManagedClub.BalanceSheet.Count >= 2)
		{
			BalanceSheet balanceSheet = CurrentManagedClub.BalanceSheet[CurrentManagedClub.BalanceSheet.Count - 2];
			return balanceSheet.GateReceipts + balanceSheet.Merchandise + balanceSheet.OtherIncome + balanceSheet.PrizeMoney + balanceSheet.SeasonTicketReceipts + balanceSheet.TransfersIn + balanceSheet.TVRevenue;
		}
		return 0.0;
	}

	public void SetClubPoints(int points)
	{
		clubPoints = points;
	}

	private IEnumerator ShowXPToastAfterMatch(ManagerPointAward award)
	{
		yield return new WaitForEndOfFrame();
		if (award.ReasonTextKey != null)
		{
			ModalController.Instance().showXPToast(Translation.GetText(award.ReasonTextKey) + " ", award.ManagerPoints);
		}
		else
		{
			ModalController.Instance().showXPToast("", award.ManagerPoints);
		}
	}

	public float GetCashAmount(long cashAmount)
	{
		_ = instance.CurrentManagedClub.currentTactics.inst.formationid;
		new Team().clubid = instance.CurrentManagedClubId;
		int num = (int)Shop.AverageTeamRatingForShopPrices();
		float num2 = (float)cashAmount * Mathf.Pow(((float)num + 1f) / 100f, 15f);
		double num3 = ((!(num2 < 250000f)) ? (Math.Round((double)num2 / 250000.0) * 250000.0) : (Math.Round((double)num2 / 1000.0) * 1000.0));
		return (float)num3;
	}

	public void ResumeGameAfterMatch()
	{
		if (ReturningFromMatchday)
		{
			ui = GameObject.Find("UI").GetComponent<UI>();
			ui.CheckForOverlayElement(Constants.OverlayElement.TOP_BAR);
			ui.CheckForOverlayElement(Constants.OverlayElement.SIDE_BAR);
			ui.CheckForOverlayElement(Constants.OverlayElement.BOTTOM_BAR);
			matchReportVisible = true;
			GameObject.Find("SceneManager/CameraAnchor/GameCamera").GetComponent<Camera>().enabled = true;
			GameObject.Find("SceneManager/CameraAnchor/GameCamera").GetComponent<AudioListener>().enabled = true;
			Matchday.ProcessAfterMatchDay();
			ui.ShowNormalModeBackground();
			NumGamesPlayedThisSession++;
			NumGamesPlayedThisSave++;
			PlayerPrefs.SetInt("gamesplayed", PlayerPrefs.GetInt("gamesplayed", 0) + 1);
			AdsController.Instance.matchPlayedCurSession++;
			Helpers.LogConsole("###AdsController(). -- Updating matchPlayedCurSession ---- " + AdsController.Instance.matchPlayedCurSession);
			ReviewController.Instance.IncrementMatchPlayedTotal();
			AudioController.Instance.changeVolume(PlayerPrefs.GetInt("Music") == 1);
			int value = PlayerPrefs.GetInt("matcheswithoutrewardad", 4) + 1;
			PlayerPrefs.SetInt("matcheswithoutrewardad", value);
			TrackMatchCompleteEvent();
			PlayerPrefs.Save();
			StartCoroutine(setupScreenAftermatch());
			StadiumGenerator.seatsMaterial = null;
			StadiumGenerator.seatsMaterialAway = null;
			StadiumGenerator.crowdMaterial = null;
			StadiumGenerator.pitchMaterial = null;
		}
	}

	private void TrackMatchCompleteEvent()
	{
		Dictionary<string, string> parameters = new Dictionary<string, string>
		{
			{
				"clubName",
				EventLoggingParam.GetClubNameParamValue()
			},
			{
				"matchesPlayed",
				EventLoggingParam.GetMatchesPlayedParamValue()
			},
			{
				"currentLeague",
				EventLoggingParam.GetLeagueNameParamValue()
			},
			{
				"startingLeague",
				EventLoggingParam.GetStartingLeagueParamValue()
			},
			{
				"gameMode",
				EventLoggingParam.GetGameModeParamValue()
			}
		};
		EventLoggingController.LogEvent("matchcomplete", parameters);
		string text = "";
		switch (PlayerPrefs.GetInt("gamesplayed", 0))
		{
		case 5:
			text = "matchcomplete_x5";
			break;
		case 10:
			text = "matchcomplete_x10";
			break;
		case 25:
			text = "matchcomplete_x25";
			break;
		case 50:
			text = "matchcomplete_x50";
			break;
		case 100:
			text = "matchcomplete_x100";
			break;
		case 150:
			text = "matchcomplete_x150";
			break;
		case 250:
			text = "matchcomplete_x250";
			break;
		case 500:
			text = "matchcomplete_x500";
			break;
		}
		if (!string.IsNullOrEmpty(text))
		{
			EventLoggingController.LogEvent(text, "clubName", EventLoggingParam.GetClubNameParamValue());
		}
	}

	public IEnumerator setupScreenAftermatch()
	{
		ui = GameObject.Find("UI").GetComponent<UI>();
		GameObject matchReportScreen = UI.screens[35];
		matchReportScreen.transform.Find("TopPanel/Continue").gameObject.SetActive(value: true);
		matchReportScreen.transform.Find("TopPanel/Continue").GetComponent<Button>().interactable = false;
		matchReportScreen.transform.Find("TopPanel/Continue/Text").GetComponent<Text>().text = Translation.GetText("loading").ToUpper();
		yield return new WaitForSeconds(3f);
		ui.DrawInGameUI();
		ReturningFromMatchday = false;
		matchReportScreen.transform.Find("TopPanel/Continue/Text").GetComponent<Text>().text = Translation.GetText("continue").ToUpper();
		matchReportScreen.transform.Find("TopPanel/Continue").GetComponent<Button>().interactable = true;
		if (instance.CurrentManagedClub.MatchdayType == 0)
		{
			GameObject.Find("matchPreviewScreen").SetActive(value: false);
		}
		if (instance.DEBUG_TOOLS_PROCESSING_MATCH_ENGINE)
		{
			instance.StartCoroutine(DebugTools.ContinueAfterMatchEngine());
		}
		yield return null;
	}

	public int playerDataIDToIndex(int playerDataID)
	{
		int result = 0;
		if (g_playerDataIDLookup != null && g_playerDataIDLookup.Count > 0)
		{
			if (g_playerDataIDLookup.ContainsKey(playerDataID))
			{
				result = g_playerDataIDLookup[playerDataID];
			}
		}
		else
		{
			foreach (KeyValuePair<int, PlayerData> g_PlayerDatum in g_PlayerData)
			{
				if (playerDataID == g_PlayerDatum.Value.PlayerDataID)
				{
					result = g_PlayerDatum.Key;
					break;
				}
			}
		}
		return result;
	}

	public void SquadHealthCheck()
	{
		short currentUserClubID = GetCurrentUserClubID();
		PlayerSquad players = Club.GetPlayers(currentUserClubID);
		List<int> list = new List<int>();
		NextFixtureInfo nextFixture = instance.GetNextFixture(currentUserClubID);
		int num = players.Count;
		int num2 = 0;
		int num3 = 0;
		int num4 = 0;
		int num5 = 0;
		int num6 = 0;
		bool flag = false;
		int num7 = 0;
		for (int i = 0; i < players.Count; i++)
		{
			int num8 = players[i];
			if (!Player.IsInjured(num8) && (nextFixture == null || !Player.IsSuspended(num8, Player.GetAssociationID(num8), nextFixture.fixture.competitiontype, nextFixture.fixture.competitionid)))
			{
				if (Player.GetFirstTeam(num8))
				{
					if (Player.GetPosition(num8) == 1)
					{
						num4++;
					}
					num2++;
				}
				else
				{
					list.Add(num8);
				}
				if (Player.GetPosition(num8) == 1)
				{
					num3++;
				}
				if (flag && PlayerData.GetCountryDataID(num8) == 42 && Player.GetPosition(num8) != 1)
				{
					num7++;
				}
			}
			if (PlayerData.IsLoanedToClub(num8))
			{
				num6++;
				if (Player.GetPosition(num8) == 1)
				{
					num5++;
				}
			}
		}
		list.Sort((int x, int y) => Player.GetRatingQuick(y).CompareTo(Player.GetRatingQuick(x)));
		if (num3 - num5 < 2)
		{
			int item = PlayerRespawn.giveRespawnToManagedClub(currentUserClubID, 1);
			list.Add(item);
			num++;
		}
		if (num4 < 2)
		{
			for (int num9 = 0; num9 < list.Count; num9++)
			{
				int num10 = list[num9];
				if (Player.GetPosition(num10) == 1)
				{
					Player.SetFirstTeam(num10);
					list.Remove(num10);
					num2++;
					break;
				}
			}
		}
		if (num - num6 < 22)
		{
			int num11 = 22 - (num - num6);
			for (int num12 = 0; num12 < num11; num12++)
			{
				int item2 = PlayerRespawn.giveRespawnToManagedClub(currentUserClubID, Constants.outfieldPos[Random.GetRand() % 3]);
				list.Add(item2);
				num7++;
			}
		}
		if (flag)
		{
			int num13 = 15 - num7;
			for (int num14 = 0; num14 < num13; num14++)
			{
				int item3 = PlayerRespawn.giveRespawnToManagedClub(currentUserClubID, Constants.outfieldPos[Random.GetRand() % 3]);
				list.Add(item3);
			}
		}
		if (flag)
		{
			int num15 = 0;
			List<int> list2 = new List<int>();
			List<int> list3 = new List<int>();
			int num16 = 15;
			for (int num17 = 0; num17 < players.Count; num17++)
			{
				int num18 = players[num17];
				bool firstTeam = Player.GetFirstTeam(num18);
				if (PlayerData.GetPosition(num18) == 1)
				{
					continue;
				}
				if (!Player.IsInjured(num18) && (nextFixture == null || !Player.IsSuspended(num18, Player.GetAssociationID(num18), nextFixture.fixture.competitiontype, nextFixture.fixture.competitionid)) && PlayerData.GetCountryDataID(num18) == 42)
				{
					if (firstTeam)
					{
						num15++;
					}
					else
					{
						list3.Add(num18);
					}
				}
				else
				{
					list2.Add(num18);
				}
			}
			if (num15 < num16)
			{
				list2.Sort((int x, int y) => Player.GetRatingQuick(x).CompareTo(Player.GetRatingQuick(y)));
				list3.Sort((int x, int y) => Player.GetRatingQuick(y).CompareTo(Player.GetRatingQuick(x)));
				for (int num19 = 0; num19 < num16 - num15; num19++)
				{
					Player.SetReserveTeam(list2[num19]);
					Player.SetFirstTeam(list3[num19]);
				}
			}
		}
		if (num2 >= 20)
		{
			return;
		}
		int num20 = 20 - num2;
		for (int num21 = 0; num21 < num20; num21++)
		{
			if (list.Count > 0)
			{
				Player.SetFirstTeam(list[0]);
				list.RemoveAt(0);
			}
		}
	}

	public void SeasonLogHealthCheck()
	{
		Season season = g_season;
		if (season.CountriesBetweenSeasons.Count <= 0)
		{
			return;
		}
		for (int i = 0; i < season.CountriesBetweenSeasons.Count; i++)
		{
			int num = season.CountriesBetweenSeasons[i];
			List<string> allSeasonIDsForDate = Association.GetAllSeasonIDsForDate(num, instance.CurrentDate);
			string seasonid = allSeasonIDsForDate[allSeasonIDsForDate.Count - 1];
			if (!Season.SeasonLogExists(num, seasonid))
			{
				instance.processEndOfSeasonErrorLogMessage = "Season log never got created for countryID " + num + " in Season::ProcessEndOfSeason()";
				instance.showErrorReportBugModal = true;
				season.CountriesBetweenSeasons.Remove(num);
				Season.ProcessEndOfSeason(num);
			}
		}
	}

	public static void CheckManagedClubPlayers()
	{
		short currentUserClubID = GetCurrentUserClubID();
		PlayerSquad players = Club.GetPlayers(currentUserClubID);
		for (int i = 0; i < players.Count; i++)
		{
			int num = players[i];
			if (instance.g_PlayerData.ContainsKey(num) && instance.g_PlayerData[num].ClubId != currentUserClubID)
			{
				short clubId = instance.g_PlayerData[num].ClubId;
				PlayerSquad.RemovePlayer(clubId, num);
				instance.g_PlayerData[num].ClubId = currentUserClubID;
				Transfers.instance.historyManager.RemoveDuplicateClubForPlayerEntries(num, clubId);
			}
		}
	}

	public static void LogConsole(string log)
	{
	}

	public static void LogEvent(eNotifyableEvent WhichEvent)
	{
		switch (WhichEvent)
		{
		case eNotifyableEvent.eTransferWindowOpen:
			EventLoggingController.LogEvent("transferwindowopen");
			NotificationController.ScheduleNotification(43200, GameName, Translation.GetText("push-notification-transfer-window-open-1"), 301);
			break;
		case eNotifyableEvent.eTransferWindowShut:
			EventLoggingController.LogEvent("transferwindowshut");
			NotificationController.CancelScheduledNotificationsByType(302);
			NotificationController.CancelScheduledNotificationsByType(302);
			break;
		case eNotifyableEvent.eTransferDeadlineApproaching:
			NotificationController.ScheduleNotification(21600, GameName, Translation.GetText("push-notification-transfer-deadline"), 302);
			break;
		case eNotifyableEvent.eTransferCashTransfer:
			EventLoggingController.LogEvent("playerSigned_cashTransfer");
			break;
		}
	}

	public void trackPlayerTransfer(int playerID, short clubToID, short clubFromID, int value, short managedClubId)
	{
		if (managedClubId == clubToID && value != -2 && value != -3 && value != -1)
		{
			EventLoggingController.LogEvent("playerSigned");
			if (clubFromID == 0)
			{
				EventLoggingController.LogEvent("playerSigned_freeTransfer");
			}
			if (Player.GetAge(playerID) < 21)
			{
				EventLoggingController.LogEvent("playerSigned_ageUnder21");
			}
			else if (Player.GetAge(playerID) >= 21 && Player.GetAge(playerID) < 25)
			{
				EventLoggingController.LogEvent("playerSigned_age21_24");
			}
			else if (Player.GetAge(playerID) >= 26 && Player.GetAge(playerID) <= 30)
			{
				EventLoggingController.LogEvent("playerSigned_age26_30");
			}
			else
			{
				EventLoggingController.LogEvent("playerSigned_over30");
			}
			switch ((Constants.PlayerPosition)Player.GetGenericPositionMask(Player.GetPosition(playerID)))
			{
			case Constants.PlayerPosition.PLAYER_POS_GK:
				EventLoggingController.LogEvent("playerSigned_GK");
				break;
			case Constants.PlayerPosition.PLAYER_POS_D:
				EventLoggingController.LogEvent("playerSigned_Def");
				break;
			case Constants.PlayerPosition.PLAYER_POS_ANY_MID:
				EventLoggingController.LogEvent("playerSigned_Mid");
				break;
			case Constants.PlayerPosition.PLAYER_POS_F:
				EventLoggingController.LogEvent("playerSigned_Att");
				break;
			}
		}
		else if (managedClubId == clubFromID && value != -2 && value != -3)
		{
			EventLoggingController.LogEvent("playerSold");
			EventLoggingController.LogEvent("playerSold_cashTransfer");
			if (Player.GetAge(playerID) < 21)
			{
				EventLoggingController.LogEvent("playerSold_ageUnder21");
			}
			else if (Player.GetAge(playerID) >= 21 && Player.GetAge(playerID) < 25)
			{
				EventLoggingController.LogEvent("playerSold_age21_24");
			}
			else if (Player.GetAge(playerID) >= 26 && Player.GetAge(playerID) <= 30)
			{
				EventLoggingController.LogEvent("playerSold_age26_30");
			}
			else
			{
				EventLoggingController.LogEvent("playerSold_over30");
			}
			switch (Player.GetGenericPositionMask(Player.GetPosition(playerID)))
			{
			case 1:
				EventLoggingController.LogEvent("playerSold_GK");
				break;
			case 14:
				EventLoggingController.LogEvent("playerSold_Def");
				break;
			case 8176:
				EventLoggingController.LogEvent("playerSold_Mid");
				break;
			case 57344:
				EventLoggingController.LogEvent("playerSold_Att");
				break;
			}
		}
		else if (managedClubId == clubToID && value == -1)
		{
			EventLoggingController.LogEvent("playerSigned_Loan");
		}
		else if (managedClubId == clubFromID && value == -1)
		{
			EventLoggingController.LogEvent("playerSold_Loan");
		}
	}

	public static bool CanHaveSpecialOffer()
	{
		bool result = false;
		if (!Constants.FULL_SHOP_ENABLED)
		{
			return false;
		}
		if (CustomerDataController.Instance.GetSpecialOfferEndTime() > CustomerDataController.Instance.ServerTime)
		{
			result = true;
		}
		if (CustomerDataController.Instance.HasBoughtCurrentSpecialOffer())
		{
			result = false;
		}
		return result;
	}
}
