using System;
using System.Collections.Generic;
using System.Linq;
using FSM.Interfaces;

namespace FSM
{
    public class StateMachine<T> where T : IState
    {
        private StateNode<T> _current;
        private readonly Dictionary<Type, StateNode<T>> _nodes = new();
        private readonly HashSet<ITransition<T>> _anyTransitions = new();

        public void Update()
        {
            ITransition<T> transition = GetTransition();
            if (transition != null) ChangeState(transition.TargetState);

            _current.State?.Update();
        }

        public void FixedUpdate()
        {
            _current.State?.FixedUpdate();
        }

        public void SetState(T state)
        {
            _current = _nodes[state.GetType()];
            _current.State.OnEnter();
        }

        public void ChangeState(T newState)
        {
            if (newState.GetType() == _current.State.GetType()) return;

            T previousState = _current.State;
            T nextState = _nodes[newState.GetType()].State;

            previousState?.OnExit();
            nextState?.OnEnter();

            _current = _nodes[newState.GetType()];
        }

        private ITransition<T> GetTransition()
        {
            foreach (ITransition<T> transition in _anyTransitions.Where(transition => transition.Predicate.Evaluate()))
                return transition;

            return _current.Transitions.FirstOrDefault(transition => transition.Predicate.Evaluate());
        }

        public void AddTransition(T from, T to, IPredicate condition)
        {
            StateNode<T> fromNode = GetOrAddNode(from);
            StateNode<T> toNode = GetOrAddNode(to);

            fromNode.AddTransition(toNode.State, condition);
        }

        public void AddAnyTransition(T to, IPredicate condition)
        {
            Transition<T> transition = new(to, condition);
            _anyTransitions.Add(transition);
        }

        private StateNode<T> GetOrAddNode(T state)
        {
            StateNode<T> node = _nodes.GetValueOrDefault(state.GetType());

            if (node != null) return node;
            node = new StateNode<T>(state);
            _nodes.Add(state.GetType(), node);

            return node;
        }
    }
}