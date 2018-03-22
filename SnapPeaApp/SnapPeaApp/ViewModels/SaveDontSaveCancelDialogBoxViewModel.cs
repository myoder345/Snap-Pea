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

        #region Commands
        public ICommand CancelCommand
        {
            get
            {
                return new RelayCommand(Cancel);
            }
        }

        public ICommand SaveCommand
        {
            get
            {
                return new RelayCommand(Save);
            }
        }
        #endregion

        #region Methods
        void Cancel(object o)
        {
            cancelClosing = true;
            (o as System.Windows.Window).Close();
        }
        
        void Save(object o)
        {
            SaveLayout();
            (o as System.Windows.Window).Close();
        }
        #endregion
    }
}
