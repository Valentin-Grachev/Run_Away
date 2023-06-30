using VG.Ads;
using System;
using TMPro;
using UnityEngine;
using VG.Localization;

namespace DrawRoad
{
    public class Level : MonoBehaviour
    {
        public enum FinishType { Leave, Win, LoseHunter, LoseObstacle, Skip }

        public static Level instance { get; private set; }


        public static Action onLevelStarted;
        public static event Action<FinishType> onFinished;

        [SerializeField] private LevelStore _levelStore;
        [SerializeField] private Collision _hunterCollision;
        [SerializeField] private Collision _obstacleCollision;
        [SerializeField] private TextMeshProUGUI _levelText;

        private GameObject _levelInstance;
        private static bool levelStarted = false;


        public static int currentLevel { get => PlayerPrefs.GetInt("Level"); }


        private void Awake()
        {
            Localization.onLanguageChanged += OnLanguageChanged;

            instance = this;
            if (!PlayerPrefs.HasKey("Level")) PlayerPrefs.SetInt("Level", 1);
            Runner.onRunnerCollided += OnMoverCollided;
        }

        private void OnLanguageChanged(Language obj)
        {
            _levelText.text = Localization.GetTranslation("level") + " " + PlayerPrefs.GetInt("Level", 1      ).ToString();
        }

        private void OnMoverCollided(Vector2 collidePosition, Runner.CollisionType collisionType)
        {
            if (collisionType == Runner.CollisionType.Hunter)
                Instantiate(_hunterCollision, collidePosition, Quaternion.identity);
            else if (collisionType == Runner.CollisionType.Obstacle)
                Instantiate(_obstacleCollision, collidePosition, Quaternion.identity);
        }

        private void Start()
        {
            StartLevel(currentLevel);
        }


        public static void Finish(FinishType finishType)
        {
            if (levelStarted)
            {
                onFinished?.Invoke(finishType);
                levelStarted = false;
            }
        }


        private void StartLevel(int level)
        {
            levelStarted = true;
            PlayerPrefs.SetInt("Level", level);
            _levelText.text = Localization.GetTranslation("level") + " " + level.ToString();

            if (_levelInstance != null) Destroy(_levelInstance);

            int levelIndex;
            if (level > _levelStore.levels.Count)
                levelIndex = (level - 1) % (_levelStore.levels.Count - 6) + 6;
            else levelIndex = level - 1;

            _levelInstance = Instantiate(_levelStore.levels[levelIndex], Vector3.zero, Quaternion.identity);

            onLevelStarted?.Invoke();
        }
        
        public void RestartLevel() => StartLevel(currentLevel);

        public void NextLevel()
        {
            StartLevel(currentLevel + 1);
        }

        public void SkipLevel()
        {
            Finish(FinishType.Skip);
            Ads.Show((success) =>
            {
                if (success)
                {
                    NextLevel();
                }    
                    
            });
        }


    }
}


