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

        public bool Flipped { get; set; } = false;

        public float WalkSpeed => _config.walkSpeed;
        
        // public float Gravity => (2 * _config.jumpHeight) / (_config.timeToPeak * _config.timeToPeak);
        // public float InitialJumpVelocity => (2 * _config.jumpHeight) / _config.timeToPeak;
        // public float MaxFallSpeed => _config.maxFallSpeed;

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
        
        public bool Grounded = false;

        public InputAction MoveAction { get; set; }
        public InputAction JumpAction { get; set; }
        public InputAction SpinAction { get; set; }
        public Animator Animator { get; set; }

        public float CurrentXVelocity = 0;
        public float CurrentYVelocity = 0;
        public Vector2 Velocity => new Vector2(CurrentXVelocity, CurrentYVelocity);

        public bool SpinEnd = false;
    }
}