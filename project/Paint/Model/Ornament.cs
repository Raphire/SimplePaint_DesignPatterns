using Paint.Composite;
using Paint.Strategy;
using Paint.Visitors;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;

namespace Paint.Model
{
    public class GroupOrnament : Ornament, IParentNode<IDrawable>, IDecorator<IParentNode<IDrawable>>
    {
        public IDrawable this[Guid id] => Target[id];

        public new IParentNode<IDrawable> Target => (IParentNode<IDrawable>) _target;

        public IReadOnlyEventList<IDrawable> Children => Target.Children;

        IParentNode<IDrawable> IDecorator<IParentNode<IDrawable>>.EndPoint => (IParentNode<IDrawable>) base.EndPoint;

        IReadOnlyList<INodeBase> IParentNode.Children => ((IParentNode)Target).Children;

        public event EventHandler<Events.BranchModifiedEventArgs> BranchModified
        {
            add => Target.BranchModified += value;
            remove => Target.BranchModified -= value;
        }

        public event EventHandler<ContentChangedEventArgs<IDrawable>> ContentChanged
        {
            add => Target.ContentChanged += value;
            remove => Target.ContentChanged -= value;
        }

        public GroupOrnament(IParentNode<IDrawable> target, string text, Side decoratedSide)
            : base(target as IDrawable, text, decoratedSide)
        {}

        public void Add(params IDrawable[] content)
        {
            Target.Add(content);
        }

        public void Clear()
        {
            Target.Clear();
        }

        public IDrawable FindNode(Guid id)
        {
            return Target.FindNode(id);
        }

        public IReadOnlyList<IDrawable> GetAllDescendants()
        {
            return Target.GetAllDescendants();
        }

        public void Remove(params IDrawable[] content)
        {
            Target.Remove(content);
        }

        public void Remove(params Guid[] contentIDs)
        {
            Target.Remove(contentIDs);
        }
    }

    public class Ornament : IDrawable, IDecorator<IDrawable>
    {
        public enum Side
        {
            Left, Right, Top, Bottom
        }

        #region IDecorator
        protected readonly IDrawable _target;

        public IDrawable Target { get => _target; }

        public IDrawable EndPoint {
            get
            {
                IDrawable result = Target;
                while (result is Ornament) result = (result as Ornament).Target;
                return result;
            }
        }
        #endregion

        public Side DecoratedSide { get; set; }

        public string Text { get; set; }

        public Point Origin
        {
            get => Target.Origin;
            set => Target.Origin = value;
        }

        public ShapeType Type => ShapeType.Ornament;

        public Point AbsoluteOrigin => Target.AbsoluteOrigin;

        public Point Minimum => Target.Minimum;
        public Point Maximum => Target.Maximum;

        public Size Size => Target.Size;

        public Guid ID => Target.ID;

        public IDrawStrategy DrawStrategy => new OrnamentDrawStrategy();

        public int Depth => Target.Depth;

        public IParentNode<IDrawable> Parent
        {
            get => _target.Parent;
            set => _target.Parent = value;
        }
        IParentNode INodeBase.Parent { get => Parent; set => Parent = (IParentNode<IDrawable>) value; }

        public IParentNode<IDrawable> Root => _target.Root;

        IParentNode INodeBase.Root => ((INodeBase)_target).Root;

        public Ornament(IDrawable target, string text, Side decoratedSide)
        {
            _target = target;
            this.DecoratedSide = decoratedSide;
            this.Text = text;
        }

        public event PropertyChangedEventHandler PropertyChanged
        {
            add
            {
                _target.PropertyChanged += value;
            }

            remove
            {
                _target.PropertyChanged -= value;
            }
        }

        public bool ContainsPoint(Point point)
        {
            return Target.ContainsPoint(point);
        }

        public void Accept(IVisitor visitor)
        {
            Target.Accept(visitor);
        }
    }
}
