using FSM;
using UnityEngine;

namespace Player.States
{
    public class ClimbState : PlayerBaseState
    {
        public ClimbState(StateMachine<PlayerBaseState> fsm, PlayerController controller, Animator animator) : base(fsm, controller, animator)
        {
        }
    }
}