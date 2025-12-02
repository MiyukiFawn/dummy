using StateMachine;
using System;
using UnityEngine;

namespace Player.StateMachine.LocomotionLayer
{
    public class Crouch : PlayerBaseState
    {
        public Crouch(PlayerContext context, FiniteStateMachine<PlayerBaseState> stateMachine) : base(context,
            stateMachine)
        {
        }

        public override Type GetInitialChild()
        {
            return typeof(CrouchStill);
        }

        public override Type CheckTransition()
        {
            if (Context.StandUpAction.WasPressedThisFrame()) return typeof(Standing);
            return null;
        }

        public override void OnEnter()
        {
            Context.CurrentMovementSpeed = Context.CrawlSpeed;
            
            Context.CanJump = false;
            Context.CanWalk = false;
            Context.Animator.Play("Crouch");
        }

        public override void OnExit()
        {
            Context.CurrentMovementSpeed = Context.WalkSpeed;
        }
    }
}