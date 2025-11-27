using System;
using StateMachine;

namespace Player.StateMachine.LocomotionLayer
{
    public class Spin : PlayerBaseState
    {
        public Spin(PlayerContext context, FiniteStateMachine<PlayerBaseState> stateMachine) : base(context, stateMachine)
        {
        }

        public override void OnEnter()
        {
            Context.CanWalk = false;
            Context.Animator.Play("Spin");
        }

        public override Type CheckTransition()
        {
            if (Context.SpinEnd) return typeof(Idle);
            
            return null;
        }

        public override void OnExit()
        {
            Context.SpinEnd = false;
            Context.CanWalk = true;
        }
    }
}