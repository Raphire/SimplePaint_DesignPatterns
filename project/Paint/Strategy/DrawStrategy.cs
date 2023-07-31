using Paint.Model;
using Paint.Visitors;
using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Paint.Strategy
{
    public abstract class DrawStrategy : IDrawStrategy
    {
        static readonly Pen DefaultPen = Pens.Black; 

        #region IDRAWSTRATEGY IMPLEMENTATION
        public abstract void Draw(Graphics target, IDrawable drawable, PaintSession session);

        public abstract string ToString(IDrawable drawable);

        public virtual TreeNode CreateNode(IDrawable drawable, PaintSession session)
        {
            return new TreeNode(GetNodeText(drawable))
            {
                ToolTipText = GetNodeToolTipText(drawable),
                Name = drawable.ID.ToString(),
                Tag = drawable
            };
        }
        #endregion

        #region HELPERS
        protected string GetIndent(IDrawable drawable)
        {
            return Enumerable.Repeat("\t", drawable.Depth).Aggregate("", (a, b) => a + b);
        }

        protected Rectangle GetProjectedShapeBase(IDrawable drawable, PaintSession session)
        {
            // Evaluate whether drawable is currently selected
            if (session.IsSelected(drawable) || session.IsAncestorSelected(drawable))
            {   
                // Evaluate whether user is dragging the mouse
                if(session.IsMouseDown && session.IsToolTypeAnySelect())
                {
                    // At this point, drawable is either being moved or resized
                    Size dragOffset = session.GetMouseDragOffset();

                    if (session.CurrentResizeOperation == ResizeOperation.None)
                    {   // If we're in this clause this shape is currently being moved
                        // Determine & return shape projection's position
                        return new Rectangle(drawable.AbsoluteOrigin + dragOffset, drawable.Size);
                    }
                    else if (session.CurrentResizeOperation != ResizeOperation.None)
                    {   // If we're in this clause this shape is currently being resized
                        // Create a temporary shape and apply Resize Operation using ResizeDrawableVisitor
                        Shape temp = new Shape(drawable.Type, drawable.AbsoluteOrigin, drawable.Size, Guid.Empty);
                        temp.Accept(new ResizeDrawableVisitor(temp.Base, dragOffset, session.CurrentResizeOperation));
                        return temp.Base;
                    }
                }
            }

            // Shape is currently not being edited
            return new Rectangle(drawable.AbsoluteOrigin, drawable.Size);
        }

        protected Pen GetShapePen(IDrawable drawable, PaintSession session)
        {
            if (session.Selection.Any(ss => ss.ID == drawable.ID) || drawable.ID == Guid.Empty)
            {
                return Pens.Red;
            }

            if (session.IsAncestorSelected(drawable))
            {
                return Pens.Orange;
            }

            return DefaultPen;
        }

        /// <summary>
        /// Draws selectionbox around the provided shape
        /// </summary>
        /// <param name="s">Shape</param>
        /// <param name="e">PaintEventArguments of canvas</param>
        protected virtual void DrawSelectionUI(IDrawable drawable, Rectangle projectedBase, Graphics target)
        {
            Pen selectionPen = new Pen(Color.Black)
            {
                DashStyle = System.Drawing.Drawing2D.DashStyle.Dot
            };

            target.DrawRectangle(selectionPen, ShapeUtils.ExpandRectangle(projectedBase, 4));

            // Get the resizeboxes for the shape
            Rectangle[] resizeBoxes = ShapeUtils.GetResizeBoxes(projectedBase);

            target.FillRectangles(Brushes.White, resizeBoxes);
            target.DrawRectangles(Pens.Black, resizeBoxes);
        }
   
        protected virtual string GetNodeText(IDrawable d)
        {
            return string.Format(
                "{0} @({1}, {2}) [{3}x{4}]",
                d.Type, 
                d.AbsoluteOrigin.X, d.AbsoluteOrigin.Y, 
                d.Size.Width, d.Size.Height
            );
        }

        protected virtual string GetNodeToolTipText(IDrawable d)
        {
            return string.Format(
                "Position: ({0}, {1})\nSize: ({2}, {3})",
                d.AbsoluteOrigin.X, d.AbsoluteOrigin.Y,
                d.Size.Width, d.Size.Height
            );
        }
        #endregion
    }
}
