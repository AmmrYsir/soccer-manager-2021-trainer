using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SeController : MonoBehaviour
{
	public enum MatchViewType
	{
		View_3D = 1,
		View_2D
	}

	public enum CameraMode
	{
		TV,
		Panoramic,
		Overview,
		Stand
	}

	public enum eReplayType
	{
		Enabled = 1,
		Disabled
	}

	public enum eMatchReplayPhase
	{
		NONE,
		PART_1,
		PART_2
	}

	public enum eReplayCameraID
	{
		REPLAY_FOLLOW_BALL,
		REPLAY_CROWD,
		REPLAY_CROWD_BEHIND,
		REPLAY_BEHIND_GOAL,
		REPLAY_OPPOSITE_END,
		REPLAY_GOAL_LINE,
		REPLAY_ABOVE_GOAL
	}

	private enum MatchView
	{
		Pitch,
		Overview,
		Tactics
	}

	private enum StatsView
	{
		Statistics,
		LiveScores,
		LiveTable,
		OtherGames
	}

	private enum MatchPerspective
	{
		Overview,
		Real
	}

	public class KeeperAnimation
	{
		public cPlayer keeper;

		public bool wrongway;

		public bool catching;

		public float distanceToBall = 15f;

		public float time;

		public KeeperAnimation(cPlayer player, bool wrong)
		{
			keeper = player;
			wrongway = wrong;
		}

		public KeeperAnimation(cPlayer player, bool catchit, bool wrong)
		{
			keeper = player;
			wrongway = wrong;
			catching = catchit;
		}

		public void SetKeeperDiveTime(float timeToDive)
		{
			time = timeToDive;
		}
	}

	public enum ObjectBeingControlled
	{
		PLAYER,
		SET_PIECE_TARGET
	}

	public enum PlayerAnimLayers
	{
		BASE_LAYER,
		UPPER_BODY,
		UPPER_BODY_NO_ROOT,
		REFEREE,
		LINESMAN,
		PHYSIO,
		READY_TO_ACT,
		ACTIONS_PREV,
		ACTIONS_CURR
	}

	public enum SkipReason
	{
		NONE,
		REFEREE,
		PHYSIO,
		SUB,
		GOAL_CELEBRATION,
		REPLAY,
		PREMATCH_LINEUP,
		POSTMATCH,
		HALFTIME,
		SUBS
	}

	public class PlayerVisuals
	{
		public GameObject playerObject;

		public Transform playerObjectTransform;

		public GameObject playerPivot;

		public Transform playerPivotTransform;

		public GameObject playerAnimObject;

		public GameObject playerDetails;

		public SkinnedMeshRenderer playerMesh;

		public PlayerDetailsCircleController playerCircle;

		public Animator playerAnimator;

		public Transform playerAnimatorTransform;

		public AnimatorOverrideController animatorOverrides;

		public IKControl playerIK;

		public Transform playerHead;

		public GameObject playerArmBand;

		public float playerHeight = 1f;
	}

	public enum PitchPlayerType
	{
		HOME_GOALKEEPER,
		AWAY_GOALKEEPER,
		HOME_GOALKEEPER_1,
		AWAY_GOALKEEPER_1,
		HOME_GOALKEEPER_2,
		AWAY_GOALKEEPER_2,
		HOME_GOALKEEPER_3,
		AWAY_GOALKEEPER_3,
		HOME_PLAYER,
		AWAY_PLAYER,
		REFEREE,
		PHYSIO,
		MEDIC,
		LINESMAN
	}

	public class pitchDebugPoint
	{
		public v3d point;

		public Color color = new Color(1f, 1f, 0f);

		public pitchDebugPoint(v3d p)
		{
			point = p;
		}

		public pitchDebugPoint(v3d p, Color c)
		{
			point = p;
			color = c;
		}
	}

	private class cVisualPlayerStats
	{
		public float performance;

		public Text playerRating;

		public float fitness;

		public Text playerFitness;

		public Transform playerRow;

		public GameObject playerSub;

		public bool playerOn = true;
	}

	public enum MatchResult
	{
		WON,
		DRAW,
		LOST
	}

	public enum PlayDirection
	{
		NONE,
		MOVING_LEFT_TO_RIGHT,
		MOVING_RIGHT_TO_LEFT
	}

	private static int NUM_PLAYERS = 18;

	public const int HIGHLIGHT_FULL = 0;

	public const int HIGHLIGHT_KEY = 1;

	public const int HIGHLIGHT_EXTENDED = 2;

	public const int HIGHLIGHT_COMMENTARY = 3;

	public const int HIGHLIGHT_DEBUG = 4;

	public const int HIGHLIGHT_NONE = 5;

	[HideInInspector]
	public cDebugMatch debugMatch = new cDebugMatch();

	[HideInInspector]
	public float debugReplayTime;

	private debugMatch debugMatchScript;

	[HideInInspector]
	public float debugReplayBaseTime;

	[HideInInspector]
	public float debugFastForwardTime;

	[HideInInspector]
	public float debugReplayDelta;

	[HideInInspector]
	public bool debugReplayMatch;

	[HideInInspector]
	public bool debugReplayInput;

	[HideInInspector]
	public bool debugReplayPause = true;

	[HideInInspector]
	public bool debugNoOffsides;

	[HideInInspector]
	public bool debugNoBotTackle;

	[HideInInspector]
	public bool debugNoBotSlide;

	[HideInInspector]
	public bool debugNoBotPass;

	[HideInInspector]
	public bool debugNoPassError;

	[HideInInspector]
	public bool debugNoSpaceballError;

	[HideInInspector]
	public bool debugNoCrossError;

	[HideInInspector]
	public bool debugNoCornerError;

	[HideInInspector]
	public bool debugNoLongballError;

	[HideInInspector]
	public bool debugNoShotError;

	[HideInInspector]
	public bool debugNoShotCurl;

	private const bool markingDefault = true;

	[HideInInspector]
	public bool debugMarkingEnabled = true;

	private const bool pressureDefault = true;

	[HideInInspector]
	public bool debugPressurePOBEnabled = true;

	[HideInInspector]
	public bool debugKnobbleBots;

	[HideInInspector]
	public float debugAutoShadowPOBDist = 3f;

	[HideInInspector]
	public bool debugReplayMouseHeld;

	private bool debugJoystickHeld;

	private Vector3 debugJoystickBase;

	[HideInInspector]
	public float debugReplayMouseXpos;

	[HideInInspector]
	public bool debugSpeedThrough;

	[HideInInspector]
	public bool debugSpotHome = true;

	[HideInInspector]
	public int debugSpotType = 1;

	[HideInInspector]
	public bool debugForcePenalty;

	[HideInInspector]
	public bool debugDeadballSpeedup = true;

	[HideInInspector]
	public bool debugSpotSide;

	[HideInInspector]
	public bool debugDribbleVelocity;

	[HideInInspector]
	public bool debugTriggerShootout;

	[HideInInspector]
	public Slider debugReplayTimeSlider;

	[HideInInspector]
	public Toggle debugReplayToggle;

	[HideInInspector]
	public Toggle debugRestartToggle;

	[HideInInspector]
	public Toggle debugInputToggle;

	[HideInInspector]
	public Toggle debugTeamToggle;

	[HideInInspector]
	public Vector3 debugJoystick;

	[HideInInspector]
	private int[] debugForceFouls = new int[2];

	[HideInInspector]
	private int debugForceInjuries;

	[HideInInspector]
	public float[] debugPlayerSpeedScale = new float[2] { 0.8f, 0.7f };

	[HideInInspector]
	public float debugGameSpeed = 1f;

	[HideInInspector]
	private bool debugUseAheadTime = true;

	[HideInInspector]
	public bool debugTestRotation = true;

	[HideInInspector]
	public bool debugSelectAction = true;

	[HideInInspector]
	private bool debugFormationsFromOppositionPositions = true;

	public const int testCorners = 0;

	public const int testThrowIns = 1;

	public const int testGoalKicks = 2;

	public const int giveAway = 3;

	[HideInInspector]
	public bool oldDebugReplayMatch;

	[HideInInspector]
	public bool olddebugReplayInput;

	[HideInInspector]
	public bool debugShowPlayerNames = true;

	private float debugTimeScale = 1f;

	public int seed = 2;

	public GameObject instantMatchObjects;

	public GameObject ballPrefab;

	public GameObject pitchPrefab;

	public GameObject medicPrefab;

	public GameObject physioPrefab;

	public GameObject refereeCardPrefab;

	private GameObject linesmanFlag;

	private Cloth[] linesmanFlagCloths = new Cloth[2];

	private Material refereeCardMaterial;

	private GameObject refereeCardInstance;

	public GameObject playerDetailsPrefab;

	private PlayerModelGenerator playerGen;

	private PlayerVisuals[] team1Players = new PlayerVisuals[NUM_PLAYERS];

	private PlayerVisuals[] team2Players = new PlayerVisuals[NUM_PLAYERS];

	private float highestPlayerSpeed;

	private float lowestPlayerSpeed;

	[HideInInspector]
	public bool refereeActionStarted = true;

	private int lastRefereeState;

	[HideInInspector]
	public PlayerVisuals refereeVisual;

	private int[] lastLinesmenStates = new int[2];

	[HideInInspector]
	public PlayerVisuals[] linesmenVisuals = new PlayerVisuals[2];

	private GameObject medic;

	private bool physioVisible = true;

	private int lastPhysioState;

	[HideInInspector]
	public PlayerVisuals physioVisual;

	private GameObject ball;

	public LineRenderer ballDebugLine;

	private GameObject ballAhead;

	private GameObject tvTarget;

	private GameObject panoramicTarget;

	private GameObject standTarget;

	private GameObject realBall;

	private GameObject pitch;

	[HideInInspector]
	public Transform ballVisual;

	public Transform ballLookTarget;

	private v3d ballPos;

	private v3d ballPosFuture;

	private v3d ballToGoalPos = new v3d();

	private const float CAMERA_GOAL_LOCK_DISTANCE = 256f;

	private int homeFormationIndex;

	private int awayFormationIndex = 1;

	private FormationPosData[] homeFormation;

	private FormationPosData[] awayFormation;

	private float width = 5f;

	private float length = 6.6f;

	private float zPos = 0.03f;

	private const float REAL_PITCH_W_SCALE = 1f;

	private const float REAL_PITCH_L_SCALE = 1f;

	private const float REAL_PITCH_H_SCALE = 1f;

	private const float REAL_PITCH_W_OFFSET = 0f;

	private const float REAL_PITCH_L_OFFSET = 0f;

	private const float REAL_PITCH_H_OFFSET = -0.05f;

	public cSoccerEngine sm;

	public cMatch match;

	public bool matchRunning = true;

	public float time;

	private float deltaTime;

	private float timeScale = 1f;

	private float eventTimeScale = 1f;

	private Quaternion ballRot = new Quaternion(0f, 0f, 0f, 1f);

	private bool enteringHighlight;

	private float liveTime;

	private int timeInt;

	private int timeIntPrev;

	private bool matchPausing;

	private bool matchPaused;

	private bool matchControlsShown;

	private int speedLevel = 4;

	private float[] matchSpeeds = new float[5] { 1f, 4f, 10f, 30f, 90f };

	private float topSpeed = 800f;

	public MatchViewType currentMatchView = MatchViewType.View_3D;

	public MatchViewType currentHighlightView = MatchViewType.View_3D;

	public CameraMode currentCameraMode;

	[HideInInspector]
	public eReplayType replaysEnabled = eReplayType.Enabled;

	[HideInInspector]
	public float lastReplayTime;

	[HideInInspector]
	public float replayBeginTime;

	[HideInInspector]
	public bool replayRequested;

	[HideInInspector]
	public float replayEndTime;

	[HideInInspector]
	public float replayResetTime;

	[HideInInspector]
	public eMatchReplayPhase currentReplayPhase;

	private GameObject replayUIOverlay;

	private bool prevInReplay;

	private bool currInReplay;

	[HideInInspector]
	public bool shownTransitionAnimation;

	[HideInInspector]
	public eReplayCameraID replayPart1Camera;

	[HideInInspector]
	public eReplayCameraID replayPart2Camera = eReplayCameraID.REPLAY_GOAL_LINE;

	[HideInInspector]
	public bool replayGoalLocation = true;

	private Vector2 matchViewTimes = new Vector2(0f, 0f);

	public int highlightMode = 1;

	public bool quickMatchShowingHighlight;

	private bool quickMatchHighlightsEnabled = true;

	private bool quickMatchReplaysEnabled = true;

	private Button quickGoalsButton;

	private Button quickReplaysButton;

	private float lastEventTime;

	private int lastEventCount = -1;

	public Sprite pauseIcon;

	public Sprite playIcon;

	public Sprite soundOnIcon;

	public Sprite soundOffIcon;

	public Sprite yellowCard;

	public Sprite redCard;

	public Sprite secondYellow;

	public Sprite goalIcon;

	private bool overviewModeOn;

	private bool tacticsOn;

	private bool playerStatsOn;

	private bool injuredPlayer;

	private bool playerSentOff;

	private bool shootout;

	[HideInInspector]
	public GameObject overviewScreen;

	private GameObject tacticsGameObject;

	[HideInInspector]
	public GameObject playerStatsScreen;

	private GameObject tacticsControls;

	[HideInInspector]
	public GameObject matchControlButtons;

	private GameObject topPanel;

	private GameObject overviewModePanel;

	private TacticsScreen tacticsScreen;

	private GameObject quickMatchScreen;

	private bool quickMatchScreenExists;

	[HideInInspector]
	public GameObject liveTable;

	[HideInInspector]
	public GameObject statistics;

	[HideInInspector]
	public GameObject liveScores;

	[HideInInspector]
	public GameObject allGames;

	private GameObject matchViewControls;

	private GameObject stats;

	private GameObject statsPanel;

	private GameObject quickMatchStatsPanel;

	private GameObject matchStatsPanel;

	private List<LiveScoresOutput> seLiveScores;

	private LiveScoresOutput currentPlayerFixtureStats;

	private List<LiveTableOutput> seLiveTable;

	private List<LiveScoresOutput> seAllGames;

	private int liveScoresCounter;

	private CanvasGroup[] scoreboard;

	private CanvasGroup commentaryBar;

	private bool narrowScreen = true;

	private const float SCOREBOARD_FADE_START = -30f;

	private const float SCOREBOARD_FADE_DISTANCE = -3f;

	private const float COMMENTARY_FADE_START = 25f;

	private const float COMMENTARY_FADE_DISTANCE = 3f;

	private const float NAME_FADE_START = 12f;

	private const float NAME_FADE_DISTANCE = 1f;

	private float commentaryBoxBackgroundAlpha = -1f;

	private bool commBarFlashing;

	private float flashTimer;

	private bool flashDimmed;

	private cCommentaryRes lastComRes;

	private MatchView currentView;

	private MatchView lastView;

	private MatchPerspective currentPerspective = MatchPerspective.Real;

	private string homeAway;

	private bool tacticsAllowed;

	private bool statsActive;

	private Toggle overviewToggle;

	private Color overviewOnColour;

	private Color overviewOffColour;

	private int currentCam;

	private Camera[] cameras;

	private int TOTAL_CAMERAS = 1;

	private bool showDebugMarkings;

	private bool halfTimeTacticsShown;

	private bool extraTimeTacticsShown;

	private bool extraHalfTimeTacticsShown;

	private bool matchStarted;

	private bool firstCommentaryShown;

	private bool matchIntroStarted;

	private bool matchOutroStarted;

	private bool matchFullTimeOutroStarted;

	private bool matchHalfTimeOutroStarted;

	private bool shootoutEndOutroStarted;

	[HideInInspector]
	public bool realPlayerSize = true;

	private bool completedFirstUpdate;

	private GameObject matchCanvas;

	private Canvas matchOverlayCanvas;

	private GameObject matchOverlay;

	private GameObject matchControls;

	private GameObject screenHolder;

	private GameObject skipOverlay;

	private GameObject homePlayersPanel;

	private GameObject awayPlayersPanel;

	private GameObject homePlayerTemplate;

	private GameObject homePlayerSubTemplate;

	private GameObject awayPlayerTemplate;

	private GameObject awayPlayerSubTemplate;

	private float timeSinceLastStatsUpdate = 0.6f;

	private bool matchFinished;

	private float matchFinishTime;

	private MatchAudio matchAudio;

	public SeFixtureInfo seFixInfo;

	private FixtureKitHelper fixtureKitHelper;

	private MatchTimeOfDay weatherController;

	private GameObject match3DUI;

	private GameObject canvasOverlay;

	[HideInInspector]
	public bool nightMatch;

	public bool useFloodLights;

	private bool isUltraThinScreen;

	private float screenMargin;

	private float desktopScreenWidth;

	private float desktopScreenHeight;

	private bool desktopResizeStarted;

	public Fixture g_matchday_nextfix;

	private short managedClubID;

	[HideInInspector]
	public string HomeColour1;

	[HideInInspector]
	public string HomeColour2;

	[HideInInspector]
	public string AwayHomeColour1;

	[HideInInspector]
	public string AwayHomeColour2;

	[HideInInspector]
	public string AwayColour1;

	[HideInInspector]
	public string AwayColour2;

	[HideInInspector]
	public string HomeLogoFilename;

	[HideInInspector]
	public string AwayLogoFilename;

	public ClubKitStyle homeKitStyle;

	public ClubKitStyle awayHomeKitStyle;

	public ClubKitStyle awayKitStyle;

	private RNG Random;

	public TacticInfo seTacticInfo;

	public TacticAction seTacticAction;

	[Space]
	[Header("Camera settings")]
	[Space]
	[Header("Manually change ball pos")]
	public bool debugBallMover;

	public Vector3 ballMoverPos;

	[Space]
	private Vector3 adjPosVisual = new Vector3(0f, 0f, 0f);

	private float dampedCameraPosZ;

	private float dampedCameraPosX;

	private float dampedCamerRot;

	private GameObject debugOptions;

	private bool debugOptionsVisible;

	private bool generatedSettings;

	private Transform debugScrollArea;

	private Transform debugVarsArea;

	private GameObject matchEditScreen;

	private bool inEditMode;

	private bool tacticsOpen;

	private System.Random globalRand = new System.Random();

	public bool IsSpectator;

	private bool isHome = true;

	private bool flipPitch;

	private bool swappedEnds;

	[HideInInspector]
	private cPlayer lastPlayerHoldingBall;

	private int lastToControl;

	private int lastTeamControlling;

	[HideInInspector]
	public bool dribbling;

	[HideInInspector]
	private int lastPlayerOnBall;

	private float cameraSmoothness = 0.6f;

	private float ballHeightMultiplier = 2.68f;

	[HideInInspector]
	public GameObject transformHelper;

	[HideInInspector]
	public GameObject transformHelper2;

	[HideInInspector]
	public GameObject transformHelper3;

	public GameObject crowdLookTarget;

	public ObjectBeingControlled controlledObject;

	public static SeController instance;

	private string[] teamStrings = new string[2] { "home", "away" };

	private GameObject homeStaticGoalNetting;

	private GameObject homeDynamicGoalNetting;

	private GameObject awayStaticGoalNetting;

	private GameObject awayDynamicGoalNetting;

	private GameObject positiveGoalDirReplayAimer;

	private GameObject negativeGoalDirReplayAimer;

	private Text homeScoreText;

	private Text awayScoreText;

	private Text homeScoreOverviewText;

	private Text awayScoreOverviewText;

	private Text homeQuickScoreText;

	private Text awayQuickScoreText;

	private GameObject playArrow;

	private bool inDeadballSpeedup;

	private bool enteredRealtime;

	private bool enteringRealtime;

	[HideInInspector]
	public static Vector3 localPos;

	[HideInInspector]
	public static Vector3 localEuler;

	[HideInInspector]
	public static Vector3 localScale;

	[HideInInspector]
	public static float UnityDeltaTime = 0f;

	public static readonly v3d centerSpotPos = new v3d();

	private bool skipButtonPressed;

	private bool skipActionPerformed;

	private bool skipButtonShown;

	private SkipReason currentSkip;

	private Vector3 adjPosAhead;

	private Vector3 adjPos;

	private string matchDataProjectFilePath = "/StreamingAssets/SavedMatchData.json";

	private Sprite homeClubLogoSprite;

	private Sprite awayClubLogoSprite;

	private Game fakeGame;

	private StadiumCrowd stadiumCrowd;

	private StadiumGenerator stadiumController;

	public Texture2D defaultBadge;

	private Texture2D homeBadge;

	private Texture2D awayBadge;

	private Texture2D leagueBadge;

	private Texture2D homeSponsor;

	private Texture2D awaySponsor;

	public Texture2D defaultSponsor;

	public Texture2D defaultLogo;

	public List<AnimationClip> playerIdles;

	private double currentPlayerRandomIdle;

	public cTactics homeTactics;

	public cTactics awayTactics;

	private const float TACTICS_SAMPLE_RATE = 1f;

	private float tacticsSampleTime = 1f;

	public cPlayer homeCaptain;

	public cPlayer awayCaptain;

	private bool started;

	private bool madeSub;

	private bool goalFixture;

	private cPlayer player1;

	private cPlayer player;

	private v3d pos;

	private cTeam team1;

	private cTeam currentTeam;

	private cTeam teamNotInPlay;

	private bool onBallSoon;

	private IKControl.IKLookTarget intendedIKLookTarget;

	private Transform intendedLookTarget;

	private bool forceLookTargetSet;

	private float homeCD1;

	private float homeCD2;

	private float awayCD1;

	private float awayCD2 = float.PositiveInfinity;

	private int homeCD1Pl;

	private int homeCD2Pl;

	private int awayCD1Pl;

	private int awayCD2Pl;

	private float ballRelativePosition;

	private float crowdStandDistance = 36f;

	private float crowdSitDistance = 39f;

	private float ballDistanceToGoal;

	private float crowdStandRadius = Mathf.Pow(34f, 2f);

	private float crowdSitRadius = Mathf.Pow(39f, 2f);

	public cManyMatches manyMatches = new cManyMatches();

	private bool setupStatsObjects;

	private static cPlayerStats[] homePlayers;

	private static cPlayerStats[] awayPlayers;

	private cVisualPlayerStats[] liveHomePlayers;

	private cVisualPlayerStats[] liveAwayPlayers;

	private const string EMPTY_STAT_STRING = "-";

	private ScrollRect homeQuickEvents;

	private ScrollRect awayQuickEvents;

	private bool setupQuickEventScrollRects;

	private int numElements;

	private GameObject debugSliderRow;

	private GameObject debugBoolRow;

	private GameObject debugDividerRow;

	private GameObject debugPrintRow;

	private GameObject debugPrintMultiline;

	private GameObject debugDropdownRow;

	private GameObject debugButtonRow;

	public Dictionary<int, Slider> sliders = new Dictionary<int, Slider>();

	private Dictionary<int, Toggle> toggles = new Dictionary<int, Toggle>();

	private Dictionary<int, Dropdown> dropdowns = new Dictionary<int, Dropdown>();

	private Dictionary<int, Text> vars = new Dictionary<int, Text>();

	private Dictionary<string, float> animationDelays = new Dictionary<string, float>();

	private Dictionary<string, float> animationSpeeds = new Dictionary<string, float>();

	private float cameraSpeedSmoothing = 3f;

	private float cameraSpeedChange;

	private Vector4 currentViewportLimits;

	private bool isSpecialCameraCase;

	private float cameraTargetSpeed;

	private bool penaltyCamSetUp;

	private MatchCamera currentPenaltyCam;

	private int penDir = 1;

	private const float REPLAY_START_DELAY = 3f;

	private const float REPLAY_PART_1_DURATION = 9f;

	private const float REPLAY_PART_2_DURATION = 7f;

	private cPlayer currCaptain;

	public int[] DebugForceFouls => debugForceFouls;

	public int DebugForceInjuries => debugForceInjuries;

	public bool UseNewF2B => debugFormationsFromOppositionPositions;

	[HideInInspector]
	public float TimeScale => timeScale * debugGameSpeed;

	public bool IsMatchPaused => matchPaused;

	public int SpeedLevel => speedLevel;

	public bool IsInReplay => currentReplayPhase != eMatchReplayPhase.NONE;

	public Vector2 MatchViewTimes => matchViewTimes;

	public bool InFastMode
	{
		get
		{
			if (highlightMode == 5)
			{
				return !quickMatchShowingHighlight;
			}
			return false;
		}
	}

	public bool AllowQuickMatchReplays => quickMatchReplaysEnabled;

	public bool HasFinishedMatch => matchFinished;

	public FixtureKitHelper FixtureKitHelper => fixtureKitHelper;

	public bool InEditMode => inEditMode;

	public bool IsHome
	{
		get
		{
			return isHome;
		}
		set
		{
			isHome = value;
			flipPitch = !value;
		}
	}

	public StadiumGenerator StadiumController => stadiumController;

	public Texture2D HomeBadge => homeBadge;

	public Texture2D AwayBadge => awayBadge;

	public Texture2D LeagueBadge => leagueBadge;

	public Texture2D HomeSponsor => homeSponsor;

	public Texture2D AwaySponsor => awaySponsor;

	public cTactics YourTactics
	{
		get
		{
			if (homeAway == "home")
			{
				return homeTactics;
			}
			return awayTactics;
		}
	}

	public cTactics OppTactics
	{
		get
		{
			if (homeAway != "home")
			{
				return homeTactics;
			}
			return awayTactics;
		}
	}

	public bool Started => started;

	private void Awake()
	{
		if (instance == null)
		{
			instance = this;
		}
		else if (instance != this)
		{
			UnityEngine.Object.Destroy(gameObject);
		}
		TacticsController.IsMatchTactics = true;
		if (Game.instance == null)
		{
			StartCoroutine(AsyncStart1());
		}
		else
		{
			StartSeController();
		}
	}

	private IEnumerator AsyncStart2()
	{
		string curUserID = CustomerDataController.Instance.GetCurUserID();
		PlayerPrefs.GetString("prefusername");
		UI.GetInstance().instantMatchMode = true;
		fakeGame = UnityEngine.Object.Instantiate(Resources.Load<GameObject>("GameContainer")).GetComponent<Game>();
		fakeGame.gameObject.name = "GameContainer";
		yield return StartCoroutine(Save.LoadData(0, curUserID));
		UI.GetInstance().overlayCanvas = GameObject.Find("OverlayCanvasMatch");
		yield return new WaitForSeconds(1f);
		StartSeController();
	}

	private IEnumerator AsyncStart1()
	{
		yield return new WaitForEndOfFrame();
		instantMatchObjects.SetActive(value: true);
		InitGameSplashScreen.InitialiseFlag.AnimationDone = true;
		while (!InitGameSplashScreen.InitialiseFlag.InitialisingDone())
		{
			yield return new WaitForSeconds(0.2f);
		}
		StartCoroutine(AsyncStart2());
	}

	private void StartSeController()
	{
		started = true;
		if (Game.instance != null && Game.instance.DEBUG_TOOLS_PROCESSING_MATCH_ENGINE)
		{
			instance.debugSpeedThrough = true;
		}
		UI.GetInstance().SetupUIMatch();
		localPos = base.transform.position;
		localEuler = base.transform.eulerAngles;
		localScale = base.transform.localScale;
		Application.targetFrameRate = 30;
		Screen.sleepTimeout = -1;
		if (!PlayerPrefs.HasKey("MatchCameraModeSetting"))
		{
			PlayerPrefs.SetInt("MatchCameraModeSetting", (int)currentCameraMode);
		}
		else
		{
			currentCameraMode = (CameraMode)PlayerPrefs.GetInt("MatchCameraModeSetting");
		}
		if (!PlayerPrefs.HasKey("MatchSpeed"))
		{
			PlayerPrefs.SetInt("MatchSpeed", speedLevel);
		}
		else
		{
			speedLevel = PlayerPrefs.GetInt("MatchSpeed");
		}
		if (!PlayerPrefs.HasKey("HighlightMode_1.0.5"))
		{
			PlayerPrefs.SetInt("HighlightMode_1.0.5", highlightMode);
		}
		else if (speedLevel == 5)
		{
			highlightMode = 5;
		}
		else
		{
			highlightMode = PlayerPrefs.GetInt("HighlightMode_1.0.5");
		}
		GameObject.Find("MatchCameras").GetComponent<MatchCameraController>().AwakenCameras(currentCameraMode);
		NUM_PLAYERS = 18;
		team1Players = new PlayerVisuals[NUM_PLAYERS];
		team2Players = new PlayerVisuals[NUM_PLAYERS];
		float num = 0.5625f;
		float num2 = 0f;
		_ = (float)(Screen.height + 111) / (float)Screen.width;
		float num3 = ((float)Screen.height + num2) / (float)Screen.width;
		float num4 = num3 / num;
		matchCanvas = GameObject.Find("OverlayCanvasMatch");
		GameObject gameObject = UnityEngine.Object.Instantiate(Resources.Load<GameObject>("UI/Desktop/OverlayCanvasMatch_Desktop"));
		gameObject.GetComponent<Canvas>().worldCamera = matchCanvas.GetComponent<Canvas>().worldCamera;
		matchCanvas.name = "OverlayCanvasMatch_Mobile";
		UnityEngine.Object.Destroy(GameObject.Find("OverlayCanvasMatch_Mobile"));
		matchCanvas = gameObject;
		matchCanvas.name = "OverlayCanvasMatch";
		UI.GetInstance().SetupUIMatch();
		overviewScreen = UnityEngine.Object.Instantiate(Resources.Load<GameObject>("UI/OverviewModeDesktop"), matchCanvas.transform, worldPositionStays: false);
		playerStatsScreen = UnityEngine.Object.Instantiate(Resources.Load<GameObject>("UI/PlayerStatsScreenDesktop"), matchCanvas.transform, worldPositionStays: false);
		matchOverlay = UnityEngine.Object.Instantiate(Resources.Load<GameObject>("UI/MatchOverlayDesktop"), matchCanvas.transform, worldPositionStays: false);
		matchControls = UnityEngine.Object.Instantiate(Resources.Load<GameObject>("UI/MatchControlsDesktop"), matchCanvas.transform, worldPositionStays: false);
		skipOverlay = UnityEngine.Object.Instantiate(Resources.Load<GameObject>("UI/MatchSkipOverlayDesktop"), matchCanvas.transform, worldPositionStays: false);
		tacticsGameObject = UnityEngine.Object.Instantiate(Resources.Load<GameObject>("Desktop/Screens/TacticsMatchScreen"), matchCanvas.transform, worldPositionStays: false);
		Canvas canvas = (Canvas)tacticsGameObject.AddComponent(typeof(Canvas));
		canvas.overrideSorting = true;
		canvas.sortingLayerID = 1;
		canvas.sortingOrder = 1;
		tacticsGameObject.AddComponent(typeof(GraphicRaycaster));
		tacticsGameObject.name = "TacticsScreen";
		debugOptions = UnityEngine.Object.Instantiate(Resources.Load<GameObject>("DebugOptions"), matchCanvas.transform, worldPositionStays: false);
		manyMatches.manyMatchesObj = UnityEngine.Object.Instantiate(Resources.Load<GameObject>("ManyMatches"), matchCanvas.transform, worldPositionStays: false);
		if (highlightMode == 5)
		{
			quickMatchScreen = UnityEngine.Object.Instantiate(Resources.Load<GameObject>("Desktop/Screens/QuickMatchOverlay"), matchCanvas.transform, worldPositionStays: false);
			quickMatchScreenExists = true;
		}
		overviewScreen.name = "OverviewMode";
		playerStatsScreen.name = "PlayerStatsScreen";
		matchOverlay.name = "MatchOverlay";
		matchControls.name = "MatchControls";
		skipOverlay.name = "MatchSkipOverlay";
		overviewScreen.SetActive(value: false);
		playerStatsScreen.SetActive(value: false);
		if (quickMatchScreenExists)
		{
			quickMatchScreen.name = "QuickMatchScreen";
		}
		skipOverlay.SetActive(value: false);
		topPanel = matchOverlay.transform.Find("MatchDayTopPanel").gameObject;
		tacticsControls = tacticsGameObject.transform.Find("ScreenContainer/ScreenContent/TacticsControls").gameObject;
		matchControlButtons = topPanel.transform.Find("MatchControls").gameObject;
		liveTable = overviewScreen.transform.Find("statsPanel/Table").gameObject;
		liveScores = overviewScreen.transform.Find("statsPanel/LatestScores").gameObject;
		allGames = overviewScreen.transform.Find("OverviewPanel/Right/Panel/AllGames").gameObject;
		tacticsGameObject.GetComponent<Canvas>().sortingLayerID = SortingLayer.NameToID("MatchOverview");
		tacticsGameObject.GetComponent<Canvas>().sortingOrder = 2;
		StartCoroutine(AdjustLast5PossesionForDesktop());
		matchControlButtons = matchOverlay.transform.Find("MatchDayTopPanel/MatchControls").gameObject;
		statsPanel = matchCanvas.transform.Find("PlayerStatsScreen/playerStatsPanel/Stats").gameObject;
		matchViewControls = matchCanvas.transform.Find("MatchOverlay/SideBarMatch").gameObject;
		seFixInfo = new SeFixtureInfo();
		SetMatchProperties();
		if (Game.instance != null && Constants.LIVE_SCORES_ENABLED)
		{
			GameObject gameObject2 = matchCanvas.transform.Find("OverviewMode").gameObject.transform.Find("statsPanel/Panel/LiveTableBtn").gameObject;
			gameObject2.SetActive(value: false);
			if (g_matchday_nextfix.competitiontype == 2)
			{
				AddStartGameDrawPoints();
				DrawLeagueTablesTesting(continentalCup: false);
				gameObject2.SetActive(value: true);
			}
			else if (g_matchday_nextfix.competitiontype == 4 && Game.instance.g_continentalCompetitions[g_matchday_nextfix.competitionid].Status == ContinentalCompStatus.GroupStage)
			{
				AddStartGameDrawPoints();
				DrawLeagueTablesTesting(continentalCup: true);
				gameObject2.SetActive(value: true);
			}
			DrawAllCurrentGamesPlayed();
		}
		statsPanel = matchCanvas.transform.Find("OverviewMode/OverviewPanel/Centre/Panel/PlayerRatings/Panel").gameObject;
		homePlayersPanel = statsPanel.transform.Find("HomeTeam").gameObject;
		awayPlayersPanel = statsPanel.transform.Find("AwayTeam").gameObject;
		tacticsControls.transform.Find("CancelBtn/Text").GetComponent<Text>().text = Translation.GetText("cancel").ToUpper();
		tacticsControls.transform.Find("ConfirmBtn/Text").GetComponent<Text>().text = Translation.GetText("continue").ToUpper();
		tacticsControls.transform.Find("ResetBtn/Text").GetComponent<Text>().text = Translation.GetText("reset").ToUpper();
		skipOverlay.transform.Find("Skip/SkipText").GetComponent<Text>().text = Translation.GetText("tap-to-skip");
		short clubDataID = seFixInfo.fixtureInfo.home.clubDataID;
		short clubDataID2 = seFixInfo.fixtureInfo.away.clubDataID;
		Transform transform = topPanel.transform;
		scoreboard = new CanvasGroup[3];
		scoreboard[0] = transform.Find("HomeTeam").GetComponent<CanvasGroup>();
		scoreboard[1] = transform.Find("AwayTeam").GetComponent<CanvasGroup>();
		scoreboard[2] = transform.Find("ScorePanel").GetComponent<CanvasGroup>();
		commentaryBar = GameObject.Find("OverlayCanvasMatch/MatchOverlay/CommentryBox").GetComponent<CanvasGroup>();
		if (speedLevel == 5)
		{
			ChangeView(MatchView.Pitch);
			MatchCameraController.instance.SetCameraBehaviour(CameraTypes.QUICK_MATCH, CameraBehaviour.STATIC, null, snapTo: true);
		}
		if (!PlayerPrefs.HasKey("ReplayEnabledSetting"))
		{
			PlayerPrefs.SetInt("ReplayEnabledSetting", (int)replaysEnabled);
		}
		else
		{
			replaysEnabled = (eReplayType)PlayerPrefs.GetInt("ReplayEnabledSetting");
		}
		if (!PlayerPrefs.HasKey("MatchViewSetting"))
		{
			PlayerPrefs.SetInt("MatchViewSetting", (int)currentMatchView);
		}
		else
		{
			currentMatchView = (MatchViewType)PlayerPrefs.GetInt("MatchViewSetting");
		}
		if (!PlayerPrefs.HasKey("HighlightViewSetting"))
		{
			PlayerPrefs.SetInt("HighlightViewSetting", (int)currentHighlightView);
		}
		else
		{
			currentHighlightView = (MatchViewType)PlayerPrefs.GetInt("HighlightViewSetting");
		}
		if (!PlayerPrefs.HasKey("MatchQuickGoals"))
		{
			PlayerPrefs.SetInt("MatchQuickGoals", quickMatchHighlightsEnabled ? 1 : 0);
		}
		else
		{
			quickMatchHighlightsEnabled = PlayerPrefs.GetInt("MatchQuickGoals") == 1;
		}
		if (!PlayerPrefs.HasKey("MatchQuickReplays"))
		{
			PlayerPrefs.SetInt("MatchQuickReplays", quickMatchReplaysEnabled ? 1 : 0);
		}
		else
		{
			quickMatchReplaysEnabled = PlayerPrefs.GetInt("MatchQuickReplays") == 1;
		}
		weatherController = GameObject.Find("PitchExtras/SunRotationCorrector/Sun").AddComponent<MatchTimeOfDay>();
		match3DUI = GameObject.Find("Match3DUI");
		if (InFastMode)
		{
			weatherController.weatherEnabled = false;
		}
		weatherController.RandomiseTimeOfDay(clubDataID);
		if (!quickMatchScreenExists)
		{
			SetupMatchControls();
		}
		sm = new cSoccerEngine(seed, this);
		sm.match.setController(this);
		ballRot = Quaternion.identity;
		MonoBehaviour.print(base.gameObject.name);
		homeAway = ((clubDataID == managedClubID) ? "home" : "away");
		debugScrollArea = debugOptions.transform.Find("Scroll View/Viewport/Content");
		debugVarsArea = debugOptions.transform.Find("VariableWatch/Viewport/Content");
		manyMatches.debugScrollArea = manyMatches.manyMatchesObj.transform.Find("Scroll View/Viewport/Content");
		manyMatches.debugVarsArea = manyMatches.manyMatchesObj.transform.Find("VariableWatch/Viewport/Content");
		homeTactics = sm.match.getTeamTactics("home", ignorePending: true);
		awayTactics = sm.match.getTeamTactics("away", ignorePending: true);
		cTactics yourTactics = YourTactics;
		TacticsController.GetInstance().InitMatchTactics(yourTactics, sm.match.matchStats.teams[homeAway].playerStats);
		tacticsGameObject.SetActive(value: false);
		if ((bool)fakeGame)
		{
			sm.match.setTeamTactics(yourTactics, homeAway);
		}
		fixtureKitHelper = new FixtureKitHelper();
		fixtureKitHelper.SetFixtureKits(HomeColour1, HomeColour2, AwayHomeColour1, AwayHomeColour2, AwayColour1, AwayColour2);
		fixtureKitHelper.SetKitStyle(homeKitStyle, awayHomeKitStyle, awayKitStyle);
		ball = UnityEngine.Object.Instantiate(ballPrefab);
		ball.name = "BallSprite";
		ball.transform.localScale = new Vector3(ball.transform.localScale.x * localScale.x, ball.transform.localScale.y * localScale.y, ball.transform.localScale.z * localScale.z);
		ballVisual = ball.transform.Find("BallVisual");
		ballLookTarget = ball.transform.Find("BallLookTarget");
		ballDebugLine = ball.transform.Find("DashedLine").GetComponent<LineRenderer>();
		_ = seFixInfo.fixtureInfo.home.name;
		_ = seFixInfo.fixtureInfo.away.name;
		homeSponsor = defaultSponsor;
		awaySponsor = defaultSponsor;
		if (goalFixture)
		{
			homeSponsor = Resources.Load<Texture2D>(SponsorData.GetLogoFileName(4));
			awaySponsor = Resources.Load<Texture2D>(SponsorData.GetLogoFileName(4));
		}
		else
		{
			if (Game.instance.managedClub[Game.instance.g_currUser].CurrentSponsor.ID > 0)
			{
				if (homeAway == "home")
				{
					homeSponsor = Resources.Load<Texture2D>(SponsorData.GetLogoFileName(Game.instance.managedClub[Game.instance.g_currUser].CurrentSponsor.ID));
				}
				else
				{
					awaySponsor = Resources.Load<Texture2D>(SponsorData.GetLogoFileName(Game.instance.managedClub[Game.instance.g_currUser].CurrentSponsor.ID));
				}
			}
			if (homeAway == "home")
			{
				awaySponsor = Resources.Load<Texture2D>(SponsorData.GetLogoFileName(seFixInfo.fixtureInfo.away.clubDataID % 8 + 1));
			}
			else
			{
				homeSponsor = Resources.Load<Texture2D>(SponsorData.GetLogoFileName(seFixInfo.fixtureInfo.home.clubDataID % 8 + 1));
			}
		}
		homeBadge = new Texture2D(defaultBadge.width, defaultBadge.height, defaultBadge.format, mipChain: true);
		homeBadge.SetPixels(defaultBadge.GetPixels());
		homeBadge.Apply();
		awayBadge = new Texture2D(defaultBadge.width, defaultBadge.height, defaultBadge.format, mipChain: true);
		awayBadge.SetPixels(defaultBadge.GetPixels());
		awayBadge.Apply();
		leagueBadge = new Texture2D(defaultBadge.width, defaultBadge.height, defaultBadge.format, mipChain: true);
		leagueBadge.SetPixels(defaultBadge.GetPixels());
		leagueBadge.Apply();
		StartCoroutine(ImageController.GetInstance().LoadClubBadgeTexture(seFixInfo.fixtureInfo.home.clubDataID, homeBadge));
		StartCoroutine(ImageController.GetInstance().LoadClubBadgeTexture(seFixInfo.fixtureInfo.away.clubDataID, awayBadge));
		int competitionid = Game.instance.g_matchday_nextfix.competitionid;
		switch (Game.instance.g_matchday_nextfix.competitiontype)
		{
		case 2:
		case 7:
		case 8:
			StartCoroutine(ImageController.GetInstance().LoadLeagueBadgeTexture(competitionid, leagueBadge, ImageType.League));
			break;
		case 3:
			StartCoroutine(ImageController.GetInstance().LoadLeagueBadgeTexture(competitionid, leagueBadge, ImageType.Cup));
			break;
		case 4:
			StartCoroutine(ImageController.GetInstance().LoadLeagueBadgeTexture(competitionid, leagueBadge, ImageType.ContiComp));
			break;
		}
		playerGen = GameObject.Find("PlayerGenerator").GetComponent<PlayerModelGenerator>();
		playerGen.kitHelper = fixtureKitHelper;
		bool flag = true;
		for (int i = 0; i < NUM_PLAYERS; i++)
		{
			flag = (PlayerData.GetPreferredPosition(seFixInfo.fixtureInfo.home.players[i + 1].id) & 1) != 0;
			GeneratePlayerModel(ref team1Players[i], seFixInfo.fixtureInfo.home.players[i + 1], (!flag) ? PitchPlayerType.HOME_PLAYER : PitchPlayerType.HOME_GOALKEEPER, HomeLogoFilename, clubDataID);
			team1Players[i].playerObject.name = "PlayerModelHome" + i;
			team1Players[i].playerObject.SetActive(value: false);
			flag = (PlayerData.GetPreferredPosition(seFixInfo.fixtureInfo.away.players[i + 1].id) & 1) != 0;
			GeneratePlayerModel(ref team2Players[i], seFixInfo.fixtureInfo.away.players[i + 1], flag ? PitchPlayerType.AWAY_GOALKEEPER : PitchPlayerType.AWAY_PLAYER, AwayLogoFilename, clubDataID2);
			team2Players[i].playerObject.name = "PlayerModelAway" + i;
			team2Players[i].playerObject.SetActive(value: false);
		}
		stadiumController = GameObject.Find("PitchExtras").GetComponentInChildren<StadiumGenerator>(includeInactive: true);
		stadiumCrowd = GameObject.Find("PitchExtras/StadiumContainer").GetComponent<StadiumCrowd>();
		stadiumCrowd.LoadMaterials();
		stadiumCrowd.SetCrowdCapacity(StadiumData.GetSize(Club.GetStadiumDataID(clubDataID)), g_matchday_nextfix.attendance);
		stadiumController.GenerateStadium(clubDataID, clubDataID2, showCrowd: true);
		stadiumController.SetSubBenchAppearance(clubDataID, hasSubs: true);
		GetComponent<MatchBillboardController>().SetupBillboards();
		transformHelper = new GameObject("TransformHelper");
		transformHelper.transform.SetParent(base.transform);
		transformHelper2 = new GameObject("TransformHelper2");
		transformHelper3 = new GameObject("TransformHelper3");
		crowdLookTarget = new GameObject("CrowdLookTarget");
		SeFixtureInfo.SePlayerInfo sePlayerInfo = new SeFixtureInfo.SePlayerInfo();
		sePlayerInfo.height = 1.85f;
		sePlayerInfo.weight = 80f;
		sePlayerInfo.hairStyle = 1;
		sePlayerInfo.hairColour = 2;
		sePlayerInfo.skinColour = 1;
		sePlayerInfo.facialHair = 1;
		GeneratePlayerModel(ref refereeVisual, sePlayerInfo, PitchPlayerType.REFEREE, null, 0);
		refereeVisual.playerObject.name = "RefereeModel";
		Transform boneTransform = refereeVisual.playerAnimator.GetBoneTransform(HumanBodyBones.RightHand);
		refereeCardInstance = UnityEngine.Object.Instantiate(refereeCardPrefab, boneTransform.Find("CardHolder"), worldPositionStays: false);
		refereeCardMaterial = new Material(refereeCardInstance.GetComponent<MeshRenderer>().sharedMaterial);
		refereeCardInstance.GetComponent<MeshRenderer>().sharedMaterial = refereeCardMaterial;
		refereeCardInstance.SetActive(value: false);
		refereeVisual.playerObject.SetActive(value: true);
		refereeVisual.playerAnimator.SetLayerWeight(3, 1f);
		refereeVisual.playerAnimator.SetLayerWeight(1, 0f);
		refereeVisual.playerAnimator.SetLayerWeight(2, 0f);
		refereeVisual.playerAnimator.SetLayerWeight(6, 0f);
		linesmanFlag = Resources.Load<GameObject>("LinesmanFlag");
		SeFixtureInfo.SePlayerInfo sePlayerInfo2 = new SeFixtureInfo.SePlayerInfo();
		sePlayerInfo2.height = 1.85f;
		sePlayerInfo2.weight = 80f;
		sePlayerInfo2.hairStyle = 1;
		sePlayerInfo2.hairColour = 2;
		sePlayerInfo2.skinColour = 4;
		sePlayerInfo2.facialHair = 1;
		GeneratePlayerModel(ref linesmenVisuals[0], sePlayerInfo2, PitchPlayerType.LINESMAN, null, 0);
		linesmenVisuals[0].playerObject.name = "Linesman1Model";
		GameObject gameObject3 = UnityEngine.Object.Instantiate(linesmanFlag, linesmenVisuals[0].playerAnimator.GetBoneTransform(HumanBodyBones.RightHand).Find("FlagHolder"), worldPositionStays: false);
		linesmanFlagCloths[0] = gameObject3.transform.GetChild(0).GetComponent<Cloth>();
		SeFixtureInfo.SePlayerInfo sePlayerInfo3 = new SeFixtureInfo.SePlayerInfo();
		sePlayerInfo3.height = 1.85f;
		sePlayerInfo3.weight = 80f;
		sePlayerInfo3.hairStyle = 1;
		sePlayerInfo3.hairColour = 2;
		sePlayerInfo3.skinColour = 1;
		sePlayerInfo3.facialHair = 1;
		GeneratePlayerModel(ref linesmenVisuals[1], sePlayerInfo3, PitchPlayerType.LINESMAN, null, 0);
		linesmenVisuals[1].playerObject.name = "Linesman2Model";
		gameObject3 = UnityEngine.Object.Instantiate(linesmanFlag, linesmenVisuals[1].playerAnimator.GetBoneTransform(HumanBodyBones.RightHand).Find("FlagHolder"), worldPositionStays: false);
		linesmanFlagCloths[1] = gameObject3.transform.GetChild(0).GetComponent<Cloth>();
		SeFixtureInfo.SePlayerInfo sePlayerInfo4 = new SeFixtureInfo.SePlayerInfo();
		sePlayerInfo4.height = 1.85f;
		sePlayerInfo4.weight = 80f;
		sePlayerInfo4.hairStyle = 1;
		sePlayerInfo4.hairColour = 2;
		sePlayerInfo4.skinColour = 1;
		sePlayerInfo4.facialHair = 1;
		GeneratePlayerModel(ref physioVisual, sePlayerInfo4, PitchPlayerType.PHYSIO, null, 0);
		physioVisual.playerObject.name = "PhysioModel";
		medic = UnityEngine.Object.Instantiate(medicPrefab);
		medic.name = "MedicSprite";
		medic.transform.SetParent(base.transform, worldPositionStays: false);
		medic.transform.Translate(Vector2.left);
		medic.transform.localRotation = Quaternion.Euler(0f, 0f, -90f);
		medic.transform.Find("NightShadow").gameObject.SetActive(useFloodLights ? true : false);
		ballAhead = UnityEngine.Object.Instantiate(ballPrefab);
		ballAhead.SetActive(value: false);
		tvTarget = GameObject.Find("MatchCameras/TVCamera/DirectionTarget");
		panoramicTarget = GameObject.Find("MatchCameras/PanoramicCamera/DirectionTarget");
		standTarget = GameObject.Find("MatchCameras/StandCamera/DirectionTarget");
		homeStaticGoalNetting = GetOppositionGoalTransform("away").Find("GoalStaticNet").gameObject;
		awayStaticGoalNetting = GetOppositionGoalTransform("home").Find("GoalStaticNet").gameObject;
		homeDynamicGoalNetting = GetOppositionGoalTransform("away").Find("GoalNet").gameObject;
		awayDynamicGoalNetting = GetOppositionGoalTransform("home").Find("GoalNet").gameObject;
		homeDynamicGoalNetting.SetActive(value: false);
		awayDynamicGoalNetting.SetActive(value: false);
		negativeGoalDirReplayAimer = GameObject.Find("PitchExtras/Goal1ReplayAim");
		positiveGoalDirReplayAimer = GameObject.Find("PitchExtras/Goal2ReplayAim");
		cMatch cMatch2 = sm.match;
		string[] array = new string[2] { "home", "away" };
		foreach (string text in array)
		{
			foreach (KeyValuePair<int, cPlayer> item in cMatch2.teams[text].playerCache)
			{
				cPlayer value = item.Value;
				if (text == "home")
				{
					value.currentVisual = team1Players[value.teamPos - 1];
					value.currentVisual.playerIK.SetPlayerReference(value);
					value.currentVisual.playerIK.SetupIKController(value.currentVisual);
					value.preferredFoot = cPlayer.GetPreferredFoot(seFixInfo.fixtureInfo.home.players[value.teamPos].foot);
					value.currentVisual.playerCircle.associatedPlayer = value;
				}
				else
				{
					value.currentVisual = team2Players[value.teamPos - 1];
					value.currentVisual.playerIK.SetPlayerReference(value);
					value.currentVisual.playerIK.SetupIKController(value.currentVisual);
					value.preferredFoot = cPlayer.GetPreferredFoot(seFixInfo.fixtureInfo.away.players[value.teamPos].foot);
					value.currentVisual.playerCircle.associatedPlayer = value;
				}
				_ = value.preferredFoot;
				_ = 1;
			}
		}
		UpdateCaptainsArmbands();
		cMatch2.referee.currentVisual = refereeVisual;
		cMatch2.referee.currentVisual.playerIK.SetupIKController(cMatch2.referee.currentVisual);
		cMatch2.physio.currentVisual = physioVisual;
		for (int k = 0; k < 2; k++)
		{
			cMatch2.linesmen[k].currentVisual = linesmenVisuals[k];
			cMatch2.linesmen[k].currentVisual.playerIK.SetupIKController(cMatch2.linesmen[k].currentVisual);
		}
		initMatch(seFixInfo);
		InitMatchOverlay(seFixInfo);
		SetUpSideBar();
		InitMatchStatsView(seFixInfo);
		InitPlayerStatsView(seFixInfo);
		sm.match.commentary.SetupHomeAwayColours(fixtureKitHelper.homeColour1, fixtureKitHelper.awayColour1);
		GameObject.Find("AudioController").GetComponent<AudioController>().LoadBackgroundMusic(AudioController.SoundToPlay.MATCH_BACKGROUND);
		matchAudio = GameObject.Find("MatchAudio").GetComponent<MatchAudio>();
		matchAudio.RegisterSeController(this, sm);
		if (PlayerPrefs.GetInt("SFX") == 0)
		{
			matchAudio.SoundEnabled = true;
		}
		else
		{
			matchAudio.SoundEnabled = false;
		}
		homeScoreText = topPanel.transform.Find("ScorePanel/HomeScore").GetComponent<Text>();
		awayScoreText = topPanel.transform.Find("ScorePanel/AwayScore").GetComponent<Text>();
		homeScoreOverviewText = overviewScreen.transform.Find("OverviewPanel/left/Panel/ScorePanel/ScorePanel/homeback/hometeamscore").GetComponent<Text>();
		awayScoreOverviewText = overviewScreen.transform.Find("OverviewPanel/left/Panel/ScorePanel/ScorePanel/awayback/awayteamscore").GetComponent<Text>();
		playArrow = matchOverlay.transform.Find("PlayArrow").gameObject;
		if (quickMatchScreenExists)
		{
			homeQuickScoreText = quickMatchScreen.transform.Find("Scores/HomeScore").GetComponent<Text>();
			awayQuickScoreText = quickMatchScreen.transform.Find("Scores/AwayScore").GetComponent<Text>();
			quickMatchScreen.transform.Find("Scores/AggrBox/AggText").GetComponent<Text>().text = Translation.GetText("aggregate-score");
			quickMatchScreen.transform.Find("Scores/PensBox/PenText").GetComponent<Text>().text = Translation.GetText("penalties");
			if (!seFixInfo.fixtureInfo.isLeg2)
			{
				quickMatchScreen.transform.Find("Scores/AggrBox").GetComponent<CanvasGroup>().alpha = 0f;
			}
			if (!shootout)
			{
				quickMatchScreen.transform.Find("Scores/PensBox").GetComponent<CanvasGroup>().alpha = 0f;
			}
			quickMatchScreen.transform.Find("EndContinueButton").gameObject.SetActive(value: false);
			quickMatchScreen.transform.Find("EndContinueButton/Text").GetComponent<Text>().text = Translation.GetText("continue").ToUpper();
			quickMatchScreen.transform.Find("EndContinueButton").GetComponent<Button>().onClick.RemoveAllListeners();
			quickMatchScreen.transform.Find("EndContinueButton").GetComponent<Button>().onClick.AddListener(() =>
			{
				quickMatchScreen.transform.Find("EndContinueButton").GetComponent<Button>().enabled = false;
				GameObject.Find("PitchHolder").GetComponent<SeController>().LoadBackToMainMenu();
			});
			quickMatchScreen.transform.Find("PlayPauseButton").GetComponent<Button>().onClick.AddListener(() =>
			{
				if (matchPaused)
				{
					UnPauseGame();
					if (matchControlsShown)
					{
						SetMatchControlsVisible(visible: false);
					}
				}
				else
				{
					StartCoroutine(PauseGame());
				}
			});
			quickMatchScreen.transform.Find("GoalHighlights/Text").GetComponent<Text>().text = Translation.GetText("goals").ToUpper();
			quickGoalsButton = quickMatchScreen.transform.Find("GoalHighlights").GetComponent<Button>();
			quickGoalsButton.transform.Find("ButtonOn").gameObject.SetActive(quickMatchHighlightsEnabled);
			quickGoalsButton.transform.Find("ButtonOn/Text").GetComponent<Text>().text = Translation.GetText("on").ToUpper();
			quickGoalsButton.transform.Find("ButtonOff").gameObject.SetActive(!quickMatchHighlightsEnabled);
			quickGoalsButton.transform.Find("ButtonOff/Text").GetComponent<Text>().text = Translation.GetText("off").ToUpper();
			quickGoalsButton.onClick.RemoveAllListeners();
			quickGoalsButton.onClick.AddListener(() =>
			{
				quickMatchHighlightsEnabled = !quickMatchHighlightsEnabled;
				PlayerPrefs.SetInt("MatchQuickGoals", quickMatchHighlightsEnabled ? 1 : 0);
				quickGoalsButton.transform.Find("ButtonOn").gameObject.SetActive(quickMatchHighlightsEnabled);
				quickGoalsButton.transform.Find("ButtonOff").gameObject.SetActive(!quickMatchHighlightsEnabled);
				quickReplaysButton.gameObject.SetActive(quickMatchHighlightsEnabled);
			});
			quickMatchScreen.transform.Find("GoalReplays/Text").GetComponent<Text>().text = Translation.GetText("replay").ToUpper();
			quickReplaysButton = quickMatchScreen.transform.Find("GoalReplays").GetComponent<Button>();
			quickReplaysButton.transform.Find("ButtonOn").gameObject.SetActive(quickMatchReplaysEnabled);
			quickReplaysButton.transform.Find("ButtonOn/Text").GetComponent<Text>().text = Translation.GetText("on").ToUpper();
			quickReplaysButton.transform.Find("ButtonOff").gameObject.SetActive(!quickMatchReplaysEnabled);
			quickReplaysButton.transform.Find("ButtonOff/Text").GetComponent<Text>().text = Translation.GetText("off").ToUpper();
			quickReplaysButton.onClick.RemoveAllListeners();
			quickReplaysButton.onClick.AddListener(() =>
			{
				quickMatchReplaysEnabled = !quickMatchReplaysEnabled;
				PlayerPrefs.SetInt("MatchQuickReplays", quickMatchReplaysEnabled ? 1 : 0);
				quickReplaysButton.transform.Find("ButtonOn").gameObject.SetActive(quickMatchReplaysEnabled);
				quickReplaysButton.transform.Find("ButtonOff").gameObject.SetActive(!quickMatchReplaysEnabled);
			});
			if (!quickMatchHighlightsEnabled)
			{
				quickReplaysButton.gameObject.SetActive(value: false);
			}
		}
		topPanel.transform.Find("ScorePanel/HalfTimeString").GetComponent<Text>().text = Translation.GetText("mrbar-ht");
		topPanel.transform.Find("ScorePanel/FullTimeString").GetComponent<Text>().text = Translation.GetText("mrbar-ft");
		topPanel.transform.Find("ScorePanel/ExtraTimeString").GetComponent<Text>().text = Translation.GetText("et");
		topPanel.transform.Find("ScorePanel/ExtraHalfTimeString").GetComponent<Text>().text = Translation.GetText("mrbar-ht");
		if ((bool)GameObject.Find("SceneManager"))
		{
			GameObject.Find("SceneManager/CameraAnchor/GameCamera").GetComponent<Camera>().enabled = false;
			GameObject.Find("SceneManager/CameraAnchor/GameCamera").GetComponent<AudioListener>().enabled = false;
		}
		CalculatePlayerAnimationDelays();
		match = sm.match;
		UpdateEvents();
		UpdateCamera();
		if (!UI.GetInstance().instantMatchMode)
		{
			DontDestroySingleton<SceneController>.Instance.ChangeQuality(SceneController.QualitySetting.MATCH);
		}
	}

	private void GeneratePlayerModel(ref PlayerVisuals visual, SeFixtureInfo.SePlayerInfo player, PitchPlayerType type, string badgePath, short clubID = 0)
	{
		visual = new PlayerVisuals();
		visual.playerHeight = player.height;
		visual.playerObject = playerGen.GeneratePlayer(player, type, visual, badgePath, clubID);
		visual.playerObjectTransform = visual.playerObject.transform;
		visual.playerPivot = visual.playerObject.transform.GetChild(0).gameObject;
		visual.playerPivotTransform = visual.playerPivot.transform;
		visual.playerAnimObject = visual.playerPivot.transform.GetChild(0).gameObject;
		visual.playerAnimator = visual.playerAnimObject.GetComponent<Animator>();
		visual.playerAnimatorTransform = visual.playerAnimator.transform;
		visual.playerHead = visual.playerAnimator.GetBoneTransform(HumanBodyBones.Head);
		visual.animatorOverrides = new AnimatorOverrideController(visual.playerAnimator.runtimeAnimatorController);
		visual.playerAnimator.runtimeAnimatorController = visual.animatorOverrides;
		visual.playerMesh = visual.playerAnimObject.transform.GetChild(0).GetComponent<SkinnedMeshRenderer>();
		visual.playerIK = visual.playerAnimObject.gameObject.AddComponent<IKControl>();
		visual.playerArmBand = visual.playerAnimator.transform.Find("SM21_CHARACTER_grp/MESH_grp/SM21_Manager_Tracksuit_grp/Player_Clothes_grp/Player_Clothes_Base_grp/skin_Player_Armband_Base").gameObject;
		if (type == PitchPlayerType.PHYSIO || type == PitchPlayerType.REFEREE || type == PitchPlayerType.MEDIC || type == PitchPlayerType.LINESMAN)
		{
			visual.playerAnimator.SetLayerWeight(1, 0f);
			visual.playerAnimator.SetLayerWeight(2, 0f);
			visual.playerAnimator.SetLayerWeight(6, 0f);
		}
		visual.playerObject.transform.position = new Vector3(0f, -100f, 0f);
		GameObject gameObject = UnityEngine.Object.Instantiate(playerDetailsPrefab);
		visual.playerDetails = gameObject;
		visual.playerCircle = gameObject.GetComponentInChildren<PlayerDetailsCircleController>();
		visual.playerCircle.SetName(player.surName);
		visual.playerCircle.Set2DColour(playerGen.kitHelper.GetColourUsingPlayerType(type, clubID)[0]);
		if (showDebugMarkings)
		{
			visual.playerCircle.transform.Find("DebugPos").GetComponent<SpriteRenderer>().enabled = true;
		}
		if (type == PitchPlayerType.HOME_GOALKEEPER || type == PitchPlayerType.AWAY_GOALKEEPER)
		{
			visual.playerAnimator.SetBool("IsGoalkeeper", value: true);
			visual.playerIK.isGoalkeeper = true;
		}
		if (type == PitchPlayerType.PHYSIO)
		{
			visual.playerCircle.SetColliderActive(active: false);
		}
		if (type == PitchPlayerType.REFEREE)
		{
			visual.playerIK.isReferee = true;
			visual.playerCircle.SetColliderActive(active: false);
		}
		if (type == PitchPlayerType.LINESMAN)
		{
			visual.playerIK.isLinesman = true;
			visual.playerCircle.SetColliderActive(active: false);
		}
		gameObject.transform.SetParent(visual.playerObject.transform, worldPositionStays: false);
		gameObject.transform.localPosition = new Vector3(0f, 0f, 0f);
		if (useFloodLights)
		{
			visual.playerCircle.transform.Find("NightFootShadow").gameObject.SetActive(value: true);
		}
		if (type == PitchPlayerType.AWAY_PLAYER || type == PitchPlayerType.HOME_PLAYER)
		{
			currentPlayerRandomIdle = globalRand.NextDouble();
			for (float num = 0f; num < (float)playerIdles.Count; num++)
			{
				if (currentPlayerRandomIdle < (double)((num + 1f) * (1f / (float)(playerIdles.Count + 1))))
				{
					visual.animatorOverrides["Idle"] = playerIdles[(int)num];
					break;
				}
			}
		}
		if (InFastMode)
		{
			playerGen.GeneratePlayerQuick(player, type, visual.playerObject.transform, clubID);
		}
	}

	private IEnumerator DisableAnimatorLater(Animator animator)
	{
		yield return new WaitForEndOfFrame();
		animator.enabled = false;
	}

	public void SetMatchProperties()
	{
		short num = 581;
		short num2 = 589;
		if ((bool)fakeGame)
		{
			UnityEngine.Object.Instantiate(Resources.Load<GameObject>("AudioController")).gameObject.name = "AudioController";
			Constants.LIVE_SCORES_ENABLED = false;
			NextFixtureInfo nextFixture = Game.instance.GetNextFixture(Game.instance.CurrentManagedClubId);
			Game.instance.CurrentDate = nextFixture.date;
			Fixture fixture = Game.ClubHasUnplayedMatchToday(Game.instance.CurrentManagedClubId);
			Game.instance.g_matchday_nextfix = fixture;
			Game.instance.g_matchday_nextfix.PreFixture(Game.instance.CurrentDate);
			FixtureConverter.LoadFixtureIntoSE(Game.instance.g_matchday_nextfix, Game.instance.g_fixtureTime, Game.instance.g_matchday_saveseed);
		}
		if (goalFixture)
		{
			int num3 = 0;
			int num4 = 1;
			int num5 = num3;
			Game.instance.managedClub[0].ClubID = num;
			if ((float)PlayerPrefs.GetInt("GoalTeam") == 1f)
			{
				num5 = num4;
				Game.instance.managedClub[0].ClubID = num2;
			}
			Game.instance.g_tm[0].clubid = num;
			Game.instance.g_tm[1].clubid = num2;
			Game.instance.g_tm[num3].lineup[0] = 460;
			Game.instance.g_tm[num3].lineup[1] = 23219;
			Game.instance.g_tm[num3].lineup[2] = 17767;
			Game.instance.g_tm[num3].lineup[3] = 1017;
			Game.instance.g_tm[num3].lineup[4] = 161;
			Game.instance.g_tm[num3].lineup[5] = 74;
			Game.instance.g_tm[num3].lineup[6] = 16319;
			Game.instance.g_tm[num3].lineup[7] = 173;
			Game.instance.g_tm[num3].lineup[8] = 1185;
			Game.instance.g_tm[num3].lineup[9] = 463;
			Game.instance.g_tm[num3].lineup[10] = 19502;
			Game.instance.g_tm[num3].lineup[11] = 1024;
			Game.instance.g_tm[num3].lineup[12] = 17519;
			Game.instance.g_tm[num3].lineup[13] = 1814;
			Game.instance.g_tm[num3].lineup[14] = 10546;
			Game.instance.g_tm[num3].lineup[15] = 1093;
			Game.instance.g_tm[num3].lineup[16] = 2037;
			Game.instance.g_tm[num3].lineup[17] = 388;
			Game.instance.g_tm[num3].formationid = 10;
			Game.instance.g_tm[num3].captain = 8;
			Game.instance.g_tm[num3].cornertakerleft = 8;
			Game.instance.g_tm[num3].cornertakerright = 8;
			Game.instance.g_tm[num3].deadballtakerleft = 8;
			Game.instance.g_tm[num3].deadballtakerright = 8;
			Game.instance.g_tm[num3].penaltytaker = 8;
			Game.instance.g_tm[num4].lineup[0] = 162;
			Game.instance.g_tm[num4].lineup[1] = 1186;
			Game.instance.g_tm[num4].lineup[3] = 1814;
			Game.instance.g_tm[num4].lineup[4] = 161;
			Game.instance.g_tm[num4].lineup[2] = 6857;
			Game.instance.g_tm[num4].lineup[7] = 173;
			Game.instance.g_tm[num4].lineup[5] = 74;
			Game.instance.g_tm[num4].lineup[6] = 520;
			Game.instance.g_tm[num4].lineup[9] = 463;
			Game.instance.g_tm[num4].lineup[10] = 392;
			Game.instance.g_tm[num4].lineup[8] = 174;
			Game.instance.g_tm[num4].lineup[11] = 5191;
			Game.instance.g_tm[num4].lineup[12] = 149;
			Game.instance.g_tm[num4].lineup[13] = 461;
			Game.instance.g_tm[num4].lineup[14] = 20116;
			Game.instance.g_tm[num4].lineup[15] = 18490;
			Game.instance.g_tm[num4].lineup[16] = 7991;
			Game.instance.g_tm[num4].lineup[17] = 4226;
			Game.instance.g_tm[num4].formationid = 10;
			Game.instance.g_tm[num4].captain = 8;
			Game.instance.g_tm[num4].cornertakerleft = 8;
			Game.instance.g_tm[num4].cornertakerright = 8;
			Game.instance.g_tm[num4].deadballtakerleft = 8;
			Game.instance.g_tm[num4].deadballtakerright = 8;
			Game.instance.g_tm[num4].penaltytaker = 8;
			for (int i = 0; i < Game.instance.g_tm[0].lineup.Length; i++)
			{
				Game.instance.managedClub[0].currentTactics.lineUp[i] = Game.instance.g_tm[num5].lineup[i];
			}
			Game.instance.managedClub[0].currentTactics.inst.formationid = Game.instance.g_tm[num5].formationid;
			Game.instance.managedClub[0].currentTactics.inst.captain.value = Game.instance.g_tm[num5].captain;
			Game.instance.managedClub[0].currentTactics.inst.cornertakerleft.value = Game.instance.g_tm[num5].cornertakerleft;
			Game.instance.managedClub[0].currentTactics.inst.cornertakerright.value = Game.instance.g_tm[num5].cornertakerright;
			Game.instance.managedClub[0].currentTactics.inst.deadballtakerleft.value = Game.instance.g_tm[num5].deadballtakerleft;
			Game.instance.managedClub[0].currentTactics.inst.deadballtakerright.value = Game.instance.g_tm[num5].deadballtakerright;
			Game.instance.managedClub[0].currentTactics.inst.penaltytaker.value = Game.instance.g_tm[num5].penaltytaker;
			Game.instance.g_ClubData[num].Name = "Naz FC";
			Game.instance.g_ClubData[num].MediumName = "Naz";
			Game.instance.g_ClubData[num].ShortName = "NAZ";
			Game.instance.g_ClubData[num2].Name = "Sam FC";
			Game.instance.g_ClubData[num2].MediumName = "Sam";
			Game.instance.g_ClubData[num2].ShortName = "SAM";
			Game.instance.g_ClubData[num].LogoFilename = "https://smimgs.com/images/assets/blankclubbadge.png";
			Game.instance.g_ClubData[num2].LogoFilename = "https://smimgs.com/images/assets/blankclubbadge.png";
			Fixture fixture2 = Game.instance.g_matchday_nextfix;
			fixture2.HomeClubId(Game.instance.g_tm[0].clubid);
			fixture2.AwayClubId(Game.instance.g_tm[1].clubid);
			fixture2.stadiumid = Club.GetStadiumDataID(53);
			fixture2.attendance = StadiumData.GetSize(fixture2.stadiumid);
			FixtureConverter.LoadFixtureIntoSE(fixture2, fixture2.Time, 0);
		}
		seFixInfo = Game.instance.g_fixInfo;
		seLiveScores = Game.instance.g_LiveScores;
		seLiveTable = Game.instance.g_LiveTable;
		seAllGames = Game.instance.g_allGames;
		g_matchday_nextfix = Game.instance.g_matchday_nextfix;
		managedClubID = Game.instance.managedClub[Game.instance.g_currUser].ClubID;
		HomeColour1 = Game.instance.g_ClubData[seFixInfo.fixtureInfo.home.clubDataID].HomeColour1;
		HomeColour2 = Game.instance.g_ClubData[seFixInfo.fixtureInfo.home.clubDataID].HomeColour2;
		AwayHomeColour1 = Game.instance.g_ClubData[seFixInfo.fixtureInfo.away.clubDataID].HomeColour1;
		AwayHomeColour2 = Game.instance.g_ClubData[seFixInfo.fixtureInfo.away.clubDataID].HomeColour2;
		AwayColour1 = Game.instance.g_ClubData[seFixInfo.fixtureInfo.away.clubDataID].AwayColour1;
		AwayColour2 = Game.instance.g_ClubData[seFixInfo.fixtureInfo.away.clubDataID].AwayColour2;
		homeKitStyle = Club.GetKitStyle(Game.instance.g_ClubData[seFixInfo.fixtureInfo.home.clubDataID].KitID, seFixInfo.fixtureInfo.home.clubDataID);
		awayHomeKitStyle = Club.GetKitStyle(Game.instance.g_ClubData[seFixInfo.fixtureInfo.away.clubDataID].KitID, seFixInfo.fixtureInfo.away.clubDataID);
		awayKitStyle = Club.GetKitStyle(Game.instance.g_ClubData[seFixInfo.fixtureInfo.away.clubDataID].AwayKitID, seFixInfo.fixtureInfo.away.clubDataID);
		Random = Game.instance.Random;
		seTacticInfo = Game.instance.g_tacticinfo;
		seTacticAction = Game.instance.g_tacticaction;
		HomeLogoFilename = Game.instance.g_ClubData[seFixInfo.fixtureInfo.home.clubDataID].LogoFilename;
		AwayLogoFilename = Game.instance.g_ClubData[seFixInfo.fixtureInfo.away.clubDataID].LogoFilename;
		IsHome = seFixInfo.fixtureInfo.home.clubDataID == managedClubID;
	}

	public void LoadBackToMainMenu()
	{
		GameObject gameObject = GameObject.Find("OverlayCanvasMatch").transform.Find("LoadSpinner").gameObject;
		gameObject.transform.Find("LoadingPanel/Text").GetComponent<Text>().text = Translation.GetText("loading");
		ImageController.ChangeGameLogoImage(gameObject.transform.Find("LoadingPanel/Logo"));
		gameObject.transform.Find("LoadingPanel/Logo").gameObject.SetActive(value: true);
		gameObject.GetComponent<CanvasGroup>().alpha = 1f;
		FixtureConverter.SaveFixtureFromSE(ref g_matchday_nextfix, sm.matchStats, sm.matchSetup);
		StartCoroutine(LoadBackToMainScene());
	}

	private IEnumerator LoadBackToMainScene()
	{
		Application.targetFrameRate = 60;
		yield return SceneManager.LoadSceneAsync(1);
	}

	public void initMatch(SeFixtureInfo fixInfo)
	{
		homeFormation = FormationData.g_formations[fixInfo.fixtureInfo.home.formationID];
		awayFormation = FormationData.g_formations[fixInfo.fixtureInfo.away.formationID];
		changeHomeFormation(fixInfo.fixtureInfo.home.formationID);
		changeAwayFormation(fixInfo.fixtureInfo.away.formationID);
		if (Game.instance.SkipMatch)
		{
			EventLoggingController.LogEvent("matchskipped");
		}
		EventLoggingController.LogEvent("matchstart", "clubName", EventLoggingParam.GetClubNameParamValue());
	}

	public void changeHomeFormation(int formationIndex)
	{
	}

	public void changeAwayFormation(int formationIndex)
	{
	}

	public v3d GetPlayerPosition(cPlayer player, float liveTime)
	{
		return player.getPosition(liveTime);
	}

	public static Vector3 pitchToScreen(v3d v, bool useHeight = false)
	{
		return pitchToScreen(new Vector3(v.vy, v.vx, v.vz), useHeight);
	}

	public static Vector3 pitchToScreen(Vector3 v, bool useHeight = false, bool makeNew = true)
	{
		float x = (v.y + 0f - localPos.x) * 1f * localScale.z;
		float z = (0f - v.x + 0f - localPos.z) * 1f * localScale.x;
		float y = 0f;
		if (useHeight)
		{
			y = (v.z + -0.05f - localPos.y) * 1f * localScale.y;
		}
		if (makeNew)
		{
			return new Vector3(x, y, z);
		}
		v.x = x;
		v.y = y;
		v.z = z;
		return v;
	}

	private PlayerVisuals getPlayerFromIndex(int team, int index)
	{
		if (team == 0)
		{
			return team1Players[index];
		}
		return team2Players[index];
	}

	private string GetTeamTypeString(bool isHome)
	{
		if (isHome)
		{
			return "home";
		}
		return "away";
	}

	public void slowDebug()
	{
		highlightMode = 4;
	}

	public void debugSetName(GameObject player, string text)
	{
		player.transform.Find("Name/PlayerName").GetComponent<TextMesh>().text = text;
		player.transform.Find("Name/NameShadow").GetComponent<TextMesh>().text = text;
		player.transform.Find("RealName/PlayerNameReal").GetComponent<TextMesh>().text = text;
		player.transform.Find("RealName/NameShadowReal").GetComponent<TextMesh>().text = text;
	}

	public float debugReplay(float timeScale, float dt)
	{
		cMatch cMatch2 = sm.match;
		if (oldDebugReplayMatch && !debugReplayMatch)
		{
			cMatch2.loadStateFromDebugHistory(debugReplayTime);
			cMatch2.chopDebugHistory(debugReplayTime);
			cMatch2.updater.rollback(debugReplayTime);
			time = debugReplayTime;
		}
		oldDebugReplayMatch = debugReplayMatch;
		if (olddebugReplayInput)
		{
			_ = debugReplayInput;
		}
		olddebugReplayInput = debugReplayInput;
		timeScale *= debugTimeScale;
		if (debugReplayMatch && debugReplayPause)
		{
			float num = debugReplayTime - time;
			if (num != 0f)
			{
				foreach (cPlayer value in cMatch2.teams["home"].players.Values)
				{
					value.overrideTimeScale = num;
				}
				foreach (cPlayer value2 in cMatch2.teams["away"].players.Values)
				{
					value2.overrideTimeScale = num;
				}
			}
			time = debugReplayTime;
			timeScale = 0f;
		}
		else
		{
			debugReplayTime = time;
			debugReplayBaseTime = time;
		}
		if (debugTeamToggle != null)
		{
			debugTeamToggle.isOn = cMatch2.teamInPlay == "home";
		}
		if (!Input.GetMouseButton(1))
		{
			if (debugReplayTimeSlider != null)
			{
				debugReplayTimeSlider.value = time;
			}
			debugReplayMouseHeld = false;
			debugJoystickHeld = false;
			debugJoystick = new Vector3(0f, 0f, 0f);
		}
		else
		{
			if (!debugReplayMouseHeld)
			{
				float num2 = Input.mousePosition.y / (float)Screen.height;
				if (num2 > 0.087f && num2 < 0.27f)
				{
					debugReplayMouseHeld = true;
					debugReplayMouseXpos = Input.mousePosition.x / (float)Screen.width;
				}
			}
			else
			{
				float num3 = Input.mousePosition.x / (float)Screen.width - debugReplayMouseXpos;
				debugReplayTime += num3 * Math.Abs(num3) * 1f;
				debugReplayTime = Math.Max(0f, Math.Min(5400f, debugReplayTime));
			}
			if (!debugJoystickHeld)
			{
				if (Input.mousePosition.y / (float)Screen.height > 0.27f)
				{
					debugJoystickHeld = true;
					debugJoystickBase = new Vector3(Input.mousePosition.x / (float)Screen.width, Input.mousePosition.y / (float)Screen.height);
				}
			}
			else
			{
				float num4 = Input.mousePosition.x / (float)Screen.width - debugJoystickBase.x;
				float num5 = Input.mousePosition.y / (float)Screen.height - debugJoystickBase.y;
				debugJoystick.x += num4 * 4f;
				debugJoystick.y += num5 * 4f;
			}
		}
		return timeScale;
	}

	public void debugPrint()
	{
		while (sm.match.debugPrint.Count > 0)
		{
			MonoBehaviour.print(sm.match.debugPrint[0]);
			sm.match.debugPrint.RemoveAt(0);
		}
	}

	public void debugMatchEnd()
	{
		manyMatches.matchEnd();
		time = 0f;
		seed++;
		MonoBehaviour.print("seed " + seed);
		cMatch.simTime = 0f;
		sm = new cSoccerEngine(seed, this);
		match = sm.match;
		sm.match.setController(this);
		string[] array = new string[2] { "home", "away" };
		foreach (string text in array)
		{
			foreach (KeyValuePair<int, cPlayer> item in match.teams[text].playerCache)
			{
				player = item.Value;
				if (text == "home")
				{
					player.currentVisual = team1Players[player.teamPos - 1];
				}
				else
				{
					player.currentVisual = team2Players[player.teamPos - 1];
				}
			}
		}
		match.referee.currentVisual = refereeVisual;
		match.physio.currentVisual = physioVisual;
		match.linesmen[0].currentVisual = linesmenVisuals[0];
		match.linesmen[1].currentVisual = linesmenVisuals[1];
	}

	private IEnumerator ResizeDesktopScreen()
	{
		yield return new WaitForSeconds(0.5f);
		GameObject gameObject = GameObject.Find("OverlayCanvasMatch");
		if (gameObject != null)
		{
			desktopScreenWidth = Screen.width;
			desktopScreenHeight = Screen.height;
			float num = desktopScreenWidth / desktopScreenHeight;
			float num2 = 1f;
			float num3 = 1920f / desktopScreenWidth;
			if (num > num2)
			{
				num3 = 1080f / desktopScreenHeight;
			}
			if ((bool)gameObject.GetComponent<CanvasScaler>())
			{
				gameObject.GetComponent<CanvasScaler>().scaleFactor = 0.65f / num3;
			}
		}
		desktopResizeStarted = false;
	}

	private void AddStartGameDrawPoints()
	{
		if (!Constants.LIVE_SCORES_ENABLED)
		{
			return;
		}
		bool flag = false;
		bool flag2 = false;
		for (int i = 0; i < seAllGames.Count; i++)
		{
			for (int j = 0; j < seLiveTable.Count; j++)
			{
				if (seAllGames[i].homeClubName == seLiveTable[j].clubName || seAllGames[i].awayClubName == seLiveTable[j].clubName)
				{
					seLiveTable[j].currentPoints++;
					seLiveTable[j].played++;
				}
				else if (seLiveTable[j].clubName == seFixInfo.fixtureInfo.home.name && !flag)
				{
					seLiveTable[j].currentPoints++;
					seLiveTable[j].played++;
					flag = true;
				}
				else if (seLiveTable[j].clubName == seFixInfo.fixtureInfo.away.name && !flag2)
				{
					seLiveTable[j].currentPoints++;
					seLiveTable[j].played++;
					flag2 = true;
				}
			}
		}
	}

	private void UpdateLiveTableStats(int index)
	{
		if (!Constants.LIVE_SCORES_ENABLED)
		{
			return;
		}
		int num = 0;
		int num2 = 0;
		int num3;
		int num4;
		if (index == -1)
		{
			cMatch cMatch2 = sm.match;
			num3 = cMatch2.matchStats.teams["home"].score;
			num4 = cMatch2.matchStats.teams["away"].score;
			for (int i = 0; i < seLiveTable.Count; i++)
			{
				if (seLiveTable[i].clubName == cMatch2.fixtureInfo.home.name)
				{
					num = i;
				}
				if (seLiveTable[i].clubName == cMatch2.fixtureInfo.away.name)
				{
					num2 = i;
				}
			}
			if (seLiveTable[num].clubName != cMatch2.fixtureInfo.home.name)
			{
				for (int j = 0; j < seLiveTable.Count; j++)
				{
					if (seLiveTable[j].clubName == cMatch2.fixtureInfo.home.name)
					{
						num = seLiveTable[j].currentPosition;
					}
				}
			}
			if (seLiveTable[num2].clubName != cMatch2.fixtureInfo.away.name)
			{
				for (int k = 0; k < seLiveTable.Count; k++)
				{
					if (seLiveTable[k].clubName == cMatch2.fixtureInfo.away.name)
					{
						num2 = seLiveTable[k].currentPosition;
					}
				}
			}
		}
		else
		{
			for (int l = 0; l < seLiveTable.Count; l++)
			{
				if (seLiveTable[l].clubID == seLiveScores[index].homeClubID && l != 0)
				{
					num = seLiveTable[l - 1].currentPosition;
				}
				if (seLiveTable[l].clubID == seLiveScores[index].awayClubID && l != 0)
				{
					num2 = seLiveTable[l - 1].currentPosition;
				}
			}
			if (seLiveTable[num].clubID != seLiveScores[index].homeClubID)
			{
				for (int m = 0; m < seLiveTable.Count; m++)
				{
					if (seLiveTable[m].clubID == seLiveScores[index].homeClubID)
					{
						num = seLiveTable[m].currentPosition;
					}
				}
			}
			if (seLiveTable[num2].clubID != seLiveScores[index].awayClubID)
			{
				for (int n = 0; n < seLiveTable.Count; n++)
				{
					if (seLiveTable[n].clubID == seLiveScores[index].awayClubID)
					{
						num2 = seLiveTable[n].currentPosition;
					}
				}
			}
			num3 = seLiveScores[index].homeScore;
			num4 = seLiveScores[index].awayScore;
		}
		if (num == 20)
		{
			num--;
		}
		else if (num2 == 20)
		{
			num2--;
		}
		seLiveTable[num].currentGoalDifference = seLiveTable[num].StartingGD + (num3 - num4);
		if (num3 > num4)
		{
			seLiveTable[num].currentPoints = seLiveTable[num].startingPoints + 3;
		}
		else if (num3 == num4)
		{
			seLiveTable[num].currentPoints = seLiveTable[num].startingPoints + 1;
		}
		else
		{
			seLiveTable[num].currentPoints = seLiveTable[num].startingPoints;
		}
		if (seLiveTable[num].currentPoints != seLiveTable[num].startingPoints || seLiveTable[num].currentGoalDifference != seLiveTable[num].StartingGD)
		{
			seLiveTable[num].redraw = true;
		}
		seLiveTable[num2].currentGoalDifference = seLiveTable[num2].StartingGD + (num4 - num3);
		if (num3 < num4)
		{
			seLiveTable[num2].currentPoints = seLiveTable[num2].startingPoints + 3;
		}
		else if (num3 == num4)
		{
			seLiveTable[num2].currentPoints = seLiveTable[num2].startingPoints + 1;
		}
		else
		{
			seLiveTable[num2].currentPoints = seLiveTable[num2].startingPoints;
		}
		if (seLiveTable[num2].currentPoints != seLiveTable[num2].startingPoints || seLiveTable[num2].currentGoalDifference != seLiveTable[num2].StartingGD)
		{
			seLiveTable[num2].redraw = true;
		}
	}

	private void UpdateLiveScore(int minute, float time)
	{
		if (!Constants.LIVE_SCORES_ENABLED)
		{
			return;
		}
		_ = g_matchday_nextfix;
		int num = 60;
		if (seLiveScores == null)
		{
			return;
		}
		for (int i = liveScoresCounter; i < seLiveScores.Count && (seLiveScores[i].eventTime == minute || (seLiveScores[i].eventTime == num + minute && time > (float)(num * num))); i++)
		{
			short[] clubID = new short[2]
			{
				seLiveScores[i].homeClubID,
				seLiveScores[i].awayClubID
			};
			DrawLiveGoalEvent(minute);
			if (g_matchday_nextfix.competitiontype == 2)
			{
				UpdateLiveTableStats(i);
				UpdateLivetable(clubID, i);
			}
			else if (g_matchday_nextfix.competitiontype == 4)
			{
				Helpers.NotFullyPorted("Needs the update functions updating to accomodate group fixtures which are being played at the same time as yoru current game");
			}
			UpDateAllCurrentGamesPlayed(minute, time);
			liveScoresCounter++;
		}
	}

	private void Update()
	{
		if (!started)
		{
			return;
		}
		if (matchControlsShown)
		{
			if (Input.GetMouseButtonDown(0) && Physics2D.RaycastAll(MatchCameraController.instance.overlayCamera.ScreenToWorldPoint(Input.mousePosition), Vector2.zero).Length == 0)
			{
				SetMatchControlsVisible(visible: false);
				if (matchPaused)
				{
					UnPauseGame();
				}
				else if (!manyMatches.visible)
				{
					StartCoroutine(PauseGame());
				}
			}
		}
		else if (Input.GetMouseButtonDown(0) && !tacticsOpen && !inEditMode && !InFastMode && completedFirstUpdate && Physics2D.RaycastAll(MatchCameraController.instance.overlayCamera.ScreenToWorldPoint(Input.mousePosition), Vector2.zero).Length == 0)
		{
			if (skipButtonShown)
			{
				SkipButtonPressed();
			}
			else if (matchPaused)
			{
				UnPauseGame();
			}
			else if (!manyMatches.visible)
			{
				StartCoroutine(PauseGame());
			}
		}
		UnityDeltaTime = Mathf.Max(Time.deltaTime, 0.001f);
		match = sm.match;
		debugPrint();
		if (generatedSettings)
		{
			UpdateRuntimeVars();
			manyMatches.updateGUI();
		}
		if (((float)Screen.width != desktopScreenWidth || (float)Screen.height != desktopScreenHeight) && !desktopResizeStarted)
		{
			desktopResizeStarted = true;
			StartCoroutine(ResizeDesktopScreen());
		}
		if (matchPaused)
		{
			if (match.time == 0f)
			{
				weatherController.SetWeatherEffectSpeed(1f);
			}
			else
			{
				weatherController.SetWeatherEffectSpeed(0f);
			}
		}
		if (match.engineState == 212)
		{
			match.halfTimeRun = true;
		}
		if (CheckForShootout(match.engineState))
		{
			shootout = true;
			matchOverlay.transform.Find("MatchDayTopPanel/ScorePanel/pen").gameObject.SetActive(value: true);
		}
		if (!shootout)
		{
			int score = match.matchStats.teams["home"].score;
			int score2 = match.matchStats.teams["away"].score;
			homeScoreText.text = score.ToString();
			awayScoreText.text = score2.ToString();
			homeScoreOverviewText.text = score.ToString();
			awayScoreOverviewText.text = score2.ToString();
			if (quickMatchScreenExists)
			{
				homeQuickScoreText.text = score.ToString();
				awayQuickScoreText.text = score2.ToString();
			}
			if (match.fixtureInfo.isLeg2)
			{
				string text = "( " + (match.fixtureInfo.leg1AwayScore + score) + " - " + (match.fixtureInfo.leg1HomeScore + score2) + " )";
				topPanel.transform.Find("ScorePanel/agg/aggregatescore").GetComponent<Text>().text = text;
				matchOverlay.transform.Find("MatchDayTopPanel/ScorePanel/pen/PenScore").GetComponent<Text>().text = text;
				if (quickMatchScreenExists)
				{
					quickMatchScreen.transform.Find("Scores/AggrBox/AggHomeScore").GetComponent<Text>().text = (match.fixtureInfo.leg1AwayScore + score).ToString();
					quickMatchScreen.transform.Find("Scores/AggrBox/AggAwayScore").GetComponent<Text>().text = (match.fixtureInfo.leg1HomeScore + score2).ToString();
					quickMatchScreen.transform.Find("Scores/AggrBox").GetComponent<CanvasGroup>().alpha = 1f;
				}
			}
		}
		else
		{
			string text2 = "( " + match.matchStats.teams["home"].shootout + " - " + match.matchStats.teams["away"].shootout + " )";
			topPanel.transform.Find("ScorePanel/pen/PenScore").GetComponent<Text>().text = text2;
			matchOverlay.transform.Find("MatchDayTopPanel/ScorePanel/pen/PenScore").GetComponent<Text>().text = text2;
			if (quickMatchScreenExists)
			{
				quickMatchScreen.transform.Find("Scores/PensBox/PenHomeScore").GetComponent<Text>().text = match.matchStats.teams["home"].shootout.ToString();
				quickMatchScreen.transform.Find("Scores/PensBox/PenAwayScore").GetComponent<Text>().text = match.matchStats.teams["away"].shootout.ToString();
				quickMatchScreen.transform.Find("Scores/PensBox").GetComponent<CanvasGroup>().alpha = 1f;
			}
		}
		Text component = topPanel.transform.Find("ScorePanel/Time").GetComponent<Text>();
		TimeSpan timeSpan = TimeSpan.FromSeconds(match.simTimeToMatchTime(time));
		string text3 = $"{timeSpan.Hours * 60 + timeSpan.Minutes:D1}:{timeSpan.Seconds:D2}";
		if (!matchStarted)
		{
			text3 = $"{0:D1}:{0:D2}";
			if (match.engineState == 6 || match.prevEngineState == 6 || match.engineState == 1)
			{
				matchStarted = true;
				if (PlayerPrefs.HasKey("MatchView"))
				{
					currentView = (MatchView)PlayerPrefs.GetInt("MatchView");
				}
			}
		}
		if (matchFinished)
		{
			if (topPanel.transform.Find("ScorePanel/Time").gameObject.activeSelf)
			{
				topPanel.transform.Find("ScorePanel/Time").gameObject.SetActive(value: false);
				topPanel.transform.Find("ScorePanel/FullTimeString").gameObject.SetActive(value: true);
				if (quickMatchScreenExists)
				{
					quickMatchScreen.transform.Find("Time").GetComponent<Text>().text = text3;
				}
			}
		}
		else if (match.engineState != 211 && match.engineState != 210)
		{
			component.text = text3;
			if (quickMatchScreenExists)
			{
				quickMatchScreen.transform.Find("Time").GetComponent<Text>().text = text3;
			}
		}
		if (InFastMode)
		{
			updateMatchStats();
		}
		else
		{
			timeSinceLastStatsUpdate += UnityDeltaTime;
			if (timeSinceLastStatsUpdate > 0.5f)
			{
				updateMatchStats();
				UpdatePlayerStats();
				timeSinceLastStatsUpdate = 0f;
			}
		}
		deltaTime = UnityDeltaTime;
		timeScale = 1f;
		if (Game.debugFast2DPitchEngine)
		{
			timeScale = 250f;
		}
		enteringHighlight = false;
		if (InFastMode)
		{
			enteringHighlight = (IsInReplay || match.highlightState != 0) && quickMatchHighlightsEnabled;
		}
		else
		{
			enteringHighlight = IsInReplay || match.highlightState != 0;
		}
		if (enteringHighlight)
		{
			if (highlightMode == 5 && !quickMatchShowingHighlight)
			{
				quickMatchShowingHighlight = true;
				instance.StadiumController.ShowCameraStandRoof(showRoof: false);
				ChangeView(MatchView.Pitch);
			}
			enteringRealtime = true;
		}
		else
		{
			enteredRealtime = false;
			enteringRealtime = false;
			if (highlightMode == 5)
			{
				timeScale = topSpeed;
				if (quickMatchShowingHighlight)
				{
					quickMatchShowingHighlight = false;
					instance.StadiumController.ShowCameraStandRoof(showRoof: true);
					ChangeView(MatchView.Pitch);
				}
			}
			else
			{
				timeScale = matchSpeeds[speedLevel];
			}
		}
		if (manyMatches.Running || debugSpeedThrough)
		{
			timeScale = 1000f;
		}
		inDeadballSpeedup = false;
		if (debugDeadballSpeedup && highlightMode != 5 && !IsInReplay && match.engineState != 101 && CutScenesController.instance.PlayersVisible)
		{
			int engineState = match.engineState;
			if (engineState >= 100 && engineState < 601 && (engineState < 301 || engineState > 406) && engineState != 504 && engineState != 505 && (match.nextEngineState >= 100 || match.time + match.timeTaken - time > 1f) && (match.prevEngineState >= 100 || time - match.time > 1f))
			{
				timeScale = 25f;
				enteredRealtime = false;
				enteringRealtime = false;
				inDeadballSpeedup = true;
			}
		}
		timeScale = debugReplay(timeScale, deltaTime);
		if (matchPaused)
		{
			timeScale = 0f;
			if (InEditMode || currentView == MatchView.Tactics)
			{
				Time.timeScale = 1f;
			}
			else
			{
				Time.timeScale = 0f;
			}
		}
		else
		{
			Time.timeScale = 1f;
		}
		if (InFastMode)
		{
			stadiumCrowd.SetCrowdSpeed(0f);
			weatherController.SetWeatherEffectSpeed(0f);
		}
		else
		{
			stadiumCrowd.SetCrowdSpeed(timeScale);
			weatherController.SetWeatherEffectSpeed(timeScale);
		}
		deltaTime *= timeScale;
		deltaTime *= eventTimeScale;
		debugUseAheadTime = true;
		if (!matchFinished)
		{
			if (debugUseAheadTime)
			{
				match.aheadTime = 20f;
				if (match.engineState >= 600)
				{
					match.aheadTime = 10f;
				}
			}
			else
			{
				match.aheadTime = 0f;
			}
			if (highlightMode == 5)
			{
				match.aheadTime = 10f;
			}
			while (time < debugFastForwardTime)
			{
				time = sm.match.update(time, deltaTime);
			}
			debugFastForwardTime = 0f;
			time += deltaTime * debugGameSpeed;
			if (matchRunning || manyMatches.Running || debugSpeedThrough)
			{
				time = sm.match.update(time, deltaTime);
			}
			if (Constants.LIVE_SCORES_ENABLED)
			{
				UpdateLiveScore(timeSpan.Minutes, time);
			}
			sm.match.UpdateAllPlayerAnimations(time);
			if (!enteredRealtime && enteringRealtime)
			{
				enteredRealtime = true;
				string[] array = teamStrings;
				foreach (string key in array)
				{
					currentTeam = match.teams[key];
					foreach (KeyValuePair<int, cPlayer> teamPo in currentTeam.teamPos)
					{
						if (teamPo.Value.lastAnimPlayed != null)
						{
							teamPo.Value.animAmountToSkip = time - teamPo.Value.lastAnimPlayed.time;
						}
					}
				}
			}
			matchAudio.UpdateSound();
			if (!InFastMode)
			{
				if (match.commBar != null)
				{
					GameObject gameObject = match.commBar.transform.Find("CommentryText").gameObject;
					cCommentaryRes cCommentaryRes2 = null;
					cCommentaryRes2 = match.commentary.updateCommentary(time);
					if (cCommentaryRes2 != null)
					{
						lastComRes = cCommentaryRes2;
						Image component2 = match.commBar.GetComponent<Image>();
						if (commentaryBoxBackgroundAlpha == -1f)
						{
							commentaryBoxBackgroundAlpha = component2.color.a;
						}
						Color color = cCommentaryRes2.colours[0];
						Color color2 = new Color(color.r, color.g, color.b, commentaryBoxBackgroundAlpha);
						if (cCommentaryRes2.text == "")
						{
							color2.a = 0f;
						}
						component2.color = color2;
						gameObject.GetComponent<Text>().color = cCommentaryRes2.colours[1];
						gameObject.GetComponent<Text>().text = cCommentaryRes2.text;
						if (!firstCommentaryShown)
						{
							firstCommentaryShown = true;
						}
						commBarFlashing = cCommentaryRes2.flashing;
					}
					if (match.highlightState != 1)
					{
						commBarFlashing = false;
					}
				}
				if (commBarFlashing)
				{
					flashTimer += UnityDeltaTime;
					if (flashTimer > 0.25f)
					{
						flashTimer = 0f;
						Image component3 = match.commBar.GetComponent<Image>();
						Text component4 = match.commBar.transform.Find("CommentryText").GetComponent<Text>();
						Color color3 = default;
						Color color4 = default;
						if (flashDimmed)
						{
							flashDimmed = false;
							color4 = lastComRes.colours[0];
							color3 = lastComRes.colours[1];
						}
						else
						{
							flashDimmed = true;
							color3 = lastComRes.colours[0];
							color4 = lastComRes.colours[1];
						}
						component3.color = color3;
						component4.color = color4;
					}
				}
				else
				{
					flashTimer = 0f;
				}
			}
			if (!tacticsOpen)
			{
				cMinorityReport minorityReport = match.minorityReport;
				bool flag = false;
				if (skipButtonPressed && currentSkip != SkipReason.NONE && !skipActionPerformed)
				{
					switch (currentSkip)
					{
					case SkipReason.GOAL_CELEBRATION:
						time = match.nextStateTime - 3f;
						skipActionPerformed = true;
						break;
					case SkipReason.REPLAY:
						shownTransitionAnimation = true;
						currentReplayPhase = eMatchReplayPhase.PART_2;
						time = EndReplay();
						skipActionPerformed = true;
						break;
					case SkipReason.PREMATCH_LINEUP:
						CutScenesController.instance.Stop();
						time = match.nextStateTime;
						skipActionPerformed = true;
						matchCanvas.transform.Find("MatchOverlay").GetComponent<Animator>().SetTrigger("SkipIntro");
						break;
					case SkipReason.POSTMATCH:
						CutScenesController.instance.Stop();
						skipActionPerformed = true;
						break;
					case SkipReason.HALFTIME:
						CutScenesController.instance.Stop();
						ShowBall();
						skipActionPerformed = true;
						break;
					case SkipReason.SUBS:
						CutScenesController.instance.Stop();
						skipActionPerformed = true;
						break;
					default:
						flag = true;
						switch (match.engineState)
						{
						default:
							time = match.nextStateTime;
							break;
						case 301:
						case 302:
						case 303:
						case 304:
						case 306:
						case 307:
							time = match.nextStateTime - 1f;
							skipActionPerformed = true;
							break;
						}
						break;
					}
				}
				if (currentSkip != SkipReason.REPLAY && currentSkip != SkipReason.REFEREE)
				{
					if (time > minorityReport.startTime && time < minorityReport.endTime && !flag)
					{
						if (match.highlightState != 1 && match.highlightState != 2)
						{
							match.highlightState = 1;
							ResetPlayerAnimations(time);
							MatchView matchView = currentView;
							ChangeView(MatchView.Pitch);
							currentView = matchView;
						}
					}
					else if (match.highlightState == 1)
					{
						match.highlightState = 0;
						minorityReport.hasChanges = false;
						if (timeSpan.Hours * 60 + timeSpan.Minutes < 89)
						{
							ChangeView(currentView);
						}
					}
				}
			}
			liveTime = time - match.time;
			if (liveTime < 0f)
			{
				liveTime = 0f;
			}
			cBall cBall2 = match.ball;
			cBall2.currentPos = cBall2.getPosition(liveTime);
			ballPos = v3d.clone(cBall2.currentPos);
			if (debugBallMover)
			{
				ballPos.vx = ballMoverPos.x;
				ballPos.vy = ballMoverPos.y;
				ballPos.vz = ballMoverPos.z;
			}
			float num = (YourTeamInPossession() ? 5f : (-2f));
			if (!isHome)
			{
				num *= -1f;
			}
			if (swappedEnds)
			{
				num *= -1f;
			}
			ballPosFuture = cBall2.getPositionAbs(time + 1.5f);
			v3d.length(cBall2.getVelocity(liveTime));
			_ = 1f;
			UpdateAllPlayers();
			UpdateCrowd();
			UpdateRefAndLinesmmenVisibility();
			UpdateCaptainsArmbands();
			CardShower[] behaviours = refereeVisual.playerAnimator.GetBehaviours<CardShower>();
			foreach (CardShower obj in behaviours)
			{
				obj.refereeCardObject = refereeCardInstance;
				obj.cardShown = false;
			}
			if (lastRefereeState != match.engineState)
			{
				if (match.engineState == 305)
				{
					refereeCardInstance.SetActive(value: false);
					refereeVisual.playerAnimator.SetBool("MadeDecision", value: false);
				}
				else if (match.engineState == 301)
				{
					refereeVisual.playerAnimator.SetBool("MadeDecision", value: true);
					refereeVisual.playerAnimator.SetTrigger("ShowCard");
					refereeVisual.playerAnimator.ResetTrigger("IssueWarning");
					refereeCardMaterial.color = Color.yellow;
				}
				else if (match.engineState == 302)
				{
					refereeVisual.playerAnimator.SetBool("MadeDecision", value: true);
					refereeVisual.playerAnimator.SetTrigger("ShowCard");
					refereeVisual.playerAnimator.ResetTrigger("IssueWarning");
					refereeCardMaterial.color = Color.red;
				}
				else if (match.engineState == 307)
				{
					refereeVisual.playerAnimator.SetBool("MadeDecision", value: true);
					refereeVisual.playerAnimator.SetTrigger("ShowCard");
					refereeVisual.playerAnimator.SetTrigger("ShowCard2");
					refereeVisual.playerAnimator.ResetTrigger("IssueWarning");
					refereeCardMaterial.color = Color.yellow;
				}
				else if (match.engineState == 306 && match.engineStateInfo.ContainsKey("refState") && (int)match.engineStateInfo["refState"] == 3)
				{
					refereeVisual.playerAnimator.SetBool("MadeDecision", value: true);
					refereeVisual.playerAnimator.SetTrigger("IssueWarning");
					refereeCardInstance.SetActive(value: false);
				}
				lastRefereeState = match.engineState;
			}
			if (match.engineState == 307 && refereeVisual.playerAnimator.GetCurrentAnimatorStateInfo(3).IsTag("RedCard"))
			{
				refereeCardMaterial.color = Color.red;
			}
			if (UnityDeltaTime > 0f)
			{
				match.referee.UpdateSpeedAndRotation(time, liveTime);
			}
			if (match.referee.refState == 0 && match.engineState != 301 && match.engineState != 302 && match.engineState != 305 && match.engineState != 307)
			{
				if (refereeActionStarted)
				{
					ShouldHideSkipButton(SkipReason.REFEREE);
					refereeActionStarted = false;
				}
			}
			else if (!refereeActionStarted)
			{
				ShouldShowSkipButton(SkipReason.REFEREE);
				refereeActionStarted = true;
			}
			for (int j = 0; j < 2; j++)
			{
				if (lastLinesmenStates[j] != match.linesmen[j].lineState)
				{
					if (match.linesmen[j].lineState == 1)
					{
						if (!linesmenVisuals[j].playerIK.isFlagging)
						{
							linesmenVisuals[j].playerAnimator.SetTrigger("Offside");
							linesmenVisuals[j].playerIK.isFlagging = true;
						}
					}
					else
					{
						linesmenVisuals[j].playerIK.isFlagging = false;
					}
					lastLinesmenStates[j] = match.linesmen[j].lineState;
				}
				if (UnityDeltaTime > 0f)
				{
					match.linesmen[j].UpdateSpeedAndRotation(time, liveTime);
				}
			}
			if (match.physio.physioState == 0)
			{
				if (physioVisible)
				{
					physioVisual.playerObject.SetActive(value: false);
					physioVisible = false;
				}
				ShouldHideSkipButton(SkipReason.PHYSIO);
			}
			else
			{
				if (!physioVisible)
				{
					physioVisual.playerObject.SetActive(value: true);
					physioVisible = true;
					physioVisual.playerAnimator.SetLayerWeight(5, 1f);
					physioVisual.playerAnimator.SetLayerWeight(1, 0f);
					physioVisual.playerAnimator.SetLayerWeight(2, 0f);
					physioVisual.playerAnimator.SetLayerWeight(6, 0f);
				}
				if (lastPhysioState != match.engineState)
				{
					if (match.engineState == 403)
					{
						physioVisual.playerAnimator.SetBool("PhysioCrouch", value: true);
					}
					else
					{
						physioVisual.playerAnimator.SetBool("PhysioCrouch", value: false);
					}
					lastPhysioState = match.engineState;
				}
				if (UnityDeltaTime > 0f)
				{
					match.physio.UpdateSpeedAndRotation(time, liveTime);
				}
				ShouldShowSkipButton(SkipReason.PHYSIO);
			}
			currInReplay = IsInReplay;
			if (currInReplay)
			{
				ShouldShowSkipButton(SkipReason.REPLAY);
				if (currInReplay != prevInReplay)
				{
					replayUIOverlay.SetActive(value: true);
					matchCanvas.transform.Find("MatchOverlay").GetComponent<Animator>().SetTrigger("ReplayStart");
				}
			}
			else
			{
				ShouldHideSkipButton(SkipReason.REPLAY);
				if (currInReplay != prevInReplay)
				{
					replayUIOverlay.SetActive(value: false);
					matchCanvas.transform.Find("MatchOverlay").GetComponent<Animator>().SetTrigger("ReplayEnd");
				}
			}
			prevInReplay = currInReplay;
			if (medic.activeSelf)
			{
				medic.SetActive(value: false);
			}
			adjPos = pitchToScreen(ballPos);
			bool num2 = match.engineState == 10 || match.engineState == 206 || match.engineState == 205 || match.engineState == 66 || match.engineState == 67 || match.engineState == 65;
			bool flag2 = match.engineState == 9 || match.engineState == 73 || match.engineState == 110 || match.engineState == 104 || match.engineState == 72 || match.engineState == 204;
			bool flag3 = match.engineState == 100 || match.engineState == 224 || match.engineState == 108 || match.engineState == 107 || match.engineState == 112 || match.engineState == 601 || match.engineState == 206;
			bool flag4 = match.nextEngineState == 100 || match.nextEngineState == 224 || match.nextEngineState == 108 || match.nextEngineState == 107 || match.nextEngineState == 112 || match.nextEngineState == 601 || match.nextEngineState == 206;
			if (num2 | flag2)
			{
				v3d.copy(ballPosFuture, ballToGoalPos);
				v3d.sub(ballToGoalPos, sm.places.goal[sm.match.teams[sm.match.teamInPlay].OtherTeam.dirIndex]);
				v3d.normalize(ballToGoalPos);
				v3d.scale(ballToGoalPos, 0f - Mathf.Clamp(v3d.dist(ballPosFuture, sm.places.goal[sm.match.teams[sm.match.teamInPlay].OtherTeam.dirIndex]) / 2f, 0f, 10f));
				v3d.add(ballToGoalPos, ballPosFuture);
				adjPosAhead = pitchToScreen(ballToGoalPos);
			}
			else
			{
				if (flag3 | flag4)
				{
					v3d v3d2 = null;
					if (match.engineStateInfo != null && match.engineStateInfo.ContainsKey("pPos"))
					{
						v3d2 = (v3d)match.engineStateInfo["pPos"];
					}
					else if (match.nextEngineStateInfo != null && match.nextEngineStateInfo.ContainsKey("pPos"))
					{
						v3d2 = (v3d)match.nextEngineStateInfo["pPos"];
					}
					if (v3d2 != null)
					{
						ballPosFuture = v3d.clone(v3d2);
					}
				}
				if (v3d.distSquared(ballPosFuture, sm.places.goal[0]) < 256f)
				{
					ballPosFuture.vx = -44f;
					ballPosFuture.vy = 0f;
					ballPosFuture.vz = 0f;
				}
				else if (v3d.distSquared(ballPosFuture, sm.places.goal[1]) < 256f)
				{
					ballPosFuture.vx = 44f;
					ballPosFuture.vy = 0f;
					ballPosFuture.vz = 0f;
				}
				adjPosAhead = pitchToScreen(ballPosFuture);
			}
			adjPosVisual = new Vector3(0f, pitchToScreen(ballPos, useHeight: true).y / ball.transform.localScale.y, 0f);
			if (debugBallMover)
			{
				adjPos.x = ballMoverPos.x;
				adjPos.z = ballMoverPos.y;
				adjPosAhead.x = ballMoverPos.x;
				adjPosAhead.z = ballMoverPos.y;
				adjPosVisual.y = ballMoverPos.z;
			}
			if (adjPos.y < 0.11f)
			{
				adjPos.y = 0.11f;
			}
			cPlayer playerAbs = cBall2.getPlayerAbs(time);
			if (playerAbs == null)
			{
				cBall2.angVel = v3d.cross(cBall2.getVelocity(liveTime), new v3d(0f, 0f, 1f), makeNew: true);
			}
			else
			{
				cBall2.angVel = new v3d();
			}
			Quaternion quaternion = new Quaternion(0f - cBall2.angVel.vx, cBall2.angVel.vz, cBall2.angVel.vy, 0f);
			quaternion *= ballRot;
			float num3 = 0.5f * deltaTime;
			ballRot.x += quaternion.x * num3;
			ballRot.y += quaternion.y * num3;
			ballRot.z += quaternion.z * num3;
			ballRot.w += quaternion.w * num3;
			ballRot = cQuat.Normalize(ballRot);
			_ = ballPos.vz;
			ball.transform.position = adjPos;
			if (playerAbs != null)
			{
				player = playerAbs;
				Transform transform = (playerAbs.currentVisual.playerAnimator.GetBool("MirrorAnim") ? playerAbs.currentVisual.playerAnimator.GetBoneTransform(HumanBodyBones.LeftHand).Find("LeftPalm") : playerAbs.currentVisual.playerAnimator.GetBoneTransform(HumanBodyBones.RightHand).Find("RightPalm"));
				if (lastPlayerHoldingBall == null && !player.isReleasingBall && !player.isDiving && !playerAbs.currentVisual.playerAnimator.GetCurrentAnimatorStateInfo(0).IsTag("Uninterrupted"))
				{
					lastPlayerHoldingBall = playerAbs;
					lastPlayerHoldingBall.currentVisual.playerAnimator.SetBool("HoldBall", value: true);
					lastPlayerHoldingBall.isHoldingBall = true;
				}
				if (transform != null)
				{
					ballVisual.position = transform.position;
				}
				if (lastPlayerHoldingBall != null && !player.isReleasingBall)
				{
					AnimatorStateInfo currentAnimatorStateInfo = player.currentVisual.playerAnimator.GetCurrentAnimatorStateInfo(0);
					AnimatorStateInfo nextAnimatorStateInfo = player.currentVisual.playerAnimator.GetNextAnimatorStateInfo(0);
					if (currentAnimatorStateInfo.IsTag("GKReleasing") || nextAnimatorStateInfo.IsTag("GKReleasing"))
					{
						lastPlayerHoldingBall.isReleasingBall = true;
					}
					else if (player.currActionLayerWeight > 0f && player.currentPlayerAction.clipData.animName == PlayerAnimationData.AnimationName.Throw_In)
					{
						lastPlayerHoldingBall.isReleasingBall = true;
					}
				}
			}
			else
			{
				if (lastPlayerHoldingBall != null)
				{
					lastPlayerHoldingBall.currentVisual.playerAnimator.SetBool("HoldBall", value: false);
					lastPlayerHoldingBall.isHoldingBall = false;
					lastPlayerHoldingBall.isReleasingBall = false;
					lastPlayerHoldingBall = null;
				}
				ballVisual.localPosition = adjPosVisual;
			}
			for (int k = 0; k < 2; k++)
			{
				currentTeam = match.teams[teamStrings[k]];
				foreach (cPlayer value in currentTeam.teamPos.Values)
				{
					player = value;
					if (!player.isReleasingBall)
					{
						continue;
					}
					if (player.currentVisual.playerIK.isGoalkeeper)
					{
						AnimatorStateInfo currentAnimatorStateInfo2 = player.currentVisual.playerAnimator.GetCurrentAnimatorStateInfo(0);
						AnimatorStateInfo nextAnimatorStateInfo2 = player.currentVisual.playerAnimator.GetNextAnimatorStateInfo(0);
						if (!currentAnimatorStateInfo2.IsTag("GKReleasing") && !nextAnimatorStateInfo2.IsTag("GKReleasing"))
						{
							player.isReleasingBall = false;
						}
					}
					else if (player.currActionLayerWeight == 0f)
					{
						player.isReleasingBall = false;
					}
					else if (player.currentPlayerAction.clipData.animName != PlayerAnimationData.AnimationName.Throw_In)
					{
						player.isReleasingBall = false;
					}
				}
			}
			ballVisual.localRotation = ballRot;
			if (InFastMode)
			{
				if (!matchIntroStarted)
				{
					matchCanvas.transform.Find("MatchOverlay").GetComponent<Animator>().SetBool("QuickMatch", value: true);
					matchCanvas.transform.Find("MatchOverlay").GetComponent<Animator>().SetTrigger("SkipIntro");
					matchIntroStarted = true;
				}
			}
			else if (match.engineState == 250 && !matchIntroStarted)
			{
				matchCanvas.transform.Find("MatchOverlay").GetComponent<Animator>().SetTrigger("FadeIn");
				matchIntroStarted = true;
			}
			if (match.engineState == 210)
			{
				if (manyMatches.Running)
				{
					debugMatchEnd();
				}
				else if (!matchOutroStarted)
				{
					if (Game.instance.DEBUG_TOOLS_PROCESSING_MATCH_ENGINE)
					{
						StartCoroutine(ShowMatchOverDialog(0f));
					}
					else if (GetTrophyCutSceneTeam() >= 0)
					{
						StartCoroutine(ShowMatchOverDialog(8f));
					}
					else
					{
						StartCoroutine(ShowMatchOverDialog(0f));
					}
					ChangeView(MatchView.Pitch);
					matchOutroStarted = true;
					matchFinishTime = time + 30f;
					MatchBillboardController.instance.MatchEnded();
				}
				else if (!matchFinished && time > matchFinishTime)
				{
					matchFinished = true;
				}
			}
			else if (match.engineState == 54)
			{
				if (!matchFullTimeOutroStarted && !InFastMode)
				{
					matchFullTimeOutroStarted = true;
					if (!match.shootoutComing)
					{
						if (match.matchStats.teams["home"].score == match.matchStats.teams["away"].score)
						{
							CutScenesController.instance.StartPostMatchDraw(1.5f, () =>
							{
							});
						}
						else
						{
							CutScenesController.instance.StartPostMatchWin(match.matchStats.teams["home"].score > match.matchStats.teams["away"].score, 1.5f, () =>
							{
							});
						}
					}
				}
			}
			else if (match.engineState == 53)
			{
				if (!matchHalfTimeOutroStarted && !InFastMode)
				{
					matchHalfTimeOutroStarted = true;
					CutScenesController.instance.StartHalfTime(1.5f, () =>
					{
					});
				}
			}
			else if (match.engineState == 607 && !shootoutEndOutroStarted && !InFastMode)
			{
				Dictionary<string, object> engineStateInfo = match.engineStateInfo;
				shootoutEndOutroStarted = true;
				CutScenesController.instance.StartPenaltyShootout((string)engineStateInfo["winners"] == "home", 1f, (int)engineStateInfo["celebrator"], () =>
				{
				});
			}
			if (!matchFinished)
			{
				CheckForcedTactics();
			}
			if (Time.frameCount % 30 == 0)
			{
				topPanel.transform.Find("TacticsPendingHome").gameObject.SetActive(sm.match.getTeamTacticsAtTime("home", time).pending);
				topPanel.transform.Find("TacticsPendingAway").gameObject.SetActive(sm.match.getTeamTacticsAtTime("away", time).pending);
			}
			if (currentView == MatchView.Tactics)
			{
				bool flag5 = Enumerable.SequenceEqual(sm.match.getTeamTactics(homeAway, ignorePending: false).lineUp, TacticsController.GetInstance().ModifiedMatchTactics.lineUp);
				bool flag6 = TacticsController.GetInstance().MatchTactics.inst.equal(TacticsController.GetInstance().ModifiedMatchTactics.inst);
				if (sm.match.getTeamTactics(homeAway, ignorePending: false).pending)
				{
					tacticsControls.transform.Find("ResetBtn").GetComponent<Button>().enabled = true;
					tacticsControls.transform.Find("ResetBtn").GetComponent<Image>().color = Color.blue;
					if (flag5 && flag6)
					{
					}
				}
				else if (!flag5 || !flag6)
				{
					tacticsControls.transform.Find("ResetBtn").GetComponent<Button>().enabled = true;
					tacticsControls.transform.Find("ResetBtn").GetComponent<Image>().color = Color.blue;
				}
				else
				{
					tacticsControls.transform.Find("ResetBtn").GetComponent<Button>().enabled = false;
					tacticsControls.transform.Find("ResetBtn").GetComponent<Image>().color = Color.grey;
				}
			}
			tacticsAllowed = !sm.match.isSubState(sm.match.engineState) && matchStarted && !sm.match.isTacticsAllowed(sm.match.engineState) && !sm.match.isTacticsAllowed(sm.match.nextEngineState);
			topPanel.transform.Find("MatchControls/Tactics").GetComponent<CanvasGroup>().alpha = (tacticsAllowed ? 1f : 0.4f);
		}
		bool flag7 = false;
		if (narrowScreen && currentView == MatchView.Pitch && timeScale <= 2f)
		{
			flag7 = true;
		}
		else if (currentView == MatchView.Overview)
		{
			flag7 = true;
		}
		if (timeScale > 2f)
		{
			flag7 = false;
		}
		if (tacticsOpen)
		{
			flag7 = false;
		}
		if (!firstCommentaryShown || match.engineState == 52 || match.engineState == 210)
		{
			flag7 = false;
		}
		if (highlightMode == 5)
		{
			flag7 = false;
		}
		if (matchPaused && statsActive)
		{
			flag7 = false;
		}
		commentaryBar.alpha = (flag7 ? 1f : 0f);
		if (currentView != MatchView.Tactics)
		{
			if (match.lastEvent != match.currentEvent)
			{
				bool flag8 = match.currentEvent == MatchEvent.GOAL_CELEBRATION_END;
				bool flag9 = match.currentEvent == MatchEvent.NO_CELEBRATION;
				bool flag10 = lastReplayTime != match.prevStateTime + 3f;
				bool flag11 = false;
				int i = highlightMode;
				flag11 = ((i != 5) ? (replaysEnabled == eReplayType.Enabled) : AllowQuickMatchReplays);
				if ((flag8 | flag9) & flag10 & flag11)
				{
					RequestReplay();
				}
				UpdateEvents();
			}
			UpdateCamera();
		}
		if (replayRequested && time > replayBeginTime)
		{
			StartReplay();
		}
		match.lastEvent = match.currentEvent;
		lastPlayerOnBall = match.playerOnBall;
		if (!InFastMode)
		{
			bool flag12 = false;
			bool flag13 = false;
			if (Mathf.Abs(ballPos.vx) > sm.places.penaltyArea[1].topLeft.vx && Mathf.Abs(ballPos.vy) < Mathf.Abs(sm.places.frontPost[1].vy) + 1f)
			{
				if (ballPos.vx > 0f)
				{
					flag13 = true;
				}
				else
				{
					flag12 = true;
				}
			}
			homeDynamicGoalNetting.SetActive(flag12);
			awayDynamicGoalNetting.SetActive(flag13);
			homeStaticGoalNetting.SetActive(!flag12);
			awayStaticGoalNetting.SetActive(!flag13);
		}
		if (InFastMode)
		{
			if (linesmanFlagCloths[0].enabled)
			{
				linesmanFlagCloths[0].enabled = false;
				linesmanFlagCloths[1].enabled = false;
			}
		}
		else if (!linesmanFlagCloths[0].enabled)
		{
			linesmanFlagCloths[0].enabled = true;
			linesmanFlagCloths[1].enabled = true;
		}
		if (timeScale > 0f)
		{
			matchViewTimes[(currentMatchView != MatchViewType.View_2D) ? 1 : 0] += UnityDeltaTime;
		}
		if (!completedFirstUpdate)
		{
			completedFirstUpdate = true;
		}
	}

	private IEnumerator ShowMatchOverDialog(float delay)
	{
		yield return new WaitForSeconds(delay);
		matchCanvas.transform.Find("MatchOverlay").GetComponent<Animator>().SetTrigger("FadeOut");
		Transform transform = GameObject.Find("OverlayCanvasMatch/MatchOverlay/OutroScores").transform;
		transform.Find("HomeScore").GetComponent<Text>().text = match.matchStats.teams["home"].score.ToString();
		transform.Find("AwayScore").GetComponent<Text>().text = match.matchStats.teams["away"].score.ToString();
		if (seFixInfo.fixtureInfo.isLeg2)
		{
			transform.Find("AggrBox").GetComponent<CanvasGroup>().alpha = 1f;
			transform.Find("AggrBox/AggText").GetComponent<Text>().text = Translation.GetText("aggregate-score");
			transform.Find("AggrBox/AggHomeScore").GetComponent<Text>().text = (seFixInfo.fixtureInfo.leg1AwayScore + sm.match.matchStats.teams["home"].score).ToString();
			transform.Find("AggrBox/AggAwayScore").GetComponent<Text>().text = (seFixInfo.fixtureInfo.leg1HomeScore + sm.match.matchStats.teams["away"].score).ToString();
		}
		else
		{
			transform.Find("AggrBox").GetComponent<CanvasGroup>().alpha = 0f;
		}
		if (shootout)
		{
			transform.Find("PensBox").GetComponent<CanvasGroup>().alpha = 1f;
			transform.Find("PensBox/PenText").GetComponent<Text>().text = Translation.GetText("penalties");
			transform.Find("PensBox/PenHomeScore").GetComponent<Text>().text = match.matchStats.teams["home"].shootout.ToString();
			transform.Find("PensBox/PenAwayScore").GetComponent<Text>().text = match.matchStats.teams["away"].shootout.ToString();
		}
		else
		{
			transform.Find("PensBox").GetComponent<CanvasGroup>().alpha = 0f;
		}
		if (quickMatchScreenExists)
		{
			quickMatchScreen.transform.Find("EndContinueButton").gameObject.SetActive(value: true);
			if (Game.instance.DEBUG_TOOLS_PROCESSING_MATCH_ENGINE)
			{
				quickMatchScreen.transform.Find("EndContinueButton").GetComponent<Button>().onClick.Invoke();
			}
		}
	}

	public void UpdateAllPlayersVisbility()
	{
		int num = 0;
		for (int i = 0; i < 2; i++)
		{
			currentTeam = match.teams[teamStrings[i]];
			if (!CutScenesController.instance.PlayersVisible)
			{
				foreach (cPlayer value in currentTeam.playerCache.Values)
				{
					PlayerVisuals playerFromIndex = getPlayerFromIndex(num, value.playerNum - 1);
					if (value.isSetActive)
					{
						playerFromIndex.playerObject.SetActive(value: false);
						value.isSetActive = false;
					}
				}
			}
			else if (InFastMode && match.engineState == 210)
			{
				foreach (cPlayer value2 in currentTeam.playerCache.Values)
				{
					PlayerVisuals playerFromIndex = getPlayerFromIndex(num, value2.playerNum - 1);
					if (value2.isSetActive)
					{
						playerFromIndex.playerObject.SetActive(value: false);
						value2.isSetActive = false;
					}
				}
			}
			else
			{
				foreach (cPlayer value3 in currentTeam.playerCache.Values)
				{
					player = value3;
					PlayerVisuals playerFromIndex = getPlayerFromIndex(num, player.playerNum - 1);
					if (player.onPitch)
					{
						if (!player.isSetActive)
						{
							playerFromIndex.playerObject.SetActive(value: true);
							player.isSetActive = true;
						}
					}
					else if (player.isSetActive)
					{
						playerFromIndex.playerObject.SetActive(value: false);
						player.isSetActive = false;
					}
				}
			}
			num++;
		}
		UpdateRefAndLinesmmenVisibility();
	}

	private void UpdateRefAndLinesmmenVisibility()
	{
		if (!CutScenesController.instance.PlayersVisible)
		{
			if (refereeVisual.playerObject.activeSelf)
			{
				refereeVisual.playerObject.SetActive(value: false);
			}
			if (linesmenVisuals[0].playerObject.activeSelf)
			{
				linesmenVisuals[0].playerObject.SetActive(value: false);
			}
			if (linesmenVisuals[1].playerObject.activeSelf)
			{
				linesmenVisuals[1].playerObject.SetActive(value: false);
			}
		}
		else
		{
			if (!refereeVisual.playerObject.activeSelf)
			{
				refereeVisual.playerObject.SetActive(value: true);
			}
			if (!linesmenVisuals[0].playerObject.activeSelf)
			{
				linesmenVisuals[0].playerObject.SetActive(value: true);
			}
			if (!linesmenVisuals[1].playerObject.activeSelf)
			{
				linesmenVisuals[1].playerObject.SetActive(value: true);
			}
		}
	}

	private bool YourTeamInPossession()
	{
		return match.teamInPlay == (IsHome ? "home" : "away");
	}

	public void ChangeEnds()
	{
		swappedEnds = !swappedEnds;
	}

	public void liveUpdate(cState state)
	{
		if (sm == null)
		{
			return;
		}
		cMatch cMatch2 = sm.match;
		if (state.engineState == 2 && lastEventTime != state.time)
		{
			lastEventTime = state.time;
			cPlayerID[] array = (cPlayerID[])state.engineStateInfo["players"];
			int playerNum = array[0].playerNum;
			string team = array[0].team;
			if (!state.engineStateInfo.ContainsKey("goalType"))
			{
				state.engineStateInfo["goalType"] = 0;
			}
			string text = team;
			if ((int)state.engineStateInfo["goalType"] == 4)
			{
				text = ((team == "home") ? "away" : "home");
			}
			if (sm.matchStats.teams[text].scorerInfo[playerNum].Count > 1)
			{
				UpdateMatchEvents(text, playerNum, "goal", (int)state.engineStateInfo["goalType"]);
			}
			else
			{
				CreateMatchEventUI(text, playerNum, state.engineState, (int)state.engineStateInfo["goalType"]);
			}
			if (Constants.LIVE_SCORES_ENABLED)
			{
				if (g_matchday_nextfix.competitiontype == 2)
				{
					UpdateLiveTableStats(-1);
					UpdateLivetable(null, -1);
				}
				else if (g_matchday_nextfix.competitiontype == 4 && Game.instance.g_continentalCompetitions[g_matchday_nextfix.competitionid].Status == ContinentalCompStatus.GroupStage)
				{
					UpdateLiveTableStats(-1);
					UpdateLivetable(null, -1, continentalCup: true);
				}
			}
		}
		if ((state.engineState == 307 && lastEventTime != state.time) || (state.engineState == 301 && lastEventTime != state.time))
		{
			lastEventTime = state.time;
			string text2 = (string)state.engineStateInfo["team"];
			int num = (int)state.engineStateInfo["playerNum"];
			bool flag = sm.matchStats.teams[text2].ycInfo[num].Count == 2;
			_ = sm.matchStats.teams[text2].rcInfo[num];
			if (text2 == homeAway)
			{
				TacticsController.GetInstance().SetPlayerYellowCards(sm.match.teams[text2].teamInfo.players[num].id);
			}
			if (flag)
			{
				UpdateMatchEvents(text2, num, "yellow");
			}
			else
			{
				CreateMatchEventUI(text2, num, state.engineState);
			}
		}
		if (state.engineState == 302 && lastEventTime != state.time)
		{
			lastEventTime = state.time;
			string team2 = (string)state.engineStateInfo["team"];
			int playerIx = (int)state.engineStateInfo["playerNum"];
			CreateMatchEventUI(team2, playerIx, state.engineState);
		}
		if (state.engineState == 501 || (state.engineState == 212 && cMatch2.halfTimeRun))
		{
			if (TacticsController.TacticsPending)
			{
				TacticsController.GetInstance().MatchTactics = sm.match.getTeamTactics(homeAway, ignorePending: false);
				TacticsController.GetInstance().ConfirmedTactics();
			}
			if (state.engineStateInfo != null && state.engineStateInfo.ContainsKey("team"))
			{
				if ((string)state.engineStateInfo["team"] == "home")
				{
					topPanel.transform.Find("SubIconHome").gameObject.SetActive(value: true);
				}
				else if ((string)state.engineStateInfo["team"] == "away")
				{
					topPanel.transform.Find("SubIconAway").gameObject.SetActive(value: true);
				}
			}
		}
		if (state.engineState == 501)
		{
			if ((string)state.engineStateInfo["team"] == "home")
			{
				topPanel.transform.Find("SubIconHome").gameObject.SetActive(value: true);
			}
			else if ((string)state.engineStateInfo["team"] == "away")
			{
				topPanel.transform.Find("SubIconAway").gameObject.SetActive(value: true);
			}
			if ((string)state.engineStateInfo["team"] == homeAway)
			{
				tacticsControls.transform.Find("ResetBtn").GetComponent<Button>().enabled = false;
				tacticsControls.transform.Find("ResetBtn").GetComponent<Image>().color = Color.grey;
			}
		}
		if (state.engineState == 503 && state.engineStateInfo != null && state.engineStateInfo.ContainsKey("team"))
		{
			if ((string)state.engineStateInfo["team"] == "home")
			{
				topPanel.transform.Find("SubIconHome").gameObject.SetActive(value: false);
			}
			else if ((string)state.engineStateInfo["team"] == "away")
			{
				topPanel.transform.Find("SubIconAway").gameObject.SetActive(value: false);
			}
		}
		if (state.engineState == 504 && (string)state.engineStateInfo["team"] == homeAway)
		{
			tacticsControls.transform.Find("ResetBtn").GetComponent<Button>().enabled = false;
			tacticsControls.transform.Find("ResetBtn").GetComponent<Image>().color = Color.grey;
		}
	}

	private void UpdateAllPlayers()
	{
		_ = match;
		for (int i = 0; i < 2; i++)
		{
			currentTeam = match.teams[teamStrings[i]];
			foreach (cPlayer value in currentTeam.playerCache.Values)
			{
				if (value.onPitch && UnityDeltaTime > 0f)
				{
					v3dCache.Add(value.livePosition);
					value.livePosition = value.getPosition(liveTime);
				}
			}
		}
		timeIntPrev = timeInt;
		int num = 0;
		match.playerCollision.update(time - match.time);
		teamNotInPlay = ((match.nextTeamInPlay == "home") ? match.teams["away"] : match.teams["home"]);
		int num2 = 1;
		if (match.teams[match.nextTeamInPlay].players.ContainsKey(match.nextPlayerOnBall))
		{
			num2 = teamNotInPlay.getClosestPlayerLive(match.teams[match.nextTeamInPlay].players[match.nextPlayerOnBall].currentPitchPos);
		}
		for (int j = 0; j < 2; j++)
		{
			currentTeam = match.teams[teamStrings[j]];
			if (!CutScenesController.instance.PlayersVisible)
			{
				foreach (cPlayer value2 in currentTeam.playerCache.Values)
				{
					PlayerVisuals playerFromIndex = getPlayerFromIndex(num, value2.playerNum - 1);
					if (value2.isSetActive)
					{
						playerFromIndex.playerObject.SetActive(value: false);
						value2.isSetActive = false;
					}
				}
			}
			else if (InFastMode && match.engineState == 210)
			{
				foreach (cPlayer value3 in currentTeam.playerCache.Values)
				{
					PlayerVisuals playerFromIndex = getPlayerFromIndex(num, value3.playerNum - 1);
					if (value3.isSetActive)
					{
						playerFromIndex.playerObject.SetActive(value: false);
						value3.isSetActive = false;
					}
				}
			}
			else
			{
				foreach (cPlayer value4 in currentTeam.playerCache.Values)
				{
					player = value4;
					_ = player;
					PlayerVisuals playerFromIndex = getPlayerFromIndex(num, player.playerNum - 1);
					if (player.onPitch)
					{
						if (!player.isSetActive)
						{
							playerFromIndex.playerObject.SetActive(value: true);
							player.isSetActive = true;
						}
					}
					else if (player.isSetActive)
					{
						playerFromIndex.playerObject.SetActive(value: false);
						player.isSetActive = false;
					}
				}
			}
			foreach (cPlayer value5 in currentTeam.playerCache.Values)
			{
				if (value5.onPitch)
				{
					player = value5;
					_ = player;
					Animator playerAnimator = player.currentVisual.playerAnimator;
					if (UnityDeltaTime > 0f)
					{
						player.UpdateSpeedAndRotation(liveTime);
					}
					if (match.engineState != 6)
					{
						if (player.playerNum == match.playerOnBall && currentTeam.homeAway == match.teamInPlay && !IsFreeBall() && match.engineState == 1)
						{
							playerAnimator.GetComponent<IKControl>().onBall = true;
						}
						else
						{
							onBallSoon = false;
							if (player.playerNum == match.nextPlayerOnBall && currentTeam.homeAway == match.nextTeamInPlay && v3d.dist(player.currentPitchPos, match.ball.currentPos) < 0.8f)
							{
								onBallSoon = true;
							}
							playerAnimator.GetComponent<IKControl>().onBall = onBallSoon;
						}
					}
					if (player.StandOff != player.isReadyToAct)
					{
						SetPlayerReadyToAct(player, player.StandOff);
					}
					intendedIKLookTarget = IKControl.IKLookTarget.BALL;
					intendedLookTarget = instance.ballLookTarget;
					forceLookTargetSet = false;
					if (match.engineState == 2)
					{
						if (liveTime > 1f)
						{
							match.engineStateInfo.ContainsKey("players");
							cPlayerID[] array = (cPlayerID[])match.engineStateInfo["players"];
							if (array[0].team == player.team.homeAway)
							{
								if (!player.isFacingScorer)
								{
									player.isFacingScorer = true;
									if (array[0].playerNum != player.playerNum)
									{
										intendedIKLookTarget = IKControl.IKLookTarget.CELEBRATOR;
										intendedLookTarget = match.teams[array[0].team].players[array[0].playerNum].currentVisual.playerHead;
										forceLookTargetSet = true;
									}
									else if (!player.hasAnimatedLookDirection)
									{
										intendedIKLookTarget = IKControl.IKLookTarget.CROWD;
										intendedLookTarget = crowdLookTarget.transform;
										forceLookTargetSet = true;
									}
								}
							}
							else if (!player.isUpset)
							{
								if (player.teamPos == 1)
								{
									if (!player.currentVisual.playerAnimator.GetCurrentAnimatorStateInfo(0).IsTag("Uninterrupted"))
									{
										MakePlayerUpset(player, repeat: true);
									}
								}
								else
								{
									MakePlayerUpset(player, repeat: true);
								}
							}
						}
						ShouldShowSkipButton(SkipReason.GOAL_CELEBRATION);
					}
					else if (match.nextEngineState == 607 && (string)match.engineStateInfo["winners"] == player.team.homeAway)
					{
						match.engineStateInfo.ContainsKey("celebrator");
						int num3 = (int)match.engineStateInfo["celebrator"];
						string key = (string)match.engineStateInfo["winners"];
						if (num3 != player.playerNum)
						{
							intendedIKLookTarget = IKControl.IKLookTarget.CELEBRATOR;
							intendedLookTarget = match.teams[key].playerCache[num3].currentVisual.playerHead;
							forceLookTargetSet = true;
						}
						else if (!player.hasAnimatedLookDirection)
						{
							intendedIKLookTarget = IKControl.IKLookTarget.CROWD;
							intendedLookTarget = crowdLookTarget.transform;
							forceLookTargetSet = true;
						}
					}
					else
					{
						if (player.celebrationRole != cPlayer.CelebrateRole.NONE || player.isFacingScorer)
						{
							MakePlayerStopCelebrating(player);
						}
						else if (player.isUpset)
						{
							MakePlayerStopBeingUpset(player);
						}
						if (player.currentFormationData.formationAction == formationActions.MARKING)
						{
							intendedIKLookTarget = IKControl.IKLookTarget.MARKER;
							intendedLookTarget = player.currentFormationData.formationActionTargetPlayer.currentVisual.playerHead;
						}
						if (match.engineState == 305 || match.engineState == 301 || match.engineState == 302 || match.engineState == 307 || match.engineState == 306)
						{
							intendedIKLookTarget = IKControl.IKLookTarget.REFEREE;
							intendedLookTarget = refereeVisual.playerHead;
						}
						ShouldHideSkipButton(SkipReason.GOAL_CELEBRATION);
					}
					if (!player.isFacingScorer || forceLookTargetSet)
					{
						player.currentVisual.playerIK.SetLookTarget(intendedIKLookTarget, intendedLookTarget);
					}
					if (match.engineState == 504)
					{
						match.engineStateInfo.ContainsKey("playersOff");
						match.engineStateInfo.ContainsKey("playersOn");
						match.engineStateInfo.ContainsKey("team");
						List<int> list = (List<int>)match.engineStateInfo["playersOff"];
						List<int> list2 = (List<int>)match.engineStateInfo["playersOn"];
						if ((string)match.engineStateInfo["team"] == player.team.homeAway)
						{
							for (int k = 0; k < list.Count; k++)
							{
								if (list2[k] == player.teamPos)
								{
									if (!player.isClapping && player.velocityThisFrame.sqrMagnitude < 0.25f)
									{
										MakePlayerClap(player);
									}
									else if (player.velocityThisFrame.sqrMagnitude > 0.25f)
									{
										MakePlayerStopClapping(player);
									}
								}
								if (list[k] == player.teamPos)
								{
									if (!player.isClapping && player.velocityThisFrame.sqrMagnitude > 0.010000001f)
									{
										MakePlayerClap(player, high: true);
									}
									else if (player.velocityThisFrame.sqrMagnitude < 0.010000001f)
									{
										MakePlayerStopClapping(player);
									}
								}
							}
						}
						ShouldShowSkipButton(SkipReason.SUB);
					}
					else
					{
						if (player.isClapping)
						{
							MakePlayerStopClapping(player);
						}
						ShouldHideSkipButton(SkipReason.SUB);
					}
					if (match.engineState == 66 || match.engineState == 67 || match.engineState == 65 || match.engineState == 10)
					{
						if (!player.isInWall && player.velocityThisFrame.sqrMagnitude < 0.010000001f)
						{
							if (match.teamInPlay != player.team.homeAway && match.engineStateInfo != null)
							{
								match.engineStateInfo.ContainsKey("wallPlayers");
								if (match.engineStateInfo.ContainsKey("wallPlayers"))
								{
									foreach (int item in (List<int>)match.engineStateInfo["wallPlayers"])
									{
										if (item == player.playerNum)
										{
											player.isInWall = true;
											player.currentVisual.playerAnimator.SetTrigger("StandWall");
											player.currentVisual.playerAnimator.SetBool("InWall", value: true);
										}
									}
								}
							}
						}
						else if (player.isInWall && player.velocityThisFrame.sqrMagnitude > 0.010000001f)
						{
							player.isInWall = false;
							player.currentVisual.playerAnimator.SetBool("InWall", value: false);
						}
					}
					else if (player.isInWall)
					{
						player.isInWall = false;
						player.currentVisual.playerAnimator.SetBool("InWall", value: false);
					}
					if (player.isInjured && match.engineState == 1)
					{
						SetPlayerFouledState(player, fouled: false);
					}
					bool showName = false;
					if (currentTeam.homeAway == match.nextTeamInPlay)
					{
						if (player.playerNum == match.nextPlayerOnBall)
						{
							showName = true;
						}
					}
					else if (num2 == player.playerNum)
					{
						showName = true;
					}
					if (match.engineState == 2)
					{
						showName = ((player.playerNum == match.playerOnBall && player.team.homeAway == match.teamInPlay) ? true : false);
					}
					else if (match.prevEngineState == 2 || match.engineState == 250 || match.engineState == 251 || match.engineState == 252)
					{
						showName = false;
					}
					else if (match.engineState == 305 || match.engineState == 301 || match.engineState == 307 || match.engineState == 302 || match.engineState == 303 || match.engineState == 306)
					{
						showName = false;
						if (match.engineStateInfo.ContainsKey("playerNum") && match.engineStateInfo.ContainsKey("team") && player.playerNum == (int)match.engineStateInfo["playerNum"] && player.team.homeAway == (string)match.engineStateInfo["team"])
						{
							showName = true;
						}
					}
					else if (player.showNameOverride)
					{
						if (match.engineState == 504 || match.engineState == 505)
						{
							if (!player.subStarted)
							{
								player.subStarted = true;
							}
							showName = true;
						}
						else if (player.subStarted)
						{
							player.showNameOverride = false;
							player.subStarted = false;
						}
					}
					else if (match.squads[homeAway].isSentOff(player.playerNum - 1))
					{
						showName = false;
					}
					if (IsFreeBall())
					{
						showName = false;
					}
					if (!debugShowPlayerNames)
					{
						showName = false;
					}
					_ = player.team.homeAway == GetTeamTypeString(isHome);
					player.currentVisual.playerCircle.showName = showName;
				}
				num++;
			}
		}
	}

	private void ClampPlayers()
	{
		homeCD1 = float.PositiveInfinity;
		homeCD2 = float.PositiveInfinity;
		awayCD1 = float.PositiveInfinity;
		awayCD2 = float.PositiveInfinity;
		homeCD1Pl = 0;
		homeCD2Pl = 0;
		awayCD1Pl = 0;
		awayCD2Pl = 0;
		float num = 0f;
		string[] array = teamStrings;
		foreach (string text in array)
		{
			team1 = match.teams[text];
			foreach (KeyValuePair<int, cPlayer> teamPo in team1.teamPos)
			{
				player1 = teamPo.Value;
				pos = match.clampInsideWalls(GetPlayerPosition(player1, liveTime));
				num = v3d.dist(ballPos, pos);
				if (text == "home")
				{
					if (num < homeCD1)
					{
						homeCD2 = homeCD1;
						homeCD2Pl = homeCD1Pl;
						homeCD1 = num;
						homeCD1Pl = player1.playerNum;
					}
					else if (num < homeCD2)
					{
						homeCD2 = num;
						homeCD2Pl = player1.playerNum;
					}
				}
				else if (num < awayCD1)
				{
					awayCD2 = awayCD1;
					awayCD2Pl = awayCD1Pl;
					awayCD1 = num;
					awayCD1Pl = player1.playerNum;
				}
				else if (num < awayCD2)
				{
					awayCD2 = num;
					awayCD2Pl = player1.playerNum;
				}
			}
		}
	}

	public bool IsFreeBall()
	{
		if (sm.match.teamInPlay == sm.match.nextTeamInPlay)
		{
			bool result = sm.match.nextPlayerOnBall != sm.match.playerOnBall;
			if (sm.match.engineState == 100 || sm.match.nextEngineState == 100)
			{
				result = true;
			}
			return result;
		}
		return true;
	}

	public bool IsPass()
	{
		if (match.playerOnBall != match.nextPlayerOnBall)
		{
			return match.teamInPlay == match.nextTeamInPlay;
		}
		return false;
	}

	public void MakePlayerShoot(cPlayer player)
	{
		player.currentVisual.playerAnimator.SetTrigger("Shoot");
	}

	public void ResetMainAnimatorLayer(cPlayer player)
	{
		player.currentVisual.playerAnimator.SetTrigger("Reset");
	}

	public void MakePlayerPass(cPlayer player, float power)
	{
		Animator playerAnimator = player.currentVisual.playerAnimator;
		playerAnimator.SetFloat("Power", power);
		playerAnimator.SetTrigger("Kick");
		_ = 1f;
	}

	public void MakePlayerTackle(cPlayer player)
	{
		Animator playerAnimator = player.currentVisual.playerAnimator;
		playerAnimator.SetFloat("TackleNum", (float)globalRand.Next(0, 10) / 10f);
		playerAnimator.SetTrigger("Tackle");
	}

	public void MakePlayerStopCelebrating(cPlayer player)
	{
		player.celebrationRole = cPlayer.CelebrateRole.NONE;
		player.isFacingScorer = false;
		player.currentVisual.playerAnimator.SetBool("CelebrateRepeat", value: false);
		player.currentVisual.playerAnimator.SetBool("CelebrateBackground", value: false);
		player.currentVisual.playerAnimator.SetBool("Trotting", value: false);
	}

	public void MakePlayerDoCelebration(cPlayer player, cSoccerEngine.celebTypes type)
	{
		player.celebrationRole = cPlayer.CelebrateRole.SCORER;
		float num = (float)globalRand.Next(-5, 5) / 100f;
		float value = (float)globalRand.Next(90, 110) / 100f;
		switch (type)
		{
		case cSoccerEngine.celebTypes.KNEE_SLIDE:
			player.hasAnimatedLookDirection = false;
			player.celebrationLayerWeight = 0f;
			player.currentVisual.playerAnimator.SetTrigger("KneeSlide");
			return;
		case cSoccerEngine.celebTypes.CHEST_SLIDE:
			player.hasAnimatedLookDirection = false;
			player.celebrationLayerWeight = 0f;
			player.currentVisual.playerAnimator.SetTrigger("ChestSlide");
			return;
		}
		player.hasAnimatedLookDirection = false;
		player.celebrationLayerWeight = 1f;
		player.currentVisual.playerAnimator.SetBool("Trotting", value: true);
		player.currentVisual.playerAnimator.SetBool("CelebrateRepeat", value: true);
		player.currentVisual.playerAnimator.SetFloat("CelebrateSpeed", value);
		switch (type)
		{
		case cSoccerEngine.celebTypes.ARMS_DOWN:
			player.currentVisual.playerAnimator.SetFloat("CelebrateNum", Mathf.Max(num, 0f));
			break;
		case cSoccerEngine.celebTypes.ARMS_OUT:
			player.currentVisual.playerAnimator.SetFloat("CelebrateNum", 1f + num);
			break;
		case cSoccerEngine.celebTypes.HANDS_OUT:
			player.currentVisual.playerAnimator.SetFloat("CelebrateNum", 2f + num);
			break;
		case cSoccerEngine.celebTypes.POINT_TO_SKY:
			player.currentVisual.playerAnimator.SetFloat("CelebrateNum", 3f);
			break;
		case cSoccerEngine.celebTypes.CAN_YOU_HEAR_ME:
			player.currentVisual.playerAnimator.SetFloat("CelebrateNum", 4f);
			break;
		case cSoccerEngine.celebTypes.AEROPLANE:
			player.currentVisual.playerAnimator.SetFloat("CelebrateNum", 5f);
			break;
		case cSoccerEngine.celebTypes.HEART:
			player.currentVisual.playerAnimator.SetFloat("CelebrateNum", 6f);
			break;
		case cSoccerEngine.celebTypes.HANDS_BEHIND_HEAD:
			player.currentVisual.playerAnimator.SetFloat("CelebrateNum", 7f);
			break;
		case cSoccerEngine.celebTypes.SHUSH:
			player.currentVisual.playerAnimator.SetFloat("CelebrateNum", 8f);
			break;
		case cSoccerEngine.celebTypes.TELEPHONE:
			player.currentVisual.playerAnimator.SetFloat("CelebrateNum", 9f);
			break;
		case cSoccerEngine.celebTypes.SUCK_THUMB:
			player.currentVisual.playerAnimator.SetFloat("CelebrateNum", 10f);
			break;
		}
	}

	public void MakePlayerDoFlourish(cPlayer player, cSoccerEngine.flourishTypes flourishType)
	{
		player.celebrationRole = cPlayer.CelebrateRole.SCORER;
		player.hasAnimatedLookDirection = false;
		player.celebrationLayerWeight = 0f;
		player.currentVisual.playerAnimator.SetTrigger("DoFlourish");
		switch (flourishType)
		{
		case cSoccerEngine.flourishTypes.CHEERING:
			player.currentVisual.playerAnimator.SetFloat("FlourishNum", 1f);
			break;
		case cSoccerEngine.flourishTypes.CLAPPING:
			player.currentVisual.playerAnimator.SetFloat("FlourishNum", 2f);
			break;
		case cSoccerEngine.flourishTypes.FIST_PUMP:
			player.currentVisual.playerAnimator.SetFloat("FlourishNum", 3f);
			break;
		case cSoccerEngine.flourishTypes.FLEX:
			player.currentVisual.playerAnimator.SetFloat("FlourishNum", 4f);
			break;
		case cSoccerEngine.flourishTypes.NUMBER1:
			player.currentVisual.playerAnimator.SetFloat("FlourishNum", 5f);
			break;
		case cSoccerEngine.flourishTypes.KISS_FINGER:
			player.currentVisual.playerAnimator.SetFloat("FlourishNum", 6f);
			break;
		case cSoccerEngine.flourishTypes.MEXICAN_WAVE:
			player.currentVisual.playerAnimator.SetFloat("FlourishNum", 7f);
			break;
		}
		player.currentVisual.playerAnimator.SetBool("CelebrateRepeat", value: false);
		player.celebrationLayerWeight = 1f;
	}

	public void MakePlayerDoBackgroundFlourish(cPlayer player, cSoccerEngine.backgroundFlourishTypes flourishType)
	{
		player.celebrationRole = cPlayer.CelebrateRole.BACKGROUND;
		player.currentVisual.playerAnimator.SetBool("CelebrateBackground", value: true);
		switch (flourishType)
		{
		case cSoccerEngine.backgroundFlourishTypes.ARMS_UP:
			player.currentVisual.playerAnimator.SetFloat("FlourishNum", 1f);
			break;
		case cSoccerEngine.backgroundFlourishTypes.ARMS_OUT:
			player.currentVisual.playerAnimator.SetFloat("FlourishNum", 2f);
			break;
		case cSoccerEngine.backgroundFlourishTypes.PUMP:
			player.currentVisual.playerAnimator.SetFloat("FlourishNum", 3f);
			break;
		case cSoccerEngine.backgroundFlourishTypes.FIST_PUMP:
			player.currentVisual.playerAnimator.SetFloat("FlourishNum", 4f);
			break;
		}
		player.celebrationLayerWeight = 1f;
	}

	public void MakePlayerUpset(cPlayer player, bool repeat = false)
	{
		player.isUpset = true;
		RandomiseDisappointment(player.currentVisual.playerAnimator, repeat);
	}

	private void RandomiseDisappointment(Animator playerAnimator, bool repeat = false)
	{
		float value = (float)globalRand.Next(90, 110) / 100f;
		playerAnimator.SetFloat("CelebrateSpeed", value);
		if (repeat)
		{
			double num = globalRand.NextDouble();
			if (num < 0.8)
			{
				playerAnimator.SetBool("UpsetRepeat", value: true);
				playerAnimator.SetBool("MirrorAnim", num < 0.4);
			}
			playerAnimator.SetFloat("AnimOffset", (float)globalRand.NextDouble());
		}
		else
		{
			playerAnimator.SetTrigger("BeUpset");
			playerAnimator.SetFloat("AnimOffset", 0f);
		}
		float num2 = 0f;
		num2 = (repeat ? ((float)globalRand.Next(0, 5)) : ((float)globalRand.Next(0, 3)));
		playerAnimator.SetFloat("Disappointment", num2);
	}

	public void MakePlayerStopBeingUpset(cPlayer player)
	{
		player.isUpset = false;
		player.currentVisual.playerAnimator.SetBool("UpsetRepeat", value: false);
	}

	public void MakePlayerClap(cPlayer player, bool high = false)
	{
		player.isClapping = true;
		player.currentVisual.playerAnimator.SetBool("Clapping", value: true);
		if (high)
		{
			player.currentVisual.playerAnimator.SetFloat("ClapVal", 1f);
		}
		else
		{
			player.currentVisual.playerAnimator.SetFloat("ClapVal", 0f);
		}
	}

	public void MakePlayerStopClapping(cPlayer player)
	{
		player.isClapping = false;
		player.currentVisual.playerAnimator.SetBool("Clapping", value: false);
	}

	public void MakePlayerStandingHeader(cPlayer player)
	{
		player.currentVisual.playerAnimator.SetTrigger("StandHeader");
	}

	public void MakePlayerJumpingHeader(cPlayer player)
	{
		float value = (float)globalRand.Next(0, 100) / 100f;
		player.currentVisual.playerAnimator.SetFloat("HeaderRNG", value);
		player.currentVisual.playerAnimator.SetTrigger("Header");
	}

	public void MakePlayerVolley(cPlayer player)
	{
		player.currentVisual.playerAnimator.SetTrigger("Volley");
	}

	public void MakePlayerControlBall(cPlayer player)
	{
		if (!player.currentVisual.playerAnimator.GetCurrentAnimatorStateInfo(0).IsTag("Sliding"))
		{
			player.currentVisual.playerAnimator.SetTrigger("Control");
		}
	}

	public void MakePlayerThrowIn(cPlayer player)
	{
		player.currentVisual.playerAnimator.SetTrigger("ThrowIn");
	}

	public void MakePlayerDropKick(cPlayer player)
	{
		player.currentVisual.playerAnimator.SetTrigger("Dropkick");
	}

	public void MakePlayerLongThrow(cPlayer player)
	{
		player.currentVisual.playerAnimator.SetTrigger("LongThrow");
	}

	public void MakePlayerShortThrow(cPlayer player)
	{
		player.currentVisual.playerAnimator.SetTrigger("ShortThrow");
	}

	public void MakePlayerWallJump(cPlayer player)
	{
		player.currentVisual.playerAnimator.SetTrigger("Jump");
	}

	public void MakeKeeperGetUp(cPlayer player)
	{
		player.currentVisual.playerAnimator.SetBool("KeeperOnGround", value: false);
	}

	public void MakePlayerFallOver(cPlayer player)
	{
		player.currentVisual.playerAnimator.SetTrigger("FallOver");
	}

	public void SetPlayerFouledState(cPlayer player, bool fouled)
	{
		player.isFouled = fouled;
		player.currentVisual.playerAnimator.SetBool("Fouled", fouled);
	}

	public void SetPlayerInjured(cPlayer player)
	{
		player.isInjured = true;
		player.currentVisual.playerAnimator.SetBool("IsInjured", value: true);
	}

	public void SetPlayerReadyToAct(cPlayer player, bool ready)
	{
		player.isReadyToAct = ready;
		player.currentVisual.playerAnimator.SetBool("ReadyToAct", ready);
	}

	public void MakePlayerJumpToSave(cPlayer player)
	{
		player.currentVisual.playerAnimator.SetTrigger("OverBar");
	}

	public void MakePlayerDiveHigh(cPlayer player, bool diveLeft = false)
	{
		player.currentVisual.playerAnimator.SetTrigger("SaveHigh");
		player.currentVisual.playerAnimator.SetBool("MirrorAnim", diveLeft);
		player.isDiving = true;
		player.hitGroundAfterDive = false;
	}

	public void MakePlayerDiveMid(cPlayer player, bool diveLeft = false)
	{
		player.currentVisual.playerAnimator.SetTrigger("SaveMid");
		player.currentVisual.playerAnimator.SetBool("MirrorAnim", diveLeft);
		player.isDiving = true;
		player.hitGroundAfterDive = false;
	}

	public void MakePlayerDiveLow(cPlayer player, bool diveLeft = false)
	{
		player.currentVisual.playerAnimator.SetTrigger("SaveLow");
		player.currentVisual.playerAnimator.SetBool("MirrorAnim", diveLeft);
		player.isDiving = true;
		player.hitGroundAfterDive = false;
	}

	public void MakePlayerPrepare(cPlayer player, v3d pos, bool diveLeft = false)
	{
		player.currentVisual.playerAnimator.SetTrigger("DivePrepare");
		player.currentVisual.playerAnimator.SetBool("MirrorAnim", diveLeft);
	}

	public void MakePlayerDive(cPlayer player, v3d pos, bool diveLeft = false)
	{
		player.currentVisual.playerAnimator.SetFloat("BallSaveX", Mathf.Abs(pos.vy));
		player.currentVisual.playerAnimator.SetFloat("BallSaveY", pos.vz);
		player.currentVisual.playerAnimator.SetTrigger("StartDive");
		player.currentVisual.playerAnimator.SetBool("MirrorAnim", diveLeft);
	}

	public void MakePlayerStandingSave(cPlayer player, bool diveLeft = false)
	{
		player.currentVisual.playerAnimator.SetTrigger("StandingSave");
		player.currentVisual.playerAnimator.SetBool("MirrorAnim", diveLeft);
	}

	public void MakePlayerCatchHigh(cPlayer player)
	{
		player.currentVisual.playerAnimator.SetTrigger("CatchHigh");
	}

	public void MakePlayerCatchMid(cPlayer player)
	{
		player.currentVisual.playerAnimator.SetTrigger("CatchMid");
	}

	public void MakePlayerCatchLow(cPlayer player)
	{
		player.currentVisual.playerAnimator.SetTrigger("LowScoop");
	}

	private void UpdateCrowd()
	{
		UpdateCrowdTeam(StadiumCrowd.Team.HOME, "home", "away");
		UpdateCrowdTeam(StadiumCrowd.Team.AWAY, "away", "home");
	}

	private void UpdateCrowdTeam(StadiumCrowd.Team crowdTeam, string you, string opp)
	{
		switch ((crowdTeam == StadiumCrowd.Team.HOME) ? stadiumCrowd.HomeCurrentAction : stadiumCrowd.AwayCurrentAction)
		{
		case StadiumCrowd.Action.DISAPPOINTED:
			if (match.engineState != 2)
			{
				stadiumCrowd.SetCrowdState(crowdTeam, StadiumCrowd.Action.SITTING);
			}
			break;
		case StadiumCrowd.Action.SITTING:
			ballRelativePosition = sm.match.ball.currentPos.vx * (float)match.teams[you].goalDirection;
			ballDistanceToGoal = v3d.distSquared(sm.match.ball.currentPos, sm.places.goal[match.teams[opp].dirIndex]);
			if (ballDistanceToGoal < crowdStandRadius && !IsFreeBall() && sm.match.teamInPlay == you)
			{
				stadiumCrowd.SetCrowdState(crowdTeam, StadiumCrowd.Action.STANDING);
			}
			break;
		case StadiumCrowd.Action.STANDING:
			ballRelativePosition = sm.match.ball.currentPos.vx * (float)match.teams[you].goalDirection;
			ballDistanceToGoal = v3d.distSquared(sm.match.ball.currentPos, sm.places.goal[match.teams[opp].dirIndex]);
			if (ballDistanceToGoal > crowdSitRadius)
			{
				stadiumCrowd.SetCrowdState(crowdTeam, StadiumCrowd.Action.SITTING);
			}
			if (match.engineState == 2 && match.engineStateInfo.ContainsKey("players"))
			{
				if (((cPlayerID[])match.engineStateInfo["players"])[0].team == you)
				{
					stadiumCrowd.SetCrowdState(crowdTeam, StadiumCrowd.Action.CHEERING);
				}
				else
				{
					stadiumCrowd.SetCrowdState(crowdTeam, StadiumCrowd.Action.DISAPPOINTED);
				}
			}
			break;
		case StadiumCrowd.Action.CHEERING:
			if (match.engineState != 2)
			{
				stadiumCrowd.SetCrowdState(crowdTeam, StadiumCrowd.Action.SITTING);
			}
			break;
		}
	}

	private void UpdateEvents()
	{
		MatchCameraController matchCameraController = MatchCameraController.instance;
		eventTimeScale = 1f;
		if (debugReplayMatch)
		{
			return;
		}
		CheckOverviewForReenable();
		switch (match.currentEvent)
		{
		case MatchEvent.FAST_FORWARD:
			instance.StadiumController.ShowCameraStandRoof(showRoof: true);
			break;
		case MatchEvent.FREE_PLAY_TV:
		case MatchEvent.FREE_PLAY_PANORAMIC:
		case MatchEvent.FREE_PLAY_OVERVIEW:
		case MatchEvent.FREE_PLAY_STAND:
			instance.StadiumController.ShowCameraStandRoof(showRoof: false);
			break;
		}
		switch (match.currentEvent)
		{
		case MatchEvent.FAST_FORWARD:
			if (highlightMode == 5)
			{
				matchCameraController.SetCameraBehaviour(CameraTypes.QUICK_MATCH, CameraBehaviour.STATIC, null, snapTo: true);
			}
			else
			{
				matchCameraController.SetCameraBehaviour(CameraTypes.FAST_FORWARD, CameraBehaviour.STATIC, null, snapTo: true);
			}
			break;
		case MatchEvent.GOAL_CELEBRATION:
		case MatchEvent.GOAL_CELEBRATION_END:
		{
			if (match.currentEvent == match.lastEvent || match.lastEvent == MatchEvent.GOAL_CELEBRATION)
			{
				break;
			}
			Transform transform = null;
			if (match.nextEngineState == 607)
			{
				int key = (int)match.engineStateInfo["celebrator"];
				string key2 = (string)match.engineStateInfo["winners"];
				transform = match.teams[key2].playerCache[key].currentVisual.playerObject.transform;
			}
			else
			{
				cPlayerID[] array = (cPlayerID[])match.engineStateInfo["players"];
				transform = match.teams[array[0].team].playerCache[array[0].playerNum].currentVisual.playerObject.transform;
			}
			v3d runStartPos = null;
			if (match.nextEngineStateInfo.ContainsKey("runStartPoint"))
			{
				runStartPos = (v3d)match.nextEngineStateInfo["runStartPoint"];
			}
			Vector3 crowdLookTargetPosition = SetGoalCutsceneCamPosition(runStartPos, (v3d)match.nextEngineStateInfo["celebPoint"], transform);
			SetCrowdLookTargetPosition(crowdLookTargetPosition);
			if (match.nextEngineState == 607 || globalRand.NextDouble() <= 0.5)
			{
				matchCameraController.SetCameraBehaviour(CameraTypes.GOAL_CUTSCENE, CameraBehaviour.GOAL_CUTSCENE, transform);
				break;
			}
			switch (currentCameraMode)
			{
			default:
				matchCameraController.SetTVCameraBehaviour(CameraBehaviour.TV, tvTarget.transform, snapTo: true);
				break;
			case CameraMode.Panoramic:
				matchCameraController.SetCameraBehaviour(CameraTypes.PANORAMIC_CAM, CameraBehaviour.PANORAMIC, panoramicTarget.transform, snapTo: true);
				break;
			case CameraMode.Overview:
				matchCameraController.SetCameraBehaviour(CameraTypes.OVERVIEW, CameraBehaviour.STATIC, null, snapTo: true);
				break;
			case CameraMode.Stand:
				matchCameraController.SetCameraBehaviour(CameraTypes.STAND_CAM, CameraBehaviour.TV, standTarget.transform, snapTo: true);
				break;
			}
			break;
		}
		case MatchEvent.REPLAY_PART_1:
			SwitchToReplayCamera(replayPart1Camera);
			break;
		case MatchEvent.REPLAY_PART_2:
			SwitchToReplayCamera(replayPart2Camera);
			break;
		case MatchEvent.FAST_FORWARD_2D:
		case MatchEvent.NORMAL_2D:
			matchCameraController.SetCameraBehaviour(CameraTypes.MODE_2D, CameraBehaviour.STATIC, null, snapTo: true);
			break;
		case MatchEvent.FREE_PLAY_PANORAMIC:
			matchCameraController.SetCameraBehaviour(CameraTypes.PANORAMIC_CAM, CameraBehaviour.PANORAMIC, panoramicTarget.transform, snapTo: true);
			break;
		case MatchEvent.FREE_PLAY_OVERVIEW:
			matchCameraController.SetCameraBehaviour(CameraTypes.OVERVIEW, CameraBehaviour.STATIC, null, snapTo: true);
			break;
		case MatchEvent.FREE_PLAY_STAND:
			matchCameraController.SetCameraBehaviour(CameraTypes.STAND_CAM, CameraBehaviour.TV, standTarget.transform, snapTo: true);
			break;
		default:
			matchCameraController.SetTVCameraBehaviour(CameraBehaviour.TV, tvTarget.transform, snapTo: true);
			break;
		case MatchEvent.NO_CELEBRATION:
			break;
		}
	}

	private void SwitchToReplayCamera(eReplayCameraID replayCam)
	{
		MatchCameraController matchCameraController = MatchCameraController.instance;
		CameraTypes cameraTypes;
		Transform transform;
		Vector3 position;
		switch (replayCam)
		{
		default:
			return;
		case eReplayCameraID.REPLAY_FOLLOW_BALL:
			matchCameraController.GetCameraFromType(CameraTypes.REPLAY_FOLLOW_BALL).target2 = (replayGoalLocation ? positiveGoalDirReplayAimer.transform : negativeGoalDirReplayAimer.transform);
			matchCameraController.GetCameraFromType(CameraTypes.REPLAY_FOLLOW_BALL).transform.GetChild(0).GetComponent<Animator>().SetTrigger("Move");
			matchCameraController.SetCameraBehaviour(CameraTypes.REPLAY_FOLLOW_BALL, CameraBehaviour.REPLAY_FOLLOW_BALL, ballVisual, snapTo: true);
			return;
		case eReplayCameraID.REPLAY_CROWD:
			transform = matchCameraController.GetCameraFromType(CameraTypes.REPLAY_CROWD).transform;
			position = transform.position;
			position.x = Mathf.Abs(position.x) * (replayGoalLocation ? 1f : (-1f));
			transform.position = position;
			matchCameraController.SetCameraBehaviour(CameraTypes.REPLAY_CROWD, CameraBehaviour.REPLAY_FOLLOW_AND_ZOOM, ballVisual);
			return;
		case eReplayCameraID.REPLAY_CROWD_BEHIND:
			transform = matchCameraController.GetCameraFromType(CameraTypes.REPLAY_CROWD_BEHIND).transform;
			position = transform.position;
			position.x = Mathf.Abs(position.x) * (replayGoalLocation ? 1f : (-1f));
			transform.position = position;
			matchCameraController.SetCameraBehaviour(CameraTypes.REPLAY_CROWD_BEHIND, CameraBehaviour.REPLAY_FOLLOW_AND_ZOOM, ballVisual);
			return;
		case eReplayCameraID.REPLAY_BEHIND_GOAL:
			transform = matchCameraController.GetCameraFromType(CameraTypes.REPLAY_BEHIND_GOAL).transform;
			position = transform.position;
			position.x = Mathf.Abs(position.x) * (replayGoalLocation ? 1f : (-1f));
			transform.position = position;
			matchCameraController.SetCameraBehaviour(CameraTypes.REPLAY_BEHIND_GOAL, CameraBehaviour.REPLAY_FOLLOW_AND_ZOOM, ballVisual);
			return;
		case eReplayCameraID.REPLAY_OPPOSITE_END:
			transform = matchCameraController.GetCameraFromType(CameraTypes.REPLAY_OPPOSITE_END).transform;
			position = transform.position;
			position.x = Mathf.Abs(position.x) * (replayGoalLocation ? (-1f) : 1f);
			transform.position = position;
			matchCameraController.SetCameraBehaviour(CameraTypes.REPLAY_OPPOSITE_END, CameraBehaviour.REPLAY_FOLLOW_AND_ZOOM, ballVisual);
			return;
		case eReplayCameraID.REPLAY_GOAL_LINE:
			cameraTypes = CameraTypes.REPLAY_GOAL_LINE;
			break;
		case eReplayCameraID.REPLAY_ABOVE_GOAL:
			cameraTypes = CameraTypes.REPLAY_ABOVE_GOAL;
			break;
		}
		transform = matchCameraController.GetCameraFromType(cameraTypes).transform;
		position = transform.position;
		Vector3 localEulerAngles = transform.localEulerAngles;
		while (localEulerAngles.y > 180f)
		{
			localEulerAngles.y -= 360f;
		}
		position.x = Mathf.Abs(position.x) * (replayGoalLocation ? 1f : (-1f));
		localEulerAngles.y = Mathf.Abs(localEulerAngles.y) * (replayGoalLocation ? 1f : (-1f));
		transform.position = position;
		transform.localEulerAngles = localEulerAngles;
		matchCameraController.SetCameraBehaviour(cameraTypes, CameraBehaviour.STATIC, null);
	}

	public float CurrentTimeThisState()
	{
		return Mathf.Abs(match.time + match.timeTaken - time);
	}

	public IEnumerator PauseGame()
	{
		if (matchPausing || matchPaused || matchFinished || matchOutroStarted || CutScenesController.instance.IsPlaying)
		{
			yield break;
		}
		matchPausing = true;
		if (match.currentEvent != MatchEvent.FAST_FORWARD && match.currentEvent != MatchEvent.FAST_FORWARD_2D)
		{
			if (highlightMode == 5)
			{
				matchOverlay.GetComponent<Animator>().Play("MatchPauseQuick");
			}
			else
			{
				matchOverlay.GetComponent<Animator>().Play("MatchPause");
			}
			if (!InEditMode && !InFastMode)
			{
				yield return new WaitForSeconds(0.3f);
			}
		}
		else
		{
			yield return new WaitForEndOfFrame();
		}
		matchPaused = true;
		matchPausing = false;
		matchAudio.SoundPause();
		if (highlightMode != 5)
		{
			MatchEvent currentEvent = match.currentEvent;
			if ((uint)(currentEvent - 5) > 1u && currentView != MatchView.Tactics)
			{
				ShowStatsOverlay();
			}
		}
		topPanel.transform.Find("MatchControls/PlayImage/PlaySign").gameObject.SetActive(value: false);
		topPanel.transform.Find("MatchControls/PlayImage/PauseSign").gameObject.SetActive(value: true);
		if (quickMatchScreenExists)
		{
			quickMatchScreen.transform.Find("PlayPauseButton/PauseSymbol").gameObject.SetActive(value: false);
			quickMatchScreen.transform.Find("PlayPauseButton/PlayArrow").gameObject.SetActive(value: true);
		}
	}

	public void UnPauseGame()
	{
		if (matchPausing)
		{
			return;
		}
		matchPaused = false;
		if (matchAudio.SoundEnabled)
		{
			matchAudio.SoundUnPause();
		}
		if (highlightMode != 5)
		{
			MatchEvent currentEvent = match.currentEvent;
			if ((uint)(currentEvent - 5) > 1u)
			{
				HideStatsOverlay();
			}
		}
		topPanel.transform.Find("MatchControls/PlayImage/PlaySign").gameObject.SetActive(value: true);
		topPanel.transform.Find("MatchControls/PlayImage/PauseSign").gameObject.SetActive(value: false);
		if (quickMatchScreenExists)
		{
			quickMatchScreen.transform.Find("PlayPauseButton/PauseSymbol").gameObject.SetActive(value: true);
			quickMatchScreen.transform.Find("PlayPauseButton/PlayArrow").gameObject.SetActive(value: false);
		}
		if (match.currentEvent != MatchEvent.FAST_FORWARD && match.currentEvent != MatchEvent.FAST_FORWARD_2D)
		{
			if (highlightMode == 5)
			{
				matchOverlay.GetComponent<Animator>().Play("MatchUnPauseQuick");
			}
			else
			{
				matchOverlay.GetComponent<Animator>().Play("MatchUnPause");
			}
		}
	}

	public void ToggleSound()
	{
		matchAudio.SoundEnabled = !matchAudio.SoundEnabled;
		if (!matchAudio.SoundEnabled)
		{
			matchAudio.SoundPause();
			topPanel.transform.Find("MatchControls/SoundOn/Image").GetComponent<Image>().sprite = soundOffIcon;
		}
		else
		{
			matchAudio.SoundUnPause();
			topPanel.transform.Find("/MatchControls/SoundOn/Image").GetComponent<Image>().sprite = soundOnIcon;
		}
	}

	public void BackButton()
	{
		if (IsTacticsOpen())
		{
			CloseTactics();
		}
		else if (IsMatchPaused)
		{
			UnPauseGame();
		}
		else
		{
			StartCoroutine(PauseGame());
		}
	}

	public Transform GetOppositionGoalTransform(string homeAway, bool far = false)
	{
		int num = 1;
		if (homeAway == "away" && swappedEnds)
		{
			num = 2;
		}
		if (homeAway == "home" && !swappedEnds)
		{
			num = 2;
		}
		string text = (far ? "Far" : "");
		return GameObject.Find("PitchExtras/Goal" + num + text).transform;
	}

	public Transform GetClosestGoalToBall()
	{
		int num = ((ballPos.vx < 0f) ? 1 : 2);
		return GameObject.Find("PitchExtras/Goal" + num).transform;
	}

	public GameObject DrawDebugPoint(pitchDebugPoint pitchDebugPoint, bool transparent = false)
	{
		Vector3 position = pitchToScreen(pitchDebugPoint.point, useHeight: true);
		GameObject gameObject = UnityEngine.Object.Instantiate(Resources.Load<GameObject>(transparent ? "PitchPointTransparent" : "PitchPoint"));
		Material material = gameObject.transform.GetComponent<MeshRenderer>().material;
		if (transparent)
		{
			pitchDebugPoint.color.a = 0.3f;
		}
		material.color = pitchDebugPoint.color;
		gameObject.transform.GetComponent<MeshRenderer>().sharedMaterial = new Material(material);
		gameObject.transform.position = position;
		return gameObject;
	}

	public void DrawGoalDebugPoints()
	{
		List<pitchDebugPoint> list = new List<pitchDebugPoint>();
		v3d v3d2 = new v3d();
		list.Add(new pitchDebugPoint(sm.places.frontPost[0]));
		list.Add(new pitchDebugPoint(sm.places.frontPost[1]));
		list.Add(new pitchDebugPoint(sm.places.backPost[0]));
		list.Add(new pitchDebugPoint(sm.places.backPost[1]));
		list.Add(new pitchDebugPoint(sm.places.bottomCorner[0]));
		list.Add(new pitchDebugPoint(sm.places.bottomCorner[1]));
		list.Add(new pitchDebugPoint(sm.places.topCorner[0]));
		list.Add(new pitchDebugPoint(sm.places.topCorner[1]));
		v3d2 = new v3d(sm.places.frontPost[0].vx, sm.places.frontPost[0].vy, sm.matchSetup.dimensions.goal.vz);
		list.Add(new pitchDebugPoint(v3d2));
		v3d2 = new v3d(sm.places.frontPost[1].vx, sm.places.frontPost[1].vy, sm.matchSetup.dimensions.goal.vz);
		list.Add(new pitchDebugPoint(v3d2));
		v3d2 = new v3d(sm.places.backPost[0].vx, sm.places.backPost[0].vy, sm.matchSetup.dimensions.goal.vz);
		list.Add(new pitchDebugPoint(v3d2));
		v3d2 = new v3d(sm.places.backPost[1].vx, sm.places.backPost[1].vy, sm.matchSetup.dimensions.goal.vz);
		list.Add(new pitchDebugPoint(v3d2));
		list.Add(new pitchDebugPoint(new v3d()));
		list.Add(new pitchDebugPoint(sm.places.penaltyArea[1].topLeft));
		list.Add(new pitchDebugPoint(sm.places.penaltyArea[1].bottomRight));
		for (int i = -10; i <= 10; i++)
		{
			for (int j = 0; j <= 5; j++)
			{
				list.Add(new pitchDebugPoint(new v3d(i, 0f, j)));
			}
		}
		for (int k = 0; k < list.Count; k++)
		{
			DrawDebugPoint(list[k]);
		}
	}

	public void speedButton()
	{
		speedLevel++;
		if (speedLevel >= matchSpeeds.Length)
		{
			speedLevel = 0;
		}
		topPanel.transform.Find("MatchControls/Speed/Text").GetComponent<Text>().text = "X" + matchSpeeds[speedLevel];
		PlayerPrefs.SetInt("MatchSpeed", speedLevel);
	}

	public void highlightButton(int mode)
	{
		highlightMode = mode;
		_ = new string[4] { "FULL MATCH", "SHORT", "EXTENDED", "COMMENTARY" };
		if (matchPaused)
		{
			UnPauseGame();
		}
		topPanel.transform.Find("MatchControls/Play").GetComponent<Image>().color = new Color32(86, 86, 86, byte.MaxValue);
		topPanel.transform.Find("MatchControls/All").GetComponent<Image>().color = new Color32(86, 86, 86, byte.MaxValue);
		topPanel.transform.Find("MatchControls/Short").GetComponent<Image>().color = new Color32(86, 86, 86, byte.MaxValue);
		topPanel.transform.Find("MatchControls/Extended").GetComponent<Image>().color = new Color32(86, 86, 86, byte.MaxValue);
		if (highlightMode == 0)
		{
			topPanel.transform.Find("MatchControls/All").GetComponent<Image>().color = new Color32(108, 222, 171, byte.MaxValue);
		}
		else if (highlightMode == 2)
		{
			topPanel.transform.Find("MatchControls/Extended").GetComponent<Image>().color = new Color32(108, 222, 171, byte.MaxValue);
		}
		else if (highlightMode == 1)
		{
			topPanel.transform.Find("MatchControls/Short").GetComponent<Image>().color = new Color32(108, 222, 171, byte.MaxValue);
		}
		PlayerPrefs.SetInt("HighlightMode_1.0.5", highlightMode);
	}

	public void SetupMatchControls()
	{
		Transform transform = matchControls.transform.Find("Panel/Contents");
		matchControls.transform.Find("Panel/Image/Title").GetComponent<Text>().text = Translation.GetText("match-controls").ToUpper();
		matchControls.transform.Find("Panel/Image/CloseBtn").GetComponent<Button>().onClick.AddListener(() =>
		{
			SetMatchControlsVisible(visible: false);
		});
		transform.Find("MatchView/Title").GetComponent<Text>().text = Translation.GetText("match-view");
		transform.Find("MatchView/Buttons/3D/Text").GetComponent<Text>().text = Translation.GetText("3d");
		transform.Find("MatchView/Buttons/2D/Text").GetComponent<Text>().text = Translation.GetText("2d");
		Toggle component = transform.Find("MatchView/Buttons/3D").GetComponent<Toggle>();
		component.isOn = currentMatchView == MatchViewType.View_3D;
		component.onValueChanged.AddListener((bool value) =>
		{
			if (value)
			{
				EventLoggingController.LogEvent("matchMatchView", "viewType", "view_3D");
				currentMatchView = MatchViewType.View_3D;
				PlayerPrefs.SetInt("MatchViewSetting", 1);
			}
		});
		Toggle component2 = transform.Find("MatchView/Buttons/2D").GetComponent<Toggle>();
		component2.isOn = currentMatchView == MatchViewType.View_2D;
		component2.onValueChanged.AddListener((bool value) =>
		{
			if (value)
			{
				EventLoggingController.LogEvent("matchMatchView", "viewType", "view_2D");
				currentMatchView = MatchViewType.View_2D;
				PlayerPrefs.SetInt("MatchViewSetting", 2);
			}
		});
		transform.Find("GameSpeed/Title").GetComponent<Text>().text = Translation.GetText("speed");
		if (!InFastMode)
		{
			transform.Find("GameSpeed/Buttons").GetChild(speedLevel).GetComponent<Toggle>()
				.isOn = true;
		}
		for (int num = 0; num < 5; num++)
		{
			Toggle component3 = transform.Find("GameSpeed/Buttons").GetChild(num).GetComponent<Toggle>();
			int localValue = num;
			component3.onValueChanged.AddListener((bool value) =>
			{
				if (value)
				{
					speedLevel = Mathf.RoundToInt(localValue);
					PlayerPrefs.SetInt("MatchSpeed", speedLevel);
					EventLoggingController.LogEvent("matchSpeed", "speedLevel", "speed_" + matchSpeeds[speedLevel] + "x");
				}
			});
		}
		transform.Find("HighlightView/Title").GetComponent<Text>().text = Translation.GetText("highlight-view");
		transform.Find("HighlightView/Buttons/3D/Text").GetComponent<Text>().text = Translation.GetText("3d");
		transform.Find("HighlightView/Buttons/2D/Text").GetComponent<Text>().text = Translation.GetText("2d");
		Toggle component4 = transform.Find("HighlightView/Buttons/3D").GetComponent<Toggle>();
		component4.isOn = currentHighlightView == MatchViewType.View_3D;
		component4.onValueChanged.AddListener((bool value) =>
		{
			if (value)
			{
				EventLoggingController.LogEvent("matchHighlightView", "viewType", "view_3D");
				currentHighlightView = MatchViewType.View_3D;
				PlayerPrefs.SetInt("HighlightViewSetting", 1);
			}
		});
		Toggle component5 = transform.Find("HighlightView/Buttons/2D").GetComponent<Toggle>();
		component5.isOn = currentHighlightView == MatchViewType.View_2D;
		component5.onValueChanged.AddListener((bool value) =>
		{
			if (value)
			{
				EventLoggingController.LogEvent("matchHighlightView", "viewType", "view_2D");
				currentHighlightView = MatchViewType.View_2D;
				PlayerPrefs.SetInt("HighlightViewSetting", 2);
			}
		});
		transform.Find("Highlights/Title").GetComponent<Text>().text = Translation.GetText("highlights");
		transform.Find("Highlights/Buttons/All/Text").GetComponent<Text>().text = Translation.GetText("all").ToUpper();
		transform.Find("Highlights/Buttons/Extended/Text").GetComponent<Text>().text = Translation.GetText("extended").ToUpper();
		transform.Find("Highlights/Buttons/Key/Text").GetComponent<Text>().text = Translation.GetText("key-highlight").ToUpper();
		Toggle component6 = transform.Find("Highlights/Buttons/All").GetComponent<Toggle>();
		component6.isOn = highlightMode == 0;
		component6.onValueChanged.AddListener((bool value) =>
		{
			if (value)
			{
				EventLoggingController.LogEvent("matchHighlightType", "highlightType", "highlight_All");
				highlightMode = 0;
				PlayerPrefs.SetInt("HighlightMode_1.0.5", highlightMode);
			}
		});
		Toggle component7 = transform.Find("Highlights/Buttons/Extended").GetComponent<Toggle>();
		component7.isOn = highlightMode == 2;
		component7.onValueChanged.AddListener((bool value) =>
		{
			if (value)
			{
				EventLoggingController.LogEvent("matchHighlightType", "highlightType", "highlight_Extended");
				highlightMode = 2;
				PlayerPrefs.SetInt("HighlightMode_1.0.5", highlightMode);
			}
		});
		Toggle component8 = transform.Find("Highlights/Buttons/Key").GetComponent<Toggle>();
		component8.isOn = highlightMode == 1;
		component8.onValueChanged.AddListener((bool value) =>
		{
			if (value)
			{
				EventLoggingController.LogEvent("matchHighlightType", "highlightType", "highlight_Key");
				highlightMode = 1;
				PlayerPrefs.SetInt("HighlightMode_1.0.5", highlightMode);
			}
		});
		transform.Find("Replays/Title").GetComponent<Text>().text = Translation.GetText("replay");
		transform.Find("Replays/Buttons/On/Text").GetComponent<Text>().text = Translation.GetText("on").ToUpper();
		transform.Find("Replays/Buttons/Off/Text").GetComponent<Text>().text = Translation.GetText("off").ToUpper();
		Toggle component9 = transform.Find("Replays/Buttons/On").GetComponent<Toggle>();
		component9.isOn = replaysEnabled == eReplayType.Enabled;
		component9.onValueChanged.AddListener((bool value) =>
		{
			if (value)
			{
				replaysEnabled = eReplayType.Enabled;
				PlayerPrefs.SetInt("ReplayEnabledSetting", 1);
			}
		});
		Toggle component10 = transform.Find("Replays/Buttons/Off").GetComponent<Toggle>();
		component10.isOn = replaysEnabled == eReplayType.Disabled;
		component10.onValueChanged.AddListener((bool value) =>
		{
			if (value)
			{
				replaysEnabled = eReplayType.Disabled;
				PlayerPrefs.SetInt("ReplayEnabledSetting", 2);
			}
		});
		transform.Find("CameraMode/Title").GetComponent<Text>().text = Translation.GetText("camera-mode");
		transform.Find("CameraMode/Buttons/TV/Text").GetComponent<Text>().text = Translation.GetText("tv").ToUpper();
		transform.Find("CameraMode/Buttons/Panoramic/Text").GetComponent<Text>().text = Translation.GetText("panoramic").ToUpper();
		transform.Find("CameraMode/Buttons/Overview/Text").GetComponent<Text>().text = Translation.GetText("overview").ToUpper();
		Toggle component11 = transform.Find("CameraMode/Buttons/TV").GetComponent<Toggle>();
		component11.isOn = currentCameraMode == CameraMode.TV;
		component11.onValueChanged.AddListener((bool value) =>
		{
			if (value)
			{
				currentCameraMode = CameraMode.TV;
				PlayerPrefs.SetInt("MatchCameraModeSetting", 0);
			}
		});
		Toggle component12 = transform.Find("CameraMode/Buttons/Panoramic").GetComponent<Toggle>();
		component12.isOn = currentCameraMode == CameraMode.Panoramic;
		component12.onValueChanged.AddListener((bool value) =>
		{
			if (value)
			{
				currentCameraMode = CameraMode.Panoramic;
				PlayerPrefs.SetInt("MatchCameraModeSetting", 1);
			}
		});
		Toggle component13 = transform.Find("CameraMode/Buttons/Overview").GetComponent<Toggle>();
		component13.isOn = currentCameraMode == CameraMode.Overview;
		component13.onValueChanged.AddListener((bool value) =>
		{
			if (value)
			{
				currentCameraMode = CameraMode.Overview;
				PlayerPrefs.SetInt("MatchCameraModeSetting", 2);
			}
		});
		Toggle component14 = transform.Find("CameraMode/Buttons/Stand").GetComponent<Toggle>();
		component14.isOn = currentCameraMode == CameraMode.Stand;
		component14.onValueChanged.AddListener((bool value) =>
		{
			if (value)
			{
				currentCameraMode = CameraMode.Stand;
				PlayerPrefs.SetInt("MatchCameraModeSetting", 3);
			}
		});
	}

	public void SetMatchControlsVisible(bool visible)
	{
		matchControlsShown = visible;
		CanvasGroup component = matchControls.transform.Find("Panel").GetComponent<CanvasGroup>();
		component.alpha = (matchControlsShown ? 1f : 0f);
		component.blocksRaycasts = matchControlsShown;
		component.interactable = matchControlsShown;
		matchControls.transform.Find("Panel/ControlsBackground").GetComponent<BoxCollider2D>().enabled = matchControlsShown;
		topPanel.transform.Find("MatchControls/Controls").GetComponent<Image>().color = (matchControlsShown ? new Color32(36, 39, 49, 0) : new Color32(36, 39, 49, 0));
	}

	private void CheckForcedTactics()
	{
		if (sm.match.isTacticsChangeSuggested() != "")
		{
			if (manyMatches.Running || instance.debugSpeedThrough)
			{
				StartCoroutine(AutoCloseTactics());
			}
			string tacticsChangeSuggested = sm.match.tacticsChangeSuggested;
			sm.match.clearSuggestedTacticsChange();
			if (tacticsChangeSuggested == "injury")
			{
				injuredPlayer = true;
				int num = sm.matchStats.teams[homeAway].injIDs.Last();
				TacticsController.GetInstance().SetPlayerInjured(sm.match.teams[homeAway].teamInfo.players[num].id);
				sm.match.forceSub = true;
			}
			if (tacticsChangeSuggested == "sendingOff")
			{
				playerSentOff = true;
				int num2 = sm.matchStats.teams[homeAway].rcIDs.Last();
				_ = sm.match.teams[homeAway].squad.getYCs(num2 - 1).Count;
				TacticsController.GetInstance().SetPlayerToSentOff(sm.match.teams[homeAway].teamInfo.players[num2].id);
				sm.match.forceSub = true;
			}
			if (tacticsChangeSuggested == "halfTime")
			{
				halfTimeTacticsShown = true;
				sm.match.forceSub = true;
			}
			if (tacticsChangeSuggested == "extraTime")
			{
				extraTimeTacticsShown = true;
				sm.match.forceSub = true;
			}
			if (tacticsChangeSuggested == "extraHalfTime")
			{
				extraHalfTimeTacticsShown = true;
				sm.match.forceSub = true;
			}
			ChangeView(MatchView.Tactics);
		}
	}

	private void RemoveShadow(int playerID)
	{
		short clubID = Player.GetClubID(playerID);
		pitchPrefab = GameObject.Find("PitchHolder").gameObject;
		pitchPrefab.transform.Find(Club.GetName(clubID) + " " + Player.GetSurname(playerID)).gameObject.transform.Find("NightShadow").gameObject.SetActive(value: false);
	}

	private void ConfirmTacticChanges(int homeClubID)
	{
		if ((halfTimeTacticsShown || extraTimeTacticsShown || extraHalfTimeTacticsShown) && highlightMode != 5)
		{
			CutScenesController.instance.Stop();
			ShowBall();
		}
		if (halfTimeTacticsShown)
		{
			halfTimeTacticsShown = false;
			GameObject gameObject = topPanel;
			gameObject.transform.Find("ScorePanel/Time").gameObject.SetActive(value: true);
			gameObject.transform.Find("ScorePanel/HalfTimeString").gameObject.SetActive(value: false);
			if (seFixInfo.fixtureInfo.isLeg2)
			{
				gameObject.transform.Find("ScorePanel/agg").gameObject.SetActive(value: true);
			}
		}
		if (extraTimeTacticsShown)
		{
			extraTimeTacticsShown = false;
			GameObject gameObject2 = topPanel;
			gameObject2.transform.Find("ScorePanel/Time").gameObject.SetActive(value: true);
			gameObject2.transform.Find("ScorePanel/ExtraTimeString").gameObject.SetActive(value: false);
		}
		if (extraHalfTimeTacticsShown)
		{
			extraHalfTimeTacticsShown = false;
			GameObject gameObject3 = topPanel;
			gameObject3.transform.Find("ScorePanel/Time").gameObject.SetActive(value: true);
			gameObject3.transform.Find("ScorePanel/ExtraHalfTimeString").gameObject.SetActive(value: false);
		}
		bool flag = Enumerable.SequenceEqual(TacticsController.GetInstance().MatchTactics.lineUp, TacticsController.GetInstance().ModifiedMatchTactics.lineUp);
		bool flag2 = TacticsController.GetInstance().MatchTactics.inst.equal(TacticsController.GetInstance().ModifiedMatchTactics.inst);
		if (!flag && !madeSub)
		{
			int[] lineUp = TacticsController.GetInstance().ModifiedMatchTactics.lineUp;
			for (int i = 0; i < 11; i++)
			{
				if (lineUp[i] >= 11)
				{
					madeSub = true;
					EventLoggingController.LogEvent("matchSubMade");
					break;
				}
			}
		}
		if (!flag || !flag2)
		{
			cTactics modifiedMatchTactics = TacticsController.GetInstance().ModifiedMatchTactics;
			sm.match.setTeamTactics(modifiedMatchTactics, homeAway);
			TacticsController.TacticsPending = true;
		}
		if (sm.match.teams[homeAway].squad.usedAllSubs())
		{
			injuredPlayer = false;
		}
		injuredPlayer = false;
		playerSentOff = false;
		sm.match.clearSuggestedTacticsChange();
		matchRunning = true;
		ChangeView(lastView, manuallyChanged: true);
		UnPauseGame();
		sm.match.rollback = true;
		tacticsOpen = false;
		if (currentSkip == SkipReason.HALFTIME)
		{
			SkipButtonPressed();
		}
	}

	private void ResetTacticBtnOnclick()
	{
		sm.match.clearPendingTactics(homeAway);
		TacticsController.GetInstance().CancelTactics();
	}

	private void CancelTacticsChanges()
	{
		if ((halfTimeTacticsShown || extraTimeTacticsShown || extraHalfTimeTacticsShown) && highlightMode != 5)
		{
			CutScenesController.instance.Stop();
			ShowBall();
		}
		if (halfTimeTacticsShown)
		{
			halfTimeTacticsShown = false;
			GameObject gameObject = topPanel;
			gameObject.transform.Find("ScorePanel/Time").gameObject.SetActive(value: true);
			gameObject.transform.Find("ScorePanel/HalfTimeString").gameObject.SetActive(value: false);
		}
		if (extraTimeTacticsShown)
		{
			extraTimeTacticsShown = false;
			GameObject gameObject2 = topPanel;
			gameObject2.transform.Find("ScorePanel/Time").gameObject.SetActive(value: true);
			gameObject2.transform.Find("ScorePanel/ExtraTimeString").gameObject.SetActive(value: false);
		}
		if (extraHalfTimeTacticsShown)
		{
			extraHalfTimeTacticsShown = false;
			GameObject gameObject3 = topPanel;
			gameObject3.transform.Find("ScorePanel/Time").gameObject.SetActive(value: true);
			gameObject3.transform.Find("ScorePanel/ExtraHalfTimeString").gameObject.SetActive(value: false);
		}
		sm.match.clearPendingTactics(homeAway);
		TacticsController.GetInstance().CancelTactics();
		ChangeView(lastView, manuallyChanged: true);
		UnPauseGame();
		matchRunning = true;
		tacticsOpen = false;
		if (currentSkip == SkipReason.HALFTIME)
		{
			SkipButtonPressed();
		}
	}

	public void CloseTactics()
	{
		CancelTacticsChanges();
	}

	public bool IsTacticsOpen()
	{
		return tacticsOpen;
	}

	private void SetUpSideBar()
	{
		GameObject gameObject = GameObject.Find("OverlayCanvasMatch");
		_ = gameObject.transform.Find("MatchOverlay/SideBarMatch/Menu/Panel").gameObject;
		topPanel.transform.Find("MatchControls/Tactics").GetComponent<Button>().onClick.AddListener(() =>
		{
			EventLoggingController.LogEvent("matchTacticsOpened");
			ChangeView(MatchView.Tactics);
		});
		topPanel.transform.Find("Debug").gameObject.SetActive(value: false);
		topPanel.transform.Find("Debug2").gameObject.SetActive(value: false);
		topPanel.transform.Find("MatchControls/Tactics/Text").GetComponent<Text>().text = Translation.GetText("tactics").ToUpper();
		gameObject.transform.Find("MatchOverlay/ReplayOverlay/ReplayLabel/Text").GetComponent<Text>().text = Translation.GetText("replay");
	}

	private void ChangeOverView(StatsView statsView)
	{
		if (Constants.LIVE_SCORES_ENABLED)
		{
			GameObject gameObject = GameObject.Find("OverlayCanvasMatch").transform.Find("OverviewMode").gameObject;
			GameObject gameObject2 = gameObject.transform.Find("statsPanel/Stats").gameObject;
			GameObject gameObject3 = gameObject.transform.Find("statsPanel/Table").gameObject;
			GameObject gameObject4 = gameObject.transform.Find("statsPanel/LatestScores").gameObject;
			GameObject gameObject5 = gameObject.transform.Find("OverviewPanel/Right/Panel/AllGames").gameObject;
			Transform transform = gameObject.transform.Find("statsPanel/Panel");
			TabsBlue component = transform.GetComponent<TabsBlue>();
			switch (statsView)
			{
			case StatsView.Statistics:
				gameObject2.SetActive(value: true);
				gameObject3.SetActive(value: false);
				gameObject4.SetActive(value: false);
				gameObject5.SetActive(value: false);
				component.TabSelected(transform.Find("StatsBtn").gameObject);
				break;
			case StatsView.LiveScores:
				gameObject4.SetActive(value: true);
				gameObject2.SetActive(value: false);
				gameObject3.SetActive(value: false);
				gameObject5.SetActive(value: false);
				component.TabSelected(transform.Find("LiveScoresBtn").gameObject);
				break;
			case StatsView.LiveTable:
				gameObject3.SetActive(value: true);
				gameObject2.SetActive(value: false);
				gameObject4.SetActive(value: false);
				gameObject5.SetActive(value: false);
				component.TabSelected(transform.Find("LiveTableBtn").gameObject);
				break;
			case StatsView.OtherGames:
				gameObject3.SetActive(value: false);
				gameObject2.SetActive(value: false);
				gameObject4.SetActive(value: false);
				gameObject5.SetActive(value: true);
				component.TabSelected(transform.Find("AllGamesBtn").gameObject);
				break;
			default:
				gameObject2.SetActive(value: true);
				gameObject3.SetActive(value: false);
				gameObject4.SetActive(value: false);
				gameObject5.SetActive(value: false);
				component.TabSelected(transform.Find("StatsBtn").gameObject);
				break;
			}
		}
	}

	private void ChangeView(MatchView matchView, bool manuallyChanged = false)
	{
		Transform transform = matchOverlay.transform.Find("SideBarMatch/Menu/Panel");
		TabsBlue component = transform.GetComponent<TabsBlue>();
		playerStatsScreen.SetActive(value: false);
		tacticsGameObject.SetActive(value: false);
		tacticsControls.SetActive(value: false);
		matchControlButtons.SetActive(value: false);
		if (highlightMode == 5)
		{
			if (quickMatchShowingHighlight)
			{
				matchOverlay.GetComponent<CanvasGroup>().alpha = 1f;
				if (quickMatchScreenExists)
				{
					quickMatchScreen.SetActive(value: false);
				}
				matchViewControls.SetActive(value: false);
			}
			else
			{
				matchOverlay.GetComponent<CanvasGroup>().alpha = 0f;
				if (quickMatchScreenExists)
				{
					quickMatchScreen.SetActive(value: true);
				}
				matchViewControls.SetActive(value: true);
			}
		}
		switch (matchView)
		{
		case MatchView.Pitch:
		case MatchView.Overview:
			if (manuallyChanged)
			{
				currentView = matchView;
				PlayerPrefs.SetInt("MatchView", (int)currentView);
				component.TabSelected(transform.Find("2DPitch").gameObject);
			}
			lastView = matchView;
			if (highlightMode != 5)
			{
				matchControlButtons.SetActive(value: true);
			}
			break;
		case MatchView.Tactics:
			if (tacticsAllowed)
			{
				if (highlightMode == 5)
				{
					matchOverlay.GetComponent<CanvasGroup>().alpha = 1f;
				}
				tacticsControls.SetActive(value: true);
				TacticsController.GetInstance().UpdateAllPlayersMatchStats();
				string key = ((homeAway == "home") ? "away" : "home");
				TacticsOpposition.Instance.OppTactics = sm.match.getTeamTactics(key, ignorePending: true);
				TacticsOpposition.Instance.OppTeamStats = sm.match.matchStats.teams[key];
				TacticsOpposition.Instance.fixtureInfo = sm.match.fixtureInfo;
				TacticsOpposition.Instance.TacticsControls = tacticsControls;
				TacticsMatchStats.StatsPanel = GameObject.Find("OverlayCanvasMatch").transform.Find("OverviewMode/OverviewPanel").gameObject;
				HideStatsOverlay();
				tacticsGameObject.SetActive(value: true);
				StartCoroutine(PauseGame());
				GameObject gameObject = topPanel;
				if ((halfTimeTacticsShown || extraTimeTacticsShown || extraHalfTimeTacticsShown) && gameObject.transform.Find("ScorePanel/Time").gameObject.activeSelf)
				{
					gameObject.transform.Find("ScorePanel/Time").gameObject.SetActive(value: false);
				}
				if (halfTimeTacticsShown)
				{
					gameObject.transform.Find("ScorePanel/HalfTimeString").gameObject.SetActive(value: true);
					if (seFixInfo.fixtureInfo.isLeg2)
					{
						gameObject.transform.Find("ScorePanel/agg").gameObject.SetActive(value: false);
					}
				}
				if (extraTimeTacticsShown)
				{
					gameObject.transform.Find("ScorePanel/ExtraTimeString").gameObject.SetActive(value: true);
				}
				if (extraHalfTimeTacticsShown)
				{
					gameObject.transform.Find("ScorePanel/ExtraHalfTimeString").gameObject.SetActive(value: true);
				}
				_ = sm.match.getTeamTactics(homeAway, ignorePending: false).pending;
				tacticsOpen = true;
				currentView = matchView;
			}
			else
			{
				matchControlButtons.SetActive(value: true);
			}
			break;
		}
		CheckOverviewForReenable();
	}

	private void ShowStatsOverlay()
	{
		if (!statsActive)
		{
			overviewScreen.SetActive(value: true);
			statsActive = true;
		}
	}

	private void HideStatsOverlay()
	{
		if (statsActive)
		{
			overviewScreen.SetActive(value: false);
			statsActive = false;
		}
	}

	private void CheckOverviewForReenable()
	{
		if (highlightMode == 5)
		{
			return;
		}
		if ((match.currentEvent == MatchEvent.FAST_FORWARD || match.currentEvent == MatchEvent.FAST_FORWARD_2D) && currentView != MatchView.Tactics)
		{
			ShowStatsOverlay();
			matchOverlay.GetComponent<Animator>().SetBool("FastForward", value: true);
			return;
		}
		if (IsMatchPaused && currentView != MatchView.Tactics)
		{
			ShowStatsOverlay();
		}
		else
		{
			HideStatsOverlay();
		}
		matchOverlay.GetComponent<Animator>().SetBool("FastForward", value: false);
	}

	private void InitMatchOverlay(SeFixtureInfo fixInfo)
	{
		short homeClubID = fixInfo.fixtureInfo.home.clubDataID;
		short clubDataID = fixInfo.fixtureInfo.away.clubDataID;
		GameObject gameObject = topPanel;
		gameObject.transform.Find("HomeTeam/HomeName").GetComponent<Text>().text = Club.GetShortName(homeClubID).ToUpper();
		gameObject.transform.Find("AwayTeam/AwayName").GetComponent<Text>().text = Club.GetShortName(clubDataID).ToUpper();
		gameObject.transform.Find("MatchControls/Controls").GetComponent<Button>().onClick.AddListener(() =>
		{
			if (matchControlsShown)
			{
				UnPauseGame();
				SetMatchControlsVisible(visible: false);
			}
			else
			{
				StartCoroutine(PauseGame());
				SetMatchControlsVisible(visible: true);
				EventLoggingController.LogEvent("matchSettings");
			}
		});
		Transform transform = GameObject.Find("OverlayCanvasMatch/MatchOverlay/IntroLogos").transform;
		if (!(Game.instance == null))
		{
			gameObject.transform.Find("HomeTeam/HomeBadge/Colour1").GetComponent<Image>().color = fixtureKitHelper.homeColour1;
			gameObject.transform.Find("HomeTeam/HomeBadge/Colour2").GetComponent<Image>().color = fixtureKitHelper.homeColour2;
			gameObject.transform.Find("AwayTeam/AwayBadge/Colour1").GetComponent<Image>().color = fixtureKitHelper.awayColour1;
			gameObject.transform.Find("AwayTeam/AwayBadge/Colour2").GetComponent<Image>().color = fixtureKitHelper.awayColour2;
			ImageController.GetInstance().LoadImage(homeClubID, transform.Find("HomeIntro/HomeIntroBadge").GetComponent<Image>(), ImageType.Club);
			ImageController.GetInstance().LoadImage(clubDataID, transform.Find("AwayIntro/AwayIntroBadge").GetComponent<Image>(), ImageType.Club);
			ImageController.GetInstance().LoadImage(homeClubID, overviewScreen.transform.Find("OverviewPanel/left/Panel/ScorePanel/HomeTeamImage").GetComponent<Image>(), ImageType.Club);
			ImageController.GetInstance().LoadImage(clubDataID, overviewScreen.transform.Find("OverviewPanel/left/Panel/ScorePanel/AwayTeamImage").GetComponent<Image>(), ImageType.Club);
			transform.Find("HomeIntro/ClubNameBackgroundHome/HomeIntroName").GetComponent<Text>().text = Club.GetName(homeClubID);
			transform.Find("AwayIntro/ClubNameBackgroundAway/AwayIntroName").GetComponent<Text>().text = Club.GetName(clubDataID);
			if (quickMatchScreenExists)
			{
				ImageController.GetInstance().LoadImage(homeClubID, quickMatchScreen.transform.Find("IntroLogos/HomeIntroBadge").GetComponent<Image>(), ImageType.Club);
				ImageController.GetInstance().LoadImage(clubDataID, quickMatchScreen.transform.Find("IntroLogos/AwayIntroBadge").GetComponent<Image>(), ImageType.Club);
				quickMatchScreen.transform.Find("IntroLogos/HomeIntroBadge/HomeIntroName").GetComponent<Text>().text = Club.GetName(homeClubID);
				quickMatchScreen.transform.Find("IntroLogos/AwayIntroBadge/AwayIntroName").GetComponent<Text>().text = Club.GetName(clubDataID);
			}
			tacticsControls.transform.Find("ConfirmBtn").GetComponent<Button>().onClick.AddListener(() =>
			{
				ConfirmTacticChanges(homeClubID);
			});
			tacticsControls.transform.Find("CancelBtn").GetComponent<Button>().onClick.AddListener(() =>
			{
				CancelTacticsChanges();
			});
			tacticsControls.transform.Find("ResetBtn").GetComponent<Button>().onClick.AddListener(() =>
			{
				ResetTacticBtnOnclick();
			});
			GameObject gameObject2 = GameObject.Find("Last5Possesion");
			gameObject2.transform.Find("Home").GetComponent<Image>().color = fixtureKitHelper.homeColour1;
			gameObject2.transform.Find("Away").GetComponent<Image>().color = fixtureKitHelper.awayColour1;
			gameObject2.transform.Find("Text").GetComponent<Text>().text = Translation.GetText("last-5-poss");
			replayUIOverlay = GameObject.Find("OverlayCanvasMatch/MatchOverlay/ReplayOverlay");
			replayUIOverlay.SetActive(value: false);
			if (fixInfo.fixtureInfo.isLeg2)
			{
				gameObject.transform.Find("ScorePanel/agg").gameObject.SetActive(value: true);
				gameObject.transform.Find("ScorePanel/agg/aggregatescore").GetComponent<Text>().text = "( " + (fixInfo.fixtureInfo.leg1AwayScore + sm.match.matchStats.teams["home"].score) + " - " + (fixInfo.fixtureInfo.leg1HomeScore + sm.match.matchStats.teams["away"].score) + " )";
			}
			else
			{
				gameObject.transform.Find("ScorePanel/agg").gameObject.SetActive(value: false);
			}
		}
	}

	private void InitMatchStatsView(SeFixtureInfo fixInfo)
	{
		GameObject gameObject = GameObject.Find("OverlayCanvasMatch");
		GameObject gameObject2 = gameObject.transform.Find("OverviewMode").gameObject;
		GameObject gameObject3 = gameObject2.transform.Find("statsPanel").gameObject;
		GameObject gameObject4 = gameObject2.transform.Find("statsPanel/Panel").gameObject;
		matchStatsPanel = gameObject.transform.Find("OverviewMode/OverviewPanel/left/Panel/MatchStats/Panel").gameObject;
		GameObject gameObject5 = gameObject4.transform.Find("StatsBtn").gameObject;
		GameObject gameObject6 = gameObject4.transform.Find("LiveTableBtn").gameObject;
		GameObject gameObject7 = gameObject4.transform.Find("LiveScoresBtn").gameObject;
		GameObject gameObject8 = gameObject4.transform.Find("AllGamesBtn").gameObject;
		liveScores = gameObject3.transform.Find("LatestScores").gameObject;
		liveTable = gameObject3.transform.Find("Table").gameObject;
		allGames = gameObject3.transform.Find("AllGames").gameObject;
		gameObject2.transform.Find("statsPanel/Panel/StatsBtn/Text").GetComponent<Text>().text = Translation.GetText("match-statistics");
		gameObject2.transform.Find("statsPanel/Panel/LiveTableBtn/Text").GetComponent<Text>().text = Translation.GetText("league-table-title");
		gameObject2.transform.Find("statsPanel/Panel/LiveScoresBtn/Text").GetComponent<Text>().text = Translation.GetText("string-live-scores");
		gameObject8.transform.Find("Text").GetComponent<Text>().text = Translation.GetText("live-games");
		if (Constants.LIVE_SCORES_ENABLED)
		{
			gameObject4.transform.Find("StatsBtn").GetComponent<Button>().onClick.AddListener(() =>
			{
				ChangeOverView(StatsView.Statistics);
			});
			gameObject4.transform.Find("LiveTableBtn").GetComponent<Button>().onClick.AddListener(() =>
			{
				ChangeOverView(StatsView.LiveTable);
			});
			gameObject4.transform.Find("LiveScoresBtn").GetComponent<Button>().onClick.AddListener(() =>
			{
				ChangeOverView(StatsView.LiveScores);
			});
			gameObject8.GetComponent<Button>().onClick.AddListener(() =>
			{
				ChangeOverView(StatsView.OtherGames);
			});
			Transform transform = gameObject.transform.Find("OverviewMode/statsPanel/Panel");
			transform.GetComponent<TabsBlue>().TabSelected(transform.Find("StatsBtn").gameObject);
			GameObject gameObject9 = liveTable.transform.Find("LeagueTablePanel/LeaguePanel/HelperRow").gameObject;
			gameObject9.transform.Find("Pos").GetComponent<Text>().text = Translation.GetText("Squad-Position");
			gameObject9.transform.Find("ClubName").GetComponent<Text>().text = Translation.GetText("team");
			gameObject9.transform.Find("Played").GetComponent<Text>().text = Translation.GetText("p");
			gameObject9.transform.Find("GoalDiff").GetComponent<Text>().text = Translation.GetText("gd");
			gameObject9.transform.Find("Points").GetComponent<Text>().text = Translation.GetText("pts");
		}
		else
		{
			gameObject5.GetComponent<Image>().color = Color.clear;
			gameObject6.SetActive(value: false);
			gameObject7.SetActive(value: false);
			gameObject8.SetActive(value: false);
		}
		matchStatsPanel.SetActive(value: true);
		liveTable.SetActive(value: false);
		liveScores.SetActive(value: false);
		allGames.SetActive(value: false);
		matchStatsPanel.transform.Find("Stat1/StatName").GetComponent<Text>().text = Translation.GetText("possession");
		matchStatsPanel.transform.Find("Stat2/StatName").GetComponent<Text>().text = Translation.GetText("shots");
		matchStatsPanel.transform.Find("Stat3/StatName").GetComponent<Text>().text = Translation.GetText("shots-on-target");
		matchStatsPanel.transform.Find("Stat4/StatName").GetComponent<Text>().text = Translation.GetText("attr-corners");
		matchStatsPanel.transform.Find("Stat5/StatName").GetComponent<Text>().text = Translation.GetText("yellow-cards");
		matchStatsPanel.transform.Find("Stat6/StatName").GetComponent<Text>().text = Translation.GetText("red-cards");
		matchStatsPanel.transform.Find("Stat7/StatName").GetComponent<Text>().text = Translation.GetText("shots-blocked");
		gameObject4.transform.Find("StatsBtn/Text").GetComponent<Text>().text = Translation.GetText("match-statistics");
		Color homeColour = fixtureKitHelper.homeColour1;
		_ = fixtureKitHelper.homeColour2;
		Color awayColour = fixtureKitHelper.awayColour1;
		_ = fixtureKitHelper.awayColour2;
		foreach (Transform item in matchStatsPanel.transform)
		{
			item.Find("StatBars/StatBarHome").GetComponent<Image>().color = homeColour;
			item.Find("StatBars/StatBarAway").GetComponent<Image>().color = awayColour;
		}
		if (!quickMatchScreenExists)
		{
			return;
		}
		quickMatchStatsPanel = quickMatchScreen.transform.Find("Stats").gameObject;
		quickMatchStatsPanel.transform.Find("Stat1/StatName").GetComponent<Text>().text = Translation.GetText("possession");
		quickMatchStatsPanel.transform.Find("Stat2/StatName").GetComponent<Text>().text = Translation.GetText("shots");
		quickMatchStatsPanel.transform.Find("Stat3/StatName").GetComponent<Text>().text = Translation.GetText("shots-on-target");
		quickMatchStatsPanel.transform.Find("Stat4/StatName").GetComponent<Text>().text = Translation.GetText("attr-corners");
		quickMatchStatsPanel.transform.Find("Stat5/StatName").GetComponent<Text>().text = Translation.GetText("yellow-cards");
		quickMatchStatsPanel.transform.Find("Stat6/StatName").GetComponent<Text>().text = Translation.GetText("red-cards");
		quickMatchStatsPanel.transform.Find("Stat7/StatName").GetComponent<Text>().text = Translation.GetText("shots-blocked");
		foreach (Transform item2 in quickMatchStatsPanel.transform)
		{
			item2.Find("StatBars/StatBarHome").GetComponent<Image>().color = homeColour;
			item2.Find("StatBars/StatBarAway").GetComponent<Image>().color = awayColour;
		}
	}

	private void InitOverview(SeFixtureInfo fixInfo)
	{
		short clubDataID = fixInfo.fixtureInfo.home.clubDataID;
		short clubDataID2 = fixInfo.fixtureInfo.away.clubDataID;
		GameObject gameObject = topPanel;
		gameObject.transform.Find("HomeTeam/HomeName").GetComponent<Text>().text = Club.GetName(clubDataID);
		gameObject.transform.Find("AwayTeam/AwayName").GetComponent<Text>().text = Club.GetName(clubDataID2);
		if (!(Game.instance == null))
		{
			ImageController.GetInstance().LoadImage(clubDataID, gameObject.transform.Find("HomeTeam/HomeBadge").GetComponent<Image>(), ImageType.Club);
			ImageController.GetInstance().LoadImage(clubDataID2, gameObject.transform.Find("AwayTeam/AwayBadge").GetComponent<Image>(), ImageType.Club);
		}
	}

	private bool CheckForShootout(int state)
	{
		if (state != 600)
		{
			return state == 605;
		}
		return true;
	}

	private void InitPlayerStatsView(SeFixtureInfo fixInfo)
	{
		GameObject gameObject = GameObject.Find("OverlayCanvasMatch");
		GameObject gameObject2 = gameObject.transform.Find("OverviewMode/OverviewPanel/Centre/Panel/PlayerRatings/Panel").gameObject;
		GameObject gameObject3 = gameObject2.transform.Find("HomeTeam").gameObject;
		GameObject gameObject4 = gameObject2.transform.Find("AwayTeam").gameObject;
		gameObject.transform.Find("OverviewMode/OverviewPanel/Centre/Panel/PlayerRatings/Header/ContentHeader/Text").GetComponent<Text>().text = Translation.GetText("player-statistics").ToUpper();
		SeFixtureInfo.SePlayerInfo[] players = fixInfo.fixtureInfo.home.players;
		SeFixtureInfo.SePlayerInfo[] players2 = fixInfo.fixtureInfo.away.players;
		for (int i = 1; i < players.Length; i++)
		{
			GameObject gameObject5 = gameObject3.transform.GetChild(i - 1).gameObject;
			if (i <= 11)
			{
				gameObject5.transform.Find("RatingStar/rating").GetComponentInChildren<Text>().text = "6";
				gameObject5.transform.Find("RatingStar/rating").GetComponent<Text>().color = Game.instance.getColourForText(6f);
			}
			else
			{
				gameObject5.transform.Find("RatingStar/rating").GetComponentInChildren<Text>().text = "-";
			}
			gameObject5.transform.Find("Playername").GetComponent<Text>().text = Player.GetShortName(players[i].id);
			gameObject5.transform.Find("Fitness").GetComponent<Text>().text = players[i].fitness.value + "%";
			gameObject5.transform.Find("Fitness").GetComponent<Text>().color = Game.instance.getColourForText(players[i].fitness.value / 10f);
			gameObject5.transform.Find("Sub").gameObject.SetActive(value: false);
			gameObject5.SetActive(value: true);
		}
		for (int j = 1; j < players2.Length; j++)
		{
			GameObject gameObject5 = gameObject4.transform.GetChild(j - 1).gameObject;
			if (j <= 11)
			{
				gameObject5.transform.Find("RatingStar/rating").GetComponentInChildren<Text>().text = "6";
				gameObject5.transform.Find("RatingStar/rating").GetComponent<Text>().color = Game.instance.getColourForText(6f);
			}
			else
			{
				gameObject5.transform.Find("RatingStar/rating").GetComponentInChildren<Text>().text = "-";
			}
			gameObject5.transform.Find("Playername").GetComponent<Text>().text = Player.GetShortName(players2[j].id);
			gameObject5.transform.Find("Fitness").GetComponent<Text>().text = players2[j].fitness.value + "%";
			gameObject5.transform.Find("Fitness").GetComponent<Text>().color = Game.instance.getColourForText(players2[j].fitness.value / 10f);
			gameObject5.transform.Find("Sub").gameObject.SetActive(value: false);
			gameObject5.SetActive(value: true);
		}
	}

	private void UpdatePlayerStats()
	{
		if (!setupStatsObjects)
		{
			homePlayers = sm.match.matchStats.teams["home"].playerStats;
			awayPlayers = sm.match.matchStats.teams["away"].playerStats;
			liveHomePlayers = new cVisualPlayerStats[homePlayers.Length];
			liveAwayPlayers = new cVisualPlayerStats[awayPlayers.Length];
			for (int i = 1; i < homePlayers.Length; i++)
			{
				liveHomePlayers[i] = new cVisualPlayerStats();
				liveHomePlayers[i].performance = homePlayers[i].performance;
				liveHomePlayers[i].fitness = homePlayers[i].fitness;
				liveHomePlayers[i].playerRow = homePlayersPanel.transform.GetChild(i - 1);
				liveHomePlayers[i].playerRating = liveHomePlayers[i].playerRow.Find("RatingStar/rating").GetComponentInChildren<Text>();
				liveHomePlayers[i].playerFitness = liveHomePlayers[i].playerRow.Find("Fitness").GetComponent<Text>();
				liveHomePlayers[i].playerSub = liveHomePlayers[i].playerRow.Find("Sub").gameObject;
				liveAwayPlayers[i] = new cVisualPlayerStats();
				liveAwayPlayers[i].performance = awayPlayers[i].performance;
				liveAwayPlayers[i].fitness = awayPlayers[i].fitness;
				liveAwayPlayers[i].playerRow = awayPlayersPanel.transform.GetChild(i - 1);
				liveAwayPlayers[i].playerRating = liveAwayPlayers[i].playerRow.Find("RatingStar/rating").GetComponentInChildren<Text>();
				liveAwayPlayers[i].playerFitness = liveAwayPlayers[i].playerRow.Find("Fitness").GetComponent<Text>();
				liveAwayPlayers[i].playerSub = liveAwayPlayers[i].playerRow.Find("Sub").gameObject;
			}
			setupStatsObjects = true;
		}
		for (int j = 0; j < 2; j++)
		{
			cPlayerStats[] array;
			cVisualPlayerStats[] array2;
			List<cSubStats> sub;
			if (j == 0 || j != 1)
			{
				array = homePlayers;
				array2 = liveHomePlayers;
				_ = homePlayersPanel.transform;
				sub = sm.match.matchStats.teams["home"].sub;
			}
			else
			{
				array = awayPlayers;
				array2 = liveAwayPlayers;
				_ = awayPlayersPanel.transform;
				sub = sm.match.matchStats.teams["away"].sub;
			}
			int k;
			for (k = 1; k < array.Length; k++)
			{
				bool flag = sub.Exists((cSubStats x) => x.playerOff == k);
				bool flag2 = sub.Exists((cSubStats x) => x.playerOn == k);
				if (k <= 11)
				{
					flag2 = true;
				}
				if (array2[k].playerOn != flag2 || array2[k].performance != array[k].performance)
				{
					array2[k].playerRating.text = (flag2 ? array[k].performance.ToString("F1") : "-");
					array2[k].playerRating.color = (flag2 ? Game.instance.getColourForText(array[k].performance) : Color.white);
					array2[k].performance = (flag2 ? array[k].performance : (-1f));
					array2[k].playerOn = flag2;
				}
				if (flag && array2[k].playerSub.activeSelf != flag)
				{
					array2[k].playerSub.SetActive(flag);
				}
				if (flag2 && array2[k].playerSub.activeSelf == flag2)
				{
					array2[k].playerSub.SetActive(!flag2);
				}
				SetPlayerStatsCardimage(array[k], array2[k].playerRow);
				if (array2[k].fitness != array[k].fitness)
				{
					array2[k].playerFitness.text = array[k].fitness + "%";
					array2[k].playerFitness.color = Game.instance.getColourForText(array[k].fitness / 10f);
					array2[k].fitness = array[k].fitness;
				}
			}
		}
	}

	private void SetPlayerStatsCardimage(cPlayerStats player, Transform playerRow)
	{
		int yellowCards = player.yellowCards;
		int redCards = player.redCards;
		if (yellowCards == 2)
		{
			playerRow.Find("Playername").GetComponent<Text>().color = Color.red;
		}
		else if (redCards == 1)
		{
			playerRow.Find("Playername").GetComponent<Text>().color = Color.red;
		}
		else if (yellowCards == 1)
		{
			playerRow.Find("Playername").GetComponent<Text>().color = Color.yellow;
		}
	}

	private void DrawLiveGoalEvent(int minute)
	{
		if (!Constants.LIVE_SCORES_ENABLED)
		{
			return;
		}
		GameObject gameObject = GameObject.Find("OverlayCanvasMatch").transform.Find("OverviewMode").gameObject.transform.Find("statsPanel").gameObject.transform.Find("LatestScores").gameObject.transform.Find("ScoresContainer/ScrollView/ScoresPanel").gameObject;
		GameObject gameObject2 = gameObject.transform.Find("DetailsHelper").gameObject;
		GameObject gameObject3 = UnityEngine.Object.Instantiate(gameObject2, gameObject2.transform.parent, worldPositionStays: false);
		RectTransform component = gameObject3.GetComponent<RectTransform>();
		gameObject3.transform.SetParent(gameObject.transform);
		component.localPosition = new Vector3(transform.position.x, transform.position.y, 0f);
		gameObject3.transform.localScale = gameObject2.transform.localScale;
		gameObject3.transform.Find("HomeTeamName").GetComponent<Text>().text = seLiveScores[liveScoresCounter].homeClubName;
		gameObject3.transform.Find("AwayTeamName").GetComponent<Text>().text = seLiveScores[liveScoresCounter].awayClubName;
		gameObject3.transform.Find("HomeScore").GetComponent<Text>().text = seLiveScores[liveScoresCounter].homeScore.ToString();
		gameObject3.transform.Find("AwayScore").GetComponent<Text>().text = seLiveScores[liveScoresCounter].awayScore.ToString();
		if (seLiveScores[liveScoresCounter].goalEvent)
		{
			gameObject3.name = "goalscorer " + seLiveScores[liveScoresCounter].playerName;
			if (seLiveScores[liveScoresCounter].HomeEvent)
			{
				gameObject3.transform.Find("HomeTeamName/GoalScorerNameHome").GetComponent<Text>().text = seLiveScores[liveScoresCounter].playerName;
				gameObject3.transform.Find("HomeTeamName/GoalTime").GetComponent<Text>().text = seLiveScores[liveScoresCounter].eventTime.ToString();
				if (seLiveScores[liveScoresCounter].type == 4)
				{
					gameObject3.transform.Find("HomeTeamName/GoalScorerNameHome/GoalType").GetComponent<Text>().text = "(" + Translation.GetText("player-own-goals") + ")";
				}
				else if (seLiveScores[liveScoresCounter].type == 1)
				{
					gameObject3.transform.Find("HomeTeamName/GoalScorerNameHome/GoalType").GetComponent<Text>().text = "(" + Translation.GetText("p") + ")";
				}
			}
			else
			{
				gameObject3.transform.Find("AwayTeamName/GoalScorerNameAway").GetComponent<Text>().text = seLiveScores[liveScoresCounter].playerName;
				gameObject3.transform.Find("AwayTeamName/GoalTime").GetComponent<Text>().text = seLiveScores[liveScoresCounter].eventTime.ToString();
				if (seLiveScores[liveScoresCounter].type == 4)
				{
					gameObject3.transform.Find("AwayTeamName/GoalScorerNameAway/GoalType").GetComponent<Text>().text = "(" + Translation.GetText("player-own-goals") + ")";
				}
				else if (seLiveScores[liveScoresCounter].type == 1)
				{
					gameObject3.transform.Find("AwayTeamName/GoalScorerNameAway/GoalType").GetComponent<Text>().text = "(" + Translation.GetText("p") + ")";
				}
			}
			gameObject3.transform.Find("Image").GetComponent<Image>().sprite = goalIcon;
		}
		else if (!seLiveScores[liveScoresCounter].goalEvent)
		{
			gameObject3.name = "Carded Player " + seLiveScores[liveScoresCounter].playerName;
			if (seLiveScores[liveScoresCounter].HomeEvent)
			{
				gameObject3.transform.Find("HomeTeamName/RedCardNameHome").GetComponent<Text>().text = seLiveScores[liveScoresCounter].playerName;
				gameObject3.transform.Find("HomeTeamName/GoalTime").GetComponent<Text>().text = seLiveScores[liveScoresCounter].eventTime.ToString();
			}
			else
			{
				gameObject3.transform.Find("AwayTeamName/RedCardNameAway").GetComponent<Text>().text = seLiveScores[liveScoresCounter].playerName;
				gameObject3.transform.Find("AwayTeamName/GoalTime").GetComponent<Text>().text = seLiveScores[liveScoresCounter].eventTime.ToString();
			}
			gameObject3.transform.Find("Image").GetComponent<Image>().sprite = redCard;
		}
		gameObject3.transform.Find("Versus").GetComponent<Text>().text = Translation.GetText("Versus");
		gameObject3.SetActive(value: true);
	}

	private void DrawAllCurrentGamesPlayed()
	{
		if (Constants.LIVE_SCORES_ENABLED)
		{
			GameObject gameObject = GameObject.Find("OverlayCanvasMatch");
			GameObject gameObject2 = gameObject.transform.Find("OverviewMode").gameObject;
			_ = gameObject2.transform.Find("statsPanel").gameObject;
			GameObject gameObject3 = gameObject2.transform.Find("OverviewPanel").gameObject.transform.Find("Right/Panel/AllGames").gameObject.transform.Find("GamesContainer/ScrollView/GamesPanel").gameObject;
			GameObject gameObject4 = gameObject3.transform.Find("DetailsHelper").gameObject;
			gameObject.transform.Find("OverviewMode/OverviewPanel/Right/Panel/AllGames/GamesContainer/Header/Name").GetComponent<Text>().text = Translation.GetText("live-scores").ToUpper();
			for (int i = 0; i < seAllGames.Count; i++)
			{
				GameObject gameObject5 = UnityEngine.Object.Instantiate(gameObject4, gameObject4.transform.parent, worldPositionStays: false);
				RectTransform component = gameObject5.GetComponent<RectTransform>();
				gameObject5.transform.SetParent(gameObject3.transform);
				component.localPosition = new Vector3(transform.position.x, transform.position.y, 0f);
				gameObject5.transform.localScale = gameObject4.transform.localScale;
				int homeClubID = seAllGames[i].homeClubID;
				_ = seAllGames[i].awayClubID;
				gameObject5.name = homeClubID.ToString();
				gameObject5.transform.Find("HomeName").GetComponent<Text>().text = seAllGames[i].homeClubName;
				gameObject5.transform.Find("AwayName").GetComponent<Text>().text = seAllGames[i].awayClubName;
				gameObject5.transform.Find("HomeScore").GetComponent<Text>().text = "0";
				gameObject5.transform.Find("AwayScore").GetComponent<Text>().text = "0";
				gameObject5.transform.Find("Versus").GetComponent<Text>().text = Translation.GetText("Versus");
				gameObject5.SetActive(value: true);
			}
		}
	}

	private void DrawLeagueTablesTesting(bool continentalCup)
	{
		if (!Constants.LIVE_SCORES_ENABLED)
		{
			return;
		}
		GameObject gameObject = GameObject.Find("OverlayCanvasMatch").transform.Find("OverviewMode").gameObject.transform.Find("OverviewPanel/Right/Panel").gameObject.transform.Find("Table").gameObject;
		gameObject.SetActive(value: true);
		GameObject gameObject2 = gameObject.transform.Find("LeagueTable/LeagueContainer/ScrollView/Viewport/LeaguePanel").gameObject;
		GameObject gameObject3 = gameObject2.transform.Find("HelperRow").gameObject;
		short num = managedClubID;
		LeagueList.GetLeaguePosition(num);
		int leagueDataId = Club.GetLeagueDataId(num);
		Club.GetDivision(num);
		Helpers.GetLeagueFromSeason(leagueDataId);
		for (int i = 0; i < seLiveTable.Count; i++)
		{
			GameObject gameObject4 = UnityEngine.Object.Instantiate(gameObject3, gameObject3.transform.parent, worldPositionStays: false);
			RectTransform component = gameObject4.GetComponent<RectTransform>();
			gameObject4.transform.SetParent(gameObject2.transform);
			component.localPosition = new Vector3(transform.position.x, transform.position.y, 0f);
			if (continentalCup)
			{
				VerticalLayoutGroup component2 = gameObject2.GetComponent<VerticalLayoutGroup>();
				component2.padding = new RectOffset(component2.padding.left, component2.padding.right, 0, component2.padding.bottom);
			}
			gameObject4.transform.localScale = gameObject3.transform.localScale;
			int num2 = i + 1;
			short clubID = seLiveTable[i].clubID;
			gameObject4.name = Club.GetName(clubID).Replace('/', ' ');
			drawLeagueRow(gameObject4, num, i, num2);
		}
	}

	private void drawLeagueRow(GameObject currRow, short ClubID, int index, int pos)
	{
		if (Constants.LIVE_SCORES_ENABLED)
		{
			currRow.transform.Find("GoalDiff").GetComponent<Text>().text = seLiveTable[index].StartingGD.ToString();
			currRow.transform.Find("Points").GetComponent<Text>().text = seLiveTable[index].currentPoints.ToString();
			currRow.transform.Find("Pos").GetComponent<Text>().text = pos.ToString();
			currRow.transform.Find("Played").GetComponent<Text>().text = seLiveTable[index].played.ToString();
			currRow.transform.Find("ClubName").GetComponent<Text>().text = seLiveTable[index].clubName;
			ImageController.GetInstance().LoadImage(seLiveTable[index].clubID, currRow.transform.Find("ClubImage/Image").GetComponent<Image>(), ImageType.Club);
			if (seLiveTable[index].clubName == Club.GetName(ClubID))
			{
				currRow.transform.Find("TeamHighlight").gameObject.SetActive(value: true);
			}
			else
			{
				currRow.transform.Find("TeamHighlight").gameObject.SetActive(value: false);
			}
			DrawleaguePositionChange(currRow, index, firstDraw: false);
			currRow.SetActive(value: true);
		}
	}

	private void UpDateAllCurrentGamesPlayed(int minute, float time)
	{
		if (!Constants.LIVE_SCORES_ENABLED)
		{
			return;
		}
		GameObject gameObject = GameObject.Find("OverlayCanvasMatch").transform.Find("OverviewMode").gameObject;
		_ = gameObject.transform.Find("statsPanel").gameObject;
		GameObject gameObject2 = gameObject.transform.Find("OverviewPanel").gameObject.transform.Find("Right/Panel/AllGames").gameObject.transform.Find("GamesContainer/ScrollView/GamesPanel").gameObject;
		_ = gameObject2.transform.Find("DetailsHelper").gameObject;
		_ = sm.match;
		int num = 60;
		for (int i = 0; i < seAllGames.Count; i++)
		{
			for (int j = 0; j < seLiveScores.Count; j++)
			{
				if (j == liveScoresCounter && ((minute == seLiveScores[liveScoresCounter].eventTime && time < (float)(num * num)) || (seLiveScores[liveScoresCounter].eventTime == num + minute && time > (float)(num * num))))
				{
					GameObject gameObject3 = gameObject2.transform.Find(seLiveScores[liveScoresCounter].homeClubID.ToString()).gameObject;
					_ = seLiveScores[liveScoresCounter].homeClubID;
					_ = seLiveScores[liveScoresCounter].awayClubID;
					gameObject3.transform.Find("HomeScore").GetComponent<Text>().text = seLiveScores[liveScoresCounter].homeScore.ToString();
					gameObject3.transform.Find("AwayScore").GetComponent<Text>().text = seLiveScores[liveScoresCounter].awayScore.ToString();
					break;
				}
			}
		}
	}

	private void UpdateLivetable(short[] ClubID, int livesScoresIndex, bool continentalCup)
	{
		GameObject gameObject = GameObject.Find("OverlayCanvasMatch").transform.Find("OverviewMode").gameObject.transform.Find("OverviewPanel/Right/Panel").gameObject.transform.Find("Table").gameObject.transform.Find("LeagueTable/LeagueContainer/ScrollView/Viewport/LeaguePanel").gameObject;
		_ = gameObject.transform.Find("HelperRow").gameObject;
		int[] array = new int[2];
		if (livesScoresIndex != -1)
		{
			for (int i = 0; i < seLiveTable.Count; i++)
			{
				if (seLiveTable[i].clubName == Club.GetName(ClubID[0]))
				{
					array[0] = i;
				}
				if (seLiveTable[i].clubName == Club.GetName(ClubID[1]))
				{
					array[1] = i;
				}
			}
			if (array[0] == 0 && array[1] == 0)
			{
				return;
			}
		}
		else
		{
			cMatch cMatch2 = sm.match;
			for (int j = 0; j < seLiveTable.Count; j++)
			{
				if (seLiveTable[j].clubName == cMatch2.fixtureInfo.home.name)
				{
					array[0] = j;
				}
				if (seLiveTable[j].clubName == cMatch2.fixtureInfo.away.name)
				{
					array[1] = j;
				}
			}
		}
		GameObject gameObject2 = gameObject.transform.Find(seLiveTable[array[0]].clubName.Replace('/', ' ')).gameObject;
		GameObject gameObject3 = gameObject.transform.Find(seLiveTable[array[1]].clubName.Replace('/', ' ')).gameObject;
		gameObject2.transform.Find("Points").GetComponent<Text>().text = seLiveTable[array[0]].currentPoints.ToString();
		gameObject2.transform.Find("GoalDiff").GetComponent<Text>().text = seLiveTable[array[0]].currentGoalDifference.ToString();
		gameObject3.transform.Find("Points").GetComponent<Text>().text = seLiveTable[array[1]].currentPoints.ToString();
		gameObject3.transform.Find("GoalDiff").GetComponent<Text>().text = seLiveTable[array[1]].currentGoalDifference.ToString();
		seLiveTable = (from value in seLiveTable
			orderby value.currentPoints descending, value.currentGoalDifference descending
			select value).ToList();
		for (int num = 0; num < seLiveTable.Count; num++)
		{
			gameObject2 = gameObject.transform.Find(seLiveTable[num].clubName.Replace('/', ' ')).gameObject;
			gameObject2.transform.SetSiblingIndex(num);
		}
		int num2 = 0;
		num2 = Game.instance.g_continentalCompetitions[Game.instance.g_matchday_nextfix.competitionid].groups[0].numclubs;
		int num3 = 1;
		for (int num4 = 0; num4 < num2; num4++)
		{
			gameObject2 = gameObject.transform.Find(seLiveTable[num4].clubName.Replace('/', ' ')).gameObject;
			gameObject2.transform.Find("Pos").GetComponent<Text>().text = num3.ToString();
			seLiveTable[num4].currentPosition = num4;
			DrawleaguePositionChange(gameObject2, num4, firstDraw: true);
			num3++;
		}
		seLiveTable[array[0]].redraw = false;
		seLiveTable[array[1]].redraw = false;
	}

	private void UpdateLivetable(short[] ClubID, int livesScoresIndex)
	{
		GameObject gameObject = GameObject.Find("OverlayCanvasMatch").transform.Find("OverviewMode").gameObject.transform.Find("OverviewPanel/Right/Panel").gameObject.transform.Find("Table").gameObject.transform.Find("LeagueTable/LeagueContainer/ScrollView/Viewport/LeaguePanel").gameObject;
		_ = gameObject.transform.Find("HelperRow").gameObject;
		int[] array = new int[2];
		if (livesScoresIndex != -1)
		{
			for (int i = 0; i < seLiveTable.Count; i++)
			{
				if (seLiveTable[i].clubName == Club.GetName(ClubID[0]))
				{
					array[0] = i;
				}
				if (seLiveTable[i].clubName == Club.GetName(ClubID[1]))
				{
					array[1] = i;
				}
			}
			if (seLiveTable[array[0]].clubID != seLiveScores[livesScoresIndex].homeClubID)
			{
				for (int j = 0; j < seLiveTable.Count; j++)
				{
					if (seLiveTable[j].clubID == seLiveScores[livesScoresIndex].homeClubID)
					{
						array[0] = seLiveTable[j].currentPosition;
					}
				}
			}
			if (seLiveTable[array[1]].clubID != seLiveScores[livesScoresIndex].awayClubID)
			{
				for (int k = 0; k < seLiveTable.Count; k++)
				{
					if (seLiveTable[k].clubID == seLiveScores[livesScoresIndex].awayClubID)
					{
						array[0] = seLiveTable[k].currentPosition;
					}
				}
			}
		}
		else
		{
			cMatch cMatch2 = sm.match;
			for (int l = 0; l < seLiveTable.Count; l++)
			{
				if (seLiveTable[l].clubName == cMatch2.fixtureInfo.home.name)
				{
					array[0] = l;
				}
				if (seLiveTable[l].clubName == cMatch2.fixtureInfo.away.name)
				{
					array[1] = l;
				}
			}
		}
		GameObject gameObject2 = gameObject.transform.Find(seLiveTable[array[0]].clubName.Replace('/', ' ')).gameObject;
		GameObject gameObject3 = gameObject.transform.Find(seLiveTable[array[1]].clubName.Replace('/', ' ')).gameObject;
		gameObject2.transform.Find("Points").GetComponent<Text>().text = seLiveTable[array[0]].currentPoints.ToString();
		gameObject2.transform.Find("GoalDiff").GetComponent<Text>().text = seLiveTable[array[0]].currentGoalDifference.ToString();
		gameObject3.transform.Find("Points").GetComponent<Text>().text = seLiveTable[array[1]].currentPoints.ToString();
		gameObject3.transform.Find("GoalDiff").GetComponent<Text>().text = seLiveTable[array[1]].currentGoalDifference.ToString();
		seLiveTable = (from value in seLiveTable
			orderby value.currentPoints descending, value.currentGoalDifference descending, value.clubName descending
			select value).ToList();
		for (int num = 0; num < seLiveTable.Count; num++)
		{
			gameObject2 = gameObject.transform.Find(seLiveTable[num].clubName.Replace('/', ' ')).gameObject;
			gameObject2.transform.SetSiblingIndex(num);
		}
		ChosenLeague leagueFromSeason = Helpers.GetLeagueFromSeason(seLiveTable[0].leagueID);
		int num2 = 1;
		for (int num3 = 0; num3 < leagueFromSeason.NumClubs; num3++)
		{
			gameObject2 = gameObject.transform.Find(seLiveTable[num3].clubName.Replace('/', ' ')).gameObject;
			gameObject2.transform.Find("Pos").GetComponent<Text>().text = num2.ToString();
			seLiveTable[num3].currentPosition = num3;
			DrawleaguePositionChange(gameObject2, num3, firstDraw: true);
			num2++;
		}
		seLiveTable[array[0]].redraw = false;
		seLiveTable[array[1]].redraw = false;
	}

	private void DrawleaguePositionChange(GameObject currRow, int index, bool firstDraw)
	{
		int num = 0;
		if (firstDraw)
		{
			num = 1;
		}
		if (seLiveTable[index].currentPosition + num > seLiveTable[index].StartPosition)
		{
			currRow.transform.Find("ArrowImage").GetComponent<Image>().sprite = UI.GetInstance().arrow10;
		}
		else if (seLiveTable[index].currentPosition + num < seLiveTable[index].StartPosition)
		{
			currRow.transform.Find("ArrowImage").GetComponent<Image>().sprite = UI.GetInstance().arrow1;
		}
		else
		{
			currRow.transform.Find("ArrowImage").GetComponent<Image>().sprite = UI.GetInstance().arrowNeutral;
		}
	}

	private void CreateMatchEventUI(string team, int playerIx, int state, int goalType = 0)
	{
		GameObject gameObject = GameObject.Find("OverlayCanvasMatch");
		statsPanel = gameObject.transform.Find("OverviewMode/OverviewPanel").gameObject;
		GameObject gameObject2 = statsPanel.transform.Find("left/Panel/MatchEvents/ScrollView/Viewport/Panel/HomeTeam/EventRow").gameObject;
		GameObject gameObject3 = statsPanel.transform.Find("left/Panel/MatchEvents/ScrollView/Viewport/Panel/AwayTeam/EventRow").gameObject;
		TimeSpan timeSpan = TimeSpan.FromSeconds(sm.match.simTimeToMatchTime(time));
		string text = (timeSpan.Hours * 60 + timeSpan.Minutes + 1).ToString();
		GameObject gameObject4;
		if (team == "home")
		{
			gameObject4 = UnityEngine.Object.Instantiate(gameObject2, gameObject2.transform.parent, worldPositionStays: false);
			gameObject4.transform.localScale = gameObject2.transform.localScale;
			if (goalType == 4)
			{
				gameObject4.transform.Find("Player").GetComponent<Text>().text = Player.GetShortName(sm.match.fixtureInfo.away.players[playerIx].id);
			}
			else
			{
				gameObject4.transform.Find("Player").GetComponent<Text>().text = Player.GetShortName(sm.match.fixtureInfo.home.players[playerIx].id);
			}
			Text component = gameObject4.transform.Find("Player").GetComponent<Text>();
			component.text = component.text + " " + text;
		}
		else
		{
			gameObject4 = UnityEngine.Object.Instantiate(gameObject3, gameObject3.transform.parent, worldPositionStays: false);
			gameObject4.transform.localScale = gameObject3.transform.localScale;
			if (goalType == 4)
			{
				gameObject4.transform.Find("Player").GetComponent<Text>().text = Player.GetShortName(sm.match.fixtureInfo.home.players[playerIx].id);
			}
			else
			{
				gameObject4.transform.Find("Player").GetComponent<Text>().text = Player.GetShortName(sm.match.fixtureInfo.away.players[playerIx].id);
			}
			Text component2 = gameObject4.transform.Find("Player").GetComponent<Text>();
			component2.text = component2.text + " " + text;
		}
		switch (state)
		{
		case 301:
			gameObject4.transform.name = "yellow" + playerIx;
			gameObject4.transform.Find("Image").GetComponent<Image>().sprite = yellowCard;
			break;
		case 302:
			gameObject4.transform.name = "red" + playerIx;
			gameObject4.transform.Find("Image").GetComponent<Image>().sprite = redCard;
			break;
		case 2:
			switch (goalType)
			{
			case 4:
			{
				Text component3 = gameObject4.transform.Find("Player").GetComponent<Text>();
				component3.text = component3.text + "(" + Translation.GetText("player-own-goals") + ")";
				break;
			}
			case 1:
				gameObject4.transform.Find("Player").GetComponent<Text>().text += "(pen)";
				break;
			}
			gameObject4.transform.name = "goal" + playerIx;
			gameObject4.transform.Find("Image").GetComponent<Image>().sprite = goalIcon;
			break;
		}
		gameObject4.SetActive(value: true);
		if (quickMatchScreenExists)
		{
			GameObject gameObject5 = ((!(team == "home")) ? UnityEngine.Object.Instantiate(gameObject4, quickMatchScreen.transform.Find("ScrollViewAway/AwayScorers"), worldPositionStays: false) : UnityEngine.Object.Instantiate(gameObject4, quickMatchScreen.transform.Find("ScrollViewHome/HomeScorers"), worldPositionStays: false));
			gameObject5.transform.name = gameObject4.transform.name;
			if (!setupQuickEventScrollRects)
			{
				homeQuickEvents = quickMatchScreen.transform.Find("ScrollViewHome").GetComponent<ScrollRect>();
				awayQuickEvents = quickMatchScreen.transform.Find("ScrollViewAway").GetComponent<ScrollRect>();
				setupQuickEventScrollRects = true;
			}
			homeQuickEvents.enabled = homeQuickEvents.transform.GetChild(0).childCount > 7;
			awayQuickEvents.enabled = awayQuickEvents.transform.GetChild(0).childCount > 7;
		}
	}

	private void UpdateMatchEvents(string team, int playerIx, string state, int goalType = 0)
	{
		GameObject gameObject = GameObject.Find("OverlayCanvasMatch");
		statsPanel = gameObject.transform.Find("OverviewMode/OverviewPanel").gameObject;
		GameObject gameObject2 = statsPanel.transform.Find("left/Panel/MatchEvents/ScrollView/Viewport/Panel/HomeTeam").gameObject;
		GameObject gameObject3 = statsPanel.transform.Find("left/Panel/MatchEvents/ScrollView/Viewport/Panel/AwayTeam").gameObject;
		GameObject gameObject4 = ((team == "home") ? gameObject2.transform.Find(state + playerIx).gameObject : gameObject3.transform.Find(state + playerIx).gameObject);
		TimeSpan timeSpan = TimeSpan.FromSeconds(sm.match.simTimeToMatchTime(time));
		string text = (timeSpan.Hours * 60 + timeSpan.Minutes).ToString();
		Text component = gameObject4.transform.Find("Player").GetComponent<Text>();
		component.text = component.text + ", " + text;
		if (state == "yellow")
		{
			gameObject4.transform.Find("Image").GetComponent<Image>().sprite = secondYellow;
		}
		switch (goalType)
		{
		case 4:
		{
			Text component3 = gameObject4.transform.Find("Player").GetComponent<Text>();
			component3.text = component3.text + " (" + Translation.GetText("player-own-goals") + ")";
			break;
		}
		case 1:
		{
			Text component2 = gameObject4.transform.Find("Player").GetComponent<Text>();
			component2.text = component2.text + " (" + Translation.GetText("PenMarker") + ")";
			break;
		}
		}
		if (quickMatchScreenExists)
		{
			GameObject gameObject5 = quickMatchScreen.transform.Find("ScrollViewHome/HomeScorers").gameObject;
			GameObject gameObject6 = quickMatchScreen.transform.Find("ScrollViewAway/AwayScorers").gameObject;
			UnityEngine.Object.Destroy((team == "home") ? gameObject5.transform.Find(state + playerIx).gameObject : gameObject6.transform.Find(state + playerIx).gameObject);
			GameObject gameObject7 = ((!(team == "home")) ? UnityEngine.Object.Instantiate(gameObject4, quickMatchScreen.transform.Find("ScrollViewAway/AwayScorers"), worldPositionStays: false) : UnityEngine.Object.Instantiate(gameObject4, quickMatchScreen.transform.Find("ScrollViewHome/HomeScorers"), worldPositionStays: false));
			gameObject7.transform.name = gameObject4.transform.name;
		}
	}

	private void updateMatchStats()
	{
		UpdateStatsPanel(matchStatsPanel.transform);
		if (quickMatchScreenExists)
		{
			UpdateStatsPanel(quickMatchStatsPanel.transform);
		}
	}

	private void UpdateStatsPanel(Transform statsPanel)
	{
		float num = sm.matchStats.teams["home"].poss + sm.matchStats.teams["away"].poss;
		float homeValue = ((num != 0f) ? (sm.matchStats.teams["home"].poss / num * 100f) : 0f);
		float awayValue = ((num != 0f) ? (sm.matchStats.teams["away"].poss / num * 100f) : 0f);
		statsPanel.Find("Stat1/HomeText").GetComponent<Text>().text = homeValue.ToString("n0") + " % ";
		statsPanel.Find("Stat1/AwayText").GetComponent<Text>().text = awayValue.ToString("n0") + " % ";
		updateStatsBarWidth(statsPanel.transform.Find("Stat1/StatBars").gameObject, homeValue, awayValue);
		statsPanel.Find("Stat2/HomeText").GetComponent<Text>().text = sm.matchStats.teams["home"].shot.ToString();
		statsPanel.Find("Stat2/AwayText").GetComponent<Text>().text = sm.matchStats.teams["away"].shot.ToString();
		updateStatsBarWidth(statsPanel.Find("Stat2/StatBars").gameObject, (int)sm.matchStats.teams["home"].shot, (int)sm.matchStats.teams["away"].shot);
		statsPanel.Find("Stat3/HomeText").GetComponent<Text>().text = sm.matchStats.teams["home"].shotot.ToString();
		statsPanel.Find("Stat3/AwayText").GetComponent<Text>().text = sm.matchStats.teams["away"].shotot.ToString();
		updateStatsBarWidth(statsPanel.Find("Stat3/StatBars").gameObject, (int)sm.matchStats.teams["home"].shotot, (int)sm.matchStats.teams["away"].shotot);
		statsPanel.Find("Stat4/HomeText").GetComponent<Text>().text = sm.matchStats.teams["home"].corner.ToString();
		statsPanel.Find("Stat4/AwayText").GetComponent<Text>().text = sm.matchStats.teams["away"].corner.ToString();
		updateStatsBarWidth(statsPanel.Find("Stat4/StatBars").gameObject, (int)sm.matchStats.teams["home"].corner, (int)sm.matchStats.teams["away"].corner);
		statsPanel.Find("Stat5/HomeText").GetComponent<Text>().text = sm.matchStats.teams["home"].yc.ToString();
		statsPanel.Find("Stat5/AwayText").GetComponent<Text>().text = sm.matchStats.teams["away"].yc.ToString();
		updateStatsBarWidth(statsPanel.Find("Stat5/StatBars").gameObject, (int)sm.matchStats.teams["home"].yc, (int)sm.matchStats.teams["away"].yc);
		statsPanel.Find("Stat6/HomeText").GetComponent<Text>().text = sm.matchStats.teams["home"].rc.ToString();
		statsPanel.Find("Stat6/AwayText").GetComponent<Text>().text = sm.matchStats.teams["away"].rc.ToString();
		updateStatsBarWidth(statsPanel.Find("Stat6/StatBars").gameObject, (int)sm.matchStats.teams["home"].rc, (int)sm.matchStats.teams["away"].rc);
		statsPanel.Find("Stat7/HomeText").GetComponent<Text>().text = sm.matchStats.teams["home"].shotBlocked.ToString();
		statsPanel.Find("Stat7/AwayText").GetComponent<Text>().text = sm.matchStats.teams["away"].shotBlocked.ToString();
		updateStatsBarWidth(statsPanel.Find("Stat7/StatBars").gameObject, (int)sm.matchStats.teams["home"].shotBlocked, (int)sm.matchStats.teams["away"].shotBlocked);
	}

	private void UpdateCameraZoom()
	{
		float num = (float)Screen.width / (float)Screen.height;
		float num2 = 1.7777778f;
		float num3 = 1.3333334f;
		float num4 = (num - num2) / (num3 - num2);
		float orthographicSize = 4.4f + num4 * 1f;
		if (num - num2 > 0.3f)
		{
			narrowScreen = false;
		}
		GameObject.Find("SceneManager/CameraAnchor/GameCamera").GetComponent<Camera>().orthographicSize = orthographicSize;
	}

	private void updateStatsBarWidth(GameObject statBar, float homeValue, float awayValue)
	{
		RectTransform component = statBar.transform.Find("StatBarHome").GetComponent<RectTransform>();
		RectTransform component2 = statBar.transform.Find("StatBarAway").GetComponent<RectTransform>();
		float num = statBar.GetComponent<RectTransform>().rect.width;
		if (homeValue != 0f && awayValue != 0f)
		{
			component.sizeDelta = new Vector2(num * (homeValue / (homeValue + awayValue)) - 5f, component.sizeDelta.y);
			component2.sizeDelta = new Vector2(num * (awayValue / (homeValue + awayValue)) - 5f, component2.sizeDelta.y);
		}
		else if (homeValue == 0f && awayValue == 0f)
		{
			component.sizeDelta = new Vector2(num / 2f - 5f, component.sizeDelta.y);
			component2.sizeDelta = new Vector2(num / 2f - 5f, component2.sizeDelta.y);
		}
		else if (homeValue == 0f)
		{
			component.sizeDelta = new Vector2(num * 0.1f - 5f, component.sizeDelta.y);
			component2.sizeDelta = new Vector2(num * 0.9f - 5f, component2.sizeDelta.y);
		}
		else if (awayValue == 0f)
		{
			component.sizeDelta = new Vector2(num * 0.9f - 5f, component.sizeDelta.y);
			component2.sizeDelta = new Vector2(num * 0.1f - 5f, component2.sizeDelta.y);
		}
	}

	private void NextCamera()
	{
		currentCam++;
		if (currentCam >= cameras.Length)
		{
			currentCam = 0;
		}
		PlayerPrefs.SetInt("MatchCameraAngle", currentCam);
		ChangeView(MatchView.Pitch);
	}

	private void ShowTopCam()
	{
		for (int i = 0; i < cameras.Length; i++)
		{
			cameras[i].enabled = i == 0;
		}
		topPanel.transform.Find("CameraButton/Text").GetComponent<Text>().text = (currentCam + 1).ToString();
		GameObject.Find("OverlayCanvasMatch").GetComponent<Canvas>().worldCamera = cameras[0];
	}

	private void DrawCurrentCam()
	{
		for (int i = 0; i < cameras.Length; i++)
		{
			cameras[i].enabled = i == currentCam;
		}
		topPanel.transform.Find("CameraButton/Text").GetComponent<Text>().text = (currentCam + 1).ToString();
		GameObject.Find("OverlayCanvasMatch").GetComponent<Canvas>().worldCamera = cameras[currentCam];
	}

	private IEnumerator AdjustLast5PossesionForDesktop()
	{
		yield return new WaitForEndOfFrame();
		_ = GameObject.Find("OverlayCanvasMatch/MatchOverlay/Last5Possesion").GetComponent<RectTransform>().rect.width;
		GameObject.Find("OverlayCanvasMatch/MatchOverlay/Last5Possesion/Home").GetComponent<Image>().fillAmount = 0.5f;
	}

	public void SettingsToggled()
	{
		debugOptionsVisible = !debugOptionsVisible;
		debugOptions.GetComponent<CanvasGroup>().alpha = (debugOptionsVisible ? 1f : 0f);
		debugOptions.GetComponent<CanvasGroup>().interactable = debugOptionsVisible;
		debugOptions.GetComponent<CanvasGroup>().blocksRaycasts = debugOptionsVisible;
		if (!generatedSettings)
		{
			UpdateRuntimeVars();
			generatedSettings = true;
			if (showDebugMarkings)
			{
				DrawGoalDebugPoints();
			}
		}
	}

	public void OpenMatchEditor()
	{
		matchEditScreen.SetActive(value: true);
		CanvasGroup component = matchOverlay.GetComponent<CanvasGroup>();
		component.alpha = 0f;
		component.interactable = false;
		component.blocksRaycasts = false;
		MatchCameraController.instance.SwitchToCamera(CameraTypes.MATCH_EDITOR);
		MatchCameraController.instance.inEditorMode = true;
		inEditMode = true;
		StartCoroutine(PauseGame());
		speedLevel = 0;
	}

	public void CloseMatchEditor()
	{
		matchEditScreen.SetActive(value: false);
		CanvasGroup component = matchOverlay.GetComponent<CanvasGroup>();
		component.alpha = 1f;
		component.interactable = true;
		component.blocksRaycasts = true;
		MatchCameraController.instance.inEditorMode = false;
		switch (currentCameraMode)
		{
		case CameraMode.TV:
			MatchCameraController.instance.SwitchToCamera(CameraTypes.TV_CAM);
			break;
		case CameraMode.Stand:
			MatchCameraController.instance.SwitchToCamera(CameraTypes.STAND_CAM);
			break;
		case CameraMode.Panoramic:
			MatchCameraController.instance.SwitchToCamera(CameraTypes.PANORAMIC_CAM);
			break;
		case CameraMode.Overview:
			MatchCameraController.instance.SwitchToCamera(CameraTypes.OVERVIEW);
			break;
		}
		inEditMode = false;
		UnPauseGame();
		speedLevel = PlayerPrefs.GetInt("MatchSpeed");
	}

	public void UpdateRuntimeVars()
	{
		numElements = 0;
		EditableBool("Use aheadTime", ref debugUseAheadTime);
		EditableBool("Test Rot", ref debugTestRotation);
		EditableBool("Debug Action", ref debugSelectAction);
		EditableSlider("GameSpeed", ref debugGameSpeed, 0f, 4f);
		debugReplayTimeSlider = EditableSlider("ReplayTime", ref debugReplayDelta, 0f, 300f);
		debugReplayToggle = EditableBool("Replay", ref debugReplayMatch);
		EditableBool("Pause", ref debugReplayPause);
		debugInputToggle = EditableBool("Input", ref debugReplayInput);
		EditableBool("Knobble Away", ref debugKnobbleBots);
		EditableTitle("Camera Settings");
		EditableSlider("Camera Smoothness", ref cameraSmoothness, 0.1f, 2.5f);
		EditableSlider("Debug Cam X", ref MatchCameraController.instance.debugCamPosition.vx, sm.places.bottomCorner[0].vx, sm.places.bottomCorner[1].vx);
		EditableSlider("Debug Cam Y", ref MatchCameraController.instance.debugCamPosition.vy, sm.places.topCorner[0].vy, sm.places.bottomCorner[0].vy);
		EditableSlider("Debug Cam Rotation", ref MatchCameraController.instance.debugCamRotation, -180f, 180f);
		EditableTitle("Debug Spot Settings");
		EditableBool("Spot Home", ref debugSpotHome);
		EditableDropdown("Debug Spot P", new string[4] { "Corners", "Throw Ins", "Goal Kicks", "Give Away" }, ref debugSpotType);
		EditableBool("Speed Through", ref debugSpeedThrough);
		EditableBool("Force Penalty", ref debugForcePenalty);
		EditableBool("Spot Side", ref debugSpotSide);
		EditableDropdown("home fouls", new string[6] { "Auto", "No Fouls", "Warning1", "Warning2", "Yellow", "Red" }, ref debugForceFouls[0]);
		EditableDropdown("away fouls", new string[6] { "Auto", "No Fouls", "Warning1", "Warning2", "Yellow", "Red" }, ref debugForceFouls[1]);
		EditableDropdown("injuries", new string[3] { "Auto", "None", "Lots" }, ref debugForceInjuries);
		EditableBool("Deadball Speedup", ref debugDeadballSpeedup);
		EditableButton("ManyMatches Attrs", () =>
		{
			manyMatches.showManyMatches();
			SettingsToggled();
		});
		EditableBool("Dribble Velocity", ref debugDribbleVelocity);
		EditableBool("Trigger Shootout", ref debugTriggerShootout);
		EditableTitle("Debug Settings");
		EditableBool("No Offsides", ref debugNoOffsides);
		EditableBool("No Away Tackle", ref debugNoBotTackle);
		EditableBool("No Away Slide", ref debugNoBotSlide);
		EditableBool("No Away Pass", ref debugNoBotPass);
		EditableBool("No Pass Error", ref debugNoPassError);
		EditableBool("No Spaceball Error", ref debugNoSpaceballError);
		EditableBool("No Cross Error", ref debugNoCrossError);
		EditableBool("No Longball Error", ref debugNoLongballError);
		EditableBool("No Corner Error", ref debugNoCornerError);
		EditableBool("No Shot Error", ref debugNoShotError);
		EditableBool("No Shot Curl", ref debugNoShotCurl);
		EditableButton("Kick out of play", () =>
		{
			match.debugSpot = true;
		});
		EditableTitle("");
		EditableTitle("Visuals + Camera");
		EditableBool("Show Player Names", ref debugShowPlayerNames);
		EditableButton("Hide Club Badges", () =>
		{
			ImageController.GetInstance().LoadDefaultImage(ImageType.Club, topPanel.transform.Find("HomeTeam/HomeBadge").GetComponent<Image>());
			ImageController.GetInstance().LoadDefaultImage(ImageType.Club, topPanel.transform.Find("AwayTeam/AwayBadge").GetComponent<Image>());
			ImageController.GetInstance().LoadDefaultImage(ImageType.Club, overviewScreen.transform.Find("Goalscorers/Score/HomeTeam/HomeBadge").GetComponent<Image>());
			ImageController.GetInstance().LoadDefaultImage(ImageType.Club, overviewScreen.transform.Find("Goalscorers/Score/AwayTeam/AwayBadge").GetComponent<Image>());
		});
		WatchVar("Prev : ", match.prevPlayerOnBall + ", " + match.prevTeamInPlay.ToString());
		WatchVar("Curr : ", match.playerOnBall + ", " + match.teamInPlay.ToString());
		if (match.nextTeamInPlay != null)
		{
			WatchVar("Next : ", match.nextPlayerOnBall + ", " + match.nextTeamInPlay.ToString());
		}
		WatchVar("", "");
		WatchVar("", "");
		WatchVar("Engine States", "");
		WatchVar("Prev : ", engineStates.GetConstantName(match.prevEngineState) + ", " + match.prevEngineState);
		WatchVar("Curr : ", engineStates.GetConstantName(match.engineState) + ", " + match.engineState);
		WatchVar("Next : ", engineStates.GetConstantName(match.nextEngineState) + ", " + match.nextEngineState);
		WatchVar("", "");
		WatchVar("Match Actions", "");
		WatchVar("Prev : ", match.prevAction.ToString());
		WatchVar("Curr : ", match.action.ToString());
		WatchVar("", "");
		string text = "";
		if (match.engineStateInfo != null)
		{
			foreach (KeyValuePair<string, object> item in match.engineStateInfo)
			{
				text += Environment.NewLine;
				text = text + "       -> " + item.Key.ToString() + "  :  ";
				if (!(item.Value is v3d))
				{
					text = ((item.Value != null) ? (text + item.Value.ToString()) : (text + "null"));
				}
				else
				{
					text += ((v3d)item.Value).ToString();
				}
			}
		}
		else
		{
			text = "null";
		}
		WatchVarMultiline("Curr ESI : ", text);
		WatchVar("", "");
		string text2 = "";
		if (match.nextEngineStateInfo != null)
		{
			foreach (KeyValuePair<string, object> item2 in match.nextEngineStateInfo)
			{
				text2 += Environment.NewLine;
				text2 = text2 + "       -> " + item2.Key.ToString() + "  :  ";
				if (!(item2.Value is v3d))
				{
					text2 = ((item2.Value != null) ? (text2 + item2.Value.ToString()) : (text2 + "null"));
				}
				else
				{
					text2 += ((v3d)item2.Value).ToString();
				}
			}
		}
		else
		{
			text2 = "null";
		}
		WatchVarMultiline("Next ESI : ", text2);
		WatchVar("", "");
		string text3 = "";
		if (match.prevEngineStateInfo != null)
		{
			foreach (KeyValuePair<string, object> item3 in match.prevEngineStateInfo)
			{
				text3 += Environment.NewLine;
				text3 = text3 + "       -> " + item3.Key.ToString() + "  :  ";
				if (!(item3.Value is v3d))
				{
					text3 = ((item3.Value != null) ? (text3 + item3.Value.ToString()) : (text3 + "null"));
				}
				else
				{
					text3 += ((v3d)item3.Value).ToString();
				}
			}
		}
		else
		{
			text3 = "null";
		}
		WatchVarMultiline("Prev ESI : ", text3);
		string text4 = "";
		foreach (cPlayerState player in match.currentState.teams["home"].players)
		{
			if (player.playerActions.hasActions)
			{
				cPlayerAction cPlayerAction2 = player.playerActions.actions[0];
				text4 = text4 + "[" + player.playerNum + "]  => " + cPlayerAction2.actionType.ToString() + " - start: " + cPlayerAction2.actionStart.ToString("F2") + " - end: " + cPlayerAction2.actionEnd.ToString("F2");
				text4 += Environment.NewLine;
			}
		}
		foreach (cPlayerState player2 in match.currentState.teams["away"].players)
		{
			if (player2.playerActions.hasActions)
			{
				cPlayerAction cPlayerAction2 = player2.playerActions.actions[0];
				text4 = text4 + "[" + player2.playerNum + "]  => " + cPlayerAction2.actionType.ToString() + " - start: " + cPlayerAction2.actionStart.ToString("F2") + " - end: " + cPlayerAction2.actionEnd.ToString("F2");
				text4 += Environment.NewLine;
			}
		}
		WatchVarMultiline("Player Actions : ", text4);
	}

	public Slider EditableSlider(string varName, ref float varUsed, float minVal, float maxVal)
	{
		numElements++;
		Slider slider = null;
		if (generatedSettings)
		{
			if (sliders.ContainsKey(numElements))
			{
				varUsed = sliders[numElements].value;
				sliders[numElements].transform.parent.Find("ValueBox/Value").GetComponent<Text>().text = varUsed.ToString("0.###");
				slider = sliders[numElements];
			}
		}
		else
		{
			if (debugSliderRow == null)
			{
				debugSliderRow = Resources.Load<GameObject>("DebugSliderRow");
			}
			GameObject gameObject = UnityEngine.Object.Instantiate(debugSliderRow, debugScrollArea, worldPositionStays: false);
			gameObject.transform.Find("VarName").GetComponent<Text>().text = varName;
			gameObject.transform.Find("ValueBox/Value").GetComponent<Text>().text = varUsed.ToString("0.###");
			slider = gameObject.transform.Find("Slider").GetComponent<Slider>();
			slider.minValue = minVal;
			slider.maxValue = maxVal;
			slider.value = varUsed;
			sliders.Add(numElements, slider);
		}
		return slider;
	}

	private Toggle EditableBool(string varName, ref bool varUsed)
	{
		numElements++;
		Toggle toggle = null;
		if (generatedSettings)
		{
			if (toggles.ContainsKey(numElements))
			{
				varUsed = toggles[numElements].isOn;
				toggle = toggles[numElements];
			}
		}
		else
		{
			if (debugBoolRow == null)
			{
				debugBoolRow = Resources.Load<GameObject>("DebugBoolRow");
			}
			GameObject gameObject = UnityEngine.Object.Instantiate(debugBoolRow, debugScrollArea, worldPositionStays: false);
			toggle = gameObject.transform.Find("Toggle").GetComponent<Toggle>();
			gameObject.transform.Find("VarName").GetComponent<Text>().text = varName;
			toggle.isOn = varUsed;
			toggles.Add(numElements, toggle);
		}
		return toggle;
	}

	private void EditableTitle(string text)
	{
		if (!generatedSettings)
		{
			if (debugDividerRow == null)
			{
				debugDividerRow = Resources.Load<GameObject>("DebugDividerRow");
			}
			UnityEngine.Object.Instantiate(debugDividerRow, debugScrollArea, worldPositionStays: false).transform.Find("Title").GetComponent<Text>().text = text;
		}
	}

	private void WatchVar(string varName, string var)
	{
		numElements++;
		if (!generatedSettings)
		{
			if (debugPrintRow == null)
			{
				debugPrintRow = Resources.Load<GameObject>("DebugPrintVariable");
			}
			GameObject gameObject = UnityEngine.Object.Instantiate(debugPrintRow, debugVarsArea, worldPositionStays: false);
			gameObject.transform.Find("Title").GetComponent<Text>().text = varName + " " + var;
			vars[numElements] = gameObject.transform.Find("Title").GetComponent<Text>();
		}
		else
		{
			vars[numElements].text = varName + " " + var;
		}
	}

	private void WatchVarMultiline(string varName, string var)
	{
		numElements++;
		if (!generatedSettings)
		{
			if (debugPrintMultiline == null)
			{
				debugPrintMultiline = Resources.Load<GameObject>("DebugPrintMultiLineVariable");
			}
			GameObject gameObject = UnityEngine.Object.Instantiate(debugPrintMultiline, debugVarsArea, worldPositionStays: false);
			gameObject.GetComponent<Text>().text = varName + " " + var;
			vars[numElements] = gameObject.GetComponent<Text>();
		}
		else
		{
			vars[numElements].text = varName + " " + var;
		}
	}

	private Dropdown EditableDropdown(string varName, string[] dropdownOptions, ref int valueUpdated)
	{
		numElements++;
		Dropdown dropdown = null;
		List<Dropdown.OptionData> list = new List<Dropdown.OptionData>();
		if (generatedSettings)
		{
			if (dropdowns.ContainsKey(numElements))
			{
				valueUpdated = dropdowns[numElements].value;
				dropdown = dropdowns[numElements];
				if (dropdown.transform.childCount > 3)
				{
					dropdown.transform.parent.GetComponent<LayoutElement>().minHeight = 215f;
				}
				else
				{
					dropdown.transform.parent.GetComponent<LayoutElement>().minHeight = 65f;
				}
			}
		}
		else
		{
			if (debugDropdownRow == null)
			{
				debugDropdownRow = Resources.Load<GameObject>("DebugDropdownRow");
			}
			GameObject gameObject = UnityEngine.Object.Instantiate(debugDropdownRow, debugScrollArea, worldPositionStays: false);
			gameObject.transform.Find("VarName").GetComponent<Text>().text = varName;
			dropdown = gameObject.transform.Find("Dropdown").GetComponent<Dropdown>();
			dropdown.ClearOptions();
			foreach (string text in dropdownOptions)
			{
				list.Add(new Dropdown.OptionData(text));
			}
			dropdown.AddOptions(list);
			dropdown.value = valueUpdated;
			dropdowns.Add(numElements, dropdown);
		}
		return dropdown;
	}

	private Button EditableButton(string buttonText, Action methodToRun)
	{
		Button button = null;
		if (!generatedSettings)
		{
			if (debugButtonRow == null)
			{
				debugButtonRow = Resources.Load<GameObject>("DebugButtonRow");
			}
			GameObject gameObject = UnityEngine.Object.Instantiate(debugButtonRow, debugScrollArea, worldPositionStays: false);
			gameObject.transform.Find("Button/ButtonText").GetComponent<Text>().text = buttonText;
			button = gameObject.transform.Find("Button").GetComponent<Button>();
			button.onClick.AddListener(() =>
			{
				methodToRun();
			});
		}
		return button;
	}

	public void DrawCircleOnPitch(v3d pos, float radius, float timeVisible = 2f)
	{
		UnityEngine.Object.Destroy(DrawDebugPoint(new pitchDebugPoint(pos, new Color(0.8f, 0.8f, 0f))), timeVisible);
		float num = 16f;
		for (float num2 = 0f; num2 < (float)Math.PI * 2f; num2 += (float)Math.PI / num)
		{
			v3d pointOnCircle = GetPointOnCircle(pos, radius, 0.15f, num2);
			UnityEngine.Object.Destroy(DrawDebugPoint(new pitchDebugPoint(pointOnCircle, new Color(0.2f, 1f, 0f))), timeVisible);
		}
	}

	public v3d GetPointOnCircle(v3d center, float radius, float delta, float angle)
	{
		radius -= delta;
		float x = center.vx + radius * Mathf.Cos(angle);
		float y = center.vy + radius * Mathf.Sin(angle);
		return new v3d(x, y, center.vz);
	}

	public PlayerVisuals GetPlayerVisual(int playerNum, bool homeTeam)
	{
		if (homeTeam)
		{
			return team1Players[playerNum - 1];
		}
		return team2Players[playerNum - 1];
	}

	public cPlayer GetPlayerOnBall()
	{
		return match.teams[match.teamInPlay].playerCache[match.playerOnBall];
	}

	public cPlayer GetNextPlayerOnBall()
	{
		return match.teams[match.teamInPlay].playerCache[match.nextPlayerOnBall];
	}

	public PlayerVisuals GetPlayerOnBallVisual()
	{
		return GetPlayerVisual(match.playerOnBall, match.teamInPlay == "home");
	}

	public PlayerVisuals GetNextPlayerOnBallVisual()
	{
		return GetPlayerVisual(match.nextPlayerOnBall, match.nextTeamInPlay == "home");
	}

	public void HideBall()
	{
		ballVisual.gameObject.SetActive(value: false);
	}

	public void ShowBall()
	{
		ballVisual.gameObject.SetActive(value: true);
	}

	private IEnumerator AutoCloseTactics()
	{
		yield return new WaitForSeconds(0.5f);
		ConfirmTacticChanges(seFixInfo.fixtureInfo.home.clubDataID);
	}

	public MatchResult GetMatchResult()
	{
		if (match.matchStats.teams["home"].score == match.matchStats.teams["away"].score)
		{
			return MatchResult.DRAW;
		}
		if (match.matchStats.teams["home"].score > match.matchStats.teams["away"].score)
		{
			if (!(homeAway == "home"))
			{
				return MatchResult.LOST;
			}
			return MatchResult.WON;
		}
		if (!(homeAway == "home"))
		{
			return MatchResult.WON;
		}
		return MatchResult.LOST;
	}

	public bool IsValidPlayerLookState()
	{
		if (highlightMode == 5 && !quickMatchShowingHighlight)
		{
			return false;
		}
		if (match.engineState == 504 || match.engineState == 505 || match.engineState == 303 || match.engineState == 406 || match.engineState == 401 || match.engineState == 404 || match.engineState == 405 || match.engineState == 403 || match.engineState == 402 || match.engineState == 408 || match.engineState == 407 || match.engineState == 252 || match.engineState == 251 || match.engineState == 250 || match.engineState == 224 || match.engineState == 101 || match.engineState == 111 || match.engineState == 6)
		{
			return false;
		}
		return true;
	}

	public void CalculatePlayerAnimationDelays()
	{
		AnimationClip[] animationClips = GetPlayerVisual(1, homeTeam: true).playerAnimator.runtimeAnimatorController.animationClips;
		foreach (AnimationClip animationClip in animationClips)
		{
			AnimationEvent[] events = animationClip.events;
			foreach (AnimationEvent animationEvent in events)
			{
				if (animationEvent.functionName == "Delay" && !animationDelays.ContainsKey(animationClip.name))
				{
					animationDelays.Add(animationClip.name, animationEvent.time);
					if (animationEvent.floatParameter > 0f)
					{
						animationSpeeds.Add(animationClip.name, animationEvent.floatParameter);
					}
				}
			}
		}
		Game.LogConsole("Done calculating animation delays.");
	}

	public float GetAnimationLength(PlayerAnimation animType)
	{
		PlayerAnimationData.ClipData clipDataReference = PlayerAnimationData.instance.GetClipDataReference(animType);
		if (clipDataReference == null)
		{
			return 0f;
		}
		return clipDataReference.clipReference.length / clipDataReference.playBackSpeed;
	}

	public float GetAnimationDelay(PlayerAnimation animType)
	{
		PlayerAnimationData.ClipData clipDataReference = PlayerAnimationData.instance.GetClipDataReference(animType);
		switch (animType)
		{
		case PlayerAnimation.PASS_LONG:
		case PlayerAnimation.PASS_SHORT:
		case PlayerAnimation.PASS_CURL:
		case PlayerAnimation.PASS_CUT:
		case PlayerAnimation.PASS_SLICE:
		case PlayerAnimation.PASS_STRAIGHT:
		case PlayerAnimation.PASS_SIDEFOOT:
		case PlayerAnimation.FOOT_CONTROL:
		case PlayerAnimation.FOOT_CONTROL_2:
		case PlayerAnimation.KNEE_CONTROL:
		case PlayerAnimation.WAIST_CONTROL:
		case PlayerAnimation.CHEST_CONTROL:
		case PlayerAnimation.SHOOT:
		case PlayerAnimation.KICK_FOLLOWTHROUGH:
		case PlayerAnimation.VOLLEY:
		case PlayerAnimation.OVERHEAD_KICK:
		case PlayerAnimation.STANDING_HEADER:
		case PlayerAnimation.HEADER:
		case PlayerAnimation.SLIDE_TACKLE:
		case PlayerAnimation.TACKLE:
		case PlayerAnimation.TACKLE_REACTION:
		case PlayerAnimation.THROW_IN:
		case PlayerAnimation.CHOP:
		case PlayerAnimation.LACROQUETA:
		case PlayerAnimation.CROSSBODYDRAG:
		case PlayerAnimation.SINGLEFEINT:
		case PlayerAnimation.DOUBLEFEINT:
		case PlayerAnimation.HIGH_BLOCK:
		case PlayerAnimation.GK_JUMP_CATCH:
		case PlayerAnimation.GK_CHEST_CATCH:
		case PlayerAnimation.GK_SCOOP:
		case PlayerAnimation.GK_PUNCH:
		case PlayerAnimation.PLACE_BALL:
		case PlayerAnimation.TOUCH_SIDE_HEEL:
		case PlayerAnimation.DRIBBLE_LEFT:
		case PlayerAnimation.DRIBBLE_RIGHT:
		case PlayerAnimation.DRIBBLE:
		case PlayerAnimation.DRIBBLE2:
		case PlayerAnimation.CONTROL_HIGH_KICK:
		case PlayerAnimation.CONTROL_DROP_CUSH:
		case PlayerAnimation.CONTROL_CUSH_PASS:
		case PlayerAnimation.TACKLE_BARGE:
			if (clipDataReference.makesContact)
			{
				if (clipDataReference.clipReference == null)
				{
					return 0f;
				}
				_ = clipDataReference.playBackSpeed;
				_ = 0f;
				return clipDataReference.clipReference.length / clipDataReference.playBackSpeed * clipDataReference.contactPoint.contactTime;
			}
			return 0f;
		case PlayerAnimation.KEEPER_KICK:
			return animationDelays["KeeperKick"];
		default:
			return 0f;
		}
	}

	private void SkipButtonPressed()
	{
		if (!skipButtonPressed)
		{
			skipButtonPressed = true;
			skipActionPerformed = false;
			HideSkipButton();
		}
	}

	private void ShowSkipButton()
	{
		skipButtonPressed = false;
		skipActionPerformed = false;
		skipButtonShown = true;
		skipOverlay.SetActive(value: true);
	}

	public void HideSkipButton()
	{
		skipButtonShown = false;
		skipOverlay.SetActive(value: false);
	}

	private void ShouldHideSkipButton(SkipReason reason)
	{
		if (currentSkip == reason)
		{
			currentSkip = SkipReason.NONE;
			skipButtonPressed = false;
			HideSkipButton();
		}
	}

	public void ShouldShowSkipButton(SkipReason reason)
	{
		bool flag = false;
		if (reason == SkipReason.REPLAY || reason == SkipReason.GOAL_CELEBRATION || reason == SkipReason.PREMATCH_LINEUP || reason == SkipReason.HALFTIME)
		{
			flag = true;
		}
		else if (match.highlightState == 1 && speedLevel != 0)
		{
			flag = true;
		}
		if ((currentSkip != reason) & flag)
		{
			currentSkip = reason;
			ShowSkipButton();
		}
	}

	public void UpdateCamera()
	{
		MatchCameraSettingTree.instance.targetPos.x = adjPosAhead.x;
		MatchCameraSettingTree.instance.targetPos.y = adjPosAhead.z;
		MatchCameraSettingTree.instance.UpdateWeights();
		MatchCameraSetting cameraData = MatchCameraSettingTree.instance.cameraData;
		isSpecialCameraCase = false;
		float num = 0f;
		float num2 = 0f;
		Vector3 vector = default;
		switch (CalculatePlayDirection())
		{
		case PlayDirection.MOVING_LEFT_TO_RIGHT:
			currentViewportLimits = cameraData.leftToRightViewportLimits;
			num = cameraData.leftRightFOVOffset;
			num2 = cameraData.leftRightZoomOffset;
			vector = cameraData.leftRightTargetOffset;
			break;
		case PlayDirection.MOVING_RIGHT_TO_LEFT:
			currentViewportLimits = cameraData.rightToLeftViewportLimits;
			num = cameraData.rightLeftFOVOffset;
			num2 = cameraData.rightLeftZoomOffset;
			vector = cameraData.rightLeftTargetOffset;
			break;
		default:
			currentViewportLimits = cameraData.leftToRightViewportLimits;
			break;
		}
		Transform transform = ballAhead.transform;
		if (match.engineState == 2)
		{
			if (match.engineStateInfo.ContainsKey("players"))
			{
				cPlayerID[] array = (cPlayerID[])match.engineStateInfo["players"];
				transform = match.teams[array[0].team].playerCache[array[0].playerNum].currentVisual.playerObject.transform;
				currentViewportLimits = new Vector4(0.5f, 0.5f, 0.5f, 0.5f);
			}
		}
		else if (match.engineState == 6 || match.engineState == 111 || match.engineState == 101)
		{
			currentViewportLimits = new Vector4(0.5f, 0.5f, 0.4f, 0.4f);
			transformHelper.transform.position = Vector3.zero;
			transform = transformHelper.transform;
		}
		else if (match.nextEngineState == 607)
		{
			int key = (int)match.engineStateInfo["celebrator"];
			string key2 = (string)match.engineStateInfo["winners"];
			transform = match.teams[key2].playerCache[key].currentVisual.playerObject.transform;
			currentViewportLimits = new Vector4(0.5f, 0.5f, 0.5f, 0.5f);
		}
		else if (match.engineState == 7 || match.nextEngineState == 7)
		{
			currentViewportLimits = new Vector4(0.5f, 0.5f, 0.5f, 0.5f);
			vector += new Vector3(0f, 0f, 5f);
		}
		else if (match.physio.physioState != 0)
		{
			transform = physioVisual.playerObject.transform;
			vector = Vector3.zero;
		}
		else if (match.engineState == 305 || match.engineState == 301 || match.engineState == 302 || match.engineState == 307 || match.engineState == 306)
		{
			transform = refereeVisual.playerObject.transform;
			vector = Vector3.zero;
		}
		else if (match.engineState == 303)
		{
			if (match.teams[(string)match.engineStateInfo["team"]].players.ContainsKey((int)match.engineStateInfo["playerNum"]))
			{
				transform = match.teams[(string)match.engineStateInfo["team"]].players[(int)match.engineStateInfo["playerNum"]].currentVisual.playerObject.transform;
			}
			vector = Vector3.zero;
		}
		else if (match.engineState == 504)
		{
			List<int> list = (List<int>)match.engineStateInfo["playersOff"];
			List<int> list2 = (List<int>)match.engineStateInfo["playersOn"];
			int num3 = list.Count + list2.Count;
			cPlayer[] array2 = new cPlayer[num3];
			for (int i = 0; i < array2.Length; i++)
			{
				if (i >= list.Count)
				{
					array2[i] = match.teams[(string)match.engineStateInfo["team"]].playerCache[list2[i - list.Count]];
				}
				else
				{
					array2[i] = match.teams[(string)match.engineStateInfo["team"]].playerCache[list[i]];
				}
				if (array2[i].velocityThisFrame.sqrMagnitude < 0.010000001f)
				{
					array2[i] = null;
					num3--;
				}
			}
			Vector3 position = default;
			for (int j = 0; j < array2.Length; j++)
			{
				if (array2[j] != null)
				{
					position += array2[j].currentVisual.playerObject.transform.position * (1f / (float)num3);
				}
			}
			transformHelper.transform.position = position;
			transform = transformHelper.transform;
		}
		Vector3 position2 = adjPosAhead + vector;
		position2.x = Mathf.Max(position2.x, -52f);
		position2.x = Mathf.Min(position2.x, 52f);
		position2.z = Mathf.Max(position2.z, -39f);
		position2.z = Mathf.Min(position2.z, 39f);
		ballAhead.transform.position = position2;
		Transform transform2 = ((currentCameraMode == CameraMode.Stand) ? standTarget.transform : tvTarget.transform);
		transform2.transform.LookAt(transform);
		MatchCamera cameraFromType = MatchCameraController.instance.GetCameraFromType(CameraTypes.TV_CAM);
		if (currentCameraMode == CameraMode.Stand)
		{
			cameraFromType = MatchCameraController.instance.GetCameraFromType(CameraTypes.STAND_CAM);
		}
		Vector3 vector2 = cameraFromType.camera.WorldToViewportPoint(transform.position);
		bool flag = false;
		float num4 = 0f;
		transform2.transform.localScale = new Vector3(cameraData.cameraFOV + num, 1f, cameraData.cameraZoom + num2);
		bool flag2 = match.engineState == 106 || match.engineState == 112 || match.engineState == 63 || match.engineState == 64;
		if ((currentCameraMode == CameraMode.TV || currentCameraMode == CameraMode.Stand) && ((match.engineState >= 600 && match.nextEngineState != 607) | flag2))
		{
			if (!match.engineStateInfo.ContainsKey("penDirection"))
			{
				if (match.prevEngineStateInfo.ContainsKey("penDirection"))
				{
					penDir = (int)match.prevEngineStateInfo["penDirection"];
				}
			}
			else
			{
				penDir = (int)match.engineStateInfo["penDirection"];
			}
			switch (currentCameraMode)
			{
			case CameraMode.TV:
				currentPenaltyCam = MatchCameraController.instance.GetCameraFromType((penDir < 0) ? CameraTypes.PENALTY_RIGHT : CameraTypes.PENALTY_LEFT);
				break;
			case CameraMode.Stand:
				currentPenaltyCam = MatchCameraController.instance.GetCameraFromType((penDir < 0) ? CameraTypes.PENALTY_RIGHT_STAND : CameraTypes.PENALTY_LEFT_STAND);
				break;
			}
			transform2.transform.eulerAngles = currentPenaltyCam.camera.transform.eulerAngles;
			transform2.transform.localScale = new Vector3(currentPenaltyCam.camera.fieldOfView, 1f, 0f);
			isSpecialCameraCase = true;
			cameraTargetSpeed = 5f;
		}
		else
		{
			penaltyCamSetUp = false;
		}
		if (vector2.x < currentViewportLimits.x)
		{
			flag = true;
			num4 += Mathf.Pow(currentViewportLimits.x - vector2.x, 2f);
		}
		else if (vector2.x > 1f - currentViewportLimits.y)
		{
			flag = true;
			num4 += Mathf.Pow(vector2.x - (1f - currentViewportLimits.y), 2f);
		}
		if (vector2.y > 1f - currentViewportLimits.z)
		{
			flag = true;
			num4 += Mathf.Pow(vector2.y - (1f - currentViewportLimits.z), 2f);
		}
		else if (vector2.y < currentViewportLimits.w)
		{
			flag = true;
			num4 += Mathf.Pow(currentViewportLimits.w - vector2.y, 2f);
		}
		num4 = Mathf.Sqrt(Mathf.Max(num4, 0.01f));
		if (!isSpecialCameraCase)
		{
			if (flag)
			{
				cameraTargetSpeed = 250f * num4;
				cameraTargetSpeed = Mathf.Max(0.3f, cameraTargetSpeed);
				cameraTargetSpeed = Mathf.Min(45f, cameraTargetSpeed);
			}
			else
			{
				cameraTargetSpeed = 0f;
			}
		}
		cameraFromType.cameraMaxSpeed = Mathf.SmoothDamp(cameraFromType.cameraMaxSpeed, cameraTargetSpeed, ref cameraSpeedChange, cameraSpeedSmoothing * UnityDeltaTime);
		cameraFromType.cameraMaxSpeed = Mathf.Max(cameraFromType.cameraMaxSpeed, 0.001f);
		Vector3 position3 = panoramicTarget.transform.position;
		float f = Mathf.Clamp(Mathf.Abs(transform.position.x) / 47f, 0f, 1f);
		f = ((transform.position.x < 0f) ? (-1f) : 1f) * Mathf.Pow(f, 1.2f);
		f *= 32f;
		position3.x = f;
		panoramicTarget.transform.position = position3;
	}

	public PlayDirection CalculatePlayDirection()
	{
		if (match.playerOnBall == 0)
		{
			return PlayDirection.NONE;
		}
		if (match.teams[match.teamInPlay].goalDirection == -1)
		{
			return PlayDirection.MOVING_RIGHT_TO_LEFT;
		}
		return PlayDirection.MOVING_LEFT_TO_RIGHT;
	}

	private Vector3 SetGoalCutsceneCamPosition(v3d runStartPos, v3d pitchPos, Transform lookTarget)
	{
		Vector3 point = pitchToScreen(pitchPos);
		Vector3 vector = pitchToScreen(runStartPos);
		Vector3 vector2 = pitchToScreen(pitchPos);
		Vector3 bounds = new Vector3(57.8f, 0f, 45f);
		point -= vector;
		point.Normalize();
		point *= 10f;
		point += vector2;
		Vector3 closestPosToPointOnRect = GetClosestPosToPointOnRect(point, bounds);
		MatchCamera cameraFromType = MatchCameraController.instance.GetCameraFromType(CameraTypes.GOAL_CUTSCENE);
		cameraFromType.transform.position = closestPosToPointOnRect;
		cameraFromType.transform.LookAt(lookTarget);
		cameraFromType.transform.Find("Animated").GetComponent<Animator>().SetTrigger("Reset");
		return closestPosToPointOnRect;
	}

	private void SetCrowdLookTargetPosition(Vector3 pitchPos)
	{
		crowdLookTarget.transform.position = pitchPos + new Vector3((float)globalRand.NextDouble() * 5f, 3f, (float)globalRand.NextDouble() * 5f);
	}

	public Vector3 GetClosestPosToPointOnRect(Vector3 point, Vector3 bounds)
	{
		Vector3 result = default;
		bool num = 0f - bounds.x < point.x && point.x < bounds.x;
		bool flag = 0f - bounds.z < point.z && point.z < bounds.z;
		if (!(num & flag))
		{
			result.x = Mathf.Max(0f - bounds.x, Mathf.Min(point.x, bounds.x));
			result.z = Mathf.Max(0f - bounds.z, Mathf.Min(point.z, bounds.z));
		}
		else
		{
			Vector3 vector = bounds - point;
			Vector3 vector2 = -(-bounds - point);
			float a = Mathf.Min(vector.x, vector2.x);
			float b = Mathf.Min(vector.z, vector2.z);
			float num2 = Mathf.Min(a, b);
			if (num2 == vector.x)
			{
				result.x = bounds.x;
				result.z = point.z;
			}
			else if (num2 == vector2.x)
			{
				result.x = 0f - bounds.x;
				result.z = point.z;
			}
			else if (num2 == vector.z)
			{
				result.x = point.x;
				result.z = bounds.z;
			}
			else
			{
				result.x = point.x;
				result.z = 0f - bounds.z;
			}
		}
		return result;
	}

	public byte GetPlayerFitness(int playerID)
	{
		string key = ((seFixInfo.fixtureInfo.home.clubDataID == managedClubID) ? "home" : "away");
		foreach (cPlayer value in sm.match.teams[key].playerCache.Values)
		{
			if (value.playerSetup.id == playerID)
			{
				return (byte)Mathf.RoundToInt(value.getFitness());
			}
		}
		return Player.GetFitness(playerID);
	}

	private void RequestReplay()
	{
		PlayReplayCameraTransition();
		replayGoalLocation = match.goalPositions[match.goalPositions.Count - 1].vx > 0f;
		replayBeginTime = time + 1f;
		replayRequested = true;
	}

	private void StartReplay()
	{
		replayRequested = false;
		replayResetTime = time + 8f;
		replayEndTime = match.goalTimes[match.goalTimes.Count - 1] + 3f;
		lastReplayTime = replayEndTime;
		time = replayEndTime - 9f;
		ResetPlayerAnimations(time);
		shownTransitionAnimation = false;
		RandomiseReplayCameraAngles();
		currentReplayPhase = eMatchReplayPhase.PART_1;
	}

	public float EndReplay()
	{
		switch (currentReplayPhase)
		{
		case eMatchReplayPhase.PART_1:
		{
			float result = replayEndTime - 7f;
			ResetPlayerAnimations(result);
			shownTransitionAnimation = false;
			currentReplayPhase = eMatchReplayPhase.PART_2;
			return result;
		}
		case eMatchReplayPhase.PART_2:
			currentReplayPhase = eMatchReplayPhase.NONE;
			ResetPlayerAnimations(replayResetTime);
			return replayResetTime;
		default:
			return time;
		}
	}

	public void PlayReplayCameraTransition()
	{
		match3DUI.GetComponent<Animator>().Play("Wipe" + globalRand.Next(0, 2), 0, 0f);
		if (highlightMode != 5)
		{
			matchOverlay.GetComponent<Animator>().Play("MatchScoreShow");
		}
	}

	private void RandomiseReplayCameraAngles()
	{
		int maxValue = Enum.GetValues(typeof(eReplayCameraID)).Length;
		bool flag = false;
		while (!flag)
		{
			replayPart1Camera = (eReplayCameraID)globalRand.Next(0, maxValue);
			if (replayPart1Camera != eReplayCameraID.REPLAY_BEHIND_GOAL)
			{
				flag = true;
			}
		}
		bool flag2 = false;
		while (!flag2)
		{
			replayPart2Camera = (eReplayCameraID)globalRand.Next(0, maxValue);
			if (replayPart2Camera != replayPart1Camera && replayPart2Camera != eReplayCameraID.REPLAY_FOLLOW_BALL)
			{
				flag2 = true;
			}
		}
	}

	private void ResetPlayerAnimations(float time)
	{
		foreach (cPlayer value in match.teams["home"].playerCache.Values)
		{
			HardResetPlayerAnimators(value, time);
		}
		foreach (cPlayer value2 in match.teams["away"].playerCache.Values)
		{
			HardResetPlayerAnimators(value2, time);
		}
	}

	private void HardResetPlayerAnimators(cPlayer player, float time)
	{
		ClearAnimationPlayedFlag(player, time);
		ResetPlayerAnimationFlags(player);
		if (player.currentVisual.playerAnimator.gameObject.activeInHierarchy)
		{
			player.currentVisual.playerAnimator.Play("Movement", 0);
			player.currentVisual.playerAnimator.Play("Use Base Layer", 1);
			player.currentVisual.playerAnimator.Play("Use Base Layer", 2);
		}
	}

	private void ClearAnimationPlayedFlag(cPlayer player, float time)
	{
		int count = player.animationList.Count;
		for (int i = 0; i < count; i++)
		{
			cPlayerAnimation cPlayerAnimation2 = player.animationList[i];
			if (cPlayerAnimation2.time > time)
			{
				cPlayerAnimation2.played = false;
				if (cPlayerAnimation2.animationID == PlayerAnimation.FOULED)
				{
					SetPlayerFouledState(player, fouled: false);
				}
			}
		}
	}

	private void ResetPlayerAnimationFlags(cPlayer player)
	{
		player.celebrationRole = cPlayer.CelebrateRole.NONE;
		player.isFacingScorer = false;
		player.isUpset = false;
		player.isClapping = false;
		player.isFouled = false;
		player.isInjured = false;
		player.isHoldingBall = false;
		player.isPassing = false;
		player.isReleasingBall = false;
		player.isSentOff = false;
		player.isReadyToAct = false;
		player.isDiving = false;
		player.isSliding = false;
		player.isJumping = false;
		player.isInWall = false;
		player.isIKOff = false;
		player.hitGroundAfterDive = false;
		player.lockAngle = 0f;
		player.isCloseToBall = false;
		player.hasAnimatedLookDirection = false;
		SetPlayerFouledState(player, fouled: false);
		MakePlayerStopCelebrating(player);
		MakePlayerStopClapping(player);
		MakePlayerStopBeingUpset(player);
	}

	public void UpdateCaptainsArmbands()
	{
		currCaptain = sm.match.teams["home"].GetCaptain();
		if (homeCaptain != currCaptain)
		{
			if (homeCaptain != null)
			{
				homeCaptain.currentVisual.playerArmBand.SetActive(value: false);
			}
			homeCaptain = currCaptain;
			homeCaptain.currentVisual.playerArmBand.SetActive(value: true);
		}
		currCaptain = sm.match.teams["away"].GetCaptain();
		if (awayCaptain != currCaptain)
		{
			if (awayCaptain != null)
			{
				awayCaptain.currentVisual.playerArmBand.SetActive(value: false);
			}
			awayCaptain = currCaptain;
			awayCaptain.currentVisual.playerArmBand.SetActive(value: true);
		}
	}

	public int GetTrophyCutSceneTeam()
	{
		if (g_matchday_nextfix.finalCompetitionFixture)
		{
			_ = g_matchday_nextfix.competitiontype;
			SeFixtureInfo seFixtureInfo = instance.seFixInfo;
			short clubDataID = seFixtureInfo.fixtureInfo.home.clubDataID;
			short clubDataID2 = seFixtureInfo.fixtureInfo.away.clubDataID;
			int num = -1;
			int num2 = -1;
			for (int i = 0; i < seLiveTable.Count; i++)
			{
				if (seLiveTable[i].clubName == Club.GetName(clubDataID))
				{
					num = i;
					break;
				}
				if (seLiveTable[i].clubName == Club.GetName(clubDataID2))
				{
					num2 = i;
					break;
				}
			}
			if (num < 0 && num2 < 0)
			{
				cMatch cMatch2 = sm.match;
				int score = cMatch2.matchStats.teams["home"].score;
				int score2 = cMatch2.matchStats.teams["away"].score;
				if (score > score2)
				{
					num = 0;
				}
				else if (score < score2)
				{
					num2 = 0;
				}
				else
				{
					int num3 = cMatch2.matchStats.teams["home"].shootout;
					int num4 = cMatch2.matchStats.teams["away"].shootout;
					if (num3 > num4)
					{
						num = 0;
					}
					else if (num3 < num4)
					{
						num2 = 0;
					}
				}
			}
			if (num == 0)
			{
				return 0;
			}
			if (num2 == 0)
			{
				return 1;
			}
		}
		return -1;
	}
}
