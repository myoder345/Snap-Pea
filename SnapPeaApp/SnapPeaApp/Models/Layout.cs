using System;
using System.Collections.Generic;
using System.Text;

namespace SnapPeaApp
{
    public class Layout
    {
        public Layout()
        {
            Regions = new List<Region>();
            Name = "";
        }

        public string Name { get; set; }

        public List<Region> Regions { get; set; }

        public void AddRegion(Region r)
        {
            Regions.Add(r);
        }
    }
}
