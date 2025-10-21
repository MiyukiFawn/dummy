using FSM;

namespace Player.States
{
    public class SlideState : PlayerBaseState
    {
        public SlideState(StateMachine<PlayerBaseState> fsm, PlayerController controller) : base(fsm, controller)
        {
        }
    }
}