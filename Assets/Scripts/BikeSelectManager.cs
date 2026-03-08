using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class BikeSelectManager : MonoBehaviour
{
    public static BikeSelectManager Instance;

    public List<bool> unlockedBikes = new List<bool>();
    public List<BikeDataClass> bikes = new List<BikeDataClass>();

    public int currentBikeIndex = 0;
    public GameObject currentBike;
    private void Awake()
    {

        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
        }

        if (!PlayerPrefs.HasKey("HasInitialized"))
        {
            Debug.Log("initializing");
            unlockedBikes = InitializeDefaults();
            PlayerPrefs.SetInt("HasInitialized", 1);
            PlayerPrefs.Save();
        }
        else
        {//initializes unlocked bikes from player prefs
            string savedData = PlayerPrefs.GetString("UnlockedBikes", "1");
            unlockedBikes = new List<bool>();
            for (int i = 0; i < savedData.Length; i++)
            {
                if (savedData[i] == '1')
                    unlockedBikes.Add(true);
                else
                    unlockedBikes.Add(false);
            }
        }
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        bool bikeAssigned = false;
        int i = 0;

        do
        {
            if (bikes[i].bikeIndex == currentBikeIndex)
            {
                SelectCharacter(bikes[i]);
                Debug.Log("Current bike set to " + bikes[i].bikeName);
            }

            i++;
        }while (i < bikes.Count && !bikeAssigned);
    }

    void Update()
    {
        
    }

    string BoolsToString(List<bool> list)
    {
        string saveData = "1"; // sets first character always unlocked

        if (list != null)
        {
            for (int i = 1; i < list.Count; i++) // starts at 1 to skip first character, then adds data if unlocked
            {
                if (list[i])
                    saveData += "1";
                else
                    saveData += "0";
            }
        }

        return saveData;
    }

    bool CheckForUnlocked(BikeDataClass queryBike)
    {
        if (unlockedBikes[queryBike.bikeIndex])
        {
            return true;
        }
        Debug.Log("Bike " + queryBike.bikeName + " is NOT unlocked");
        return false;
    }

    public void SelectCharacter(BikeDataClass newBike) //takes input from button in the editor, needs to be assigned
    {
        if(CheckForUnlocked(newBike))
        {
            Transform bikeTransform = currentBike.transform;
            Destroy(currentBike);
            currentBike = Instantiate(newBike.bikePrefab, bikeTransform.position, Quaternion.identity);
            currentBikeIndex = newBike.bikeIndex;
        }
        else
        {
            Debug.Log("Bike " + newBike.bikeName + " is locked.");
        }
    }

    public void UnlockBike(BikeDataClass bikeToUnlock)
    {
        if (bikeToUnlock.bikeIndex < unlockedBikes.Count)
        {
            unlockedBikes[bikeToUnlock.bikeIndex] = true;
            PlayerPrefs.SetString("UnlockedBikes", BoolsToString(unlockedBikes));
            PlayerPrefs.Save();
            Debug.Log("Bike " + bikeToUnlock.bikeName + " has been unlocked!");
        }
        else
        {
            Debug.LogError("Bike index out of range: " + bikeToUnlock.bikeIndex);
        }
    }

    List<bool> InitializeDefaults() // start with first char unlocked, only called when there's no save data
    {
        List<bool> defaultUnlocks = new()
        {
            true //first character is always unlocked
        };

        for (int i = 1; i < bikes.Count; i++)
        {
            defaultUnlocks.Add(false);
        }

        PlayerPrefs.SetString("UnlockedBikes", BoolsToString(defaultUnlocks));
        PlayerPrefs.Save();

        return defaultUnlocks;
    }

    public void ReturnToMainMenu()
    {
        //gridBike.SetActive(false);
        SceneManager.LoadScene(0);
        gameObject.SetActive(false);
    }
}
