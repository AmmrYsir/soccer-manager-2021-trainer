using System;

public class ES2UserType_ManagedClubProfile : ES2Type
{
	public override void Write(object obj, ES2Writer writer)
	{
		ManagedClubProfile managedClubProfile = (ManagedClubProfile)obj;
		writer.Write(managedClubProfile.CurrentSeasonID);
		writer.Write(managedClubProfile.CurrentClubID);
		writer.Write(managedClubProfile.CurrentClubConfidence);
		writer.Write(managedClubProfile.CareerHistory);
		writer.Write(managedClubProfile.ClubWonTrophies);
		writer.Write(managedClubProfile.CustomFormations);
		writer.Write((int)managedClubProfile.SeasonObjective);
		writer.Write(managedClubProfile.ObjectivesList);
		writer.Write(managedClubProfile.RecordSigningAmount);
		writer.Write(managedClubProfile.RecordSigningPlayerID);
		writer.Write(managedClubProfile.RecordSigningReceivedAmount);
		writer.Write(managedClubProfile.RecordSigningReceivedPlayerID);
		writer.Write(managedClubProfile.RecordAttendance);
		writer.Write(managedClubProfile.RecordAttendanceClubID);
		writer.Write(managedClubProfile.RecordWinOppClub);
		writer.Write(managedClubProfile.RecordWinScore);
		writer.Write(managedClubProfile.RecordLossOppClub);
		writer.Write(managedClubProfile.RecordLossScore);
		writer.Write(managedClubProfile.CurrentUnbeatenStreak);
		writer.Write(managedClubProfile.HighestUnbeatenStreak);
		writer.Write(managedClubProfile.CurrentWinStreak);
		writer.Write(managedClubProfile.HighestWinStreak);
		writer.Write(managedClubProfile.GoalsScoredSeason);
		writer.Write(managedClubProfile.GoalsConcededSeason);
		writer.Write(managedClubProfile.CleanSheetsSeason);
		writer.Write(managedClubProfile.AverageAttendanceSeason);
		writer.Write(managedClubProfile.HomeMatchesSeason);
	}

	public override object Read(ES2Reader reader)
	{
		ManagedClubProfile managedClubProfile = new ManagedClubProfile();
		Read(reader, managedClubProfile);
		return managedClubProfile;
	}

	public override void Read(ES2Reader reader, object c)
	{
		ManagedClubProfile managedClubProfile = (ManagedClubProfile)c;
		managedClubProfile.CurrentSeasonID = reader.Read<string>();
		managedClubProfile.CurrentClubID = reader.Read<short>();
		managedClubProfile.CurrentClubConfidence = reader.Read<ClubConfidence>();
		managedClubProfile.CareerHistory = reader.ReadDictionary<short, CareerEntry>();
		managedClubProfile.ClubWonTrophies = reader.ReadDictionary<short, TrophyList>();
		managedClubProfile.CustomFormations = reader.ReadDictionary<int, CustomFormation>();
		managedClubProfile.SeasonObjective = (OverallObjective)reader.Read<int>();
		managedClubProfile.ObjectivesList = reader.Read<ClubObjectivesList>();
		managedClubProfile.RecordSigningAmount = reader.Read<int>();
		managedClubProfile.RecordSigningPlayerID = reader.Read<int>();
		managedClubProfile.RecordSigningReceivedAmount = reader.Read<int>();
		managedClubProfile.RecordSigningReceivedPlayerID = reader.Read<int>();
		managedClubProfile.RecordAttendance = reader.Read<int>();
		managedClubProfile.RecordAttendanceClubID = reader.Read<int>();
		managedClubProfile.RecordWinOppClub = reader.Read<short>();
		managedClubProfile.RecordWinScore = reader.ReadArray<int>();
		managedClubProfile.RecordLossOppClub = reader.Read<short>();
		managedClubProfile.RecordLossScore = reader.ReadArray<int>();
		managedClubProfile.CurrentUnbeatenStreak = reader.Read<int>();
		managedClubProfile.HighestUnbeatenStreak = reader.Read<int>();
		managedClubProfile.CurrentWinStreak = reader.Read<int>();
		managedClubProfile.HighestWinStreak = reader.Read<int>();
		managedClubProfile.GoalsScoredSeason = reader.Read<short>();
		managedClubProfile.GoalsConcededSeason = reader.Read<short>();
		managedClubProfile.CleanSheetsSeason = reader.Read<short>();
		managedClubProfile.AverageAttendanceSeason = reader.Read<short>();
		managedClubProfile.HomeMatchesSeason = reader.Read<short>();
	}

	public override void WriteNonAlloc(ES2Writer writer)
	{
		throw new NotImplementedException();
	}

	public ES2UserType_ManagedClubProfile()
		: base(typeof(ManagedClubProfile))
	{
	}
}
