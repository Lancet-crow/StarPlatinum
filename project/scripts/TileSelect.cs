using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileSelect : MonoBehaviour
{
    public GameObject hoverPrefab; // ������, ������� ����� ������������ ��� ���������
    private GameObject currentHover; // ������� ��������� �������

    private void OnMouseEnter()
    {
        // ���������, ���� ������� ��������� �� ����������
        if (currentHover == null)
        {
            // ������� ��������� ������� �� ������� �����
            currentHover = Instantiate(hoverPrefab, transform.position, Quaternion.identity);
            // ��������, ��� �� ��������� �� ���������� ���� (��������, ���� ������)
            currentHover.transform.SetParent(transform);
            currentHover.transform.position = new Vector3(currentHover.transform.position.x,
                currentHover.transform.position.y,
                currentHover.transform.position.z-3);
        }
    }

    private void OnMouseExit()
    {
        // ������� ������, ����� ������ ������
        if (currentHover != null)
        {
            Destroy(currentHover);
        }
    }
}
