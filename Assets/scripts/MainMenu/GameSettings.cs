using UnityEngine;
using System.Collections;

public class GameSettings : MonoBehaviour {
	int gameDifficulty = 0;

	public void setGameDifficulty(int difficulty) {
		gameDifficulty = difficulty;
	}

	public int getGameDifficulty() {
		return gameDifficulty;
	}


	// Use this for initialization
	void Start () {
	
	}
}
