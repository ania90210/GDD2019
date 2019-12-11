using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Loader : MonoBehaviour
{
    public GameObject gameManager;			
    
    void Awake ()
    {
        //Check if a GameManager1 has already been assigned to static variable GameManager1.instance or if it's still null
        if (GameManager1.instance == null)
            Instantiate(gameManager);
    }
}
