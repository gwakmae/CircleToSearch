// File: Core/Display/MonitorHelper.cs
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.InteropServices;

namespace CircleToSearch.Wpf.Core.Display
{
    internal static class MonitorHelper
    {
        // Win32 구조체
        [StructLayout(LayoutKind.Sequential)]
        private struct POINT { public int X; public int Y; }

        [StructLayout(LayoutKind.Sequential)]
        private struct RECT { public int Left; public int Top; public int Right; public int Bottom; }

        // Win32 함수
        [DllImport("user32.dll")]
        private static extern bool GetCursorPos(out POINT lpPoint);

        [DllImport("user32.dll")]
        private static extern IntPtr MonitorFromPoint(POINT pt, uint flags);

        private const uint MONITOR_DEFAULTTONEAREST = 2;

        [DllImport("user32.dll")]
        private static extern bool EnumDisplayMonitors(
            IntPtr hdc, IntPtr lprcClip, MonitorEnumProc lpfnEnum, IntPtr dwData);

        private delegate bool MonitorEnumProc(IntPtr hMonitor, IntPtr hdcMonitor, ref RECT lprcMonitor, IntPtr dwData);

        // DPI
        [DllImport("shcore.dll")]
        private static extern int GetDpiForMonitor(
            IntPtr hmonitor, MonitorDpiType dpiType, out uint dpiX, out uint dpiY);

        private enum MonitorDpiType
        {
            MDT_EFFECTIVE_DPI = 0,
            MDT_ANGULAR_DPI = 1,
            MDT_RAW_DPI = 2
        }

        public static IList<MonitorInfo> GetAllMonitors()
        {
            var list = new List<MonitorInfo>();

            EnumDisplayMonitors(IntPtr.Zero, IntPtr.Zero,
                (IntPtr hMon, IntPtr hdcMon, ref RECT rc, IntPtr data) =>
                {
                    var bounds = Rectangle.FromLTRB(rc.Left, rc.Top, rc.Right, rc.Bottom);

                    double scaleX = 1.0;
                    double scaleY = 1.0;

                    try
                    {
                        if (GetDpiForMonitor(hMon, MonitorDpiType.MDT_EFFECTIVE_DPI, out uint dx, out uint dy) == 0)
                        {
                            scaleX = dx / 96.0;
                            scaleY = dy / 96.0;
                        }
                    }
                    catch (DllNotFoundException)
                    {
                        // (구형 Windows) shcore.dll 없으면 100% 가정
                    }

                    list.Add(new MonitorInfo
                    {
                        Handle = hMon,
                        PixelBounds = bounds,
                        ScaleX = scaleX,
                        ScaleY = scaleY
                    });
                    return true;
                }, IntPtr.Zero);

            return list;
        }

        public static MonitorInfo GetMonitorUnderCursorOrPrimary()
        {
            var monitors = GetAllMonitors();
            if (monitors.Count == 0)
                throw new InvalidOperationException("모니터를 열거하지 못했습니다.");

            GetCursorPos(out var p);
            var h = MonitorFromPoint(p, MONITOR_DEFAULTTONEAREST);

            foreach (var m in monitors)
            {
                if (m.Handle == h)
                    return m;
            }

            // 못 찾으면 첫 번째 (주 모니터일 가능성 높음)
            return monitors[0];
        }
    }
}
