using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;
using UnityEngine.UI;

[Serializable()]
public struct UIManaferParamaters
{
    [Header("Answers Options")]
    [SerializeField] float margins;
    public float Margins { get { return margins; } }

    [Header("Resolution screen options")]
    [SerializeField] Color correctBG;
    public Color GetCorrectBG { get { return correctBG; } }

    [SerializeField] Color incorrectBG;
    public Color GetIncorrectBG { get { return incorrectBG; } }

    [SerializeField] Color finalBG;
    public Color GetfinalBG { get { return finalBG; } }

}


[Serializable()]
public struct UIElements
{
    [SerializeField] RectTransform answerContentArea;
    public RectTransform AnswerContentArea { get { return answerContentArea; } }

    [SerializeField] TextMeshProUGUI questionInfoTextObject;
    public TextMeshProUGUI QuestionInfoTextObject { get { return questionInfoTextObject; } }

    [SerializeField] TextMeshProUGUI scoreText;
    public TextMeshProUGUI ScoreText { get { return scoreText; } }
    [Space]
    [SerializeField] Image resolutionBG;
    public Image ResolutionBG { get { return resolutionBG; } }
    
    [SerializeField] TextMeshProUGUI resolutionTextInfo;
    public TextMeshProUGUI ResolutionTextInfo { get { return resolutionTextInfo; } }

    [SerializeField] TextMeshProUGUI resolutionScoreText;
    public TextMeshProUGUI ResolutionScoreText { get { return resolutionScoreText; } }


    [SerializeField] Animator resolutionAnimator;
    public Animator ResolutionAnimator { get { return resolutionAnimator; } }


    [Space]
    [SerializeField] TextMeshProUGUI highscoreText;
    public TextMeshProUGUI HighscoreText { get { return highscoreText; } }

    [SerializeField] CanvasGroup mainCanvasGroup;
    public CanvasGroup MainCanvasGroup { get { return mainCanvasGroup; } }

    [SerializeField] RectTransform finishUIElements;
    public RectTransform FinishUIElements { get { return finishUIElements; } }

}

public class UIManager : MonoBehaviour
{
    public enum ResolutionScreenType { Correct, Incorrect, Finished }

    [Header("Refrences")]
    [SerializeField] GameEvents events;

    [Header("UI Element (Prefabs)")]
    [SerializeField] AnswerData answerPrefab;

    [SerializeField] UIElements uIElements;
    [Space]
    [SerializeField] UIManaferParamaters parameters;


    private int resStateHash = 0;
    private IEnumerator IE_DisplayTimedRes = null;
    List<AnswerData> currentAnswer = new List<AnswerData>();



    void Start()
    {
        resStateHash = Animator.StringToHash("ScreenState");
        UpdateScoreUI();
    }

    void OnEnable()
    {
        events.UpdateQuestionUI += UpdateQuestionUI;
        events.DisplayResolutionScreen += DisplayResolution;
        events.ScoreUpdated += UpdateScoreUI;
    }

    void OnDisable()
    {
        events.UpdateQuestionUI -= UpdateQuestionUI;
        events.DisplayResolutionScreen -= DisplayResolution;
        events.ScoreUpdated -= UpdateScoreUI;
    }

    void DisplayResolution(ResolutionScreenType type, int score)
    {
        UpdateResolutionUI(type, score);
        uIElements.ResolutionAnimator.SetInteger(resStateHash, 2);
        uIElements.MainCanvasGroup.blocksRaycasts = false;

        if(type != ResolutionScreenType.Finished)
        {
            if(IE_DisplayTimedRes != null)
            {
                StopCoroutine(IE_DisplayTimedRes);
            }
            IE_DisplayTimedRes = DisplayTimedRes();
            StartCoroutine(IE_DisplayTimedRes);
        }

    }

    IEnumerator DisplayTimedRes()
    {
        yield return new WaitForSeconds(GameUtility.resolutionDelay);
        uIElements.ResolutionAnimator.SetInteger(resStateHash, 1);
        uIElements.MainCanvasGroup.blocksRaycasts = true;
    }

    void UpdateResolutionUI(ResolutionScreenType type, int score)
    {
        var highscore = PlayerPrefs.GetInt(GameUtility.SavePrefKey);

        Debug.Log("UI type " + type);
        switch (type)
        {
            case ResolutionScreenType.Correct:
                uIElements.ResolutionBG.color = parameters.GetCorrectBG;
                uIElements.ResolutionTextInfo.text = "Correct";
                uIElements.ResolutionScoreText.text = "+" + score;
                break;
            case ResolutionScreenType.Incorrect:
                uIElements.ResolutionBG.color = parameters.GetIncorrectBG;
                uIElements.ResolutionTextInfo.text = "Incorrect";
                uIElements.ResolutionScoreText.text = "-" + score;
                break;
            case ResolutionScreenType.Finished:
                uIElements.ResolutionBG.color = parameters.GetfinalBG;
                uIElements.ResolutionTextInfo.text = "Final score";
                StartCoroutine("CalculateScore");
                uIElements.FinishUIElements.gameObject.SetActive(true);
                uIElements.HighscoreText.gameObject.SetActive(true);
                //display the highscore
                uIElements.HighscoreText.text = (highscore > events.StartupHighScore ? "<color=yellow> new </color>" : String.Empty) + "Highscore: " + highscore; 
                break;
        }
    }

    IEnumerator CalculateScore()
    {
        /*var scoreValue = 0;
        Debug.Log(events.CurrentFinalScore);
        while(scoreValue < events.CurrentFinalScore)
        {
            Debug.Log(scoreValue);
            scoreValue++;

        }*/
        uIElements.ResolutionScoreText.text = events.CurrentFinalScore.ToString();
        yield return null;
    }

    void UpdateQuestionUI(Question question)
    {
        uIElements.QuestionInfoTextObject.text = question.Info;
        // create answers ...
        CreateAnswers(question);
    }

    void CreateAnswers (Question question)
    {
        EraseAnswers();
        float offset = 0 - parameters.Margins;
        for(int i = 0; i < question.Answers.Length; i++)
        {
            AnswerData newAnswer = (AnswerData)Instantiate(answerPrefab, uIElements.AnswerContentArea);
            newAnswer.UpdateData(question.Answers[i].Info, i);

            newAnswer.Rect.anchoredPosition = new Vector2(0, offset);
            offset -= (newAnswer.Rect.sizeDelta.y + parameters.Margins);
            uIElements.AnswerContentArea.sizeDelta = new Vector2(uIElements.AnswerContentArea.sizeDelta.x, -offset);

            currentAnswer.Add(newAnswer);
        }
    }

    void EraseAnswers()
    {
        foreach(AnswerData answer in currentAnswer)
        {
            Destroy(answer.gameObject);
        }
        currentAnswer.Clear();
    }

    void UpdateScoreUI()
    {
        uIElements.ScoreText.text = "Score: " + events.CurrentFinalScore;
    }

}


