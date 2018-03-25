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
        Layout currentLayout;

        public LayoutEditorViewModel(Layout layout)
        {
            currentLayout = layout;
        }

        DrawTools.GraphicsList graphicsList;
        public DrawTools.GraphicsList GraphicsList
        {
            get { return graphicsList; }
            set { graphicsList = value; graphicsList.GraphicsListChanged = () => changesMade = true; }
        }

        void SaveLayout()
        {
            
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
