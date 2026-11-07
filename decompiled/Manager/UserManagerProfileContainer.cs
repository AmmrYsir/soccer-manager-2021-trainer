using System.Collections.Generic;
using Newtonsoft.Json;

public class UserManagerProfileContainer
{
	[JsonProperty("RewardVideoFreePackDate")]
	public long RewardVideoFreePackDate { get; set; }

	[JsonProperty("ReviewFlag")]
	[JsonConverter(typeof(FluffyParseStringConverter))]
	public bool ReviewFlag { get; set; }

	[JsonProperty("SpecialOfferExpiryTime")]
	public long SpecialOfferExpiryTime { get; set; }

	[JsonProperty("SpecialOfferEndTime")]
	public long SpecialOfferEndTime { get; set; }

	[JsonProperty("SpecialOfferType")]
	public string SpecialOfferType { get; set; }

	[JsonProperty("AdvertisingID")]
	public string AdvertisingID { get; set; } = "0";

	[JsonProperty("NewsFeedLastViewed")]
	public long NewsFeedLastViewed { get; set; }

	[JsonProperty("ManagerName")]
	public string ManagerUserName { get; set; } = "";

	[JsonProperty("BaseManagerType")]
	public ManagerType BaseManagerType { get; set; }

	[JsonProperty("UsersObtainedTraits")]
	public List<UsersManagerTraits> UsersObtainedTraits { get; set; } = new List<UsersManagerTraits>();

	[JsonProperty("ManagerXP")]
	public int ManagerXP { get; set; }

	[JsonProperty("ManagerPoints")]
	public int ManagerPoints { get; set; }

	[JsonProperty("CareerHistory")]
	public Dictionary<string, CareerEntryList> CareerHistory { get; set; } = new Dictionary<string, CareerEntryList>();

	[JsonProperty("TrophiesEarned")]
	public Dictionary<short, TrophyList> TrophiesEarned { get; set; } = new Dictionary<short, TrophyList>();

	[JsonProperty("HasAcceptedTargetedAds")]
	[JsonConverter(typeof(FluffyParseStringConverter))]
	public bool HasAcceptedTargetedAds { get; set; }

	[JsonProperty("IsOver18")]
	[JsonConverter(typeof(FluffyParseStringConverter))]
	public bool IsOver18 { get; set; }

	[JsonProperty("DataPackHolder")]
	public DataPackContainer DataPackHolder { get; set; } = new DataPackContainer();

	[JsonProperty("CustomerDeviceData")]
	public CustomerDeviceData CustomerDeviceData { get; set; } = new CustomerDeviceData();
}
