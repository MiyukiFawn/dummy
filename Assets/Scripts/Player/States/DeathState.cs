using FSM;
using UnityEngine;

namespace Player.States
{
    public class DeathState : PlayerBaseState
    {
        public DeathState(StateMachine<PlayerBaseState> fsm, PlayerController controller, Animator animator) : base(fsm, controller, animator)
        {
        }
    }
}