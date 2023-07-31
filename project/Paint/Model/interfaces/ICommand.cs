using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Paint
{
    public interface ICommand
    {
        void Execute();
        void Undo();
    }
    
    public abstract class DrawableCommand : ICommand
    {
        protected Canvas _canvas;

        public DrawableCommand(Canvas canvas)
        {
            this._canvas = canvas;
        }

        public abstract void Execute();

        public abstract void Undo();
    }

    public class ClearCommand : DrawableCommand
    {
        private List<Drawable> _oldShapes;

        public ClearCommand(Canvas canvas): base(canvas) {}

        public override void Execute()
        {
            this._oldShapes = new List<Drawable>(_canvas.Shapes);
            _canvas.Clear();
        }

        public override void Undo()
        {
            _canvas.AddShape(this._oldShapes.ToArray());
        }
    }

    public class CreateShapeCommand : DrawableCommand
    {
        ShapeType _shapeType;

        Point _position;
        Size _dimensions;

        Guid _shapeID = Guid.Empty;

        public CreateShapeCommand(Canvas canvas, ShapeType type, Point pos, Size dims) : base(canvas)
        {
            this._shapeType = type;
            this._position = pos;
            this._dimensions = dims;
        }

        public override void Execute()
        {
            Shape s;

            if (_shapeID == Guid.Empty)
            {
                s = new Shape(_shapeType, _position, _dimensions);
                _canvas.SelectedShape = s;
            } 
            else
            {
                s = new Shape(_shapeType, _position, _dimensions, _shapeID);
            }
                
            _shapeID = s.ID;
            _canvas.AddShape(s);
        }

        public override void Undo()
        {
            _canvas.RemoveDrawable(_shapeID);

            if(_canvas.SelectedShape != null && _canvas.SelectedShape.ID == _shapeID)
            {
                _canvas.SelectedShape = null;
            }
        }
    }

    public class RemoveDrawableCommand : DrawableCommand
    {
        Guid _shapeID;
        Drawable _drawable;

        public RemoveDrawableCommand(Canvas canvas, Guid shapeID) : base(canvas)
        {
            this._shapeID = shapeID;
        }

        public override void Execute()
        {
            this._drawable = _canvas.GetDrawable(_shapeID);
            _canvas.RemoveDrawable(_shapeID);

            if (_canvas.SelectedShape != null && _canvas.SelectedShape.ID == _shapeID)
            {
                _canvas.SelectedShape = null;
            }
        }

        public override void Undo()
        {
            _canvas.AddShape(_drawable);
            _canvas.SelectedShape = _drawable;
        }
    }

    public class EditShapeCommand : DrawableCommand
    {
        Guid _shapeID;

        Point _newPosition;
        Size _newDimensions;

        Point _oldPosition;
        Size _oldDimensions;

        public EditShapeCommand(Canvas canvas, Guid shapeID, Point newPosition, Size newDimensions) 
            : base(canvas)
        {
            this._shapeID = shapeID;

            Shape shape = _canvas.GetDrawable<Shape>(this._shapeID);

            this._newPosition = newPosition;
            this._newDimensions = newDimensions;
            this._oldPosition = shape.Origin;
            this._oldDimensions = shape.Size;
        }

        public EditShapeCommand(Canvas canvas, Guid shapeID, Point newPosition)
            : base(canvas)
        {
            this._shapeID = shapeID;

            Shape shape = _canvas.GetDrawable<Shape>(this._shapeID);

            this._newPosition = newPosition;
            this._newDimensions = this._oldDimensions = shape.Size;
            this._oldPosition = shape.Origin;
        }

        public override void Execute()
        {
            Shape shape = _canvas.GetDrawable<Shape>(this._shapeID);
            shape.Resize(this._newDimensions);
            shape.Origin = this._newPosition;
        }

        public override void Undo()
        {
            Shape shape = _canvas.GetDrawable<Shape>(this._shapeID);
            shape.Resize(this._oldDimensions);
            shape.Origin = this._oldPosition;
        }
    }
}
