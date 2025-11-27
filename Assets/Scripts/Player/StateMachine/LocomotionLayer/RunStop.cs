using Player;
using Player.StateMachine;
using StateMachine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Player.StateMachine.LocomotionLayer
{
    public class RunStop : PlayerBaseState
    {
        public RunStop(PlayerContext context, FiniteStateMachine<PlayerBaseState> stateMachine) : base(context, stateMachine)
        {
        }

        public override Type CheckTransition()
        {
            if (Context.TimeToStopCounter <= 0) return typeof(Idle);
            if (Context.MoveAction.ReadValue<float>() != 0 && Context.LastInputDirection != Context.MoveAction.ReadValue<float>()) return typeof(Run);

            return null;
        }

        public override void Update(float deltaTime)
        {
            Context.TimeToStopCounter -= deltaTime;
            if (Context.TimeToStopCounter < 0) Context.TimeToStopCounter = 0;

            float decelerationRate = Context.CurrentMovementSpeed / Context.TimeToStop;
            Context.CurrentMovementSpeed -= decelerationRate * deltaTime;
            if (Context.CurrentMovementSpeed < 0) Context.CurrentMovementSpeed = 0;
        }

        public override void OnEnter()
        {
            Context.CurrentMovementSpeed = Context.RunSpeed;
            Context.IgnorePlayerInput = true;
            Context.TimeToStopCounter = Context.TimeToStop;
            Context.Animator.Play("Run_Stop_Start");
        }

        public override void OnExit()
        {
            Context.IgnorePlayerInput = false;
        }
    }
}
