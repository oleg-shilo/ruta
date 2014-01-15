using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Ruta.Dialogs
{
    public partial class SettingsForm : Form
    {
        public static void ShowDialog()
        {
            using (Form form = new SettingsForm())
                form.ShowDialog();
        }
        public SettingsForm()
        {
            InitializeComponent();
            autoSaveInterval.Enabled =
            autoSaveCheckBox.Checked = (Settings.AutoSaveIntervalInSeconds > 0);
            autoSaveInterval.Value = Math.Abs(Settings.AutoSaveIntervalInSeconds);
            editorPath.Text = Settings.EditorApp;
            generatorPath.Text = Settings.GeneratorApp;
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            if (autoSaveCheckBox.Checked)
                Settings.AutoSaveIntervalInSeconds = (int)autoSaveInterval.Value;
            else
                Settings.AutoSaveIntervalInSeconds = (int)-autoSaveInterval.Value;

            Settings.EditorApp = editorPath.Text;
            Settings.GeneratorApp = generatorPath.Text;
            Settings.Save();
            Close();
        }

        private void autoSaveCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            autoSaveInterval.Enabled = autoSaveCheckBox.Checked;
        }

        private void SettingsForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
                Close();
            else if (e.KeyCode == Keys.Return)
                okButton.PerformClick();
        }
    }
}
