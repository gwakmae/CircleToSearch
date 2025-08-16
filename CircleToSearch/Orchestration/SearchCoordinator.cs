using CircleToSearch.Wpf.Services.Abstractions;
using System.Windows;
using System.Windows.Interop; // HwndSource를 위해 추가

namespace CircleToSearch.Wpf.Orchestration
{
    public class SearchCoordinator
    {
        private readonly IHotkeyService _hotkeyService;
        private readonly IViewManager _viewManager;
        private readonly IScreenCaptureService _screenCaptureService;
        private readonly IImageProcessorService _imageProcessorService;

        public SearchCoordinator(
            IHotkeyService hotkeyService,
            IViewManager viewManager,
            IScreenCaptureService screenCaptureService,
            IImageProcessorService imageProcessorService)
        {
            _hotkeyService = hotkeyService;
            _viewManager = viewManager;
            _screenCaptureService = screenCaptureService;
            _imageProcessorService = imageProcessorService;
        }

        public void Start()
        {
            _hotkeyService.HotkeyPressed += OnHotkeyPressed;
            _hotkeyService.Register();
        }

        private void OnHotkeyPressed()
        {
            var selectionArea = _viewManager.ShowOverlayAndGetSelection();
            if (selectionArea is null) return;

            var source = new HwndSource(new HwndSourceParameters());
            var dpiScale = source.CompositionTarget.TransformToDevice;
            source.Dispose();

            var pixelRect = new System.Drawing.Rectangle(
                (int)(selectionArea.Value.Bounds.Left * dpiScale.M11),
                (int)(selectionArea.Value.Bounds.Top * dpiScale.M22),
                (int)(selectionArea.Value.Bounds.Width * dpiScale.M11),
                (int)(selectionArea.Value.Bounds.Height * dpiScale.M22)
            );

            // 너비나 높이가 0이면 캡처하지 않음
            if (pixelRect.Width <= 0 || pixelRect.Height <= 0) return;

            var capturedImage = _screenCaptureService.CaptureScreen(pixelRect);
            if (capturedImage is null) return;

            // 변경: 이미지 처리 서비스 호출 시 경로와 경계 정보도 함께 전달
            var base64String = _imageProcessorService.ProcessAndEncodeAsBase64(
                capturedImage,
                selectionArea.Value.PathPoints,
                selectionArea.Value.Bounds
            );

            if (string.IsNullOrEmpty(base64String)) return;

            _viewManager.ShowMainWindowAndSearch(base64String);
        }
    }
}
