/// <summary>
/// Класс тайла, содержащего в себе определённый ресурс (<see cref="ResourceManager.Resource"/>
/// </summary>
public class ResourcedTileComponent : TileComponent
{
    /// <summary>
    /// Ресурс, который содержит тайл
    /// </summary>
    public ResourceManager.Resource resource;
    /// <summary>
    /// Количество ресурса, который содержится в тайле
    /// </summary>
    public int amount;
    /// <summary>
    /// Переменная, определяющая, является ли ресурс в тайле бесконечным
    /// </summary>
    public bool isInfinite;
}
