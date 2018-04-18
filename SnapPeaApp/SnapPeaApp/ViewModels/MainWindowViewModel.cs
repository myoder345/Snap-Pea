using System;
using System.Windows.Forms;
using System.Windows.Input;
using SnapPeaApp.Views;

namespace SnapPeaApp.ViewModels
{
    /// <summary>
    /// Contains interaction logic for MainWindow view
    /// </summary>
    class MainWindowViewModel : ViewModelBase, IDisposable
    {
        WindowManager winHook;
        PreviewWindow previewWindow;

        public MainWindowViewModel()
        {
            Config.Configuration.LoadConfig();
            winHook = new WindowManager();
            LoadDefaultLayout();
            previewWindow = new PreviewWindow(winHook.CurrentLayout);
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

        /// <summary>
        /// Bound to window closing event
        /// </summary>
        public ICommand ClosingCommand
        {
            get { return new RelayCommand(o => Dispose()); }
        }
        #endregion

        /// <summary>
        /// Sets winHook.CurrentLayout to the default layout
        /// </summary>
        private void LoadDefaultLayout()
        {
            winHook.CurrentLayout = Layout.LoadLayout(Config.Configuration.GetStringSetting(Config.ConfigKeys.DefaultLayout));
        }

        /// <summary>
        /// Opens file browser and loads selected layout
        /// </summary>
        private void BrowseLayout()
        {
            var fileDialog = new OpenFileDialog()
            {
                InitialDirectory = Config.Configuration.GetStringSetting(Config.ConfigKeys.LayoutsPath),
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

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    winHook.Dispose();
                    previewWindow.Close();
                }

                disposedValue = true;
            }
        }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
        }
        #endregion
    }
}
