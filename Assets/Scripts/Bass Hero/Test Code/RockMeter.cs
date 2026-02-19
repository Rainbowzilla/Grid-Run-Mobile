using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RockMeter : MonoBehaviour
{
    float rm;
    GameObject needle;
    public TextMeshProUGUI rockText;

    // Start is called before the first frame update
    void Start()
    {
        needle = transform.Find("Needle").gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        rm = PlayerPrefs.GetInt("RockMeter");
        //Debug.Log(PlayerPrefs.GetInt("RockMeter"));

        needle.transform.localPosition = new Vector3 ((rm - 50) / 23.5f, 0, 0);

        //Debug.Log(rm);
    }

    public void ChangeColor()
    {
        if (rm <= 33)
        {
            rockText.color = Color.red;
        }
        else if (rm <= 66)
        {
            rockText.color = Color.yellow;
        }
        else if (rm <= 100)
        {
            rockText.color = Color.green;
        }
    }
}
