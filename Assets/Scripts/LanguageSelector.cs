using System.Collections;
using UnityEngine;
using UnityEngine.Localization.Settings;
using UnityEngine.UI;

public class LanguageSelector : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private Toggle toggleEnglish;
    [SerializeField] private Toggle toggleJapanese;

    private bool isChangingLanguage = false;

    private void Awake()
    {
        // Safety check
        if (toggleEnglish == null || toggleJapanese == null)
        {
            Debug.LogError("LanguageSelector: One or both toggles are not assigned!", this);
            return;
        }

        // Make sure they are mutually exclusive from the start
        toggleEnglish.isOn = false;
        toggleJapanese.isOn = false;

        // Add listeners
        toggleEnglish.onValueChanged.AddListener(OnEnglishToggled);
        toggleJapanese.onValueChanged.AddListener(OnJapaneseToggled);
    }

    private void Start()
    {
        // Only auto-set language the very first time the game is launched
        if (PlayerPrefs.GetInt("HasAutoSetLanguage", 0) == 0)
        {
            AutoSetLanguage();
            PlayerPrefs.SetInt("HasAutoSetLanguage", 1);
        }

        // Load saved language and update UI
        int savedLocaleID = PlayerPrefs.GetInt("LocaleKey", 0);
        LoadLanguageAndUpdateUI(savedLocaleID);
    }

    private void OnDestroy()
    {
        // Clean up listeners
        if (toggleEnglish != null)
            toggleEnglish.onValueChanged.RemoveListener(OnEnglishToggled);
        if (toggleJapanese != null)
            toggleJapanese.onValueChanged.RemoveListener(OnJapaneseToggled);
    }

    // ────────────────────────────────────────────────
    //  Toggle Handlers (mutually exclusive behavior)
    // ────────────────────────────────────────────────
    private void OnEnglishToggled(bool isOn)
    {
        if (isChangingLanguage) return;

        if (isOn)
        {
            // Turn Japanese off without triggering its event
            toggleJapanese.SetIsOnWithoutNotify(false);
            ChangeLanguage(0); // English = 0
        }
        // If turning English off → do nothing (Japanese should already be on or both off is invalid)
    }

    private void OnJapaneseToggled(bool isOn)
    {
        if (isChangingLanguage) return;

        if (isOn)
        {
            // Turn English off without triggering its event
            toggleEnglish.SetIsOnWithoutNotify(false);
            ChangeLanguage(1); // Japanese = 1
        }
    }

    // ────────────────────────────────────────────────
    //  Core language switching
    // ────────────────────────────────────────────────
    private void ChangeLanguage(int localeID)
    {
        if (isChangingLanguage) return;
        StartCoroutine(SetLanguageRoutine(localeID));
    }

    private IEnumerator SetLanguageRoutine(int localeID)
    {
        isChangingLanguage = true;

        // Wait for localization system to be ready
        if (!LocalizationSettings.InitializationOperation.IsDone)
            yield return LocalizationSettings.InitializationOperation;

        // Change locale
        if (localeID >= 0 && localeID < LocalizationSettings.AvailableLocales.Locales.Count)
        {
            LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[localeID];
            PlayerPrefs.SetInt("LocaleKey", localeID);
        }
        else
        {
            Debug.LogWarning($"Invalid locale ID: {localeID}");
        }

        isChangingLanguage = false;
    }

    // Load saved language and reflect it in the UI toggles
    private void LoadLanguageAndUpdateUI(int localeID)
    {
        // Prevent toggle callbacks during initialization
        isChangingLanguage = true;

        toggleEnglish.SetIsOnWithoutNotify(localeID == 0);
        toggleJapanese.SetIsOnWithoutNotify(localeID == 1);

        // If neither is selected (invalid state), default to English
        if (localeID != 0 && localeID != 1)
        {
            toggleEnglish.SetIsOnWithoutNotify(true);
            toggleJapanese.SetIsOnWithoutNotify(false);
            ChangeLanguage(0);
        }

        isChangingLanguage = false;
    }

    // Optional: First-time auto detection
    private void AutoSetLanguage()
    {
        int autoID = 0; // default English

        switch (Application.systemLanguage)
        {
            case SystemLanguage.Japanese:
                autoID = 1;
                break;
            // You can keep other languages here if you want to support them later
            // case SystemLanguage.French:    autoID = 2; break;
            // case SystemLanguage.Spanish:   autoID = 3; break;
        }

        PlayerPrefs.SetInt("LocaleKey", autoID);
    }
}