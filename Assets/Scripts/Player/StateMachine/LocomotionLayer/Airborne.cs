using System;
using StateMachine;

namespace Player.StateMachine.LocomotionLayer
{
    public class Airborne : PlayerBaseState
    {
        public Airborne(PlayerContext context, FiniteStateMachine<PlayerBaseState> stateMachine) : base(context,
            stateMachine)
        {
        }

        public override Type CheckTransition()
        {
            if (Context.IsTouchingGround && Context.CurrentYVelocity <= 0) return typeof(Grounded);
            return null;
        }

        public override void OnEnter()
        {
            Context.IsCrouching = false;
            Context.CanJump = true;
            Context.CanWalk = true;
            Context.CoyoteTimeCounter = Context.CoyoteTime;
        }

        public override void OnExit()
        {
            Context.CurrentYVelocity = -0.5f;
        }

        public override void Update(float deltaTime)
        {
            Context.CoyoteTimeCounter -= deltaTime;

            Context.CurrentYVelocity -= Context.CurrentFallSpeed * deltaTime;
            if (Context.CurrentYVelocity < -Context.MaxFallSpeed) Context.CurrentYVelocity = -Context.MaxFallSpeed;
        }

        public override Type GetInitialChild()
        {
            return Context.JumpBufferCounter > 0
                ? typeof(Jump)
                : typeof(Fall);
        }
    }
}