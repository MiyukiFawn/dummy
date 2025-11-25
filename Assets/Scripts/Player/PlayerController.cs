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
                JumpAction = inputActions.FindAction("Jump")
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
            float dstRays = (_collider.bounds.size.x / (config.nOfRaysVertical - 1)) -
                            config.fRaysThreshold / (config.nOfRaysVertical - 1);
            bool isGrounded = false;

            for (int i = 0; i < config.nOfRaysVertical; i++)
            {
                Vector3 rayPosition =
                    new Vector3((_collider.bounds.extents.x - (config.fRaysThreshold / 2) - (dstRays * i)),
                        _collider.bounds.extents.y);

                RaycastHit2D hit = Physics2D.Raycast(_collider.bounds.center - rayPosition, Vector2.down,
                    config.fRayLenght, config.groundLayer);
                if (hit.collider is not null && hit.distance <= config.distanceFromFloor && _rb.linearVelocityY <= 0)
                    isGrounded = true;
            }

            _context.Grounded = isGrounded;
        }

        private void OnDrawGizmos()
        {
            if (!drawGizmos) return;
            
            CapsuleCollider2D capsuleCollider2D = GetComponent<CapsuleCollider2D>();

            // Draw floor detection debug lines
            float dstRaysF = (capsuleCollider2D.bounds.size.x / (config.nOfRaysVertical - 1)) -
                             config.fRaysThreshold / (config.nOfRaysVertical - 1);
            for (int i = 0; i < config.nOfRaysVertical; i++)
            {
                Vector3 rayPosition =
                    new Vector3((capsuleCollider2D.bounds.extents.x - (config.fRaysThreshold / 2) - (dstRaysF * i)),
                        capsuleCollider2D.bounds.extents.y);

                Color rayColor;
                if (_context is null) rayColor = Color.turquoise;
                else rayColor = _context.Grounded ? Color.green : Color.red;

                Debug.DrawRay(capsuleCollider2D.bounds.center - rayPosition, Vector2.down * config.fRayLenght, rayColor);
            }

            Vector3 pos = new Vector3(capsuleCollider2D.bounds.size.x / 2,
                capsuleCollider2D.bounds.size.y / 2 + config.distanceFromFloor);
            Debug.DrawRay(capsuleCollider2D.bounds.center - pos, Vector2.right * (capsuleCollider2D.bounds.size.x), Color.magenta);
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


        #endregion
    }
}