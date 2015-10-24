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

        private List<NoteObject> _noteObjects;

        public NotePart()
        {
            _prefixWord = String.Empty;
            _postfixWord = String.Empty;
            _mainWord = String.Empty;

            _noteObjects = new List<NoteObject>();
        }

        public NotePart(NotenizerSentence originalSentence)
        {
            _noteObjects = new List<NoteObject>();
            InitializeStructure(originalSentence.DependencyWordsInSentenceCount());
        }
        
        public String Value
        {
            get
            {
                String value = String.Empty;
                foreach (NoteObject noteObjectLoop in _noteObjects)
                {
                    if (noteObjectLoop != null)
                        value += noteObjectLoop.NoteWordValue + NotenizerConstants.WordDelimeter;
                }

                return value;
            }
        }

        public List<NoteObject> NoteObjects
        {
            get { return _noteObjects; }
        }

        private void InitializeStructure(List<String> structureParts)
        {
            foreach (String structurePartLoop in structureParts)
                _noteObjects.Add(null);
        }

        private void InitializeStructure(int count)
        {
            for (int i = 0; i < count; i++)
                _noteObjects.Add(null);
        }

        public void Add(NoteObject noteObject)
        {
            _noteObjects[noteObject.NoteWord.Index - 1] = noteObject;
        }
    }
}
