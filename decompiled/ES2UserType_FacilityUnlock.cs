using System;

public class ES2UserType_FacilityUnlock : ES2Type
{
	public override void Write(object obj, ES2Writer writer)
	{
		FacilityUnlock facilityUnlock = (FacilityUnlock)obj;
		writer.Write(facilityUnlock.Level);
		writer.Write(facilityUnlock.FacilityID);
		writer.Write(facilityUnlock.UnlockDate);
	}

	public override object Read(ES2Reader reader)
	{
		FacilityUnlock facilityUnlock = new FacilityUnlock();
		Read(reader, facilityUnlock);
		return facilityUnlock;
	}

	public override void Read(ES2Reader reader, object c)
	{
		FacilityUnlock facilityUnlock = (FacilityUnlock)c;
		facilityUnlock.Level = reader.Read<int>();
		facilityUnlock.FacilityID = reader.Read<int>();
		facilityUnlock.UnlockDate = reader.Read<DateTime>();
	}

	public override void WriteNonAlloc(ES2Writer writer)
	{
		throw new NotImplementedException();
	}

	public ES2UserType_FacilityUnlock()
		: base(typeof(FacilityUnlock))
	{
	}
}
