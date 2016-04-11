using nsConstants;
using nsExtensions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace nsComponents
{
    public partial class AdvancedProgressBar : ProgressBar
    {
        #region Variables

        #endregion Variables

        #region Constructors

        public AdvancedProgressBar() : base()
        {
            InitializeComponent();
            this.Stop();
        }

        public AdvancedProgressBar(IContainer container)
        {
            container.Add(this);

            InitializeComponent();
        }

        #endregion Constuctors

        #region Properties

        #endregion Properties

        #region Event Handlers

        #endregion Event Hanlders

        #region Methods

        public void Start()
        {
            this.PerformSafely(() => base.MarqueeAnimationSpeed = ComponentConstants.ProgressBarEnabledSpeed);
        }

        public void Stop()
        {
            this.PerformSafely(() => base.MarqueeAnimationSpeed = ComponentConstants.ProgressBarDisabledSpeed);
        }

        public void Reset()
        {
            this.PerformSafely(() => base.Refresh());
        }

        public void StopAndReset()
        {
            this.Stop();
            this.Reset();
        }

        #endregion Methods
    }
}
