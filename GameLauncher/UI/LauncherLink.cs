using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace GameLauncher.UI
{
    class LauncherLink : LinkLabel
    {
        private static ToolTip toolTip = new ToolTip();

        private readonly string url;

        public LauncherLink(string url, string title)
        {
            this.url = url;
            Click += LinkClick;
            Text = title ?? url;
            Margin = new Padding(5,0,5,0);

            toolTip.SetToolTip(this, url);
        }

        private void LinkClick(object sender, EventArgs e)
        {
            Process.Start(url);
        }
    }
}
