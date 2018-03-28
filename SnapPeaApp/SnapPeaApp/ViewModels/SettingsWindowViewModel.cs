using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;

namespace SnapPeaApp.ViewModels
{
    /// <summary>
    /// Contains interaction logic for SettingsWindow view
    /// </summary>
    class SettingsWindowViewModel : ViewModelBase
    {
        public SettingsWindowViewModel()
        {
            layoutFolderPath = Config.Configuration.getStringSetting(Config.ConfigKeys.LayoutsPath);
            defaultlLayoutPath = Config.Configuration.getStringSetting(Config.ConfigKeys.DefaultLayout);
        }

        #region Properties
        /// <summary>
        /// Data binding for layoutFolderPath textbox
        /// </summary>
        string layoutFolderPath;
        public string LayoutFolderPath
        {
            get
            {
                return layoutFolderPath;
            }

            set
            {
                SetProperty(ref layoutFolderPath, value);
            }
        }

        /// <summary>
        /// Databinding for defaultLayoutPath textbox
        /// </summary>
        string defaultlLayoutPath;
        public string DefaultLayoutPath
        {
            get
            {
                return defaultlLayoutPath;
            }

            set
            {
                SetProperty(ref defaultlLayoutPath, value);
            }
        }
        #endregion

        #region Commands
        /// <summary>
        /// Bound to browseLayout button
        /// </summary>
        public ICommand BrowseLayoutCommand
        {
            get
            {
                return new RelayCommand(o => browsedefaultlayout());
            }
        }

        /// <summary>
        /// Bound to browseFolderPath button
        /// </summary>
        public ICommand BrowseFolderPathCommand
        {
            get
            {
                return new RelayCommand(o => browsefolderpath());
            }
        }
        #endregion

        #region Methods
        /// <summary>
        /// Opens file browser
        /// </summary>
        private void browsefolderpath()
        {
            OpenFileDialog filedialog = new OpenFileDialog();
            filedialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);

            var results = filedialog.ShowDialog();
            if (results == DialogResult.OK)
            {
                LayoutFolderPath = filedialog.FileName;
            }
            
            // TODO: update settings in config
        }

        private void browsedefaultlayout()
        {
            OpenFileDialog filedialog = new OpenFileDialog();
            filedialog.InitialDirectory = Config.Configuration.getStringSetting(Config.ConfigKeys.LayoutsPath);

            var results = filedialog.ShowDialog();
            if(results == DialogResult.OK)
            {
                DefaultLayoutPath = filedialog.FileName;
            }
            
            // TODO: update settings in config
        }
        #endregion
    }
}