using System;
using System.Configuration;
using System.IO;
using System.Reflection;

namespace Ruta
{
    static class Settings
    {
        public static float ScaleFactor { get; set; }

        public static string GeneratorApp { get; set; }

        public static string EditorApp { get; set; }

        public static string LastExport { get; set; }

        public static string LastRoot { get; set; }

        public static string LastAlbum { get; set; }

        public static int AutoSaveIntervalInSeconds { get; set; }

        static Settings()
        {
            Open();
        }

        static public void Open()
        {
            try
            {
                Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                AppSettingsSection app = config.AppSettings;

                var defaultRoot = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "Ruta Photo Albums");
                if (!Directory.Exists(defaultRoot))
                    Directory.CreateDirectory(defaultRoot);

                LastRoot = config.AppSettings.Get("LastRoot", defaultRoot);
                LastAlbum = config.AppSettings.Get("LastAlbum", "");
                LastExport = config.AppSettings.Get("LastExport", "");
                EditorApp = config.AppSettings.Get("EditorApp", "");
                GeneratorApp = config.AppSettings.Get("GeneratorApp", "").ExpandPath();
                AutoSaveIntervalInSeconds = config.AppSettings.Get("AutoSaveIntervalInSeconds", -5);
                ScaleFactor = config.AppSettings.Get("ScaleFactor", 0.8f);

                Save();
            }
            catch { }
        }

        static public void Save()
        {
            try
            {
                Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

                config.AppSettings.Set("LastRoot", LastRoot);
                config.AppSettings.Set("LastAlbum", LastAlbum);
                config.AppSettings.Set("LastExport", LastExport);
                config.AppSettings.Set("GeneratorApp", GeneratorApp.ExpandPath());
                config.AppSettings.Set("EditorApp", EditorApp);
                config.AppSettings.Set("AutoSaveIntervalInSeconds", AutoSaveIntervalInSeconds);
                config.AppSettings.Set("ScaleFactor", ScaleFactor);

                config.Save(ConfigurationSaveMode.Modified);
            }
            catch { }
        }

        static string ExpandPath(this string path)
        {
            if (path == "%this%")
                return Assembly.GetExecutingAssembly().Location;
            else
                return path;
        }

        public static string Get(this AppSettingsSection section, string name, string defaultValue)
        {
            var item = section.Settings[name];
            return item != null ? item.Value : defaultValue;
        }

        public static int Get(this AppSettingsSection section, string name, int defaultValue)
        {
            var item = section.Settings[name];
            int intValue = defaultValue;
            if (item != null)
                int.TryParse(item.Value, out intValue);
            return intValue;
        }

        public static float Get(this AppSettingsSection section, string name, float defaultValue)
        {
            var item = section.Settings[name];
            float fValue = defaultValue;
            if (item != null)
                float.TryParse(item.Value, out fValue);
            return fValue;
        }

        public static AppSettingsSection Set<T>(this AppSettingsSection section, string name, T value)
        {
            if (section.Settings[name] != null)
                section.Settings[name].Value = value.ToString();
            else
                section.Settings.Add(name, value.ToString());
            return section;
        }
    }
}