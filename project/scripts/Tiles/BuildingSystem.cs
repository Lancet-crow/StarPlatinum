using AYellowpaper.SerializedCollections;
using UnityEngine;
/// <summary>
/// �����, ���������� �� ������� ������������� � ����.
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
    /// �������������� ����� ���������� � ����������� � ����(��� �������). ���������� ��� �������� �������� ���������� ���� (<see cref="UIManager.CheckFinaleConditions"/>)
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
    /// ���� ��������, ������� ����� ����� ���������� � ������ �������������
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
    /// �������, �������� ������ ��������� ��� ������� ���� <see cref="BuildingType"/>
    /// </summary>
    [SerializeField] private SerializedDictionary<BuildingType, GameObject> buildingPrefabs;

    /// <summary>
    /// ������-�������� ��� ���� ����� ��������
    /// </summary>
    [SerializeField] private Transform parentObject;

    /// <summary>
    /// �������, ���������� ���������� ������� ���� �������� � ����
    /// </summary>
    [SerializeField] public SerializedDictionary<BuildingType, int> buildingAmounts;

    /// <summary>
    /// ������������� ��������� ���� <paramref name="buildingType"/> ������ ����� <paramref name="tile"/>
    /// <para>�������� � ������ ��������� ������� � �������, ����������� ��� ���������</para>
    /// <para>���������� � <see cref="buildingAmounts"/> ����������� ���������</para>
    /// </summary>
    /// <param name="buildingType">��� ���������, ������� ����� ����������� � ���</param>
    /// <param name="tile">����, ������� ��������� �������</param>
    /// <returns>���� ����� ���������</returns>
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
    /// �������������� ����� ��� <see cref="PlaceABuilding(BuildingType?, GameObject)"/>, ����������� ������ <see cref="BuildingType"/> ������ <paramref name="buildingTypeString"/>.
    /// <para>������������ ������, ��������� � � �������� <see cref="BuildingType"/> � �������� �������� ����� <see cref="PlaceABuilding(BuildingType?, GameObject)"/></para>
    /// </summary>
    /// <param name="buildingTypeString">������, ���������� ��� ���������. ������ ��������� � ������������ ��������� <see cref="BuildingType"/></param>
    /// <param name="tile">����, ������� ��������� �������</param>
    /// <remarks>���� ������ �� ����� ���������� ��� �������� <see cref="BuildingType"/>, �� ���������� ��������� ������ �����������</remarks>
    public void PlaceABuilding(string buildingTypeString, GameObject tile)
    {
        PlaceABuilding((BuildingType)System.Enum.Parse(typeof(BuildingType), buildingTypeString), tile);
    }

    /// <summary>
    /// �������������� ��������� � �����������.
    /// <para>����������� ������� �� ��������� � ������������� ��� ��������, ������ ������ ���������.</para>
    /// </summary>
    /// <param name="building">���� ���������, ������� ����� ����������.</param>
    public void DestroyABuilding(GameObject building)
    {
        building.TryGetComponent<BuildingComponent>(out var component);
        ResourceManager.Instance.FreeWorkers(component.workersAmount);
        component.StopAllCoroutines();
    }
}
