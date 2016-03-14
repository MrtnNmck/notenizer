using nsEnums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nsNotenizerObjects
{
    public class NotenizerAndParserRule : NotenizerRule
    {
        private int _setsPosition;

        public NotenizerAndParserRule(String id, List<NotenizerDependency> dependencies, CreatedBy createdBy, int setsPosition)
            : base(id, dependencies, createdBy)
        {
            _setsPosition = setsPosition;
        }

        public int SetsPosition
        {
            get { return _setsPosition; }
        }
    }
}
