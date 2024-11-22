using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class generate_field1 : MonoBehaviour
{
    public List<GameObject> hexPrefabs; // Список префабов гексов
    public Transform me;
    public int width = 10; // Количество гексов по ширине
    public int height = 10; // Количество гексов по высоте
    public float hexSize = 128f; // Размер одного гекса в пикселях
    public int seed = 12345; // Сид для генерации
    private int[,] hexGrid; // Двумерный массив для хранения индексов префабов

    private Perlin2D perlin; // Экземпляр Perlin2D

    private void Start()
    {
        perlin = new Perlin2D(seed); // Инициализация Perlin2D с заданным сидом
        GenerateFieldWithSeed(seed); // Генерация поля с заданным сидом
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
                float noiseValue = perlin.Noise(x * 0.1f, y * 0.1f);
                
                // Нормализуем значение шума в диапазоне от 0 до 1
                float normalizedValue = (noiseValue + 1) / 2; // Преобразуем значение в диапазон [0, 1]

                // Вычисляем индекс префаба на основе нормализованного значения
                int prefabIndex = Mathf.FloorToInt(normalizedValue * (hexPrefabs.Count - 1)); // Индекс от 0 до Count-1

                // Сохраняем индекс префаба в массив
                hexGrid[x, y] = prefabIndex;
            }
        }
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
            }
        }
    }
}