using Paint.Visitors;
using System;
using System.Drawing;

namespace Paint.Model
{
    public class Shape : Drawable
    {
        #region FIELDS
        private Size _size;
        #endregion

        #region PUBLIC MEMBERS
        public override Size Size
        {
            get => _size;
        }

        public Rectangle Base  => new Rectangle(this.AbsoluteOrigin, this.Size);
        #endregion

        #region CONSTRUCTOR
        public Shape(ShapeType type, Point origin, Size size) 
            : base(type, origin)
        {
            _size = size;
        }

        public Shape(ShapeType type, Point origin, Size size, Guid id) 
            : base(type, origin, id)
        {
            _size = size;
        }

        public Shape(ShapeType type, Rectangle rectangle)
            : this(type, rectangle.Location, rectangle.Size)
        { }

        public Shape(ShapeType type, Rectangle rectangle, Guid id)
            : this(type, rectangle.Location, rectangle.Size, id)
        { }

        #endregion

        #region METHODS
        public void SetSize(Size newSize)
        {
            _size = newSize;
            this.OnPropertyChanged("Size");
        }

        public void ApplyRectangle(Rectangle shapeBase)
        {
            _origin = shapeBase.Location;
            _size = shapeBase.Size;
            this.OnPropertyChanged("Base");
        }

        #region Interface IVisitable
        public override void Accept(IVisitor visitor)
        {
            visitor.Visit(this);
        }
        #endregion
        #endregion
    }
}
