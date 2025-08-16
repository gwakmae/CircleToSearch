using CircleToSearch.Wpf.Core.WinApi;
using CircleToSearch.Wpf.Services.Abstractions;
using System.Drawing;
using System.Windows.Interop;
using System.Windows.Media.Imaging;
using System.Windows; // Int32Rect를 위해 필요합니다.

namespace CircleToSearch.Wpf.Services.Capture;

public class ScreenCaptureService : IScreenCaptureService
{
    public BitmapSource? CaptureScreen(Rectangle region)
    {
        // 전체 화면의 디바이스 컨텍스트(DC)를 가져옵니다.
        IntPtr screenDc = GetDC(IntPtr.Zero);
        // 화면 DC와 호환되는 메모리 DC를 생성합니다.
        IntPtr memDc = NativeMethods.CreateCompatibleDC(screenDc);
        // 지정된 영역 크기의 비트맵을 생성합니다.
        IntPtr hBitmap = NativeMethods.CreateCompatibleBitmap(screenDc, region.Width, region.Height);
        // 생성된 비트맵을 메모리 DC에 선택하여 그림을 그릴 준비를 합니다.
        IntPtr hOldBitmap = NativeMethods.SelectObject(memDc, hBitmap);

        // 화면 DC의 지정된 영역을 메모리 DC로 복사합니다 (BitBlt).
        NativeMethods.BitBlt(memDc, 0, 0, region.Width, region.Height, screenDc, region.X, region.Y, NativeMethods.SRCCOPY);

        // 비트맵을 다시 원래대로 되돌립니다.
        NativeMethods.SelectObject(memDc, hOldBitmap);

        // 사용한 DC와 비트맵 핸들을 정리합니다.
        NativeMethods.DeleteDC(memDc);
        ReleaseDC(IntPtr.Zero, screenDc);

        // GDI 비트맵 핸들(HBITMAP)을 WPF에서 사용할 수 있는 BitmapSource로 변환합니다.
        BitmapSource? bitmapSource = Imaging.CreateBitmapSourceFromHBitmap(
            hBitmap,
            IntPtr.Zero,
            Int32Rect.Empty, // <--- 수정된 부분
            BitmapSizeOptions.FromEmptyOptions());

        // GDI 비트맵 객체를 삭제하여 메모리 누수를 방지합니다.
        NativeMethods.DeleteObject(hBitmap);

        return bitmapSource;
    }

    // GetDC와 ReleaseDC는 user32.dll에 있으므로, WinApi 폴더에 추가해도 좋습니다.
    [System.Runtime.InteropServices.DllImport("user32.dll")]
    private static extern IntPtr GetDC(IntPtr hWnd);

    [System.Runtime.InteropServices.DllImport("user32.dll")]
    private static extern int ReleaseDC(IntPtr hWnd, IntPtr hDC);
}
