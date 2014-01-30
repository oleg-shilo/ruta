using Ruta.Dialogs;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Windows.Forms;

/*
 TODO
 - after auto-scrolling reposition items to make the selected item at the cursor position
  */

namespace Ruta
{
    public partial class View : Form
    {
        int itemHeight;
        int imageHeight;
        int imageWidth;
        int itemPadding;

        static SizeF _ThumbnailImageSize = new SizeF(220, 140);
        static int _itemHeight = 80;
        static int _itemPadding = 6;
        static int _imageHeight = _itemHeight - _itemPadding * 2;
        static float aspectRatio = (_ThumbnailImageSize.Height / _ThumbnailImageSize.Width);
        static int _imageWidth = (int)(_itemHeight / aspectRatio);

        bool IsDragging = false;
        bool IsScrolling = false;
        Pen placementPan = new Pen(Brushes.Red, 2);
        Pen framePan = new Pen(Brushes.Beige, 2);

        void ScaleItems(float scalingFactor)
        {
            var newPadding = (int)(_itemPadding * scalingFactor);
            itemPadding = Math.Max(5, Math.Min(8, newPadding));

            itemHeight = (int)(_itemHeight * scalingFactor);
            imageHeight = (int)(itemHeight - itemPadding * 2);
            imageWidth = (int)((int)(imageHeight / aspectRatio));
        }

        public View()
        {
            InitializeComponent();

            toolStrip1.BackColor = Color.FromArgb(0x44, 0x44, 0x44);
            albumPath.Text = null;

            this.SafeExecute(() =>
            {
                ScaleItems(1f);
            });

            openRootDirButton.BindToShortcut(Keys.O, Keys.Control);
            addImageButton.BindToShortcut(Keys.Add, Keys.Control);
            addAlbumButton.BindToShortcut(Keys.N, Keys.Control);
            saveButton.BindToShortcut(Keys.S, Keys.Control);
            playAlbumButton.BindToShortcut(Keys.F5, Keys.None);
            moveDownButton.BindToShortcut(Keys.Down, Keys.Control);
            moveUpButton.BindToShortcut(Keys.Up, Keys.Control);
        }

        void MainView_Load(object sender, EventArgs e)
        {
            this.SafeExecute(Cursors.WaitCursor, () =>
                {
                    LoadAlbums();
                    DoPropertyGridLayout();
                    UpdateControls();
                });
        }

        void LoadAlbums()
        {
            this.SafeExecute(Cursors.WaitCursor, () =>
            {
                albumsList.Items.Clear();
                albumContent.Items.Clear();
                SyncDetails();

                albumsList.Items.AddRange(Global.Repository.GetAlbums().OrderBy(x => x.Name).ToArray());
                if (albumsList.Items.Count > 0)
                    albumsList.SelectedIndex = 0;
            });
        }

        Point dragStartPoint;

        void albumContent_MouseMove(object sender, MouseEventArgs e)
        {
            if (!IsDragging && e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                if (albumContent.SelectedItem != null)
                {
                    StartDragging();
                }
            }
        }

        void ReportVisibleItems()
        {
            Tuple<int, int> visibleItems = albumContent.GetVisibleItems();

            string start = (visibleItems.Item1 != -1) ? (albumContent.Items[visibleItems.Item1] as AlbumItem).Name : "-1";
            string end = (visibleItems.Item2 != -1) ? (albumContent.Items[visibleItems.Item2] as AlbumItem).Name : "-1";

            //Debug.WriteLine("Visible Items: {0} ... {1}", start, visibleItems.Item2);
        }

        struct DragContext
        {
            public int MouseOverItemIndex;
            public int TopVisiobleItemIndex;
            public int BottomVisiobleItemIndex;
            public Point MousePosition;
            public bool TopPlacement;
        }

        DragContext dragContext;

        void albumContent_DragOver(object sender, DragEventArgs e)
        {
            this.SafeExecute(() =>
            {
                IsDragging = true;

                var point = albumContent.PointToClient(Cursor.Position);

                if (dragContext.MousePosition == point)
                    return;

                dragContext.MousePosition = point;

                int index = albumContent.IndexFromPoint(point);
                if (index != -1 && albumContent.Items.Count > 0)
                {
                    AlbumItem itemMouseOver = albumContent.Items[index] as AlbumItem;

                    if (itemMouseOver == null)
                        return;

                    Tuple<int, int> visibleItems = albumContent.GetVisibleItems();

                    if (visibleItems.Item1 == -1)
                        return;

                    if (point.Y > (albumContent.ClientRectangle.Bottom - itemHeight / 2) && albumContent.TopIndex < (albumContent.Items.Count - 2))
                    {
                        IsScrolling = true;
                        albumContent.ScrollDown();
                        return;
                    }

                    if (point.Y < (itemHeight / 2) && albumContent.TopIndex > 0)
                    {
                        IsScrolling = true;
                        albumContent.ScrollUp();
                        return;
                    }

                    if (IsScrolling)
                    {
                        IsScrolling = false;
                        albumContent.Invalidate();
                    }

                    bool placeAbove = IsTopPlacement(itemMouseOver, point);

                    if (dragContext.TopPlacement == placeAbove && dragContext.TopVisiobleItemIndex == visibleItems.Item1 && dragContext.BottomVisiobleItemIndex == visibleItems.Item2 && dragContext.MouseOverItemIndex == index)
                        return; //nothing changed since the last repainting

                    var g = albumContent.CreateGraphics();

                    // ReportVisibleItems();

                    dragContext.MouseOverItemIndex = index;
                    dragContext.TopPlacement = placeAbove;
                    dragContext.TopVisiobleItemIndex = visibleItems.Item1;
                    dragContext.BottomVisiobleItemIndex = visibleItems.Item2;

                    int count = 0;
                    for (int i = visibleItems.Item1; i <= visibleItems.Item2; i++)
                    {
                        if (albumContent.Items.Count > i)
                        {
                            var item = albumContent.Items[i] as AlbumItem;
                            ErasePlacement(item, g);
                            Debug.WriteLine("ErasePlacement - " + i);
                            count++;
                        }
                    }

                    Point start, end;
                    if (placeAbove)
                        GetTopLinePoints(itemMouseOver.ViewContext, out start, out end);
                    else
                        GetBottomLinePoints(itemMouseOver.ViewContext, out start, out end);

                    g.DrawLine(placementPan, start, end);

                    e.Effect = DragDropEffects.Move;
                }
            });
        }

        void ErasePlacements()
        {
            var g = albumContent.CreateGraphics();
            Tuple<int, int> visibleItems = albumContent.GetVisibleItems();

            for (int i = visibleItems.Item1; i < visibleItems.Item2 && i < albumContent.Items.Count - 1; i++)
                ErasePlacement(albumContent.Items[i] as AlbumItem, g);
        }

        void ErasePlacement(AlbumItem item, Graphics g)
        {
            //g.DrawRectangle(framePan, item.ViewContext);
            DrawHorizontalEdges(g, item.ViewContext, framePan);
        }

        bool IsTopPlacement(AlbumItem itemMouseOver, Point point)
        {
            return (itemMouseOver.ViewContext.Y + itemMouseOver.ViewContext.Height / 2) > point.Y;
        }

        int GetItemIndex(AlbumItem item)
        {
            for (int i = 0; i < albumContent.Items.Count; i++)
                if (item == albumContent.Items[i])
                    return i;
            return -1;
        }

        void GetTopLinePoints(Rectangle r, out Point start, out Point end)
        {
            start = r.Location;
            end = start;
            end.X += r.Width;
        }

        void GetBottomLinePoints(Rectangle r, out Point start, out Point end)
        {
            start = r.Location;
            start.Y += r.Height;
            end = start;
            end.X += r.Width;
        }

        void albumContent_DragDrop(object sender, DragEventArgs e)
        {
            const string unassigned = "<unassigned>";
            IsScrolling = false;
            StopDraggng();

            bool invalidImagesDetected = false;

            if (albumsList.Items.Count == 0 || albumsList.SelectedIndex == -1)
            {
                MessageBox.Show("Create and/or select the album first.", "Ruta");
            }
            else
                this.SafeExecute(() =>
                    {
                        try
                        {
                            var album = (albumsList.SelectedItem as Album);

                            var data = new List<AlbumItem>();

                            var dragData = (AlbumItem)e.Data.GetData(typeof(AlbumItem));

                            if (dragData == null)
                            {
                                if (e.Data is DataObject && ((DataObject)e.Data).ContainsFileDropList())
                                {
                                    var files = new List<string>();
                                    foreach (string filePath in ((DataObject)e.Data).GetFileDropList())
                                    {
                                        if (Directory.Exists(filePath))
                                            files.AddRange(Directory.GetFiles(filePath));
                                        else
                                            files.Add(filePath);
                                    }

                                    var newImages = files.Where(file => file.IsImageFile())
                                                         .Select(file => new AlbumItem
                                                                 {
                                                                     AlbumDir = album.GetAlbumDirectory(),
                                                                     Location = file,
                                                                     Name = unassigned,
                                                                     Title = ""
                                                                 });
                                    data.AddRange(newImages);

                                    if (newImages.Count() != files.Count())
                                        invalidImagesDetected = true;
                                }
                            }
                            else
                                data.Add(dragData);

                            var point = albumContent.PointToClient(new Point(e.X, e.Y));
                            int index = albumContent.IndexFromPoint(point);

                            if (index < 0)
                                index = albumContent.Items.Count - 1;

                            AlbumItem itemMouseOver = null;
                            if (albumContent.Items.Count > 0)
                                itemMouseOver = albumContent.Items[index] as AlbumItem;

                            bool placeAbove = true;
                            if (itemMouseOver != null)
                                placeAbove = IsTopPlacement(itemMouseOver, point);

                            if (itemMouseOver == data.First())
                                return;

                            foreach (var item in data)
                                albumContent.Items.Remove(item);

                            index = GetItemIndex(itemMouseOver);

                            if (!placeAbove)
                                index++;

                            index = Math.Max(index, 0);

                            albumContent.SelectedItems.Clear();
                            data.Reverse();
                            foreach (var item in data)
                            {
                                if (item.Name == unassigned)
                                    item.Name = GenerateNewNextImageName(); //important to do now as GenerateNewNextImageName is based on the analysis of the items already in the abum

                                albumContent.Items.Insert(index, item);
                                albumContent.SelectedItems.Add(item);
                            }

                            IsModified = true;
                        }
                        catch { }
                    });

            if (invalidImagesDetected)
                MessageBox.Show("Some of the input files are not valid image files", "Ruta");

            albumContent.Focus();
        }

        Album lastSelectedAlbum;
        bool ignoreNextAlbumSelection;

        void albumsList_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.SafeExecute(Cursors.WaitCursor, () =>
           {
               if (ignoreNextAlbumSelection)
               {
                   ignoreNextAlbumSelection = false;
                   return;
               }

               if (IsModified)
               {
                   var response = MessageBox.Show("Save album '" + albumsList.Text + "'?", "Ruta", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                   if (DialogResult.Yes == response)
                   {
                       this.SafeExecute(() =>
                       {
                           if (lastSelectedAlbum != null)
                           {
                               //note album selection has already happened but the album content is not
                               //updated yet and it still corresponds to the previously selected album
                               lastSelectedAlbum.Items = albumContent.Items.ToArray<AlbumItem>();
                               lastSelectedAlbum.Save();
                               IsModified = false;
                           }
                       });
                   }
                   else if (DialogResult.Cancel == response)
                   {
                       ignoreNextAlbumSelection = true;
                       albumsList.SelectedItem = lastSelectedAlbum; //restore the original selection
                       return;
                   }
               }

               albumContent.Items.Clear();
               if (albumsList.SelectedItem != null)
               {
                   var album = albumsList.SelectedItem as Album;
                   lastSelectedAlbum = album;

                   Settings.LastRoot = Global.Repository.RootDirectory;
                   Settings.LastAlbum = album.ToString();
                   Settings.Save();

                   albumContent.Items.AddRange(album.Items.ToArray());

                   album.Items.ToList().ForEach(x => x.OnTitleChanged = item =>
                                                                        {
                                                                            IsModified = true;
                                                                            UpdateTitle(item);
                                                                        });
                   if (album.Items.Any())
                       albumContent.SelectedIndex = 0;
                   albumPath.Text = album.GetAlbumDirectory();
               }

               SyncDetails();
               IsModified = false;
           });
        }

        void UpdateTitle(AlbumItem item)
        {
            DrawTitle(albumContent.CreateGraphics(), item.ViewContext, item, true);
        }

        void DrawTitle(Graphics g, Rectangle bounds, AlbumItem item, bool redrawBackground)
        {
            var rect = bounds;
            int xOffset = imageWidth + itemPadding * 2;
            int titlePadding = itemPadding;
            rect.Offset(xOffset + titlePadding / 2, 0);
            rect.Width -= xOffset + titlePadding;
            rect.Inflate(0, -10);

            if (redrawBackground)
                g.FillRectangle(backColor, rect);

            StringFormat fmt = new StringFormat(StringFormatFlags.FitBlackBox);

            Font f = albumContent.Font;
            string text = item.Title;

            if (string.IsNullOrEmpty(item.Title))
            {
                text = "< title is not specified >";
                f = new Font(f, FontStyle.Italic | FontStyle.Regular);
            }
            else if (item.Title == Repository.PrevImageTitleTag)
            {
                text = "< same as previous image title >";
                f = new Font(f, FontStyle.Italic | FontStyle.Regular);
            }

            g.DrawString(text, f, Brushes.White, rect, fmt);
        }

        void saveToolStripButton_Click(object sender, EventArgs e)
        {
            this.SafeExecute(Cursors.WaitCursor, () =>
            {
                if (albumsList.SelectedItem != null)
                {
                    var album = (albumsList.SelectedItem as Album);
                    album.Items = albumContent.Items.ToArray<AlbumItem>();
                    album.Save();
                }
            });
        }

        Brush backColor = new SolidBrush(Color.FromArgb(0x33, 0x33, 0x33));

        void DrawHorizontalEdges(Graphics g, Rectangle r, Pen p)
        {
            g.DrawLine(p, new Point(r.X, r.Y), new Point(r.X + r.Width, r.Y));
            g.DrawLine(p, new Point(r.X, r.Y + r.Height), new Point(r.X + r.Width, r.Y + r.Height));
        }

        void albumContent_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (e.Index == -1) return;

            e.Graphics.FillRectangle(backColor, e.Bounds);

            try
            {
                var point = albumContent.PointToClient(Cursor.Position);
                int mouseOverIndex = albumContent.IndexFromPoint(point);

                DrawHorizontalEdges(e.Graphics, e.Bounds, framePan);

                if ((e.State & DrawItemState.Selected) == DrawItemState.Selected)
                {
                    var frame = e.Bounds;
                    frame.Width = imageWidth + itemPadding * 2;
                    frame.Inflate(-1, -1);
                    e.Graphics.FillRectangle(Brushes.CornflowerBlue, frame);
                }

                var item = albumContent.Items[e.Index] as AlbumItem;

                item.ViewContext = e.Bounds;

                if (item.ThumbnailCache == null)
                    try
                    {
                        if (!string.IsNullOrEmpty(item.Thumbnail) && File.Exists(item.Thumbnail) && File.GetLastWriteTimeUtc(item.Thumbnail) == File.GetLastWriteTimeUtc(item.Location))
                        {
                            using (Stream buf = new MemoryStream(File.ReadAllBytes(item.Thumbnail)))
                                item.ThumbnailCache = Image.FromStream(buf);
                        }
                        else
                        {
                            item.ThumbnailCache = Utils.CreateThumbnailImageUniform(item.Location, 220, 140, ImageFormat.Jpeg);
                            IsModified = true;
                        }
                    }
                    catch
                    {
                    }

                var image = item.ThumbnailCache as Image;
                var rect = e.Bounds;
                rect.Width = imageWidth;
                rect.Height = imageHeight;
                rect.Y += itemPadding;
                rect.X += itemPadding;

                e.Graphics.DrawImage(image, rect);
                DrawTitle(e.Graphics, e.Bounds, item, false);
            }
            catch (Exception ex)
            {
                e.Graphics.DrawString(ex.Message, albumContent.Font, Brushes.White, e.Bounds);
            }
        }

        void albumContent_MeasureItem(object sender, MeasureItemEventArgs e)
        {
            e.ItemHeight = itemHeight;
        }

        void albumContent_SelectedIndexChanged(object sender, EventArgs e)
        {
            SyncDetails();
        }

        void albumContent_MouseUp(object sender, MouseEventArgs e)
        {
            StopDraggng();
        }

        void View_MouseUp(object sender, MouseEventArgs e)
        {
            StopDraggng();
        }

        void StartDragging()
        {
            IsDragging = true;
            timer1.Enabled = true;
            SyncDetails();
            albumContent.DoDragDrop(albumContent.SelectedItem, DragDropEffects.Move);
        }

        void StopDraggng()
        {
            IsDragging = false;
            ErasePlacements();
            SyncDetails();
        }

        void timer1_Tick(object sender, EventArgs e)
        {
            this.SafeExecute(() =>
           {
               if (IsDragging)
               {
                   if ((Control.MouseButtons & MouseButtons.Left) != MouseButtons.Left)
                   {
                       var point = albumContent.PointToClient(Cursor.Position);
                       if (!albumContent.ClientRectangle.Contains(point))
                       {
                           int threshold = 15;
                           if (Math.Abs(dragStartPoint.X - Cursor.Position.X) > threshold ||
                               Math.Abs(dragStartPoint.Y - Cursor.Position.Y) > threshold)
                           {
                               StopDraggng();
                               timer1.Enabled = false;
                           }
                       }
                   }
               }
           });
        }

        void albumContent_MouseDown(object sender, MouseEventArgs e)
        {
            dragStartPoint = Cursor.Position;
        }

        void albumsList_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (e.Index != -1)
            {
                e.DrawBackground();
                Graphics g = e.Graphics;
                Brush brush = ((e.State & DrawItemState.Selected) == DrawItemState.Selected) ?
                              Brushes.CornflowerBlue : new SolidBrush(e.BackColor);
                g.FillRectangle(brush, e.Bounds);
                e.Graphics.DrawString(albumsList.Items[e.Index].ToString(), e.Font,
                         new SolidBrush(e.ForeColor), e.Bounds, StringFormat.GenericDefault);
                e.DrawFocusRectangle();
            }
        }

        void SyncDetails()
        {
            this.SafeExecute(() =>
           {
               if (!suppressDetailsSync)
                   try
                   {
                       if (albumContent.SelectedIndices.Count == 1)
                       {
                           propertyGrid1.SelectedObject = albumContent.SelectedItem;

                           var item = propertyGrid1.SelectedObject as AlbumItem;
                           if (item != null)
                               pictureBox1.ImageLocation = item.Location;
                           else
                               pictureBox1.ImageLocation = null;
                       }
                       else if (albumContent.SelectedIndices.Count == 0)
                       {
                           propertyGrid1.SelectedObject = null;
                           pictureBox1.ImageLocation = null;
                       }
                   }
                   catch { }
           });
        }

        void propertyGrid1_ClientSizeChanged(object sender, EventArgs e)
        {
        }

        void propertyGrid1_SizeChanged(object sender, EventArgs e)
        {
            DoPropertyGridLayout();
        }

        void DoPropertyGridLayout()
        {
            try
            {
                this.propertyGrid1.MoveSplitterTo(100);
            }
            catch { }
        }

        void albumContent_Resize(object sender, EventArgs e)
        {
            albumContent.Invalidate();
        }

        void albumPath_Click(object sender, EventArgs e)
        {
            try
            {
                Process.Start(albumPath.Text);
            }
            catch { }
        }

        void largeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.SafeExecute(Cursors.WaitCursor, () =>
           {
               ScaleItems(2f);
               ForceLayout();
           });
        }

        void mediumToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.SafeExecute(Cursors.WaitCursor, () =>
           {
               ScaleItems(1f);
               ForceLayout();
           });
        }

        void smallToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.SafeExecute(Cursors.WaitCursor, () =>
           {
               ScaleItems(0.5f);
               ForceLayout();
           });
        }

        void ForceLayout()
        {
            var topIndex = albumContent.TabIndex;
            var selectedItem = albumContent.SelectedItem;
            var items = albumContent.Items.Cast<object>().ToArray();

            albumContent.Items.Clear();
            albumContent.Items.AddRange(items);
            albumContent.SelectedItem = selectedItem;
            albumContent.TopIndex = topIndex;
        }

        void View_KeyDown(object sender, KeyEventArgs e)
        {
            foreach (ToolStripItem button in toolStrip1.Items)
                if (button.Tag != null)
                {
                    var keyArgs = button.Tag as Tuple<Keys, Keys>;

                    if (e.KeyCode == keyArgs.Item1 && e.Modifiers == keyArgs.Item2)
                    {
                        e.Handled = true;
                        button.PerformClick();
                        return;
                    }
                }

            Action<Keys, bool, Action> Handle = (key, modifiersCondition, action) =>
                {
                    if (!e.Handled && e.KeyCode == key && modifiersCondition)
                    {
                        e.Handled = true;
                        action();
                    }
                };

            Handle(Keys.Delete, albumContent.Focused && !e.Control && !e.Shift && !e.Alt,
                  deleteImageButton.PerformClick);

            Handle(Keys.A, e.Control && !e.Shift && !e.Alt,
                  () =>
                  {
                      albumContent.SelectedItems.Clear();
                      foreach (var item in albumContent.Items.ToArray<object>())
                          albumContent.SelectedItems.Add(item);
                  });

            Handle(Keys.Down, !e.Control && !e.Shift && e.Alt,
                   () => albumContent.SelectNextItem());

            Handle(Keys.Up, !e.Control && !e.Shift && e.Alt,
                   () => albumContent.SelectPrevItem());
        }

        bool suppressDetailsSync = false;

        void moveDownButton_Click(object sender, EventArgs e)
        {
            this.SafeExecute(() =>
           {
               suppressDetailsSync = true;

               albumContent.MoveSelectionDown();
               albumContent.Focus();
               IsModified = true;

               suppressDetailsSync = false;
               SyncDetails();
           });
        }

        void moveUpButton_Click(object sender, EventArgs e)
        {
            this.SafeExecute(() =>
              {
                  suppressDetailsSync = true;

                  albumContent.MoveSelectionUp();
                  albumContent.Focus();
                  IsModified = true;

                  suppressDetailsSync = false;
                  SyncDetails();
              });
        }

        void saveButton_Click(object sender, EventArgs e)
        {
            SaveSelectedAlbum();
        }

        void SaveSelectedAlbum()
        {
            this.SafeExecute(Cursors.WaitCursor, () =>
                  {
                      var album = albumsList.SelectedItem as Album;
                      if (album != null)
                      {
                          album.Items = albumContent.Items.ToArray<AlbumItem>();
                          album.Save();

                          if (Settings.GeneratorApp.HasText())
                              Global.RunBuildGalleryPageProcess(Settings.GeneratorApp, album.Location, album.GetGaleryPageFile());
                          else
                              Global.Repository.BuildGalleryPage(album.Location, album.GetGaleryPageFile());

                          IsModified = false;
                      }
                  });
        }

        bool isModified;

        public bool IsModified
        {
            get { return isModified; }
            set
            {
                isModified = value;
                UpdateControls();
                ResetAutosaveTimer();
            }
        }

        void ResetAutosaveTimer()
        {
            if (Settings.AutoSaveIntervalInSeconds > 0)
            {
                autoSaveTimer.Interval = Settings.AutoSaveIntervalInSeconds * 1000;
                autoSaveTimer.Enabled = IsModified;
            }
            else
            {
                autoSaveTimer.Enabled = false;
            }
        }

        void UpdateControls()
        {
            this.Text = "Ruta";
            var selectedAlbum = albumsList.SelectedItem as Album;
            if (selectedAlbum != null)
            {
                this.Text += " - " + albumsList.Text;
                if (IsModified)
                    this.Text += " (modified)";
            }

            if (!Directory.Exists(Global.Repository.RootDirectory))
            {
                saveButton.Enabled =
                addAlbumButton.Enabled =
                deleteAlbumButton.Enabled =
                playAlbumButton.Enabled =
                exportButton.Enabled =
                deleteImageButton.Enabled =
                editImageButton.Enabled =
                addImageButton.Enabled =
                moveUpButton.Enabled =
                moveDownButton.Enabled =
                scaleThumbnailButton.Enabled = false;
            }
            else
            {
                saveButton.Enabled = this.IsModified;

                exportButton.Enabled =
                playAlbumButton.Enabled =
                deleteAlbumButton.Enabled =
                addImageButton.Enabled = albumsList.SelectedItem != null;

                deleteImageButton.Enabled =
                editImageButton.Enabled =
                moveUpButton.Enabled =
                moveDownButton.Enabled =
                scaleThumbnailButton.Enabled = albumContent.SelectedItem != null;
            }
        }

        void View_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (IsModified)
            {
                var response = MessageBox.Show("Save album '" + albumsList.Text + "'?", "Ruta", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                if (DialogResult.Yes == response)
                    saveButton.PerformClick();
                else if (DialogResult.Cancel == response)
                    e.Cancel = true;

                autoSaveTimer.Enabled = false;
            }
        }

        void addAlbumButton_Click(object sender, EventArgs e)
        {
            using (var dialog = new NewAlbumForm())
                if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    var existingAlbums = albumsList.Items.ToArray<Album>();
                    if (existingAlbums.Where(x => string.CompareOrdinal(x.Name, dialog.AlbumName) == 0).Any())
                    {
                        MessageBox.Show("The album with the same name already exists.", "Ruta");
                    }
                    else
                    {
                        var newAlbum = Global.Repository.CreateAlbum(dialog.AlbumName);
                        albumsList.Items.Add(newAlbum);
                        albumsList.SelectedItem = newAlbum;
                    }
                }
        }

        void openRootDirButton_Click(object sender, EventArgs e)
        {
            UserInput.SelectRootDir(
                selectedDirectory =>
                {
                    Global.Repository.RootDirectory = selectedDirectory;
                    LoadAlbums();
                });
        }

        string GenerateNewNextImageName()
        {
            var claimedNames = albumContent.Items.ToArray<AlbumItem>().Select(x => x.Name);

            string newName;

            for (int i = 0; i < int.MaxValue; i++)
            {
                newName = "Image " + i;
                if (!claimedNames.Contains(newName))
                    return newName;
            }

            return Guid.NewGuid().ToString();
        }

        void addImageButton_Click(object sender, EventArgs e)
        {
            this.SafeExecute(() =>
           {
               if (albumsList.Items.Count == 0 || albumsList.SelectedIndex == -1)
               {
                   MessageBox.Show("Create and/or select the album first.", "Ruta");
                   return;
               }

               using (var dialog = new OpenFileDialog())
               {
                   dialog.RestoreDirectory = true;
                   dialog.Multiselect = true;

                   if (dialog.ShowDialog() == DialogResult.OK)
                   {
                       var album = (albumsList.SelectedItem as Album);
                       var newImages = dialog.FileNames
                                             .Where(file => file.IsImageFile())
                                             .Select(file => new AlbumItem
                                                     {
                                                         AlbumDir = album.GetAlbumDirectory(),
                                                         Name = "<unassigned>",
                                                         Location = file,
                                                         Title = ""
                                                     });

                       albumContent.SelectedItems.Clear();
                       foreach (var item in newImages)
                       {
                           item.Name = GenerateNewNextImageName(); //important to do now as GenerateNewNextImageName is based on the analysis of the items already in the abum
                           albumContent.Items.Add(item);
                           albumContent.SelectedItems.Add(item);
                       }

                       IsModified = true;
                       if (newImages.Count() != dialog.FileNames.Count())
                           MessageBox.Show("Some of the input files are not valid image files", "Ruta");
                   }
               }
           });
        }

        private void playButton_Click(object sender, EventArgs e)
        {
            this.SafeExecute(Cursors.WaitCursor, () =>
            {
                if (albumsList.SelectedItem != null)
                {
                    var album = albumsList.SelectedItem as Album;
                    string webPage = album.GetGaleryPageFile();

                    if (IsModified || !File.Exists(webPage))
                        SaveSelectedAlbum();

                    if (!IsModified) //successfully saved
                    {
                        if (File.Exists(webPage))
                            Process.Start(webPage);
                    }
                }
            });
        }

        private void deleteImageButton_Click(object sender, EventArgs e)
        {
            this.SafeExecute(() =>
           {
               if (albumContent.SelectedIndex != -1)
               {
                   var index = albumContent.SelectedIndex;
                   foreach (var item in albumContent.SelectedItems.ToArray<object>())
                       albumContent.Items.Remove(item);

                   albumContent.SafeSelectItemAt(index);

                   albumContent.Focus();
                   IsModified = true;
               }
           });
        }

        private void deleteAlbumButton_Click(object sender, EventArgs e)
        {
            if (albumsList.SelectedItem != null)
            {
                var album = albumsList.SelectedItem as Album;

                if (DialogResult.Yes == MessageBox.Show("Delete '" + album.Name + "' album?", "Ruta", MessageBoxButtons.YesNo, MessageBoxIcon.Question))
                {
                    bool deleted = Global.Repository.DeleteAlbum(album.Name);
                    var index = albumsList.SelectedIndex;
                    albumsList.Items.RemoveAt(index);
                    albumsList.SafeSelectItemAt(index);

                    if (!deleted)
                        MessageBox.Show("Some files/directories were not deleted. You may need to delete them manually", "Ruta");
                }
            }
        }

        private void exportButton_Click(object sender, EventArgs e)
        {
            this.SafeExecute(Cursors.WaitCursor, () =>
                {
                    if (albumsList.SelectedItem != null)
                    {
                        SaveSelectedAlbum();

                        var album = albumsList.SelectedItem as Album;

                        UserInput.SelectExportDir(
                            dir =>
                            {
                                this.WithWaitCursor(() =>
                                    {
                                        if (Settings.GeneratorApp.HasText())
                                            Global.RunExportGalleryProcess(Settings.GeneratorApp, album.Location, dir);
                                        else
                                            Global.Repository.Export(album, dir);

                                        if (Directory.Exists(dir))
                                            Process.Start(Path.Combine(dir, album.Name));
                                    });
                            });
                    }
                });
        }

        private void settingsButton_Click(object sender, EventArgs e)
        {
            SettingsForm.ShowDialog();
            ResetAutosaveTimer();
        }

        private void autoSaveTimer_Tick(object sender, EventArgs e)
        {
            this.SafeExecute(() =>
                {
                    if (IsModified)
                        SaveSelectedAlbum();
                });
        }

        private void albumContent_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data != null && ((DataObject)e.Data).ContainsFileDropList())
                e.Effect = DragDropEffects.Move;
        }

        private void editImageButton_Click(object sender, EventArgs e)
        {
            if (albumContent.SelectedItem != null)
                try
                {
                    string appPath = Environment.ExpandEnvironmentVariables(Settings.EditorApp);

                    if (!string.IsNullOrWhiteSpace(appPath) && File.Exists(appPath))
                    {
                        Process.Start(appPath, "\"" + (albumContent.SelectedItem as AlbumItem).Location + "\"");
                        return;
                    }
                }
                catch { }
            MessageBox.Show("Please specify a valid image editor path in the Settings dialog.", "Ruta");
        }

        private void aboutButton_Click(object sender, EventArgs e)
        {
            using (var dialog = new AboutBox())
                dialog.ShowDialog();
        }

        private void helpButton_Click(object sender, EventArgs e)
        {
            try
            {
                Process.Start("https://ruta.codeplex.com/documentation");
            }
            catch { }
        }
    }
}