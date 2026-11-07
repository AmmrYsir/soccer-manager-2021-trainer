using System.Linq;
using UnityEngine.SceneManagement;

public class CompetitionChallenge : MainChallenge
{
	public int Requirement;

	public int CompetitionID;

	public override int GetRequirement()
	{
		return Requirement;
	}

	public override void CheckChallengeProgress()
	{
		ChallengeSubType subType = SubType;
		if (subType != ChallengeSubType.WinLeague && subType == ChallengeSubType.Promotion)
		{
			CheckPromotionChallenge();
		}
	}

	public override void CompleteChallenge()
	{
		SceneManager.LoadScene("PostMatchEvent");
	}

	public override void FailedChallenge()
	{
		Helpers.ShowToast("FAILED CHALLENGE YA SHIT!");
	}

	private void CheckPromotionChallenge()
	{
		string currentSeasonID = Season.GetCurrentSeasonID();
		if (Season.CreateOrGetSeasonLog(AssociationID, currentSeasonID).Leagues.Any((PromotionLog i) => i.PromotedClubs.Contains(ClubID)))
		{
			CompleteChallenge();
		}
		else if (!HaveSeasonsLeft())
		{
			FailedChallenge();
		}
	}
}
