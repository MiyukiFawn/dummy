using System;
using Player.StateMachine;
using Player.StateMachine.LocomotionLayer;
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

        [SerializeField] [Header("Configuration")]
        private PlayerConfig config;

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
            CeilingCheck();
            WallCheck();
            LedgeCheck();

            _stateMachine.Update(Time.deltaTime);

            _rb.linearVelocity = _context.Velocity;

            _sprite.flipX = _context.Flipped;
        }

        private void GroundCheck()
        {
            RaycastHit2D hit = Physics2D.BoxCast(transform.position, config.gCheckBoxSize, 0, -transform.up,
                config.gCheckDistance, config.groundLayer);

            _context.IsTouchingGround = hit.collider is not null;
        }

        private void CeilingCheck()
        {
            RaycastHit2D hit = Physics2D.BoxCast(transform.position, config.ceCheckBoxSize, 0, transform.up,
                config.ceCheckDistance, config.groundLayer);

            _context.IsTouchingCeiling = hit.collider is not null;
        }

        private void WallCheck()
        {
            Vector3 wallCheckDirection = _context.Flipped ? -transform.right : transform.right;

            RaycastHit2D hit = Physics2D.BoxCast(transform.position, config.wCheckBoxSize, 0, wallCheckDirection,
                config.wCheckDistance, config.groundLayer);

            _context.IsTouchingWall = hit.collider is not null;
        }

        private void LedgeCheck()
        {
            _context.IsOnLedge = false;

            if (_context.IsTouchingGround) return;
            if (_context.IsTouchingCeiling) return;
            if (!_context.IsTouchingWall) return;
            // if (!_stateMachine.IsStateActive(typeof(Fall))) return;
            
            float ledgeRayVerticalOriginX = _context.Flipped ? -config.ledgeRayVerticalOrigin.x : config.ledgeRayVerticalOrigin.x;
            Vector3 ledgeRayVerticalOrigin = new Vector3(ledgeRayVerticalOriginX, config.ledgeRayVerticalOrigin.y);
            
            Vector3 ledgeRayHorizontalDirection = _context.Flipped ? -transform.right  : transform.right;
            
            RaycastHit2D verticalHit = Physics2D.Raycast(
                transform.position + ledgeRayVerticalOrigin, 
                Vector2.down, 
                config.ledgeRayVerticalDistance, 
                config.groundLayer);
            
            if (verticalHit.collider is null) return;

            RaycastHit2D horizontalHit = Physics2D.Raycast(
                transform.position,
                ledgeRayHorizontalDirection,
                config.ledgeRayHorizontalDistance,
                config.groundLayer);

            if (horizontalHit.collider is null) return;

            _context.LedgePosition = new Vector2(horizontalHit.point.x, verticalHit.point.y);
            _context.IsOnLedge = true;
        }

        private void OnDrawGizmos()
        {
            if (!drawGizmos) return;
            bool isFlipped = _context?.Flipped ?? false;

            // Ground Check
            Gizmos.color = Color.green;
            if (_context?.IsTouchingGround ?? false) Gizmos.DrawCube(transform.position - transform.up * config.gCheckDistance, config.gCheckBoxSize);
            else Gizmos.DrawWireCube(transform.position - transform.up * config.gCheckDistance, config.gCheckBoxSize);

            // Ceiling Check
            Gizmos.color = Color.magenta;
            if (_context?.IsTouchingCeiling ?? false) Gizmos.DrawCube(transform.position + transform.up * config.ceCheckDistance, config.ceCheckBoxSize);
            Gizmos.DrawWireCube(transform.position + transform.up * config.ceCheckDistance, config.ceCheckBoxSize);

            // Wall Detection
            Vector3 wallCheckDirection = isFlipped ? -transform.right : transform.right;
            Gizmos.color = Color.yellow;
            if (_context?.IsTouchingWall ?? false) Gizmos.DrawCube(transform.position + wallCheckDirection * config.wCheckDistance, config.wCheckBoxSize);
            else Gizmos.DrawWireCube(transform.position + wallCheckDirection * config.wCheckDistance, config.wCheckBoxSize);
            
            // Ledge Detection
            float ledgeRayVerticalOriginX = isFlipped ? -config.ledgeRayVerticalOrigin.x : config.ledgeRayVerticalOrigin.x;
            Vector3 ledgeRayVerticalOrigin = new Vector3(ledgeRayVerticalOriginX, config.ledgeRayVerticalOrigin.y);
            
            Vector3 ledgeRayHorizontalDirection = isFlipped ? -transform.right  : transform.right;
            
            Gizmos.color = Color.green;
            Gizmos.DrawRay(transform.position + ledgeRayVerticalOrigin, Vector2.down * config.ledgeRayVerticalDistance);
            
            Gizmos.color = Color.red;
            Gizmos.DrawRay(transform.position, ledgeRayHorizontalDirection * config.ledgeRayHorizontalDistance);
            
            // Hand Position
            float ledgeHoldPositionX = isFlipped ? -config.ledgeHoldPosition.x : config.ledgeHoldPosition.x;
            Vector3 ledgeHoldPosition = new Vector2(ledgeHoldPositionX, config.ledgeHoldPosition.y);
            
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere( transform.position + ledgeHoldPosition, 0.2f);
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
            float ledgeHoldPositionX = _context.Flipped ? -config.ledgeHoldPosition.x : config.ledgeHoldPosition.x;
            Vector3 ledgeHoldPosition = new Vector2(ledgeHoldPositionX, config.ledgeHoldPosition.y);
            
            Vector3 characterTargetPosition = new Vector3(_context.LedgePosition.x, _context.LedgePosition.y) - ledgeHoldPosition;
            transform.position = characterTargetPosition;
        }

        #endregion
    }
}