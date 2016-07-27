using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogonChanger
{
    public static class Logger
    {
        public static bool Verbose { get; set; }

        public static void WriteInformation(string message)
        {
            if (Verbose)
                EventLog.WriteEntry("LogonChanger", message, EventLogEntryType.Information);
        }

        public static void WriteError(string message, Exception e)
        {
            EventLog.WriteEntry("LogonChanger", message + "\n\n" + e.Message);
        }
    }
}
