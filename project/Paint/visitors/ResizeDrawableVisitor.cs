using Paint.Model;
using System;
using System.Drawing;

namespace Paint.Visitors
{
    public class ResizeDrawableVisitor : IVisitor
    {
        private Point _newOrigin;
        private Size _newSize;

        public ResizeDrawableVisitor(Point newOrigin, Size newDimensions)
        {
            _newOrigin = newOrigin;
            _newSize = newDimensions;
        }

        public ResizeDrawableVisitor(Rectangle r, Size delta, ResizeOperation resizeBoxIndex)
        {
            Rectangle n = ResizeDirectional(r, delta, resizeBoxIndex);

            _newOrigin = n.Location;
            _newSize = n.Size;
        }

        public void Visit(Shape shape)
        {
            shape.ApplyRectangle(new Rectangle(_newOrigin, _newSize));
        }

        public void Visit(DrawableGroup group)
        {
            throw new InvalidOperationException("Groups can not be resized");
        }

        public void Visit(Ornament ornament)
        {
            IDrawable rootTarget = ornament.Target;
            while(rootTarget is Ornament)
            {
                rootTarget = (rootTarget as Ornament).Target;
            }

            rootTarget.Accept(this);
        }

        /// <summary>
        /// Resizes the selected shape based on mouse movement.
        /// </summary>
        public Rectangle ResizeDirectional(Rectangle r, Size delta, ResizeOperation direction)
        {
            Size newSize;
            Point newOrigin;

            switch (direction)
            {
                case ResizeOperation.NW: // Border box @ NW corner
                    newOrigin = new Point(r.Location.X + Math.Min(delta.Width, r.Size.Width), r.Location.Y + Math.Min(delta.Height, r.Size.Height));
                    newSize = new Size(Math.Abs(r.Size.Width - delta.Width), Math.Abs(r.Size.Height - delta.Height));

                    break;

                case ResizeOperation.SE: // Border box @ SE corner
                    newOrigin = new Point(r.Location.X + Math.Min(0, delta.Width + r.Size.Width), r.Location.Y + Math.Min(0, delta.Height + r.Size.Height));
                    newSize = new Size(Math.Abs(r.Size.Width + delta.Width), Math.Abs(r.Size.Height + delta.Height));

                    break;

                case ResizeOperation.NE: // Border box @ NE corner
                    newOrigin = new Point(r.Location.X + Math.Min(0, delta.Width + r.Size.Width), r.Location.Y + Math.Min(delta.Height, r.Size.Height));
                    newSize = new Size(Math.Abs(r.Size.Width + delta.Width), Math.Abs(r.Size.Height - delta.Height));

                    break;

                case ResizeOperation.SW: // Border box @ SW corner
                    newOrigin = new Point(r.Location.X + Math.Min(delta.Width, r.Size.Width), r.Location.Y + Math.Min(0, delta.Height + r.Size.Height));
                    newSize = new Size(Math.Abs(r.Size.Width - delta.Width), Math.Abs(r.Size.Height + delta.Height));

                    break;

                case ResizeOperation.N: // Border box @ N edge
                    newOrigin = new Point(r.Location.X + 0, r.Location.Y + Math.Min(delta.Height, r.Size.Height));
                    newSize = new Size(r.Size.Width - 0, Math.Abs(r.Size.Height - delta.Height));

                    break;

                case ResizeOperation.S: // Border box @ S edge
                    newOrigin = new Point(r.Location.X + 0, r.Location.Y + Math.Min(0, delta.Height + r.Size.Height));
                    newSize = new Size(r.Size.Width - 0, Math.Abs(r.Size.Height + delta.Height));

                    break;

                case ResizeOperation.W: // Border box @ W edge
                    newOrigin = new Point(r.Location.X + Math.Min(delta.Width, r.Size.Width), r.Location.Y + 0);
                    newSize = new Size(Math.Abs(r.Size.Width - delta.Width), r.Size.Height - 0);

                    break;

                case ResizeOperation.E: // Border box @ E edge
                    newOrigin = new Point(r.Location.X + Math.Min(0, delta.Width + r.Size.Width), r.Location.Y + 0);
                    newSize = new Size(Math.Abs(r.Size.Width + delta.Width), r.Size.Height - 0);

                    break;

                default:
                    throw new Exception("");
            }

            return new Rectangle(newOrigin, newSize);
        }
    }
}
