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

        public NotePart()
        {
            _prefixWord = String.Empty;
            _postfixWord = String.Empty;
            _mainWord = String.Empty;
        }

        public NotePart(String mainWord)
        {
            _prefixWord = String.Empty;
            _postfixWord = String.Empty;
            _mainWord = String.Empty;
        }

        public String PrefixWord
        {
            get { return _prefixWord; }
            set
            {
                if (!value.IsNullOrEmpty())
                    _prefixWord = value;
            }
        }

        public String PostfixWord
        {
            get { return _postfixWord; }
            set
            {
                if (!value.IsNullOrEmpty())
                    _postfixWord = value;
            }
        }

        public String MainWord
        {
            get { return _mainWord; }
            set
            {
                if (!value.IsNullOrEmpty())
                    _mainWord = value;
            }
        }

        public String Value
        {
            get
            {
                StringBuilder sb = new StringBuilder();

                if (!_prefixWord.IsNullOrEmpty())
                    sb.Append(_prefixWord).Append(NotenizerConstants.WordDelimeter);

                if (!_mainWord.IsNullOrEmpty())
                    sb.Append(_mainWord);

                if (!_postfixWord.IsNullOrEmpty())
                {
                    if (!_mainWord.IsNullOrEmpty() && !_mainWord.EndsWith(NotenizerConstants.WordDelimeter))
                        sb.Append(NotenizerConstants.WordDelimeter).Append(_postfixWord);
                    else
                        sb.Append(_postfixWord);
                }

                return sb.ToString();
            }
        }
    }
}
