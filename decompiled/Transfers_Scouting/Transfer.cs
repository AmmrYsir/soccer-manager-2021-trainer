using System;

[Serializable]
public class Transfer
{
	public short ClubIDOld { get; set; }

	public short ClubIDNew { get; set; }

	public int PlayersIn { get; set; }

	public int Amount { get; set; }

	public string SeasonId { get; set; } = "";

	public DateTime Date { get; set; }

	public DateTime LoanEndDate { get; set; }

	public bool ToAddToSQL { get; set; }

	public Transfer()
	{
	}

	public Transfer(Transfer existingTransfer)
	{
		ClubIDNew = existingTransfer.ClubIDNew;
		ClubIDOld = existingTransfer.ClubIDOld;
		PlayersIn = existingTransfer.PlayersIn;
		Amount = existingTransfer.Amount;
		Date = existingTransfer.Date;
		LoanEndDate = existingTransfer.LoanEndDate;
		SeasonId = existingTransfer.SeasonId;
	}

	public Transfer(PendingTransfer pendingTransfer)
	{
		ClubIDNew = pendingTransfer.clubIdNew;
		ClubIDOld = pendingTransfer.clubIdOld;
		PlayersIn = pendingTransfer.playerId;
		Amount = pendingTransfer.amount;
		Date = pendingTransfer.date;
		SeasonId = Season.GetCurrentSeasonID();
	}

	public Transfer(TransferHistoryRec transferSQL)
	{
		ClubIDOld = transferSQL.ClubIDOld;
		ClubIDNew = transferSQL.ClubIDNew;
		PlayersIn = transferSQL.PlayerId;
		SeasonId = transferSQL.SeasonId;
		Amount = transferSQL.Amount;
		Date = transferSQL.Date;
		LoanEndDate = transferSQL.LoanEndDate;
	}

	public bool IsPaidTransfer()
	{
		if (Amount > 0 && Amount != -2 && Amount != -5 && Amount != -3 && Amount != -4 && Amount != -6)
		{
			return true;
		}
		return false;
	}

	public void PersistSQL()
	{
		Game.instance.sqLiteService.transferHistoryTbl.AddItem(this);
	}
}
