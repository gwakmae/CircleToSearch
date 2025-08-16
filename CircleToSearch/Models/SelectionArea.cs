using System.Windows;
using System.Windows.Media; // PointCollection을 위해 추가

namespace CircleToSearch.Wpf.Models
{
    /// <summary>
    /// 사용자가 화면에서 선택한 영역의 정보 (경계 사각형과 실제 경로)를 담습니다.
    /// </summary>
    public readonly record struct SelectionArea(Rect Bounds, PointCollection PathPoints);
}