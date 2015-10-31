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
         
        public NoteParticle(NotenizerWord noteWord, NotenizerDependency noteDependency)
        {
            _noteWord = noteWord;
            _noteDependency = NoteDependency;
            _noteWordValue = MakeWordConsiderRelation(noteWord, noteDependency.Relation);
        }

        public NoteParticle(String noteWordValue, NotenizerWord noteWord, NotenizerDependency noteDependency)
        {
            _noteWordValue = noteWordValue;
            _noteWord = noteWord;
            _noteDependency = noteDependency;
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

        private String MakeWordConsiderRelation(NotenizerWord noteWord, NotenizerRelation relation)
        {
            String word = String.Empty;

            if (relation.ShortName == GrammaticalConstants.NominalModifier)
                word = relation.Specific + NotenizerConstants.WordDelimeter + noteWord.Word;
            else
                word = noteWord.Word;

            return word;
        }
    }
}
