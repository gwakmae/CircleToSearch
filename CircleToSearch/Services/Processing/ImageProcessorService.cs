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
            // 1. 이미지를 자유 곡선 경로로 클리핑(자르기)
            var clippedImage = ClipToFreeformPath(source, pathPoints, bounds);

            // 2. 클리핑된 이미지를 PNG 형식으로 인코딩
            var encoder = new PngBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(clippedImage));

            // 3. 메모리 스트림에 저장 후 Base64 문자열로 변환
            using (var ms = new MemoryStream())
            {
                encoder.Save(ms);
                byte[] imageBytes = ms.ToArray();
                return $"data:image/png;base64,{Convert.ToBase64String(imageBytes)}";
            }
        }

        private RenderTargetBitmap ClipToFreeformPath(BitmapSource source, PointCollection pathPoints, Rect bounds)
        {
            // 경로의 점들을 경계 사각형의 (0,0)을 기준으로 하는 상대 좌표로 변환
            var normalizedPoints = pathPoints.Select(p => new System.Windows.Point(p.X - bounds.Left, p.Y - bounds.Top)); // 수정됨

            // WPF Geometry 객체 생성
            var figure = new PathFigure
            {
                StartPoint = normalizedPoints.First(),
                IsClosed = true, // 경로를 닫아 영역을 만듬
                IsFilled = true
            };
            figure.Segments.Add(new PolyLineSegment(normalizedPoints.Skip(1), true));
            var geometry = new PathGeometry(new[] { figure });

            var visual = new DrawingVisual();
            using (var dc = visual.RenderOpen())
            {
                // 생성된 Geometry로 클리핑 영역 설정
                dc.PushClip(geometry);
                // 클리핑 영역 안에 원본 이미지를 그림
                dc.DrawImage(source, new Rect(0, 0, source.PixelWidth, source.PixelHeight));
                dc.Pop();
            }

            // DrawingVisual의 결과물을 새로운 비트맵으로 렌더링
            var renderBitmap = new RenderTargetBitmap(
                source.PixelWidth, source.PixelHeight, 96, 96, PixelFormats.Pbgra32);
            renderBitmap.Render(visual);

            return renderBitmap;
        }
    }
}
