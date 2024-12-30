using UnityEngine;
using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
/// <summary>
/// Абстрактный класс, являющийся родительским для всех классов построек.
/// </summary>
public abstract class BuildingComponent : MonoBehaviour
{
    [HideInInspector]
    public int posX;
    [HideInInspector]
    public int posY;
    /// <summary>
    /// Количество свободных рабочих, которые необходимы для установки постройки
    /// </summary>
    public int workersAmount;
    /// <summary>
    /// Лист тайлов, на которые может быть установлена постройка
    /// </summary>
    public List<GameObject> canBePlacedOnTiles;
    /// <summary>
    /// Словарь ресурсов, необходимых для установки постройки, работающий по принципу "ресурс - количество ресурса"
    /// </summary>
    public SerializedDictionary<ResourceManager.Resource, int> resourcesToBuildFrom;
    /// <summary>
    /// Лист спрайтов, которые могут отображаться на постройке
    /// </summary>
    [SerializeField] protected List<Sprite> possibleSpritesList;
    /// <summary>
    /// Переменная, хранящая рендерер спрайта(используется для метода <see cref="SetSpriteForTile"/>)
    /// </summary>
    protected SpriteRenderer TileSpriteRenderer => GetComponentInChildren<SpriteRenderer>();


    /// <summary>
    /// Устанавливает выбранный спрайт (<paramref name="spriteId"/>) для тайла постройки и записывает изменения в <see cref="generate_field1.hexSkinsGrid"/>.
    /// <para>Обеспечивает возможность системы скинов для построек.</para>
    /// </summary>
    /// <param name="spriteId">ID спрайта в <see cref="possibleSpritesList"/></param>
    public void SetSpriteForTile(int spriteId)
    {
        if (0 <= spriteId && spriteId < possibleSpritesList.Count)
        {
            TileSpriteRenderer.sprite = possibleSpritesList[spriteId];
            generate_field1.Instance.hexSkinsGrid[posX, posY] = spriteId;
        }
    }
    /// <summary>
    /// Абстрактный метод для обработки установки тайла постройки игроком
    /// </summary>
    public virtual void OnBuild() { }
}
