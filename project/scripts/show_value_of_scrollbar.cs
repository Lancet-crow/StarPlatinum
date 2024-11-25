using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Unity.Mathematics;

public enum myEnum
{
    Tree,
    Rock,
    Pb,
    Ice, 
    Water,
    emptiness
};

public class show_value_of_scrollbar : MonoBehaviour
{
    public GameObject scrollbar;

    public myEnum DropDown = myEnum.Tree;

    public void Update()
    {
        //Debug.Log(scrollbar.GetComponent<Scrollbar>().value);
        gameObject.GetComponent<TextMeshProUGUI>().text = (scrollbar.GetComponent<Scrollbar>().value*10).ToString();
        if (DropDown == myEnum.Tree)
        {
            generate_field1.value_tree = (int)Mathf.Round(scrollbar.GetComponent<Scrollbar>().value * 10);
        }
        if (DropDown == myEnum.Ice)
        {
            generate_field1.value_ice = (int)Mathf.Round(scrollbar.GetComponent<Scrollbar>().value * 10);
        }
        if (DropDown == myEnum.Pb)
        {
            generate_field1.value_Pb = (int)Mathf.Round(scrollbar.GetComponent<Scrollbar>().value * 10);
        }
        if (DropDown == myEnum.Rock)
        {
            generate_field1.value_rock = (int)Mathf.Round(scrollbar.GetComponent<Scrollbar>().value * 10);
        }
        if (DropDown == myEnum.Water)
        {
            generate_field1.value_water = (int)Mathf.Round(scrollbar.GetComponent<Scrollbar>().value * 10);
        }
        if (DropDown == myEnum.emptiness)
        {
            generate_field1.value_emptiness = (int)Mathf.Round(scrollbar.GetComponent<Scrollbar>().value * 10);
        }
    }
}
