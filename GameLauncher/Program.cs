using GameLauncher.UI;
using GameLauncher.Update;
using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Windows.Forms;

namespace GameLauncher
{
    /// <summary>
    /// This class contains most data needed to configure the launcher.
    /// Just change the entries as you need.
    /// 
    /// To change the icon you need to change it here:
    /// - For the window icon change it in FormLauncher
    /// - For the application icon change it in the project properties
    /// 
    /// .NET Assemblies are easily decompiled. Make sure you enter no sensible information
    /// that you don't want anyone to have. The FTP user data you enter here should only 
    /// have read access!
    /// </summary>
    internal static class LauncherSetup
    {
        #region UI Settings
        /// <summary>
        /// Text inside the title line of the launcher
        /// </summary>
        public const string LAUNCHER_UI_TITLE = "Demo - Launcher";
        //public static readonly string LAUNCHER_UI_TITLE = Localization.GetText("Launcher.Title");

        /// <summary>
        /// A list of links to show in the sidebar of the launcher
        /// 
        /// If you define your own entries in the localization xml you can access them using:
        /// Localization.GetText("Your.Own.Key")
        /// </summary>
        public static readonly LauncherLink[] LauncherLinks = new LauncherLink[]
        {
            new LauncherLink(Localization.GetText("Example.LocalizedLink.URL"), Localization.GetText("Example.LocalizedLink.Title")),
            new LauncherLink("http://reddit.com/r/gamedev", "Gamedev subreddit")
        };

        /// <summary>
        /// Whether your changelog file (content.changes) is HTML (true) or TEXT (false)
        /// </summary>
        public const bool IS_CHANGELOG_HTML = false;

        /// <summary>
        /// A HTML style for text changelogs and the error message displayed
        /// in the changelog box if no changelog could be loaded
        /// </summary>
        public const string TEXT_CHANGELOG_STYLE = "font-family: Calibri; font-size: 12pt;";

        /// <summary>
        /// Adds additional pseudo-markdowny html replacements:
        /// 
        /// --- gets replaced by <hr />
        /// **( gets replaced by <b>
        /// )** gets replaced by </b>
        /// //( gets replaced by <i>
        /// )// gets replaced by </i>
        /// __( gets replaced by <u>
        /// )__ gets replaced by </u>
        /// </summary>
        public const bool ADDITIONAL_HTML_REPLACEMENTS = false;
        #endregion

        #region FTP Data
        /// <summary>
        /// Username of a ftp user with read-only access to your FTP server.
        /// 
        /// .NET Assemblies are easily decompiled. Make sure you enter no sensible information
        /// that you don't want anyone to have.
        /// </summary>
        public const string FTP_READONLY_USER_NAME = "<user>";

        /// <summary>
        /// Password of a ftp user with read-only access to your FTP server.
        /// 
        /// .NET Assemblies are easily decompiled. Make sure you enter no sensible information
        /// that you don't want anyone to have.
        /// </summary>
        public const string FTP_READONLY_USER_PASS = "<password>";

        /// <summary>
        /// Base address of your FTP server (with tailing slash)
        /// 
        /// The example layout is like this:
        /// ftp://your.ftp.server/
        ///  ├─ content/
        ///  │   ├─ some
        ///  │   ├─ game
        ///  │   ├─ files
        ///  │   └─ ...
        ///  ├─ content.xml
        ///  ├─ content.version
        ///  ├─ content.changes
        ///  ├─ launcher.version
        ///  └─ GameLauncher.exe
        /// 
        /// You can assign custom paths to all config files
        /// and a custom path to the wanted game file folder
        /// </summary>
        public const string FTP_BASE_URL = "ftp://your.ftp.server/";

        /// <summary>
        /// URL of the folder containing the game files
        /// </summary>
        public const string FTP_GAME_FILE_UPDATE_FOLDER = FTP_BASE_URL + "content/";
        
        /// <summary>
        /// URL of the content manifest file
        /// </summary>
        public const string FTP_GAME_CONTENT_FILE = FTP_BASE_URL + "content.xml";
        
        /// <summary>
        /// URL of the game version info file
        /// </summary>
        public const string FTP_GAME_VERSION_FILE = FTP_BASE_URL + "content.version";
        
        /// <summary>
        /// URL of the changelog file
        /// </summary>
        public const string FTP_GAME_CHANGES_FILE = FTP_BASE_URL + "content.changes";
        
        /// <summary>
        /// URL of the launcher version info file
        /// </summary>
        public const string FTP_LAUNCHER_VERSION_FILE = FTP_BASE_URL + "launcher.version";
        #endregion

        #region Launch Settings
        public const string GAME_EXE_FILE_NAME = "YourGame.exe";
        
        public const string GAME_EXE_ARGUMENTS = "";
        #endregion
    }

    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            Thread.Sleep(500);
            if (File.Exists(Application.ExecutablePath + ".old"))
                File.Delete(Application.ExecutablePath + ".old");

            var launcherUpdateInfo = LauncherUpdater.CheckForLauncherUpdate();

            if (launcherUpdateInfo.UpdateAvailable)
            {
                MessageBox.Show(Localization.GetText("LauncherUpdate.Checked.Text"), Localization.GetText("LauncherUpdate.Checked.Title"), MessageBoxButtons.OK, MessageBoxIcon.Information);
                LauncherUpdater.UpdateLauncher(launcherUpdateInfo);
                Process.Start(Application.ExecutablePath);
                return;
            }
            Application.Run(new FormLauncher());
        }
    }
}
