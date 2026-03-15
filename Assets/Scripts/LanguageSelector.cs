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
        if (toggleEnglish == null || toggleJapanese == null)
        {
            Debug.LogError("LanguageSelector: One or both toggles are not assigned!", this);
            return;
        }

        toggleEnglish.isOn = false;
        toggleJapanese.isOn = false;

        toggleEnglish.onValueChanged.AddListener(OnEnglishToggled);
        toggleJapanese.onValueChanged.AddListener(OnJapaneseToggled);
    }

    private IEnumerator Start()
    {
        // Wait for localization to initialize (important on scene reloads)
        if (!LocalizationSettings.InitializationOperation.IsDone)
            yield return LocalizationSettings.InitializationOperation;

        // Only auto-set the very first launch
        if (PlayerPrefs.GetInt("HasAutoSetLanguage", 0) == 0)
        {
            AutoSetLanguage();
            PlayerPrefs.SetInt("HasAutoSetLanguage", 1);
            PlayerPrefs.Save();
        }

        // Load saved and apply it
        int savedLocaleID = PlayerPrefs.GetInt("LocaleKey", 0);
        Debug.Log($"[LanguageSelector] Loaded saved LocaleKey = {savedLocaleID} | Current Selected = {(LocalizationSettings.SelectedLocale != null ? LocalizationSettings.SelectedLocale.Identifier.Code : "null")}");

        yield return LoadLanguageAndUpdateUI(savedLocaleID);

        Debug.Log($"[LanguageSelector] After apply → SelectedLocale = {(LocalizationSettings.SelectedLocale != null ? LocalizationSettings.SelectedLocale.Identifier.Code : "null")}");
    }

    private void OnDestroy()
    {
        if (toggleEnglish != null)
            toggleEnglish.onValueChanged.RemoveListener(OnEnglishToggled);
        if (toggleJapanese != null)
            toggleJapanese.onValueChanged.RemoveListener(OnJapaneseToggled);
    }

    // ────────────────────────────────────────────────
    //  Toggle Handlers
    // ────────────────────────────────────────────────
    private void OnEnglishToggled(bool isOn)
    {
        if (isChangingLanguage) return;
        if (!isOn) return; // ignore turning off

        toggleJapanese.SetIsOnWithoutNotify(false);
        ChangeLanguage(0);
    }

    private void OnJapaneseToggled(bool isOn)
    {
        if (isChangingLanguage) return;
        if (!isOn) return;

        toggleEnglish.SetIsOnWithoutNotify(false);
        ChangeLanguage(1);
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

        if (!LocalizationSettings.InitializationOperation.IsDone)
            yield return LocalizationSettings.InitializationOperation;

        if (localeID >= 0 && localeID < LocalizationSettings.AvailableLocales.Locales.Count)
        {
            var targetLocale = LocalizationSettings.AvailableLocales.Locales[localeID];
            LocalizationSettings.SelectedLocale = targetLocale;
            PlayerPrefs.SetInt("LocaleKey", localeID);
            PlayerPrefs.Save();

            Debug.Log($"[LanguageSelector] Changed to {targetLocale.Identifier.Code} ({targetLocale.name})");
        }
        else
        {
            Debug.LogWarning($"[LanguageSelector] Invalid locale ID: {localeID} — falling back to English");
            LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[0];
            PlayerPrefs.SetInt("LocaleKey", 0);
            PlayerPrefs.Save();
        }

        isChangingLanguage = false;
    }

    // Apply saved language + update UI toggles
    private IEnumerator LoadLanguageAndUpdateUI(int localeID)
    {
        isChangingLanguage = true;

        // Validate & fallback
        if (localeID < 0 || localeID >= LocalizationSettings.AvailableLocales.Locales.Count)
        {
            localeID = 0;
            PlayerPrefs.SetInt("LocaleKey", 0);
            PlayerPrefs.Save();
        }

        toggleEnglish.SetIsOnWithoutNotify(localeID == 0);
        toggleJapanese.SetIsOnWithoutNotify(localeID == 1);

        // Actually apply the locale (this was missing before!)
        ChangeLanguage(localeID);

        // Give one frame for everything to settle (helps in some cases)
        yield return null;

        isChangingLanguage = false;
    }

    private void AutoSetLanguage()
    {
        int autoID = 0; // English default

        if (Application.systemLanguage == SystemLanguage.Japanese)
            autoID = 1;

        PlayerPrefs.SetInt("LocaleKey", autoID);
        PlayerPrefs.Save();
    }
}