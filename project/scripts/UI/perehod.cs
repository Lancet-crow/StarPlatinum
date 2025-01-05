using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AlignSprites : MonoBehaviour
{
    public bool alignToCenter = true; // Установите эту переменную в инспекторе
    public float moveSpeed = 2f; // Скорость движения спрайтов
    public int sceneToLoad = 1; // Номер сцены для загрузки после завершения движения

    private Transform sprite1;
    private Transform sprite2;
    private Vector2 targetPosition1;
    private Vector2 targetPosition2;
    private bool isMoving = false;

    void Start()
    {
        sprite1 = transform.GetChild(0);
        sprite2 = transform.GetChild(1);
    }

    public void AlignOrSeparate()
    {
        // Рассчитываем ширину двух спрайтов
        float combinedWidth = sprite1.localScale.x + sprite2.localScale.x;

        // Рассчитываем целевые позиции в зависимости от направления
        if (alignToCenter)
        {
            targetPosition1 = new Vector2(-combinedWidth / 2f, sprite1.position.y);
            targetPosition2 = new Vector2(targetPosition1.x + combinedWidth, sprite2.position.y);
        }
        else
        {
            targetPosition1 = new Vector2(0f, sprite1.position.y);
            targetPosition2 = new Vector2(combinedWidth, sprite2.position.y);
        }

        isMoving = true;
        StartCoroutine(MoveSprites());
    }

    private IEnumerator MoveSprites()
    {
        while (isMoving)
        {
            sprite1.position = Vector2.MoveTowards(sprite1.position, targetPosition1, moveSpeed * Time.deltaTime);
            sprite2.position = Vector2.MoveTowards(sprite2.position, targetPosition2, moveSpeed * Time.deltaTime);

            // Проверяем, достигли ли мы целевых позиций
            if (Vector2.Distance(sprite1.position, targetPosition1) < 0.01f && 
                Vector2.Distance(sprite2.position, targetPosition2) < 0.01f)
            {
                isMoving = false;
            }

            yield return null;
        }

        // Переход на указанную сцену
		if (alignToCenter){
			SceneManager.LoadScene(sceneToLoad);
		}
    }
}