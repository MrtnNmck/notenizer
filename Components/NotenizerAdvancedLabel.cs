using nsEnums;
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
    public partial class NotenizerAdvancedLabel : AdvancedLabel, INotenizerComponent
    {
        private NotenizerDependency _dependency;
        private RepresentMode _representMode;
        private bool _isDeletable = true;

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

        public NotenizerDependency Dependency
        {
            get { return this._dependency; }
        }

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

        public RepresentMode RepresentMode
        {
            set { _representMode = value; }
            get { return _representMode; }
        }

        public void Init()
        {
            this.Text = this._dependency.CorrespondingWord.Word;
        }
    }
}
