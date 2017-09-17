using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

	public static bool GameIsOver;

	public GameObject gameOverUI;
	public GameObject completeLevelUI;
	public WaveSpawner waveSpawner;

	void Start ()
	{
		GameIsOver = false;
	}
	
	void Update () {
		if (GameIsOver)
			return;

		if (PlayerStats.Lives <= 0)
		{
			EndGame();
		}
	}

	void EndGame ()
	{
		GameIsOver = true;
		gameOverUI.SetActive(true);
		waveSpawner.GameOver();
	}

	public void WinLevel ()
	{
		GameIsOver = true;
		completeLevelUI.SetActive(true);
	}

}
