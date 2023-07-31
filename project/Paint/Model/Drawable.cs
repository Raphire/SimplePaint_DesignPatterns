using System;
using System.Drawing;
using Paint.Composite;
using Paint.Strategy;
using Paint.Visitors;

namespace Paint.Model
{
    /// <summary>
    /// Enum defining available types of shapes
    /// </summary>
    public enum ShapeType : int
    {
        Ellipse = 1,
        Rectangle = 0,
        Group = 2,
        Ornament = 3
    }

    public abstract class Drawable 
        : Node<IDrawable>, IDrawable
    {
        #region FIELDS
        protected readonly ShapeType _type;
        protected Point _origin;
        #endregion

        #region PUBLIC MEMBERS
        // Position relative to parent 
        new public DrawableGroup Parent => (DrawableGroup)_parent;

        public Point Origin {
            get => _origin;
            set
            {
                _origin = value;
                this.OnPropertyChanged();
            }
        }

        public abstract Size Size { get; }
        
        public Point AbsoluteOrigin => _parent == null ? Origin 
            : new Point(Parent.AbsoluteOrigin.X + _origin.X, Parent.AbsoluteOrigin.Y + _origin.Y);

        public Point Minimum => AbsoluteOrigin;
        public Point Maximum => Minimum + Size;

        public ShapeType Type { get => _type; }

        public virtual IDrawStrategy DrawStrategy 
        {
            get {
                switch (this.Type)
                {
                    case ShapeType.Ellipse: return new EllipseDrawStrategy();
                    case ShapeType.Rectangle: return new RectangleDrawStrategy();
                    case ShapeType.Group: return new GroupDrawStrategy();
                    default: throw new Exception("Unknown ShapeType '" + this.Type + "'");
                }
            }
        }
        #endregion

        #region CONSTRUCTOR
        public Drawable(ShapeType type, Point origin)
            : base()
        {
            _origin = origin;
            _type = type;
        }

        public Drawable(ShapeType type, Point origin, Guid id)
            : base(id)
        {
            _origin = origin;
            _type = type;
        }
        #endregion

        #region METHODS
        /// <summary>
        /// Checks whether the provided point is inside the shape
        /// </summary>
        /// <param name="point">Point</param>
        /// <returns>Boolean</returns>
        public virtual bool ContainsPoint(Point point)
        {
            return point.X >= this.AbsoluteOrigin.X &&
                    point.X <= this.AbsoluteOrigin.X + this.Size.Width &&
                    point.Y >= this.AbsoluteOrigin.Y &&
                    point.Y <= this.AbsoluteOrigin.Y + this.Size.Height;
        }

        public abstract void Accept(IVisitor visitor);
        #endregion
    }
}
