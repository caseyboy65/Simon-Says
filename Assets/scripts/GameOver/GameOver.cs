using UnityEngine;
using System.Collections;

public class GameOver : MonoBehaviour {

	public float hoverOverSizeIncrease;

	void OnMouseDown() {
		Debug.Log ("Going back to main menu");
		Application.LoadLevel ("MainMenu");
	}
	void OnMouseEnter() {
		gameObject.transform.localScale += new Vector3(hoverOverSizeIncrease,hoverOverSizeIncrease,0);
	}
	
	void OnMouseExit() {
		gameObject.transform.localScale -= new Vector3(hoverOverSizeIncrease,hoverOverSizeIncrease,0);
	}

}
