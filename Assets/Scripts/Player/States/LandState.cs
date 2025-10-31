using FSM;
using UnityEngine;

namespace Player.States
{
    public class LandState : PlayerBaseState
    {
        public LandState(StateMachine<PlayerBaseState> fsm, PlayerController controller, Animator animator) : base(fsm, controller, animator)
        {
        }
    }
}