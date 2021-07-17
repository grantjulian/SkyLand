using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorManager : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject circle;
    public GameObject cross;
    CursorMode current;
    public void changeMode (CursorMode mode)
    {
        if (current == mode)
        {
            return;
        }
        switch (mode)
        {
            case CursorMode.circle:
                circle.SetActive(true);
                cross.SetActive(false);
                break;
            case CursorMode.cross:
                circle.SetActive(false);
                cross.SetActive(true);
                break;
            case CursorMode.neither:
                circle.SetActive(false);
                cross.SetActive(false);
                break;
        }
        current = mode;
    }
}

public enum CursorMode
{
    circle, cross, neither
}