using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class generate_field1 : MonoBehaviour
{
    public List<GameObject> hexPrefabs; // Список префабов гексов
    private List<GameObject> hexPrefabsEx;
    private Transform me;
    public int width = 10; // Количество гексов по ширине
    public int height = 10; // Количество гексов по высоте
    public float hexSize = 128f; // Размер одного гекса в пикселях
    private int seed = 0; // Сид для генерации
    public bool RandomSeed = false;
    private int[,] hexGrid; // Двумерный массив для хранения индексов префабов
    public static int value_tree = 1;
    public static int value_rock = 1;
    public static int value_Pb = 1;
    public static int value_ice = 1;
    public static int value_water = 1;
    public static int value_emptiness = 1;

    public int oct = 2;
    public float pers = 0.5f;

    private Perlin2D perlin; // Экземпляр Perlin2D

    public TextMeshProUGUI seed_text;

    private void Start()
    {
        hexPrefabsEx = hexPrefabs;
        // Инициализация трансформации
        me = gameObject.transform;

        // Установка сидов
        seed = MainMenu.worldKey;
        if (RandomSeed || seed == 0)
        {
            seed = UnityEngine.Random.Range(int.MinValue, int.MaxValue);
            GameObject.FindWithTag("MainCamera").GetComponent<MainMenu>().UpdateInputFieldAndScrollbars();
        }
        Debug.Log("Start with seed = " + seed);

        // Дублирование префабов в зависимости от значений
        DublicateValue();

        // Инициализация Perlin2D с заданным сидом
        perlin = new Perlin2D(seed);
        GenerateFieldWithSeed(seed); // Генерация поля с заданным сидом

        // Обновление текста с сидом
        if (seed_text != null)
        {
            seed_text.text = seed.ToString();
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
                duplicateCount = value_emptiness * 2 + 1;
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

        CreateHexGrid(); // Создаем массив индексов
        CreateHexPrefabs(); // Создаем префабы на основе массива
    }

    private void CreateHexGrid()
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
                hexGrid[x, y] = prefabIndex;
            }
        }
        //Debug.Log(hexGrid);
    }

    private void CreateHexPrefabs()
    {
        // Вычисляем высоту одного гекса
        float hexHeight = hexSize * 0.575f; // Высота гекса с учетом смещения
        float hexWidth = hexSize; // Ширина гекса

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                // Вычисляем позицию гекса
                float posX = x * hexWidth * 0.5f + me.position.x; // Позиция по X
                float posY = y * hexHeight + me.position.y; // Позиция по Y

                // Смещение для четных рядов
                if (x % 2 == 1)
                {
                    posY += hexHeight * 0.5f; // Смещение для нечетных столбцов
                }

                // Получаем индекс префаба из массива
                int prefabIndex = hexGrid[x, y];

                // Создаем экземпляр гекса
                Vector3 hexPosition = new Vector3(posX, posY, y + (x % 2) * 0.5f);
                GameObject hexInstance = Instantiate(hexPrefabs[prefabIndex], hexPosition, Quaternion.identity, transform);
                hexInstance.GetComponent<TileSelect>().xpos_list = x;
                hexInstance.GetComponent<TileSelect>().ypos_list = y;
            }
        }
        
    }

    private void Update()
    {
        
    }
}