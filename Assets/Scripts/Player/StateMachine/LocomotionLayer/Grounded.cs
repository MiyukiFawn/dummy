using StateMachine;
using System;
using UnityEngine;

namespace Player.StateMachine.LocomotionLayer
{
    public class Grounded : PlayerBaseState
    {
        public Grounded(PlayerContext context, FiniteStateMachine<PlayerBaseState> stateMachine) : base(context,
            stateMachine)
        {
        }

        public override Type CheckTransition()
        {
            if (Context.JumpBufferCounter > 0f) return typeof(Airborne);
            if (!Context.Grounded) return typeof(Airborne);

            return null;
        }

        public override Type GetInitialChild()
        {
            return typeof(Idle);
        }
    }
}