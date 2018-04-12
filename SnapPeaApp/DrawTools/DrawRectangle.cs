using System;
using System.Windows.Forms;
using System.Drawing;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.Serialization;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace DrawTools
{
	/// <summary>
	/// Rectangle graphic object
	/// </summary>
	public class DrawRectangle : DrawTools.DrawObject
	{
        private Rectangle _rectangle;
        public Rectangle Rectangle
        {
            get
            {
                return _rectangle;
            }
            protected set
            {
                _rectangle = value;
            }
        }
        
		public DrawRectangle() : this(0, 0, 1, 1)
		{
		}


        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "y")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "x")]
        public DrawRectangle(int x, int y, int width, int height) : base()
        {
            _rectangle.X = x;
            _rectangle.Y = y;
            _rectangle.Width = width;
            _rectangle.Height = height;
            Initialize();
        }

        /// <summary>
        /// Clone this instance
        /// </summary>
        public override DrawObject Clone()
        {
            DrawRectangle drawRectangle = new DrawRectangle
            {
                _rectangle = this._rectangle
            };

            FillDrawObjectFields(drawRectangle);
            return drawRectangle;
        }


        /// <summary>
        /// Draw rectangle
        /// </summary>
        /// <param name="graphics"></param>
        public override void Draw(Graphics graphics)
        {
            Pen pen = new Pen(Color, PenWidth);

            graphics?.DrawRectangle(pen, DrawRectangle.GetNormalizedRectangle(Rectangle));

            pen.Dispose();
        }

        /// <summary>
        /// Modifys the internal rectangle representation according to the params
        /// </summary>
        /// <param name="x">Top left corner X coordinate</param>
        /// <param name="y">Top left corner Y coordinate</param>
        /// <param name="width">Rectangle width</param>
        /// <param name="height">Rectangle height</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "y")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "x")]
        protected void SetRectangle(int x, int y, int width, int height)
        {
            _rectangle.X = x;
            _rectangle.Y = y;
            _rectangle.Width = width;
            _rectangle.Height = height;
        }


        /// <summary>
        /// Get number of handles
        /// </summary>
        public override int HandleCount
        {
            get
            {
                return 8;
            }
        }


        /// <summary>
        /// Get handle point by 1-based number
        /// </summary>
        /// <param name="handleNumber"></param>
        /// <returns></returns>
        public override Point GetHandle(int handleNumber)
        {
            int x, y, xCenter, yCenter;

            xCenter = _rectangle.X + _rectangle.Width/2;
            yCenter = _rectangle.Y + _rectangle.Height/2;
            x = _rectangle.X;
            y = _rectangle.Y;

            switch ( handleNumber )
            {
                case 1:
                    x = _rectangle.X;
                    y = _rectangle.Y;
                    break;
                case 2:
                    x = xCenter;
                    y = _rectangle.Y;
                    break;
                case 3:
                    x = _rectangle.Right;
                    y = _rectangle.Y;
                    break;
                case 4:
                    x = _rectangle.Right;
                    y = yCenter;
                    break;
                case 5:
                    x = _rectangle.Right;
                    y = _rectangle.Bottom;
                    break;
                case 6:
                    x = xCenter;
                    y = _rectangle.Bottom;
                    break;
                case 7:
                    x = _rectangle.X;
                    y = _rectangle.Bottom;
                    break;
                case 8:
                    x = _rectangle.X;
                    y = yCenter;
                    break;
            }

            return new Point(x, y);

        }

        /// <summary>
        /// Hit test.
        /// Return value: -1 - no hit
        ///                0 - hit anywhere
        ///                > 1 - handle number
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public override int HitTest(Point point)
        {
            for ( int i = 1; i <= HandleCount; i++ )
            {
                if ( GetHandleRectangle(i).Contains(point) )
                    return i;
            }
            

            if ( PointInObject(point) )
                return 0;

            return -1;
        }

        protected override bool PointInObject(Point point)
        {
            return _rectangle.Contains(point);
        }

        /// <summary>
        /// Get cursor for the handle
        /// </summary>
        /// <param name="handleNumber"></param>
        /// <returns></returns>
        public override Cursor GetHandleCursor(int handleNumber)
        {
            switch ( handleNumber )
            {
                case 1:
                    return Cursors.SizeNWSE;
                case 2:
                    return Cursors.SizeNS;
                case 3:
                    return Cursors.SizeNESW;
                case 4:
                    return Cursors.SizeWE;
                case 5:
                    return Cursors.SizeNWSE;
                case 6:
                    return Cursors.SizeNS;
                case 7:
                    return Cursors.SizeNESW;
                case 8:
                    return Cursors.SizeWE;
                default:
                    return Cursors.Default;
            }
        }

        /// <summary>
        /// Move handle to new point (resizing)
        /// </summary>
        /// <param name="point"></param>
        /// <param name="handleNumber"></param>
        public override void MoveHandleTo(Point point, int handleNumber)
        {
            int left = Rectangle.Left;
            int top = Rectangle.Top;
            int right = Rectangle.Right;
            int bottom = Rectangle.Bottom;

            switch ( handleNumber )
            {
                case 1:
                    left = point.X;
                    top = point.Y;
                    break;
                case 2:
                    top = point.Y;
                    break;
                case 3:
                    right = point.X;
                    top = point.Y;
                    break;
                case 4:
                    right = point.X;
                    break;
                case 5:
                    right = point.X;
                    bottom = point.Y;
                    break;
                case 6:
                    bottom = point.Y;
                    break;
                case 7:
                    left = point.X;
                    bottom = point.Y;
                    break;
                case 8:
                    left = point.X;
                    break;
            }
          
            SetRectangle(left, top, right - left, bottom - top);
        }

        /// <summary>
        /// Repositions a rectangle such that it fits within the bounds created by maxX and maxY
        /// </summary>
        /// <param name="maxX"></param>
        /// <param name="maxY"></param>
        public void FixBounds(int maxX, int maxY)
        {
            int newX = Rectangle.Left;
            int newY = Rectangle.Top;

            if(Rectangle.Left < 0)
            {
                newX = 0;
            }
            else if(Rectangle.Right > maxX)
            {
                newX -= Rectangle.Right - maxX;
            }

            if(Rectangle.Top < 0)
            {
                newY = 0;
            }
            else if(Rectangle.Bottom > maxY)
            {
                newY -= Rectangle.Bottom - maxY;
            }

            _rectangle.X = newX;
            _rectangle.Y = newY;
        }

        public override bool IntersectsWith(Rectangle rectangle)
        {
            return Rectangle.IntersectsWith(rectangle);
        }

        public override bool IntersectsWith(DrawRectangle rectangle)
        {
            if (rectangle == null)
                throw new ArgumentNullException(nameof(rectangle));

            var RectB = GetNormalizedRectangle(Rectangle);
            var RectA = GetNormalizedRectangle(rectangle.Rectangle);
            return (RectA.Left < RectB.Right && RectA.Right > RectB.Left && RectA.Top < RectB.Bottom && RectA.Bottom > RectB.Top);
        }

        /// <summary>
        /// Move object
        /// </summary>
        /// <param name="deltaX">x axis translation</param>
        /// <param name="deltaY">y axis translation</param>
        public override void Move(int deltaX, int deltaY)
        {
            _rectangle.X += deltaX;
            _rectangle.Y += deltaY;
            
        }

        /// <summary>
        /// Move object
        /// </summary>
        /// <param name="x">X coordinate to move top left corner to</param>
        /// <param name="y">Y coordinate to move top left corner to</param>
        public override void MoveTo(int x, int y)
        {
            _rectangle.X = x;
            _rectangle.Y = y;
        }

        /// <summary>
        /// Prints debug info
        /// </summary>
        public override void Dump()
        {
            base.Dump ();

            Trace.WriteLine("rectangle.X = " + _rectangle.X.ToString(CultureInfo.InvariantCulture));
            Trace.WriteLine("rectangle.Y = " + _rectangle.Y.ToString(CultureInfo.InvariantCulture));
            Trace.WriteLine("rectangle.Width = " + _rectangle.Width.ToString(CultureInfo.InvariantCulture));
            Trace.WriteLine("rectangle.Height = " + _rectangle.Height.ToString(CultureInfo.InvariantCulture));
        }

        /// <summary>
        /// Normalize rectangle
        /// </summary>
        public override void Normalize()
        {
            _rectangle = DrawRectangle.GetNormalizedRectangle(_rectangle);
        }

        #region Helper Functions
        /// <summary>
        /// Returns a normalized version of the rectangle defined by the parameters
        /// </summary>
        /// <param name="x1">Top left corner x coordinate</param>
        /// <param name="y1">Top left corner y coordinate</param>
        /// <param name="x2">Bottom right corner x coordinate</param>
        /// <param name="y2">Bottom right corner y coordinate</param>
        /// <returns>Normalized rectangle</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "y")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "x")]
        public static Rectangle GetNormalizedRectangle(int x1, int y1, int x2, int y2)
        {
            if ( x2 < x1 )
            {
                int tmp = x2;
                x2 = x1;
                x1 = tmp;
            }

            if ( y2 < y1 )
            {
                int tmp = y2;
                y2 = y1;
                y1 = tmp;
            }

            return new Rectangle(x1, y1, x2 - x1, y2 - y1);
        }

        /// <summary>
        /// Returns normalized version of the rectangle defined by the parameters
        /// </summary>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        /// <returns></returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "p")]
        public static Rectangle GetNormalizedRectangle(Point p1, Point p2)
        {
            return GetNormalizedRectangle(p1.X, p1.Y, p2.X, p2.Y);
        }

        /// <summary>
        /// Returns normalized version of the rectangle defined by the parameter
        /// </summary>
        /// <param name="rect"></param>
        /// <returns></returns>
        public static Rectangle GetNormalizedRectangle(Rectangle rect)
        {
            return GetNormalizedRectangle(rect.X, rect.Y, rect.X + rect.Width, rect.Y + rect.Height);
        }

        /// <summary>
        /// Calculates the minimum distnace required to be outside the rectangle and returns a point at that location
        /// </summary>
        /// <param name="drawRect"></param>
        /// <param name="point"></param>
        /// <returns></returns>
        public static Point ClosestPointOutside(DrawRectangle drawRect, Point point)
        {
            if (drawRect == null)
                throw new ArgumentNullException(nameof(drawRect));

            var rect = drawRect.Rectangle;
            var points = new List<KeyValuePair<int, Point>>
            {
                new KeyValuePair<int, Point>(Math.Abs(point.X - rect.Left), new Point(rect.Left, point.Y)),
                new KeyValuePair<int, Point>(Math.Abs(point.X - rect.Right), new Point(rect.Right, point.Y)),
                new KeyValuePair<int, Point>(Math.Abs(point.Y - rect.Top), new Point(point.X, rect.Top)),
                new KeyValuePair<int, Point>(Math.Abs(point.Y - rect.Bottom), new Point(point.X, rect.Bottom))
            };
            points.Sort((p1, p2) => p1.Key.CompareTo(p2.Key));

            return points.First().Value;
        }

        #endregion

    }
}
