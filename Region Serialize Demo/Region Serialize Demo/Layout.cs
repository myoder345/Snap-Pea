using System;
using System.Collections.Generic;
using System.Text;

namespace Region_Serialize_Demo
{
    class Layout
    {
        public List<Region> regions { get; set; }

        public void AddRegion(Region r)
        {
            regions.Add(r);
        }
    }
}
