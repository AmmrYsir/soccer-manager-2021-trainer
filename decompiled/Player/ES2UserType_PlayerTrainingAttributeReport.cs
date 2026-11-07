using System;

public class ES2UserType_PlayerTrainingAttributeReport : ES2Type
{
	public override void Write(object obj, ES2Writer writer)
	{
		PlayerTrainingAttributeReport playerTrainingAttributeReport = (PlayerTrainingAttributeReport)obj;
		writer.Write(playerTrainingAttributeReport.playerId);
		writer.Write(playerTrainingAttributeReport.attributeId);
		writer.Write(playerTrainingAttributeReport.fullAttributeChange);
	}

	public override object Read(ES2Reader reader)
	{
		PlayerTrainingAttributeReport playerTrainingAttributeReport = new PlayerTrainingAttributeReport();
		Read(reader, playerTrainingAttributeReport);
		return playerTrainingAttributeReport;
	}

	public override void Read(ES2Reader reader, object c)
	{
		PlayerTrainingAttributeReport playerTrainingAttributeReport = (PlayerTrainingAttributeReport)c;
		playerTrainingAttributeReport.playerId = reader.Read<int>();
		playerTrainingAttributeReport.attributeId = reader.Read<int>();
		playerTrainingAttributeReport.fullAttributeChange = reader.Read<int>();
	}

	public override void WriteNonAlloc(ES2Writer writer)
	{
		throw new NotImplementedException();
	}

	public ES2UserType_PlayerTrainingAttributeReport()
		: base(typeof(PlayerTrainingAttributeReport))
	{
	}
}
