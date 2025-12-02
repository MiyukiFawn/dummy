using StateMachine;
using System;
using UnityEngine;

namespace Player.StateMachine.LocomotionLayer
{
    public class Standing : PlayerBaseState
    {
        public Standing(PlayerContext context, FiniteStateMachine<PlayerBaseState> stateMachine) : base(context,
            stateMachine)
        {
        }

        public override Type GetInitialChild()
        {
            if (Context.MoveAction.ReadValue<float>() != 0 && !SM.IsStateActive(typeof(Crouch)))
            {
                if (Context.TimeToRunCounter >= Context.TimeToRun) return typeof(Run);
                else if (Context.CanWalk) return typeof(Walk);
            }
            return typeof(Idle);
        }

        public override Type CheckTransition()
        {
            if (Context.CrouchAction.WasPressedThisFrame() && !SM.IsStateActive(typeof(Crouch)) && Context.CanCrouch) return typeof(Crouch);
            return null;
        }

        public override void OnEnter()
        {
            if (SM.IsStateActive(typeof(Crouch)))
            {
                Context.CanJump = false;
                Context.CanWalk = false;
                Context.Animator.Play("Stand_Up");
            }
        }
    }
}