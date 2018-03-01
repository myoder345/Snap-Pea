using System;
using System.Text;
using System.Windows;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace SplitScreenWindows
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Hooks WinHook;
        public MainWindow()
        {
            InitializeComponent();
            WinHook = new Hooks();

            NotifyIcon ni = new NotifyIcon();
            ni.Icon = Properties.Resources.testIcon;
            ni.Visible = true;
            ni.DoubleClick += ShowWindow;
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

        private void Window_Closed(object sender, EventArgs e)
        {
            WinHook.Unhook();
        }
    }
}
