using FSM;
using Unity.VisualScripting;
using UnityEngine;

namespace Player.States
{
    public class IdleState : PlayerBaseState
    {
        public IdleState(StateMachine<PlayerBaseState> fsm, PlayerController controller, Animator animator) : base(fsm, controller, animator)
        {
        }

        public override void OnEnter()
        {
            Controller.MoveHorizontally(0);
            Controller.MoveVertically(-0.1f);
        }
    }
}