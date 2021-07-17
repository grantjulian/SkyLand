using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HoldTransforms : MonoBehaviour
{
    //This script is for instantiating all the slot items to be used in a players UI
    public GameObject slotPrefab;
    public GameObject scalar;
    public TextMeshProUGUI itemnametext;
    public TextMeshProUGUI itemtypetext;
    [Range(7,9)]
    public int amount;
    float width;
    GameObject[] slots;

    public TextMeshProUGUI getItemNameText ()
    {
        return itemnametext;
    }
    public TextMeshProUGUI getItemTypeText()
    {
        return itemtypetext;
    }
    public void Initialize()
    {
        width = slotPrefab.GetComponentInChildren<RectTransform>().rect.width * 1.5f * transform.localScale.x;
        slots = new GameObject[amount];
        for (int i = 0; i < amount; i++)
        {
            Vector3 pos = new Vector3(width * (i - amount / 2), slotPrefab.transform.position.y * transform.localScale.y, slotPrefab.transform.position.z);
            slots[i] = Instantiate(slotPrefab, gameObject.transform.position + pos, Quaternion.identity, scalar.transform);
        }
        scalar.transform.localScale = new Vector3(.8f, .8f, .8f);
    }
    public GameObject[] getTransforms ()
    {
        return slots;
    }
}
