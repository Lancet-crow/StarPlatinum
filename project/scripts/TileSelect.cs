using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileSelect : MonoBehaviour
{
    public GameObject hoverPrefab; // Префаб, который будет отображаться при наведении
    private GameObject currentHover; // Текущий экземпляр префаба

    private void OnMouseEnter()
    {
        // Проверяем, если текущий экземпляр не установлен
        if (currentHover == null)
        {
            // Создаем экземпляр префаба на позиции тайла
            currentHover = Instantiate(hoverPrefab, transform.position, Quaternion.identity);
            // Убедимся, что он находится на правильном слое (например, выше тайлов)
            currentHover.transform.SetParent(transform);
            currentHover.transform.position = new Vector3(currentHover.transform.position.x,
                currentHover.transform.position.y,
                currentHover.transform.position.z-3);
        }
    }

    private void OnMouseExit()
    {
        // Удаляем префаб, когда курсор уходит
        if (currentHover != null)
        {
            Destroy(currentHover);
        }
    }
}
