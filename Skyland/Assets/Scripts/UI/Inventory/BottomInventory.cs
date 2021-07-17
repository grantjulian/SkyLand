using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
// PLAYER SCRIPT
public class BottomInventory : MonoBehaviour
{
    bool active;
    bool initialized = false;
    public GameObject canvasPrefab;
    public Color[] itemTypeColors;
    public Sprite emptySprite;
    public float scrollSpeed;
    public float magnifyMagnitude;
    TextMeshProUGUI itemNameText;
    TextMeshProUGUI itemTypeText;
    InventoryTemplate template; //Template that belongs to the Inventory class that contains all possible items in the game
    InventoryItem[] templateItems; // all the items that are in the template
    Inventory inventory; // Inventory that this script uses
    int[] amounts; // ledger that stores the amount of each item in inventory
    List<int> indexUsable;
    List<int> indexResources;
    List<int> indexPlaceable;
    GameObject canvas;
    GameObject[] slots;
    Image[] slotImages;
    Image[] selectedImages;
    TextMeshProUGUI [] slotItemAmounts;
    Animator[] animations;
    Inventory playerInv;
    int [] selected = new int [3];
    int numSlots;
    float scroll_amt;
    ItemType mode;
    // Start is called before the first frame update
    void Start()
    {
        if(initialized)
        {
            return;
        }
        active = true;
        indexUsable = new List<int>();
        indexResources = new List<int>();
        indexPlaceable = new List<int>();
        template = Inventory.template;
        templateItems = template.InvItems;
        inventory = gameObject.GetComponent<Inventory>();
        amounts = inventory.getAmount();
        canvas = Instantiate(canvasPrefab);
        canvas.GetComponent<HoldTransforms>().Initialize();
        itemNameText = canvas.GetComponent<HoldTransforms>().getItemNameText();
        itemTypeText = canvas.GetComponent<HoldTransforms>().getItemTypeText();
        slots = null;
        mode = ItemType.usable;
        initialized = true;
    }

    // Update is called once per frame
    public void setState(bool state)
    {
        active = state;
        canvas.SetActive(state);
    }
    void Update()
    {
        if (!active)
        {
            return;
        }
        Scroll();
        if (Input.GetKey(KeyCode.RightShift))
        {
            SwitchMode(ItemType.resources);
        }
        else if (Input.GetKey(KeyCode.RightControl))
        {
            SwitchMode(ItemType.placeable);
        }
        else
        {
            SwitchMode(ItemType.usable);
        }
    }
    void Scroll()
    {
        bool firstTime = false;
        if (slots == null)
        {
            itemTypeText.text = (mode).ToString();
            itemTypeText.color = itemTypeColors[(int)mode];
            firstTime = true;
            HoldTransforms h = canvas.GetComponent<HoldTransforms>();
            slots = h.getTransforms();
            int i = 0;

            numSlots = slots.Length;
            animations = new Animator[numSlots];
            slotImages = new Image[numSlots];
            selectedImages = new Image[numSlots];
            slotItemAmounts = new TextMeshProUGUI[numSlots];
            foreach (GameObject t in slots)
            {
                slotImages[i] = t.GetComponentsInChildren<Image>()[0];
                selectedImages[i] = t.GetComponentsInChildren<Image>()[1];
                animations[i] = t.GetComponentInChildren<Animator>();
                slotItemAmounts[i] = t.GetComponentInChildren<TextMeshProUGUI>();
                i++;
            }
            selected[0] = 0;
            selected[1] = 0;
            selected[2] = 0;
            scroll_amt = 0;
        }
        // variable selected will be the current item selected....
        if (Input.GetAxis("Mouse ScrollWheel") == 0 && !firstTime)
        {
            return;
        }
        scroll_amt += -Input.GetAxis("Mouse ScrollWheel") * scrollSpeed;
        selected[(int)mode] = (selected[(int)mode] + (int)scroll_amt) % (numSlots);
        if (selected[(int)mode] < 0)
        {
            selected[(int)mode] = numSlots + selected[(int)mode];
        }
        scroll_amt += -(int)scroll_amt;

        UpdateSelected();
        UpdateItemName();
        ShowInventory();
    } // for scrolling through all the inventory items - also contains initialization for inventory slots for some reason
    void UpdateSelected()
    {
        for (int i = 0; i < numSlots; i++)
        {
            if (selected[(int)mode] == i)
            {
                //slots[i].transform.localScale = Vector3.one * magnifyMagnitude;
                selectedImages[i].enabled = true;
                slotImages[i].color = Color.white;
                //animations[i].SetBool("Selected", true);

            }
            else
            {
               selectedImages[i].enabled = false;
                slotImages[i].color = new Color(.8f, .8f, .8f);
               // slots[i].transform.localScale = Vector3.one;
               // animations[i].SetBool("Selected", false);
            }
        }
    }
    void UpdateItemName ()
    {
        if (getSelected() != null)
        {
            itemNameText.text = getSelected().ItemName;
        }
        else
        {
            itemNameText.text = "-";
        }
    }
    public InventoryItem getSelected()
    {
        switch (mode)
        {
            case ItemType.usable:
                if (indexUsable.Count <= selected[(int)mode])
                {
                    return null;
                }
                return templateItems[indexUsable[selected[(int)mode]]];
            case ItemType.resources:
                if (indexResources.Count <= selected[(int)mode])
                {
                    return null;
                }
                return templateItems[indexResources[selected[(int)mode]]];
            case ItemType.placeable:
                if (indexPlaceable.Count <= selected[(int)mode])
                {
                    return null;
                }
                return templateItems[indexPlaceable[selected[(int)mode]]];
            default:
                return null;
        }
    }
    public InventoryItem getSelected(ItemType m)
    {
        switch (m)
        {
            case ItemType.usable:
                if (indexUsable.Count <= selected[(int)m])
                {
                    return null;
                }
                return templateItems[indexUsable[selected[(int)m]]];
            case ItemType.resources:
                if (indexResources.Count <= selected[(int)m])
                {
                    return null;
                }
                return templateItems[indexResources[selected[(int)m]]];
            case ItemType.placeable:
                if (indexPlaceable.Count <= selected[(int)m])
                {
                    return null;
                }
                return templateItems[indexPlaceable[selected[(int)m]]];
            default:
                return null;
        }
    }
    void SwitchMode(ItemType newMode)
    {
        if (mode == newMode)
        {
            return;
        }
        mode = newMode;
        itemTypeText.text = (mode).ToString();
        itemTypeText.color = itemTypeColors[(int)mode];
        ShowInventory();
        UpdateSelected();
        UpdateItemName();
    }    // for switching modes

    public ItemType getMode ()
    {
        return mode;
    }

    void ClearDisplay ()
    {
        for(int i = 0; i < numSlots; i++)
        {
            slotImages[i].sprite = null;
            slotItemAmounts[i].text = "";
        }
    } // Shold never be called outside of showinventory!
    void ShowInventory() // for changing the item images in each inventory slot and the amounts
    {
        ClearDisplay();
        List<int> list = null;
        switch (mode)
        {
            case ItemType.usable:
                list = indexUsable;
                break;
            case ItemType.resources:
                list = indexResources;
                break;
            case ItemType.placeable:
                list = indexPlaceable;
                break;
        }
        for (int i = 0; i < numSlots; i++)
        {
            if (i >= list.Count || list == null)
            {
                slotImages[i].sprite = emptySprite;
                //break;
            }
            else
            {
                slotImages[i].sprite = templateItems[list[i]].image;
                slotItemAmounts[i].text = "" + amounts[list[i]];
            }
        }
    }
    public void AddItemToLists(int itemIndex, ItemType itype) //for adding an item to the lists
    {
        if (indexUsable == null)
        {
            Start();
        }
        switch (itype)
        {
            case ItemType.usable:
                if (indexUsable.IndexOf(itemIndex) == -1)
                    indexUsable.Add(itemIndex);
                if (amounts[itemIndex] < 1)
                    indexUsable.Remove(itemIndex);
                break;
            case ItemType.resources:
                if (indexResources.IndexOf(itemIndex) == -1)
                    indexResources.Add(itemIndex);
                if (amounts[itemIndex] < 1)
                    indexResources.Remove(itemIndex);
                break;
            case ItemType.placeable:
                if (indexPlaceable.IndexOf(itemIndex) == -1)
                    indexPlaceable.Add(itemIndex);
                if (amounts[itemIndex] < 1)
                    indexPlaceable.Remove(itemIndex);
                break;
        }
        ShowInventory();
    }
}
