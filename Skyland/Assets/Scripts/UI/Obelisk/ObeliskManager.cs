using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObeliskManager : MonoBehaviour
{
    GameObject player;
    Inventory playerInv;
    public GameObject cardParent;
    public GameObject selectedCardParent;
    public GameObject blankCardPrefab;
    public InventoryTemplate items;
    public float distanceBetweenCards;
    List<GameObject> cards;
    GameObject selectedCard;
    ObeliskStates state = ObeliskStates.Craftable;
    // Start is called before the first frame update

    public void Initialize(GameObject Player)
    {
        player = Player;
        playerInv = player.GetComponent<Inventory>();
        cards = new List<GameObject>();
        Clear();
        Show();
    }
    void Clear()
    {
        if (cards != null)
        {
            foreach (GameObject card in cards)
            {
                Destroy(card);
            }
        }
        if (selectedCard != null)
        {
            Destroy(selectedCard);
        }
    }
    void Show()
    {
        foreach (InventoryItem i in items.InvItems)
        {
            bool added = false;
            if (i != null)
            {
                if (state == ObeliskStates.Craftable && i.craftable)
                {
                    cards.Add(GenerateCard(i, cardParent));
                    added = true;
                }
                else if (state == ObeliskStates.Upgrades && i.upgrade)
                {
                    cards.Add(GenerateCard(i, cardParent));
                    added = true;
                }
                if (cards.Count == 1 && added)
                {
                    selectedCard = GenerateCard(i, selectedCardParent);
                }
            }
        }
        int j = 0;
        foreach (GameObject card in cards)
        {
            card.transform.position += Vector3.right * j * distanceBetweenCards * card.GetComponent<RectTransform>().sizeDelta.x;
            j++;
        }
    }

    public void Buy()
    {
        CardManager cm = selectedCard.GetComponent<CardManager>();
        InventoryItem i = cm.getMyItem();
        if (CanAfford(i))
        {
            playerInv.AddItem(i.ItemName, i.amountPerCraft);
            int j = 0;
            foreach (InventoryItem removeItems in i.recipeItems)
            {
                playerInv.AddItem(removeItems.ItemName, -i.recipeAmounts[j]);
                j++;
            }
            RefreshCards();
        }
    }
    public void ChangeState(int newState)
    {
        state = (ObeliskStates)newState;
        Clear();
        RefreshCards();
        Show();
    }
    public void ChangeSelectedCard (InventoryItem item)
    {
        selectedCard.GetComponent<CardManager>().SetProperties(item);
        RefreshCards();
    }
    void RefreshCards()
    {
        CardManager cm;
        if (cards != null)
        {
            foreach (GameObject card in cards)
            {
                cm = card.GetComponent<CardManager>();
                cm.SetEnabled(CanAfford(cm.getMyItem()));
            }
        }
        if (selectedCard != null)
        {
            cm = selectedCard.GetComponent<CardManager>();
            cm.SetEnabled(CanAfford(cm.getMyItem()));
        }
    }
    public void ExitScreen()
    {
        player.GetComponent<Interact>().StopInteraction();
    }
  
    GameObject GenerateCard(InventoryItem item, GameObject parent)
    {
        GameObject card = Instantiate(blankCardPrefab, parent.transform);
        card.GetComponent<CardManager>().SetProperties(item);
        card.GetComponent<CardManager>().SetEnabled(CanAfford(item));
        return card;
    }
    bool CanAfford(InventoryItem item)
    {
        for (int i = 0; i < item.recipeItems.Length; i++)
        {
            if (playerInv.getAmountOfItem(item.recipeItems[i]) < item.recipeAmounts[i])
            {
                return false;
            }
        }
        return true;
    }
}

public enum ObeliskStates
{
    Craftable, Upgrades
}
