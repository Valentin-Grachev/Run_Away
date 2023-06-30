using System.Collections;
using UnityEngine;
using UnityEngine.AI;


namespace DrawRoad
{
    public class Navigation : MonoBehaviour
    {
        private NavMeshSurface2d _navSurface;


        private void Awake()
        {
            _navSurface = GetComponent<NavMeshSurface2d>();
            Level.onLevelStarted += OnLevelStarted;
        }

        private void OnLevelStarted()
        {
            StopAllCoroutines();
            StartCoroutine(BuildNavMesh());
        }

        IEnumerator BuildNavMesh()
        {
            yield return new WaitForSeconds(0.5f);
            _navSurface.BuildNavMesh();
        }

    }
}


