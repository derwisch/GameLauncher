namespace GameLauncher.UI
{
    partial class FormLauncher
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.buttonQuit = new System.Windows.Forms.Button();
            this.flowLayoutPanelLinks = new System.Windows.Forms.FlowLayoutPanel();
            this.groupBoxLaunch = new System.Windows.Forms.GroupBox();
            this.progressBarUpdate = new System.Windows.Forms.ProgressBar();
            this.buttonLaunch = new System.Windows.Forms.Button();
            this.groupBoxChangelog = new System.Windows.Forms.GroupBox();
            this.webBrowserChangelog = new System.Windows.Forms.WebBrowser();
            this.labelLauncherVersion = new System.Windows.Forms.Label();
            this.labelGameVersion = new System.Windows.Forms.Label();
            this.groupBoxLaunch.SuspendLayout();
            this.groupBoxChangelog.SuspendLayout();
            this.SuspendLayout();
            // 
            // buttonQuit
            // 
            this.buttonQuit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonQuit.Location = new System.Drawing.Point(512, 46);
            this.buttonQuit.Name = "buttonQuit";
            this.buttonQuit.Size = new System.Drawing.Size(75, 23);
            this.buttonQuit.TabIndex = 6;
            this.buttonQuit.Text = "Quit";
            this.buttonQuit.UseVisualStyleBackColor = true;
            this.buttonQuit.Click += new System.EventHandler(this.ButtonQuit_Click);
            // 
            // flowLayoutPanelLinks
            // 
            this.flowLayoutPanelLinks.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.flowLayoutPanelLinks.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.flowLayoutPanelLinks.Location = new System.Drawing.Point(3, 3);
            this.flowLayoutPanelLinks.Name = "flowLayoutPanelLinks";
            this.flowLayoutPanelLinks.Padding = new System.Windows.Forms.Padding(5);
            this.flowLayoutPanelLinks.Size = new System.Drawing.Size(180, 519);
            this.flowLayoutPanelLinks.TabIndex = 1;
            // 
            // groupBoxLaunch
            // 
            this.groupBoxLaunch.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxLaunch.Controls.Add(this.progressBarUpdate);
            this.groupBoxLaunch.Controls.Add(this.buttonLaunch);
            this.groupBoxLaunch.Controls.Add(this.buttonQuit);
            this.groupBoxLaunch.Location = new System.Drawing.Point(189, 482);
            this.groupBoxLaunch.Name = "groupBoxLaunch";
            this.groupBoxLaunch.Size = new System.Drawing.Size(590, 75);
            this.groupBoxLaunch.TabIndex = 2;
            this.groupBoxLaunch.TabStop = false;
            this.groupBoxLaunch.Text = "Update";
            // 
            // progressBarUpdate
            // 
            this.progressBarUpdate.Location = new System.Drawing.Point(6, 17);
            this.progressBarUpdate.Name = "progressBarUpdate";
            this.progressBarUpdate.Size = new System.Drawing.Size(500, 52);
            this.progressBarUpdate.TabIndex = 7;
            // 
            // buttonLaunch
            // 
            this.buttonLaunch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonLaunch.Location = new System.Drawing.Point(512, 17);
            this.buttonLaunch.Name = "buttonLaunch";
            this.buttonLaunch.Size = new System.Drawing.Size(75, 23);
            this.buttonLaunch.TabIndex = 5;
            this.buttonLaunch.Text = "Launch";
            this.buttonLaunch.UseVisualStyleBackColor = true;
            this.buttonLaunch.Click += new System.EventHandler(this.ButtonLaunch_Click);
            // 
            // groupBoxChangelog
            // 
            this.groupBoxChangelog.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxChangelog.Controls.Add(this.webBrowserChangelog);
            this.groupBoxChangelog.Location = new System.Drawing.Point(189, 3);
            this.groupBoxChangelog.Name = "groupBoxChangelog";
            this.groupBoxChangelog.Size = new System.Drawing.Size(590, 473);
            this.groupBoxChangelog.TabIndex = 0;
            this.groupBoxChangelog.TabStop = false;
            this.groupBoxChangelog.Text = "Changelog";
            // 
            // webBrowserChangelog
            // 
            this.webBrowserChangelog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.webBrowserChangelog.IsWebBrowserContextMenuEnabled = false;
            this.webBrowserChangelog.Location = new System.Drawing.Point(3, 16);
            this.webBrowserChangelog.MinimumSize = new System.Drawing.Size(20, 20);
            this.webBrowserChangelog.Name = "webBrowserChangelog";
            this.webBrowserChangelog.ScriptErrorsSuppressed = true;
            this.webBrowserChangelog.Size = new System.Drawing.Size(584, 454);
            this.webBrowserChangelog.TabIndex = 8;
            this.webBrowserChangelog.WebBrowserShortcutsEnabled = false;
            // 
            // labelLauncherVersion
            // 
            this.labelLauncherVersion.AutoSize = true;
            this.labelLauncherVersion.Enabled = false;
            this.labelLauncherVersion.ForeColor = System.Drawing.Color.DimGray;
            this.labelLauncherVersion.Location = new System.Drawing.Point(0, 544);
            this.labelLauncherVersion.Name = "labelLauncherVersion";
            this.labelLauncherVersion.Size = new System.Drawing.Size(105, 13);
            this.labelLauncherVersion.TabIndex = 4;
            this.labelLauncherVersion.Text = "Launcher Version: ...";
            // 
            // labelGameVersion
            // 
            this.labelGameVersion.AutoSize = true;
            this.labelGameVersion.Enabled = false;
            this.labelGameVersion.ForeColor = System.Drawing.Color.DimGray;
            this.labelGameVersion.Location = new System.Drawing.Point(0, 525);
            this.labelGameVersion.Name = "labelGameVersion";
            this.labelGameVersion.Size = new System.Drawing.Size(88, 13);
            this.labelGameVersion.TabIndex = 3;
            this.labelGameVersion.Text = "Game Version: ...";
            // 
            // FormLauncher
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(784, 561);
            this.Controls.Add(this.labelGameVersion);
            this.Controls.Add(this.labelLauncherVersion);
            this.Controls.Add(this.groupBoxChangelog);
            this.Controls.Add(this.groupBoxLaunch);
            this.Controls.Add(this.flowLayoutPanelLinks);
            this.Name = "FormLauncher";
            this.Text = "FormMain";
            this.groupBoxLaunch.ResumeLayout(false);
            this.groupBoxChangelog.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button buttonQuit;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanelLinks;
        private System.Windows.Forms.GroupBox groupBoxLaunch;
        private System.Windows.Forms.Button buttonLaunch;
        private System.Windows.Forms.GroupBox groupBoxChangelog;
        private System.Windows.Forms.WebBrowser webBrowserChangelog;
        private System.Windows.Forms.Label labelLauncherVersion;
        private System.Windows.Forms.Label labelGameVersion;
        private System.Windows.Forms.ProgressBar progressBarUpdate;
    }
}