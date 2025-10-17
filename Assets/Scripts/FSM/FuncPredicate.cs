using System;

public class FuncPredicate : IPredicate
{
    private readonly Func<bool> function;

    public FuncPredicate(Func<bool> function)
    {
        this.function = function;
    }

    public bool Evaluate() => function.Invoke();
}
