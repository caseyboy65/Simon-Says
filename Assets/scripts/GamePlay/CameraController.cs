using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

	bool isRotating = false;					//Set flag to animate rotating
	public float CameraRotateSpeed = .5f;		//Speed at which the camera rotates per frame
	float maxAngle = 90f;						//The maximum distance the camera can rotate before stopping
	float rotateCounter = 0.0f;					//Counter to keep track of how far the camera has rotated in current animation cycle

	void FixedUpdate() {
		if (isRotating) {
			//Keep track of total distance the camera has rotated
			rotateCounter += CameraRotateSpeed;
			if (rotateCounter > maxAngle) { //If the camera has rotated max distance
				//Reset counter for next cylce
				rotateCounter = 0;
				//Turn animation flag off
				isRotating = false;
				//Register camera has finished rotation and resume game
				GameObject.Find("GameController").GetComponent<GameController>().registerCameraRotate();
			} else { //Else animate the rotation
				transform.eulerAngles += new Vector3 (0, 0, CameraRotateSpeed);
			}
		}
	}

	/* Start the rotation animation cycle */
	public void rotateCamera() {
		isRotating = true;
	}
}
