using UnityEngine;
using System.Collections;

public class TextController : MonoBehaviour {
	public GameObject shrinkObject;
	public float animationSpeed = .01f;

	float scaleCap = 1;
	float currentSize = 0;
	bool reverseAnimation = false;
	bool isAnimateMessage = false;
	
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
				reverseAnimation = false;
				isAnimateMessage = false;
				GameObject.Find ("GameController").GetComponent<GameController>().registerMessage();
			}

			gameObject.transform.localScale += new Vector3(speedToAdd, speedToAdd, 0);
			shrinkObject.transform.localScale -= new Vector3(speedToAdd, speedToAdd, 0);
		}
	}

	public void animateMessage() {
		isAnimateMessage = true;
	}
}
