using System;
using System.Collections.Generic;
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
    static class Extensions
    {
        public static void SafeExecute(this Form obj, Action action)
        {
            obj.SafeExecute(null, action);
        }
        public static void SafeExecute(this Form obj, Cursor executCursor, Action action)
        {
            try
            {
                if (executCursor != null)
                    obj.Cursor = executCursor;
                action();
            }
            catch (Exception e)
            {
                if (executCursor != null)
                    obj.Cursor = Cursors.Default;
                MessageBox.Show(e.ToString());
            }
            if (executCursor != null)
                obj.Cursor = Cursors.Default;
        }

        public static void WithWaitCursor(this Form obj, Action action)
        {
            try
            {
                obj.Cursor = Cursors.WaitCursor;
                action();
            }
            finally
            {
                obj.Cursor = Cursors.Default;
            }
        }

        public static string Escape(this string data)
        {
            return data.Replace("|", "{$PIPE}");
        }

        public static string Unescape(this string data)
        {
            return data.Replace("{$PIPE}", "|");
        }

        public static string[] SplitAllLines(this string data)
        {
            return data.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
        }

        public static Tuple<int, int> GetVisibleItems(this ListBox control)
        {
            if (control.Items.Count == 0)
                return new Tuple<int, int>(-1, -1);

            Point top = new Point(20, control.ItemHeight / 2);
            Point bottom = new Point(20, control.ClientRectangle.Height - control.ItemHeight / 2);
            int firstItemIndex = control.IndexFromPoint(top);
            int lastItemIndex = control.IndexFromPoint(bottom);

            return new Tuple<int, int>(firstItemIndex, lastItemIndex);
        }

        static public bool ScrollDown(this ListBox control)
        {
            if (control.TopIndex < control.Items.Count + 2)
            {
                control.TopIndex = control.TopIndex + 1;
                return true;
            }
            return false;
        }

        static public bool ScrollUp(this ListBox control)
        {
            if (control.TopIndex > 0)
            {
                control.TopIndex = control.TopIndex - 1;
                return true;
            }
            return false;
        }

        static public bool SelectNextItem(this ListBox control)
        {
            if (control.SelectedIndex < control.Items.Count + 2)
            {
                int index = control.SelectedIndex + 1;
                control.SelectedItems.Clear();
                control.SelectedIndex = index;
                return true;
            }
            return false;
        }

        static public bool SelectPrevItem(this ListBox control)
        {
            if (control.SelectedIndex > 0)
            {
                int index = control.SelectedIndex - 1;
                control.SelectedItems.Clear();
                control.SelectedIndex = index;
                return true;
            }
            return false;
        }

        static public bool MoveSelectionDown(this ListBox control)
        {
            var firstSelected = control.Items.First<object>(item => control.SelectedItems.Contains(item), 0);
            if (firstSelected != -1)
            {
                var firstUnselected = control.Items.First<object>(item => !control.SelectedItems.Contains(item), firstSelected);
                if (firstUnselected != -1)
                {
                    var unselectedItem = control.Items[firstUnselected];

                    var allSelected = control.SelectedItems.ToArray<object>();

                    control.Items.Remove(unselectedItem);
                    foreach (var item in allSelected)
                        control.Items.Remove(item);

                    control.Items.Insert(firstSelected, unselectedItem);
                    foreach (var item in allSelected.Reverse().Where(x => x != null))
                        control.Items.Insert(firstSelected + 1, item);

                    //all selected are already cleared because of removing
                    foreach (var item in allSelected)
                        control.SelectedItems.Add(item);
                    return true;
                }
            }
            return false;
        }

        public static string ClearDirectory(this string dir, bool deleteSubDirs = true)
        {
            if (Directory.Exists(dir))
            {
                foreach (string file in Directory.GetFiles(dir, "*", SearchOption.AllDirectories))
                    File.Delete(file);

                if (deleteSubDirs)
                    foreach (string path in Directory.GetDirectories(dir, "*", SearchOption.AllDirectories))
                        Directory.Delete(path);
            }

            return dir;
        }

        public static string ChangeDirectory(this string path, string newDir)
        {
            return Path.Combine(newDir, Path.GetFileName(path));
        }
        
        public static string ChangeFileName(this string path, string newFileName)
        {
            return Path.Combine(Path.GetDirectoryName(path), newFileName);
        }

        public static string EnsureDirectory(this string dir)
        {
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);
            return dir;
        }

        public static bool IsImageFile(this string file)
        {
            try
            {
                using (var i = Image.FromFile(file))
                    return true;
            }
            catch { }
            return false;
        }

        public static void BindToShortcut(this ToolStripButton button, Keys key, Keys modifiers)
        {
            string modifiersDisplay = modifiers.ToString().Replace("|", "+").Replace("Control", "Ctrl");

            button.ToolTipText += "\n" + modifiersDisplay + "+" + key;
            button.Tag = new Tuple<Keys, Keys>(key, modifiers);
        }

        public static string TryConvertToRelativeUri(this string path, string relativeToDir)
        {
            string srcRoot = Path.GetPathRoot(Path.GetFullPath(path));
            string destRoot = Path.GetPathRoot(Path.GetFullPath(relativeToDir));

            if (srcRoot != destRoot)
                return new Uri(Path.GetFullPath(path)).AbsoluteUri; //these are different drives and relative path cannot be created; so return the absolute one

            var absPath = new Uri(Path.GetFullPath(path)).AbsoluteUri;
            var relativeToAbsPath = new Uri(Path.GetFullPath(relativeToDir)).AbsoluteUri;

            int commonPartLength = 0;
            for (int i = 0; i < Math.Min(absPath.Length, relativeToAbsPath.Length); i++)
            {
                if (absPath[i] != relativeToAbsPath[i])
                    break;
                commonPartLength = i;
            }

            string relPath = absPath.Substring(commonPartLength);
            string relativeToRelPath = relativeToAbsPath.Substring(commonPartLength);

            string moveUpSpec = string.Join("/", relativeToRelPath.Split(new[] { '/' }, StringSplitOptions.RemoveEmptyEntries).Select(x => "..").ToArray());

            return moveUpSpec + relPath;
        }

        static public void SafeSelectItemAt(this ListBox control, int index)
        {
            if (control.Items.Count > 0)
            {
                if (index == control.Items.Count)
                    index--;
                control.SelectedIndex = Math.Max(0, index);
            }
        }
        static public bool MoveSelectionUp(this ListBox control)
        {
            var firstSelected = control.Items.First<object>(item => control.SelectedItems.Contains(item), 0);
            if (firstSelected != -1)
            {
                var lastUnselected = firstSelected - 1;
                if (lastUnselected != -1)
                {
                    var allSelected = control.SelectedItems.ToArray<object>();

                    foreach (var item in allSelected)
                        control.Items.Remove(item);

                    foreach (var item in allSelected.Reverse())
                        control.Items.Insert(lastUnselected, item);

                    //all selected are already cleared because of removing
                    foreach (var item in allSelected)
                        control.SelectedItems.Add(item);

                    return true;
                }
            }
            return false;
        }

        public static int First<T>(this ListBox.ObjectCollection items, Predicate<T> predicate, int start)
        {
            for (int i = start; i < items.Count; i++)
                if (predicate((T)items[i]))
                    return i;

            return -1;
        }

        public static T[] ToArray<T>(this System.Collections.ICollection items)
        {
            var retval = new T[items.Count];
            items.CopyTo(retval, 0);
            return retval;
        }

        public static System.Collections.IList AddRange<T>(this System.Collections.IList items, IEnumerable<T> newItems)
        {
            foreach (var item in newItems)
                items.Add(item);
            return items;
        }
    }
}
