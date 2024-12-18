using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;


public class generate_field1 : MonoBehaviour
{
    public List<GameObject> hexPrefabs; // ������ �������� ������
    public List<GameObject> hexPrefabsEx;

    private Transform me;

    public int width = 10; // ���������� ������ �� ������
    public int height = 10; // ���������� ������ �� ������
    public float hexSize = 128f; // ������ ������ ����� � ��������

    private int seed = 0; // ��� ��� ���������
    public bool RandomSeed = false;

    public int[,] hexGrid; // ��������� ������ ��� �������� �������� ��������
    public int[,] hexGridEx; // ��������� ������ ��� �������� �������� ��������

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

    public Vector2 renderOffset; // �������� ��������� ����� �������
    public int displayWidth = 10; // ���������� ������������ ������ �� ������
    public int displayHeight = 10; // ���������� ������������ ������ �� ������

    private int lastX = -1; // ���������� X � ������� �����
    private int lastY = -1; // ���������� Y � ������� �����


    private Perlin2D perlin; // ��������� Perlin2D

    public TextMeshProUGUI seed_text;
    public TextMeshProUGUI seed_save;

    private void Awake()
    {
        hexGrid = new int[width, height];
        hexGridEx = new int[width, height];
        me = gameObject.transform;

        // ��������� �����
        //seed = MainMenu.worldKey;

        DublicateValue();

        // ������������� Perlin2D � �������� �����
        perlin = new Perlin2D(seed);
        //GenerateFieldWithSeed(seed); // ��������� ���� � �������� �����

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
        generate_field1.value_tree = int.Parse(save_code.Split('|')[1]);
        generate_field1.value_rock = int.Parse(save_code.Split('|')[2]);
        generate_field1.value_Pb = int.Parse(save_code.Split('|')[3]);
        generate_field1.value_ice = int.Parse(save_code.Split('|')[4]);
        generate_field1.value_water = int.Parse(save_code.Split('|')[5]);
        generate_field1.value_emptiness = int.Parse(save_code.Split('|')[6]);
        
        seed_text.text = seed.ToString();
        //SaveCode = seed.ToString() + '|' + value_tree.ToString() + '|' + value_rock.ToString() + '|' + value_Pb.ToString() + '|' + value_ice.ToString() + '|' + value_water.ToString() + '|' + value_emptiness.ToString() + '|';
        seed_save.text = SaveCode;
        Debug.Log("SaveCode:" + SaveCode);

        GenerateFieldWithSeed(seed);

        int count = MainMenu.worldKey.ToString().Count(c => c == ';');
        for (int i = 1; i<count; i++)
        {
            //Debug.Log(save_code.Split(';')[i]);
            hexGrid[int.Parse(save_code.Split(';')[i].Split('_')[1]), int.Parse(save_code.Split(';')[i].Split('_')[2])] = int.Parse(save_code.Split(';')[i].Split('_')[0]);
        }

        UpdateSaveCode();
    }

    private void UpdateVisibleHexes()
    {
        // �������� ������� ������
        Vector3 cameraPosition = mainCamera.transform.position;

        // ��������� ��������� ������� ��� ������������ ������ � ������ ��������
        int startX = Mathf.Max(0, Mathf.FloorToInt((cameraPosition.x - transform.position.x + renderOffset.x) / (hexSize * 0.5f)));
        int startY = Mathf.Max(0, Mathf.FloorToInt((cameraPosition.y - transform.position.y + renderOffset.y) / (hexSize * 0.575f)));

        // ���� ������� ������ ���������� �� ��������� � ������� ������, ��:
        if (startX != lastX || startY != lastY)
        {
            // ������� ������ �����
            foreach (Transform child in transform)
            {
                Destroy(child.gameObject);
            }

            // ������� ����� ����� � ������� �������
            for (int x = startX; x < startX + displayWidth; x++)
            {
                for (int y = startY; y < startY + displayHeight; y++)
                {
                    // ���������, ����� �� ����� �� ������� �������
                    if (x >= width || y >= height)
                        continue;

                    // �������� ������ ������� �� �������
                    //Debug.Log(hexGrid[x, y]);
                    int prefabIndex = hexGrid[x, y];

                    // ��������� ������� �����
                    float posX = x * hexSize * 0.5f + transform.position.x + renderOffset.x;
                    float posY = y * hexSize * 0.575f + transform.position.y + renderOffset.y;

                    // �������� ��� ������ �����
                    if (x % 2 == 1)
                    {
                        posY += hexSize * 0.575f * 0.5f; // �������� ��� �������� ��������
                    }

                    // ������� ��������� �����
                    Vector3 hexPosition = new Vector3(posX, posY, y + (x % 2) * 0.5f);
                    GameObject hexInstance = Instantiate(hexPrefabsEx[prefabIndex], hexPosition, Quaternion.identity, transform);
                    hexInstance.GetComponent<TileSelect>().xpos_list = x;
                    hexInstance.GetComponent<TileSelect>().ypos_list = y;
                    hexInstance.GetComponent<TileSelect>().num = prefabIndex;
                }
            }

            // ��������� �������� ������ �� ���� ����
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

            // ����������, ������� ��� ����������� � ����������� �� ����� �������
            if (prefab.name.Contains("tile (0)"))
            {
                duplicateCount = value_tree + 1;
            }
            else if (prefab.name.Contains("tile (4)"))
            {
                duplicateCount = (value_tree + 1)*2;
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

        CreateHexGrid(hexGrid); // ������� ������ ��������
        CreateHexGrid(hexGridEx); // ������� ������ ��������
    }

    public int FindIndexByName(string name)
    {
        return hexPrefabsEx.FindIndex(obj => obj.name.Equals(name, System.StringComparison.OrdinalIgnoreCase));
    }

    private void CreateHexGrid(int [,] hexGrid)
    {
        // ������������� �������
        //hexGrid = new int[width, height];

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
                hexGrid[x, y] = FindIndexByName(hexPrefabs[prefabIndex].name);
            }
        }
    }

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
                    SaveCode += hexGrid[x, y].ToString()+'_'+x.ToString()+'_'+y.ToString()+';';
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