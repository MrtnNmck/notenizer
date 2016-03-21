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
    public class AndParser : NotenizerParser
    {
        public AndParser()
        {
        }

        public override void Parse(NotenizerSentence sentence)
        {
            
        }

        public override Boolean IsParsableSentence(NotenizerSentence sentence)
        {
            return (sentence.CompressedDependencies.ContainsKey(GrammaticalConstants.Conjuction)
                && sentence.CompressedDependencies[GrammaticalConstants.Conjuction] != null
                && sentence.CompressedDependencies[GrammaticalConstants.Conjuction].Exists(x => x.Relation.Specific == GrammaticalConstants.AndConjuction || x.Relation.ShortName == GrammaticalConstants.AppositionalModifier));
        }

        public List<NoteParticle> GetAndSets(NotenizerSentence sentence)
        {
            return GetAndSets(sentence, true);
        }

        public List<NoteParticle> GetAndSets(NotenizerSentence sentence, bool checkParsability)
        {
            if (checkParsability && !IsParsableSentence(sentence))
                return null;

            List<NoteParticle> andSets;
            List<NotenizerDependency> repetitionPartDependencies;
            List<NotenizerDependency> andConjuctionsApposDependencies;

            andSets = new List<NoteParticle>();
            repetitionPartDependencies = new List<NotenizerDependency>();
            andConjuctionsApposDependencies = sentence.Dependencies.Where(x => x.Relation.Specific == GrammaticalConstants.AndConjuction
                                                                            || x.Relation.ShortName == GrammaticalConstants.AppositionalModifier).ToList();
            repetitionPartDependencies = repetitionPartDependencies.Concat(andConjuctionsApposDependencies).ToList();

            foreach (NotenizerDependency andApposDependencyLoop in andConjuctionsApposDependencies)
            {
                List<NotenizerDependency> apposSecondLevelDependencies = sentence.GetDependenciesByShortName(andApposDependencyLoop, ComparisonType.DependentToGovernor, GrammaticalConstants.AppositionalModifier);
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

        private void AddAditionalNoteParticles(NotenizerSentence sentence, NotenizerDependency dependency, NotePart destinationNotePart)
        {
            AddAditionalNoteParticles(sentence, dependency, destinationNotePart, ComparisonType.DependentToGovernor);
        }

        private void AddAditionalNoteParticles(NotenizerSentence sentence, NotenizerDependency dependency, NotePart destinationNotePart, ComparisonType comparisonType)
        {
            NotenizerDependency compound = sentence.GetDependencyByShortName(dependency, comparisonType, GrammaticalConstants.CompoudModifier);
            if (compound != null)
                destinationNotePart.Add(new NoteParticle(compound, TokenType.Dependent, true));

            NotenizerDependency nmod = sentence.GetDependencyByShortName(dependency, comparisonType, GrammaticalConstants.NominalModifier, GrammaticalConstants.AdjectivalModifier);
            if (nmod != null)
                destinationNotePart.Add(new NoteParticle(nmod, TokenType.Dependent, true));
        }

        private NoteParticle CreateAndSet(NotenizerSentence sentence, NotenizerDependency setDependency)
        {
            return CreateAndSet(sentence, setDependency, ComparisonType.DependentToGovernor, TokenType.Dependent);
        }

        private NoteParticle CreateAndSet(NotenizerSentence sentence, NotenizerDependency setDependency, ComparisonType comparisonType, TokenType tokenType)
        {
            NotePart notePart = new NotePart(sentence);
            setDependency.TokenType = tokenType;

            notePart.Add(new NoteParticle(setDependency, tokenType));
            AddAditionalNoteParticles(sentence, setDependency, notePart, comparisonType);

            return new NoteParticle(notePart.Value, setDependency.Dependent, setDependency);
        }
    }
}
