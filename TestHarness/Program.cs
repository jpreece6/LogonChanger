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
            Changer.InitialiseSettings(args);

            Changer.Update();
        }
    }
}
