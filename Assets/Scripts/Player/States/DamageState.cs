using FSM;
using UnityEngine;

namespace Player.States
{
    public class DamageState : PlayerBaseState
    {
        public DamageState(StateMachine<PlayerBaseState> fsm, PlayerController controller, Animator animator) : base(fsm, controller, animator)
        {
        }
    }
}