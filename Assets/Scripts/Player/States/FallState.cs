using FSM;
using UnityEngine;

namespace Player.States
{
    public class FallState : PlayerBaseState
    {
        public FallState(StateMachine<PlayerBaseState> fsm, PlayerController controller, Animator animator) : base(fsm, controller, animator)
        {
        }

        public override void OnEnter()
        {
            Controller.RigidBody2D.linearVelocityY = 0;
            Animator.SetBool("Airborn", true);
        }

        public override void OnExit()
        {
            if (Fsm.PreviousState != typeof(FallState)) Animator.SetBool("Airborn", false);
        }

        public override void Update()
        {
            var velocity = Controller.RigidBody2D.linearVelocity.y;
            velocity -= Controller.FallGravity * Time.deltaTime;
            if (Controller.RigidBody2D.linearVelocityY < Controller.MaxFallSpeed) velocity = - Controller.MaxFallSpeed;

            Controller.MoveHorizontally(Controller.WalkSpeed * Controller.inputDir);
            Controller.MoveVertically(velocity);
        }
    }
}