using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using log4net.Config;
using RestSharp;

namespace BingWallpaper
{
    public partial class FormMain : Form
    {
        private const int SpiSetdeskwallpaper = 20;
        private const string BaseUrl = "http://www.bing.com";
        private static string _currentWallpaper = string.Empty;

        public FormMain()
        {
            InitializeComponent();
            NormalToMinimized();

            AutoDownloadWallpaper();
            AutoChangeWallpaper();
        }

        private void NormalToMinimized()
        {
            this.WindowState = FormWindowState.Minimized;
            this.ShowInTaskbar = false;
            this.Visible = false;
        }

        /// <summary>
        /// 引用user32.dll的包。
        /// </summary>
        /// <param name="uAction"></param>
        /// <param name="uParam"></param>
        /// <param name="lpvparam"></param>
        /// <param name="fuwinIni"></param>
        /// <returns></returns>
        [DllImport("user32.dll", EntryPoint = "SystemParametersInfoA")]
        private static extern int SystemParametersInfo(int uAction, int uParam, string lpvparam, int fuwinIni);

        // ReSharper disable once FunctionRecursiveOnAllPaths
        private async void AutoDownloadWallpaper()
        {
            var client = new RestClient(BaseUrl);
            var request = new RestRequest("HPImageArchive.aspx?format=js&idx=0&n=100", Method.GET);
            var result = await client.ExecuteTaskAsync<WallpaperResponse>(request);
            foreach (var img in result.Data.Images)
            {
                var directory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "BingWallpapers");
                if (!Directory.Exists(directory))
                    Directory.CreateDirectory(directory);
                var fileName = $"{img.StartDate}_{img.EndDate}_{img.Url.Substring(img.Url.LastIndexOf("/", StringComparison.Ordinal) + 1)}";
                var path = Path.Combine(directory, fileName);
                var files = Directory.GetFiles(directory);
                if (files.All(x => x != path))
                {
                    var downloadRequest = new RestRequest(img.Url, Method.GET);
                    var data = client.DownloadData(downloadRequest);
                    File.WriteAllBytes(path, data);
                    LogHelper.LogInfo($"下载壁纸：{fileName}，成功");
                }
            }
            await Task.Delay(new TimeSpan(Settings.AutoDownloadWallpaperInterval, 0, 0));
            AutoDownloadWallpaper();
        }

        private async void AutoChangeWallpaper()
        {
            var directory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "BingWallpapers");
            if (!Directory.Exists(directory))
                Directory.CreateDirectory(directory);
            var files = Directory.GetFiles(directory).OrderByDescending(x => x).ToList();
            if (files.Count < 1)
                return;
            var time = DateTime.Now.TimeOfDay;
            if (time > new TimeSpan(8, 00, 0) && time < new TimeSpan(9, 0, 0))
            {
                SystemParametersInfo(SpiSetdeskwallpaper, 0, files[0], 1);
                _currentWallpaper = files[0];
                LogHelper.LogInfo($"更换壁纸：{_currentWallpaper}，成功");
            }
            else
            {
                var currentIndex = files.IndexOf(_currentWallpaper);
                var index = currentIndex + 1;
                if (index > files.Count - 1)
                    index = 0;
                SystemParametersInfo(SpiSetdeskwallpaper, 0, files[index], 1);
                _currentWallpaper = files[index];
                LogHelper.LogInfo($"更换壁纸：{_currentWallpaper}，成功");
            }
            await Task.Delay(new TimeSpan(0, Settings.AutoChangeWallpaperInterval, 0));
            AutoChangeWallpaper();
        }
    }
}
