using System;

public class ES2UserType_RequiredYouthPlayer : ES2Type
{
	public override void Write(object obj, ES2Writer writer)
	{
		RequiredYouthPlayer requiredYouthPlayer = (RequiredYouthPlayer)obj;
		writer.Write(requiredYouthPlayer.position);
		writer.Write(requiredYouthPlayer.potential);
		writer.Write(requiredYouthPlayer.time);
		writer.Write(requiredYouthPlayer.isDesperate);
	}

	public override object Read(ES2Reader reader)
	{
		RequiredYouthPlayer requiredYouthPlayer = new RequiredYouthPlayer();
		Read(reader, requiredYouthPlayer);
		return requiredYouthPlayer;
	}

	public override void Read(ES2Reader reader, object c)
	{
		RequiredYouthPlayer requiredYouthPlayer = (RequiredYouthPlayer)c;
		requiredYouthPlayer.position = reader.Read<int>();
		requiredYouthPlayer.potential = reader.Read<int>();
		requiredYouthPlayer.time = reader.Read<DateTime>();
		requiredYouthPlayer.isDesperate = reader.Read<bool>();
	}

	public override void WriteNonAlloc(ES2Writer writer)
	{
		throw new NotImplementedException();
	}

	public ES2UserType_RequiredYouthPlayer()
		: base(typeof(RequiredYouthPlayer))
	{
	}
}
