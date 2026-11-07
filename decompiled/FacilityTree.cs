using System;
using System.Collections.Generic;

[Serializable]
public class FacilityTree
{
	public List<FacilityLevel> Levels;

	public FacilityTree(List<FacilityLevel> Levels)
	{
		this.Levels = Levels;
	}
}
