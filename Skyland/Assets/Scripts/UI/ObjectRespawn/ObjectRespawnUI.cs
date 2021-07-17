using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObjectRespawnUI : MonoBehaviour
{
    List<Slider> sliders;
    public GameObject sliderPrefab;
    public GameObject parent;
    public GameObject Full;
    // Start is called before the first frame update
    void Start()
    {
        sliders = new List<Slider>();
        Full.SetActive(true);
    }

    // Update is called once per frame
    public void AddSlider(float seconds)
    {
        sliders.Add(Instantiate(sliderPrefab,parent.transform).GetComponent<Slider>());
        sliders[sliders.Count - 1].maxValue = seconds;
        Full.SetActive(false);
    }
    void Update()
    {
        for(int i = 0; i < sliders.Count; i++)
        {
            sliders[i].value += Time.deltaTime;
            if (sliders[i].value >= sliders[i].maxValue)
            {
                GameObject temp = sliders[i].gameObject;
                sliders.Remove(sliders[i]);
                Destroy(temp);
                i--;
            }
        }
        if (sliders.Count == 0)
        {
            Full.SetActive(true);
        }
    }
}
