using UnityEngine;
using System.Collections;

public class MainMenuPieces : MonoBehaviour {
	//Animation variables
	float animateSpeed = .025f;
	float animateMaxSize = .50f;
	float animateTotalChange = .00f;
	bool reverseAnimation = false;

	//Shuffle variables
	Vector3 shuffleToLocation;
	Vector3 currentLocation;
	public float shuffleSpeedByFrame = 100;
	float shuffleCounter = 0;

	Animator animate;
	bool isAnimating = false;
	bool isPlayerClicking = false;
	bool isShuffling = false;



	// Use this for initialization
	void Start () {
		GameObject.Find ("BackgroundController").GetComponent<BackgroundController> ().registerGamePiece(gameObject);
		animate = (Animator) GetComponent ("Animator");
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if (isAnimating || isPlayerClicking) {
			animateFlicker ();
		} else if (isShuffling) {
			animateShuffle();
		}
	}
	void animateShuffle (){
		float xLocChange = Mathf.Abs(currentLocation.x - shuffleToLocation.x) / shuffleSpeedByFrame;
		if (currentLocation.x > shuffleToLocation.x) {
			xLocChange *= -1.0f;
		}
		float yLocChange = Mathf.Abs(currentLocation.y - shuffleToLocation.y) / shuffleSpeedByFrame;
		if (currentLocation.y > shuffleToLocation.y) {
			yLocChange *= -1.0f;
		}
		
		if (shuffleCounter >= shuffleSpeedByFrame) {
			isShuffling = false;
			shuffleCounter = 0;
			GameObject.Find ("BackgroundController").GetComponent<BackgroundController> ().registerShuffle();
		} else {
			shuffleCounter++;
			transform.localPosition += new Vector3(xLocChange, yLocChange, 0);
		}
		
	}

	void animateFlicker () {
		if (reverseAnimation && animateTotalChange <= 0) {
			reverseAnimation = false;
			if (isAnimating) {
				GameObject.Find ("BackgroundController").GetComponent<BackgroundController> ().finishedAnimation ();
				isAnimating = false;
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

	public void animatePiece() {
		isAnimating = true;
	}
	
	public void shufflePiece(Vector3 newLocation) {
		currentLocation = transform.localPosition;
		shuffleToLocation = newLocation;
		isShuffling = true;
	}


}
