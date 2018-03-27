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

        public string Name { get; set; }

        public List<Region> Regions { get; set; }

        public void AddRegion(Region r)
        {
            Regions.Add(r);
        }

        public static Layout LoadLayout(string layoutPath)
        {
            try
            {
                return JsonConvert.DeserializeObject<Layout>(File.ReadAllText(layoutPath));
            }
            catch (Exception e)
            {
                //MessageBox.Show($"Could not load default layout\nPath: {Config.Configuration.getStringSetting(Config.ConfigKeys.DefaultLayout)}", "Error");
                System.Windows.MessageBox.Show($"{e.Message}", "Error");
                return new Layout();
            }
        }
    }
}
