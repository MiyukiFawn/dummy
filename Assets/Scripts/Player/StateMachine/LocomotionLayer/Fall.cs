using StateMachine;

namespace Player.StateMachine.LocomotionLayer
{
    public class Fall : PlayerBaseState
    {
        public Fall(PlayerContext context, FiniteStateMachine<PlayerBaseState> stateMachine) : base(context,
            stateMachine)
        {
        }

        public override void OnEnter()
        {
            Context.CurrentFallSpeed = Context.FallGravity;
        }

        public override void Update(float deltaTime)
        {
            if (Context.JumpAction.WasPressedThisFrame() && Context.CoyoteTimeCounter > 0) RequestTransition<Jump>();
        }
    }
}