using System;
using System.Configuration;

namespace Wallpaper
{
    public class Settings
    {
        public static int AutoDownloadWallpaperInterval => GetInt("AutoDownloadWallpaperInterval");

        public static int AutoChangeWallpaperInterval => GetInt("AutoChangeWallpaperInterval");

        public static int TheNewestWallpaperStartTime => GetInt("TheNewestWallpaperStartTime");

        public static int TheNewestWallpaperEndTime => GetInt("TheNewestWallpaperEndTime");

        private static DateTime GetDateTime(string key)
        {
            DateTime time;
            return DateTime.TryParse(ConfigurationManager.AppSettings[key], out time) ? time : DateTime.MinValue;
        }

        private static bool GetBool(string key)
        {
            ConfigurationManager.RefreshSection("appSettings");
            bool value;
            return bool.TryParse(ConfigurationManager.AppSettings[key], out value) && value;
        }

        private static int GetInt(string key)
        {
            ConfigurationManager.RefreshSection("appSettings");
            int value;
            int.TryParse(ConfigurationManager.AppSettings[key], out value);
            return value;
        }
    }
}