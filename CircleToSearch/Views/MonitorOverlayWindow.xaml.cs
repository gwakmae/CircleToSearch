using CircleToSearch.Wpf.Core.Display;
using CircleToSearch.Wpf.Models;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Media;


// 별칭 (WinForms/System.Drawing 과 충돌 방지)
using MouseEventArgs = System.Windows.Input.MouseEventArgs;
using MouseButtonEventArgs = System.Windows.Input.MouseButtonEventArgs;
using KeyEventArgs = System.Windows.Input.KeyEventArgs;
using MouseButton = System.Windows.Input.MouseButton;
using MouseButtonState = System.Windows.Input.MouseButtonState;
using WpfPoint = System.Windows.Point;

namespace CircleToSearch.Wpf.Views
{
    public partial class MonitorOverlayWindow : Window
    {
        private readonly MonitorInfo _monitor;
        private bool _isDrawing;
        private PointCollection _points = new();

        public SelectionArea? Result { get; private set; }

        public MonitorOverlayWindow(MonitorInfo monitor)
        {
            InitializeComponent();
            _monitor = monitor;

            // 모니터의 PixelBounds -> 이 창의 위치/크기 (DIP 로 변환)
            Left   = _monitor.PixelBounds.Left / _monitor.ScaleX;
            Top    = _monitor.PixelBounds.Top  / _monitor.ScaleY;
            Width  = Math.Max(1, _monitor.PixelBounds.Width  / _monitor.ScaleX);
            Height = Math.Max(1, _monitor.PixelBounds.Height / _monitor.ScaleY);

            Loaded += (_, __) => System.Windows.Input.Keyboard.Focus(this);
        }

        private void OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                _isDrawing = true;
                _points = new PointCollection();
                FreeformPath.Points = _points;

                var p = e.GetPosition(this);
                _points.Add(p);
                CaptureMouse();
            }
            else if (e.ChangedButton == MouseButton.Right)
            {
                CloseWithoutResult();
            }
        }

        private void OnMouseMove(object sender, MouseEventArgs e)
        {
            if (_isDrawing && e.LeftButton == MouseButtonState.Pressed)
            {
                var p = e.GetPosition(this);
                if (_points.Count == 0 || (p - _points[^1]).Length > 1.0)
                {
                    _points.Add(p);
                }
            }
        }

        private void OnMouseUp(object sender, MouseButtonEventArgs e)
        {
            if (_isDrawing && e.ChangedButton == MouseButton.Left)
            {
                _isDrawing = false;
                ReleaseMouseCapture();

                if (_points.Count < 3)
                {
                    CloseWithoutResult();
                    return;
                }

                var dipsBounds = GetDipsBounds(_points);
                var pixelRect = DipsBoundsToPixelRect(dipsBounds);

                Result = new SelectionArea(
                    dipsBounds,
                    new PointCollection(_points),
                    new System.Drawing.Rectangle(pixelRect.X, pixelRect.Y, pixelRect.Width, pixelRect.Height)
                );

                DialogResult = true;
                Close();
            }
        }

        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Escape)
            {
                CloseWithoutResult();
            }
        }

        public void CloseWithoutResult()
        {
            Result = null;
            DialogResult = false;
            Close();
        }

        private static Rect GetDipsBounds(PointCollection pts)
        {
            double minX = pts.Min(p => p.X);
            double minY = pts.Min(p => p.Y);
            double maxX = pts.Max(p => p.X);
            double maxY = pts.Max(p => p.Y);
            return new Rect(new WpfPoint(minX, minY), new WpfPoint(maxX, maxY));
        }

        private Int32Rect DipsBoundsToPixelRect(Rect dips)
        {
            int x = _monitor.PixelBounds.Left + (int)Math.Round(dips.X * _monitor.ScaleX);
            int y = _monitor.PixelBounds.Top  + (int)Math.Round(dips.Y * _monitor.ScaleY);
            int w = (int)Math.Round(dips.Width  * _monitor.ScaleX);
            int h = (int)Math.Round(dips.Height * _monitor.ScaleY);

            if (w < 1) w = 1;
            if (h < 1) h = 1;
            return new Int32Rect(x, y, w, h);
        }
    }
}
