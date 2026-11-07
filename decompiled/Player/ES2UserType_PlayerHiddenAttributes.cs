using System;

public class ES2UserType_PlayerHiddenAttributes : ES2Type
{
	public override void Write(object obj, ES2Writer writer)
	{
		PlayerHiddenAttributes playerHiddenAttributes = (PlayerHiddenAttributes)obj;
		writer.Write(playerHiddenAttributes.Ambition);
		writer.Write(playerHiddenAttributes.Loyalty);
		writer.Write(playerHiddenAttributes.HandlingPressure);
	}

	public override object Read(ES2Reader reader)
	{
		PlayerHiddenAttributes playerHiddenAttributes = new PlayerHiddenAttributes();
		Read(reader, playerHiddenAttributes);
		return playerHiddenAttributes;
	}

	public override void Read(ES2Reader reader, object c)
	{
		PlayerHiddenAttributes playerHiddenAttributes = (PlayerHiddenAttributes)c;
		playerHiddenAttributes.Ambition = reader.Read<byte>();
		playerHiddenAttributes.Loyalty = reader.Read<byte>();
		playerHiddenAttributes.HandlingPressure = reader.Read<byte>();
	}

	public override void WriteNonAlloc(ES2Writer writer)
	{
		throw new NotImplementedException();
	}

	public ES2UserType_PlayerHiddenAttributes()
		: base(typeof(PlayerHiddenAttributes))
	{
	}
}
