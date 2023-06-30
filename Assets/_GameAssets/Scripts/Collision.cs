using UnityEngine;
using DG.Tweening;

namespace DrawRoad
{
    public class Collision : MonoBehaviour
    {
        [SerializeField] private Runner.CollisionType collisionType;


        private const float animDuration = 0.6f;

        private void Awake()
        {
            Level.onLevelStarted += OnLevelStarted;
        }

        private void OnDestroy()
        {
            Level.onLevelStarted -= OnLevelStarted;
        }

        private void OnLevelStarted()
        {
            Destroy(gameObject);
        }

        private void Start()
        {
            transform.localScale = Vector3.zero;

            DOTween.Sequence()
                .Append(transform.DOScale(1f, animDuration / 2))
                .Append(transform.DOScale(0f, animDuration / 2))
                .onComplete += () =>
                {
                    if (collisionType == Runner.CollisionType.Hunter)
                        Level.Finish(Level.FinishType.LoseHunter);
                    else if (collisionType == Runner.CollisionType.Obstacle)
                        Level.Finish(Level.FinishType.LoseObstacle);

                    Destroy(gameObject);
                };
        }



    }
}



