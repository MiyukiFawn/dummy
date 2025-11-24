using StateMachine;

namespace Player.StateMachine.LocomotionLayer
{
    public class Jump : PlayerBaseState
    {
        public Jump(PlayerContext context, FiniteStateMachine<PlayerBaseState> stateMachine) : base(context,
            stateMachine)
        {
        }
        
        public override void OnEnter()
        {
            Context.CurrentYVelocity = Context.JumpVelocity;
            Context.CurrentFallSpeed = Context.JumpGravity;
            Context.CoyoteTimeCounter = 0;
            Context.JumpBufferCounter = 0;
        }
        
        public override void Update(float deltaTime)
        {
            if (Context.CurrentYVelocity <= 0) RequestTransition<Fall>();
            
            if (Context.JumpAction.WasReleasedThisFrame() && Context.CurrentYVelocity > 0)
                Context.CurrentYVelocity /= Context.ReleaseJumpSpeedMultiplier; 
        }
    }
}