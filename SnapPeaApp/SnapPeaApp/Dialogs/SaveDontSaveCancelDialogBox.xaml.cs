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

namespace SnapPeaApp.Dialogs
{
    /// <summary>
    /// Code behind for SaveDontSaveCancelDialogBox.xaml
    /// </summary>
    public partial class SaveDontSaveCancelDialogBox : Window
    {
        public SaveDontSaveCancelDialogBox()
        {
            InitializeComponent();
        }

        private void DontSave_Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
