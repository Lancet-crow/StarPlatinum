using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;


public class generate_field1 : MonoBehaviour
{
    public List<GameObject> hexPrefabs; // Список префабов гексов
    public List<GameObject> hexPrefabsEx;
    public List<GameObject> minimapHexPrefabs;

    private Transform me;

    public int width = 10; // Количество гексов по ширине
    public int height = 10; // Количество гексов по высоте
    public float hexSize = 128f; // Размер одного гекса в пикселях
    public float hexMiniSize = 16f; // Размер одного гекса в пикселях на миникарте

    private int seed = 0; // Сид для генерации
    public bool RandomSeed = false;

    public int[,] hexGrid; // Двумерный массив для хранения индексов префабов
    public int[,] hexGridEx; // Двумерный массив для хранения индексов префабов

    public int[,] hexSkinsGrid; // Двумерный массив для хранения индексов скинов построек

    public int[,] minimapGrid; // Двумерный массив для хранения индексов префабов для миникарты

    public static int value_tree = 1;
    public static int value_rock = 1;
    public static int value_Pb = 1;
    public static int value_ice = 1;
    public static int value_water = 1;
    public static int value_emptiness = 1;

    public string SaveCode;

    public int oct = 2;
    public float pers = 0.5f;

    private Camera mainCamera;

    public Vector2 renderOffset; // Смещение начальной точки рендера
    public int displayWidth = 10; // Количество отображаемых гексов по ширине
    public int displayHeight = 10; // Количество отображаемых гексов по высоте

    private int lastX = -1; // Координаты X в прошлом кадре
    private int lastY = -1; // Координаты Y в прошлом кадре


    private Perlin2D perlin; // Экземпляр Perlin2D

    public TextMeshProUGUI seed_text;
    public TextMeshProUGUI seed_save;

    public static generate_field1 Instance { get; private set; }

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
        hexGrid = new int[width, height];
        hexGridEx = new int[width, height];
        hexSkinsGrid = new int[width, height];
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                hexSkinsGrid[i, j] = -1;
            }
        }
        //minimapGrid = new int[width, height];
        me = gameObject.transform;

        // Установка сидов
        //seed = MainMenu.worldKey;

        DublicateValue();

        // Инициализация Perlin2D с заданным сидом
        
        //GenerateFieldWithSeed(seed); // Генерация поля с заданным сидом

        mainCamera = Camera.main;

        DecodeSaveCode();
        //CalculateVisibleArea();
        //UpdateVisibleHexes();
    }

    public void DecodeSaveCode()
    {
        string save_code = MainMenu.worldKey;
        //Debug.Log(save_code);
        seed = int.Parse(save_code.Split('|')[0]);
        //Debug.Log(save_code+'_'+seed+'=');
        value_tree = int.Parse(save_code.Split('|')[1]);
        value_rock = int.Parse(save_code.Split('|')[2]);
        value_Pb = int.Parse(save_code.Split('|')[3]);
        value_ice = int.Parse(save_code.Split('|')[4]);
        value_water = int.Parse(save_code.Split('|')[5]);
        value_emptiness = int.Parse(save_code.Split('|')[6]);
        
        seed_text.text = seed.ToString();
        //SaveCode = seed.ToString() + '|' + value_tree.ToString() + '|' + value_rock.ToString() + '|' + value_Pb.ToString() + '|' + value_ice.ToString() + '|' + value_water.ToString() + '|' + value_emptiness.ToString() + '|';
        seed_save.text = SaveCode;
        SaveCode = save_code;
        Debug.Log("SaveCode:" + SaveCode);

        perlin = new Perlin2D(seed);

        GenerateFieldWithSeed(seed);

        int count = save_code.ToString().Count(c => c == ';');
        var save_code_split = save_code.Split(';');
        for (int i = 1; i<count; i++)
        {
            //Debug.Log(save_code.Split(';')[i]);
            var hex_split = save_code_split[i].Split('_');
            int x = int.Parse(hex_split[1]);
            int y = int.Parse(hex_split[2]);
            hexGrid[x, y] = int.Parse(hex_split[0]);
            if (hex_split.Length == 4)
            {
                hexSkinsGrid[x, y] = int.Parse(hex_split[3]);
                print($"I WORK! This cell's skin id is {hexSkinsGrid[x, y]}");
            }
        }

        UpdateSaveCode();

        //GenerateHexGrid();
    }

    public void SaveAndExit()
    {
        SaveManager.Instance.saveCode = SaveCode;
        SaveManager.Instance.SaveFile(MainMenu.newSaveID);
        UIManager.Instance.ExitToMainMenu();
    }

    private void UpdateVisibleHexes()
    {
        // Получаем позицию камеры
        Vector3 cameraPosition = mainCamera.transform.position;

        // Вычисляем начальные индексы для отображаемых гексов с учетом смещения
        int startX = Mathf.Max(0, Mathf.FloorToInt((cameraPosition.x - transform.position.x + renderOffset.x) / (hexSize * 0.5f)));
        int startY = Mathf.Max(0, Mathf.FloorToInt((cameraPosition.y - transform.position.y + renderOffset.y) / (hexSize * 0.575f)));

        // Если индексы гексов изменились по сравнению с прошлым кадром, то:
        if (startX != lastX || startY != lastY)
        {
            // Удаляем старые гексы
            foreach (Transform child in transform)
            {
                Destroy(child.gameObject);
            }

            // Создаем новые гексы в видимой области
            for (int x = startX; x < startX + displayWidth; x++)
            {
                for (int y = startY; y < startY + displayHeight; y++)
                {
                    // Проверяем, чтобы не выйти за границы массива
                    if ((x >= width || y >= height))
                        continue;

                    // Получаем индекс префаба из массива
                    //Debug.Log(hexGrid[x, y]);
                    int prefabIndex = hexGrid[x, y];

                    // Вычисляем позицию гекса
                    float posX = x * hexSize * 0.5f + transform.position.x + renderOffset.x;
                    float posY = y * hexSize * 0.575f + transform.position.y + renderOffset.y;

                    // Смещение для четных рядов
                    if (x % 2 == 1)
                    {
                        posY += hexSize * 0.575f * 0.5f; // Смещение для нечетных столбцов
                    }

                    // Создаем экземпляр гекса
                    Vector3 hexPosition = new(posX, posY, y + (x % 2) * 0.5f);
                    GameObject hexInstance = Instantiate(hexPrefabsEx[prefabIndex], hexPosition, Quaternion.identity, transform);
                    hexInstance.TryGetComponent<TileComponent>(out var tileComponent);
                    tileComponent.xpos_list = x;
                    tileComponent.ypos_list = y;
                    tileComponent.num = prefabIndex;
                    hexInstance.TryGetComponent<BuildingComponent>(out var buildingComponent);
                    if (buildingComponent != null)
                    {
                        buildingComponent.posX = x;
                        buildingComponent.posY = y;
                        buildingComponent.SetSpriteForTile(hexSkinsGrid[x, y]);
                    }
                }
            }

            // Фиксируем значения старта на этот кадр
            lastX = startX;
            lastY = startY;
        }
    }

    public void DublicateValue()
    {
        List<GameObject> expandedHexPrefabs = new();
        foreach (GameObject prefab in hexPrefabs)
        {
            int duplicateCount = 0;

            // Определяем, сколько раз дублировать в зависимости от имени префаба
            switch (prefab.name)
            {
                case "Dense Forest":
                    duplicateCount = value_tree + 1;
                    break;
                case "Sparce Forest":
                    duplicateCount = (value_tree + 1) * 2;
                    break;
                case "Rocks":
                    duplicateCount = value_rock + 1;
                    break;
                case "Steel Rocks":
                    duplicateCount = (value_Pb + 1) * 3;
                    break;
                case "Honey Rocks":
                    duplicateCount = (value_Pb + 1) * 3;
                    break;
                case "Ice" or "Broken Ice":
                    duplicateCount = value_ice + 1;
                    break;
                case "Shattered Ice" or "Fully Shattered Ice":
                    duplicateCount = value_water + 1;
                    break;
                case "Empty Tile":
                    duplicateCount = value_emptiness + 1;
                    break;
            }
            // Дублируем префаб нужное количество раз
            for (int i = 0; i < duplicateCount; i++)
            {
                expandedHexPrefabs.Add(prefab);
            }
        }
        hexPrefabs = expandedHexPrefabs;
    }

    private void GenerateFieldWithSeed(int seed)
    {
        Random.InitState(seed); // Инициализация генератора случайных чисел

        CreateHexGrid(hexGrid); // Создаем массив индексов
        CreateHexGrid(hexGridEx); // Создаем массив индексов
        //CreateMinimapGrid(minimapGrid); // Создаём массив индексов под миникарту
    }

    /*private void GenerateHexGrid()
    {
        for (int x = 0; x < hexGrid.GetLength(0); x++)
        {
            for (int y = 0; y < hexGrid.GetLength(1); y++)
            {
                int prefabIndex = hexGrid[x, y];

                // Вычисляем позицию гекса
                float posX = x * hexSize * 0.5f + transform.position.x + renderOffset.x;
                float posY = y * hexSize * 0.575f + transform.position.y + renderOffset.y;

                // Смещение для четных рядов
                if (x % 2 == 1)
                {
                    posY += hexSize * 0.575f * 0.5f; // Смещение для нечетных столбцов
                }

                // Создаем экземпляр гекса
                Vector3 hexPosition = new(posX, posY, y + (x % 2) * 0.5f);
                GameObject hexInstance = Instantiate(hexPrefabsEx[prefabIndex], hexPosition, Quaternion.identity, transform);
                hexInstance.TryGetComponent<TileComponent>(out var tileComponent);
                tileComponent.xpos_list = x;
                tileComponent.ypos_list = y;
                tileComponent.num = prefabIndex;
                hexInstance.TryGetComponent<BuildingComponent>(out var buildingComponent);
                if (buildingComponent != null)
                {
                    buildingComponent.posX = x;
                    buildingComponent.posY = y;
                    buildingComponent.SetSpriteForTile(hexSkinsGrid[x, y]);
                }
            }
        }
    }*/

    public int FindIndexByName(string name)
    {
        return hexPrefabsEx.FindIndex(obj => obj.name.Equals(name, System.StringComparison.OrdinalIgnoreCase));
    }

    private void CreateHexGrid(int [,] hexGrid)
    {
        // Инициализация массива
        //hexGrid = new int[width, height];

        // Заполнение массива значениями на основе Perlin-шума
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                // Генерация значения шума
                float noiseValue = perlin.Noise(x * 0.1f, y * 0.1f, octaves: oct, persistence: pers);

                // Нормализуем значение шума в диапазоне от 0 до 1
                float normalizedValue = (noiseValue + 1) / 2; // Преобразуем значение в диапазон [0, 1]

                // Вычисляем индекс префаба на основе нормализованного значения
                int prefabIndex = Mathf.FloorToInt(normalizedValue * (hexPrefabs.Count - 1)); // Индекс от 0 до Count-1
                var prefab = FindIndexByName(hexPrefabs[prefabIndex].name);
                // Сохраняем индекс префаба в массив
                hexGrid[x, y] = prefab;
            }
        }
    }

    /*public void CreateMinimapGrid(int [,] minimapGrid)
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                int prefabIndex = hexGrid[x, y];
                switch (prefabIndex)
                {
                    case 0 or 4:
                        minimapGrid[x, y] = 0;
                        break;
                    case 10 or 11 or 12:
                        minimapGrid[x, y] = 2;
                        break;
                    case 9:
                        minimapGrid[x, y] = 3;
                        break;
                    case 2 or 5 or 6 or 7:
                        minimapGrid[x, y] = 4;
                        break;
                }
            }
        }
    }*/

    public void UpdateSaveCode()
    {
        //Debug.Log(hexGrid[0, 0].ToString() + '_' + hexGridEx[0, 0].ToString());
        SaveCode = seed.ToString() + '|' + value_tree.ToString() + '|' + value_rock.ToString() + '|' + value_Pb.ToString() + '|' + value_ice.ToString() + '|' + value_water.ToString() + '|' + value_emptiness.ToString() + "|;";
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (hexGrid[x, y]!=hexGridEx[x, y])
                {
                    SaveCode += hexGrid[x, y].ToString()+'_'+x.ToString()+'_'+y.ToString();
                    if (hexSkinsGrid[x, y] != -1)
                    {
                        SaveCode += '_' + hexSkinsGrid[x, y].ToString();
                    }
                    SaveCode += ';';
                }
            }
        }
        seed_save.text = SaveCode;
        Debug.Log("Save panel update");
    }

    private void Update()
    {
        UpdateVisibleHexes();
    }
}