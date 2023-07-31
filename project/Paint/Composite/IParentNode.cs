using System;
using System.Collections.Generic;
using static Paint.Model.Events;

namespace Paint.Composite
{
    public interface IParentNode : INodeBase
    {
        IReadOnlyList<INodeBase> Children { get; }
    }

    public interface IParentNode<T> : IParentNode, INotifyContentChanged<T>
        where T : INodeBase
    {
        event EventHandler<BranchModifiedEventArgs> BranchModified;

        T this[Guid id] { get; }

        new IReadOnlyEventList<T> Children { get; }
        IReadOnlyList<T> GetAllDescendants();
        T FindNode(Guid id);

        void Add(params T[] content);
        void Remove(params T[] content);
        void Remove(params Guid[] contentIDs);
        void Clear();
    }
}
