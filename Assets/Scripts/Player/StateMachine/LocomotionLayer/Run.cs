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
    public class Run : PlayerBaseState
    {
        public Run(PlayerContext context, FiniteStateMachine<PlayerBaseState> stateMachine) : base(context, stateMachine)
        {
        }

        public override Type CheckTransition()
        {
            if (Context.MoveAction.ReadValue<float>() == 0) return typeof(RunStop);

            return null;
        }

        public override void OnEnter()
        {
            Context.TimeToRunCounter = Context.TimeToRun;
            Context.CurrentMovementSpeed = Context.RunSpeed;
            Context.Animator.Play("Run");
        }
    }
}
