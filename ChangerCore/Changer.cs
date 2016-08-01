using System;
using System.CodeDom;
using System.Dynamic;
using System.IO;
using ChangerCore.Exceptions;
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

        /// <summary>
        /// Updates the logon screen with a new image from a specified resource
        /// </summary>
        public static void Update()
        {
            // Ensure we're running WIN 10 or above
            if (!Util.IsWindows10())
            {
                throw new UnsupportedOSException("Non supported OS detected aborting...");
            }

            ConfigureWallpaperDir();

            var fileName = "";

            // Determine the mode to process
            switch (Settings.Default.Get<Mode>(Config.Mode, Mode.Bing))
            {
                // Get logon image from Bing
                case Mode.Bing:
                    var bingResource = new BingWebResource();
                    fileName = bingResource.GetResourceFromConfig(Settings.Default.Get<string>(Config.ConfigPath));
                    break;
                // Get logon image from a custom url
                case Mode.Remote:
                    var remoteResource = new WebResource();
                    fileName = remoteResource.GetResource(new Uri(Settings.Default.Get<string>(Config.Url)));
                    break;
                // Get logon image from local directory
                case Mode.Local:
                    var localResource = new LocalResource();
                    fileName = localResource.GetResource(Settings.Default.Get<string>(Config.WallpaperDir));
                    break;
            }
            

            // If we failed to download the image just return
            if (string.IsNullOrEmpty(fileName))
                return;
            
            UpdatePri(fileName);

        }

        /// <summary>
        /// Initalises the application settings. From default selected options and from arguments
        /// passed to it from a source application
        /// </summary>
        /// <param name="args">arguments initalise settings from</param>
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
                Settings.Default.Set(Config.Shuffle, false);
                Settings.Default.Set(Config.FileIndex, 0);

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
                    {"s|shuffle:", (bool v) => Settings.Default.Set(Config.Shuffle, v)},
                    {"si|startIndex:", (int v) => Settings.Default.Set(Config.FileIndex, v)},
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

        /// <summary>
        /// Displays the help message when requested via cmd args
        /// </summary>
        private static void DisplayHelp()
        {
            Logger.WriteInformation("Useage ", true);
        }

        /// <summary>
        /// Updates the system PRI file with the new image
        /// </summary>
        /// <param name="resourceFileName">Path to new image</param>
        private static void UpdatePri(string resourceFileName)
        {
            TakeOwnershipPri();

            if (File.Exists(Config.NewPriPath))
                File.Delete(Config.NewPriPath);

            LogonPriEditor.ModifyLogonPri(Config.TempPriPath, Config.NewPriPath, resourceFileName);
            File.Copy(Config.NewPriPath, Config.PriFileLocation,true);

            Logger.WriteInformation("Wallpaper Set!", true);
        }

        /// <summary>
        /// Creates a new wallpaper directory. Used to store images from Bing or custom urls
        /// </summary>
        private static void ConfigureWallpaperDir()
        {
            if (!Directory.Exists(Settings.Default.Get<string>(Config.WallpaperDir)))
                Directory.CreateDirectory(Settings.Default.Get<string>(Config.WallpaperDir));
        }

        /// <summary>
        /// Aquires ownership of the system PRI file so we can update the file
        /// </summary>
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
