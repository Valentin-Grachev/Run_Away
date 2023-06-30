using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace VG.Localization
{
    public enum Language { RU, EN, TR }


    public class Localization : MonoBehaviour
    {
        public static event Action<Language> onLanguageChanged;

        [SerializeField] private Language _defaultLanguage;
        [SerializeField] private PhrasesData _phrasesData;

        private static Dictionary<string, PhrasesData.Phrase> phrases;

        public static Language currentLanguage { get; private set; }

        private void Awake()
        {
            LoadPhrases();
            
        }

        private void Start()
        {
            SetLanguage(_defaultLanguage);
        }


        private void LoadPhrases()
        {
            phrases = new Dictionary<string, PhrasesData.Phrase>();
            foreach (var phrase in _phrasesData.phrases)
                phrases.Add(phrase.id, phrase);
        }

        public static void SetLanguage(Language language)
        {
            currentLanguage = language;
            onLanguageChanged?.Invoke(language);
        }

        public static string GetTranslation(string id)
        {
            PhrasesData.Phrase phrase = phrases[id];

            switch (currentLanguage)
            {
                case Language.RU:
                    return phrase.ru;

                case Language.EN:
                    return phrase.en;

                case Language.TR:
                    return phrase.tr;
            }

            return phrase.en;
        }

    }
}


