using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Dialogue/New Dialogue", fileName = "dialogues.asset")]

public class DialogueData : ScriptableObject {

    public string id;

    public List<InfoPageData> pages;

}

[System.Serializable]
public class InfoPageData {
    public string id;
    public string header;
    public string body;
}


// Maybe I'll reuse this code to support questions later. Right now it is not necessary.

// [System.Serializable]
// public class QuestionPage : PageData {
//     public string header;
//     public string body;
//     public List<string> option;

// }