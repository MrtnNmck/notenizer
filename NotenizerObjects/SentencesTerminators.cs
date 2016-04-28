using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nsNotenizerObjects
{
    public class SentencesTerminators : List<int>
    {
        public SentencesTerminators()
        {
        }

        public SentencesTerminators(List<NotePart> noteParts)
        {
            int last = 0;
            int particlesCount = 0;

            foreach (NotePart notePartLoop in noteParts)
            {
                particlesCount = notePartLoop.InitializedNoteParticles.Count;
                this.Add(last + particlesCount);
                last = particlesCount;
            }
        }

        public SentencesTerminators(List<int> sentenceTerminators)
        {
            this.AddRange(sentenceTerminators);
        }
    }
}
