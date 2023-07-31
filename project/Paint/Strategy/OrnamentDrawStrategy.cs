using Paint.Model;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Drawing;
using System.Linq;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Windows.Forms;

namespace Paint.Strategy
{
    public class OrnamentDrawStrategy : DrawStrategy
    {
        public override void Draw(Graphics target, IDrawable drawable, PaintSession session)
        {
            Ornament o = drawable as Ornament;
            o.Target.DrawStrategy.Draw(target, o.Target, session);

            Brush b = Brushes.Black;
            Font f = new Font("Times New Roman", 12.0f);

            Size offset = TextRenderer.MeasureText(o.Text, f);

            Point p = GetDrawingPosition(o, offset, session);

            target.DrawString(o.Text, f, b, p);
        }

        private Point GetDrawingPosition(Ornament o, Size textDims, PaintSession session)
        {
            int paddingSides = 6;
            int paddingUpDown = 4;

            var rootTarget = o.Target;
            while(rootTarget is Ornament)
            {
                rootTarget = (rootTarget as Ornament).Target;
            }

            Rectangle projectedShapeBase = GetProjectedShapeBase(rootTarget, session);
            Point targetPos = projectedShapeBase.Location;
            Size tSize = projectedShapeBase.Size;

            // Fix origin point if ornament rootTarget is a group
            if (rootTarget is DrawableGroup)
            {
                DrawableGroup _rootTarget = rootTarget as DrawableGroup;

                int minX = 0, minY = 0;

                if (_rootTarget.Children.Any())
                {
                    minX = _rootTarget.Children.Min(c => c.AbsoluteOrigin.X);
                    minY = _rootTarget.Children.Min(c => c.AbsoluteOrigin.Y);

                    if (session.IsMouseDown && session.Selection.Any(ss => ss.ID == _rootTarget.ID))
                    {
                        minX += session.GetMouseDragOffset().Width;
                        minY += session.GetMouseDragOffset().Height;
                    }
                }

                targetPos = new Point(minX, minY);
            }

            switch (o.DecoratedSide)
            {   // Find centered point at specified side of Drawable
                case (Ornament.Side.Top):
                    targetPos += new Size(tSize.Width / 2 -textDims.Width / 2, -textDims.Height - paddingUpDown);
                    break;
                case (Ornament.Side.Bottom):
                    targetPos += new Size(tSize.Width / 2 - textDims.Width / 2, tSize.Height + paddingUpDown);
                    break;
                case (Ornament.Side.Right):
                    targetPos += new Size(tSize.Width + paddingSides, tSize.Height / 2 - textDims.Height / 2);
                    break;
                case (Ornament.Side.Left):
                    targetPos += new Size(-textDims.Width - paddingSides, tSize.Height / 2 - textDims.Height / 2);
                    break;
            }

            return targetPos;
        }

        public override TreeNode CreateNode(IDrawable o, PaintSession session)
        {
            IDrawable d = o;
            if (o is Ornament orn) d = orn.EndPoint;

            TreeNode node = d.DrawStrategy.CreateNode(d, session);

            node.Tag = o;
            node.Text = GetNodeText(d);
            node.ToolTipText = GetNodeToolTipText(d);

            return node;
        }

        public override string ToString(IDrawable drawable)
        {
            Ornament o = drawable as Ornament;

            return string.Format("{0}{1} {2} \"{3}\"\n{4}",
                GetIndent(drawable), "ornament",
                o.DecoratedSide, o.Text,
                o.Target.DrawStrategy.ToString(o.Target)
            );
        }

        protected override string GetNodeText(IDrawable d)
        {
            if (d is Ornament o) d = o.EndPoint;

            return string.Format(
                "{0} @({1}, {2}) [{3}x{4}]",
                d.Type,
                d.AbsoluteOrigin.X, d.AbsoluteOrigin.Y,
                d.Size.Width, d.Size.Height
            );
        }

        protected override string GetNodeToolTipText(IDrawable d)
        {
            string decoratorLines = "";

            IDrawable i = d;
            while (i is Ornament o)
            {
                decoratorLines += string.Format(
                    "Ornament {0} '{1}'\n",
                    o.DecoratedSide, o.Text
                );
                i = o.Target;
            }

            return string.Format(
                "Position: ({0}, {1})\nSize: ({2}, {3})\n{4}",
                d.AbsoluteOrigin.X, d.AbsoluteOrigin.Y,
                d.Size.Width, d.Size.Height,
                decoratorLines
            );
        }
    }
}
