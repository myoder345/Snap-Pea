using SnapPeaApp.Dialogs;
using System;
using System.ComponentModel;
using System.Windows.Forms;
using System.Windows.Input;
using SnapPeaApp.Config;

namespace SnapPeaApp.ViewModels
{
    /// <summary>
    /// Contains interaction logic for SettingsWindow view
    /// </summary>
    partial class SettingsWindowViewModel : ViewModelBase
    {
        Hotkey hotkey;

        public SettingsWindowViewModel()
        {
            layoutFolderPath = Configuration.GetStringSetting(ConfigKeys.LayoutsPath);
            defaultlLayoutPath = Configuration.GetStringSetting(ConfigKeys.DefaultLayout);

            hotkey = new Hotkey(Configuration.GetIntSetting(ConfigKeys.PreviewKeyModifiers), Configuration.GetIntSetting(ConfigKeys.PreviewKey));
            hotkeyString = hotkey.ToString();
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

        /// <summary>
        /// String representing the current hotkey setting
        /// </summary>
        string hotkeyString;
        public string HotkeyString
        {
            get
            {
                return hotkeyString;
            }
            set
            {
                SetProperty(ref hotkeyString, value);
                IsDirty = true;
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
                return new RelayCommand(o => BrowseDefaultLayout());
            }
        }

        /// <summary>
        /// Bound to browseFolderPath button
        /// </summary>
        public ICommand BrowseFolderPathCommand
        {
            get
            {
                return new RelayCommand(o => BrowseFolderPath());
            }
        }

        /// <summary>
        /// Bound to Save settings button
        /// </summary>
        public ICommand SaveSettingsCommand
        {
            get
            {
                return new RelayCommand(o => SaveSettings());
            }
        }

        /// <summary>
        /// Bounds to change hotkey button
        /// </summary>
        public ICommand OpenHotkeySelectorCommand
        {
            get
            {
                return new RelayCommand(o => OpenHotkeySelector());
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
                InitialDirectory = Configuration.GetStringSetting(ConfigKeys.LayoutsPath)
            };

            var results = filedialog.ShowDialog();
            if(results == DialogResult.OK)
            {
                DefaultLayoutPath = filedialog.FileName;
            }
        }

        /// <summary>
        /// Opens hotkey selector dialog
        /// </summary>
        private void OpenHotkeySelector()
        {
            var hotkeyDialog = new HotkeySelectionDialog(hotkey)
            {
                DataContext = this
            };
            hotkeyDialog.ShowDialog();
        }

        /// <summary>
        /// Updates settings dictionary with new values
        /// </summary>
        private void SaveSettings()
        {
            Configuration.SetStringSetting(ConfigKeys.DefaultLayout, DefaultLayoutPath);
            Configuration.SetStringSetting(ConfigKeys.LayoutsPath, LayoutFolderPath);
            Configuration.SetIntSetting(ConfigKeys.PreviewKey, hotkey.Key);
            Configuration.SetIntSetting(ConfigKeys.PreviewKeyModifiers, hotkey.Modifiers);

            IsDirty = false;
            Configuration.SaveConfig();
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