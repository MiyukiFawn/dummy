using System;
using System.Collections.Generic;

public class StateMachine<T> where T : IState
{
    private StateNode<T> Current;
    private readonly Dictionary<Type, StateNode<T>> Nodes = new();
    private readonly HashSet<ITransition<T>> AnyTransitions = new();
    
    public void Update()
    {
        ITransition<T> transition = GetTransition();
        if (transition != null) ChangeState(transition.TargetState);

        Current.State?.Update();
    }

    public void FixedUpdate()
    {
        Current.State?.FixedUpdate();
    }

    public void SetState(T state)
    {
        Current = Nodes[state.GetType()];
        Current.State.OnEnter();
    }

    public void ChangeState(T newState)
    {
        if (newState.GetType() == Current.State.GetType()) return;

        T previousState = Current.State;
        T nextState = Nodes[newState.GetType()].State;

        previousState?.OnExit();
        nextState?.OnEnter();

        Current = Nodes[newState.GetType()];
    }

    ITransition<T> GetTransition()
    {
        foreach (var transition in AnyTransitions) if (transition.Predicate.Evaluate()) return transition;
        foreach (var transition in Current.Transitions) if (transition.Predicate.Evaluate()) return transition;

        return null;
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
        AnyTransitions.Add(transition);
    }

    StateNode<T> GetOrAddNode(T state)
    {
        StateNode<T> node = Nodes.GetValueOrDefault(state.GetType());

        if (node == null)
        {
            node = new StateNode<T>(state);
            Nodes.Add(state.GetType(), node);
        }

        return node;
    }
}