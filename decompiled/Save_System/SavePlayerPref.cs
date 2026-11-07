using System.Collections.Generic;

public class SavePlayerPref : PlayerPrefsSerializer
{
	public Dictionary<string, int> managerLevels;

	public void SetNewLevel(string ID, int level)
	{
		managerLevels[ID] = level;
	}
}
