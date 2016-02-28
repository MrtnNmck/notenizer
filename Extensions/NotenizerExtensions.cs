using nsEnums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nsExtensions
{
    public static class NotenizerExtensions
    {
        public static ComparisonType CreateComperisonType(this TokenType leftSide, TokenType rigthSide)
        {
            return (ComparisonType)Enum.Parse(typeof(ComparisonType), leftSide.ToString() + "To" + rigthSide.ToString());
        }
    }
}
