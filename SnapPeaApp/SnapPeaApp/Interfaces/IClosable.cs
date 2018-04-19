using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnapPeaApp.Interfaces
{
    /// <summary>
    /// Asserts that implementing object can be closed
    /// </summary>
    interface IClosable
    {
        void Close();
    }
}
