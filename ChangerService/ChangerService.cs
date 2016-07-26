using System;
using System.Diagnostics;
using System.IO;
using System.ServiceProcess;
using System.Timers;
using LogonChanger;
using NDesk.Options;
using SettingsVault;

namespace ChangerService
{
    public partial class ChangerService : ServiceBase
    {
        public ChangerService()
        {
            InitializeComponent();
        }

        private Timer _serviceTimer;
        private Changer _changer;

        private int _interval;

        protected override void OnStart(string[] args)
        {

            _changer = new Changer();

            Settings.Init(Config.SettingsFilePath);

            // Initalise empty settings file
            if (!File.Exists(Config.SettingsFilePath))
            {
                Settings.Default.Set(Config.Interval, 3600000); // 1 hour
                Settings.Default.Set(Config.BingXmlkey, "UrlBase");
                Settings.Default.Set(Config.Resolution, "1920x1200");
                Settings.Default.Set(Config.WallpaperDir, @"C:\lockWallpapers\");
                Settings.Default.Set(Config.Url, @"http://www.bing.com/HPImageArchive.aspx?format=xml&idx=0&n=1&mkt=en-GB");
                Settings.Default.Set(Config.Verbose, false);

                Settings.Default.Save();
            }

            // Parse any arguments
            if (args.Length > 0)
            {
                string loc = "";
                string oriUrl = "";
                var p = new OptionSet()
                {
                    {"i|interval=", (int v) => Settings.Default.Set(Config.Interval, v) },
                    {"x|key=", v => Settings.Default.Set(Config.BingXmlkey, v) },
                    {"r|res=", v => Settings.Default.Set(Config.Resolution, v) },
                    {"d|dir=", v => Settings.Default.Set(Config.WallpaperDir, v) },
                    {"u|url=", v => Settings.Default.Set(Config.Url, v) },
                    {"l|loc=", v => loc = v},
                    {"v", v => Settings.Default.Set(Config.Verbose, (v != null)) },
                };

                try
                {
                    p.Parse(args);
                }
                catch (Exception ex)
                {
                    EventLog.WriteEntry("Argument Failure: \n\n" + ex.Message,EventLogEntryType.Error);
                    //log.Error("Args error: ", ex);
                }

                // Only replace the market if we have something to put there
                if (loc.Equals("") == false)
                {
                    oriUrl = Settings.Default.Get<string>("bingUrl");
                    Settings.Default.Set("bingUrl", oriUrl.Replace(oriUrl.Substring(oriUrl.Length - 5), loc));
                }

                Settings.Default.Save();
            }

            _serviceTimer = new Timer();
            _serviceTimer.Interval = _interval;
            _serviceTimer.Elapsed += ServiceTimer_Elapsed;
            _serviceTimer.Start();
            ServiceTimer_Elapsed(null, null);
        }

        private void ServiceTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            _changer.Update();
        }

        protected override void OnStop()
        {
            _serviceTimer.Stop();
        }

        protected override void OnSessionChange(SessionChangeDescription scd)
        {
            base.OnSessionChange(scd);

            //log.Info("\n\n----  Session Change  ----\n\nReason = " + scd.Reason.ToString());

            // If the user logs in or unlocks the PC try to update.
            if (scd.Reason == SessionChangeReason.SessionLogon || scd.Reason == SessionChangeReason.SessionUnlock)
            {
                _serviceTimer.Stop();
                _serviceTimer.Start();
                ServiceTimer_Elapsed(null, null);
            }
        }

        protected override void OnPause()
        {
            base.OnPause();

            _serviceTimer.Stop();
        }

        /// <summary>
        /// 
        /// </summary>
        protected override void OnContinue()
        {
            base.OnContinue();

            _serviceTimer.Start();
        }
    }
}
