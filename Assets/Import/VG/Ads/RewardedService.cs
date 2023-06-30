using System;
using UnityEngine;


namespace VG.Ads
{
    public abstract class RewardedService : MonoBehaviour
    {
        public abstract string logName { get; }

        public abstract bool available { get; }

        public abstract void Show(Action onRewarded);

        public abstract void Load(Action onAvailable);

        public abstract void Initialize(Action onInitialized);


    }
}
