using nsConstants;
using System;

using nsExtensions;
using System.Collections.Generic;
using nsEnums;

namespace nsNotenizerObjects
{
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

        public void Add(List<NotePart> noteParts)
        {
            foreach (NotePart notePartLoop in noteParts)
                Add(notePartLoop);
        }

        public void Add(NotePart notePart)
        {
            _note += notePart.Value.Trim().CapitalizeSentence() + NotenizerConstants.SentenceDelimeter;
            _noteParts.Add(notePart);
        }

        public void SplitToSentences(List<int> endsIndices)
        {
            int lastIndex = 0;
            List<NotePart> parts = new List<NotePart>();

            foreach (int indexLoop in endsIndices)
            {
                // TODO: docasna podmienka, lebo nie je doriesene rozoznavanie medzi sadami pravidiel
                // a vzdy sa zoberie prva, preto nemusi sediet pre niektoru vetu a tym padom by to tu
                // padlo na ArgumentOutOfRangeException
                if (indexLoop - lastIndex > _noteParts[0].InitializedNoteObjects.Count)
                    return;

                NotePart notePart = new NotePart(_originalSentence);
                notePart.Add(_noteParts[0].InitializedNoteObjects.GetRange(lastIndex, indexLoop - lastIndex));

                lastIndex = indexLoop;
                parts.Add(notePart);
            }

            _note = String.Empty;
            _noteParts = new List<NotePart>();

            Add(parts);
        }
    }
}
