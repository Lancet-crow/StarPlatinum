using System.Collections;
using UnityEngine;
/// <summary>
/// Абстрактный класс, от которого наследуются все постройки, способные добывать ресурсы
/// </summary>
public abstract class ResourceMinerTile : BuildingComponent
{
    /// <summary>
    /// Ресурс, который будет добывать постройка
    /// </summary>
    public ResourceManager.Resource resourceToMine;
    /// <summary>
    /// Время, за которое добывается ресурс
    /// </summary>
    public float miningSpeed;
    /// <summary>
    /// Количество ресурса, добываемое постройкой за один раз
    /// </summary>
    public int resourceMinedAtOnce = 5;

    /// <summary>
    /// Ссылка на компонент ресурсного тайла
    /// </summary>
    private ResourcedTileComponent ResourcedTile => GetComponent<ResourcedTileComponent>();

    private void Awake()
    {
        StartCoroutine(MineAResource());
    }

    /// <summary>
    /// Корутина, запускающая процесс добычи ресурса.
    /// <para>Ожидает в течение определённого времени (<see cref="miningSpeed"/>)</para>
    /// <para>А потом отнимает ресурс у ресурсного тайла (<see cref="ResourcedTile"/>)</para>
    /// <para>И добавляет его к ресурсам игрока (<see cref="ResourceManager"/>)</para>
    /// </summary>
    /// <remarks>
    /// <list type="bullet">
    /// <item>Если ресурсов на тайле больше или равно значению <see cref="resourceMinedAtOnce"/>, то добывается именно <see cref="resourceMinedAtOnce"/></item>
    /// <item>Если ресурсов на тайле больше 0, но меньше значения <see cref="resourceMinedAtOnce"/>, то добывается столько ресурса, сколько ещё есть на тайле</item>
    /// <item>Если ресурсов на тайле не осталось, то постройка удаляется</item>
    /// <item>Если тайл имеет бесконечный ресурс (<see cref="ResourcedTileComponent.isInfinite"/>), то ресурс будет добываться всегда и в полном объёме (<see cref="resourceMinedAtOnce"/>)</item>
    /// </list>
    /// </remarks>
    private IEnumerator MineAResource()
    {
        var estimatedTime = 0f;
        while (estimatedTime < miningSpeed)
        {
            estimatedTime += Time.deltaTime;
            yield return null;
        }
        var resourceAmountWasMined = resourceMinedAtOnce;
        if (!ResourcedTile.isInfinite)
        {
            if (ResourcedTile.amount < resourceMinedAtOnce)
            {
                resourceAmountWasMined = ResourcedTile.amount;
            }
        }
        ResourcedTile.amount -= resourceAmountWasMined;
        ResourceManager.Instance.AddAmount(resourceToMine, resourceAmountWasMined);
        if (ResourcedTile.amount > 0 || ResourcedTile.isInfinite)
        {
            StartCoroutine(MineAResource());
        }
        else
        {
            ResourcedTile.DestroyOnTile(gameObject);
        }
    }
}
