using FSM;
using Unity.VisualScripting;

namespace Player.States
{
    public class IdleState : PlayerBaseState
    {
        public IdleState(StateMachine<PlayerBaseState> fsm, PlayerController controller) : base(fsm, controller)
        {
        }

        public override void OnEnter()
        {
            Controller.RigidBody2D.linearVelocityX = 0;
        }
    }
}