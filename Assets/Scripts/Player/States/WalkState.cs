using FSM;
using UnityEngine;

namespace Player.States
{
    public class WalkState : PlayerBaseState
    {
        public WalkState(StateMachine<PlayerBaseState> fsm, PlayerController controller, Animator animator) : base(fsm, controller, animator)
        {
        }

        public override void OnEnter()
        {
            Animator.SetBool("Walking", true);
        }

        public override void OnExit()
        {
            Animator.SetBool("Walking", false);
        }

        public override void Update()
        {
            Controller.MoveHorizontally(Controller.WalkSpeed * Controller.inputDir);
            Controller.MoveVertically(-0.1f);
        }
    }
}