using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;

namespace SnapPeaApp.ViewModels
{
    class SettingsWindowViewModel : ViewModelBase
    {
        String layoutfolderpath;
        public string LayoutFolderPath
        {
            get
            {
                return defaultlayoutpath;
            }

            set
            {
                SetProperty(ref layoutfolderpath, value);
            }
        }

        public ICommand browsefolderpathcommand
        {
            get
            {
                return new RelayCommand(o => browsefolderpath());
            }
        }

        private void browsefolderpath()
        {
            OpenFileDialog filedialog = new OpenFileDialog();
            filedialog.InitialDirectory = "c:\\";

            var results = filedialog.ShowDialog();
            if (results == DialogResult.OK)
            {
                LayoutFolderPath = filedialog.FileName;
            }
        }

        String defaultlayoutpath;
        public string DefaultLayoutPath
        {
            get
            {
                return defaultlayoutpath;
            }

            set
            {
                SetProperty(ref defaultlayoutpath, value);
            }
        }

        public ICommand browselayoutcommand
        {
            get
            {
                return new RelayCommand(o => browsefolderpathtoo());
            }
        }

        private void browsefolderpathtoo()
        {
            OpenFileDialog filedialog = new OpenFileDialog();
            filedialog.InitialDirectory = "c:\\";

            var results = filedialog.ShowDialog();
            if(results == DialogResult.OK)
            {
                DefaultLayoutPath = filedialog.FileName;
            }
        }

    }
}