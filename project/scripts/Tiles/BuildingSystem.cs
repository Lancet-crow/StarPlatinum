using AYellowpaper.SerializedCollections;
using UnityEngine;

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
        if (SaveManager.Instance.howManyBuildingsString != null &&SaveManager.Instance.howManyBuildingsString.Length > 0)
        {
            for (int i = 0; i < SaveManager.Instance.howManyBuildingsString.Length; i++)
            {
                buildingAmounts[(BuildingType)i] = int.Parse(SaveManager.Instance.howManyBuildingsString[i]);
            }
        }
    }

    public enum BuildingType
    {
        LivingHouse,
        LaserDrill1,
        LaserDrill2,
        FishTraps,
        Lumbermill,
        WindGenerator
    }
    [SerializeField] private SerializedDictionary<BuildingType, GameObject> buildingPrefabs;

    [SerializeField] private Transform parentObject;

    [SerializeField] public SerializedDictionary<BuildingType, int> buildingAmounts;
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

    public void PlaceABuilding(string buildingTypeString, GameObject tile)
    {
        PlaceABuilding((BuildingType)System.Enum.Parse(typeof(BuildingType), buildingTypeString), tile);
    }

    public void DestroyABuilding(GameObject building)
    {
        building.TryGetComponent<BuildingComponent>(out var component);
        ResourceManager.Instance.FreeWorkers(component.workersAmount);
        component.StopAllCoroutines();
    }
}
