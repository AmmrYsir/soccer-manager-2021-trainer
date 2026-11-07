using System;

public class PlayerContractHelper
{
	public const byte MaxOneYearContractsPerClub = 3;

	public byte teamOneYearContractCount { get; set; }

	public double leagueRatingMultiplier { get; set; }

	public byte leagueStartMonth { get; set; }

	public bool isPlayableLeague { get; set; }

	public double teamRatingMultiplier { get; set; }

	public byte teamAverageRating { get; set; }

	public void SetupLeague(short leagueId, double LeagueRatingMultiplier, byte LeagueStartMonth)
	{
		leagueRatingMultiplier = LeagueRatingMultiplier;
		leagueStartMonth = LeagueStartMonth;
		int assocID = LeagueData.GetAssocID(leagueId);
		if (Game.instance.ChosenCountries.Contains(assocID) && Game.instance.g_LeagueData.ContainsKey(leagueId))
		{
			isPlayableLeague = true;
		}
		else
		{
			isPlayableLeague = false;
		}
	}

	public void SetupClub(double TeamRatingMultiplier, byte TeamAverageRating)
	{
		teamRatingMultiplier = Math.Max(0.7, TeamRatingMultiplier);
		teamAverageRating = Math.Max((byte)60, TeamAverageRating);
		teamOneYearContractCount = 0;
	}
}
