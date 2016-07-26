using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using LogonEditor;
using SettingsVault;
using HelperLib = HelperLibrary.HelperLib;

namespace LogonChanger
{
    public class Changer
    {

        public void Update()
        {
            ConfigureWallpaperDir();

            var bingResource = new BingWebResource();
            var fileName = Settings.Default.Get<string>(Config.WallpaperDir) + Util.GenerateFileTimeStamp() + ".jpg";
            bingResource.GetResource(new Uri(Settings.Default.Get<string>(Config.Url)), fileName);

            TakeOwnershipPri();
            UpdatePri(fileName);

        }

        private void UpdatePri(string resourceFileName)
        {
            if (File.Exists(Config.NewPriPath))
                File.Delete(Config.NewPriPath);

            LogonPriEditor.ModifyLogonPri(Config.TempPriPath, Config.NewPriPath, resourceFileName);
            File.Copy("", Config.PriFileLocation,true);
        }

        private void ConfigureWallpaperDir()
        {
            if (!Directory.Exists(Settings.Default.Get<string>(Config.WallpaperDir)))
                Directory.CreateDirectory(Settings.Default.Get<string>(Config.WallpaperDir));
        }

        private void TakeOwnershipPri()
        {
            HelperLib.TakeOwnership(Config.LogonFolder);
            HelperLib.TakeOwnership(Config.PriFileLocation);

            if (!File.Exists(Config.BakPriFileLocation))
            {
                //log.Info("Could not find Windows.UI.Logon.pri.bak file. Creating new.");
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
