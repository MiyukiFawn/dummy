using FSM;

namespace Player.States
{
    public class DamageState : PlayerBaseState
    {
        public DamageState(StateMachine<PlayerBaseState> fsm, PlayerController controller) : base(fsm, controller)
        {
        }
    }
}