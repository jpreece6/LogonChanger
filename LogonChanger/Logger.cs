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

        private static string _source = "LogonChanger";

        public static void WriteInformation(string message, bool overrideVerbose = false)
        {
            if (!overrideVerbose)
                if (!Verbose)
                    return;
            
            EventLog.WriteEntry(_source, message, EventLogEntryType.Information);
        }

        public static void WriteError(string message, Exception e)
        {
            EventLog.WriteEntry(_source, message + "\n\n" + e.Message);
        }

        public static void WriteWarning(string message)
        {
            EventLog.WriteEntry(_source, message, EventLogEntryType.Warning);
        }
    }
}
