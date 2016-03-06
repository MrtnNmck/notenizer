using edu.stanford.nlp.trees;
using nsEnums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nsNotenizerObjects
{
    public class NotenizerDependency
    {
        private NotenizerWord _dependent;
        private NotenizerWord _governor;
        private NotenizerRelation _relation;
        private TypedDependency _originalDependency;
        private int _position;
        private ComparisonType _comparisonType;
        private TokenType _tokenType;

        public NotenizerDependency(TypedDependency typedDependency)
        {
            _dependent = new NotenizerWord(typedDependency.dep());
            _governor = new NotenizerWord(typedDependency.gov());
            _relation = new NotenizerRelation(typedDependency.reln());
            _originalDependency = typedDependency;
        }

        public NotenizerDependency(
            NotenizerWord governor,
            NotenizerWord dependent,
            NotenizerRelation relation,
            int position,
            ComparisonType comparisonType,
            TokenType tokenType)
        {
            _governor = governor;
            _dependent = dependent;
            _relation = relation;
            _position = position;
            _comparisonType = comparisonType;
            _tokenType = tokenType;
        }

        public NotenizerWord Dependent
        {
            get { return _dependent; }
        }

        public NotenizerWord Governor
        {
            get { return _governor; }
        }

        public NotenizerRelation Relation
        {
            get { return _relation; }
        }

        public TypedDependency OriginalDependency
        {
            get { return _originalDependency; }
        }

        public ComparisonType ComparisonType
        {
            get { return _comparisonType; }
            set { _comparisonType = value; }
        }

        public String Key
        {
            get { return _relation.ShortName + Governor.Word + Governor.Index + Dependent.Word + Dependent.Index; }
        }

        public int Position
        {
            get { return _position; }
            set { this._position = value; }
        }

        public TokenType TokenType
        {
            get { return _tokenType; }
            set { this._tokenType = value; }
        }

        public NotenizerWord CorrespondingWord
        {
            get { return GetWordByTokenType(this._tokenType); }
        }

        public NotenizerWord GetWordByTokenType(TokenType tokenType)
        {
            return tokenType == TokenType.Dependent ? _dependent : _governor;
        }

        public NotenizerDependency Clone()
        {
            NotenizerDependency clonedDep = new NotenizerDependency(this._originalDependency);
            clonedDep.Position = this._position;
            clonedDep.TokenType = this._tokenType;
            clonedDep.ComparisonType = this._comparisonType;

            return clonedDep;
        }
	}
}
