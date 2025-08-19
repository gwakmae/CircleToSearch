using System;
using System.Drawing;

namespace CircleToSearch.Wpf.Core.Display
{
    public sealed class MonitorInfo
    {
        public IntPtr Handle { get; init; }
        public Rectangle PixelBounds { get; init; }   // (0,0) = 가상 스크린 기준
        public double ScaleX { get; init; } = 1.0;
        public double ScaleY { get; init; } = 1.0;
        public override string ToString() => $"Handle={Handle}, Bounds={PixelBounds}, Scale=({ScaleX},{ScaleY})";
    }
}
