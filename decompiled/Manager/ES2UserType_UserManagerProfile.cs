using System;

public class ES2UserType_UserManagerProfile : ES2Type
{
	public override void Write(object obj, ES2Writer writer)
	{
		UserManagerProfileContainer userManagerProfileCurrent = UserManagerProfileController.GetUserManagerProfileCurrent();
		writer.Write(userManagerProfileCurrent.ManagerUserName);
		writer.Write((int)userManagerProfileCurrent.BaseManagerType);
		writer.Write(userManagerProfileCurrent.UsersObtainedTraits);
		writer.Write(userManagerProfileCurrent.ManagerXP);
		writer.Write(CustomerDataController.Instance.CustomerDataHolder.Userdata.ManagerLevel);
		writer.Write(CustomerDataController.Instance.CustomerDataHolder.Userdata.Boosts);
		writer.Write(CustomerDataController.Instance.CustomerDataHolder.Userdata.Credits);
		writer.Write(userManagerProfileCurrent.ManagerPoints);
		writer.Write(CustomerDataController.Instance.CustomerDataHolder.Userdata.VIP);
		writer.Write(userManagerProfileCurrent.CareerHistory);
		writer.Write(userManagerProfileCurrent.TrophiesEarned);
	}

	public override object Read(ES2Reader reader)
	{
		UserManagerProfile userManagerProfile = new UserManagerProfile();
		Read(reader, userManagerProfile);
		return userManagerProfile;
	}

	public override void Read(ES2Reader reader, object c)
	{
		UserManagerProfile userManagerProfile = (UserManagerProfile)c;
		userManagerProfile.ManagerName = reader.Read<string>();
		userManagerProfile.BaseManagerType = (ManagerType)reader.Read<int>();
		userManagerProfile.UsersObtainedTraits = reader.ReadList<UsersManagerTraits>();
		userManagerProfile.ManagerXP = reader.Read<int>();
		userManagerProfile.ManagerLevel = reader.Read<int>();
		userManagerProfile.Boosts = reader.Read<int>();
		userManagerProfile.Credits = reader.Read<int>();
		userManagerProfile.ManagerPoints = reader.Read<int>();
		userManagerProfile.VIP = reader.Read<bool>();
		userManagerProfile.CareerHistory = reader.ReadDictionary<string, CareerEntryList>();
		userManagerProfile.TrophiesEarned = reader.ReadDictionary<short, TrophyList>();
	}

	public override void WriteNonAlloc(ES2Writer writer)
	{
		throw new NotImplementedException();
	}

	public ES2UserType_UserManagerProfile()
		: base(typeof(UserManagerProfile))
	{
	}
}
