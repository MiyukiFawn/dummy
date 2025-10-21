using FSM;

namespace Player.States
{
    public class LandState : PlayerBaseState
    {
        public LandState(StateMachine<PlayerBaseState> fsm, PlayerController controller) : base(fsm, controller)
        {
        }
    }
}