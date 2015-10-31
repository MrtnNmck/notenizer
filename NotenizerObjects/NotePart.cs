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
        private String _prefixWord;
        private String _postfixWord;
        private String _mainWord;

        private List<NoteParticle> _noteParticles;

        public NotePart()
        {
            _prefixWord = String.Empty;
            _postfixWord = String.Empty;
            _mainWord = String.Empty;

            _noteParticles = new List<NoteParticle>();
        }

        public NotePart(NotenizerSentence originalSentence)
        {
            _noteParticles = new List<NoteParticle>();
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

        public void Add(NoteParticle noteObject)
        {
            _noteParticles[noteObject.NoteWord.Index - 1] = noteObject;
        }
    }
}
