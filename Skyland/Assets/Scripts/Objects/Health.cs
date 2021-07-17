using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public ObjectType type;
    GameObject gameController; // need to notify of death
    public float StartHealth;
    float health;
    public GameObject particles;
    bool isaGoner;
    public float Fell_Magnitude;

    private void Awake()
    {
        health = StartHealth;
        isaGoner = false;
        gameController = GameObject.FindGameObjectWithTag("GameController");
    }
    public void Damage(float damage, Inventory player)
    {
        health -= damage;
        if (health <= 0 && !isaGoner)
        {
            isaGoner = true;
            BreakObject(player);
        }
    }
    void BreakObject(Inventory player)
    {
        gameObject.layer = 11;
        Destroy(gameObject, 3);
        Rigidbody rb = gameObject.AddComponent<Rigidbody>();
        float xrand = Random.Range(-0.5f, 0.5f);
        float zrand = Random.Range(-0.5f, 0.5f);
        Vector3 torqueDir = new Vector3(xrand, 0, zrand);
        torqueDir.Normalize();
        torqueDir *= Fell_Magnitude;
        rb.AddTorque(torqueDir);
        BoxCollider [] bc = GetComponentsInChildren<BoxCollider>();
        foreach (BoxCollider b in bc)
        {
            b.enabled = false;
        }
        if (particles != null)
        {
            GameObject destroyparticles = Instantiate(particles, transform.position + Vector3.up * 3, Quaternion.identity);
            Destroy(destroyparticles, 5);
        }
        if (type == ObjectType.Rock || type == ObjectType.Tree)
        {
            gameController.GetComponent<WorldGen>().OnObjectDeath(transform.position, type);
        }
        GivePlayer(player);
    }
    public float getHealth()
    {
        return health;
    }
    public float getMaxHealth()
    {
        return StartHealth;
    }
    public ObjectType getType ()
    {
        return type;
    }
    void GivePlayer (Inventory player)
    {
        player.AddInventory(gameObject.GetComponent<Inventory>());
    }
}
