using Newtonsoft.Json;

public class Sm21Cash
{
	[JsonProperty("Cash")]
	public long Cash { get; set; }

	[JsonProperty("CreditCost")]
	public long CreditCost { get; set; }
}
