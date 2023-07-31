using System;
using System.Collections;
using System.Collections.Generic;

namespace Paint.Composite
{
    public interface IEventList<T> : IList<T>, INotifyContentChanged<T>, IEnumerable
    {
        void AddRange(IEnumerable<T> collection);
    }

    public interface IReadOnlyEventList<T> : INotifyContentChanged<T>, IReadOnlyList<T>
    {
    }

    public class EventList<T> : IEventList<T>
    {
        protected List<T> _children = new List<T>();

        public T this[int index] { get => _children[index]; set => _children[index] = value; }

        public int Count => _children.Count;

        public bool IsReadOnly => ((ICollection<T>)_children).IsReadOnly;

        public event EventHandler<ContentChangedEventArgs<T>> ContentChanged;

        public void AddRange(IEnumerable<T> collection)
        {
            T[] before = _children.ToArray();
            _children.AddRange(collection);
            ContentChanged?.Invoke(this, new ContentChangedEventArgs<T>(before, _children.ToArray()));
        }

        public void Add(T item)
        {
            T[] before = _children.ToArray();
            ((ICollection<T>)_children).Add(item);
            ContentChanged?.Invoke(this, new ContentChangedEventArgs<T>(before, _children.ToArray()));
        }

        public void Clear()
        {
            T[] before = _children.ToArray();
            ((ICollection<T>)_children).Clear();
            ContentChanged?.Invoke(this, new ContentChangedEventArgs<T>(before, _children.ToArray()));
        }

        public void Insert(int index, T item)
        {
            T[] before = _children.ToArray();
            ((IList<T>)_children).Insert(index, item);
            ContentChanged?.Invoke(this, new ContentChangedEventArgs<T>(before, _children.ToArray()));
        }

        public bool Remove(T item)
        {
            T[] before = _children.ToArray();
            bool result = ((ICollection<T>)_children).Remove(item);
            ContentChanged?.Invoke(this, new ContentChangedEventArgs<T>(before, _children.ToArray()));

            return result;
        }

        public void RemoveAt(int index)
        {
            T[] before = _children.ToArray();
            ((IList<T>)_children).RemoveAt(index);
            ContentChanged?.Invoke(this, new ContentChangedEventArgs<T>(before, _children.ToArray()));
        }

        public bool Contains(T item)
        {
            return ((ICollection<T>)_children).Contains(item);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            ((ICollection<T>)_children).CopyTo(array, arrayIndex);
        }

        public IEnumerator<T> GetEnumerator()
        {
            return ((IEnumerable<T>)_children).GetEnumerator();
        }

        public int IndexOf(T item)
        {
            return ((IList<T>)_children).IndexOf(item);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)_children).GetEnumerator();
        }
    }

    public class ReadOnlyEventList<T> : IReadOnlyEventList<T>
    {
        private IEventList<T> _wrappedList;

        public ReadOnlyEventList(EventList<T> core)
        {
            _wrappedList = core;
        }

        public T this[int index] => _wrappedList[index];

        public int Count => _wrappedList.Count;

        public event EventHandler<ContentChangedEventArgs<T>> ContentChanged
        {
            add => _wrappedList.ContentChanged += value;
            remove => _wrappedList.ContentChanged -= value;
        }

        public IEnumerator<T> GetEnumerator() => _wrappedList.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => _wrappedList.GetEnumerator();
    }
}
