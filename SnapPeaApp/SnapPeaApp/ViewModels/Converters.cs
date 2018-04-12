using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace SnapPeaApp.ViewModels
{
    class RegionToScreenConverter : IValueConverter
    {
        static double scalingFactor = System.Drawing.Graphics.FromHwnd(IntPtr.Zero).DpiX/96.0;

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return System.Convert.ToDouble(value) / scalingFactor;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
