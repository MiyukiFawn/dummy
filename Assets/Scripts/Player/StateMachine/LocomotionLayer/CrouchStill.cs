using StateMachine;
using System;

namespace Player.StateMachine.LocomotionLayer
{
    public class CrouchStill : PlayerBaseState
    {
        public CrouchStill(PlayerContext context, FiniteStateMachine<PlayerBaseState> stateMachine) : base(context,
            stateMachine)
        {
        }

        public override Type CheckTransition()
        {
            if (Context.MoveAction.ReadValue<float>() != 0 && Context.CanWalk) return typeof(Crawl);

            return null;
        }

        public override void OnEnter()
        {
            if (Context.CanWalk) Context.Animator.Play("Crouch_Still");
        }
    }
}