using nsEnums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nsInterfaces
{
    /// <summary>
    /// Interface for objects, which are persistable into database.
    /// </summary>
    public interface IPersistable
    {
        String ID { get; }

        DateTime CreatedAt { get; set; }

        DateTime UpdatedAt { get; set; }
    }
}
