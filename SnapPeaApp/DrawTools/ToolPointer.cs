using System;
using System.Windows.Forms;
using System.Drawing;
using System.Linq;

namespace DrawTools
{
	/// <summary>
	/// Pointer tool
	/// </summary>
	class ToolPointer : DrawTools.Tool
	{
        private enum SelectionMode
        {
            None,
            NetSelection,   // group selection is active
            Move,           // object(s) are moves
            Size            // object is resized
        }

        private SelectionMode selectMode = SelectionMode.None;

        // Object which is currently resized:
        private DrawObject resizedObject;
        private int resizedObjectHandle;

        // Keep state about last and current point (used to move and resize objects)
        private Point lastPoint = new Point(0,0);
        private Point startPoint = new Point(0, 0);

		public ToolPointer()
		{
		}

        /// <summary>
        /// Left mouse button is pressed
        /// </summary>
        /// <param name="drawArea"></param>
        /// <param name="e"></param>
        public override void OnMouseDown(DrawArea drawArea, MouseEventArgs e)
        {
            selectMode = SelectionMode.None;
            Point point = new Point(e.X, e.Y);

            // Test for resizing (only if control is selected, cursor is on the handle)
            foreach (DrawObject o in drawArea.GraphicsList.Selection)
            {
                int handleNumber = o.HitTest(point);

                if (handleNumber > 0)
                {
                    selectMode = SelectionMode.Size;

                    // keep resized object in class member
                    resizedObject = o;
                    resizedObjectHandle = handleNumber;

                    // Since we want to resize only one object, unselect all other objects
                    drawArea.GraphicsList.UnselectAll();
                    o.Selected = true;

                    break;
                }
            }

            // Test for move (cursor is on the object)
            if ( selectMode == SelectionMode.None )
            {
                int n1 = drawArea.GraphicsList.Count;
                DrawObject o = null;

                for ( int i = 0; i < n1; i++ )
                {
                    if ( drawArea.GraphicsList[i].HitTest(point) == 0 )
                    {
                        o = drawArea.GraphicsList[i];
                        break;
                    }
                }

                if ( o != null )
                {
                    selectMode = SelectionMode.Move;

                    // Unselect all if Ctrl is not pressed and clicked object is not selected yet
                    if ( ( Control.ModifierKeys & Keys.Control ) == 0  && !o.Selected )
                        drawArea.GraphicsList.UnselectAll();

                    // Select clicked object
                    o.Selected = true;

                    drawArea.Cursor = Cursors.SizeAll;
                }
            }

            // Net selection
            if ( selectMode == SelectionMode.None )
            {
                // click on background
                if ( ( Control.ModifierKeys & Keys.Control ) == 0 )
                    drawArea.GraphicsList.UnselectAll();

                selectMode = SelectionMode.NetSelection;

            }

            lastPoint.X = e.X;
            lastPoint.Y = e.Y;
            startPoint.X = e.X;
            startPoint.Y = e.Y;

            drawArea.Capture = true;

            drawArea.Refresh();

            if ( selectMode == SelectionMode.NetSelection )
            {
                // Draw selection rectangle in initial position
                ControlPaint.DrawReversibleFrame(
                    drawArea.RectangleToScreen(DrawRectangle.GetNormalizedRectangle(startPoint, lastPoint)),
                    Color.Black,
                    FrameStyle.Dashed);
            }
        }


        /// <summary>
        /// Mouse is moved.
        /// None button is pressed, or left button is pressed.
        /// </summary>
        /// <param name="drawArea"></param>
        /// <param name="e"></param>
        public override void OnMouseMove(DrawArea drawArea, MouseEventArgs e)
        {
            Point point = new Point(e.X, e.Y);
            Point oldPoint = lastPoint;

            // set cursor when mouse button is not pressed
            if (e.Button == MouseButtons.None)
            {
                Cursor cursor = null;

                for (int i = 0; i < drawArea.GraphicsList.Count; i++)
                {
                    int n = drawArea.GraphicsList[i].HitTest(point);

                    if (n > 0)
                    {
                        cursor = drawArea.GraphicsList[i].GetHandleCursor(n);
                        break;
                    }
                }

                if (cursor == null)
                    cursor = Cursors.Default;

                drawArea.Cursor = cursor;

                return;
            }

            if (e.Button != MouseButtons.Left)
                return;

            /// Left button is pressed

            // Find difference between previous and current position
            int dx = e.X - lastPoint.X;
            int dy = e.Y - lastPoint.Y;

            lastPoint.X = e.X;
            lastPoint.Y = e.Y;

            // resize
            if (selectMode == SelectionMode.Size)
            {
                if (resizedObject != null)
                {
                    // CHECK COLLISION HERE
                    bool collision = false;
                    foreach (DrawRectangle rect in drawArea.GraphicsList.Enumeration.Where(o => resizedObject != o))
                    {
                        if (resizedObject.IntersectsWith(rect))
                        {
                            resizedObject.MoveHandleTo(DrawRectangle.ClosestPointOutside(rect,point), resizedObjectHandle);
                            collision = true;
                            break;
                        }
                    }
                    if(!collision)
                    {
                        resizedObject.MoveHandleTo(point, resizedObjectHandle);
                    }


                    drawArea.Refresh();
                }
            }

            // move
            if (selectMode == SelectionMode.Move)
            {
                drawArea.Cursor = Cursors.SizeAll;
                foreach (DrawRectangle o in drawArea.GraphicsList.Selection)
                {
                    o.Move(dx, dy);

                    // CHECK COLLISION
                    foreach (DrawRectangle rect in drawArea.GraphicsList.Enumeration.Where(x => x != o && o.IntersectsWith((DrawRectangle)x)))
                    {
                        o.Move(-dx, -dy);
                        break;
                    }
                }

                drawArea.Refresh();
            }

            if (selectMode == SelectionMode.NetSelection)
            {
                // Remove old selection rectangle
                ControlPaint.DrawReversibleFrame(
                    drawArea.RectangleToScreen(DrawRectangle.GetNormalizedRectangle(startPoint, oldPoint)),
                    Color.Black,
                    FrameStyle.Dashed);

                // Draw new selection rectangle
                ControlPaint.DrawReversibleFrame(
                    drawArea.RectangleToScreen(DrawRectangle.GetNormalizedRectangle(startPoint, point)),
                    Color.Black,
                    FrameStyle.Dashed);

                return;
            }

        }

        /// <summary>
        /// Right mouse button is released
        /// </summary>
        /// <param name="drawArea"></param>
        /// <param name="e"></param>
        public override void OnMouseUp(DrawArea drawArea, MouseEventArgs e)
        {
            if ( selectMode == SelectionMode.NetSelection )
            {
                // Remove old selection rectangle
                ControlPaint.DrawReversibleFrame(
                    drawArea.RectangleToScreen(DrawRectangle.GetNormalizedRectangle(startPoint, lastPoint)),
                    Color.Black,
                    FrameStyle.Dashed);

                // Make group selection
                drawArea.GraphicsList.SelectInRectangle(
                    DrawRectangle.GetNormalizedRectangle(startPoint, lastPoint));

                selectMode = SelectionMode.None;
            }

            if ( resizedObject != null )
            {
                // after resizing
                resizedObject.Normalize();
                resizedObject = null;
            }

            drawArea.Capture = false;
            drawArea.Refresh();
        }
	}
}
