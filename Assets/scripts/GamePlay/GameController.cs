using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour {
	//Game difficulty, 1 = easy, 2 = medium, 3 = hard, 4 = impossible
	//public int gameDifficutly = 1;
	
	//Game settings based on difficulty
	int resetCounter = 0;
	
	//Game Scoring
	int gameScore = 0;
	float roundNumberPercent = 0;					//Percentage of Game round completed. ranges from 0-1
	
	//Selection order vars
	ArrayList gamePieces = new ArrayList();
	ArrayList roundOrder = new ArrayList(); 		//List of the elements to click in order
	
	int currentPlayNumber = 0;
	
	bool startGame = false;
	
	bool inAnimationPhase = false;
	bool isPlayerPhase = false;
	
	int playerClickNumber = 0;
	
	/* Initate starting variables */
	void Start () {
		//Set up vars based on game difficulty
		/* EASY MODE */
		GameSettings gameSettings = GameObject.Find ("ParamObj").GetComponent<GameSettings> ();
		int gameDifficulty = gameSettings.getGameDifficulty ();
		if (gameDifficulty == 1) {
			resetCounter = 5;
		}
		
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if (startGame == false) {
			startNextRound ();
		} else {
			if (isPlayerPhase) {
				
			} else if (!inAnimationPhase) {
				if (currentPlayNumber > roundOrder.Count) {
					currentPlayNumber = 0;
				}
				//turn animationPhase off to prevent all animations from firing at once
				inAnimationPhase = true;
				
				
				GameObject obj = (GameObject)gamePieces [(int)roundOrder [currentPlayNumber]];
				obj.GetComponent<GamePiece> ().animatePiece ();
				
				//prepare for next click
				currentPlayNumber++;
			}
		}
	}
	
	/* Finish and animation and check if its player's turn */
	public void finishedAnimation() {
		if (currentPlayNumber >= roundOrder.Count) {
			startPlayerPhase ();
		} else {
			inAnimationPhase = false;
		}
	}
	
	/*Start the next iteration round*/
	public void startNextRound() {
		if (roundOrder.Count >= resetCounter) {
			roundOrder.Clear ();
			roundNumberPercent = 0;
		}
		
		Debug.Log ("Starting new round");
		startGame = true;
		isPlayerPhase = false;
		inAnimationPhase = false;
		//add the next block to click
		roundOrder.Add(Random.Range(0, 9));
		currentPlayNumber = 0;
		playerClickNumber = 0;
		//Get the percentage of Phase done and update the RoundIndicator UI To reflect
		roundNumberPercent = ((roundOrder.Count * 1.0f) / resetCounter);
		Debug.Log ("Round Size = " + roundOrder.Count);
		Debug.Log ("Total Round size = " + resetCounter);
		Debug.Log ("Percentage of round done = " + roundNumberPercent);
		GameObject.Find ("RoundNumber").transform.localScale = new Vector3 (roundNumberPercent, 1, 1); 		
	}	
	
	void startPlayerPhase () {
		isPlayerPhase = true;
	}
	
	public bool getPlayerPhase() {
		return isPlayerPhase;
	}
	
	public void verifyClick (GameObject clickedObj) {
		GameObject correctObj = (GameObject)gamePieces [(int) roundOrder[playerClickNumber]];
		
		if (correctObj.Equals (clickedObj)) {
			playerClickNumber++;
		} else {
			Application.LoadLevel ("GameOver");
		}
		
		if (playerClickNumber >= roundOrder.Count) {
			startNextRound ();
		}
	}
	
	public void registerGamePiece(GameObject gameObj) {
		gamePieces.Add (gameObj);
	}
}
