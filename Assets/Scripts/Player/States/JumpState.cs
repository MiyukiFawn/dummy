using FSM;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player.States
{
    public class JumpState : PlayerBaseState
    {
        private InputAction _jumpAction;

        public JumpState(StateMachine<PlayerBaseState> fsm, PlayerController controller, Animator animator, InputAction jumpAction) : base(fsm, controller, animator)
        {
            _jumpAction = jumpAction;
        }

        public override void OnEnter()
        {
            Controller.RigidBody2D.linearVelocityY = Controller.JumpVelocity;
            Animator.SetBool("Airborn", true);
        }

        public override void OnExit()
        {
            if (Fsm.PreviousState != typeof(FallState)) Animator.SetBool("Airborn", false);
        }

        public override void Update()
        {   
            var verticalVelocity = Controller.RigidBody2D.linearVelocity.y;
            verticalVelocity -= Controller.JumpGravity * Time.deltaTime;

            if (_jumpAction.WasReleasedThisFrame() && Controller.RigidBody2D.linearVelocityY > 0) verticalVelocity /= Controller.ReleaseJumpSpeedMultiplyer;

            Controller.MoveHorizontally(Controller.WalkSpeed * Controller.inputDir);
            Controller.MoveVertically(verticalVelocity);
        }
    }
}