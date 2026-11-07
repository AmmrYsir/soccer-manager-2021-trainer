using System.Collections.Generic;
using SQLite4Unity3d;

public class ClubDataTbl : DataServiceTbl
{
	private ClubDataRec clubDataRec = new ClubDataRec();

	public ClubDataTbl(SQLiteConnection _dbConnection)
	{
		dbConnection = _dbConnection;
	}

	public void Add(short id, Club club)
	{
		clubDataRec.Add(id, club);
		dbConnection.Insert(clubDataRec);
	}

	public void CreateTable()
	{
		dbConnection.CreateTable<ClubDataRec>();
	}

	public IEnumerable<ClubDataRec> GetItems()
	{
		return dbConnection.Table<ClubDataRec>();
	}
}
