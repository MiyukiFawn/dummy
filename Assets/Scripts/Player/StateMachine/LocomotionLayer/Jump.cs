using StateMachine;
using System;

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
            Context.JumpRequested = false;
            Context.CurrentYVelocity = Context.JumpVelocity;
            Context.CurrentFallSpeed = Context.JumpGravity;
            Context.CoyoteTimeCounter = 0;
            Context.JumpBufferCounter = 0;

            Context.Animator.Play("Jump_Start");
        }

        public override Type CheckTransition()
        {
            if (Context.CurrentYVelocity <= 0) return typeof(Fall);
            return null;
        }
        
        public override void Update(float deltaTime)
        {   
            if (Context.JumpAction.WasReleasedThisFrame() && Context.CurrentYVelocity > 0)
                Context.CurrentYVelocity /= Context.ReleaseJumpSpeedMultiplier; 
        }
    }
}