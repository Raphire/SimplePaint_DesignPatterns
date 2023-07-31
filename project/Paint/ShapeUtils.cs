using Paint.Model;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Paint
{
    static class ShapeUtils
    {
        public static Rectangle[] GetResizeBoxes(Rectangle shapeBase, int offsetX = 0, int offsetY = 0)
        {
            Point newOrigin = new Point(shapeBase.Location.X + offsetX, shapeBase.Location.Y + offsetY);

            int selectionMargin = 3;

            // Calculate size of the borderboxes
            Size selectionBoxSize = new Size(shapeBase.Size.Width + selectionMargin * 2, shapeBase.Size.Height + selectionMargin * 2);
            Point selectionBoxOrigin = new Point(newOrigin.X - selectionMargin, newOrigin.Y - selectionMargin);

            var resizeBoxPositions = new Point[]
            {
            // Corners
            new Point(selectionBoxOrigin.X,                             selectionBoxOrigin.Y),                              // NW-SE, NW
            new Point(selectionBoxOrigin.X + selectionBoxSize.Width,    selectionBoxOrigin.Y + selectionBoxSize.Height),    // NW-SE, SE
            new Point(selectionBoxOrigin.X + selectionBoxSize.Width,    selectionBoxOrigin.Y),                              // NE-SW, NE
            new Point(selectionBoxOrigin.X,                             selectionBoxOrigin.Y + selectionBoxSize.Height),    // NE-SW, SW

            // Edges
            new Point(selectionBoxOrigin.X + selectionBoxSize.Width / 2,    selectionBoxOrigin.Y),                                  // N-S, N
            new Point(selectionBoxOrigin.X + selectionBoxSize.Width / 2,    selectionBoxOrigin.Y + selectionBoxSize.Height    ),    // N-S, S
            new Point(selectionBoxOrigin.X,                                 selectionBoxOrigin.Y + selectionBoxSize.Height / 2),    // W-E, W
            new Point(selectionBoxOrigin.X + selectionBoxSize.Width,        selectionBoxOrigin.Y + selectionBoxSize.Height / 2),    // W-E, E
            };

            return resizeBoxPositions.Select(center => CreateSquare(center, 3)).ToArray();
        }

        public static Rectangle CreateSquare(Point center, int radius)
        {
            return new Rectangle(
                new Point(center.X - radius, center.Y - radius),
                new Size(radius * 2, radius * 2));
        }

        public static Rectangle ExpandRectangle(Rectangle r, int expandBy)
        {
            Size expandedSize = new Size(r.Width + expandBy * 2, r.Height + expandBy * 2);
            Point expandedOrigin = new Point(r.Location.X - expandBy, r.Location.Y - expandBy);

            return new Rectangle(expandedOrigin, expandedSize);
        }

        /// <summary>
        /// Generates a list of border boxes for the specified shape
        /// </summary>
        /// <param name="s">Shape</param>
        /// <returns>
        /// Returns an array of rectangles (Borderboxes for the shape)
        /// </returns>
        public static Dictionary<Guid, Rectangle[]> GetResizeBoxesByShape(IEnumerable<IDrawable> shapes,
            int offsetX = 0, int offsetY = 0)
        {
            Dictionary<Guid, Rectangle[]> rectangles = new Dictionary<Guid, Rectangle[]>();

            foreach (IDrawable d in shapes)
            {
                IDrawable t = d is Ornament o ? o.EndPoint : d;

                if (t is Shape s)
                {
                    var resizeBoxes = GetResizeBoxes(s.Base, offsetX, offsetY);
                    rectangles.Add(t.ID, resizeBoxes);
                }
            }

            return rectangles;
        }

        #region Extension Methods
        public static Point[] GetVertices(this Rectangle rect)
        {
            return new Point[]
            {
                rect.Location,
                rect.Location + rect.Size,
                rect.Location + new Size(rect.Size.Width, 0),
                rect.Location + new Size(0, rect.Size.Height)
            };
        }

        public static Point[] GetVertices(this Shape shape)
        {
            var rect = new Rectangle(shape.AbsoluteOrigin, shape.Size);

            return new Point[]
            {
                rect.Location,
                rect.Location + new Size(0, rect.Size.Height),
                rect.Location + rect.Size,
                rect.Location + new Size(rect.Size.Width, 0)
            };
        }

        public static Rectangle GetBoundingRect(this Shape shape)
        {
            return new Rectangle(shape.AbsoluteOrigin, shape.Size);
        }

        public static System.Windows.Forms.Cursor GetCursor(this ResizeOperation op)
        {
            switch (op)
            {
                case ResizeOperation.NW: // NW
                case ResizeOperation.SE: // SE
                    return System.Windows.Forms.Cursors.SizeNWSE;

                case ResizeOperation.NE: // NE
                case ResizeOperation.SW: // SW
                    return System.Windows.Forms.Cursors.SizeNESW;

                case ResizeOperation.N: // N
                case ResizeOperation.S: // S
                    return System.Windows.Forms.Cursors.SizeNS;

                case ResizeOperation.W: // W
                case ResizeOperation.E: // E
                    return System.Windows.Forms.Cursors.SizeWE;

                default:
                    throw new Exception();
            }
        }
        #endregion
    }
}
