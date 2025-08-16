using CircleToSearch.Wpf.Core.WinApi;
using CircleToSearch.Wpf.Services.Abstractions;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interop;

namespace CircleToSearch.Wpf.Services.Hotkey
{
    public class GlobalHotkeyService : IHotkeyService
    {
        public event Action? HotkeyPressed;

        private const int HOTKEY_ID = 9000;
        private HwndSource? _source;
        private IntPtr _windowHandle;
        private Window? _helperWindow; // 가비지 컬렉션 방지

        public void Register()
        {
            _helperWindow = new Window();
            var helperInterop = new WindowInteropHelper(_helperWindow);
            _windowHandle = helperInterop.EnsureHandle();

            _source = HwndSource.FromHwnd(_windowHandle);
            _source.AddHook(HwndHook);

            uint vk = (uint)KeyInterop.VirtualKeyFromKey(Key.S);
            uint modifiers = NativeMethods.MOD_ALT | NativeMethods.MOD_SHIFT;

            if (!NativeMethods.RegisterHotKey(_windowHandle, HOTKEY_ID, modifiers, vk))
            {
                // 오류 처리
            }
        }

        private IntPtr HwndHook(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            if (msg == NativeMethods.WM_HOTKEY && wParam.ToInt32() == HOTKEY_ID)
            {
                HotkeyPressed?.Invoke();
                handled = true;
            }
            return IntPtr.Zero;
        }

        public void Dispose()
        {
            _source?.RemoveHook(HwndHook);
            _source?.Dispose();
            NativeMethods.UnregisterHotKey(_windowHandle, HOTKEY_ID);
            _helperWindow?.Close();
            GC.SuppressFinalize(this);
        }
    }
}