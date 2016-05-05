using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using nsExtensions;
using nsConstants;

namespace nsNotenizerObjects
{
    /// <summary>
    /// Part of a note.
    /// </summary>
    public class NotePart
    {
        #region Variables

        private List<NoteParticle> _noteParticles;
        private NotenizerSentence _originalSentence;

        #endregion Variables

        #region Constructors

        public NotePart(NotenizerSentence originalSentence)
        {
            _noteParticles = new List<NoteParticle>();
            _originalSentence = originalSentence;
            InitializeStructure(originalSentence.Structure.DependencyWordsInSentenceCount());
        }

        #endregion Constuctors

        #region Properties

        /// <summary>
        /// String representation of note part.
        /// </summary>
        public String Value
        {
            get
            {
                String value = String.Empty;
                foreach (NoteParticle noteObjectLoop in _noteParticles)
                {
                    if (noteObjectLoop != null)
                        value += noteObjectLoop.NoteWordValue + NotenizerConstants.WordDelimeter;
                }

                return value;
            }
        }

        /// <summary>
        /// Adjusted value.
        /// Trims, capitalizes and termiantes note part.
        /// </summary>
        public String AdjustedValue
        {
            get { return Value.Trim().CapitalizeSentence().TerminateSentence(NotenizerConstants.SentenceTerminator); }
        }

        /// <summary>
        /// List of note particles.
        /// </summary>
        public List<NoteParticle> NoteParticles
        {
            get { return _noteParticles; }
        }

        /// <summary>
        /// Initialized note particles.
        /// </summary>
        public List<NoteParticle> InitializedNoteParticles
        {
            get
            {
                return _noteParticles.Where(x => x != null).ToList();
            }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Initializes structure.
        /// </summary>
        /// <param name="structureParts"></param>
        private void InitializeStructure(List<String> structureParts)
        {
            foreach (String structurePartLoop in structureParts)
                _noteParticles.Add(null);
        }

        /// <summary>
        /// Initializes structure.
        /// </summary>
        /// <param name="count"></param>
        private void InitializeStructure(int count)
        {
            for (int i = 0; i < count; i++)
                _noteParticles.Add(null);
        }

        /// <summary>
        /// Adds note particles into note part.
        /// </summary>
        /// <param name="noteObjects"></param>
        public void Add(List<NoteParticle> noteObjects)
        {
            foreach (NoteParticle noteObjectLoop in noteObjects)
                Add(noteObjectLoop);
        }

        /// <summary>
        /// And note particle into note part.
        /// </summary>
        /// <param name="noteParticle"></param>
        public void Add(NoteParticle noteParticle)
        {
            Add(noteParticle, noteParticle.NoteDependency.Position);
        }

        /// <summary>
        /// Adds note particle into note part on specified position.
        /// </summary>
        /// <param name="noteParticle"></param>
        /// <param name="position"></param>
        public void Add(NoteParticle noteParticle, int position)
        {
            if (position >= _noteParticles.Count)
                position = _noteParticles.Count - 1;

            _noteParticles[position] = noteParticle;
        }

        /// <summary>
        /// Fully clones note part object.
        /// </summary>
        /// <returns></returns>
        public NotePart Clone()
        {
            NotePart clonedNotePart = new NotePart(this._originalSentence);

            foreach (NoteParticle noteParticleLoop in this.InitializedNoteParticles)
                clonedNotePart.Add(noteParticleLoop);

            return clonedNotePart;
        }

        #endregion Methods
    }
}
