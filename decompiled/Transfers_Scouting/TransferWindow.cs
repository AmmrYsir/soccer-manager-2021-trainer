public class TransferWindow
{
	public short startDay;

	public short startMonth;

	public short endDay;

	public short endMonth;

	public TransferWindow(TransferWindowRec transferWindowRec)
	{
		startDay = (short)transferWindowRec.StartDay;
		startMonth = (short)transferWindowRec.StartMonth;
		endDay = (short)transferWindowRec.EndDay;
		endMonth = (short)transferWindowRec.EndMonth;
	}
}
