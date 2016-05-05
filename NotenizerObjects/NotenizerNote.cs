using nsConstants;
using System;
using nsExtensions;
using System.Collections.Generic;
using nsEnums;
using System.Linq;

namespace nsNotenizerObjects
{
    /// <summary>
    /// Notenizer note.
    /// </summary>
    public class NotenizerNote
    {
        #region Variables

        private NotenizerSentence _originalSentence;
        private String _text;
        private List<NotePart> _noteParts;
        private DateTime _createdAt;
        private DateTime _updatedAt;
        private NotenizerNoteRule _rule;
        private NotenizerAndRule _andParserRule;
        private NotenizerStructure _structure;
        private Note _note;

        #endregion Variables

        #region Constructors

        public NotenizerNote(NotenizerSentence originalSentence)
        {
            _text = String.Empty;
            _noteParts = new List<NotePart>();
            _originalSentence = originalSentence;
            _createdAt = DateTime.Now;
            _updatedAt = DateTime.Now;
            _structure = new NotenizerStructure();
        }

        #endregion Constuctors

        #region Properties

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
        public String Text
        {
            get { return _text.Trim(); }
        }

        /// <summary>
        /// Created at timestamp.
        /// </summary>
        public DateTime CreatedAt
        {
            set { _createdAt = value; }
            get { return _createdAt; }
        }

        /// <summary>
        /// Updated at timestamp.
        /// </summary>
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

        /// <summary>
        /// Dependencies of note.
        /// </summary>
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

        /// <summary>
        /// Unused dependencies from original sentence.
        /// </summary>
        public NotenizerDependencies UnusedDependencies
        {
            get
            {
                return this.NoteDependencies.Complement(_originalSentence.Structure.Dependencies, _originalSentence.Structure.CompressedDependencies);
            }
        }

        /// <summary>
        /// Rule of note.
        /// </summary>
        public NotenizerNoteRule Rule
        {
            get { return _rule; }
            set { _rule = value; }
        }

        /// <summary>
        /// And-rule of note.
        /// </summary>
        public NotenizerAndRule AndRule
        {
            get { return _andParserRule; }
            set { _andParserRule = value; }
        }

        /// <summary>
        /// List of sentence terminators.
        /// </summary>
        public SentencesTerminators SentencesTerminators
        {
            get { return new SentencesTerminators(this._noteParts); }
        }

        /// <summary>
        /// Dependencies of note.
        /// </summary>
        public NotenizerDependencies Dependencies
        {
            get
            {
                NotenizerDependencies dependencies = new NotenizerDependencies();

                foreach (NotePart notePartLoop in this._noteParts)
                {
                    foreach (NoteParticle noteParticleLoop in notePartLoop.NoteParticles)
                    {
                        if (noteParticleLoop == null)
                            continue;

                        dependencies.Add(noteParticleLoop.NoteDependency);
                    }
                }

                return dependencies;
            }
        }

        /// <summary>
        /// Structure of note.
        /// </summary>
        public NotenizerStructure Structure
        {
            get { return this._structure; }
            set { this._structure = value; }
        }

        /// <summary>
        /// Persistable note.
        /// </summary>
        public Note Note
        {
            get { return this._note; }
            set { this._note = value; }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Replaces note parts with other note parts.
        /// </summary>
        /// <param name="noteParts"></param>
        public void Replace(List<NotePart> noteParts)
        {
            _text = String.Empty;
            _noteParts.Clear();
            _structure.Dependencies.Clear();
            _structure.CompressedDependencies.Clear();

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
            _text += notePart.AdjustedValue + NotenizerConstants.WordDelimeter;
            _noteParts.Add(notePart);

            foreach (NoteParticle notePartNoteParticleLoop in notePart.InitializedNoteParticles)
            {
                this._structure.CompressedDependencies.Add(notePartNoteParticleLoop.NoteDependency);
                this._structure.Dependencies.Add(notePartNoteParticleLoop.NoteDependency);
            }
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
                if (indexLoop - lastIndex >= _noteParts[0].InitializedNoteParticles.Count)
                    return;

                if (lastIndex > _noteParts[0].InitializedNoteParticles.Count)
                    lastIndex = _noteParts[0].InitializedNoteParticles.Count - 1;

                if (indexLoop < lastIndex)
                    return;

                NotePart notePart = new NotePart(_originalSentence);

                notePart.Add(_noteParts[0].InitializedNoteParticles.GetRange(lastIndex, indexLoop - lastIndex));

                lastIndex = indexLoop;
                parts.Add(notePart);
            }

            _text = String.Empty;
            _noteParts = new List<NotePart>();

            Add(parts);
        }

        /// <summary>
        /// Splits note into sentences.
        /// </summary>
        /// <param name="sentenceEnd"></param>
        public void SplitToSentences(int sentenceEnd)
        {
            List<NotePart> parts = new List<NotePart>();

            NotePart notePart = new NotePart(_originalSentence);
            if (sentenceEnd > _noteParts[0].InitializedNoteParticles.Count)
                sentenceEnd = _noteParts[0].InitializedNoteParticles.Count - 1;

            if (sentenceEnd < 0)
                return;

            notePart.Add(_noteParts[0].InitializedNoteParticles.GetRange(0, sentenceEnd));
            parts.Add(notePart);
            _text = String.Empty;
            _noteParts = new List<NotePart>();
            Add(parts);
        }

        /// <summary>
        /// Creates rule for note.
        /// </summary>
        /// <returns></returns>
        public NotenizerNoteRule CreateRule()
        {
            this._rule = new NotenizerNoteRule();
            this._rule.Match = new Match(NotenizerConstants.MaxMatchValue);
            this._rule.SentencesTerminators = new SentencesTerminators(this._noteParts.Select(x => x.InitializedNoteParticles.Count).ToList<int>());

            return this._rule;
        }

        /// <summary>
        /// Create structure of note.
        /// </summary>
        /// <returns></returns>
        public NotenizerStructure CreateStructure()
        {
            this._structure = new NotenizerStructure(this.Dependencies);
            this._rule.Structure = this._structure;

            return this._structure;
        }

        /// <summary>
        /// Create persistable note from note.
        /// </summary>
        /// <returns></returns>
        public Note CreateNote()
        {
            this._note = new Note(this._text);

            return this._note;
        }

        #endregion Methods
    }
}
