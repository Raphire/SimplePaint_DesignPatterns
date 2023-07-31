using System;
using System.Collections.Generic;
using System.Linq;

namespace Paint.Composite
{
    public interface INotifyContentChanged<T>
    {
        event EventHandler<ContentChangedEventArgs<T>> ContentChanged;
    }

    public class ContentChangedEventArgs<T> : EventArgs
    {
        [Flags]
        public enum Type
        {
            None = 0,
            Added = 1,
            Removed = 2,
            Cleared = 4
        }

        protected T[] _oldContent, _newContent;

        public ContentChangedEventArgs(T[] oldContent, T[] newContent)
        { _oldContent = oldContent; _newContent = newContent; }

        public Type ChangeType => ContentAdded.Any() ? Type.Added : Type.None |
            (ContentRemoved.Any() ? Type.Removed : Type.None);

        public IEnumerable<T> ContentBefore => _oldContent;
        public IEnumerable<T> ContentAfter => _newContent;

        public IEnumerable<T> ContentAdded => _newContent.Except(_oldContent);
        public IEnumerable<T> ContentRemoved => _oldContent.Except(_newContent);
        public IEnumerable<T> ContentDiff => _oldContent.Concat(_newContent);
    }

    public class ContentChangedEventArgs : ContentChangedEventArgs<object>
    {
        public ContentChangedEventArgs(object[] oldContent, object[] newContent) 
            : base(oldContent, newContent) { }

    }
}
