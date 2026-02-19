using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TemporaryLoadButton : MonoBehaviour
{
    public void LoadBackToSong(int i)
    {
        SceneManager.LoadScene(i);
    }
}
