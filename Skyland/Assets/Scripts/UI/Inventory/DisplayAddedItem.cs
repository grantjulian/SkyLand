using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DisplayAddedItem : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject canvasPrefab;
    [Range(1,5)]
    public float animationSpeed;
    [Range(1, 7)]
    public float timeDelay;
    public int batchSize;
    GameObject canvas;
    public GameObject AddedItem;
    Queue<InventoryItem> itemlist;
    Queue<int> amountlist;
    float waitTime;
    bool nextFrame;


    void Start()
    {
        canvas = Instantiate(canvasPrefab);
        waitTime = 0;
        nextFrame = true;
        itemlist = new Queue<InventoryItem>();
        amountlist = new Queue<int>();
    }

    private void Update()
    {
        waitTime += Time.deltaTime;
        if (nextFrame)
        {
            nextFrame = false;
            int max = (itemlist.Count < batchSize) ? itemlist.Count : batchSize;
            for (int i = 0; i < max; i++)
            {
                createObject(itemlist.Dequeue(), amountlist.Dequeue(), 120 * i);
            }
        }
        if (waitTime > timeDelay && itemlist.Count > 0)
        {
            nextFrame = true;
            waitTime = 0;
        }
    }

    public void AddItemDisplay(InventoryItem item, int amount)
    {
        if (itemlist == null)
        {
            return;
        }
        itemlist.Enqueue(item);
        amountlist.Enqueue(amount);
    }
    void createObject (InventoryItem item, int amount, float yoffset)
    {
        Vector3 pos = canvas.transform.position + Vector3.up * yoffset * canvas.transform.localScale.y;
        GameObject instance = Instantiate(AddedItem, pos, Quaternion.identity, canvas.transform);
        instance.GetComponentInChildren<Animator>().speed = animationSpeed;
        Image image = instance.GetComponentInChildren<Image>();
        TextMeshProUGUI amounttext = instance.GetComponentInChildren<TextMeshProUGUI>();
        image.sprite = item.image;
        amounttext.text = ((amount > 0) ? "+" : "") + amount;
        Destroy(instance, 5);
    }
}
