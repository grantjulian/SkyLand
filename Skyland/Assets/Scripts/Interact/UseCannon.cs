using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UseCannon : MonoBehaviour
{

    private GameObject player;
    public GameObject attachPlayerTo;
    public GameObject cannonBody;
    public GameObject CannonBall;
    Animator animator;
    private float timer;
    public float spinSpeed;
    public float aimSpeed;
    public float camDistance;
    public float firePower;
    public float reloadTimer;
    bool beingUsed = false;
    bool canFire;

    void Start()
    {
        animator = gameObject.GetComponentInParent<Animator>();
        animator.Play("build");
    }
    void Update()
    {
        if (!beingUsed)
        {
            return;
        }

        timer += Time.deltaTime;
        if (timer > reloadTimer)
        {
            canFire = true;
        }
        if (canFire && Input.GetMouseButtonDown(0))
        {
            FireCannon();
        }
        float h = Input.GetAxisRaw("Horizontal");
        float v = -Input.GetAxisRaw("Vertical");

        if (h != 0)
        {
            attachPlayerTo.transform.Rotate(Vector3.up, h * spinSpeed * Time.deltaTime);
        }
        if (v != 0)
        {
            cannonBody.transform.Rotate(Vector3.right, v * aimSpeed * Time.deltaTime);
        }
    }
    void FireCannon ()
    {
        animator.Play("fire");
        GameObject cannonball = Instantiate(CannonBall, cannonBody.transform.position - cannonBody.transform.up * .05f, Quaternion.identity);
        cannonball.GetComponent<Rigidbody>().AddForce(firePower * cannonBody.transform.forward, ForceMode.VelocityChange);
        cannonball.GetComponent<CannonBallBehavior>().setPlayer(player);
        canFire = false;
        timer = 0;
    }
    public void AttachPlayer (GameObject p)
    {
        if (beingUsed)
        {
            return;
        }
        player = p;
        player.transform.parent = attachPlayerTo.transform;
        player.transform.position = gameObject.transform.position - 1 * attachPlayerTo.transform.forward;
        player.GetComponent<CameraFollow>().setFollowDistance(camDistance);
        player.transform.LookAt(player.transform.position + attachPlayerTo.transform.forward);
        beingUsed = true;
        timer = 0;
        canFire = false;
    }
    public void DetachPlayer (GameObject p)
    {
        p.GetComponent<CameraFollow>().setFollowDistance();
        p.transform.parent = null;
        beingUsed = false;
        player = null;
    }
}
