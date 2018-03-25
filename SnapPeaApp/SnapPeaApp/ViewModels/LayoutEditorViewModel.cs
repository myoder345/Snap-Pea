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

        public LayoutEditorViewModel(Layout layout, DrawTools.GraphicsList graphicsList)
        {
            currentLayout = layout;
            GraphicsList = graphicsList;
            GraphicsList.GraphicsListChanged = () => changesMade = true;

            foreach(var region in layout.Regions)
            {
                GraphicsList.Add(new DrawTools.DrawRectangle(region.Left, region.Top, region.Width, region.Height));
            }
        }

        public DrawTools.GraphicsList GraphicsList { get; private set; }

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
