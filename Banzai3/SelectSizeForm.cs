using System;
using System.Drawing;
using System.Windows.Forms;

namespace Banzai3
{
    public partial class SelectSizeForm : Form
    {
        private SelectSizeForm()
        {
            InitializeComponent();
        }

        private void SelectSize_Load(object sender, EventArgs e)
        {
            Localization.SetLocalName(this);
            Localization.SetLocalName(controlWidth);
            Localization.SetLocalName(controlHeight);
            Localization.SetLocalName(btnOk);
            Localization.SetLocalName(btnCancel);
        }

        public static Size? GetSize(Size? size)
        {
            var form = new SelectSizeForm();
            if (size != null)
            {
                form.controlWidth.Value = size.Value.Width;
                form.controlHeight.Value = size.Value.Height;
            }
            else
            {
                form.controlWidth.Value = 40;
                form.controlHeight.Value = 30;
            }
            var res = form.ShowDialog();
            if (res != DialogResult.OK)
                return null;
            return new Size((int) form.controlWidth.Value, (int) form.controlHeight.Value);
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
        }
    }
}
