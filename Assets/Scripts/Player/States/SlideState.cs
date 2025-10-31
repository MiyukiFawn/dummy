using FSM;
using UnityEngine;

namespace Player.States
{
    public class SlideState : PlayerBaseState
    {
        public SlideState(StateMachine<PlayerBaseState> fsm, PlayerController controller, Animator animator) : base(fsm, controller, animator)
        {
        }
    }
}