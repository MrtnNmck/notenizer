using edu.stanford.nlp.trees;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using nsConstants;

namespace nsNotenizerObjects
{
	public class NotenizerRelation
	{
        #region Variables

        private String _longName;
        private String _shortName;
        private String _specific;

        #endregion Variables

        #region Constructors

        public NotenizerRelation(GrammaticalRelation grammaticalRelation)
        {
            _longName = grammaticalRelation.getLongName();
            _shortName = grammaticalRelation.getShortName();
            _specific = grammaticalRelation.getSpecific();
        }

        public NotenizerRelation(String shortName)
        {
            _shortName = shortName;
        }

        #endregion Constuctors

        #region Properties

        public String LongName
        {
            get { return _longName; }
        }

        public String ShortName
        {
            get { return _shortName; }
        }

        public String Specific
        {
            get { return _specific; }
        }

        public String AdjustedSpecific
        {
            get
            {
                if (_specific == GrammaticalConstants.AgentRelationSpecific)
                    return GrammaticalConstants.AdjustedAgentRelationSpecific;

                return _specific;
            }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Compares if this relation is grammaticalRelation.
        /// </summary>
        /// <param name="grammaticalRelation">Short name of GramaticalRelations from GrammaticalConstants</param>
        /// <returns></returns>
        public bool IsRelation(String grammaticalRelation)
        {
            return (_shortName == grammaticalRelation);
        }

        public override String ToString()
        {
            return _shortName;
        }

        public bool IsNominalSubject()
        {
            return IsRelation(GrammaticalConstants.NominalSubject) || IsRelation(GrammaticalConstants.NominalSubjectPassive);
        }

        #endregion Methods
    }
}
