using System.Drawing; // Rectangle
using System.Windows;
using System.Windows.Media;

namespace CircleToSearch.Wpf.Models
{
    /// <summary>
    /// 사용자가 선택한 영역:
    /// Bounds (DIP 단위, 선택 경로의 경계),
    /// PathPoints (그린 자유곡선),
    /// PixelRect (전역 화면 좌표의 픽셀 단위 사각형)
    /// </summary>
    public readonly record struct SelectionArea(
        Rect Bounds,
        PointCollection PathPoints,
        Rectangle PixelRect
    );
}
