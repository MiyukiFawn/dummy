public interface ITransition<T> where T : IState
{
    T TargetState { get; }
    IPredicate Predicate { get; }
}