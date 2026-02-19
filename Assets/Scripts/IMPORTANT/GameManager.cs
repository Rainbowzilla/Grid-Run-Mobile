using PrimeTween;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PlayerSkadaddle;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public SkadaddlePlayerData saveData;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);

            // Load save at start
            SaveSystem.Load(out saveData, "Save");
        }
        else
        {
            Destroy(gameObject);
        }
        PrimeTweenConfig.warnEndValueEqualsCurrent = false;

        //saveData.Lives = 3;
    }

    public void SaveGame()
    {
        SaveSystem.Save(saveData, "Save");
    }
    public void ResetAll()
    {
        if (saveData != null)
        {
            saveData.Coins = 0;
            saveData.Lives = 3; // default starting lives
            SaveGame();
        }
    }

    public void AddCoins()
    {
        saveData.Coins++;
        SaveGame();
        DebugCheckSave();
    }

    public void AddLives()
    {
        saveData.Lives++;
        SaveGame();
    }
    public void SkadaddlePlayerDied()
    {
        saveData.Lives--;

        if (saveData.Lives <= 0)
        {
            saveData.Coins = 0;
            saveData.Lives = 3;
        }

        SaveSystem.Save(saveData, "Save");
    }


    public void DebugCheckSave()
    {
        SkadaddlePlayerData loaded;
        SaveSystem.Load(out loaded, "Save");
        Debug.Log($"File says Coins={loaded.Coins}, Lives={loaded.Lives}");
    }
}
