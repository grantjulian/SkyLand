using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interact : MonoBehaviour
{
    public PlayerMovement pm;
    public ObjectSelect os;
    public CameraFollow cf;
    public BottomInventory bi;
    public WorldGen wg;
    public GameObject interact_Canvas_prefab;
    public Transform camTransform;
    public float rayCastDistance;
    GameObject interact_Canvas;
    GameObject interacting;
    RaycastHit hit;
    public bool hasHit;

    // For obelisk UI
    [Header("Obelisk UI")]
    public GameObject obeliskCanvas_prefab;
    GameObject obeliskCanvas;

    void Start()
    {
        interacting = null;
        interact_Canvas = Instantiate(interact_Canvas_prefab);
        interact_Canvas.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && interacting != null)
        {
            StopInteraction();
        }
    }

    public void TriggerEscape()
    {
        StopInteraction();
    }
    void OnTriggerStay(Collider collider)
    {
        if (interacting != null)
        {
            return;
        }
        //DoRaycast();
        if (true)//(hasHit)
        {
            if (Input.GetKey(KeyCode.I))
            {
                StartInteraction(collider.gameObject);
                //StartInteraction(hit.collider.gameObject);
            }
            else
            {
                interact_Canvas.SetActive(true);
            }
        }
        else
        {
            //interact_Canvas.SetActive(false);
        }
    }
    void OnTriggerExit()
    {
        interact_Canvas.SetActive(false);
    }
    void ToggleMovement(bool toggle)
    {
        pm.setMovement(toggle);
        os.setGameplayState(toggle);
        cf.setState(toggle, toggle);
        //cf.setRotatable(toggle);
    }
    void ToggleUI(bool toggle)
    {
        //wg.setUIState(toggle);
        bi.setState(toggle);
    }
    void StartInteraction(GameObject g)
    {
        string s = g.tag;
        interacting = g;
        ToggleMovement(false);
        ToggleUI(false);
        interact_Canvas.SetActive(false);
        switch (s)
        {
            case "Obelisk":
                OpenObeliskUI();
                break;
            case "Cannon":
                OpenCannon();
                break;
            case "Watchtower":
                OpenWatchtower();
                break;
        }
    }
    public void StopInteraction()
    {
        string s = interacting.tag;
        ToggleMovement(true);
        ToggleUI(true);
        switch (s)
        {
            case "Obelisk":
                CloseObeliskUI();
                break;
            case "Cannon":
                CloseCannon();
                break;
            case "Watchtower":
                CloseWatchtower();
                break;
        }
        interacting = null;
    }
    void OpenObeliskUI()
    {
        obeliskCanvas = Instantiate(obeliskCanvas_prefab);
        obeliskCanvas.GetComponent<ObeliskManager>().Initialize(gameObject);
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

    }
    void CloseObeliskUI()
    {
        Destroy(obeliskCanvas);
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void OpenCannon()
    {
        interacting.GetComponent<UseCannon>().AttachPlayer(gameObject);
        cf.setState(false, true);
    }
    void CloseCannon()
    {
        interacting.GetComponent<UseCannon>().DetachPlayer(gameObject);
    }
    void OpenWatchtower()
    {
        interacting.GetComponent<UseWatchtower>().AttachPlayer(gameObject);
        cf.setState(true, true);
    }
    void CloseWatchtower()
    {
        interacting.GetComponent<UseWatchtower>().DetachPlayer(gameObject);
    }
    void DoRaycast()
    {
        int layerMask = 1 << 12;
        hasHit = Physics.Raycast(transform.position, transform.TransformDirection(camTransform.forward), out hit, rayCastDistance, layerMask);
    }
}
