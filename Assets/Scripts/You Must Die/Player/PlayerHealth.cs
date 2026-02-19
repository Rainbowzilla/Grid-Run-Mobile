using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    private float health;
    //private float lerpTimer;

    [Header("Health Bar")]
    public float maxHealth = 100f;
    //public float chipSpeed = 2f;

    [Header("Damage Overlay")]
    public Image overlay; //The DamageOverlay Gameobject
    public float duration; //How long the image stays fully opaque
    public float fadeSpeed; //How quickly the image will fade

    private float durationTimer; //Timer to check against the duration

    // Start is called before the first frame update
    void Start()
    {
        health = maxHealth;
        overlay.color = new Color(overlay.color.r, overlay.color.g, overlay.color.b, 0);
    }

    // Update is called once per frame
    void Update()
    {
        //Makes sure health never goes below 0 and never above max health.
        health = Mathf.Clamp(health, 0, maxHealth);

        UpdateHealthUI();
        if(overlay.color.a > 0)
        {
            if (health < 30)
                return;
            durationTimer += Time.deltaTime;
            if(durationTimer > duration)
            {
                //Fade the image
                float tempAlpha = overlay.color.a;
                tempAlpha -= Time.deltaTime * fadeSpeed;
                overlay.color = new Color(overlay.color.r, overlay.color.g, overlay.color.b, tempAlpha);
            }
        }
    }

    public void UpdateHealthUI()
    {
        //Debug.Log(health);
        /*
        float fillF = frontHealthBar.fillAmount;
        float fillB = backHealthBar.fillamount;
        float hFraction = health / maxHealth; //Converts into a decimal for UI to read
        if (fillB > hFraction)
        {
            frontHealthBar.fillAmount = hFraction;
            backHealthBar.color = Color.red;
            lerpTimer += Time.deltaTime;
            float percentComplete = lerpTimer / chipSpeed;
            percentComplete = percentComplete * percentComplete;
            backHealthBar.fillAmount = Mathf.Lerp(fillB, hFration, percentComplete);
        }
        if (fillF < hFraction)
        {
            backHealthBar.color = Color.green;
            backHealthBar.fillAmount = hFraction;
            lerpTimer += Time.deltaTime;
            float percentComplete = lerpTimer / chipSpeed;
            percentComplete = percentComplete * percentComplete;
            frontHealthBar.fillAmount = Mathf.Lerp(fillF, hFraction, percentComplete);
        }
        */
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        //lerpTimer = 0f;
        durationTimer = 0f;
        overlay.color = new Color(overlay.color.r, overlay.color.g, overlay.color.b, 1);
    }

    public void RestoreHealth(float healAmount)
    {
        health += healAmount;
        //lerpTimer = 0f;
    }
}
