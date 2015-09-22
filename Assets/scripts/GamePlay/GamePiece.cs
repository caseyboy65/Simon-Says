using UnityEngine;
using System.Collections;

public class GamePiece : MonoBehaviour {

	Animator animate;
	bool isAnimating = false;
	bool isPlayerClicking = false;

	//Animation variables
	float animateSpeed = .025f;
	float animateMaxSize = .50f;
	float animateTotalChange = .00f;
	bool reverseAnimation = false;
	
	//GameController GameController;
	// Use this for initialization
	void Start () {
		GameObject.Find ("GameController").GetComponent<GameController> ().registerGamePiece(gameObject);
		animate = (Animator) GetComponent ("Animator");
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if (isAnimating || isPlayerClicking) {
			if (reverseAnimation && animateTotalChange <= 0) {
				reverseAnimation = false;
				if (isAnimating) {
					GameObject.Find ("GameController").GetComponent<GameController> ().finishedAnimation ();
					isAnimating = false;
				} else if (isPlayerClicking) {
					GameObject.Find ("GameController").GetComponent<GameController> ().verifyClick (gameObject);
					isPlayerClicking = false;
				}
			} else if (animateTotalChange >= animateMaxSize) {
				reverseAnimation = true;
			} 

			if (reverseAnimation) {
				animateTotalChange -= animateSpeed;
			} else {
				animateTotalChange += animateSpeed;
			}

			float newSize = 0;
			if (reverseAnimation) {
				newSize = transform.localScale.x + animateSpeed;
			} else {
				newSize = transform.localScale.x - animateSpeed;
			}

			transform.localScale = new Vector3(newSize, newSize, transform.localScale.z);

		}
	}

	void OnMouseDown() {
		if (GameObject.Find ("GameController").GetComponent<GameController> ().getPlayerPhase ()) {
			isPlayerClicking = true;
		}
	}

	public void animatePiece() {
		isAnimating = true;
	}

	public void setAnimateSpeed(float newSpeed) {
		animateSpeed = newSpeed;
	}

	public void setAnimateMaxSize(float newMaxSize) {
		animateMaxSize = newMaxSize;
	}

}
