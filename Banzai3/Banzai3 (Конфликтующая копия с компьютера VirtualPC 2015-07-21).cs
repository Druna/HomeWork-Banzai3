using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Banzai3
{
    public partial class Banzai3 : Form
    {
        const string DefaultSave = "last.banzai";
        Cross cross;
        
        public Banzai3()
        {
            InitializeComponent();
            cross = new Cross(10, 10);
        }

        private void Banzai3_Load(object sender, EventArgs e)
        {
            var stdFileName = Application.LocalUserAppDataPath + DefaultSave;
            if (File.Exists(stdFileName))
            {
                try
                {
                    cross = new Cross(stdFileName); 
                }
                catch(Exception ex)
                {
                    MessageBox.Show("Error loading last Saved Game" + Environment.NewLine + ex.ToString());
                }
            }
        }
    }
}
