using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class generate_field1 : MonoBehaviour
{
    public List<GameObject> hexPrefabs; // ������ �������� ������
    public Transform me;
    public int width = 10; // ���������� ������ �� ������
    public int height = 10; // ���������� ������ �� ������
    public float hexSize = 128f; // ������ ������ ����� � ��������
    public int seed = 12345; // ��� ��� ���������
    private int[,] hexGrid; // ��������� ������ ��� �������� �������� ��������

    private void Start()
    {
        GenerateFieldWithSeed(seed); // ��������� ���� � �������� �����
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

        // ���������� ������� ���������� ��������� ��������
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                hexGrid[x, y] = Random.Range(0, hexPrefabs.Count); // ��������� ���������� �������
            }
        }
    }

    private void CreateHexPrefabs()
    {
        // ��������� ������ ������ �����
        float hexHeight = hexSize * 0.6f; // ������ ����� � ������ ��������
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