using UnityEngine;

public interface IState {
    string label {get; set;}
    bool executeOnce {get; set;}
    bool executed {get; set;}
    void Enter();
    void Execute();
    void Exit();
}