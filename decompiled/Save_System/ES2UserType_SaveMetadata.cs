using System;

public class ES2UserType_SaveMetadata : ES2Type
{
	public override void Write(object obj, ES2Writer writer)
	{
		SaveMetadata saveMetadata = (SaveMetadata)obj;
		writer.Write(saveMetadata.Name);
		writer.Write(saveMetadata.ClubBadge);
		writer.Write(saveMetadata.GameDate);
		writer.Write(saveMetadata.SaveDate);
		writer.Write(saveMetadata.BuildVersion);
		writer.Write(saveMetadata.SaveUID);
		writer.Write(saveMetadata.Balance);
		writer.Write(saveMetadata.LeaguePosition);
		writer.Write(saveMetadata.ClubID);
		writer.Write(saveMetadata.ClubDataID);
		writer.Write(saveMetadata.DataPackID);
	}

	public override object Read(ES2Reader reader)
	{
		SaveMetadata saveMetadata = new SaveMetadata();
		Read(reader, saveMetadata);
		return saveMetadata;
	}

	public override void Read(ES2Reader reader, object c)
	{
		SaveMetadata saveMetadata = (SaveMetadata)c;
		saveMetadata.Name = reader.Read<string>();
		saveMetadata.ClubBadge = reader.Read<string>();
		saveMetadata.GameDate = reader.Read<DateTime>();
		saveMetadata.SaveDate = reader.Read<DateTime>();
		saveMetadata.BuildVersion = reader.Read<string>();
		saveMetadata.SaveUID = reader.Read<string>();
		saveMetadata.Balance = reader.Read<int>();
		saveMetadata.LeaguePosition = reader.Read<short>();
		saveMetadata.ClubID = reader.Read<short>();
		saveMetadata.ClubDataID = reader.Read<short>();
		saveMetadata.DataPackID = reader.Read<int>();
	}

	public override void WriteNonAlloc(ES2Writer writer)
	{
		throw new NotImplementedException();
	}

	public ES2UserType_SaveMetadata()
		: base(typeof(SaveMetadata))
	{
	}
}
