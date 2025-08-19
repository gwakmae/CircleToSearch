using System.Drawing; // Rectangle
using System.Windows;
using System.Windows.Media;

namespace CircleToSearch.Wpf.Models
{
    /// <summary>
    /// ����ڰ� ������ ����:
    /// Bounds (DIP ����, ���� ����� ���),
    /// PathPoints (�׸� �����),
    /// PixelRect (���� ȭ�� ��ǥ�� �ȼ� ���� �簢��)
    /// </summary>
    public readonly record struct SelectionArea(
        Rect Bounds,
        PointCollection PathPoints,
        Rectangle PixelRect
    );
}
