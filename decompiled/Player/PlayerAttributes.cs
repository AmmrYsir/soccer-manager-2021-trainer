using System;
using System.Collections.Generic;

public class PlayerAttributes
{
	public double[] attrs;

	public float[] attrsDisplay;

	public float[] groupedAttrs;

	public List<int[]> orderedAttrsVotes;

	public int desc;

	public bool isKeeper;

	public float GetAttribute(byte attributeID)
	{
		return attrsDisplay[attributeID];
	}

	public float GetGroupedAttribute(GroupedGKAttributes attributeID)
	{
		if (!isKeeper)
		{
			throw new Exception("Trying to get goalkeeper attribute when player is not a goalkeeper");
		}
		return groupedAttrs[(int)attributeID];
	}

	public float GetGroupedAttribute(GroupedOutfieldAttributes attributeID)
	{
		if (isKeeper)
		{
			throw new Exception("Trying to get outfield attribute when player is a goalkeeper");
		}
		return groupedAttrs[(int)attributeID];
	}

	public float GetOverallRating()
	{
		if (isKeeper)
		{
			return GetGroupedAttribute(GroupedGKAttributes.Overall);
		}
		return GetGroupedAttribute(GroupedOutfieldAttributes.Overall);
	}
}
