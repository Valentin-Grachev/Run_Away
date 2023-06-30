using UnityEngine;


namespace DrawRoad
{
    public class MooneeTracker : MonoBehaviour
    {

        private void Awake()
        {
            Level.onFinished += OnFinished;
        }

        private void OnFinished(Level.FinishType finishType)
        {
            if (finishType == Level.FinishType.Win)
                MoonSDK.TrackLevelEvents(MoonSDK.LevelEvents.Complete, Level.currentLevel);
        }
    }
}


