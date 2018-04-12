﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
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
            layoutFolderPath = Config.Configuration.GetStringSetting(Config.ConfigKeys.LayoutsPath);
            defaultlLayoutPath = Config.Configuration.GetStringSetting(Config.ConfigKeys.DefaultLayout);
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
                IsDirty = true;
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
                IsDirty = true;
            }
        }

        /// <summary>
        /// Represents whether settings have changed and need saving
        /// </summary>
        private bool changesMade;
        public bool IsDirty
        {
            get
            {
                return changesMade;
            }
            set
            {
                SetProperty(ref changesMade, value);
            }
        }

        #endregion

        #region Commands
        /// <summary>
        /// Bound to browseLayout button
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        public ICommand BrowseLayoutCommand
        {
            get
            {
                return new RelayCommand(o => BrowseDefaultLayout());
            }
        }

        /// <summary>
        /// Bound to browseFolderPath button
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        public ICommand BrowseFolderPathCommand
        {
            get
            {
                return new RelayCommand(o => BrowseFolderPath());
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        public ICommand SaveSettingsCommand
        {
            get
            {
                return new RelayCommand(o => SaveSettings());
            }
        }
        #endregion

        #region Methods
        /// <summary>
        /// Opens folder browser and sets LayoutFolderPath property
        /// </summary>
        private void BrowseFolderPath()
        {
            var filedialog = new FolderBrowserDialog
            {
                RootFolder = Environment.SpecialFolder.UserProfile
            };

            var results = filedialog.ShowDialog();
            if (results == DialogResult.OK)
            {
                LayoutFolderPath = filedialog.SelectedPath;
            }
        }

        /// <summary>
        /// Opens file browser and sets DefaultLayoutPath property
        /// </summary>
        private void BrowseDefaultLayout()
        {
            var filedialog = new OpenFileDialog
            {
                InitialDirectory = Config.Configuration.GetStringSetting(Config.ConfigKeys.LayoutsPath)
            };

            var results = filedialog.ShowDialog();
            if(results == DialogResult.OK)
            {
                DefaultLayoutPath = filedialog.FileName;
            }
        }

        /// <summary>
        /// Updates settings dictionary with new values
        /// </summary>
        private void SaveSettings()
        {
            Config.Configuration.SetStringSetting(Config.ConfigKeys.DefaultLayout, DefaultLayoutPath);
            Config.Configuration.SetStringSetting(Config.ConfigKeys.LayoutsPath, LayoutFolderPath);
            IsDirty = false;
            Config.Configuration.SaveConfig();
        }

        /// <summary>
        /// Event handler for window closing
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        internal void SettingsWindow_Closing(object sender, CancelEventArgs e)
        {
            if(IsDirty)
            {
                if(MessageBox.Show("Would you like to save changes?", "Closing", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    SaveSettings();
                }
            }
        }
        #endregion
    }
}