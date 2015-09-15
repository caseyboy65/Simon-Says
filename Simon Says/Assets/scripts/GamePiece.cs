using UnityEngine;
using System.Collections;

public class GamePiece : MonoBehaviour {

	Animator animate;
	bool isAnimating = false;
	bool isPlayerClicking = false;
	
	//GameController GameController;
	// Use this for initialization
	void Start () {
		GameObject.Find ("GameController").GetComponent<GameController> ().registerGamePiece(gameObject);
		animate = (Animator) GetComponent ("Animator");
	}
	
	// Update is called once per frame
	void Update () {
		if (isAnimating || isPlayerClicking) {
			Debug.Log ("Is in idle state" + animate.GetCurrentAnimatorStateInfo (0).IsName ("Idle"));
			if (animate.GetCurrentAnimatorStateInfo (0).IsName ("Idle")) {
			//if (animate.IsInTransition ()) {
				if (isAnimating) {
					GameObject.Find ("GameController").GetComponent<GameController> ().finishedAnimation ();
					isAnimating = false;
				} else if (isPlayerClicking) {
					GameObject.Find ("GameController").GetComponent<GameController> ().verifyClick (gameObject);
					isPlayerClicking = false;
				}
			}
		}
	}

	public void animatePiece() {
		//Trigger animation here
		isAnimating = true;
		animate.Play ("Flicker");

	}

	void OnMouseDown() {
		if (GameObject.Find ("GameController").GetComponent<GameController> ().getPlayerPhase ()) {
			isPlayerClicking = true;
			animate.Play ("Flicker");
		}

	}
}
