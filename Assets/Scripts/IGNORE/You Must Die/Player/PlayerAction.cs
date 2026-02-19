using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAction : MonoBehaviour
{
    [SerializeField] private PlayerGunSelector GunSelector;

    
    void Update()
    {
        if (Mouse.current.leftButton.isPressed && GunSelector.ActiveGun != null)
        {
            GunSelector.ActiveGun.Shoot(); 
        }
    }
}
