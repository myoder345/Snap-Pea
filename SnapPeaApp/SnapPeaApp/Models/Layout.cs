using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace SnapPeaApp
{
    public class Layout
    {
        /// <summary>
        /// Event invoked whenever a new layout is loaded
        /// </summary>
        public static event EventHandler<LayoutEventArgs> LayoutLoaded;

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
        /// <remarks>raises LayoutLoaded event</remarks>
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

            // invoke event
            LayoutLoaded?.Invoke(null, new LayoutEventArgs(newLayout));

            return newLayout;
        }
    }

    /// <summary>
    /// EventArgs class for Layout.LayoutChanged event
    /// </summary>
    public class LayoutEventArgs : EventArgs
    {
        public LayoutEventArgs(Layout layout)
        {
            Layout = layout;
        }

        public Layout Layout { get; set; }
    }
}
