using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace SnapPeaApp
{
    class Region
    {
        public int Left { get; set; }
        public int Top { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public int Color { get; set; }

        public bool IsPointIn(Point p)
        {
            return p.X > Left && p.X < (Left + Width) && p.Y > Top && p.Y < (Top + Height);
        }
    }
}
