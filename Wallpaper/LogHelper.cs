using System;
using System.Diagnostics;
using log4net;

namespace Wallpaper
{
    public class LogHelper
    {
        public static void LogInfo(string info)
        {
            LogManager.GetLogger(GetTypeFromStackTrace()).Info(info);
        }

        public static void LogWarn(string warn)
        {
            LogManager.GetLogger(GetTypeFromStackTrace()).Warn(warn);
        }

        public static void LogDebug(string debug)
        {
            LogManager.GetLogger(GetTypeFromStackTrace()).Debug(debug);
        }

        public static void LogError(string error, Exception ex = null)
        {
            LogManager.GetLogger(GetTypeFromStackTrace()).Error(error, ex);
        }

        public static void LogFatal(string fatal, Exception ex = null)
        {
            LogManager.GetLogger(GetTypeFromStackTrace()).Fatal(fatal, ex);
        }

        private static Type GetTypeFromStackTrace()
        {
            var st = new StackTrace();
            var sf = st.GetFrame(2);
            return sf?.GetMethod()?.DeclaringType;
        }
    }
}