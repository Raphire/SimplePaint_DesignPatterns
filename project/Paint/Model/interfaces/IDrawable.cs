using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Paint
{
    public interface IDrawable
    {
        IDrawableGroup Parent { get; }
        ShapeType Type { get; }
        Point Origin { get; }
        Size Size { get; }
        Guid ID { get; }

        void Draw(PaintEventArgs e, Pen pen, int offsetX = 0, int offsetY = 0);

        bool ContainsPoint(Point point);
    }
}
