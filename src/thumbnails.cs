using System.IO;
using System.Text;
using System.Linq;
using System.Drawing;
using System.IO;
using System;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

namespace Ruta
{
    class Utils1
    {
        //static public void Main()
        //{
        //    CreateFolderAlbumDefinition(@"E:\My Photos\Processed\Grampians 2013", @"E:\My Photos\WebAlbums\StaticAlbum\album_1\album_definition.txt");
        //    // CreateThumbnailImageUniform(@"E:\My Photos\Processed\Grampians 2013\IMG_0650.JPG",
        //                         // @"E:\My Photos\WebAlbums\StaticAlbum\album_1\thumbnails\test.jpg",
        //                              // 220, 140, ImageFormat.Jpeg);

        //    // CreateThumbnailImageUniform(@"E:\My Photos\Processed\Grampians 2013\IMG_0654.JPG",
        //                         // @"E:\My Photos\WebAlbums\StaticAlbum\album_1\thumbnails\test2.jpg",
        //                          // 220, 140, ImageFormat.Jpeg);

        //    Console.WriteLine("Done");
        //}

        //public static void CreateFolderAlbumDefinition(string dir, string definitionFile)
        //{
        //    //E:\My Photos\Processed\Grampians 2013\Album\IMG_0650_2.JPG|image 1

        //    int count = 0;
        //    var lines = Directory.GetFiles(dir, "*jpg", SearchOption.TopDirectoryOnly)
        //                         .Select(x =>x+"|image "+(++count))
        //                         .ToArray();
        //    File.WriteAllLines(definitionFile, lines);
        //}

        //public static void CreateThumbnailImage(string fromPath, string toPath, int width, int height, ImageFormat format = null)
        //{
        //    Bitmap srcBmp = new Bitmap(fromPath);
        //    float ratio = (float)srcBmp.Width / (float)srcBmp.Height;
        //    SizeF newSize;
        //    if (srcBmp.Width > srcBmp.Height)
        //        newSize = new SizeF(width, width / ratio);
        //    else
        //        newSize = new SizeF(height * ratio, height);

        //    Bitmap target = new Bitmap((int)newSize.Width, (int)newSize.Height);

        //    using (Graphics graphics = Graphics.FromImage(target))
        //    {
        //        graphics.CompositingQuality = CompositingQuality.HighSpeed;
        //        graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
        //        graphics.CompositingMode = CompositingMode.SourceCopy;
        //        graphics.DrawImage(srcBmp, 0, 0, newSize.Width, newSize.Height);
        //        target.Save(toPath, ImageFormat.Jpeg);
        //    }
        //}

        
    }
}