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
		} else if (gameObject.name == "CrazyMode") {
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
		//Load game play scene to start game
		Application.LoadLevel ("gamePlay");
	}

}
