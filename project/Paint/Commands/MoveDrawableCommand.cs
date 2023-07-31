using System;
using System.Drawing;
using System.Linq;
using Paint.Model;
using Paint.Visitors;

namespace Paint.Commands
{
    public class MoveDrawableCommand : ICommand<PaintSession>
    {
        readonly Guid[] _drawableIDs;
        Size _offset;

        public MoveDrawableCommand(Size offset, params Guid[] drawableIDs)
        {
            _drawableIDs = drawableIDs;
            _offset = offset;
        }

        public void Execute(PaintSession session)
        {
            var drawables = _drawableIDs.Select(id => session.Canvas.FindNode(id));

            MoveDrawableVisitor visitor = new MoveDrawableVisitor(_offset);

            foreach(IDrawable d in drawables) d.Accept(visitor);
        }

        public void Undo(PaintSession session)
        {
            var drawables = _drawableIDs.Select(id => session.Canvas.FindNode(id));

            MoveDrawableVisitor visitor = new MoveDrawableVisitor(new Size() - _offset);

            foreach (IDrawable d in drawables) d.Accept(visitor);
        }
    }
}
