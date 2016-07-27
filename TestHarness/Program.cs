using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LogonChanger;
using SettingsVault;

namespace TestHarness
{
    class Program
    {
        static void Main(string[] args)
        {
            var changer = new Changer();

            Settings.Init(Config.SettingsFilePath);

            Settings.Default.Set(Config.Interval, 3600000); // 1 hour
            Settings.Default.Set(Config.BingXmlkey, "urlBase");
            Settings.Default.Set(Config.Resolution, "1920x1200");
            Settings.Default.Set(Config.WallpaperDir, @"C:\LogonWallpapers\");
            Settings.Default.Set(Config.Url, @"http://www.bing.com/HPImageArchive.aspx?format=xml&idx=0&n=1&mkt=en-GB");
            Settings.Default.Set(Config.Verbose, false);

            Settings.Default.Save();

            changer.Update();
        }
    }
}
