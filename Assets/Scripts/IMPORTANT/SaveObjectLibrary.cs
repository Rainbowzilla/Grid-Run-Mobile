using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveObjectLibrary : MonoBehaviour
{
    public static Dictionary<int, GameObject> SaveableObjects;

    [SerializeField] private GameObject[] RegisteredObjects;

    private void Awake()
    {
        SaveableObjects = new Dictionary<int, GameObject>();

        for (int i = 0; i < RegisteredObjects.Length; i++)
        {
            int IDToRegistar = RegisteredObjects[i].GetComponent<SaveableObjectID>().ID;
            SaveableObjects.Add(IDToRegistar, RegisteredObjects[i]);
        }
    }
}
