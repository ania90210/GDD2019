using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class WaitForInput : IState
{
    public string label {get; set;}
    public bool executeOnce {get; set;}


    /// <summary>
    /// The input button to listen to to trigger the event
    /// </summary>
    KeyCode inputBtn;
    event Action onInputTrigger;

    public WaitForInput(string label, bool executeOnce, KeyCode inputBtn, Action onPostCollect) {
        this.inputBtn = inputBtn;
        this.onInputTrigger += onPostCollect;

        this.label = label;
        this.executeOnce = executeOnce;
    }

    public void Enter() {
        
    }

    public void Execute() {
        if (Input.GetKey(inputBtn)) {
            executeOnce = true;
            onInputTrigger();
        }
    }

    public void Exit() {
        executeOnce = false;
    }
}
