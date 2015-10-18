using nsConstants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using nsExtensions;

namespace nsNotenizerObjects
{
    public class Note
    {
        private NotenizerSentence _originalSentence;
        private String _note;

        public Note()
        {
        }

        public Note(NotenizerSentence originalSentence)
        {
            _originalSentence = originalSentence;
        }

        public Note(NotePart firstPart, NotePart secondPart, NotePart thirdPart)
        {
            _note = JoinParts(firstPart, secondPart, thirdPart);
        }

        public NotenizerSentence OriginalSentence
        {
            get { return _originalSentence; }
        }

        public String Value
        {
            get { return _note + NotenizerConstants.SentenceFinisher; }
        }

        public void Concat(NotePart firstPart, NotePart secondPart, NotePart thirdPart)
        {
            if (firstPart.Value.IsNullOrEmpty() && secondPart.Value.IsNullOrEmpty() && thirdPart.Value.IsNullOrEmpty())
                return;

            if (_note.IsNullOrEmpty())
                _note = JoinParts(firstPart, secondPart, thirdPart);
            else
                _note += NotenizerConstants.SentenceDelimeter + JoinParts(firstPart, secondPart, thirdPart);
        }

        private String JoinParts(NotePart firstPart, NotePart secondPart, NotePart thirdPart)
        {
            return String.Join(NotenizerConstants.WordDelimeter, firstPart.Value.Trim(), secondPart.Value.Trim(), thirdPart.Value.Trim()).CapitalizeSentence();
        }
    }
}
