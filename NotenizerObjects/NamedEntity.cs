using nsEnums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nsNotenizerObjects
{
    public class NamedEntity
    {
        private String _value;
        private NamedEntityType _neType;

        public NamedEntity(String namedEntity)
        {
            _value = namedEntity == null ? String.Empty : namedEntity;
            _neType = Parse(namedEntity);
        }

        private NamedEntityType Parse(String namedEntity)
        {
            switch (namedEntity)
            {
                case "LOCATION":
                    return NamedEntityType.Location;
                case "ORGANIZATION":
                    return NamedEntityType.Organization;
                case "DATE":
                    return NamedEntityType.Date;
                case "TIME":
                    return NamedEntityType.Time;
                case "MONEY":
                    return NamedEntityType.Money;
                case "PERCENT":
                    return NamedEntityType.Percent;
                case "PERSON":
                    return NamedEntityType.Person;
                case "NUMBER":
                    return NamedEntityType.Number;

                default:
                    return NamedEntityType.Other;
            }
        }

        public String Value
        {
            get { return _value; }
        }

        public NamedEntityType Type
        {
            get { return _neType; }
        }
    }
}
