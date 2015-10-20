using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nsNotenizerObjects
{
    public class NoteObject
    {
        private NotenizerWord _noteWord;
        private String _noteWordValue;
        private NotenizerDependency _noteDependency;

        public NoteObject(String noteWordValue, NotenizerWord noteWord, NotenizerDependency noteDependency)
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
    }
}
