using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace StateMachine
{
    /// <summary>
    /// Represents a hierarchical finite state machine composed of one or more
    /// independently updated state layers. Each layer maintains its own root
    /// state and active state chain. The machine is responsible for initializing
    /// layers, performing updates, and handling state transitions between
    /// sibling states.
    /// </summary>
    public class FiniteStateMachine<T> where T : State<T>
    {
        /// <summary>
        /// Internal list of state layers managed by the state machine.
        /// </summary>
        private List<StateLayer<T>> _layers = new List<StateLayer<T>>();

        /// <summary>
        /// Adds a new state layer to the state machine.
        /// </summary>
        /// <param name="layer">The layer to be added.</param>
        public FiniteStateMachine<T> AddLayer(StateLayer<T> layer)
        {
            _layers.Add(layer);
            return this;
        }

        /// <summary>
        /// Initializes the state machine by sorting its layers, initializing each
        /// layer's state hierarchy, and entering the active state chain starting
        /// from each layer's root state.
        /// </summary>
        public FiniteStateMachine<T> Initialize()
        {
            SortLayers();
            foreach (StateLayer<T> layer in _layers)
            {
                InitializeStateBranch(layer.Root);
                EnterState(layer.Root);
            }

            return this;
        }

        /// <summary>
        /// Updates each active state in every layer. Traverses from each layer's root state
        /// down through its active child chain, calling <c>Update</c> on each state.
        /// </summary>
        /// <param name="deltaTime">The time elapsed since the last frame.</param>
        public void Update(float deltaTime)
        {
            foreach (StateLayer<T> layer in _layers)
            {
                if (!layer.IsActive) continue;
                T current = layer.Root;

                while (current != null)
                {
                    current.Update(deltaTime);
                    current = current.ActiveChild;
                }

                Debug.Log(StateHelper.GetActivePath(layer));
            }
        }

        /// <summary>
        /// Changes from the current state to a sibling state of the specified type.
        /// </summary>
        /// <typeparam name="TTarget">The type of the target sibling state to transition to.</typeparam>
        /// <param name="from">The current state requesting the transition.</param>
        /// <exception cref="ArgumentNullException">
        /// Thrown when the source state is null.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// Thrown when attempting to change to the same state type, or when the requesting state is the root.
        /// </exception>
        /// <exception cref="KeyNotFoundException">
        /// Thrown when the target sibling state does not exist.
        /// </exception>
        public void ChangeState<TTarget>(T from) where TTarget : State<T>
        {
            if (from == null) throw new ArgumentNullException(nameof(from), "Requesting state cannot be null");
            if (from.GetType() == typeof(TTarget))
                throw new InvalidOperationException(
                    $"Cannot transition from state '{typeof(TTarget).Name}' to the same state type.");
            if (from.Parent == null)
                throw new InvalidOperationException("The root state cannot request a transition.");
            if (!from.Parent.Children.ContainsKey(typeof(TTarget)))
                throw new KeyNotFoundException(
                    $"The state '{typeof(TTarget).Name}' is not a sibling of '{from.GetType().Name}'.");

            ExitState(from);

            T to = from.Parent.Children[typeof(TTarget)];
            InitializeStateBranch(to);

            EnterState(to);

            from.Parent.ActiveChild = to;
        }

        /// <summary>
        /// Sorts the state machine layers by their priority, preserving original order
        /// when priorities are equal.
        /// </summary>
        private void SortLayers()
        {
            _layers = _layers
                .Select((layer, index) => new { layer, index })
                .OrderBy(x => x.layer.Priority)
                .ThenBy(x => x.index)
                .Select(x => x.layer)
                .ToList();
        }

        /// <summary>
        /// Initializes a state branch by following each state's configured initial child
        /// and activating it, recursively descending until the leaf state.
        /// </summary>
        /// <param name="target">The starting state whose branch should be initialized.</param>
        private static void InitializeStateBranch(T target)
        {
            T current = target;
            while (current.GetInitialChild() != null)
            {
                T targetChild = current.Children[current.GetInitialChild()];
                current.ActiveChild = targetChild;
                current = targetChild;
            }
        }

        /// <summary>
        /// Enters the specified state hierarchy by invoking <c>OnEnter</c> on the root
        /// state and then on each active child state recursively until the leaf state.
        /// </summary>
        /// <param name="root">The root state from which the enter sequence begins.</param>
        private static void EnterState(T root)
        {
            T current = root;
            while (current != null)
            {
                current.OnEnter();
                current = current.ActiveChild;
            }
        }

        /// <summary>
        /// Exits the specified state hierarchy by invoking <c>OnExit</c> on the leaf
        /// state and each of its ancestors, stopping before reaching the root's parent state.
        /// </summary>
        /// <param name="root">The root state from which the exit sequence begins.</param>
        private static void ExitState(T root)
        {
            T leaf = StateHelper.GetStateLeaf(root) as T;

            T current = leaf;
            while (current != root.Parent)
            {
                current.OnExit();
                current = current.Parent;
            }
        }
    }
}