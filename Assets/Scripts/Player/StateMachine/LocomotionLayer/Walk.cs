using StateMachine;
using System;

namespace Player.StateMachine.LocomotionLayer
{
    public class Walk : PlayerBaseState
    {
        public Walk(PlayerContext context, FiniteStateMachine<PlayerBaseState> stateMachine) : base(context,
            stateMachine)
        {
        }

        public override Type CheckTransition()
        {
            if (Context.MoveAction.ReadValue<float>() == 0) return typeof(Idle);

            return null;
        }

        public override void OnEnter()
        {
            Context.CurrentMovementSpeed = Context.WalkSpeed;
            Context.Animator.Play("Walk");
        }
    }
}