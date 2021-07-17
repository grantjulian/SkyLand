using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CardManager : MonoBehaviour
{
    public Image sprite;
    public Sprite empty;
    public TextMeshProUGUI name;
    public TextMeshProUGUI description;
    public Image faded;
    public TextMeshProUGUI amount;
    public Image[] recipeItems;
    public TextMeshProUGUI[] recipeCosts;
    public Image blackout;
    InventoryItem myItem;

    public void SetProperties(InventoryItem item)
    {
        myItem = item;
        sprite.sprite = myItem.image;
        name.text = myItem.ItemName;
        description.text = myItem.description;
        faded.sprite = myItem.image;
        amount.text = "" + myItem.amountPerCraft;
        for (int i = 0; i < recipeItems.Length; i++)
        {
            if (i < myItem.recipeItems.Length && myItem.recipeItems[i] != null)
            {
                recipeItems[i].sprite = myItem.recipeItems[i].image;
                recipeCosts[i].text = "" + myItem.recipeAmounts[i];
            }
            else
            {
                recipeItems[i].sprite = empty;
                recipeCosts[i].text = "";
            }
        }
    }
    public void SetEnabled(bool enabled)
    {
        if (!enabled)
        { 
            blackout.color = new Color(0,0,0,.8f);
        }
        else if (enabled)
        {
            blackout.color = new Color(0, 0, 0, 0);
        }
    }
    public InventoryItem getMyItem ()
    {
        return myItem;
    }
    public void ChangeSelected()
    {
        gameObject.GetComponentInParent<ObeliskManager>().ChangeSelectedCard(myItem);
    }
}
