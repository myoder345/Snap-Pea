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
            NotifyIcon ni = new NotifyIcon
            {
                Icon = Properties.Resources.testIcon,
                Visible = true
            };
            ni.DoubleClick += ShowWindow;
            ni.MouseDown += Ni_MouseDown;
            this.Closing += (DataContext as ViewModels.MainWindowViewModel).MainWindow_Closing;
        }

        private void Ni_MouseDown(object sender, MouseEventArgs e)
        {
            if(e.Button == MouseButtons.Right)
            {
                System.Windows.Controls.ContextMenu menu = (System.Windows.Controls.ContextMenu)this.FindResource("NotifierContextMenu");
                menu.IsOpen = true;
            }
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

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
