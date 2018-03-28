using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
using Newtonsoft.Json;
using SnapPeaApp.Views;

namespace SnapPeaApp.ViewModels
{
    /// <summary>
    /// Contains interaction logic for MainWindow view
    /// </summary>
    class MainWindowViewModel : ViewModelBase
    {
        Layout currentLayout;
        Hooks winHook;

        public MainWindowViewModel()
        {
            winHook = new Hooks(WinEventProc);
            Config.Configuration.LoadConfig();
            LoadDefaultLayout();
        }

        /// <summary>
        /// String for layout name text block
        /// </summary>
        public string LayoutName
        {
            get
            {
                return String.IsNullOrEmpty(currentLayout.Name) ? "No layout loaded" : currentLayout.Name;
            }
        }

        #region Commands
        /// <summary>
        /// Bound to Create Layout button
        /// </summary>
        public ICommand CreateLayoutCommand
        {
            get { return new RelayCommand(o => OpenLayoutEditor(new Layout())); }
        }

        /// <summary>
        /// Bound to Edit Layout button
        /// </summary>
        public ICommand EditLayoutCommand
        {
            get { return new RelayCommand(o => OpenLayoutEditor(currentLayout)); }
        }

        /// <summary>
        /// Bound to Load Layout button
        /// </summary>
        public ICommand LoadLayoutCommand
        {
            get { return new RelayCommand(o => BrowseLayout()); }
        }

        /// <summary>
        /// Bound to Settings button
        /// </summary>
        public ICommand SettingsWindowCommand
        {
            get { return new RelayCommand(o => OpenSettingsWindow()); }
        }

        #endregion

        /// <summary>
        /// Callback function called whenever a window drag stops.
        /// Resizes the dragged window if it is released within a region
        /// </summary>
        /// <param name="hWinEventHook">Handle to event hook function</param>
        /// <param name="eventType">Specifies event that occurred (window moved)</param>
        /// <param name="hwnd">Handle to window moved</param>
        /// <param name="sender">Object that triggered event</param>
        /// <param name="idObject">ID of object associated with event</param>
        /// <param name="idChild">Specifies whether event was trigger by object or child of object</param>
        /// <param name="dwEventThread">ID of thread that generated event</param>
        /// <param name="dwmsEventTime">Time in milliseconds that the event was generated</param>
        void WinEventProc(IntPtr hWinEventHook, uint eventType,
            IntPtr hwnd, object sender, int idObject, int idChild, uint dwEventThread, uint dwmsEventTime)
        {
            var windowName = winHook.GetWindowName(sender, hwnd);
            var cursorPos = winHook.GetMousePosition();
            Console.WriteLine($"Window {windowName} Moved, {cursorPos.ToString()}");

            // Check if window released within a region. If so, resize.
            foreach(Region r in currentLayout.Regions)
            {
                if (r.IsPointIn(new Point(cursorPos.X, cursorPos.Y)))
                {
                    winHook.MoveWindow(hwnd, r.Left, r.Top, r.Width, r.Height);
                }
            }
        }

        /// <summary>
        /// Sets currentLayout to the default layout
        /// </summary>
        private void LoadDefaultLayout()
        {
           currentLayout = Layout.LoadLayout(Config.Configuration.getStringSetting(Config.ConfigKeys.DefaultLayout));
        }

        /// <summary>
        /// Opens file browser and loads selected layout
        /// </summary>
        private void BrowseLayout()
        {
            var fileDialog = new OpenFileDialog()
            {
                InitialDirectory = Config.Configuration.getStringSetting(Config.ConfigKeys.LayoutsPath),
                Filter = "json (*.json)|*.json|All files (*.*)|*.*"
            };

            if(fileDialog.ShowDialog() == DialogResult.OK)
            {
                currentLayout = Layout.LoadLayout(fileDialog.FileName);
                OnPropertyChanged("LayoutName");
            }
        }

        /// <summary>
        /// Opens settings window
        /// </summary>
        private void OpenSettingsWindow()
        {
            var window = new SettingsWindow();
            window.ShowDialog();
        }

        /// <summary>
        /// Opens layout editer window
        /// </summary>
        /// <param name="layout"></param>
        private void OpenLayoutEditor(Layout layout)
        {
            var window = new LayoutEditorWindow(layout);
            window.Show();
            window.Activate();
        }

        /// <summary>
        /// Event handler for MainWindow closing.
        /// Unhooks message hooks
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            winHook.Unhook();
        }
    }
}
