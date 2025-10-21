using FSM;

namespace Player.States
{
    public class DeathState : PlayerBaseState
    {
        public DeathState(StateMachine<PlayerBaseState> fsm, PlayerController controller) : base(fsm, controller)
        {
        }
    }
}