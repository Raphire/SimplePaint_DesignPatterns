using Paint.Composite;
using Paint.Strategy;
using Paint.Visitors;
using System.Drawing;
using System.Linq;

namespace Paint.Model
{
    public class DrawableGroup : ParentNode<IDrawable>, IDrawable
    {
        protected Point _origin;

        new public DrawableGroup Parent => _parent as DrawableGroup;

        public DrawableGroup(Point origin) : base()
        {
            _origin = origin;
        }

        #region Interface IDrawable
        public Size Size => GetContentBoundingRectangle().Size;

        public ShapeType Type => ShapeType.Group;

        public Point Origin
        {
            get => _origin;
            set
            {
                _origin = value;
                this.OnPropertyChanged();
            }
        }

        public Point AbsoluteOrigin => _parent == null ? Origin
            : new Point(Parent.AbsoluteOrigin.X + _origin.X, Parent.AbsoluteOrigin.Y + _origin.Y);

        public Point Minimum => _children.Any() ? new Point(_children.Min(c => c.Minimum.X), _children.Min(c => c.Minimum.Y)) : AbsoluteOrigin;
        public Point Maximum => _children.Any() ? new Point(_children.Max(c => c.Maximum.X), _children.Max(c => c.Maximum.Y)) : AbsoluteOrigin;

        public IDrawStrategy DrawStrategy => new GroupDrawStrategy();

        public bool ContainsPoint(Point point)
        {
            return Children.Any(c => c.ContainsPoint(point));
        }

        public Rectangle GetContentBoundingRectangle()
        {
            Point min = Minimum, max = Maximum;

            return new Rectangle(min.X, min.Y, max.X - min.X, max.Y - min.Y);
        }
        #endregion

        #region Interface IVisitable
        public void Accept(IVisitor visitor)
        {
            visitor.Visit(this);
        }
        #endregion
    }
}
