using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Linq;


namespace DrawTools
{
    /// <summary>
    /// User control for drawing shapes on.
    /// </summary>
    public partial class DrawArea : UserControl
    {

        #region Constructor

        public DrawArea()
        {
            InitializeComponent();
            Initialize();
        }

        #endregion Constructor

        #region Enumerations

        public enum DrawToolType
        {
            Pointer,
            Rectangle,
            NumberOfDrawTools
        };

        #endregion

        #region Members

        private GraphicsList graphicsList;    // list of draw objects
        // (instances of DrawObject-derived classes)

        private DrawToolType activeTool;      // active drawing tool
        private Tool[] tools;                 // array of tools


        #endregion

        #region Properties


        /// <summary>
        /// Active drawing tool.
        /// </summary>
        public DrawToolType ActiveTool
        {
            get
            {
                return activeTool;
            }
            set
            {
                activeTool = value;
            }
        }

        /// <summary>
        /// List of graphics objects.
        /// </summary>
        public GraphicsList GraphicsList
        {
            get
            {
                return graphicsList;
            }
            set
            {
                graphicsList = value;
            }
        }

        /// <summary>
        /// Tests whether a point is within the bounds of the control
        /// </summary>
        /// <remarks>If the test point is not in bounds the point is modified to be in bounds</remarks>
        /// <param name="point">the point to test</param>
        /// <returns>true if the original point is in bounds, else false</returns>
        internal bool BoundPoint(ref Point point)
        {
            bool inBoudns = true;
            if (point.X > Bounds.Right)
            {
                point.X = Bounds.Right;
                inBoudns = false;
            }
            if (point.Y > Bounds.Bottom)
            {
                point.Y = Bounds.Bottom;
                inBoudns = false;
            }
            if (point.X < Bounds.Left)
            {
                point.X = Bounds.Left;
                inBoudns = false;
            }
            if (point.Y < Bounds.Top)
            {
                point.Y = Bounds.Top;
                inBoudns = false;
            }

            return inBoudns;
        }


        #endregion

        #region Other Functions

        /// <summary>
        /// Initialization
        /// </summary>
        /// <param name="owner"></param>
        /// <param name="docManager"></param>
        public void Initialize()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint |
                ControlStyles.UserPaint | ControlStyles.OptimizedDoubleBuffer, true);
             
            // set default tool
            activeTool = DrawToolType.Rectangle;

            // create list of graphic objects
            graphicsList = new GraphicsList();

            // Create undo manager
            // create array of drawing tools
            tools = new Tool[(int)DrawToolType.NumberOfDrawTools];
            tools[(int)DrawToolType.Pointer] = new ToolPointer();
            tools[(int)DrawToolType.Rectangle] = new ToolRectangle();
        }

        /// <summary>
        /// Invalidates draw area and renders shapes. 
        /// Any intersecting shapes are colored red before redrawing.
        /// </summary>
        public override void Refresh()
        {
            GraphicsList.SetColorToAll(Color.Black);
            foreach(DrawRectangle r1 in GraphicsList.Enumeration)
            {
                foreach(DrawRectangle r2 in GraphicsList.Enumeration)
                {
                    if(r1 == r2)
                    {
                        continue;
                    }

                    if (r1.IntersectsWith(r2))
                    {
                        r1.Color = Color.Red;
                        r2.Color = Color.Red;
                    }
                }
            }
            base.Refresh();
        }

        /// <summary>
        /// Right-click handler. If the mouse was clicked within a shape, the shape is deleted.
        /// </summary>
        /// <param name="e"></param>
        private void OnContextMenu(MouseEventArgs e)
        {
            Point point = new Point(e.X, e.Y);

            int n = GraphicsList.Count;
            DrawObject o = null;

            for (int i = 0; i < n; i++)
            {
                if (GraphicsList[i].HitTest(point) == 0)
                {
                    o = GraphicsList[i];
                    break;
                }
            }

            if (o != null)
            {
                if (!o.Selected)
                    GraphicsList.UnselectAll();

                // Select clicked object
                o.Selected = true;
                if (GraphicsList.DeleteSelection())
                {
                    GraphicsList.GraphicsListChanged?.Invoke();
                    Refresh();      // in the case selection was changed
                }
            }
            else
            {
                GraphicsList.UnselectAll();
            }
        }

        #endregion

        #region Event Handlers

        /// <summary>
        /// Draw graphic objects and 
        /// group selection rectangle (optionally)
        /// </summary>
        private void DrawArea_Paint(object sender, PaintEventArgs e)
        {
            SolidBrush brush = new SolidBrush(Color.FromArgb(255, 255, 255));
            e.Graphics.FillRectangle(brush,
                this.ClientRectangle);

            if (graphicsList != null)
            {
                graphicsList.Draw(e.Graphics);
            }

            brush.Dispose();
        }

        /// <summary>
        /// Mouse down.
        /// Left button down event is passed to active tool.
        /// Right button down event is handled in this class.
        /// </summary>
        private void DrawArea_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
                tools[(int)activeTool].OnMouseDown(this, e);
            else if (e.Button == MouseButtons.Right)
            {
                activeTool = DrawToolType.Pointer;
                OnContextMenu(e);
                activeTool = DrawToolType.Rectangle;
            }
        }

        /// <summary>
        /// Mouse move.
        /// Moving without button pressed or with left button pressed
        /// is passed to active tool.
        /// </summary>
        private void DrawArea_MouseMove(object sender, MouseEventArgs e)
        {
            // test mouse over rectangle
            if(e.Button == MouseButtons.None)
            {
                var point = new Point(e.X, e.Y);
                if (GraphicsList.Enumeration.Any(o => o.HitTest(point) > -1))
                {
                    activeTool = DrawToolType.Pointer;
                }
                else
                {
                    activeTool = DrawToolType.Rectangle;
                }
            }

            if (e.Button == MouseButtons.Left || e.Button == MouseButtons.None)
                tools[(int)activeTool].OnMouseMove(this, e);
            else
                this.Cursor = Cursors.Default;
        }

        /// <summary>
        /// Mouse up event.
        /// Left button up event is passed to active tool.
        /// </summary>
        private void DrawArea_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
                tools[(int)activeTool].OnMouseUp(this, e);
        }

        #endregion Event Handlers

    }
}
