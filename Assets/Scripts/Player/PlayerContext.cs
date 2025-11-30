using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class PlayerContext
    {
        private readonly PlayerConfig _config;

        public PlayerContext(PlayerConfig config)
        {
            _config = config;
        }

        public bool IgnorePlayerInput { get; set; } = false;

        public bool Flipped { get; set; } = false;
        public bool CanJump { get; set; } = true;
        public bool CanWalk { get; set; } = true;
        public bool CanCrouch { get; set; } = true;
        public bool IsCrouching { get; set; } = false;

        public float CrawlSpeed => _config.crawlSpeed;
        public float WalkSpeed => _config.walkSpeed;
        public float RunSpeed => _config.runSpeed;
        public float CurrentMovementSpeed { get; set; } = 0;

        public float TimeToRun => _config.timeToRun;
        public float TimeToRunCounter { get; set; } = 0;

        public float TimeToStop => _config.timeToStop;
        public float TimeToStopCounter { get; set; } = 0;

        public float LastInputDirection { get; set; } = 0;

        public float JumpVelocity => (2.0f * _config.jumpHeight) / _config.timeToPeak;
        public float JumpGravity => (2.0f * _config.jumpHeight) / Mathf.Pow(_config.timeToPeak, 2);
        public float FallGravity => (2.0f * _config.jumpHeight) / Mathf.Pow(_config.timeToFall, 2);

        public bool JumpRequested { get; set; }

        public float CurrentFallSpeed { get; set; } = 0;
        public float MaxFallSpeed => _config.maxFallSpeed;
        public float ReleaseJumpSpeedMultiplier => _config.releaseJumpSpeedMultiplier;
        public int JumpCounter { get; set; } = 0;

        public float CoyoteTime => _config.coyoteTime;
        public float CoyoteTimeCounter { get; set; }

        public float JumpBuffer => _config.jumpBuffer;
        public float JumpBufferCounter { get; set; }

        public bool IsTouchingGround { get; set; } = false;
        public bool IsTouchingWall { get; set; } = false;
        public bool IsTouchingCeiling { get; set; } = false;
        public bool IsOnLedge { get; set;  } = false;
        public Vector2 LedgePosition { get; set; } = Vector2.one;
        
        public InputAction MoveAction { get; set; }
        public InputAction JumpAction { get; set; }
        public InputAction SpinAction { get; set; }
        public InputAction CrouchAction { get; set; }
        public InputAction StandUpAction { get; set; }

        public Animator Animator { get; set; }

        public float CurrentXVelocity = 0;
        public float CurrentYVelocity = 0;
        public Vector2 Velocity => new Vector2(CurrentXVelocity, CurrentYVelocity);

        public bool SpinEnd = false;
    }
}