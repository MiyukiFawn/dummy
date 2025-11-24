using System;

namespace StateMachine
{
    public class StateLayer<T> where T : State<T>
    {
        public readonly string Name;
        public readonly T Root;
        public readonly int Priority;

        /// <summary>
        /// True if the layer is active and should be updated/evaluated.
        /// </summary>
        public bool IsActive { get; private set; }

        public StateLayer(string name, T root, int priority = 1)
        {
            if (name == null)
                throw new ArgumentNullException(nameof(name));

            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Layer name cannot be empty", nameof(name));

            if (priority < 1)
                throw new ArgumentOutOfRangeException(nameof(priority), priority, "Value cannot be lower than 1");

            Name = name;
            Root = root;
            Priority = priority;
            IsActive = true;
        }

        /// <summary>
        /// Activates or deactivates the layer.
        /// </summary>
        public void ToggleLayer() => IsActive = !IsActive;
        
        public override string ToString()
        {
            return $"{Name} (Priority {Priority}) Active: {IsActive}";
        }
    }
}