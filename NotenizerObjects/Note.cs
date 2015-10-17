using nsConstants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nsNotenizerObjects
{
    public class Note
    {
        private NotenizerSentence _originalSentence;
        private String _note;

        public Note()
        {
        }

        public Note(NotePart firstPart, NotePart secondPart, NotePart thirdPart)
        {
            _note = String.Join(NotenizerConstants.WordDelimeter, firstPart.Value.Trim(), secondPart.Value.Trim(), thirdPart.Value.Trim());
        }

        public NotenizerSentence OriginalSentence
        {
            get { return _originalSentence; }
        }

        public String Value
        {
            get { return _note; }
        }
    }
}
