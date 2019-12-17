using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CustomState : IState
{
    public string label {get; set;}
    public bool executeOnce {get; set;}
    public bool executed {get; set;}
    System.Action enterCallback;
    System.Action exitCallback;
    System.Action executeCallback;

    public CustomState( string label, bool executeOnce, 
                        System.Action enterCallback, 
                        System.Action exitCallback, 
                        System.Action executeCallback) {
        this.label = label;
        this.executeOnce = executeOnce;
        executed = false;
        
        this.enterCallback = enterCallback;
        this.exitCallback = exitCallback;
        this.executeCallback = executeCallback;
    }

    public void Enter() {
        if (enterCallback != null) enterCallback();
    }
    public void Execute() {
        // if the state is to be executed only once and it already has, then return
        if (executeOnce && executed) return;

        if (executeCallback != null) executeCallback();
    }
    public void Exit() {
        if (exitCallback != null) exitCallback();
    }
}
