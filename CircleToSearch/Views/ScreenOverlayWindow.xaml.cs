using CircleToSearch.Wpf.Models;
using System.Windows;
using System.Windows.Media;

namespace CircleToSearch.Wpf.Views
{
    public partial class ScreenOverlayWindow : Window
    {
        private bool _isSelecting = false;

        public SelectionArea? SelectedArea { get; private set; }

        public ScreenOverlayWindow()
        {
            InitializeComponent();
        }

        private void Window_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Escape)
            {
                DialogResult = false;
                Close();
            }
        }

        private void Window_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (e.LeftButton == System.Windows.Input.MouseButtonState.Pressed)
            {
                _isSelecting = true;
                FreeformPath.Points.Clear();
                FreeformPath.Points.Add(e.GetPosition(this));

                // --- 이 부분을 수정하세요 ---
                // Mouse.Capture(this); -> System.Windows.Input.Mouse.Capture(this);
                System.Windows.Input.Mouse.Capture(this);
                // -------------------------
            }
            else if (e.RightButton == System.Windows.Input.MouseButtonState.Pressed)
            {
                DialogResult = false;
                Close();
            }
        }

        private void Window_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (!_isSelecting) return;
            FreeformPath.Points.Add(e.GetPosition(this));
        }

        private void Window_MouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (!_isSelecting) return;
            _isSelecting = false;

            // --- 이 부분을 수정하세요 ---
            // Mouse.Capture(null); -> System.Windows.Input.Mouse.Capture(null);
            System.Windows.Input.Mouse.Capture(null);
            // -------------------------

            if (FreeformPath.Points.Count < 10)
            {
                DialogResult = false;
                Close();
                return;
            }

            Rect bounds = new Rect(FreeformPath.Points[0], FreeformPath.Points[0]);
            foreach (var point in FreeformPath.Points)
            {
                bounds.Union(point);
            }

            SelectedArea = new SelectionArea(bounds, FreeformPath.Points);
            DialogResult = true;
            Close();
        }
    }
}