using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StarPowerUI : MonoBehaviour
{
    float sp;
    public Slider slider;
    public Image fill;

    void Start()
    {
       sp = GameManagerBassHero.starPowerTime = 0;
    }
    void Update()
    {
        slider.value = sp;
        if (GameManagerBassHero.Instance.isPlayerInStarMode == false)
        {
            sp = GameManagerBassHero.starPowerTime;
        }
        if (GameManagerBassHero.Instance.isPlayerInStarMode == true)
        {
            sp = GameManagerBassHero.countTimer;
        }
    }

   /* public void ChangeFillColor()
    {
        if (sp <= 33)
        {
            fill.color = Color.red;
        }
        else if (sp <= 66)
        {
            fill.color = Color.yellow;
        }
        else if (sp <= 100)
        {
            fill.color = Color.green;
        }
    }
   */
}
