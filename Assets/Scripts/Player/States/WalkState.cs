using FSM;

namespace Player.States
{
    public class WalkState : PlayerBaseState
    {
        public WalkState(StateMachine<PlayerBaseState> fsm, PlayerController controller) : base(fsm, controller)
        {
        }

        public override void Update()
        {
            Controller.RigidBody2D.linearVelocityX = 10;
        }
    }
}