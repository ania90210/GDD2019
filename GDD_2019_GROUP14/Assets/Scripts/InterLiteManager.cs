using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InterLiteManager : MonoBehaviour
{

    [SerializeField] TextMeshProUGUI scoreLabel = null;

    private void Awake()
    {
        int score = PlayerPrefs.GetInt(GameUtility.SavePrefKeyGameScore);
        scoreLabel.text = "Score: " + score;
    }



}
