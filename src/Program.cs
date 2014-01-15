using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace Ruta
{
    class Program
    {
        [STAThread]
        static public void Main(string[] args)
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

            if (!RunAsHandleCommandLineApp(args))
            {
                if (Directory.Exists(Settings.LastRoot))
                    Global.Repository.RootDirectory = Settings.LastRoot;

                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new View());
            }
        }

        static bool RunAsHandleCommandLineApp(string[] args)
        {
            if (args.Length > 0)
            {
                Global.HandleErrors(() =>
                    {
                        //<app> -generate <album_file> <output_file>
                        if (args.Length == 3 && args[0] == "-generate")
                        {
                            Global.Repository.BuildGalleryPage(args[1], args[2]);
                        }
                        //<app> -export <album_file> <output_dir>
                        else if (args.Length == 3 && args[0] == "-export")
                        {
                            Global.Repository.ExportGallery(args[1], args[2]);
                        }
                        //? 
                        else
                        {
                        }
                    });
                return true;
            }
            return false;
        }
    }
}
