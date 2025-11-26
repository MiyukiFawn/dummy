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

            Context.CurrentXVelocity = Context.CurrentMovementSpeed == 0 ? 0 : Context.CurrentMovementSpeed * Context.MoveAction.ReadValue<float>();
            if (!Context.CanWalk) Context.CurrentXVelocity = 0;

            float inputDir = Context.MoveAction.ReadValue<float>();
            if (inputDir > 0) Context.Flipped = false;
            else if (inputDir < 0) Context.Flipped = true;
        }

        public override Type GetInitialChild() => typeof(Grounded);
    }
}