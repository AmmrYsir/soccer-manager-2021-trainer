public class TrainingConfig : DebugDataController
{
	public static float AttributeIncreaseLow = 0.02f;

	public static float AttributeIncreaseMid = 0.04f;

	public static float AttributeIncreaseHigh = 0.06f;

	public static float RestIncreaseAmount = 16f;

	public static float UntrainDecreaseAmount = 0.01f;

	public static float FacilityBoostAmount = 2f;

	public static float InjuryChanceLow = 1f;

	public static float InjuryChangeMid = 1.5f;

	public static float InjuryChangeHigh = 2.5f;

	public static float FitnessDecreaseLow = -3f;

	public static float FitnessDecreaseMid = -5f;

	public static float FitnessDecreaseHigh = -10f;

	public static int AttributeCap = 10;

	public static int AttributeTotalCap = 100;

	public static float IncreaseDaysAmount = 8f;

	public static float DeclineDelayAmount = 8f;

	public static float DeclineDecreaseAmount = 8f;

	public static int MoraleDecreasePerc = 25;

	public static int MoraleLevelDecrease = 3;

	public static byte SessionLimit = 2;

	public static float CapacityPenaltyPerc = 2f;

	public static float PhysicalAttrIncrease = 0.05f;

	public static float InjuryChance = 70f;

	public static int YouthGameTimeHighAmount = 30;

	public static int YouthGameTimeLowAmount = 10;

	public static float YouthGameTimeHighBoost = 1.5f;

	public static float YouthGameTimeLowBoost = 1.2f;
}
