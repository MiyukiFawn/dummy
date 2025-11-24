namespace StateMachine
{
    public static class StateHelper
    {
        /// <summary>
        /// Returns the deepest active state in a branch.
        /// </summary>
        public static T GetStateLeaf<T>(T parent) where T : State<T>
        {
            T current = parent;
            while (current.ActiveChild != null) current = current.ActiveChild;

            return current;
        }

        /// <summary>
        /// Returns a readable string of the currently active path.
        /// </summary>
         public static string GetActivePath<T>(StateLayer<T> layer) where T : State<T>
        {
            System.Text.StringBuilder sb = new();
            T curr = layer.Root;

            sb.Append(layer.Name);
            
            while (curr != null)
            {
                sb.Append(curr.GetType().Name);
                if (curr.ActiveChild != null)
                    sb.Append(" > ");

                curr = curr.ActiveChild;
            }

            return sb.ToString();
        }
    }
}