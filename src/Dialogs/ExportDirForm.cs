using System;
using System.Windows.Forms;

namespace Ruta.Dialogs
{
    public partial class ExportDirForm : Form
    {
        public string SelectedDirectory { get; set; }

        public string InitialDir { get; set; }

        public float ScaleFactor { get; set; }

        public ExportDirForm()
        {
            InitializeComponent();
        }

        private void ExportDirForm_Load(object sender, EventArgs e)
        {
            nameCtrl.Text = InitialDir;
            numericUpDown1.Value = (decimal)ScaleFactor;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            SelectedDirectory = nameCtrl.Text;
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(nameCtrl.Text))
            {
                MessageBox.Show("Please specify the Export Directory.");
            }
            else
            {
                SelectedDirectory = nameCtrl.Text;
                ScaleFactor = (float)numericUpDown1.Value;
                this.DialogResult = System.Windows.Forms.DialogResult.OK;
                Close();
            }
        }

        private void NewAlbumForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Return)
                okButton.PerformClick();
            else if (e.KeyCode == Keys.Escape)
                Close();
        }

        private void browse_Click(object sender, EventArgs e)
        {
            using (var dialog = new FolderBrowserDialog())
            {
                dialog.Description = "Select the directory where the album content will be exported";
                dialog.ShowNewFolderButton = true;
                dialog.SelectedPath = InitialDir;

                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    nameCtrl.Text = dialog.SelectedPath;
                }
            }
        }
    }

    static partial class UserInput
    {
        static public void SelectExportDir(Action<string> handleUserSelection)
        {
            using (var dialog = new ExportDirForm())
            {
                dialog.InitialDir = Settings.LastExport;
                dialog.ScaleFactor = Settings.ScaleFactor;
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    handleUserSelection(dialog.SelectedDirectory);
                    Settings.LastExport = dialog.SelectedDirectory;
                    Settings.ScaleFactor = dialog.ScaleFactor;
                    Settings.Save();
                }
            }
        }
    }
}