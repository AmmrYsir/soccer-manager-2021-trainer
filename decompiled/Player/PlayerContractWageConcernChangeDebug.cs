public class PlayerContractWageConcernChangeDebug : PlayerConcernChangeDebug
{
	public PlayerConcernSummary IsHappy { get; set; }

	public int CurrentWage { get; set; }

	public int ExpectedWage { get; set; }

	public float AcceptableWage { get; set; }

	public float AboveExpectedWage { get; set; }

	public PlayerContractWageConcernChangeDebug(int playerID, sbyte concernChange, sbyte concernValue)
		: base(playerID, concernChange, concernValue)
	{
	}

	public override string ToString()
	{
		return base.ToString() + $"IsHappy = <color={GetPlayerConcernHappinessDebugColour(IsHappy)}>{IsHappy}</color> " + $"CurrentWage = <color={GetGreenOrangeRedValue(AcceptableWage, AboveExpectedWage, CurrentWage)}>{CurrentWage}</color> " + $"ExpectedWage = {ExpectedWage}, " + $"AcceptableWage = {AcceptableWage}, " + $"AboveExpectedWage = {AboveExpectedWage}";
	}

	public override void WriteToSql()
	{
	}
}
