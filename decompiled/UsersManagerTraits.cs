public class UsersManagerTraits : ManagerTrait
{
	public int CurrentTraitLevel;

	public UsersManagerTraits()
	{
	}

	public UsersManagerTraits(ManagerTrait baseTrait)
	{
		TraitID = baseTrait.TraitID;
		TraitName = baseTrait.TraitName;
		Type = baseTrait.Type;
		Area = baseTrait.Area;
		Affect = baseTrait.Affect;
		BaseTrait = baseTrait.BaseTrait;
		LevelCount = baseTrait.LevelCount;
		Levels = baseTrait.Levels;
		Obtained = true;
		CurrentTraitLevel = 0;
	}

	public double ApplyAffect(double originalValue)
	{
		return Affect switch
		{
			ManagerTraitAffect.IncreasePercentage => originalValue += originalValue * (double)Levels[CurrentTraitLevel].AffectAmount, 
			ManagerTraitAffect.ReductionPercentage => originalValue - originalValue * (double)Levels[CurrentTraitLevel].AffectAmount, 
			ManagerTraitAffect.IncreaseValue => originalValue + (double)Levels[CurrentTraitLevel].AffectAmount, 
			ManagerTraitAffect.DecreaseValue => originalValue - (double)Levels[CurrentTraitLevel].AffectAmount, 
			_ => 1.0, 
		};
	}
}
