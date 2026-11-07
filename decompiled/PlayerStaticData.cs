using System;

public abstract class PlayerStaticData
{
	public int PlayerId { get; set; }

	internal string Forename { get; set; }

	internal string Surname { get; set; }

	public short CountryDataID { get; set; }

	public string PhotoFileName { get; set; }

	public int Position { get; set; }

	public int PreferredPosition { get; set; }

	public byte Description { get; set; }

	public DateTime DateOfBirth { get; set; }

	public byte Foot { get; set; }

	public byte Height { get; set; }

	public byte Weight { get; set; }

	public byte Potential { get; set; }

	public byte GeneratedPotential { get; set; }

	public RGYBCode RGYBCode { get; set; }

	public byte HairColour { get; set; }

	public byte HairStyle { get; set; }

	public byte SkinColour { get; set; }

	public byte FacialHair { get; set; }

	public byte AttributesRankVotes { get; set; }

	public string AttributesRank { get; set; }

	public int PlayerDataID { get; set; }
}
