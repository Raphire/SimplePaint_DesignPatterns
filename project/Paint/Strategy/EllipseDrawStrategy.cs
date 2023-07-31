using Paint.Model;
using System.Drawing;
using System.Linq;

namespace Paint.Strategy
{
    public class EllipseDrawStrategy : DrawStrategy
    {
        public override void Draw(Graphics target, IDrawable drawable, PaintSession session)
        {
            Rectangle projectedShapeBase = GetProjectedShapeBase(drawable, session);
            Pen shapePen = GetShapePen(drawable, session);

            target.DrawEllipse(shapePen, projectedShapeBase);

            if (session.Selection.Any(ss => ss.ID == drawable.ID))
            {
                DrawSelectionUI(drawable, projectedShapeBase, target);
            }
        }

        public override string ToString(IDrawable drawable)
        {
            return string.Format("{0}{1} {2} {3} {4} {5}",
                GetIndent(drawable),
                "ellipse",
                drawable.Origin.X, drawable.Origin.Y,
                drawable.Size.Width, drawable.Size.Height
            );
        }
    }
}
