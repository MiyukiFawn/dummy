using StateMachine;
using System;

namespace Player.StateMachine.LocomotionLayer
{
    public class Crawl : PlayerBaseState
    {
        public Crawl(PlayerContext context, FiniteStateMachine<PlayerBaseState> stateMachine) : base(context,
            stateMachine)
        {
        }

        public override Type CheckTransition()
        {
            if (Context.MoveAction.ReadValue<float>() == 0) return typeof(CrouchStill);

            return null;
        }

        public override void OnEnter()
        {
            Context.CurrentMovementSpeed = Context.CrawlSpeed;
            Context.Animator.Play("Crawl");
        }
    }
}