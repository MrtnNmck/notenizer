using nsConstants;
using nsEnums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nsNotenizerObjects
{
    public class NoteParticle
    {
        private NotenizerWord _noteWord;
        private String _noteWordValue;
        private NotenizerDependency _noteDependency;
        private TokenType _tokenType;

        public NoteParticle(NotenizerWord noteWord, NotenizerDependency noteDependency)
        {
            _noteWord = noteWord;
            _noteDependency = noteDependency.Clone();
            _noteWordValue = MakeWordConsiderRelation(noteWord, noteDependency.Relation);
        }

        public NoteParticle(String noteWordValue, NotenizerWord noteWord, NotenizerDependency noteDependency)
        {
            _noteWordValue = noteWordValue;
            _noteWord = noteWord;
            _noteDependency = noteDependency.Clone();
        }

        public NoteParticle(NotenizerDependency dependency, TokenType tokenType)
        {
            _tokenType = tokenType;
            _noteDependency = dependency.Clone();
            _noteDependency.TokenType = tokenType;
            _noteWord = _tokenType == TokenType.Dependent ? dependency.Dependent : dependency.Governor;
            _noteWordValue = MakeWordConsiderRelation(_noteWord, _noteDependency.Relation);
        }

        public NoteParticle(String noteWordValue, NotenizerDependency dependency, TokenType tokenType)
        {
            _tokenType = tokenType;
            _noteDependency = dependency.Clone();
            _noteDependency.TokenType = tokenType;
            _noteWordValue = noteWordValue;
            _noteWord = tokenType == TokenType.Dependent ? dependency.Dependent : dependency.Governor;
        }

        public NotenizerWord NoteWord
        {
            get { return _noteWord; }
        }

        public NotenizerDependency NoteDependency
        {
            get { return _noteDependency; }
        }

        public String NoteWordValue
        {
            get { return _noteWordValue; }
        }

        //public TokenType TokenType
        //{
        //    get { return _tokenType; }
        //}

        private String MakeWordConsiderRelation(NotenizerWord noteWord, NotenizerRelation relation)
        {
            String word = String.Empty;

            if (relation.ShortName == GrammaticalConstants.NominalModifier)
                word = relation.AdjustedSpecific + NotenizerConstants.WordDelimeter + noteWord.Word;
            else
                word = noteWord.Word;

            return word;
        }

        //public NotenizerWord GetCorrespondingWord()
        //{
        //    return _noteDependency.GetWordByTokenType(_tokenType);
        //}
    }
}
