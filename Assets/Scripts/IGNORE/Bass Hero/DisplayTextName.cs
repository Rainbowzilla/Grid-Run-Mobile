using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DisplayTextName : MonoBehaviour
{
    public TextMeshProUGUI displaySongName;

    public void Awake()
    {
        displaySongName.text = GetSceneName.GSN.sceneName;
    }
}
