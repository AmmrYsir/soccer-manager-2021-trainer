public class Sponsor
{
	public enum PaymentType
	{
		WEEKLY,
		MONTHLY,
		ANNUAL
	}

	public int ID;

	public int PerSeasonAmount;

	public int Seasons;

	public PaymentType PaymentMethod;

	public void GenerateSponsor(int id)
	{
		ID = id;
		if (id <= 2)
		{
			Seasons = 1;
		}
		else if (id <= 4)
		{
			Seasons = 2;
		}
		else if (id <= 6)
		{
			Seasons = 1;
		}
		else
		{
			Seasons = 2;
		}
		PerSeasonAmount = GenerateSeasonAmount(Seasons);
		if (id % 2 == 0)
		{
			PaymentMethod = PaymentType.MONTHLY;
			return;
		}
		PaymentMethod = PaymentType.ANNUAL;
		PerSeasonAmount = (int)((float)PerSeasonAmount * 0.6f);
	}

	public int GetPerSeasonAmount()
	{
		int num = PerSeasonAmount;
		if (UserManagerProfileController.UserHasTrait(ManagerTraitArea.SponsorshipTicketPrice))
		{
			num = (int)UserManagerProfileController.ApplyTrait(ManagerTraitArea.SponsorshipTicketPrice, num);
		}
		return num;
	}

	private int GenerateSeasonAmount(int Seasons)
	{
		int num = Economy.CalcSponsorshipMoney(Game.GetCurrentUserClubID());
		int num2 = 0;
		num2 = Seasons switch
		{
			1 => Game.instance.Random.GetRand(75, 80), 
			2 => Game.instance.Random.GetRand(80, 85), 
			_ => Game.instance.Random.GetRand(105, 110), 
		};
		int num3 = num / 100 * num2;
		if (UserManagerProfileController.UserHasTrait(ManagerTraitArea.SponsorshipTicketPrice))
		{
			num3 = (int)UserManagerProfileController.ApplyTrait(ManagerTraitArea.SponsorshipTicketPrice, num3);
		}
		return num3;
	}
}
