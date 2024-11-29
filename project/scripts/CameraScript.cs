using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    public Transform player; // Ссылка на объект игрока
    public float smoothTime = 0.3f; // Время сглаживания
    public Vector3 offset; // Смещение камеры относительно игрока

    public GameObject ReplasePrefab;

    private Vector3 velocity = Vector3.zero; // Текущая скорость камеры
    public float mouseSensitivity = 0.1f; // Чувствительность мыши

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
        else
        {
            // Получаем смещение на основе движения мыши
            float moveX = Input.GetAxis("Mouse X") * mouseSensitivity;
            float moveY = Input.GetAxis("Mouse Y") * mouseSensitivity;

            // Применяем смещение к текущей позиции камеры
            Vector3 targetPosition = transform.position + new Vector3(moveX, moveY, 0);
            // Плавное движение камеры к целевой позиции
            transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
        }
    }
}
