using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileSelect : MonoBehaviour
{
    public GameObject hoverPrefab; // Префаб, который будет отображаться при наведении
    private GameObject currentHover; // Текущий экземпляр префаба
    public GameObject NewPrefab; //на что заменять
    private GameObject me;
    public GameObject camera;
    public int xpos_list, ypos_list;

    public bool Select = false;

    public void Start()
    {
        me = gameObject;
        camera = GameObject.FindGameObjectsWithTag("MainCamera")[0];
    }

    private void OnMouseEnter()
    {
        camera = GameObject.FindGameObjectsWithTag("MainCamera")[0];
        if (currentHover == null && camera.GetComponent<MainMenu>() == null)
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
        if (currentHover != null && camera.GetComponent<MainMenu>() == null)
        {
            Destroy(currentHover);
            currentHover = null; // Сбрасываем ссылку на текущий префаб
        }
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && currentHover != null && camera.GetComponent<MainMenu>() == null)
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
            if (currentHover != null && (hoverPrefab.CompareTag("BuildSelect") || hoverPrefab.CompareTag("TrueSelect")))
            {
                GameObject inst;
                if (hoverPrefab.CompareTag("TrueSelect"))
                {
                    inst = Instantiate(NewPrefab, me.transform.position, Quaternion.identity);
                }
                else
                {
                    inst = Instantiate(camera.GetComponent<CameraScript>().ReplasePrefab, me.transform.position, Quaternion.identity, GameObject.FindWithTag("Generator").transform);
                }
                
                Destroy(me); //"Ты чево наделал..."  *Он испарился*
                currentHover = null; // Сбрасываем ссылку на текущий префаб

            }
        }
        else
        {
            if (currentHover != null)
            {
                camera.GetComponent<CameraScript>().ReplasePrefab = NewPrefab;
            }
        }
    }
}
