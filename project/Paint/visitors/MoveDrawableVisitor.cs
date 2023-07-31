using Paint.Model;
using System.Drawing;

namespace Paint.Visitors
{
    public class MoveDrawableVisitor : IVisitor
    {
        private Size _offset;

        public MoveDrawableVisitor(Size offset)
        {
            _offset = offset;
        }

        public void Visit(Shape shape)
        {
            shape.Origin += _offset;
        }

        public void Visit(DrawableGroup group)
        {
            group.Origin += _offset;
        }

        public void Visit(Ornament ornament)
        {
            ornament.Origin += _offset;
        }
    }
}
