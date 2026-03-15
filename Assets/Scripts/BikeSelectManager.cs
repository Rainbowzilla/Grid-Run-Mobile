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

    public Transform spawnParent;
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

        //FOR TESTING
        UnlockAllBikes();

        spawnParent = GameObject.FindGameObjectWithTag("SpawnParent").transform;

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

        for(int i = 0; i <= currentBikeIndex; i++)
        {
            if (bikes[i].bikeIndex == currentBikeIndex)
            {
                if(currentBike != null)
                {
                    Destroy(currentBike);
                    //Instantiate(bikes[i].bikePrefab, bikes[i].spawnPositionOffset, Quaternion.Euler(bikes[i].spawnRotationOffset, spawnParent));
                    
                    spawnParent = GameObject.FindGameObjectWithTag("SpawnParent").transform;

                    GameObject bikeInstance = Instantiate(
                        bikes[i].bikePrefab,
                        bikes[i].spawnPositionOffset,
                        Quaternion.Euler(bikes[i].spawnRotationOffset),
                        spawnParent
                    );

                    
                    Debug.Log("Current bike set to " + bikes[i].bikeName);
                }
                break;
            }
        }
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
            if(currentBike != null)
            {
                Destroy(currentBike);
            }

            spawnParent = GameObject.FindGameObjectWithTag("SpawnParent").transform;

            currentBike = Instantiate(newBike.bikePrefab, newBike.spawnPositionOffset, Quaternion.Euler(newBike.spawnRotationOffset), spawnParent);
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

    //FOR TESTING
    void UnlockAllBikes()
    {
        if (unlockedBikes == null || unlockedBikes.Count == 0)
        {
            Debug.LogWarning("unlockedBikes list is empty or null — nothing to unlock.");
            return;
        }

        // Set every bike to unlocked
        for (int i = 0; i < unlockedBikes.Count; i++)
        {
            unlockedBikes[i] = true;
        }

        // Save once at the end (more efficient)
        PlayerPrefs.SetString("UnlockedBikes", BoolsToString(unlockedBikes));
        PlayerPrefs.Save();

        Debug.Log($"All {unlockedBikes.Count} bikes have been unlocked!");
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
