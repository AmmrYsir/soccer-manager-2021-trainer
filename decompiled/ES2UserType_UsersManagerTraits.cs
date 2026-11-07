using System;

public class ES2UserType_UsersManagerTraits : ES2Type
{
	public override void Write(object obj, ES2Writer writer)
	{
		UsersManagerTraits usersManagerTraits = (UsersManagerTraits)obj;
		writer.Write(usersManagerTraits.TraitID);
		writer.Write(usersManagerTraits.TraitName);
		writer.Write((int)usersManagerTraits.Type);
		writer.Write((int)usersManagerTraits.Area);
		writer.Write((int)usersManagerTraits.Affect);
		writer.Write(usersManagerTraits.Obtained);
		writer.Write(usersManagerTraits.BaseTrait);
		writer.Write(usersManagerTraits.LevelCount);
		writer.Write(usersManagerTraits.Levels);
		writer.Write(usersManagerTraits.CurrentTraitLevel);
	}

	public override object Read(ES2Reader reader)
	{
		UsersManagerTraits usersManagerTraits = new UsersManagerTraits();
		Read(reader, usersManagerTraits);
		return usersManagerTraits;
	}

	public override void Read(ES2Reader reader, object c)
	{
		UsersManagerTraits usersManagerTraits = (UsersManagerTraits)c;
		usersManagerTraits.TraitID = reader.Read<int>();
		usersManagerTraits.TraitName = reader.Read<string>();
		usersManagerTraits.Type = (ManagerType)reader.Read<int>();
		usersManagerTraits.Area = (ManagerTraitArea)reader.Read<int>();
		usersManagerTraits.Affect = (ManagerTraitAffect)reader.Read<int>();
		usersManagerTraits.Obtained = reader.Read<bool>();
		usersManagerTraits.BaseTrait = reader.Read<bool>();
		usersManagerTraits.LevelCount = reader.Read<int>();
		usersManagerTraits.Levels = reader.ReadList<ManagerTraitLevel>();
		usersManagerTraits.CurrentTraitLevel = reader.Read<int>();
	}

	public override void WriteNonAlloc(ES2Writer writer)
	{
		throw new NotImplementedException();
	}

	public ES2UserType_UsersManagerTraits()
		: base(typeof(UsersManagerTraits))
	{
	}
}
