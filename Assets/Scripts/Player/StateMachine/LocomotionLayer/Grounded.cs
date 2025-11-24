using StateMachine;
using UnityEngine;

namespace Player.StateMachine.LocomotionLayer
{
    public class Grounded : PlayerBaseState
    {
        public Grounded(PlayerContext context, FiniteStateMachine<PlayerBaseState> stateMachine) : base(context,
            stateMachine)
        {
        }

        public override void OnEnter()
        {
            Context.CoyoteTimeCounter = Context.CoyoteTime;
            if (Context.JumpBufferCounter > 0) RequestTransition<Airborne>();
        }

        public override void Update(float deltaTime)
        {
            if (!Context.Grounded) RequestTransition<Airborne>();
            if (Context.JumpBufferCounter > 0) RequestTransition<Airborne>();
        }
    }
}