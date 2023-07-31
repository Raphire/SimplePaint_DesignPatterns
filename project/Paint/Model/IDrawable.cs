using Paint.Composite;
using Paint.Strategy;
using Paint.Visitors;
using System.ComponentModel;
using System.Drawing;

namespace Paint.Model
{
    public interface IDrawable : IVisitable, INodeBase<IDrawable>, INotifyPropertyChanged
    {
        ShapeType Type { get; }
        Point Origin { get; set; }
        Point AbsoluteOrigin { get; }
        Size Size { get; }

        Point Minimum { get; }
        Point Maximum { get; }

        IDrawStrategy DrawStrategy { get; }

        bool ContainsPoint(Point point);
    }
}
