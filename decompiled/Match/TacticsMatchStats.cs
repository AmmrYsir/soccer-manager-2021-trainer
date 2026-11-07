using UnityEngine;

public class TacticsMatchStats : MonoBehaviour
{
	public static GameObject StatsPanel;

	private void OnEnable()
	{
		if (gameObject.activeInHierarchy)
		{
			if (transform.childCount > 0)
			{
				Object.Destroy(transform.GetChild(0).gameObject);
			}
			Object.Instantiate(StatsPanel, transform, worldPositionStays: false);
		}
	}
}
