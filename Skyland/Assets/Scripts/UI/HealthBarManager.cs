using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HealthBarManager : MonoBehaviour
{
    public Slider curr_health;
    public Slider damage_done;
    public TextMeshProUGUI health_text;
    public GameObject parent;

    // Start is called before the first frame update
    void Start()
    {

    }
    public void setHealthBar(float health, float damage, string text)
    {
        toggleHealthBar(true);
        curr_health.value = health;
        damage_done.value = damage;
        health_text.text = text;
    }
    public void toggleHealthBar(bool enabled)
    {
        parent.SetActive(enabled);
    }
}
