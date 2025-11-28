using System;
using Player.StateMachine;
using StateMachine;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;
using UnityEngine.UIElements;

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
            LedgeGrabCheck();

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

        private void LedgeGrabCheck()
        {
            Vector2 ledgeRayOrigin = new Vector2(transform.position.x, transform.position.y);
            float ledgeRayOriginX = _context.Flipped ? -config.ledgeGrabRayOrigin.x : config.ledgeGrabRayOrigin.x;
            ledgeRayOrigin += new Vector2(ledgeRayOriginX, config.ledgeGrabRayOrigin.y);

            RaycastHit2D hit = Physics2D.Raycast(ledgeRayOrigin, Vector2.down, 1, config.groundLayer);

            if (hit.collider != null) {
                if (Vector2.Distance(hit.point, ledgeRayOrigin) <= config.ledgeGrabRayDistance) _context.IsLedgeGrabbing = true;
            }
            else
            {
                _context.IsLedgeGrabbing = false;
            }
        }

        private void OnDrawGizmos()
        {
            if (!drawGizmos) return;

            Gizmos.color = Color.green;
            Gizmos.DrawWireCube(transform.position - transform.up * config.gCheckDistance, config.gCheckBoxSize);

            Gizmos.color = Color.red;

            Vector2 ledgeAnchorPosition = new Vector2(transform.position.x, transform.position.y);
            float ledgeAnchorX = _context?.Flipped ?? false ? -config.grabPositionOffsed.x : config.grabPositionOffsed.x;
            ledgeAnchorPosition += new Vector2(ledgeAnchorX, config.grabPositionOffsed.y);

            Gizmos.DrawWireSphere(ledgeAnchorPosition, 0.2f);

            Gizmos.color = Color.white;
            Vector2 ledgeRayOrigin = new Vector2(transform.position.x, transform.position.y);
            float ledgeRayOriginX = _context?.Flipped ?? false ? -config.ledgeGrabRayOrigin.x : config.ledgeGrabRayOrigin.x;
            ledgeRayOrigin += new Vector2(ledgeRayOriginX, config.ledgeGrabRayOrigin.y);

            Gizmos.DrawWireSphere(ledgeRayOrigin, 0.1f);
            Gizmos.DrawRay(ledgeRayOrigin, Vector2.down);

            Gizmos.color = Color.yellow;
            Gizmos.DrawRay(ledgeRayOrigin, Vector2.down * config.ledgeGrabRayDistance);
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

        private void OnGrabLedge()
        {
            //Vector2 ledgeAnchorPosition = new Vector2(transform.position.x, transform.position.y);
            //float ledgeAnchorX = _context?.Flipped ?? false ? -config.grabPositionOffsed.x : config.grabPositionOffsed.x;
            //ledgeAnchorPosition -= new Vector2(ledgeAnchorX, config.grabPositionOffsed.y);

            //transform.position = ledgeAnchorPosition;
        }

        #endregion
    }
}