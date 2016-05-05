using nsEnums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nsNotenizerObjects
{
    /// <summary>
    /// Named enitity.
    /// </summary>
    public class NamedEntity
    {
        #region Variables

        private String _value;
        private NamedEntityType _neType;

        #endregion Variables

        #region Constructors

        public NamedEntity(String namedEntity)
        {
            _value = namedEntity == null ? String.Empty : namedEntity;
            _neType = Parse(namedEntity);
        }

        #endregion Constuctors

        #region Properties

        /// <summary>
        /// Text representation of named entity.
        /// </summary>
        public String Value
        {
            get { return _value; }
        }

        /// <summary>
        /// Type of named entity.
        /// </summary>
        public NamedEntityType Type
        {
            get { return _neType; }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Gets type of named entity from string.
        /// </summary>
        /// <param name="namedEntity"></param>
        /// <returns></returns>
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


        #endregion Methods
    }
}
