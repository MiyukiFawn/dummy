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
            if (Context.SpinAction.WasPressedThisFrame()) return typeof(Spin);
            if (Context.MoveAction.ReadValue<float>() != 0) return typeof(Walk);

            return null;
        }

        public override void OnEnter()
        {
            Context.CurrentMovementSpeed = Context.WalkSpeed;
            if (!IsStateActive(typeof(Crouch))) Context.Animator.Play("Idle");
        }
    }
}