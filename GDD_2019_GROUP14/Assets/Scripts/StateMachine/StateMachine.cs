using UnityEngine;

public class StateMachine : MonoBehaviour {

    [SerializeField] string currentLabel = "";

    IState current;
    IState previous;

    public void ChangeState(IState newState) {
        if (current != null) {
            current.Exit();
        }

        previous = current;
        current = newState;

        if (current != null) {
            current.Enter();
            if (current.label != null) currentLabel = current.label;
        }
    }

    public void ExecuteStateUpdate() {
        var runningState = current;
        if (runningState != null) {
            runningState.Execute();
        }
    }

    public void SwitchToPreviousState() {
        if (current != null) current.Exit();
        current = previous;
        if (current != null) current.Enter();
    }

    public IState CurrentState() {
        return current;
    }
}
