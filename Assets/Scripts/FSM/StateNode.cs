using System.Collections.Generic;
using FSM.Interfaces;

namespace FSM
{
    public class StateNode<T> where T : IState
    {
        public T State { get; }
        public HashSet<ITransition<T>> Transitions { get; }

        public StateNode(T state)
        {
            State = state;
            Transitions = new HashSet<ITransition<T>>();
        }

        public void AddTransition(T targetState, IPredicate predicate)
        {
            Transitions.Add(new Transition<T>(targetState, predicate));
        }
    }
}