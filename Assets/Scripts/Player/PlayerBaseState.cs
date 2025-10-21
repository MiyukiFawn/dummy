using FSM;
using FSM.Interfaces;

namespace Player
{
    public abstract class PlayerBaseState : IState
    {
        protected StateMachine<PlayerBaseState> Fsm;
        protected PlayerController Controller;
        
        protected PlayerBaseState(StateMachine<PlayerBaseState> fsm, PlayerController controller)
        {
            Fsm = fsm;
            Controller = controller;
        }
        
        public virtual void FixedUpdate() { }

        public virtual void OnEnter() { }

        public virtual void OnExit() { }

        public virtual void Update() { }
    }
}
