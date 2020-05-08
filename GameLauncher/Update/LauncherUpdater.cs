using GameLauncher.Misc;
using System;
using System.IO;
using System.Net;
using System.Windows.Forms;

namespace GameLauncher.Update
{
    internal static class LauncherUpdater
    {
        public class UpdateInfo
        {
            private UpdateInfo(bool available, string address)
            {
                UpdateAvailable = available;
                DownloadAddress = address;
            }

            public bool UpdateAvailable { get; }
            public string DownloadAddress { get; }

            public static UpdateInfo Available(string address)
            {
                return new UpdateInfo(true, address);
            }

            public static UpdateInfo NotAvailable()
            {
                return new UpdateInfo(false, null);
            }
        }

        public static UpdateInfo CheckForLauncherUpdate()
        {
            WebClient webClient = Helpers.BuildWebClient();
            string[] lines = webClient.DownloadString(LauncherSetup.FTP_LAUNCHER_VERSION_FILE).Replace("\r", String.Empty).Split('\n');
            string productVersion = Application.ProductVersion;
            string onlineVersion = String.Empty;
            string address = String.Empty;
            foreach (string line in lines)
            {
                if (line.StartsWith("VERSION="))
                    onlineVersion = line.Replace("VERSION=", String.Empty);
                else if (line.StartsWith("FILENAME="))
                    address = line.Replace("FILENAME=", String.Empty);
            }
            if (String.IsNullOrEmpty(address) || String.IsNullOrEmpty(onlineVersion))
                return UpdateInfo.NotAvailable();
            if (onlineVersion == productVersion)
                return UpdateInfo.NotAvailable();
            return UpdateInfo.Available(address);
        }

        public static bool UpdateLauncher(UpdateInfo info)
        {
            WebClient webClient = Helpers.BuildWebClient();
            string executablePath = Application.ExecutablePath;
            try
            {
                webClient.DownloadFile(info.DownloadAddress, executablePath + ".new");
                File.Move(executablePath, executablePath + ".old");
                File.Move(executablePath + ".new", executablePath);
            }
            catch
            {
                return false;
            }
            return true;
        }
    }
}
