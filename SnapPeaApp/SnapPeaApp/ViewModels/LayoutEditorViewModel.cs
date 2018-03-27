using Newtonsoft.Json;
using SnapPeaApp.Dialogs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SnapPeaApp.ViewModels
{
    partial class LayoutEditorViewModel : ViewModelBase
    {
        bool changesMade;

        public LayoutEditorViewModel(Layout layout, DrawTools.GraphicsList graphicsList)
        {
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
            var tempLayout = new Layout();

            foreach(DrawTools.DrawRectangle drawRect in GraphicsList.Enumeration)
            {
                tempLayout.AddRegion(new Region(drawRect.GetRectangle()));
            }

            // overwrite layout file or svae to new layout file?
            var saveFileDialog = new SaveFileDialog()
            {
                Filter = "json (*.json)|*.json|All files (*.*)|*.*",
                InitialDirectory = Config.Configuration.getStringSetting(Config.ConfigKeys.LayoutsPath)
            };

            if(saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                tempLayout.Name = Path.GetFileNameWithoutExtension(saveFileDialog.FileName);
                string jsonString = JsonConvert.SerializeObject(tempLayout);
                System.IO.File.WriteAllText(saveFileDialog.FileName, jsonString);
            }
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
