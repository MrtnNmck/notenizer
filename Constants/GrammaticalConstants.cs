using edu.stanford.nlp.trees;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using edu.stanford.nlp.ling;

namespace nsConstants
{
    /// <summary>
    /// Class containing short names of EnglishGrammaticalRelations.
    /// </summary>
    public class GrammaticalConstants
    {
        public const String NominalSubject = "nsubj";
        public const String NominalSubjectPassive = "nsubjpass";
        public const String CompoudModifier = "compound";
        public const String AuxModifier = "aux";
        public const String AuxModifierPassive = "auxpass";
        public const String Copula = "cop";
        public const String Conjuction = "conj";
        public const String NegationModifier = "neg";
        public const String DirectObject = "dobj";
        public const String NominalModifier = "nmod";
        public const String NumericModifier = "nummod";
        public const String AdjectivalModifier = "amod";
        public const String AgentRelationSpecific = "agent";
        public const String AdjustedAgentRelationSpecific = "by";
    }
}
