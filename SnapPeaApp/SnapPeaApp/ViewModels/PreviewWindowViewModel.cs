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

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        public IList<Region> Regions { get; private set; }
    }
}
