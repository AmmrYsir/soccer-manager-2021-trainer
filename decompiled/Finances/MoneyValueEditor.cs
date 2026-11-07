using UnityEngine;
using UnityEngine.UI;

public class MoneyValueEditor : MonoBehaviour
{
	private enum ValueInputFields
	{
		Millions,
		Thousands,
		Hundreds,
		None
	}

	private Color showInputField = new Color(255f, 255f, 255f, 255f);

	private Color hideInputField = new Color(255f, 255f, 255f, 0f);

	private ValueInputFields focusedInputField;

	private InputField millionsInput;

	private InputField thousandsInput;

	private InputField hundredsInput;

	private GameObject hundredMillions;

	private GameObject tensMillions;

	private GameObject millions;

	private GameObject hundredThousands;

	private GameObject tensThousands;

	private GameObject thousands;

	private GameObject hundreds;

	private GameObject tens;

	private GameObject ones;

	private GameObject[] wheels;

	private double totalValue;

	private int millionsVal;

	private double maxAmount;

	public OnAmountChanged Callback { get; set; }

	private void Update()
	{
		ValueInputFields valueInputFields = ValueInputFields.None;
		if (millionsInput != null && millionsInput.isFocused)
		{
			valueInputFields = ValueInputFields.Millions;
		}
		if (thousandsInput != null && thousandsInput.isFocused)
		{
			valueInputFields = ValueInputFields.Thousands;
		}
		if (hundredsInput != null && hundredsInput.isFocused)
		{
			valueInputFields = ValueInputFields.Hundreds;
		}
		if (valueInputFields != focusedInputField)
		{
			switch (valueInputFields)
			{
			case ValueInputFields.Millions:
				millionsInput.GetComponent<Image>().color = showInputField;
				millionsInput.text = "";
				break;
			case ValueInputFields.Thousands:
				thousandsInput.GetComponent<Image>().color = showInputField;
				thousandsInput.text = "";
				break;
			case ValueInputFields.Hundreds:
				hundredsInput.GetComponent<Image>().color = showInputField;
				hundredsInput.text = "";
				break;
			}
			switch (focusedInputField)
			{
			case ValueInputFields.Millions:
				millionsInput.GetComponent<Image>().color = hideInputField;
				millionsInput.text = "";
				break;
			case ValueInputFields.Thousands:
				thousandsInput.GetComponent<Image>().color = hideInputField;
				thousandsInput.text = "";
				break;
			case ValueInputFields.Hundreds:
				hundredsInput.GetComponent<Image>().color = hideInputField;
				hundredsInput.text = "";
				break;
			}
		}
		focusedInputField = valueInputFields;
	}

	public void Init()
	{
		millionsInput = transform.Find("MillionsInput").GetComponent<InputField>();
		millionsInput.onEndEdit.AddListener(delegate
		{
			SetMillions(millionsInput);
		});
		thousandsInput = transform.Find("ThousandsInput").GetComponent<InputField>();
		thousandsInput.onEndEdit.AddListener(delegate
		{
			SetThousands(thousandsInput);
		});
		hundredsInput = transform.Find("HundredsInput").GetComponent<InputField>();
		hundredsInput.onEndEdit.AddListener(delegate
		{
			SetHundreds(hundredsInput);
		});
		hundredMillions = transform.Find("HundMill").gameObject;
		tensMillions = transform.Find("TensMill").gameObject;
		millions = transform.Find("Millions").gameObject;
		hundredThousands = transform.Find("HundThou").gameObject;
		tensThousands = transform.Find("TensThou").gameObject;
		thousands = transform.Find("Thou").gameObject;
		hundreds = transform.Find("Hund").gameObject;
		tens = transform.Find("Tens").gameObject;
		ones = transform.Find("Ones").gameObject;
		wheels = new GameObject[6] { hundredThousands, tensThousands, thousands, hundreds, tens, ones };
		for (int num = 0; num < wheels.Length; num++)
		{
			int num2 = num;
			wheels[num].transform.Find("Inc").GetComponent<Button>().onClick.AddListener(() =>
			{
				IncWheel(num2);
			});
			wheels[num].transform.Find("Dec").GetComponent<Button>().onClick.AddListener(() =>
			{
				DecWheel(num2);
			});
		}
		millions.transform.Find("Inc").GetComponent<Button>().onClick.AddListener(IncMillions);
		millions.transform.Find("Dec").GetComponent<Button>().onClick.AddListener(DecMillions);
	}

	public void SetValue(double transferOffer, double clubBalance)
	{
		char[] array = transferOffer.ToString().ToCharArray();
		int num = array.Length;
		hundredMillions.GetComponentInChildren<Text>().text = ((num >= 9) ? array[num - 9].ToString() : "");
		tensMillions.GetComponentInChildren<Text>().text = ((num >= 8) ? array[num - 8].ToString() : "");
		millions.GetComponentInChildren<Text>().text = ((num >= 7) ? array[num - 7].ToString() : "0");
		millionsVal = int.Parse("0" + hundredMillions.GetComponentInChildren<Text>().text + tensMillions.GetComponentInChildren<Text>().text + millions.GetComponentInChildren<Text>().text);
		hundredThousands.GetComponentInChildren<Text>().text = ((num >= 6) ? array[num - 6].ToString() : "0");
		tensThousands.GetComponentInChildren<Text>().text = ((num >= 5) ? array[num - 5].ToString() : "0");
		thousands.GetComponentInChildren<Text>().text = ((num >= 4) ? array[num - 4].ToString() : "0");
		hundreds.GetComponentInChildren<Text>().text = ((num >= 3) ? array[num - 3].ToString() : "0");
		tens.GetComponentInChildren<Text>().text = ((num >= 2) ? array[num - 2].ToString() : "0");
		ones.GetComponentInChildren<Text>().text = ((num >= 1) ? array[num - 1].ToString() : "0");
		maxAmount = clubBalance;
		SetTextColour();
	}

	public double GetNewValue()
	{
		string text = "";
		for (int i = 0; i < wheels.Length; i++)
		{
			text += wheels[i].GetComponentInChildren<Text>().text;
		}
		return double.Parse(text) + (double)(millionsVal * 1000000);
	}

	private void SetMillions(InputField input)
	{
		if (input.text.Length > 0)
		{
			millionsVal = int.Parse(input.text);
			UpdateMillions();
		}
		input.text = "";
	}

	private void IncMillions()
	{
		if (millionsVal < 999)
		{
			millionsVal++;
			UpdateMillions();
		}
	}

	private void DecMillions()
	{
		if (millionsVal > 0)
		{
			millionsVal--;
			UpdateMillions();
		}
	}

	private void UpdateMillions()
	{
		char[] array = millionsVal.ToString().ToCharArray();
		int num = array.Length;
		hundredMillions.GetComponentInChildren<Text>().text = ((num >= 3) ? array[num - 3].ToString() : "");
		tensMillions.GetComponentInChildren<Text>().text = ((num >= 2) ? array[num - 2].ToString() : "");
		millions.GetComponentInChildren<Text>().text = ((num >= 1) ? array[num - 1].ToString() : "");
		UpdateValue();
	}

	private void SetThousands(InputField input)
	{
		if (input.text.Length > 0)
		{
			char[] array = input.text.ToCharArray();
			int num = array.Length;
			hundredThousands.GetComponentInChildren<Text>().text = ((num >= 3) ? array[num - 3].ToString() : "0");
			tensThousands.GetComponentInChildren<Text>().text = ((num >= 2) ? array[num - 2].ToString() : "0");
			thousands.GetComponentInChildren<Text>().text = ((num >= 1) ? array[num - 1].ToString() : "0");
			UpdateValue();
		}
		input.text = "";
	}

	private void SetHundreds(InputField input)
	{
		if (input.text.Length > 0)
		{
			char[] array = input.text.ToCharArray();
			int num = array.Length;
			hundreds.GetComponentInChildren<Text>().text = ((num >= 3) ? array[num - 3].ToString() : "0");
			tens.GetComponentInChildren<Text>().text = ((num >= 2) ? array[num - 2].ToString() : "0");
			ones.GetComponentInChildren<Text>().text = ((num >= 1) ? array[num - 1].ToString() : "0");
			UpdateValue();
		}
		input.text = "";
	}

	private void IncWheel(int index)
	{
		int num = ((wheels[index].GetComponentInChildren<Text>().text != "") ? int.Parse(wheels[index].GetComponentInChildren<Text>().text) : 0);
		if (num < 9)
		{
			wheels[index].GetComponentInChildren<Text>().text = (num + 1).ToString();
			UpdateValue();
		}
	}

	private void DecWheel(int index)
	{
		int num = ((wheels[index].GetComponentInChildren<Text>().text != "") ? int.Parse(wheels[index].GetComponentInChildren<Text>().text) : 0);
		if (num > 0)
		{
			string text = (num - 1).ToString();
			wheels[index].GetComponentInChildren<Text>().text = text;
			UpdateValue();
		}
	}

	private void SetTextColour()
	{
		if (GetNewValue() > maxAmount)
		{
			hundredMillions.GetComponentInChildren<Text>().color = Color.red;
			tensMillions.GetComponentInChildren<Text>().color = Color.red;
			millions.GetComponentInChildren<Text>().color = Color.red;
			hundredThousands.GetComponentInChildren<Text>().color = Color.red;
			tensThousands.GetComponentInChildren<Text>().color = Color.red;
			thousands.GetComponentInChildren<Text>().color = Color.red;
			hundreds.GetComponentInChildren<Text>().color = Color.red;
			tens.GetComponentInChildren<Text>().color = Color.red;
			ones.GetComponentInChildren<Text>().color = Color.red;
		}
		else
		{
			hundredMillions.GetComponentInChildren<Text>().color = Color.black;
			tensMillions.GetComponentInChildren<Text>().color = Color.black;
			millions.GetComponentInChildren<Text>().color = Color.black;
			hundredThousands.GetComponentInChildren<Text>().color = Color.black;
			tensThousands.GetComponentInChildren<Text>().color = Color.black;
			thousands.GetComponentInChildren<Text>().color = Color.black;
			hundreds.GetComponentInChildren<Text>().color = Color.black;
			tens.GetComponentInChildren<Text>().color = Color.black;
			ones.GetComponentInChildren<Text>().color = Color.black;
		}
	}

	private void UpdateValue()
	{
		totalValue = GetNewValue();
		SetTextColour();
		if (Callback != null)
		{
			Callback(totalValue);
		}
	}
}
