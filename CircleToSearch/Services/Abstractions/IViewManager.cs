using CircleToSearch.Wpf.Models;

namespace CircleToSearch.Wpf.Services.Abstractions;

public interface IViewManager
{
    /// <summary>
    /// �������� â�� ���� ������� ���� ������ �޾ƿɴϴ�.
    /// </summary>
    /// <returns>����ڰ� ������ ���� ����. ��� �� null.</returns>
    SelectionArea? ShowOverlayAndGetSelection();

    /// <summary>
    /// ���� â�� �����ְ�, ĸó�� �̹����� �˻��� �����ϵ��� �����մϴ�.
    /// </summary>
    /// <param name="base64ImageData">�˻��� �̹����� Base64 ������</param>
    void ShowMainWindowAndSearch(string base64ImageData);
}