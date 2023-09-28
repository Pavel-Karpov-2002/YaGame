using System.Runtime.InteropServices;
using TMPro;
using UnityEngine;

public class ChangeLanguageController : Singleton<ChangeLanguageController>
{
    private const int RuIDLanguage = 0;
    private const int EnIDLanguage = 1;
    private const int TrIDLanguage = 2;

    [DllImport("__Internal")]
    private static extern string GetLang();

    public void SetLanguage()
    {
#if UNITY_WEBGL && !UNITY_EDITOR
        string lang = GetLang();

        if (lang == "ru")
        {
            LangsList.SetLanguage(RuIDLanguage, true);
            return;
        }

        if (lang == "en")
        {
            LangsList.SetLanguage(EnIDLanguage, true);
            return;
        }

        if (lang == "tr")
        {
            LangsList.SetLanguage(TrIDLanguage, true);
            return;
        }

#elif UNITY_2020_1_OR_NEWER

        if (Application.systemLanguage == SystemLanguage.Russian)
            LangsList.SetLanguage(RuIDLanguage, true);

        if (Application.systemLanguage == SystemLanguage.English)
            LangsList.SetLanguage(EnIDLanguage, true);

        if (Application.systemLanguage == SystemLanguage.Turkish)
            LangsList.SetLanguage(TrIDLanguage, true);

#endif
    }

    public void ChangeLang (TMP_Dropdown tmp)
    {
        LangsList.SetLanguage(tmp.value, true);
    }
}
