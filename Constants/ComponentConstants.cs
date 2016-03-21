using nsEnums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace nsConstants
{
    public class ComponentConstants
    {
        public const int ProgressBarEnabledSpeed = 20;
        public const int ProgressBarDisabledSpeed = 0;
        public const float NotenizerAdvancedTextBoxTextBoxRowSize = 50F;
        public const float NotenizerAdvancedTextBoxButtonsRowSize = 20F;
        public const float NotenizerAdvancedTextBoxSize = NotenizerAdvancedTextBoxTextBoxRowSize + NotenizerAdvancedTextBoxButtonsRowSize;
        public const float AdvancedLabelActiveFontSize = 15F;
        public const float AdvancedLabelDeletedFontSize = 10F;
        public const String AdvancedLabelFontFamilyName = "Consolas";
        public const String AdvancedTextBoxFontFamilyName = "Consolas";
        public const String AndSetPositionConstant = "[{AND}]";

        public static readonly Dictionary<NamedEntityType, Color> NamedEntityColors = new Dictionary<NamedEntityType, Color>()
        {
            { NamedEntityType.Location, Color.FromArgb(204, 102, 0) },
            { NamedEntityType.Organization, Color.FromArgb(102, 0, 102) },
            { NamedEntityType.Date, Color.FromArgb(204, 0, 102) },
            { NamedEntityType.Money, Color.FromArgb(153, 0, 0) },
            { NamedEntityType.Person, Color.FromArgb(152, 0, 204) },
            { NamedEntityType.Percent, Color.FromArgb(255, 102, 0) },
            { NamedEntityType.Time, Color.FromArgb(255, 102, 153) },
            { NamedEntityType.Number, Color.FromArgb(102, 255, 204) }
        };
    }
}
