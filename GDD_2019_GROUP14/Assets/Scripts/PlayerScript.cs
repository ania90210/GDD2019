using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class PlayerScript : MonoBehaviour
{

    private static int lastLevel = 18;
    public static int lvlId;
    private int score;

    private void Awake()
    {
        score = SceneManager.GetActiveScene().buildIndex == 2 ? 0 : PlayerPrefs.GetInt(GameUtility.SavePrefKeyGameScore);
        Debug.Log("Awake");
        Debug.Log(this.name);

    }
    // Start is called before the first frame update
    void Start()
    {

        Debug.Log("START");
        Debug.Log(this.name);
        lvlId = SceneManager.GetActiveScene().buildIndex;
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(GameManager1.instance.storage.artifacts.Count);
        //Debug.Log("trash, " + GameManager1.instance.storage.trashCollected);
        //Debug.Log(GameManager1.instance.artefactPickedUp);
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("LEVEL ID " + lvlId);
        if(collision.gameObject.tag == "Exit" && lvlId != lastLevel)
        {
            Debug.Log("EXIT to new level");
            //SceneManager.LoadScene("InterGameQuizScene", LoadSceneMode.Single);
            SceneManager.LoadScene(lvlId+1, LoadSceneMode.Single);
            // you reached the end of the level, run the end level scene with positive message, You successfully completed the level
        }else if (collision.gameObject.tag == "Exit" && lvlId == lastLevel)
        {
            Debug.Log("EXIT TO QUIZ");
            SceneManager.LoadScene("InterGameQuizScene", LoadSceneMode.Single);
        }
    }

    private void OnDestroy()
    {
        Debug.Log(GameManager1.instance.artefactPickedUp);
        score += GameManager1.instance.artefactPickedUp * 5 + GameManager1.instance.storage.trashCollected;
        PlayerPrefs.SetInt(GameUtility.SavePrefKeyGameScore, score);
        Debug.Log("current score: " + score);
        Debug.Log("DEATH");
        //SceneManager.LoadScene("InterGameQuizScene", LoadSceneMode.Single); // this should be moved to some manager
        // you were killed
        // run end level scene with a "negative" message, like, You've been killed
    }

}
