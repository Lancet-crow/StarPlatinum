using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    public Transform player; // ������ �� ������ ������
    public float smoothTime = 0.3f; // ����� �����������
    public Vector3 offset; // �������� ������ ������������ ������

    public GameObject ReplasePrefab;

    private Vector3 velocity = Vector3.zero; // ������� �������� ������

    void LateUpdate()
    {
        // ���������, ���������� �� �����
        if (player != null)
        {
            // ������� ������� ������
            Vector3 targetPosition = player.position + offset;
            // ������� �������� ������ � ������� �������
            transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
        }
        else
        {
            Vector3 targetPosition = new Vector3(Input.mousePosition.x/1000, Input.mousePosition.y/1000, 0) + offset;
            transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
        }
    }
}