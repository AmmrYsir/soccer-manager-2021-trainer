using System;

public class ES2UserType_PlayerShortlist : ES2Type
{
	public override void Write(object obj, ES2Writer writer)
	{
		PlayerShortlist playerShortlist = (PlayerShortlist)obj;
		writer.Write(playerShortlist.date);
		writer.Write(playerShortlist.notes);
	}

	public override object Read(ES2Reader reader)
	{
		PlayerShortlist playerShortlist = new PlayerShortlist();
		Read(reader, playerShortlist);
		return playerShortlist;
	}

	public override void Read(ES2Reader reader, object c)
	{
		PlayerShortlist playerShortlist = (PlayerShortlist)c;
		playerShortlist.date = reader.Read<DateTime>();
		playerShortlist.notes = reader.Read<string>();
	}

	public override void WriteNonAlloc(ES2Writer writer)
	{
		throw new NotImplementedException();
	}

	public ES2UserType_PlayerShortlist()
		: base(typeof(PlayerShortlist))
	{
	}
}
