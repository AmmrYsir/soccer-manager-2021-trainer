using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class FinanceReports : MonoBehaviour
{
	public void DrawScreen()
	{
	}

	private void TranslateScreen()
	{
		transform.Find("ScrollView/Viewport/ScreenContainer/ContentContainerWhite/ContentHeader/Text").GetComponent<Text>().text = Translation.GetText("top-five-earners");
		transform.Find("ScrollView/Viewport/ScreenContainer/ContentContainerWhite/AdviceHeader/Text").GetComponent<Text>().text = Translation.GetText("assistants-advice");
		transform.Find("ScrollView/Viewport/ScreenContainer/ContentContainerWhite/TableContainer/SquadContainer/PlayerGeneralHeaders/Pos").GetComponent<Text>().text = Translation.GetText("Squad-Position");
		transform.Find("ScrollView/Viewport/ScreenContainer/ContentContainerWhite/TableContainer/SquadContainer/PlayerGeneralHeaders/Number").GetComponent<Text>().text = Translation.GetText("Squad-Number");
		transform.Find("ScrollView/Viewport/ScreenContainer/ContentContainerWhite/TableContainer/SquadContainer/PlayerGeneralHeaders/Nat").GetComponent<Text>().text = Translation.GetText("Squad-Nat");
		transform.Find("ScrollView/Viewport/ScreenContainer/ContentContainerWhite/TableContainer/SquadContainer/PlayerGeneralHeaders/Name").GetComponent<Text>().text = Translation.GetText("Squad-Name");
		transform.Find("ScrollView/Viewport/ScreenContainer/ContentContainerWhite/TableContainer/SquadContainer/PlayerGeneralHeaders/Info").GetComponent<Text>().text = Translation.GetText("Squad-Info");
		transform.Find("ScrollView/Viewport/ScreenContainer/ContentContainerWhite/TableContainer/SquadContainer/PlayerGeneralHeaders/Age").GetComponent<Text>().text = Translation.GetText("Squad-Age");
		transform.Find("ScrollView/Viewport/ScreenContainer/ContentContainerWhite/TableContainer/SquadContainer/PlayerGeneralHeaders/Wage").GetComponent<Text>().text = Translation.GetText("wages");
		transform.Find("ScrollView/Viewport/ScreenContainer/ContentContainerWhite/TableContainer/SquadContainer/PlayerGeneralHeaders/Value").GetComponent<Text>().text = Translation.GetText("Squad-Value");
		transform.Find("ScrollView/Viewport/ScreenContainer/ContentContainerWhite/TableContainer/SquadContainer/PlayerGeneralHeaders/Condition").GetComponent<Text>().text = Translation.GetText("Squad-Con");
		transform.Find("ScrollView/Viewport/ScreenContainer/ContentContainerWhite/TableContainer/SquadContainer/PlayerGeneralHeaders/PlayerMoral").GetComponent<Text>().text = Translation.GetText("Squad-Mor");
		transform.Find("ScrollView/Viewport/ScreenContainer/ContentContainerWhite/TableContainer/SquadContainer/PlayerGeneralHeaders/Rating").GetComponent<Text>().text = Translation.GetText("Squad-Rating");
	}

	private void DrawTopEarners()
	{
		List<int> top5Earners = Club.GetTop5Earners(Game.GetCurrentUserClubID());
		for (int i = 0; i < top5Earners.Count; i++)
		{
			GameObject gameObject = ((i == 0) ? transform.Find("ScrollView/Viewport/ScreenContainer/ContentContainerWhite/TableContainer/SquadContainer/SquadPanel/PlayerRow").gameObject : transform.Find("ScrollView/Viewport/ScreenContainer/ContentContainerWhite/TableContainer/SquadContainer/SquadPanel/PlayerRow (" + i + ")").gameObject);
			Color positionColour = Player.GetPositionColour(top5Earners[i]);
			gameObject.transform.Find("PlayerContent/POSImage").GetComponent<Image>().color = positionColour;
			gameObject.transform.Find("PlayerContent/PlayerPos").GetComponent<Text>().color = positionColour;
			gameObject.transform.Find("PlayerContent/PlayerNumber").GetComponent<Text>().text = ((PlayerData.GetSquadNo(top5Earners[i]) != 0) ? PlayerData.GetSquadNo(top5Earners[i]).ToString() : "");
			gameObject.transform.Find("PlayerContent/PlayerName").GetComponent<Text>().text = Player.GetName(top5Earners[i]);
			gameObject.transform.Find("PlayerContent/PlayerNation").GetComponent<Image>().sprite = CountryFlagImageController.GetInstance().GetFlagImage(CountryData.GetCode(PlayerData.GetCountryDataID(top5Earners[i])));
			Helpers.SetPlayerRatingAndColour(gameObject.transform.Find("PlayerContent/PlayerRating").gameObject, Player.GetDisplayRating(top5Earners[i]));
			gameObject.transform.Find("PlayerContent/PlayerAge").GetComponent<Text>().text = Player.GetAge(top5Earners[i]).ToString();
			gameObject.transform.Find("PlayerContent/MoraleInfoRowPanel/Donut/PlayerMoral").GetComponent<Image>().sprite = UI.GetInstance().GetMoralSprite(Player.GetMorale(top5Earners[i]));
			gameObject.transform.Find("PlayerContent/MoraleInfoRowPanel/Donut").GetComponent<Image>().fillAmount = (float)(int)Player.GetMorale(top5Earners[i]) / 10f;
			gameObject.transform.Find("PlayerContent/MoraleInfoRowPanel/Donut").GetComponent<Image>().color = Helpers.GetDonutFillColour(gameObject.transform.Find("PlayerContent/MoraleInfoRowPanel/Donut").GetComponent<Image>().fillAmount);
			float num = (int)Player.GetFitness(top5Earners[i]);
			gameObject.transform.Find("PlayerContent/PlayerCondition/progbar").GetComponent<ProgressBar>().maxAmount = 100f;
			gameObject.transform.Find("PlayerContent/PlayerCondition/progbar").GetComponent<ProgressBar>().targetAmount = num;
			gameObject.transform.Find("PlayerContent/PlayerCondition/progbar/Image").GetComponent<Image>().color = Game.instance.getColourForFitnessText(num);
			gameObject.transform.Find("PlayerContent/PlayerCondition/Value").GetComponent<Text>().text = num + "%";
			gameObject.transform.Find("PlayerContent/Chemistry/Text").GetComponent<Text>().text = ((Player.GetStatus(top5Earners[i]) != null) ? Player.GetTeamCohesion(top5Earners[i]).Total.ToString() : "-");
			gameObject.transform.Find("PlayerContent/PlayerWage").GetComponent<Text>().text = Helpers.GetValueText(Player.GetWages(top5Earners[i]));
			gameObject.transform.Find("PlayerContent/PlayerValue").GetComponent<Text>().text = Helpers.GetValueText((int)Player.GetValue(top5Earners[i]));
		}
	}

	private void DrawAdviceContainer()
	{
		List<int> advicePlayers = GetAdvicePlayers();
		for (int i = 0; i < advicePlayers.Count; i++)
		{
			GameObject gameObject = ((i == 0) ? transform.Find("ScrollView/Viewport/ScreenContainer/ContentContainerWhite/AdviceContainer/ContentTextGray").gameObject : transform.Find("ScrollView/Viewport/ScreenContainer/ContentContainerWhite/AdviceContainer/ContentTextGray (" + i + ")").gameObject);
			if (advicePlayers[i] == 0)
			{
				gameObject.SetActive(value: false);
			}
			else
			{
				gameObject.transform.GetComponent<Text>().text = "";
			}
		}
	}

	private List<int> GetAdvicePlayers()
	{
		Dictionary<int, Tuple<int, int>> dictionary = new Dictionary<int, Tuple<int, int>>();
		List<int> players = Club.GetPlayers(Game.GetCurrentUserClubID()).players;
		int num = 0;
		double num2 = 0.0;
		for (int i = 0; i < players.Count; i++)
		{
			int num3 = players[i];
			int stat = PlayerCareerStats.GetCurrentSeasonStats(num3).GetStat(StatType.MinsPlayed);
			if (stat != 0)
			{
				int wages = Player.GetWages(num3);
				num += stat;
				num2 += (double)wages;
				dictionary.Add(num3, new Tuple<int, int>(stat, wages));
			}
		}
		if (dictionary.Count != 0)
		{
			int num4 = num / dictionary.Count;
			_ = num2 / (double)dictionary.Count;
			Dictionary<int, int> dictionary2 = new Dictionary<int, int>();
			for (int j = 0; j < dictionary.Count; j++)
			{
				if (dictionary.ElementAt(j).Value.Item1 < num4)
				{
					dictionary2.Add(dictionary.ElementAt(j).Key, dictionary.ElementAt(j).Value.Item2);
				}
			}
			dictionary2.OrderBy((KeyValuePair<int, int> x) => x.Value).ToDictionary((KeyValuePair<int, int> pair) => pair.Key, (KeyValuePair<int, int> pair) => pair.Value);
			return dictionary2.Keys.Slice(0, 6).ToList();
		}
		return new List<int>();
	}
}
