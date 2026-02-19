using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScoreUI : MonoBehaviour
{
    public new string name;

    void Update()
    {
        GetComponent<TextMeshProUGUI>().text = PlayerPrefs.GetInt(name) + "";
    }
}
