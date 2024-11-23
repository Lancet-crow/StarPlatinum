using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class generate_field1 : MonoBehaviour
{
    public List<GameObject> hexPrefabs; // ������ �������� ������
    public Transform me;
    public int width = 10; // ���������� ������ �� ������
    public int height = 10; // ���������� ������ �� ������
    public float hexSize = 128f; // ������ ������ ����� � ��������
    private int seed = 0; // ��� ��� ���������
    public bool RandomSeed = false;
    private int[,] hexGrid; // ��������� ������ ��� �������� �������� ��������

    private Perlin2D perlin; // ��������� Perlin2D

    public TextMeshProUGUI seed_text;

    private void Start()
    {
        seed = MainMenu.worldKey;
        if (RandomSeed || seed==0)
        {
            seed = UnityEngine.Random.Range(int.MinValue, int.MaxValue);
        }
        Debug.Log("Start with seed = " + seed);
        perlin = new Perlin2D(seed); // ������������� Perlin2D � �������� �����
        GenerateFieldWithSeed(seed); // ��������� ���� � �������� �����
        if (seed_text != null)
        {
            seed_text.text = seed.ToString();
        }
    }

    private void GenerateFieldWithSeed(int seed)
    {
        Random.InitState(seed); // ������������� ���������� ��������� �����

        CreateHexGrid(); // ������� ������ ��������
        CreateHexPrefabs(); // ������� ������� �� ������ �������
    }

    private void CreateHexGrid()
    {
        // ������������� �������
        hexGrid = new int[width, height];

        // ���������� ������� ���������� �� ������ Perlin-����
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                // ��������� �������� ����
                float noiseValue = perlin.Noise(x * 0.1f, y * 0.1f);
                
                // ����������� �������� ���� � ��������� �� 0 �� 1
                float normalizedValue = (noiseValue + 1) / 2; // ����������� �������� � �������� [0, 1]

                // ��������� ������ ������� �� ������ ���������������� ��������
                int prefabIndex = Mathf.FloorToInt(normalizedValue * (hexPrefabs.Count - 1)); // ������ �� 0 �� Count-1

                // ��������� ������ ������� � ������
                hexGrid[x, y] = prefabIndex;
            }
        }
    }

    private void CreateHexPrefabs()
    {
        // ��������� ������ ������ �����
        float hexHeight = hexSize * 0.575f; // ������ ����� � ������ ��������
        float hexWidth = hexSize; // ������ �����

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                // ��������� ������� �����
                float posX = x * hexWidth * 0.5f + me.position.x; // ������� �� X
                float posY = y * hexHeight + me.position.y; // ������� �� Y

                // �������� ��� ������ �����
                if (x % 2 == 1)
                {
                    posY += hexHeight * 0.5f; // �������� ��� �������� ��������
                }

                // �������� ������ ������� �� �������
                int prefabIndex = hexGrid[x, y];

                // ������� ��������� �����
                Vector3 hexPosition = new Vector3(posX, posY, y + (x % 2) * 0.5f);
                GameObject hexInstance = Instantiate(hexPrefabs[prefabIndex], hexPosition, Quaternion.identity, transform);
            }
        }
    }
}