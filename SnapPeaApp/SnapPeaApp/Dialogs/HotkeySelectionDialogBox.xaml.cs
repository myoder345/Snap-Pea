using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using SnapPeaApp.Interfaces;

namespace SnapPeaApp.Dialogs
{
    public partial class HotkeySelectionDialog : Window, IClosable
    {
        Hotkey hotkey;

        public HotkeySelectionDialog(Hotkey hotkey)
        {
            InitializeComponent();
            this.hotkey = hotkey;
        }

        /// <summary>
        /// PreviewKeyDown event handler. Formats pressed keys as string and updates hotkey textblock
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if(Keyboard.Modifiers != ModifierKeys.Alt)
            {
                hotkey.Modifiers = (int)Keyboard.Modifiers;
                hotkey.Key = KeyInterop.VirtualKeyFromKey(e.Key);
                KeysTextBox.Text = hotkey.ToString();
            }
            e.Handled = true;
        }
    }
}
