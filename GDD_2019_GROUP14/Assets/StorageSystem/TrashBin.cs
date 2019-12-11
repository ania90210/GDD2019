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

    /// <summary>
    /// This event is triggered when the maxAmt of trash is reached
    /// </summary>
    public UnityEvent OnFilled;
    /// <summary>
    /// Whether the OnFilled event was called yet or not
    /// </summary>
    private bool isDone_OnFilled;
    
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

        // return the amount of leftovers if there are some
        return leftover;
    }
}
