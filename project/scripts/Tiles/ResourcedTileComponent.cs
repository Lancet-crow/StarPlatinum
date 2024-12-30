/// <summary>
/// ����� �����, ����������� � ���� ����������� ������ (<see cref="ResourceManager.Resource"/>
/// </summary>
public class ResourcedTileComponent : TileComponent
{
    /// <summary>
    /// ������, ������� �������� ����
    /// </summary>
    public ResourceManager.Resource resource;
    /// <summary>
    /// ���������� �������, ������� ���������� � �����
    /// </summary>
    public int amount;
    /// <summary>
    /// ����������, ������������, �������� �� ������ � ����� �����������
    /// </summary>
    public bool isInfinite;
}
