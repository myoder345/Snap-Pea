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
        Hooks winHook;

        public MainWindowViewModel()
        {
            winHook = new Hooks();
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
                return String.IsNullOrEmpty(winHook.CurrentLayout.Name) ? "No layout loaded" : winHook.CurrentLayout.Name;
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
            get { return new RelayCommand(o => OpenLayoutEditor(winHook.CurrentLayout)); }
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
        /// Sets winHook.CurrentLayout to the default layout
        /// </summary>
        private void LoadDefaultLayout()
        {
           winHook.CurrentLayout = Layout.LoadLayout(Config.Configuration.getStringSetting(Config.ConfigKeys.DefaultLayout));
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
                winHook.CurrentLayout = Layout.LoadLayout(fileDialog.FileName);
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
