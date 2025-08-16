using System.Drawing; // System.Drawing.Common NuGet ��Ű�� �ʿ�
using System.Windows.Media.Imaging;

namespace CircleToSearch.Wpf.Services.Abstractions;

public interface IScreenCaptureService
{
    /// <summary>
    /// ȭ���� ������ �簢 ������ ĸó�մϴ�.
    /// </summary>
    /// <param name="region">ĸó�� ����</param>
    /// <returns>ĸó�� �̹���(WPF��)</returns>
    BitmapSource? CaptureScreen(Rectangle region);
}