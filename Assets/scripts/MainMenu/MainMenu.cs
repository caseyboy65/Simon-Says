using UnityEngine;
using System.Collections;

public class MainMenu : MonoBehaviour {

	void OnMouseDown() {
		if (gameObject.name == "EasyMode") {
			storeGameSettings(1);
		} else if (gameObject.name == "MediumMode") {
			//Something to add later
		} else if (gameObject.name == "HardMode") {
			//Something to add later
		} else if (gameObject.name == "CrazyMode") {
			//Something to add later
		} else if (gameObject.name == "Quit") {
			//Something to add later
		}
	}

	void storeGameSettings(int difficulty) {
		GameObject ParamObj = GameObject.Find ("ParamObj");
		//Debug.Log ("ParamObj obj = " + ParamObj.name);
		GameSettings gameSettings = ParamObj.GetComponent<GameSettings> ();
		gameSettings.setGameDifficulty (difficulty);

		//Prevent object from being to destroyed to pass params between scenes
		DontDestroyOnLoad(ParamObj);
		//Load game play scene to start game
		Application.LoadLevel ("gamePlay");
	}

}
