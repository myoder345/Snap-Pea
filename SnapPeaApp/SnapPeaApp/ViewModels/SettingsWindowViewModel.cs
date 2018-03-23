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
        string layoutFolderPath;
        public string LayoutFolderPath
        {
            get
            {
                return defaultlLayoutPath;
            }

            set
            {
                SetProperty(ref layoutFolderPath, value);
            }
        }

        string defaultlLayoutPath;
        public string DefaultLayoutPath
        {
            get
            {
                return defaultlLayoutPath;
            }

            set
            {
                SetProperty(ref defaultlLayoutPath, value);
            }
        }

        public ICommand browselayoutcommand
        {
            get
            {
                return new RelayCommand(o => browsefolderpathtoo());
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