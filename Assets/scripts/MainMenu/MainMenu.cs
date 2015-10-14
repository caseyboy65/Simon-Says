using UnityEngine;
using System.Collections;

public class MainMenu : MonoBehaviour {

	void OnMouseDown() {
		if (gameObject.name == "EasyMode") {
			storeGameSettings(1);
		} else if (gameObject.name == "NormalMode") {
			storeGameSettings(2);
		} else if (gameObject.name == "HardMode") {
			storeGameSettings(3);
		} else if (gameObject.name == "SuperMode") {
			storeGameSettings(4);
		} else if (gameObject.name == "Quit") {
			//Something to add later
		}
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
