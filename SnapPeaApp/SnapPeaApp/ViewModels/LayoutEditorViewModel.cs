using Newtonsoft.Json;
using SnapPeaApp.Dialogs;
using System.IO;
using System.Windows.Forms;

namespace SnapPeaApp.ViewModels
{
    /// <summary>
    /// Interaction logic for LayoutEditer view
    /// </summary>
    public partial class LayoutEditorViewModel : ViewModelBase
    {
        bool changesMade;

        public LayoutEditorViewModel(Layout layout, DrawTools.GraphicsList graphicsList)
        {
            GraphicsList = graphicsList;
            GraphicsList.GraphicsListChanged = () => changesMade = true;

            // Draw rectangles on screen representing regions in layout
            foreach(var region in layout.Regions)
            {
                GraphicsList.Add(new DrawTools.DrawRectangle(region.Left, region.Top, region.Width, region.Height));
            }
        }

        /// <summary>
        /// Collection of DrawTools.DrawObject objects. Bound to the drawArea user control
        /// </summary>
        public DrawTools.GraphicsList GraphicsList { get; private set; }

        /// <summary>
        /// Converts the rectangles drawn on the draw area to a layout representation and writes it to file
        /// </summary>
        public void SaveLayout()
        {
            // Write to file
            var saveFileDialog = new SaveFileDialog()
            {
                Filter = "json (*.json)|*.json|All files (*.*)|*.*",
                InitialDirectory = Config.Configuration.GetStringSetting(Config.ConfigKeys.LayoutsPath)
            };

            if(saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                SaveLayout(saveFileDialog.FileName, Path.GetFileNameWithoutExtension(saveFileDialog.FileName));
            }
        }

        /// <summary>
        /// Converts the rectangles drawn on the draw area to a layout representation and writes it to file
        /// </summary>
        /// <param name="path"></param>
        public void SaveLayout(string path, string name)
        {
            var tempLayout = new Layout();

            // Add to layout object regions representing rectangles drawn
            foreach (DrawTools.DrawRectangle drawRect in GraphicsList.Enumeration)
            {
                tempLayout.AddRegion(new Region(drawRect.Rectangle));
            }
            
            tempLayout.Name = name;
            string jsonString = JsonConvert.SerializeObject(tempLayout);
            System.IO.File.WriteAllText(path, jsonString);
        }

        /// <summary>
        /// Event handler for LayoutEditor window closing.
        /// Prompts user to save changes before closing if any have been made.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void LayoutEditorWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if(changesMade)
            {
                var saveChangesDialog = new SaveDontSaveCancelDialogBox
                {
                    DataContext = this
                };
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
