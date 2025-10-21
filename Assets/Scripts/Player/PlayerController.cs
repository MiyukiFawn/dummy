using System;
using FSM;
using Player.States;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class PlayerController : MonoBehaviour
    {
        public bool _drawDebugLines = false;

        public InputActionAsset InputActions;

        [Header("Movement")]
        public float WalkSpeed = 10;

        [Header("Jump")]
        [Min(1)]          public int   MaxJumpCount = 1;
        [Min(1)]          public float JumpHeight = 1;
        [Min(0.1f)]       public float TimeToPeak = 1;
        [Min(0.1f)]       public float TimeToFall = 1;
        [Min(0)]          public float MaxFallSpeed = 25;
        [Min(1)]          public float ReleaseJumpSpeedMultiplyer = 1;
        [Range(0.01f, 1)] public float CoyoteTime = 0.2f;
        [Range(0.01f, 1)] public float JumpBuffer = 0.2f;

        [Header("Ground detection")]
        [SerializeField] private LayerMask _groundLayer;
        [SerializeField][Range(2, 14)] private int nOfRaysVertical = 2;
        [SerializeField][Range(0.01f, 0.2f)] private float _distanceFromFloor = 0.1f;
        [SerializeField][Range(0, 0.5f)] private float _fRaysTreshold;
        [SerializeField][Range(0.1f, 1)] private float _fRayLenght = 0.5f;

        [DoNotSerialize] public Rigidbody2D RigidBody2D { get; private set; }

        private StateMachine<PlayerBaseState> _fsm;

        #region Input Actions

        private InputAction _moveAction;
        private InputAction _jumpAction;

        #endregion

        [DoNotSerialize] public float inputDir = 0;

        private CapsuleCollider2D _collider;

        [DoNotSerialize] public bool IsOnGround;

        [DoNotSerialize] public float JumpVelocity { get; private set; }
        [DoNotSerialize] public float JumpGravity { get; private set; }
        [DoNotSerialize] public float FallGravity { get; private set; }

        private void Awake()
        {
            RigidBody2D = GetComponent<Rigidbody2D>();

            SetupInputs();
            SetupStateMachine();
        }

        private void SetupInputs()
        {
            _moveAction = InputActions.FindAction("Move");
            _jumpAction = InputActions.FindAction("Jump");
        }

        private void SetupStateMachine()
        {
            // Set State Machine
            _fsm = new StateMachine<PlayerBaseState>();

            // Declare States
            PlayerBaseState idleState   = new IdleState(_fsm, this);
            PlayerBaseState walkState   = new WalkState(_fsm, this);
            PlayerBaseState jumpState   = new JumpState(_fsm, this);
            PlayerBaseState fallState   = new FallState(_fsm, this);
            PlayerBaseState landState   = new LandState(_fsm, this);
            PlayerBaseState slideState  = new SlideState(_fsm, this);
            PlayerBaseState climbState  = new ClimbState(_fsm, this);
            PlayerBaseState damageState = new DamageState(_fsm, this);
            PlayerBaseState deathState  = new DeathState(_fsm, this);

            // Declare Transitions
            _fsm.AddTransition(idleState, walkState, new FuncPredicate(() => inputDir != 0));
            _fsm.AddTransition(walkState, idleState, new FuncPredicate(() => inputDir == 0));

            _fsm.AddTransition(idleState, jumpState, new FuncPredicate(() => _jumpAction.WasPressedThisFrame()));
            _fsm.AddTransition(walkState, jumpState, new FuncPredicate(() => _jumpAction.WasPressedThisFrame()));

            _fsm.AddTransition(idleState, fallState, new FuncPredicate(() => !IsOnGround && RigidBody2D.linearVelocityY < 0));
            _fsm.AddTransition(walkState, fallState, new FuncPredicate(() => !IsOnGround && RigidBody2D.linearVelocityY < 0));
            _fsm.AddTransition(jumpState, fallState, new FuncPredicate(() => !IsOnGround && RigidBody2D.linearVelocityY < 0));

            _fsm.AddTransition(fallState, idleState, new FuncPredicate(() => IsOnGround));

            _fsm.SetState(idleState);
        }

        public void GroundCheck()
        {
            float dstRays = (_collider.bounds.size.x / (nOfRaysVertical - 1)) - _fRaysTreshold / (nOfRaysVertical - 1);
            bool _isGrounded = false;

            for (var i = 0; i < nOfRaysVertical; i++)
            {
                Vector3 rayPosition = new Vector3((_collider.bounds.extents.x - (_fRaysTreshold / 2) - (dstRays * i)), _collider.bounds.extents.y);

                RaycastHit2D hit = Physics2D.Raycast(_collider.bounds.center - rayPosition, Vector2.down, _fRayLenght, _groundLayer);
                if (hit.collider != null && hit.distance <= _distanceFromFloor && RigidBody2D.linearVelocityY <= 0)
                    _isGrounded = true;
            }

            IsOnGround = _isGrounded;
        }

        private void OnDrawGizmos()
        {
            if (_drawDebugLines)
            {
                CapsuleCollider2D collider2D = GetComponent<CapsuleCollider2D>();

                // Draw floor detection debug lines
                float dstRaysF = (collider2D.bounds.size.x / (nOfRaysVertical - 1)) - _fRaysTreshold / (nOfRaysVertical - 1);
                for (var i = 0; i < nOfRaysVertical; i++)
                {
                    Vector3 rayPosition = new Vector3((collider2D.bounds.extents.x - (_fRaysTreshold / 2) - (dstRaysF * i)), collider2D.bounds.extents.y);

                    Color rayColor = IsOnGround ? Color.green : Color.red;
                    Debug.DrawRay(collider2D.bounds.center - rayPosition, Vector2.down * _fRayLenght, rayColor);
                }
                Vector3 pos = new Vector3(collider2D.bounds.size.x / 2, collider2D.bounds.size.y / 2 + _distanceFromFloor);
                Debug.DrawRay(collider2D.bounds.center - pos, Vector2.right * (collider2D.bounds.size.x), Color.magenta);

                if (_fsm != null)
                {
                    Handles.Label(transform.position * new Vector2(0, 1), _fsm.GetCurrentState().ToString());
                }
            }
        }

        public void MoveHorizontally(float velocity)
        {
            RigidBody2D.linearVelocityX = velocity;
        }

        private void Update()
        {
            JumpVelocity = (2.0f * JumpHeight) / TimeToPeak;
            JumpGravity  = (2.0f * JumpHeight) / (TimeToPeak * TimeToPeak);
            FallGravity  = (2.0f * JumpHeight) / (TimeToFall * TimeToFall);

            inputDir = _moveAction.ReadValue<float>();

            _fsm.Update();
        }

        private void FixedUpdate()
        {
            _fsm.FixedUpdate();
        }

        private void OnEnable()
        {
            InputActions.Enable();
        }

        private void OnDisable()
        {
            InputActions.Disable();
        }
    }
}