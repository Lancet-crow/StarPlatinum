using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectSlotSave : MonoBehaviour
{
    public Transform Selection;
    private Vector3 st;
    private GameObject me;

    private void Start()
    {
        me = gameObject;
        st = new Vector3(me.transform.position.x, me.transform.position.y, me.transform.position.z);
    }

    public void SetPos()
    {
        if (Selection != null)
        {
            //me.SetActive(true);
            me.transform.position = Selection.position;
        }
        else
        {
            SetFalse();
        }
    }

    public void SetFalse()
    {
        Selection = null;
        me.transform.position = st;
    }
}
