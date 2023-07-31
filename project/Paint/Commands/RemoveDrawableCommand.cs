using System;
using System.Linq;
using Paint.Model;

namespace Paint.Commands
{
    public class RemoveDrawableCommand : ICommand<PaintSession>
    {
        readonly Guid[] _shapeIDs;

        IDrawable[] _removedDrawables;

        public RemoveDrawableCommand(params Guid[] shapeIDs)
        {
            _shapeIDs = shapeIDs;
        }

        public void Execute(PaintSession session)
        {
            session.ClearSelection();
            _removedDrawables = _shapeIDs.Select(id => session.Canvas.FindNode(id)).ToArray();

            foreach(IDrawable d in _removedDrawables)
            {
                (d.Parent as DrawableGroup).Remove(d.ID);
            }
        }

        public void Undo(PaintSession session)
        {
            foreach (IDrawable d in _removedDrawables)
            {
                session.Canvas.Add(d);
            }
            session.SetSelection(_removedDrawables);
        }
    }
}
