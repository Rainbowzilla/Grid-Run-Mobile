using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScoreUIColorChanger : MonoBehaviour
{
    public TextMeshProUGUI text;

    // Update is called once per frame
    void Update()
    {
        if (GameManagerBassHero.Instance.isPlayerInStarMode == false)
        {
            if (GameManagerBassHero.Instance.mutiplier == 4)
            {
                text.color = new Color(128, 0, 128);
            }
            else if (GameManagerBassHero.Instance.mutiplier == 3)
            {
                text.color = Color.green;
            }
            else if (GameManagerBassHero.Instance.mutiplier == 2)
            {
                text.color = new Color(255, 165, 0);
                text.enabled = true;
            }
            else if (GameManagerBassHero.Instance.mutiplier == 1)
            {
                text.color = Color.white;
                text.enabled = false;
            }
        }
        else if (GameManagerBassHero.Instance.isPlayerInStarMode == true)
            text.color = Color.cyan;
    }
}
