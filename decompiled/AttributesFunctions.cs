using System.Collections.Generic;

public class AttributesFunctions
{
	public static int GetAttributeType(int attr, bool isKeeper)
	{
		return (isKeeper ? Attributes.keeperAttributeTypes : Attributes.outfieldAttributeTypes)[attr];
	}

	public static int[] GetTechnicalAttributesDisplayOrder(bool isKeeper)
	{
		if (!isKeeper)
		{
			return new int[15]
			{
				10, 11, 12, 14, 15, 16, 17, 18, 19, 20,
				21, 24, 23, 22, 13
			};
		}
		return new int[7] { 10, 11, 12, 13, 14, 15, 16 };
	}

	public static int[] GetPhysicalAttributesDisplayOrder(bool isKeeper)
	{
		if (!isKeeper)
		{
			return new int[5] { 6, 4, 25, 5, 27 };
		}
		return new int[3] { 6, 4, 5 };
	}

	public static int[] GetMentalAttributesDisplayOrder(bool isKeeper)
	{
		if (!isKeeper)
		{
			return new int[5] { 0, 1, 2, 7, 8 };
		}
		return new int[4] { 0, 1, 2, 8 };
	}

	public static string GetHiddenAttributeDisplayString()
	{
		return "??";
	}

	public static List<int> GetAllAttrributesOfType(int type, bool isKeeper)
	{
		Dictionary<int, int> dictionary = (isKeeper ? Attributes.keeperAttributeTypes : Attributes.outfieldAttributeTypes);
		List<int> list = new List<int>();
		foreach (KeyValuePair<int, int> item in dictionary)
		{
			int key = item.Key;
			if (item.Value == type)
			{
				list.Add(key);
			}
		}
		return list;
	}

	public static string GetAttributeName(int attr, bool isKeeper)
	{
		if (isKeeper)
		{
			return attr switch
			{
				12 => Translation.GetText("attr-handling"), 
				13 => Translation.GetText("attr-oneOnOnes"), 
				14 => Translation.GetText("attr-shotStopping"), 
				15 => Translation.GetText("attr-positioning"), 
				16 => Translation.GetText("attr-savingPenalties"), 
				1 => Translation.GetText("attr-composure"), 
				4 => Translation.GetText("attr-pace"), 
				5 => Translation.GetText("attr-strength"), 
				6 => Translation.GetText("attr-aerialAbility"), 
				0 => Translation.GetText("attr-aggression"), 
				2 => Translation.GetText("attr-concentration"), 
				8 => Translation.GetText("attr-leadership"), 
				11 => Translation.GetText("attr-distribution"), 
				_ => Translation.GetText("attr-commandOfArea"), 
			};
		}
		return attr switch
		{
			11 => Translation.GetText("attr-tackling"), 
			12 => Translation.GetText("attr-positioning"), 
			14 => Translation.GetText("attr-movement"), 
			15 => Translation.GetText("attr-finishing"), 
			16 => Translation.GetText("attr-dribbling"), 
			17 => Translation.GetText("attr-longshots"), 
			18 => Translation.GetText("attr-crossing"), 
			1 => Translation.GetText("attr-composure"), 
			19 => Translation.GetText("attr-control"), 
			20 => Translation.GetText("attr-longPassing"), 
			21 => Translation.GetText("attr-passing"), 
			4 => Translation.GetText("attr-pace"), 
			5 => Translation.GetText("attr-strength"), 
			6 => Translation.GetText("attr-aerialAbility"), 
			25 => Translation.GetText("attr-stamina"), 
			27 => Translation.GetText("attr-workrate"), 
			0 => Translation.GetText("attr-aggression"), 
			2 => Translation.GetText("attr-concentration"), 
			7 => Translation.GetText("attr-flair"), 
			8 => Translation.GetText("attr-leadership"), 
			13 => Translation.GetText("attr-creativity"), 
			24 => Translation.GetText("attr-penalties"), 
			23 => Translation.GetText("attr-freekicks"), 
			22 => Translation.GetText("attr-corners"), 
			_ => Translation.GetText("attr-marking"), 
		};
	}

	public static string GetAttributeName(GroupedGKAttributes attr)
	{
		return attr switch
		{
			GroupedGKAttributes.Defense => Translation.GetText("defending"), 
			GroupedGKAttributes.Saving => Translation.GetText("attr-shotStopping"), 
			GroupedGKAttributes.Passing => Translation.GetText("attr-passing"), 
			GroupedGKAttributes.OneOnOne => Translation.GetText("attr-oneOnOnes"), 
			GroupedGKAttributes.Aerial => Translation.GetText("attr-aerialAbility"), 
			_ => Translation.GetText("attacking"), 
		};
	}

	public static string GetAttributeName(GroupedOutfieldAttributes attr)
	{
		return attr switch
		{
			GroupedOutfieldAttributes.Defense => Translation.GetText("defending"), 
			GroupedOutfieldAttributes.Shooting => Translation.GetText("shooting"), 
			GroupedOutfieldAttributes.Passing => Translation.GetText("attr-passing"), 
			GroupedOutfieldAttributes.Dribbling => Translation.GetText("attr-dribbling"), 
			GroupedOutfieldAttributes.Strength => Translation.GetText("attr-strength"), 
			GroupedOutfieldAttributes.Tackling => Translation.GetText("attr-tackling"), 
			GroupedOutfieldAttributes.Pace => Translation.GetText("attr-pace"), 
			GroupedOutfieldAttributes.Overall => Translation.GetText("overall"), 
			_ => Translation.GetText("attacking"), 
		};
	}

	public static string GetAttributeShortName(GroupedGKAttributes attr)
	{
		return GetAttributeName(attr);
	}

	public static string GetAttributeShortName(GroupedOutfieldAttributes attr)
	{
		return GetAttributeName(attr);
	}
}
