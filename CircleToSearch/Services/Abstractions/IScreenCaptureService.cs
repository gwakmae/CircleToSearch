using System.Drawing;
using System.Windows.Media.Imaging;

namespace CircleToSearch.Wpf.Services.Abstractions
{
    public interface IScreenCaptureService
    {
        BitmapSource? CaptureScreen(Rectangle region);
    }
}