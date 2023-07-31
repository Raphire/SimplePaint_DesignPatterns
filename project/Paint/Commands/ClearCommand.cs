using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using Paint.Model;

namespace Paint.Commands
{
    public class ClearCommand : ICommand<PaintSession>
    {
        private List<IDrawable> _oldShapes;

        public void Execute(PaintSession session)
        {
            _oldShapes = new List<IDrawable>(session.Canvas.Children);
            session.ClearSelection();
            session.Canvas.Clear();
        }

        public void Undo(PaintSession session)
        {
            session.Canvas.Add(_oldShapes.ToArray());
        }
    }
}
