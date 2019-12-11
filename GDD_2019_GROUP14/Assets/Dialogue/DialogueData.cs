using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Dialogue/New Dialogue", fileName = "dialogues.asset")]

public class DialogueData : ScriptableObject {

    public string id;

    public List<PageData> pages;

}

public enum PageType {
    Info, Question 
}

[System.Serializable]
public class PageData {
    public PageType pageType;

    public InfoPage infoPage;
    public QuestionPage questionPage;

}

[System.Serializable]
public class InfoPage : PageData {
    public string header;
    public string body;
}

[System.Serializable]
public class QuestionPage : PageData {
    public string header;
    public string body;
    public List<string> option;

}