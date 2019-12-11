using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class gives the player functionality to collect the trash.
/// It requires a GameManager1.cs in the scene.
/// </summary>
public class PlayerCollector : MonoBehaviour {

    /// <summary>
    /// The input button to collect trash
    /// </summary>
    public KeyCode inputBtn;
    public string tagToCollect;

    public StateMachine stateMachine;

    public void CollectTrash(GameObject trash) {
        if (trash == null) {
            Debug.Log("Attempted to collect trash... nothing collected");
            return;
        }

        GameManager1.instance.storage.trashCollected += 1;
        Destroy(trash);
    }

    void Awake() {
        stateMachine = gameObject.AddComponent<StateMachine>();
        DontDestroyOnLoad(this.gameObject);
    }

    void Update() {
        stateMachine.ExecuteStateUpdate();

    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        // if you collide with an object that matches the tag to collect,
        //  then listen for the input button to collect the trash
        if (collider.gameObject.tag == tagToCollect) {
            Debug.Log("Reached trash!");
            stateMachine.ChangeState(new WaitForInput("Collect", true, inputBtn, delegate {
                CollectTrash(collider.gameObject);
            }));
        }

        // If collided with a trash bin, attempt to toss out the trash
        TrashBin trashBin = collider.gameObject.GetComponent<TrashBin>();
        if (trashBin != null) {
            // Get the total trash
            int trashCnt = GameManager1.instance.storage.trashCollected;
            
            // Wait for input for this event
            stateMachine.ChangeState(new WaitForInput("Throw Trash", true, inputBtn, delegate {
                // throw into bin and store leftover trash back in storage
                GameManager1.instance.storage.trashCollected = trashBin.TryThrowInto(trashCnt);
            }));            
        }
    }

    void OnTriggerExit2D(Collider2D collider)
    {
        stateMachine.ChangeState(null);
    }

}