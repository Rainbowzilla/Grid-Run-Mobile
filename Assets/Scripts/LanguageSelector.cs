using Newtonsoft.Json.Bson;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization.Settings;

public class LanguageSelector : MonoBehaviour
{
    bool isActive = false;

    private void Start()
    {
        if (PlayerPrefs.GetInt("HasAutoSetLangauge") != 1)
        {
            AutoSetLanguage();
            PlayerPrefs.SetInt("HasAutoSetLangauge", 1);
        }

        int savedLocaleID = PlayerPrefs.GetInt("LocaleKey", 0);
        ChangeLangauge(savedLocaleID);
    }
    public void ChangeLangauge(int localeID)
    {
        if (isActive) return;

        StartCoroutine(SetLanguage(localeID));
    }
    IEnumerator SetLanguage(int localeID)
    {
        isActive = true;
        yield return LocalizationSettings.InitializationOperation;
        LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[localeID];
        PlayerPrefs.SetInt("LocaleKey", localeID);
        isActive = false;
    }

    void AutoSetLanguage()
    {
        switch (Application.systemLanguage)
        {
            case SystemLanguage.English:
                PlayerPrefs.SetInt("LocaleKey", 0);
                break;
            case SystemLanguage.Japanese:
                PlayerPrefs.SetInt("LocaleKey", 1);
                break;
            case SystemLanguage.French:
                PlayerPrefs.SetInt("LocaleKey", 2);
                break;
            case SystemLanguage.Spanish:
                PlayerPrefs.SetInt("LocaleKey", 3);
                break;
        }
    }
}
