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
            // 경로는 실행 파일 기준 상대 경로입니다.
            _notifyIcon.Icon = new Icon("Assets/Images/tray_icon.ico");
            _notifyIcon.Text = "Circle to Search\n(활성: Alt+Shift+S)"; // 마우스를 올리면 보이는 툴팁
            _notifyIcon.Visible = true;

            // 우클릭 메뉴 설정
            _notifyIcon.ContextMenuStrip = new ContextMenuStrip();
            _notifyIcon.ContextMenuStrip.Items.Add("사용법", null, OnShowHelpClicked);
            _notifyIcon.ContextMenuStrip.Items.Add("정보", null, OnShowAboutClicked);
            _notifyIcon.ContextMenuStrip.Items.Add(new ToolStripSeparator()); // 구분선
            _notifyIcon.ContextMenuStrip.Items.Add("종료", null, OnExitClicked);
        }

        private void OnShowHelpClicked(object? sender, EventArgs e)
        {
            string helpText = "1. Alt + Shift + S 키를 눌러 캡처 모드를 시작하세요.\n\n" +
                              "2. 화면에서 검색하고 싶은 영역을 자유롭게 그리세요.\n\n" +
                              "3. 마우스 버튼을 놓으면 자동으로 검색이 시작됩니다.";

            System.Windows.MessageBox.Show(helpText, "사용법", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void OnShowAboutClicked(object? sender, EventArgs e)
        {
            string aboutText = "Circle to Search for Windows\n\n" +
                               "Version 1.0.0\n\n" +
                               "화면의 어느 곳이든 캡처하여 검색하는 프로그램입니다.";

            System.Windows.MessageBox.Show(aboutText, "정보", MessageBoxButton.OK, MessageBoxImage.Information);
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