using System;
using System.Configuration;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

namespace Ruta
{

    public static class Utils
    {
        /// <summary>
        /// Gets the (private) PropertyGridView instance.
        /// </summary>
        /// <param name="propertyGrid">The property grid.</param>
        /// <returns>The PropertyGridView instance.</returns>
        private static object GetPropertyGridView(PropertyGrid propertyGrid)
        {
            //private PropertyGridView GetPropertyGridView();
            //PropertyGridView is an internal class...
            MethodInfo methodInfo = typeof(PropertyGrid).GetMethod("GetPropertyGridView", BindingFlags.NonPublic | BindingFlags.Instance);
            return methodInfo.Invoke(propertyGrid, new object[0]);
        }

        /// <summary>
        /// Gets the width of the left column.
        /// </summary>
        /// <param name="propertyGrid">The property grid.</param>
        /// <returns>
        /// The width of the left column.
        /// </returns>
        public static int GetInternalLabelWidth(this PropertyGrid propertyGrid)
        {
            //System.Windows.Forms.PropertyGridInternal.PropertyGridView
            object gridView = GetPropertyGridView(propertyGrid);

            //protected int InternalLabelWidth
            PropertyInfo propInfo = gridView.GetType().GetProperty("InternalLabelWidth", BindingFlags.NonPublic | BindingFlags.Instance);
            return (int)propInfo.GetValue(gridView, new object[0]);
        }

        /// <summary>
        /// Moves the splitter to the supplied horizontal position.
        /// </summary>
        /// <param name="propertyGrid">The property grid.</param>
        /// <param name="xpos">The horizontal position.</param>
        public static void MoveSplitterTo(this PropertyGrid propertyGrid, int xpos)
        {
            //System.Windows.Forms.PropertyGridInternal.PropertyGridView
            object gridView = GetPropertyGridView(propertyGrid);

            //private void MoveSplitterTo(int xpos);
            MethodInfo methodInfo = gridView.GetType().GetMethod("MoveSplitterTo", BindingFlags.NonPublic | BindingFlags.Instance);
            methodInfo.Invoke(gridView, new object[] { xpos });
        }

        public static void CreateThumbnailImageUniform(string fromPath, string toPath, int width, int height, ImageFormat format = null)
        {
            CreateThumbnailImageUniform(fromPath, width, height, format).Save(toPath, ImageFormat.Jpeg);
        }

        public static Image CreateThumbnailImageUniform(string fromPath, int width, int height, ImageFormat format = null)
        {
            Bitmap srcBmp = new Bitmap(fromPath);
            float ratio = (float)srcBmp.Width / (float)srcBmp.Height;

            SizeF newSize;
            if (srcBmp.Width > srcBmp.Height)
                newSize = new SizeF(width, width / ratio);
            else
                newSize = new SizeF(height * ratio, height);

            Bitmap target = new Bitmap(width, height);

            using (Graphics graphics = Graphics.FromImage(target))
            {
                graphics.CompositingQuality = CompositingQuality.HighSpeed;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.CompositingMode = CompositingMode.SourceCopy;

                graphics.FillRectangle(Brushes.Black, 0, 0, srcBmp.Width, srcBmp.Height);

                if (newSize.Height < height)
                {
                    graphics.DrawImage(srcBmp, 0, (int)((height - newSize.Height) / 2), newSize.Width, newSize.Height);
                }
                else
                {
                    float ratio2 = (float)newSize.Height / (float)height;
                    SizeF newSize2 = new SizeF(newSize.Width / ratio2, newSize.Height / ratio2);

                    graphics.DrawImage(srcBmp, (int)((width - newSize2.Width) / 2), 0, newSize2.Width, newSize2.Height);
                }

                return target;
            }
        }

        public static void ScaleImage(string fromPath, string toPath, float scalingFactor)
        {
            using (var srcBmp = new Bitmap(fromPath))
            {
                Size newSize = new Size((int)(srcBmp.Width * scalingFactor), (int)(srcBmp.Height * scalingFactor));
                using (Bitmap destBmp = new Bitmap(newSize.Width, newSize.Height))
                using (Graphics graphics = Graphics.FromImage(destBmp))
                {
                    graphics.CompositingQuality = CompositingQuality.HighSpeed;
                    graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    graphics.CompositingMode = CompositingMode.SourceCopy;
                    graphics.DrawImage(srcBmp, 0, 0, newSize.Width, newSize.Height);

                    destBmp.Save(toPath, srcBmp.RawFormat);
                }

            }
        }
    }


}