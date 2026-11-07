using UnityEngine;
using UnityEngine.UI;

public class SaveGame : MonoBehaviour
{
	private bool saveWaiting;

	private bool saveBegan;

	private Button saveButton;

	private float cooldown = 5f;

	private float remainingCooldown;

	private bool coolingDown;

	private void Start()
	{
		saveButton = transform.GetComponent<Button>();
		saveButton.onClick.RemoveAllListeners();
		saveButton.onClick.AddListener(() =>
		{
			if (!coolingDown)
			{
				Save.SaveData(Save.currentSlot);
				remainingCooldown = cooldown;
				coolingDown = true;
				saveButton.interactable = false;
			}
		});
	}

	private void Update()
	{
		if (coolingDown)
		{
			if (remainingCooldown > 0f)
			{
				remainingCooldown -= Time.deltaTime;
				return;
			}
			saveButton.interactable = true;
			coolingDown = false;
		}
	}
}
