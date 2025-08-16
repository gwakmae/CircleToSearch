using CircleToSearch.Wpf.Models;
using CircleToSearch.Wpf.Services.Abstractions;
using CircleToSearch.Wpf.Views;
using System.Threading.Tasks;

namespace CircleToSearch.Wpf.Services.UI
{
    public class WpfViewManager : IViewManager
    {
        private MainWindow? _mainWindow;

        public SelectionArea? ShowOverlayAndGetSelection()
        {
            // --- 이 부분을 수정하세요 ---
            // var overlay = new ScreenOverlay_final_Window(); -> var overlay = new ScreenOverlayWindow();
            var overlay = new ScreenOverlayWindow();
            // -------------------------
            bool? result = overlay.ShowDialog();

            if (result == true)
            {
                return overlay.SelectedArea;
            }
            return null;
        }

        public async void ShowMainWindowAndSearch(string? base64ImageData)
        {
            if (_mainWindow == null)
            {
                _mainWindow = new MainWindow();
                _mainWindow.Closed += (sender, args) => _mainWindow = null;
                _mainWindow.Show();
            }
            else
            {
                _mainWindow.Activate();
            }

            if (!string.IsNullOrEmpty(base64ImageData))
            {
                if (!_mainWindow.IsLoaded)
                {
                    await Task.Delay(100);
                }
                await _mainWindow.SearchWithImage(base64ImageData);
            }
        }
    }
}