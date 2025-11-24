using StateMachine;

namespace Player.StateMachine.LocomotionLayer
{
    public class Idle : PlayerBaseState
    {
        public Idle(PlayerContext context, FiniteStateMachine<PlayerBaseState> stateMachine) : base(context,
            stateMachine)
        {
        }
    }
}