using Paint.Strategy;
using System;
using System.Drawing;

namespace Paint.Model
{
    public class UIShape : Shape
    {
        public enum UIShapeType
        {
            DrawingPreview,
            SelectionPreview
        }

        protected readonly UIShapeType _uiType;

        public UIShapeType UIType => _uiType;

        public UIShape(ShapeType shapeType, Rectangle rectangle, UIShapeType uiType)
            : base(shapeType, rectangle.Location, rectangle.Size, Guid.Empty)
        {
            _uiType = uiType;
        }

        public UIShape(Rectangle rectangle, UIShapeType uiType)
            : base(ShapeType.Rectangle, rectangle.Location, rectangle.Size, Guid.Empty)
        {
            _uiType = uiType;
        }

        public override IDrawStrategy DrawStrategy => new UIDrawStrategy();
    }
}
