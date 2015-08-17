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
                var label = new Label {Text = exist, AutoSize = true};
                panelLibrary.Controls.Add(label);
            }
            UpdateControls();
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
            return form.controlName.Text.Trim();
        }

        private void controlName_TextChanged(object sender, EventArgs e)
        {
            UpdateControls();
        }

        private void UpdateControls()
        {
            //TODO this code is blocking rename of saved cross, it will be fix to unblock
            var textLower = controlName.Text.Trim().ToLower();
            var nameLower = name.Trim().ToLower();
            foreach (var label in panelLibrary.Controls.OfType<Label>())
            {
                label.ForeColor = (label.Text.Trim().ToLower() == nameLower)
                    ? Color.DarkGreen
                    : (label.Text.Trim().ToLower() != textLower)
                        ? SystemColors.WindowText
                        : Color.Red;
            }
            var exist = crossNames.Any(n => textLower == n.Trim().ToLower()) && (textLower != nameLower);
            labelName.BackColor = exist ? SystemColors.Info : SystemColors.Window;
            btnOk.Enabled = !exist && !string.IsNullOrEmpty(textLower);
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            var textLower = controlName.Text.Trim().ToLower();
            if (string.IsNullOrEmpty(textLower))
                return;
            var nameLower = name.Trim().ToLower();
            var exist = crossNames.Any(n => textLower == n.Trim().ToLower()) && (textLower != nameLower);
            if (!exist || (textLower == nameLower))
                DialogResult = DialogResult.OK;
        }
    }
}
