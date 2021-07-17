using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    // Start is called before the first frame update
    public static InventoryTemplate template;
    public InventoryItem[] startItems; // items that this inventory will have
    public int[] startAmounts; // correspond to the start items
    InventoryItem[] items; //shortcut to "template.InvItems"
    int[] amount;
    int length;
    DisplayAddedItem thisDisplay; //purely visual
    BottomInventory thisBottomInventory;

    void Start()
    {
        
        items = template.InvItems;
        length = items.Length;
        amount = new int[length];
        thisDisplay = gameObject.GetComponent<DisplayAddedItem>();
        thisBottomInventory = gameObject.GetComponent<BottomInventory>();
        int i = 0;
        foreach (InventoryItem item in startItems)
        {
            AddItem(item.ItemName, startAmounts[i]);
            i++;
        }
        
    }
    public int[] getAmount()
    {
        return amount;
    }
    public int getAmountOfItem (InventoryItem i)
    {
        int j = 0;
        foreach (InventoryItem ii in items)
        { 
            if (i == ii)
            {
                return amount[j];
            }
            j++;
        }
        return 0;
    }
    public void AddInventory(Inventory inv)
    {
        for (int i = 0; i < length; i++)
        {
            if (inv.getAmount()[i] != 0)
            {
                AddItem(items[i].ItemName, inv.getAmount()[i]);
            }
        }
        OnInventoryChange();
    }

    public void AddItem(string id, int amt)
    {
        int i;
        for (i = 0; i < length; i++)
        {
            if (items[i].ItemName == id)
            {
                amount[i] += amt;
                break;
            }
        }
        if (thisDisplay != null)
        {
            thisDisplay.AddItemDisplay(items[i], amt);
            thisBottomInventory.AddItemToLists(i, items[i].type);
        }
        OnInventoryChange();
    }
    public void PrintInventory()
    {
        for (int i = 0; i < length; i++)
        {
            Debug.Log(items[i].ItemName + ": " + amount[i]);
        }
    }
    private void OnInventoryChange()
    {
        //PrintInventory();
    }

    private void OnValidate()
    {
        if (startItems.Length != startAmounts.Length) // Make sure that each startItem has a corresponding amount
        {
            startAmounts = new int[startItems.Length];
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && thisBottomInventory != null)
        {
            PrintInventory();
        }
    }

}
