using nsConstants;
using System;

using nsExtensions;
using System.Collections.Generic;

namespace nsNotenizerObjects
{
    public enum CreatedBy
    {
        User,
        Notenizer
    }

    public class Note
    {
        private NotenizerSentence _originalSentence;
        private String _note;
        private List<NotePart> _noteParts;
        private CreatedBy _createdBy;

        public Note()
        {
        }

        public Note(NotenizerSentence originalSentence)
        {
            _note = String.Empty;
            _noteParts = new List<NotePart>();
            _originalSentence = originalSentence;
        }

        public NotenizerSentence OriginalSentence
        {
            get { return _originalSentence; }
        }

        public String Value
        {
            get { return _note; }
        }

        public CreatedBy CreatedBy
        {
            get { return _createdBy; }
            set { _createdBy = value; }
        }

        public List<NotePart> NoteParts
        {
            get { return _noteParts; }
        }

        public void Add(NotePart notePart)
        {
            _note += notePart.Value.Trim().CapitalizeSentence() + NotenizerConstants.SentenceDelimeter;
            _noteParts.Add(notePart);
        }
    }
}
