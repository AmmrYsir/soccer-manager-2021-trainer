using System;
using SQLite4Unity3d;

public class SaveHistoryRec
{
	[PrimaryKey]
	[AutoIncrement]
	public int Id { get; set; }

	public DateTime date { get; set; }
}
