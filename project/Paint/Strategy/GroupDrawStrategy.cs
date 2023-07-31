using Paint.Model;
using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Paint.Strategy
{
    public class GroupDrawStrategy : DrawStrategy
    {
        public override void Draw(Graphics target, IDrawable drawable, PaintSession session)
        {
            DrawableGroup group = drawable as DrawableGroup;
            foreach (var c in group.Children)
            {
                c.DrawStrategy.Draw(target, c, session);
            }

            if (session.Selection.Any(ss => ss.ID == drawable.ID))
            {
                Rectangle projectedShapeBase = GetProjectedShapeBase(drawable, session);

                int tX = projectedShapeBase.X - drawable.AbsoluteOrigin.X;
                int tY = projectedShapeBase.Y - drawable.AbsoluteOrigin.Y;

                int minX = tX, minY = tY;

                if(group.Children.Any())
                {
                    minX = group.Children.Min(c => c.AbsoluteOrigin.X);
                    minY = group.Children.Min(c => c.AbsoluteOrigin.Y);
                }

                projectedShapeBase = group.GetContentBoundingRectangle();
                projectedShapeBase.Location += new Size(tX, tY);

                DrawSelectionUI(drawable, projectedShapeBase, target);
            }
        }

        public override string ToString(IDrawable drawable)
        {
            if (!(drawable is DrawableGroup))
            {
                throw new ArgumentException("Drawable is not of type IDrawableGroup");
            }

            var group = drawable as DrawableGroup;

            string lines = "";

            foreach (IDrawable c in group.Children)
            {
                lines += "\n" + c.DrawStrategy.ToString(c);
            }

            return String.Format("{0}group {1} {2} {3}{4}",
                GetIndent(drawable),
                group.Children.Count,
                drawable.Origin.X, drawable.Origin.Y,
                lines
            );
        }

        public override TreeNode CreateNode(IDrawable drawable, PaintSession session)
        {
            var children = (drawable as DrawableGroup).Children.Select(cc => 
                cc.DrawStrategy.CreateNode(cc, session)).ToArray();

            TreeNode node = new TreeNode(GetNodeText(drawable), children)
            {
                Name = drawable.ID.ToString(), Tag = drawable
            };

            node.Expand();

            return node;
        }

        protected override void DrawSelectionUI(IDrawable drawable, Rectangle projectedBase, Graphics target)
        {
            int selectionMargin = 3;

            Pen selectionPen = new Pen(Color.Black)
            {
                DashStyle = System.Drawing.Drawing2D.DashStyle.Dot
            };

            // Calculate size of the selection box to be slightly bigger then the shape
            Size selectionBoxSize = new Size(projectedBase.Width + selectionMargin * 2, projectedBase.Height + selectionMargin * 2);
            Point selectionBoxOrigin = new Point(projectedBase.Location.X - selectionMargin, projectedBase.Location.Y - selectionMargin);

            target.DrawRectangle(selectionPen, new Rectangle(selectionBoxOrigin, selectionBoxSize));
        }
    }
}
