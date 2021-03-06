﻿using UnityEngine;
using System;
using System.Collections.Generic; 		
using Random = UnityEngine.Random; 		

public class BoardManager : MonoBehaviour
{
    // Using Serializable allows us to embed a class with sub properties in the inspector.
    [Serializable]
    public class Count
    {
        public int minimum; 			//Minimum value for Count class.
        public int maximum; 			//Maximum
        
        
        //Assignment constructor.
        public Count (int min, int max)
        {
            minimum = min;
            maximum = max;
        }
    }
    
    
    public int columns = 16; 										//Number of columns in game board.
    public int rows = 16;	                                        //Number of rows in game board.
    //16 by 16 game board											
    public Count wallCount = new Count (9, 17);						//Lower and upper limit for random number of walls per level.
    public Count trashCount = new Count (5, 10);						//Lower and upper limit for random number of trash items per level.
    public GameObject exit;											//Prefab to spawn for exit.
    public GameObject[] floorTiles;									//Array of floor prefabs.
    public GameObject[] wallTiles;									//Array of wall prefabs.
    public GameObject[] trashTiles;									//Array of trash prefabs.
    public GameObject[] enemyTiles;									//Array of enemy prefabs.
    public GameObject[] outerWallTiles;								//Array of outer tile prefabs.
    
    private Transform boardHolder;									//to store a reference to the transform of Board object.
    private List <Vector3> gridPositions = new List <Vector3> ();	//A list of possible locations to place tiles.
    

    //Clears list gridPositions and prepares it to generate a new board.
    void InitialiseList ()
    {
        gridPositions.Clear ();
        
        //Loop through x axis (columns).
        for(int x = 1; x < columns-1; x++)
        {
            //Within each column, loop through y axis (rows).
            for(int y = 1; y < rows-1; y++)
            {
                //At each index add a new Vector3 to the list with the x and y coordinates of that position.
                gridPositions.Add (new Vector3(x, y, 0f));
            }
        }
    }
    
    
    //Sets up the outer walls and floor (background) of the game board.
    void BoardSetup ()
    {
        //Instantiate Board and set boardHolder to its transform.
        boardHolder = new GameObject ("Board").transform;
        
        //Loop along x axis, starting from -1 (to fill corner) with floor or outerwall edge tiles.
        for(int x = -1; x < columns + 1; x++)
        {
            //Loop along y axis, starting from -1 to place floor or outerwall tiles.
            for(int y = -1; y < rows + 1; y++)
            {
                //Choose a random tile from array of floor tile prefabs and prepare to instantiate it.
                GameObject toInstantiate = floorTiles[Random.Range (0,floorTiles.Length)];
                
                //Check if we current position is at board edge, if so choose a random outer wall prefab from array of outer wall tiles.
                if(x == -1 || x == columns || y == -1 || y == rows)
                    toInstantiate = outerWallTiles [Random.Range (0, outerWallTiles.Length)];
                
                //Instantiate the GameObject instance using the prefab chosen for toInstantiate at the Vector3 corresponding to current grid position in loop, cast it to GameObject.
                GameObject instance =
                    Instantiate (toInstantiate, new Vector3 (x, y, 0f), Quaternion.identity) as GameObject;
                
                //Set the parent of newly instantiated object instance to boardHolder to avoid cluttering hierarchy.
                instance.transform.SetParent (boardHolder);
            }
        }
    }
    
    
    //returns a random position from list gridPositions.
    Vector3 RandomPosition ()
    {
        //Declare an integer randomIndex, set it's value to a random number between 0 and the count of items in List gridPositions.
        int randomIndex = Random.Range (0, gridPositions.Count);
        
        //Declare a variable of type Vector3 called randomPosition, set it's value to the entry at randomIndex from List gridPositions.
        Vector3 randomPosition = gridPositions[randomIndex];
        
        //Remove the entry at randomIndex from the list so that it can't be re-used.
        gridPositions.RemoveAt (randomIndex);
        
        //Return the randomly selected Vector3 position.
        return randomPosition;
    }
    
    
    //LayoutObjectAtRandom accepts an array of game objects to choose from along with a minimum and maximum range for the number of objects to create.
    void LayoutObjectAtRandom (GameObject[] tileArray, int minimum, int maximum)
    {
        //Choose a random number of objects to instantiate within the minimum and maximum limits
        int objectCount = Random.Range (minimum, maximum+1);
        
        //Instantiate objects until the randomly chosen limit objectCount is reached
        for(int i = 0; i < objectCount; i++)
        {
            //Choose a position for randomPosition by getting a random position from list of available Vector3s stored in gridPosition
            Vector3 randomPosition = RandomPosition();
            
            //Choose a random tile from tileArray and assign it to tileChoice
            GameObject tileChoice = tileArray[Random.Range (0, tileArray.Length)];
            
            //Instantiate tileChoice at the position returned by RandomPosition with no change in rotation
            Instantiate(tileChoice, randomPosition, Quaternion.identity);
        }
    }
    
    
    //SetupScene initializes level and calls the previous functions to lay out the game board
    public void SetupScene (int level)
    {
        //Creates the outer walls and floor.
        BoardSetup ();
        
        //Reset list of gridpositions.
        InitialiseList ();
        
        //Instantiate a random number of wall tiles based on minimum and maximum, at randomized positions.
        LayoutObjectAtRandom (wallTiles, wallCount.minimum, wallCount.maximum);
        
        //Instantiate a random number of trash tiles based on minimum and maximum, at randomized positions.
        LayoutObjectAtRandom (trashTiles, trashCount.minimum, trashCount.maximum);
        
        //Determine number of enemies based on current level number and logarithmic progression + whatever number
        int enemyCount = (int)Mathf.Log(level, 2f) + 3;
        
        //Instantiate a random number of enemies based on minimum and maximum, at randomized positions.
        LayoutObjectAtRandom (enemyTiles, enemyCount, enemyCount);
        
        //Instantiate the exit tile in the upper right hand corner
        Instantiate (exit, new Vector3 (columns - 1, rows - 1, 0f), Quaternion.identity);
    }

    public GameObject PlaceObjectRandom(GameObject prefab) {
        GameObject newObj = Instantiate(prefab, RandomPosition(), Quaternion.identity);
        return newObj;
    }
}
