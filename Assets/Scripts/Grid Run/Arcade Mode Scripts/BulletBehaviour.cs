using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBehaviour : MonoBehaviour
{
    public void AddScore(int score)
    {
        GridRunArcadeModeGameManager.static_score += score;
        Debug.Log("Player gained " + score + " points!");
    }
}
