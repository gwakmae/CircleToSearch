using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace CircleToSearch.Wpf.Services.Abstractions
{
    public interface IImageProcessorService
    {
        /// <summary>
        /// 이미지를 주어진 경로로 자르고 Base64 문자열로 인코딩합니다.
        /// </summary>
        /// <param name="source">캡처된 원본 사각 이미지</param>
        /// <param name="pathPoints">사용자가 그린 경로의 점들</param>
        /// <param name="bounds">경로의 경계 사각형</param>
        /// <returns>처리된 이미지의 Base64 문자열</returns>
        string ProcessAndEncodeAsBase64(BitmapSource source, PointCollection pathPoints, Rect bounds);
    }
}