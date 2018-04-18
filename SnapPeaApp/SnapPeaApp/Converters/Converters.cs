using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

/// <summary>
/// Contains various converters for XAML
/// </summary>
namespace SnapPeaApp.Converters
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

    public class BoolToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (bool)value ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            switch ((Visibility)value)
            {
                case Visibility.Visible:
                    return true;
                case Visibility.Hidden:
                case Visibility.Collapsed:
                default:
                    return false;
            }
        }
    }
}
