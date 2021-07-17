using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    // Start is called before the first frame update
    bool xState, yState;
    public Camera cam;
    public float xSensitivity;
    public float ySensitivity;
    float xrotation;
    float yrotation;
    public float MinAngle;
    public float MaxAngle;
    public float defaultFollowDistance;
    float followDistance;
    GameObject camObject;
    Transform camTransform;
    void Start()
    {
        camObject = cam.gameObject;
        camTransform = camObject.transform;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        xrotation = 0;
        yrotation = 0;
        xState = true;
        yState = true;
        setFollowDistance();
    }
    public void setState(bool x, bool y)
    {
        xState = x;
        yState = y;
    }
    // Update is called once per frame
    void Update()
    {
        // check how far off the ground the camera is...
        Vector3 relVector = camTransform.position - transform.position;
        float angle = Vector3.Angle(relVector, relVector - Vector3.up * relVector.y);
        float xchange = Input.GetAxisRaw("Mouse X") * xSensitivity;
        float ychange = Input.GetAxisRaw("Mouse Y") * -ySensitivity;
        if (angle < 22.5)
        {
            camTransform.position = transform.position + relVector.normalized * (Mathf.Sin((angle) * 4 * Mathf.PI / 180) + .25f) * followDistance;
        }
        else
        {
            camTransform.position = transform.position + relVector.normalized * 1.25f * followDistance;
        }
        if (xchange != 0 && xState)
        {
            RotatePlayer(xchange);
        }
        if (ychange != 0 && yState)
        {
            if ((angle + ychange > MinAngle && ychange < 0) || (angle + ychange < MaxAngle && ychange > 0))
            {
                RotateCamera(ychange);
            }
            else if (angle + ychange > MaxAngle)
            {
                RotateCamera(MaxAngle - angle);
            }
            else if (angle + ychange < MinAngle)
            {
                RotateCamera(MinAngle-angle);
            }
        }
    }
    void RotateCamera(float dy)
    {
        yrotation += dy;
        if (yrotation > 90 || yrotation < -90)
        {
            yrotation = (yrotation > 90) ? 90 : -90;
        }
        //camTransform.eulerAngles = new Vector3(yrotation, camTransform.eulerAngles.y, camTransform.eulerAngles.z);
        camTransform.RotateAround(transform.position, transform.right, dy);
    }
    void RotatePlayer(float dx)
    {
        transform.Rotate(Vector3.up, dx);
    }
    public void setFollowDistance (float fd) //set follow distance of the camera
    {
        followDistance = fd;
    }
    public void setFollowDistance() //uses default follow distance
    {
        followDistance = defaultFollowDistance;
    }
}
