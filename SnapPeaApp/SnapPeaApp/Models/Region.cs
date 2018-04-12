using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Forms;
using System.Text;
using System.Drawing;
using Newtonsoft.Json;

namespace SnapPeaApp
{
    public class Region
    {
        public Region(Rectangle rect)
        {
            LeftF = (float)rect.Left / Screen.PrimaryScreen.WorkingArea.Width;
            TopF = (float)rect.Top / Screen.PrimaryScreen.WorkingArea.Height;
            WidthF = (float)rect.Width / Screen.PrimaryScreen.WorkingArea.Width;
            HeightF = (float)rect.Height / Screen.PrimaryScreen.WorkingArea.Height;
        }

        [JsonProperty]
        private float LeftF { get; set; }
        [JsonProperty]
        private float TopF { get; set; }
        [JsonProperty]
        private float WidthF { get; set; }
        [JsonProperty]
        private float HeightF { get; set; }

        [JsonIgnore]
        public int Left
        {
            get { return (int)(LeftF * Screen.PrimaryScreen.WorkingArea.Width); }
            set { LeftF = value / Screen.PrimaryScreen.WorkingArea.Width; }
        }

        [JsonIgnore]
        public int Top
        {
            get { return (int)(TopF * Screen.PrimaryScreen.WorkingArea.Height); }
            set { TopF = value / Screen.PrimaryScreen.WorkingArea.Height; }
        }

        [JsonIgnore]
        public int Width
        {
            get { return (int)(WidthF * Screen.PrimaryScreen.WorkingArea.Width); }
            set { WidthF = value / Screen.PrimaryScreen.WorkingArea.Width; }
        }

        [JsonIgnore]
        public int Height
        {
            get { return (int)(HeightF * Screen.PrimaryScreen.WorkingArea.Height); }
            set { HeightF = value / Screen.PrimaryScreen.WorkingArea.Height; }
        }

        public int Color { get; set; }

        /// <summary>
        /// Tests whether point is within the region
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public bool IsPointIn(System.Windows.Point point)
        {
            Console.Out.WriteLine(Left + "," + Width + "\n" + Top + "\n" + Height);
            return point.X > Left && point.X < (Left + Width) && point.Y > Top && point.Y < (Top + Height);
        }
    }
}
