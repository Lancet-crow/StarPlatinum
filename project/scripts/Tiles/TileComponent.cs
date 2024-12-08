using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

        if (!Select)
        {
            if (currentHover != null)
            {
                GameObject inst = null;
                if (UIManager.Instance.modeState == UIManager.ModeState.destroyingMode && NewPrefab != null)
                {
                    inst = Instantiate(NewPrefab, this_transform.position, Quaternion.identity, gen.transform);
                }
                else if (UIManager.Instance.modeState == UIManager.ModeState.buildingMode)
                {
                    inst = BuildingSystem.Instance.PlaceABuilding(UIManager.Instance.currentBuildingTypeChoice, this_transform.gameObject);
                }
                if (inst != null)
                {
                    print(inst.name);
                    inst.TryGetComponent<TileComponent>(out var tileComponent);
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
        }
        else
        {
            if (currentHover != null)
            {
                camera.ReplasePrefab = NewPrefab;
            }
        }
    }

    public void DestroyOnTile()
    {
        var inst = Instantiate(NewPrefab, this_transform.position, Quaternion.identity, gen.transform);
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
