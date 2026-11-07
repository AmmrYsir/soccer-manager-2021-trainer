using System;
using System.Collections.Generic;

public class PlayerData : PlayerStaticData
{
	public const int YouthPlayerMaxAge = 22;

	public const byte CONST_POTENTIAL_AGE_RATING_60_TO_93 = 0;

	public const byte CONST_POTENTIAL_AGE_RATING_94_TO_99 = 1;

	public static Dictionary<byte, byte[][,]> _potentialAgeData = new Dictionary<byte, byte[][,]>
	{
		{
			1,
			new byte[2][,]
			{
				new byte[9, 2]
				{
					{ 24, 1 },
					{ 25, 2 },
					{ 26, 5 },
					{ 27, 10 },
					{ 28, 20 },
					{ 29, 40 },
					{ 30, 10 },
					{ 31, 5 },
					{ 32, 2 }
				},
				new byte[9, 2]
				{
					{ 24, 1 },
					{ 25, 1 },
					{ 26, 2 },
					{ 27, 5 },
					{ 28, 20 },
					{ 29, 40 },
					{ 30, 10 },
					{ 31, 5 },
					{ 32, 2 }
				}
			}
		},
		{
			2,
			new byte[2][,]
			{
				new byte[9, 2]
				{
					{ 24, 1 },
					{ 25, 2 },
					{ 26, 5 },
					{ 27, 10 },
					{ 28, 20 },
					{ 29, 40 },
					{ 30, 10 },
					{ 31, 5 },
					{ 32, 2 }
				},
				new byte[9, 2]
				{
					{ 24, 1 },
					{ 25, 1 },
					{ 26, 2 },
					{ 27, 5 },
					{ 28, 20 },
					{ 29, 40 },
					{ 30, 10 },
					{ 31, 5 },
					{ 32, 2 }
				}
			}
		},
		{
			10,
			new byte[2][,]
			{
				new byte[10, 2]
				{
					{ 23, 1 },
					{ 24, 1 },
					{ 25, 5 },
					{ 26, 10 },
					{ 27, 20 },
					{ 28, 40 },
					{ 29, 20 },
					{ 30, 10 },
					{ 31, 5 },
					{ 32, 2 }
				},
				new byte[10, 2]
				{
					{ 23, 1 },
					{ 24, 1 },
					{ 25, 2 },
					{ 26, 5 },
					{ 27, 10 },
					{ 28, 40 },
					{ 29, 20 },
					{ 30, 10 },
					{ 31, 5 },
					{ 32, 2 }
				}
			}
		},
		{
			11,
			new byte[2][,]
			{
				new byte[10, 2]
				{
					{ 23, 1 },
					{ 24, 1 },
					{ 25, 5 },
					{ 26, 10 },
					{ 27, 20 },
					{ 28, 40 },
					{ 29, 20 },
					{ 30, 10 },
					{ 31, 5 },
					{ 32, 2 }
				},
				new byte[10, 2]
				{
					{ 23, 1 },
					{ 24, 1 },
					{ 25, 2 },
					{ 26, 5 },
					{ 27, 10 },
					{ 28, 40 },
					{ 29, 20 },
					{ 30, 10 },
					{ 31, 5 },
					{ 32, 2 }
				}
			}
		},
		{
			12,
			new byte[2][,]
			{
				new byte[10, 2]
				{
					{ 23, 1 },
					{ 24, 1 },
					{ 25, 5 },
					{ 26, 10 },
					{ 27, 20 },
					{ 28, 40 },
					{ 29, 20 },
					{ 30, 10 },
					{ 31, 5 },
					{ 32, 2 }
				},
				new byte[10, 2]
				{
					{ 23, 1 },
					{ 24, 1 },
					{ 25, 2 },
					{ 26, 5 },
					{ 27, 10 },
					{ 28, 40 },
					{ 29, 20 },
					{ 30, 10 },
					{ 31, 5 },
					{ 32, 2 }
				}
			}
		},
		{
			13,
			new byte[2][,]
			{
				new byte[9, 2]
				{
					{ 23, 1 },
					{ 24, 1 },
					{ 25, 10 },
					{ 26, 20 },
					{ 27, 40 },
					{ 28, 20 },
					{ 29, 10 },
					{ 30, 5 },
					{ 31, 2 }
				},
				new byte[9, 2]
				{
					{ 23, 1 },
					{ 24, 1 },
					{ 25, 5 },
					{ 26, 10 },
					{ 27, 20 },
					{ 28, 20 },
					{ 29, 10 },
					{ 30, 5 },
					{ 31, 2 }
				}
			}
		},
		{
			14,
			new byte[2][,]
			{
				new byte[9, 2]
				{
					{ 23, 1 },
					{ 24, 1 },
					{ 25, 10 },
					{ 26, 20 },
					{ 27, 40 },
					{ 28, 20 },
					{ 29, 10 },
					{ 30, 5 },
					{ 31, 2 }
				},
				new byte[9, 2]
				{
					{ 23, 1 },
					{ 24, 1 },
					{ 25, 10 },
					{ 26, 20 },
					{ 27, 40 },
					{ 28, 20 },
					{ 29, 10 },
					{ 30, 5 },
					{ 31, 2 }
				}
			}
		},
		{
			20,
			new byte[2][,]
			{
				new byte[9, 2]
				{
					{ 23, 1 },
					{ 24, 1 },
					{ 25, 10 },
					{ 26, 20 },
					{ 27, 40 },
					{ 28, 20 },
					{ 29, 10 },
					{ 30, 5 },
					{ 31, 2 }
				},
				new byte[9, 2]
				{
					{ 23, 1 },
					{ 24, 1 },
					{ 25, 5 },
					{ 26, 10 },
					{ 27, 20 },
					{ 28, 20 },
					{ 29, 10 },
					{ 30, 5 },
					{ 31, 2 }
				}
			}
		},
		{
			21,
			new byte[2][,]
			{
				new byte[10, 2]
				{
					{ 24, 1 },
					{ 25, 1 },
					{ 26, 5 },
					{ 27, 10 },
					{ 28, 20 },
					{ 29, 40 },
					{ 30, 20 },
					{ 31, 10 },
					{ 32, 5 },
					{ 33, 2 }
				},
				new byte[10, 2]
				{
					{ 24, 1 },
					{ 25, 1 },
					{ 26, 2 },
					{ 27, 5 },
					{ 28, 20 },
					{ 29, 40 },
					{ 30, 20 },
					{ 31, 10 },
					{ 32, 5 },
					{ 33, 2 }
				}
			}
		},
		{
			22,
			new byte[2][,]
			{
				new byte[9, 2]
				{
					{ 23, 1 },
					{ 24, 1 },
					{ 25, 10 },
					{ 26, 20 },
					{ 27, 40 },
					{ 28, 20 },
					{ 29, 10 },
					{ 30, 5 },
					{ 31, 2 }
				},
				new byte[9, 2]
				{
					{ 23, 1 },
					{ 24, 1 },
					{ 25, 5 },
					{ 26, 10 },
					{ 27, 20 },
					{ 28, 20 },
					{ 29, 10 },
					{ 30, 5 },
					{ 31, 2 }
				}
			}
		},
		{
			23,
			new byte[2][,]
			{
				new byte[10, 2]
				{
					{ 23, 1 },
					{ 24, 1 },
					{ 25, 5 },
					{ 26, 10 },
					{ 27, 20 },
					{ 28, 40 },
					{ 29, 20 },
					{ 30, 10 },
					{ 31, 5 },
					{ 32, 2 }
				},
				new byte[10, 2]
				{
					{ 23, 1 },
					{ 24, 1 },
					{ 25, 2 },
					{ 26, 5 },
					{ 27, 10 },
					{ 28, 40 },
					{ 29, 20 },
					{ 30, 10 },
					{ 31, 5 },
					{ 32, 2 }
				}
			}
		},
		{
			24,
			new byte[2][,]
			{
				new byte[10, 2]
				{
					{ 23, 1 },
					{ 24, 1 },
					{ 25, 5 },
					{ 26, 10 },
					{ 27, 20 },
					{ 28, 40 },
					{ 29, 20 },
					{ 30, 10 },
					{ 31, 5 },
					{ 32, 2 }
				},
				new byte[10, 2]
				{
					{ 23, 1 },
					{ 24, 1 },
					{ 25, 2 },
					{ 26, 5 },
					{ 27, 10 },
					{ 28, 40 },
					{ 29, 20 },
					{ 30, 10 },
					{ 31, 5 },
					{ 32, 2 }
				}
			}
		},
		{
			25,
			new byte[2][,]
			{
				new byte[9, 2]
				{
					{ 23, 1 },
					{ 24, 1 },
					{ 25, 10 },
					{ 26, 20 },
					{ 27, 40 },
					{ 28, 20 },
					{ 29, 10 },
					{ 30, 5 },
					{ 31, 2 }
				},
				new byte[9, 2]
				{
					{ 23, 1 },
					{ 24, 1 },
					{ 25, 5 },
					{ 26, 10 },
					{ 27, 20 },
					{ 28, 20 },
					{ 29, 10 },
					{ 30, 5 },
					{ 31, 2 }
				}
			}
		},
		{
			27,
			new byte[2][,]
			{
				new byte[9, 2]
				{
					{ 23, 1 },
					{ 24, 1 },
					{ 25, 10 },
					{ 26, 20 },
					{ 27, 40 },
					{ 28, 20 },
					{ 29, 10 },
					{ 30, 5 },
					{ 31, 2 }
				},
				new byte[9, 2]
				{
					{ 23, 1 },
					{ 24, 1 },
					{ 25, 5 },
					{ 26, 10 },
					{ 27, 20 },
					{ 28, 20 },
					{ 29, 10 },
					{ 30, 5 },
					{ 31, 2 }
				}
			}
		},
		{
			28,
			new byte[2][,]
			{
				new byte[10, 2]
				{
					{ 23, 1 },
					{ 24, 1 },
					{ 25, 5 },
					{ 26, 10 },
					{ 27, 20 },
					{ 28, 40 },
					{ 29, 20 },
					{ 30, 10 },
					{ 31, 5 },
					{ 32, 2 }
				},
				new byte[10, 2]
				{
					{ 23, 1 },
					{ 24, 1 },
					{ 25, 2 },
					{ 26, 5 },
					{ 27, 10 },
					{ 28, 40 },
					{ 29, 20 },
					{ 30, 10 },
					{ 31, 5 },
					{ 32, 2 }
				}
			}
		},
		{
			29,
			new byte[2][,]
			{
				new byte[10, 2]
				{
					{ 23, 1 },
					{ 24, 1 },
					{ 25, 5 },
					{ 26, 10 },
					{ 27, 20 },
					{ 28, 40 },
					{ 29, 20 },
					{ 30, 10 },
					{ 31, 5 },
					{ 32, 2 }
				},
				new byte[10, 2]
				{
					{ 23, 1 },
					{ 24, 1 },
					{ 25, 2 },
					{ 26, 5 },
					{ 27, 10 },
					{ 28, 40 },
					{ 29, 20 },
					{ 30, 10 },
					{ 31, 5 },
					{ 32, 2 }
				}
			}
		},
		{
			40,
			new byte[2][,]
			{
				new byte[10, 2]
				{
					{ 23, 1 },
					{ 24, 1 },
					{ 25, 5 },
					{ 26, 10 },
					{ 27, 20 },
					{ 28, 40 },
					{ 29, 20 },
					{ 30, 10 },
					{ 31, 5 },
					{ 32, 2 }
				},
				new byte[10, 2]
				{
					{ 23, 1 },
					{ 24, 1 },
					{ 25, 2 },
					{ 26, 5 },
					{ 27, 10 },
					{ 28, 40 },
					{ 29, 20 },
					{ 30, 10 },
					{ 31, 5 },
					{ 32, 2 }
				}
			}
		},
		{
			41,
			new byte[2][,]
			{
				new byte[10, 2]
				{
					{ 23, 1 },
					{ 24, 1 },
					{ 25, 5 },
					{ 26, 10 },
					{ 27, 20 },
					{ 28, 40 },
					{ 29, 20 },
					{ 30, 10 },
					{ 31, 5 },
					{ 32, 2 }
				},
				new byte[10, 2]
				{
					{ 23, 1 },
					{ 24, 1 },
					{ 25, 2 },
					{ 26, 5 },
					{ 27, 10 },
					{ 28, 40 },
					{ 29, 20 },
					{ 30, 10 },
					{ 31, 5 },
					{ 32, 2 }
				}
			}
		},
		{
			42,
			new byte[2][,]
			{
				new byte[9, 2]
				{
					{ 23, 1 },
					{ 24, 1 },
					{ 25, 10 },
					{ 26, 20 },
					{ 27, 40 },
					{ 28, 20 },
					{ 29, 10 },
					{ 30, 5 },
					{ 31, 2 }
				},
				new byte[9, 2]
				{
					{ 23, 1 },
					{ 24, 1 },
					{ 25, 5 },
					{ 26, 10 },
					{ 27, 20 },
					{ 28, 20 },
					{ 29, 10 },
					{ 30, 5 },
					{ 31, 2 }
				}
			}
		},
		{
			43,
			new byte[2][,]
			{
				new byte[10, 2]
				{
					{ 23, 1 },
					{ 24, 1 },
					{ 25, 5 },
					{ 26, 10 },
					{ 27, 20 },
					{ 28, 40 },
					{ 29, 20 },
					{ 30, 10 },
					{ 31, 5 },
					{ 32, 2 }
				},
				new byte[10, 2]
				{
					{ 23, 1 },
					{ 24, 1 },
					{ 25, 2 },
					{ 26, 5 },
					{ 27, 10 },
					{ 28, 40 },
					{ 29, 20 },
					{ 30, 10 },
					{ 31, 5 },
					{ 32, 2 }
				}
			}
		},
		{
			44,
			new byte[2][,]
			{
				new byte[10, 2]
				{
					{ 23, 1 },
					{ 24, 1 },
					{ 25, 5 },
					{ 26, 10 },
					{ 27, 20 },
					{ 28, 40 },
					{ 29, 20 },
					{ 30, 10 },
					{ 31, 5 },
					{ 32, 2 }
				},
				new byte[10, 2]
				{
					{ 23, 1 },
					{ 24, 1 },
					{ 25, 2 },
					{ 26, 5 },
					{ 27, 10 },
					{ 28, 40 },
					{ 29, 20 },
					{ 30, 10 },
					{ 31, 5 },
					{ 32, 2 }
				}
			}
		}
	};

	private static readonly short[,] _averageRetirementAge = new short[8, 2]
	{
		{ 32, 4 },
		{ 33, 32 },
		{ 34, 16 },
		{ 35, 8 },
		{ 36, 4 },
		{ 37, 2 },
		{ 38, 1 },
		{ 39, 1 }
	};

	private const int NumPeakYears = 2;

	private const int StartMorale = 5;

	private static readonly (byte Min, byte Max) MinMax = (Min: 1, Max: 10);

	public const int MinRating = 60;

	public bool ReachedPotential
	{
		get
		{
			if (!InDecline)
			{
				return Age() >= GetPotentialAge();
			}
			return true;
		}
	}

	public byte Rating { get; set; }

	public short ClubId { get; set; }

	public byte CacheRating { get; set; }

	public bool TransferPlanned { get; set; }

	public DateTime ContractExpiryDate { get; set; }

	public int ContractWage { get; set; }

	public ePlayerSquadStatus SquadStatus { get; set; }

	public DateTime LastTransferDate { get; set; }

	public DateTime RespawnDate { get; set; }

	public bool IsRespawned { get; set; }

	public short LoanParentClubId { get; set; }

	public bool PendingTransferExists { get; set; }

	public ePlayerStatus Status { get; set; }

	public bool InDecline { get; set; }

	public byte SquadNo { get; set; }

	public List<short> PreviousClubs { get; set; } = new List<short>();

	public PlayerHiddenAttributes HiddenAttributes { get; set; }

	public PlayerConcernsManager Concerns { get; set; }

	public PlayerConcernsManagedClub ConcernsManagedClub => Player.GetStatus(PlayerId).Concerns;

	internal bool WillMoveOverseas
	{
		get
		{
			Game.instance.g_ClubData.TryGetValue(ClubId, out var value);
			if (value != null)
			{
				if (value.transferTier <= 5 && Rating > Transfers.instance.transferTierList.list[5].MinimumPotential && Age() > 18)
				{
					return true;
				}
				if (value.AssociationID != CountryDataID)
				{
					return true;
				}
			}
			return false;
		}
	}

	public int Age()
	{
		DateTime currentDate = Game.instance.CurrentDate;
		int num = currentDate.Year - DateOfBirth.Year;
		int num2 = currentDate.Month - DateOfBirth.Month;
		if (num2 < 0 || (num2 == 0 && currentDate.Day < DateOfBirth.Day))
		{
			num--;
		}
		return num;
	}

	public byte Age(DateTime theDate)
	{
		if (theDate == DateTime.MinValue)
		{
			return (byte)Age();
		}
		int num = theDate.Year - DateOfBirth.Year;
		int num2 = theDate.Month - DateOfBirth.Month;
		if (num2 < 0 || (num2 == 0 && theDate.Day < DateOfBirth.Day))
		{
			num--;
		}
		return (byte)num;
	}

	public int GetAgeInMonths()
	{
		DateTime currentDate = Game.instance.CurrentDate;
		return currentDate.Month - DateOfBirth.Month + 12 * (currentDate.Year - DateOfBirth.Year);
	}

	public static byte GetPotentialAgeRatingBracket(byte rating)
	{
		if (rating < 94)
		{
			return 0;
		}
		return 1;
	}

	public byte GetPotentialAge()
	{
		int seed = Game.instance.Random.GetSeed();
		int num = (Game.instance.Seed + PlayerId) * 1234 % 32768;
		if (num == 0)
		{
			num = 1;
		}
		Game.instance.Random.SetSeed(num);
		byte description = GetDescription();
		byte b = Helpers.WeightedRand(_potentialAgeData[description][GetPotentialAgeRatingBracket(Potential)]);
		if (description == 1)
		{
			if (Potential >= 95)
			{
				if (b < 25)
				{
					b = 25;
				}
			}
			else if (Potential >= 94 && b < 24)
			{
				b = 24;
			}
		}
		DateTime theDate = ((!(RespawnDate != DateTime.MinValue)) ? Game.instance.StartDate : RespawnDate);
		int num2 = Age(theDate);
		if (Age(theDate) >= b && Rating < Potential)
		{
			b = (byte)(num2 + 1);
			if (b > 40)
			{
				b = 40;
			}
		}
		Game.instance.Random.SetSeed(seed);
		return b;
	}

	public PLAYER_LONGEVITY GetLongevity()
	{
		PLAYER_LONGEVITY pLAYER_LONGEVITY = PLAYER_LONGEVITY.POOR;
		byte description = GetDescription(PlayerId);
		if (description == 1)
		{
			if (Potential >= 90)
			{
				return PLAYER_LONGEVITY.GOOD;
			}
			return PLAYER_LONGEVITY.POOR;
		}
		return GetLongevityFromDesc(description);
	}

	public static PLAYER_LONGEVITY GetLongevityFromDesc(int description)
	{
		PLAYER_LONGEVITY result = PLAYER_LONGEVITY.POOR;
		switch (description)
		{
		case 10:
		case 12:
		case 21:
		case 24:
		case 28:
		case 40:
		case 41:
		case 43:
			result = PLAYER_LONGEVITY.GOOD;
			break;
		case 11:
		case 13:
		case 14:
		case 20:
		case 22:
		case 23:
		case 25:
		case 27:
		case 42:
		case 44:
			result = PLAYER_LONGEVITY.POOR;
			break;
		}
		return result;
	}

	public byte RetirementAge()
	{
		int seed = Game.instance.Random.GetSeed();
		Game.instance.Random.SetSeed(Game.instance.Seed + PlayerId);
		byte b = (byte)Helpers.WeightedRand(_averageRetirementAge);
		if (GetPosition(PlayerId) == 1)
		{
			b += 2;
		}
		if (GetLongevity() == PLAYER_LONGEVITY.GOOD)
		{
			b++;
		}
		if (b < 38)
		{
			if (Potential >= 93)
			{
				b += 2;
			}
			else if (Potential >= 90)
			{
				b++;
			}
		}
		Game.instance.Random.SetSeed(seed);
		return b;
	}

	public static PlayerConcernSummary IsHappyWithClubStatus(int playerID, short clubID)
	{
		int desiredClubAverageRatingDiff = GetDesiredClubAverageRatingDiff(playerID, clubID);
		if (desiredClubAverageRatingDiff >= 1)
		{
			return PlayerConcernSummary.VeryHappy;
		}
		if (desiredClubAverageRatingDiff >= 0)
		{
			return PlayerConcernSummary.Happy;
		}
		if (desiredClubAverageRatingDiff <= -3)
		{
			return PlayerConcernSummary.VeryUnhappy;
		}
		if (desiredClubAverageRatingDiff <= -2)
		{
			return PlayerConcernSummary.Unhappy;
		}
		return PlayerConcernSummary.Indifferent;
	}

	public static int GetDesiredClubAverageRatingDiff(int playerID, short clubID)
	{
		int num = Player.GetRatingQuick(playerID);
		int averageRating = Club.GetAverageRating(clubID);
		if (num > averageRating)
		{
			num = Game.instance.g_PlayerData[playerID].Potential;
		}
		if (num > 93)
		{
			num = 93;
		}
		PlayerHiddenAttributes hiddenAttributes = GetHiddenAttributes(playerID);
		switch (hiddenAttributes.AmbitionDiscreteValue)
		{
		case AttributeDiscreteValue.High:
			num++;
			break;
		case AttributeDiscreteValue.Medium:
			num--;
			break;
		case AttributeDiscreteValue.Low:
			num -= 3;
			break;
		}
		switch (hiddenAttributes.LoyaltyDiscreteValue)
		{
		case AttributeDiscreteValue.High:
			num -= 2;
			break;
		case AttributeDiscreteValue.Medium:
			num--;
			break;
		case AttributeDiscreteValue.Low:
			num = num;
			break;
		}
		if (averageRating < num && (Player.GetInDecline(playerID) || averageRating >= 93))
		{
			num = averageRating;
		}
		return averageRating - num;
	}

	public float CalculatePlayerCompensationFee()
	{
		float num = 0f;
		if (ClubId == 0 && Age() < 24)
		{
			num = GetValue() * 0.2f;
		}
		return Helpers.RoundMonetaryValue(num);
	}

	public float GetYearsRemainingOnContract()
	{
		return (float)(ContractExpiryDate - Game.instance.CurrentDate).Days / 365f;
	}

	public int GenerateNewContractWageForUnmanagedClub(PlayerContractHelper helper, int contractLength)
	{
		byte ratingQuick = GetRatingQuick();
		return Helpers.RoundMonetaryValue(Player.GenerateNewContractWages(Player.GetPlayerBasicValue(ratingQuick, InDecline, Potential), Player.GetPlayerSquadRoleMultiplier(ratingQuick, (int)helper.teamAverageRating), helper.teamRatingMultiplier, helper.leagueRatingMultiplier, Player.GetContractLengthMultiplier(contractLength)));
	}

	public int GenerateInitialPlayerContractLength(PlayerContractHelper helper)
	{
		bool flag = IsTransferBanned();
		bool flag2 = IsLoanedToClub();
		int min = ((helper.teamOneYearContractCount < 3 && !flag) ? 1 : 2);
		int max = ((Age() < 32) ? 5 : 3);
		int num = ((!flag2) ? Game.instance.Random.GetRand(min, max) : 2);
		if (num == 1)
		{
			helper.teamOneYearContractCount++;
		}
		return num;
	}

	internal bool WillManagedPlayerRenewContract()
	{
		PlayerConcernSquadStatusExtra contractSquadStatusExtra = ConcernsManagedClub.ContractSquadStatusExtra;
		PlayerConcernGameTime playerGameTime = ConcernsManagedClub.PlayerGameTime;
		PlayerConcernClubStatus clubStatus = Concerns.ClubStatus;
		PlayerConcernsPromiseManager playerPromises = ConcernsManagedClub.PlayerPromises;
		if ((!contractSquadStatusExtra.IsConcerned || contractSquadStatusExtra.ConcernReason == ConcernReasonType.GameTimeHighForSquadStatus) && !playerGameTime.IsConcerned && !clubStatus.IsConcerned)
		{
			return !playerPromises.IsConcerned;
		}
		return false;
	}

	private bool WillUnmanagedPlayerRenewContract()
	{
		PlayerConcernSquadStatus contractSquadStatus = Concerns.ContractSquadStatus;
		PlayerConcernClubStatus clubStatus = Concerns.ClubStatus;
		if (!contractSquadStatus.IsConcerned)
		{
			return clubStatus.Concern != clubStatus.GetMaxValue();
		}
		return false;
	}

	public bool WillPlayerRenewContract()
	{
		if (IsLoanedToClub())
		{
			return true;
		}
		if (ClubId == Game.GetCurrentUserClubID())
		{
			return WillManagedPlayerRenewContract();
		}
		return WillUnmanagedPlayerRenewContract();
	}

	public ePlayerSquadStatus GetWantedSquadStatusForAge()
	{
		byte b = (byte)Age();
		if (b <= 22)
		{
			return ePlayerSquadStatus.Youngster;
		}
		byte potentialAge = GetPotentialAge();
		if (b < potentialAge && !IsGoalKeeper())
		{
			return ePlayerSquadStatus.Rotation;
		}
		if (b >= potentialAge && b <= potentialAge + 2)
		{
			return ePlayerSquadStatus.FirstTeam;
		}
		return ePlayerSquadStatus.Backup;
	}

	public bool IsPlayerHappyWithReducedSquadStatus(short clubID)
	{
		if (RGYBCodeHelpers.GetCodeCompatibility(RGYBCode, Club.GetRGYBCode(clubID)) != RGYBCompatibility.High)
		{
			return false;
		}
		if (HiddenAttributes.AmbitionDiscreteValue == AttributeDiscreteValue.Low)
		{
			return true;
		}
		if (HiddenAttributes.LoyaltyDiscreteValue == AttributeDiscreteValue.Low || HiddenAttributes.HandlingPressureDiscreteValue == AttributeDiscreteValue.Low)
		{
			return false;
		}
		if (HiddenAttributes.AmbitionDiscreteValue == AttributeDiscreteValue.High && HiddenAttributes.LoyaltyDiscreteValue != AttributeDiscreteValue.High)
		{
			return false;
		}
		return true;
	}

	public float CalculatePlayerSigningFee(short toClubId)
	{
		float value = GetValue();
		float num = 0f;
		if (ClubId == 0)
		{
			num = ((Age() < 23) ? (value * 0.1f) : (value * 0.25f));
		}
		else if (ClubId == toClubId)
		{
			int seed = Game.instance.Random.GetSeed();
			Game.instance.Random.SetSeed(PlayerId + Game.instance.Seed);
			float num2 = ((GetRating() <= Club.GetAverageRating(ClubId)) ? ((float)Game.instance.Random.GetRand(0.025, 0.05)) : ((float)Game.instance.Random.GetRand(0.05, 0.1)));
			Game.instance.Random.SetSeed(seed);
			num = value * num2;
		}
		else
		{
			num = value * 0.1f;
		}
		if (toClubId == Game.GetCurrentUserClubID() && UserManagerProfileController.UserHasTrait(ManagerTraitArea.TransferSigning))
		{
			num = (float)UserManagerProfileController.ApplyTrait(ManagerTraitArea.TransferSigning, num);
		}
		return Helpers.RoundMonetaryValue(num);
	}

	internal ePlayerSquadStatus GetExpectedSquadStatusInClubSquad(short clubID)
	{
		int currentUserClubID = Game.GetCurrentUserClubID();
		if (ClubId == clubID)
		{
			if (clubID == currentUserClubID)
			{
				return Player.GetStatus(PlayerId).ExpectedSquadStatus;
			}
			return SquadStatus;
		}
		return PlayerSquad.SquadStatusManager.GetExpectedSquadStatusWithinClubForNewPlayer(clubID, PlayerId);
	}

	public ePlayerSquadStatus GetExpectedSquadStatusForClub(short clubID)
	{
		ePlayerSquadStatus expectedSquadStatusInClubSquad = GetExpectedSquadStatusInClubSquad(clubID);
		ePlayerSquadStatus wantedSquadStatusForAge = GetWantedSquadStatusForAge();
		if (PlayerSquadStatus.Compare(wantedSquadStatusForAge, expectedSquadStatusInClubSquad) > 0)
		{
			return wantedSquadStatusForAge;
		}
		return expectedSquadStatusInClubSquad;
	}

	public void CheckIfContractExpired()
	{
		if (ClubId <= 0 || !(Helpers.TruncateDate(ContractExpiryDate.Date) == Helpers.TruncateDate(Game.instance.CurrentDate)))
		{
			return;
		}
		if (TransferPlayer.transferPlayer(PlayerId, ClubId, 0, -6, 0, DateTime.MinValue) != null)
		{
			Transfers.instance.bidManager.removeUnprocessedTransferBids(PlayerId);
			if (ClubId == Game.instance.CurrentManagedClubId || Game.instance.CurrentManagedClub.PlayerShortlist.ContainsKey(PlayerId))
			{
				if (ClubId == Game.instance.CurrentManagedClubId)
				{
					EventLoggingController.LogEvent("playerSold_expired");
					Game.instance.CurrentManagedClub.SwapDepartingPlayerFromLineup(PlayerId);
				}
				MessageManager.PlayerLeavesFreeAgent(PlayerId);
			}
		}
		else
		{
			Debug.LogError("Player::ProcessFreeAgents() - Tried to make " + Player.GetName(PlayerId) + "(" + PlayerId + ") a free agent but something went wrong");
		}
	}

	public PlayerData()
	{
		InDecline = false;
		PendingTransferExists = false;
		TransferPlanned = false;
		ContractExpiryDate = DateTime.MinValue;
		ContractWage = 0;
		AttributesRankVotes = 0;
		AttributesRank = "";
	}

	public PlayerData(string file, List<int> FieldsInFile, int end)
	{
		Forename = "Player";
		Surname = ((FieldsInFile.Count > 14) ? DataPackHelpers.ParseSectionAsString(FieldsInFile[16] + 1, FieldsInFile[17], file) : "0");
		CountryDataID = (short)DataPackHelpers.ParseSectionAsInt(FieldsInFile[5] + 1, FieldsInFile[6], file);
		PhotoFileName = DataPackHelpers.ParseSectionAsString(FieldsInFile[9] + 1, FieldsInFile[10], file);
		Position = DataPackHelpers.ParseSectionAsInt(FieldsInFile[3] + 1, FieldsInFile[4], file);
		PreferredPosition = DataPackHelpers.ParseSectionAsInt(FieldsInFile[4] + 1, FieldsInFile[5], file);
		Rating = (byte)DataPackHelpers.ParseSectionAsInt(FieldsInFile[6] + 1, FieldsInFile[7], file);
		CacheRating = Rating;
		DateOfBirth = Extensions.FromEpochSeconds(default, DataPackHelpers.ParseSectionAsInt(FieldsInFile[7] + 1, FieldsInFile[8], file));
		Foot = (byte)DataPackHelpers.ParseSectionAsInt(FieldsInFile[8] + 1, FieldsInFile[9], file);
		Height = (byte)DataPackHelpers.ParseSectionAsInt(FieldsInFile[10] + 1, FieldsInFile[11], file);
		Weight = (byte)DataPackHelpers.ParseSectionAsInt(FieldsInFile[11] + 1, FieldsInFile[12], file);
		Potential = (byte)DataPackHelpers.ParseSectionAsInt(FieldsInFile[12] + 1, FieldsInFile[13], file);
		InDecline = DataPackHelpers.ParseSectionAsInt(FieldsInFile[13] + 1, FieldsInFile[14], file) < 0;
		Description = (byte)((FieldsInFile.Count > 14) ? ((byte)DataPackHelpers.ParseSectionAsInt(FieldsInFile[14] + 1, FieldsInFile[15], file)) : 0);
		PlayerDataID = ((FieldsInFile.Count > 14) ? DataPackHelpers.ParseSectionAsInt(FieldsInFile[16] + 1, FieldsInFile[17], file) : 0);
		HairColour = (byte)((FieldsInFile.Count > 17) ? ((byte)DataPackHelpers.ParseSectionAsInt(FieldsInFile[17] + 1, FieldsInFile[18], file)) : 0);
		HairStyle = (byte)((FieldsInFile.Count > 18) ? ((byte)DataPackHelpers.ParseSectionAsInt(FieldsInFile[18] + 1, FieldsInFile[19], file)) : 0);
		SkinColour = (byte)((FieldsInFile.Count > 19) ? ((byte)DataPackHelpers.ParseSectionAsInt(FieldsInFile[19] + 1, FieldsInFile[20], file)) : 0);
		FacialHair = (byte)((FieldsInFile.Count > 20) ? ((byte)DataPackHelpers.ParseSectionAsInt(FieldsInFile[20] + 1, FieldsInFile[21], file)) : 0);
		new DateTime(2020, 1, 4);
		LastTransferDate = ((FieldsInFile.Count > 21) ? Extensions.FromEpochSeconds(default, DataPackHelpers.ParseSectionAsInt(FieldsInFile[21] + 1, FieldsInFile[22], file)) : DateTime.MinValue);
		try
		{
			SquadNo = (byte)DataPackHelpers.ParseSectionAsInt(FieldsInFile[22] + 1, FieldsInFile[23], file);
		}
		catch
		{
			SquadNo = 0;
		}
		byte rawLoyalty = (byte)DataPackHelpers.ParseSectionAsInt(FieldsInFile[23] + 1, end, file);
		HiddenAttributes = new PlayerHiddenAttributes();
		HiddenAttributes.SetRawLoyalty(rawLoyalty);
		Concerns = new PlayerConcernsManager();
		PendingTransferExists = false;
		AttributesRankVotes = 0;
		AttributesRank = "";
		GeneratedPotential = Potential;
	}

	public void SetAttributeData(string file, List<int> FieldsInFile, int endLine)
	{
		AttributesRankVotes = (byte)DataPackHelpers.ParseSectionAsInt(FieldsInFile[1] + 1, FieldsInFile[2], file);
		AttributesRank = DataPackHelpers.ParseSectionAsString(FieldsInFile[2] + 1, endLine, file);
	}

	public void LoadNamePack(string forenameString, string surNameString, string urlString)
	{
		Forename = forenameString;
		Surname = surNameString;
		PhotoFileName = urlString;
	}

	public void LoadNamePack(string[] fields)
	{
		Forename = fields[1].Replace('ş', 's').Replace("&#39;", "'");
		Surname = fields[2].Replace('ş', 's').Replace("&#39;", "'");
		PhotoFileName = fields[5];
	}

	public static void RemovePlayer(int playerID)
	{
		Game.instance.g_PlayerData.Remove(playerID);
	}

	public bool willSignforClub(short clubFromId)
	{
		if (Club.AreBitterRivals(clubFromId, ClubId))
		{
			return false;
		}
		bool result = true;
		foreach (short previousClub in PreviousClubs)
		{
			if (Club.AreBitterRivals(clubFromId, previousClub) && Game.instance.Random.GetRand(0, 10) > 8)
			{
				result = false;
			}
		}
		return result;
	}

	public static void AddAward(int playerID, AwardType type, int compType = 0, int compId = 0, int AvgRating = 0)
	{
		Game.instance.sqLiteService.playerAwardTbl.AddSeasonAward(playerID, type, compType, compId, AvgRating, 0, 0);
	}

	public static void AddMonthAward(int playerID, AwardType type, int compType, int compId)
	{
		int num = Game.instance.CurrentDate.Month - 1;
		if (num == 0)
		{
			num = 12;
		}
		Game.instance.sqLiteService.playerAwardTbl.AddMonthAward(playerID, type, num, compType, compId);
	}

	public static List<PlayerAwardContainer> GetCurrentSeasonAwards(int playerId, string seasonId)
	{
		List<PlayerAwardContainer> currentSeasonAwards = Game.instance.sqLiteService.playerAwardTbl.GetCurrentSeasonAwards(playerId, seasonId);
		PlayerAwardContainerListHelper.RemoveUnannouncedAwards(currentSeasonAwards);
		return currentSeasonAwards;
	}

	public static void SetSquadNo(int playerID, byte squadNo)
	{
		Game.instance.g_PlayerData[playerID].SquadNo = squadNo;
	}

	public static byte GetSquadNo(int PlayerID)
	{
		return Game.instance.g_PlayerData[PlayerID].SquadNo;
	}

	public static void SetSquadStatus(int playerID, ePlayerSquadStatus status)
	{
		Game.instance.g_PlayerData[playerID].SquadStatus = status;
	}

	public static ePlayerSquadStatus GetSquadStatus(int playerID)
	{
		return Game.instance.g_PlayerData[playerID].SquadStatus;
	}

	public static PlayerHiddenAttributes GetHiddenAttributes(int playerID)
	{
		return Game.instance.g_PlayerData[playerID].HiddenAttributes;
	}

	public static bool HasConcern(int playerID)
	{
		foreach (PlayerConcern allConcern in PlayerConcernsManager.GetAllConcerns(playerID))
		{
			if (allConcern.IsConcerned)
			{
				return true;
			}
		}
		return false;
	}

	public static void ResetConcerns(int playerID)
	{
		foreach (PlayerConcern allConcern in PlayerConcernsManager.GetAllConcerns(playerID))
		{
			allConcern.Reset();
		}
	}

	public static PlayerConcernsManagedClub GetConcernsManagedClub(int playerID)
	{
		return Player.GetStatus(playerID).Concerns;
	}

	public static PlayerConcernsPromiseManager GetPromiseManager(int playerID)
	{
		return Player.GetStatus(playerID).Concerns.PlayerPromises;
	}

	public bool IsTransferWanted()
	{
		return GetTransferWantedReason() != ePlayerWantsTransferReason.NULL;
	}

	public ePlayerWantsTransferReason GetTransferWantedReason()
	{
		if (ClubId == Game.GetCurrentUserClubID())
		{
			return DoesManagedPlayerWantTransfer();
		}
		return DoesUnmanagedPlayerWantTransfer();
	}

	private ePlayerWantsTransferReason DoesManagedPlayerWantTransfer()
	{
		PlayerConcernSquadStatusExtra contractSquadStatusExtra = ConcernsManagedClub.ContractSquadStatusExtra;
		PlayerConcernGameTime playerGameTime = ConcernsManagedClub.PlayerGameTime;
		PlayerConcernClubStatus clubStatus = Concerns.ClubStatus;
		PlayerConcernContractWage contractWage = Concerns.ContractWage;
		if (clubStatus.Concern == clubStatus.GetMaxValue())
		{
			return ePlayerWantsTransferReason.BIGGER_CLUB;
		}
		if (contractSquadStatusExtra.Concern == contractSquadStatusExtra.GetMaxValue() && contractSquadStatusExtra.ConcernReason != ConcernReasonType.GameTimeHighForSquadStatus)
		{
			return ePlayerWantsTransferReason.MORE_GAMETIME;
		}
		if (playerGameTime.Concern == playerGameTime.GetMaxValue())
		{
			return ePlayerWantsTransferReason.MORE_GAMETIME;
		}
		if (contractWage.Concern == contractWage.GetMaxValue())
		{
			return ePlayerWantsTransferReason.BETTER_CONTRACT;
		}
		return ePlayerWantsTransferReason.NULL;
	}

	private ePlayerWantsTransferReason DoesUnmanagedPlayerWantTransfer()
	{
		PlayerConcernSquadStatus contractSquadStatus = Concerns.ContractSquadStatus;
		PlayerConcernClubStatus clubStatus = Concerns.ClubStatus;
		if (clubStatus.Concern == clubStatus.GetMaxValue())
		{
			return ePlayerWantsTransferReason.BIGGER_CLUB;
		}
		if (contractSquadStatus.Concern == contractSquadStatus.GetMaxValue())
		{
			return ePlayerWantsTransferReason.MORE_GAMETIME;
		}
		return ePlayerWantsTransferReason.NULL;
	}

	public static bool HasGameTimeConcerns(int playerID)
	{
		if (Player.GetClubID(playerID) == Game.GetCurrentUserClubID())
		{
			return GetConcernsManagedClub(playerID).PlayerGameTime.GetConcern() > 0;
		}
		return false;
	}

	public byte GetDescription()
	{
		byte b = Description;
		if (b == 0)
		{
			int num = PreferredPosition;
			if (num == 0)
			{
				num = GetPosition();
			}
			b = GetPlayerDescriptionFromPosition(num);
		}
		return b;
	}

	public void setLastBidDate(DateTime theDate)
	{
		if (Game.instance.CurrentManagedClub.PlayerStatus.ContainsKey(PlayerId))
		{
			Game.instance.CurrentManagedClub.PlayerStatus[PlayerId].lastBidDate = theDate;
		}
	}

	public int GenerateNewContractLengthForUnmanagedClub(int minimum = 1)
	{
		int num = Age();
		int val = ((num >= 23) ? 1 : 3);
		val = Math.Max(minimum, val);
		int num2 = ((num >= 32) ? 3 : 5) - val + 1;
		return (int)((Game.instance.Random.BellCurve(4) + 0.5f) * (float)num2) + val;
	}

	public byte GetPlayerDescriptionFromPosition(int position)
	{
		byte result = 0;
		if ((position & 1) != 0)
		{
			result = 1;
		}
		else if ((position & 0xE) != 0)
		{
			if (((position & 4) == 0 || (position & 2) == 0) && (position & 8) == 0)
			{
				if (((position & 4) == 0 || (position & 0x10) == 0) && (position & 0x40) == 0)
				{
					if ((position & 4) != 0)
					{
						result = 10;
					}
					else if ((position & 0x10) == 0 && (position & 0x40) == 0)
					{
						result = (byte)(((position & 2) == 0 && (position & 8) == 0) ? 11 : 13);
					}
					else
					{
						result = 14;
					}
				}
				else
				{
					result = 11;
				}
			}
			else
			{
				result = 11;
			}
		}
		else if ((position & 0x70) != 0)
		{
			if ((position & 0x20) != 0)
			{
				result = 20;
			}
			else
			{
				result = (byte)(((position & 0x10) == 0 && (position & 0x40) == 0) ? 20 : 14);
			}
		}
		else if ((position & 0x1C00) != 0)
		{
			if ((position & 0x800) != 0 && (position & 0xE000) != 0)
			{
				result = 43;
			}
			else if ((position & 0x800) != 0)
			{
				result = 28;
			}
			else
			{
				result = (byte)(((position & 0x400) == 0 && (position & 0x1000) == 0) ? 28 : 27);
			}
		}
		else if ((position & 0x380) != 0)
		{
			if ((position & 0x100) != 0)
			{
				result = 23;
			}
			else
			{
				result = (byte)(((position & 0x80) == 0 && (position & 0x200) == 0) ? 23 : 25);
			}
		}
		else if ((position & 0xE000) != 0)
		{
			if ((position & 0x2000) != 0 && (position & 0x4000) != 0)
			{
				result = 44;
			}
			else if ((position & 0x2000) != 0 || (position & 0x8000) != 0)
			{
				result = 42;
			}
			else
			{
				result = (byte)(((position & 0x4000) == 0) ? 44 : 40);
			}
		}
		return result;
	}

	public DateTime GetPotentialDate()
	{
		DateTime theDate = ((!(RespawnDate != DateTime.MinValue)) ? Game.instance.StartDate : RespawnDate);
		byte b = Age(theDate);
		int num = GetPotentialAge();
		if (b >= num && Rating < Potential)
		{
			num = Math.Min(40, b + 1);
		}
		return new DateTime(theDate.Year + (num - b), theDate.Month, Math.Min(theDate.Day, 28));
	}

	public int GetDeclineAge()
	{
		int seed = Game.instance.Random.GetSeed();
		int seed2 = Game.instance.Seed + PlayerId;
		int potentialAge = GetPotentialAge();
		Game.instance.Random.SetSeed(seed2);
		DateTime theDate = ((!(RespawnDate != DateTime.MinValue)) ? Game.instance.StartDate : RespawnDate);
		byte b = Age(theDate);
		int num = ((GetLongevity() == PLAYER_LONGEVITY.GOOD) ? Game.instance.Random.GetRand(30, 33) : Game.instance.Random.GetRand(28, 31));
		if (GetPosition(PlayerId) == 1)
		{
			num++;
		}
		if (num < potentialAge + 2 && !InDecline)
		{
			num = potentialAge + 2;
		}
		if (num <= b)
		{
			num = b + 1;
		}
		Game.instance.Random.SetSeed(seed);
		return num;
	}

	public int GetDeclineAge(int seed, int potentialAge, DateTime startDate, byte startAge)
	{
		Game.instance.Random.SetSeed(seed);
		int num = ((GetLongevity() == PLAYER_LONGEVITY.GOOD) ? Game.instance.Random.GetRand(30, 33) : Game.instance.Random.GetRand(28, 31));
		if (GetPosition(PlayerId) == 1)
		{
			num++;
		}
		if (num < potentialAge + 2 && !InDecline)
		{
			num = potentialAge + 2;
		}
		if (num <= startAge)
		{
			num = startAge + 1;
		}
		return num;
	}

	public PlayerRatingState GetRatingState()
	{
		int potentialAge = GetPotentialAge();
		if (Player.GetInDecline(PlayerId))
		{
			return PlayerRatingState.DECLINING;
		}
		if (Potential == Rating || Age() >= potentialAge)
		{
			return PlayerRatingState.PEAK;
		}
		return PlayerRatingState.INCREASING;
	}

	public static string GetStatusName(int transferAmount)
	{
		if (transferAmount == -3)
		{
			return Translation.GetText("youth-academy");
		}
		return Translation.GetText("free-agent");
	}

	public static string GetSurname(int playerID)
	{
		if (playerID > 0)
		{
			return Game.instance.g_PlayerData[playerID].Surname;
		}
		return "";
	}

	public static string GetPhotoFilename(int playerID)
	{
		return Game.instance.g_PlayerData[playerID].PhotoFileName;
	}

	public static string GetForename(int playerID)
	{
		return Game.instance.g_PlayerData[playerID].Forename;
	}

	public static void SetPendingTransferExists(int playerId, bool val)
	{
		Game.instance.g_PlayerData[playerId].PendingTransferExists = val;
	}

	public static DateTime GetLastTransferDate(int playerID)
	{
		return Game.instance.g_PlayerData[playerID].LastTransferDate;
	}

	public static short GetCountryDataID(int playerID)
	{
		return Game.instance.g_PlayerData[playerID].CountryDataID;
	}

	public static int GetPosition(int playerID, bool preferred = false)
	{
		return Game.instance.g_PlayerData[playerID].GetPosition(preferred);
	}

	public int GetPosition(bool preferred = false)
	{
		if (preferred && PreferredPosition != 0)
		{
			return PreferredPosition;
		}
		return Position;
	}

	public bool IsGoalKeeper()
	{
		return Position == 1;
	}

	public static int GetPreferredPosition(int playerID)
	{
		return Game.instance.g_PlayerData[playerID].PreferredPosition;
	}

	public static bool IsGoalKeeper(int playerID)
	{
		return GetPosition(playerID) == 1;
	}

	public static DateTime GetDateOfBirth(int playerID)
	{
		return Game.instance.g_PlayerData[playerID].DateOfBirth;
	}

	public static byte GetFoot(int playerID)
	{
		return Game.instance.g_PlayerData[playerID].Foot;
	}

	public static byte GetHeight(int playerID)
	{
		return Game.instance.g_PlayerData[playerID].Height;
	}

	public static byte GetWeight(int playerID)
	{
		return Game.instance.g_PlayerData[playerID].Weight;
	}

	public static byte GetSkinColour(int playerID)
	{
		return Game.instance.g_PlayerData[playerID].SkinColour;
	}

	public static byte GetHairStyle(int playerID)
	{
		return Game.instance.g_PlayerData[playerID].HairStyle;
	}

	public static byte GetHairColour(int playerID)
	{
		return Game.instance.g_PlayerData[playerID].HairColour;
	}

	public static byte GetFacialHair(int playerID)
	{
		return Game.instance.g_PlayerData[playerID].FacialHair;
	}

	public byte GetPotential()
	{
		return Potential;
	}

	public static RGYBCode GetRGYBCode(int playerID)
	{
		return Game.instance.g_PlayerData[playerID].RGYBCode;
	}

	public static bool GetIndecline(int playerID)
	{
		return Game.instance.g_PlayerData[playerID].InDecline;
	}

	public static byte GetDescription(int playerID)
	{
		return Game.instance.g_PlayerData[playerID].Description;
	}

	public static string GetPlayerDescriptionString(int playerID)
	{
		return Helpers.GetPlayerDescriptionString(GetDescription(playerID));
	}

	public static int GetPlayerDataID(int playerID)
	{
		if (playerID == 0)
		{
			return 0;
		}
		return Game.instance.g_PlayerData[playerID].PlayerDataID;
	}

	public static bool IsRespawn(int playerID)
	{
		return GetPlayerRespawnDate(playerID) != DateTime.MinValue;
	}

	public static DateTime GetPlayerRespawnDate(int playerID)
	{
		return Game.instance.g_PlayerData[playerID].RespawnDate;
	}

	public bool HasRespawnedAnotherPlayer(int playerID)
	{
		return Game.instance.g_PlayerData[playerID].IsRespawned;
	}

	public static bool GetRetired(int playerID)
	{
		return Game.instance.g_PlayerData[playerID].GetRetired();
	}

	public bool GetRetired()
	{
		return Status == ePlayerStatus.RETIRED;
	}

	public int YearsToRetirement()
	{
		return RetirementAge() - Age();
	}

	public static bool IsLoanedToClub(int playerID)
	{
		return Game.instance.g_PlayerData[playerID].IsLoanedToClub();
	}

	public bool IsLoanedToClub()
	{
		return LoanParentClubId > 0;
	}

	public bool IsTransferBanned()
	{
		bool result = false;
		if (LoanParentClubId < 1 && Game.instance.CurrentDate - LastTransferDate < TimeSpan.FromSeconds(23673600.0))
		{
			result = true;
		}
		return result;
	}

	public bool IsInjured()
	{
		bool result = false;
		InjuredPlayer value = new InjuredPlayer();
		if (Game.instance.g_injuredPlayers.Injuries.TryGetValue(PlayerId, out value))
		{
			DateTime end = value.End;
			if (value.Start <= Game.instance.CurrentDate && end > Game.instance.CurrentDate)
			{
				if (Game.GetCurrentUserClubID() == ClubId)
				{
					if (end > Game.instance.CurrentDate)
					{
						result = true;
					}
				}
				else
				{
					result = true;
				}
			}
		}
		return result;
	}

	public bool NameContains(string searchString)
	{
		if (Status != ePlayerStatus.RETIRED)
		{
			string text = Helpers.RemoveDiacritics(Forename).ToLower();
			string text2 = Helpers.RemoveDiacritics(Surname).ToLower();
			if (text.Contains(searchString) || text2.Contains(searchString) || (text + " " + text2).Contains(searchString))
			{
				return true;
			}
		}
		return false;
	}

	public bool IsSuitableForLoan()
	{
		if (LoanParentClubId > 0 || GeneratedPotential - CacheRating < 5 || InDecline || (Game.instance.CurrentDate - LastTransferDate).TotalSeconds < 15768000.0 || RetirementAge() - Age() < 2 || IsInjured())
		{
			return false;
		}
		return true;
	}

	public bool HasBeenTransferedRecently()
	{
		if ((Game.instance.CurrentDate - LastTransferDate).TotalSeconds < 23673600.0)
		{
			return true;
		}
		return false;
	}

	public void CreateNewContract(int wage, DateTime expiryDate)
	{
		ContractExpiryDate = expiryDate;
		ContractWage = wage;
	}

	public bool IsSuspended(int assocID, int competitionType, int competitionID)
	{
		bool result = false;
		if (competitionType != -1 && competitionID != -1)
		{
			switch (competitionType)
			{
			case 4:
			{
				ContinentalComp continentalComp = Game.instance.g_continentalCompetitions[competitionID];
				if (continentalComp.matchban.ContainsKey(PlayerId) && continentalComp.matchban[PlayerId] > 0 && ClubId != 0)
				{
					result = true;
				}
				break;
			}
			default:
				if (assocID != 2 && assocID != 3 && Game.instance.g_association.ContainsKey(assocID))
				{
					Dictionary<int, int> matchBan = Game.instance.g_association[assocID].MatchBan;
					if (matchBan.ContainsKey(PlayerId) && matchBan[PlayerId] > 0 && ClubId != 0)
					{
						result = true;
					}
				}
				break;
			case 5:
				break;
			}
		}
		return result;
	}

	public double SeasonGameTimePercentage()
	{
		if (Club.GetNumberOfLeagueGamesPlayed(ClubId) > 0)
		{
			StatContainer currentSeasonStats = PlayerCareerStats.GetCurrentSeasonStats(PlayerId);
			if (currentSeasonStats != null)
			{
				return Math.Max(0, currentSeasonStats.GetAverageGameTime());
			}
		}
		return 70.0;
	}

	public double SeasonForm()
	{
		if (Club.GetNumberOfLeagueGamesPlayed(ClubId) > 0)
		{
			StatContainer currentSeasonStats = PlayerCareerStats.GetCurrentSeasonStats(PlayerId);
			if (currentSeasonStats != null)
			{
				return currentSeasonStats.GetAveragePerformance();
			}
			return 0.0;
		}
		return 7.0;
	}

	public void NotAvailableForMatch(int assocID, int competitionType, int competitionID)
	{
		if (competitionType != -1 && competitionID != -1 && competitionType != 5 && assocID != 2 && assocID != 3)
		{
			PlayerCareerStats value = null;
			if (Game.instance.g_playerCareerStats.TryGetValue(PlayerId, out value))
			{
				value.AddToStats(1, ClubId, competitionID, competitionType, StatType.NotAvail);
			}
		}
	}

	public void ReturnFromLoan(short clubId)
	{
		ClubId = clubId;
		LastTransferDate = DateTime.MinValue;
		LoanParentClubId = 0;
	}

	public float GetRemainingContractYears()
	{
		return (float)(ContractExpiryDate - Game.instance.CurrentDate).Days / 365f;
	}

	public bool IsBidable()
	{
		bool flag = Player.IsRetiring(PlayerId);
		if (!flag)
		{
			flag = IsInjured();
		}
		if (!flag)
		{
			flag = TransferPlanned;
		}
		if (!flag)
		{
			flag = PendingTransferExists;
		}
		if (!flag)
		{
			flag = IsLoanedToClub();
		}
		if (!flag)
		{
			flag = Player.IsOnLoanList(PlayerId);
		}
		if (!flag)
		{
			flag = Player.IsTransferBanned(PlayerId);
		}
		return !flag;
	}

	internal void GenerateRating(byte maxPotential)
	{
		Potential = maxPotential;
		Rating = Math.Max((byte)60, Math.Min((byte)75, (byte)(maxPotential - 10)));
	}

	public float GetValue()
	{
		byte b = (byte)Math.Min((int)Age(Game.instance.CurrentDate), 36);
		bool inDecline = Player.GetInDecline(PlayerId);
		byte ratingQuick = Player.GetRatingQuick(PlayerId);
		double num;
		if (inDecline)
		{
			num = Math.Pow(ratingQuick - 55, 8.0) * 50.0 * ((Math.Pow(37.0, 2.0) - Math.Pow((int)b, 2.0)) / 18.0) / 150000000.0;
		}
		else
		{
			num = Math.Pow(ratingQuick - 55, 8.0) * 50.0 * ((Math.Pow(37.0, 2.0) - Math.Pow((int)b, 2.0)) / 18.0) / 150000000.0 * 0.8;
			num += Math.Pow(Math.Max(Potential, ratingQuick) - 57, 8.0) * 50.0 * ((Math.Pow(37.0, 2.0) - Math.Pow((int)b, 2.0)) / 18.0 / 150000000.0) * 0.2;
		}
		float num2 = (int)Player.GetMorale(PlayerId);
		double num3 = SeasonGameTimePercentage();
		double num4 = (0.85 + (Math.Pow(num3 / 200.0, 2.0) - 1.0)) * num;
		double num5 = Math.Pow(SeasonForm() / 10.0, 2.0) * (0.2 * num) * num3 / 100.0 - num * 0.1;
		return (float)Math.Max((num + num4 + num5) * (double)(num2 * 0.02f + 0.8f), 1000.0);
	}

	public static byte GetMorale(int playerID)
	{
		PlayerData playerData = Game.instance.g_PlayerData[playerID];
		if (Game.instance.ChosenCountries.Contains(Club.GetAssociationID(playerData.ClubId)))
		{
			int num = playerData.CalcMoraleFromConcerns();
			if (playerData.ClubId == Game.GetCurrentUserClubID())
			{
				int moraleBoost = Player.GetStatus(playerID).moraleBoost;
				num += moraleBoost;
				num = Math.Min(MinMax.Max, Math.Max(moraleBoost, num));
			}
			return (byte)num;
		}
		return CalcMorale(playerID);
	}

	public static byte CalcMorale(int playerID)
	{
		int seed = Game.instance.Seed + playerID;
		int seed2 = Game.instance.Random.GetSeed();
		Game.instance.Random.SetSeed(seed);
		byte result = (byte)(7 + Game.instance.Random.GetRand() % 4);
		Game.instance.Random.SetSeed(seed2);
		return result;
	}

	public int CalcMoraleFromConcerns()
	{
		int num = 5;
		foreach (PlayerConcern allConcern in PlayerConcernsManager.GetAllConcerns(PlayerId))
		{
			num += allConcern.GetMoraleEffect(PlayerId);
		}
		return num.Clamp(MinMax.Min, MinMax.Max);
	}

	public void IncrementMoraleBoost(int amount = 1)
	{
		PlayerStatusInfo status = Player.GetStatus(PlayerId);
		status.moraleBoost += (byte)amount;
		status.moraleBoostDate = Game.instance.CurrentDate;
	}

	public byte GetRating(DateTime theDate)
	{
		return (byte)GetRatingPrecise(theDate);
	}

	public byte GetRatingQuick()
	{
		return Math.Max((byte)60, CacheRating);
	}

	public byte GetRating()
	{
		return Rating;
	}

	public double GetRatingPrecise(DateTime theDate, bool getTrainingIncrease = false)
	{
		int seed = Game.instance.Random.GetSeed();
		int seed2 = Game.instance.Seed + PlayerId;
		Game.instance.Random.SetSeed(seed2);
		bool flag = ClubId == Game.GetCurrentUserClubID();
		DateTime dateTime = ((!(RespawnDate != DateTime.MinValue)) ? Game.instance.StartDate : RespawnDate);
		if (theDate == DateTime.MinValue)
		{
			theDate = Game.instance.CurrentDate;
		}
		if (theDate == DateTime.MinValue || (theDate <= dateTime && !getTrainingIncrease && !flag))
		{
			Game.instance.Random.SetSeed(seed);
			return (int)Rating;
		}
		int num = 0;
		byte rating = Rating;
		byte b = Age(dateTime);
		byte b2 = Potential;
		int potentialAge = GetPotentialAge();
		if (b2 < rating)
		{
			b2 = rating;
		}
		DateTime potentialDate = GetPotentialDate();
		double result = (int)rating;
		if (PlayerId != 0)
		{
			bool inDecline = InDecline;
			int num2 = 0;
			int num3 = 0;
			bool flag2 = Game.instance.g_trainingDrillManager.TrainingPlayer.ContainsKey(PlayerId);
			if (flag2)
			{
				bool flag3 = Age() >= GetPotentialAge();
				if (!inDecline)
				{
					int num4 = ((getTrainingIncrease && !flag3) ? ((int)Training.GetTrainingIncrease(PlayerId, TrainingManager.GetIncreaseAmounts())) : 0);
					num = (int)Game.instance.g_trainingDrillManager.TrainingPlayer[PlayerId].DaysIncrease + num4;
					num2 = (int)Game.instance.g_trainingDrillManager.TrainingPlayer[PlayerId].DeclineDecrease + num4;
					num3 = (int)Game.instance.g_trainingDrillManager.TrainingPlayer[PlayerId].DeclineDelay + num4;
				}
				else
				{
					int num5 = (getTrainingIncrease ? ((int)Training.GetTrainingIncrease(PlayerId, TrainingManager.GetIncreaseAmounts())) : 0);
					num2 = (int)Game.instance.g_trainingDrillManager.TrainingPlayer[PlayerId].DeclineDecrease + num5;
					num = (theDate - dateTime).Days - num2;
				}
			}
			else
			{
				num = (theDate - dateTime).Days;
			}
			if (ShouldUseIncreasingWave(inDecline, rating, b2, theDate, potentialDate))
			{
				result = UseIncreasingWave(rating, b2, num, potentialDate, dateTime);
			}
			else
			{
				bool longevity = GetLongevity() == PLAYER_LONGEVITY.GOOD;
				int declineAge = GetDeclineAge(seed2, potentialAge, dateTime, b);
				DateTime dateTime2 = default;
				dateTime2 = new DateTime(dateTime.Year + (declineAge - b), dateTime.Month, Math.Min(dateTime.Day, 28)).AddDays(num3);
				if (inDecline)
				{
					b2 = rating;
					dateTime2 = dateTime;
				}
				else if (theDate < dateTime2)
				{
					result = ((!flag2) ? ((double)(int)Potential) : UseIncreasingWave(rating, b2, num, potentialDate, dateTime));
				}
				if (inDecline || theDate >= dateTime2)
				{
					if (flag2)
					{
						byte potentialRating = (byte)UseIncreasingWave(rating, b2, num, potentialDate, dateTime);
						result = UseDecreasingWave(dateTime2, dateTime, num, potentialRating, longevity);
					}
					else
					{
						result = UseDecreasingWave(dateTime2, dateTime, num, b2, longevity);
					}
				}
			}
		}
		else
		{
			result = 60.0;
		}
		Game.instance.Random.SetSeed(seed);
		return result;
	}

	public static double GetIncreasingRatingFromSinWavePrecise(byte startRating, byte potentialRating, int startAge, int potentialAge, int currentAge)
	{
		byte b = (byte)(potentialRating - startRating);
		int num = potentialAge - startAge;
		double num2 = (double)(currentAge - startAge) / (double)num * 90.0;
		if (double.IsNaN(num2))
		{
			num2 = 0.0;
		}
		return (double)(int)startRating + Math.Sin(0.01745329251 * num2) * (double)(int)b;
	}

	public static double GetDecreasingRatingFromSinWavePrecise(byte startRating, byte lowestRating, double startAge, double lowestRatingAge, int currentAge)
	{
		byte b = (byte)(startRating - lowestRating);
		double num = lowestRatingAge - startAge;
		double num2 = ((double)currentAge - startAge) / num * (double)(int)b;
		return (double)(int)startRating - num2;
	}

	private static bool ShouldUseIncreasingWave(bool inDecline, byte startRating, byte potentialRating, DateTime theDate, DateTime potentialDate)
	{
		if (!inDecline && startRating < potentialRating)
		{
			return theDate < potentialDate;
		}
		return false;
	}

	private static double UseIncreasingWave(byte startRating, byte potentialRating, int currentDateDays, DateTime potentialDate, DateTime startDate)
	{
		int days = (potentialDate - startDate).Days;
		return GetIncreasingRatingFromSinWavePrecise(startRating, potentialRating, 0, days, Math.Min(currentDateDays, days));
	}

	private static double UseDecreasingWave(DateTime declineDate, DateTime startDate, int currentDateDays, byte potentialRating, bool longevity)
	{
		int days = (declineDate - startDate).Days;
		int days2 = (new DateTime(declineDate.Year + 15, startDate.Month, Math.Min(startDate.Day, 28)) - startDate).Days;
		int num = potentialRating - (longevity ? Game.instance.Random.GetRand(8, 13) : Game.instance.Random.GetRand(13, 18));
		if (potentialRating >= 93)
		{
			num += 2;
		}
		else if (potentialRating >= 90)
		{
			num++;
		}
		num = Math.Min(potentialRating, num);
		return GetDecreasingRatingFromSinWavePrecise(potentialRating, (byte)Math.Max(num, 60), days, days2, Math.Min(currentDateDays, days2));
	}

	public int GetRatingChangeDate(int year, int month)
	{
		int seed = Game.instance.Random.GetSeed();
		Game.instance.Random.SetSeed((Game.instance.Seed * (PlayerId * 10) + year * 100 + month * 1000) % 32768);
		int rand = Game.instance.Random.GetRand(1, 28);
		Game.instance.Random.SetSeed(seed);
		return rand;
	}

	public int GetTransferRating()
	{
		int num = (int)Attributes.MapRating((int)CacheRating);
		switch (Math.Min(Age(), 37))
		{
		case 36:
		case 37:
			num -= 6;
			break;
		case 35:
			num -= 5;
			break;
		case 33:
		case 34:
			num -= 4;
			break;
		case 31:
		case 32:
			num -= 3;
			break;
		default:
			num -= 2;
			break;
		}
		switch (Potential)
		{
		case 96:
		case 97:
		case 98:
		case 99:
			num += 4;
			break;
		case 94:
		case 95:
			num += 3;
			break;
		case 92:
		case 93:
			num += 2;
			break;
		case 90:
		case 91:
			num++;
			break;
		}
		return Math.Min(num, 99);
	}

	public double GetDisplayRatingPrecise()
	{
		double num = Attributes.MapRating(GetRatingPrecise(Game.instance.CurrentDate));
		switch (Math.Min(Age(), 37))
		{
		case 36:
		case 37:
			num -= 6.0;
			break;
		case 35:
			num -= 5.0;
			break;
		case 33:
		case 34:
			num -= 4.0;
			break;
		case 31:
		case 32:
			num -= 3.0;
			break;
		default:
			num -= 2.0;
			break;
		}
		switch (Potential)
		{
		case 96:
		case 97:
		case 98:
		case 99:
			num += 4.0;
			break;
		case 94:
		case 95:
			num += 3.0;
			break;
		case 92:
		case 93:
			num += 2.0;
			break;
		case 90:
		case 91:
			num++;
			break;
		}
		return Math.Min(num, 99.0);
	}

	public double GetDisplayRatingAfterTraining()
	{
		double num = Attributes.MapRating(GetRatingPrecise(Game.instance.CurrentDate, getTrainingIncrease: true));
		int num2 = Age();
		num -= 2.0;
		if (num2 >= 36)
		{
			num -= 4.0;
		}
		else if (num2 >= 35)
		{
			num -= 3.0;
		}
		else if (num2 >= 33)
		{
			num -= 2.0;
		}
		else if (num2 >= 31)
		{
			num--;
		}
		if (Potential >= 96)
		{
			num += 4.0;
		}
		else if (Potential >= 94)
		{
			num += 3.0;
		}
		else if (Potential >= 92)
		{
			num += 2.0;
		}
		else if (Potential >= 90)
		{
			num++;
		}
		if (!(num <= 99.0))
		{
			return 99.0;
		}
		return num;
	}
}
