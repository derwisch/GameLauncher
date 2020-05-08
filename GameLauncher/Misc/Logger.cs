using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace GameLauncher.Misc
{
    internal static class Logger
    {
        private static FileStream logStream = File.Open("launcher.log", FileMode.OpenOrCreate);
        private static StreamWriter logWriter = new StreamWriter((Stream)Logger.logStream);

        internal static void Log(string message)
        {
            logWriter.WriteLine($"[{DateTime.Now:dd.MM.yyyy hh:mm:ss.ffff}] {message}");
        }
        internal static void Log(Exception ex)
        {
            logWriter.WriteLine(DateTime.Now.ToString("[dd.MM.yyyy hh:mm:ss.ffff]") + String.Format(" An exception of type: {0} occured", ex.GetType()));
            logWriter.WriteLine($"[{DateTime.Now:dd.MM.yyyy hh:mm:ss.ffff}] An exception of type: {ex.GetType()} occured");
            foreach (string s in ex.StackTrace.Split('\n'))
            {
                logWriter.WriteLine(String.Empty.PadLeft(26, ' ') + s);
            }
            logWriter.WriteLine(String.Empty.PadLeft(26, ' ') + ex.Message);
        }
    }
}
