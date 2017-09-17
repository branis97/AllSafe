using UnityEngine;
using UnityEngine.SceneManagement;

public class Credits : MonoBehaviour {

	public string mainMenu = "MainMenu";

	public SceneFader sceneFader;

	public void ToMenu ()
	{
		sceneFader.FadeTo(mainMenu);
	}
}
