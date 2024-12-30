using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f; // �������� ��������
    public float runSpeed = 10f; // �������� ����
    public LayerMask walkableLayer; // ���� ��� ���������� ������
    private Vector3 moveDirection; // ����������� ��������
    private Animator AnimatorController => GetComponent<Animator>();

    private SpriteRenderer Sprite => GetComponentInChildren<SpriteRenderer>();

    private void Update()
    {
        HandleMovement(); // ��������� ��������
    }

    private void HandleMovement()
    {
        // �������� ���� � ����������
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        // ���������� ����������� �������� � ����������� ������, ����� �������� ��������� ������ ��� �������� �� ���������
        moveDirection = new Vector2(horizontal, vertical).normalized;

        // ���������, ����� �� �� ��������� � �������� �����������
        if (moveDirection.magnitude >= 0.1f)
        {
            MoveCharacter();
            AnimatorController.SetBool("isWalking", true);
        }
        else
        {
            AnimatorController.SetBool("isWalking", false);
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
            // ���� �������� �������� �����, �� ������������� ���, ���� ������ - ���������� � �������� ���������
            if (newPosition.x - transform.position.x < 0) {
                Sprite.flipX = true;
            }
            else
            {
                Sprite.flipX = false;
            }
            transform.position = newPosition; // ���������� ���������
        }
    }

    private bool IsWalkable(Vector2 position)
    {
        // ���������, ��������� �� ������� �� ���������� �����
        return true || Physics.CheckSphere(position, 0.5f, walkableLayer);
    }
}
