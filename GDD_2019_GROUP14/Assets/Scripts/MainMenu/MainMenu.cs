using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void PlayGame()
    {
        // Pilot scene is indexed at 0
        SceneManager.LoadScene("Pilot");
    }

    public void ExitGame()
    {
        Debug.Log("QUIT");
        Application.Quit();
    }

    public void LoadQuizFromInterScene()
    {
        SceneManager.LoadScene("QuizScene");
    }

}
