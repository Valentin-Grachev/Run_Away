using TMPro;
using UnityEngine;

namespace VG.Localization
{

    [RequireComponent(typeof(TextMeshProUGUI))]
    public class LocalizedText : MonoBehaviour
    {
        [SerializeField] private string _id;

        private TextMeshProUGUI _text;


        private void Awake()
        {
            _text = GetComponent<TextMeshProUGUI>();
            Localization.onLanguageChanged += OnLanguageChanged;
        }

        private void Start()
        {
            _text.text = Localization.GetTranslation(_id);   
        }


        private void OnDestroy()
        {
            Localization.onLanguageChanged -= OnLanguageChanged;
        }


        private void OnLanguageChanged(Language language)
        {
            _text.text = Localization.GetTranslation(_id);
        }
            


    }
}
    
