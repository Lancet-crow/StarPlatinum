using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class generate_field1 : MonoBehaviour
{
    public List<GameObject> hexPrefabs; // Список префабов гексов
    public List<GameObject> hexPrefabsEx;

    private Transform me;

    public int width = 10; // Количество гексов по ширине
    public int height = 10; // Количество гексов по высоте
    public float hexSize = 128f; // Размер одного гекса в пикселях

    private int seed = 0; // Сид для генерации
    public bool RandomSeed = false;

    public int[,] hexGrid; // Двумерный массив для хранения индексов префабов
    public int[,] hexGridEx; // Двумерный массив для хранения индексов префабов

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

    private void Start()
    {
        me = gameObject.transform;

        // Установка сидов
        seed = MainMenu.worldKey;
        if (RandomSeed || seed == 0)
        {
            seed = UnityEngine.Random.Range(int.MinValue, int.MaxValue);
            GameObject.FindWithTag("MainCamera").GetComponent<MainMenu>().UpdateInputFieldAndScrollbars();
        }
        Debug.Log("Start with seed = " + seed);

        if (seed_text != null)
        {
            seed_text.text = seed.ToString();
            SaveCode = seed.ToString() + '|' + value_tree.ToString() + '|' + value_rock.ToString() + '|' + value_Pb.ToString() + '|' + value_ice.ToString() + '|' + value_water.ToString() + '|' + value_emptiness.ToString() + '|';
            seed_save.text = SaveCode;
        }

        DublicateValue();

        // Инициализация Perlin2D с заданным сидом
        perlin = new Perlin2D(seed);
        GenerateFieldWithSeed(seed); // Генерация поля с заданным сидом

        mainCamera = Camera.main;
        //CalculateVisibleArea();
        UpdateVisibleHexes();
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
                    if (x >= width || y >= height)
                        continue;

                    // Получаем индекс префаба из массива
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
                    Vector3 hexPosition = new Vector3(posX, posY, y + (x % 2) * 0.5f);
                    GameObject hexInstance = Instantiate(hexPrefabsEx[prefabIndex], hexPosition, Quaternion.identity, transform);
                    hexInstance.GetComponent<TileSelect>().xpos_list = x;
                    hexInstance.GetComponent<TileSelect>().ypos_list = y;
                    hexInstance.GetComponent<TileSelect>().num = prefabIndex;
                }
            }

            // Фиксируем значения старта на этот кадр
            lastX = startX;
            lastY = startY;
        }
    }

    public void DublicateValue()
    {
        List<GameObject> expandedHexPrefabs = new List<GameObject>();
        foreach (GameObject prefab in hexPrefabs)
        {
            int duplicateCount = 0;

            // Определяем, сколько раз дублировать в зависимости от имени префаба
            if (prefab.name.Contains("tile (0)") || (prefab.name.Contains("tile (4)")))
            {
                duplicateCount = value_tree + 1;
            }
            else if (prefab.name.Contains("tile (1)"))
            {
                duplicateCount = value_rock + 1;
            }
            else if (prefab.name.Contains("tile (10)"))
            {
                duplicateCount = value_Pb + 1;
            }
            else if (prefab.name.Contains("tile (7)") || prefab.name.Contains("tile (8)"))
            {
                duplicateCount = value_ice + 1;
            }
            else if (prefab.name.Contains("tile (2)") || prefab.name.Contains("tile (9)"))
            {
                duplicateCount = value_water + 1;
            }
            else if (prefab.name.Contains("tile (3)"))
            {
                duplicateCount = value_emptiness + 1;
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
    }

    public int FindIndexByName(string name)
    {
        return hexPrefabsEx.FindIndex(obj => obj.name.Equals(name, System.StringComparison.OrdinalIgnoreCase));
    }

    private void CreateHexGrid(int[,] hexGrid)
    {
        // Инициализация массива
        hexGrid = new int[width, height];

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

                // Сохраняем индекс префаба в массив
                hexGrid[x, y] = FindIndexByName(hexPrefabs[prefabIndex].name);
            }
        }
    }

    public void SaveAndExit()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (hexGrid[x, y]!=hexGridEx[x, y])
                {
                    SaveCode += hexGrid[x, y].ToString()+'_'+x.ToString()+'_'+y.ToString()+';';
                }
            }
        }
        seed_save.text = SaveCode;
    }

    private void Update()
    {
        UpdateVisibleHexes();
    }
}