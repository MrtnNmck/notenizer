using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nsNotenizerObjects
{
    public class CompressedDependencies : Dictionary<String, List<NotenizerDependency>>
    {
        public CompressedDependencies()
        {
        }

        public void Add(NotenizerDependency dependency)
        {
            if (!this.ContainsKey(dependency.Relation.ShortName))
                this.Add(dependency.Relation.ShortName, new List<NotenizerDependency>() { dependency });
            else
                this[dependency.Relation.ShortName].Add(dependency);
        }
    }
}
