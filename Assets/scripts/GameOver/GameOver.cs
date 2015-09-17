using UnityEngine;
using System.Collections;

public class GameOver : MonoBehaviour {

	void OnMouseDown() {
		Debug.Log ("Going back to main menu");
		Application.LoadLevel ("MainMenu");
	}
}
