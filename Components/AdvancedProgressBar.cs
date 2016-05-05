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
    /// <summary>
    /// Advanced progress bar of marquee style.
    /// </summary>
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

        #endregion Event Handlers

        #region Methods

        /// <summary>
        /// Starts infinite marquee progress bar.
        /// </summary>
        public void Start()
        {
            this.PerformSafely(() => base.MarqueeAnimationSpeed = ComponentConstants.ProgressBarEnabledSpeed);
        }

        /// <summary>
        /// Stops progress bar.
        /// </summary>
        public void Stop()
        {
            this.PerformSafely(() => base.MarqueeAnimationSpeed = ComponentConstants.ProgressBarDisabledSpeed);
        }

        /// <summary>
        /// Reset's progress bar.
        /// </summary>
        public void Reset()
        {
            this.PerformSafely(() => base.Refresh());
        }

        /// <summary>
        /// Stops and resets progress bar.
        /// </summary>
        public void StopAndReset()
        {
            this.Stop();
            this.Reset();
        }

        #endregion Methods
    }
}
