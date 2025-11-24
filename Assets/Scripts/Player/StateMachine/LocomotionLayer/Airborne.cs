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

        public override void OnExit()
        {
            Context.CurrentYVelocity = -0.5f;
        }

        public override void Update(float deltaTime)
        {
            if (Context.Grounded && Context.CurrentYVelocity <= 0) RequestTransition<Grounded>();

            Context.CoyoteTimeCounter -= deltaTime;

            Context.CurrentYVelocity -= Context.CurrentFallSpeed * deltaTime;
            if (Context.CurrentYVelocity < -Context.MaxFallSpeed) Context.CurrentYVelocity = -Context.MaxFallSpeed;
        }

        public override Type GetInitialChild()
        {
            return Context.CoyoteTimeCounter > 0 && Context.JumpBufferCounter > 0
                ? typeof(Jump)
                : typeof(Fall);
        }
    }
}