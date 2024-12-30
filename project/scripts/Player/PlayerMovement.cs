using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f; // —корость движени€
    public float runSpeed = 10f; // —корость бега
    public LayerMask walkableLayer; // —лой дл€ проходимых гексов
    private Vector3 moveDirection; // Ќаправление движени€
    private Animator AnimatorController => GetComponent<Animator>();

    private SpriteRenderer Sprite => GetComponentInChildren<SpriteRenderer>();

    private void Update()
    {
        HandleMovement(); // ќбработка движени€
    }

    private void HandleMovement()
    {
        // ѕолучаем ввод с клавиатуры
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        // ќпредел€ем направление движени€ и нормализуем вектор, чтобы избежать ускорени€ игрока при движении по диагонали
        moveDirection = new Vector2(horizontal, vertical).normalized;

        // ѕровер€ем, можем ли мы двигатьс€ в заданном направлении
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
        // ќпредел€ем скорость в зависимости от того, удерживаетс€ ли Shift
        float currentSpeed = Input.GetKey(KeyCode.LeftShift) ? runSpeed : moveSpeed;

        // ¬ычисл€ем новую позицию
        Vector3 newPosition = transform.position + moveDirection * currentSpeed * Time.deltaTime;

        // ѕровер€ем, находитс€ ли нова€ позици€ на проходимом гексе
        if (IsWalkable(newPosition))
        {
            // ≈сли персонаж движетс€ влево, то разворачиваем его, если вправо - возвращаем в исходное состо€ние
            if (newPosition.x - transform.position.x < 0) {
                Sprite.flipX = true;
            }
            else
            {
                Sprite.flipX = false;
            }
            transform.position = newPosition; // ѕеремещаем персонажа
        }
    }

    private bool IsWalkable(Vector2 position)
    {
        // ѕровер€ем, находитс€ ли позици€ на проходимом гексе
        return true || Physics.CheckSphere(position, 0.5f, walkableLayer);
    }
}
