using System.Collections.Generic;

public class UserManagerProfile
{
	public string ManagerName = string.Empty;

	public int ManagerPoints;

	public Dictionary<short, TrophyList> TrophiesEarned = new Dictionary<short, TrophyList>();

	public ManagerType BaseManagerType { get; set; }

	public List<UsersManagerTraits> UsersObtainedTraits { get; set; } = new List<UsersManagerTraits>();

	public int ManagerXP { get; set; }

	public int ManagerLevel { get; set; }

	public int Boosts { get; set; }

	public int Credits { get; set; }

	public bool VIP { get; set; }

	public Dictionary<string, CareerEntryList> CareerHistory { get; set; } = new Dictionary<string, CareerEntryList>();
}
