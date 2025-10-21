using System;
using FSM;
using Player.States;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class PlayerController : MonoBehaviour
    {
        [DoNotSerialize] public Rigidbody2D RigidBody2D { get; private set; }

        public PlayerInputActions PlayerInputActions;
        private StateMachine<PlayerBaseState> _fsm;

        #region Input Actions

        private InputAction _moveAction;
        private InputAction _jumpAction;

        #endregion

        private float _inputDir = 0;

        private void Awake()
        {
            RigidBody2D = GetComponent<Rigidbody2D>();

            SetupInputs();
            SetupStateMachine();
        }

        private void SetupInputs()
        {
            _moveAction = PlayerInputActions.FindAction("Move");
            _jumpAction = PlayerInputActions.FindAction("Jump");
        }

        private void SetupStateMachine()
        {
            // Set State Machine
            _fsm = new StateMachine<PlayerBaseState>();

            // Declare States
            PlayerBaseState idleState = new IdleState(_fsm, this);
            PlayerBaseState walkState = new WalkState(_fsm, this);

            // Declare Transitions
            _fsm.AddTransition(idleState, walkState, new FuncPredicate(() => _inputDir != 0));
            _fsm.AddTransition(walkState, idleState, new FuncPredicate(() => _inputDir == 0));

            _fsm.SetState(idleState);
        }

        private void Update()
        {
            _inputDir = _moveAction.ReadValue<float>();
        }

        private void OnEnable()
        {
            PlayerInputActions.Enable();
        }

        private void OnDisable()
        {
            PlayerInputActions.Disable();
        }
    }
}