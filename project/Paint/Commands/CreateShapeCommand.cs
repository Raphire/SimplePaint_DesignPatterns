using System;
using System.Drawing;
using Paint.Model;

namespace Paint.Commands
{
    public class CreateShapeCommand : ICommand<PaintSession>
    {
        readonly ShapeType _shapeType;

        Point _position;
        Size _dimensions;

        Guid _shapeID = Guid.Empty;

        public CreateShapeCommand(ShapeType type, Point pos, Size dims) 
        {
            _shapeType = type;
            _position = pos;
            _dimensions = dims;
        }

        public void Execute(PaintSession session)
        {
            Shape s;

            if (_shapeID == Guid.Empty)
            {
                s = new Shape(_shapeType, _position, _dimensions);
            }
            else s = new Shape(_shapeType, _position, _dimensions, _shapeID);

            _shapeID = s.ID;
            
            session.Canvas.Add(s);
            session.SetSelection(s);
        }

        public void Undo(PaintSession session)
        {
            session.ClearSelection();
            session.Canvas.Remove(_shapeID);
        }
    }
}
