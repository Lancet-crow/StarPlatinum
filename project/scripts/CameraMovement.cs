using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public Transform player; // Ссылка на объект игрока
    public float smoothTime = 0.3f; // Время сглаживания
    public Vector3 offset; // Смещение камеры относительно игрока

    private Vector3 velocity = Vector3.zero; // Текущая скорость камеры

    void LateUpdate()
    {
        // Проверяем, установлен ли игрок
        if (player != null)
        {
            // Целевая позиция камеры
            Vector3 targetPosition = player.position + offset;
            // Плавное движение камеры к целевой позиции
            transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
        }
    }
}