using UnityEngine;

public class PlayerController : MonoBehaviour
{
    StateMachine<PlayerBaseState> FSM;

    private void Awake()
    {
        SetupStateMachine();
    }

    private void SetupStateMachine()
    {
        // Set State Machine
        FSM = new StateMachine<PlayerBaseState>();

        // Declare States

        // Declare Transitions
    }
}