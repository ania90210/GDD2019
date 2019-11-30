using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueSystem : MonoBehaviour
{

    public Canvas canvas;
    public DialogueView dialoguePrefab;
    public PageView pagePrefab;

    public List<DialogueData> dialogues;

    public Dictionary<string, DialogueData> dialogueLookup = new Dictionary<string, DialogueData>();

    public Dialogue currentDialogue;

    void Start() {

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

        // Create a new dialogue object and pass in the data
        currentDialogue = new Dialogue(dialogueData);
        PageData page = currentDialogue.Begin();

        // Update GUI
        currentDialogue.dialogueView = Instantiate(dialoguePrefab, canvas.transform);
        currentDialogue.dialogueView.nextPage.onClick.AddListener( delegate {
            NextPage(currentDialogue);
        });

        if (page is InfoPage) {

        }
        currentDialogue.pageView = Instantiate(pagePrefab, currentDialogue.dialogueView.panel.transform);
        // currentDialogue.pageView.SetHeader(page.header);
        // currentDialogue.pageView.SetBody(page.body); 
    }

    public void NextPage(Dialogue dialogue) {
        if (dialogue == null) return;

        // Get the next page data
        PageData page = dialogue.NextPage();
        if (page == null) {
            CloseDialogue();
        };

        // Update GUI
        Destroy(dialogue.pageView.gameObject);
        dialogue.pageView = Instantiate(pagePrefab, dialogue.dialogueView.panel.transform);
        // dialogue.pageView.SetHeader(page.header);
        // dialogue.pageView.SetBody(page.body); 
    }

    public void CloseDialogue() {
        if (currentDialogue == null) return;

        Destroy(currentDialogue.pageView.gameObject);
        Destroy(currentDialogue.dialogueView.gameObject);

        currentDialogue = null;
    }
}

[System.Serializable]
public class Dialogue {

    public DialogueData data;
    
    private int pageIndex;
    public PageData currentPage;

    public bool reachedEnd;

    public DialogueView dialogueView;
    public PageView pageView;

    public Dialogue(DialogueData data) {
        this.data = data;
    }

    public PageData Begin() {
        if (data.pages.Count <= 0) {
            Debug.LogError("There are no pages in this dialogue");
            return null;
        }

        pageIndex = 0;
        currentPage = data.pages[pageIndex];
        return currentPage;
    }

    public PageData NextPage() {
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

