using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

//This scripts name does not describe what it does properly, in fact this might be the most important script
// in the game. What this script does is it allows the user to use whatever usable or placeable item they have
// selected in their inventory
public class ObjectSelect : MonoBehaviour
{
    public float maxDistance;
    public GameObject Canvas;
    //
    public Transform arm;
    // Laser Stuff
    public LineRenderer Laser;
    public ParticleSystem Impact;
    // End Laser Stuff
    // Harvesting Stuff
    HealthBarManager hbm;
    CursorManager cm;
    GameObject last;
    [Header("Harvesting / Damage")]
    public float DamagePerSecond;
    public Inventory playerInv;
    public BottomInventory bInv;
    bool state;
    bool hasHit;
    RaycastHit hit;
    // End Harvesting Stuff
    //for clearing previous item effects
    InventoryItem previousItem;
    //Building Stuff
    public Material Blueprint;
    GameObject buildGhost;
    public int maxBuildDistance;

    private void Start()
    {
        GameObject c = Instantiate(Canvas);
        hbm = c.GetComponent<HealthBarManager>();
        cm = c.GetComponent<CursorManager>();
        cm.changeMode(CursorMode.cross);
        state = true;
        buildGhost = null;

    }
    private void DealDamage(GameObject g, float damage)
    {
        Health health = g.GetComponentInParent<Health>();
        health.Damage(damage, playerInv);

        ObjectType type = health.getType();
        float object_health = health.getHealth();
        float max_health = health.getMaxHealth();


        float par1 = object_health / max_health;
        float par2 = object_health / max_health + (damage / max_health);
        string par3 = type.ToString() + ": " + string.Format("{0:0.0}", object_health) + " / " + max_health;

        hbm.setHealthBar(par1, par2, par3);
    }
    void Update()
    {
        ItemType mode = bInv.getMode();
        InventoryItem item = bInv.getSelected(mode);

        //check if item has changed
        if (previousItem != null && previousItem != item || (!state && previousItem != null))
        {
            switch (previousItem.ItemName) // switch through usables
            {
                case "Laser Gun":
                    DisableLaser();
                    break;
            }
            if (previousItem.type == ItemType.placeable)
            {
                DisableBuild();
            }
        }

        UseRaycast();

        if (!state)
        {
            return;
        }
        //////////////////////////////////////////////////////////
        //////////////////////////////////////////////////////////
        //////////////// CASES FOR EACH ITEM TYPE ////////////////
        //////////////////////////////////////////////////////////
        //////////////////////////////////////////////////////////
        if (mode == ItemType.resources) // RESOURCES
        {
            // shouldnt really do anything
        }
        else if (mode == ItemType.placeable) // PLACEABLES
        {
            // enter build menu
            if (item != null)
            {
                /*
                switch (item.ItemName)
                {
                    case "Cannon":
                        UseBuild(item);
                        break;
                }
                */
                UseBuild(item);
                if (Input.GetMouseButtonDown(0) && buildGhost != null)
                {
                    PlaceBuild(item);
                }
            }
        }
        else if (mode == ItemType.usable) // USABLES
        {
            // open the UI and functionality for whatever the item is
            if (item != null)
            {
                switch (item.ItemName)
                {
                    case "Laser Gun":
                        UseLaser();
                        break;
                }
            }
        }
        previousItem = item;
    }
    void UseRaycast()
    {
        int layerMask = 1 << 9;
        hasHit = Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, maxDistance, layerMask);
        // Does the ray intersect any objects excluding the player layer
        if (hasHit)
        {
            hbm.toggleHealthBar(true);
            arm.LookAt(transform.TransformDirection(Vector3.forward) + arm.position);
            GameObject h = hit.collider.gameObject;
            DealDamage(h, 0);
        }
        else
        {
            hbm.toggleHealthBar(false);
        }
    }
    void UseLaser()
    {

        if (hasHit)
        {
            cm.changeMode(CursorMode.circle);
            GameObject h = hit.collider.gameObject;
            if (Input.GetMouseButton(0)) // shoot laser
            {
                Laser.enabled = true;
                Laser.SetPosition(0, Laser.transform.position);
                Laser.SetPosition(1, Laser.transform.position + (transform.position - Laser.transform.position) + transform.TransformDirection(Vector3.forward) * hit.distance);
                Impact.transform.position = Laser.transform.position + (transform.position - Laser.transform.position) + transform.TransformDirection(Vector3.forward) * (hit.distance - .2f);
                DealDamage(h, Time.deltaTime * DamagePerSecond);
                if (h == last) // if its the same one
                {

                }
                else // if its different
                {
                    Impact.Play(true);
                }
                last = h;
            }
            else // dont shoot laser
            {
                //DealDamage(h, 0);
                Laser.enabled = false;
                Impact.Stop(true);
                last = null;
                //Impact.gameObject.SetActive(false);
            }
        }
        else
        {
            last = null;
            Laser.enabled = false;
            Impact.Stop(true);
            cm.changeMode(CursorMode.cross);
        }
    }
    void DisableLaser()
    {
        hbm.toggleHealthBar(false);
        last = null;
        Laser.enabled = false;
        Impact.Stop(true);
        cm.changeMode(CursorMode.cross);
    }

    void UseBuild (InventoryItem item)
    {
        int layerMask = 1 << 13;
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, maxBuildDistance, layerMask))
        {
            if (((int)(hit.point.y * 2)) - hit.point.y * 2 != 0)
            {
                //invalid position
                return;
            }
            float yheight = (((int)(hit.point.y * 2))) / 2.0f;
            float xAdj = (hit.point.x > 0) ? .5f : -.5f;
            float zAdj = (hit.point.z > 0) ? .5f : -.5f;
            Vector3 newPos = new Vector3((int)(hit.point.x + xAdj), yheight, (int)(hit.point.z + zAdj));
            if (!WorldGen.CheckPosition(newPos,item.radius))
            {
                return;
            }
            if (buildGhost == null)
            {
                buildGhost = Instantiate(item.prefab);
                MeshRenderer[] renderers = buildGhost.GetComponentsInChildren<MeshRenderer>();
                Collider[] colliders = buildGhost.GetComponentsInChildren<Collider>();
                foreach (MeshRenderer m in renderers)
                {
                    m.material = Blueprint;
                }
                foreach(Collider c in colliders)
                {
                    c.enabled = false;
                }
            }
            buildGhost.transform.position = newPos;
            //buildGhost.transform.LookAt()
        }
    }
    void DisableBuild ()
    {
        Destroy(buildGhost);
        buildGhost = null;
    }

    void PlaceBuild (InventoryItem item)
    {
        WorldGen.AddPosition(buildGhost.transform.position, item.radius);
        GameObject build = Instantiate(item.prefab, buildGhost.transform.position, buildGhost.transform.rotation);
        playerInv.AddItem(item.ItemName, -1);
        Destroy(buildGhost);
    }

    public RaycastHit getRaycast()
    {
        return hit;
    }
    public bool isRaycastHit()
    {
        return hasHit;
    }
    public void setGameplayState(bool s)
    {
        state = s;
    }
}
