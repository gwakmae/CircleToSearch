// File: C:\Users\Public\Documents\C#_Code\CircleToSearch\CircleToSearch\Components\SystemTrayIcon.cs

using System.Drawing; // System.Drawing.Icon을 위해 필요
using System.Windows;
using System.Windows.Forms; // NotifyIcon, ContextMenuStrip 등을 위해 필요

namespace CircleToSearch.Wpf.Components
{
    public class SystemTrayIcon : IDisposable
    {
        private readonly NotifyIcon _notifyIcon;

        public SystemTrayIcon()
        {
            _notifyIcon = new NotifyIcon();

            // 아이콘 및 툴팁 설정
            _notifyIcon.Icon = new Icon("Assets/Images/tray_icon.ico");
            _notifyIcon.Text = "Circle to Search\n(Shortcut: Alt+Shift+S)"; // 마우스를 올리면 보이는 툴팁
            _notifyIcon.Visible = true;

            // 우클릭 메뉴 설정
            _notifyIcon.ContextMenuStrip = new ContextMenuStrip();
            _notifyIcon.ContextMenuStrip.Items.Add("How to use", null, OnShowHelpClicked);
            _notifyIcon.ContextMenuStrip.Items.Add("About", null, OnShowAboutClicked);
            _notifyIcon.ContextMenuStrip.Items.Add(new ToolStripSeparator()); // 구분선
            _notifyIcon.ContextMenuStrip.Items.Add("Exit", null, OnExitClicked);
        }

        private void OnShowHelpClicked(object? sender, EventArgs e)
        {
            string helpText = "1. Press Alt + Shift + S to start capture mode.\n\n" +
                              "2. Freely draw on the screen to select the area you want to search.\n\n" +
                              "3. Release the mouse button to start the search automatically.";

            System.Windows.MessageBox.Show(helpText, "How to use", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void OnShowAboutClicked(object? sender, EventArgs e)
        {
            string aboutText = "Circle to Search for Windows\n\n" +
                               "Version 1.0.0\n\n" +
                               "Capture and search any part of your screen.";

            System.Windows.MessageBox.Show(aboutText, "About", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void OnExitClicked(object? sender, EventArgs e)
        {
            // 애플리케이션을 완전히 종료합니다.
            System.Windows.Application.Current.Shutdown();
        }

        public void Dispose()
        {
            _notifyIcon.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}