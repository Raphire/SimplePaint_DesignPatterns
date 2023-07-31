using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Paint
{
    public interface IDrawableGroup
    {
        IReadOnlyCollection<Drawable> Children { get; }

        void AddChild(Drawable child);

        void RemoveChild(Guid childID);
    }
}
