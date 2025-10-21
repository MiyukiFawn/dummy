namespace FSM.Interfaces
{
    public interface ITransition<out T> where T : IState
    {
        T TargetState { get; }
        IPredicate Predicate { get; }
    }
}