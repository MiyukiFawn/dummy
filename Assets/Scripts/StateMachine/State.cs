using System;
using System.Collections.Generic;

namespace StateMachine
{
    /// <summary>
    /// Represents a node in a hierarchical finite state machine (HFSM).
    /// A state may contain child states, have a parent state, and define
    /// behavior for entering, exiting, and updating.
    /// </summary>
    public abstract class State<T> where T : State<T>
    {
        /// <summary>
        /// Reference to the finite state machine that owns this state.
        /// </summary>
        private readonly FiniteStateMachine<T> _machine;

        /// <summary>
        /// Collection of child states indexed by their concrete type.
        /// </summary>
        public readonly Dictionary<Type, T> Children;
        
        /// <summary>
        /// The parent state of this state, or <c>null</c> if this is a root state.
        /// </summary>
        public T Parent;
        
        /// <summary>
        /// The currently active child state of this state, or <c>null</c> if this is a leaf state.
        /// </summary>
        public T ActiveChild;

        /// <summary>
        /// Creates a new state instance with a reference to the owning state machine
        /// and an optional parent state in the hierarchy.
        /// </summary>
        /// <param name="machine">The state machine that owns this state.</param>
        protected State(FiniteStateMachine<T> machine)
        {
            _machine = machine;
            Children = new Dictionary<Type, T>();
        }

        /// <summary>
        /// Adds a child state to this state by setting its parent reference
        /// and storing it in the <see cref="Children"/> dictionary keyed by its type.
        /// </summary>
        /// <param name="child">The child state to add.</param>
        public T AddChild(T child)
        {
            child.Parent = this as T;
            Children.Add(child.GetType(), child);
            return this as T;
        }
        
        /// <summary>
        /// Called when the state becomes active.
        /// Override this method to implement enter logic.
        /// </summary>
        public virtual void OnEnter() {}
        
        /// <summary>
        /// Called when the state stops being active.
        /// Override this method to implement exit logic.
        /// </summary>
        public virtual void OnExit() {}
        
        /// <summary>
        /// Called once per update cycle while the state is active.
        /// Override this to implement state-specific update logic.
        /// </summary>
        /// <param name="deltaTime">Time elapsed since the previous update.</param>
        public virtual void Update(float deltaTime) {}

        /// <summary>
        /// Returns the type of the default initial child state for this state.
        /// Returning <c>null</c> when the state is the last on the chain.
        /// </summary>
        public virtual Type GetInitialChild() => null;

        /// <summary>
        /// Requests a transition from this state to another sibling state of type <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="TTarget">The sibling state type to transition to.</typeparam>
        protected void RequestTransition<TTarget>() where TTarget : T => _machine.ChangeState<TTarget>(this as T);
    }
}