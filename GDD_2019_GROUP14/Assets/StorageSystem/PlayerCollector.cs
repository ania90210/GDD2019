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
        DontDestroyOnLoad(this.gameObject);
    }

    void Start() {
        stateMachine = new StateMachine();
    }

    void Update() {
        stateMachine.ExecuteStateUpdate();

    }

    void hello() {}

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
    }

    void OnTriggerExit2D(Collider2D collider)
    {
        stateMachine.ChangeState(null);
    }

    void OnColliderEnter2D(Collision collisionInfo) {
        
        // If collided with a trash bin, attempt to toss out the trash
        TrashBin trashBin = collisionInfo.gameObject.GetComponent<TrashBin>();
        if (trashBin != null) {
            // Get the total trash
            int trashCnt = GameManager1.instance.storage.trashCollected;
            
            // throw into bin and store leftover trash back in storage
            stateMachine.ChangeState(new WaitForInput("Throw Trash", true, inputBtn, delegate {
                GameManager1.instance.storage.trashCollected = trashBin.TryThrowInto(trashCnt);
            }));            
        }
    }

}