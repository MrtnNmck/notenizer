using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nsInterfaces
{
    /// <summary>
    /// Interface for mousable events.
    /// </summary>
    public interface IMousable
    {
        void DoMouseWheel(EventArgs e);
    }
}
