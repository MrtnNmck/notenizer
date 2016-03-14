using nsConstants;
using System;

using nsExtensions;
using System.Collections.Generic;
using nsEnums;

namespace nsNotenizerObjects
{
    public class Note
    {
        private NotenizerSentence _originalSentence;
        private String _note;
        private List<NotePart> _noteParts;
        private CreatedBy _createdBy = CreatedBy.Notenizer;
        private List<NotenizerDependency> _unusedDependencies;
        private List<NotenizerDependency> _noteDependencies;
        private DateTime _createdAt;
        private DateTime _updatedAt;
        private NotenizerNoteRule _rule;

        public Note(NotenizerSentence originalSentence)
        {
            _note = String.Empty;
            _noteParts = new List<NotePart>();
            _originalSentence = originalSentence;
            _unusedDependencies = new List<NotenizerDependency>();
            _noteDependencies = new List<NotenizerDependency>();
            _createdAt = DateTime.Now;
            _updatedAt = DateTime.Now;
        }

        /// <summary>
        /// Original sentence that is being parsed.
        /// </summary>
        public NotenizerSentence OriginalSentence
        {
            get { return _originalSentence; }
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
