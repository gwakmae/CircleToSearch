using CircleToSearch.Wpf.Services.Abstractions;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace CircleToSearch.Wpf.Services.Processing
{
    public class ImageProcessorService : IImageProcessorService
    {
        public string ProcessAndEncodeAsBase64(BitmapSource source, PointCollection pathPoints, Rect bounds)
        {
            // 1. �̹����� ���� � ��η� Ŭ����(�ڸ���)
            var clippedImage = ClipToFreeformPath(source, pathPoints, bounds);

            // 2. Ŭ���ε� �̹����� PNG �������� ���ڵ�
            var encoder = new PngBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(clippedImage));

            // 3. �޸� ��Ʈ���� ���� �� Base64 ���ڿ��� ��ȯ
            using (var ms = new MemoryStream())
            {
                encoder.Save(ms);
                byte[] imageBytes = ms.ToArray();
                return $"data:image/png;base64,{Convert.ToBase64String(imageBytes)}";
            }
        }

        private RenderTargetBitmap ClipToFreeformPath(BitmapSource source, PointCollection pathPoints, Rect bounds)
        {
            // ����� ������ ��� �簢���� (0,0)�� �������� �ϴ� ��� ��ǥ�� ��ȯ
            var normalizedPoints = pathPoints.Select(p => new System.Windows.Point(p.X - bounds.Left, p.Y - bounds.Top)); // ������

            // WPF Geometry ��ü ����
            var figure = new PathFigure
            {
                StartPoint = normalizedPoints.First(),
                IsClosed = true, // ��θ� �ݾ� ������ ����
                IsFilled = true
            };
            figure.Segments.Add(new PolyLineSegment(normalizedPoints.Skip(1), true));
            var geometry = new PathGeometry(new[] { figure });

            var visual = new DrawingVisual();
            using (var dc = visual.RenderOpen())
            {
                // ������ Geometry�� Ŭ���� ���� ����
                dc.PushClip(geometry);
                // Ŭ���� ���� �ȿ� ���� �̹����� �׸�
                dc.DrawImage(source, new Rect(0, 0, source.PixelWidth, source.PixelHeight));
                dc.Pop();
            }

            // DrawingVisual�� ������� ���ο� ��Ʈ������ ������
            var renderBitmap = new RenderTargetBitmap(
                source.PixelWidth, source.PixelHeight, 96, 96, PixelFormats.Pbgra32);
            renderBitmap.Render(visual);

            return renderBitmap;
        }
    }
}
