using Paint.Commands;
using Paint.Composite;
using Paint.Control;
using Paint.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Input;

namespace Paint
{
    public partial class ComponentForm : Form
    {
        #region FIELDS
        private readonly PaintSession _session;
        private readonly TreeViewDrawableBinder _dataBinder;
        #endregion

        #region CONSTRUCTOR
        public ComponentForm(PaintSession session)
        {
            InitializeComponent();

            _session = session;

            _dataBinder = new TreeViewDrawableBinder(ComponentView, _session);
            _dataBinder.Bind(_session.Canvas);

            _session.SelectionChanged += Session_SelectionChanged;
        }
        #endregion

        #region HELPERS
        private void HandleControlClick(TreeNodeMouseClickEventArgs e)
        {
            IDrawable clicked = e.Node.Tag as IDrawable;

            if (_session.IsSelected(clicked))
            {
                _session.Deselect(clicked);
            }
            else _session.Select(clicked);
        }

        private void HandleShiftClick(TreeNodeMouseClickEventArgs e)
        {
            try
            {
                TreeNode prevSelectedNode = _dataBinder.FindNode(_session.Selection.Last());

                if (prevSelectedNode.Parent == e.Node.Parent)
                {
                    int clickedIndex = e.Node.Index;
                    int prevSelectedIndex = prevSelectedNode.Index;

                    int min = clickedIndex > prevSelectedIndex ? prevSelectedIndex : clickedIndex;
                    int max = clickedIndex > prevSelectedIndex ? clickedIndex : prevSelectedIndex;

                    var select = new List<IDrawable>();

                    for (int i = min; i <= max; i++)
                    {
                        if (e.Node.Parent != null)
                        {
                            select.Add(e.Node.Parent.Nodes[i].Tag as IDrawable);
                        }
                        else select.Add(ComponentView.Nodes[i].Tag as IDrawable);
                    }

                    _session.Select(select.ToArray());
                }
            }
            catch {
                IDrawable clicked = e.Node.Tag as IDrawable;
                _session.Select(clicked);
            }
        }
        #endregion

        #region EVENT HANDLERS
        private void Session_SelectionChanged(object sender, ContentChangedEventArgs<IDrawable> args)
        {
            _dataBinder.RefreshNodes(args.ContentDiff.ToArray());
        }

        private void ComponentView_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (Keyboard.IsKeyDown(Key.LeftCtrl)) HandleControlClick(e);
                else if (Keyboard.IsKeyDown(Key.LeftShift)) HandleShiftClick(e);
                else _session.SetSelection(e.Node.Tag as IDrawable);
            } 
            else if (e.Button == MouseButtons.Right)
            {
                e.Node.ContextMenu.Show(e.Node.TreeView, e.Location);
            }
        }

        private void CreateGroupButton_Click(object sender, EventArgs e)
        {
            _session.ExecuteCommand(
                new CreateGroupCommand(
                    _session.Canvas, _session.Selection.ToArray()));
        }
        #endregion
    }
}
