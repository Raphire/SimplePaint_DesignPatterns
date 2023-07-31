using System;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Paint.Composite
{
    public abstract class Node<T> : INodeBase<T>
        where T : INodeBase
    {
        #region EVENTHANDLERS
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion

        #region FIELDS
        protected Guid _id;
        protected IParentNode<T> _parent;
        #endregion

        public Node(Guid id)
        {
            _id = id;
        }

        public Node()
            : this(Guid.NewGuid())
        {}
        #region PUBLIC MEMBERS
        public Guid ID => _id;

        public IParentNode<T> Parent 
        {
            get => _parent;
            set
            {
                if (_parent != null)
                {
                    if (_parent.Children.Any(c => c.ID == this.ID))
                    {
                        throw new ArgumentException("Child is still present in parent node");
                    }
                }

                if (value != null)
                {
                    if (!value.Children.Any(c => c.ID == this.ID))
                    {
                        throw new ArgumentException("Child is not present in new parent node");
                    }
                }

                _parent = value;
            }
        }
        IParentNode INodeBase.Parent
        {
            get => _parent;
            set => Parent = (IParentNode<T>)value;
        }

        public IParentNode<T> Root => (IParentNode<T>)(Parent?.Root);
        IParentNode INodeBase.Root => this is IParentNode && Parent == null 
            ? this as IParentNode : Parent?.Root;

        public int Depth => _parent == null ? 0 : _parent.Depth + 1;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}
