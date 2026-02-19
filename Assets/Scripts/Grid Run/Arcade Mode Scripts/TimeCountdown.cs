using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TimeCountdown : MonoBehaviour
{
    public float startTime;
    [HideInInspector] public static float countTimer;
    float countTimerInt;
    [SerializeField] TextMeshProUGUI countDownText;
    public TextMeshProUGUI thresholdText;
    public AudioSource alarm;
    public GameObject TheRecognizer;
    public GameObject gameOverCanvas;
    public AudioManagerForGridRun AMFGR;
    private bool isInBossPhase;
    public float fadeTime = 5;
    public float fadeBossTime = 5;
    private float fadeAwayPerSecond;
    private float alphaValue;

    void Start()
    {
        countTimerInt = 0;
        countTimer = startTime;
        BossAI.isBossDead = false;
        isInBossPhase = false;
        thresholdText.enabled = false;
        fadeAwayPerSecond = 1 / fadeTime;
        alphaValue = thresholdText.color.a;
    }

    void Update()
    {   
        if (!RunnerController.isPlayerDead)
        {
            countTimer -= 1 * Time.deltaTime;
            countTimerInt = Mathf.RoundToInt(countTimer);
            countDownText.text = countTimerInt.ToString();
        
            if (countTimerInt <= 0 && GridRunArcadeModeGameManager.static_score < GridRunArcadeModeGameManager.Minimum_Score_So_Unlock_Boss &&!isInBossPhase)
            {
                Cursor.visible = true;
                RunnerController.isPlayerDead = true;
                countTimer = 0;
                gameObject.GetComponent<PowerUps>().enabled = false;
                AudioListener.pause = true;
                //Time.timeScale = 0f;
                //Time.fixedDeltaTime = 0f;
                gameOverCanvas.SetActive(true);
            }
            if (countTimerInt <= 0 && GridRunArcadeModeGameManager.static_score >= GridRunArcadeModeGameManager.Minimum_Score_So_Unlock_Boss && !isInBossPhase)
            {
                countTimer = 35;
                Instantiate(TheRecognizer);
                alarm.Play();
                if (StaticVariableController.statusBool6)
                    AMFGR.StartBossPhase();
                isInBossPhase = true;
            }

            if (countTimer <= 0 && !BossAI.isBossDead && isInBossPhase)
            {
                Cursor.visible = true;
                RunnerController.isPlayerDead = true;
                countTimer = 0;
                gameObject.GetComponent<PowerUps>().enabled = false;
                AudioListener.pause = true;
                gameOverCanvas.SetActive(true);
            }

            if (countTimerInt <= 0 && BossAI.isBossDead && isInBossPhase)
            {
                Cursor.visible = true;
                countTimer = 0;
                gameObject.GetComponent<PowerUps>().enabled = false;
                AudioListener.pause = true;
                //Time.timeScale = 0f;
                //Time.fixedDeltaTime = 0f;
                gameOverCanvas.SetActive(true);
                RunnerController.isPlayerDead = true;
            }

            if (GridRunArcadeModeGameManager.static_score >= GridRunArcadeModeGameManager.Minimum_Score_So_Unlock_Boss)
            {
                FadeText();
            }
        }
    }

    void FadeText()
    {
        thresholdText.enabled = true;

        if (fadeTime > 0)
        {
            alphaValue -= fadeAwayPerSecond * Time.deltaTime;
            thresholdText.color = new Color(thresholdText.color.r, thresholdText.color.g, thresholdText.color.b, alphaValue);
            fadeTime -= Time.deltaTime;
        }
    }
}
