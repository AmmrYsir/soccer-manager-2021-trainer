using System;

[Serializable]
public class UserManagerLevel
{
	public int LevelID { get; set; }

	public int XPRequired { get; set; }

	public UserManagerLevelReward LevelReward { get; set; }
}
