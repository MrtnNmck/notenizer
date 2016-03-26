using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nsNotenizerObjects
{
    public class Match
    {
        private double _structure;
        private double _content;

        public Match()
        {
        }

        public Match(double structure, double content)
        {
            _structure = structure;
            _content = content;
        }

        public double Structure
        {
            get { return Math.Round(_structure); }
            set { _structure = value; }
        }

        public double Content
        {
            get { return Math.Round(_content); }
            set { _content = value; }
        }
    }
}
