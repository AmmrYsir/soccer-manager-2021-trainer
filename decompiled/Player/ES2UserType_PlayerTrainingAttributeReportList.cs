using System;

public class ES2UserType_PlayerTrainingAttributeReportList : ES2Type
{
	public override void Write(object obj, ES2Writer writer)
	{
		PlayerTrainingAttributeReportList playerTrainingAttributeReportList = (PlayerTrainingAttributeReportList)obj;
		writer.Write(playerTrainingAttributeReportList.list);
	}

	public override object Read(ES2Reader reader)
	{
		PlayerTrainingAttributeReportList playerTrainingAttributeReportList = new PlayerTrainingAttributeReportList();
		Read(reader, playerTrainingAttributeReportList);
		return playerTrainingAttributeReportList;
	}

	public override void Read(ES2Reader reader, object c)
	{
		((PlayerTrainingAttributeReportList)c).list = reader.ReadList<PlayerTrainingAttributeReport>();
	}

	public override void WriteNonAlloc(ES2Writer writer)
	{
		throw new NotImplementedException();
	}

	public ES2UserType_PlayerTrainingAttributeReportList()
		: base(typeof(PlayerTrainingAttributeReportList))
	{
	}
}
