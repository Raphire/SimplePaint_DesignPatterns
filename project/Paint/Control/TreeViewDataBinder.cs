using Paint.Composite;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;

namespace Paint.Control
{
    public interface ITreeBuilder<TBase, TBranch>
    {
        IEnumerable<TBase> SolveBranch(TBranch branch);
        string GetUniqueKey(TBase node);
    }

    public interface INodeBuilder<T>
    {
        TreeNode ConstructNode(T data);
        string BuildNodeText(T data);
        string BuildNodeToolTip(T data);
        ContextMenu BuildNodeContextMenu(T data);
        void FormatNode(TreeNode node, T data);
    }

    public interface ITreeViewDataBinder<TBase, TParent>
    where TBase : INotifyPropertyChanged
    where TParent : class, INotifyContentChanged<TBase>
    {
        void Bind(TBase root);

        void RefreshNodes(params TBase[] data);

        TreeNode FindNode(TBase data);
    }

    public abstract class TreeViewDataBinder<TBase, TParent>
        : ITreeViewDataBinder<TBase, TParent>
        where TBase : INotifyPropertyChanged
        where TParent : class, INotifyContentChanged<TBase>
    {
        protected ITreeBuilder<TBase, TParent> _treeBuilderStrategy;
        protected INodeBuilder<TBase> _nodeBuilderStrategy;

        protected readonly BufferedTreeView _treeView;
        protected readonly object _context;

        protected TParent _dataRoot;
        protected TreeNode _rootNode;

        public TreeViewDataBinder(BufferedTreeView treeView, object context)
        {
            _context = context;
            _treeView = treeView;
        }

        public void Bind(TBase root)
        {
            if (root is TParent p) _dataRoot = p;
            else throw new ArgumentException("");

            _treeView.BeginUpdate();
            _rootNode = Build(root);
            _treeView.EndUpdate();
        }

        private TreeNode Build(TBase data)
        {
            TreeNode node = _nodeBuilderStrategy.ConstructNode(data);
            node.Name = _treeBuilderStrategy.GetUniqueKey(data);

            if (data is TParent pData)
            {
                node.Nodes.AddRange(
                    _treeBuilderStrategy.SolveBranch(pData)
                    .Select(cd => Build(cd))
                    .ToArray());

                pData.ContentChanged += DataRoot_ContentChanged;

                node.Expand();
            }

            data.PropertyChanged += (sender, args) => UpdateNode(node, data, args.PropertyName);

            return node;
        }

        protected virtual void UpdateNode(TreeNode node, TBase data, string changedPropertyName = null)
        {
            node.Text = _nodeBuilderStrategy.BuildNodeText(data);
            node.ToolTipText = _nodeBuilderStrategy.BuildNodeToolTip(data);
            node.ContextMenu = _nodeBuilderStrategy.BuildNodeContextMenu(data);

            _nodeBuilderStrategy.FormatNode(node, data);
        }

        public TreeNode FindNode(TBase data)
        {
            return _treeView.Nodes.Find(_treeBuilderStrategy.GetUniqueKey(data), true).Single();
        }

        public void RefreshNodes(params TBase[] data)
        {
            _treeView.BeginUpdate();

            foreach (TBase b in data)
            {
                TreeNode node = FindNode(b);
                UpdateNode(node, b, null);
            }

            _treeView.EndUpdate();
        }

        private void DataRoot_ContentChanged(object sender, ContentChangedEventArgs<TBase> e)
        {
            var target = sender == _dataRoot ? _treeView.Nodes : FindNode((TBase) sender).Nodes;

            _treeView.BeginUpdate();
            foreach (TBase data in e.ContentRemoved) FindNode(data).Remove();
            foreach (TBase data in e.ContentAdded) target.Add(Build(data));
            _treeView.EndUpdate();
        }
    }
}
