using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nsNotenizerObjects
{
    /// <summary>
    /// Match of structures.
    /// </summary>
    public class Match
    {
        #region Variables

        private double _structure;
        private double _content;
        private double _value;

        #endregion Variables

        #region Constructors

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

        #endregion Constuctors

        #region Properties

        /// <summary>
        /// Structure part of match.
        /// </summary>
        public double Structure
        {
            get { return Math.Round(_structure); }
            set { _structure = value; }
        }

        /// <summary>
        /// Content part of match.
        /// </summary>
        public double Content
        {
            get { return Math.Round(_content); }
            set { _content = value; }
        }

        /// <summary>
        /// Value part of match.
        /// </summary>
        public double Value
        {
            get { return Math.Round(_value); }
            set { _value = value; }
        }

        #endregion Properties

        #region Methods

        public override String ToString()
        {
            return String.Format("Structure match: {1}{0}Content match: {2}{0}Value match: {3}{0}", Environment.NewLine, this.Structure, this.Content, this.Value);
        }

        #endregion Methods
    }
}
