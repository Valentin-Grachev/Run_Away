using System;
using System.Collections.Generic;
using UnityEngine;


namespace VG.Ads
{
    public class Ads : MonoBehaviour
    {
        private static Ads instance;
        public static Action onAvailable;


        [SerializeField] private List<RewardedService> rewardedServices;

        [Space(10)]
        [SerializeField] private bool showLogs;
        [SerializeField] private bool _useTestAds; public static bool useTestAds { get => instance._useTestAds; }
        [SerializeField] private bool _skipAds; 
        public static bool skipAds { get => instance._skipAds; }



        public static void Log(string message)
        {
            if (instance.showLogs) Debug.Log(message);
        }



        public static bool available
        {
            get
            {
                foreach (var service in instance.rewardedServices)
                    if (service.available) return true;
                return false;
            }
        }



        private void Awake()
        {
            instance ??= this;
        }

        private void Start()
        {
            foreach (var service in rewardedServices)
                service.Initialize(() =>
                    service.Load(() => onAvailable?.Invoke()));
        }


        public static void Show(Action<bool> onSuccess)
        {
            if (skipAds)
            {
                onSuccess?.Invoke(true);
                return;
            }


            foreach (var service in instance.rewardedServices)
            {
                if (service.available)
                {
                    service.Show(() =>
                    {
                        onSuccess?.Invoke(true);
                        service.Load(() => onAvailable?.Invoke());
                    });
                    return;
                }
            }

            onSuccess?.Invoke(false);

            foreach (var service in instance.rewardedServices)
                service.Load(() => onAvailable?.Invoke());
        }


    }
}
