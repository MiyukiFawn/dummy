using FSM;

namespace Player.States
{
    public class JumpState : PlayerBaseState
    {
        public JumpState(StateMachine<PlayerBaseState> fsm, PlayerController controller) : base(fsm, controller)
        {
        }

        public override void OnEnter()
        {
            Controller.RigidBody2D.linearVelocityY = 0;
        }

        public override void Update()
        {
            Controller.MoveHorizontally(Controller.WalkSpeed * Controller.inputDir);
        }
    }
}