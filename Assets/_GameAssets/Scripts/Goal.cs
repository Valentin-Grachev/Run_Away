using System;
using System.Collections.Generic;
using UnityEngine;

namespace DrawRoad
{
    public class Goal : MonoBehaviour
    {
        [SerializeField] private List<int> _acceptableIds; public List<int> acceptableIds { get => _acceptableIds; }

        [HideInInspector] public Runner acceptedMover;

        public const float touchRadius = 0.6f;

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
            acceptedMover = null;
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, touchRadius);
        }

    }

}