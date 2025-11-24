using StateMachine;
using System;

namespace Player.StateMachine.LocomotionLayer
{
    public class Fall : PlayerBaseState
    {
        public Fall(PlayerContext context, FiniteStateMachine<PlayerBaseState> stateMachine) : base(context,
            stateMachine)
        {
        }

        public override Type CheckTransition()
        {
            if (Context.JumpAction.WasPressedThisFrame() && Context.CoyoteTimeCounter > 0) return typeof(Jump);

            return null;
        }

        public override void OnEnter()
        {
            Context.CurrentFallSpeed = Context.FallGravity;
        }
    }
}