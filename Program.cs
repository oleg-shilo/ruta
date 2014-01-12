using System;
using System.Drawing;
using System.Windows.Forms;
using System.ComponentModel;
using System.IO;
using System.Drawing.Imaging;

namespace Ruta
{
    class Program
    {
        [STAThread]
        static public void Main()
        {
            //foreach (string file in Directory.GetFiles(@"E:\cs-script\cs-scriptWEB\ruta\sample_album\images\original", "*"))
            //{
            //    string tempFile = file.ChangeDirectory(@"E:\cs-script\cs-scriptWEB\ruta\sample_album\images");
            //    Utils.ScaleImage(file, tempFile, 0.5F);
            //    //File.Delete(file);
            //    //File.Move(tempFile, file);
            //}
            //return;

            //Utils.CreateThumbnailImageUniform(@"E:\My Photos\IMAG0036.jpg",
            //                                  @"C:\Users\osh\Documents\Ruta Photo Albums\aewqqw\thumbnails\test.jpg",
            //                                    240, 120);

            //Global.Repository.RootDirectory = @"E:\My Photos\WebAlbums\StaticAlbum";
            //Global.Repository.RootDirectory = @"\\MYBOOKLIVE\Public\photo\albums";
            if (Directory.Exists(Settings.LastRoot))
                Global.Repository.RootDirectory = Settings.LastRoot;

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new View());
        }
    }
}
