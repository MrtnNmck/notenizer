using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using nsExtensions;
using nsConstants;

namespace nsNotenizerObjects
{
    public class NotePart
    {
        private List<NoteParticle> _noteParticles;
        private NotenizerSentence _originalSentence;

        public NotePart()
        {
            _noteParticles = new List<NoteParticle>();
        }

        public NotePart(NotenizerSentence originalSentence)
        {
            _noteParticles = new List<NoteParticle>();
            _originalSentence = originalSentence;
            InitializeStructure(originalSentence.DependencyWordsInSentenceCount());
        }
        
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

        public List<NoteParticle> NoteParticles
        {
            get { return _noteParticles; }
        }

        public List<NoteParticle> InitializedNoteParticles
        {
            get
            {
                return _noteParticles.Where(x => x != null).ToList();
            }
        }

        private void InitializeStructure(List<String> structureParts)
        {
            foreach (String structurePartLoop in structureParts)
                _noteParticles.Add(null);
        }

        private void InitializeStructure(int count)
        {
            for (int i = 0; i < count; i++)
                _noteParticles.Add(null);
        }

        public void Add(List<NoteParticle> noteObjects)
        {
            foreach (NoteParticle noteObjectLoop in noteObjects)
                Add(noteObjectLoop);
        }

        public void Add(NoteParticle noteParticle)
        {
            _noteParticles[noteParticle.NoteDependency.Position] = noteParticle;
        }

        public NotePart Clone()
        {
            NotePart clonedNotePart = new NotePart(this._originalSentence);

            foreach (NoteParticle noteParticleLoop in this.InitializedNoteParticles)
                clonedNotePart.Add(noteParticleLoop);

            return clonedNotePart;
        }
    }
}
