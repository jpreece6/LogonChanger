using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogonChanger
{
    public static class Config
    {
        // Settings Keys
        public static readonly string Interval = "PingInterval";
        public static readonly string BingXmlkey = "XmlKey";
        public static readonly string Resolution = "Resolution";
        public static readonly string WallpaperDir = "WallpaperDir";
        public static readonly string Url = "Url";
        public static readonly string Verbose = "Verbose";
        public static readonly string BingHash = "BingHash";

        public static readonly string PriFileName = "Windows.UI.Logon.pri";
        public static readonly string BakPriFileName = $"{PriFileName}.bak";
        public static readonly string CurrentImage = "current.img";

        public static readonly string TempPriPath = Path.Combine(Path.GetTempPath(), "temp_pri.pri");
        public static readonly string NewPriPath = Path.Combine(Path.GetTempPath(), "new_pri.pri");

        public static readonly string LogonFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Windows),
            "SystemResources", "Windows.UI.Logon");

        public static readonly string PriFileLocation = Path.Combine(LogonFolder, PriFileName);
        public static readonly string BakPriFileLocation = Path.Combine(LogonFolder, BakPriFileName);
        public static readonly string CurrentImageLocation = Path.Combine(LogonFolder, CurrentImage);

        public static string SettingsFilePath = Path.Combine(@"C:\", "LoginChangerService", "settings.bin");
    }
}
