using CircleToSearch.Wpf.Core.WinApi;
using CircleToSearch.Wpf.Services.Abstractions;
using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media.Imaging;

namespace CircleToSearch.Wpf.Services.Capture
{
    public class ScreenCaptureService : IScreenCaptureService
    {
        public BitmapSource? CaptureScreen(Rectangle region)
        {
            if (region.Width <= 0 || region.Height <= 0)
                return null;

            IntPtr screenDc = GetDC(IntPtr.Zero);
            IntPtr memDc = NativeMethods.CreateCompatibleDC(screenDc);
            IntPtr hBitmap = NativeMethods.CreateCompatibleBitmap(screenDc, region.Width, region.Height);
            IntPtr hOld = NativeMethods.SelectObject(memDc, hBitmap);

            try
            {
                NativeMethods.BitBlt(memDc, 0, 0, region.Width, region.Height,
                                     screenDc, region.X, region.Y, NativeMethods.SRCCOPY);

                var bmp = Imaging.CreateBitmapSourceFromHBitmap(
                    hBitmap, IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());

                // 선택: Freeze 해서 UI 스레드 외 사용 가능
                if (bmp.CanFreeze) bmp.Freeze();
                return bmp;
            }
            finally
            {
                NativeMethods.SelectObject(memDc, hOld);
                NativeMethods.DeleteObject(hBitmap);
                NativeMethods.DeleteDC(memDc);
                ReleaseDC(IntPtr.Zero, screenDc);
            }
        }

        [DllImport("user32.dll")] private static extern IntPtr GetDC(IntPtr hWnd);
        [DllImport("user32.dll")] private static extern int ReleaseDC(IntPtr hWnd, IntPtr hDC);
    }
}
