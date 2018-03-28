using SnapPeaApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace SnapPeaApp.Views
{
    /// <summary>
    /// Code behind for LayoutEditorWindow.xaml
    /// </summary>
    public partial class LayoutEditorWindow : Window
    {
        public LayoutEditorWindow(Layout l)
        {
            InitializeComponent();

            // Logic for handling dpi conversions and setting window size to fill workarea
            var graphics = System.Drawing.Graphics.FromHwnd(IntPtr.Zero);
            var pixelWidth = System.Windows.Forms.Screen.PrimaryScreen.WorkingArea.Width;
            var pixelHeight = System.Windows.Forms.Screen.PrimaryScreen.WorkingArea.Height;
            var pixelToDPI = 96.0 / graphics.DpiX;
            this.Width = pixelWidth * pixelToDPI;
            this.Height = pixelHeight * pixelToDPI;
            this.WindowState = WindowState.Normal;

            DataContext = new LayoutEditorViewModel(l, DA.GraphicsList);
            this.Closing += (DataContext as LayoutEditorViewModel).LayoutEditorWindow_Closing;
        }

        /// <summary>
        /// Close window when <Esc> key is pressed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Escape)
            {
                this.Close();
            }
        }
    }
}
