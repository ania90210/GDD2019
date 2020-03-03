using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using TMPro;
using UnityEngine.SceneManagement;

public class QuizManager : MonoBehaviour
{
    Question[] _questions = null;
    public Question[] GetQuestions { get { return _questions; } }

    [SerializeField] GameEvents events = null;

    [SerializeField] Animator timerAnimator = null;
    [SerializeField] TextMeshProUGUI timerText = null;
    [SerializeField] Color timerHalfTimeCol = Color.yellow;
    [SerializeField] Color timerAlmostOutCol = Color.red;
    private List<AnswerData> PickedAnswers = new List<AnswerData>();
    private List<int> FinishedQuestions = new List<int>();
    private int currentQuestion = 0;
    private Color timerDefaultCol;


    private int timerStateHash = 0;

    private bool IsFinished
    {
        get
        {
            return !(FinishedQuestions.Count < GetQuestions.Length);
        }
    }

    private IEnumerator IE_WaitTillNext = null;
    private IEnumerator IE_StartTimer = null;

    private void Awake()
    {
        events.CurrentFinalScore = 0;
    }

    void Start()
    {
        events.StartupHighScore = PlayerPrefs.GetInt(GameUtility.SavePrefKey);

        timerStateHash = Animator.StringToHash("TimerState");

        timerDefaultCol = timerText.color;
        int seed = Random.Range(int.MinValue, int.MaxValue);
        Random.InitState(seed);
        
        LoadQuestions();
        foreach(Question question in GetQuestions)
        {
            Debug.Log(question.Info);
        }
        Display();
    }

    private void OnEnable()
    {
        events.UpdateQuestionAnswer += UpdateAnswers;
    }

    private void OnDisable()
    {
        events.UpdateQuestionAnswer -= UpdateAnswers;
    }

    public void UpdateAnswers(AnswerData newAnswer)
    {

        if(GetQuestions[currentQuestion].GetAnswerType == Question.AnswerType.Single)
        {
            foreach(var answer in PickedAnswers)
            {
                if(answer != newAnswer)
                {
                    answer.Reset();
                }
            }
            PickedAnswers.Clear();
            PickedAnswers.Add(newAnswer);
        }
        else
        {
            bool alreadyPicked = PickedAnswers.Exists(x => x == newAnswer);
            if (alreadyPicked)
            {
                PickedAnswers.Remove(newAnswer);
            }
            else
            {
                PickedAnswers.Add(newAnswer);
            }
        }


    }


    public void EraseAnswers()
    {
        PickedAnswers = new List<AnswerData>();
    }


    void Display()
    {
        EraseAnswers();
        Question question = GetRandomQuestion();
        if(events.UpdateQuestionUI != null)
        {
            events.UpdateQuestionUI(question);
        }
        else
        {
            Debug.LogWarning("Something went wrong in Display Method with the events.UpdateQuestionUI being null!");
        }

        if (question.UseTimer)
        {
            UpdateTimer(question.UseTimer);
        }

    }

    public void Accept()
    {
        UpdateTimer(false);
        bool isCorrect = CheckAnswers();
        FinishedQuestions.Add(currentQuestion);



        UpdateScore((isCorrect) ? GetQuestions[currentQuestion].AddScore : -GetQuestions[currentQuestion].AddScore);
        if (IsFinished)
        {
            SetHighscore();
        }

        var type = (IsFinished) ? UIManager.ResolutionScreenType.Finished : (isCorrect) ? UIManager.ResolutionScreenType.Correct : UIManager.ResolutionScreenType.Incorrect;


        events.DisplayResolutionScreen?.Invoke(type, GetQuestions[currentQuestion].AddScore);


        if (IE_WaitTillNext != null)
        {
            StopCoroutine(IE_WaitTillNext);
        }
        IE_WaitTillNext = WaitTillNextRound();
        StartCoroutine(IE_WaitTillNext);
    }

    IEnumerator WaitTillNextRound()
    {
        yield return new WaitForSeconds(GameUtility.resolutionDelay);
        Display();
    }


    bool CheckAnswers()
    {
        return CompareAnswers();
    }

    bool CompareAnswers()
    {
        if(PickedAnswers.Count > 0)
        {
            List<int> c = GetQuestions[currentQuestion].GetCorrectAnswers();
            List<int> p = PickedAnswers.Select(x => x.AnswerIndex).ToList();
            var f = c.Except(p).ToList();
            var s = p.Except(c).ToList();
            return !f.Any() && !s.Any();
        }
        return false;
    }

    Question GetRandomQuestion()
    {
        int randomIndex = GetRandomQuestionIndex();
        currentQuestion = randomIndex;
        return GetQuestions[currentQuestion];
    }

    int GetRandomQuestionIndex()
    {
        int random = 0;
        if(FinishedQuestions.Count < GetQuestions.Length)
        {
            do
            {
                random = UnityEngine.Random.Range(0, GetQuestions.Length);
            } while (FinishedQuestions.Contains(random) || random == currentQuestion);
        }
        return random;
    }


    void LoadQuestions()
    {
        Object[] objs = Resources.LoadAll("Questions", typeof(Question));
        _questions = new Question[objs.Length];
        for(int i = 0; i < objs.Length; i++)
        {
            _questions[i] = (Question)objs[i];
        }
    }

    void UpdateTimer(bool state)
    {
        switch (state)
        {
            case true:
                IE_StartTimer = StartTimer();
                StartCoroutine(IE_StartTimer);
                timerAnimator.SetInteger(timerStateHash, 2);
                break;
            case false:
                if(IE_StartTimer != null)
                {
                    StopCoroutine(IE_StartTimer);
                }
                timerAnimator.SetInteger(timerStateHash, 1);
                break;
        }
    }

    IEnumerator StartTimer()
    {
        var totalTime = GetQuestions[currentQuestion].Timer;
        var timeLeft = totalTime;


        timerText.color = timerDefaultCol;
        while (timeLeft > 0)
        {
            timeLeft--;

            if(timeLeft < totalTime / 2 && timeLeft >= totalTime / 4)
            {
                timerText.color = timerHalfTimeCol;
            }
            if(timeLeft < totalTime / 4)
            {
                timerText.color = timerAlmostOutCol;
            }

            timerText.text = timeLeft.ToString();
            yield return new WaitForSeconds(1.0f);
        }
        // ran out of time
        Accept();
    }


    private void UpdateScore(int addScore)
    {
        events.CurrentFinalScore += addScore;
        events.ScoreUpdated?.Invoke();
    }

    private void SetHighscore()
    {
        var highscore = PlayerPrefs.GetInt(GameUtility.SavePrefKey);
        if(highscore < events.CurrentFinalScore)
        {
            PlayerPrefs.SetInt(GameUtility.SavePrefKey, events.CurrentFinalScore);
        }
    }

    public void RestartGame()
    {
        // this button is actually for the MENU button to go to the MENU ! customize it!
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    // this is for quitting
    public void QuitGame()
    {
        Application.Quit();
    }

}
