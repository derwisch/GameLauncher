using System.Net;

namespace GameLauncher.Misc
{
    internal static class Helpers
    {
        /// <summary>
        /// Builds a webclient for the configured FTP
        /// </summary>
        public static WebClient BuildWebClient()
        {
            WebClient webClient = new WebClient
            {
                Credentials = new NetworkCredential(
                                        LauncherSetup.FTP_READONLY_USER_NAME, 
                                        LauncherSetup.FTP_READONLY_USER_PASS)
            };
            return webClient;
        }
    }
}
