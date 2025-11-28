using StateMachine;
using System;
using UnityEngine;
using static UnityEngine.Rendering.STP;
using static UnityEngine.RuleTile.TilingRuleOutput;

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
            if (Context.IsLedgeGrabbing) return typeof(LedgeGrab);

            return null;
        }

        public override void OnEnter()
        {
            Context.CurrentFallSpeed = Context.FallGravity;

            Context.Animator.Play("Fall_Start");
        }
    }
}