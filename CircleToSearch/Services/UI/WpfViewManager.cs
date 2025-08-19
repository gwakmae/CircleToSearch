using CircleToSearch.Wpf.Core.Display;
using CircleToSearch.Wpf.Models;
using CircleToSearch.Wpf.Services.Abstractions;
using CircleToSearch.Wpf.Views;

namespace CircleToSearch.Wpf.Services.UI
{
    public class WpfViewManager : IViewManager
    {
        private MainWindow? _mainWindow;

        public SelectionArea? ShowOverlayAndGetSelection()
        {
            // 커서 아래 모니터 획득
            var monitor = MonitorHelper.GetMonitorUnderCursorOrPrimary();

            var overlay = new MonitorOverlayWindow(monitor);
            bool? dlg = overlay.ShowDialog();
            if (dlg == true && overlay.Result.HasValue)
                return overlay.Result.Value;
            return null;
        }

        public async void ShowMainWindowAndSearch(string? base64ImageData)
        {
            if (_mainWindow == null)
            {
                _mainWindow = new MainWindow();
                _mainWindow.Closed += (_, __) => _mainWindow = null;
                _mainWindow.Show();
            }
            else
            {
                _mainWindow.Activate();
            }

            if (!string.IsNullOrEmpty(base64ImageData))
            {
                if (!_mainWindow.IsLoaded)
                    await Task.Delay(100);

                await _mainWindow.SearchWithImage(base64ImageData);
            }
        }
    }
}
