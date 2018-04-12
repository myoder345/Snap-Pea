using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
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

        /// <summary>
        /// Name of layout
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Collection of Region objects
        /// </summary>
        public IList<Region> Regions { get; }

        public void AddRegion(Region region)
        {
            Regions.Add(region);
        }

        /// <summary>
        /// Reads a layout json file and deserializes it into a layout object
        /// </summary>
        /// <param name="layoutPath"></param>
        /// <returns></returns>
        public static Layout LoadLayout(string layoutPath)
        {
            try
            {
                return JsonConvert.DeserializeObject<Layout>(File.ReadAllText(layoutPath));
            }
            catch (IOException e)
            {
                System.Windows.MessageBox.Show($"Error loading layout: {e.Message}", "Error");
                return new Layout();
            }
        }
    }
}
