using nsConstants;
using System;
using nsExtensions;
using System.Collections.Generic;
using nsEnums;
using System.Linq;

namespace nsNotenizerObjects
{
    public class NotenizerNote
    {
        private NotenizerSentence _originalSentence;
        private String _note;
        private List<NotePart> _noteParts;
        private CreatedBy _createdBy = CreatedBy.Notenizer;
        private DateTime _createdAt;
        private DateTime _updatedAt;
        private NotenizerNoteRule _rule;

        public NotenizerNote(NotenizerSentence originalSentence)
        {
            _note = String.Empty;
            _noteParts = new List<NotePart>();
            _originalSentence = originalSentence;
            _createdAt = DateTime.Now;
            _updatedAt = DateTime.Now;
        }

        /// <summary>
        /// Original sentence that is being parsed.
        /// </summary>
        public NotenizerSentence OriginalSentence
        {
            get { return _originalSentence; }
            set { _originalSentence = value; }
        }

        /// <summary>
        /// String representation of note from sentence.
        /// </summary>
        public String Value
        {
            get { return _note; }
        }

        /// <summary>
        /// Indicator who created the note.
        /// User or Notenizer.
        /// </summary>
        public CreatedBy CreatedBy
        {
            get { return _createdBy; }
            set { _createdBy = value; }
        }

        public DateTime CreatedAt
        {
            set { _createdAt = value; }
            get { return _createdAt; }
        }

        public DateTime UpdatedAt
        {
            set { _updatedAt = value; }
            get { return _updatedAt; }
        }

        /// <summary>
        /// Parts of note.
        /// </summary>
        public List<NotePart> NoteParts
        {
            get { return _noteParts; }
        }

        public List<NotenizerDependency> NoteDependencies
        {
            get
            {
                List<NotenizerDependency> noteDependencies = new List<NotenizerDependency>();

                foreach (NotePart notePartLoop in this._noteParts)
                {
                    foreach (NoteParticle noteParticleLoop in notePartLoop.InitializedNoteParticles)
                    {
                        noteDependencies.Add(noteParticleLoop.NoteDependency);
                    }
                }

                return noteDependencies;
            }
        }

        public List<NotenizerDependency> UnusedDependencies
        {
            get
            {
                List<NotenizerDependency> unusedDependencies = new List<NotenizerDependency>();
                List<NotenizerDependency> noteDependencies = this.NoteDependencies;

                foreach (NotenizerDependency originalDependencyLoop in this._originalSentence.Dependencies)
                {
                    if (!originalDependencyLoop.Relation.IsRelation(GrammaticalConstants.Root) && !noteDependencies.Exists(x => x.Key == originalDependencyLoop.Key))
                        unusedDependencies.Add(originalDependencyLoop);
                }

                // we need to check, if both tokens of NSUBJ / NSUBJPASS were in note dependencies
                // if not, we need to add the other one to the unused dependencies.
                IEnumerable<NotenizerDependency> nsubjDependencies = new List<NotenizerDependency>();

                if (_originalSentence.CompressedDependencies.ContainsKey(GrammaticalConstants.NominalSubject))
                    nsubjDependencies = _originalSentence.CompressedDependencies[GrammaticalConstants.NominalSubject];

                if (_originalSentence.CompressedDependencies.ContainsKey(GrammaticalConstants.NominalSubjectPassive))
                    nsubjDependencies = nsubjDependencies.Concat(_originalSentence.CompressedDependencies[GrammaticalConstants.NominalSubjectPassive]);

                foreach (NotenizerDependency nsubjDependencyLoop in nsubjDependencies)
                {
                    if (!noteDependencies.Exists(x => x.Key == nsubjDependencyLoop.Key && x.TokenType == nsubjDependencyLoop.TokenType)
                        && !noteDependencies.Exists(x => x.Key == nsubjDependencyLoop.Key && x.TokenType == nsubjDependencyLoop.TokenType))
                        unusedDependencies.Add(nsubjDependencyLoop);
                }

                return unusedDependencies;
            }
        }

        public NotenizerNoteRule Rule
        {
            get { return _rule; }
            set { _rule = value; }
        }

        public void Replace(List<NotePart> noteParts)
        {
            _note = String.Empty;
            _noteParts.Clear();

            Add(noteParts);
        }

        /// <summary>
        /// Adds List of parts to this note.
        /// </summary>
        /// <param name="noteParts"></param>
        public void Add(List<NotePart> noteParts)
        {
            foreach (NotePart notePartLoop in noteParts)
                Add(notePartLoop);
        }

        /// <summary>
        /// Adds note part to this note.
        /// </summary>
        /// <param name="notePart"></param>
        public void Add(NotePart notePart)
        {
            _note += notePart.Value.Trim().CapitalizeSentence() + NotenizerConstants.SentenceDelimeter;
            _noteParts.Add(notePart);
        }

        /// <summary>
        /// Splits sentence to sentences by indexes of sentences last words.
        /// </summary>
        /// <param name="endsIndices"></param>
        public void SplitToSentences(List<int> endsIndices)
        {
            int lastIndex = 0;
            List<NotePart> parts = new List<NotePart>();

            foreach (int indexLoop in endsIndices)
            {
                // TODO: docasna podmienka, lebo nie je doriesene rozoznavanie medzi sadami pravidiel
                // a vzdy sa zoberie prva, preto nemusi sediet pre niektoru vetu a tym padom by to tu
                // padlo na ArgumentOutOfRangeException
                if (indexLoop - lastIndex > _noteParts[0].InitializedNoteParticles.Count)
                    return;

                NotePart notePart = new NotePart(_originalSentence);
                notePart.Add(_noteParts[0].InitializedNoteParticles.GetRange(lastIndex, indexLoop - lastIndex));

                lastIndex = indexLoop;
                parts.Add(notePart);
            }

            _note = String.Empty;
            _noteParts = new List<NotePart>();

            Add(parts);
        }
    }
}
