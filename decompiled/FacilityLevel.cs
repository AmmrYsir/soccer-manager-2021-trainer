using System;
using System.Collections.Generic;

[Serializable]
public class FacilityLevel
{
	public byte Level;

	public int Cost;

	public int DaysToComplete;

	public int ManagerLevelRequired;

	public Dictionary<byte, float> Benefits;

	public FacilityLevel()
	{
	}

	public FacilityLevel(byte Level, int Cost, int DaysToComplete, int ManagerLevelRequired, Dictionary<byte, float> Benefits)
	{
		this.Level = Level;
		this.Cost = Cost;
		this.DaysToComplete = DaysToComplete;
		this.ManagerLevelRequired = ManagerLevelRequired;
		this.Benefits = Benefits;
	}

	public int GetDaysToComplete()
	{
		return 14 + 14 * Level;
	}
}
