using System.Windows.Forms;

namespace Banzai3
{
    public sealed partial class PanelCross : Control
    {
        /// <summary>
        /// double buffering only
        /// </summary>
        public PanelCross()
        {
            InitializeComponent();
            DoubleBuffered = true;
        }
    }
}
