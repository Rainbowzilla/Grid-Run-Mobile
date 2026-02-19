using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GetSceneName : MonoBehaviour
{
    public static GetSceneName GSN;

    public string sceneName;
    
    void Awake()
    {
        if (GSN == null)
        {
            GSN = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        sceneName = SceneManager.GetActiveScene().name;
    }
}
