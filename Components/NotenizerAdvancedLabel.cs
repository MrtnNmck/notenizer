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

        public NotenizerAdvancedLabel(NotenizerDependency dependency) : base()
        {
            this._dependency = dependency;

            InitializeComponent();
            Init();
        }

        public NotenizerAdvancedLabel(NotenizerDependency dependency, Font font) : base(font)
        {
            this._dependency = dependency;

            InitializeComponent();
            Init();
        }

        public NotenizerDependency Dependency
        {
            get { return this._dependency; }
        }

        public void Init()
        {
            this.Text = this._dependency.CorrespondingWord.Word;
        }
    }
}
