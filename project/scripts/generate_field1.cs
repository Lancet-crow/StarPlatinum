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

    private void Start()
    {
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

        // Заполнение массива случайными индексами префабов
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                hexGrid[x, y] = Random.Range(0, hexPrefabs.Count); // Генерация случайного индекса
            }
        }
    }

    private void CreateHexPrefabs()
    {
        // Вычисляем высоту одного гекса
        float hexHeight = hexSize * 0.6f; // Высота гекса с учетом смещения
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