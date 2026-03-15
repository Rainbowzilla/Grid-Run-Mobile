using UnityEngine;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    [SerializeField] private GameObject _settingsMenuGO;
    [Header("Options")]
    [SerializeField] private Toggle _crtFilterToggle;
    [SerializeField] private Toggle _showScoreToggle;
    [SerializeField] private Toggle _cameraPerspectiveToggle;
    [SerializeField] private Toggle _cameraShakeToggle;
    [SerializeField] private Toggle _musicToggle;
    [SerializeField] private Toggle _tutorialToggle;
    [SerializeField] private Toggle _tiltToggle;
    [Header("Misc assignments")]
    [SerializeField] private CameraFilterPack_TV_ARCADE _crtFilter;
    [SerializeField] private AudioSource Something_You_Know_But_Dont;

    private void Start()
    {
        _settingsMenuGO.SetActive(true);
        Debug.Log(PlayerPrefs.GetInt("CRTFilter"));

        //Default settings for the very first playthrough of the game
        if (!PlayerPrefs.HasKey("CRTFilter"))
        {
            PlayerPrefs.SetInt("CRTFilter", 1);
        }

        if (!PlayerPrefs.HasKey("ShowScore"))
            PlayerPrefs.SetInt("ShowScore", 1);

        if (!PlayerPrefs.HasKey("CameraPerspective"))
            PlayerPrefs.SetInt("CameraPerspective", 1);

        if (!PlayerPrefs.HasKey("CameraShake"))
            PlayerPrefs.SetInt("CameraShake", 0);

        if (!PlayerPrefs.HasKey("CameraToggle"))
            PlayerPrefs.SetInt("CameraToggle", 1);

        if (!PlayerPrefs.HasKey("MusicToggle"))
            PlayerPrefs.SetInt("MusicToggle", 1);

        if (!PlayerPrefs.HasKey("PhoneTutorial"))
            PlayerPrefs.SetInt("PhoneTutorial", 1);

        if (!PlayerPrefs.HasKey("TiltMode"))
            PlayerPrefs.SetInt("TiltMode", 1);
        SoManyIfStatements();

        _settingsMenuGO.SetActive(false);
        //Debug.Log("Is the camera 3rd person perspective on: " + PlayerPrefs.GetInt("CameraPerspective"));
    }

    public void onCRTFilterToggle(bool value)
    {
        if (value) 
        { 
            PlayerPrefs.SetInt("CRTFilter", 1);
            _crtFilter.enabled = true;
            _crtFilterToggle.isOn = true;
        } 
        else 
        { 
            PlayerPrefs.SetInt("CRTFilter", 0);
            _crtFilter.enabled = false;
            _crtFilterToggle.isOn = false;
        }
    }

    public void onShowScoreToggle(bool value)
    {
        PlayerPrefs.SetInt("ShowScore", value ? 1 : 0);
    }

    public void onCameraPerspectiveToggle(bool value)
    {
        PlayerPrefs.SetInt("CameraPerspective", value ? 1 : 0);
    }

    public void onCameraShakeToggle(bool value)
    {
        PlayerPrefs.SetInt("CameraShake", value ? 1 : 0);
    }

    public void onMusicToggle(bool value)
    {
        PlayerPrefs.SetInt("MusicToggle", value ? 1 : 0);

        if (value)
            Something_You_Know_But_Dont.Play();
        else
            Something_You_Know_But_Dont.Pause();
    }

    public void onTutorialToggle(bool value)
    {
        PlayerPrefs.SetInt("PhoneTutorial", value ? 1 : 0);
    }

    public void onTiltModeToggle(bool value)
    {
        PlayerPrefs.SetInt("TiltMode", value ? 1 : 0);
    }

    void SoManyIfStatements()
    {
        if (PlayerPrefs.GetInt("CRTFilter") == 1)
        {
            _crtFilterToggle.isOn = true;
            _crtFilter.enabled = true;
        }
        else
        {
            Debug.Log("startup crt filter check is false");
            _crtFilterToggle.isOn = false;
            _crtFilter.enabled = false;
        }

        if (PlayerPrefs.GetInt("ShowScore") == 1)
        {
            _showScoreToggle.isOn = true;
            _showScoreToggle.enabled = true;
        }
        else
        {
            Debug.Log("startup show score check is false");
            _showScoreToggle.isOn = false;
            //_showScoreToggle.enabled = false;
        }

        if (PlayerPrefs.GetInt("CameraPerspective") == 1)
        {
            _cameraPerspectiveToggle.isOn = true;
            _cameraPerspectiveToggle.enabled = true;
        }
        else
        {
            Debug.Log("startup camera perspective check is false");
            _cameraPerspectiveToggle.isOn = false;
            //_cameraPerspectiveToggle.enabled = false;
        }

        if (PlayerPrefs.GetInt("CameraShake") == 1)
        {
            _cameraShakeToggle.isOn = true;
            _cameraShakeToggle.enabled = true;
        }
        else
        {
            Debug.Log("startup camera shake check is false");
            _cameraShakeToggle.isOn = false;
            //_cameraShakeToggle.enabled = false;
        }

        if (PlayerPrefs.GetInt("MusicToggle") == 1)
        {
            _musicToggle.isOn = true;
            _musicToggle.enabled = true;
        }
        else
        {
            Debug.Log("startup music check is false");
            _musicToggle.isOn = false;
            //_cameraShakeToggle.enabled = false;
        }

        if (PlayerPrefs.GetInt("TiltMode") == 1)
        {
            _tiltToggle.isOn = true;
            _tiltToggle.enabled = true;
        }
        else
        {
            Debug.Log(" tilt check is false");
            _tiltToggle.isOn = false;
        }

        if (PlayerPrefs.GetInt("PhoneTutorial") == 1)
        {
            _tutorialToggle.isOn = true;
            _tutorialToggle.enabled = true;
        }
        else
            _tutorialToggle.isOn = false;
    }
}
