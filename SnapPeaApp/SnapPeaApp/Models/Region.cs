using System;
using System.Collections.Generic;
using System.Windows;
using System.Text;
using System.Drawing;

namespace SnapPeaApp
{
    public class Region
    {
        public Region(Rectangle r)
        {
            Left = r.Left;
            Top = r.Top;
            Width = r.Width;
            Height = r.Height;
        }

        public int Left { get; set; }
        public int Top { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public int Color { get; set; }

        /// <summary>
        /// Tests whether point is bounded by the region
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public bool IsPointIn(System.Windows.Point p)
        {
            return p.X > Left && p.X < (Left + Width) && p.Y > Top && p.Y < (Top + Height);
        }
    }
}
