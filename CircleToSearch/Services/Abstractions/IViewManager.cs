using CircleToSearch.Wpf.Models;

namespace CircleToSearch.Wpf.Services.Abstractions;

public interface IViewManager
{
    /// <summary>
    /// 오버레이 창을 띄우고 사용자의 선택 영역을 받아옵니다.
    /// </summary>
    /// <returns>사용자가 선택한 영역 정보. 취소 시 null.</returns>
    SelectionArea? ShowOverlayAndGetSelection();

    /// <summary>
    /// 메인 창을 보여주고, 캡처한 이미지로 검색을 시작하도록 지시합니다.
    /// </summary>
    /// <param name="base64ImageData">검색할 이미지의 Base64 데이터</param>
    void ShowMainWindowAndSearch(string base64ImageData);
}