using System;
using System.Windows.Forms;
using System.Drawing;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.Serialization;

namespace DrawTools
{
	/// <summary>
	/// Base class for all draw objects
	/// </summary>
	public abstract class DrawObject
	{
        protected DrawObject()
        {
            Id = this.GetHashCode();
        }

        #region Properties

        /// <summary>
        /// Selection flag
        /// </summary>
        public bool Selected { get; set; }

        /// <summary>
        /// Color
        /// </summary>
        public Color Color { get; set; }

        /// <summary>
        /// Pen width
        /// </summary>
        public int PenWidth { get; set; }

        /// <summary>
        /// Number of handles
        /// </summary>
        public virtual int HandleCount
        {
            get
            {
                return 0;
            }
        }

        /// <summary>
        /// Object ID
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Last used color
        /// </summary>
        public static Color LastUsedColor { get; set; } = Color.Black;

        /// <summary>
        /// Last used pen width
        /// </summary>
        public static int LastUsedPenWidth { get; set; } = 1;

        #endregion

        #region Virtual Functions

        /// <summary>
        /// Clone this instance.
        /// </summary>
        public abstract DrawObject Clone();

        /// <summary>
        /// Draw object
        /// </summary>
        /// <param name="graphics"></param>
        public virtual void Draw(Graphics graphics)
        {
        }

        /// <summary>
        /// Get handle point by 1-based number
        /// </summary>
        /// <param name="handleNumber"></param>
        /// <returns></returns>
        public virtual Point GetHandle(int handleNumber)
        {
            return new Point(0, 0);
        }

        /// <summary>
        /// Get handle rectangle by 1-based number
        /// </summary>
        /// <param name="handleNumber"></param>
        /// <returns></returns>
        public virtual Rectangle GetHandleRectangle(int handleNumber)
        {
            Point point = GetHandle(handleNumber);

            return new Rectangle(point.X - 3, point.Y - 3, 7, 7);
        }

        /// <summary>
        /// Draw tracker for selected object
        /// </summary>
        /// <param name="graphics"></param>
        public virtual void DrawTracker(Graphics graphics)
        {
            if ( ! Selected )
                return;

            SolidBrush brush = new SolidBrush(Color.Black);

            for ( int i = 1; i <= HandleCount; i++ )
            {
                graphics?.FillRectangle(brush, GetHandleRectangle(i));
            }

            brush.Dispose();
        }

        /// <summary>
        /// Hit test.
        /// Return value: -1 - no hit
        ///                0 - hit anywhere
        ///                > 1 - handle number
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public virtual int HitTest(Point point)
        {
            return -1;
        }


        /// <summary>
        /// Test whether point is inside of the object
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        protected virtual bool PointInObject(Point point)
        {
            return false;
        }
        

        /// <summary>
        /// Get cursor for the handle
        /// </summary>
        /// <param name="handleNumber"></param>
        /// <returns></returns>
        public virtual Cursor GetHandleCursor(int handleNumber)
        {
            return Cursors.Default;
        }

        /// <summary>
        /// Test whether object intersects with rectangle
        /// </summary>
        /// <param name="rectangle"></param>
        /// <returns></returns>
        public virtual bool IntersectsWith(Rectangle rectangle)
        {
            return false;
        }

        public virtual bool IntersectsWith(DrawRectangle rectangle)
        {
            return false;
        }

        /// <summary>
        /// Move object
        /// </summary>
        /// <param name="deltaX"></param>
        /// <param name="deltaY"></param>
        public virtual void Move(int deltaX, int deltaY)
        {
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "y")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "x")]
        public virtual void MoveTo(int x, int y)
        {
        }

        /// <summary>
        /// Move handle to the point
        /// </summary>
        /// <param name="point"></param>
        /// <param name="handleNumber"></param>
        public virtual void MoveHandleTo(Point point, int handleNumber)
        {
        }

        /// <summary>
        /// Dump (for debugging)
        /// </summary>
        public virtual void Dump()
        {
            Trace.WriteLine(this.GetType().Name);
            Trace.WriteLine("Selected = " + 
                Selected.ToString(CultureInfo.InvariantCulture)
                + " ID = " + Id.ToString(CultureInfo.InvariantCulture));
        }

        /// <summary>
        /// Normalize object.
        /// Call this function in the end of object resizing.
        /// </summary>
        public virtual void Normalize()
        {
        }
        #endregion

        #region Other functions

        /// <summary>
        /// Initialization
        /// </summary>
        protected void Initialize()
        {
            Color = LastUsedColor;
            PenWidth = LastUsedPenWidth;
        }

        /// <summary>
        /// Copy fields from this instance to cloned instance drawObject.
        /// Called from Clone functions of derived classes.
        /// </summary>
        protected void FillDrawObjectFields(DrawObject drawObject)
        {
            if(drawObject != null)
            {
                drawObject.Selected = this.Selected;
                drawObject.Color = this.Color;
                drawObject.PenWidth = this.PenWidth;
                drawObject.Id = this.Id;
            }
        }

        #endregion
    }
}
