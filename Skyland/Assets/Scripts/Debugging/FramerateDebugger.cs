using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FramerateDebugger : MonoBehaviour
{
    public TextMeshProUGUI text;
    public int updateEvery;
    float [] lastFrames = new float[30];
    int counter = 0;
    // Update is called once per frame
    void Update()
    {       
        float sum = 0;
        for (int i = 0; i < lastFrames.Length - 1; i++)
        {
            lastFrames[i+1] = lastFrames[i];
            sum += lastFrames[i];
        }
        lastFrames[0] = Time.deltaTime;
        if (counter < lastFrames.Length)
        {
            counter++;
            return;
        }
        counter = 0;
        text.text = "" + (int)(lastFrames.Length / sum);
    }
}
