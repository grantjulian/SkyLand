using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Inventory Item", order = 1)]
public class InventoryItem : ScriptableObject
{
    [Header("Basic Information")]
    public string ItemName;
    public Sprite image;
    public ItemType type;
    public string description;
    [Header("Can the item be crafted?")]
    public bool craftable;
    [Header("Is this a template for an island upgrade?")]
    public bool upgrade;
    [Header("Crafting / Upgrade information")]
    public InventoryItem[] recipeItems;
    public int[] recipeAmounts;
    public int amountPerCraft;
    [Header("Placeable information")]
    public GameObject prefab;
    public float radius;
}

public enum ItemType
{
    // usable are any item that can be used in inventory i.e. laser
    //placeable are any item that can be placed in world i.e. cannon
    // resources are items that are extracted from the world i.e. wood stone metal
    usable, placeable, resources,  NUM_TYPES
}