using UnityEngine;
using System.Collections;

public class TextController : MonoBehaviour {
	public GameObject shrinkObject;
	public float animationSpeed = .01f;
	public Vector3 startingLoc;

	float scaleCap = 1;
	float currentSize = 0;
	bool reverseAnimation = false;
	bool isAnimateMessage = false;
	float startingZLoc;
	float startingYLoc;
	float startingXLoc;

	// Update is called once per frame
	void Update () {
		if (isAnimateMessage) {
			float speedToAdd = animationSpeed;
			if (reverseAnimation) {
				speedToAdd *= -1f;
			}
			currentSize += speedToAdd;
			if (currentSize >= 1) {
				reverseAnimation = true;
			} else if (reverseAnimation && currentSize <= 0) {
				endAnimation();
			}

			gameObject.transform.localScale += new Vector3(speedToAdd, speedToAdd, 0);
			shrinkObject.transform.localScale -= new Vector3(speedToAdd, speedToAdd, 0);
		}
	}

	void endAnimation() {
		reverseAnimation = false;
		isAnimateMessage = false;
		GameObject.Find ("GameController").GetComponent<GameController>().registerMessage();
		//Moving obj behind camera. This is to fix and issue where the text wasn't really disappearing into a 0x0 size object
		gameObject.transform.localPosition = new Vector3(gameObject.transform.localPosition.x, gameObject.transform.localPosition.y, -1f);
	}

	public void animateMessage() {
		//Moving obj behind in correct place to play animation
		gameObject.transform.localPosition = startingLoc;
		isAnimateMessage = true;
	}
}
