public class Transition<T> : ITransition<T> where T : IState
{
    public T TargetState { get; }

    public IPredicate Predicate { get; }

    public Transition(T targetState, IPredicate predicate)
    {
        TargetState = targetState;
        Predicate = predicate;
    }
}