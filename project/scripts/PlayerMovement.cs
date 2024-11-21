using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f; // �������� ��������
    public float runSpeed = 10f; // �������� ����
    public LayerMask walkableLayer; // ���� ��� ���������� ������
    private Vector3 moveDirection; // ����������� ��������

    private void Update()
    {
        HandleMovement(); // ��������� ��������
    }

    private void HandleMovement()
    {
        // �������� ���� � ����������
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        // ���������� ����������� ��������
        moveDirection = new Vector2(horizontal, vertical).normalized;

        // ���������, ����� �� �� ��������� � �������� �����������
        if (moveDirection.magnitude >= 0.1f)
        {
            MoveCharacter();
        }
    }

    private void MoveCharacter()
    {
        // ���������� �������� � ����������� �� ����, ������������ �� Shift
        float currentSpeed = Input.GetKey(KeyCode.LeftShift) ? runSpeed : moveSpeed;

        // ��������� ����� �������
        Vector3 newPosition = transform.position + moveDirection * currentSpeed * Time.deltaTime;

        // ���������, ��������� �� ����� ������� �� ���������� �����
        if (IsWalkable(newPosition))
        {
            transform.position = newPosition; // ���������� ���������
        }
    }

    private bool IsWalkable(Vector2 position)
    {
        // ���������, ��������� �� ������� �� ���������� �����
        return true || Physics.CheckSphere(position, 0.5f, walkableLayer);
    }
}
