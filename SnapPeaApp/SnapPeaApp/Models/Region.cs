using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace SnapPeaApp
{
    class Region
    {
        public float Left { get; set; }
        public float Top { get; set; }
        public float Width { get; set; }
        public float Height { get; set; }
        public int Color { get; set; }

        public bool IsPointIn(Point p)
        {
            return p.X > Left && p.X < (Left + Width) && p.Y > Top && p.Y < (Top + Height);
        }
    }
}
