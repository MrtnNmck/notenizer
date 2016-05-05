using nsConstants;
using nsEnums;
using nsExtensions;
using nsInterfaces;
using nsNotenizerObjects;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nsComponents
{
    /// <summary>
    /// AdvancedLabel with Notenizer data.
    /// </summary>
    public partial class NotenizerAdvancedLabel : AdvancedLabel, INotenizerComponent
    {
        #region Variables

        private NotenizerDependency _dependency;
        private RepresentMode _representMode;
        private bool _isDeletable = true;
        private String _toolTip;

        #endregion Variables

        #region Constructors

        public NotenizerAdvancedLabel(String text)
        {
            InitializeComponent();

            this.Text = text;
        }

        public NotenizerAdvancedLabel(NotenizerDependency dependency) : base()
        {
            this._dependency = dependency;
            this._representMode = RepresentMode.Dependency;

            InitializeComponent();
            Init();
        }

        public NotenizerAdvancedLabel(NotenizerDependency dependency, Font font) : base(font)
        {
            this._dependency = dependency;
            this._representMode = RepresentMode.Dependency;

            InitializeComponent();
            Init();
        }

        #endregion Constuctors

        #region Properties

        /// <summary>
        /// Notenizer dependency.
        /// </summary>
        public NotenizerDependency Dependency
        {
            get { return this._dependency; }
        }

        /// <summary>
        /// Flag if component is deletable.
        /// </summary>
        public Boolean IsDeletable
        {
            get
            {
                return this._isDeletable;
            }

            set
            {
                this._isDeletable = value;
            }
        }

        /// <summary>
        /// Representation mode of component.
        /// </summary>
        public RepresentMode RepresentMode
        {
            set { _representMode = value; }
            get { return _representMode; }
        }

        #endregion Properties

        #region Event Handlers

        #endregion Event Handlers

        #region Methods

        /// <summary>
        /// Initializes NotenizerAdvancedLabel.
        /// </summary>
        public void Init()
        {
            this.Text = this._dependency.CorrespondingWord.Word;

            if (_dependency.CorrespondingWord.NamedEntity.Type != NamedEntityType.Other)
            {
                this.BorderWidth = 2;
                this.BorderColor = ComponentConstants.NamedEntityColors[_dependency.CorrespondingWord.NamedEntity.Type];
                this._toolTip = "Entity: " + _dependency.CorrespondingWord.NamedEntity.Type.ToString();
                ResetToolTip();
            }
        }

        /// <summary>
        /// Resets tooltip.
        /// </summary>
        public void ResetToolTip()
        {
            this.SetToolTip(this._toolTip);
        }

        #endregion Methods
    }
}
