using System;
using System.Collections.Generic;
using System.Text;

namespace SnapPeaApp
{
    class Layout
    {
        public List<Region> Regions { get; set; }

        public void AddRegion(Region r)
        {
            Regions.Add(r);
        }
    }
}
