using System;
using Player.StateMachine;
using StateMachine;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

namespace Player
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private bool drawGizmos = false;
        [SerializeField][Header("Configuration")] private PlayerConfig config;
        [SerializeField] private InputActionAsset inputActions;

        private Rigidbody2D _rb;
        private CapsuleCollider2D _collider;
        private SpriteRenderer _sprite;
        private Animator _animator;

        private PlayerContext _context;
        private FiniteStateMachine<PlayerBaseState> _stateMachine;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
            _collider = GetComponent<CapsuleCollider2D>();
            _sprite = GetComponent<SpriteRenderer>();
            _animator = GetComponent<Animator>();

            _context = new PlayerContext(config)
            {
                Animator = _animator,
                MoveAction = inputActions.FindAction("Move"),
                JumpAction = inputActions.FindAction("Jump"),
                CrouchAction = inputActions.FindAction("Crouch"),
                StandUpAction = inputActions.FindAction("Stand Up"),
                SpinAction = inputActions.FindAction("Spin"),
            };

            _stateMachine = PlayerFactory.BuildStateMachine(_context);
        }

        private void Update()
        {
            // 1° Checks
            // 2° Update state machine
            // 3° Handle physics and other shenanigans

            GroundCheck();

            _stateMachine.Update(Time.deltaTime);

            _rb.linearVelocity = _context.Velocity;

            _sprite.flipX = _context.Flipped;
        }

        private void GroundCheck()
        {
            RaycastHit2D hit = Physics2D.BoxCast(transform.position, config.gCheckBoxSize, 0, -transform.up,
                config.gCheckDistance, config.groundLayer);

            _context.Grounded = hit.collider != null;
        }

        private void OnDrawGizmos()
        {
            if (!drawGizmos) return;

            Gizmos.DrawWireCube(transform.position - transform.up * config.gCheckDistance, config.gCheckBoxSize);
        }

        private void OnEnable()
        {
            inputActions.Enable();
        }

        private void OnDisable()
        {
            inputActions.Disable();
        }

        #region Animation Events

        private void OnSpinEnd()
        {
            _context.SpinEnd = true;
        }
        
        private void OnCrouchEnd()
        {
            _context.CanWalk = true;
            _context.IsCrouching = false;
        }

        private void OnStandUpEnd()
        {
            _context.CanWalk = true;
            _context.CanJump = true;
            _context.IsCrouching = false;
        }

        #endregion
    }
}