using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace DrawRoad
{
    public class Tutorial : MonoBehaviour
    {
        [SerializeField] private Transform _cursor;
        [SerializeField] private List<Transform> _points;

        private const float moveDuration = 0.5f;
        private bool _tutorialDisabled = false;

        private Sequence _sequence;

        private void Awake()
        {
            Drawer.onBeginDrawing += OnBeginDrawing;
            Drawer.onEndDrawing += OnEndDrawing;
            Runner.onAllRunnersReady += OnAllRunnersReady;
            Level.onFinished += OnFinished;
        }



        private void OnFinished(Level.FinishType finishType)
        {
            Drawer.onBeginDrawing -= OnBeginDrawing;
            Drawer.onEndDrawing -= OnEndDrawing;
            Runner.onAllRunnersReady -= OnAllRunnersReady;
            Level.onFinished -= OnFinished;
            StopAnimation();
        }


        private void OnAllRunnersReady()
        {
            _tutorialDisabled = true;
            StopAnimation();
        }

        private void OnEndDrawing()
        {
            if (!_tutorialDisabled) StartAnimation();
        }


        private void OnBeginDrawing() => StopAnimation();

        private void Start() => StartAnimation();



        private void StopAnimation()
        {
            _sequence?.Kill();
            _cursor.gameObject?.SetActive(false);
        }

        private void StartAnimation()
        {
            _cursor.gameObject.SetActive(true);

            _cursor.position = _points[0].position;
            _cursor.localScale = Vector3.one;

            _sequence = DOTween.Sequence();
            _sequence.Append(_cursor.DOScale(0.7f, moveDuration));

            for (int i = 1; i < _points.Count; i++)
                _sequence.Append(_cursor.DOMove(_points[i].position, moveDuration));

            _sequence.Append(_cursor.DOScale(1f, moveDuration));

            _sequence.SetLoops(-1);
            _sequence.Play();
        }



    }
}



