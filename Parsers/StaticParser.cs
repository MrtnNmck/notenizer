using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using nsNotenizerObjects;
using nsConstants;
using nsEnums;

namespace nsParsers
{
    public class StaticParser : NotenizerParser
    {
        #region Variables

        #endregion Variables

        #region Constructors

        public StaticParser()
        {
        }

        #endregion Constuctors

        #region Properties

        #endregion Properties

        #region Methods

        public override NotenizerNote Parse(NotenizerSentence sentence)
        {
            NotenizerNote note = new NotenizerNote(sentence);

            foreach (NotenizerDependency dependencyLoop in sentence.Structure.Dependencies)
            {
                if (dependencyLoop.Relation.IsNominalSubject()
                    && !((note.Structure.CompressedDependencies.ContainsKey(GrammaticalConstants.NominalSubject)
                            && note.Structure.CompressedDependencies[GrammaticalConstants.NominalSubject].Any(x => x.Key == dependencyLoop.Key))
                        || (note.Structure.CompressedDependencies.ContainsKey(GrammaticalConstants.NominalSubjectPassive)
                            && note.Structure.CompressedDependencies[GrammaticalConstants.NominalSubjectPassive].Any(x => x.Key == dependencyLoop.Key))))
                {
                    NotePart notePart = new NotePart(sentence);

                    NoteParticle nsubj = new NoteParticle(dependencyLoop, TokenType.Dependent);
                    notePart.Add(nsubj);

                    String pos = dependencyLoop.Governor.POS.Tag;
                    if (POSConstants.NounLikePOS.Contains(pos))
                    {
                        NotenizerDependency compound = sentence.Structure.GetDependencyByShortName(
                            dependencyLoop,
                            ComparisonType.DependentToGovernor,
                            GrammaticalConstants.CompoudModifier);

                        if (compound != null)
                        {
                            NoteParticle compoundObj = new NoteParticle(compound, TokenType.Dependent);
                            notePart.Add(compoundObj);
                        }

                        NotenizerDependency aux = sentence.Structure.GetDependencyByShortName(
                            dependencyLoop,
                            ComparisonType.GovernorToGovernor,
                            GrammaticalConstants.AuxModifier,
                            GrammaticalConstants.AuxModifierPassive);

                        if (aux != null)
                        {
                            NoteParticle auxObj = new NoteParticle(aux, TokenType.Dependent);
                            notePart.Add(auxObj);
                        }

                        NotenizerDependency cop = sentence.Structure.GetDependencyByShortName(dependencyLoop, ComparisonType.GovernorToGovernor, GrammaticalConstants.Copula);

                        if (cop != null)
                        {
                            NoteParticle copObj = new NoteParticle(cop, TokenType.Dependent);
                            notePart.Add(copObj);
                        }

                        List<NotenizerDependency> conjuctions = sentence.Structure.GetDependenciesByShortName(
                            dependencyLoop,
                            ComparisonType.GovernorToGovernor,
                            GrammaticalConstants.Conjuction);


                        String specific = String.Empty;
                        if (conjuctions != null && conjuctions.Count > 0)
                        {
                            List<NotenizerDependency> filteredConjs = FilterByPOS(conjuctions, POSConstants.ConjustionPOS);

                            foreach (NotenizerDependency filteredConjLoop in filteredConjs)
                            {
                                NotenizerDependency cc = sentence.Structure.GetDependencyByShortName(dependencyLoop, ComparisonType.GovernorToGovernor, GrammaticalConstants.CoordinatingConjuction);

                                if (cc.Dependent.Word == filteredConjLoop.Relation.Specific
                                    && sentence.Structure.DependencyIndex(filteredConjLoop) > sentence.Structure.DependencyIndex(cc))
                                {
                                    NoteParticle ccObj = new NoteParticle(cc, TokenType.Dependent);
                                    NoteParticle filteredConjObj = new NoteParticle(filteredConjLoop, TokenType.Dependent);

                                    notePart.Add(ccObj);
                                    notePart.Add(filteredConjObj);
                                }
                            }
                        }

                        // <== NMODS ==>
                        List<NotenizerDependency> nmodsList = sentence.Structure.GetDependenciesByShortName(
                            dependencyLoop, ComparisonType.GovernorToGovernor, GrammaticalConstants.NominalModifier);

                        if (nmodsList != null && nmodsList.Count > 0)
                        {
                            NotenizerDependency first = nmodsList.First();
                            NotenizerDependency neg = sentence.Structure.GetDependencyByShortName(
                                first, ComparisonType.DependentToGovernor, GrammaticalConstants.NegationModifier);

                            if (neg == null)
                            {
                                NoteParticle firstObj = new NoteParticle(first.Relation.AdjustedSpecific + NotenizerConstants.WordDelimeter + first.Dependent.Word, first, TokenType.Dependent);
                                notePart.Add(firstObj);
                            }
                            else
                            {
                                NoteParticle negObj = new NoteParticle(neg, TokenType.Dependent);
                                NoteParticle firstObj = new NoteParticle(first.Relation.AdjustedSpecific + NotenizerConstants.WordDelimeter + first.Dependent.Word, first, TokenType.Dependent);

                                notePart.Add(negObj);
                                notePart.Add(firstObj);
                            }

                            // second nmod depending on first one
                            NotenizerDependency nmodSecond = sentence.Structure.GetDependencyByShortName(
                                first, ComparisonType.DependentToGovernor, GrammaticalConstants.NominalModifier);

                            if (nmodSecond != null)
                            {
                                neg = sentence.Structure.GetDependencyByShortName(
                                first, ComparisonType.GovernorToGovernor, GrammaticalConstants.NegationModifier);

                                if (neg == null)
                                {
                                    NoteParticle secondObj = new NoteParticle(nmodSecond.Relation.AdjustedSpecific + NotenizerConstants.WordDelimeter + nmodSecond.Dependent.Word, nmodSecond, TokenType.Dependent);
                                    notePart.Add(secondObj);
                                }
                                else
                                {
                                    NoteParticle negObj = new NoteParticle(neg, TokenType.Dependent);
                                    NoteParticle secondObj = new NoteParticle(nmodSecond.Relation.AdjustedSpecific + NotenizerConstants.WordDelimeter + nmodSecond.Dependent.Word, nmodSecond, TokenType.Dependent);

                                    notePart.Add(negObj);
                                    notePart.Add(secondObj);
                                }
                            }
                        }
                        else
                        {
                            // <== AMODS ==>
                            NotenizerDependency amod1 = sentence.Structure.GetDependencyByShortName(dependencyLoop, ComparisonType.DependentToGovernor, GrammaticalConstants.AdjectivalModifier);

                            // <== AMODS ==>
                            NotenizerDependency amod2 = sentence.Structure.GetDependencyByShortName(dependencyLoop, ComparisonType.GovernorToGovernor, GrammaticalConstants.AdjectivalModifier);

                            if (amod1 != null || amod2 != null)
                            {
                                if (amod1 != null)
                                {
                                    NoteParticle amod1Obj = new NoteParticle(amod1, TokenType.Dependent);
                                    notePart.Add(amod1Obj);
                                }

                                if (amod2 != null)
                                {
                                    NoteParticle amod2Obj = new NoteParticle(amod2, TokenType.Dependent);
                                    notePart.Add(amod2Obj);
                                }
                            }
                            else
                            {
                                // <== NUMMODS ==>
                                NotenizerDependency nummod1 = sentence.Structure.GetDependencyByShortName(dependencyLoop, ComparisonType.DependentToGovernor, GrammaticalConstants.NumericModifier);

                                // <== NUMMODS ==>
                                NotenizerDependency nummod2 = sentence.Structure.GetDependencyByShortName(dependencyLoop, ComparisonType.GovernorToGovernor, GrammaticalConstants.NumericModifier);

                                if (nummod1 != null)
                                {
                                    NoteParticle nummod1Obj = new NoteParticle(nummod1, TokenType.Dependent);
                                    notePart.Add(nummod1Obj);
                                }

                                if (nummod2 != null)
                                {
                                    NoteParticle nummod2Obj = new NoteParticle(nummod2, TokenType.Dependent);
                                    notePart.Add(nummod2Obj);
                                }
                            }
                        }

                        NoteParticle governorObj = new NoteParticle(dependencyLoop, TokenType.Governor);
                        notePart.Add(governorObj);
                    }
                    else if (POSConstants.VerbLikePOS.Contains(pos))
                    {

                        NoteParticle gov = new NoteParticle(dependencyLoop, TokenType.Governor);
                        notePart.Add(gov);

                        NotenizerDependency dobj = sentence.Structure.GetDependencyByShortName(dependencyLoop, ComparisonType.GovernorToGovernor, GrammaticalConstants.DirectObject);

                        if (dobj != null)
                        {
                            NoteParticle dobjObj = new NoteParticle(dobj, TokenType.Dependent);
                            notePart.Add(dobjObj);

                            NotenizerDependency neg = sentence.Structure.GetDependencyByShortName(dobj, ComparisonType.DependentToGovernor, GrammaticalConstants.NegationModifier);

                            if (neg != null)
                            {
                                NoteParticle negObj = new NoteParticle(neg, TokenType.Dependent);
                                notePart.Add(negObj);
                            }
                        }

                        NotenizerDependency aux = sentence.Structure.GetDependencyByShortName(
                            dependencyLoop,
                            ComparisonType.GovernorToGovernor,
                            GrammaticalConstants.AuxModifier,
                            GrammaticalConstants.AuxModifierPassive);

                        if (aux != null)
                        {
                            NoteParticle auxObj = new NoteParticle(aux, TokenType.Dependent);
                            notePart.Add(auxObj);
                        }

                        // <== NMODS ==>
                        List<NotenizerDependency> nmodsList = sentence.Structure.GetDependenciesByShortName(
                            dependencyLoop,
                            ComparisonType.GovernorToGovernor,
                            GrammaticalConstants.NominalModifier);

                        if (nmodsList != null && nmodsList.Count > 0)
                        {
                            NotenizerDependency first = nmodsList.First();
                            NotenizerDependency neg = sentence.Structure.GetDependencyByShortName(first, ComparisonType.DependentToGovernor, GrammaticalConstants.NegationModifier);

                            if (neg == null)
                            {
                                NoteParticle firstObj = new NoteParticle(first.Relation.AdjustedSpecific + NotenizerConstants.WordDelimeter + first.Dependent.Word, first, TokenType.Dependent);
                                notePart.Add(firstObj);
                            }
                            else
                            {
                                NoteParticle negObj = new NoteParticle(neg, TokenType.Dependent);
                                NoteParticle firstObj = new NoteParticle(first.Relation.AdjustedSpecific + NotenizerConstants.WordDelimeter + first.Dependent.Word, first, TokenType.Dependent);
                                notePart.Add(firstObj);
                                notePart.Add(negObj);
                            }

                            // second nmod depending on first one
                            NotenizerDependency nmodSecond = sentence.Structure.GetDependencyByShortName(first, ComparisonType.DependentToGovernor, GrammaticalConstants.NominalModifier);

                            if (nmodSecond != null)
                            {
                                neg = sentence.Structure.GetDependencyByShortName(first, ComparisonType.GovernorToGovernor, GrammaticalConstants.NegationModifier);

                                if (neg == null)
                                {
                                    NoteParticle secondObj = new NoteParticle(nmodSecond.Relation.AdjustedSpecific + NotenizerConstants.WordDelimeter + nmodSecond.Dependent.Word, nmodSecond, TokenType.Dependent);
                                    notePart.Add(secondObj);
                                }
                                else
                                {
                                    NoteParticle negObj = new NoteParticle(neg, TokenType.Dependent);
                                    NoteParticle secondObj = new NoteParticle(nmodSecond.Relation.AdjustedSpecific + NotenizerConstants.WordDelimeter + nmodSecond.Dependent.Word, nmodSecond, TokenType.Dependent);
                                    notePart.Add(secondObj);
                                    notePart.Add(negObj);
                                }
                            }
                        }
                    }
                    note.Add(notePart);
                }
            }

            return note;
        }

        /// <summary>
        /// Filters dependencies by POS tag.
        /// </summary>
        /// <param name="dependencies"></param>
        /// <param name="poses"></param>
        /// <returns></returns>
        public List<NotenizerDependency> FilterByPOS(List<NotenizerDependency> dependencies, String[] poses)
        {
            List<NotenizerDependency> filteredDependencies = new List<NotenizerDependency>();

            foreach (NotenizerDependency dependencyLoop in dependencies)
            {
                if (poses.Contains(dependencyLoop.Dependent.POS.Tag))
                    filteredDependencies.Add(dependencyLoop);
            }

            return filteredDependencies;
        }

        #endregion Methods
    }
}
