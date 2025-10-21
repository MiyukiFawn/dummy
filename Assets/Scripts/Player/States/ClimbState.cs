using FSM;

namespace Player.States
{
    public class ClimbState : PlayerBaseState
    {
        public ClimbState(StateMachine<PlayerBaseState> fsm, PlayerController controller) : base(fsm, controller)
        {
        }
    }
}