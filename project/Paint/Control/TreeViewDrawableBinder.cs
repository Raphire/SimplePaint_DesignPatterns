using Paint.Commands;
using Paint.Composite;
using Paint.Model;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Paint.Control
{
    public class DrawableNodeBuilder
        : INodeBuilder<IDrawable>
    {
        private PaintSession _context;

        public DrawableNodeBuilder(PaintSession ctx) { _context = ctx; }

        public TreeNode ConstructNode(IDrawable data)
        {
            TreeNode node = new TreeNode()
            {
                Text = BuildNodeText(data),
                ToolTipText = BuildNodeToolTip(data),
                ContextMenu = BuildNodeContextMenu(data),
                Tag = data
            };

            FormatNode(node, data);

            return node;
        }

        public ContextMenu BuildNodeContextMenu(IDrawable data)
        {
            List<MenuItem> menuItems = new List<MenuItem>();

            string promptInput(Ornament.Side s) => Prompt.ShowDialog(
                string.Format("Please enter the text you would like " +
                "to be displayed on the {0} side of the object", s), "New Ornament");

            MenuItem[] setOrnamentMenuItems = Enum.GetValues(typeof(Ornament.Side))
                .Cast<Ornament.Side>().ToList()
                .Select(side => new MenuItem(
                    string.Format("Set {0} ornament ...", side),
                    (s, args) => _context.ExecuteCommand(new DecoratorCommand(
                        data.ID,
                        new Dictionary<Ornament.Side, string>() {
                            { side, promptInput(side) }
                        }
                    ))
                )).ToArray();

            menuItems.Add(new MenuItem("Set ornament...", setOrnamentMenuItems.ToArray()));

            if (data is Ornament ornament)
            {
                List<MenuItem> removeOrnamentMenuItems = new List<MenuItem>();

                var ornaments = new List<Ornament>() { ornament };
                IDrawable target = ornament;

                while (target is Ornament o)
                {
                    removeOrnamentMenuItems.Add(
                        new MenuItem(
                            string.Format("Remove {0} ornament '{1}'", o.DecoratedSide, o.Text),
                            (s, args) => _context.ExecuteCommand(new DecoratorCommand(o.ID, null, o.DecoratedSide))
                        )
                    );

                    target = o.Target;
                }

                menuItems.Add(new MenuItem("Remove ornament...", removeOrnamentMenuItems.ToArray()));

                data = ornament.EndPoint;
            }

            if (data is DrawableGroup)
            {
                DrawableGroup group = data as DrawableGroup;

                void moveSelectionToGroupHandler(object s, EventArgs args) =>
                    group.Add(_context.Selection.ToArray());

                menuItems.Add(
                    new MenuItem(
                        "Move selection to group",
                        moveSelectionToGroupHandler
                    )
                );
            }

            return new ContextMenu(menuItems.ToArray());
        }

        public string BuildNodeText(IDrawable d)
        {
            if (d is Ornament o) d = o.EndPoint;

            return string.Format(
                "{0} @({1}, {2}) [{3}x{4}]",
                d.Type,
                d.AbsoluteOrigin.X, d.AbsoluteOrigin.Y,
                d.Size.Width, d.Size.Height
            );
        }

        public string BuildNodeToolTip(IDrawable d)
        {
            string decoratorLines = "";

            IDrawable i = d;
            while (i is Ornament o)
            {
                decoratorLines += string.Format(
                    "Ornament {0} '{1}'\n",
                    o.DecoratedSide, o.Text
                );
                i = o.Target;
            }

            return string.Format(
                "Position: ({0}, {1})\nSize: ({2}, {3})\n{4}",
                d.AbsoluteOrigin.X, d.AbsoluteOrigin.Y,
                d.Size.Width, d.Size.Height,
                decoratorLines
            );
        }

        public void FormatNode(TreeNode node, IDrawable data)
        {
            if (_context.IsSelected(data))
            {
                node.BackColor = Color.FromArgb(0, 120, 215);
                node.ForeColor = Color.White;
            }
            else
            {
                node.BackColor = Color.White;
                node.ForeColor = Color.Black;
            }
        }
    }

    public class DrawableTreeBuilder
        : ITreeBuilder<IDrawable, IParentNode<IDrawable>>
    {
        public IEnumerable<IDrawable> SolveBranch(IParentNode<IDrawable> branch) => branch.Children;

        public string GetUniqueKey(IDrawable node) => node.ID.ToString();
    }

    public class TreeViewDrawableBinder : TreeViewDataBinder<IDrawable, IParentNode<IDrawable>>
    {
        public TreeViewDrawableBinder(BufferedTreeView treeView, PaintSession context)
            : base(treeView, context)
        {
            _nodeBuilderStrategy = new DrawableNodeBuilder(_context as PaintSession);
            _treeBuilderStrategy = new DrawableTreeBuilder();
        }
    }
}
