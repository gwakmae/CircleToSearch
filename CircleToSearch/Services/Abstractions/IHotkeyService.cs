namespace CircleToSearch.Wpf.Services.Abstractions
{
    public interface IHotkeyService : IDisposable
    {
        /// <summary>
        /// ����Ű�� ������ �� �߻��ϴ� �̺�Ʈ
        /// </summary>
        event Action? HotkeyPressed;

        /// <summary>
        /// ����Ű�� ����ϰ� ������ �����մϴ�.
        /// </summary>
        void Register();
    }
}