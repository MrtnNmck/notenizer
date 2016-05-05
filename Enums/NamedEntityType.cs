using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nsEnums
{
    /// <summary>
    /// Types of named entities.
    /// </summary>
    public enum NamedEntityType
    {
        Location,
        Person,
        Organization,
        Money,
        Percent,
        Date,
        Time,
        Number,
        Other
    }
}
