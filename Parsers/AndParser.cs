using nsConstants;
using nsEnums;
using nsNotenizerObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nsParsers
{
    /// <summary>
    /// And-Parser.
    /// </summary>
    public class AndParser : NotenizerParser
    {
        #region Variables

        #endregion Variables

        #region Constructors

        public AndParser()
        {
        }

        #endregion Constuctors

        #region Properties

        #endregion Properties

        #region Methods

        /// <summary>
        /// Checks if sentence is parsable by And-Parser
        /// </summary>
        /// <param name="sentence">Sentence to check</param>
        /// <returns></returns>
        public override Boolean IsParsableSentence(NotenizerSentence sentence)
        {
            return (sentence.Structure.CompressedDependencies.ContainsKey(GrammaticalConstants.Conjuction)
                && sentence.Structure.CompressedDependencies[GrammaticalConstants.Conjuction] != null
                && sentence.Structure.CompressedDependencies[GrammaticalConstants.Conjuction].Exists(x => x.Relation.Specific == GrammaticalConstants.AndConjuction || x.Relation.ShortName == GrammaticalConstants.AppositionalModifier));
        }

        /// <summary>
        /// Extracts set of cunjuction AND.
        /// </summary>
        /// <param name="sentence">Sentence to extract set from</param>
        /// <returns></returns>
        public List<NoteParticle> GetAndSets(NotenizerSentence sentence)
        {
            return GetAndSets(sentence, true);
        }

        /// <summary>
        /// Extracts set of cunjuction AND.
        /// Checks for parseability.
        /// </summary>
        /// <param name="sentence">Sentence to extract set from</param>
        /// <param name="checkParsability"></param>
        /// <returns></returns>
        public List<NoteParticle> GetAndSets(NotenizerSentence sentence, bool checkParsability)
        {
            if (checkParsability && !IsParsableSentence(sentence))
                return null;

            List<NoteParticle> andSets;
            List<NotenizerDependency> repetitionPartDependencies;
            List<NotenizerDependency> andConjuctionsApposDependencies;

            andSets = new List<NoteParticle>();
            repetitionPartDependencies = new List<NotenizerDependency>();
            andConjuctionsApposDependencies = sentence.Structure.Dependencies.Where(x => x.Relation.Specific == GrammaticalConstants.AndConjuction
                                                                            || x.Relation.ShortName == GrammaticalConstants.AppositionalModifier).ToList();
            repetitionPartDependencies = repetitionPartDependencies.Concat(andConjuctionsApposDependencies).ToList();

            foreach (NotenizerDependency andApposDependencyLoop in andConjuctionsApposDependencies)
            {
                List<NotenizerDependency> apposSecondLevelDependencies = sentence.Structure.GetDependenciesByShortName(andApposDependencyLoop, ComparisonType.DependentToGovernor, GrammaticalConstants.AppositionalModifier);
                foreach (NotenizerDependency apposSecondLevelDependencyLoop in apposSecondLevelDependencies)
                {
                    // add only disctinct dependencies
                    if (repetitionPartDependencies.Any(x => x.Key == apposSecondLevelDependencyLoop.Key))
                        continue;

                    repetitionPartDependencies.Add(apposSecondLevelDependencyLoop);
                }
            }

            foreach (NotenizerDependency repetitionPartDependencyLoop in repetitionPartDependencies)
            {
                andSets.Add(CreateAndSet(sentence, repetitionPartDependencyLoop));
            }

            // also add first dependencies' governor from and set
            andSets.Add(CreateAndSet(sentence, repetitionPartDependencies.First(), ComparisonType.GovernorToGovernor, TokenType.Governor));

            return andSets;
        }

        /// <summary>
        /// Adds additional note particles into destination note part.
        /// </summary>
        /// <param name="sentence">Source sentence</param>
        /// <param name="dependency">Dependency to extract</param>
        /// <param name="destinationNotePart">Destination note part to add note particles to</param>
        private void AddAditionalNoteParticles(NotenizerSentence sentence, NotenizerDependency dependency, NotePart destinationNotePart)
        {
            AddAditionalNoteParticles(sentence, dependency, destinationNotePart, ComparisonType.DependentToGovernor);
        }

        /// <summary>
        /// Adds additional note particles into destination note part.
        /// </summary>
        /// <param name="sentence">Source sentence</param>
        /// <param name="dependency">Dependency to extract</param>
        /// <param name="destinationNotePart">Destination note part to add note particles to</param>
        /// <param name="comparisonType">Type of comparison to use in extraction of dependency</param>
        private void AddAditionalNoteParticles(NotenizerSentence sentence, NotenizerDependency dependency, NotePart destinationNotePart, ComparisonType comparisonType)
        {
            NotenizerDependency compound = sentence.Structure.GetDependencyByShortName(dependency, comparisonType, GrammaticalConstants.CompoudModifier);
            if (compound != null)
                destinationNotePart.Add(new NoteParticle(compound, TokenType.Dependent, true));

            NotenizerDependency nmod = sentence.Structure.GetDependencyByShortName(dependency, comparisonType, GrammaticalConstants.NominalModifier, GrammaticalConstants.AdjectivalModifier);
            if (nmod != null)
                destinationNotePart.Add(new NoteParticle(nmod, TokenType.Dependent, true));
        }

        /// <summary>
        /// Creates set of conjuction and.
        /// </summary>
        /// <param name="sentence"></param>
        /// <param name="setDependency"></param>
        /// <returns></returns>
        private NoteParticle CreateAndSet(NotenizerSentence sentence, NotenizerDependency setDependency)
        {
            return CreateAndSet(sentence, setDependency, ComparisonType.DependentToGovernor, TokenType.Dependent);
        }

        /// <summary>
        /// Creates and set.
        /// </summary>
        /// <param name="sentence"></param>
        /// <param name="setDependency"></param>
        /// <param name="comparisonType"></param>
        /// <param name="tokenType"></param>
        /// <returns></returns>
        private NoteParticle CreateAndSet(NotenizerSentence sentence, NotenizerDependency setDependency, ComparisonType comparisonType, TokenType tokenType)
        {
            NotePart notePart = new NotePart(sentence);
            setDependency.TokenType = tokenType;

            notePart.Add(new NoteParticle(setDependency, tokenType));
            AddAditionalNoteParticles(sentence, setDependency, notePart, comparisonType);

            return new NoteParticle(notePart.Value, setDependency.Dependent, setDependency);
        }

        #endregion Methods
    }
}
