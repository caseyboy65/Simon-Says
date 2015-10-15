using UnityEngine;
using System.Collections;

public class MainMenu : MonoBehaviour {

	public float hoverOverSizeIncrease;

	void OnMouseDown() {
		if (gameObject.name == "Easy") {
			storeGameSettings(1);
		} else if (gameObject.name == "Normal") {
			storeGameSettings(2);
		} else if (gameObject.name == "Hard") {
			storeGameSettings(3);
		} else if (gameObject.name == "Insane") {
			storeGameSettings(4);
		} else if (gameObject.name == "Quit") {
			//Something to add later
		}
	}

	void OnMouseEnter() {
		gameObject.transform.localScale += new Vector3(hoverOverSizeIncrease,hoverOverSizeIncrease,0);
	}

	void OnMouseExit() {
		gameObject.transform.localScale -= new Vector3(hoverOverSizeIncrease,hoverOverSizeIncrease,0);
	}

	void storeGameSettings(int difficulty) {
		GameObject ParamObj = GameObject.Find ("ParamObj");
		GameSettings gameSettings = ParamObj.GetComponent<GameSettings> ();
		gameSettings.setGameDifficulty (difficulty);

		//Prevent object from being to destroyed to pass params between scenes
		DontDestroyOnLoad(ParamObj);

		//Sound effect
		GameObject.Find("StartGameSound").GetComponent<SoundEffects>().playSound();

		//Load game play scene to start game
		Invoke("loadGame", 2);
	}

	void loadGame() {
		Application.LoadLevel ("gamePlay");
	}

}
