using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New GameEvents", menuName = "Quiz/New GameEvents")]
public class GameEvents : ScriptableObject
{
    public delegate void updateQuestionUICallback(Question question);
    public updateQuestionUICallback UpdateQuestionUI;

    public delegate void UpdateQuestionAnswerCallback(AnswerData pickedAnswer);
    public UpdateQuestionAnswerCallback UpdateQuestionAnswer;

    public delegate void DisplayResolutionScreenCallback(UIManager.ResolutionScreenType type, int score);
    public DisplayResolutionScreenCallback DisplayResolutionScreen;

    public delegate void ScoreUpdateCallback();
    public ScoreUpdateCallback ScoreUpdated;


    public int StartupHighScore;

    public int CurrentFinalScore;
}
