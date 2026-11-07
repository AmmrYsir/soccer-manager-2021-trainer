public class cTactics
{
	public cInstructions inst;

	public int[] lineUp;

	public float time;

	public bool pending;

	public cTactics(bool cleanCopy)
	{
		if (lineUp == null)
		{
			lineUp = new int[18];
		}
		inst = new cInstructions();
	}

	public cTactics(float time = 0f, cInstructions inst = null, int[] lineUp = null, bool pending = false)
	{
		if (lineUp == null)
		{
			lineUp = new int[18];
		}
		this.inst = inst;
		this.lineUp = lineUp;
		this.time = time;
		this.pending = pending;
	}
}
