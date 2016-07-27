using System.ServiceProcess;
using System.Timers;
using LogonChanger;
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

        protected override void OnStart(string[] args)
        {

            Changer.InitialiseSettings(args);

            _serviceTimer = new Timer();
            _serviceTimer.Interval = Settings.Default.Get<int>(Config.Interval);
            _serviceTimer.Elapsed += ServiceTimer_Elapsed;
            _serviceTimer.Start();
            ServiceTimer_Elapsed(null, null);
        }

        private void ServiceTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            Changer.Update();
        }

        protected override void OnStop()
        {
            _serviceTimer.Stop();
        }

        protected override void OnSessionChange(SessionChangeDescription scd)
        {
            base.OnSessionChange(scd);

            Logger.WriteInformation("----  Session Change  ----\n\nReason = " + scd.Reason.ToString());

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
