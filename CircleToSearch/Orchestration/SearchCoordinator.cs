using CircleToSearch.Wpf.Services.Abstractions;

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

            // 바로 PixelRect 사용
            var pixelRect = selectionArea.Value.PixelRect;

            if (pixelRect.Width <= 0 || pixelRect.Height <= 0) return;

            var capturedImage = _screenCaptureService.CaptureScreen(pixelRect);
            if (capturedImage is null) return;

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
