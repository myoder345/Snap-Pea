using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SnapPeaApp
{
    public class Layout
    {
        public static event EventHandler<LayoutEventArgs> LayoutChanged;

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
        /// <returns>the loaded layout</returns>
        public static Layout LoadLayout(string layoutPath)
        {
            Layout newLayout;
            try
            {
                newLayout = JsonConvert.DeserializeObject<Layout>(File.ReadAllText(layoutPath));
            }
            catch (IOException e)
            {
                System.Windows.MessageBox.Show($"Error loading layout: {e.Message}", "Error");
                newLayout = new Layout();
            }

            // raise layout changed event
            LayoutChanged?.Invoke(null, new LayoutEventArgs(newLayout));

            return newLayout;
        }
    }

    public class LayoutEventArgs : EventArgs
    {
        public LayoutEventArgs(Layout layout)
        {
            Layout = layout;
        }

        public Layout Layout { get; set; }
    }
}
