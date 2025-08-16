using CircleToSearch.Wpf.Core.WinApi;
using CircleToSearch.Wpf.Services.Abstractions;
using System.Drawing;
using System.Windows.Interop;
using System.Windows.Media.Imaging;
using System.Windows; // Int32Rect�� ���� �ʿ��մϴ�.

namespace CircleToSearch.Wpf.Services.Capture;

public class ScreenCaptureService : IScreenCaptureService
{
    public BitmapSource? CaptureScreen(Rectangle region)
    {
        // ��ü ȭ���� ����̽� ���ؽ�Ʈ(DC)�� �����ɴϴ�.
        IntPtr screenDc = GetDC(IntPtr.Zero);
        // ȭ�� DC�� ȣȯ�Ǵ� �޸� DC�� �����մϴ�.
        IntPtr memDc = NativeMethods.CreateCompatibleDC(screenDc);
        // ������ ���� ũ���� ��Ʈ���� �����մϴ�.
        IntPtr hBitmap = NativeMethods.CreateCompatibleBitmap(screenDc, region.Width, region.Height);
        // ������ ��Ʈ���� �޸� DC�� �����Ͽ� �׸��� �׸� �غ� �մϴ�.
        IntPtr hOldBitmap = NativeMethods.SelectObject(memDc, hBitmap);

        // ȭ�� DC�� ������ ������ �޸� DC�� �����մϴ� (BitBlt).
        NativeMethods.BitBlt(memDc, 0, 0, region.Width, region.Height, screenDc, region.X, region.Y, NativeMethods.SRCCOPY);

        // ��Ʈ���� �ٽ� ������� �ǵ����ϴ�.
        NativeMethods.SelectObject(memDc, hOldBitmap);

        // ����� DC�� ��Ʈ�� �ڵ��� �����մϴ�.
        NativeMethods.DeleteDC(memDc);
        ReleaseDC(IntPtr.Zero, screenDc);

        // GDI ��Ʈ�� �ڵ�(HBITMAP)�� WPF���� ����� �� �ִ� BitmapSource�� ��ȯ�մϴ�.
        BitmapSource? bitmapSource = Imaging.CreateBitmapSourceFromHBitmap(
            hBitmap,
            IntPtr.Zero,
            Int32Rect.Empty, // <--- ������ �κ�
            BitmapSizeOptions.FromEmptyOptions());

        // GDI ��Ʈ�� ��ü�� �����Ͽ� �޸� ������ �����մϴ�.
        NativeMethods.DeleteObject(hBitmap);

        return bitmapSource;
    }

    // GetDC�� ReleaseDC�� user32.dll�� �����Ƿ�, WinApi ������ �߰��ص� �����ϴ�.
    [System.Runtime.InteropServices.DllImport("user32.dll")]
    private static extern IntPtr GetDC(IntPtr hWnd);

    [System.Runtime.InteropServices.DllImport("user32.dll")]
    private static extern int ReleaseDC(IntPtr hWnd, IntPtr hDC);
}
