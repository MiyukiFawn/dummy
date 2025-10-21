using FSM;
using UnityEngine;

namespace Player.States
{
    public class FallState : PlayerBaseState
    {
        public FallState(StateMachine<PlayerBaseState> fsm, PlayerController controller) : base(fsm, controller)
        {
        }

        public override void OnEnter()
        {
            Controller.RigidBody2D.linearVelocityY = 0;
        }

        public override void Update()
        {
            Controller.MoveHorizontally(Controller.WalkSpeed * Controller.inputDir);

            Controller.RigidBody2D.linearVelocityY -= Controller.FallGravity * Time.deltaTime;
            if (Controller.RigidBody2D.linearVelocityY < Controller.MaxFallSpeed) Controller.RigidBody2D.linearVelocityY = - Controller.MaxFallSpeed;
        }
    }
}