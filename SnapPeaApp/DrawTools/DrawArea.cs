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
        /// Right-click handler
        /// </summary>
        /// <param name="e"></param>
        private void OnContextMenu(MouseEventArgs e)
        {
            // Change current selection if necessary

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
                    Refresh();      // in the case selection was changed
                }
            }
            else
            {
                GraphicsList.UnselectAll();
            }


            //// Show context menu.
            //// Context menu items are filled from owner form Edit menu items.
            //m_ContextMenu = new ContextMenuStrip();

            //int nItems = owner.ContextParent.DropDownItems.Count;

            //// Read Edit items and move them to context menu.
            //// Since every move reduces number of items, read them in reverse order.
            //// To get items in direct order, insert each of them to beginning.
            //for (int i = nItems - 1; i >= 0; i--)
            //{
            //    m_ContextMenu.Items.Insert(0, owner.ContextParent.DropDownItems[i]);
            //}

            //// Show context menu for owner form, so that it handles items selection.
            //// Convert point from this window coordinates to owner's coordinates.
            //point.X += this.Left;
            //point.Y += this.Top;

            //m_ContextMenu.Show(owner, point);

            //Owner.SetStateOfControls();  // enable/disable menu items

            //// Context menu is shown, but owner's Edit menu is now empty.
            //// Subscribe to context menu Closed event and restore items there.
            //m_ContextMenu.Closed += delegate(object sender, ToolStripDropDownClosedEventArgs args)
            //{
            //    if (m_ContextMenu != null)      // precaution
            //    {
            //        nItems = m_ContextMenu.Items.Count;

            //        for (int k = nItems - 1; k >= 0; k--)
            //        {
            //            owner.ContextParent.DropDownItems.Insert(0, m_ContextMenu.Items[k]);
            //        }
            //    }
            //};
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

            //DrawNetSelection(e.Graphics);

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
