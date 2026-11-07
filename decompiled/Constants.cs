using System;
using System.Collections.Generic;
using UnityEngine;

public static class Constants
{
	public enum SearchEnquiry
	{
		FreeAgent,
		ThreeMonths,
		SixMonths,
		TwelveMonths,
		EighteenMonths,
		TwentyFourMonths,
		All
	}

	public enum PlayerMainRole
	{
		Attack,
		Defensive,
		Both,
		GoalKeeper
	}

	[Flags]
	public enum PlayerPosition
	{
		PLAYER_POS_GK = 1,
		PLAYER_POS_D = 0xE,
		PLAYER_POS_DM = 0x70,
		PLAYER_POS_M = 0x380,
		PLAYER_POS_AM = 0x1C00,
		PLAYER_POS_F = 0xE000,
		PLAYER_POS_ANY_DEF = PLAYER_POS_D | PLAYER_POS_DM,
		PLAYER_POS_ANY_MID = PLAYER_POS_DM | PLAYER_POS_M | PLAYER_POS_AM,
		PLAYER_POS_LEFT = 0x2492,
		PLAYER_POS_CENTRE = 0x4924,
		PLAYER_POS_RIGHT = 0x9248,
		PLAYER_POS_DL = 2,
		PLAYER_POS_DC = 4,
		PLAYER_POS_DR = 8,
		PLAYER_POS_DML = 0x10,
		PLAYER_POS_DMC = 0x20,
		PLAYER_POS_DMR = 0x40,
		PLAYER_POS_ML = 0x80,
		PLAYER_POS_MC = 0x100,
		PLAYER_POS_MR = 0x200,
		PLAYER_POS_AML = 0x400,
		PLAYER_POS_AMC = 0x800,
		PLAYER_POS_AMR = 0x1000,
		PLAYER_POS_FL = 0x2000,
		PLAYER_POS_FC = 0x4000,
		PLAYER_POS_FR = 0x8000
	}

	[Flags]
	public enum TacticChange
	{
		NONE = 0,
		DEFENSIVE = 1,
		ATTACKING = 2,
		SWAPPLAYER = 4,
		SUB = 8,
		HUMAN_INITIATED = 0x10
	}

	public enum MatchOutcome
	{
		Won,
		Lost,
		Drawn
	}

	public enum eScreenGroup
	{
		HOME_SCREEN,
		SQUAD_SCREEN,
		MANAGER_SCREEN,
		FINANCE_SCREEN,
		TACTICS_SCREEN,
		YOUTH_ACADEMY_SCREEN,
		SCOUTING_SCREEN,
		TRAINING_SCREEN,
		TRANSFERS_SCREEN,
		CLUBZONE_SCREEN,
		LEAGUE_SCREEN,
		DOMESTICCUP_SCREEN,
		CONTINENTALCUP_SCREEN,
		SHOP_SCREEN,
		SCHEDULES_SCREEN,
		SEARCHES_SCREEN
	}

	public enum OverlayElement
	{
		SIDE_BAR,
		TOP_BAR,
		BOTTOM_BAR,
		MODAL_CONTROLLER
	}

	public enum ScreenBottomBars
	{
		BOTTOM_BAR,
		MSG_BOTTOM_BAR,
		LEAGUE_TABLES_BOTTOM_BAR,
		PLAYER_PROFILE_BOTTOM_BAR,
		SEARCH_BOTTOM_BAR,
		SQUAD_BOTTOM_BAR,
		UNMANAGED_CLUB_BOTTOM_BAR,
		UNMANAGED_PLAYER_PROFILE_BOTTOM_BAR
	}

	public enum TopBarObjects
	{
		CLUB_COLOUR,
		OPP_CLUB_COLOUR,
		CLUB_LOGO,
		OPP_CLUB_LOGO,
		CLUB_NAME,
		OPP_CLUB_NAME
	}

	public const string AuthType = "Steam";

	public const PlatformType PLATFORM_TYPE = PlatformType.Steam;

	public const bool IS_DESKTOP = true;

	public const bool DEVELOPMENT_MODE = false;

	public const bool LOGGING_ENABLED = false;

	public static string ASSET_BUNDLE_BUILD_TYPE = "Steam";

	public static bool ZULU_MODE = false;

	public static bool LIVE_SCORES_ENABLED = true;

	public static bool IS_BETA = false;

	public static bool BETA_BUG_REPORTING_ENABLED = false;

	public static bool FULL_SHOP_ENABLED = true;

	public static bool BOOSTS_ENABLED = true;

	public static bool FACEBOOK_ENABLED = true;

	public const bool STEAM_DEBUG = false;

	public const bool USE_YEN_FOR_CHINA_BUILD = false;

	public const bool CHINA_TEST_BUILD = false;

	public const string APPLICATION_ID_ANDROID_SOCCER_MANAGER = "com.soccermanagerltd.soccermanager2021";

	public const string APPLICATION_ID_IOS_SOCCER_MANAGER = "Soccer-Manager-Ltd.Soccer-Manager-2020";

	public const string APPLICATION_ID_ANDROID_CRAZY_SPORTS = "com.crazysports.sm20";

	public const string APPLICATION_ID_IOS_CRAZY_SPORTS = "com.crazysports.sm20";

	public const string CHINA_AWS_SP_URL = "https://chinaapi.soccermanager2020.com/";

	public const string CHINA_AWS_DEV_SP_URL = "https://devchinaapi.soccermanager2020.com/";

	public const string WEST_AWS_SP_URL = "https://api.soccermanager2021.com/";

	public const string WEST_AWS_DEV_SP_URL = "https://dev-api.soccermanager2021.com/";

	public const string WEST_AWS_SP_DATAPACKS_URL = "https://api.footballdatapacks.com/";

	public const string WEST_AWS_DEV_SP_DATAPACKS_URL = "https://dev-api.footballdatapacks.com/";

	public const string DATAPACKS_UPLOAD_STRING = "v1/UploadDatapack";

	public const string DATAPACKS_UPLOADURL_STRING = "CreateDatapackURL";

	public const string DATAPACKS_RETRIEVE_STRING = "v1/RetrieveDatapackList";

	public const string DATAPACKS_SEARCH_STRING = "v1/SearchDatapacks";

	public const string SERVER_CAREERHISTORY_SAVE_STRING = "SaveCareerHistoryEntry";

	public const string SERVER_CAREERHISTORY_RETRIEVE_STRING = "RetrieveUserCareerHistory";

	public const string SERVER_DAILYLOGINREWARD_CLAIM_STRING = "ClaimDailyLoginReward";

	public const string SERVER_PREORDERREWARD_CLAIM = "GiveReleaseDayRewards";

	public const string SERVER_RETRIEVE_SUPPORTTICKETS = "RetrieveSupportTickets";

	public const string SERVER_SEND_SUPPORTTICKET = "CreateSupportTicket";

	public const string SERVER_SEND_SUPPORTTICKET_REPLY = "SendReply";

	public const string SERVER_SEND_FEEDBACK = "SubmitFeedback";

	public const string SERVER_GET_S3SIGNEDURL = "GetS3SignedURL";

	public const string SERVER_SEND_CRASHREPORT = "CreateCrashReport";

	public const string SERVER_BUY_NONIAP_ITEM = "BuyNonIapItems";

	public const string SERVER_SEND_TICKET_FAILEDPURCHASERECOVER = "CreateFailedPayment";

	public const string SERVER_STEAM_INIT_TRANSACATION = "InitialiseSteamPayment";

	public const string SERVER_STEAM_FINAILSE_TRANSACTION = "FinaliseSteamPayment";

	public const string SERVER_MANAGERLEVELUP_STRING = "LevelUpManager";

	public const string SERVER_CONSUMEITEMS_STRING = "ConsumeItems";

	public const string SERVER_NEWSFEED_RETRIEVEDATA = "RetrieveNewsfeedByLanguage";

	public const string SERVER_FREEPACK_GIVE = "GiveFreePack";

	public const string SERVER_AUTHFIREBASE_STRING = "AuthenticateUser";

	public const string SERVER_RETRIEVEUSERDATA_STRING = "RetrieveUserData";

	public const string SERVER_SAVEUSERDATA_STRING = "SaveUserManagerProfile";

	public const string PREFS_LOGIN_ATTEMPT_DECLINED = "prefsloginattemptdeclined";

	public const AndroidChinaBuild CHINA_BUILD_TYPE = AndroidChinaBuild.quicksdk;

	public const string SP_URL = "https://api.soccermanager2021.com/";

	public const string SP_URL_Datapack = "https://api.footballdatapacks.com/";

	public const string SP_URL_DATAPACKS_UPLOAD = "https://api.footballdatapacks.com/v1/UploadDatapack";

	public const string SP_URL_DATAPACKS_RETRIEVE = "https://api.footballdatapacks.com/v1/RetrieveDatapackList";

	public const string SP_URL_DATAPACKS_SEARCH = "https://api.footballdatapacks.com/v1/SearchDatapacks";

	public const string SP_URL_DATAPACKS_UPLOADURL = "https://api.soccermanager2021.com/CreateDatapackURL";

	public const string SP_URL_MANAGER_LEVELUP = "https://api.soccermanager2021.com/LevelUpManager";

	public const string SP_URL_NEWSFEEDATA_RETRIEVE = "https://api.soccermanager2021.com/RetrieveNewsfeedByLanguage";

	public const string SP_URL_GIVEFREEPACK = "https://api.soccermanager2021.com/GiveFreePack";

	public const string SP_URL_DAILYLOGIN_REWARD_CLAIM = "https://api.soccermanager2021.com/ClaimDailyLoginReward";

	public const string SP_URL_PREORDER_REWARD_CLAIM = "https://api.soccermanager2021.com/GiveReleaseDayRewards";

	public const string SP_URL_RETRIEVE_SUPPORT_TICKETS = "https://api.soccermanager2021.com/RetrieveSupportTickets";

	public const string SP_URL_SEND_SUPPORT_TICKET = "https://api.soccermanager2021.com/CreateSupportTicket";

	public const string SP_URL_SEND_SUPPORT_TICKET_REPLY = "https://api.soccermanager2021.com/SendReply";

	public const string SP_URL_SEND_FEEDBACK = "https://api.soccermanager2021.com/SubmitFeedback";

	public const string SP_URL_GET_S3SIGNEDURL = "https://api.soccermanager2021.com/GetS3SignedURL";

	public const string SP_URL_SEND_CRASHREPORT = "https://api.soccermanager2021.com/CreateCrashReport";

	public const string SP_URL_BUY_NONIAP_ITEM = "https://api.soccermanager2021.com/BuyNonIapItems";

	public const string SP_URL_SEND_FAILEDRECOVER_PURCHASE_TICKET = "https://api.soccermanager2021.com/CreateFailedPayment";

	public const string SP_URL_STEAM_INIT_TRANSACATION = "https://api.soccermanager2021.com/InitialiseSteamPayment";

	public const string SP_URL_STEAM_FINAILSE_TRANSACTION = "https://api.soccermanager2021.com/FinaliseSteamPayment";

	public const string SP_URL_CAREERHISTORY_RETRIEVE = "https://api.soccermanager2021.com/RetrieveUserCareerHistory";

	public const string SP_URL_CAREERHISTORY_SAVE = "https://api.soccermanager2021.com/SaveCareerHistoryEntry";

	public const string SP_URL_SERVER_CONSUMEITEM = "https://api.soccermanager2021.com/ConsumeItems";

	public const string SP_URL_SERVER_AUTH = "https://api.soccermanager2021.com/AuthenticateUser";

	public const string SP_URL_SERVER_RETRIEVEDATA = "https://api.soccermanager2021.com/RetrieveUserData";

	public const string SP_URL_SERVER_SAVEDATA = "https://api.soccermanager2021.com/SaveUserManagerProfile";

	public static Dictionary<string, string> AVAILABLE_LANGUAGES = new Dictionary<string, string>
	{
		{ "en_EN", "English" },
		{ "es_ES", "Español" },
		{ "fr_FR", "Français" },
		{ "de_DE", "Deutsch" },
		{ "id_ID", "Indonesian" },
		{ "it_IT", "Italiano" },
		{ "nl_NL", "Nederlands" },
		{ "pt_PT", "Português" },
		{ "tr_TR", "Türkçe" },
		{ "ar_EG", "العربية" },
		{ "zh_CN", "中文 (简体)" },
		{ "zh_TW", "中文 (繁體)" },
		{ "ja_JP", "日本語" },
		{ "ko_KR", "한국어" },
		{ "ru_RU", "русский" }
	};

	public static Dictionary<SystemLanguage, string> DEFAULT_LANGUAGE = new Dictionary<SystemLanguage, string>
	{
		{
			SystemLanguage.Arabic,
			"ar_EG"
		},
		{
			SystemLanguage.Spanish,
			"es_ES"
		},
		{
			SystemLanguage.French,
			"fr_FR"
		},
		{
			SystemLanguage.German,
			"de_DE"
		},
		{
			SystemLanguage.Indonesian,
			"id_ID"
		},
		{
			SystemLanguage.Italian,
			"it_IT"
		},
		{
			SystemLanguage.Dutch,
			"nl_NL"
		},
		{
			SystemLanguage.Portuguese,
			"pt_PT"
		},
		{
			SystemLanguage.Turkish,
			"tr_TR"
		},
		{
			SystemLanguage.Japanese,
			"ja_JP"
		},
		{
			SystemLanguage.Korean,
			"ko_KR"
		},
		{
			SystemLanguage.Russian,
			"ru_RU"
		},
		{
			SystemLanguage.Chinese,
			"zh_CN"
		},
		{
			SystemLanguage.ChineseSimplified,
			"zh_CN"
		},
		{
			SystemLanguage.ChineseTraditional,
			"zh_TW"
		}
	};

	public static Dictionary<string, string> AVAILABLE_LANGUAGES_ISO391 = new Dictionary<string, string>
	{
		{ "en", "English" },
		{ "es", "Español" },
		{ "fr", "Français" },
		{ "de", "Deutsch" },
		{ "id", "Indonesian" },
		{ "it", "Italiano" },
		{ "nl", "Nederlands" },
		{ "pt", "Português" },
		{ "tr", "Türkçe" },
		{ "ar", "العربية" },
		{ "zh", "中文 (简体)" },
		{ "ja", "日本語" },
		{ "ko", "한국어" },
		{ "ru", "русский" }
	};

	public const int FreePackWaitTimeHours = 4;

	public static DateTime UnixEpochTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);

	public const string CONST_WEST_GAME_NAME_LONG = "Soccer Manager 2021";

	public const string CONST_CHINA_GAME_NAME_CHINESE = "Soccer Manager 2021";

	public const string APPLE_STORE_LINK = "https://itunes.apple.com/app/soccer-manager-2021/id1499328884";

	public const bool IS_CHINA_BUILD = false;

	public const string DATA_BUNDLE_WEB_ADDRESS = "http://static1.szb.soccermanager.net/scene";

	public const bool SHOW_OVERALL_RATING = true;

	public const string IOS_HAS_DEVICE_ID = "ioshasdeviceid1234";

	public const string IOS_KEYCHAIN_PLATFORM_ID = "ioskeychainplatformid1234";

	public const string IOS_KEYCHAIN_PLATFORM_NAME = "ioskeychainplatformname1234";

	public const int MAX_SEED = 32768;

	public const string PLAYER_USERNAME_KEY = "playerusernamekey1234";

	public const string HAS_BOUGHT_LIMITED_OFFER = "hasboughtlimitedoffer";

	public const string HAS_ACCEPTED_TARGETTED_ADS = "hasacceptedtargetedadsnew";

	public const string PREFS_DOB_MONTH = "prefsdobmonth1";

	public const string PREFS_DOB_YEAR = "prefsdobyear1";

	public const string FIRSTGAMEWONPREF = "FirstGameWonPref";

	public const int MANAGER_NAME_SIZE_LIMIT = 20;

	public const string PlayerPrefGeneratedUid = "PlayerPrefGeneratedUid";

	public const string VERSION_NUMBER = "DEV";

	public const string PREFS_ANALYTICS_ACQUIRE_INTERSTITIAL_ADVERT = "analyticsAcquireInterstitialAdvert";

	public const string IRONSRC_ANDROID = "b419adbd";

	public const string IRONSRC_IOS = "b41b06ed";

	public const bool CHINA_ANDROID_RESTRICTED_VERSION = false;

	public const bool CHINA_ANDROID_DISABLE_PAYMENTS = false;

	public const string CHINA_ANDROID_PAYMENT_STRING = "亲爱的玩家，本次测试为不计费测试，\n正式公测即可体验全部功能，敬请期待！";

	public const long FACEBOOK_ID = 22686820887L;

	public const byte SUPPORT_TICKET_CATEGORY_USER_SUBMITTED = 1;

	public const byte SUPPORT_TICKET_CATEGORY_AUTO_SUBMITTED = 2;

	public const string GLOBAL_DATA_FILENAME = "unity_assets";

	public const string GLOBAL_DATA_TAG_SMCREDITS = "polygon_count";

	public const short CONST_CURRENCY_GBP = 1;

	public const short CONST_CURRENCY_EURO = 2;

	public const short CONST_CURRENCY_DOLLAR = 3;

	public const short CONST_CURRENCY_YUAN = 4;

	public static readonly Dictionary<short, string> CONST_CURRENCY_SYMBOLS = new Dictionary<short, string>
	{
		{ 1, "£" },
		{ 2, "€" },
		{ 3, "$" }
	};

	public static readonly Dictionary<short, double> CONST_CURRENCY_CONVERSION = new Dictionary<short, double>
	{
		{ 1, 1.0 },
		{ 2, 1.1 },
		{ 3, 1.5 }
	};

	public static readonly Dictionary<short, string> CONST_CURRENCY_SYMBOLS_CHINA = new Dictionary<short, string>
	{
		{ 4, "¥" },
		{ 3, "$" }
	};

	public static readonly Dictionary<short, double> CONST_CURRENCY_CONVERSION_CHINA = new Dictionary<short, double>
	{
		{ 4, 9.07 },
		{ 3, 1.5 }
	};

	public const string DEFAULT_MANAGER_NAME = "Mr. Manager";

	public const bool DEBUG_LOG = true;

	public const byte DEBUG_SET_RESULT = 0;

	public const bool SCROLL_TO_TOP_ENABLED = true;

	public const string ZULU_STRING = "xxxx";

	public const float MUSIC_VOLUME = 0.12f;

	public const float MATCH_SOUNDS_VOLUME = 0.2f;

	public const string BUTTON_ON_COLOUR = "009D05FF";

	public const string BUTTON_OFF_COLOUR = "8B0000FF";

	public const string BUTTON_ON_SIDEBAR = "00EAFF";

	public const string PENDING_PURCHASE_CUSTOMER_ID = "PENDING_PURCHASE_CUSTOMER_ID";

	public const byte PLAYER_BOOST_COST_HEAL = 50;

	public const byte PLAYER_BOOST_COST_FITNESS = 25;

	public const byte PLAYER_BOOST_COST_MORALE = 25;

	public const byte PLAYER_BOOST_COST_SCOUT = 100;

	public const byte PLAYER_BOOST_COST_COACH_REPORT_1 = 180;

	public const byte PLAYER_BOOST_COST_COACH_REPORT_2 = 60;

	public const byte PLAYER_BOOST_COST_COACH_REPORT_3 = 30;

	public const string CONST_PLAYER_BOOST_HEAL = "heal";

	public const string CONST_PLAYER_BOOST_FITNESS = "fitness";

	public const string CONST_PLAYER_BOOST_MORALE = "morale";

	public const string CONST_PLAYER_BOOST_SUSPENSION = "suspension";

	public const string CONST_PLAYER_BOOST_CHEMISTRY = "chemistry";

	public const short RESET_FACILITIES_COST = 500;

	public const string DATA_FILE_NAME = "DataFile-2019-03";

	public const string FAKE_NAME_PACK_FILE_NAME_11 = "SoccerWiki_2020-07-17_1594975435";

	public const string DFL_PACK_FILE_NAME = "dfl_datapack";

	public const string SAVE_GAME_CONTAINER = "https://storage.googleapis.com/save_games/";

	public static byte CONST_MAX_NUM_COUNTRIES_SELECTABLE = 1;

	public const string ACHIEVEMENT_1_0 = "ACHIEVEMENT_1_0";

	public const string ACHIEVEMENT_1_1 = "ACHIEVEMENT_1_1";

	public const string ACHIEVEMENT_1_2 = "ACHIEVEMENT_1_2";

	public const string ACHIEVEMENT_1_3 = "ACHIEVEMENT_1_3";

	public const string ACHIEVEMENT_1_4 = "ACHIEVEMENT_1_4";

	public const string ACHIEVEMENT_1_5 = "ACHIEVEMENT_1_5";

	public const string ACHIEVEMENT_1_6 = "ACHIEVEMENT_1_6";

	public const string ACHIEVEMENT_1_7 = "ACHIEVEMENT_1_7";

	public const string ACHIEVEMENT_1_8 = "ACHIEVEMENT_1_8";

	public const string ACHIEVEMENT_1_9 = "ACHIEVEMENT_1_9";

	public const string ACHIEVEMENT_1_10 = "ACHIEVEMENT_1_10";

	public const string ACHIEVEMENT_1_11 = "ACHIEVEMENT_1_11";

	public const string ACHIEVEMENT_1_12 = "ACHIEVEMENT_1_12";

	public const string ACHIEVEMENT_1_13 = "ACHIEVEMENT_1_13";

	public const string ACHIEVEMENT_1_14 = "ACHIEVEMENT_1_14";

	public const string ACHIEVEMENT_1_15 = "ACHIEVEMENT_1_15";

	public const string ACHIEVEMENT_1_16 = "ACHIEVEMENT_1_16";

	public const string ACHIEVEMENT_1_17 = "ACHIEVEMENT_1_17";

	public const string ACHIEVEMENT_1_18 = "ACHIEVEMENT_1_18";

	public const string ACHIEVEMENT_1_19 = "ACHIEVEMENT_1_19";

	public const string ACHIEVEMENT_1_20 = "ACHIEVEMENT_1_20";

	public const string ACHIEVEMENT_1_21 = "ACHIEVEMENT_1_21";

	public const string ACHIEVEMENT_1_22 = "ACHIEVEMENT_1_22";

	public const string ACHIEVEMENT_1_23 = "ACHIEVEMENT_1_23";

	public const string ACHIEVEMENT_1_24 = "ACHIEVEMENT_1_24";

	public const string ACHIEVEMENT_1_25 = "ACHIEVEMENT_1_25";

	public const string ACHIEVEMENT_1_26 = "ACHIEVEMENT_1_26";

	public const string ACHIEVEMENT_1_27 = "ACHIEVEMENT_1_27";

	public const string ACHIEVEMENT_1_28 = "ACHIEVEMENT_1_28";

	public const string ACHIEVEMENT_1_29 = "ACHIEVEMENT_1_29";

	public const string ACHIEVEMENT_1_30 = "ACHIEVEMENT_1_30";

	public const string ACHIEVEMENT_1_31 = "ACHIEVEMENT_1_31";

	public const string ACHIEVEMENT_1_32 = "ACHIEVEMENT_1_32";

	public const string ACHIEVEMENT_1_33 = "ACHIEVEMENT_1_33";

	public const string ACHIEVEMENT_1_34 = "ACHIEVEMENT_1_34";

	public const string ACHIEVEMENT_1_35 = "ACHIEVEMENT_1_35";

	public const string ACHIEVEMENT_1_36 = "ACHIEVEMENT_1_36";

	public const string ACHIEVEMENT_1_37 = "ACHIEVEMENT_1_37";

	public const string ACHIEVEMENT_1_38 = "ACHIEVEMENT_1_38";

	public const string ACHIEVEMENT_1_39 = "ACHIEVEMENT_1_39";

	public const string ACHIEVEMENT_1_40 = "ACHIEVEMENT_1_40";

	public const string ACHIEVEMENT_1_41 = "ACHIEVEMENT_1_41";

	public const string ACHIEVEMENT_1_42 = "ACHIEVEMENT_1_42";

	public const string ACHIEVEMENT_1_43 = "ACHIEVEMENT_1_43";

	public const string ACHIEVEMENT_1_44 = "ACHIEVEMENT_1_44";

	public const string ACHIEVEMENT_1_45 = "ACHIEVEMENT_1_45";

	public const string ACHIEVEMENT_1_46 = "ACHIEVEMENT_1_46";

	public const string ACHIEVEMENT_1_47 = "ACHIEVEMENT_1_47";

	public const string ACHIEVEMENT_1_48 = "ACHIEVEMENT_1_48";

	public const string ACHIEVEMENT_1_49 = "ACHIEVEMENT_1_49";

	public const string PREFS_LOCAL_PLATFORM_CID = "prefscid";

	public const string PREFS_LOCAL_PLATFORM_USERNAME = "prefusername";

	public const string PREFS_GUEST_KEY = "prefguestkey";

	public const string PREFS_SESSION_ID = "prefssessionid";

	public const string PREFS_SERVER_PLATFORM_USERNAME = "prefserverplatusername";

	public const string PREFS_SERVER_LANG = "prefsserverlang";

	public const string PREFS_SERVER_REGISTERED = "prefserverregistered";

	public const string PREFS_SERVER_NUM_INTER_TICKETS = "prefnumintertickets";

	public const string PREFS_SERVER_INTER_PASS_EXPIRE_DATE = "prefinterpassexpire";

	public const string PREFS_SERVER_CURR_INT_START_DATE = "prefcurrintergamestartdate";

	public const string PREFS_SERVER_AD_INTERVAL = "prefadinterval";

	public const string PREFS_HIGHLIGHT_MODE = "HighlightMode_1.0.5";

	public const string PREFS_LAST_HIGHLIGHT_MODE = "LastHighlightMode_1.0.5";

	public const string PREFS_MATCH_SPEED = "MatchSpeed";

	public const string PREFS_LAST_MATCH_SPEED = "LastMatchSpeed";

	public const string PREFS_MATCH_VIEW = "MatchViewSetting";

	public const string PREFS_HIGHLIGHT_VIEW = "HighlightViewSetting";

	public const string PREFS_REPLAYS_ENABLED = "ReplayEnabledSetting";

	public const string PREFS_MATCH_CAMERA_MODE = "MatchCameraModeSetting";

	public const string PREFS_MATCH_QUICK_GOALS = "MatchQuickGoals";

	public const string PREFS_MATCH_QUICK_REPLAYS = "MatchQuickReplays";

	public const string PREFS_MATCH_SHOW_STATS_WHEN_PAUSED = "MatchPauseStats";

	public const string PREF_LANG = "PrefLang";

	public const string PREFS_FACEBOOK_USER_ID = "preffacebookid";

	public const string PREFS_PREVIOUS_FACEBOOK_USER_ID = "prefpreviousfacebookid";

	public const string PREFS_VIP = "prefvip";

	public const string PREFS_CUSTOMER_ID = "Customer_AppsFlyer_ID";

	public const string PREFS_SERVER_SAVE_SLOTS = "prefserversaveslots";

	public const string PREFS_ONE_SDK_PURCHASE_NAME = "onesdkpurchasename";

	public const string PREFS_HAS_SEEN_CLOUD_SAVE_WARNING = "prefhasseencloudsavewarning";

	public const string PREFS_HAS_MADE_FIRST_PURCHASE = "prefhasmadefirstpurchase";

	public const string PREFS_PENDING_PURCHASE = "pendingpurchase";

	public const string PREFS_SERVER_COUNTRY_CODE = "prefcountrycode";

	public const string PREFS_MATCHES_WITHOUT_REWARD_AD = "matcheswithoutrewardad";

	public const string PREFS_REWARD_ADS_WATCHED = "rewardadswatched";

	public const string PREFS_UNIQUE_SAVES = "uniquesaves";

	public const string PREFS_PUSH_NOTIFICATIONS_REGISTRATIONID = "prefpushregid";

	public const string PREFS_PUSH_NOTIFICATIONS_SERVER_SENT = "prefpushserversent";

	public const string PREFS_PUSH_NOTIFICATIONS_USERID = "prefpushuserid";

	public const string PREFS_PUSH_NOTIFICATIONS_PROVIDER_NAME = "prefpushprovidername";

	public const string PREFS_ANALYTICS_MATCH_COMPLETE = "prefanalyticsmatchcomplete";

	public const string PREFS_ANALYTICS_FACEBOOK_LOGIN = "prefanalyticsfacebooklogin";

	public const string PREFS_ANALYTICS_VISITED_SHOP = "prefanalyticsvisitedshop";

	public const string PREFS_ANALYTICS_VISITED_SHOP_SPOTLIGHT = "prefanalyticsvisitedshopspotlight";

	public const string PREFS_ANALYTICS_VISITED_FACILITIES = "prefanalyticsvisitedfacilities";

	public const string PREFS_ANALYTICS_RUN_TRAINING = "prefanalyticsruntraining";

	public const string PREFS_ANALYTICS_RUN_SCOUT = "prefanalyticsrunscout";

	public const string PREFS_ANALYTICS_RUN_STADIUM_BUILDING = "prefanalyticsrunstadiumbuilding";

	public const string PREFS_ANALYTICS_SPENT_CLUB_POINTS = "prefanalyticsspentclubpoints";

	public const string PREFS_ANALYTICS_COMPLETED_TRANSFER = "prefanalyticscompletedtransfer";

	public const string PREFS_ANALYTICS_SIGNED_PLAYER = "prefanalyticssignedplayer";

	public const string PREFS_DATE_VISTED_TRAINING = "prefdatevisitedtraining";

	public const string PREFS_ASSET_BUNDLE_MATCH_SCENE_18_VERSION_ON_DEVICE = "prefsassetbundlematchscene18versionondevice";

	public const string PREFS_ASSET_BUNDLE_MATCH_SCENE_19_VERSION_ON_DEVICE = "prefsassetbundlematchscene19versionondevice";

	public const string PREFS_ASSET_BUNDLE_GENERATED_SCENE_18_VERSION_ON_DEVICE = "prefsassetbundlegeneratedscene18versionondevice";

	public const string PREFS_ASSET_BUNDLE_GENERATED_SCENE_19_VERSION_ON_DEVICE = "prefsassetbundlegeneratedscene19versionondevice";

	public const string PREFS_ASSET_BUNDLE_PREFABS_18_VERSION_ON_DEVICE = "prefsassetbundleprefabs18versionondevice";

	public const string PREFS_ASSET_BUNDLE_PREFABS_19_VERSION_ON_DEVICE = "prefsassetbundleprefabs1versionondevice";

	public const string PREFS_SERVER_AUTH_CODE = "prefgoogleserverauthcode";

	public const string PREFS_CURRENCY = "prefcurrency";

	public const string PREFS_SESSIONS = "sessions";

	public const string PREFS_GAMES_PLAYED = "gamesplayed";

	public const string PREFS_ADS_WATCHED = "adswatches";

	public const string PREFS_PURCHASES_MADE = "purchasesmade";

	public const string PREFS_PROMO_ADS_TRACKER = "prefpromoadverts";

	public static string ASSET_BUNDLE_LATEST_VERSION_MATCH_SCENE_SM18 = null;

	public static string ASSET_BUNDLE_LATEST_VERSION_GENERATED_SCENE_SM18 = null;

	public static string ASSET_BUNDLE_LATEST_VERSION_PREFABS_SM18 = null;

	public static string ASSET_BUNDLE_LATEST_VERSION_PREFABS_SM19 = null;

	public static string ASSET_BUNDLE_LATEST_VERSION_MATCH_SCENE_SM19 = null;

	public static string ASSET_BUNDLE_LATEST_VERSION_GENERATED_SCENE_SM19 = null;

	public const string PREFABS_18_BUNDLE_NAME = "prefabs_18";

	public const string PREFABS_19_BUNDLE_NAME = "prefabs_19";

	public const string MATCH_18_BUNDLE_NAME = "match_18";

	public const string MATCH_19_BUNDLE_NAME = "match_19";

	public const string GENERATED_SCENE_18_BUNDLE_NAME = "sp_generated_scene_18";

	public const string GENERATED_SCENE_19_BUNDLE_NAME = "sp_generated_scene_19";

	public const string CLUB_HOME_SCREEN_NAME = "HomeScreenGroup";

	public const byte SessionsBeforeInterstitial = 1;

	public const byte MatchesBeforeInterstitial = 1;

	public const byte RewardAdsInterstitial = 4;

	public const string PREF_HAS_SCHEDULED_NOTIFICATIONS = "HasScheduledNotifications";

	public const char SECTION_DELIMITER = '\u0001';

	public const char ROW_DELIMITER = '\u0002';

	public const char FIELD_DELIMITER = '\u0003';

	public const char SAVE_SPACER = '│';

	public const byte MAX_WEB_REQUEST_ATTEMPTS = 2;

	public const int FACEBOOK_CONNECT_NUM_SLOTS_REWARD = 2;

	public const byte FACEBOOK_CONNECT_NUM_CREDITS_REWARD = 250;

	public const byte CONST_COUNTRYDATA_SECTION = 0;

	public const byte CONST_LEAGUEDATA_SECTION = 1;

	public const byte CONST_CLUBDATA_SECTION = 2;

	public const byte CONST_STADIUMDATA_SECTION = 3;

	public const byte CONST_PLAYERDATA_SECTION = 4;

	public const byte CONST_PLAYERCLUBDATA_SECTION = 5;

	public const byte CONST_CLUBRIVALSDATA_SECTION = 6;

	public const byte CONST_MANAGERDATA_SECTION = 7;

	public const byte CONST_PLAYERATTRIBUTESRANK_SECTION = 8;

	public const byte CONST_CONTINENTALCOMPNAMES_SECTION = 9;

	public const byte CONST_CONTINENTALCOMPENTRANTS_SECTION = 10;

	public const byte CONST_CONTINENTALCOMPSTADIUM_SECTION = 11;

	public const byte CONST_CUPDATA_LAST_SEASON_WINNERS_SECTION = 12;

	public const byte CONST_CUPDATA_SECTION = 13;

	public const byte CONST_CUPVENUES_SECTION = 14;

	public const byte CONST_GAMETIMES_SECTION = 15;

	public const byte CONST_SUPERCUP_ENTRANTS_SECTION = 16;

	public const byte CONST_UNPLAYABLE_LEAGUEDATA_SECTION = 17;

	public const int CONST_PUSHTYPYE_SENDGIFT = 1;

	public const bool CONTINENTAL_COMPS_ENABLED = true;

	public const bool SUSPENSIONS_ENABLED = true;

	public const bool INJURIES_ENABLED = true;

	public const bool PLAYER_RETIREMENT_ENABLED = true;

	public const bool RESTING_PLAYERS_ENABLED = true;

	public const bool CONST_TRANSFERS_ENABLED = true;

	public const bool CONST_STADIUM_BUILDER_ENABLED = true;

	public const bool MANUAL_SAVE_ONLY = false;

	public static readonly Dictionary<int, ManagerLevelReward> CONST_MANAGER_LEVEL_REWARDS = new Dictionary<int, ManagerLevelReward>
	{
		{
			1,
			new ManagerLevelReward
			{
				ClubPoints = 5,
				Cash = 0,
				Credits = ManagerLevelConfig.ManagerLevel1CreditReward,
				Boosts = new int[4]
				{
					ManagerLevelConfig.ManagerLevel1HealBoostReward,
					ManagerLevelConfig.ManagerLevel1FitnessBoostReward,
					ManagerLevelConfig.ManagerLevel1MoraleBoostReward,
					ManagerLevelConfig.ManagerLevel1SuspensionReward
				}
			}
		},
		{
			2,
			new ManagerLevelReward
			{
				ClubPoints = 5,
				Cash = 0,
				Credits = ManagerLevelConfig.ManagerLevel2CreditReward,
				Boosts = new int[4]
				{
					ManagerLevelConfig.ManagerLevel2HealBoostReward,
					ManagerLevelConfig.ManagerLevel2FitnessBoostReward,
					ManagerLevelConfig.ManagerLevel2MoraleBoostReward,
					ManagerLevelConfig.ManagerLevel2SuspensionReward
				}
			}
		},
		{
			3,
			new ManagerLevelReward
			{
				ClubPoints = 5,
				Cash = 0,
				Credits = ManagerLevelConfig.ManagerLevel3CreditReward,
				Boosts = new int[4]
				{
					ManagerLevelConfig.ManagerLevel3HealBoostReward,
					ManagerLevelConfig.ManagerLevel3FitnessBoostReward,
					ManagerLevelConfig.ManagerLevel3MoraleBoostReward,
					ManagerLevelConfig.ManagerLevel3SuspensionReward
				}
			}
		},
		{
			4,
			new ManagerLevelReward
			{
				ClubPoints = 10,
				Cash = 0,
				Credits = ManagerLevelConfig.ManagerLevel4CreditReward,
				Boosts = new int[4]
				{
					ManagerLevelConfig.ManagerLevel4HealBoostReward,
					ManagerLevelConfig.ManagerLevel4FitnessBoostReward,
					ManagerLevelConfig.ManagerLevel4MoraleBoostReward,
					ManagerLevelConfig.ManagerLevel4SuspensionReward
				}
			}
		},
		{
			5,
			new ManagerLevelReward
			{
				ClubPoints = 10,
				Cash = 0,
				Credits = ManagerLevelConfig.ManagerLevel5CreditReward,
				Boosts = new int[4]
				{
					ManagerLevelConfig.ManagerLevel5HealBoostReward,
					ManagerLevelConfig.ManagerLevel5FitnessBoostReward,
					ManagerLevelConfig.ManagerLevel5MoraleBoostReward,
					ManagerLevelConfig.ManagerLevel5SuspensionReward
				}
			}
		},
		{
			6,
			new ManagerLevelReward
			{
				ClubPoints = 15,
				Cash = 0,
				Credits = ManagerLevelConfig.ManagerLevel6CreditReward,
				Boosts = new int[4]
				{
					ManagerLevelConfig.ManagerLevel6HealBoostReward,
					ManagerLevelConfig.ManagerLevel6FitnessBoostReward,
					ManagerLevelConfig.ManagerLevel6MoraleBoostReward,
					ManagerLevelConfig.ManagerLevel6SuspensionReward
				}
			}
		},
		{
			7,
			new ManagerLevelReward
			{
				ClubPoints = 15,
				Cash = 0,
				Credits = ManagerLevelConfig.ManagerLevel7CreditReward,
				Boosts = new int[4]
				{
					ManagerLevelConfig.ManagerLevel7HealBoostReward,
					ManagerLevelConfig.ManagerLevel7FitnessBoostReward,
					ManagerLevelConfig.ManagerLevel7MoraleBoostReward,
					ManagerLevelConfig.ManagerLevel7SuspensionReward
				}
			}
		},
		{
			8,
			new ManagerLevelReward
			{
				ClubPoints = 20,
				Cash = 0,
				Credits = ManagerLevelConfig.ManagerLevel8CreditReward,
				Boosts = new int[4]
				{
					ManagerLevelConfig.ManagerLevel8HealBoostReward,
					ManagerLevelConfig.ManagerLevel8FitnessBoostReward,
					ManagerLevelConfig.ManagerLevel8MoraleBoostReward,
					ManagerLevelConfig.ManagerLevel8SuspensionReward
				}
			}
		},
		{
			9,
			new ManagerLevelReward
			{
				ClubPoints = 20,
				Cash = 0,
				Credits = ManagerLevelConfig.ManagerLevel9CreditReward,
				Boosts = new int[4]
				{
					ManagerLevelConfig.ManagerLevel9HealBoostReward,
					ManagerLevelConfig.ManagerLevel9FitnessBoostReward,
					ManagerLevelConfig.ManagerLevel9MoraleBoostReward,
					ManagerLevelConfig.ManagerLevel9SuspensionReward
				}
			}
		},
		{
			10,
			new ManagerLevelReward
			{
				ClubPoints = 25,
				Cash = 0,
				Credits = ManagerLevelConfig.ManagerLevel10CreditReward,
				Boosts = new int[4]
				{
					ManagerLevelConfig.ManagerLevel10HealBoostReward,
					ManagerLevelConfig.ManagerLevel10FitnessBoostReward,
					ManagerLevelConfig.ManagerLevel10MoraleBoostReward,
					ManagerLevelConfig.ManagerLevel10SuspensionReward
				}
			}
		}
	};

	public static readonly ManagerPointAward CONST_MANAGER_REWARD_WIN_MATCH = new ManagerPointAward
	{
		ReasonTextKey = "won-game",
		ManagerPoints = 25
	};

	public static readonly ManagerPointAward CONST_MANAGER_REWARD_WON_TROPHY = new ManagerPointAward
	{
		ReasonTextKey = "CompetitionWinners",
		ManagerPoints = 100
	};

	public const byte CHANCE_GOAL = 0;

	public const byte CHANCE_SAVED = 1;

	public const byte CHANCE_MISSED = 2;

	public const byte EQ_MATCH_TM1_WIN = 0;

	public const byte EQ_MATCH_TM2_WIN = 1;

	public const byte EQ_MATCH_DRAW = 2;

	public const byte EQ_BIG_SCORE_WIN = 3;

	public const short EQ_CHANCE_RED = 330;

	public const byte EQ_CHANCE_YELLOW = 9;

	public const byte EQ_CHANCE_INJURY = 24;

	public const byte EQ_CHANCE_OWNGOAL = 240;

	public const byte EQ_CHANCE_SUB = 6;

	public const byte EQ_RED_INCREASE = 20;

	public const byte EQ_YELLOW_INCREASE = 2;

	public const byte EQ_CHANCE_PENALTY_GOAL = 25;

	public const byte EQ_CHANCE_SETPIECE_GOAL = 10;

	public const short EQ_CHANCE_CORNER_GOAL = 4000;

	public const byte EQ_CHANCE_SETPIECE_ASSIST = 10;

	public const byte EQ_CHANCE_CORNER_ASSIST = 25;

	public const byte EQ_CAT_EVENT_RED = 1;

	public const byte EQ_CAT_EVENT_YELLOW = 2;

	public const byte EQ_CAT_EVENT_INJURY = 4;

	public const byte EQ_CAT_EVENT_GOAL = 8;

	public const byte EQ_CAT_EVENT_OWNGOAL = 16;

	public const byte EQ_CAT_EVENT_SUB = 32;

	public const byte EQ_MAX_EVENTS = 91;

	public const byte CONST_TRANSFER_RETIRE_AGE = 37;

	public const int CONST_SECS_IN_DAY = 86400;

	public const int CONST_SECS_IN_YEAR = 31536000;

	public const int CONST_AVG_SECS_IN_YEAR = 31557600;

	public const int CONST_SECS_IN_WEEK = 604800;

	public const byte NUM_PLAYER_RATING_CACHE_ELEMENTS = 25;

	public const byte PLATFORM_ANDROID = 1;

	public const byte PLATFORM_IOS = 2;

	public const byte PLATFORM_OTHER = 3;

	public const byte PLATFORM_STEAM = 4;

	public const byte PLATFORM_FACEBOOK = 5;

	public static int DefensivePosMask = 127;

	public static int AttackingPosMask = 65408;

	public static int DefenceRightMask = 72;

	public static int DefenceLeftMask = 18;

	public static int MidfieldRightMask = 37440;

	public static int MidfieldCentreMask = 2336;

	public static int MidfieldLeftMask = 9360;

	public static int ForwardCentreMask = 57344;

	public static int[] outfieldPos = new int[3] { 14, 8176, 57344 };

	public static readonly byte[] CONST_FIXTURE_HOURS = new byte[13]
	{
		12, 13, 14, 15, 16, 17, 18, 19, 20, 21,
		22, 23, 24
	};

	public const byte CONST_FIXTURE_HOUR_SATURDAY = 15;

	public const byte CONST_FIXTURE_HOUR_SUNDAY = 16;

	public const byte CONST_FIXTURE_HOUR_WEEKDAY = 20;

	public const byte CONST_FACILITY_UPGRADE_PROMPT_CHANCE = 50;

	public const int CONST_LOW_BALANCE_PROMPT_CHANCE = 500;

	public const byte CONST_SCOUTS_IDLE_PROMPT_CHANCE = 50;

	public const byte CONST_TRAINING_IDLE_PROMPT_CHANCE = 50;

	public const short CONST_NUM_RATING_BANDINGS = 6;

	public const byte CONST_RATING_BANDING_60_69 = 0;

	public const byte CONST_RATING_BANDING_70_79 = 1;

	public const byte CONST_RATING_BANDING_80_84 = 2;

	public const byte CONST_RATING_BANDING_85_89 = 3;

	public const byte CONST_RATING_BANDING_90_94 = 4;

	public const byte CONST_RATING_BANDING_95_99 = 5;

	public static readonly string[] CONST_RATING_BANDING_NAMES = new string[6] { "60-69", "70-79", "80-84", "85-89", "90-94", "95-99" };

	public const byte GOAL50_RATING = 87;

	public const int CONST_STADIUM_SIZE_ACHIEVEMENT_REQUIREMENT = 100000;

	public const int CONST_SELLINGBUYING_ACHIEVEMENT_AMOUNT = 60000000;

	public const float CONST_OUTFIELD_EXPECTED_GAMES_KEY = 0.7f;

	public const float CONST_OUTFIELD_EXPECTED_GAMES_FIRST_TEAM = 0.55f;

	public const float CONST_OUTFIELD_EXPECTED_GAMES_SQUAD_ROTATION = 0.35f;

	public const float CONST_OUTFIELD_EXPECTED_GAMES_SQUAD_BACKUP = 0.15f;

	public const float CONST_OUTFIELD_EXPECTED_GAMES_MIN_SENIOR = 0.1f;

	public const float CONST_OUTFIELD_EXPECTED_GAMES_MIN_GRADUATE = 0.05f;

	public const float CONST_GOALKEEPER_EXPECTED_GAMES_KEY = 0.9f;

	public const float CONST_GOALKEEPER_EXPECTED_GAMES_FIRST_TEAM = 0.85f;

	public const float CONST_GOALKEEPER_EXPECTED_GAMES_SQUAD_ROTATION = 0.1f;

	public const float CONST_GOALKEEPER_EXPECTED_GAMES_SQUAD_BACKUP = 0.1f;

	public const float CONST_GOALKEEPER_EXPECTED_GAMES_MIN_SENIOR = 0.05f;

	public const float CONST_GOALKEEPER_EXPECTED_GAMES_MIN_GRADUATE = 0f;

	public const short CONST_CLUB_LEAGUE_PERFORMANCE_WORSE_THAN_EXPECTED = -1;

	public const byte CONST_CLUB_LEAGUE_PERFORMANCE_AS_EXPECTED = 0;

	public const byte CONST_CLUB_LEAGUE_PERFORMANCE_BETTER_THAN_EXPECTED = 1;

	public const byte CONST_MAX_INBOX_MESSAGE_AGE_DAYS = 14;

	public const DayOfWeek CONST_FIRST_DAY_OF_SEASON_DAY_OF_WEEK = DayOfWeek.Saturday;

	public const byte CONST_FIRST_DAY_OF_SEASON_INDEX = 3;

	public const byte CONST_RANDEV_REACTION_IMPROVED_TRAINING = 1;

	public const byte CONST_RANDEV_REACTION_WORSE_TRAINING = 2;

	public const byte CONST_BOARD_EXPECTATION_WIN_LEAGUE = 1;

	public const byte CONST_BOARD_EXPECTATION_TOP_FEW = 2;

	public const byte CONST_BOARD_EXPECTATION_MID_TABLE = 3;

	public const byte CONST_BOARD_EXPECTATION_SURVIVE = 4;

	public const byte CONST_BOARD_EXPECTATION_PROMOTION = 5;

	public const byte CONST_COMPETITION_TYPE_INTERNATIONAL = 1;

	public const byte CONST_COMPETITION_TYPE_LEAGUE = 2;

	public const byte CONST_COMPETITION_TYPE_CUP = 3;

	public const byte CONST_COMPETITION_TYPE_CONTINENTAL = 4;

	public const byte CONST_COMPETITION_TYPE_FRIENDLY = 5;

	public const byte CONST_COMPETITION_TYPE_WORLD_CUP = 6;

	public const byte CONST_COMPETITION_TYPE_PROMOTION_PLAYOFF = 7;

	public const byte CONST_COMPETITION_TYPE_RELEGATION_PLAYOFF = 8;

	public const byte CONST_CUP_TYPE_KNOCKOUT_CUP = 0;

	public const byte CONST_CUP_TYPE_WINNERS_CUP = 1;

	public const byte CONST_TRANSFERS_PROCESSED_HOUR_OF_DAY = 9;

	public const byte CONST_CUP_DRAW_HOUR_OF_DAY = 12;

	public const byte CONST_CONTINENTAL_CUP_DRAW_HOUR_OF_DAY = 20;

	public const byte CONST_SUSPENSION_MATCHES_TWO_YELLOWS = 1;

	public const byte CONST_SUSPENSION_MATCHES_RED_CARD = 3;

	public const byte CONST_RATING_A = 91;

	public const byte CONST_RATING_B = 87;

	public const byte CONST_RATING_C = 82;

	public const byte CONST_RATING_D = 75;

	public const byte CONST_MATCHDAY_FLOW_POS_OFF = 0;

	public const byte CONST_MATCHDAY_FLOW_POS_FIXTURES = 1;

	public const byte CONST_MATCHDAY_FLOW_POS_PREVIEW = 2;

	public const byte CONST_MATCHDAY_FLOW_POS_TACTICS = 3;

	public const byte CONST_MATCHDAY_FLOW_POS_MATCHDAY = 4;

	public const byte CONST_MATCHDAY_FLOW_POS_PLAY = 5;

	public const byte CONST_MATCHDAY_FLOW_POS_MATCHDAY_TYPE_SELECTA = 6;

	public const byte CONST_MATCHDAY_FLOW_POS_BRINGUPTODATE = 11;

	public const byte CONST_MATCHDAY_FLOW_POS_FINISH = 12;

	public const byte CONST_MATCHDAY_FLOW_POS_BREAKOUT = 13;

	public static readonly List<string> CONST_19_REQUIRED_FOLDERS = new List<string>
	{
		"StartUpFiles", "Mobile", "Debug", "SM19IMAGES", "NEWSPLASH", "RequiredImages19", "RequiredFiles19", "Continents", "COUNTRYFLAGS", "RequiredFiles",
		"Stadium", "sm18Int_Match"
	};

	public static readonly List<string> CONST_19_DIRECTORY_LIST = new List<string> { "Assets\\Prefabs\\Mobile\\Resources", "Assets\\Prefabs", "Assets\\SP images", "Assets\\Resources", "Assets\\AssetBundles", "Assets\\AssetBundleBrowser" };

	public const byte CONST_MANAGER_SACKED_REASON_POOR_FORM = 0;

	public const byte CONST_MANAGER_SACKED_REASON_OBJECTIVE_FAILED = 1;

	public const byte CONST_PLAYER_MAX_RATING = 99;

	public const string CONST_TACTICS_STORED_MATCHDAY = "storedTactics-MatchDay";

	public const byte CONST_MINIMUM_NUMBER_OF_AVAILABLE_PLAYERS_POST_MATCH = 18;

	public const byte CONST_FRIENDLY_ASSOCIATION_CODE = 1;

	public const byte CONST_NO_ASSOCIATION_CODE = 2;

	public const byte CONST_RESPAWNED_ASSOCIATION_CODE = 3;

	public const byte CONST_CONTINENT_EUROPE = 1;

	public const byte CONST_CONTINENT_SOUTH_AMERICA = 2;

	public const byte CONST_CONTINENT_NORTH_AMERICA = 3;

	public const byte CONST_CONTINENT_ASIA = 4;

	public const byte CONST_CONTINENT_AFRICA = 5;

	public const byte CONST_CONTINENT_AUSTRALIA = 6;

	public const string CONST_STRING_NEGOTIATE_CONTRACT = "string-negotiate-contract";

	public const byte CONST_PURCHASE_HEAL_INJURY = 2;

	public const byte CONST_PURCHASE_BOOST_MORALE = 4;

	public const byte CONST_PURCHASE_BOOST_FITNESS = 5;

	public const byte CONST_PURCHASE_PLAYER = 10;

	public const byte CONST_PURCHASE_FACILITY = 13;

	public const byte CONST_PURCHASE_LIFE_CONTRACT = 14;

	public const byte CONST_PURCHASE_COACH_REPORT = 15;

	public const string CONST_STRING_UNKNOWN = "string-Unknown";

	public const byte CONST_ATTRIBUTE_AGGRESSION = 0;

	public const byte CONST_ATTRIBUTE_COMPOSURE = 1;

	public const byte CONST_ATTRIBUTE_CONCENTRATION = 2;

	public const byte CONST_ATTRIBUTE_DETERMINATION = 3;

	public const byte CONST_ATTRIBUTE_PACE = 4;

	public const byte CONST_ATTRIBUTE_STRENGTH = 5;

	public const byte CONST_ATTRIBUTE_AERIALABILITY = 6;

	public const byte CONST_ATTRIBUTE_FLAIR = 7;

	public const byte CONST_ATTRIBUTE_LEADERSHIP = 8;

	public const byte CONST_ATTRIBUTE_LONGTHROWS = 9;

	public const byte CONST_ATTRIBUTE_HIDDEN_FITNESS = 28;

	public const byte CONST_ATTRIBUTE_MARKING = 10;

	public const byte CONST_ATTRIBUTE_TACKLING = 11;

	public const byte CONST_ATTRIBUTE_POSITIONING = 12;

	public const byte CONST_ATTRIBUTE_CREATIVITY = 13;

	public const byte CONST_ATTRIBUTE_MOVEMENT = 14;

	public const byte CONST_ATTRIBUTE_FINISHING = 15;

	public const byte CONST_ATTRIBUTE_DRIBBLING = 16;

	public const byte CONST_ATTRIBUTE_LONGSHOTS = 17;

	public const byte CONST_ATTRIBUTE_CROSSING = 18;

	public const byte CONST_ATTRIBUTE_CONTROL = 19;

	public const byte CONST_ATTRIBUTE_LONGPASSING = 20;

	public const byte CONST_ATTRIBUTE_PASSING = 21;

	public const byte CONST_ATTRIBUTE_CORNERS = 22;

	public const byte CONST_ATTRIBUTE_FREEKICKS = 23;

	public const byte CONST_ATTRIBUTE_PENALTIES = 24;

	public const byte CONST_ATTRIBUTE_STAMINA = 25;

	public const byte CONST_ATTRIBUTE_VOLLEYING = 26;

	public const byte CONST_ATTRIBUTE_WORKRATE = 27;

	public const byte CONST_ATTRIBUTE_KEEPER_COMMANDOFAREA = 10;

	public const byte CONST_ATTRIBUTE_KEEPER_DISTRIBUTION = 11;

	public const byte CONST_ATTRIBUTE_KEEPER_HANDLING = 12;

	public const byte CONST_ATTRIBUTE_KEEPER_ONEONONES = 13;

	public const byte CONST_ATTRIBUTE_KEEPER_SHOTSTOPPING = 14;

	public const byte CONST_ATTRIBUTE_KEEPER_POSITIONING = 15;

	public const byte CONST_ATTRIBUTE_KEEPER_SAVINGPENALTIES = 16;

	public const byte CONST_ATTRIBUTE_TYPE_TECHNICAL = 0;

	public const byte CONST_ATTRIBUTE_TYPE_MENTAL = 1;

	public const byte CONST_ATTRIBUTE_TYPE_PHYSICAL = 2;

	public const byte CONST_BASIC_POSITION_KEEPER = 0;

	public const byte CONST_BASIC_POSITION_DEFENDER = 1;

	public const byte CONST_BASIC_POSITION_MIDFIELDER = 2;

	public const byte CONST_BASIC_POSITION_FORWARD = 3;

	public const byte CONST_NUM_BASIC_POSITIONS = 4;

	public const byte CONST_POSITION_DESC_KEEPER = 1;

	public const byte CONST_POSITION_DESC_MODERN_KEEPER = 2;

	public const byte CONST_POSITION_DESC_STOPPER = 10;

	public const byte CONST_POSITION_DESC_GENERAL_DEF = 11;

	public const byte CONST_POSITION_DESC_BALL_PLAYING_DEF = 12;

	public const byte CONST_POSITION_DESC_FULLBACK = 13;

	public const byte CONST_POSITION_DESC_WINGBACK = 14;

	public const byte CONST_POSITION_DESC_BALL_WINNING_MID = 20;

	public const byte CONST_POSITION_DESC_DEEP_PLAYMAKER = 21;

	public const byte CONST_POSITION_DESC_BOX_TO_BOX_MID = 22;

	public const byte CONST_POSITION_DESC_GENERAL_MID = 23;

	public const byte CONST_POSITION_DESC_PLAY_MAKER = 24;

	public const byte CONST_POSITION_DESC_WIDE_MID = 25;

	public const byte CONST_POSITION_DESC_WINGER = 27;

	public const byte CONST_POSITION_DESC_ADV_PLAYMAKER = 28;

	public const byte CONST_POSITION_DESC_SUPPORT_STRIKER = 29;

	public const byte CONST_POSITION_DESC_TARGET_MAN = 40;

	public const byte CONST_POSITION_DESC_FINISHER = 41;

	public const byte CONST_POSITION_DESC_WIDE_FWD = 42;

	public const byte CONST_POSITION_DESC_DEEP_FWD = 43;

	public const byte CONST_POSITION_DESC_GENERAL_FWD = 44;

	public const byte CONST_LOW_MORAL = 5;

	public const byte CONST_MINIMUM_FITNESS_FOR_LOW_FITNESS_MSG = 65;

	public const byte CONST_NUM_OUTFIELD_PLYS = 11;

	public const byte CONST_NUM_SUB_PLYS = 7;

	public const byte CONST_NUM_TEAM_PLYS = 18;

	public const byte NUM_SQUAD_PLYS = 18;

	public const byte CONST_MAX_NUM_SUBS = 3;

	public const byte CONST_NUMBER_OF_SQUAD_PLAYERS = 18;

	public const byte CONST_NUM_MINUTES_PER_MATCH = 90;

	public const byte CONST_GOALTYPE_OPENPLAY = 0;

	public const byte CONST_GOALTYPE_PENALTY = 1;

	public const byte CONST_GOALTYPE_SETPIECE = 2;

	public const byte CONST_GOALTYPE_CORNER = 3;

	public const byte CONST_GOALTYPE_OWNGOAL = 4;

	public const byte CONST_ASSISTTYPE_OPENPLAY = 0;

	public const byte CONST_ASSISTTYPE_SETPIECE = 1;

	public const byte CONST_ASSISTTYPE_CORNER = 2;

	public const string CONST_DEFAULT_FORMATION = "4-2-3-1";

	public const byte CONST_TEAM_PICK_STRONGEST = 0;

	public const byte CONST_TEAM_PICK_NORMAL = 1;

	public const byte CONST_TEAM_PICK_REST = 2;

	public const byte CONST_TEAM_PICK_RESTALL = 3;

	public const byte CONST_TEAM_AIM_DRAW = 0;

	public const byte CONST_TEAM_AIM_WIN = 1;

	public const byte CONST_TEAM_AIM_MUSTWIN = 2;

	public const byte CONST_TEAM_AIM_GIVENUP = 3;

	public const byte CONST_TEAM_WINNING = 0;

	public const byte CONST_TEAM_DRAWING = 1;

	public const byte CONST_TEAM_LOSING = 2;

	public const byte CONST_TACTICS_TACKLINGSTYLE_NORMAL = 1;

	public const byte CONST_TACTICS_TACKLINGSTYLE_HARD = 2;

	public const byte CONST_TACTICS_TACKLINGSTYLE_AGGRESSIVE = 3;

	public const byte CONST_TACTICS_WIDTH_NARROW = 1;

	public const byte CONST_TACTICS_WIDTH_NORMAL = 2;

	public const byte CONST_TACTICS_WIDTH_WIDE = 3;

	public const byte CONST_TACTICS_MENTALITY_VDEF = 1;

	public const byte CONST_TACTICS_MENTALITY_DEF = 2;

	public const byte CONST_TACTICS_MENTALITY_NORMAL = 3;

	public const byte CONST_TACTICS_MENTALITY_ATT = 4;

	public const byte CONST_TACTICS_MENTALITY_VATT = 5;

	public const byte CONST_TACTICS_PASSINGSTYLE_SHORT = 1;

	public const byte CONST_TACTICS_PASSINGSTYLE_MIXED = 2;

	public const byte CONST_TACTICS_PASSINGSTYLE_DIRECT = 3;

	public const byte CONST_TACTICS_PASSINGSTYLE_LONGBALL = 4;

	public const byte CONST_TACTICS_ATTACKINGSTYLE_MIXED = 1;

	public const byte CONST_TACTICS_ATTACKINGSTYLE_BOTHFLANKS = 2;

	public const byte CONST_TACTICS_ATTACKINGSTYLE_LEFTFLANK = 3;

	public const byte CONST_TACTICS_ATTACKINGSTYLE_RIGHTFLANK = 4;

	public const byte CONST_TACTICS_ATTACKINGSTYLE_MIDDLE = 5;

	public const byte CONST_TACTICS_TEMPO_SLOW = 1;

	public const byte CONST_TACTICS_TEMPO_NORMAL = 2;

	public const byte CONST_TACTICS_TEMPO_FAST = 3;

	public const byte CONST_TACTICS_PRESSING_OWNAREA = 1;

	public const byte CONST_TACTICS_PRESSING_OWNHALF = 2;

	public const byte CONST_TACTICS_PRESSING_ALLOVER = 3;

	public const byte CONST_TACTICS_FORWARDS_MIXED = 1;

	public const byte CONST_TACTICS_FORWARDS_WORK_INTO_BOX = 2;

	public const byte CONST_TACTICS_FORWARDS_SHOOT_ON_SIGHT = 3;

	public const byte CONST_TACTICS_MIDFIELDERS_MIXED = 1;

	public const byte CONST_TACTICS_MIDFIELDERS_THROUGH_BALLS = 2;

	public const byte CONST_TACTICS_MIDFIELDERS_RUN_AT_DEFENCE = 3;

	public const byte CONST_TACTICS_WINGERS_MIXED = 1;

	public const byte CONST_TACTICS_WINGERS_OVERLAPPING_RUNS = 2;

	public const byte CONST_TACTICS_WINGERS_EARLY_CROSSES = 3;

	public const byte CONST_TACTICS_WINGERS_WORK_INTO_BOX = 4;

	public const byte CONST_TACTICS_BACKLINE_DEEP = 1;

	public const byte CONST_TACTICS_BACKLINE_NORMAL = 2;

	public const byte CONST_TACTICS_BACKLINE_VERY_HIGH = 3;

	public const byte CONST_TACTICS_CREATIVITY_DISCIPLINED = 1;

	public const byte CONST_TACTICS_CREATIVITY_NORMAL = 2;

	public const byte CONST_TACTICS_CREATIVITY_ADVENTUROUS = 3;

	public const byte CONST_TACTICS_INST_AGGRESSION = 0;

	public const byte CONST_TACTICS_INST_ATTACKINGSTYLE = 1;

	public const byte CONST_TACTICS_INST_PASSINGSTYLE = 2;

	public const byte CONST_TACTICS_INST_FOCUSPASSING = 3;

	public const byte CONST_TACTICS_INST_TEMPO = 4;

	public const byte CONST_TACTICS_INST_PRESSING = 5;

	public const byte CONST_TACTICS_INST_COUNTERATTACK = 6;

	public const byte CONST_TACTICS_INST_MENBEHINDBALL = 7;

	public const byte CONST_TACTICS_INST_TIGHTMARKING = 8;

	public const byte CONST_TACTICS_INST_PLAYOFFSIDE = 9;

	public const byte CONST_TACTICS_INST_USEPLAYMAKER = 10;

	public const byte CONST_TACTICS_INST_USETARGETMAN = 11;

	public const byte CONST_TACTICS_INST_CAPTAIN = 12;

	public const byte CONST_TACTICS_INST_PENALTYTAKER = 13;

	public const byte CONST_TACTICS_INST_DEADBALLTAKER = 14;

	public const byte CONST_TACTICS_INST_CORNERTAKER = 15;

	public const byte CONST_TACTICS_INST_PLAYMAKER = 16;

	public const byte CONST_TACTICS_INST_TARGETMAN = 17;

	public const byte CONST_TACTICS_INST_FORMATION = 20;

	public const byte CONST_TACTICS_UNCONFIRMED_SUB = 1;

	public const byte CONST_TACTICS_CONFIRMED_SUB = 2;

	public const byte CONST_NUM_PRE_SEASON_FRIENDLIES = 3;

	public const byte CONST_MIN_SQUADSIZE = 22;

	public const byte CONST_NUM_FIRST_TEAM_PLAYERS = 25;

	public const byte CONST_MIN_NUM_FIRST_TEAM_PLAYERS = 20;

	public const byte CONST_MAX_NUM_LOANED_IN_PLAYERS = 5;

	public const byte CONST_MAX_SQUADSIZE = 150;

	public const byte CONST_SQUAD_CAP = 100;

	public const int CONST_MAX_PLAYERS = 50000;

	public const byte CHINA_MAX_FOREIGN_PLAYERS_LINEUP = 3;

	public const byte CHINA_COUNTRY_ID = 42;

	public static readonly List<string> CONST_EXTRA_CUP_COUNTRY_CODES = new List<string> { "ENG", "FRA", "SCO", "POR" };

	public const byte CONST_NUM_CUPS_PER_ASSOC = 2;

	public const byte CONST_MAX_CLUBS_PER_LEAGUE = 24;

	public const byte CONST_MAX_CLUBS_PER_CUPGROUP = 4;

	public const byte CONST_MAX_TURNS_PER_CUPGROUP = 6;

	public const byte CONST_MAX_TEAMS_RELEGATED_PER_LEAGUE = 4;

	public const byte CONST_NUM_TEAMS_TO_STORE_IN_TOP_DIV_PROMOTION_LOG = 8;

	public const byte CONST_SCOUT_REPORT_WORLD_CLASS = 1;

	public const byte CONST_SCOUT_REPORT_EXCELLENT = 2;

	public const byte CONST_SCOUT_REPORT_VERY_GOOD = 3;

	public const byte CONST_SCOUT_REPORT_GOOD = 4;

	public const byte CONST_SCOUT_REPORT_DECENT = 5;

	public const byte CONST_SCOUT_REPORT_NOT_GOOD_ENOUGH = 6;

	public const short CONST_SCOUT_TOO_GOOD_FOR_CLUB = -2;

	public const short CONST_SCOUT_NOT_GOOD_ENOUGH = -1;

	public const byte CONST_SCOUT_FOR_STARTING_LINEUP = 0;

	public const byte CONST_SCOUT_FOR_SQUAD = 1;

	public const byte CONST_SCOUT_FOR_YOUTH_TEAM = 2;

	public const byte CONST_COACH_REPORT_DURATION_DAYS = 30;

	public const byte CONST_SCOUT_FACILITY = 0;

	public const byte CONST_COACHING_FACILITY = 1;

	public const byte CONST_TRAINING_MAX_INJURIES_PER_CLUB = 5;

	public const byte CONST_TRAINING_INJURY_SERIOUS_PROPORTION = 20;

	public const byte CONST_FACILITY_TECH_TREE_SCREEN = 1;

	public const byte CONST_FACILITY_ACTIVITY_SCREEN = 2;

	public const int CONST_FACILITY_COST_TRAINING = 500000;

	public const int CONST_FACILITY_COST_STADIUM = 2000000;

	public const int CONST_FACILITY_COST_SCOUTING = 1000000;

	public const int CONST_FACILITY_COST_YOUTH = 1000000;

	public const byte CONST_SQUAD_GENERAL_VIEW = 0;

	public const byte CONST_SQUAD_STATS_VIEW = 1;

	public const byte CONST_SQUAD_ATTR_VIEW = 2;

	public const byte CONST_SQUAD_CONTRACT_VIEW = 3;

	public static readonly Dictionary<byte, List<string>> CONST_CL_QUALIFIER_NATIONS = new Dictionary<byte, List<string>>
	{
		{
			1,
			new List<string> { "CZE", "NED", "GRE", "CRO", "DEN", "POL", "SWE", "SCG", "SCO", "NOR" }
		},
		{
			2,
			new List<string> { "POR", "UKR", "BEL", "TUR", "AUT", "SUI", "CZE", "NED" }
		},
		{
			3,
			new List<string> { "FRA", "RUS" }
		}
	};

	public static readonly Dictionary<byte, List<string>> CONST_CL_AUTO_PLAYOFF = new Dictionary<byte, List<string>> { 
	{
		1,
		new List<string> { "AUT", "SUI" }
	} };

	public static readonly Dictionary<byte, List<string>> CONST_CL_AUTO_QUALIFY = new Dictionary<byte, List<string>>
	{
		{
			1,
			new List<string> { "ESP", "ENG", "ITA", "GER", "FRA", "RUS", "POR", "UKR", "BEL", "TUR" }
		},
		{
			2,
			new List<string> { "ESP", "ENG", "ITA", "GER", "FRA", "RUS" }
		},
		{
			3,
			new List<string> { "ESP", "ENG", "ITA", "GER" }
		},
		{
			4,
			new List<string> { "ESP", "ENG", "ITA", "GER" }
		}
	};

	public static readonly Dictionary<short, List<string>> CONST_EL_QUALIFIER_NATIONS = new Dictionary<short, List<string>>
	{
		{
			6,
			new List<string> { "ESP", "ITA", "GER" }
		},
		{
			5,
			new List<string> { "RUS" }
		},
		{
			4,
			new List<string> { "POR", "UKR", "BEL", "TUR", "AUT", "SUI", "CZE", "NED" }
		},
		{
			3,
			new List<string> { "CZE", "NED", "GRE", "CRO", "DEN", "POL", "SWE", "SCG", "SCO", "NOR" }
		},
		{
			2,
			new List<string> { "GRE", "CRO", "DEN", "POL", "SWE", "SCG", "SCO", "NOR" }
		},
		{
			-1,
			new List<string> { "DEN", "POL", "SWE", "SCG", "SCO", "NOR" }
		},
		{
			-2,
			new List<string> { "ENG", "FRA" }
		}
	};

	public static readonly Dictionary<short, List<string>> CONST_EL_AUTO_PLAYOFF = new Dictionary<short, List<string>>
	{
		{
			4,
			new List<string> { "RUS" }
		},
		{
			3,
			new List<string> { "POR", "UKR", "BEL", "TUR", "AUT", "SUI" }
		},
		{
			-1,
			new List<string> { "CZE", "NED", "GRE", "CRO" }
		}
	};

	public static readonly Dictionary<short, List<string>> CONST_EL_AUTO_QUALIFY = new Dictionary<short, List<string>>
	{
		{
			-1,
			new List<string>
			{
				"ESP", "ENG", "ITA", "GER", "FRA", "RUS", "POR", "UKR", "BEL", "TUR",
				"AUT", "SUI"
			}
		},
		{
			5,
			new List<string> { "ESP", "ENG", "ITA", "GER" }
		},
		{
			4,
			new List<string> { "FRA" }
		}
	};

	public static readonly Dictionary<string, byte> CONST_COPA_LIB_GROUPS = new Dictionary<string, byte>
	{
		{ "BRA", 8 },
		{ "ARG", 7 },
		{ "COL", 5 },
		{ "CHI", 5 },
		{ "URU", 5 }
	};

	public static readonly Dictionary<string, byte> CONST_COPA_SUB_GROUPS = new Dictionary<string, byte>
	{
		{ "BRA", 6 },
		{ "ARG", 6 },
		{ "COL", 4 },
		{ "CHI", 4 },
		{ "URU", 4 }
	};

	public static readonly Dictionary<string, byte> CONST_ASIAN_CHAMP_GROUPS = new Dictionary<string, byte>
	{
		{ "CHN", 4 },
		{ "JPN", 4 },
		{ "KOR", 4 },
		{ "AUS", 4 }
	};

	public static readonly Dictionary<string, byte> CONST_NORTH_AMERICAN_CHAMP_GROUPS = new Dictionary<string, byte>
	{
		{ "MEX", 8 },
		{ "USA", 8 }
	};

	public const byte CONST_FACILITIES_STADIUM_CAPACITY = 0;

	public const byte CONST_FACILITIES_YOUTH_ACADEMY = 1;

	public const byte CONST_FACILITIES_TRAINING_GROUND = 2;

	public const byte CONST_FACILITIES_MEDICAL_CENTRE = 3;

	public const byte CONST_FACILITIES_SCOUTING_NETWORK = 4;

	public const byte CONST_FACILITIES_ASSISTANT_MANAGER = 5;

	public const byte OG_PLAYER_INDEX_MODIFIER = 100;

	public const byte MATCHDAY_ENGINE_QUICK = 0;

	public const byte MATCHDAY_ENGINE_2DPITCH = 1;

	public const byte MATCHDAY_ENGINE_V4 = 2;

	public const int DEFAULT_AJAX_TIMEOUT_AMOUNT = 25;

	public const float TV_PREC = 0.5f;

	public const float GATE_PERC = 0.5f;

	public const byte PRIZEMONEY_PERC = 14;

	public const float CONST_MIN_TRANSFER_BID_MODIFIER = 0.5f;

	public const byte CONST_ACHIEVEMENT_TOTAL_SHOTS_1 = 25;

	public const byte CONST_ACHIEVEMENT_TOTAL_SHOTS_ON_TARGET_1 = 15;

	public static short[][] g_team_aims_drawok = new short[4][]
	{
		new short[3] { -1, 0, 0 },
		new short[3] { -1, 0, 0 },
		new short[3] { -2, -1, 1 },
		new short[3] { -2, -1, 1 }
	};

	public static short[][] g_team_aims_win = new short[4][]
	{
		new short[3],
		new short[3],
		new short[3] { 0, 0, 1 },
		new short[3] { -1, 1, 1 }
	};

	public static short[][] g_team_aims_mustwin = new short[4][]
	{
		new short[3] { 0, 1, 1 },
		new short[3] { 0, 1, 2 },
		new short[3] { 0, 2, 2 },
		new short[3] { 0, 2, 2 }
	};

	public static Color32 selectedBackgroundColour = new Color32(8, 66, 120, byte.MaxValue);

	public static Color32 unselectedBackgroundColour = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);

	public static Color32 attributeRedColour = new Color32(byte.MaxValue, 0, 0, byte.MaxValue);

	public static Color32 attributeOrangeColour = new Color32(byte.MaxValue, 132, 0, byte.MaxValue);

	public static Color32 attributeYellowColour = new Color32(241, 225, 0, byte.MaxValue);

	public static Color32 attributeGreenColour = new Color32(0, byte.MaxValue, 44, byte.MaxValue);

	public static Color32 attributeDarkGreenColour = new Color32(93, byte.MaxValue, 0, byte.MaxValue);

	public static Color32 attributeGreyColour = new Color32(66, 66, 66, byte.MaxValue);

	public static Color32 blue = Helpers.HexToColour("0094B3");

	public static Color32 red = Helpers.HexToColour("FF0000");

	public static Color32 green = Helpers.HexToColour("00A908FF");

	public static List<string> g_countrydata_playable_list = new List<string>
	{
		"ENG", "ESP", "ITA", "GER", "FRA", "NED", "POR", "RUS", "SCO", "TUR",
		"GRE", "BEL", "SUI", "AUT", "UKR", "ROM", "CRO", "POL", "CZE", "SCG",
		"DEN", "SWE", "NOR", "USA", "MEX", "BRA", "ARG", "COL", "CHI", "URU",
		"CHN", "JPN", "KOR", "AUS", "EGY"
	};

	public static List<string> eu_countries_only = new List<string>
	{
		"ENG", "ESP", "ITA", "GER", "FRA", "NED", "POR", "RUS", "SCO", "TUR",
		"GRE", "BEL", "SUI", "AUT", "UKR", "CRO", "POL", "CZE", "SCG", "DEN",
		"SWE", "NOR"
	};

	public static List<string> american_countries_only = new List<string> { "USA", "MEX", "BRA", "ARG", "COL", "CHI", "URU" };

	public static List<string> asia_countries_only = new List<string> { "CHN", "JPN", "KOR", "AUS" };

	public const byte SPLASH_SCREEN = 0;

	public const byte FINANCE_OVERVIEW_SCREEN = 1;

	public const byte CLUB_PICKER_SCREEN = 2;

	public const byte WORLD_NEWS_SCREEN = 3;

	public const byte CLUB_OVERVIEW_SCREEN = 4;

	public const byte WORLD_SCREEN = 5;

	public const byte SEARCH_SCREEN = 6;

	public const byte SHOP_HOME_SCREEN = 8;

	public const byte SQUAD_LIST_SCREEN = 9;

	public const byte PLAYER_SCREEN = 10;

	public const byte SCHEDULE_SCREEN = 11;

	public const byte TRAINING_SCREEN = 13;

	public const byte TACTICS_SCREEN = 14;

	public const byte MANAGER_SCREEN = 15;

	public const byte PLAYER_COMPARE_SCREEN = 16;

	public const byte SAVE_LOAD_SCREEN = 18;

	public const byte MATCHDAY_RESULTS_SCREEN = 19;

	public const byte SCOUT_SCREEN = 20;

	public const byte PLAYER_PROFILE_SCREEN = 21;

	public const byte COUNTRY_PICKER_SCREEN = 22;

	public const byte MANAGER_TRAITS_SCREEN = 23;

	public const byte UNMANAGEABLE_CLUB_OVERVIEW_SCREEN = 24;

	public const byte PLAYER_TRAITS_SCREEN = 25;

	public const byte INBOX_SCREEN = 26;

	public const byte UNMANAGED_SQUAD_SCREEN = 27;

	public const byte MATCHDAY_PREVIEW_SCREEN = 28;

	public const byte MATCHDAY_LINEUPS_SCREEN = 29;

	public const byte UNMANAGED_PLAYER_PROFILE_SCREEN = 30;

	public const byte TROPHY_SCREEN = 32;

	public const byte CLUB_LIST_SCREEEN = 33;

	public const byte MATCH_PREVIEW_SCREEEN = 34;

	public const byte MATCH_REPORT_SCREEEN = 35;

	public const byte CUP_DRAW_SCREEN = 36;

	public const byte UNMANAGED_OVERVIEW_SCREEN = 37;

	public const byte UNMANAGED_SCHEDULE_SCREEN = 38;

	public const byte UNMANAGED_TRANSFERS_SCREEN = 39;

	public const byte CLUB_TRANSFERS_SCREEN = 40;

	public const byte YOUTH_SCREEN = 41;

	public const byte MEDICAL_SCREEN = 42;

	public const byte STADIUM_SCREEN = 43;

	public const byte ASSISTANT_SCREEN = 44;

	public const byte FACILITIES_OVERVIEW_SCREEN = 45;

	public const byte SHORT_LIST_SCREEN = 46;

	public const byte TRANSFER_SCREEN = 47;

	public const byte UNMANAGED_STADIUM_SCREEN = 49;

	public const byte CLUB_SEARCH_SCREEN = 50;

	public const byte SQUAD_STATS_SCREEN = 51;

	public const byte SQUAD_REPORTS_SCREEN = 52;

	public const byte BOARD_SCREEN = 53;

	public const byte MANAGER_STATS_SCREEN = 54;

	public const byte TROPHY_CABINET_SCREEN = 55;

	public const byte JOB_OFFERS_SCREEN = 56;

	public const byte FINANCE_GRAPHS_SCREEN = 58;

	public const byte TACTICS_INSTRUCTIONS_SCREEN = 60;

	public const byte TACTICS_SQUAD_SCREEN = 61;

	public const byte YOUTH_UPGRADES_SCREEN = 62;

	public const byte YOUTH_PLAYERS_SCREEN = 63;

	public const byte SCOUT_ASSIGNMENTS_SCREEN = 64;

	public const byte SCOUT_UPGRADES_SCREEN = 65;

	public const byte SCOUT_REPORTS_SCREEN = 66;

	public const byte TRAINING_DRILLS_SCREEN = 67;

	public const byte TRAINING_UPGRADES_SCREEN = 68;

	public const byte TRAINING_REPORTS_SCREEN = 69;

	public const byte TRANSFERS_WORLD_SCREEN = 71;

	public const byte YOUTH_REPORTS_SCREEN = 72;

	public const byte CLUBZONE_UPGRADE_SCREEN = 73;

	public const byte CLUBZONE_ACTIONS_SCREEN = 74;

	public const byte CLUBZONE_REPORTS_SCREEN = 75;

	public const byte LEAGUE_SCHEDULE_SCREEN = 76;

	public const byte LEAGUE_TABLE_SCREEN = 77;

	public const byte LEAGUE_STATS_SCREEN = 78;

	public const byte DOMESTICCUP_SCHEDULE_SCREEN = 79;

	public const byte DOMESTICCUP_RESULTS_SCREEN = 80;

	public const byte DOMESTICCUP_STATS_SCREEN = 81;

	public const byte CONTINENTALCUP_SCHEDULE_SCREEN = 82;

	public const byte CONTINENTALCUP_TABLE_SCREEN = 83;

	public const byte CONTINENTALCUP_STATS_SCREEN = 84;

	public const byte CONTINENTALCUP_HISTORY_SCREEN = 85;

	public const byte DOMESTICCUP_HISTORY_SCREEN = 86;

	public const byte LEAGUE_HISTORY_SCREEN = 87;

	public const byte BOOSTS_SCREEN = 89;

	public const byte CREDITS_SCREEN = 90;

	public const byte CASH_SCREEN = 91;

	public const byte TRANSFER_PLAYER_SEARCH_SCREEN = 92;

	public const byte PLAYER_SHOP_SCREEN = 93;

	public const byte BUNDLES_SCREEN = 94;

	public const byte SCHEDULE_GRID_SCREEN = 95;

	public const byte MANAGER_XP_SCREEN = 96;

	public const byte END_OF_SEASON_SCREEN = 97;

	public const byte SACKING_SCREEN = 98;

	public const byte NEW_GAME_DATA_PACK_SCREEN = 99;

	public const byte SCHEDULE_DEBUG_SCREEN = 100;

	public const byte DEBUG_TOOLS_PANEL = 101;

	public static readonly byte[] HOME_SCREEN_GROUP = new byte[3] { 26, 4, 3 };

	public static readonly byte[] SQUAD_SCREEN_GROUP = new byte[3] { 52, 9, 51 };

	public static readonly byte[] MANAGER_SCREEN_GROUP = new byte[3] { 15, 53, 54 };

	public static readonly byte[] FINANCE_SCREEN_GROUP = new byte[2] { 58, 1 };

	public static readonly byte[] TACTICS_SCREEN_GROUP = new byte[3] { 61, 14, 60 };

	public static readonly byte[] YOUTH_SCREEN_GROUP = new byte[3] { 62, 63, 72 };

	public static readonly byte[] SCOUTING_SCREEN_GROUP = new byte[3] { 65, 64, 66 };

	public static readonly byte[] TRAINING_SCREEN_GROUP = new byte[3] { 68, 67, 69 };

	public static readonly byte[] TRANSFERS_SCREEN_GROUP = new byte[3] { 40, 92, 71 };

	public static readonly byte[] CLUBZONE_SCREEN_GROUP = new byte[3] { 73, 74, 75 };

	public static readonly byte[] LEAGUE_SCREEN_GROUP = new byte[4] { 76, 77, 78, 87 };

	public static readonly byte[] DOMESTICCUP_SCREEN_GROUP = new byte[4] { 79, 80, 81, 86 };

	public static readonly byte[] CONTINENTALCUP_SCREEN_GROUP = new byte[4] { 82, 83, 84, 85 };

	public static readonly byte[] SHOP_SCREEN_GROUP = new byte[6] { 8, 94, 90, 91, 93, 89 };

	public static readonly byte[] SCHEDULE_SCREEN_GROUP = new byte[2] { 11, 95 };

	public static readonly byte[] SEARCHES_SCREEN_GROUP = new byte[3] { 6, 50, 5 };

	public const string SAVE_GAME_TEMP_FILENAME = "SM21_save_TEMP";

	public const string SAVE_GAME_FILENAME_PREFIX = "SM21_save_";

	public const string SAVE_GAME_BACKUP_FILENAME = "SM21_save_BACKUP";

	public const string SAVE_GAME_LOG_FILENAME = "SaveLog.txt";

	public const string ERROR_LOG_FILENAME = "SM21_ErrorLog.txt";

	public const string STATIC_DATABASE_FILENAME_PREFIX = "SM21_StaticData";

	public const int SAVE_DATABASE_TEMP_SLOTNUM = -1;

	public const string SAVE_DATABASE_FILENAME_PREFIX = "SM21_";

	public const string SAVE_GAME_TAG_PREFIX = "?tag=";

	public const string SAVE_GAME_TAG_METADATA = "metadata";

	public const string SAVE_GAME_TAG_GAME = "game";

	public const byte LATEST_SAVE_FILE_VERSION = 18;

	public const byte REFRESH_CACHE_DAYS = 1;

	public const string COLOUR_GREY = "898989FF";

	public const string COLOUR_WHITE = "FFFFFFFF";

	public const string COLOUR_RED = "FF0000FF";

	public const string COLOUR_GREEN = "00FF2188";

	public const string COLOUR_BLUE = "0094B3";

	public const string COLOUR_GOLD = "EBB40088";

	public const string COLOUR_LIGHT_GREEN = "2BEB0088";

	public const string COLOUR_DARK_GREEN = "1F960588";

	public const string COLOUR_RELEGATION_RED = "FF0000FF";

	public const string COLOUR_RESULT_RED = "FF0000FF";

	public const string COLOUR_RESULT_GREEN = "00B510FF";

	public const string COLOUR_RESULT_BLUE = "0033B3FF";

	public static bool runOnce = true;

	public static bool usingRTL = false;

	public static bool isGameLoading = false;

	public const byte MESSAGE_LENGTH = 40;

	public const byte MESSAGE_LENGTH_DESKTOP = 70;

	public const byte FIRST_FREE_PACK = 1;

	public const byte EXTRA_FREE_PACK = 3;

	public const short AMOUNT_CREDIT_1 = 250;

	public const short AMOUNT_CREDIT_2 = 1350;

	public const short AMOUNT_CREDIT_3 = 3000;

	public const short AMOUNT_CREDIT_4 = 6750;

	public const short AMOUNT_CREDIT_5 = 11750;

	public const int AMOUNT_CREDIT_6 = 21000;

	public const short AMOUNT_BUNDLE_CREDIT_1 = 250;

	public const short AMOUNT_BUNDLE_CREDIT_2 = 1350;

	public const short AMOUNT_BUNDLE_CREDIT_3 = 3000;

	public const short AMOUNT_BUNDLE_CREDIT_4 = 21000;

	public const short SPOTLIGHT_REFRESH_PRICE = 100;

	public const short CREDIT_AMOUNT_CP_BUNDLE_1 = 125;

	public const short CREDIT_AMOUNT_CP_BUNDLE_2 = 275;

	public const short CREDIT_AMOUNT_CP_BUNDLE_3 = 500;

	public const short CREDIT_AMOUNT_CP_BUNDLE_4 = 875;

	public const short CREDIT_AMOUNT_CP_BUNDLE_5 = 1875;

	public const short CREDIT_AMOUNT_CP_BUNDLE_6 = 2500;

	public const byte AMOUNT_CP_BUNDLE_1 = 2;

	public const byte AMOUNT_CP_BUNDLE_2 = 5;

	public const byte AMOUNT_CP_BUNDLE_3 = 10;

	public const byte AMOUNT_CP_BUNDLE_4 = 20;

	public const byte AMOUNT_CP_BUNDLE_5 = 50;

	public const byte AMOUNT_CP_BUNDLE_6 = 100;

	public const long DEFAULT_CASH_BUNDLE1_AMOUNT = 50000000L;

	public const long DEFAULT_CASH_BUNDLE2_AMOUNT = 265000000L;

	public const long DEFAULT_CASH_BUNDLE3_AMOUNT = 560000000L;

	public const long DEFAULT_CASH_BUNDLE4_AMOUNT = 1250000000L;

	public const long DEFAULT_CASH_BUNDLE5_AMOUNT = 2250000000L;

	public const long DEFAULT_CASH_BUNDLE6_AMOUNT = 4000000000L;

	public const short SHOP_MYSTERY_PLAYER_PRICE = 1500;

	public static readonly int[] AMOUNT_BUNDLE_BOOSTS_1 = new int[4] { 10, 10, 10, 10 };

	public static readonly int[] AMOUNT_BUNDLE_BOOSTS_2 = new int[4] { 25, 25, 25, 25 };

	public static readonly int[] AMOUNT_BUNDLE_BOOSTS_3 = new int[4] { 50, 50, 50, 50 };

	public static readonly int[] AMOUNT_BUNDLE_BOOSTS_4 = new int[4] { 100, 100, 100, 100 };

	public const byte MIN_BUG_REPORT_LENGTH = 0;

	public const byte MIN_BUG_REPORT_LENGTH_NEW = 0;

	public const string RATE_DONT_SHOW_STATUS_KEY = "RATE_DONT_SHOW_STATUS";

	public const string RATE_DONT_SHOW_STATUS_KEY_1 = "RATE_DONT_SHOW_STATUS_1";

	public const string RATE_REMIND_LIMIT_KEY = "RATE_REMIND_LIMIT";

	public const byte RATE_REDIRECT_LIMIT = 4;

	public const int RATE_REMIND_LIMIT = 5;

	public const string GOOGLE_PLAY_LINK_PREFIX = "market://details?id=";

	public const byte CONST_TIME_UNIT_HOURS = 1;

	public const byte CONST_TIME_UNIT_DAYS = 2;

	public const byte CONST_TIME_UNIT_WEEKS = 3;

	public const byte CONST_TIME_UNIT_MONTHS = 4;

	public const byte CONST_TIME_UNIT_YEARS = 5;

	public const int CONST_MIN_PLAYER_CREDITS_AMOUNT = 25;

	public const int CONST_MIN_PLAYER_CREDITS_AMOUNT_DISCOUNT = 18;

	public static readonly List<int> CONST_FIRST_SEASON_GOAL50 = new List<int>
	{
		2772, 53468, 85260, 1131, 32924, 55579, 56394, 52690, 3513, 68359,
		59194, 47215, 41018, 70860, 88385, 65977, 35895, 1110, 61438, 87683,
		82275, 19587, 43493, 45279, 68182, 38457, 46218, 35346, 34893, 34634,
		90936, 41780, 4165, 49590, 81586, 68341, 58838, 59016, 6378, 29182,
		44009, 2370, 52519, 16087, 39419, 56786, 35330, 73924, 13340, 41105
	};

	public const float defaultAspect = 1.7777778f;

	public const int SCENE_INDEX_SPLASH = 0;

	public const int SCENE_INDEX_GENERATED_MENU = 1;

	public const int SCENE_INDEX_MATCH = 2;

	public const int SCENE_INDEX_NIGHT_MATCH = 3;

	public const int SCENE_INDEX_POST_MATCH = 4;

	public const int SCENE_INDEX_PRE_MATCH = 5;

	public const byte CONST_PRESEASON_TRANSFER_WINDOW = 1;

	public const byte CONST_MIDSEASON_TRANSFER_WINDOW = 2;

	public static List<MessageTemplate> MessageTemplatesList;

	public const string SHOP_ITEM_TYPE_CREDITS = "Credits";

	public const string SHOP_ITEM_TYPE_BOOSTS = "Boosts";

	public const string SHOP_ITEM_TYPE_CASH = "Cash";

	public const string SHOP_ITEM_TYPE_XP = "XP";

	public const string SHOP_ITEM_TYPE_VIP = "VIP";

	public const byte CONST_MESSAGE_TYPE_CLUB = 0;

	public const byte CONST_MESSAGE_TYPE_NEWS = 1;

	public const byte CONST_MESSAGE_TYPE_BOTH = 2;

	public const byte CONST_CLUB_MESSAGE = 0;

	public const byte CONST_PLAYER_MESSAGE = 1;

	public const byte CONST_MATCH_MESSAGE = 2;

	public const byte CONST_MESSAGE_INFO_BOX_TYPE_CLUB = 1;

	public const byte CONST_MESSAGE_INFO_BOX_TYPE_PLAYER = 2;

	public const byte CONST_MESSAGE_INFO_BOX_TYPE_VERSUS = 3;

	public const byte CONST_MESSAGE_INFO_BOX_TYPE_CASH_INJECTION = 5;

	public const byte CONST_MESSAGE_INFO_BOX_TYPE_TRAINING_REPORT = 6;

	public const byte CONST_MESSAGE_INFO_BOX_TYPE_TRANSFER_BID = 7;

	public const byte CONST_MESSAGE_INFO_BOX_TYPE_LOAN_BID = 8;

	public const byte CONST_MESSAGE_INFO_BOX_TYPE_CONTRACT = 9;

	public const byte CONST_MESSAGE_INFO_BOX_TYPE_SCOUT_REPORT = 10;

	public const byte CONST_MESSAGE_INFO_BOX_TYPE_SIGNED_PLAYER = 11;

	public const byte CONST_MESSAGE_INFO_BOX_TYPE_JOB_VACANCIES = 13;

	public const byte CONST_MESSAGE_INFO_BOX_TYPE_JOB_OFFER = 14;

	public const int CONST_MESSAGE_INFO_PREVIEW_NEXT_MATCH = 15;

	public const byte CONST_MESSAGE_INFO_INJURY = 16;

	public const byte CONST_MESSAGE_INFO_BOX_CUP_DRAW = 17;

	public const byte CONST_MESSAGE_INFO_BOX_TYPE_PLAYER_SQUAD_NUMBER = 18;

	public const byte CONST_MESSAGE_INFO_CLUB_AWARDS = 19;

	public const byte CONST_MESSAGE_INFO_COMP_AWARDS = 20;

	public const byte CONST_MESSAGE_INFO_TEAM_AWARDS = 21;

	public const byte CONST_MESSAGE_INFO_TOP_50_AWARDS = 22;

	public const byte CONST_MESSAGE_INFO_WORLD_AWARDS = 23;

	public const byte CONST_MESSAGE_INFO_SUSPENDED = 24;

	public const byte CONST_MESSAGE_INFO_BOX_RANDEV_MESSAGES = 25;

	public const byte CONST_MESSAGE_TYPE_NEW_APPOINTMENT = 1;

	public const byte CONST_MESSAGE_TYPE_BOARD_EXPECTATIONS = 2;

	public const byte CONST_MESSAGE_TYPE_MANAGER_RESIGNED = 3;

	public const byte CONST_MESSAGE_TYPE_THIS_MANAGER_SACKED = 4;

	public const byte CONST_MESSAGE_TYPE_MANAGER_SACKED = 5;

	public const byte CONST_MESSAGE_TYPE_MANAGER_RESIGNED_FAN_REACTION = 6;

	public const byte CONST_MESSAGE_TYPE_UNEMPLOYED = 7;

	public const byte CONST_MESSAGE_TYPE_JOB_OFFER = 8;

	public const byte CONST_MESSAGE_TYPE_JOB_APPLICATION = 9;

	public const byte CONST_MESSAGE_TYPE_JOB_APPLICATION_ACCEPTED = 10;

	public const byte CONST_MESSAGE_TYPE_JOB_APPLICATION_REJECTED = 11;

	public const byte CONST_MESSAGE_TYPE_FIXTURE_LIST_PUBLISHED = 12;

	public const byte CONST_MESSAGE_TYPE_TV_MONEY_ANNOUNCED = 13;

	public const byte CONST_MESSAGE_TYPE_LOANED_OUT_PLAYERS = 14;

	public const byte CONST_MESSAGE_TYPE_FRIENDLIES_ANNOUNCED = 15;

	public const byte CONST_MESSAGE_TYPE_SEASON_TICKET_REVENUE = 16;

	public const short CONST_MESSAGE_TYPE_SEASON_STARTING_SOON = 17;

	public const byte CONST_MESSAGE_TYPE_SEASON_FINISHED = 18;

	public const byte CONST_MESSAGE_TYPE_SEASON_FINISHED_BOARD_HAPPY = 19;

	public const byte CONST_MESSAGE_TYPE_SEASON_FINISHED_BOARD_UNHAPPY = 20;

	public const byte CONST_MESSAGE_TYPE_BOARD_STATEMENT = 21;

	public const byte CONST_MESSAGE_TYPE_INSTANT_PURCHASE_CONFIRMATION = 22;

	public const byte CONST_MESSAGE_TYPE_BOARD_BUDGET_ALLOCATED = 23;

	public const byte CONST_MESSAGE_TYPE_BUDGETS_ALLOCATED_FOR_SEASON = 24;

	public const byte CONST_MESSAGE_TYPE_BUDGETS_ALLOCATED_FOR_REST_OF_SEASON = 25;

	public const byte CONST_MESSAGE_TYPE_MATCH_TODAY = 100;

	public const byte CONST_MESSAGE_TYPE_MATCH_RESULT_IN = 101;

	public const byte CONST_MESSAGE_TYPE_FRIENDLY_ARRANGED = 102;

	public const byte CONST_MESSAGE_TYPE_BOARD_HAPPY_WITH_RESULT = 103;

	public const byte CONST_MESSAGE_TYPE_BOARD_UNHAPPY_WITH_RESULT = 104;

	public const byte CONST_MESSAGE_TYPE_PLAYER_INJURED_FROM_LAST_MATCH = 105;

	public const byte CONST_MESSAGE_TYPE_PLAYER_BANNED_FROM_LAST_MATCH = 106;

	public const byte CONST_MESSAGE_TYPE_PLAYER_INJURED_FROM_TRAINING = 107;

	public const byte CONST_MESSAGE_TYPE_TRANSFER_RENEWED_CONTRACT_WITHDRAWN = 197;

	public const byte CONST_MESSAGE_TYPE_TRANSFER_RENEWED_CONTRACT_FAILED_LACK_OF_FUNDS = 198;

	public const byte CONST_MESSAGE_TYPE_TRANSFER_RENEWED_CONTRACT_ACCEPTED = 199;

	public const byte CONST_MESSAGE_TYPE_TRANSFER_RENEWED_CONTRACT_REJECTED = 200;

	public const byte CONST_MESSAGE_TYPE_TRANSFER_BID_ACCEPTED = 201;

	public const byte CONST_MESSAGE_TYPE_TRANSFER_BID_REJECTED = 202;

	public const byte CONST_MESSAGE_TYPE_TRANSFER_BID_RECEIVED = 203;

	public const byte CONST_MESSAGE_TYPE_TRANSFER_BID_SENT = 204;

	public const byte CONST_MESSAGE_TYPE_TRANSFER_SHORTLIST_BID_RECEIVED = 205;

	public const byte CONST_MESSAGE_TYPE_TRANSFER_SHORTLIST_BID_ACCEPTED = 206;

	public const byte CONST_MESSAGE_TYPE_TRANSFER_SHORTLIST_TRANSFER_LISTED = 207;

	public const byte CONST_MESSAGE_TYPE_TRANSFER_CONTRACT_NEGOTIATIONS = 208;

	public const byte CONST_MESSAGE_TYPE_TRANSFER_CONTRACT_ACCEPTED = 209;

	public const byte CONST_MESSAGE_TYPE_TRANSFER_CONTRACT_REJECTED = 211;

	public const byte CONST_MESSAGE_TYPE_TRANSFER_CONTRACT_RECEIVED = 212;

	public const byte CONST_MESSAGE_TYPE_TRANSFER_CONTRACT_WITHDRAWN = 213;

	public const byte CONST_MESSAGE_TYPE_TRANSFER_BID_ACCEPTED_PLAYER_REJECT = 214;

	public const byte CONST_MESSAGE_TYPE_PLAYER_RETIRED = 215;

	public const byte CONST_MESSAGE_TYPE_PLAYER_RATING_INCREASE = 216;

	public const byte CONST_MESSAGE_TYPE_PLAYER_RATING_DECREASE = 217;

	public const byte CONST_MESSAGE_TYPE_PLAYER_YOUTH_PROMOTION = 218;

	public const byte CONST_MESSAGE_TYPE_TRANSFERS_PLAYER_RETIRING_SOON = 219;

	public const byte CONST_MESSAGE_TYPE_TRANSFERS_PLAYER_LONG_TERM_INJURED = 220;

	public const byte CONST_MESSAGE_TYPE_TRANSFERS_PLAYER_SHORTAGE = 221;

	public const byte CONST_MESSAGE_TYPE_TRANSFERS_CHAIRMAN_ACCEPTS_BID = 222;

	public const byte CONST_MESSAGE_TYPE_TRANSFER_BID_WITHDRAWN = 223;

	public const byte CONST_MESSAGE_TYPE_PLAYER_RECALLED_FROM_LOAN = 224;

	public const byte CONST_MESSAGE_TYPE_TRANSFER_WINDOW_INFO = 225;

	public const byte CONST_MESSAGE_TYPE_INCOMING_PENDING_TRANSFER_CREATED = 226;

	public const byte CONST_MESSAGE_TYPE_OUTGOING_PENDING_TRANSFER_CREATED = 227;

	public const byte CONST_MESSAGE_TYPE_PENDING_TRANSFER_COMPLETED = 228;

	public const byte CONST_MESSAGE_TYPE_PENDING_TRANSFER_FALLEN_THROUGH = 229;

	public const byte CONST_MESSAGE_TYPE_TRANSFER_FAILED_LACK_OF_FUNDS = 230;

	public const byte CONST_MESSAGE_TYPE_TRANSFERS_LOAN_BID_ACCEPTED = 231;

	public const byte CONST_MESSAGE_TYPE_TRANSFERS_LOAN_BID_REJECTED = 232;

	public const byte CONST_MESSAGE_TYPE_LOAN_BID_RECEIVED = 233;

	public const byte CONST_MESSAGE_TYPE_TRANSFER_CONTRACT_CANCELLED = 234;

	public const byte CONST_MESSAGE_TYPE_UNMANAGED_TRANSFER_BID_PLACED = 235;

	public const byte CONST_MESSAGE_TYPE_TRANSFER_CAUSED_LINEUP_CHANGE = 236;

	public const byte CONST_MESSAGE_TYPE_TRANSFER_WINDOW_OPEN_SOON = 237;

	public const byte CONST_MESSAGE_TYPE_TRANSFER_WINDOW_CLOSED_SOON = 238;

	public const byte CONST_MESSAGE_TYPE_TRANSFERS_PLAYER_RELEASED = 239;

	public const byte CONST_MESSAGE_TYPE_LOAN_BID_SENT = 240;

	public const short CONST_MESSAGE_TYPE_INCOMING_PENDING_TRANSFER_REMINDER = 241;

	public const byte CONST_MESSAGE_TYPE_UNMANAGED_TRANSFER_COMPLETED = 242;

	public const byte CONST_MESSAGE_TYPE_UNMANAGED_TRANSFER_BID_REJECTED = 243;

	public const byte CONST_MESSAGE_TYPE_CHAIRMAN_SIGNS_PLAYER = 244;

	public const byte CONST_MESSAGE_TYPE_NO_YOUTH_PLAYERS_SQUAD_FULL = 245;

	public const byte CONST_MESSAGE_TYPE_PLAYER_LEAVES_FREE_AGENT = 246;

	public const short CONST_MESSAGE_TYPE_STAFFTRANSFER_RELEASED = 293;

	public const short CONST_MESSAGE_TYPE_STAFFTRANSFER_COMPLETED = 294;

	public const short CONST_MESSAGE_TYPE_STAFFTRANSFER_CONFIRMED = 295;

	public const short CONST_MESSAGE_TYPE_STAFFTRANSFER_BROKE_DOWN = 296;

	public const short CONST_MESSAGE_TYPE_STAFFTRANSFER_CONTRACT_NEGOTIATIONS = 297;

	public const short CONST_MESSAGE_TYPE_STAFFTRANSFER_BID_REJECTED = 298;

	public const short CONST_MESSAGE_TYPE_STAFFTRANSFER_BID_SENT = 299;

	public const short CONST_MESSAGE_TYPE_SCOUT_IDLE = 300;

	public const short CONST_MESSAGE_TYPE_SCOUT_REPORT_REQUESTED = 301;

	public const short CONST_MESSAGE_TYPE_SCOUT_REPORT_RECEIVED = 302;

	public const short CONST_MESSAGE_TYPE_SCOUT_SEARCH_BEGUN = 303;

	public const short CONST_MESSAGE_TYPE_SCOUT_SEARCH_COMPLETED = 304;

	public const short CONST_MESSAGE_TYPE_SCOUT_SEARCH_CANCELLED = 305;

	public const short CONST_MESSAGE_TYPE_SCOUT_VIP_RESPAWN = 306;

	public const short CONST_MESSAGE_TYPE_PLAYER_SCOUT_SEARCH_BEGUN = 307;

	public const short CONST_MESSAGE_TYPE_TRAINING_IDLE = 400;

	public const short CONST_MESSAGE_TYPE_TRAINING_LAST_CHANCE_1 = 401;

	public const short CONST_MESSAGE_TYPE_TRAINING_PLAYER_INJURED = 405;

	public const short CONST_MESSAGE_TYPE_CUP_BYE_RECEIVED = 500;

	public const short CONST_MESSAGE_TYPE_CUP_PROGRESS = 501;

	public const short CONST_MESSAGE_TYPE_CUP_KNOCKED_OUT = 502;

	public const short CONST_MESSAGE_TYPE_CUP_FINAL_WON = 503;

	public const short CONST_MESSAGE_TYPE_CUP_FINAL_LOST = 504;

	public const short CONST_MESSAGE_TYPE_CUP_DRAWN = 505;

	public const short CONST_MESSAGE_TYPE_CUP_FINAL_RESULT = 506;

	public const short CONST_MESSAGE_TYPE_CUP_DRAW_DUE = 507;

	public const short CONST_MESSAGE_TYPE_CONTINENTAL_CUP_GROUPS_DRAWN = 508;

	public const short CONST_MESSAGE_TYPE_CONTINENTAL_CUP_ROUND_DRAWN = 509;

	public const short CONST_MESSAGE_TYPE_CONTINENTAL_CUP_FINAL_RESULT = 510;

	public const short CONST_MESSAGE_TYPE_CONTINENTAL_CUP_FINAL_WON = 511;

	public const short CONST_MESSAGE_TYPE_TRAINING_ATTRIBUTES_CHANGE = 512;

	public const short CONST_MESSAGE_TYPE_INSTANT_PURCHASE = 513;

	public const string CONST_RANDEV_ANSWER_ID_SEPARATOR = ",";

	public const short CONST_MESSAGE_RANDEV_SIGNING_EXPENSIVE = 600;

	public const short CONST_MESSAGE_RANDEV_SIGNING_EXPENSIVE_RESP_0 = 601;

	public const short CONST_MESSAGE_RANDEV_SIGNING_EXPENSIVE_RESP_3 = 602;

	public const short CONST_MESSAGE_RANDEV_SIGNING_PLEASED = 603;

	public const short CONST_MESSAGE_RANDEV_RIVALGAME_GENERAL = 604;

	public const short CONST_MESSAGE_RANDEV_RIVALGAME_CHALLENGE = 605;

	public const short CONST_MESSAGE_RANDEV_RIVALGAME_FAVOURITES = 606;

	public const short CONST_MESSAGE_RANDEV_RIVALGAME_UNDERPERFORMING_PLAYER = 607;

	public const short CONST_MESSAGE_RANDEV_RIVALGAME_KEY_PLAYER = 608;

	public const short CONST_MESSAGE_RANDEV_RIVALGAME_DROP_PLAYER = 609;

	public const short CONST_MESSAGE_RANDEV_RIVALGAME_PERFORMANCE = 610;

	public const short CONST_MESSAGE_RANDEV_RIVALGAME_INTERESTING_RESULTS = 611;

	public const short CONST_MESSAGE_RANDEV_RIVALGAME_GENERAL_RESP_4 = 612;

	public const short CONST_MESSAGE_RANDEV_RIVALGAME_GENERAL_RESP_4_F1 = 613;

	public const short CONST_MESSAGE_RANDEV_RIVALGAME_GENERAL_RESP_4_F2 = 614;

	public const short CONST_MESSAGE_TRAINING_IMPROVED = 615;

	public const short CONST_MESSAGE_TRAINING_WORSE = 616;

	public const short CONST_MESSAGE_RANDEV_RIVALGAME_CHALLENGE_RESP_3 = 617;

	public const short CONST_MESSAGE_RANDEV_RIVALGAME_CHALLENGE_RESP_3_F1 = 618;

	public const short CONST_MESSAGE_RANDEV_RIVALGAME_FAVOURITES_RESP_0 = 619;

	public const short CONST_MESSAGE_RANDEV_RIVALGAME_UNDERPERFORMING_RESP_0 = 620;

	public const short CONST_MESSAGE_RANDEV_RIVALGAME_UNDERPERFORMING_RESP_0_F1 = 621;

	public const short CONST_MESSAGE_RANDEV_RIVALGAME_UNDERPERFORMING_RESP_2 = 622;

	public const short CONST_MESSAGE_RANDEV_RIVALGAME_UNDERPERFORMING_RESP_2_F1 = 623;

	public const short CONST_MESSAGE_RANDEV_RIVALGAME_KEY_PLAYER_RESP_0 = 624;

	public const short CONST_MESSAGE_RANDEV_RIVALGAME_KEY_PLAYER_RESP_1 = 625;

	public const short CONST_MESSAGE_RANDEV_RIVALGAME_KEY_PLAYER_RESP_3 = 626;

	public const short CONST_MESSAGE_RANDEV_RIVALGAME_KEY_PLAYER_RESP_4 = 627;

	public const short CONST_MESSAGE_RANDEV_RIVALGAME_KEY_PLAYER_RESP_0_F1 = 628;

	public const short CONST_MESSAGE_RANDEV_RIVALGAME_KEY_PLAYER_RESP_1_F1 = 629;

	public const short CONST_MESSAGE_RANDEV_RIVALGAME_KEY_PLAYER_RESP_3_F1 = 630;

	public const short CONST_MESSAGE_RANDEV_RIVALGAME_KEY_PLAYER_RESP_4_F1 = 631;

	public const short CONST_MESSAGE_RANDEV_RIVALGAME_KEY_PLAYER_RESP_1_F2 = 632;

	public const short CONST_MESSAGE_RANDEV_RIVALGAME_KEY_PLAYER_RESP_3_F2 = 633;

	public const short CONST_MESSAGE_RANDEV_RIVALGAME_KEY_PLAYER_RESP_4_F2 = 634;

	public const short CONST_MESSAGE_RANDEV_RIVALGAME_KEY_PLAYER_RESP_3_F3 = 635;

	public const short CONST_MESSAGE_RANDEV_RIVALGAME_DROP_PLAYER_RESP_0 = 636;

	public const short CONST_MESSAGE_RANDEV_RIVALGAME_DROP_PLAYER_RESP_3 = 637;

	public const short CONST_MESSAGE_RANDEV_RIVALGAME_DROP_PLAYER_RESP_4 = 638;

	public const short CONST_MESSAGE_RANDEV_RIVALGAME_DROP_PLAYER_RESP_0_F1 = 639;

	public const short CONST_MESSAGE_RANDEV_RIVALGAME_DROP_PLAYER_RESP_3_F1 = 640;

	public const short CONST_MESSAGE_RANDEV_RIVALGAME_DROP_PLAYER_RESP_4_F1 = 641;

	public const short CONST_MESSAGE_RANDEV_RIVALGAME_DROP_PLAYER_RESP_0_F2 = 642;

	public const short CONST_MESSAGE_RANDEV_RIVALGAME_DROP_PLAYER_RESP_3_F2 = 643;

	public const short CONST_MESSAGE_RANDEV_RIVALGAME_DROP_PLAYER_RESP_4_F2 = 644;

	public const short CONST_MESSAGE_RANDEV_RIVALGAME_PERFORMANCE_RESP_3 = 645;

	public const short CONST_MESSAGE_RANDEV_RIVALGAME_PERFORMANCE_RESP_3_F1 = 646;

	public const short CONST_MESSAGE_RANDEV_RIVALGAME_INTERESTING_RESULTS_RESP_0 = 647;

	public const short CONST_MESSAGE_RANDEV_RIVALGAME_INTERESTING_RESULTS_RESP_2 = 648;

	public const short CONST_MESSAGE_RANDEV_RIVALGAME_INTERESTING_RESULTS_RESP_0_F1 = 649;

	public const short CONST_MESSAGE_RANDEV_RIVALGAME_INTERESTING_RESULTS_RESP_2_F1 = 650;

	public const short CONST_MESSAGE_RANDEV_CUP_DRAW = 651;

	public const short CONST_MESSAGE_RANDEV_CUP_DRAW_0_4 = 652;

	public const short CONST_MESSAGE_RANDEV_CUP_DRAW_1_4 = 654;

	public const short CONST_MESSAGE_RANDEV_CUP_DRAW_7_0 = 655;

	public const short CONST_MESSAGE_RANDEV_CUP_DRAW_8_0 = 657;

	public const short CONST_MESSAGE_RANDEV_GOALS = 658;

	public const short CONST_MESSAGE_RANDEV_GOALS_2_4 = 659;

	public const short CONST_MESSAGE_RANDEV_DISCIPLINE_RESP_0 = 660;

	public const short CONST_MESSAGE_RANDEV_DISCIPLINE_RESP_3 = 661;

	public const short CONST_MESSAGE_RANDEV_DISCIPLINE_RESP_4 = 662;

	public const short CONST_MESSAGE_RANDEV_DISCIPLINE_RESP_0_F1 = 663;

	public const short CONST_MESSAGE_RANDEV_DISCIPLINE_RESP_3_F1 = 664;

	public const short CONST_MESSAGE_RANDEV_DISCIPLINE_RESP_4_F1 = 665;

	public const short CONST_MESSAGE_RANDEV_DISCIPLINE_RESP_0_F2 = 666;

	public const short CONST_MESSAGE_RANDEV_DISCIPLINE_RESP_3_F2 = 667;

	public const short CONST_MESSAGE_RANDEV_DISCIPLINE_RESP_4_F2 = 668;

	public const short CONST_MESSAGE_RANDEV_DISCIPLINE_TRAINING = 669;

	public const short CONST_MESSAGE_RANDEV_DISCIPLINE_NIGHTCLUB = 670;

	public const short CONST_MESSAGE_RANDEV_DISCIPLINE_TRAINING_RESP_0 = 671;

	public const short CONST_MESSAGE_RANDEV_DISCIPLINE_TRAINING_RESP_1 = 672;

	public const short CONST_MESSAGE_RANDEV_DISCIPLINE_TRAINING_RESP_2 = 673;

	public const short CONST_MESSAGE_RANDEV_DISCIPLINE_TRAINING_RESP_3 = 674;

	public const short CONST_MESSAGE_RANDEV_DISCIPLINE_TRAINING_RESP_4 = 675;

	public const short CONST_MESSAGE_RANDEV_DISCIPLINE_TRAINING_RESP_5 = 676;

	public const short CONST_MESSAGE_RANDEV_DISCIPLINE_NIGHTCLUB_RESP_0 = 677;

	public const short CONST_MESSAGE_RANDEV_DISCIPLINE_NIGHTCLUB_RESP_1 = 678;

	public const short CONST_MESSAGE_RANDEV_DISCIPLINE_NIGHTCLUB_RESP_2 = 679;

	public const short CONST_MESSAGE_RANDEV_DISCIPLINE_NIGHTCLUB_RESP_3 = 680;

	public const short CONST_MESSAGE_RANDEV_DISCIPLINE_NIGHTCLUB_RESP_4 = 681;

	public const short CONST_MESSAGE_RANDEV_DISCIPLINE_NIGHTCLUB_RESP_5 = 682;

	public const short CONST_MESSAGE_RANDEV_LOW_BALANCE = 683;

	public const short CONST_MESSAGE_TYPE_MEDICAL_CENTRE_AGREEMENT = 700;

	public const short CONST_MESSAGE_TYPE_MEDICAL_CENTRE_COMPLETION = 701;

	public const short CONST_MESSAGE_TYPE_YOUTH_ACADEMY_AGREEMENT = 702;

	public const short CONST_MESSAGE_TYPE_YOUTH_ACADEMY_COMPLETION = 703;

	public const short CONST_MESSAGE_TYPE_TRAINING_COMPLEX_AGREEMENT = 704;

	public const short CONST_MESSAGE_TYPE_TRAINING_COMPLEX_COMPLETION = 705;

	public const short CONST_MESSAGE_TYPE_STADIUM_EXPANSION_AGREEMENT = 706;

	public const short CONST_MESSAGE_TYPE_STADIUM_EXPANSION_COMPLETION = 707;

	public const short CONST_MESSAGE_TYPE_SCOUTING_NETWORK_AGREEMENT = 708;

	public const short CONST_MESSAGE_TYPE_SCOUTING_NETWORK_COMPLETION = 709;

	public const short CONST_MESSAGE_TYPE_FACILITY_UPGRADE_PROMPT = 710;

	public const short CONST_MESSAGE_TYPE_CHECK_APPROVAL_RATING_PROMPT = 711;

	public const short CONST_MESSAGE_TYPE_COMPS_PROMOTED = 800;

	public const short CONST_MESSAGE_TYPE_COMPS_RELEGATED = 801;

	public const short CONST_MESSAGE_TYPE_COMPS_WON_LEAGUE = 802;

	public const short CONST_MESSAGE_TYPE_COMPS_WON_CUP = 803;

	public const short CONST_MESSAGE_TYPE_CHAIRMAN_INVESTS_1 = 806;

	public const short CONST_MESSAGE_TYPE_LAST_SEASON_WARNING = 810;

	public const short CONST_MESSAGE_TYPE_GOLDEN_BOOT = 900;

	public const short CONST_MESSAGE_TYPE_GOLDEN_GLOVE = 901;

	public const short CONST_MESSAGE_TYPE_MANAGER_MONTH = 902;

	public const short CONST_MESSAGE_TYPE_MANAGER_YEAR = 903;

	public const short CONST_MESSAGE_TYPE_TEAM_MONTH = 904;

	public const short CONST_MESSAGE_TYPE_PLAYER_MONTH = 905;

	public const short CONST_MESSAGE_TYPE_YOUNG_PLAYER_MONTH = 906;

	public const short CONST_MESSAGE_TYPE_PLAYERS_WANT_MORE_GAMES = 1000;

	public const short CONST_MESSAGE_TYPE_PLAYERS_UPSET_WITH_NUM_GAMES = 1001;

	public const short CONST_MESSAGE_TYPE_PLAYERS_NOT_HAPPY_WITH_NUM_GAMES = 1002;

	public const short CONST_MESSAGE_TYPE_PLAYERS_ANGRY_WITH_NUM_GAMES = 1003;

	public const short CONST_MESSAGE_TYPE_TEAM_SEASON = 1004;

	public const short CONST_MESSAGE_TYPE_CLAIM_REWARD = 1005;

	public const short CONST_MESSAGE_TYPE_NEW_JOBS_AVAILABLE = 1006;

	public const short CONST_MESSAGE_TYPE_JOB_APPLICATION_ACCEPT = 1007;

	public const short CONST_MESSAGE_TYPE_JOB_APPLICATION_REJECT = 1008;

	public const short CONST_MESSAGE_SCOUT_NEXT_OPPOSITION = 1009;

	public const short CONST_MESSAGE_PLAYER_CONTRACT_EXPIRY = 1010;

	public const short CONST_MESSAGE_PLAYERS_LOW_FITNESS = 1011;

	public const short CONST_MESSAGE_WAGE_BILL_EXCEEDING_INCOME = 1012;

	public const short CONST_MESSAGE_TYPE_CLUB_PLAYER_MONTH = 1100;

	public const short CONST_MESSAGE_TYPE_LEAGUE_PLAYER_MONTH = 1101;

	public const short CONST_MESSAGE_TYPE_LEAGUE_TEAM_MONTH = 1102;

	public const short CONST_MESSAGE_TYPE_CLUB_PLAYER_YEAR = 1103;

	public const short CONST_MESSAGE_TYPE_LEAGUE_PLAYER_YEAR = 1104;

	public const short CONST_MESSAGE_TYPE_LEAGUE_YOUNG_PLAYER_YEAR = 1105;

	public const short CONST_MESSAGE_TYPE_LEAGUE_GOLDEN_BOOT = 1106;

	public const short CONST_MESSAGE_TYPE_LEAGUE_GOLDEN_GLOVE = 1107;

	public const short CONST_MESSAGE_TYPE_LEAGUE_PLAYMAKER = 1108;

	public const short CONST_MESSAGE_TYPE_LEAGUE_TEAM_YEAR = 1109;

	public const short CONST_MESSAGE_TYPE_CONTINENTAL_TEAM_YEAR = 1110;

	public const short CONST_MESSAGE_TYPE_WORLD_PLAYER_YEAR = 1111;

	public const short CONST_MESSAGE_TYPE_WOLRD_GOALKEEPER_YEAR = 1112;

	public const short CONST_MESSAGE_TYPE_GOAL50 = 1113;

	public const short CONST_MESSAGE_TYPE_NXGN_BEST_U19 = 1114;

	public const byte NO_SPONSOR = 0;

	public const byte SPONSOR_90MIN = 1;

	public const byte SPONSOR_KAPPA = 2;

	public const byte SPONSOR_TRIBUNA = 3;

	public const byte SPONSOR_GOAL = 4;

	public const byte SPONSOR_FOOTBOLE = 5;

	public const byte SPONSOR_FOOTBALL_DAILY = 6;

	public const byte SPONSOT_XAYA = 7;

	public const byte SPONSOR_LOTTO = 8;

	public const byte TOTAL_SPONSORS = 8;

	public const string PRODUCT_ID_STARTER_1 = "sm21_bundle_0";

	public const string PRODUCT_ID_BUNDLE_1_0 = "sm21_bundle_1_0";

	public const string PRODUCT_ID_BUNDLE_2_0 = "sm21_bundle_2_0";

	public const string PRODUCT_ID_BUNDLE_3_0 = "sm21_bundle_3_0";

	public const string PRODUCT_ID_BUNDLE_4_0 = "sm21_bundle_4_0";

	public const string PRODUCT_ID_BUNDLE_1 = "sm21_bundle_1";

	public const string PRODUCT_ID_BUNDLE_2 = "sm21_bundle_2";

	public const string PRODUCT_ID_BUNDLE_3 = "sm21_bundle_3";

	public const string PRODUCT_ID_BUNDLE_4 = "sm21_bundle_4";

	public const string PRODUCT_ID_CREDITS_1 = "sm21_credits_1";

	public const string PRODUCT_ID_CREDITS_2 = "sm21_credits_2";

	public const string PRODUCT_ID_CREDITS_3 = "sm21_credits_3";

	public const string PRODUCT_ID_CREDITS_4 = "sm21_credits_4";

	public const string PRODUCT_ID_CREDITS_5 = "sm21_credits_5";

	public const string PRODUCT_ID_CREDITS_6 = "sm21_credits_6";

	public const string PRODUCT_ID_NONIAP_BOOST_1 = "sm21_boost_1";

	public const string PRODUCT_ID_NONIAP_BOOST_2 = "sm21_boost_2";

	public const string PRODUCT_ID_NONIAP_BOOST_3 = "sm21_boost_3";

	public const string PRODUCT_ID_NONIAP_BOOST_4 = "sm21_boost_4";

	public const string PRODUCT_ID_NONIAP_MANPOINT_1 = "sm21_manpoint_1";

	public const string PRODUCT_ID_NONIAP_MANPOINT_2 = "sm21_manpoint_2";

	public const string PRODUCT_ID_NONIAP_MANPOINT_3 = "sm21_manpoint_3";

	public const string PRODUCT_ID_NONIAP_MANPOINT_4 = "sm21_manpoint_4";

	public const string PRODUCT_ID_FB_BUNDLE_1 = "smc_fb_bundle_1";

	public const string PRODUCT_ID_INT_ENTRY_FREE = "sm_int_entry_fee";

	public const string PRODUCT_ID_INT_7_DAY_PASS = "sm_int_7_day_pass";

	public const string PRODUCT_ID_INT_30_DAY_PASS = "sm_int_30_day_pass";

	public const string PRODUCT_ID_VIP = "sm21_vip";

	public const string PRODUCT_ID_VIP_Light = "sm21_vip_light";

	public static int[] GetAllPlayerPositions()
	{
		return new int[16]
		{
			1, 2, 4, 8, 16, 32, 64, 128, 256, 512,
			1024, 2048, 4096, 8192, 16384, 32768
		};
	}

	public static byte[] GetAllPlayerDescriptions()
	{
		return new byte[21]
		{
			1, 2, 10, 11, 12, 13, 14, 20, 21, 22,
			23, 24, 25, 27, 28, 29, 40, 41, 42, 43,
			44
		};
	}

	public static byte[] GetScreenGroupMembers(eScreenGroup group)
	{
		return group switch
		{
			eScreenGroup.HOME_SCREEN => HOME_SCREEN_GROUP, 
			eScreenGroup.SQUAD_SCREEN => SQUAD_SCREEN_GROUP, 
			eScreenGroup.MANAGER_SCREEN => MANAGER_SCREEN_GROUP, 
			eScreenGroup.FINANCE_SCREEN => FINANCE_SCREEN_GROUP, 
			eScreenGroup.TACTICS_SCREEN => TACTICS_SCREEN_GROUP, 
			eScreenGroup.YOUTH_ACADEMY_SCREEN => YOUTH_SCREEN_GROUP, 
			eScreenGroup.SCOUTING_SCREEN => SCOUTING_SCREEN_GROUP, 
			eScreenGroup.TRAINING_SCREEN => TRAINING_SCREEN_GROUP, 
			eScreenGroup.TRANSFERS_SCREEN => TRANSFERS_SCREEN_GROUP, 
			eScreenGroup.CLUBZONE_SCREEN => CLUBZONE_SCREEN_GROUP, 
			eScreenGroup.LEAGUE_SCREEN => LEAGUE_SCREEN_GROUP, 
			eScreenGroup.DOMESTICCUP_SCREEN => DOMESTICCUP_SCREEN_GROUP, 
			eScreenGroup.CONTINENTALCUP_SCREEN => CONTINENTALCUP_SCREEN_GROUP, 
			eScreenGroup.SHOP_SCREEN => SHOP_SCREEN_GROUP, 
			eScreenGroup.SCHEDULES_SCREEN => SCHEDULE_SCREEN_GROUP, 
			eScreenGroup.SEARCHES_SCREEN => SEARCHES_SCREEN_GROUP, 
			_ => null, 
		};
	}

	public static KeyValuePair<bool, eScreenGroup> IsMemberOfAnyScreenGroup(byte screenID)
	{
		new KeyValuePair<bool, eScreenGroup>(key: false, eScreenGroup.HOME_SCREEN);
		Array values = Enum.GetValues(typeof(eScreenGroup));
		int num = 0;
		for (int i = 0; i < values.Length; i++)
		{
			byte[] screenGroupMembers = GetScreenGroupMembers((eScreenGroup)values.GetValue(i));
			for (num = 0; num < screenGroupMembers.Length; num++)
			{
				if (screenGroupMembers[num] == screenID)
				{
					return new KeyValuePair<bool, eScreenGroup>(key: true, (eScreenGroup)values.GetValue(i));
				}
			}
		}
		return new KeyValuePair<bool, eScreenGroup>(key: false, eScreenGroup.HOME_SCREEN);
	}

	public static bool IsMemberOfScreenGroup(byte screenID, eScreenGroup screenGroup)
	{
		byte[] screenGroupMembers = GetScreenGroupMembers(screenGroup);
		if (screenGroupMembers == null)
		{
			return false;
		}
		for (int i = 0; i < screenGroupMembers.Length; i++)
		{
			if (screenGroupMembers[i] == screenID)
			{
				return true;
			}
		}
		return false;
	}
}
