using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueSystem : MonoBehaviour
{
    private static DialogueSystem _instance;
    public static DialogueSystem instance {
        get {
            return _instance;
        }
    }
    public Canvas canvasPrefab;
    public Canvas canvas;
    public DialogueView dialoguePrefab;
    public PageView pagePrefab;

    public List<DialogueData> dialogues;

    public Dictionary<string, DialogueData> dialogueLookup = new Dictionary<string, DialogueData>();

    public Dialogue currentDialogue;

    /// <summary>
    /// Seconds to wait before beginning the dialogue
    /// </summary>
    public float startDelay;

    void Start() {
        if (_instance == null) _instance = this;

        // Add dialogues to the dictionary
        foreach (DialogueData dialogue in dialogues) {
            dialogueLookup.Add(dialogue.id, dialogue);
        }
    }

    public void BeginDialogue(string id) {
        DialogueData dialogueData = dialogueLookup[id];
        if (dialogueData == null) {
            Debug.LogError("The requested dialogue, '" + id + "'  doesn't exist.");
        }

        canvas = Instantiate(canvasPrefab);

        // Create a new dialogue object and pass in the data
        currentDialogue = new Dialogue(dialogueData);
        InfoPageData page = currentDialogue.Begin();

        // Update GUI
        currentDialogue.dialogueView = Instantiate(dialoguePrefab, canvas.transform);

        // 12/13/2019 TODO this is not working currently. I can't move to the next page
        currentDialogue.dialogueView.nextPage.onClick.AddListener( delegate {
            Debug.Log("next page is called");
            NextPage(currentDialogue);
        });


        currentDialogue.pageView = Instantiate(pagePrefab, currentDialogue.dialogueView.panel.transform);
        // currentDialogue.pageView.SetHeader(page.header);
        currentDialogue.pageView.SetBody(page.body);

//        StartCoroutine(IEBeginDialogue(id));
    }

    IEnumerator IEBeginDialogue(string id) {
        DialogueData dialogueData = dialogueLookup[id];
        if (dialogueData == null) {
            Debug.LogError("The requested dialogue, '" + id + "'  doesn't exist.");
        }

        yield return new WaitForSeconds(startDelay);

        canvas = Instantiate(canvasPrefab);

        // Create a new dialogue object and pass in the data
        currentDialogue = new Dialogue(dialogueData);
        InfoPageData page = currentDialogue.Begin();

        // Update GUI
        currentDialogue.dialogueView = Instantiate(dialoguePrefab, canvas.transform);

        // 12/13/2019 TODO this is not working currently. I can't move to the next page
        currentDialogue.dialogueView.nextPage.onClick.AddListener( delegate {
            Debug.Log("next page is called");
            NextPage(currentDialogue);
        });


        currentDialogue.pageView = Instantiate(pagePrefab, currentDialogue.dialogueView.panel.transform);
        // currentDialogue.pageView.SetHeader(page.header);
        currentDialogue.pageView.SetBody(page.body); 
    }

    public void NextPage(Dialogue dialogue) {
        if (dialogue == null) return;

        // Get the next page data
        InfoPageData page = dialogue.NextPage();
        if (page == null) {
            CloseDialogue();
        };

        // Update GUI
        Destroy(dialogue.pageView.gameObject);
        dialogue.pageView = Instantiate(pagePrefab, dialogue.dialogueView.panel.transform);
        // dialogue.pageView.SetHeader(page.header);
        dialogue.pageView.SetBody(page.body); 
    }

    public void CloseDialogue() {
        if (currentDialogue == null) return;

        Destroy(currentDialogue.pageView.gameObject);
        Destroy(currentDialogue.dialogueView.gameObject);

        currentDialogue = null;
        Destroy(canvas.gameObject);

        
    }
}

[System.Serializable]
public class Dialogue {

    public DialogueData data;
    
    private int pageIndex;
    public InfoPageData currentPage;

    public bool reachedEnd;

    public DialogueView dialogueView;
    public PageView pageView;

    public Dialogue(DialogueData data) {
        this.data = data;
    }

    public InfoPageData Begin() {
        if (data.pages.Count <= 0) {
            Debug.LogError("There are no pages in this dialogue");
            return null;
        }

        pageIndex = 0;
        currentPage = data.pages[pageIndex];
        return currentPage;
    }

    public InfoPageData NextPage() {
        pageIndex++;
        if (pageIndex >= data.pages.Count) {
            Debug.Log("Reached the end of dialogue, '" + data.id + "'");
            reachedEnd = true;
            return null;
        }

        currentPage = data.pages[pageIndex];
        return currentPage;
    } 
}

