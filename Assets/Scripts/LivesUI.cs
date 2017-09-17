using UnityEngine;
using UnityEngine.UI;

public class LivesUI : MonoBehaviour {

	public Text livesText;

	// Update is called once per frame
	void Update () {
		if (PlayerStats.Lives > 0)
		{
			livesText.text = PlayerStats.Lives.ToString() + " LIVES";
		} else
		{
			livesText.text = "0 LIVES";
		}
	}
}
