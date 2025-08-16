using System.Drawing; // System.Drawing.Common NuGet 패키지 필요
using System.Windows.Media.Imaging;

namespace CircleToSearch.Wpf.Services.Abstractions;

public interface IScreenCaptureService
{
    /// <summary>
    /// 화면의 지정된 사각 영역을 캡처합니다.
    /// </summary>
    /// <param name="region">캡처할 영역</param>
    /// <returns>캡처된 이미지(WPF용)</returns>
    BitmapSource? CaptureScreen(Rectangle region);
}