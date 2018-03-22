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
    /// Interaction logic for LayoutEditorWindow.xaml
    /// </summary>
    public partial class LayoutEditorWindow : Window
    {
        public LayoutEditorWindow()
        {
            InitializeComponent();
            DataContext = new LayoutEditorViewModel();
            this.Height = System.Windows.Forms.Screen.PrimaryScreen.WorkingArea.Height;
            this.Width = System.Windows.Forms.Screen.PrimaryScreen.WorkingArea.Width;
            this.Closing += (DataContext as LayoutEditorViewModel).LayoutEditorWindow_Closing;
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Escape)
            {
                this.Close();
            }
        }
    }
}
