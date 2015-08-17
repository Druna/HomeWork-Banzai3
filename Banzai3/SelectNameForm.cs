using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Banzai3
{
    public partial class SelectNameForm : Form
    {
        private List<string> crossNames;
        private string name;

        private SelectNameForm()
        {
            InitializeComponent();
        }

        private void SelectNameForm_Load(object sender, EventArgs e)
        {
            Localization.SetLocalName(this);
            Localization.SetLocalName(labelName);
            Localization.SetLocalName(btnOk);
            Localization.SetLocalName(btnCancel);
            foreach (var exist in crossNames)
            {
                var label = new Label {Text = exist};
                panelLibrary.Controls.Add(label);
            }
        }

        public static string SelectName(string dir, string name)
        {
            var form = new SelectNameForm
            {
                crossNames = CrossIO.GetListFiles(dir).ToList(),
                name = name,
                controlName = {Text = name}
            };
            var res = form.ShowDialog();
            if (res != DialogResult.OK)
                return null;
            return form.controlName.Text;
        }

        private void controlName_TextChanged(object sender, EventArgs e)
        {
            //TODO this code block rename of saved cross, it will be unblocked
            foreach (var label in panelLibrary.Controls.OfType<Label>())
            {
                label.ForeColor = (label.Text == name)
                    ? Color.DarkGreen
                    : (label.Text != controlName.Text)
                        ? SystemColors.WindowText
                        : Color.Red;
            }
            var exist = crossNames.Contains(controlName.Text) && (controlName.Text != name);
            labelName.BackColor = exist ? SystemColors.Info : SystemColors.Window;
            btnOk.Enabled = !exist && !string.IsNullOrEmpty(controlName.Text);
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            var text = controlName.Text;
            if (string.IsNullOrEmpty(text))
                return;
            if (!crossNames.Contains(text) || (text == name))
                DialogResult = DialogResult.OK;
        }
    }
}
