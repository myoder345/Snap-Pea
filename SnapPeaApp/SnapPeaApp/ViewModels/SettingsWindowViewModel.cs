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
        public SettingsWindowViewModel()
        {
            layoutFolderPath = Config.Configuration.getStringSetting(Config.ConfigKeys.LayoutsPath);
            defaultlLayoutPath = Config.Configuration.getStringSetting(Config.ConfigKeys.DefaultLayout);
        }

        string layoutFolderPath;
        public string LayoutFolderPath
        {
            get
            {
                return layoutFolderPath;
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

        public ICommand BrowseLayoutCommand
        {
            get
            {
                return new RelayCommand(o => browsedefaultlayout());
            }
        }

        public ICommand BrowseFolderPathCommand
        {
            get
            {
                return new RelayCommand(o => browsefolderpath());
            }
        }

        private void browsefolderpath()
        {
            OpenFileDialog filedialog = new OpenFileDialog();
            filedialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);

            var results = filedialog.ShowDialog();
            if (results == DialogResult.OK)
            {
                LayoutFolderPath = filedialog.FileName;
            }
            // update settings in config
        }

        private void browsedefaultlayout()
        {
            OpenFileDialog filedialog = new OpenFileDialog();
            filedialog.InitialDirectory = Config.Configuration.getStringSetting(Config.ConfigKeys.LayoutsPath);

            var results = filedialog.ShowDialog();
            if(results == DialogResult.OK)
            {
                DefaultLayoutPath = filedialog.FileName;
            }
            // update settings in config
        }

    }
}