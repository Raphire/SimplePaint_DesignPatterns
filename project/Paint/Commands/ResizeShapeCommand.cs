using System;
using System.Drawing;
using Paint.Model;
using Paint.Visitors;

namespace Paint.Commands
{
    public class ResizeShapeCommand : ICommand<PaintSession>
    {
        ResizeOperation _direction;
        Guid _shapeID;
        Size _delta;

        Point _oldPosition;
        Size _oldDimensions;

        public ResizeShapeCommand(Guid shapeID, Size delta, ResizeOperation direction)
        {
            _shapeID = shapeID;
            _delta = delta;
            _direction = direction;
        }

        public void Execute(PaintSession session)
        {
            IDrawable shape = session.Canvas.FindNode(_shapeID);

            _oldPosition = shape.Origin;
            _oldDimensions = shape.Size;

            Rectangle shapeBase = new Rectangle(shape.Origin, shape.Size);
            shape.Accept(new ResizeDrawableVisitor(shapeBase, _delta, _direction));
        }

        public void Undo(PaintSession session)
        {
            IDrawable shape = session.Canvas.FindNode(_shapeID);
            shape.Accept(new ResizeDrawableVisitor(_oldPosition, _oldDimensions));
        }
    }
}
