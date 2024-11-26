using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class generate_field1 : MonoBehaviour
{
    public List<GameObject> hexPrefabs; // ������ �������� ������
    private List<GameObject> hexPrefabsEx;
    private Transform me;
    public int width = 10; // ���������� ������ �� ������
    public int height = 10; // ���������� ������ �� ������
    public float hexSize = 128f; // ������ ������ ����� � ��������
    private int seed = 0; // ��� ��� ���������
    public bool RandomSeed = false;
    private int[,] hexGrid; // ��������� ������ ��� �������� �������� ��������
    public static int value_tree = 1;
    public static int value_rock = 1;
    public static int value_Pb = 1;
    public static int value_ice = 1;
    public static int value_water = 1;
    public static int value_emptiness = 1;

    public int oct = 2;
    public float pers = 0.5f;

    private Perlin2D perlin; // ��������� Perlin2D

    public TextMeshProUGUI seed_text;

    private void Start()
    {
        hexPrefabsEx = hexPrefabs;
        // ������������� �������������
        me = gameObject.transform;

        // ��������� �����
        seed = MainMenu.worldKey;
        if (RandomSeed || seed == 0)
        {
            seed = UnityEngine.Random.Range(int.MinValue, int.MaxValue);
            GameObject.FindWithTag("MainCamera").GetComponent<MainMenu>().UpdateInputFieldAndScrollbars();
        }
        Debug.Log("Start with seed = " + seed);

        // ������������ �������� � ����������� �� ��������
        DublicateValue();

        // ������������� Perlin2D � �������� �����
        perlin = new Perlin2D(seed);
        GenerateFieldWithSeed(seed); // ��������� ���� � �������� �����

        // ���������� ������ � �����
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

            // ����������, ������� ��� ����������� � ����������� �� ����� �������
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

            // ��������� ������ ������ ���������� ���
            for (int i = 0; i < duplicateCount; i++)
            {
                expandedHexPrefabs.Add(prefab);
            }
        }
        hexPrefabs = expandedHexPrefabs;
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
                float noiseValue = perlin.Noise(x * 0.1f, y * 0.1f, octaves: oct, persistence: pers);

                // ����������� �������� ���� � ��������� �� 0 �� 1
                float normalizedValue = (noiseValue + 1) / 2; // ����������� �������� � �������� [0, 1]

                // ��������� ������ ������� �� ������ ���������������� ��������
                int prefabIndex = Mathf.FloorToInt(normalizedValue * (hexPrefabs.Count - 1)); // ������ �� 0 �� Count-1

                // ��������� ������ ������� � ������
                hexGrid[x, y] = prefabIndex;
            }
        }
        //Debug.Log(hexGrid);
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
                hexInstance.GetComponent<TileSelect>().xpos_list = x;
                hexInstance.GetComponent<TileSelect>().ypos_list = y;
            }
        }
        
    }

    private void Update()
    {
        
    }
}