namespace CircleToSearch.Wpf.Services.Abstractions
{
    public interface IHotkeyService : IDisposable
    {
        /// <summary>
        /// 단축키가 눌렸을 때 발생하는 이벤트
        /// </summary>
        event Action? HotkeyPressed;

        /// <summary>
        /// 단축키를 등록하고 감지를 시작합니다.
        /// </summary>
        void Register();
    }
}