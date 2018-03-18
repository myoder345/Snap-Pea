using System;
using System.Windows;
using System.Windows.Forms;

namespace SnapPeaApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new ViewModels.MainWindowViewModel();
            NotifyIcon ni = new NotifyIcon();
            ni.Icon = Properties.Resources.testIcon;
            ni.Visible = true;
            ni.DoubleClick += ShowWindow;
            this.Closing += (DataContext as ViewModels.MainWindowViewModel).MainWindow_Closing;
        }

        void ShowWindow(object sender, EventArgs args)
        {
            this.Show();
            this.WindowState = System.Windows.WindowState.Normal;
        }

        protected override void OnStateChanged(EventArgs e)
        {
            if (WindowState == System.Windows.WindowState.Minimized)
                this.Hide();
            base.OnStateChanged(e);
        }
    }
}
