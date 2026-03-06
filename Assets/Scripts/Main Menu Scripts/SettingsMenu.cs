using UnityEngine;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    [SerializeField] private GameObject _settingsMenuGO;
    [Header("Options")]
    [SerializeField] private Toggle _crtFilterToggle;
    [Header("Misc assignments")]
    [SerializeField] private CameraFilterPack_TV_ARCADE _crtFilter;

    private void Start()
    {
        _settingsMenuGO.SetActive(true);
        Debug.Log(PlayerPrefs.GetInt("CRTFilter"));
        if (PlayerPrefs.GetInt("CRTFilter") == 1)
        { 
            _crtFilterToggle.isOn = true; 
        }
        else
        {
            Debug.Log("startup crt filter check is false");
            _crtFilterToggle.isOn = false; 
            _crtFilter.enabled = false;
        }

        _settingsMenuGO.SetActive(false);
    }

    public void onCRTFilterToggle(bool value)
    {
        if (value) 
        { 
            PlayerPrefs.SetInt("CRTFilter", 1);
            _crtFilter.enabled = true;
        } 
        else 
        { 
            PlayerPrefs.SetInt("CRTFilter", 0);
            _crtFilter.enabled = false;
        }
    }
}
