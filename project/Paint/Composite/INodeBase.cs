using System;
using System.ComponentModel;

namespace Paint.Composite
{
    public interface INodeBase : INotifyPropertyChanged
    {
        Guid ID { get; }
        IParentNode Parent { get; set; }
        IParentNode Root { get; }
        int Depth { get; }
    }

    public interface INodeBase<T> : INodeBase
        where T : INodeBase
    {
        new IParentNode<T> Parent { get; set; }
        new IParentNode<T> Root { get; }
    }
}
