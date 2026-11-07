using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHiddenAttributes
{
	private static readonly Dictionary<byte, byte> HandlingPressureAgeIncrease = new Dictionary<byte, byte>
	{
		{ 22, 1 },
		{ 23, 1 },
		{ 24, 1 },
		{ 25, 1 },
		{ 26, 1 }
	};

	public static readonly (int Min, int Max) MinMax = (Min: 1, Max: 10);

	private static readonly (int Low, int High) AmbitionThreshold = (Low: 3, High: 8);

	private static readonly (int Low, int High) LoyaltyThreshold = (Low: 3, High: 8);

	private static readonly (int Low, int High) HandlingPressureThreshold = (Low: 3, High: 8);

	private static readonly (int Min, int Max) LoyaltyRandomnessAgeRange = (Min: 20, Max: 30);

	private byte rawLoyalty;

	public byte Ambition { get; set; }

	public byte Loyalty { get; set; }

	public byte HandlingPressure { get; set; }

	public AttributeDiscreteValue AmbitionDiscreteValue => GetAttributeDiscreteValue(Ambition, AmbitionThreshold);

	public AttributeDiscreteValue LoyaltyDiscreteValue => GetAttributeDiscreteValue(Loyalty, LoyaltyThreshold);

	public AttributeDiscreteValue HandlingPressureDiscreteValue => GetAttributeDiscreteValue(HandlingPressure, HandlingPressureThreshold);

	public static void ProcessAnnual()
	{
		foreach (int key in Game.instance.g_PlayerData.Keys)
		{
			if (!Player.IsRetired(key))
			{
				byte handlingPressureIncrement = GetHandlingPressureIncrement(Player.GetAge(key));
				if (handlingPressureIncrement > 0)
				{
					Game.instance.g_PlayerData[key].HiddenAttributes.HandlingPressure += handlingPressureIncrement;
				}
			}
		}
	}

	private AttributeDiscreteValue GetAttributeDiscreteValue(byte attribute, (int Low, int High) lowHighThreshold)
	{
		if (attribute <= lowHighThreshold.Low)
		{
			return AttributeDiscreteValue.Low;
		}
		if (attribute >= lowHighThreshold.High)
		{
			return AttributeDiscreteValue.High;
		}
		return AttributeDiscreteValue.Medium;
	}

	public void Init(int playerId)
	{
		PlayerData playerData = Game.instance.g_PlayerData[playerId];
		byte age = Player.GetAge(playerId);
		Ambition = GenerateAmbition(playerData.Potential);
		Loyalty = GenerateLoyalty(age);
		HandlingPressure = GenerateHandlingPressure(age);
	}

	public void InitRegen(int playerID)
	{
		RNG random = Game.instance.Random;
		Ambition = (byte)random.GetRand(MinMax.Min, MinMax.Max);
		Loyalty = (byte)random.GetRand(MinMax.Min, MinMax.Max);
		HandlingPressure = GenerateHandlingPressure(Player.GetAge(playerID));
	}

	public void SetRawLoyalty(byte rawLoyalty)
	{
		this.rawLoyalty = rawLoyalty;
	}

	protected byte GenerateAmbition(int playerPotential)
	{
		RNG random = Game.instance.Random;
		return (byte)Mathf.Clamp((float)(playerPotential - 50) / 5f + (float)random.GetRand(0, 4) - 2f, MinMax.Min, MinMax.Max);
	}

	protected byte GenerateHandlingPressure(byte playerAge)
	{
		int num = Game.instance.Random.GetRand(1, 5);
		foreach (KeyValuePair<byte, byte> item in HandlingPressureAgeIncrease)
		{
			int key = item.Key;
			int value = item.Value;
			if (playerAge >= key)
			{
				num += value;
			}
		}
		return (byte)num;
	}

	protected byte GenerateLoyalty(byte playerAge)
	{
		RNG random = Game.instance.Random;
		byte value;
		if (playerAge < LoyaltyRandomnessAgeRange.Min)
		{
			value = (byte)random.GetRand(MinMax.Min, MinMax.Max);
		}
		else if (playerAge < LoyaltyRandomnessAgeRange.Max)
		{
			int rand = random.GetRand(0, Math.Max(LoyaltyRandomnessAgeRange.Max - playerAge, MinMax.Max));
			value = (byte)(rawLoyalty + rand);
		}
		else
		{
			value = rawLoyalty;
		}
		return (byte)Mathf.Clamp(value, MinMax.Min, MinMax.Max);
	}

	private static byte GetHandlingPressureIncrement(byte playerAge)
	{
		if (HandlingPressureAgeIncrease.ContainsKey(playerAge))
		{
			return HandlingPressureAgeIncrease[playerAge];
		}
		return 0;
	}
}
