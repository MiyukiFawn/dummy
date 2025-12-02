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

        private const float LedgeGrabTimer = 0.2f;
        private float _ledgeGrabTimerCounter = 0;

        public override Type CheckTransition()
        {
            if (Context.JumpAction.WasPressedThisFrame() && Context.CoyoteTimeCounter > 0) return typeof(Jump);
            if (Context.IsLedgeAvaliable && Context.IsOnLedge && _ledgeGrabTimerCounter > LedgeGrabTimer) return typeof(LedgeGrab);

            return null;
        }

        public override void Update(float deltaTime)
        {
            _ledgeGrabTimerCounter += deltaTime;
        }

        public override void OnEnter()
        {
            if (SM.IsStateActive(typeof(LedgeGrab))) _ledgeGrabTimerCounter = 0;
            else _ledgeGrabTimerCounter = LedgeGrabTimer;
            
            Context.CurrentFallSpeed = Context.FallGravity;
            Context.Animator.Play("Fall_Start");
        }
    }
}