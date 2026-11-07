using System;

public class ES2UserType_CompetitionStatsContainer : ES2Type
{
	public override void Write(object obj, ES2Writer writer)
	{
		CompetitionStatsContainer competitionStatsContainer = (CompetitionStatsContainer)obj;
		writer.Write(competitionStatsContainer.AllClubStats);
	}

	public override object Read(ES2Reader reader)
	{
		CompetitionStatsContainer competitionStatsContainer = new CompetitionStatsContainer();
		Read(reader, competitionStatsContainer);
		return competitionStatsContainer;
	}

	public override void Read(ES2Reader reader, object c)
	{
		((CompetitionStatsContainer)c).AllClubStats = reader.ReadDictionary<short, StatContainer>();
	}

	public override void WriteNonAlloc(ES2Writer writer)
	{
		throw new NotImplementedException();
	}

	public ES2UserType_CompetitionStatsContainer()
		: base(typeof(CompetitionStatsContainer))
	{
	}
}
