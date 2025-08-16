using System.Windows;
using System.Windows.Media; // PointCollection�� ���� �߰�

namespace CircleToSearch.Wpf.Models
{
    /// <summary>
    /// ����ڰ� ȭ�鿡�� ������ ������ ���� (��� �簢���� ���� ���)�� ����ϴ�.
    /// </summary>
    public readonly record struct SelectionArea(Rect Bounds, PointCollection PathPoints);
}