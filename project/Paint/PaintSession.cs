using Paint.Commands;
using Paint.Composite;
using Paint.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Paint
{
    public enum ToolType
    {
        Draw,
        Select,
        TempSelect
    }

    public enum ResizeOperation
    {
        None = -1,
        NW = 0,
        SE = 1,
        NE = 2,
        SW = 3,
        N = 4,
        S = 5,
        W = 6,
        E = 7
    }

    public enum PaintOperation
    {
        Idle,
        DrawShape,
        SelectArea,
        MoveDrawable,
        ResizeShape
    }

    public delegate void OnSelectionChangedDelegate(IReadOnlyList<IDrawable> selection);

    public class PaintSession : CommandQueue<PaintSession>
    {
        public event EventHandler<ContentChangedEventArgs<IDrawable>> SelectionChanged;

        #region FIELDS
        private readonly EventList<IDrawable> _selectedShapes;

        private readonly Canvas _canvas;
        #endregion

        #region PROPERTIES
        public Canvas Canvas { get => _canvas; }

        public bool IsMouseDown { get; set; } = false;

        public ToolType SelectedToolType { get; set; } = ToolType.Draw;
        public ShapeType SelectedShapeType { get; set; } = ShapeType.Rectangle;
        public PaintOperation CurrentOperation { 
            get {
                // When mouse is not down, only PaintOperation.None can be the current operation
                if (!IsMouseDown) return PaintOperation.Idle;
                // If CurrentResizeOperation is not set to None the user has to be performing a resize operation
                if (CurrentResizeOperation != ResizeOperation.None) return PaintOperation.ResizeShape;
                
                if (Selection.Any())
                {   // If selection is not empty and the current tooltype is select/tempselect the used has to be performing a drag operation
                    if (SelectedToolType != ToolType.Draw) return PaintOperation.MoveDrawable;
                } else 
                {   // When no content is selected and the selected tooltype is 'Select' the user is currently drag-selecting an area
                    if (SelectedToolType == ToolType.Select) return PaintOperation.SelectArea;
                    // With the mouse down and the SelectedTooltype being Draw the user has to be drawing at this point
                    if (SelectedToolType == ToolType.Draw) return PaintOperation.DrawShape;
                }

                // Code shouldn't ever reach this point
                return PaintOperation.Idle;
            }
        } 

        public Point MouseStartPos { get; set; }
        public Point MouseCurrentPos { get; set; }

        public ResizeOperation CurrentResizeOperation { get; set; } = ResizeOperation.None;
        public IReadOnlyList<IDrawable> Selection { get => new ReadOnlyCollection<IDrawable>(_selectedShapes); }
        #endregion

        #region CONSTRUCTOR
        public PaintSession(PictureBox paintArea)
        {
            _selectedShapes = new EventList<IDrawable>();
            _selectedShapes.ContentChanged += (s, e) => SelectionChanged?.Invoke(this,
                new ContentChangedEventArgs<IDrawable>(e.ContentBefore.ToArray(), e.ContentAfter.ToArray()));

            _canvas = new Canvas(paintArea);
            _canvas.ContentChanged += (s, e) =>
            {
                Console.WriteLine("Content changed for canvas");
            };

            _canvas.BranchModified += (s, e) =>
            {
                EventArgs sea = e.SourceEventArgs;
                if (sea is ContentChangedEventArgs<IDrawable> cce) Group_ContentChanged(e.SourceEventSender, cce);
                else if (sea is PropertyChangedEventArgs pce) Drawable_PropertyChanged(e.SourceEventSender, pce);
                else Console.WriteLine("Unhandled event {0}", e.SourceEventArgs);
            };
        }
        #endregion

        #region PUBLIC METHODS

        public bool IsIdle() => CurrentOperation == PaintOperation.Idle;
        public bool IsResizingShape() => CurrentOperation == PaintOperation.ResizeShape;
        public bool IsMovingDrawable() => CurrentOperation == PaintOperation.MoveDrawable;
        public bool IsSelectingArea() => CurrentOperation == PaintOperation.SelectArea;
        public bool IsDrawingShape() => CurrentOperation == PaintOperation.DrawShape;

        public bool IsToolTypeAnySelect() => 
            SelectedToolType == ToolType.Select || SelectedToolType == ToolType.TempSelect;

        #region CommandQueue
        public override void UndoLast()
        {
            base.UndoLast(this);
        }

        public override void RedoLast()
        {
            base.RedoLast(this);
        }

        public override void ExecuteCommand(ICommand<PaintSession> cmd)
        {
            var sw = new Stopwatch();

            sw.Start();
            base.ExecuteCommand(cmd, this);
            sw.Stop();

            //Console.WriteLine("Command '{0}' completed in {1} MS ({2} Ticks).", cmd.ToString(), sw.ElapsedMilliseconds, sw.Elapsed.Ticks);
        }
        #endregion

        #region Selection methods
        public bool IsSelected(IDrawable c)
        {
            return _selectedShapes.Any(ss => ss.ID == c.ID);
        }

        public bool IsAncestorSelected(IDrawable drawable)
        {
            var parent = drawable.Parent;

            while (parent != null)
            {
                if (Selection.Any(ss => ss.ID == parent.ID)) return true;
                parent = (IParentNode<IDrawable>) parent.Parent;
            }

            return false;
        }

        private IDrawable[] validateSelectionAdd(params IDrawable[] add)
        {
            return add.Where(c => c.ID != Guid.Empty && c.Root is Canvas).ToArray();
        }

        public void Select(params IDrawable[] children)
        {
            var valid = validateSelectionAdd(children);

            foreach (IDrawable c in valid)
            {
                if (!_selectedShapes.Contains(c))
                {
                    _selectedShapes.Add(c);
                }
            }
        }

        public void Deselect(params IDrawable[] children)
        {
            foreach (IDrawable c in children)
            {
                if (_selectedShapes.Contains(c))
                {
                    _selectedShapes.Remove(c);
                }
            }
        }

        public void SetSelection(params IDrawable[] s)
        {
            _selectedShapes.Clear();

            var valid = validateSelectionAdd(s);
            _selectedShapes.AddRange(valid);
        }

        public void ClearSelection()
        {
            _selectedShapes.Clear();
        }

        public void SelectShapesInArea(Rectangle selectArea)
        {
            var sw = new Stopwatch();
            sw.Start();

            var allDrawables = Canvas.GetAllDescendants();

            // Unwraps core drawables from their decorators
            IEnumerable<IDrawable> unDecoratedDrawables = allDrawables
                .Select(d => d is Ornament o ? o.EndPoint : d);

            // Filter shapes from groups, and aquire a reference to their ids & bounding rectangles
            Dictionary<Guid, Rectangle> mappedShapeRects = unDecoratedDrawables
                .OfType<Shape>()
                .Select(d => (d.ID, d.GetBoundingRect()))
                .ToDictionary(sv => sv.ID, sv => sv.Item2);

            // Determine which shapes' bounding rectangles intersect with the selected area
            IEnumerable<Guid> shapeIDsInArea = mappedShapeRects
                .Where(idToRect => idToRect.Value.IntersectsWith(selectArea))
                .Select(idToVerts => idToVerts.Key);

            // Fetch the shapes back in their wrapped forms from 'allDrawables', create an array from the results
            IDrawable[] shapesInArea = allDrawables.Where(d => shapeIDsInArea.Contains(d.ID)).ToArray();

            sw.Stop();

            Console.WriteLine("Queried shapes in selectArea in {0} MS ({1} Ticks)", sw.ElapsedMilliseconds, sw.ElapsedTicks);

            SetSelection(shapesInArea);
        }
        #endregion

        #region File IO
        /// <summary>
        /// Opens and loads canvas from a savefile.
        /// </summary>
        public void LoadCanvas()
        {
            var dialog = new OpenFileDialog
            {
                Filter = "Peent markup lenkwatsj (*.pml)|*.pml|All files (*.*)|*.*",
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                FilterIndex = 1,
                RestoreDirectory = true,
                FileName = "MijPeentProjeqt.pml"
            };

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                this.Canvas.Load(dialog.FileName);

                this.ClearUndo();
                this.ClearRedo();
            }
        }

        /// <summary>
        /// Saves the current canvas to a savefile.
        /// </summary>
        public void SaveCanvas()
        {
            var dialog = new SaveFileDialog
            {
                FileName = "MijPeentProjeqt.pml",
                Filter = "Peent markup lenkwatsj (*.pml)|*.pml|All files (*.*)|*.*",
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                RestoreDirectory = true,
                FilterIndex = 1
            };

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                if(Canvas.Save(dialog.FileName))
                {
                    Console.WriteLine("Canvas saved to '{0}'!", dialog.FileName);
                }
            }
        }

        /// <summary>
        /// Exports the current canvas as an image
        /// </summary>
        public void ExportCanvas()
        {
            var dialog = new SaveFileDialog
            {
                FileName = "MijPeentAart.png",
                Filter = "Bitmap image (*.png)|*.png",
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures),
                RestoreDirectory = true,
                FilterIndex = 1
            };

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                ClearSelection();

                Canvas.Export(dialog.FileName);
                Console.WriteLine("Canvas exported! {0}", dialog.FileName);
            }
            else
            {
                Console.WriteLine("Export canvas aborted...");
            }
        }
        #endregion

        /// <summary>
        /// Calculate the rectangle Origin point and size from the mouse starting position (Mouse down)
        /// and the current mouse position
        /// </summary>
        /// <returns>
        /// Rectangle based on mouse start and current position
        /// </returns>
        public Rectangle GetRectangleFromMouse()
        {
            return new Rectangle(
                // Calculate the top left corner as the origin point
                Math.Min(MouseStartPos.X, MouseCurrentPos.X),
                Math.Min(MouseStartPos.Y, MouseCurrentPos.Y),

                // Calculate the absolute width and height of the shape
                Math.Abs(MouseStartPos.X - MouseCurrentPos.X),
                Math.Abs(MouseStartPos.Y - MouseCurrentPos.Y));
        }

        public Size GetMouseDragOffset()
        {
            return new Size(MouseCurrentPos.X - MouseStartPos.X, MouseCurrentPos.Y - MouseStartPos.Y);
        }
        #endregion

        void Group_ContentChanged(object sender, ContentChangedEventArgs<IDrawable> args)
        {
            Console.WriteLine("Content changed for {0}", (sender as IDrawable).ID);
        }

        void Drawable_PropertyChanged(object sender, PropertyChangedEventArgs args)
        {
            Console.WriteLine("Property changed for {0}", (sender as IDrawable).ID);
        }
    }
}
