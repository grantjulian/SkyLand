using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Inventory", order = 1)]
public class InventoryTemplate : ScriptableObject
{
    public  InventoryItem [] InvItems;
}
