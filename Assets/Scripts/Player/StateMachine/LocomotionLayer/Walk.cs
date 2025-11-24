using StateMachine;

namespace Player.StateMachine.LocomotionLayer
{
    public class Walk : PlayerBaseState
    {
        public Walk(PlayerContext context, FiniteStateMachine<PlayerBaseState> stateMachine) : base(context,
            stateMachine)
        {
        }
    }
}