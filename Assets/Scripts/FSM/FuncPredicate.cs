using System;
using FSM.Interfaces;

namespace FSM
{
    public class FuncPredicate : IPredicate
    {
        private readonly Func<bool> _function;

        public FuncPredicate(Func<bool> function)
        {
            _function = function;
        }

        public bool Evaluate() => _function.Invoke();
    }
}
