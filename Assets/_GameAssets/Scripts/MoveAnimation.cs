using System.Collections.Generic;
using UnityEngine;

namespace DrawRoad
{
    public class MoveAnimation : MonoBehaviour
    {
        [SerializeField] private Movable _movable;

        [SerializeField] private SpriteRenderer _spriteRenderer;
        [SerializeField] private float _changeSpriteTimeInterval;
        [Space(10)]
        [SerializeField] private Sprite _idleSprite;
        [SerializeField] private List<Sprite> _moveSprites;

        private int _currentSpriteIndex;
        private float _timeUntilSpriteChange;

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
            _movable.transform.localScale = Vector3.one;
            _timeUntilSpriteChange = _changeSpriteTimeInterval;
            _currentSpriteIndex = 0;
            _spriteRenderer.sprite = _idleSprite;
        }


        private void Update()
        {
            if (_movable.Move)
            {
                _timeUntilSpriteChange -= Time.deltaTime;
                if (_timeUntilSpriteChange < 0f)
                {
                    _timeUntilSpriteChange = _changeSpriteTimeInterval;
                    _currentSpriteIndex = (_currentSpriteIndex + 1) % _moveSprites.Count;
                    _spriteRenderer.sprite = _moveSprites[_currentSpriteIndex];

                    SetLookSide(_movable.moveSide);
                }
            }
        }


        private void SetLookSide(Movable.Side side)
        {
            Vector3 scale = _movable.transform.localScale;

            if (side == Movable.Side.Left) scale.x = -1;
            else scale.x = 1;

            _movable.transform.localScale = scale;
        }


    }
}




