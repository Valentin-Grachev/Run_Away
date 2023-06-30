using System.Collections.Generic;
using UnityEngine;
using TMPro;


namespace VG.Localization
{
    [CreateAssetMenu(menuName = "VG/Localization/PhrasesData", fileName ="Phrases")]
    public class PhrasesData : ScriptableObject
    {
        [System.Serializable]
        public struct Phrase
        {
            public string id;
            [Space(10)]
            public string ru;
            public string en;
            public string tr;
        }

        public List<Phrase> phrases;




    }
}
    
