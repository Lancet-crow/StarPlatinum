using System.Collections;
using UnityEngine;
/// <summary>
/// Класс, содержащий все параметры тайла
/// </summary>
public class TileComponent : MonoBehaviour
{
    public GameObject hoverPrefab; // Префаб, который будет отображаться при наведении
    private GameObject currentHover; // Текущий экземпляр префаба
    public GameObject NewPrefab; //на что заменять
    private Transform this_transform;
    public CameraScript camera;
    public int xpos_list, ypos_list, num;
    private generate_field1 gen;

    public bool Select = false;

    public void Start()
    {
        gen = GameObject.FindGameObjectWithTag("Generator").GetComponent<generate_field1>();
        this_transform = gameObject.transform;
        camera = GameObject.FindGameObjectsWithTag("MainCamera")[0].GetComponent<CameraScript>();
    }

    private void OnMouseEnter()
    {
        if (currentHover == null && !UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject())
        {
            currentHover = Instantiate(hoverPrefab, transform.position, Quaternion.identity);
            currentHover.transform.SetParent(transform);
            currentHover.transform.position = new Vector3(currentHover.transform.position.x,
                currentHover.transform.position.y,
                currentHover.transform.position.z - 3);
        }
    }

    private void OnMouseExit()
    {
        if (currentHover != null)
        {
            Destroy(currentHover);
            currentHover = null; // Сбрасываем ссылку на текущий префаб
        }
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && currentHover != null)
        {
            StartCoroutine(WaitForMouseRelease());
        }
    }

    private IEnumerator WaitForMouseRelease()
    {
        while (Input.GetMouseButton(0))
        {
            yield return null; // Ждем следующего кадра
        }
        if (currentHover != null)
        {
            if (Select)
            {
                camera.ReplasePrefab = NewPrefab;
            }
            else
            {
                GameObject inst = null;
                switch (GameManager.Instance.currentGameMode)
                {
                    case BuildingMode:
                        inst = BuildingSystem.Instance.PlaceABuilding(GameManager.Instance.currentBuildingTypeChoice, this_transform.gameObject);
                        break;
                    case DestroyingMode:
                        if (NewPrefab != null)
                        {
                            BuildingSystem.Instance.DestroyABuilding(this_transform.gameObject);
                            inst = Instantiate(NewPrefab, this_transform.position, Quaternion.identity, gen.transform);
                        }
                        break;
                }
                if (inst != null)
                {
                    DestroyOnTile(inst);
                }
            }
        }
    }
    /// <summary>
    /// Заменяет тайл на новый(<paramref name="inst"/>)
    /// </summary>
    /// <param name="inst"></param>
    public void DestroyOnTile(GameObject inst)
    {
        var tileComponent = inst.GetComponent<TileComponent>();
        tileComponent.xpos_list = xpos_list;
        tileComponent.ypos_list = ypos_list;
        //Debug.Log(gen.hexGrid[xpos_list, ypos_list]);
        inst.GetComponent<TileComponent>().num = gen.FindIndexByName(inst.name.Replace("(Clone)", ""));
        gen.hexGrid[xpos_list, ypos_list] = tileComponent.num;
        gen.UpdateSaveCode();
        //Debug.Log(gen.hexGrid[xpos_list, ypos_list]);
        //GameObject.FindWithTag("Generator").transform;
        Destroy(gameObject); //"Ты чево наделал..."  *Он испарился*
        currentHover = null; // Сбрасываем ссылку на текущий префаб
    }
}
