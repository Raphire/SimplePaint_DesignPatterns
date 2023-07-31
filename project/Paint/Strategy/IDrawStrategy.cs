using Paint.Model;
using System.Drawing;
using System.Windows.Forms;

namespace Paint.Strategy
{
    public interface IDrawStrategy
    {
        /// <summary>
        /// Draws drawable on target surface
        /// </summary>
        void Draw(Graphics target, IDrawable drawable, PaintSession session);

        /// <summary>
        /// Serializes specified drawable into a string used in the PML format
        /// </summary>
        string ToString(IDrawable drawable);

        /// <summary>
        /// Creates a TreeNode used in ComponentForm
        /// </summary>
        TreeNode CreateNode(IDrawable drawable, PaintSession session);
    }
}
