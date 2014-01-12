using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Ruta
{
    public class Album
    {
        static public Album Open(string location)
        {
            var albumDir = Path.GetDirectoryName(location);

            var album = new Album
            {
                Location = location,
                Items = File.ReadAllLines(location)
                            .Skip(1) //version line
                            .Select(x =>
                                    {
                                        var item = AlbumItem.Deserialize(x);
                                        item.AlbumDir = albumDir;
                                        return item;
                                    })
                            .ToArray()
            };
            return album;
        }

        public string GetAlbumThumbnailDirectory()
        {
            try
            {
                return Path.Combine(Path.GetDirectoryName(Location), "thumbnails");
            }
            catch { return null; }
        }

        public void Save()
        {
            var albumThumbnailDir = GetAlbumThumbnailDirectory().EnsureDirectory();

            foreach (AlbumItem image in Items)
            {
                string file = Path.Combine(albumThumbnailDir, image.Name.Replace(" ", "_") + ".thumb.jpg");
                var thumbImage = image.ThumbnailCache as Image;
                if (thumbImage != null)
                {
                    if (!File.Exists(file) || File.GetLastWriteTimeUtc(file) != File.GetLastWriteTimeUtc(image.Location))
                        thumbImage.Save(file);
                }
                else
                {
                    Utils.CreateThumbnailImageUniform(image.Location, file, 220, 140, ImageFormat.Jpeg);
                }
                File.SetLastWriteTimeUtc(image.Thumbnail, File.GetLastWriteTimeUtc(image.Location));
            }

            EnsureSequentialNaming();
            DeleteIrrelevantThumbnailFiles();

            var lines = new List<string>(Items.Select(x => x.Serialize()));
            lines.Insert(0, Assembly.GetExecutingAssembly().GetName().Version.ToString());
            File.WriteAllLines(Location, lines);
        }

        void DeleteIrrelevantThumbnailFiles()
        {
            var allThumbnailFiles = Directory.GetFiles(GetAlbumThumbnailDirectory(), "*");
            var relevantThumbnailFiles = Items.Select(x => x.Thumbnail);

            foreach (string irrelervantFile in allThumbnailFiles.Except(relevantThumbnailFiles))
                try
                {
                    File.Delete(irrelervantFile);
                }
                catch { }
        }

        void EnsureSequentialNaming()
        {
            //renaming files is much faster comparing to overwriting the file content
            int count = 0;
            foreach (AlbumItem image in Items)
            {
                count++;

                string oldFile = image.Thumbnail;
                string newName = "Image " + count;
                string newFile = image.GenerateThumbnailNameFor("_pending_" + newName);
                image.Name = newName;

                File.Move(oldFile, newFile);
            }

            foreach (string file in Directory.GetFiles(GetAlbumThumbnailDirectory(), "_pending_*"))
            {
                string newFile = Path.Combine(Path.GetDirectoryName(file),
                                              Path.GetFileName(file).Substring("_pending_".Length));
                File.Move(file, newFile);
            }
        }

        public string Location { get; set; }

        public AlbumItem[] Items { get; set; }

        public string GetAlbumDirectory()
        {
            try
            {
                return Path.GetDirectoryName(Location);
            }
            catch { return null; }
        }

        public string GetGaleryPageFile()
        {
            try
            {
                return Path.Combine(Path.GetDirectoryName(Location), "index.html");
            }
            catch { return null; }
        }

        public string Name
        {
            get
            {
                try
                {
                    return Path.GetFileName(Path.GetDirectoryName(Location));
                }
                catch { return null; }
            }
        }

        public override string ToString()
        {
            return Name;
        }
    }
}