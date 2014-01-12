using System.Windows.Forms;

namespace Ruta
{
    partial class View
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(View));
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.albumsList = new System.Windows.Forms.ListBox();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.albumPath = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.albumContent = new System.Windows.Forms.ListBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.propertyGrid1 = new System.Windows.Forms.PropertyGrid();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.openRootDirButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
            this.addAlbumButton = new System.Windows.Forms.ToolStripButton();
            this.deleteAlbumButton = new System.Windows.Forms.ToolStripButton();
            this.saveButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator9 = new System.Windows.Forms.ToolStripSeparator();
            this.exportButton = new System.Windows.Forms.ToolStripButton();
            this.playAlbumButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator7 = new System.Windows.Forms.ToolStripSeparator();
            this.addImageButton = new System.Windows.Forms.ToolStripButton();
            this.deleteImageButton = new System.Windows.Forms.ToolStripButton();
            this.editImageButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator8 = new System.Windows.Forms.ToolStripSeparator();
            this.moveUpButton = new System.Windows.Forms.ToolStripButton();
            this.moveDownButton = new System.Windows.Forms.ToolStripButton();
            this.scaleThumbnailButton = new System.Windows.Forms.ToolStripDropDownButton();
            this.largeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mediumToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.smallToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.settingsButton = new System.Windows.Forms.ToolStripButton();
            this.helpButton = new System.Windows.Forms.ToolStripButton();
            this.aboutButton = new System.Windows.Forms.ToolStripButton();
            this.autoSaveTimer = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer1.Location = new System.Drawing.Point(0, 28);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.albumsList);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.splitContainer2);
            this.splitContainer1.Size = new System.Drawing.Size(1089, 474);
            this.splitContainer1.SplitterDistance = 122;
            this.splitContainer1.TabIndex = 3;
            // 
            // albumsList
            // 
            this.albumsList.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.albumsList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.albumsList.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.albumsList.ForeColor = System.Drawing.Color.White;
            this.albumsList.FormattingEnabled = true;
            this.albumsList.Location = new System.Drawing.Point(0, 0);
            this.albumsList.Name = "albumsList";
            this.albumsList.Size = new System.Drawing.Size(122, 474);
            this.albumsList.Sorted = true;
            this.albumsList.TabIndex = 0;
            this.albumsList.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.albumsList_DrawItem);
            this.albumsList.SelectedIndexChanged += new System.EventHandler(this.albumsList_SelectedIndexChanged);
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.albumPath);
            this.splitContainer2.Panel1.Controls.Add(this.label1);
            this.splitContainer2.Panel1.Controls.Add(this.albumContent);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.pictureBox1);
            this.splitContainer2.Panel2.Controls.Add(this.propertyGrid1);
            this.splitContainer2.Size = new System.Drawing.Size(963, 474);
            this.splitContainer2.SplitterDistance = 399;
            this.splitContainer2.TabIndex = 0;
            // 
            // albumPath
            // 
            this.albumPath.AutoSize = true;
            this.albumPath.Cursor = System.Windows.Forms.Cursors.Hand;
            this.albumPath.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.albumPath.ForeColor = System.Drawing.Color.DodgerBlue;
            this.albumPath.Location = new System.Drawing.Point(83, 4);
            this.albumPath.Name = "albumPath";
            this.albumPath.Size = new System.Drawing.Size(35, 13);
            this.albumPath.TabIndex = 1;
            this.albumPath.Text = "label1";
            this.albumPath.Click += new System.EventHandler(this.albumPath_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(2, 4);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(83, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Album Location:";
            // 
            // albumContent
            // 
            this.albumContent.AllowDrop = true;
            this.albumContent.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.albumContent.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.albumContent.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.albumContent.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.albumContent.FormattingEnabled = true;
            this.albumContent.Location = new System.Drawing.Point(0, 20);
            this.albumContent.Name = "albumContent";
            this.albumContent.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.albumContent.Size = new System.Drawing.Size(397, 454);
            this.albumContent.TabIndex = 0;
            this.albumContent.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.albumContent_DrawItem);
            this.albumContent.MeasureItem += new System.Windows.Forms.MeasureItemEventHandler(this.albumContent_MeasureItem);
            this.albumContent.SelectedIndexChanged += new System.EventHandler(this.albumContent_SelectedIndexChanged);
            this.albumContent.DragDrop += new System.Windows.Forms.DragEventHandler(this.albumContent_DragDrop);
            this.albumContent.DragEnter += new System.Windows.Forms.DragEventHandler(this.albumContent_DragEnter);
            this.albumContent.DragOver += new System.Windows.Forms.DragEventHandler(this.albumContent_DragOver);
            this.albumContent.MouseDown += new System.Windows.Forms.MouseEventHandler(this.albumContent_MouseDown);
            this.albumContent.MouseMove += new System.Windows.Forms.MouseEventHandler(this.albumContent_MouseMove);
            this.albumContent.MouseUp += new System.Windows.Forms.MouseEventHandler(this.albumContent_MouseUp);
            this.albumContent.Resize += new System.EventHandler(this.albumContent_Resize);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBox1.Location = new System.Drawing.Point(0, 2);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(557, 321);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 1;
            this.pictureBox1.TabStop = false;
            // 
            // propertyGrid1
            // 
            this.propertyGrid1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.propertyGrid1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(68)))), ((int)(((byte)(68)))), ((int)(((byte)(68)))));
            this.propertyGrid1.HelpBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(68)))), ((int)(((byte)(68)))), ((int)(((byte)(68)))));
            this.propertyGrid1.HelpForeColor = System.Drawing.Color.White;
            this.propertyGrid1.Location = new System.Drawing.Point(0, 329);
            this.propertyGrid1.Name = "propertyGrid1";
            this.propertyGrid1.PropertySort = System.Windows.Forms.PropertySort.NoSort;
            this.propertyGrid1.Size = new System.Drawing.Size(560, 143);
            this.propertyGrid1.TabIndex = 0;
            this.propertyGrid1.ToolbarVisible = false;
            this.propertyGrid1.ClientSizeChanged += new System.EventHandler(this.propertyGrid1_ClientSizeChanged);
            this.propertyGrid1.SizeChanged += new System.EventHandler(this.propertyGrid1_SizeChanged);
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // toolStrip1
            // 
            this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openRootDirButton,
            this.toolStripSeparator6,
            this.addAlbumButton,
            this.deleteAlbumButton,
            this.saveButton,
            this.toolStripSeparator9,
            this.exportButton,
            this.playAlbumButton,
            this.toolStripSeparator7,
            this.addImageButton,
            this.deleteImageButton,
            this.editImageButton,
            this.toolStripSeparator8,
            this.moveUpButton,
            this.moveDownButton,
            this.scaleThumbnailButton,
            this.toolStripSeparator1,
            this.settingsButton,
            this.helpButton,
            this.aboutButton});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.toolStrip1.Size = new System.Drawing.Size(1089, 25);
            this.toolStrip1.TabIndex = 4;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // openRootDirButton
            // 
            this.openRootDirButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.openRootDirButton.Image = ((System.Drawing.Image)(resources.GetObject("openRootDirButton.Image")));
            this.openRootDirButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.openRootDirButton.Name = "openRootDirButton";
            this.openRootDirButton.Size = new System.Drawing.Size(23, 22);
            this.openRootDirButton.Text = "toolStripButton1";
            this.openRootDirButton.ToolTipText = "Load albums";
            this.openRootDirButton.Click += new System.EventHandler(this.openRootDirButton_Click);
            // 
            // toolStripSeparator6
            // 
            this.toolStripSeparator6.Name = "toolStripSeparator6";
            this.toolStripSeparator6.Size = new System.Drawing.Size(6, 25);
            // 
            // addAlbumButton
            // 
            this.addAlbumButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.addAlbumButton.Image = ((System.Drawing.Image)(resources.GetObject("addAlbumButton.Image")));
            this.addAlbumButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.addAlbumButton.Name = "addAlbumButton";
            this.addAlbumButton.Size = new System.Drawing.Size(23, 22);
            this.addAlbumButton.Text = "toolStripButton1";
            this.addAlbumButton.ToolTipText = "Create new album";
            this.addAlbumButton.Click += new System.EventHandler(this.addAlbumButton_Click);
            // 
            // deleteAlbumButton
            // 
            this.deleteAlbumButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.deleteAlbumButton.Image = ((System.Drawing.Image)(resources.GetObject("deleteAlbumButton.Image")));
            this.deleteAlbumButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.deleteAlbumButton.Name = "deleteAlbumButton";
            this.deleteAlbumButton.Size = new System.Drawing.Size(23, 22);
            this.deleteAlbumButton.Text = "deleteAlbumButton";
            this.deleteAlbumButton.ToolTipText = "Delete selected album";
            this.deleteAlbumButton.Click += new System.EventHandler(this.deleteAlbumButton_Click);
            // 
            // saveButton
            // 
            this.saveButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.saveButton.Image = ((System.Drawing.Image)(resources.GetObject("saveButton.Image")));
            this.saveButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.saveButton.Name = "saveButton";
            this.saveButton.Size = new System.Drawing.Size(23, 22);
            this.saveButton.Text = "Save selected album";
            this.saveButton.Click += new System.EventHandler(this.saveButton_Click);
            // 
            // toolStripSeparator9
            // 
            this.toolStripSeparator9.Name = "toolStripSeparator9";
            this.toolStripSeparator9.Size = new System.Drawing.Size(6, 25);
            // 
            // exportButton
            // 
            this.exportButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.exportButton.Image = ((System.Drawing.Image)(resources.GetObject("exportButton.Image")));
            this.exportButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.exportButton.Name = "exportButton";
            this.exportButton.Size = new System.Drawing.Size(23, 22);
            this.exportButton.Text = "toolStripButton1";
            this.exportButton.ToolTipText = "Export selected album.\r\nUse this option to prepare the whole album for the deploy" +
    "ment on another PC.";
            this.exportButton.Click += new System.EventHandler(this.exportButton_Click);
            // 
            // playAlbumButton
            // 
            this.playAlbumButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.playAlbumButton.Image = ((System.Drawing.Image)(resources.GetObject("playAlbumButton.Image")));
            this.playAlbumButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.playAlbumButton.Name = "playAlbumButton";
            this.playAlbumButton.Size = new System.Drawing.Size(23, 22);
            this.playAlbumButton.Text = "play";
            this.playAlbumButton.ToolTipText = "Save and play selected album";
            this.playAlbumButton.Click += new System.EventHandler(this.playButton_Click);
            // 
            // toolStripSeparator7
            // 
            this.toolStripSeparator7.Name = "toolStripSeparator7";
            this.toolStripSeparator7.Size = new System.Drawing.Size(6, 25);
            // 
            // addImageButton
            // 
            this.addImageButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.addImageButton.Image = ((System.Drawing.Image)(resources.GetObject("addImageButton.Image")));
            this.addImageButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.addImageButton.Name = "addImageButton";
            this.addImageButton.Size = new System.Drawing.Size(23, 22);
            this.addImageButton.Text = "toolStripButton2";
            this.addImageButton.ToolTipText = "Add image(s)";
            this.addImageButton.Click += new System.EventHandler(this.addImageButton_Click);
            // 
            // deleteImageButton
            // 
            this.deleteImageButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.deleteImageButton.Image = ((System.Drawing.Image)(resources.GetObject("deleteImageButton.Image")));
            this.deleteImageButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.deleteImageButton.Name = "deleteImageButton";
            this.deleteImageButton.Size = new System.Drawing.Size(23, 22);
            this.deleteImageButton.Text = "toolStripButton2";
            this.deleteImageButton.ToolTipText = "Remove selected image(s)";
            this.deleteImageButton.Click += new System.EventHandler(this.deleteImageButton_Click);
            // 
            // editImageButton
            // 
            this.editImageButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.editImageButton.Image = ((System.Drawing.Image)(resources.GetObject("editImageButton.Image")));
            this.editImageButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.editImageButton.Name = "editImageButton";
            this.editImageButton.Size = new System.Drawing.Size(23, 22);
            this.editImageButton.Text = "toolStripButton1";
            this.editImageButton.ToolTipText = "Edit selected image(s) ";
            // 
            // toolStripSeparator8
            // 
            this.toolStripSeparator8.Name = "toolStripSeparator8";
            this.toolStripSeparator8.Size = new System.Drawing.Size(6, 25);
            // 
            // moveUpButton
            // 
            this.moveUpButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.moveUpButton.Image = ((System.Drawing.Image)(resources.GetObject("moveUpButton.Image")));
            this.moveUpButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.moveUpButton.Name = "moveUpButton";
            this.moveUpButton.Size = new System.Drawing.Size(23, 22);
            this.moveUpButton.Text = "toolStripButton1";
            this.moveUpButton.ToolTipText = "Move selected image(s) up";
            this.moveUpButton.Click += new System.EventHandler(this.moveUpButton_Click);
            // 
            // moveDownButton
            // 
            this.moveDownButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.moveDownButton.Image = ((System.Drawing.Image)(resources.GetObject("moveDownButton.Image")));
            this.moveDownButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.moveDownButton.Name = "moveDownButton";
            this.moveDownButton.Size = new System.Drawing.Size(23, 22);
            this.moveDownButton.Text = "toolStripButton2";
            this.moveDownButton.ToolTipText = "Move selected image(s) down";
            this.moveDownButton.Click += new System.EventHandler(this.moveDownButton_Click);
            // 
            // scaleThumbnailButton
            // 
            this.scaleThumbnailButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.scaleThumbnailButton.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.largeToolStripMenuItem,
            this.mediumToolStripMenuItem,
            this.smallToolStripMenuItem});
            this.scaleThumbnailButton.Image = ((System.Drawing.Image)(resources.GetObject("scaleThumbnailButton.Image")));
            this.scaleThumbnailButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.scaleThumbnailButton.Name = "scaleThumbnailButton";
            this.scaleThumbnailButton.Size = new System.Drawing.Size(29, 22);
            this.scaleThumbnailButton.Text = "toolStripDropDownButton1";
            this.scaleThumbnailButton.ToolTipText = "Change thumbnail size";
            // 
            // largeToolStripMenuItem
            // 
            this.largeToolStripMenuItem.Name = "largeToolStripMenuItem";
            this.largeToolStripMenuItem.Size = new System.Drawing.Size(119, 22);
            this.largeToolStripMenuItem.Text = "Large";
            this.largeToolStripMenuItem.Click += new System.EventHandler(this.largeToolStripMenuItem_Click);
            // 
            // mediumToolStripMenuItem
            // 
            this.mediumToolStripMenuItem.Name = "mediumToolStripMenuItem";
            this.mediumToolStripMenuItem.Size = new System.Drawing.Size(119, 22);
            this.mediumToolStripMenuItem.Text = "Medium";
            this.mediumToolStripMenuItem.Click += new System.EventHandler(this.mediumToolStripMenuItem_Click);
            // 
            // smallToolStripMenuItem
            // 
            this.smallToolStripMenuItem.Name = "smallToolStripMenuItem";
            this.smallToolStripMenuItem.Size = new System.Drawing.Size(119, 22);
            this.smallToolStripMenuItem.Text = "Small";
            this.smallToolStripMenuItem.Click += new System.EventHandler(this.smallToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // settingsButton
            // 
            this.settingsButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.settingsButton.Image = ((System.Drawing.Image)(resources.GetObject("settingsButton.Image")));
            this.settingsButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.settingsButton.Name = "settingsButton";
            this.settingsButton.Size = new System.Drawing.Size(23, 22);
            this.settingsButton.Text = "toolStripButton1";
            this.settingsButton.ToolTipText = "Edit settings";
            this.settingsButton.Click += new System.EventHandler(this.settingsButton_Click);
            // 
            // helpButton
            // 
            this.helpButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.helpButton.Image = ((System.Drawing.Image)(resources.GetObject("helpButton.Image")));
            this.helpButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.helpButton.Name = "helpButton";
            this.helpButton.Size = new System.Drawing.Size(23, 22);
            this.helpButton.Text = "toolStripButton1";
            this.helpButton.ToolTipText = "Show help";
            // 
            // aboutButton
            // 
            this.aboutButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.aboutButton.Image = ((System.Drawing.Image)(resources.GetObject("aboutButton.Image")));
            this.aboutButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.aboutButton.Name = "aboutButton";
            this.aboutButton.Size = new System.Drawing.Size(23, 22);
            this.aboutButton.Text = "toolStripButton1";
            this.aboutButton.ToolTipText = "About Ruta";
            // 
            // autoSaveTimer
            // 
            this.autoSaveTimer.Tick += new System.EventHandler(this.autoSaveTimer_Tick);
            // 
            // View
            // 
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(68)))), ((int)(((byte)(68)))), ((int)(((byte)(68)))));
            this.ClientSize = new System.Drawing.Size(1089, 502);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.splitContainer1);
            this.DoubleBuffered = true;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.Name = "View";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.View_FormClosing);
            this.Load += new System.EventHandler(this.MainView_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.View_KeyDown);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.View_MouseUp);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel1.PerformLayout();
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        private SplitContainer splitContainer1;
        private ListBox albumsList;
        private SplitContainer splitContainer2;
        private PropertyGrid propertyGrid1;
        private ListBox albumContent;
        #endregion
        private Label albumPath;
        private Timer timer1;
        private PictureBox pictureBox1;
        private ToolStrip toolStrip1;
        private ToolStripButton addAlbumButton;
        private ToolStripDropDownButton scaleThumbnailButton;
        private ToolStripMenuItem largeToolStripMenuItem;
        private ToolStripMenuItem mediumToolStripMenuItem;
        private ToolStripMenuItem smallToolStripMenuItem;
        private ToolStripButton openRootDirButton;
        private ToolStripSeparator toolStripSeparator6;
        private ToolStripButton deleteAlbumButton;
        private ToolStripSeparator toolStripSeparator7;
        private ToolStripButton addImageButton;
        private ToolStripButton deleteImageButton;
        private ToolStripButton editImageButton;
        private ToolStripSeparator toolStripSeparator8;
        private ToolStripButton moveUpButton;
        private ToolStripButton moveDownButton;
        private ToolStripSeparator toolStripSeparator9;
        private ToolStripButton aboutButton;
        private ToolStripButton helpButton;
        public Label label1;
        private ToolStripButton exportButton;
        private ToolStripButton saveButton;
        private ToolStripButton settingsButton;
        private ToolStripSeparator toolStripSeparator1;
        private ToolStripButton playAlbumButton;
        private Timer autoSaveTimer;
    }
}