using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {

	public string levelToLoad = "MainLevel";
	public string creditsScreen = "Credits";

	public SceneFader sceneFader;

	public void Play ()
	{
		sceneFader.FadeTo(levelToLoad);
	}

	public void Quit ()
	{
		Debug.Log("Exciting...");
		Application.Quit();
	}

	public void Credits ()
	{
		sceneFader.FadeTo(creditsScreen);
	}
}
