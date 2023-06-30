using UnityEngine;


namespace DrawRoad
{
    public class SoundOfRun : MonoBehaviour
    {

        private void Awake()
        {
            Runner.onAllRunnersReady += OnAllRunnersReady;
            Runner.onRunnerCollided += OnRunnerCollided;
            Level.onFinished += OnLevelFinished;
        }

        private void OnLevelFinished(Level.FinishType obj)
        {
            //Sound.Stop();
        }

        private void OnRunnerCollided(Vector2 arg1, Runner.CollisionType arg2)
        {
            //Sound.Stop();
        }

        private void OnAllRunnersReady()
        {
            //Sound.Play("run");
        }



    }
}


