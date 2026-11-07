using System;
using System.Collections.Generic;
using System.Linq;

public class LeagueFinancesController
{
	public static void UpdateLeagueFinances()
	{
		_ = Game.instance.g_LeagueFinances;
		Dictionary<int, DateTime> g_LeagueFinanceSchedule = Game.instance.g_LeagueFinanceSchedule;
		foreach (int item in g_LeagueFinanceSchedule.Keys.ToList())
		{
			if (g_LeagueFinanceSchedule[item] <= Game.instance.CurrentDate)
			{
				if (Game.instance.g_LeagueFinances.ContainsKey(item))
				{
					Game.instance.g_LeagueFinances.Remove(item);
				}
				Game.instance.g_LeagueFinances.Add(item, ChosenLeague.GetClubsByProfits(item));
				g_LeagueFinanceSchedule[item] = g_LeagueFinanceSchedule[item].AddYears(1);
			}
		}
	}
}
