using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

public class CharacterSelectManager : MonoBehaviour
{
    public static CharacterSelectManager Instance;

    public List<bool> unlockedCharacters = new List<bool>();
    public List<GameObject> characters = new List<GameObject>();

    private void Awake()
    {
        if (Instance == null)
            Instance = this;

        if (PlayerPrefs.GetString("unlockedCharacters") == null)
        {
            
        }
    }

    void Start()
    {
        for (int i = 0; i < characters.Count; i++)
        {
            if (characters[i] != null)
                unlockedCharacters.Add(characters[i].activeSelf);
            else
                Debug.LogError("list element " + i + "is null");
        }
    }

    void Update()
    {
        
    }

    string BoolsToString(List<GameObject> list)
    {
        string saveData = "1"; // sets first character always unlocked

        if (list != null)
        {
            for (int i = 1; i <list.Count; i++) // starts at 1 to skip first character, then adds
            {
                if (list[i] != null && list[i])
                    saveData += "1";
                else if (list[i] != null)
                    Debug.LogError("list element " + i + "is null");
                else
                    saveData += "0";
            }
        }

        return saveData;
    }

    void CheckForUnlocked()
    {

    }

    void SelectCharacter()
    {

    }
}
