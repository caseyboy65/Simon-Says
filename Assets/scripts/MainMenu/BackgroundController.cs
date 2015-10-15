using UnityEngine;
using System.Collections;

public class BackgroundController : MonoBehaviour {

	ArrayList gamePieces = new ArrayList();			//List of all game pieces invovled in game
	ArrayList gamePiecesLocations = new ArrayList ();//List of the starting locations of all game pieces (not linked to each piece, doesn't matter)\

	int piecesToPlay = 0;
	int shuffleRegisterCounter = 0;
	bool isShuffling = false;
	bool isFlickering = false;
	
	// Update is called once per frame
	void FixedUpdate () {
		if (!isShuffling && !isFlickering) {
			if (piecesToPlay < 5) {
				flicker();
				piecesToPlay++;
			} else {
				piecesToPlay = 0;
				shuffle();
			}
		}
	}

	void flicker(){
		isFlickering = true;
		//Find the next gamepiece to animate and animate it
		GameObject obj = (GameObject)gamePieces [Random.Range (0, gamePieces.Count)];
		obj.GetComponent<MainMenuPieces> ().animatePiece ();
	}

	void shuffle() {
		isShuffling = true;
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
			currentPiece.GetComponent<MainMenuPieces>().shufflePiece(newLocation);
			gamePieceCounter++;
		}
	}
	
	/* Register the game pieces in use of the game, this is called on start of GamePiece objects */
	public void registerGamePiece(GameObject gameObj) {
		//Add the game piece itself
		gamePieces.Add (gameObj);
		//Add its current location to know where pieces should shuffle to
		gamePiecesLocations.Add (gameObj.transform.localPosition);
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
			isShuffling = false;
		}
	}

	/* Finish and animation and check if its player's turn or to continue the next animation */ 
	public void finishedAnimation() {
		isFlickering = false;
	}
}
