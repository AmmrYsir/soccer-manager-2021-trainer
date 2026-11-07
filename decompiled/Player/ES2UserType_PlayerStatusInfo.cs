using System;

public class ES2UserType_PlayerStatusInfo : ES2Type
{
	public override void Write(object obj, ES2Writer writer)
	{
		PlayerStatusInfo playerStatusInfo = (PlayerStatusInfo)obj;
		writer.Write(playerStatusInfo.shirtNum);
		writer.Write(playerStatusInfo.fitness);
		writer.Write(playerStatusInfo.morale);
		writer.Write(playerStatusInfo.moraleBoost);
		writer.Write(playerStatusInfo.moraleBoostDate);
		writer.Write(playerStatusInfo.firstTeam);
		writer.Write(playerStatusInfo.retirementWarned);
		writer.Write(playerStatusInfo.lastBidDate);
		writer.Write(playerStatusInfo.IsInstantPurchase);
		writer.Write(playerStatusInfo.Cohesion);
		writer.Write(playerStatusInfo.Concerns);
		writer.Write(playerStatusInfo.ExpectedSquadStatus);
	}

	public override object Read(ES2Reader reader)
	{
		PlayerStatusInfo playerStatusInfo = new PlayerStatusInfo();
		Read(reader, playerStatusInfo);
		return playerStatusInfo;
	}

	public override void Read(ES2Reader reader, object c)
	{
		PlayerStatusInfo playerStatusInfo = (PlayerStatusInfo)c;
		playerStatusInfo.shirtNum = reader.Read<byte>();
		playerStatusInfo.fitness = reader.Read<byte>();
		playerStatusInfo.morale = reader.Read<byte>();
		playerStatusInfo.moraleBoost = reader.Read<byte>();
		playerStatusInfo.moraleBoostDate = reader.Read<DateTime>();
		playerStatusInfo.firstTeam = reader.Read<bool>();
		playerStatusInfo.retirementWarned = reader.Read<bool>();
		playerStatusInfo.lastBidDate = reader.Read<DateTime>();
		playerStatusInfo.IsInstantPurchase = reader.Read<bool>();
		playerStatusInfo.Cohesion = reader.Read<PlayerTeamCohesion>();
		playerStatusInfo.Concerns = reader.Read<PlayerConcernsManagedClub>();
		playerStatusInfo.ExpectedSquadStatus = (ePlayerSquadStatus)reader.Read<int>();
	}

	public override void WriteNonAlloc(ES2Writer writer)
	{
		throw new NotImplementedException();
	}

	public ES2UserType_PlayerStatusInfo()
		: base(typeof(PlayerStatusInfo))
	{
	}
}
