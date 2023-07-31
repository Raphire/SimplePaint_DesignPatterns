using System.Drawing;
using System.Linq;
using Paint.Model;

namespace Paint.Commands
{
    public class CreateGroupCommand : ICommand<PaintSession>
    {
        readonly DrawableGroup _parent;

        readonly IGrouping<DrawableGroup, IDrawable>[] _children;

        DrawableGroup _group;

        public CreateGroupCommand(DrawableGroup parent, params IDrawable [] children) 
        {
            _parent = parent;
            _children = children.GroupBy(c => c.Parent as DrawableGroup).ToArray();
        }

        public void Execute(PaintSession session)
        {
            _group = new DrawableGroup(new Point(0, 0));
            _group.Add(_children.SelectMany(g => g.ToArray()).ToArray());

            _parent.Add(_group);
            session.SetSelection(_group);
        }

        public void Undo(PaintSession session)
        {
            _parent.Remove(_group.ID);

            foreach(var grouped in _children)
            {
                grouped.Key.Add(grouped.ToArray());
            }
        }
    }
}
