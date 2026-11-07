using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using UnityEngine;

public static class UserManagerProfileData
{
	public static List<ManagerTrait> StaticTraits;

	public static Dictionary<int, UserManagerLevel> StaticManagerLevels;

	public static UserManagerProfile UserManagerProfile;

	public const string SaveFileUserProfile = "sm21_User_Profile";

	public const int GamesNeedForCareerEntry = 10;

	public const int MaxManagerLevels = 50;

	public const int PointsAwardWinMatch = 25;

	public const int PointsAwardWinComp = 100;

	public static bool NeedNewProfile;

	public static void LoadUserManagerProfile()
	{
		string identifier = "sm21_User_Profile";
		if (ES2.Exists(identifier))
		{
			try
			{
				UserManagerProfile = ES2.Load<UserManagerProfile>(identifier);
				return;
			}
			catch
			{
				ES2.Delete(identifier);
				UserManagerProfile = new UserManagerProfile();
				NeedNewProfile = true;
				return;
			}
		}
		UserManagerProfile = new UserManagerProfile();
		NeedNewProfile = true;
	}

	public static void SaveUserManagerProfile()
	{
		string identifier = "sm21_User_Profile";
		ES2.Save(UserManagerProfile, identifier);
	}

	public static void PopulateStaticManagerTraits()
	{
		StaticTraits = JsonConvert.DeserializeObject<ManagerTraitList>(((TextAsset)Resources.Load("ManagerTraitsJson", typeof(TextAsset))).text, new JsonSerializerSettings
		{
			TypeNameHandling = TypeNameHandling.Objects
		}).ManagerTraits.OrderByDescending((ManagerTrait i) => i.BaseTrait).ToList();
	}

	public static void PopulateUserManagerLevels()
	{
		StaticManagerLevels = JsonConvert.DeserializeObject<UserManagerLevels>(((TextAsset)Resources.Load("UserManagerLevelsJson", typeof(TextAsset))).text, new JsonSerializerSettings
		{
			TypeNameHandling = TypeNameHandling.Objects
		}).Levels;
	}
}
