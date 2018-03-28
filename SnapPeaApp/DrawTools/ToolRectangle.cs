using System;
using System.Windows.Forms;
using System.Drawing;
using System.Linq;

namespace DrawTools
{
	/// <summary>
	/// Rectangle tool
	/// </summary>
	class ToolRectangle : DrawTools.ToolObject
	{

		public ToolRectangle()
		{
            Cursor = new Cursor(GetType(), "Rectangle.cur");
		}

        /// <summary>
        /// Event handler for mouse down.
        /// Creates a new rectangle
        /// </summary>
        /// <param name="drawArea"></param>
        /// <param name="e"></param>
        public override void OnMouseDown(DrawArea drawArea, MouseEventArgs e)
        {
            AddNewObject(drawArea, new DrawRectangle(e.X, e.Y, 1, 1));
        }

        /// <summary>
        /// Mouse up event handler
        /// </summary>
        /// <param name="drawArea"></param>
        /// <param name="e"></param>
        public override void OnMouseUp(DrawArea drawArea, MouseEventArgs e)
        {
            base.OnMouseUp(drawArea, e);
            drawArea.GraphicsList.GraphicsListChanged?.Invoke();
        }

        /// <summary>
        /// Mouse move event handler.
        /// Resizes rectangle on creation to follow cursor
        /// </summary>
        /// <param name="drawArea"></param>
        /// <param name="e"></param>
        public override void OnMouseMove(DrawArea drawArea, MouseEventArgs e)
        {
            drawArea.Cursor = Cursor;

            if ( e.Button == MouseButtons.Left )
            {
                Point point = new Point(e.X, e.Y);
                // CHECK COLLISION HERE
                DrawRectangle thisRectangle = (DrawRectangle)drawArea.GraphicsList[0];
                bool collision = false;
                foreach (DrawRectangle rect in drawArea.GraphicsList.Enumeration.Where(o => thisRectangle != o))
                {
                    if (rect.IntersectsWith(thisRectangle))
                    {
                        thisRectangle.MoveHandleTo(DrawRectangle.ClosestPointOutside(rect, point), 5);
                        collision = true;
                        break;
                    }
                }
                // keep handle in bounds
                drawArea.BoundPoint(ref point);
                if (!collision)
                {
                    thisRectangle.MoveHandleTo(point, 5);
                }
                drawArea.Refresh();
            }
        }
	}
}
