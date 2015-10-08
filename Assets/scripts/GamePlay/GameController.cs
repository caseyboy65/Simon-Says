using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameController : MonoBehaviour {
	//Reference to the round indicator
	public Scrollbar roundBar;
	
	//Game settings based on difficulty
	int resetCounter = 5;							//Number of rounds to go before the end of the phase
	int scoreMultiplier = 1;						//Number of points to add per click based on difficulty level
	bool shuffleModeEnabled = false;				//Turn on shuffle mode, off for easy, enabled for normal+
	int shuffleIndex = 5;							//Number of rounds before the game pieces shuffle.
	bool isCameraRotateEnabled = false;				//Turn on rotate camera mode for hard+
	
	//Game Scoring
	int gameScore = 0;
	
	//Counter / flags to keep track
	int shuffleRegisterCounter = 0;					//Used to register the number of pieces that check back after shuffle
	float roundNumberPercent = 0;					//Percentage of Game round completed. ranges from 0-1
	bool startGame = false;							//Game has started 			//TODO: Check if this is even needed, could probablly find a way around this.
	bool inAnimationPhase = false;					//Game is currently animating the click order		//TODO: May not need if the FixedUpdate function is cleaned up and all animation are removed from it
	int currentRoundNumber = 0;						//Current item that needs to animate in animation phase
	bool isPlayerPhase = false;						//Game is currently waiting for the player to replay click order //TODO: May not need if the FixedUpdate function is cleaned up and all animation are removed from it
	int playerClickNumber = 0;						//Number of clicks that the player has processed
	bool isShufflePhase = false;					//Game is currently shuffling the game pieces //TODO: May not need if the FixedUpdate function is cleaned up and all animation are removed from it
	bool isCameraRotating = false;					//Game is currently rotating camera //TODO: May not need if the FixedUpdate function is cleaned up and all animation are removed from it
	bool playWatch = false;							
	bool playGo = false;
	int numberOfTimeToPlayWatchGo = 0;

	//Game data 
	ArrayList gamePieces = new ArrayList();			//List of all game pieces invovled in game
	ArrayList gamePiecesLocations = new ArrayList ();//List of the starting locations of all game pieces (not linked to each piece, doesn't matter)
	ArrayList roundOrder = new ArrayList(); 		//List of the elements to click in order
	
	/****************************************************************
	 * 				PRIVATE FUNCTIONS								*
	 ****************************************************************/
	/* Initate starting variables */
	void Start () {
		//Find the game param obj that was passed from start menu to get game settings
		GameSettings gameSettings = GameObject.Find ("ParamObj").GetComponent<GameSettings> ();
		int gameDifficulty = gameSettings.getGameDifficulty ();
		
		//Set game data based on game difficulty
		if (gameDifficulty == 1) { 				/* EASY MODE */
			resetCounter = 5;
			scoreMultiplier = 1;
			numberOfTimeToPlayWatchGo = 2;
		} else if (gameDifficulty == 2) { 		/* Normal MODE */
			resetCounter = 10;
			scoreMultiplier = 2;
			shuffleIndex = 5;
			shuffleModeEnabled = true;
			numberOfTimeToPlayWatchGo = 2;
		} else if (gameDifficulty == 3) { 		/* Hard MODE */
			resetCounter = 15;
			scoreMultiplier = 3;
			shuffleIndex = 5;
			shuffleModeEnabled = true;
			isCameraRotateEnabled = true;
		} else if (gameDifficulty == 4) { 		/* Super MODE */
			resetCounter = 20;
			scoreMultiplier = 4;
			shuffleIndex = 5;
			shuffleModeEnabled = true;
			isCameraRotateEnabled = true;
		}
	}
	
	// FixedUpdate is called once per frame
	void FixedUpdate () {
		//Start the game TODO: Might be able to just call this in the start function and remove the need for this check 
		if (startGame == false) {
			startGame = true;
			playWatch = true;
			startNextRound ();
		} else {
			if (isPlayerPhase || isShufflePhase || isCameraRotating || playWatch || playGo) {	//If the game is in player phase or shuffle do nothing TODO: Clean this up, might be able to remove this check or combine it below
				
			} else if (!inAnimationPhase) {	//If the game is not in an Animation phase, play next animation TODO: Clean this up as well, might be able to pull this out of fixed update
				//Reset count if at on the last animation TODO: May be able to remove if this is pulled out of fixed update
				if (currentRoundNumber > roundOrder.Count) {
					currentRoundNumber = 0;
				}
				//turn animationPhase flag on to prevent next animation until finished.
				inAnimationPhase = true;		
				
				//Find the next gamepiece to animate and animate it
				GameObject obj = (GameObject)gamePieces [(int)roundOrder [currentRoundNumber]];
				obj.GetComponent<GamePiece> ().animatePiece ();
				
				//prepare for next click
				currentRoundNumber++;
			}
		}
	}
	
	/* Finish up game phase and prepare to start next phase */
	void finishPhase() {
		//Clear the round data
		roundOrder.Clear ();
		//Reset the Round indicator
		roundNumberPercent = 0;
		if (shuffleModeEnabled) {//Shuffle piece to start next round
			shufflePieces ();
		} else { //Else start next round for easy mode
			startNextRound();
		}
	}
	
	/* Update the game score by adding to the game score and updating the Score Indicator to reflect score */
	void updateScore() {
		gameScore += scoreMultiplier;
		GameObject.Find ("GameScore").GetComponent<TextMesh> ().text = gameScore.ToString ();
	}
	
	/* Shuffle game peices by calling the shuffle function in all game piece objects */
	void shufflePieces() {
		//Turn shuffle phase on
		isShufflePhase = true;
		
		//Create a temp list of all game piece locations, and keep track of the number of game pieces
		ArrayList tempGamePiecesLocations = new ArrayList(gamePiecesLocations);
		int gamePieceCounter = 0;
		
		//While the temp list still has elements in it, this is done because as we use a location 
		//we will remove it from the list and loop again to use another random location
		while (tempGamePiecesLocations.Count > 0) {
			//Grab a random location in the list
			int randomNum = Random.Range (0, tempGamePiecesLocations.Count);
			//Set it in a temp newLocation var
			Vector3 newLocation = (Vector3) tempGamePiecesLocations[randomNum];
			//Remove the location from the temp list so we do not use it again
			tempGamePiecesLocations.RemoveAt(randomNum);
			//Find the current game piece with the counter and call its shuffle function to animate it to its new lcoation
			GameObject currentPiece = (GameObject) gamePieces[gamePieceCounter];
			currentPiece.GetComponent<GamePiece>().shufflePiece(newLocation);
			gamePieceCounter++;
		}
	}
	/* Trigger the next animation to show player */
	void playNextAnimation() { //TODO: This should probablly call what ever function that will do the animation if fixedupdate is cleaned up
		//Check if the current animation number = the size of the round order, if so start player phase
		if (currentRoundNumber >= roundOrder.Count) {
			startPlayerPhase ();
		} else { //else go to next animation
			inAnimationPhase = false;
		}
	}
	
	/* Register when a shuffle is finished. This is called when the game piece has finished its shuffle and game will continue
	 * when all peices are done 
	 */
	public void registerShuffle() {
		//Keep track of the number of pieces that have successfully finished shuffling
		shuffleRegisterCounter++;
		
		//If the number of pieces that have shuffled equal the number of known game pieces then continue the game
		if (shuffleRegisterCounter == gamePieces.Count) {
			//Reset counter for next use of register shuffle phase
			shuffleRegisterCounter = 0;
			startNextRound();
		}
	}
	
	/* Finish the rotation animation and continue game */
	public void registerCameraRotate() {
		isCameraRotating = false;
		playNextAnimation();
	}
	
	public void registerMessage() {
		if (playWatch) {
			playWatch = false;
			startNextRound ();
		} else if (playGo) {
			playGo = false;
		}
	}
	
	/* Finish and animation and check if its player's turn or to continue the next animation */ 
	public void finishedAnimation() {
		//Rotate camera if game diffuclty hard+ enabled
		if (isCameraRotateEnabled) {
			GameObject.Find("Main Camera").GetComponent<CameraController>().rotateCamera();
			isCameraRotating = true;
		} else { //else play next animation
			playNextAnimation();
		}
	}
	
	
	/*Start the next round iteration*/
	public void startNextRound() {
		if (roundOrder.Count >= resetCounter) {	//Check if the round is over
			finishPhase ();
		} else if (shuffleModeEnabled && 
		           roundOrder.Count != 0 && 
		           roundOrder.Count % shuffleIndex == 0 && 
		           !isShufflePhase) { //Check if the game should shuffle its game pieces
			shufflePieces ();
		} else if (playWatch ) {
			if (numberOfTimeToPlayWatchGo > 0) {
				GameObject.Find ("Watch").GetComponent<TextController>().animateMessage();
			} else {
				registerMessage();
			}
		} else {	//Else start next round	
			//Turn off all flags and reset counters as next round is starting //TODO: This might make sense in its own function as a reset function
			isPlayerPhase = false;
			inAnimationPhase = false;
			isShufflePhase = false;
			currentRoundNumber = 0;
			playerClickNumber = 0;
			
			int newItemToClick;
			if (roundOrder.Count > 0){
				do {
					newItemToClick = Random.Range (0, 9);
				} while (newItemToClick == (int) roundOrder[roundOrder.Count - 1]);	//Keep looping if the new piece is the same as last.
			} else {
				newItemToClick = Random.Range (0, 9);
			}
			//add the next game piece to click
			roundOrder.Add (newItemToClick);
			
			//Get the percentage of Phase done and update the RoundIndicator UI To reflect //TODO: This should probablly be its own function
			roundNumberPercent = ((roundOrder.Count * 1.0f) / resetCounter);
			roundBar.size = (roundOrder.Count * 1.0f) / (resetCounter * 1.0f);
		}
	}	
	
	/* Start the players turn */ //TODO: There may not be need for this, especially if fixedUpdate is cleaned up
	void startPlayerPhase () {
		isPlayerPhase = true;
		if (numberOfTimeToPlayWatchGo > 0) {
			numberOfTimeToPlayWatchGo--;
			GameObject.Find ("Go").GetComponent<TextController> ().animateMessage ();
		} else {
			registerMessage ();
		}
	}
	
	/* Checks if the game is in player phase, this is used in GamePieces to know if the player can click on gamepieces */
	public bool getPlayerPhase() {
		return isPlayerPhase;
	}
	
	/* Check if what the player has selected is the correct gamePiece in the correct order */
	public void verifyClick (GameObject clickedObj) {
		//Get the current game piece that was expected to be selected
		GameObject correctObj = (GameObject)gamePieces [(int) roundOrder[playerClickNumber]];
		
		if (correctObj.Equals (clickedObj)) { //Check if the last clicked game piece is the correct piece
			playerClickNumber++;
			updateScore();
		} else { //else if not, then game over
			Application.LoadLevel ("GameOver");
		}
		
		if (playerClickNumber >= roundOrder.Count) { //If all game pieces were selected then start next round
			playWatch = true;
			startNextRound ();
		}
	}
	
	/* Register the game pieces in use of the game, this is called on start of GamePiece objects */
	public void registerGamePiece(GameObject gameObj) {
		//Add the game piece itself
		gamePieces.Add (gameObj);
		//Add its current location to know where pieces should shuffle to
		gamePiecesLocations.Add (gameObj.transform.localPosition);
	}
}
