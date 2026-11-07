using SQLite4Unity3d;

public class ClubDataRec
{
	[PrimaryKey]
	[AutoIncrement]
	public int Id { get; set; }

	public short clubId { get; set; }

	public short clubDataId { get; set; }

	public string name { get; set; }

	public byte transferTier { get; set; }

	public short rivalclubId1 { get; set; }

	public short rivalclubId2 { get; set; }

	public short rivalclubId3 { get; set; }

	public short rivalclubId4 { get; set; }

	public short rivalclubId5 { get; set; }

	public short rivalclubId6 { get; set; }

	public void Add(short id, Club clubData)
	{
		clubId = id;
		clubDataId = clubData.ClubDataID;
		name = clubData.Name;
		transferTier = clubData.transferTier;
		switch (clubData.BitterRivals.Count)
		{
		case 0:
		{
			short num15 = (rivalclubId6 = 0);
			short num10 = (rivalclubId5 = num15);
			short num6 = (rivalclubId4 = num10);
			short num3 = (rivalclubId3 = num6);
			short num = (rivalclubId2 = num3);
			rivalclubId1 = num;
			break;
		}
		case 1:
		{
			rivalclubId1 = clubData.BitterRivals[0];
			short num10 = (rivalclubId6 = 0);
			short num6 = (rivalclubId5 = num10);
			short num3 = (rivalclubId4 = num6);
			short num = (rivalclubId3 = num3);
			rivalclubId2 = num;
			break;
		}
		case 2:
		{
			rivalclubId1 = clubData.BitterRivals[0];
			rivalclubId2 = clubData.BitterRivals[1];
			short num6 = (rivalclubId6 = 0);
			short num3 = (rivalclubId5 = num6);
			short num = (rivalclubId4 = num3);
			rivalclubId3 = num;
			break;
		}
		case 3:
		{
			rivalclubId1 = clubData.BitterRivals[0];
			rivalclubId2 = clubData.BitterRivals[1];
			rivalclubId3 = clubData.BitterRivals[2];
			short num3 = (rivalclubId6 = 0);
			short num = (rivalclubId5 = num3);
			rivalclubId4 = num;
			break;
		}
		case 4:
		{
			rivalclubId1 = clubData.BitterRivals[0];
			rivalclubId2 = clubData.BitterRivals[1];
			rivalclubId3 = clubData.BitterRivals[2];
			rivalclubId4 = clubData.BitterRivals[3];
			short num = (rivalclubId6 = 0);
			rivalclubId5 = num;
			break;
		}
		case 5:
			rivalclubId1 = clubData.BitterRivals[0];
			rivalclubId2 = clubData.BitterRivals[1];
			rivalclubId3 = clubData.BitterRivals[2];
			rivalclubId4 = clubData.BitterRivals[3];
			rivalclubId5 = clubData.BitterRivals[4];
			rivalclubId6 = 0;
			break;
		case 6:
			rivalclubId1 = clubData.BitterRivals[0];
			rivalclubId2 = clubData.BitterRivals[1];
			rivalclubId3 = clubData.BitterRivals[2];
			rivalclubId4 = clubData.BitterRivals[3];
			rivalclubId5 = clubData.BitterRivals[4];
			rivalclubId6 = clubData.BitterRivals[5];
			break;
		}
	}
}
