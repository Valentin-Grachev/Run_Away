using UnityEngine;
using UnityEngine.UI;

namespace VG.Components
{
    [RequireComponent(typeof(Button))]
    public abstract class ButtonAction : MonoBehaviour
    {
        public static bool actionsEnabled = true;

        protected abstract void OnClick();


        private void Start()
            => GetComponent<Button>().onClick.AddListener(ButtonClick);


        private void ButtonClick()
        {
            if (actionsEnabled) OnClick();
        }


    }
}




