using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FinanceOverview : MonoBehaviour
{
	private enum Tabs
	{
		DETAILED,
		PRIZE,
		CLUB
	}

	public Text leagueTitleText;

	private double weeklyIncome;

	private double weeklyOutgoings;

	private double seasonIncome;

	private double seasonOutgoings;

	private List<int> drawnComps = new List<int>();

	public void DrawScreen()
	{
		InitButtons();
		DrawDetailed();
		DrawSponsor();
		DrawClubZone();
		DrawSummary();
		DrawPrizeMoney();
	}

	private void DrawDetailed()
	{
		DrawIncome();
		DrawOutgoings();
	}

	private void InitButtons()
	{
		transform.Find("ScreenContainer/NavBar/DetailedView").GetComponent<Button>().onClick.RemoveAllListeners();
		transform.Find("ScreenContainer/NavBar/PrizeMoney").GetComponent<Button>().onClick.RemoveAllListeners();
		transform.Find("ScreenContainer/NavBar/ClubZone").GetComponent<Button>().onClick.RemoveAllListeners();
		transform.Find("ScreenContainer/NavBar/DetailedView").GetComponent<Button>().onClick.AddListener(() =>
		{
			SwitchTab(Tabs.DETAILED);
		});
		transform.Find("ScreenContainer/NavBar/PrizeMoney").GetComponent<Button>().onClick.AddListener(() =>
		{
			SwitchTab(Tabs.PRIZE);
		});
		transform.Find("ScreenContainer/NavBar/ClubZone").GetComponent<Button>().onClick.AddListener(() =>
		{
			SwitchTab(Tabs.CLUB);
		});
		transform.Find("ScreenContainer/NavBar/DetailedView/text").GetComponent<Text>().text = Translation.GetText("detailed-view").ToUpper();
		transform.Find("ScreenContainer/NavBar/PrizeMoney/text").GetComponent<Text>().text = Translation.GetText("prize-money").ToUpper();
		transform.Find("ScreenContainer/NavBar/ClubZone/text").GetComponent<Text>().text = Translation.GetText("club-zone").ToUpper();
	}

	private void SwitchTab(Tabs tab)
	{
		switch (tab)
		{
		case Tabs.CLUB:
			transform.Find("ScreenContainer/DarkPanel/Detailed").gameObject.SetActive(value: false);
			transform.Find("ScreenContainer/DarkPanel/PrizeMoney").gameObject.SetActive(value: false);
			transform.Find("ScreenContainer/DarkPanel/ClubZone").gameObject.SetActive(value: true);
			break;
		case Tabs.DETAILED:
			transform.Find("ScreenContainer/DarkPanel/Detailed").gameObject.SetActive(value: true);
			transform.Find("ScreenContainer/DarkPanel/PrizeMoney").gameObject.SetActive(value: false);
			transform.Find("ScreenContainer/DarkPanel/ClubZone").gameObject.SetActive(value: false);
			break;
		case Tabs.PRIZE:
			transform.Find("ScreenContainer/DarkPanel/Detailed").gameObject.SetActive(value: false);
			transform.Find("ScreenContainer/DarkPanel/PrizeMoney").gameObject.SetActive(value: true);
			transform.Find("ScreenContainer/DarkPanel/ClubZone").gameObject.SetActive(value: false);
			break;
		}
	}

	private void DrawIncome()
	{
		transform.Find("ScreenContainer/DarkPanel/Detailed/Income/Header/Text").GetComponent<Text>().text = Translation.GetText("income").ToUpper();
		transform.Find("ScreenContainer/DarkPanel/Detailed/Income/Header/Text (1)").GetComponent<Text>().text = Translation.GetText("weekly").ToUpper();
		transform.Find("ScreenContainer/DarkPanel/Detailed/Income/Header/Text (2)").GetComponent<Text>().text = Translation.GetText("gn-Season").ToUpper();
		transform.Find("ScreenContainer/DarkPanel/Detailed/Income/SeasonTicket/Title").GetComponent<Text>().text = Translation.GetText("season-tickets");
		transform.Find("ScreenContainer/DarkPanel/Detailed/Income/Gate/Title").GetComponent<Text>().text = Translation.GetText("gate-receipts");
		transform.Find("ScreenContainer/DarkPanel/Detailed/Income/TVRevenue/Title").GetComponent<Text>().text = Translation.GetText("tv-revenue");
		transform.Find("ScreenContainer/DarkPanel/Detailed/Income/Sponsors/Title").GetComponent<Text>().text = Translation.GetText("sponsor");
		transform.Find("ScreenContainer/DarkPanel/Detailed/Income/Merchandise/Title").GetComponent<Text>().text = Translation.GetText("merchandise");
		transform.Find("ScreenContainer/DarkPanel/Detailed/Income/Transfers/Title").GetComponent<Text>().text = Translation.GetText("transfer-money-in");
		transform.Find("ScreenContainer/DarkPanel/Detailed/Income/CashInjection/Title").GetComponent<Text>().text = Translation.GetText("cash-injection");
		transform.Find("ScreenContainer/DarkPanel/Detailed/Income/PrizeMoney/Title").GetComponent<Text>().text = Translation.GetText("prize-money");
		transform.Find("ScreenContainer/DarkPanel/Detailed/Income/OtherIncome/Title").GetComponent<Text>().text = Translation.GetText("other-income");
		transform.Find("ScreenContainer/DarkPanel/Detailed/Income/TotalRow/ContentTextWhite").GetComponent<Text>().text = Translation.GetText("total-income");
		double num = 0.0;
		double num2 = 0.0;
		double num3 = 0.0;
		double num4 = 0.0;
		double num5 = 0.0;
		double num6 = 0.0;
		double num7 = 0.0;
		double num8 = 0.0;
		double num9 = 0.0;
		for (int i = 0; i < Game.instance.managedClub[Game.instance.g_currUser].BalanceSheet.Count; i++)
		{
			num += Game.instance.managedClub[Game.instance.g_currUser].BalanceSheet[i].SeasonTicketReceipts;
			num2 += Game.instance.managedClub[Game.instance.g_currUser].BalanceSheet[i].GateReceipts;
			num3 += Game.instance.managedClub[Game.instance.g_currUser].BalanceSheet[i].TVRevenue;
			num4 += Game.instance.managedClub[Game.instance.g_currUser].BalanceSheet[i].Sponsor;
			num5 += Game.instance.managedClub[Game.instance.g_currUser].BalanceSheet[i].Merchandise + Game.instance.managedClub[Game.instance.g_currUser].BalanceSheet[i].Hospitality;
			num6 += Game.instance.managedClub[Game.instance.g_currUser].BalanceSheet[i].TransfersIn;
			num7 += Game.instance.managedClub[Game.instance.g_currUser].BalanceSheet[i].CashInjection;
			num8 += Game.instance.managedClub[Game.instance.g_currUser].BalanceSheet[i].PrizeMoney;
			num9 += Game.instance.managedClub[Game.instance.g_currUser].BalanceSheet[i].OtherIncome;
		}
		BalanceSheet balanceSheet = Game.instance.managedClub[Game.instance.g_currUser].BalanceSheet[Game.instance.managedClub[Game.instance.g_currUser].CurrentBalanceSheet];
		transform.Find("ScreenContainer/DarkPanel/Detailed/Income/SeasonTicket/WeeklyAmount").GetComponent<Text>().text = Helpers.GetValueText(balanceSheet.SeasonTicketReceipts);
		transform.Find("ScreenContainer/DarkPanel/Detailed/Income/Gate/WeeklyAmount").GetComponent<Text>().text = Helpers.GetValueText(balanceSheet.GateReceipts);
		transform.Find("ScreenContainer/DarkPanel/Detailed/Income/TVRevenue/WeeklyAmount").GetComponent<Text>().text = Helpers.GetValueText(balanceSheet.TVRevenue);
		transform.Find("ScreenContainer/DarkPanel/Detailed/Income/Sponsors/WeeklyAmount").GetComponent<Text>().text = Helpers.GetValueText(balanceSheet.Sponsor);
		transform.Find("ScreenContainer/DarkPanel/Detailed/Income/Merchandise/WeeklyAmount").GetComponent<Text>().text = Helpers.GetValueText(balanceSheet.Merchandise + balanceSheet.Hospitality);
		transform.Find("ScreenContainer/DarkPanel/Detailed/Income/Transfers/WeeklyAmount").GetComponent<Text>().text = Helpers.GetValueText(balanceSheet.TransfersIn);
		transform.Find("ScreenContainer/DarkPanel/Detailed/Income/CashInjection/WeeklyAmount").GetComponent<Text>().text = Helpers.GetValueText(balanceSheet.CashInjection);
		transform.Find("ScreenContainer/DarkPanel/Detailed/Income/PrizeMoney/WeeklyAmount").GetComponent<Text>().text = Helpers.GetValueText(balanceSheet.PrizeMoney);
		transform.Find("ScreenContainer/DarkPanel/Detailed/Income/OtherIncome/WeeklyAmount").GetComponent<Text>().text = Helpers.GetValueText(balanceSheet.OtherIncome);
		weeklyIncome = balanceSheet.SeasonTicketReceipts + balanceSheet.GateReceipts + balanceSheet.TVRevenue + balanceSheet.Sponsor + balanceSheet.Hospitality + balanceSheet.Merchandise + balanceSheet.TransfersIn + balanceSheet.CashInjection + balanceSheet.PrizeMoney + balanceSheet.OtherIncome;
		transform.Find("ScreenContainer/DarkPanel/Detailed/Income/TotalRow/ContentTextWhite (1)").GetComponent<Text>().text = Helpers.GetValueText(weeklyIncome);
		transform.Find("ScreenContainer/DarkPanel/Detailed/Income/SeasonTicket/SeasonAmount").GetComponent<Text>().text = Helpers.GetValueText(num);
		transform.Find("ScreenContainer/DarkPanel/Detailed/Income/Gate/SeasonAmount").GetComponent<Text>().text = Helpers.GetValueText(num2);
		transform.Find("ScreenContainer/DarkPanel/Detailed/Income/TVRevenue/SeasonAmount").GetComponent<Text>().text = Helpers.GetValueText(num3);
		transform.Find("ScreenContainer/DarkPanel/Detailed/Income/Sponsors/SeasonAmount").GetComponent<Text>().text = Helpers.GetValueText(num4);
		transform.Find("ScreenContainer/DarkPanel/Detailed/Income/Merchandise/SeasonAmount").GetComponent<Text>().text = Helpers.GetValueText(num5);
		transform.Find("ScreenContainer/DarkPanel/Detailed/Income/Transfers/SeasonAmount").GetComponent<Text>().text = Helpers.GetValueText(num6);
		transform.Find("ScreenContainer/DarkPanel/Detailed/Income/CashInjection/SeasonAmount").GetComponent<Text>().text = Helpers.GetValueText(num7);
		transform.Find("ScreenContainer/DarkPanel/Detailed/Income/PrizeMoney/SeasonAmount").GetComponent<Text>().text = Helpers.GetValueText(num8);
		transform.Find("ScreenContainer/DarkPanel/Detailed/Income/OtherIncome/SeasonAmount").GetComponent<Text>().text = Helpers.GetValueText(num9);
		seasonIncome = num + num2 + num3 + num4 + num5 + num6 + num7 + num8 + num9;
		transform.Find("ScreenContainer/DarkPanel/Detailed/Income/TotalRow/ContentTextWhite (2)").GetComponent<Text>().text = Helpers.GetValueText(seasonIncome);
	}

	private void DrawOutgoings()
	{
		transform.Find("ScreenContainer/DarkPanel/Detailed/Expenditure/Header/Text").GetComponent<Text>().text = Translation.GetText("expenditure").ToUpper();
		transform.Find("ScreenContainer/DarkPanel/Detailed/Expenditure/Header/Text (1)").GetComponent<Text>().text = Translation.GetText("weekly").ToUpper();
		transform.Find("ScreenContainer/DarkPanel/Detailed/Expenditure/Header/Text (2)").GetComponent<Text>().text = Translation.GetText("gn-Season").ToUpper();
		transform.Find("ScreenContainer/DarkPanel/Detailed/Expenditure/PlayerWages/Title").GetComponent<Text>().text = Translation.GetText("player-wages");
		transform.Find("ScreenContainer/DarkPanel/Detailed/Expenditure/StaffWages/Title").GetComponent<Text>().text = Translation.GetText("staff-wages");
		transform.Find("ScreenContainer/DarkPanel/Detailed/Expenditure/GroundMaintenance/Title").GetComponent<Text>().text = Translation.GetText("ground-maintenance");
		transform.Find("ScreenContainer/DarkPanel/Detailed/Expenditure/TrainingGround/Title").GetComponent<Text>().text = Translation.GetText("training-ground");
		transform.Find("ScreenContainer/DarkPanel/Detailed/Expenditure/YouthAcademy/Title").GetComponent<Text>().text = Translation.GetText("facilities-name-youth-academy");
		transform.Find("ScreenContainer/DarkPanel/Detailed/Expenditure/Transfers/Title").GetComponent<Text>().text = Translation.GetText("transfer-money-out");
		transform.Find("ScreenContainer/DarkPanel/Detailed/Expenditure/AdminCosts/Title").GetComponent<Text>().text = Translation.GetText("administration");
		transform.Find("ScreenContainer/DarkPanel/Detailed/Expenditure/Overheads/Title").GetComponent<Text>().text = Translation.GetText("overheads");
		transform.Find("ScreenContainer/DarkPanel/Detailed/Expenditure/OtherOutgoings/Title").GetComponent<Text>().text = Translation.GetText("other-outgoings");
		transform.Find("ScreenContainer/DarkPanel/Detailed/Expenditure/TotalRow/ContentTextWhite").GetComponent<Text>().text = Translation.GetText("total-expenditure");
		double num = 0.0;
		double num2 = 0.0;
		double num3 = 0.0;
		double num4 = 0.0;
		double num5 = 0.0;
		double num6 = 0.0;
		double num7 = 0.0;
		double num8 = 0.0;
		double num9 = 0.0;
		for (int i = 0; i < Game.instance.managedClub[Game.instance.g_currUser].BalanceSheet.Count; i++)
		{
			num += Game.instance.managedClub[Game.instance.g_currUser].BalanceSheet[i].PlayerWages;
			num2 += Game.instance.managedClub[Game.instance.g_currUser].BalanceSheet[i].StaffWages;
			num3 += Game.instance.managedClub[Game.instance.g_currUser].BalanceSheet[i].GroundMaintenance;
			num4 += Game.instance.managedClub[Game.instance.g_currUser].BalanceSheet[i].TrainingGround;
			num5 += Game.instance.managedClub[Game.instance.g_currUser].BalanceSheet[i].YouthAcademy;
			num6 += Game.instance.managedClub[Game.instance.g_currUser].BalanceSheet[i].TransfersOut;
			num7 += Game.instance.managedClub[Game.instance.g_currUser].BalanceSheet[i].Administration;
			num8 += Game.instance.managedClub[Game.instance.g_currUser].BalanceSheet[i].Overheads;
			num9 += Game.instance.managedClub[Game.instance.g_currUser].BalanceSheet[i].OtherOutgoings;
		}
		BalanceSheet balanceSheet = Game.instance.managedClub[Game.instance.g_currUser].BalanceSheet[Game.instance.managedClub[Game.instance.g_currUser].CurrentBalanceSheet];
		transform.Find("ScreenContainer/DarkPanel/Detailed/Expenditure/PlayerWages/WeeklyAmount").GetComponent<Text>().text = Helpers.GetValueText(balanceSheet.PlayerWages);
		transform.Find("ScreenContainer/DarkPanel/Detailed/Expenditure/StaffWages/WeeklyAmount").GetComponent<Text>().text = Helpers.GetValueText(balanceSheet.StaffWages);
		transform.Find("ScreenContainer/DarkPanel/Detailed/Expenditure/GroundMaintenance/WeeklyAmount").GetComponent<Text>().text = Helpers.GetValueText(balanceSheet.GroundMaintenance);
		transform.Find("ScreenContainer/DarkPanel/Detailed/Expenditure/TrainingGround/WeeklyAmount").GetComponent<Text>().text = Helpers.GetValueText(balanceSheet.TrainingGround);
		transform.Find("ScreenContainer/DarkPanel/Detailed/Expenditure/YouthAcademy/WeeklyAmount").GetComponent<Text>().text = Helpers.GetValueText(balanceSheet.YouthAcademy);
		transform.Find("ScreenContainer/DarkPanel/Detailed/Expenditure/Transfers/WeeklyAmount").GetComponent<Text>().text = Helpers.GetValueText(balanceSheet.TransfersOut);
		transform.Find("ScreenContainer/DarkPanel/Detailed/Expenditure/AdminCosts/WeeklyAmount").GetComponent<Text>().text = Helpers.GetValueText(balanceSheet.Administration);
		transform.Find("ScreenContainer/DarkPanel/Detailed/Expenditure/Overheads/WeeklyAmount").GetComponent<Text>().text = Helpers.GetValueText(balanceSheet.Overheads);
		transform.Find("ScreenContainer/DarkPanel/Detailed/Expenditure/OtherOutgoings/WeeklyAmount").GetComponent<Text>().text = Helpers.GetValueText(balanceSheet.OtherOutgoings);
		weeklyOutgoings = balanceSheet.PlayerWages + balanceSheet.StaffWages + balanceSheet.GroundMaintenance + balanceSheet.TrainingGround + balanceSheet.YouthAcademy + balanceSheet.TransfersOut + balanceSheet.Administration + balanceSheet.Overheads + balanceSheet.OtherOutgoings;
		transform.Find("ScreenContainer/DarkPanel/Detailed/Expenditure/TotalRow/ContentTextWhite (1)").GetComponent<Text>().text = Helpers.GetValueText(weeklyOutgoings);
		transform.Find("ScreenContainer/DarkPanel/Detailed/Expenditure/PlayerWages/SeasonAmount").GetComponent<Text>().text = Helpers.GetValueText(num);
		transform.Find("ScreenContainer/DarkPanel/Detailed/Expenditure/StaffWages/SeasonAmount").GetComponent<Text>().text = Helpers.GetValueText(num2);
		transform.Find("ScreenContainer/DarkPanel/Detailed/Expenditure/GroundMaintenance/SeasonAmount").GetComponent<Text>().text = Helpers.GetValueText(num3);
		transform.Find("ScreenContainer/DarkPanel/Detailed/Expenditure/TrainingGround/SeasonAmount").GetComponent<Text>().text = Helpers.GetValueText(num4);
		transform.Find("ScreenContainer/DarkPanel/Detailed/Expenditure/YouthAcademy/SeasonAmount").GetComponent<Text>().text = Helpers.GetValueText(num5);
		transform.Find("ScreenContainer/DarkPanel/Detailed/Expenditure/Transfers/SeasonAmount").GetComponent<Text>().text = Helpers.GetValueText(num6);
		transform.Find("ScreenContainer/DarkPanel/Detailed/Expenditure/AdminCosts/SeasonAmount").GetComponent<Text>().text = Helpers.GetValueText(num7);
		transform.Find("ScreenContainer/DarkPanel/Detailed/Expenditure/Overheads/SeasonAmount").GetComponent<Text>().text = Helpers.GetValueText(num8);
		transform.Find("ScreenContainer/DarkPanel/Detailed/Expenditure/OtherOutgoings/SeasonAmount").GetComponent<Text>().text = Helpers.GetValueText(num9);
		seasonOutgoings = num + num2 + num3 + num4 + num5 + num6 + num7 + num8 + num9;
		transform.Find("ScreenContainer/DarkPanel/Detailed/Expenditure/TotalRow/ContentTextWhite (2)").GetComponent<Text>().text = Helpers.GetValueText(seasonOutgoings);
	}

	private void DrawSummary()
	{
		transform.Find("ScreenContainer/DarkPanel/Summary/Summary/Header (1)/Text").GetComponent<Text>().text = Translation.GetText("summary").ToUpper();
		transform.Find("ScreenContainer/DarkPanel/Summary/Summary/Header (1)/Text (1)").GetComponent<Text>().text = Translation.GetText("weekly").ToUpper();
		transform.Find("ScreenContainer/DarkPanel/Summary/Summary/Header (1)/Text (2)").GetComponent<Text>().text = Translation.GetText("gn-Season").ToUpper();
		transform.Find("ScreenContainer/DarkPanel/Summary/Summary/TopRow/Text (3)").GetComponent<Text>().text = Translation.GetText("total");
		transform.Find("ScreenContainer/DarkPanel/Summary/Income/Header/Text").GetComponent<Text>().text = Translation.GetText("income").ToUpper();
		transform.Find("ScreenContainer/DarkPanel/Summary/Income/Header/Text (1)").GetComponent<Text>().text = Translation.GetText("weekly").ToUpper();
		transform.Find("ScreenContainer/DarkPanel/Summary/Income/Header/Text (2)").GetComponent<Text>().text = Translation.GetText("gn-Season").ToUpper();
		transform.Find("ScreenContainer/DarkPanel/Summary/Income/TotalRow/ContentTextWhite").GetComponent<Text>().text = Translation.GetText("total-income");
		transform.Find("ScreenContainer/DarkPanel/Summary/Expenditure/Header/Text").GetComponent<Text>().text = Translation.GetText("expenditure").ToUpper();
		transform.Find("ScreenContainer/DarkPanel/Summary/Expenditure/Header/Text (1)").GetComponent<Text>().text = Translation.GetText("weekly").ToUpper();
		transform.Find("ScreenContainer/DarkPanel/Summary/Expenditure/Header/Text (2)").GetComponent<Text>().text = Translation.GetText("gn-Season").ToUpper();
		transform.Find("ScreenContainer/DarkPanel/Summary/Expenditure/TotalRow/ContentTextWhite").GetComponent<Text>().text = Translation.GetText("total-expenditure");
		transform.Find("ScreenContainer/DarkPanel/Summary/Income/TotalRow/ContentTextWhite (1)").GetComponent<Text>().text = Helpers.GetValueText(weeklyIncome);
		transform.Find("ScreenContainer/DarkPanel/Summary/Income/TotalRow/ContentTextWhite (2)").GetComponent<Text>().text = Helpers.GetValueText(seasonIncome);
		transform.Find("ScreenContainer/DarkPanel/Summary/Expenditure/TotalRow/ContentTextWhite (1)").GetComponent<Text>().text = Helpers.GetValueText(weeklyOutgoings);
		transform.Find("ScreenContainer/DarkPanel/Summary/Expenditure/TotalRow/ContentTextWhite (2)").GetComponent<Text>().text = Helpers.GetValueText(seasonOutgoings);
		transform.Find("ScreenContainer/DarkPanel/Summary/Summary/TopRow/WeeklyAmount").GetComponent<Text>().text = Helpers.GetValueText(weeklyIncome - weeklyOutgoings);
		transform.Find("ScreenContainer/DarkPanel/Summary/Summary/TopRow/SeasonAmount").GetComponent<Text>().text = Helpers.GetValueText(seasonIncome - seasonOutgoings);
		transform.Find("ScreenContainer/DarkPanel/Summary/Summary/TopRow/WeeklyAmount").GetComponent<Text>().color = ((weeklyIncome - weeklyOutgoings > 0.0) ? Helpers.HexToColour("00FF2188") : Helpers.HexToColour("FF0000FF"));
		transform.Find("ScreenContainer/DarkPanel/Summary/Summary/TopRow/SeasonAmount").GetComponent<Text>().color = ((seasonIncome - seasonOutgoings > 0.0) ? Helpers.HexToColour("00FF2188") : Helpers.HexToColour("FF0000FF"));
	}

	private void DrawSponsor()
	{
		GameObject gameObject = transform.Find("ScreenContainer/DarkPanel/ClubZone/ClubZoneSponsor").gameObject;
		GameObject gameObject2 = transform.Find("ScreenContainer/DarkPanel/ClubZone/ClubZoneSponsornew").gameObject;
		gameObject2.SetActive(value: false);
		gameObject.SetActive(value: false);
		if (ClubZoneReportScreen.ShouldGenerateSponsors())
		{
			ClubZoneReportScreen.GenerateSponsors();
		}
		if (Game.instance.managedClub[Game.instance.g_currUser].CurrentSponsor.ID == 0)
		{
			gameObject2.SetActive(value: true);
			gameObject2.transform.Find("SignSponsor/SignSponsorText").GetComponent<Text>().text = Translation.GetText("sponsor").ToUpper();
			gameObject2.transform.Find("SignSponsor/ButtonPrimary/ButtonText").GetComponent<Text>().text = Translation.GetText("choose-sponsor").ToUpper();
			gameObject2.transform.Find("SignSponsor/ButtonPrimary").GetComponent<Button>().onClick.RemoveAllListeners();
			gameObject2.transform.Find("SignSponsor/ButtonPrimary").GetComponent<Button>().onClick.AddListener(() =>
			{
				UI.GetInstance().ChangeScreen(75);
			});
		}
		else
		{
			gameObject.SetActive(value: true);
			Sponsor currentSponsor = Game.instance.managedClub[Game.instance.g_currUser].CurrentSponsor;
			gameObject.transform.Find("Header/Text").GetComponent<Text>().text = Translation.GetText("sponsor").ToUpper();
			gameObject.transform.Find("TextPanel/ButtonPrimary").gameObject.SetActive(value: false);
			Sprite sprite = Resources.Load<Sprite>(SponsorData.GetLogoFileName(currentSponsor.ID));
			gameObject.transform.Find("TextPanel/Sponsor").GetComponent<Image>().sprite = sprite;
			gameObject.transform.Find("TextPanel/LevelBonus").GetComponent<Text>().text = currentSponsor.Seasons + " " + Translation.GetText("gn-Season");
			gameObject.transform.Find("TextPanel/Total").GetComponent<Text>().text = Helpers.GetValueText(currentSponsor.GetPerSeasonAmount()) + " " + Translation.GetText("per-season");
			if (currentSponsor.PaymentMethod != Sponsor.PaymentType.ANNUAL)
			{
				Text component = gameObject.transform.Find("TextPanel/Total").GetComponent<Text>();
				component.text = component.text + " (" + ClubZoneReportScreen.GetSponsorAmountPerTime(currentSponsor) + ")";
			}
		}
		transform.Find("ScreenContainer/DarkPanel/ClubZone/ClubZoneSponsor/Header/Text").GetComponent<Text>().text = Translation.GetText("sponsor").ToUpper();
		transform.Find("ScreenContainer/DarkPanel/ClubZone/Clubzone/FacilityBonuses/TopRow/TvRevenue/ContentHeader/Text").GetComponent<Text>().text = Translation.GetText("tv-revenue").ToUpper();
		transform.Find("ScreenContainer/DarkPanel/ClubZone/Clubzone/FacilityBonuses/TopRow/Matchday/ContentHeader/Text").GetComponent<Text>().text = Translation.GetText("faq-9-t").ToUpper();
		transform.Find("ScreenContainer/DarkPanel/ClubZone/Clubzone/FacilityBonuses/BottomRow/Merchandise/ContentHeader/Text").GetComponent<Text>().text = Translation.GetText("merchandise").ToUpper();
		transform.Find("ScreenContainer/DarkPanel/ClubZone/Clubzone/FacilityBonuses/BottomRow/Hospitaity/ContentHeader/Text").GetComponent<Text>().text = Translation.GetText("hospitality").ToUpper();
		transform.Find("ScreenContainer/DarkPanel/ClubZone/Clubzone/FacilityBonuses/TotalRow/ContentTextWhite").GetComponent<Text>().text = Translation.GetText("total-income").ToUpper();
	}

	private void DrawClubZone()
	{
		int num = Economy.CalcClubAttendance(Game.GetCurrentUserClubID(), random: false);
		double num2 = Economy.CalcClubTicketPricePrecise(Game.GetCurrentUserClubID()) * (float)num * 20f;
		double num3 = Economy.CalcWeeklyTvMoney(Game.GetCurrentUserClubID()) * 52;
		double num4 = Economy.GetMerchEstimate(Game.GetCurrentUserClubID()) * 52;
		double num5 = Economy.GetHospitalityEstimate(Game.GetCurrentUserClubID()) * 52;
		double value = num2 - (double)(Economy.CalcClubTicketPricePrecise(Game.GetCurrentUserClubID(), facilityIncrease: false) * (float)num * 20f);
		double value2 = num3 - (double)(Economy.CalcWeeklyTvMoney(Game.GetCurrentUserClubID(), facilityIncrease: false) * 52);
		double value3 = num4 - (double)(Economy.GetMerchEstimate(Game.GetCurrentUserClubID(), facilityIncrease: false) * 52);
		double value4 = num5 - (double)(Economy.GetHospitalityEstimate(Game.GetCurrentUserClubID(), facilityIncrease: false) * 52);
		double value5 = num2 + num3 + num4 + num5;
		FacilitiesTechTree.FillLevel(transform.Find("ScreenContainer/DarkPanel/ClubZone/Clubzone/FacilityBonuses/TopRow/Matchday/Stars").gameObject, Facilities.GetUpgradeLevel(28));
		FacilitiesTechTree.FillLevel(transform.Find("ScreenContainer/DarkPanel/ClubZone/Clubzone/FacilityBonuses/TopRow/TvRevenue/Stars").gameObject, Facilities.GetUpgradeLevel(29));
		FacilitiesTechTree.FillLevel(transform.Find("ScreenContainer/DarkPanel/ClubZone/Clubzone/FacilityBonuses/BottomRow/Hospitaity/Stars").gameObject, Facilities.GetUpgradeLevel(32));
		FacilitiesTechTree.FillLevel(transform.Find("ScreenContainer/DarkPanel/ClubZone/Clubzone/FacilityBonuses/BottomRow/Merchandise/Stars").gameObject, Facilities.GetUpgradeLevel(30));
		transform.Find("ScreenContainer/DarkPanel/ClubZone/Clubzone/FacilityBonuses/TopRow/Matchday/TextPanel/Item1").gameObject.SetActive(value: false);
		transform.Find("ScreenContainer/DarkPanel/ClubZone/Clubzone/FacilityBonuses/TopRow/Matchday/TextPanel/Item2/Title").GetComponent<Text>().text = Translation.GetText("bonus") + " " + Translation.GetText("per-season").ToLower();
		transform.Find("ScreenContainer/DarkPanel/ClubZone/Clubzone/FacilityBonuses/TopRow/Matchday/TextPanel/Item2/Value").GetComponent<Text>().text = Helpers.GetValueText(value);
		transform.Find("ScreenContainer/DarkPanel/ClubZone/Clubzone/FacilityBonuses/TopRow/Matchday/TextPanel/Item3").gameObject.SetActive(value: false);
		transform.Find("ScreenContainer/DarkPanel/ClubZone/Clubzone/FacilityBonuses/TopRow/Matchday/TextPanel/Item4/Title").GetComponent<Text>().text = Translation.GetText("total") + " " + Translation.GetText("per-season").ToLower();
		transform.Find("ScreenContainer/DarkPanel/ClubZone/Clubzone/FacilityBonuses/TopRow/Matchday/TextPanel/Item4/Value").GetComponent<Text>().text = Helpers.GetValueText(num2);
		transform.Find("ScreenContainer/DarkPanel/ClubZone/Clubzone/FacilityBonuses/TopRow/TvRevenue/TextPanel/Item1").gameObject.SetActive(value: false);
		transform.Find("ScreenContainer/DarkPanel/ClubZone/Clubzone/FacilityBonuses/TopRow/TvRevenue/TextPanel/Item2/Title").GetComponent<Text>().text = Translation.GetText("bonus") + " " + Translation.GetText("per-season").ToLower();
		transform.Find("ScreenContainer/DarkPanel/ClubZone/Clubzone/FacilityBonuses/TopRow/TvRevenue/TextPanel/Item2/Value").GetComponent<Text>().text = Helpers.GetValueText(value2);
		transform.Find("ScreenContainer/DarkPanel/ClubZone/Clubzone/FacilityBonuses/TopRow/TvRevenue/TextPanel/Item3").gameObject.SetActive(value: false);
		transform.Find("ScreenContainer/DarkPanel/ClubZone/Clubzone/FacilityBonuses/TopRow/TvRevenue/TextPanel/Item4/Title").GetComponent<Text>().text = Translation.GetText("total") + " " + Translation.GetText("per-season").ToLower();
		transform.Find("ScreenContainer/DarkPanel/ClubZone/Clubzone/FacilityBonuses/TopRow/TvRevenue/TextPanel/Item4/Value").GetComponent<Text>().text = Helpers.GetValueText(num3);
		transform.Find("ScreenContainer/DarkPanel/ClubZone/Clubzone/FacilityBonuses/BottomRow/Hospitaity/TextPanel/Item1").gameObject.SetActive(value: false);
		transform.Find("ScreenContainer/DarkPanel/ClubZone/Clubzone/FacilityBonuses/BottomRow/Hospitaity/TextPanel/Item2/Title").GetComponent<Text>().text = Translation.GetText("bonus") + " " + Translation.GetText("per-season").ToLower();
		transform.Find("ScreenContainer/DarkPanel/ClubZone/Clubzone/FacilityBonuses/BottomRow/Hospitaity/TextPanel/Item2/Value").GetComponent<Text>().text = Helpers.GetValueText(value4);
		transform.Find("ScreenContainer/DarkPanel/ClubZone/Clubzone/FacilityBonuses/BottomRow/Hospitaity/TextPanel/Item3").gameObject.SetActive(value: false);
		transform.Find("ScreenContainer/DarkPanel/ClubZone/Clubzone/FacilityBonuses/BottomRow/Hospitaity/TextPanel/Item4/Title").GetComponent<Text>().text = Translation.GetText("total") + " " + Translation.GetText("per-season").ToLower();
		transform.Find("ScreenContainer/DarkPanel/ClubZone/Clubzone/FacilityBonuses/BottomRow/Hospitaity/TextPanel/Item4/Value").GetComponent<Text>().text = Helpers.GetValueText(num5);
		transform.Find("ScreenContainer/DarkPanel/ClubZone/Clubzone/FacilityBonuses/BottomRow/Merchandise/TextPanel/Item1").gameObject.SetActive(value: false);
		transform.Find("ScreenContainer/DarkPanel/ClubZone/Clubzone/FacilityBonuses/BottomRow/Merchandise/TextPanel/Item2/Title").GetComponent<Text>().text = Translation.GetText("bonus") + " " + Translation.GetText("per-season").ToLower();
		transform.Find("ScreenContainer/DarkPanel/ClubZone/Clubzone/FacilityBonuses/BottomRow/Merchandise/TextPanel/Item2/Value").GetComponent<Text>().text = Helpers.GetValueText(value3);
		transform.Find("ScreenContainer/DarkPanel/ClubZone/Clubzone/FacilityBonuses/BottomRow/Merchandise/TextPanel/Item3").gameObject.SetActive(value: false);
		transform.Find("ScreenContainer/DarkPanel/ClubZone/Clubzone/FacilityBonuses/BottomRow/Merchandise/TextPanel/Item4/Title").GetComponent<Text>().text = Translation.GetText("total") + " " + Translation.GetText("per-season").ToLower();
		transform.Find("ScreenContainer/DarkPanel/ClubZone/Clubzone/FacilityBonuses/BottomRow/Merchandise/TextPanel/Item4/Value").GetComponent<Text>().text = Helpers.GetValueText(num4);
		transform.Find("ScreenContainer/DarkPanel/ClubZone/Clubzone/FacilityBonuses/TotalRow/ContentTextWhite (2)").GetComponent<Text>().text = Helpers.GetValueText(value5);
	}

	private void DrawPrizeMoney()
	{
		drawnComps = new List<int>();
		DrawLeagueBox(transform.Find("ScreenContainer/DarkPanel/PrizeMoney/Container/FacilityBonuses/TopRow/League").gameObject);
		DrawContCupBox(transform.Find("ScreenContainer/DarkPanel/PrizeMoney/Container/FacilityBonuses/TopRow/Europe").gameObject);
		DrawDomesticCupBox(transform.Find("ScreenContainer/DarkPanel/PrizeMoney/Container/FacilityBonuses/MiddleRow/Cup1").gameObject);
		DrawDomesticCupBox(transform.Find("ScreenContainer/DarkPanel/PrizeMoney/Container/FacilityBonuses/MiddleRow/Cup2").gameObject);
		DrawWorldCupBox(transform.Find("ScreenContainer/DarkPanel/PrizeMoney/Container/FacilityBonuses/BottomRow/Cup3").gameObject);
		DrawSuperCupBox(transform.Find("ScreenContainer/DarkPanel/PrizeMoney/Container/FacilityBonuses/BottomRow/Cup4").gameObject);
	}

	private void DrawLeagueBox(GameObject box)
	{
		int leagueDataId = Club.GetLeagueDataId(Game.GetCurrentUserClubID());
		int leaguePositionOfClub = Helpers.GetLeagueFromSeason(leagueDataId).GetLeaguePositionOfClub(Game.GetCurrentUserClubID());
		int num = Economy.AwardPrizeMoney4League(1, awardMoney: false);
		int num2 = Economy.AwardPrizeMoney4League(leaguePositionOfClub, awardMoney: false);
		box.transform.Find("ContentHeader/Text").GetComponent<Text>().text = LeagueData.GetName(leagueDataId).ToUpper();
		box.transform.Find("WinningTotal/Total").GetComponent<Text>().text = Helpers.GetValueText(num);
		box.transform.Find("CurrentPosition/Value").GetComponent<Text>().text = leaguePositionOfClub.ToString();
		box.transform.Find("PrizeMoneyPanel/Value").GetComponent<Text>().text = Helpers.GetValueText(num2);
		box.transform.Find("CurrentPosition/Title").GetComponent<Text>().text = Translation.GetText("current-position");
		box.transform.Find("PrizeMoneyPanel/Title").GetComponent<Text>().text = Translation.GetText("expected-prize");
		box.transform.Find("WinningTotal/LevelBonus").GetComponent<Text>().text = Translation.GetText("CompetitionWinners");
		box.transform.Find("TrophyImage").GetComponent<Image>().sprite = TrophyImageController.GetInstance().GetLeagueTrophyImage(leagueDataId);
	}

	private void DrawContCupBox(GameObject box)
	{
		int num = -1;
		foreach (ContinentalComp value in Game.instance.g_continentalCompetitions.Values)
		{
			if (!value.clubs.Contains(Game.GetCurrentUserClubID()) || value.Status == ContinentalCompStatus.NotInitialised || value.competitionid == 7 || value.competitionid == 5)
			{
				continue;
			}
			num = value.competitionid;
			double totalContCompWinnings = Economy.GetTotalContCompWinnings(num);
			box.transform.Find("ContentHeader/Text").GetComponent<Text>().text = ContinentalComp.GetName(num).ToUpper();
			box.transform.Find("WinningTotal/Total").GetComponent<Text>().text = Helpers.GetValueText(totalContCompWinnings);
			box.transform.Find("CurrentPosition/Value").GetComponent<Text>().text = value.GetRoundName();
			box.transform.Find("CurrentPosition/Title").GetComponent<Text>().text = Translation.GetText("current-position");
			box.transform.Find("PrizeMoneyPanel/Title").GetComponent<Text>().text = Translation.GetText("prize-money");
			box.transform.Find("WinningTotal/LevelBonus").GetComponent<Text>().text = Translation.GetText("CompetitionWinners");
			if (Game.instance.CurrentManagedClub.ContinentalCupPrizeMoney.ContainsKey(num))
			{
				box.transform.Find("PrizeMoneyPanel/Value").GetComponent<Text>().text = Helpers.GetValueText(Game.instance.CurrentManagedClub.ContinentalCupPrizeMoney[num]);
				if (totalContCompWinnings < (double)Game.instance.CurrentManagedClub.ContinentalCupPrizeMoney[num])
				{
					box.transform.Find("WinningTotal/Total").GetComponent<Text>().text = Helpers.GetValueText(Game.instance.CurrentManagedClub.ContinentalCupPrizeMoney[num]);
				}
			}
			else
			{
				box.transform.Find("PrizeMoneyPanel/Value").GetComponent<Text>().text = Helpers.GetValueText(0.0);
			}
			box.transform.Find("TrophyImage").GetComponent<Image>().sprite = TrophyImageController.GetInstance().GetContiTrophyImage(num);
			box.SetActive(value: true);
			break;
		}
		if (num == -1)
		{
			box.SetActive(value: false);
		}
	}

	private void DrawDomesticCupBox(GameObject box)
	{
		int num = -1;
		foreach (Cup cup in Game.instance.g_season.Cups)
		{
			if (cup.AssocID != Club.GetAssociationID(Game.GetCurrentUserClubID()))
			{
				continue;
			}
			if (Cup.IsClubInCup(cup.CupDataID, Game.GetCurrentUserClubID()) && !drawnComps.Contains(cup.CupDataID))
			{
				num = cup.CupDataID;
				drawnComps.Add(num);
				box.transform.Find("ContentHeader/Text").GetComponent<Text>().text = CupData.GetName(num).ToUpper();
				double totalWinningsForCup = Economy.GetTotalWinningsForCup(cup);
				box.transform.Find("WinningTotal/Total").GetComponent<Text>().text = Helpers.GetValueText(totalWinningsForCup);
				box.transform.Find("CurrentPosition/Value").GetComponent<Text>().text = cup.GetRoundName(cup.CurrentRound);
				box.transform.Find("CurrentPosition/Title").GetComponent<Text>().text = Translation.GetText("current-position");
				box.transform.Find("PrizeMoneyPanel/Title").GetComponent<Text>().text = Translation.GetText("prize-money");
				box.transform.Find("WinningTotal/LevelBonus").GetComponent<Text>().text = Translation.GetText("CompetitionWinners");
				if (Game.instance.CurrentManagedClub.DomesticCupPrizeMoney.ContainsKey(cup.CupDataID))
				{
					box.transform.Find("PrizeMoneyPanel/Value").GetComponent<Text>().text = Helpers.GetValueText(Game.instance.CurrentManagedClub.DomesticCupPrizeMoney[cup.CupDataID]);
					if ((double)Game.instance.CurrentManagedClub.DomesticCupPrizeMoney[cup.CupDataID] > totalWinningsForCup)
					{
						box.transform.Find("WinningTotal/Total").GetComponent<Text>().text = Helpers.GetValueText(Game.instance.CurrentManagedClub.DomesticCupPrizeMoney[cup.CupDataID]);
					}
				}
				else
				{
					box.transform.Find("PrizeMoneyPanel/Value").GetComponent<Text>().text = Helpers.GetValueText(0.0);
				}
				box.transform.Find("TrophyImage").GetComponent<Image>().sprite = TrophyImageController.GetInstance().GetCupTrophyImage(cup);
				box.SetActive(value: true);
				break;
			}
			box.SetActive(value: false);
		}
	}

	private void DrawSuperCupBox(GameObject box)
	{
		int num = -1;
		foreach (ContinentalComp value in Game.instance.g_continentalCompetitions.Values)
		{
			if (!value.cup.InCup.Contains(Game.GetCurrentUserClubID()) || value.Status == ContinentalCompStatus.NotInitialised || value.competitionid != 5)
			{
				continue;
			}
			num = value.competitionid;
			double totalContCompWinnings = Economy.GetTotalContCompWinnings(num);
			box.transform.Find("ContentHeader/Text").GetComponent<Text>().text = ContinentalComp.GetName(num).ToUpper();
			box.transform.Find("WinningTotal/Total").GetComponent<Text>().text = Helpers.GetValueText(totalContCompWinnings);
			box.transform.Find("CurrentPosition/Value").GetComponent<Text>().text = value.GetRoundName();
			box.transform.Find("CurrentPosition/Title").GetComponent<Text>().text = Translation.GetText("current-position");
			box.transform.Find("PrizeMoneyPanel/Title").GetComponent<Text>().text = Translation.GetText("prize-money");
			box.transform.Find("WinningTotal/LevelBonus").GetComponent<Text>().text = Translation.GetText("CompetitionWinners");
			if (Game.instance.CurrentManagedClub.ContinentalCupPrizeMoney.ContainsKey(num))
			{
				box.transform.Find("PrizeMoneyPanel/Value").GetComponent<Text>().text = Helpers.GetValueText(Game.instance.CurrentManagedClub.ContinentalCupPrizeMoney[5]);
				if ((double)Game.instance.CurrentManagedClub.ContinentalCupPrizeMoney[5] > totalContCompWinnings)
				{
					box.transform.Find("WinningTotal/Total").GetComponent<Text>().text = Helpers.GetValueText(Game.instance.CurrentManagedClub.ContinentalCupPrizeMoney[5]);
				}
			}
			else
			{
				box.transform.Find("PrizeMoneyPanel/Value").GetComponent<Text>().text = Helpers.GetValueText(0.0);
			}
			box.transform.Find("TrophyImage").GetComponent<Image>().sprite = TrophyImageController.GetInstance().GetContiTrophyImage(num);
			box.SetActive(value: true);
			break;
		}
		if (num == -1)
		{
			box.SetActive(value: false);
		}
	}

	private void DrawWorldCupBox(GameObject box)
	{
		int num = -1;
		foreach (ContinentalComp value in Game.instance.g_continentalCompetitions.Values)
		{
			if (!value.cup.InCup.Contains(Game.GetCurrentUserClubID()) || value.Status == ContinentalCompStatus.NotInitialised || value.competitionid != 7)
			{
				continue;
			}
			num = value.competitionid;
			double totalContCompWinnings = Economy.GetTotalContCompWinnings(num);
			box.transform.Find("ContentHeader/Text").GetComponent<Text>().text = ContinentalComp.GetName(num).ToUpper();
			box.transform.Find("WinningTotal/Total").GetComponent<Text>().text = Helpers.GetValueText(totalContCompWinnings);
			box.transform.Find("CurrentPosition/Value").GetComponent<Text>().text = value.GetRoundName();
			box.transform.Find("CurrentPosition/Title").GetComponent<Text>().text = Translation.GetText("current-position");
			box.transform.Find("PrizeMoneyPanel/Title").GetComponent<Text>().text = Translation.GetText("prize-money");
			box.transform.Find("WinningTotal/LevelBonus").GetComponent<Text>().text = Translation.GetText("CompetitionWinners");
			if (Game.instance.CurrentManagedClub.ContinentalCupPrizeMoney.ContainsKey(num))
			{
				box.transform.Find("PrizeMoneyPanel/Value").GetComponent<Text>().text = Helpers.GetValueText(Game.instance.CurrentManagedClub.ContinentalCupPrizeMoney[7]);
				if (totalContCompWinnings > (double)Game.instance.CurrentManagedClub.ContinentalCupPrizeMoney[7])
				{
					box.transform.Find("WinningTotal/Total").GetComponent<Text>().text = Helpers.GetValueText(Game.instance.CurrentManagedClub.ContinentalCupPrizeMoney[7]);
				}
			}
			else
			{
				box.transform.Find("PrizeMoneyPanel/Value").GetComponent<Text>().text = Helpers.GetValueText(0.0);
			}
			box.transform.Find("TrophyImage").GetComponent<Image>().sprite = TrophyImageController.GetInstance().GetContiTrophyImage(num);
			box.SetActive(value: true);
			break;
		}
		if (num == -1)
		{
			box.SetActive(value: false);
		}
	}
}
