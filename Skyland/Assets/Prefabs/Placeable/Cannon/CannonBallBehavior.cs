using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonBallBehavior : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject explosion; //explosion prefab
    GameObject player;
    public float minTime; //minimum time to blow up after being fired
    public float radius; //explosive radius
    public float damage; //damage to be dealt
    float timer = 0;
    bool isDoomed = false;
    void Start()
    {

    }
    void Update()
    {
        timer += Time.deltaTime;
    }
    public void setPlayer (GameObject p)
    {
        player = p;
    }

    // Update is called once per frame
    void OnCollisionEnter(Collision collision)
    {
        if (timer > minTime && !isDoomed)
        {
            isDoomed = true;
            GameObject boom = Instantiate(explosion, collision.GetContact(0).point, Quaternion.identity);
            Destroy(boom, 10);
            Destroy(gameObject);
            Collider[] colliders = Physics.OverlapSphere(collision.GetContact(0).point, radius);
            List<Health> healths = new List<Health>();
            foreach(Collider c in colliders)
            {
                Health h = c.GetComponentInParent<Health>();
                if (h != null)
                {
                    if (!healths.Contains(h))
                    {
                        healths.Add(h);
                        h.Damage(damage, player.GetComponent<Inventory>());
                    }
                }
            }
        }
    }
}
