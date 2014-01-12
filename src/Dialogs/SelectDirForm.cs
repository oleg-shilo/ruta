using System;
using System.Windows.Forms;

namespace Ruta.Dialogs
{
    public partial class SelectDirForm : Form
    {
        public string SelectedDirectory { get; set; }

        public SelectDirForm()
        {
            InitializeComponent();
        }

        string initialDir;

        public string InitialDir
        {
            set { nameCtrl.Text = initialDir = value; }
        }

        public string Prompt = "Select Directory";

        string displayName = "Directory";

        public string DisplayName
        {
            get { return displayName; }
            set { displayName = value; label1.Text = displayName + ":"; }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            SelectedDirectory = nameCtrl.Text;
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(nameCtrl.Text))
            {
                MessageBox.Show("Please specify the " + DisplayName.ToLower() + ".");
            }
            else
            {
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
                dialog.Description = Prompt;
                dialog.ShowNewFolderButton = true;
                dialog.SelectedPath = initialDir;

                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    nameCtrl.Text = dialog.SelectedPath;
                }
            }
        }
    }

    static partial class UserInput
    {
        static public void SelectRootDir(Action<string> handleUserSelection)
        {
            using (var dialog = new SelectDirForm())
            {
                dialog.InitialDir = Settings.LastRoot;
                dialog.Text = "Load Albums";
                dialog.DisplayName = "Albums Directory";
                dialog.Prompt = "Select root directory where the all album definitions will be stored";
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    handleUserSelection(dialog.SelectedDirectory);
                    Settings.LastRoot = dialog.SelectedDirectory;
                    Settings.Save();
                }
            }
        }
    }
}