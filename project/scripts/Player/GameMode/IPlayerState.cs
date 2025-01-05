/// <summary>
/// ��������� ��� �������� ����� ������� ������.
/// </summary>
public interface IPlayerState
{
    /// <summary>
    /// �����, �������������� ���������� ����������� ��������� � ������ ����� � ������ �����
    /// </summary>
    void Enter();
    /// <summary>
    /// �����, �������������� ���������� ����������� ��������� � ������ ������ �� ������� ������
    /// </summary>
    void Exit();
    /// <summary>
    /// �����, �������������� ���������� ����������� ��������� � ������ �������� ������� ������
    /// </summary>
    void Update();
    /// <summary>
    /// �����, �������������� �������� ������ � ���������� � ���� � ������ �������� ������� ������
    /// </summary>
    void HandleInput();
}
