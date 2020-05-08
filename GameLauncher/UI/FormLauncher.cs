using GameLauncher.Misc;
using GameLauncher.Update;
using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace GameLauncher.UI
{
    public partial class FormLauncher : Form
    {
        private readonly GameUpdater updater;

        public FormLauncher()
        {
            InitializeComponent();

            buttonLaunch.Text = Localization.GetText("Launcher.Button.Launch");
            buttonQuit.Text = Localization.GetText("Launcher.Button.Quit");
            groupBoxChangelog.Text = Localization.GetText("Launcher.Label.Changelog");

            Text = LauncherSetup.LAUNCHER_UI_TITLE;

            foreach (var link in LauncherSetup.LauncherLinks)
            {
                try
                {
                    flowLayoutPanelLinks.Controls.Add(link);
                }
                catch { }
            }

            labelLauncherVersion.Text = $"{Localization.GetText("Launcher.Label.LauncherVersion")}: {Application.ProductVersion}";
            buttonLaunch.Enabled = false;

            updater = new GameUpdater();
            updater.StatusUpdate += Updater_StatusUpdate;
            updater.ChangelogLoaded += Updater_ChangelogLoaded;
            updater.GameVersionDetected += Updater_GameVersionDetected;
            updater.ProgressUpdate += Updater_ProgressUpdate;
            updater.UpdateFinished += Updater_UpdateFinished;
            updater.UpdateFailed += Updater_UpdateFailed;
            updater.Start();
        }

        private void Updater_UpdateFailed(object sender, EventArgs e)
        {
            Invoke(new Action(() =>
            {
                groupBoxLaunch.Text = Localization.GetText("Launcher.Label.UpdateFailed");
            }));
        }

        private void Updater_UpdateFinished(object sender, EventArgs e)
        {
            Invoke(new Action(() =>
            {
                buttonLaunch.Enabled = true;
            }));
        }

        private void Updater_ProgressUpdate(int value, int maximum)
        {
            Invoke(new Action(() =>
            {
                progressBarUpdate.Value = value;
                progressBarUpdate.Maximum = maximum;
            }));
        }

        private void Updater_GameVersionDetected(string data)
        {
            Invoke(new Action(() =>
            {
                labelGameVersion.Text = $"{Localization.GetText("Launcher.Label.GameVersion")}: {data}";
            }));
        }

        private void Updater_ChangelogLoaded(string data)
        {
            Invoke(new Action(() =>
            {
#pragma warning disable CS0162
                if (LauncherSetup.IS_CHANGELOG_HTML)
                {
                    displayHtml(data);
                }
                else
                {
                    displayHtml(data.WrapAsHTML());
                }
#pragma warning restore CS0162

            }));

            void displayHtml(string html)
            {
                webBrowserChangelog.Navigate("about:blank");
                try
                {
                    if (webBrowserChangelog.Document != null)
                    {
                        webBrowserChangelog.Document.Write(String.Empty);
                    }
                }
                catch (InvalidCastException) { }
                webBrowserChangelog.DocumentText = html;
            }
        }

        private void Updater_StatusUpdate(string newStatus)
        {
            Invoke(new Action(() =>
            {
                groupBoxLaunch.Text = $"{Localization.GetText("Launcher.Label.Status")}: {newStatus}";
            }));
        }

        private void ButtonLaunch_Click(object sender, EventArgs e)
        {
            try
            {
                Process.Start(LauncherSetup.GAME_EXE_FILE_NAME, LauncherSetup.GAME_EXE_ARGUMENTS);
            }
            catch
            {
                var text = Localization.GetText("Launcher.Error.CannotStartGame.Text");
                var title = Localization.GetText("Launcher.Error.CannotStartGame.Title");

                MessageBox.Show(text, title, MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
        }

        private void ButtonQuit_Click(object sender, EventArgs e)
        {
            updater.Stop();
            Application.Exit();
        }
    }
}
