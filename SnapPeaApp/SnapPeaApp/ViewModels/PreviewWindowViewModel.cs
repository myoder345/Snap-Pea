using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnapPeaApp.ViewModels
{
    class PreviewWindowViewModel : ViewModelBase
    {
        public PreviewWindowViewModel(Layout layout)
        {
            Regions = layout.Regions;
        }

        public IList<Region> Regions { get; private set; }
    }
}
