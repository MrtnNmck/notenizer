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
        private double _value;

        public Match()
        {
        }

        public Match(double all)
        {
            _structure = _content = _value = all;
        }

        public Match(double structure, double content, double value)
        {
            _structure = structure;
            _content = content;
            _value = value;
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

        public double Value
        {
            get { return Math.Round(_value); }
            set { _value = value; }
        }

        public override String ToString()
        {
            return String.Format("Structure match: {1}{0}Content match: {2}{0}Value match: {2}{0}", Environment.NewLine, this.Structure, this.Content, this.Value);
        }
    }
}
