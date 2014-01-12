using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;

namespace Ruta
{
    public class AlbumItem
    {
        public Action<AlbumItem> OnTitleChanged;

        string title;

        [Description("Te image title to be displayed in the Photo Album.\nYou can provide the description relevant to the content of the image.")]
        public string Title
        {
            get { return title; }
            set { title = value; if (OnTitleChanged != null) OnTitleChanged(this); }
        }

        public static AlbumItem Deserialize(string data)
        {
            string[] parts = data.Split('|');

            return new AlbumItem
            {
                Location = parts[0].Unescape(),
                Name = parts[1].Unescape(),
                Title = parts.Skip(2).FirstOrDefault() ?? ""
            };
        }

        public string Serialize()
        {
            return Location.Escape() + "|" + Name.Escape() + "|" + Title.Escape();
        }

        [Description("Location of the image file.")]
        public string Location { get; internal set; }

        public object ThumbnailCache;

        [Description("Location of the image thumbnail file. The thumbnail is generated when the album with the inserted image for the first time.")]
        public string Thumbnail
        {
            get { return GenerateThumbnailNameFor(Name); }
        }

        public string GenerateThumbnailNameFor(string imageName)
        {
            //image_6.thumb.jpg
            return Path.Combine(AlbumDir, "thumbnails\\" + imageName.Replace(" ", "_") + ".thumb.jpg");
        }

        internal string AlbumDir { get; set; }

        [Description("Auto-generated name of the image. This information is never displayed in the Photo Album")]
        public string Name { get; internal set; }

        internal bool IsMouseOver { get; set; }

        internal Rectangle ViewContext { get; set; }

        public override string ToString()
        {
            return Name + ": " + Thumbnail;
        }
    }
}
