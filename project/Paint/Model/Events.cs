using System;

namespace Paint.Model
{
    public static class Events
    {
        public class SelectionChangedEventArgs<T> : EventArgs
        {
            public SelectionChangedEventArgs(params T[] changed)
            {
                Changed = changed;
            }

            public T[] Changed { get; }
        }

        public class BranchModifiedEventArgs : EventArgs
        {
            public object SourceEventSender { get; }
            public EventArgs SourceEventArgs { get; }
            public int Iteration { get; }

            public BranchModifiedEventArgs(object sourceSender, EventArgs sourceArgs, int iteration = 1)
            {
                if (iteration < 1) throw new ArgumentOutOfRangeException("Iteration should be 1 or higher");

                SourceEventSender = sourceSender;
                SourceEventArgs = sourceArgs;

                Iteration = iteration;
            }
        }
    }
}
