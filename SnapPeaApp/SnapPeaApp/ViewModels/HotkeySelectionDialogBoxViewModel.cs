using System;
using System.Windows.Input;
using SnapPeaApp.Interfaces;

namespace SnapPeaApp.ViewModels
{
    /// <summary>
    /// Handles interaction logic for HotkeySelectionDialogBox
    /// </summary>
    partial class SettingsWindowViewModel
    {
        /// <summary>
        /// Holds the currently selected hotkey string
        /// </summary>
        public string HotkeyStringHolder { get; set; }

        /// <summary>
        /// Bound to done button
        /// </summary>
        public ICommand HotkeySelectedCommand
        {
            get
            {
                return new RelayCommand(HotkeySelected);
            }
        }

        /// <summary>
        /// Updates hotkey string and closes window
        /// </summary>
        /// <param name="sender"></param>
        private void HotkeySelected(object sender)
        {
            HotkeyString = HotkeyStringHolder;
            (sender as IClosable).Close();
        }
    }
}
