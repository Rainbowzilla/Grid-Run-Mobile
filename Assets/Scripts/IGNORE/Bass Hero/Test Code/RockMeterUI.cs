using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RockMeterUI : MonoBehaviour
{
    public float rm;
    public Slider slider;
    public Image fill;

    void Update()
    {
        rm = PlayerPrefs.GetInt("RockMeter");
        slider.value = rm;
    }

    public void ChangeFillColor()
    {
        if (rm <= 33)
        {
            fill.color = Color.red;
        }
        else if (rm <= 66)
        {
            fill.color = Color.yellow;
        }
        else if (rm <= 100)
        {
            fill.color = Color.green;
        }
    }
}
