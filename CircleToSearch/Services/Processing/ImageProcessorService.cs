// File: Services/Processing/ImageProcessorService.cs
using CircleToSearch.Wpf.Services.Abstractions;
using System;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace CircleToSearch.Wpf.Services.Processing
{
    public class ImageProcessorService : IImageProcessorService
    {
        public string ProcessAndEncodeAsBase64(BitmapSource source, PointCollection pathPointsDips, Rect boundsDips)
        {
            var clippedImage = ClipToFreeformPath(source, pathPointsDips, boundsDips);
            var encoder = new PngBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(clippedImage));
            using (var ms = new MemoryStream())
            {
                encoder.Save(ms);
                byte[] imageBytes = ms.ToArray();
                return $"data:image/png;base64,{Convert.ToBase64String(imageBytes)}";
            }
        }

        private RenderTargetBitmap ClipToFreeformPath(BitmapSource source, PointCollection pathPointsDips, Rect boundsDips)
        {
            var normalizedPoints = pathPointsDips.Select(p => new System.Windows.Point(p.X - boundsDips.Left, p.Y - boundsDips.Top));
            var figure = new PathFigure
            {
                StartPoint = normalizedPoints.First(),
                IsClosed = true,
                IsFilled = true
            };
            figure.Segments.Add(new PolyLineSegment(normalizedPoints.Skip(1), true));

            double safeWidth = Math.Max(1, boundsDips.Width);
            double safeHeight = Math.Max(1, boundsDips.Height);
            double scaleX = source.PixelWidth / safeWidth;
            double scaleY = source.PixelHeight / safeHeight;

            var scaleTransform = new ScaleTransform(scaleX, scaleY);
            var geometry = new PathGeometry(new[] { figure })
            {
                Transform = scaleTransform
            };

            var visual = new DrawingVisual();
            using (var dc = visual.RenderOpen())
            {
                dc.PushClip(geometry);
                dc.DrawImage(source, new Rect(0, 0, source.PixelWidth, source.PixelHeight));
                dc.Pop();
            }

            var renderBitmap = new RenderTargetBitmap(
                source.PixelWidth, source.PixelHeight, 96, 96, PixelFormats.Pbgra32);
            renderBitmap.Render(visual);
            return renderBitmap;
        }
    }
}