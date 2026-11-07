using System.Collections.Generic;

public class FacilityManager
{
	public bool FacilityUnlockInProgress;

	public List<FacilityUnlock> FacilityUnlocksInProgress = new List<FacilityUnlock>();

	public Dictionary<int, int> FacilityBlobs = new Dictionary<int, int>();

	public Dictionary<int, int> FacilityLevels = new Dictionary<int, int>();

	public FacilityUnlock RewardAdUnlock = new FacilityUnlock();

	public bool FacilityRedraw = true;

	public FacilityManager()
	{
		FacilityUnlockInProgress = false;
		FacilityUnlocksInProgress = new List<FacilityUnlock>();
		FacilityBlobs = new Dictionary<int, int>();
		FacilityLevels = new Dictionary<int, int>
		{
			{ 2, 0 },
			{ 1, 0 },
			{ 4, 0 },
			{ 0, 0 }
		};
		RewardAdUnlock = new FacilityUnlock();
	}
}
