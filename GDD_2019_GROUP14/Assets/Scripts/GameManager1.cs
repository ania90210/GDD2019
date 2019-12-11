using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager1 : MonoBehaviour
{
    public static GameManager1 instance = null;

	public BoardManager boardScript;
	public PlayerStorage storage;
	
	private int level = 3;
	
	
	// Use this for initialization
	void Awake () {
		if (instance == null) 
			instance = this;
		else if (instance != this)
			Destroy (gameObject);

        //Sets this to not be destroyed when reloading scene
		DontDestroyOnLoad (gameObject);

		// Get a reference to the board and set it up
		boardScript = GetComponent<BoardManager>();
		InitGame (); 

		// Create a player storage to keep track of collectables
		storage = new PlayerStorage();
	}
	
    //Initializes the game for each level
	void InitGame() {
		boardScript.SetupScene(level);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}

