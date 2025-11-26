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
            return typeof(Idle);
        }

        public override Type CheckTransition()
        {
            if (Context.CrouchAction.WasPressedThisFrame() && !Context.IsCrouching) return typeof(Crouch);
            return null;
        }

        public override void OnEnter()
        {
            if (IsStateActive(typeof(Crouch)))
            {
                Context.CanJump = false;
                Context.CanWalk = false;
                Context.Animator.Play("Stand_Up");
            }
        }
    }
}