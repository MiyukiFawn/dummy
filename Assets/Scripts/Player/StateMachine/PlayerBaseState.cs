using StateMachine;

namespace Player.StateMachine
{
    public class PlayerBaseState : State<PlayerBaseState>
    {
        protected readonly PlayerContext Context;

        protected PlayerBaseState(PlayerContext context, FiniteStateMachine<PlayerBaseState> stateMachine) : base(
            stateMachine)
        {
            Context = context;
        }
    }
}