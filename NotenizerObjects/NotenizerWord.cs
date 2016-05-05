using edu.stanford.nlp.ling;
using nsExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nsNotenizerObjects
{
    /// <summary>
    /// Word of sentence.
    /// </summary>
	public class NotenizerWord
	{
        #region Variables

        private IndexedWord _indexedWord;
        private String _wordString;
        private int _startingPosition;
        private int _endPosition;
        private String _lemma;
        private PartOfSpeech _pos;
        private int _index;
        private NamedEntity _namedEntity;

        #endregion Variables

        #region Constructors

        public NotenizerWord(IndexedWord indexedWord)
        {
            _indexedWord = indexedWord;
            _wordString = indexedWord.word();
            _startingPosition = indexedWord.beginPosition();
            _endPosition = indexedWord.endPosition();
            _namedEntity = new NamedEntity(indexedWord.ner());

            Object temp = indexedWord.get(typeof(CoreAnnotations.EndIndexAnnotation));

            if (temp != null)
                _index = temp.ToInt();
            else
                _index = -1;

            temp = indexedWord.get(typeof(CoreAnnotations.PartOfSpeechAnnotation));

            if (temp != null)
                _pos = new PartOfSpeech(temp.ToString());
            else
                _pos = new PartOfSpeech(String.Empty);

            temp = indexedWord.lemma();

            if (temp != null)
                _lemma = temp.ToString();
            else
                _lemma = String.Empty;
        }

        public NotenizerWord(String pos, int index, String lemma, String ner)
        {
            _pos = new PartOfSpeech(pos);
            _index = index;
            _lemma = lemma;
            _namedEntity = new NamedEntity(ner);
        }

        #endregion Constuctors

        #region Properties

        /// <summary>
        /// Value of word.
        /// </summary>
        public String Word
        {
            get { return _wordString; }
        }

        /// <summary>
        /// Starting posisiton of word.
        /// </summary>
        public int StartingPosition
        {
            get { return _startingPosition; }
        }

        /// <summary>
        /// End position of word.
        /// </summary>
        public int EndPosition
        {
            get { return _endPosition; }
        }

        /// <summary>
        /// Lemma of word.
        /// </summary>
        public String Lemma
        {
            get { return _lemma; }
        }

        /// <summary>
        /// Word's named entity.
        /// </summary>
        public NamedEntity NamedEntity
        {
            get { return _namedEntity; }
        }

        /// <summary>
        /// Part-of-Speech tag of word.
        /// </summary>
        public PartOfSpeech POS
        {
            get { return _pos; }
        }

        /// <summary>
        /// Word's index.
        /// </summary>
        public int Index
        {
            get { return _index; }
        }

        #endregion Properties

        #region Methods

        public override string ToString()
        {
            return _wordString;
        }

        /// <summary>
        /// Operator to compare two words.
        /// </summary>
        /// <param name="w1"></param>
        /// <param name="w2"></param>
        /// <returns></returns>
        public static bool operator ==(NotenizerWord w1, NotenizerWord w2)
        {
            return (w1.GetUniqueIdentifier() == w2.GetUniqueIdentifier());
        }

        /// <summary>
        /// Operator to compare two words.
        /// </summary>
        /// <param name="w1"></param>
        /// <param name="w2"></param>
        /// <returns></returns>
        public static bool operator !=(NotenizerWord w1, NotenizerWord w2)
        {
            return !(w1 == w2);
        }

        /// <summary>
        /// Checks words is equal with another word.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override Boolean Equals(Object obj)
        {
            if (!(obj is NotenizerWord))
                return false;

            return (this.GetUniqueIdentifier() == (obj as NotenizerWord).GetUniqueIdentifier());
        }

        /// <summary>
        /// Gets hash code of object.
        /// </summary>
        /// <returns></returns>
        public override Int32 GetHashCode()
        {
            return base.GetHashCode();
        }

        /// <summary>
        /// Gets unique identifier.
        /// </summary>
        /// <returns></returns>
        private String GetUniqueIdentifier()
        {
            return _wordString
                + "/"
                + _pos
                + "/"
                + _index;
        }

        #endregion Methods
    }
}
