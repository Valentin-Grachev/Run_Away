using UnityEngine;


namespace DrawRoad
{
    public class Window : MonoBehaviour
    {
        [SerializeField] private GameObject _commonPanel;
        [Space(10)]
        [SerializeField] private GameObject _winPanel;
        [SerializeField] private GameObject _hunterLosePanel;
        [SerializeField] private GameObject _obstacleLosePanel;
        [Space(10)]
        [SerializeField] private GameObject _skipButton;


        private void Awake()
        {
            Level.onLevelStarted += OnLevelStarted;
            Level.onFinished += OnLevelFinished;
        }


        private void DisablePanels()
        {
            _commonPanel.SetActive(false);
            _winPanel.SetActive(false);
            _hunterLosePanel.SetActive(false);
            _obstacleLosePanel.SetActive(false);

        }

        private void OnLevelStarted()
        {
            DisablePanels();
            _skipButton.SetActive(true);
        }

        private void OnLevelFinished(Level.FinishType finishType)
        {
            if (finishType == Level.FinishType.Skip) return;

            DisablePanels();
            _commonPanel.SetActive(true);
            _skipButton.SetActive(false);


            if (finishType == Level.FinishType.LoseHunter)
            {
                _hunterLosePanel.SetActive(true);
                Sounds.Play(Sounds.Key.lose);
            }
                

            else if (finishType == Level.FinishType.LoseObstacle)
            {
                _obstacleLosePanel.SetActive(true);
                Sounds.Play(Sounds.Key.lose);
            }

            else if (finishType == Level.FinishType.Win)
            {
                _winPanel.SetActive(true);
                Sounds.Play(Sounds.Key.win);
            }
                
        }

    }
}


