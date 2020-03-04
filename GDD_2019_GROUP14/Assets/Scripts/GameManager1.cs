using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager1 : MonoBehaviour
{
    public static GameManager1 instance = null;

	public BoardManager boardScript;
	public PlayerStorage storage;
	public GameObject artifactPrefab;
	[HideInInspector] public GameObject artifact;
	public TrashBin trashBinPrefab;
	[HideInInspector] public TrashBin trashBin;

	public GameManagerView view;
	
	private int level = 3;
	
	
	// Use this for initialization
	void Awake () {
		if (instance == null) 
			instance = this;
		else if (instance != this) {
			Destroy (gameObject);
			return;
		}

		// Get a reference to the board and set it up
		boardScript = GetComponent<BoardManager>();
		InitGame (); 

		// Create a player storage to keep track of collectables
		storage = new PlayerStorage();
	}
	
    //Initializes the game for each level
	void InitGame() {
		boardScript.SetupScene(level);

		// Place the trashbin somewhere on the board
		trashBin = boardScript.PlaceObjectRandom(trashBinPrefab.gameObject).GetComponent<TrashBin>();

		// When the trash bin is filled, do these actions
		trashBin.OnFilled.AddListener(delegate {
			artifact.SetActive(true);
		});

		// Place the artifact somewhere on the board
		artifact = boardScript.PlaceObjectRandom(artifactPrefab.gameObject);
		// At the start, the artifact is invisble to the player
		artifact.gameObject.SetActive(false);


	}
	
}

[System.Serializable]
public class GameManagerView {
	public Text trashCountText;

}
