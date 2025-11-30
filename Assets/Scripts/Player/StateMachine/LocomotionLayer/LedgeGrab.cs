using StateMachine;
using System;
using UnityEngine;

namespace Player.StateMachine.LocomotionLayer
{
    public class LedgeGrab : PlayerBaseState
    {
        public LedgeGrab(PlayerContext context, FiniteStateMachine<PlayerBaseState> stateMachine) : base(context,
            stateMachine)
        {
        }

        public override Type CheckTransition()
        {
            if (Context.JumpAction.WasPressedThisFrame()) return typeof(Jump);
            if (Context.CrouchAction.WasPressedThisFrame()) return typeof(Fall);

            return null;
        }

        public override void OnEnter()
        {
            Context.IgnorePlayerInput = true;
            Context.CanWalk = false;
            Context.CurrentFallSpeed = 0;
            Context.CurrentYVelocity = 0;

            Context.Animator.Play("Ledge_Grab");
        }

        public override void OnExit()
        {
            Context.IgnorePlayerInput = false;
            Context.CanWalk = true;
        }
    }
}