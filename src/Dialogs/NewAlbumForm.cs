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
    public partial class NewAlbumForm : Form
    {
        public string AlbumName { get; set; }

        public NewAlbumForm()
        {
            InitializeComponent();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            AlbumName = nameCtrl.Text;
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(nameCtrl.Text))
            {
                MessageBox.Show("Please specify the album name.");
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
    }
}
