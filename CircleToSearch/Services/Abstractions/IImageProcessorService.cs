// File: Services/Abstractions/IImageProcessorService.cs
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace CircleToSearch.Wpf.Services.Abstractions
{
    public interface IImageProcessorService
    {
        string ProcessAndEncodeAsBase64(
            BitmapSource source,
            PointCollection pathPointsDips,
            Rect boundsDips
        );
    }
}