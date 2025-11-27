using StateMachine;
using System;

namespace Player.StateMachine.LocomotionLayer
{
    public class Walk : PlayerBaseState
    {
        public Walk(PlayerContext context, FiniteStateMachine<PlayerBaseState> stateMachine) : base(context,
            stateMachine)
        {
        }

        public override Type CheckTransition()
        {
            if (Context.TimeToRunCounter >= Context.TimeToRun) return typeof(Run);
            if (Context.MoveAction.ReadValue<float>() == 0) return typeof(Idle);

            return null;
        }

        public override void Update(float deltaTime)
        {
            Context.TimeToRunCounter += deltaTime;
        }

        public override void OnEnter()
        {
            Context.CurrentMovementSpeed = Context.WalkSpeed;
            Context.Animator.Play("Walk");
        }
    }
}