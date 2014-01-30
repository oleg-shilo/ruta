using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security;
using System.Text;
using System.Windows.Forms;

namespace Ruta
{
    static class Global
    {
        static public Repository Repository;

        static Global()
        {
            Global.Repository = new Repository();
        }

        public static void RunBuildGalleryPageProcess(string app, string inputFile, string outputFile)
        {
            RunProcess(app, string.Format("-generate \"{0}\" \"{1}\"", inputFile, outputFile), "save album");
        }

        public static void RunExportGalleryProcess(string app, string inputFile, string outputDir)
        {
            RunProcess(app, string.Format("-export \"{0}\" \"{1}\"", inputFile, outputDir), "export album");
        }

        public static void RunProcess(string app, string args, string context)
        {
            try
            {
                using (var proc = new Process())
                {
                    proc.StartInfo.FileName = app;
                    proc.StartInfo.Arguments = args;
                    proc.StartInfo.UseShellExecute = false;
                    proc.StartInfo.CreateNoWindow = true;
                    proc.Start();
                    proc.WaitForExit();
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Cannot " + context + " with " + app + ": " + e.Message, "Ruta");
            }
        }

        public static void HandleErrors(Action action)
        {
            try
            {
                action();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString(), "Ruta");
            }
        }
    }

    public class Repository
    {
        public string RootDirectory { get; set; }

        public Repository()
        {
            RootDirectory = Path.GetDirectoryName(this.GetType().Assembly.Location);
        }

        public IEnumerable<Album> GetAlbums()
        {
            return Directory.GetFiles(RootDirectory, "album_definition.txt", SearchOption.AllDirectories)
                            .Select(dir => Album.Open(dir));
        }

        public Album CreateAlbum(string name)
        {
            string albumDir = Path.Combine(RootDirectory, name);
            string definitionFile = Path.Combine(albumDir, "album_definition.txt");
            if (!Directory.Exists(albumDir))
                Directory.CreateDirectory(albumDir);

            File.WriteAllText(definitionFile, "");

            return Album.Open(definitionFile);
        }

        public bool DeleteAlbum(string name)
        {
            string albumDir = Path.Combine(RootDirectory, name);

            try
            {
                Directory.Delete(albumDir, true);
            }
            catch { }

            return !Directory.Exists(albumDir);
        }

        void SetupDependencies(string destinationDir)
        {
            string dir = Path.Combine(destinationDir, "bits");
            dir.EnsureDirectory();

            File.WriteAllText(Path.Combine(dir, "jquery.easing.1.3.js"), GalleryTemplates.Resources.jquery_easing_1_3);

            File.WriteAllBytes(Path.Combine(dir, "empty.gif"), GalleryTemplates.Resources.empty_gif);
            File.WriteAllBytes(Path.Combine(dir, "ajax-loader_dark.gif"), GalleryTemplates.Resources.ajax_loader_dark_gif);
            File.WriteAllBytes(Path.Combine(dir, "fs_img_g_bg.png"), GalleryTemplates.Resources.fs_img_g_bg_png);
            File.WriteAllBytes(Path.Combine(dir, "generic_thumbnail.jpg"), GalleryTemplates.Resources.generic_thumbnail_jpg);
            File.WriteAllBytes(Path.Combine(dir, "playImgBtn.png"), GalleryTemplates.Resources.playImgBtn_png);
            File.WriteAllBytes(Path.Combine(dir, "play2ImgBtn.png"), GalleryTemplates.Resources.play2ImgBtn_png);
            File.WriteAllBytes(Path.Combine(dir, "pauseImgBtn.png"), GalleryTemplates.Resources.pauseImgBtn_png);
            File.WriteAllBytes(Path.Combine(dir, "nextImgBtn.png"), GalleryTemplates.Resources.nextImgBtn_png);
            File.WriteAllBytes(Path.Combine(dir, "prevImgBtn.png"), GalleryTemplates.Resources.prevImgBtn_png);
            File.WriteAllBytes(Path.Combine(dir, "toolbar_fs_icon.png"), GalleryTemplates.Resources.toolbar_fs_icon_png);
            File.WriteAllBytes(Path.Combine(dir, "toolbar_n_icon.png"), GalleryTemplates.Resources.toolbar_n_icon_png);
        }

        public string ExportGallery(string definitionFile, string destinationRootDir)
        {
            return Export(Album.Open(definitionFile), destinationRootDir);
        }

        public string Export(Album album, string destinationRootDir)
        {
            var destinationDir = Path.Combine(destinationRootDir, album.Name);

            destinationDir.EnsureDirectory();
            destinationDir.ClearDirectory(false);

            string imagesDir = Path.Combine(destinationDir, "images");
            string bitsDir = Path.Combine(destinationDir, "bits");
            string bitsSrcDir = Path.Combine(Path.GetDirectoryName(album.Location), "bits");
            string thumbnailsDir = Path.Combine(destinationDir, "thumbnails");
            string thumbnailsSrcDir = Path.Combine(Path.GetDirectoryName(album.Location), "thumbnails");

            bitsDir.EnsureDirectory();
            thumbnailsDir.EnsureDirectory();

            foreach (string file in Directory.GetFiles(thumbnailsSrcDir, "*"))
            {
                File.Copy(file, file.ChangeDirectory(thumbnailsDir), true);
            }

            foreach (string file in Directory.GetFiles(bitsSrcDir, "*"))
            {
                File.Copy(file, file.ChangeDirectory(bitsDir), true);
            }

            foreach (AlbumItem item in album.Items)
            {
                imagesDir.EnsureDirectory();
                string destFile = item.Location.ChangeDirectory(imagesDir);
                if (Settings.ScaleFactor != 1)
                    Utils.ScaleImage(item.Location, destFile, Settings.ScaleFactor);
                else
                    File.Copy(item.Location, destFile, true);
            }

            //the WEB page should not be copied but rather regenerated as the original contains invalid (for exporting) path references
            string webPageFile = Path.Combine(destinationDir, "index.html");
            BuildGalleryPage(album.Location, webPageFile, true);
            return webPageFile;

        }

        static public string PrevImageTitleTag = "-";

        public void BuildGalleryPage(string definitionFile, string destinationFile, bool localContent = false)
        {
            string destinationDir = Path.GetDirectoryName(destinationFile);
            destinationDir.EnsureDirectory();

            SetupDependencies(destinationDir);

            var albumSpec = File.ReadAllLines(definitionFile)
                                .Skip(1) //version
                                .Select(x =>
                                {
                                    //<path>|<name>|<title>
                                    string[] parts = x.Split('|');

                                    var path = parts[0];
                                    var name = parts[1];
                                    var title = string.IsNullOrEmpty(parts[2]) ? name : parts[2];

                                    var thumbUrl = "thumbnails/" + name.Replace(" ", "_") + ".thumb.jpg";
                                    string imagerUrl;

                                    if (localContent)
                                        imagerUrl = "images/" + Path.GetFileName(path);
                                    else
                                        imagerUrl = path.TryConvertToRelativeUri(destinationDir);

                                    //DateTime imageTimestamp = File.GetLastWriteTimeUtc(fullPath);
                                    //DateTime thumbTimestamp = File.GetLastWriteTimeUtc(thumbUrl);
                                    //if (thumbTimestamp != imageTimestamp)
                                    //{
                                    //    Utils.CreateThumbnailImageUniform(fullPath, thumbUrl, 220, 140, ImageFormat.Jpeg);
                                    //    File.SetLastWriteTimeUtc(thumbUrl, imageTimestamp);
                                    //}

                                    return new { ImagePath = path, ImageUrl = imagerUrl, Thumb = thumbUrl, Title = parts.Last() };
                                })
                                  .ToArray(); //ToArray must be called to 'seal' the collection

            var builder = new StringBuilder();
            var injectionBuilder = new StringBuilder();

            string prevTitle = " ";

            Func<string, string> ToHtmlTitle = rawTitle =>
                {
                    string retval = rawTitle;
                    if (string.IsNullOrEmpty(rawTitle))
                        retval = " ";
                    else if (rawTitle == PrevImageTitleTag)
                        retval = prevTitle;

                    retval = SecurityElement.Escape(retval);
                    return retval;
                };

            builder.AppendLine("");
            if (albumSpec.Any())
                builder.AppendLine("    setInitalBackground(\"" + albumSpec.First().ImageUrl + "\",\"" + ToHtmlTitle(albumSpec.First().Title) + "\");");

            bool injectingHtml = true;

            foreach (var data in albumSpec)
            {
                Console.WriteLine("Injecting: " + data.ImageUrl);

                //HTML        | Browser
                //-------------------------------------
                //empty title | show the previous title
                //white space | show the empty title

                string title =
                prevTitle = ToHtmlTitle(data.Title);

                if (!injectingHtml)
                {
                    //injecting JS
                    builder.AppendLine(string.Format("    addImage(\"{0}\", \"{1}\", \"{2}\");", data.ImageUrl, title, data.Thumb));
                }
                else
                {
                    //injecting HTML
                    string html =
                        "<div class=\"content\">" +
                        "   <div>" +
                        "       <a href=\"" + data.ImageUrl + "\">" +
                        "           <img src=\"" + data.Thumb + "\" title=\"" + title + "\" alt=\"" + title + "\" class=\"thumb\" />" +
                        "       </a>" +
                        "   </div>" +
                        "</div>";

                    injectionBuilder.AppendLine(html);
                }
            }

            string albumHtml = GalleryTemplates.Resources.gallery_template
                                   .Replace("{$LOAD_IMAGES}", builder.ToString())
                                   .Replace("<!-- {$INJECT_IMAGES} -->", injectionBuilder.ToString());

            File.WriteAllText(destinationFile, albumHtml);
        }

        public void BuildGalleryPage_Old(string definitionFile, string destinationFile)
        {
            string destinationDir = Path.GetDirectoryName(destinationFile);

            SetupDependencies(destinationDir);

            var albumSpec = File.ReadAllLines(definitionFile)
                                .Skip(1) //version
                                .Select(x =>
                                {
                                    //<path>|<name>|<title>
                                    string[] parts = x.Split('|');

                                    var path = parts[0];
                                    var name = parts[1];
                                    var title = string.IsNullOrEmpty(parts[2]) ? name : parts[2];
                                    var fullPath = path;

                                    var imagerUrl = fullPath.TryConvertToRelativeUri(destinationDir);
                                    var thumbUrl = "thumbnails/" + name.Replace(" ", "_") + ".thumb.jpg";

                                    return new { ImagePath = path, ImageUrl = imagerUrl, Thumb = thumbUrl, Title = parts.Last() };
                                })
                               .ToArray(); //ToArray must be called to 'seal' the collection

            var builder = new StringBuilder();

            builder.AppendLine("");
            builder.AppendLine("    setInitalBackground(\"" + albumSpec.First().ImageUrl + "\");");

            foreach (var data in albumSpec)
            {
                builder.AppendLine(string.Format("    addImage(\"{0}\", \"{1}\", \"{2}\");", data.ImageUrl, data.Title, data.Thumb));
            }

            string albumHtml = GalleryTemplates.Resources.gallery_template
                                               .Replace("{$LOAD_IMAGES}", builder.ToString());

            File.WriteAllText(destinationFile, albumHtml);
        }
    }
}