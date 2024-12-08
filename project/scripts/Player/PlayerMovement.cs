using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f; // Скорость движения
    public float runSpeed = 10f; // Скорость бега
    public LayerMask walkableLayer; // Слой для проходимых гексов
    private Vector3 moveDirection; // Направление движения
    private Animator AnimatorController => GetComponent<Animator>();

    private SpriteRenderer Sprite => GetComponentInChildren<SpriteRenderer>();

    private void Update()
    {
        HandleMovement(); // Обработка движения
    }

    private void HandleMovement()
    {
        // Получаем ввод с клавиатуры
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        // Определяем направление движения
        moveDirection = new Vector2(horizontal, vertical).normalized;

        // Проверяем, можем ли мы двигаться в заданном направлении
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
        // Определяем скорость в зависимости от того, удерживается ли Shift
        float currentSpeed = Input.GetKey(KeyCode.LeftShift) ? runSpeed : moveSpeed;

        // Вычисляем новую позицию
        Vector3 newPosition = transform.position + moveDirection * currentSpeed * Time.deltaTime;

        // Проверяем, находится ли новая позиция на проходимом гексе
        if (IsWalkable(newPosition))
        {
            if (newPosition.x - transform.position.x < 0) {
                Sprite.flipX = true;
            }
            else
            {
                Sprite.flipX = false;
            }
            transform.position = newPosition; // Перемещаем персонажа
        }
    }

    private bool IsWalkable(Vector2 position)
    {
        // Проверяем, находится ли позиция на проходимом гексе
        return true || Physics.CheckSphere(position, 0.5f, walkableLayer);
    }
}
