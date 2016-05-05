using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nsComparsions
{
    /// <summary>
    /// List of generic comparsions.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Comparsions<T> : List<Comparsion<T>>
    {
        public Comparsions()
        {
        }
    }
}
