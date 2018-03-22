using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SnapPeaApp.ViewModels
{
    partial class LayoutEditorViewModel
    {
        bool cancelClosing = false;

        public ICommand CancelCommand
        {
            get
            {
                return new RelayCommand(Cancel);
            }
        }
        void Cancel(object o)
        {
            cancelClosing = true;
            (o as System.Windows.Window).Close();
        }
        
        public ICommand SaveCommand
        {
            get
            {
                return new RelayCommand(Save);
            }
        }
        void Save(object o)
        {
            SaveLayout();
            (o as System.Windows.Window).Close();
        }
    }
}
