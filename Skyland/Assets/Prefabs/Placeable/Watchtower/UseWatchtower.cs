using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UseWatchtower : MonoBehaviour
{
    public GameObject attachPlayerTo;
    public GameObject canopy;
    public GameObject supportPrefab;
    public GameObject supportParent;
    GameObject player;
    Stack<GameObject> supports;
    public float camDistance;
    public float buildTimer;
    float timer;
    bool beingUsed;
    public int lowestHeight;
    public int hightestHeight;
    int height;
    void Start()
    {
        height = lowestHeight;
        timer = 0;
        supports = new Stack<GameObject>();       
    }
    // Update is called once per frame
    void Update()
    {
        if (!beingUsed)
        {
            return;
        }
        float axis = Input.GetAxisRaw("Vertical");
        timer += Time.deltaTime;
        if (axis != 0 && timer > buildTimer)
        {
            timer = 0;
            if (axis > 0)
            {
                AddSupport();
            }
            if (axis < 0)
            {
                RemoveSupport();
            }
        }
    }
    void AddSupport()
    {
        if (height < hightestHeight)
        {
            canopy.transform.position += Vector3.up;
            supports.Push(Instantiate(supportPrefab, canopy.transform.position, Quaternion.identity, supportParent.transform));
            height++;
        }
    }
    void RemoveSupport()
    {
        if (height > lowestHeight)
        {
            canopy.transform.position -= Vector3.up;
            Destroy(supports.Pop());
            height--;
        }
    }
    public void AttachPlayer(GameObject p)
    {
        if (beingUsed)
        {
            return;
        }
        player = p;
        player.transform.parent = attachPlayerTo.transform;
        player.transform.position = attachPlayerTo.transform.position + Vector3.up;
        player.GetComponent<CameraFollow>().setFollowDistance(camDistance);
        beingUsed = true;
    }
    public void DetachPlayer(GameObject p)
    {
        p.GetComponent<CameraFollow>().setFollowDistance();
        p.transform.parent = null;
        beingUsed = false;
        player = null;
    }
}
