using System;
using System.Collections.Generic;
using System.IO;
using SQLite4Unity3d;
using UnityEngine;

public class SQLiteService
{
	internal BestTeamAwardTbl bestTeamAwardTbl;

	internal TrainingDrillsLogTbl trainingDrillsTbl;

	private int SlotNumber { get; set; }

	private SQLiteConnection dbConnection { get; set; }

	internal GameDisciplinaryEventTbl gameDisciplinaryEventTbl { get; set; }

	internal GameInjuryEventTbl gameInjuryEventTbl { get; set; }

	internal GameSubEventTbl gameSubEventTbl { get; set; }

	internal GameGoalEventTbl gameGoalEventTbl { get; set; }

	internal GameAssistEventTbl gameAssistEventTbl { get; set; }

	internal FixtureTbl fixtureTbl { get; set; }

	internal FixtureClubTbl fixtureClubTbl { get; set; }

	internal List<int> PlayerStaticAdd { get; set; } = new List<int>();

	internal List<int> PlayerStaticUpdate { get; set; } = new List<int>();

	private PlayerStaticTbl playerStaticDataTbl { get; set; }

	internal CareerItemTbl careerTbl { get; set; }

	internal PlayerAwardTbl playerAwardTbl { get; set; }

	internal PlayerInjuryTbl playerInjuryTbl { get; set; }

	internal PlayerInjuryHistoryTbl playerInjuryHistoryTbl { get; set; }

	internal RetiredPlayerTbl retiredPlayerTbl { get; set; }

	internal BidTbl bidTbl { get; set; }

	internal ContractInfoTbl contractInfoTbl { get; set; }

	internal LoanProfileTbl loanProfileTbl { get; set; }

	internal RatingBandingGroupTbl ratingBandingGroupTbl { get; set; }

	internal SquadProfileTbl squadProfileTbl { get; set; }

	internal TransferHistoryTbl transferHistoryTbl { get; set; }

	internal TransferProfileTbl transferProfileTbl { get; set; }

	internal FreeAgentTbl freeAgentTbl { get; set; }

	internal void Start(int slotNumber)
	{
		SlotNumber = slotNumber;
		if (!OpenSQLite("SM21_" + slotNumber + ".db"))
		{
			CreateDB();
		}
	}

	internal void OpenConnection(int slotNumber)
	{
		SlotNumber = slotNumber;
		string text = "SM21_" + slotNumber + ".db";
		if (!OpenSQLite(text))
		{
			Debug.LogError("SQLiteService OpenConnection Failed for fileName: " + text);
		}
	}

	internal void CloseConnection()
	{
		if (dbConnection != null)
		{
			dbConnection.Close();
			dbConnection = null;
		}
	}

	internal void CloseConnectionWithGarbageCollection()
	{
		CloseConnection();
		if (RemoteDefaultController.Instance.GetRemoteConfigValue(RemoteKeys.SQLiteCloseConnectionGC, defaultValue: false))
		{
			GC.Collect();
			GC.WaitForPendingFinalizers();
		}
	}

	public void BeginTransaction()
	{
		dbConnection.BeginTransaction();
	}

	public void EndTransaction()
	{
		dbConnection.Commit();
		CloseConnection();
		OpenSQLite("SM21_" + SlotNumber + ".db");
	}

	private void CreateDB()
	{
		CreatePlayerTables();
		bestTeamAwardTbl.CreateTable();
		trainingDrillsTbl.CreateTable();
		CreateFixtureTables();
		CreateTransferTables();
	}

	private void CreateTransferTables()
	{
		bidTbl.CreateTable();
		contractInfoTbl.CreateTable();
		loanProfileTbl.CreateTable();
		ratingBandingGroupTbl.CreateTable();
		squadProfileTbl.CreateTable();
		transferHistoryTbl.CreateTable();
		transferProfileTbl.CreateTable();
		freeAgentTbl.CreateTable();
	}

	private void CreatePlayerTables()
	{
		careerTbl.CreateTable();
		playerAwardTbl.CreateTable();
		playerInjuryTbl.CreateTable();
		playerInjuryHistoryTbl.CreateTable();
		playerStaticDataTbl.CreateTable();
		retiredPlayerTbl.CreateTable();
	}

	public void CreateInjuryTbl()
	{
		playerInjuryTbl.RecreateTable();
	}

	internal void CreateFixtureTables()
	{
		fixtureTbl.CreateTable();
		fixtureClubTbl.CreateTable();
		gameDisciplinaryEventTbl.CreateTable();
		gameInjuryEventTbl.CreateTable();
		gameSubEventTbl.CreateTable();
		gameGoalEventTbl.CreateTable();
		gameAssistEventTbl.CreateTable();
	}

	private void DeleteFixtureTables()
	{
		dbConnection.DropTable<FixtureRec>();
		dbConnection.DropTable<FixtureClubRec>();
		dbConnection.DropTable<GameDisciplinaryEventRec>();
		dbConnection.DropTable<GameInjuryEventRec>();
		dbConnection.DropTable<GameSubEventRec>();
		dbConnection.DropTable<GameGoalEventRec>();
		dbConnection.DropTable<GameAssistEventRec>();
	}

	public void RemoveAllSingleSeasonData()
	{
		if (dbConnection != null)
		{
			BeginTransaction();
			DeleteFixtureTables();
			CreateFixtureTables();
			EndTransaction();
		}
	}

	private bool OpenSQLite(string databaseName)
	{
		string text = Application.persistentDataPath + "/" + databaseName;
		bool result = File.Exists(text);
		try
		{
			dbConnection = new SQLiteConnection(text, SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.Create);
			careerTbl = new CareerItemTbl(dbConnection);
			gameDisciplinaryEventTbl = new GameDisciplinaryEventTbl(dbConnection);
			gameInjuryEventTbl = new GameInjuryEventTbl(dbConnection);
			gameSubEventTbl = new GameSubEventTbl(dbConnection);
			gameGoalEventTbl = new GameGoalEventTbl(dbConnection);
			gameAssistEventTbl = new GameAssistEventTbl(dbConnection);
			playerInjuryTbl = new PlayerInjuryTbl(dbConnection);
			playerInjuryHistoryTbl = new PlayerInjuryHistoryTbl(dbConnection);
			playerAwardTbl = new PlayerAwardTbl(dbConnection);
			playerStaticDataTbl = new PlayerStaticTbl(dbConnection);
			if (!playerStaticDataTbl.Exists())
			{
				playerStaticDataTbl.CreateTable();
			}
			retiredPlayerTbl = new RetiredPlayerTbl(dbConnection);
			bestTeamAwardTbl = new BestTeamAwardTbl(dbConnection);
			trainingDrillsTbl = new TrainingDrillsLogTbl(dbConnection);
			fixtureTbl = new FixtureTbl(dbConnection);
			fixtureClubTbl = new FixtureClubTbl(dbConnection);
			bidTbl = new BidTbl(dbConnection);
			contractInfoTbl = new ContractInfoTbl(dbConnection);
			loanProfileTbl = new LoanProfileTbl(dbConnection);
			ratingBandingGroupTbl = new RatingBandingGroupTbl(dbConnection);
			squadProfileTbl = new SquadProfileTbl(dbConnection);
			transferHistoryTbl = new TransferHistoryTbl(dbConnection);
			transferProfileTbl = new TransferProfileTbl(dbConnection);
			freeAgentTbl = new FreeAgentTbl(dbConnection);
		}
		catch (Exception ex)
		{
			if (ex != null)
			{
				string.IsNullOrEmpty(ex.Message);
			}
		}
		return result;
	}

	public void DeleteDB(int slotNumber)
	{
		string text = "SM21_" + slotNumber + ".db";
		if (Save.currentSlot == slotNumber)
		{
			CloseConnection();
		}
		try
		{
			string path = Application.persistentDataPath + "/" + text;
			if (File.Exists(path))
			{
				File.Delete(path);
			}
		}
		catch (Exception ex)
		{
			if (ex != null && !string.IsNullOrEmpty(ex.Message))
			{
				Debug.LogError("Error: " + ex.Message + " - trying to delete sqlite database " + text);
			}
		}
	}

	public void CopyDB(int sourceSlot, int destSlot)
	{
		try
		{
			string sourceFileName = Application.persistentDataPath + "/SM21_" + sourceSlot + ".db";
			string text = Application.persistentDataPath + "/SM21_" + destSlot + ".db";
			string text2 = Application.persistentDataPath + "/SM21_temp.db";
			CloseConnection();
			File.Copy(sourceFileName, text2, overwrite: true);
			if (File.Exists(text))
			{
				File.Delete(text);
			}
			File.Move(text2, text);
		}
		catch (Exception ex)
		{
			if (ex != null && !string.IsNullOrEmpty(ex.Message))
			{
				Debug.LogError("Error: " + ex.Message + " - trying to copy database from slot " + sourceSlot + " into slot " + destSlot);
			}
			throw ex;
		}
	}

	public void SaveDBToTemp()
	{
		CopyDB(Save.currentSlot, -1);
	}

	internal void Persist()
	{
		try
		{
			OpenConnection(Save.currentSlot);
			BeginTransaction();
			foreach (int item in PlayerStaticAdd)
			{
				playerStaticDataTbl.Add(item);
			}
			PlayerStaticAdd.Clear();
			foreach (int item2 in PlayerStaticUpdate)
			{
				playerStaticDataTbl.Update(item2);
			}
			PlayerStaticUpdate.Clear();
			freeAgentTbl.Persist(Transfers.instance.unattachedPlayerManager.freeAgentManager.allFreeAgentsList);
			Transfers.instance.historyManager.PersistTransferHistory();
			Game.instance.g_season.Schedule.PersistPlayedFixtures();
			Game.instance.g_injuredPlayers.Persist();
			retiredPlayerTbl.Persist();
			EndTransaction();
		}
		catch (Exception ex)
		{
			if (ex != null)
			{
				string.IsNullOrEmpty(ex.Message);
			}
			throw ex;
		}
	}

	internal void RestoreAll()
	{
		BeginTransaction();
		_ = Save.saveVersion;
		if (Save.saveVersion == 16)
		{
			foreach (int key in Game.instance.g_PlayerData.Keys)
			{
				Game.instance.sqLiteService.PlayerStaticAdd.Add(key);
			}
		}
		else
		{
			playerStaticDataTbl.RestoreAll();
		}
		playerInjuryTbl.RestoreAll();
		EndTransaction();
	}

	public void Add(eSQLiteTbl eSqLiteTbl, TransferProfileData transferProfileData, short clubId, bool isOriginal)
	{
		if (eSqLiteTbl == eSQLiteTbl.TransferProfileTbl)
		{
			transferProfileTbl.Add(transferProfileData, clubId, isOriginal);
			ratingBandingGroupTbl.Add(transferProfileData.ratingBandings, clubId, isOriginal);
		}
	}

	public bool Add(eSQLiteTbl eSqLiteTbl, Transfer _transfer)
	{
		if (eSqLiteTbl == eSQLiteTbl.TransferHistoryTbl)
		{
			transferHistoryTbl.AddItem(_transfer);
		}
		return true;
	}
}
