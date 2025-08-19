using CircleToSearch.Wpf.Models;

namespace CircleToSearch.Wpf.Services.Abstractions
{
    public interface IViewManager
    {
        SelectionArea? ShowOverlayAndGetSelection();
        void ShowMainWindowAndSearch(string base64ImageData);
    }
}
