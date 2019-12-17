using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// The TrashBin object is where the player can toss away trash collected from their PlayerStorage
/// </summary>
public class TrashBin : MonoBehaviour
{

    public int currentAmt;
    public int maxAmt = 5;

    public SpriteRenderer capacityIndicator;

    public UnityEvent OnReceivedTrash;

    /// <summary>
    /// This event is triggered when the maxAmt of trash is reached
    /// </summary>
    public UnityEvent OnFilled;
    /// <summary>
    /// Whether the OnFilled event was called yet or not
    /// </summary>
    private bool isDone_OnFilled;

    public StateMachine stateMachine;
    public CustomState level0_state;
    public CustomState level1_state;
    public CustomState level2_state;
    

    void Start() {
        OnReceivedTrash.AddListener(UpgradeTrashBin);

        // Throughout its lifetime, the trashbin will go through 3 states
        // Each state has an Enter (which triggers when first entering the state),
        //   an Execute (which triggers at every frame update)
        //   and an Exit (which triggeres when leaving the state)
        // Delegates are used to write functions within functions
        level0_state = new CustomState("Level0", true,
            // enter callback
            delegate {
                capacityIndicator.color = Color.red;
            },
            // execute callback
            null,
            // exit callback
            null
        );

        level1_state = new CustomState("Level1", true,
            delegate {
                capacityIndicator.color = Color.yellow;
                SoundManager.PlaySound(SoundManager.Sound.TrashUpgrade1);
            },
            null,
            null
        );

        level2_state = new CustomState("Level2", true,
            delegate {
                capacityIndicator.color = Color.green;
                SoundManager.PlaySound(SoundManager.Sound.TrashUpdrage2);
            },
            null,
            null
        );

        // At the start, the trash bin is at level0
        stateMachine = gameObject.AddComponent<StateMachine>();
        stateMachine.ChangeState(level0_state);
    }

    void Update() {
        stateMachine.ExecuteStateUpdate();
    }

    public void UpgradeTrashBin() {
        if (currentAmt <= 0) {
            stateMachine.ChangeState(level0_state);
        }
        else if (currentAmt >= maxAmt) {
            stateMachine.ChangeState(level2_state);
        } else {
            stateMachine.ChangeState(level1_state);
        }
    }

    /// <summary>
    /// This function tries to throw trash into the bin
    /// </summary>
    /// <param name="amt"></param>
    /// <returns></returns>
    public int TryThrowInto(int amt) {
        if (isDone_OnFilled) return 0;
        
        // Add the amount of trash we are trying to throw into the trash bin's current amount of trash
        currentAmt += amt;

        // If the trash bin is overflowed, then give the remaining trash back to the player
        int leftover = 0;
        if (currentAmt > maxAmt) {
            leftover = currentAmt - maxAmt; // get the leftover amount
            currentAmt -=leftover;          // remove the leftover trash from this bin
        }

        // check if the bin is full and call the OnFilled event
        if (currentAmt == maxAmt) {
            if (OnFilled != null) OnFilled.Invoke();
        }

        // Call any other events when a trash is received
        if (OnReceivedTrash != null) OnReceivedTrash.Invoke();

        // return the amount of leftovers if there are some
        return leftover;
    }
}
