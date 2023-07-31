using Paint.Model;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace Paint.Strategy
{
    public class UIDrawStrategy : DrawStrategy
    {
        public static readonly Pen SelectionPreviewPen = new Pen(Color.Gray) 
        { DashStyle = System.Drawing.Drawing2D.DashStyle.Dot };
        public static readonly Pen DrawingPreviewPen = new Pen(Color.Red) 
        { };

        public override void Draw(Graphics target, IDrawable drawable, PaintSession session)
        {
            if (!(drawable is UIShape uiShape))
            {
                throw new ArgumentException("drawable should be derived from type UIShape");
            }

            switch(uiShape.Type)
            {
                case ShapeType.Rectangle: 
                    target.DrawRectangle(GetUIPen(uiShape.UIType), uiShape.Base); 
                    break;

                case ShapeType.Ellipse:
                    target.DrawEllipse(GetUIPen(uiShape.UIType), uiShape.Base);
                    break;

                default: throw new NotImplementedException();
            }
            
        }

        protected Pen GetUIPen(UIShape.UIShapeType uiType)
        {
            switch (uiType)
            {
                case UIShape.UIShapeType.DrawingPreview: return DrawingPreviewPen;
                case UIShape.UIShapeType.SelectionPreview: return SelectionPreviewPen;
                default: throw new NotImplementedException();
            }
        }

        public override string ToString(IDrawable drawable)
        {
            throw new InvalidOperationException("UI Elements do not support serialization");
        }

        public override TreeNode CreateNode(IDrawable drawable, PaintSession session)
        {
            throw new InvalidOperationException("UI Elements do not support TreeNode creation");
        }

    }
}
