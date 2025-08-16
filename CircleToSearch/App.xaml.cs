using CircleToSearch.Wpf.Components; // SystemTrayIcon 클래스를 위해 추가
using CircleToSearch.Wpf.Orchestration;
using CircleToSearch.Wpf.Services.Abstractions;
using CircleToSearch.Wpf.Services.Capture;
using CircleToSearch.Wpf.Services.Hotkey;
using CircleToSearch.Wpf.Services.Processing;
using CircleToSearch.Wpf.Services.UI;
using System.Windows;

namespace CircleToSearch.Wpf
{
    // 변경: Application -> System.Windows.Application
    public partial class App : System.Windows.Application
    {
        private SearchCoordinator? _coordinator;
        private IHotkeyService? _hotkeyService;
        private SystemTrayIcon? _systemTrayIcon; // 트레이 아이콘 멤버 변수 추가

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            ShutdownMode = ShutdownMode.OnExplicitShutdown;

            // 트레이 아이콘 생성
            _systemTrayIcon = new SystemTrayIcon();

            // 모든 서비스의 인스턴스 생성 (의존성 주입, Dependency Injection)
            _hotkeyService = new GlobalHotkeyService();
            IScreenCaptureService screenCaptureService = new ScreenCaptureService();
            IImageProcessorService imageProcessorService = new ImageProcessorService();
            IViewManager viewManager = new WpfViewManager();

            // 오케스트레이터 생성 및 서비스 주입
            _coordinator = new SearchCoordinator(
                _hotkeyService!,
                viewManager,
                screenCaptureService,
                imageProcessorService
            );

            // 애플리케이션 흐름 시작
            _coordinator.Start();
        }

        protected override void OnExit(ExitEventArgs e)
        {
            // 리소스 정리
            _hotkeyService?.Dispose();
            _systemTrayIcon?.Dispose(); // 트레이 아이콘 리소스 정리 추가
            base.OnExit(e);
        }
    }
}
