using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class WaitForInput : IState
{
    public string label {get; set;}
    public bool executeOnce {get; set;}
    public bool executed {get; set;}


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
        // if the state is to be executed only once and it already has, then return
        if (executeOnce && executed) return;

        if (Input.GetKey(inputBtn)) {
            onInputTrigger();
            executed = true;
        }
    }

    public void Exit() {
        // on exit, reset the executed flag in case we reuse this instance of the state
        executed = false;
    }
}
