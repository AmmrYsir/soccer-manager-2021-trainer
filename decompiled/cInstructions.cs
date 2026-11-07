public class cInstructions
{
	public byte formationid;

	public SeFixtureInfo.TacticsOption attackingstyle = new SeFixtureInfo.TacticsOption(1);

	public bool counterattack;

	public bool menbehindball;

	public SeFixtureInfo.TacticsOption width = new SeFixtureInfo.TacticsOption(2);

	public SeFixtureInfo.TacticsOption mentality = new SeFixtureInfo.TacticsOption(3);

	public SeFixtureInfo.TacticsOption passingstyleid = new SeFixtureInfo.TacticsOption(2);

	public bool playoffside;

	public SeFixtureInfo.TacticsOption pressingid = new SeFixtureInfo.TacticsOption(2);

	public SeFixtureInfo.TacticsOption tacklingstyle = new SeFixtureInfo.TacticsOption(1);

	public SeFixtureInfo.TacticsOption tempoid = new SeFixtureInfo.TacticsOption(2);

	public bool tightmarking;

	public bool useplaymaker;

	public bool usetargetman;

	public int[] plyrinst = new int[11];

	public SeFixtureInfo.TacticsRole[] playerRoles = new SeFixtureInfo.TacticsRole[11];

	public SeFixtureInfo.TacticsOption attforwards = new SeFixtureInfo.TacticsOption(3);

	public SeFixtureInfo.TacticsOption attmidfielders = new SeFixtureInfo.TacticsOption(3);

	public SeFixtureInfo.TacticsOption attwingers = new SeFixtureInfo.TacticsOption(3);

	public SeFixtureInfo.TacticsOption backline = new SeFixtureInfo.TacticsOption(2);

	public SeFixtureInfo.TacticsOption creativity = new SeFixtureInfo.TacticsOption(2);

	public SeFixtureInfo.TacticsOption captain = new SeFixtureInfo.TacticsOption(7);

	public SeFixtureInfo.TacticsOption penaltytaker = new SeFixtureInfo.TacticsOption(7);

	public SeFixtureInfo.TacticsOption deadballtakerleft = new SeFixtureInfo.TacticsOption(7);

	public SeFixtureInfo.TacticsOption deadballtakerright = new SeFixtureInfo.TacticsOption(7);

	public SeFixtureInfo.TacticsOption cornertakerleft = new SeFixtureInfo.TacticsOption(7);

	public SeFixtureInfo.TacticsOption cornertakerright = new SeFixtureInfo.TacticsOption(7);

	public int captainid;

	public int penaltytakerid;

	public int deadballtakerleftid;

	public int deadballtakerrightid;

	public int cornertakerleftid;

	public int cornertakerrightid;

	public cInstructions()
	{
		for (int i = 0; i < 11; i++)
		{
			playerRoles[i] = new SeFixtureInfo.TacticsRole(0);
		}
	}

	public bool equal(cInstructions inst)
	{
		bool result = false;
		if (formationid == inst.formationid && attackingstyle.value == inst.attackingstyle.value && counterattack == inst.counterattack && menbehindball == inst.menbehindball && mentality.value == inst.mentality.value && passingstyleid.value == inst.passingstyleid.value && playoffside == inst.playoffside && pressingid.value == inst.pressingid.value && tacklingstyle.value == inst.tacklingstyle.value && tempoid.value == inst.tempoid.value && tightmarking == inst.tightmarking && useplaymaker == inst.useplaymaker && usetargetman == inst.usetargetman && plyrinst == inst.plyrinst && PlayerRolesEqual(inst) && width.value == inst.width.value && attforwards.value == inst.attforwards.value && attmidfielders.value == inst.attmidfielders.value && attwingers.value == inst.attwingers.value && backline.value == inst.backline.value && creativity.value == inst.creativity.value && captain.value == inst.captain.value && penaltytaker.value == inst.penaltytaker.value && deadballtakerleft.value == inst.deadballtakerleft.value && deadballtakerright.value == inst.deadballtakerright.value && cornertakerleft.value == inst.cornertakerleft.value && cornertakerright.value == inst.cornertakerright.value)
		{
			result = true;
		}
		return result;
	}

	private bool PlayerRolesEqual(cInstructions inst)
	{
		for (int i = 0; i < playerRoles.Length; i++)
		{
			if (playerRoles[i].value != inst.playerRoles[i].value)
			{
				return false;
			}
		}
		return true;
	}

	public void copy(cInstructions inst)
	{
		formationid = inst.formationid;
		attackingstyle.value = inst.attackingstyle.value;
		counterattack = inst.counterattack;
		menbehindball = inst.menbehindball;
		mentality.value = inst.mentality.value;
		passingstyleid.value = inst.passingstyleid.value;
		playoffside = inst.playoffside;
		pressingid.value = inst.pressingid.value;
		tacklingstyle.value = inst.tacklingstyle.value;
		tempoid.value = inst.tempoid.value;
		tightmarking = inst.tightmarking;
		useplaymaker = inst.useplaymaker;
		usetargetman = inst.usetargetman;
		plyrinst = inst.plyrinst;
		playerRoles = new SeFixtureInfo.TacticsRole[11];
		for (int i = 0; i < inst.playerRoles.Length; i++)
		{
			playerRoles[i] = new SeFixtureInfo.TacticsRole(inst.playerRoles[i].value);
		}
		width.value = inst.width.value;
		attforwards.value = inst.attforwards.value;
		attmidfielders.value = inst.attmidfielders.value;
		attwingers.value = inst.attwingers.value;
		backline.value = inst.backline.value;
		creativity.value = inst.creativity.value;
		captain.value = inst.captain.value;
		captainid = inst.captainid;
		penaltytaker.value = inst.penaltytaker.value;
		penaltytakerid = inst.penaltytakerid;
		deadballtakerleft.value = inst.deadballtakerleft.value;
		deadballtakerleftid = inst.deadballtakerleftid;
		deadballtakerright.value = inst.deadballtakerright.value;
		deadballtakerrightid = inst.deadballtakerrightid;
		cornertakerleft.value = inst.cornertakerleft.value;
		cornertakerleftid = inst.cornertakerleftid;
		cornertakerright.value = inst.cornertakerright.value;
		cornertakerrightid = inst.cornertakerrightid;
	}

	public void ClearPlayerRoles()
	{
		playerRoles = new SeFixtureInfo.TacticsRole[11];
		for (int i = 0; i < playerRoles.Length; i++)
		{
			playerRoles[i] = new SeFixtureInfo.TacticsRole(0);
		}
	}

	public void RevertTacticsMods()
	{
		attackingstyle.RevertValue();
		width.RevertValue();
		mentality.RevertValue();
		passingstyleid.RevertValue();
		pressingid.RevertValue();
		tacklingstyle.RevertValue();
		tempoid.RevertValue();
		for (int i = 0; i < playerRoles.Length; i++)
		{
			playerRoles[i].RevertValue();
		}
		attforwards.RevertValue();
		attmidfielders.RevertValue();
		attwingers.RevertValue();
		backline.RevertValue();
		creativity.RevertValue();
		captain.RevertValue();
		penaltytaker.RevertValue();
		deadballtakerleft.RevertValue();
		deadballtakerright.RevertValue();
		cornertakerleft.RevertValue();
		cornertakerright.RevertValue();
	}
}
