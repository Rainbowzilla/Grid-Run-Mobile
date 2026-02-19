using System.Collections;
using System.Collections.Generic;
using PrimeTween;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;

public class FadeTransition : MonoBehaviour
{
    public float fadeDuration = 3f;
    private Image _fadeImage;

    void Awake()
    {
        _fadeImage = GetComponent<Image>();
        PrimeTweenConfig.SetTweensCapacity(800);
    }

    public void FadeToBlack()
    {
        Tween.Alpha(_fadeImage, 1, fadeDuration);
        //Set fadeValue to 1 for Fading In
        //Set fadeValue to 0 for Fading out
    }

    public void FadeIn()
    {
        Tween.Alpha(_fadeImage, 0, fadeDuration);
    }
}
