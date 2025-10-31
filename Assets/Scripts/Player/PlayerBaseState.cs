using FSM;
using FSM.Interfaces;
using UnityEngine;

namespace Player
{
    public abstract class PlayerBaseState : IState
    {
        protected StateMachine<PlayerBaseState> Fsm;
        protected PlayerController Controller;
        protected Animator Animator;

        protected PlayerBaseState(StateMachine<PlayerBaseState> fsm, PlayerController controller, Animator animator)
        {
            Fsm = fsm;
            Controller = controller;
            Animator = animator;
        }
        
        public virtual void FixedUpdate() { }

        public virtual void OnEnter() { }

        public virtual void OnExit() { }

        public virtual void Update() { }
    }
}
