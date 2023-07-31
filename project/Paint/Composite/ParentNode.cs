using System;
using System.Collections.Generic;
using System.Linq;

using static Paint.Model.Events;

namespace Paint.Composite
{
    public abstract class ParentNode<T> : Node<T>, IParentNode<T>
        where T : INodeBase
    {
        protected EventList<T> _children = new EventList<T>();

        public event EventHandler<BranchModifiedEventArgs> BranchModified;
        public event EventHandler<ContentChangedEventArgs<T>> ContentChanged;

        public IReadOnlyEventList<T> Children { get => new ReadOnlyEventList<T>(_children); }

        IReadOnlyList<INodeBase> IParentNode.Children => (IReadOnlyList<INodeBase>) Children;

        public T this[Guid id] => _children.Where(c => c.ID == id).Single();

        public ParentNode() : base()
        {
            _children.ContentChanged += EventList_ContentChanged;
        }

        public void Add(params T[] content)
        {
            // Filter invalid values from argument 'drawables' to avoid duplicate child references and other issues
            T[] valid = content.Distinct()
                .Where(d => (d.Parent == null || d.Parent.ID != this.ID) && d.ID != this.ID).ToArray();

            // Group drawables by current parent object so they can be simultaneously removed from their parents
            var groupedByParent = valid.GroupBy(d => d.Parent).ToArray();

            foreach (var grouping in groupedByParent)
            {
                // Remove drawables from their old parents simultaneously
                var ids = grouping.Select(v => v.ID).ToArray();
                var p = grouping.Key as ParentNode<T>;
                if (grouping.Key != null) p.Remove(ids);
            }

            // Add filtered drawables (diff) to _children
            _children.AddRange(valid);

            foreach (INodeBase c in valid)
            {
                c.Parent = this;
                SubscribeBranchHandlers((T)c);
            }
        }

        public void Remove(params T[] content)
        {
            Remove(content.Select(c => c.ID).ToArray());
        }

        public void Remove(params Guid[] contentIDs)
        {
            T[] valid = _children.Where(c => contentIDs.Contains(c.ID)).ToArray();

            foreach (T c in valid)
            {
                _children.Remove(c);
                c.Parent = null;
                UnSubscribeBranchHandlers(c);
            }
        }

        public void Clear()
        {
            T[] diff = _children.ToArray();

            _children.Clear();

            foreach (T c in diff) c.Parent = null;
        }

        public IReadOnlyList<T> GetAllDescendants()
        {
            var childrenOfChildren = this.Children.Where(c => c is IParentNode<T>)
                .Select(c => c as IParentNode<T>)
                .SelectMany(p => p.GetAllDescendants());

            return this.Children.Concat(childrenOfChildren).ToList();
        }

        public T FindNode(Guid id)
        {
            return GetAllDescendants().Where(d => d.ID == id).SingleOrDefault();
        }

        private void SubscribeBranchHandlers(T target)
        {
            if (target is T)
            {
                T node = target;
                node.PropertyChanged += Child_EventRaised;
            }

            if (target is IParentNode<T>)
            {
                IParentNode<T> group = target as IParentNode<T>;
                group.ContentChanged += Child_EventRaised;
                group.BranchModified += Child_EventRaised;
            }
        }

        private void UnSubscribeBranchHandlers(T target)
        {
            if (target is T)
            {
                T node = target;
                node.PropertyChanged -= Child_EventRaised;
            }

            if (target is IParentNode<T>)
            {
                IParentNode<T> group = target as IParentNode<T>;
                group.ContentChanged -= Child_EventRaised;
                group.BranchModified -= Child_EventRaised;
            }
        }

        private void EventList_ContentChanged(object sender, ContentChangedEventArgs<T> e)
        {
            ContentChanged?.Invoke(this,
                new ContentChangedEventArgs<T>(e.ContentBefore.ToArray(), e.ContentAfter.ToArray()));
        }

        private void Child_EventRaised(object sender, EventArgs args)
        {
            if (args is BranchModifiedEventArgs)
            {
                BranchModifiedEventArgs bmeArgs = args as BranchModifiedEventArgs;

                BranchModified?.Invoke(
                    this,
                    new BranchModifiedEventArgs(
                        bmeArgs.SourceEventSender,
                        bmeArgs.SourceEventArgs,
                        bmeArgs.Iteration + 1
                    ));
            }
            else
            {
                BranchModified?.Invoke(this,
                    new BranchModifiedEventArgs(sender, args, 1));
            }
        }

    }
}
