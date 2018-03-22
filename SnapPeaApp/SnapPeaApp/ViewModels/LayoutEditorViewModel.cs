using SnapPeaApp.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnapPeaApp.ViewModels
{
    partial class LayoutEditorViewModel : ViewModelBase
    {
        bool changesMade;

        public LayoutEditorViewModel()
        {
            changesMade = true;
        }

        #region Commands

        #endregion

        void SaveLayout()
        {
            throw new NotImplementedException();
        }

        public void LayoutEditorWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if(changesMade)
            {
                var saveChangesDialog = new SaveDontSaveCancelDialogBox();
                saveChangesDialog.DataContext = this;
                saveChangesDialog.ShowDialog();
                if(cancelClosing)
                {
                    cancelClosing = false;
                    e.Cancel = true;
                }
                
            }
        }
    }
}
