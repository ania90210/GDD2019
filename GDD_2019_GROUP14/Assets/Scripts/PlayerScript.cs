using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class PlayerScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Exit")
        {
            Debug.Log("EXIT");
            SceneManager.LoadScene("Name", LoadSceneMode.Single);
            // you reached the end of the level, run the end level scene with positive message, You successfully completed the level
        }
    }

    private void OnDestroy()
    {
        Debug.Log("DEATH");
        SceneManager.LoadScene("Name", LoadSceneMode.Single); // this should be moved to some manager
        // you were killed
        // run end level scene with a "negative" message, like, You've been killed
    }

}
