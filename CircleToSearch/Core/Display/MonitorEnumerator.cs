// File: Core/Display/MonitorEnumerator.cs
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Point = System.Drawing.Point;
using Rectangle = System.Drawing.Rectangle;

namespace CircleToSearch.Wpf.Core.Display
{
    public static class MonitorEnumerator
    {
        [DllImport("user32.dll")]
        static extern IntPtr MonitorFromPoint(Point pt, uint flags);

        [DllImport("shcore.dll")]
        static extern int GetDpiForMonitor(IntPtr hmonitor, int dpiType, out uint dpiX, out uint dpiY);

        const int MDT_EFFECTIVE_DPI = 0;
        const uint MONITOR_DEFAULTTONEAREST = 2;

        public static List<MonitorInfo> GetMonitors()
        {
            var list = new List<MonitorInfo>();
            foreach (var screen in Screen.AllScreens)
            {
                var midPoint = new Point(screen.Bounds.Left + screen.Bounds.Width / 2, screen.Bounds.Top + screen.Bounds.Height / 2);
                IntPtr hMonitor = MonitorFromPoint(midPoint, MONITOR_DEFAULTTONEAREST);

                uint dpiX = 96, dpiY = 96;
                try
                {
                    GetDpiForMonitor(hMonitor, MDT_EFFECTIVE_DPI, out dpiX, out dpiY);
                }
                catch (DllNotFoundException) { /* For older Windows versions */ }

                list.Add(new MonitorInfo
                {
                    Handle = hMonitor,          // midPoint 로 얻은 hMonitor
                    PixelBounds = screen.Bounds,
                    ScaleX = dpiX / 96.0,
                    ScaleY = dpiY / 96.0
                });
            }
            return list;
        }
    }
}