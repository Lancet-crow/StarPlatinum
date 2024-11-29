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
    public float mouseSensitivity = 0.1f; // ���������������� ����

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
            // �������� �������� �� ������ �������� ����
            float moveX = Input.GetAxis("Mouse X") * mouseSensitivity;
            float moveY = Input.GetAxis("Mouse Y") * mouseSensitivity;

            // ��������� �������� � ������� ������� ������
            Vector3 targetPosition = transform.position + new Vector3(moveX, moveY, 0);
            // ������� �������� ������ � ������� �������
            transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
        }
    }
}
