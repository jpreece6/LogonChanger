using System;
using System.IO;
using LogonEditor;
using NDesk.Options;
using SettingsVault;
using HelperLib = HelperLibrary.HelperLib;

namespace ChangerCore
{
    public static class Changer
    {

        public enum Mode
        {
            Local,
            Remote,
            Bing
        }

        public static void Update()
        {
            ConfigureWallpaperDir();

            var fileName = "";

            switch (Settings.Default.Get<Mode>(Config.Mode, Mode.Bing))
            {
                case Mode.Bing:
                    var bingResource = new BingWebResource();
                    fileName = bingResource.GetResourceFromConfig(Settings.Default.Get<string>(Config.ConfigPath));
                    break;
                case Mode.Remote:
                    var remoteResource = new WebResource();
                    fileName = remoteResource.GetResource(new Uri(Settings.Default.Get<string>(Config.Url)));
                    break;
                case Mode.Local:
                    var localResource = new LocalResource();
                    fileName = localResource.GetResource(Settings.Default.Get<string>(Config.WallpaperDir));
                    break;
            }
            

            // If we failed to download the image just return
            if (string.IsNullOrEmpty(fileName))
                return;
            
            TakeOwnershipPri();
            UpdatePri(fileName);

        }

        public static void InitialiseSettings(string[] args = null)
        {
            Settings.Init(Config.SettingsFilePath);

            // Initalise empty settings file
            if (!File.Exists(Config.SettingsFilePath))
            {
                Settings.Default.Set(Config.Interval, 3600000); // 1 hour
                Settings.Default.Set(Config.WallpaperDir, @"C:\LogonWallpapers\");
                Settings.Default.Set(Config.Verbose, false);
                Settings.Default.Set(Config.Mode, Mode.Bing);
                Settings.Default.Set(Config.ConfigPath, Config.RemoteConfigPath);

                Settings.Default.Save();
            }

            // Parse any arguments
            if (args?.Length > 0)
            {
                var p = new OptionSet()
                {
                    {"i|interval:", (int v) => Settings.Default.Set(Config.Interval, v)},
                    {"d|dir:", v => Settings.Default.Set(Config.WallpaperDir, v)},
                    {"u|url:", v => Settings.Default.Set(Config.Url, v)},
                    {"m|mode:", (int v) => Settings.Default.Set(Config.Mode, (Mode)v)},
                    {"c|config:", v => Settings.Default.Set(Config.ConfigPath,v) },
                    {"v", v => Settings.Default.Set(Config.Verbose, (v != null))},
                    {"h|help", v => DisplayHelp() }
                };

                try
                {
                    p.Parse(args);
                }
                catch (Exception ex)
                {
                    Logger.WriteError("Argument Failure: \n\n", ex);
                }

            }

            Logger.Verbose = Settings.Default.Get<bool>(Config.Verbose,false);
            Settings.Default.Save();
        }

        private static void DisplayHelp()
        {
            Logger.WriteInformation("Useage ", true);
        }

        private static void UpdatePri(string resourceFileName)
        {
            if (File.Exists(Config.NewPriPath))
                File.Delete(Config.NewPriPath);

            LogonPriEditor.ModifyLogonPri(Config.TempPriPath, Config.NewPriPath, resourceFileName);
            File.Copy(Config.NewPriPath, Config.PriFileLocation,true);

            Logger.WriteInformation("Wallpaper Set!", true);
        }

        private static void ConfigureWallpaperDir()
        {
            if (!Directory.Exists(Settings.Default.Get<string>(Config.WallpaperDir)))
                Directory.CreateDirectory(Settings.Default.Get<string>(Config.WallpaperDir));
        }

        private static void TakeOwnershipPri()
        {
            HelperLib.TakeOwnership(Config.LogonFolder);
            HelperLib.TakeOwnership(Config.PriFileLocation);

            if (!File.Exists(Config.BakPriFileLocation))
            {
                Logger.WriteWarning("Could not find Windows.UI.Logon.pri.bak file. Creating new.");
                File.Copy(Config.PriFileLocation, Config.BakPriFileLocation);
            }

            HelperLib.TakeOwnership(Config.BakPriFileLocation);

            File.Copy(Config.BakPriFileLocation, Config.TempPriPath, true);

            if (File.Exists(Config.CurrentImageLocation))
            {
                var temp = Path.GetTempFileName();
                File.Copy(Config.CurrentImageLocation, temp, true);

                File.Delete(Config.CurrentImageLocation);
            }
        }
    }
}
