using System;

public class ES2UserType_FacilityManager : ES2Type
{
	public override void Write(object obj, ES2Writer writer)
	{
		FacilityManager facilityManager = (FacilityManager)obj;
		writer.Write(facilityManager.FacilityUnlockInProgress);
		writer.Write(facilityManager.FacilityUnlocksInProgress);
		writer.Write(facilityManager.FacilityBlobs);
		writer.Write(facilityManager.FacilityLevels);
		writer.Write(facilityManager.RewardAdUnlock);
	}

	public override object Read(ES2Reader reader)
	{
		FacilityManager facilityManager = new FacilityManager();
		Read(reader, facilityManager);
		return facilityManager;
	}

	public override void Read(ES2Reader reader, object c)
	{
		FacilityManager facilityManager = (FacilityManager)c;
		facilityManager.FacilityUnlockInProgress = reader.Read<bool>();
		facilityManager.FacilityUnlocksInProgress = reader.ReadList<FacilityUnlock>();
		facilityManager.FacilityBlobs = reader.ReadDictionary<int, int>();
		facilityManager.FacilityLevels = reader.ReadDictionary<int, int>();
		facilityManager.RewardAdUnlock = reader.Read<FacilityUnlock>();
	}

	public override void WriteNonAlloc(ES2Writer writer)
	{
		throw new NotImplementedException();
	}

	public ES2UserType_FacilityManager()
		: base(typeof(FacilityManager))
	{
	}
}
