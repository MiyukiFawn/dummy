using System;
using StateMachine;
using UnityEngine;

namespace Player.StateMachine.LocomotionLayer
{
    public class Root : PlayerBaseState
    {
        public Root(PlayerContext context, FiniteStateMachine<PlayerBaseState> stateMachine) : base(context,
            stateMachine)
        {
        }

        public override void Update(float deltaTime)
        {
            if (Context.JumpAction.WasPressedThisFrame()) Context.JumpBufferCounter = Context.JumpBuffer;
            else Context.JumpBufferCounter -= deltaTime;
            
            Context.CurrentXVelocity = Context.WalkSpeed * Context.MoveAction.ReadValue<float>();
        }

        public override Type GetInitialChild() => typeof(Grounded);
    }
}