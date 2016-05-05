using nsConstants;
using nsEnums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nsNotenizerObjects
{
    /// <summary>
    /// Part of note part.
    /// </summary>
    public class NoteParticle
    {
        #region Variables

        private NotenizerWord _noteWord;
        private String _noteWordValue;
        private NotenizerDependency _noteDependency;
        private TokenType _tokenType;

        #endregion Variables

        #region Constructors

        public NoteParticle(NotenizerDependency dependency)
        {
            _tokenType = dependency.TokenType;
            _noteDependency = dependency.Clone();
            _noteWord = dependency.CorrespondingWord;
            //_noteWordValue = MakeWordConsiderRelation(_noteWord, _noteDependency.Relation);
            _noteWordValue = _noteWord.Word;
        }

        public NoteParticle(String noteWordValue, NotenizerWord noteWord, NotenizerDependency noteDependency)
        {
            _noteWordValue = noteWordValue;
            _noteWord = noteWord;
            _noteDependency = noteDependency.Clone();
        }

        public NoteParticle(NotenizerDependency dependency, TokenType tokenType, bool considerRelationInWordValueMaking = false)
        {
            if (tokenType == TokenType.Unidentified)
                throw new Exception("Can't make NoteParticle from token of type " + tokenType.ToString());

            _tokenType = tokenType;
            _noteDependency = dependency.Clone();
            _noteDependency.TokenType = tokenType;
            _noteWord = _tokenType == TokenType.Dependent ? dependency.Dependent : dependency.Governor;
            _noteWordValue = considerRelationInWordValueMaking ? MakeWordConsiderRelation(_noteWord, _noteDependency.Relation) : _noteWord.Word; ;
        }

        public NoteParticle(NotenizerDependency dependency, TokenType tokenType, int position, bool considerRelationInWordValueMaking = false)
        {
            if (tokenType == TokenType.Unidentified)
                throw new Exception("Can't make NoteParticle from token of type " + tokenType.ToString());

            _tokenType = tokenType;
            _noteDependency = dependency.Clone();
            _noteDependency.TokenType = tokenType;
            _noteDependency.Position = position;
            _noteWord = _tokenType == TokenType.Dependent ? dependency.Dependent : dependency.Governor;
            _noteWordValue = considerRelationInWordValueMaking ? MakeWordConsiderRelation(_noteWord, _noteDependency.Relation) : _noteWord.Word; ;
        }

        public NoteParticle(String noteWordValue, NotenizerDependency dependency, TokenType tokenType)
        {
            _tokenType = tokenType;
            _noteDependency = dependency.Clone();
            _noteDependency.TokenType = tokenType;
            _noteWordValue = noteWordValue;
            _noteWord = tokenType == TokenType.Dependent ? dependency.Dependent : dependency.Governor;
        }

        #endregion Constuctors

        #region Properties

        /// <summary>
        /// Corresponding word.
        /// </summary>
        public NotenizerWord NoteWord
        {
            get { return _noteWord; }
        }

        /// <summary>
        /// Corresponding dependency.
        /// </summary>
        public NotenizerDependency NoteDependency
        {
            get { return _noteDependency; }
        }

        /// <summary>
        /// String represetntation of corresponding word.
        /// </summary>
        public String NoteWordValue
        {
            get { return _noteWordValue; }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Create word with considering special relations.
        /// </summary>
        /// <param name="noteWord"></param>
        /// <param name="relation"></param>
        /// <returns></returns>
        private String MakeWordConsiderRelation(NotenizerWord noteWord, NotenizerRelation relation)
        {
            String word = String.Empty;

            if (relation.ShortName == GrammaticalConstants.NominalModifier)
                word = relation.AdjustedSpecific + NotenizerConstants.WordDelimeter + noteWord.Word;
            else
                word = noteWord.Word;

            return word;
        }

        #endregion Methods
    }
}
