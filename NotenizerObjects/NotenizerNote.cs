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
        private NotenizerAndParserRule _andParserRule;
        private CompressedDependencies _compressedDependencies;

        public NotenizerNote(NotenizerSentence originalSentence)
        {
            _note = String.Empty;
            _noteParts = new List<NotePart>();
            _originalSentence = originalSentence;
            _createdAt = DateTime.Now;
            _updatedAt = DateTime.Now;
            _compressedDependencies = new CompressedDependencies();
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
            get { return _note.Trim(); }
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

        public NotenizerDependencies NoteDependencies
        {
            get
            {
                NotenizerDependencies noteDependencies = new NotenizerDependencies();

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

        public NotenizerDependencies UnusedDependencies
        {
            get
            {
                return this.NoteDependencies.Complement(_originalSentence.Dependencies, _originalSentence.CompressedDependencies);
            }
        }

        public NotenizerNoteRule Rule
        {
            get { return _rule; }
            set { _rule = value; }
        }

        public NotenizerAndParserRule AndParserRule
        {
            get { return _andParserRule; }
            set { _andParserRule = value; }
        }

        public CompressedDependencies CompressedDependencies
        {
            get { return _compressedDependencies; }
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
            _note += notePart.AdjustedValue + NotenizerConstants.WordDelimeter;
            _noteParts.Add(notePart);

            foreach (NoteParticle notePartNoteParticleLoop in notePart.InitializedNoteParticles)
                _compressedDependencies.Add(notePartNoteParticleLoop.NoteDependency);
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

        public void SplitToSentences(int sentenceEnd)
        {
            List<NotePart> parts = new List<NotePart>();

            NotePart notePart = new NotePart(_originalSentence);
            notePart.Add(_noteParts[0].InitializedNoteParticles.GetRange(0, sentenceEnd));
            parts.Add(notePart);
            _note = String.Empty;
            _noteParts = new List<NotePart>();
            Add(parts);
        }

        public void CreateRule()
        {
            this._rule = new NotenizerNoteRule(nsEnums.CreatedBy.Notenizer);
            this._rule.Match = new Match(0, 0);
        }
    }
}
