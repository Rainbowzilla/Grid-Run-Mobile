using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
    public int level;
    public float[] position;

    public PlayerData (PlayerController player)
    {
        position = new float[3];
        position[0] = player.transform.position.x;
        position[0] = player.transform.position.y;
        position[0] = player.transform.position.z;
    }
}
