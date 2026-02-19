using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.Windows;
using UnityEngine.UI;
using Input = UnityEngine.Input;
using Unity.VisualScripting;

public class GridRunMenu : MonoBehaviour
{
    public GameObject mainMenu, endlessMenu, arcadeMenu;
    public GameObject floor, leftWall, rightWall;
    public GameObject synthwaveSunOrMoon, title;
    public Material purpleSynthwave, purpleSkyBox, blueSynthwave, blueSkyBox;
    public Sprite sun, moon, gridRunTitle, gridRunTitleBlue;
    public TextMeshProUGUI easyScore, mediumScore, hardScore, insaneScore, arcadeScore;
    public static int menuID;
    public Image cameraIconDefault;
    public TextMeshProUGUI smoothText, instantText;
    public AudioSource pew;
    public CameraFilterPack_TV_ARCADE cameraFilterPack_TV_ARCADE;
    public AudioSource Something_You_Know_But_Dont;


    void Start()
    {
        Time.timeScale = 1f;
        Time.fixedDeltaTime = 0.02f;
        endlessMenu.SetActive(false);
        arcadeMenu.SetActive(false);

        easyScore.text = $"{PlayerPrefs.GetInt("HighScoreGridRunEasy", 0)}";
        mediumScore.text = $"{PlayerPrefs.GetInt("HighScoreGridRunMedium", 0)}";
        hardScore.text = $"{PlayerPrefs.GetInt("HighScoreGridRun", 0)}";
        insaneScore.text = $"{PlayerPrefs.GetInt("HighScoreGridRunInsane", 0)}";
        arcadeScore.text = $"{PlayerPrefs.GetInt("HighScoreGridRunArcade", 0)}";

        menuID = 0;
        ToggleIcon(true);
        ToggleShakeCameraStaticVariable(false);
        ToggleScoreStaticVariable(true);
        ToggleSmoothOrInstantCameraVariable(true);
        ToggleCameraFilter(true);
        ToggleMusic(true);
        smoothText.enabled = true;
        floor.GetComponent<Renderer>().sharedMaterial = purpleSynthwave;
        leftWall.GetComponent<Renderer>().sharedMaterial = purpleSynthwave;
        rightWall.GetComponent<Renderer>().sharedMaterial = purpleSynthwave;
        synthwaveSunOrMoon.GetComponent<SpriteRenderer>().sprite = sun;
        RunnerController.isPlayerDead = false;
    }

    void Update()
    {
        if (menuID == 0)
        {
            ChangeEnviornment(0);

            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                pew.Play();
                mainMenu.SetActive(false);
                endlessMenu.SetActive(true);
                arcadeMenu.SetActive(false);
                menuID = 1;
                GoToAnotherMenu(1);
            }
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                pew.Play();
                menuID = 2;
                GoToAnotherMenu(2);
                mainMenu.SetActive(false);
                endlessMenu.SetActive(false);
                arcadeMenu.SetActive(true);
            }
            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                pew.Play();
                //Application.Quit();
                SceneManager.LoadScene("Arcade HUB");
            }
        }
        else if (menuID == 1)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                MenuSwitcherScript(0);
                mainMenu.SetActive(false);
            }
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                MenuSwitcherScript(1);
                mainMenu.SetActive(false);
            }
            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                MenuSwitcherScript(2);
                mainMenu.SetActive(false);
            }
            if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                MenuSwitcherScript(3);
                mainMenu.SetActive(false);
            }
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                pew.Play();
                mainMenu.SetActive(true);
                endlessMenu.SetActive(false);
                menuID = 0;
                GoToAnotherMenu(0);
            }
        }
        else if (menuID == 2)
        {
            ChangeEnviornment(1);

            if (Input.GetKeyDown(KeyCode.Return))
            {
                SceneManager.LoadScene("Grid Run Arcade Mode");
            }

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                pew.Play();
                mainMenu.SetActive(true);
                endlessMenu.SetActive(false);
                menuID = 0;
                GoToAnotherMenu(0);
            }
        }
    }

    public void GoToAnotherMenu(int x)
    {
        if (x == 0)
        {
            mainMenu.SetActive(true);
            endlessMenu.SetActive(false);
            arcadeMenu.SetActive(false);
            menuID = 0;
        }
        if (x == 1)
        {
            mainMenu.SetActive(false);
            endlessMenu.SetActive(true);
            arcadeMenu.SetActive(false);
            menuID = 1;
        }
        if (x == 2)
        {
            mainMenu.SetActive(false);
            endlessMenu.SetActive(false);
            arcadeMenu.SetActive(true);
            menuID = 2;
        }
    }


    public void ChangeEnviornment(int x)
    {
        if (x == 0)
        {
            floor.GetComponent<Renderer>().sharedMaterial = purpleSynthwave;
            leftWall.GetComponent<Renderer>().sharedMaterial = purpleSynthwave;
            rightWall.GetComponent<Renderer>().sharedMaterial = purpleSynthwave;
            synthwaveSunOrMoon.GetComponent<SpriteRenderer>().sprite = sun;
            synthwaveSunOrMoon.transform.localScale = new Vector3(25, 25, 25);
            title.GetComponent<Image>().sprite = gridRunTitle;
            RenderSettings.skybox = purpleSkyBox;
            RenderSettings.fogColor = new Color(255 / 255f, 142 / 255f, 199 / 255f);
        }
        else if (x == 1)
        {
            floor.GetComponent<Renderer>().sharedMaterial = blueSynthwave;
            leftWall.GetComponent<Renderer>().sharedMaterial = blueSynthwave;
            rightWall.GetComponent<Renderer>().sharedMaterial = blueSynthwave;
            synthwaveSunOrMoon.GetComponent<SpriteRenderer>().sprite = moon;
            synthwaveSunOrMoon.transform.localScale = new Vector3(13.5f, 13.5f, 13.5f);
            title.GetComponent<Image>().sprite = gridRunTitleBlue;
            RenderSettings.skybox = blueSkyBox;
            RenderSettings.fogColor = new Color(132 / 255f, 250 / 255f, 255 / 255f);
        }
    }

    public void MenuSwitcherScript(int x)
    {
        pew.Play();
        mainMenu.SetActive(true);
        endlessMenu.SetActive(false);
        SceneManager.LoadScene("Grid Run");
        StaticVariableController.difficulty = x;
    }
    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void SetDifficultyNumber(int input)
    {
        StaticVariableController.difficulty = input;
    }

    #region Monobehaviour API
    public void ToggleIcon(bool cameraStatus)
    {
        pew.Play();
        if (cameraStatus)
        {
            cameraIconDefault.transform.rotation = Quaternion.Euler(15, 0, 15);
            cameraIconDefault.transform.localPosition = new Vector3(cameraIconDefault.transform.localPosition.x, cameraIconDefault.transform.localPosition.y + 15, cameraIconDefault.transform.localPosition.z);
            StaticVariableController.statusBool4 = true;
        }
        if (!cameraStatus)
        {
            cameraIconDefault.transform.rotation = Quaternion.Euler(15, 0, 0);
            cameraIconDefault.transform.localPosition = new Vector3(cameraIconDefault.transform.localPosition.x, cameraIconDefault.transform.localPosition.y - 15, cameraIconDefault.transform.localPosition.z);
            StaticVariableController.statusBool4 = false;
        }
    }

    public void ToggleScoreStaticVariable(bool showScore)
    {
        pew.Play();
        if (showScore)
        {
            StaticVariableController.statusBool1 = true;
        }
        else StaticVariableController.statusBool1 = false;
    }
    public void ToggleShakeCameraStaticVariable(bool currentCameraShakeState)
    {
        pew.Play();
        if (currentCameraShakeState)
            StaticVariableController.statusBool2 = true;
        else StaticVariableController.statusBool2 = false;
    }

    public void ToggleSmoothOrInstantCameraVariable(bool currentSoSCameraState)
    {
        pew.Play();
        if (currentSoSCameraState)
        {
            StaticVariableController.statusBool3 = true;
            smoothText.enabled = true;
            instantText.enabled = false;
        }
        else if (!currentSoSCameraState)
        {
            StaticVariableController.statusBool3 = false;
            smoothText.enabled = false;
            instantText.enabled = true;
        }
    }
    
    public void ToggleCameraFilter(bool currentCameraFilterState)
    {
        pew.Play();
        if (currentCameraFilterState)
        {
            StaticVariableController.statusBool5 = true;
            cameraFilterPack_TV_ARCADE.enabled = true;
        }
        else
        {
            StaticVariableController.statusBool5 = false;
            cameraFilterPack_TV_ARCADE.enabled = false;
        }
    }

    public void ToggleMusic(bool musicState)
    {
        if (musicState)
        {
            Something_You_Know_But_Dont.Play();
            StaticVariableController.statusBool6 = true;
        }
        else
        {
            Something_You_Know_But_Dont.Pause();
            StaticVariableController.statusBool6 = false;
        }
    }

    public void Quit()
    {
        Application.Quit();
    }
    #endregion
}
