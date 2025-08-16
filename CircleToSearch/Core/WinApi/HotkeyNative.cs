using System.Runtime.InteropServices;

namespace CircleToSearch.Wpf.Core.WinApi;

/// <summary>
/// 전역 단축키 등록을 위한 user32.dll의 네이티브 함수 및 상수
/// </summary>
internal static partial class NativeMethods
{
    [DllImport("user32.dll", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool RegisterHotKey(IntPtr hWnd, int id, uint fsModifiers, uint vk);

    [DllImport("user32.dll", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool UnregisterHotKey(IntPtr hWnd, int id);

    // Modifier Keys
    public const uint MOD_ALT = 0x0001;
    public const uint MOD_CONTROL = 0x0002;
    public const uint MOD_SHIFT = 0x0004;
    public const uint MOD_WIN = 0x0008;

    // Windows Messages
    public const int WM_HOTKEY = 0x0312;
}