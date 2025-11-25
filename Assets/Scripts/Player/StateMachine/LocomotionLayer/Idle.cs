using StateMachine;
using System;

namespace Player.StateMachine.LocomotionLayer
{
    public class Idle : PlayerBaseState
    {
        public Idle(PlayerContext context, FiniteStateMachine<PlayerBaseState> stateMachine) : base(context,
            stateMachine)
        {
        }

        public override Type CheckTransition()
        {
            if (Context.MoveAction.ReadValue<float>() != 0) return typeof(Walk);

            return null;
        }

        public override void OnEnter()
        {
            Context.Animator.Play("Idle");
        }
    }
}