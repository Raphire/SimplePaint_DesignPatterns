using Paint.Model;

namespace Paint.Visitors
{
    public interface IVisitable
    {
        void Accept(IVisitor visitor);
    }

    public interface IVisitor
    {
        void Visit(Shape shape);

        void Visit(DrawableGroup group);

        void Visit(Ornament ornament);

    }

}
