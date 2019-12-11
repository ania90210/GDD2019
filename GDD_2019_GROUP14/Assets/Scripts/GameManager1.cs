using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager1 : MonoBehaviour
{
    public static GameManager1 instance = null;

	public BoardManager boardScript;
	
	private int level = 3;
	
	
	// Use this for initialization
	void Awake () {
		if (instance == null) 
			instance = this;
		else if (instance != this)
			Destroy (gameObject);

        //Sets this to not be destroyed when reloading scene
		DontDestroyOnLoad (gameObject);

		boardScript = GetComponent<BoardManager>();
		InitGame (); 
	}
	
    //Initializes the game for each level
	void InitGame() {
		boardScript.SetupScene(level);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}

