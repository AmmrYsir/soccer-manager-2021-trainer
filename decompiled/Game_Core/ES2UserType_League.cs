using System;

public class ES2UserType_League : ES2Type
{
	public override void Write(object obj, ES2Writer writer)
	{
		ChosenLeague chosenLeague = (ChosenLeague)obj;
		writer.Write(chosenLeague.ClubID);
		writer.Write(chosenLeague.ClubIX);
		writer.Write(chosenLeague.GamesPld);
		writer.Write(chosenLeague.GamesWon);
		writer.Write(chosenLeague.GamesDrawn);
		writer.Write(chosenLeague.GoalsFor);
		writer.Write(chosenLeague.GoalsAgainst);
		writer.Write(chosenLeague.Form);
		writer.Write(chosenLeague.PredictedStandings);
		writer.Write(chosenLeague.LeagueDataID);
	}

	public override object Read(ES2Reader reader)
	{
		ChosenLeague chosenLeague = new ChosenLeague();
		Read(reader, chosenLeague);
		return chosenLeague;
	}

	public override void Read(ES2Reader reader, object c)
	{
		ChosenLeague chosenLeague = (ChosenLeague)c;
		chosenLeague.ClubID = reader.ReadArray<short>();
		chosenLeague.ClubIX = reader.ReadArray<byte>();
		chosenLeague.GamesPld = reader.ReadArray<byte>();
		chosenLeague.GamesWon = reader.ReadArray<byte>();
		chosenLeague.GamesDrawn = reader.ReadArray<byte>();
		chosenLeague.GoalsFor = reader.ReadArray<short>();
		chosenLeague.GoalsAgainst = reader.ReadArray<short>();
		chosenLeague.Form = reader.ReadArray<string>();
		chosenLeague.PredictedStandings = reader.ReadList<ClubRating>();
		chosenLeague.LeagueDataID = reader.Read<short>();
	}

	public override void WriteNonAlloc(ES2Writer writer)
	{
		throw new NotImplementedException();
	}

	public ES2UserType_League()
		: base(typeof(ChosenLeague))
	{
	}
}
