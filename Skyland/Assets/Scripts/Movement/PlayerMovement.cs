using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // Start is called before the first frame update
    public float walkSpeed;
    public float jumpHeight = 12;
    public float fallGravityMult = .2f;
    public float jumpTimingBuffer = .2f;
    bool toJump;
    float toJumpTimer;
    Rigidbody rb;
    bool active;
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        active = true;
        toJump = false;
    }

    public void setMovement(bool state)
    {
        active = state;
    }
    // Update is called once per frame
    void Update()
    {
        if (!active)
        {
            return;
        }
        if (toJump)
        {
            updateJumpTimer();
        }
        if (Input.GetMouseButtonDown(1))
        {
            toJump = true;
            toJumpTimer = jumpTimingBuffer;
        }
        if (toJump && isGrounded())
        {
            rb.velocity = Vector3.zero;
            rb.AddForce(transform.up * jumpHeight, ForceMode.VelocityChange);
            toJump = false;
        }
        if (rb.velocity.y < 0f)
        {
            rb.AddForce(Vector3.down * fallGravityMult * Time.deltaTime, ForceMode.VelocityChange);
        }
    }
    void FixedUpdate()
    {
        if (!active)
        {
            return;
        }
        Vector3 xaxis = Input.GetAxisRaw("Horizontal") * Vector3.right;
        Vector3 yaxis = Input.GetAxisRaw("Vertical") * Vector3.forward;
        if (xaxis != Vector3.zero || yaxis != Vector3.zero)
        {
            Vector3 move = (xaxis + yaxis).normalized * walkSpeed * Time.deltaTime;
            MoveUp(move);
            transform.Translate(move);
        }
    }

    void MoveUp(Vector3 move)
    {
        int layermask = 1 << 13;
        float buffer = .05f;
        Vector3 castFrom = transform.position - new Vector3(0,((transform.localScale.y - .1f) / 2f),0);
        float maxDistanceForward = transform.localScale.z / 2 + Vector3.Dot(move, Vector3.forward) + buffer;
        float maxDistanceRight = transform.localScale.x / 2 + Vector3.Dot(move, Vector3.right) + buffer;
        float maxDistanceLeft = transform.localScale.x / 2 + Vector3.Dot(move, Vector3.left) + buffer;
        float maxDistanceBack = transform.localScale.z / 2 + Vector3.Dot(move, Vector3.back) + buffer;
        bool hop = false;

        if (Physics.Raycast(castFrom, transform.forward, maxDistanceForward, layermask))
        {
            hop = true;
        }
        if (Physics.Raycast(castFrom, transform.right, maxDistanceRight, layermask))
        {
            hop = true;
        }
        if (Physics.Raycast(castFrom, -transform.right, maxDistanceLeft, layermask))
        {
            hop = true;
        }
        if (Physics.Raycast(castFrom, -transform.forward, maxDistanceBack, layermask))
        {
            hop = true;
        }
        if (hop)
        {
            transform.Translate(Vector3.up * .6f);
        }
    }
    bool isGrounded ()
    {
        if (Physics.Raycast(transform.position + transform.forward * transform.localScale.z / 2, Vector3.down, transform.localScale.y / 2 + .5f))
        {
            return true;
        }
        else if (Physics.Raycast(transform.position - transform.forward * transform.localScale.z / 2, Vector3.down, transform.localScale.y / 2 + .5f))
        {
            return true;
        }
        else if (Physics.Raycast(transform.position + transform.right * transform.localScale.z / 2, Vector3.down, transform.localScale.y / 2 + .5f))
        {
            return true;
        }
        else if (Physics.Raycast(transform.position - transform.right * transform.localScale.z / 2, Vector3.down, transform.localScale.y / 2 + .5f))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    void updateJumpTimer()
    {
        toJumpTimer -= Time.deltaTime;
        if (toJumpTimer < 0)
        {
            toJump = false;
        }
    }
}
