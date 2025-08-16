using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace CircleToSearch.Wpf.Services.Abstractions
{
    public interface IImageProcessorService
    {
        /// <summary>
        /// �̹����� �־��� ��η� �ڸ��� Base64 ���ڿ��� ���ڵ��մϴ�.
        /// </summary>
        /// <param name="source">ĸó�� ���� �簢 �̹���</param>
        /// <param name="pathPoints">����ڰ� �׸� ����� ����</param>
        /// <param name="bounds">����� ��� �簢��</param>
        /// <returns>ó���� �̹����� Base64 ���ڿ�</returns>
        string ProcessAndEncodeAsBase64(BitmapSource source, PointCollection pathPoints, Rect bounds);
    }
}