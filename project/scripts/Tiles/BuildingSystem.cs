using AYellowpaper.SerializedCollections;
using UnityEngine;
/// <summary>
/// Класс, отвечающий за систему строительства в игре.
/// </summary>
public class BuildingSystem : MonoBehaviour
{
    public static BuildingSystem Instance { get; private set; }
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        DecypherHowManyBuildingsString();
    }

    /// <summary>
    /// Расшифровывает часть сохранения с постройками в мире(при наличии). Необходимо для нынешней проверки завершения игры (<see cref="UIManager.CheckFinaleConditions"/>)
    /// </summary>
    private void DecypherHowManyBuildingsString()
    {
        if (SaveManager.Instance.howManyBuildingsString != null && SaveManager.Instance.howManyBuildingsString.Length > 0)
        {
            for (int i = 0; i < SaveManager.Instance.howManyBuildingsString.Length; i++)
            {
                buildingAmounts[(BuildingType)i] = int.Parse(SaveManager.Instance.howManyBuildingsString[i]);
            }
        }
    }
    /// <summary>
    /// Типы построек, которые игрок может установить в режиме строительства
    /// </summary>
    public enum BuildingType
    {
        LivingHouse,
        LaserDrill1,
        LaserDrill2,
        FishTraps,
        Lumbermill,
        WindGenerator
    }
    /// <summary>
    /// Словарь, хранящий префаб постройки для каждого типа <see cref="BuildingType"/>
    /// </summary>
    [SerializeField] private SerializedDictionary<BuildingType, GameObject> buildingPrefabs;

    /// <summary>
    /// Объект-родитель для всех новых построек
    /// </summary>
    [SerializeField] private Transform parentObject;

    /// <summary>
    /// Словарь, содержащий количество каждого типа построек в мире
    /// </summary>
    [SerializeField] public SerializedDictionary<BuildingType, int> buildingAmounts;

    /// <summary>
    /// Устанавливает постройку типа <paramref name="buildingType"/> вместо тайла <paramref name="tile"/>
    /// <para>Отнимает у игрока свободных рабочих и ресурсы, необходимые для постройки</para>
    /// <para>Записывает в <see cref="buildingAmounts"/> добавленную постройку</para>
    /// </summary>
    /// <param name="buildingType">Тип постройки, которая будет установлена в мир</param>
    /// <param name="tile">Тайл, который постройка заменит</param>
    /// <returns>Тайл новой постройки</returns>
    public GameObject PlaceABuilding(BuildingType? buildingType, GameObject tile)
    {
        if (buildingType != null)
        {
            var buildingPrefab = buildingPrefabs[(BuildingType)buildingType];
            buildingPrefab.TryGetComponent<BuildingComponent>(out var buildingComponent);
            var tilename = tile.name;
            if (buildingComponent.canBePlacedOnTiles.Exists(x => tilename.Contains(x.name))
                && ResourceManager.Instance.HasEnoughResources(buildingComponent.resourcesToBuildFrom) &&
                ResourceManager.Instance.HasEnoughWorkers(buildingComponent))
            {
                buildingAmounts[(BuildingType)buildingType] += 1;
                ResourceManager.Instance.TakeAmount(buildingComponent.resourcesToBuildFrom);
                ResourceManager.Instance.BusyWorkers(buildingComponent.workersAmount);
                var readyBuilding = Instantiate(buildingPrefab, tile.transform.position, tile.transform.rotation, parent: parentObject);
                readyBuilding.TryGetComponent<BuildingComponent>(out var readyBuildingComponent);
                tile.TryGetComponent<TileComponent>(out var tileComponent);
                readyBuildingComponent.posX = tileComponent.xpos_list;
                readyBuildingComponent.posY = tileComponent.ypos_list;
                readyBuildingComponent.OnBuild();
                return readyBuilding;
            }
        }
        return null;
    }

    /// <summary>
    /// Дополнительный метод для <see cref="PlaceABuilding(BuildingType?, GameObject)"/>, принимающий вместо <see cref="BuildingType"/> строку <paramref name="buildingTypeString"/>.
    /// <para>Обрабатывает строку, превращая её в значение <see cref="BuildingType"/> и вызывает основной метод <see cref="PlaceABuilding(BuildingType?, GameObject)"/></para>
    /// </summary>
    /// <param name="buildingTypeString">Строка, содержащая тип постройки. Должна совпадать с существующим значением <see cref="BuildingType"/></param>
    /// <param name="tile">Тайл, который постройка заменит</param>
    /// <remarks>Если строка не будет определена как значение <see cref="BuildingType"/>, то выполнение основного метода остановится</remarks>
    public void PlaceABuilding(string buildingTypeString, GameObject tile)
    {
        PlaceABuilding((BuildingType)System.Enum.Parse(typeof(BuildingType), buildingTypeString), tile);
    }

    /// <summary>
    /// Подготавливает постройку к уничтожению.
    /// <para>Освобождает рабочих из постройки и останавливает все корутины, идущие внутри постройки.</para>
    /// </summary>
    /// <param name="building">Тайл постройки, которая будет уничтожена.</param>
    public void DestroyABuilding(GameObject building)
    {
        building.TryGetComponent<BuildingComponent>(out var component);
        ResourceManager.Instance.FreeWorkers(component.workersAmount);
        component.StopAllCoroutines();
    }
}
