using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
