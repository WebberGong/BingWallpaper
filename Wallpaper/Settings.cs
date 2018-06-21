using System;
using System.Configuration;

namespace BingWallpaper
{
    public class Settings
    {
        public static int AutoDownloadWallpaperInterval => GetInt("AutoDownloadWallpaperInterval");

        public static int AutoChangeWallpaperInterval => GetInt("AutoChangeWallpaperInterval");

        private static DateTime GetDateTime(string key)
        {
            DateTime time;
            return DateTime.TryParse(ConfigurationManager.AppSettings[key], out time) ? time : DateTime.MinValue;
        }

        private static bool GetBool(string key)
        {
            bool value;
            return bool.TryParse(ConfigurationManager.AppSettings[key], out value) && value;
        }

        private static int GetInt(string key)
        {
            int value;
            int.TryParse(ConfigurationManager.AppSettings[key], out value);
            return value;
        }
    }
}