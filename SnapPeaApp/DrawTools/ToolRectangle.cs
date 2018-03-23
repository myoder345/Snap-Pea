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

        public override void OnMouseDown(DrawArea drawArea, MouseEventArgs e)
        {
            AddNewObject(drawArea, new DrawRectangle(e.X, e.Y, 1, 1));
        }

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
                    //if (rect.HitTest(point) > -1)
                    if (rect.IntersectsWith(thisRectangle))
                    {
                        thisRectangle.MoveHandleTo(DrawRectangle.ClosestPointOutside(rect, point), 5);
                        collision = true;
                        break;
                    }
                }
                if (!collision)
                {
                    thisRectangle.MoveHandleTo(point, 5);
                }
                //drawArea.GraphicsList[0].MoveHandleTo(point, 5);
                drawArea.Refresh();
            }
        }
	}
}
