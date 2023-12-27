using System;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Windows.Forms;

namespace NetManager
{
    public class TrayIcon
    {
        private NotifyIcon notifyIcon;
        private ContextMenuStrip contextMenuStrip;
        private MainWindow mainWindow;

        public TrayIcon()
        {
            notifyIcon = new NotifyIcon
            {
                Icon = GetIconFromResource("icon.ico"),
                Visible = true
            };
            string? openAppMenuText = System.Windows.Application.Current.Resources["showFromTray"] as string;
            string? exitAppMenuText = System.Windows.Application.Current.Resources["quitFromTray"] as string;
            contextMenuStrip = new ContextMenuStrip();
            contextMenuStrip.Items.Add(openAppMenuText, null, OnOpenClick);
            contextMenuStrip.Items.Add(exitAppMenuText, null, OnExitClick);

            notifyIcon.ContextMenuStrip = contextMenuStrip;

            notifyIcon.DoubleClick += OnTrayIconDoubleClick;
        }

        private Icon GetIconFromResource(string resourceName)
        {
            Assembly assembly = Assembly.GetExecutingAssembly();

            using (Stream iconStream = assembly.GetManifestResourceStream(resourceName))
            {
                if (iconStream != null)
                {
                    return new Icon(iconStream);
                }
                else
                {
                    // Handle the case where the resource is not found
                    return null;
                }
            }
        }

        private void OnTrayIconDoubleClick(object sender, EventArgs e)
        {
            MainWindow.Instance.WindowState = System.Windows.WindowState.Normal;
            MainWindow.Instance.Show();
            MainWindow.Instance.ShowInTaskbar = true;
        }
        private void OnOpenClick(object sender, EventArgs e)
        {
            if(MainWindow.Instance.WindowState == System.Windows.WindowState.Minimized)
            {
                MainWindow.Instance.WindowState = System.Windows.WindowState.Normal;
                MainWindow.Instance.Show();
                MainWindow.Instance.ShowInTaskbar = true;
            }
        }
        private void OnExitClick(object sender, EventArgs e)
        {
            System.Windows.Application.Current.Shutdown();
        }
        public void Dispose()
        {
            if (notifyIcon != null)
            {
                notifyIcon.Visible = false;
                notifyIcon.Dispose();
            }
        }
    }
}
