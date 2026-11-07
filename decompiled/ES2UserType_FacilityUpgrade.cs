using System;

public class ES2UserType_FacilityUpgrade : ES2Type
{
	public override void Write(object obj, ES2Writer writer)
	{
		_ = (FacilityUpgrade)obj;
	}

	public override object Read(ES2Reader reader)
	{
		FacilityUpgrade facilityUpgrade = new FacilityUpgrade();
		Read(reader, facilityUpgrade);
		return facilityUpgrade;
	}

	public override void Read(ES2Reader reader, object c)
	{
		_ = (FacilityUpgrade)c;
	}

	public override void WriteNonAlloc(ES2Writer writer)
	{
		throw new NotImplementedException();
	}

	public ES2UserType_FacilityUpgrade()
		: base(typeof(FacilityUpgrade))
	{
	}
}
