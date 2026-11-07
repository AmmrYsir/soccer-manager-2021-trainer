using System;

public class ES2UserType_LeagueEconomy : ES2Type
{
	public override void Write(object obj, ES2Writer writer)
	{
		LeagueEconomy leagueEconomy = (LeagueEconomy)obj;
		writer.Write(leagueEconomy.AveTicketPrice);
		writer.Write(leagueEconomy.AveAttendance);
		writer.Write(leagueEconomy.WeeklyTVMoney);
		writer.Write(leagueEconomy.PrizeMoneyPot);
	}

	public override object Read(ES2Reader reader)
	{
		LeagueEconomy leagueEconomy = new LeagueEconomy();
		Read(reader, leagueEconomy);
		return leagueEconomy;
	}

	public override void Read(ES2Reader reader, object c)
	{
		LeagueEconomy leagueEconomy = (LeagueEconomy)c;
		leagueEconomy.AveTicketPrice = reader.Read<int>();
		leagueEconomy.AveAttendance = reader.Read<int>();
		leagueEconomy.WeeklyTVMoney = reader.Read<int>();
		leagueEconomy.PrizeMoneyPot = reader.Read<int>();
	}

	public override void WriteNonAlloc(ES2Writer writer)
	{
		throw new NotImplementedException();
	}

	public ES2UserType_LeagueEconomy()
		: base(typeof(LeagueEconomy))
	{
	}
}
