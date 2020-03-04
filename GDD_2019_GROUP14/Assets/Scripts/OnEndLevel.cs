using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OnEndLevel : MonoBehaviour
{


    void OpenEndScene(string endMessage)
    {
        // opens end scene with given message

        SceneManager.LoadScene("Scenes/EndLevelScene");
    }


    public void RestartLevel()
    {
        // restarts level
        SceneManager.LoadScene(2);
    }

    public void ContinueLevel()
    {
        // goes on, for now restarts level
        RestartLevel();
    }

    public void ExitGame()
    {
        // closes the game
        Debug.Log("Exit");
        Application.Quit(0);
    }


}
