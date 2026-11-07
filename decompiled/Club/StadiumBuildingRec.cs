using SQLite4Unity3d;

public class StadiumBuildingRec
{
	[PrimaryKey]
	[AutoIncrement]
	public int Id { get; set; }

	public short clubId { get; set; }

	public byte stand1 { get; set; }

	public byte stand2 { get; set; }

	public byte stand3 { get; set; }

	public byte stand4 { get; set; }

	public byte stand5 { get; set; }

	public byte stand6 { get; set; }

	public byte stand7 { get; set; }

	public byte stand8 { get; set; }

	public byte roof1 { get; set; }

	public byte roof2 { get; set; }

	public byte roof3 { get; set; }

	public byte roof4 { get; set; }

	public byte roof5 { get; set; }

	public byte roof6 { get; set; }

	public byte roof7 { get; set; }

	public byte roof8 { get; set; }

	public byte[] BuildRoofList()
	{
		return new byte[8] { roof1, roof2, roof3, roof4, roof5, roof6, roof7, roof8 };
	}

	public byte[] BuildStandList()
	{
		return new byte[8] { stand1, stand2, stand3, stand4, stand5, stand6, stand7, stand8 };
	}

	public void Add(short id, StadiumStaticData.StadiumBuilding stadiumBuilding)
	{
		clubId = id;
		stand1 = stadiumBuilding.stands[0];
		stand2 = stadiumBuilding.stands[1];
		stand3 = stadiumBuilding.stands[2];
		stand4 = stadiumBuilding.stands[3];
		stand5 = stadiumBuilding.stands[4];
		stand6 = stadiumBuilding.stands[5];
		stand7 = stadiumBuilding.stands[6];
		stand8 = stadiumBuilding.stands[7];
		roof1 = stadiumBuilding.roof[0];
		roof2 = stadiumBuilding.roof[1];
		roof3 = stadiumBuilding.roof[2];
		roof4 = stadiumBuilding.roof[3];
		roof5 = stadiumBuilding.roof[4];
		roof6 = stadiumBuilding.roof[5];
		roof7 = stadiumBuilding.roof[6];
		roof8 = stadiumBuilding.roof[7];
	}
}
