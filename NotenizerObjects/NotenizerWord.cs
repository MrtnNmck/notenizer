using edu.stanford.nlp.ling;
using nsExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nsNotenizerObjects
{
	public class NotenizerWord
	{
		private IndexedWord _indexedWord;
		private String _wordString;
		private int _startingPosition;
		private int _endPosition;
		private String _lemma;
        private PartOfSpeech _pos;
        private int _index;
        private NamedEntity _namedEntity;

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

        public NotenizerWord(String value, String POS)
        {
            _wordString = value;
            _pos = new PartOfSpeech(POS);
        }

		public String Word
		{
			get { return _wordString; }
		}

		public int StartingPosition
		{
			get { return _startingPosition; }
		}

		public int EndPosition
		{
			get { return _endPosition; }
		}

		public String Lemma
		{
			get { return _lemma; }
		}

		public NamedEntity NamedEntity
		{
			get { return _namedEntity; }
		}

        public PartOfSpeech POS
        {
            get { return _pos; }
        }

        public int Index
        {
            get { return _index; }
        }

        public override string ToString()
        {
            return _wordString;
        }

        public static bool operator ==(NotenizerWord w1, NotenizerWord w2)
        {
            return (w1.GetUniqueIdentifier() == w2.GetUniqueIdentifier());
        }

        public static bool operator !=(NotenizerWord w1, NotenizerWord w2)
        {
            return !(w1 == w2);
        }

        public override Boolean Equals(Object obj)
        {
            if (!(obj is NotenizerWord))
                return false;

            return (this.GetUniqueIdentifier() == (obj as NotenizerWord).GetUniqueIdentifier());
        }

        public override Int32 GetHashCode()
        {
            return base.GetHashCode();
        }

        private String GetUniqueIdentifier()
        {
            return _wordString
                + "/"
                + _pos
                + "/"
                + _index;
        }
    }
}
