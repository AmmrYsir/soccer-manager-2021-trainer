using System;

public class ES2UserType_LeagueData : ES2Type
{
	public override void Write(object obj, ES2Writer writer)
	{
		LeagueData leagueData = (LeagueData)obj;
		writer.Write(leagueData.LeagueID);
		writer.Write(leagueData.Name);
		writer.Write((leagueData.PhotoFilename != null) ? leagueData.PhotoFilename : "");
		writer.Write(leagueData.BaseImgURLType);
		writer.Write(leagueData.AssociationID);
		writer.Write(leagueData.Division);
		writer.Write(leagueData.NumPromoted);
		writer.Write(leagueData.NumRelegated);
		writer.Write(leagueData.NumRounds);
		writer.Write(leagueData.NumTeams);
		writer.Write(leagueData.SeasonOneStart);
		writer.Write(leagueData.SeasonOneEnd);
		writer.Write(leagueData.SeasonOneBreak);
		writer.Write(leagueData.SeasonOneBreakLength);
		writer.Write(leagueData.SeasonTwoStart);
		writer.Write(leagueData.SeasonTwoEnd);
		writer.Write(leagueData.SeasonTwoBreak);
		writer.Write(leagueData.SeasonTwoBreakLength);
		writer.Write(leagueData.BeginTransferMonth);
		writer.Write(leagueData.BeginTransferDay);
		writer.Write(leagueData.EndTransferMonth);
		writer.Write(leagueData.EndTransferDay);
		writer.Write(leagueData.ClubIDs);
		writer.Write(leagueData.Honours);
		writer.Write(leagueData.NumPlayoffPromoted);
		writer.Write(leagueData.NumPlayoffRelegated);
		writer.Write(leagueData.OrigAverageRating);
	}

	public override object Read(ES2Reader reader)
	{
		LeagueData leagueData = new LeagueData();
		Read(reader, leagueData);
		return leagueData;
	}

	public override void Read(ES2Reader reader, object c)
	{
		LeagueData leagueData = (LeagueData)c;
		leagueData.LeagueID = reader.Read<short>();
		leagueData.Name = reader.Read<string>();
		leagueData.PhotoFilename = reader.Read<string>();
		leagueData.BaseImgURLType = reader.Read<short>();
		leagueData.AssociationID = reader.Read<short>();
		leagueData.Division = reader.Read<byte>();
		leagueData.NumPromoted = reader.Read<byte>();
		leagueData.NumRelegated = reader.Read<byte>();
		leagueData.NumRounds = reader.Read<byte>();
		leagueData.NumTeams = reader.Read<byte>();
		leagueData.SeasonOneStart = reader.Read<byte>();
		leagueData.SeasonOneEnd = reader.Read<byte>();
		leagueData.SeasonOneBreak = reader.Read<byte>();
		leagueData.SeasonOneBreakLength = reader.Read<byte>();
		leagueData.SeasonTwoStart = reader.Read<byte>();
		leagueData.SeasonTwoEnd = reader.Read<byte>();
		leagueData.SeasonTwoBreak = reader.Read<byte>();
		leagueData.SeasonTwoBreakLength = reader.Read<byte>();
		leagueData.BeginTransferMonth = reader.Read<short>();
		leagueData.BeginTransferDay = reader.Read<short>();
		leagueData.EndTransferMonth = reader.Read<short>();
		leagueData.EndTransferDay = reader.Read<short>();
		leagueData.ClubIDs = reader.ReadList<short>();
		leagueData.Honours = reader.ReadDictionary<short, short>();
		leagueData.NumPlayoffPromoted = reader.Read<byte>();
		leagueData.NumPlayoffRelegated = reader.Read<byte>();
		leagueData.OrigAverageRating = reader.Read<byte>();
	}

	public override void WriteNonAlloc(ES2Writer writer)
	{
		throw new NotImplementedException();
	}

	public ES2UserType_LeagueData()
		: base(typeof(LeagueData))
	{
	}
}
