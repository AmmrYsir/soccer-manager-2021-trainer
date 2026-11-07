using System.Collections.Generic;
using UnityEngine;

public class cMatchStats
{
	public cSoccerEngine soccerEngine;

	public SeFixtureInfo.SeFixtureInfoData fixtureInfo;

	public bool hasChanges;

	public float time;

	public List<cDomination> domination;

	public Dictionary<float, int> timingEvents;

	private bool timingEventsChanged;

	public Dictionary<string, cTeamStats> teams;

	public cPlayerStats createPlayerStatsEntry()
	{
		return new cPlayerStats();
	}

	public void initPlayerStats(cTeamStats teamStats)
	{
		SeFixtureInfo seFixtureInfo = new SeFixtureInfo();
		fixtureInfo = seFixtureInfo.fixtureInfo;
		int num = 18;
		teamStats.playerStats = new cPlayerStats[num + 1];
		for (int i = 1; i <= num; i++)
		{
			teamStats.playerStats[i] = createPlayerStatsEntry();
		}
	}

	public void init(cSoccerEngine soccerEngine)
	{
		this.soccerEngine = soccerEngine;
		teams = new Dictionary<string, cTeamStats>();
		string[] array = new string[2] { "home", "away" };
		domination = new List<cDomination>();
		for (int i = 0; i < array.Length; i++)
		{
			teams.Add(array[i], new cTeamStats());
			cTeamStats teamStats = teams[array[i]];
			initPlayerStats(teamStats);
		}
		hasChanges = true;
		time = 0f;
		timingEvents = new Dictionary<float, int>();
		timingEventsChanged = true;
	}

	private float toClockTime(float time, bool FTclamp = true)
	{
		float num = soccerEngine.match.simTimeToMatchTime(time);
		if (FTclamp && soccerEngine.match.currentPeriod == 1 && num > 5400f)
		{
			num = 5400f;
		}
		return num;
	}

	public void registerTimingEvent(float time, int type)
	{
		time = toClockTime(time, FTclamp: false);
		foreach (KeyValuePair<float, int> timingEvent in timingEvents)
		{
			if (timingEvent.Value == type)
			{
				return;
			}
		}
		timingEvents.Add(time, type);
		hasChanges = true;
		timingEventsChanged = true;
	}

	public void updatePossession(string homeAway, float timeOnBall)
	{
		cTeamStats cTeamStats2 = teams[homeAway];
		cTeamStats cTeamStats3 = teams[soccerEngine.otherTeam(homeAway)];
		cTeamStats2.realPoss += timeOnBall;
		float num = cTeamStats2.realPoss + cTeamStats3.realPoss;
		float f = (cTeamStats2.realPoss - cTeamStats3.realPoss) / num;
		float num2 = Mathf.Pow(Mathf.Abs(f), 0.5f) * Mathf.Sign(f);
		cTeamStats2.poss = 1f + num2;
		cTeamStats3.poss = 1f - num2;
		domination.Add(new cDomination(homeAway, timeOnBall));
		if (domination.Count > 15)
		{
			domination.RemoveAt(0);
		}
		hasChanges = true;
	}

	public void updateFitness(string homeAway, int playerID, float fitness)
	{
		teams[homeAway].playerStats[playerID].fitness = Mathf.Round(fitness);
	}

	public void updateShootout(string[] order, int[] score)
	{
		teams[order[0]].shootout = (byte)score[0];
		teams[order[1]].shootout = (byte)score[1];
	}

	public void updatePerf(string homeAway, int playerID, float perf)
	{
		teams[homeAway].playerStats[playerID].performance = Mathf.Round(perf * 10f) / 10f;
	}

	public void registerGoal(string homeAway, string scorerTeam, int playerID, int goalType, float time, string assistHomeAway, int assistPlayerID, int prevAction)
	{
		time = toClockTime(time);
		int num = 0;
		soccerEngine.match.debugGoal(homeAway, goalType);
		teams[homeAway].score++;
		if (goalType != 4)
		{
			if (assistHomeAway == homeAway && assistPlayerID != playerID)
			{
				num = assistPlayerID;
				teams[homeAway].playerStats[num].assists++;
			}
		}
		else if (scorerTeam == "home")
		{
			scorerTeam = "away";
		}
		else if (scorerTeam == "away")
		{
			scorerTeam = "home";
		}
		if (teams[scorerTeam].scorerInfo[playerID] == null)
		{
			teams[scorerTeam].scorerInfo[playerID] = new List<cScorerInfo>();
			teams[scorerTeam].scorerIDs.Add(playerID);
		}
		teams[scorerTeam].scorerInfo[playerID].Add(new cScorerInfo(Mathf.Floor(time), goalType, num));
		if (goalType == 4)
		{
			teams[scorerTeam].playerStats[playerID].ownGoals++;
		}
		else
		{
			teams[scorerTeam].playerStats[playerID].goalsScored++;
			if (prevAction == 6)
			{
				registerEvent(scorerTeam, playerID, "shotot");
			}
		}
		hasChanges = true;
		timingEventsChanged = true;
	}

	public void registerInjury(string homeAway, int playerID, int injType, int injLen, float time)
	{
		time = toClockTime(time);
		if (teams[homeAway].injInfo[playerID] == null)
		{
			teams[homeAway].injInfo[playerID] = new List<cInjInfo>();
			teams[homeAway].injIDs.Add(playerID);
		}
		teams[homeAway].injInfo[playerID].Add(new cInjInfo(Mathf.Floor(time), injLen, injType));
		hasChanges = true;
		timingEventsChanged = true;
	}

	public void registerCard(string homeAway, int playerID, string cardType, float time)
	{
		time = toClockTime(time);
		if (cardType == "yc")
		{
			teams[homeAway].yc++;
			teams[homeAway].playerStats[playerID].yellowCards++;
			if (teams[homeAway].ycInfo[playerID] == null)
			{
				teams[homeAway].ycInfo[playerID] = new List<float>();
				teams[homeAway].ycIDs.Add(playerID);
			}
			teams[homeAway].ycInfo[playerID].Add(Mathf.Floor(time));
			if (teams[homeAway].playerStats[playerID].yellowCards > 1)
			{
				cardType = "rc";
			}
		}
		if (cardType == "rc")
		{
			teams[homeAway].rc++;
			teams[homeAway].playerStats[playerID].redCards++;
			teams[homeAway].rcInfo[playerID] = Mathf.Floor(time);
			teams[homeAway].rcIDs.Add(playerID);
		}
		hasChanges = true;
		timingEventsChanged = true;
	}

	public void registerSub(string homeAway, int playerOn, int PlayerOff, float time)
	{
		time = toClockTime(time);
		teams[homeAway].sub.Add(new cSubStats(Mathf.Floor(time), playerOn, PlayerOff));
		hasChanges = true;
		timingEventsChanged = true;
	}

	public void registerEvent(string homeAway, int playerID, string type)
	{
		cTeamStats cTeamStats2 = teams[homeAway];
		switch (type)
		{
		case "pass":
			cTeamStats2.pass++;
			break;
		case "cpass":
			cTeamStats2.pass++;
			cTeamStats2.cpass++;
			break;
		case "shot":
			cTeamStats2.shot++;
			break;
		case "shotot":
			cTeamStats2.shot++;
			cTeamStats2.shotot++;
			break;
		case "shotBlocked":
			cTeamStats2.shot++;
			cTeamStats2.shotBlocked++;
			break;
		case "corner":
			cTeamStats2.corner++;
			break;
		}
		cPlayerStats cPlayerStats2 = teams[homeAway].playerStats[playerID];
		if (cPlayerStats2 != null)
		{
			switch (type)
			{
			case "pass":
				cPlayerStats2.passes++;
				break;
			case "cpass":
				cPlayerStats2.passes++;
				cPlayerStats2.successfulPasses++;
				break;
			case "shot":
				cPlayerStats2.shots++;
				break;
			case "shotot":
				cPlayerStats2.shots++;
				cPlayerStats2.shotsOnTarget++;
				break;
			case "inter":
				cPlayerStats2.intercepts++;
				break;
			case "tackle":
				cPlayerStats2.tackles++;
				break;
			}
			hasChanges = true;
		}
	}
}
