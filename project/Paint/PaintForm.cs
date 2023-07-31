using Paint.Commands;
using Paint.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Paint
{
    public partial class PaintForm : Form
    {
        #region FIELDS
        private readonly PaintSession _session;
        private readonly ComponentForm _componentForm;

        private ToolStripButton[] EditModeButtons, ShapeSelectionButtons;
        private Timer _refreshTimer;
        #endregion

        #region CONSTRUCTOR
        public PaintForm()
        {
            InitializeComponent();

            // Use the cross "+" cursor
            base.Cursor = Cursors.Cross;

            // This will reduce flicker (Recommended)
            base.DoubleBuffered = true;

            // TODO: PaintSession should be constructed and instantiate all forms ...
            // instead of this form instantiating PaintSession.
            _session = new PaintSession(PaintArea);

            _session.SelectionChanged += (s, e) => RefreshDelayed();
            _session.Canvas.BranchModified += (s, e) => RefreshDelayed();

            _componentForm = new ComponentForm(_session)
            {
                TopLevel = false, Dock = DockStyle.Right
            };

            _componentForm.Show();

            panel1.Controls.Add(_componentForm);
        }
        #endregion

        #region HELPERS
        private void RefreshDelayed(int ms = 50)
        {
            if(_refreshTimer == null)
            {
                _refreshTimer = new Timer { Interval = ms };
                _refreshTimer.Tick += new EventHandler((sender, args) =>
                {
                    _refreshTimer.Enabled = false;
                    Refresh();
                });
            } 

            _refreshTimer.Enabled = true;
        }

        /// <summary>
        /// Update cursor type based on current position of the mouse
        /// </summary>
        /// <param name="mousePosition">Position of the mouse</param>
        /// <param name="resizeBoxes">Array of rectangles (Borderboxes)</param>
        private void UpdateCursor()
        {
            if (_session.IsMouseDown)
            {   // Lock cursor while primary mouse button is being pressed
                return;
            }

            if (_session.Selection.Any(s => s.ContainsPoint(_session.MouseCurrentPos)))
            {   // Cursor is on a shape
                this.Cursor = Cursors.SizeAll;
            }
            else if (_session.CurrentResizeOperation == ResizeOperation.None)
            {   // Cursor is not on a shape
                this.Cursor = Cursors.Cross;
            }

            if (_session.Selection.Any())
            {   // A shape is currently selected
                // Fetch and flatten hierarchy of the Resize-box rectangles, we only need to know 
                // whether the mouse is on a Resize-box and its direction to update the cursor.
                var resizeBoxes = ShapeUtils.GetResizeBoxesByShape(_session.Selection)
                    .SelectMany(kvp => kvp.Value).ToList();

                for (int i = 0; i < resizeBoxes.Count; i++)
                {
                    if (resizeBoxes[i].Contains(_session.MouseCurrentPos))
                    {   // Mouse is inside a resize-box
                        this.Cursor = ((ResizeOperation)(i % 8)).GetCursor();
                    }
                }
            }
        }

        /// <summary>
        /// Checks whether the mousecursor is inside any of the provided border boxes, and selects it
        /// </summary>
        /// <param name="resizeBoxes">List with rectangles (resizeboxes)</param>
        private bool IsResizeOperationAtCurrentMousePos(Dictionary<Guid, Rectangle[]> resizeBoxes, 
            out ResizeOperation op, out IDrawable d)
        {
            foreach (Guid id in resizeBoxes.Keys)
            {
                for (int i = 0; i < 8; i++)
                {
                    if (resizeBoxes[id][i].Contains(_session.MouseCurrentPos))
                    {
                        op = (ResizeOperation) i;
                        d = _session.Selection.Where(s => s.ID == id).Single();
                        return true;
                    }
                }
            }

            op = ResizeOperation.None; d = null;
            return false;
        }

        /// <summary>
        /// Select the provided button and deselect the others in its group.
        /// </summary>
        /// <param name="selected_button">Button clicked by user</param>
        /// <param name="buttons">List of buttons</param>
        private void SelectToolStripButton(ToolStripButton selected_button, ToolStripButton[] buttons)
        {
            foreach (ToolStripButton button in buttons)
            {
                button.Checked = (button == selected_button);
            }
        }

        /// <summary>
        /// Select provided tooltype and update the text in the statusstrip
        /// </summary>
        private void SelectToolType(ToolType toolType, string labelText)
        {
            _session.SelectedToolType = toolType;
            SelectedToolLabel.Text = labelText;
        }
        #endregion

        #region EVENT HANDLERS
        private void PaintForm_Load(object sender, EventArgs args)
        {
            // Prepare the shape tools.
            EditModeButtons = new[] { toolSelectButton, toolDrawButton };
            ShapeSelectionButtons = new[] { shapeRectangleButton, shapeEllipseButton };

            // Register single-line event handlers
            ClearButton.Click += (s, e) => _session.ExecuteCommand(new ClearCommand());
            UndoButton.Click += (s, e) => { _session.UndoLast(); RefreshDelayed(); };
            RedoButton.Click += (s, e) => { _session.RedoLast(); RefreshDelayed(); };

            OpenToolStripMenuItem.Click +=  (s, e) => _session.LoadCanvas();
            ExportToolStripMenuItem.Click += (s, e) => _session.ExportCanvas();
            SaveToolStripMenuItem.Click += (s, e) => _session.SaveCanvas();

            ToolStripSaveButton.Click += (s, e) => _session.SaveCanvas();
            ToolStripOpenButton.Click += (s, e) => _session.LoadCanvas();

            DeleteButton.Click += (s, e) => _session.ExecuteCommand(
                new RemoveDrawableCommand(_session.Selection.Select(ss => ss.ID).ToArray()));

            statusStripVisibleButton.Click += (s, e) 
                => statusStripVisibleButton.Checked = statusStrip.Visible = !statusStrip.Visible;

            groupsVisibleButton.Click += (s, e)
                => groupsVisibleButton.Checked = _componentForm.Visible = !_componentForm.Visible;

            toolStripVisibleButton.Click += (s, e)
                => toolStripVisibleButton.Checked = toolStrip.Visible = !toolStrip.Visible;
        }

        private void PaintArea_Paint(object sender, PaintEventArgs e)
        {
            // (Re)draw the canvas and its content onto PaintArea
            _session.Canvas.DrawStrategy.Draw(e.Graphics, _session.Canvas, _session);

            // If user is currently drawing a new shape ...
            if (_session.CurrentOperation == PaintOperation.DrawShape)
            {   // Draw a temporary shape that does not actually exist in the 'model'
                IDrawable drawPreview = new UIShape(_session.SelectedShapeType, _session.GetRectangleFromMouse(), UIShape.UIShapeType.DrawingPreview);
                drawPreview.DrawStrategy.Draw(e.Graphics, drawPreview, _session);
            }

            // If user is currently selecting an area ...
            if (_session.CurrentOperation == PaintOperation.SelectArea)
            {   // Draw UI giving feedback what area the user is drag-selecting
                IDrawable selectionAreaPreview = new UIShape(_session.GetRectangleFromMouse(), UIShape.UIShapeType.SelectionPreview);
                selectionAreaPreview.DrawStrategy.Draw(e.Graphics, selectionAreaPreview, _session);
            }
        }

        private void PaintArea_MouseMove(object sender, MouseEventArgs e)
        {
            _session.MouseCurrentPos = e.Location;
            UpdateCursor();

            if (_session.IsMouseDown)
            {
                Refresh();
            }
        }

        private void PaintArea_MouseDown(object sender, MouseEventArgs e)
        {
            _session.IsMouseDown = true;
            _session.MouseCurrentPos = _session.MouseStartPos = e.Location;

            var resizeBoxes = ShapeUtils.GetResizeBoxesByShape(_session.Selection);

            // Start a resize-operation when a border-box has been clicked
            if(IsResizeOperationAtCurrentMousePos(resizeBoxes,
                out ResizeOperation op, out IDrawable resizedDrawable))
            {
                _session.SetSelection(resizedDrawable);
            } 
            _session.CurrentResizeOperation = op;

            if (_session.SelectedToolType == ToolType.Draw)
            {   // Is a selected shape being clicked or is a selected shape being resized?
                if (_session.Selection.Any(s => s.ContainsPoint(e.Location)) || op != ResizeOperation.None) 
                {   // User just clicked a selected shape or one of its resize-boxes
                    _session.SelectedToolType = ToolType.TempSelect;
                    SelectedToolLabel.Text = "Temporary Select";
                }   
                // Mouse is not within any selected shape, clear selection list
                else _session.ClearSelection();
            }

            // Is a selection-tool currently selected?
            else if (_session.IsToolTypeAnySelect())
            {   // Are there any drawables selected at this moment?
                if (_session.Selection.Any())
                {   // If no shape is currently being resized
                    if (!_session.IsResizingShape())
                    {   // If mouse was rightclicked or mouse position is outside the currently selected shape...
                        if (e.Button == MouseButtons.Right || 
                            !_session.Selection.Any(ss => ss.ContainsPoint(e.Location)))
                        {   // Clear selected drawables.
                            _session.ClearSelection();
                        }
                    }
                }

                var leafs = _session.Canvas.GetAllDescendants()
                    .Where(d => !(d is DrawableGroup)).ToList();

                foreach (IDrawable d in leafs)
                {
                    if (!_session.Selection.Any() && d.ContainsPoint(_session.MouseCurrentPos))
                    {   // No shape is currently selected and mouse is within drawable d
                        _session.SetSelection(d);
                        break;
                    }
                }
            }
            PaintOperationLabel.Text = _session.CurrentOperation.ToString();
        }

        private void PaintArea_MouseUp(object sender, MouseEventArgs e)
        {
            if (_session.IsMouseDown && _session.SelectedToolType == ToolType.Draw)
            {   // If a draw operation was being executed ...
                Rectangle shapeBase = _session.GetRectangleFromMouse();

                if (shapeBase.Width > 0 && shapeBase.Height > 0)
                {   // If drawn shape actually has an area ...
                    // ... create a new shape object using 'shapeBase' and the currently selected type of shape to be used.
                    _session.ExecuteCommand(new CreateShapeCommand(_session.SelectedShapeType, shapeBase.Location, shapeBase.Size));
                }
            }
            else if (_session.IsToolTypeAnySelect())
            {
                if(_session.SelectedToolType == ToolType.TempSelect)
                {   // Tool was Temporary Select, so time to switch back to the draw tool
                    _session.SelectedToolType = ToolType.Draw;
                    SelectedToolLabel.Text = "Draw";
                }

                if (_session.IsMouseDown && _session.Selection.Any())
                {   // A shape has been edited, so execute edit command now based on the edited point and size
                    if (_session.CurrentResizeOperation == ResizeOperation.None)
                    {
                        var selectedIDs = _session.Selection.Select(ss => ss.ID).ToArray();
                        _session.ExecuteCommand(new MoveDrawableCommand(_session.GetMouseDragOffset(), selectedIDs));
                    }
                    else
                    {
                        _session.ExecuteCommand(new ResizeShapeCommand(
                            _session.Selection.Single().ID, 
                            _session.GetMouseDragOffset(), 
                            _session.CurrentResizeOperation
                        ));
                    }
                }

                if (e.Button == MouseButtons.Left)
                {
                    if(_session.IsSelectingArea())
                    {
                        Rectangle selectArea = _session.GetRectangleFromMouse();
                        Console.WriteLine("Selecting {0} to {1}!", selectArea.Location, selectArea.GetVertices()[2]);
                        _session.SelectShapesInArea(selectArea);
                        
                        RefreshDelayed();
                    }
                }
            }

            // Clear selected border box (read 'dragged' border-box)
            _session.CurrentResizeOperation = ResizeOperation.None;
            _session.IsMouseDown = false;
            PaintOperationLabel.Text = _session.CurrentOperation.ToString();
        }

        private void PaintArea_MouseLeave(object sender, EventArgs e)
        {
            this.Cursor = Cursors.Default;
        }

        private void ToolSelectButton_Click(object sender, EventArgs e)
        {
            SelectToolStripButton(sender as ToolStripButton, EditModeButtons);

            SelectToolType(ToolType.Select, "Select");
        }

        private void ToolDrawButton_Click(object sender, EventArgs e)
        {
            SelectToolStripButton(sender as ToolStripButton, EditModeButtons);

            SelectToolType(ToolType.Draw, "Draw");
        }

        private void ShapeEllipseButton_Click(object sender, EventArgs e)
        {
            SelectToolStripButton(sender as ToolStripButton, ShapeSelectionButtons);

            _session.SelectedShapeType = ShapeType.Ellipse;

            SelectToolStripButton(toolDrawButton, EditModeButtons);

            SelectToolType(ToolType.Draw, "Draw");
        }

        private void ShapeRectangleButton_Click(object sender, EventArgs e)
        {
            SelectToolStripButton(sender as ToolStripButton, ShapeSelectionButtons);

            _session.SelectedShapeType = ShapeType.Rectangle;

            SelectToolStripButton(toolDrawButton, EditModeButtons);

            SelectToolType(ToolType.Draw, "Draw");
        }

        private void PaintForm_KeyDown(object sender, KeyEventArgs e)
        {
            switch(e.KeyCode)
            {
                case Keys.Z:
                    if (e.Control)
                    {
                        if (e.Shift) _session.RedoLast();
                        else _session.UndoLast();
                        e.Handled = true;
                    }
                    break;

                case Keys.Y:
                    if (e.Control)
                    {
                        _session.RedoLast();
                        e.Handled = true;
                    }
                    break;

                case Keys.A:
                    if (e.Control)
                    {
                        _session.Select(_session.Canvas.GetAllDescendants().ToArray());
                        e.Handled = true;
                    }
                    break;
                    
                case Keys.Delete:
                    Guid[] ids = _session.Selection.Select(ss => ss.ID).ToArray();
                    _session.ExecuteCommand(new RemoveDrawableCommand(ids));
                    e.Handled = true;
                    break;
            }
        }
        #endregion
    }
}
