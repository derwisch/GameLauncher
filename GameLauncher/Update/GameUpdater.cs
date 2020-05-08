using GameLauncher.Misc;
using GameLauncher.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Net;
using System.Windows.Forms;

namespace GameLauncher.Update
{
    internal class GameUpdater
    {
        public event UpdaterProgressUpdateEventHandler ProgressUpdate;
        public event UpdaterStatusUpdateEventHandler StatusUpdate;
        public event UpdaterStatusUpdateEventHandler ChangelogLoaded;
        public event UpdaterStatusUpdateEventHandler GameVersionDetected;
        public event EventHandler UpdateFinished;
        public event EventHandler UpdateFailed;

        private readonly List<HashGenerator.FileStateInfo> corruptFiles = new List<HashGenerator.FileStateInfo>();
        private HashGenerator.ContentFile contentFile;
        private BackgroundWorker contentFileDownloadWorker;
        private BackgroundWorker fileCheckWorker;
        private BackgroundWorker fileUpdateWorker;
        private int filesUpdated;

        public void Start()
        {
            contentFileDownloadWorker = new BackgroundWorker();
            contentFileDownloadWorker.DoWork += ContentFilesDownload;
            contentFileDownloadWorker.RunWorkerCompleted += ContentFilesDownloadFinished;
            contentFileDownloadWorker.RunWorkerAsync();
        }

        public void Stop()
        {
            if (contentFileDownloadWorker?.IsBusy ?? false)
            {
                contentFileDownloadWorker.CancelAsync();
            }
            if (fileCheckWorker?.IsBusy ?? false)
            {
                fileCheckWorker.CancelAsync();
            }
            if (fileUpdateWorker?.IsBusy ?? false)
            {
                fileUpdateWorker.CancelAsync();
            }
        }

        private void ContentFilesDownload(object sender, DoWorkEventArgs e)
        {
            StatusUpdate?.Invoke(Localization.GetText("GameUpdater.Status.DownloadingIndexFiles"));
            WebClient webClient = Helpers.BuildWebClient();

            if (File.Exists("content.xml"))
            {
                File.Delete("content.xml");
            }
            if (File.Exists("content.version"))
            {
                File.Delete("content.version");
            }
            if (File.Exists("content.changes"))
            {
                File.Delete("content.changes");
            }

            try
            {
                webClient.DownloadFile(LauncherSetup.FTP_GAME_CONTENT_FILE, "content.xml");
            }
            catch
            {
                Logger.Log($"{Localization.GetText("GameUpdater.Log.FailedToDownload")} \"content.xml\"");
            }

            try
            {
                webClient.DownloadFile(LauncherSetup.FTP_GAME_VERSION_FILE, "content.version");
            }
            catch
            {
                Logger.Log($"{Localization.GetText("GameUpdater.Log.FailedToDownload")} \"content.version\"");
            }

            try
            {
                webClient.DownloadFile(LauncherSetup.FTP_GAME_CHANGES_FILE, "content.changes");
            }
            catch
            {
                Logger.Log($"{Localization.GetText("GameUpdater.Log.FailedToDownload")} \"content.changes\"");
            }

            contentFile = HashGenerator.ContentFile.FromFile(HashGenerator.RootDirectory.CombinePath("content.xml"));
            if (contentFile != null)
            {
                return;
            }

            MessageBox.Show(Localization.GetText("GameUpdater.Error.CannotReadContentFile"), "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            e.Cancel = true;
        }

        private void ContentFilesDownloadFinished(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Cancelled)
            {
                var message = Localization.GetText("GameUpdater.Message.CannotReadContentFile");

#pragma warning disable CS0162
                if (LauncherSetup.IS_CHANGELOG_HTML)
                {
                    //only wrap in HTML if we already expect an HTML document
                    message = message.WrapAsHTML();
                }
#pragma warning restore CS0162

                ChangelogLoaded?.Invoke(message);
                UpdateFailed?.Invoke(this, EventArgs.Empty);
            }
            else
            {
                if (File.Exists("content.changes"))
                {
                    ChangelogLoaded?.Invoke(File.ReadAllText("content.changes"));
                }
                StatusUpdate?.Invoke(Localization.GetText("GameUpdater.Status.CheckingLocalFiles"));
                GameVersionDetected?.Invoke(contentFile.GameVersion);

                fileCheckWorker = new BackgroundWorker();
                fileCheckWorker.DoWork += CheckFiles;
                fileCheckWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(this.CheckFilesFinished);
                fileCheckWorker.RunWorkerAsync();
            }
        }

        private void CheckFiles(object sender, DoWorkEventArgs e)
        {
            filesUpdated = 0;
            corruptFiles.Clear();
            if (contentFile == null)
                return;
            foreach (HashGenerator.FileStateInfo file in this.contentFile.Files)
            {
                if (!file.CheckLocal())
                    corruptFiles.Add(file);
            }
        }

        private void CheckFilesFinished(object sender, RunWorkerCompletedEventArgs e)
        {
            if (corruptFiles.Count > 0)
            {
                ReportDownloadStatus();

                fileUpdateWorker = new BackgroundWorker();
                fileUpdateWorker.DoWork += UpdateFiles;
                fileUpdateWorker.RunWorkerCompleted += UpdateFilesFinished;
                fileUpdateWorker.RunWorkerAsync();
            }
            else
            {
                StatusUpdate?.Invoke(Localization.GetText("GameUpdater.Status.Finished"));
                UpdateFinished?.Invoke(this, EventArgs.Empty);
            }
        }

        private void ReportDownloadStatus()
        {
            StatusUpdate?.Invoke($"{Localization.GetText("GameUpdater.Status.DownloadingNeededFiles")} ({filesUpdated}/{corruptFiles.Count})");
            ProgressUpdate?.Invoke(filesUpdated, corruptFiles.Count);
        }

        private void UpdateFiles(object sender, DoWorkEventArgs e)
        {
            foreach (HashGenerator.FileStateInfo corruptFile in corruptFiles)
            {
                RequestFile(corruptFile);
            }
        }

        private void UpdateFilesFinished(object sender, RunWorkerCompletedEventArgs e)
        {
            if (File.Exists("content.xml"))
            {
                File.Delete("content.xml");
            }
            if (File.Exists("content.version"))
            {
                File.Delete("content.version");
            }
            if (File.Exists("content.changes"))
            {
                File.Delete("content.changes");
            }

            StatusUpdate?.Invoke(Localization.GetText("GameUpdater.Status.Finished"));
            UpdateFinished?.Invoke(this, EventArgs.Empty);
        }

        private void RequestFile(HashGenerator.FileStateInfo file)
        {
            WebClient webClient = Helpers.BuildWebClient();
            ++filesUpdated;
            ReportDownloadStatus();
            string uriString = LauncherSetup.FTP_GAME_FILE_UPDATE_FOLDER + file.FileName.Replace('\\', '/');
            string fileName = HashGenerator.RootDirectory.CombinePath(file.FileName);
            FileInfo fileInfo = new FileInfo(fileName);
            if (!Directory.Exists(fileInfo.DirectoryName))
                Directory.CreateDirectory(fileInfo.DirectoryName);
            try
            {
                Uri address = new Uri(uriString);
                webClient.DownloadFile(address, fileName + ".download");
                if (File.Exists(fileName))
                {
                    File.Delete(fileName);
                }
                File.Move(fileName + ".download", fileName);
            }
            catch
            {
                Logger.Log($"{Localization.GetText("GameUpdater.Log.FailedToDownload")} \"{file.FileName}\"");
            }
        }
    }

    delegate void UpdaterStatusUpdateEventHandler(string data);
    delegate void UpdaterProgressUpdateEventHandler(int value, int maximum);
}
