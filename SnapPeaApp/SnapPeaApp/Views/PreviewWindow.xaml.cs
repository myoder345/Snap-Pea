using SnapPeaApp.ViewModels;
using System.Windows;

namespace SnapPeaApp.Views
{
    /// <summary>
    /// Interaction logic for PreviewWindow.xaml
    /// </summary>
    public partial class PreviewWindow : Window
    {
        public PreviewWindow(Layout layout)
        {
            InitializeComponent();
            this.Show();
            this.Hide();
            DataContext = new PreviewWindowViewModel(layout);
        }
    }
}
